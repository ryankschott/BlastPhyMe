using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChangLab.Common;
using ChangLab.Jobs;
using ChangLab.Genes;
using ChangLab.RecordSets;
using ChangLab.Taxonomy;
using Pilgrimage.Activities;
using Pilgrimage.RecordSets;

namespace Pilgrimage.GeneSequences
{
    public partial class uctRecordSetGenes : Pilgrimage.UserControls.uctSubSetView
    {
        private uctGeneSequencesMain MainForm { get; set; }
        protected internal override DataGridView RecordsGrid { get { return grdResults; } }
        
        private List<GenericGeneRowDataItem> GeneTable { get; set; }
        private FilterProperties Filter { get; set; }
        /// <remarks>
        /// It's possible that the filter conditions result in showing all of the rows in the subset, in which case a simple comparison of displayed
        /// rows vs total rows doesn't correctly show the Change Filter and Clear Filter buttons, and the user might think something is broken.  This
        /// variable gets reset when ApplyFilter() runs through the FilterProperties, and if anything has been configured for a filter it will be set
        /// true, otherwise at the end of ApplyFilter() it will still be false.  This is then used to determine the state of the Filter LinkLabels.
        /// </remarks>
        private bool Filtered { get; set; }

        public List<Gene> SelectedGenes
        {
            get
            {
                return SelectedRows.Select(row => ((GenericGeneRowDataItem)row).Gene).ToList();
            }
        }
        
        public uctRecordSetGenes()
        {
            InitializeComponent();
            clmSequenceMatch.DefaultCellStyle.NullValue = null;
            Filter = new FilterProperties();
            this.Loaded = false;
        }

        public void Initialize(uctGeneSequencesMain MainForm, SubSet CurrentSet)
        {
            this.MainForm = MainForm;
            this.CurrentSubSet = CurrentSet;
            this.DataGridHelper = new DataGridViewHelper(this.ParentForm, grdResults);
            this.DataGridHelper.SelectedRowsChanged += new DataGridViewHelper.SelectedRowsChangedEventHandler(DataGridHelper_SelectedRowsChanged);
            this.DataGridHelper.ViewDetails += new DataGridViewHelper.ViewDetailsEventHandler(DataGridHelper_ViewDetails);
            this.DataGridHelper.ConfigureDateTimeColumn(clmModifiedAt);
            this.DataGridHelper.ConfigureDateTimeColumn(clmLastUpdatedAt);

#if EEB460
            clmGenBankUrl.Visible = false;
            clmHasBlastNResults.Visible = false;
#endif

            //this.grdResults.KeyDown += new KeyEventHandler(grdResults_KeyDown);
            //this.grdResults.KeyUp += new KeyEventHandler(grdResults_KeyUp);
        }

        public void Clear()
        {
            grdResults.DataSource = null;
        }

        public override void RefreshRecords(bool RefreshFromDatabase = true, bool SoftRefresh = false)
        {
            if (bwRefreshGenes.IsBusy) { return; }
            if (SoftRefresh) { grdResults.Refresh(); return; }

            Refreshing = true;

            if (RefreshFromDatabase)
            {
                StartRefreshing();
                bwRefreshGenes.RunWorkerAsync();
                
                return;
            }

            GenericGeneRowDataItem[] copy = new GenericGeneRowDataItem[GeneTable.Count]; GeneTable.CopyTo(copy);
            IEnumerable<GenericGeneRowDataItem> query = copy.Select(r => r);
            int totalRecords = query.Count();
            Filtered = this.Filter.ApplyFilter(ref query);
            int filteredRecords = query.Count();
            
            if (SelectedRows.Count != 0)
            {
                // Reselect the genes that were already selected.
                SelectedRows.ForEach(srow =>
                    {
                        GenericGeneRowDataItem row = query.FirstOrDefault(qrow => GuidCompare.Equals(qrow.ID, srow.ID));
                        if (row != null) { row.Selected = true; }
                    }
                );
            }

            Filter_UpdateText(filteredRecords, totalRecords);

            grdResults.AutoGenerateColumns = false;
            grdResults.DataSource = null;
            grdResults.DataSource = new SortableBindingList<GenericGeneRowDataItem>(query);

            DataGridHelper_SelectedRowsChanged(null);
            if (this.Filter.SequenceMatch) { grdResults_SelectionChanged(grdResults, EventArgs.Empty); }

            if (!tblForm.Enabled)
            {
                EndRefreshing();
            }

            Refreshing = false;
            if (!Loaded) { Loaded = true; }
        }

        //private void ApplyFilter(ref IEnumerable<GenericGeneRowDataItem> Records)
        //{
        //    Filtered = false;

