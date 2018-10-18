using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChangLab.Jobs;
using ChangLab.Common;
using ChangLab.Genes;
using ChangLab.RecordSets;
using Pilgrimage.Activities;

namespace Pilgrimage.GeneSequences.Alignment
{
    public partial class frmAlignmentResults<T> : AddGeneSequencesToRecordSetForm where T : CommandLineGeneProcessingJob
    {
        private T Job { get; set; }
        private FilterProperties Filter { get; set; }
        private bool Filtered { get; set; }
        private List<GenericGeneRowDataItem> GeneTable { get; set; }

        public frmAlignmentResults(string JobID, bool OpenedFromCompletedJob = false)
        {
            InitializeComponent();
            this.Icon = Properties.Resources.Icon_Settings;

            SetButtonImage(btnSave, DialogButtonPresets.Add);
            SetButtonImage(btnSaveOutput, DialogButtonPresets.Save);
            SetButtonImage(btnClose_Results, DialogButtonPresets.Cancel);
            SetButtonImage(btnClose_Output, DialogButtonPresets.Cancel);

            Configure(grdResults, cmbSubSets, btnSave, DataTypes.GeneSequence);
            this.AddedGenes += new AddedGeneUpdatesEventHandler(frmAlignmentResults_AddedGenes);
            this.RemovingRows += new DataGridViewRowsActionEventHandler(frmAlignmentResults_RemovingRows);

            cmbSubSets.SelectedIndex = 0;
            this.RemoveAfterAdd = true;
            this.PerformUpdateFromGenBank = false;

            RunAlignment alignmentInMemory = Program.InProgressActivities.GetActivityByJobID<RunAlignment>(JobID);
            if (alignmentInMemory != null) 
            {
                this.Job = (T)alignmentInMemory.CurrentJob;
                Program.InProgressActivities.RemoveActivity(alignmentInMemory); // We no longer need to track it because it has completed.
            }
            else
            {
                // Fetch from the database.
                this.Job = (T)Activator.CreateInstance(typeof(T), new object[] { JobID });
                if (this.Job.HasDatabaseExceptions) { this.Job.RefreshExceptionsFromDatabase(); }
            }

            this.Text = this.Job.Target.ToString() + " Alignment Results";
            pgOutput.Text = this.Job.Target.ToString() + " Output";

            if (this.Job.KeepOutputFiles && System.IO.Directory.Exists(this.Job.JobDirectory))
            { lnkWorkingFolder.Text = this.Job.JobDirectory; }
            else
            { lnkWorkingFolder.Parent.Controls.Remove(lnkWorkingFolder); }

            // We fetch the genes from the database regardless of whether the job is still in memory, because we need the InRecordset flag to be set,
            // and that's a property of a GenericGeneRowDataItem object, not a Gene object. If the user viewed the results of an alignment in memory,
            // added some genes to their recordset, and then closed and re-opened the results for the same alignment, we'd have lost the status of 
            // that InRecordset flag.
            this.GeneTable = GeneProcessingJob.ListGenesFromDatabase(this.Job.ID, GeneDirections.Output).ToRowDataItemList(true);
            this.Filter = new FilterProperties();
            this.Filter.MatchingSequences += new FilterProperties.MatchingSequencesEventHandler(Filter_MatchingSequences);

            if (this.GeneTable.Count == 0)
            {
                lnkFilter.Enabled = false;
                lnkClearFilter.Enabled = false;
                chkToggleSelected.Enabled = false;
                grdResults.Enabled = false;
                cmbSubSets.Enabled = false;
            }

            if (string.IsNullOrWhiteSpace(Job.Output) && Job.ProgressMessages.Count == 0 && Job.Exceptions.Count == 0)
            {
                tbForm.TabPages.Remove(pgOutput);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(Job.Output))
                { txtOutput.Text = this.Job.Output; }
                else if (Job.ProgressMessages.Count != 0)
                { txtOutput.Lines = this.Job.ProgressMessages.Select(msg => msg.Elapsed.ElapsedTimeStamp() + ": " + msg.Message).ToArray(); }

                if (this.Job.Exceptions.Count != 0)
                {
                    txtOutput.Text += (string.IsNullOrWhiteSpace(txtOutput.Text) ? "" : "\r\n\r\n") + "Error" + (this.Job.Exceptions.Count > 1 ? "s" : "") + " occured:"
                        + "\r\n" + this.Job.Exceptions.AggregateInnerMessages("\r\n");

                    tbForm.SelectedTab = pgOutput;
                }

                txtOutput.ScrollToEnd(false);
            }

            this.FocusOnLoad = grdResults;
        }

        private void frmAlignmentResults_Load(object sender, EventArgs e)
        {
#if DOCUMENTATION
            this.Size = new System.Drawing.Size(806, 600);
#endif

            this.SubjectDataGridHelper = new DataGridViewHelper(this, grdResults);
            this.SubjectDataGridHelper.SelectedRowsChanged += new DataGridViewHelper.SelectedRowsChangedEventHandler(DataGridHelper_SelectedRowsChanged);
            this.SubjectDataGridHelper.ViewDetails += new DataGridViewHelper.ViewDetailsEventHandler(DataGridHelper_ViewDetails);

            this.SubjectDataGridHelper.Loaded = false;
            RefreshResults();
        }

