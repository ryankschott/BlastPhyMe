using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChangLab.Common;
using ChangLab.Genes;
using ChangLab.Jobs;
using Pilgrimage.Activities;

namespace Pilgrimage.GeneSequences.BlastN
{
    public partial class frmBlastNAnnotations : AddGeneSequencesToRecordSetForm
    {
        private BlastNAtNCBI Job { get; set; }
        
        public frmBlastNAnnotations(BlastNAtNCBI Job) : base()
        {
            InitializeComponent();
            SetButtonImage(btnSave, "Add");
            SetButtonImage(btnClose_Results, "Cancel");
            SetButtonImage(btnClose_History, "Cancel");
            SetButtonImage(btnClose_Progress, "Cancel");

            Configure(grdResults, null, btnSave, null);
            this.RemoveAfterAdd = true;
            // If PerformUpdateFromGenBank were set true, this form would try to update from GenBank the source Gene.Gene records during the Add To
            // process, but at that point none of the source records would have GenBank IDs because we haven't merged them with their selected 
            // alignment gene.  We still have to do an EFetch from GenBank to get the details for the records we're using to merge, but that's not
            // affected by this particular flag.
            this.PerformUpdateFromGenBank = false; 
            this.SavingAddedGenes += new AddedGeneUpdatesEventHandler(frmBlastNAnnotations_SavingAddedGenes);

            this.Job = Job;
            if (this.Job.HasDatabaseExceptions && this.Job.Exceptions.Count == 0) { this.Job.RefreshExceptionsFromDatabase(); }

            BlastSequencesAtNCBI activityInMemory = Program.InProgressActivities.GetActivityByJobID<BlastSequencesAtNCBI>(this.Job.ID);
            if (activityInMemory != null) { Program.InProgressActivities.RemoveActivity(activityInMemory); }

            if (string.IsNullOrEmpty(this.Job.ID))
            { tbForm.TabPages.Remove(pgHistory); }
            else
            { txtProgress.Text = this.Job.Output; }

            if (this.Job.Exceptions.Count != 0 || this.Job.Status == JobStatuses.Failed || this.Job.Status == JobStatuses.Cancelled)
            { this.FocusOnLoad = grdHistory; }
            else // Default
            { this.FocusOnLoad = grdResults; }
        }

        private void frmBlastNAnnotations_Load(object sender, EventArgs e)
        {
#if DOCUMENTATION
            this.Size = new System.Drawing.Size(806, 600);
#endif

            this.SubjectDataGridHelper = new DataGridViewHelper(this, grdResults, chkToggleSelected);
            this.SubjectDataGridHelper.SelectedRowsChanged += new DataGridViewHelper.SelectedRowsChangedEventHandler(DataGridHelper_SelectedRowsChanged);
            this.SubjectDataGridHelper.CheckBoxChanged += new DataGridViewHelper.SelectedRowsChangedEventHandler(DataGridHelper_CheckBoxChanged);
            this.SubjectDataGridHelper.ViewDetails += new DataGridViewHelper.ViewDetailsEventHandler(DataGridHelper_ViewDetails);

            this.SubjectDataGridHelper.Loaded = false;
            if (!RefreshAlignmentsAsync())
            {
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            }

            if (this.Job.Exceptions.Count != 0 || this.Job.Status == JobStatuses.Failed || this.Job.Status == JobStatuses.Cancelled)
            {
                // We don't need to check for output genes if the job was cancelled because frmBlastNAlignments won't be opened unless there some
                // genes were downloaded before the job was cancelled.
                tbForm.SelectedTab = pgHistory;
            }
        }

        private void frmBlastNAnnotations_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Program.DatabaseSettings.BlastNAtNCBI.Annotate_ReplaceAllSequences = chkReplaceSequencesAll.Checked;
        }

        #region Results
        private List<BlastNAnnotationRowDataItem> Alignments { get; set; }

        private bool RefreshAlignmentsAsync()
        {
            using (ProgressForm = new frmProgress("Retrieving alignment results", new frmProgress.ProgressOptions() { UseNeverEndingTimer = true, AllowCancellation = true }))
            {
                ProgressForm.Cancelled += new frmProgress.CancelledEventHandler(ProgressForm_Cancelled);
                bwAlignments.RunWorkerAsync();

                return (ProgressForm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK);
            }
        }

        private void ProgressForm_Cancelled(RunWorkerCompletedEventArgs e)
        {
            bwAlignments.CancelAsync();
        }