        //    if (!string.IsNullOrWhiteSpace(this.Filter.OrganismName))
        //    {
        //        Records = Records.Where(g => g.Organism.Match(this.Filter.OrganismName, this.Filter.OrganismMatchLogic));
        //        Filtered = true;
        //    }
        //    if (!string.IsNullOrWhiteSpace(this.Filter.Definition))
        //    {
        //        Records = Records.Where(g => g.Definition.Match(this.Filter.Definition, this.Filter.DefinitionMatchLogic));
        //        Filtered = true;
        //    }
        //    if (Filter.Taxa.Count != 0)
        //    {
        //        Records = Records.Where(g => Filter.Taxa.Any(t => g.TaxonomyHierarchy.StartsWith(t.Hierarchy)));
        //        Filtered = true;
        //    }
        //    if (Filter.BLASTNSubmission)
        //    {
        //        Records = Records.Where(g => g.HasBlastNResults == Filter.BLASTN_HasResults);
        //        Filtered = true;
        //    }
        //    // These ones should be done last because they use a group by.
        //    if (Filter.Duplicates)
        //    {
        //        List<string> duplicateOrganismNames = Records.GroupBy(g => g.Organism).Where(gb => gb.Count() > 1).Select(gb => gb.Key).Distinct().ToList();
        //        if (Filter.Duplicates_Have)
        //        {
        //            Records = Records.Where(g => duplicateOrganismNames.Contains(g.Organism));
        //        }
        //        else
        //        {
        //            Records = Records.Where(g => !duplicateOrganismNames.Contains(g.Organism));
        //        }
        //        Filtered = true;
        //    }
        //    if (Filter.SequenceMatch)
        //    {
        //        if (Filter.SequenceMatch_CDS)
        //        {
        //            List<KeyValuePair<string, int>> duplicateSequences = Records.GroupBy(g => g.Gene.Nucleotides).Where(gb => gb.Count() > 1).Select(gb => gb.Key).Distinct().Select((s, index) => new KeyValuePair<string, int>(s, index + 1)).ToList();
        //            Records = Records.Join(duplicateSequences, g => g.Gene.Nucleotides, ds => ds.Key, (g, ds) => { g.DuplicateSequenceKey = ds.Value; return g; });
        //        }
        //        else
        //        {
        //            // If the user has filtered by the whole gene sequence we have to go out to the database to filter because we don't have the
        //            // whole sequence in memory unless a record has been opened into detailed view.
        //            List<KeyValuePair<string, int>> filteredGeneIds = Gene.FilterAtDatabase(Records.Select(g => g.ID).ToList(), false, true);
        //            Records = Records.Join(filteredGeneIds, g => g.ID, fg => fg.Key, (g, fg) => { g.DuplicateSequenceKey = fg.Value; return g; });
        //        }
        //        Filtered = true;
        //    }
        //}

        #region Async Refresh
        private void bwRefreshGenes_DoWork(object sender, DoWorkEventArgs e)
        {
            CurrentRecordSet.RefreshGenesFromDatabase(this.CurrentSubSet.ID);
            this.GeneTable = CurrentRecordSet.GeneTable(this.CurrentSubSet.ID).ToRowDataItemList();
        }

        private void bwRefreshGenes_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            RefreshRecords(false);
        }

        private void StartRefreshing()
        {
            tblForm.Enabled = false;
            pbRefreshing.BringToFront();
            NeverEndingTimer.Start();
        }

        private void EndRefreshing()
        {
            NeverEndingTimer.Stop();
            pbRefreshing.SendToBack();
            tblForm.Enabled = true;
        }

        private void NeverEndingTimer_Tick(object sender, EventArgs e)
        {
            if (pbRefreshing.Parent != null)
            {
                if (pbRefreshing.Value == pbRefreshing.Maximum)
                { pbRefreshing.Value = pbRefreshing.Minimum; }
                else
                { pbRefreshing.PerformStep(); }
            }
        }
        #endregion

        public override void RemoveRecords(List<RowDataItem> Genes)
        {
            Genes.ForEach(gene => {
                GenericGeneRowDataItem match = GeneTable.FirstOrDefault(row => GuidCompare.Equals(row.ID, gene.ID));
                if (match != null) { GeneTable.Remove(match); }
            });

            SortableBindingList<GenericGeneRowDataItem> dataSource = (SortableBindingList<GenericGeneRowDataItem>)grdResults.DataSource;
            Genes.Select(gene => gene.ID).ToList().ForEach(id =>
            {
                GenericGeneRowDataItem match = dataSource.FirstOrDefault(row => GuidCompare.Equals(row.ID, id));
                if (match != null) { dataSource.Remove(match); }
            });

            Filter_UpdateText(dataSource.Count, GeneTable.Count);
            DataGridHelper_SelectedRowsChanged(null);
        }