        private void RefreshResults()
        {
            // Apply any filter conditions.
            GenericGeneRowDataItem[] copy = new GenericGeneRowDataItem[GeneTable.Count]; GeneTable.CopyTo(copy);
            IEnumerable<GenericGeneRowDataItem> query = copy.Select(r => r);
            int totalRecords = query.Count();
            this.Filtered = this.Filter.ApplyFilter(ref query);
            int filteredRecords = query.Count();
            
            if (SelectedRows.Count != 0)
            {
                // Reselect the genes that were already selected.
                SelectedRows.ForEach(srow =>
                {
                    GenericGeneRowDataItem row = query.FirstOrDefault(qrow => GuidCompare.Equals(qrow.ID, srow.ID));
                    if (row != null) { row.Selected = true; }
                });
            }
            
            grdResults.AutoGenerateColumns = false;
            grdResults.DataSource = null;
            grdResults.DataSource = new SortableBindingList<GenericGeneRowDataItem>(query);

            DataGridHelper_SelectedRowsChanged(null);

            UpdateTotalRowsLabel(filteredRecords, totalRecords);
            UpdateControlsForSelectedRows();
            
            btnSave.Enabled = false;
            if (!this.SubjectDataGridHelper.Loaded) { this.SubjectDataGridHelper.Loaded = true; }
        }

        private void Filter_MatchingSequences(FilterProperties.MatchingSequencesEventArgs e)
        {
            e.MatchWith = FilterProperties.MatchingSequencesEventArgs.MatchingSequencesTypes.SourceSequence;
        }

        private void UpdateTotalRowsLabel(int FilteredRows, int TotalRows)
        {
            lblTotalRows.Text = string.Format("Showing {0} of {1} alignments", FilteredRows.ToString("N0"), TotalRows.ToString("N0"));
        }

        private void chkToggleSelected_CheckedChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in grdResults.Rows)
            {
                row.Cells[this.SubjectDataGridHelper.SelectedColumnIndex].Value = chkToggleSelected.Checked;
            }

            UpdateControlsForSelectedRows();
        }

        private void UpdateControlsForSelectedRows()
        {
            chkToggleSelected.Text = (chkToggleSelected.Checked ? "Deselect all" : "Select all");
            lblSelectedRows.Text = this.SelectedGeneRows.Count.ToString("N0") + " of " + grdResults.Rows.Count.ToString("N0") + " records selected";
            btnSave.Enabled = (SelectedGeneRows.Count != 0);
        }

        private void DataGridHelper_SelectedRowsChanged(DataGridViewHelper.SelectedRowEventArgs e)
        {
            bool allSelected = (grdResults.Rows.Count > 0 && this.SelectedGenes.Count == grdResults.Rows.Count);
            chkToggleSelected.CheckedChanged -= chkToggleSelected_CheckedChanged;

            chkToggleSelected.Checked = allSelected;
            UpdateControlsForSelectedRows();

            chkToggleSelected.CheckedChanged += new EventHandler(chkToggleSelected_CheckedChanged);
        }

        private void frmAlignmentResults_AddedGenes(object sender, AddGeneSequencesToRecordSetForm.AddedGeneUpdatesEventArgs e)
        {
            DataGridHelper_SelectedRowsChanged(null);
            UpdateTotalRowsLabel(grdResults.Rows.Count, this.GeneTable.Count);
        }

        private void frmAlignmentResults_RemovingRows(DataGridViewRowsActionEventArgs e)
        {
            this.GeneTable.RemoveAll(g => e.Rows.Select(row => ((GenericGeneRowDataItem)row.DataBoundItem).ID).Contains(g.ID));
        }

        private void DataGridHelper_ViewDetails(DataGridViewHelper.ViewDetailsEventArgs e)
        {
            Gene gene = ((GenericGeneRowDataItem)e.Row.DataBoundItem).Gene;
            using (frmGeneDetails frm = new frmGeneDetails(gene, false, null, false))
            {
                frm.ShowDialog(this);
            }
        }

        private void lnkFilter_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FilterProperties updatedFilter = null;
            this.Filter.MatchingSequences -= Filter_MatchingSequences;

            if (sender == lnkFilter)
            {
                using (frmFilterGenes frm = new frmFilterGenes(this.Filter, this.Job.SubSetID) { ShowBLASTN = false, SimpleSequenceMatching = true })
                {
                    if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        this.SubjectDataGridHelper.ShortTermDisableKeyPress();
                        updatedFilter = frm.Filter;
                    }
                    else
                    {
                        grdResults.Focus();
                        return;
                    }
                }
            }
            else
            {
                updatedFilter = new FilterProperties();
            }

            this.Filter = updatedFilter;
            this.Filter.MatchingSequences += new FilterProperties.MatchingSequencesEventHandler(Filter_MatchingSequences);

            List<string> selectedGeneIDs = SelectedGenes.Select(g => g.ID).ToList();
            this.GeneTable.ForEach(g => g.Selected = false); // Clear this so that we don't have any orphaned sequences flagged as selected.

            RefreshResults(); // Apply the updated filter

            // Re-select what rows are still visible.
            grdResults.Rows.Cast<DataGridViewRow>()
                .Where(row => selectedGeneIDs.Contains(((RowDataItem)row.DataBoundItem).ID))
                .ToList()
                .ForEach(row => ((RowDataItem)row.DataBoundItem).Selected = true);

            DataGridHelper_SelectedRowsChanged(null);
            grdResults_SelectionChanged(grdResults, EventArgs.Empty);
            this.clmSequenceMatch.Visible = this.Filter.SequenceMatch;
            grdResults.Focus();
        }

        private void btnSaveOutput_Click(object sender, EventArgs e)
        {
            string filePath = string.Empty;
            if (IODialogHelper.SaveFile(IODialogHelper.DialogPresets.Text, "output.txt", this, ref filePath))
            {
                System.IO.File.WriteAllLines(filePath, txtOutput.Lines);
            }
        }

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

        private void lnkWorkingFolder_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(lnkWorkingFolder.Text);
        }
    }
}