        private void bwAlignments_DoWork(object sender, DoWorkEventArgs e)
        {
            DataSet results = BlastNAtNCBI.ListAnnotationGenes(this.Job.ID);
            List<BlastNAnnotationRowDataItem> alignments =
                results.Tables[0].Rows.Cast<DataRow>().Select(row =>
                {
                    Gene query = Gene.FromDatabaseRow(row, false);

                    // The Merged gene holds in-memory updates to the final edited gene record, allowing the user to customize the annotation before
                    // committing the save against the Query gene.  If the user never opens frmAnnotatedGeneDetails to customize (or just confirm)
                    // the details, btnSave will take into account that the Merged gene is flagged as New, download the full details for the
                    // top-ranked Subject gene, and use it to populate Query before Query.Save() is called.
                    Gene merged = null; bool hasAlignments = true;
                    if (!string.IsNullOrWhiteSpace(row.ToSafeString("SubjectID")))
                    {
                        merged = new Gene(row["SubjectID"].ToString()) { SourceID = (int)row["SubjectSourceID"] };
                        merged.Merge(query);

                        merged.Definition = (string)row["SubjectDefinition"];
                        merged.GenBankID = row.ToSafeInt("SubjectGenBankID");
                        // That's all we'll likely have at first because we most likely haven't done an UpdateFromGenBank yet...
                        merged.LastUpdatedAt = row.ToSafeDateTime("SubjectLastUpdatedAt");
                        merged.RefreshedFromDatabase = false;
                    }
                    else
                    {
                        // Set up Merged as a shell that the user can custom edit.
                        merged = new Gene() { SourceID = query.SourceID, LastUpdateSourceID = query.LastUpdateSourceID };
                        hasAlignments = false;
                    }

                    return new BlastNAnnotationRowDataItem(
                        new BlastSequencesAtNCBI.Alignment() 
                            { Query = query, HasAlignments = hasAlignments, Alignments = null, Merged = merged }) 
                        { SequenceIdentityMatchPercentage = (int)row["SequenceIdentityMatchPercentage"] }
                    ;
                }).ToList();

            if (bwAlignments.CancellationPending) { e.Cancel = true; }
            else { e.Result = alignments; }
        }

