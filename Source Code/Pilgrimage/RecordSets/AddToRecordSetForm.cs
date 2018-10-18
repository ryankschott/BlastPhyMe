using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChangLab.Genes;
using ChangLab.NCBI.GenBank;
using ChangLab.RecordSets;
using Pilgrimage.Activities;

namespace Pilgrimage
{
    public partial class AddRecordsToRecordSetForm : DialogForm
    {
        internal DataTypes DataType { get; set; }
        internal DataGridView ResultsGrid { get; set; }
        internal ComboBox SubSetsList { get; set; }
        internal Button AddToButton { get; set; }
        internal bool PerformUpdateFromGenBank { get; set; }
        internal bool RemoveAfterAdd { get; set; }

        internal virtual List<RowDataItem> SelectedRows
        {
            get
            {
                if (ResultsGrid.DataSource == null) { return new List<RowDataItem>(); }
                else
                {
                    return ResultsGrid
                        .Rows
                        .Cast<DataGridViewRow>()
                        .Where(row => ((RowDataItem)row.DataBoundItem).Selected)
                        .Select(row => (RowDataItem)row.DataBoundItem)
                        .ToList();
                }
            }
        }

        /// <summary>
        /// Subsets that should be refreshed if open, and opened if closed.
        /// </summary>
        internal List<string> EditedSubSetIDs { get; set; }
        /// <summary>
        /// Subsets that should be refreshed if open.
        /// </summary>
        internal List<string> UpdatedSubSetIDs { get; set; }

        protected internal string _selectedSubSetID;
        internal string SelectedSubSetID
        {
            get
            {
                if (SubSetsList != null)
                {
                    if (string.IsNullOrWhiteSpace(_selectedSubSetID))
                    { return (string)SubSetsList.SelectedValue; }
                    else
                    { return _selectedSubSetID; }
                }
                else { return string.Empty; }
            }
        }

        internal AddRecordsToRecordSetForm()
        {
            EditedSubSetIDs = new List<string>();
            UpdatedSubSetIDs = new List<string>();
            PerformUpdateFromGenBank = true;
            RemoveAfterAdd = false;
        }

        protected internal void Configure(DataGridView ResultsGrid, ComboBox SubSetsList, Button AddToButton, DataTypes DataType)
        {
            this.DataType = DataType;
            this.ResultsGrid = ResultsGrid;
            if (SubSetsList != null)
            {
                this.SubSetsList = SubSetsList;
                ConfigureSubSetList(SubSetsList, this.DataType);
            }
            if (AddToButton != null)
            {
                this.AddToButton = AddToButton;
                this.AddToButton.Click += new EventHandler(AddToButton_Click);
            }
        }

        protected internal bool AddRecordsAfterClick { get; private set; }
        protected internal virtual void AddToButton_Click(object sender, EventArgs e)
        {
            AddRecordsAfterClick = false;

            if (SelectedRows.Count == 0)
            {
                Utility.ShowMessage(this.OwnerForm, Properties.Resources.Messages_NoGenesSelected);
                return;
            }

            if (SubSetsList != null && string.IsNullOrWhiteSpace(SelectedSubSetID))
            {
                using (RecordSets.frmEditSubSet frm = new RecordSets.frmEditSubSet(new SubSet(this.DataType)))
                {
                    if (frm.ShowDialog(this.OwnerForm) == System.Windows.Forms.DialogResult.OK)
                    {
                        _selectedSubSetID = frm.CurrentSubSet.ID;

                        Program.Settings.AllSubSets(this.DataType).Add(frm.CurrentSubSet);
                        ConfigureSubSetList(this.SubSetsList, this.DataType, _selectedSubSetID);
                    }
                    else { return; }
                }
            }

            AddRecordsAfterClick = true;
        }

        protected virtual void OnRemovingRows(DataGridViewRowsActionEventArgs e)
        {
            if (RemovingRows != null)
            {
                RemovingRows(e);
            }
        }
        public event DataGridViewRowsActionEventHandler RemovingRows;
    }

    public partial class AddGeneSequencesToRecordSetForm : AddRecordsToRecordSetForm
    {
        internal DataGridViewHelper SubjectDataGridHelper { get; set; }
        internal CheckBox UpdateFromGenBankCheckBox { get; set; }

        internal virtual List<GenericGeneRowDataItem> SelectedGeneRows
        {
            get
            {
                if (ResultsGrid.DataSource == null) { return new List<GenericGeneRowDataItem>(); }
                else
                {
                    return ResultsGrid
                        .Rows
                        .Cast<DataGridViewRow>()
                        .Where(row => ((GenericGeneRowDataItem)row.DataBoundItem).Selected)
                        .Select(row => (GenericGeneRowDataItem)row.DataBoundItem)
                        .ToList();
                }
            }
        }
        internal List<Gene> SelectedGenes
        {
            get
            {
                return SelectedGeneRows.Select(row => row.Gene).ToList();
            }
        }

        internal GenBankSearch Search { get; set; }

        internal AddGeneSequencesToRecordSetForm() : base() { }
        
        internal void Configure(DataGridView ResultsGrid, ComboBox SubSetsList, Button AddToButton, CheckBox UpdateFromGenBankCheckBox)
        {
            base.Configure(ResultsGrid, SubSetsList, AddToButton, DataTypes.GeneSequence);
            this.UpdateFromGenBankCheckBox = UpdateFromGenBankCheckBox;
        }