        public void UpdateHasAlignedSubjectSequences(List<string> QueryIDs)
        {
            if (!this.Loaded)
            {
                // This SubSet view hasn't been opened yet, so there's nothing to refresh.  When the user opens it and it loads on demand, it will be
                // refreshing from the database and thus will pick up the latest flag for having aligned subject sequences.
                return;
            }

            QueryIDs.ForEach(id =>
            {
                GenericGeneRowDataItem match = GeneTable.FirstOrDefault(row => GuidCompare.Equals(row.ID, id));
                if (match != null) { match.ProcessedThroughBLASTNAtNCBI = true; }
            });

            SortableBindingList<GenericGeneRowDataItem> dataSource = (SortableBindingList<GenericGeneRowDataItem>)grdResults.DataSource;
            QueryIDs.ForEach(id =>
            {
                DataGridViewRow match = grdResults.Rows.Cast<DataGridViewRow>().FirstOrDefault(row => GuidCompare.Equals(((GenericGeneRowDataItem)row.DataBoundItem).ID, id));
                if (match != null)
                {
                    GenericGeneRowDataItem gene = (GenericGeneRowDataItem)match.DataBoundItem;
                    gene.ProcessedThroughBLASTNAtNCBI = true;

                    match.Selected = (!match.Selected); match.Selected = (!match.Selected); // Poor man's Refresh()
                }
            });
        }
        
        private void chkToggleSelected_CheckedChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in grdResults.Rows)
            {
                row.Cells[DataGridHelper.SelectedColumnIndex].Value = chkToggleSelected.Checked;
            }

            Header_UpdateText();
        }

        private void DataGridHelper_SelectedRowsChanged(DataGridViewHelper.SelectedRowEventArgs e)
        {
            bool allSelected = (grdResults.Rows.Count > 0 && this.SelectedGenes.Count == grdResults.Rows.Count);
            chkToggleSelected.CheckedChanged -= chkToggleSelected_CheckedChanged;

            chkToggleSelected.Checked = allSelected;
            Header_UpdateText();

            chkToggleSelected.CheckedChanged += new EventHandler(chkToggleSelected_CheckedChanged);
        }

        private void DataGridHelper_ViewDetails(DataGridViewHelper.ViewDetailsEventArgs e)
        {
            GenericGeneRowDataItem row = (GenericGeneRowDataItem)e.Row.DataBoundItem;
            using (frmGeneDetails frm = new frmGeneDetails(row.Gene, row.Gene.NeedsUpdateFromGenBank, null, row.HasBlastNResults) { SaveUpdates = true })
            {
                frm.ShowDialog(this);
                if (frm.Updated)
                {
                    row.Gene.Merge(frm.Gene); // No need to save because frmGeneDetails will already have done so.
                    e.Row.Selected = false; e.Row.Selected = true; // Yeah, that's what you do to refresh the row.
                }
                if (frm.EditedSubSetIDs.Count != 0)
                { frm.EditedSubSetIDs.ForEach(id => MainForm.ShowAndRefreshSubSet(id, null)); }
                if (frm.UpdatedSubSetIDs.Count != 0)
                { frm.UpdatedSubSetIDs.ForEach(id => MainForm.RefreshSubSet(id)); }
            }
        }

        private void Header_UpdateText()
        {
            chkToggleSelected.Text = (chkToggleSelected.Checked ? "Deselect all" : "Select all");
            lblTotalRows.Text = this.SelectedGenes.Count.ToString("N0") + " of " + grdResults.Rows.Count.ToString("N0") + " records selected";
        }

        private void Filter_UpdateText(int FilteredRecords, int TotalRecords)
        {
            lblFilterRows.Text = string.Format("Showing {0} of {1} sequences", FilteredRecords.ToString("N0"), TotalRecords.ToString("N0"));
            lnkFilter.Text = (Filtered ? "Change" : "Apply") + " Filter";
            lnkClearFilter.Text = (Filtered ? "Clear Filter" : string.Empty);
        }

        private void lnkFilter_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (sender == lnkFilter)
            {
                using (frmFilterGenes frm = new frmFilterGenes(this.Filter, this.CurrentSubSet.ID))
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        this.DataGridHelper.ShortTermDisableKeyPress();

                        this.Filter = frm.Filter;
                        RefreshRecords(false);
                    }
                }
            }
            else
            {
                this.Filter = new FilterProperties();
                RefreshRecords();
            }

            this.clmSequenceMatch.Visible = this.Filter.SequenceMatch;
            grdResults.Focus();
        }

        #region Sequence Match Column
        private void grdResults_SelectionChanged(object sender, EventArgs e)
        {
            if (Filter.SequenceMatch && grdResults.SelectedRows.Count == 1 && grdResults.Rows.Count > 1)
            {
                GenericGeneRowDataItem selectedRow = (GenericGeneRowDataItem)grdResults.SelectedRows[0].DataBoundItem;
                int sequenceKey = selectedRow.DuplicateSequenceKey;

                foreach (DataGridViewRow row in grdResults.Rows)
                {
                    if (((GenericGeneRowDataItem)row.DataBoundItem).DuplicateSequenceKey == sequenceKey)
                    {
                        row.DefaultCellStyle.BackColor = Color.LightYellow;
                        ((GenericGeneRowDataItem)row.DataBoundItem).DuplicateSequenceImage = Properties.Resources.DuplicateSequence;
                    }
                    else if (row.DefaultCellStyle.BackColor == Color.LightYellow)
                    {
                        row.DefaultCellStyle.BackColor = Color.White;
                        ((GenericGeneRowDataItem)row.DataBoundItem).DuplicateSequenceImage = null;
                    }
                }
            }
        }
        #endregion
    }
}