        private void bwAlignments_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error == null && !e.Cancelled)
            {
                this.Alignments = (List<BlastNAnnotationRowDataItem>)e.Result;
                LoadAlignments();

                //chkReplaceSequencesAll.Checked = Program.DatabaseSettings.BlastNAtNCBI.Annotate_ReplaceAllSequences;
                UpdateControlsForSelectedRows();
                btnSave.Enabled = false;

                this.SubjectDataGridHelper.Loaded = true;
                this.CloseProgressForm(System.Windows.Forms.DialogResult.OK);
            }
            else
            {
                if (e.Error != null)
                {
                    Utility.ShowErrorMessage(this, e.Error);
                    this.CloseProgressForm(System.Windows.Forms.DialogResult.Cancel);
                }
                else if (e.Cancelled)
                {
                    this.CloseProgressForm(System.Windows.Forms.DialogResult.Cancel);
                }
            }
        }

        private void LoadAlignments()
        {
            IEnumerable<BlastNAnnotationRowDataItem> query = this.Alignments;
            grdResults.AutoGenerateColumns = false;
            grdResults.DataSource = new SortableBindingList<BlastNAnnotationRowDataItem>(query);
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
            lblSelectedRows.Text = this.SelectedGeneRows.Count.ToString("N0") + " of " + grdResults.Rows.Count.ToString("N0") + " records selected";
            btnSave.Enabled = (SelectedGeneRows.Count != 0);
        }

        private void DataGridHelper_SelectedRowsChanged(DataGridViewHelper.SelectedRowEventArgs e)
        {
            UpdateControlsForSelectedRows();
        }

        private void DataGridHelper_CheckBoxChanged(DataGridViewHelper.SelectedRowEventArgs e)
        {
            //if (e.CheckBox.OwningColumn.DataPropertyName == "ReplaceSequence")
            //{
            //    ((BlastNAnnotationRowDataItem)e.UpdatedRow).ReplaceSequence = !((BlastNAnnotationRowDataItem)e.UpdatedRow).ReplaceSequence;
            //    e.CheckBox.Value = ((BlastNAnnotationRowDataItem)e.UpdatedRow).ReplaceSequence;

            //    bool allSelected = (grdResults.Rows.Cast<DataGridViewRow>().All(row => (bool)row.Cells[clmReplaceSequence.Name].Value));
            //    chkReplaceSequencesAll.CheckedChanged -= chkReplaceSequencesAll_CheckedChanged;
            //    chkReplaceSequencesAll.Checked = allSelected;
            //    chkReplaceSequencesAll.CheckedChanged += new EventHandler(chkReplaceSequencesAll_CheckedChanged);

            //    grdResults.EndEdit();
            //}
        }

        private void chkReplaceSequencesAll_CheckedChanged(object sender, EventArgs e)
        {
            //foreach (DataGridViewRow row in grdResults.Rows)
            //{
            //    row.Cells[clmReplaceSequence.Name].Value = chkReplaceSequencesAll.Checked;
            //}
        }

        private void DataGridHelper_ViewDetails(DataGridViewHelper.ViewDetailsEventArgs e)
        {
            BlastNAnnotationRowDataItem rowItem = (BlastNAnnotationRowDataItem)e.Row.DataBoundItem;
            BlastSequencesAtNCBI.Alignment alignment = rowItem.Alignment;

            using (frmAnnotatedGeneDetails frm = new frmAnnotatedGeneDetails() { Alignment = alignment, JobID = this.Job.ID })
            {
                if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    e.Row.Selected = false; e.Row.Selected = true;
                    if (!rowItem.Selected) { rowItem.Selected = true; UpdateControlsForSelectedRows(); }
                }
            }
        }

        private void frmBlastNAnnotations_SavingAddedGenes(object sender, AddGeneSequencesToRecordSetForm.AddedGeneUpdatesEventArgs e)
        {
            try
            {
                int willNotBeMergedCount = this.SelectedGeneRows.Cast<BlastNAnnotationRowDataItem>().Count(row => !row.Alignment.HasAlignments && !row.Alignment.HasMerged);
                if (willNotBeMergedCount > 0)
                {
                    Utility.ShowMessage(this,
                        string.Format("{0} of the selected sequences will not be updated because {1} {2} not matched by NCBI and {2} not manually edited.",
                            willNotBeMergedCount.ToString("N0"),
                            (willNotBeMergedCount == 1 ? "it" : "they"),
                            (willNotBeMergedCount == 1 ? "was" : "were")));
                }

                // Batch together the top-ranked Subject genes that need updating from GenBank
                List<Gene> updatedFromGenBank = this.SelectedGeneRows.Cast<BlastNAnnotationRowDataItem>()
                    .Where(row => !row.Alignment.HasMerged && row.Alignment.Alignments == null && row.Alignment.Merged.NeedsUpdateFromGenBank)
                    .Select(row => row.Alignment.Merged).ToList();
                if (!PopulateFromGenBank.PopulateSync(updatedFromGenBank, null, this, true))
                {
                    e.Cancel = true;
                    return;
                }

                foreach (var row in this.SelectedGeneRows.Cast<BlastNAnnotationRowDataItem>())
                {
                    if (row.Alignment.HasMerged)
                    {
                        // The user has made edits using the Details view, so we take whatever those are, including the Description.
                        row.Alignment.Query.Merge(row.Alignment.Merged, row.Alignment.ReplaceSequence, row.Alignment.ReplaceSequence, true);
                    }
                    else
                    {
                        // The user never opened the Details view, or never saved edits from that screen, so we go ahead and merge in the annotation from
                        // the top-ranked Subject gene, which will already be set as row.Alignment.Subject, unless there were no hits.

                        if (row.Alignment.HasAlignments) // If false, there were no hits for this Query gene, so we don't need to do anything.
                        {
                            if (row.Alignment.Alignments == null)
                            {
                                // User never opened the Details view, so either we just got the details from GenBank or we need them from the database.
                                if (updatedFromGenBank.Contains(row.Alignment.Merged))
                                {
                                    row.Alignment.Query.Merge(row.Alignment.Merged, row.Alignment.ReplaceSequence, row.Alignment.ReplaceSequence, false);
                                }
                                else
                                {
                                    // This tells us that we did, at one point, update from GenBank, so all we need to do is fetch the details.
                                    row.Alignment.Query.Merge(Gene.Get(row.Alignment.Merged.ID, true), row.Alignment.ReplaceSequence, row.Alignment.ReplaceSequence, false);
                                }
                            }
                            else
                            {
                                // User opened the Details view but never saved, so the first Gene will be the top-ranked Subject.
                                row.Alignment.Query.Merge(row.Alignment.Alignments.First(g => !GuidCompare.Equals(g.ID, row.Alignment.Query.ID)).Gene, row.Alignment.ReplaceSequence, row.Alignment.ReplaceSequence, false);
                            }
                        }
                        else
                        {
                            // Exclude it from the save.
                            e.Genes.Remove(row.Alignment.Query);
                        }
                    }

                    row.Alignment.Query.LastUpdatedAt = DateTime.Now;
                    row.Alignment.Query.LastUpdateSource = GeneSources.User;

                    e.Genes.First(selected => GuidCompare.Equals(selected.ID, row.Alignment.Query.ID)).Merge(row.Alignment.Query);
                }

                // Now get a list of subsets for which these genes belong to, so that the calling form can refresh any of those that are currently open.
                this.UpdatedSubSetIDs.AddRange(
                    ChangLab.RecordSets.SubSet.ListSubSetIDsForGeneIDs(this.SelectedGeneRows.Cast<BlastNAnnotationRowDataItem>().Select(row => row.Alignment.Query.ID))
                    .Where(id => !this.UpdatedSubSetIDs.Contains(id))
                );
            }
            catch (Exception ex)
            {
                e.Error = ex;
                e.Cancel = true;
            }
        }

        public class BlastNAnnotationRowDataItem : GenericGeneRowDataItem
        {
            internal BlastSequencesAtNCBI.Alignment Alignment { get; private set; }

            public string OriginalDefinition { get { return this.Alignment.Query.Definition; } }
            public string NewDefinition { get { return this.Alignment.Merged.Definition; } }
            public string NewOrganism { get { return this.Alignment.Merged.Organism; } }
            public string NewGenBankURL { get { return this.Alignment.Merged.GenBankUrl; } }
            public string NewGeneName { get { return this.Alignment.Merged.GeneName; } }
            public bool ReplaceSequence { get { return this.Alignment.ReplaceSequence; } set { this.Alignment.ReplaceSequence = value; } }
            public int SequenceIdentityMatchPercentage { get; set; }

            public override string ID
            {
                get { return this.Alignment.Query.ID; }
            }

            public BlastNAnnotationRowDataItem(BlastSequencesAtNCBI.Alignment Alignment) : base(Alignment.Query)
            {
                this.Alignment = Alignment;
            }
        }
        #endregion

        #region Query Sequences, Request History, and Progress
        private DataGridViewHelper QuerySequencesGridHelper { get; set; }
        private DataGridViewHelper HistoryGridHelper { get; set; }

        private void tbForm_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPage == pgHistory && grdHistory.DataSource == null)
            {
                this.HistoryGridHelper = new DataGridViewHelper(this, grdHistory);
                this.HistoryGridHelper.ViewDetails += new DataGridViewHelper.ViewDetailsEventHandler(HistoryGridHelper_ViewDetails);
                this.HistoryGridHelper.Loaded = false;
                this.grdHistory.AutoGenerateColumns = false;
                clmStartedAt.DefaultCellStyle.Format = DataGridViewHelper.DateTimeFormatStringWithSeconds;
                clmEndedAt.DefaultCellStyle.Format = DataGridViewHelper.DateTimeFormatStringWithSeconds;
                using (DataTable requestHistory = BlastNAtNCBI.ListRequests(this.Job.ID))
                {
                    IEnumerable<RequestHistoryRow> historyRows = requestHistory.Rows.Cast<DataRow>().Select(row => RequestHistoryRow.FromDatabaseRow(row));
                    this.grdHistory.DataSource = new SortableBindingList<RequestHistoryRow>(historyRows);
                }
                this.HistoryGridHelper.Loaded = true;
            }

            switch (e.TabPage.Name)
            {
                case "pgResults": this.CancelButton = btnClose_Results; break;
                case "pgHistory": this.CancelButton = btnClose_History; break;
                case "pgProgress": this.CancelButton = btnClose_Progress; break;
            }
        }

        private void HistoryGridHelper_ViewDetails(DataGridViewHelper.ViewDetailsEventArgs e)
        {
            using (frmRequestDetails frm = new frmRequestDetails((RequestHistoryRow)e.Row.DataBoundItem, this.Job.ID))
            {
                frm.ShowDialog(this);
                this.EditedSubSetIDs.AddRange(frm.EditedSubSetIDs);
            }
        }

        private void grdHistory_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex != -1 && (e.ColumnIndex == grdHistory.Columns[clmEndedAt.Name].Index || e.ColumnIndex == grdHistory.Columns[clmStartedAt.Name].Index))
            {
                if (e.Value != null && e.Value.GetType() == typeof(DateTime) && (DateTime)e.Value == DateTime.MinValue)
                {
                    e.Value = string.Empty;
                }
            }
        }

        private void btnSaveProgress_Click(object sender, EventArgs e)
        {
            string filePath = string.Empty;
            if (IODialogHelper.SaveFile(IODialogHelper.DialogPresets.Text, "progress.txt", this, ref filePath))
            {
                System.IO.File.WriteAllLines(filePath, txtProgress.Lines);
            }
        }
        #endregion
    }
}