        protected override internal void AddToButton_Click(object sender, EventArgs e)
        {
            base.AddToButton_Click(sender, e);
            if (AddRecordsAfterClick)
            {
                if (PerformUpdateFromGenBank && (UpdateFromGenBankCheckBox == null || UpdateFromGenBankCheckBox.Checked))
                {
                    // Perform an EFetch for each gene to populate it from the full GenBank record.
                    PopulateFromGenBank gb = new PopulateFromGenBank(this.OwnerForm);
                    gb.ActivityCompleted += new Activity.ActivityCompletedEventHandler(GenBankCompleted);
                    gb.Populate(SelectedGenes.Where(g => g.NeedsUpdateFromGenBank), this.Search);
                }
                else
                {
                    // The SelectedGenes collection will be saved to the database but with only whatever shell data they have.
                    GenBankCompleted(new RunWorkerCompletedEventArgs(new List<Gene>(), null, false));
                }
            }
        }

        protected internal virtual void GenBankCompleted(RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            { return; }
            else if (e.Error != null)
            { Utility.ShowErrorMessage(this.OwnerForm, e.Error); }
            else
            {
                List<Gene> PopulatedGenes = (List<Gene>)e.Result;

                AddedGeneUpdatesEventArgs populatingEventArgs = new AddedGeneUpdatesEventArgs(PopulatedGenes);
                OnMergingPopulatedGenes(populatingEventArgs);

                if (!populatingEventArgs.Cancel)
                {
                    // Merge in the annotation information we got from GenBank.
                    // If the user popped open a Details panel for a gene, that gene will already have been populated.
                    PopulatedGenes.ForEach(pg =>
                    {
                        Gene match = SelectedGenes.FirstOrDefault(sg => sg.GenBankID == pg.GenBankID);
                        if (match != null)
                        {
                            match.Merge(pg);
                        }
                    });
                }

                AddedGeneUpdatesEventArgs savingEventArgs = new AddedGeneUpdatesEventArgs(SelectedGenes);
                OnSavingAddedGenes(savingEventArgs);

                if (savingEventArgs.Error != null)
                {
                    Utility.ShowErrorMessage(this.OwnerForm, savingEventArgs.Error);
                    return;
                }
                else if (savingEventArgs.Cancel) { return; }
                else
                {
                    savingEventArgs.Genes.ForEach(g =>
                    {
                        g.Save(true, true);
                    });
                }

                SelectedGeneRows.ForEach(row =>
                {
                    if (savingEventArgs.Genes.Contains(row.Gene))
                    {
                        if (!string.IsNullOrWhiteSpace(SelectedSubSetID))
                        { Program.Settings.CurrentRecordSet.AddGene(row.Gene, SelectedSubSetID); }
                        row.ModifiedAt = DateTime.Now;
                        row.InRecordSet = true;
                    }
                });
                if (!string.IsNullOrWhiteSpace(SelectedSubSetID)) { EditedSubSetIDs.Add(SelectedSubSetID); }
                _selectedSubSetID = string.Empty; // Reset so as to not affect a subsequent addition to a new subset.

                Program.Settings.CurrentRecordSet.Save(); // Update the ModifiedAt value.

                if (RemoveAfterAdd)
                {
                    List<DataGridViewRow> rowsForRemoval = ResultsGrid.Rows.Cast<DataGridViewRow>().ToList()
                        .Where(row => ((GenericGeneRowDataItem)row.DataBoundItem).Selected
                                        && savingEventArgs.Genes.Contains(((GenericGeneRowDataItem)row.DataBoundItem).Gene)
                        ).ToList();

                    DataGridViewRowsActionEventArgs args = new DataGridViewRowsActionEventArgs(rowsForRemoval);
                    OnRemovingRows(args);
                    if (!args.Cancel)
                    {
                        rowsForRemoval.ForEach(row => ResultsGrid.Rows.Remove(row));
                    }
                }
                else
                {
                    ResultsGrid.Rows.Cast<DataGridViewRow>().ToList().ForEach(row => ((GenericGeneRowDataItem)row.DataBoundItem).Selected = false);
                }
                ResultsGrid.Refresh();

                OnAddedGenes(new AddedGeneUpdatesEventArgs(savingEventArgs.Genes));
            }
        }

        #region Events
        protected virtual void OnMergingPopulatedGenes(AddedGeneUpdatesEventArgs e)
        {
            if (MergingPopulatedGenes != null)
            {
                MergingPopulatedGenes(this, e);
            }
        }
        public event AddedGeneUpdatesEventHandler MergingPopulatedGenes;

        protected virtual void OnSavingAddedGenes(AddedGeneUpdatesEventArgs e)
        {
            if (SavingAddedGenes != null)
            {
                SavingAddedGenes(this, e);
            }
        }
        public event AddedGeneUpdatesEventHandler SavingAddedGenes;

        protected virtual void OnAddedGenes(AddedGeneUpdatesEventArgs e)
        {
            if (AddedGenes != null)
            {
                AddedGenes(this, e);
            }
        }
        public event AddedGeneUpdatesEventHandler AddedGenes;

        public delegate void AddedGeneUpdatesEventHandler(object sender, AddedGeneUpdatesEventArgs e);
        public class AddedGeneUpdatesEventArgs : EventArgs
        {
            public List<Gene> Genes { get; set; }
            public bool Cancel { get; set; }
            public Exception Error { get; set; }

            public AddedGeneUpdatesEventArgs(List<Gene> Genes)
            {
                this.Genes = Genes;
            }
        }
        #endregion
    }
}
