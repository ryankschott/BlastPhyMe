using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChangLab.BlastN;
using ChangLab.Jobs;
using ChangLab.Common;
using ChangLab.Genes;
using ChangLab.RecordSets;
using Pilgrimage.Activities;

namespace Pilgrimage.GeneSequences.BlastN
{
    public partial class frmBlastNAlignments : AddGeneSequencesToRecordSetForm
    {
        private BlastNAtNCBI Job { get; set; }
        private FilterProperties Filter { get; set; }

        public frmBlastNAlignments(BlastNAtNCBI Job) : base()
        {
            InitializeComponent();
            SetButtonImage(btnSave, "Add");
            SetButtonImage(btnClose_Results, "Cancel");
            SetButtonImage(btnClose_QuerySequences, "Cancel");
            SetButtonImage(btnResubmit, "Refresh");
            SetButtonImage(btnClose_History, "Cancel");
            
            Configure(grdResults, cmbSubSets, btnSave, chkUpdateFromGenBank);
            RemoveAfterAdd = true;

            this.Job = Job;
            if (this.Job.HasDatabaseExceptions && this.Job.Exceptions.Count == 0) { this.Job.RefreshExceptionsFromDatabase(); }
            this.Filter = new FilterProperties();

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

        public frmBlastNAlignments(string JobID) : this(new BlastNAtNCBI(JobID)) { }

        private void frmBlastNAlignments_Load(object sender, EventArgs e)
        {
#if DOCUMENTATION
            this.Size = new System.Drawing.Size(806, 600);
#endif

            this.SubjectDataGridHelper = new DataGridViewHelper(this, grdResults);
            this.SubjectDataGridHelper.SelectedRowsChanged += new DataGridViewHelper.SelectedRowsChangedEventHandler(DataGridHelper_SelectedRowsChanged);
            this.SubjectDataGridHelper.ViewDetails += new DataGridViewHelper.ViewDetailsEventHandler(DataGridHelper_ViewDetails);

            this.SubjectDataGridHelper.Loaded = false;
            this.RemovingRows += new DataGridViewRowsActionEventHandler(frmBlastNAlignments_RemovingRows);

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

        #region Results
        private List<BlastNAlignmentRow> Alignments { get; set; }

        private void bwAlignments_DoWork(object sender, DoWorkEventArgs e)
        {
            List<BlastNAlignmentRow> alignments = new List<BlastNAlignmentRow>();
            BlastNAtNCBI.ListAlignments(
                    Convert.ToInt32(this.Filter.ResultsExclusion),
                    this.Job.ID,
                    Program.Settings.CurrentRecordSet.ID
                ).Rows.Cast<DataRow>().ToList().ForEach(row =>
                {
                    alignments.Add(new BlastNAlignmentRow(row));
                });

            if (bwAlignments.CancellationPending) { e.Cancel = true; }
            else { e.Result = alignments; }
        }

        private void bwAlignments_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error == null && !e.Cancelled)
            {
                this.Alignments = (List<BlastNAlignmentRow>)e.Result;
                LoadAlignments();

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
            IEnumerable<BlastNAlignmentRow> query = this.Alignments;
            int totalRows = query.Count();
            if (!string.IsNullOrWhiteSpace(this.Filter.Definition))
            {
                query = query.Where(r => r.Definition.Match(this.Filter.Definition, this.Filter.DefinitionMatchLogic));
            }
            int filteredRows = query.Count();

            grdResults.AutoGenerateColumns = false;
            grdResults.DataSource = new SortableBindingList<BlastNAlignmentRow>(query);
            UpdateTotalRowsLabel(filteredRows, totalRows);

            if (!string.IsNullOrWhiteSpace(this.Filter.Definition))
            {
                lnkFilter.Text = "Filtered by Definition";
            }
            else { lnkFilter.Text = string.Empty; }

            switch (this.Filter.ResultsExclusion)
            {
                case BlastNAlignmentResultsExclusions.ExistsByAccessionVersion:
                    lnkFilter.Text += (!string.IsNullOrWhiteSpace(lnkFilter.Text) ? ", e" : "E") + "xcluded by Accession";
                    clmInRecordSet.Visible = false; // Because by definition we're filtering to only show what's not in the recordset...
                    break;
                case BlastNAlignmentResultsExclusions.ExistsByOrganismName:
                    lnkFilter.Text += (!string.IsNullOrWhiteSpace(lnkFilter.Text) ? ", e" : "E") + "xcluded by Organism Name";
                    clmInRecordSet.Visible = false; // Because by definition we're filtering to only show what's not in the recordset...
                    break;
                default:
                    if (string.IsNullOrWhiteSpace(lnkFilter.Text)) { lnkFilter.Text = "Apply Filter"; }
                    clmInRecordSet.Visible = true;
                    break;
            }

            lnkClearFilter.Text = (filteredRows != totalRows ? "Clear Filter" : string.Empty);
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

        private void DataGridHelper_ViewDetails(DataGridViewHelper.ViewDetailsEventArgs e)
        {
            Gene gene = ((BlastNAlignmentRow)e.Row.DataBoundItem).Gene;
            bool populate = (gene.NeedsUpdateFromGenBank);

            using (frmSubjectGeneDetails frm = new frmSubjectGeneDetails(this.Job, gene.ID))
            {
                frm.ShowDialog(this);
                if (populate)
                {
                    gene.Merge(frm.SubjectGene);
                    e.Row.Selected = false; e.Row.Selected = true;
                }
            }
        }
        
        private void lnkFilter_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FilterProperties updatedFilter = null;

            if (sender == lnkFilter)
            {
                using (frmBlastNAlignmentsFilter frm = new frmBlastNAlignmentsFilter(this.Filter))
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

            bool exclusionChanged = (this.Filter.ResultsExclusion != updatedFilter.ResultsExclusion);
            this.Filter = updatedFilter;

            List<string> selectedGeneIDs = SelectedGenes.Select(g => g.ID).ToList();
            this.Alignments.ForEach(al => al.Selected = false); // Clear this so that we don't have any orphaned alignments flagged as selected.

            if (exclusionChanged) // We'll need to reload from the database.
            {
                if (!RefreshAlignmentsAsync()) { return; } // There was an error or the user cancelled.
            }
            else // We can just refresh using what we've got.
            {
                LoadAlignments();
            }

            // Re-select what rows are still visible.
            grdResults.Rows.Cast<DataGridViewRow>()
                .Where(row => selectedGeneIDs.Contains(((RowDataItem)row.DataBoundItem).ID))
                .ToList()
                .ForEach(row => ((RowDataItem)row.DataBoundItem).Selected = true);

            DataGridHelper_SelectedRowsChanged(null);
            grdResults.Focus();
        }

        protected internal override void GenBankCompleted(RunWorkerCompletedEventArgs e)
        {
            base.GenBankCompleted(e);
            if (e.Error == null && !e.Cancelled)
            {
                UpdateTotalRowsLabel(grdResults.Rows.Count, this.Alignments.Count);
                UpdateControlsForSelectedRows();
                grdResults.Focus();
            }
        }

        private void frmBlastNAlignments_RemovingRows(DataGridViewRowsActionEventArgs e)
        {
            this.Alignments.RemoveAll(al => e.Rows.Select(row => ((GenericGeneRowDataItem)row.DataBoundItem).ID).Contains(al.ID));
        }
        #endregion

        #region Query Sequences, Request History, and Progress
        private DataGridViewHelper QuerySequencesGridHelper { get; set; }
        private DataGridViewHelper HistoryGridHelper { get; set; }

        private void tbForm_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPage == pgQuerySequences && grdQuerySequences.DataSource == null)
            {
                this.QuerySequencesGridHelper = new DataGridViewHelper(this, grdQuerySequences);
                this.QuerySequencesGridHelper.ViewDetails += new DataGridViewHelper.ViewDetailsEventHandler(QuerySequencesGridHelper_ViewDetails);
                this.QuerySequencesGridHelper.Loaded = false;
                this.grdQuerySequences.AutoGenerateColumns = false;
                this.grdQuerySequences.DataSource = new SortableBindingList<GenericGeneRowDataItem>(BlastNAtNCBI.ListInputGenesForJob(this.Job.ID, Program.Settings.CurrentRecordSet.ID).ToRowDataItemList());
                this.QuerySequencesGridHelper.Loaded = true;
            }
            else if (e.TabPage == pgHistory && grdHistory.DataSource == null)
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
                    btnResubmit.Visible = false; // historyRows.Any(row => row.LastStatus != ChangLab.NCBI.RequestStatus.Ready);
                    this.grdHistory.DataSource = new SortableBindingList<RequestHistoryRow>(historyRows);
                }
                this.HistoryGridHelper.Loaded = true;
            }

            this.CancelButton = (e.TabPage == pgResults ? btnClose_Results
                                    : e.TabPage == pgQuerySequences ? btnClose_QuerySequences
                                    : e.TabPage == pgHistory ? btnClose_History
                                    : btnClose_Progress);
        }

        private void QuerySequencesGridHelper_ViewDetails(DataGridViewHelper.ViewDetailsEventArgs e)
        {
            Gene gene = ((GenericGeneRowDataItem)e.Row.DataBoundItem).Gene;
            using (GeneSequences.frmGeneDetails frm = new GeneSequences.frmGeneDetails(gene.ID, true))
            {
                frm.ShowDialog(this);
                if (frm.Updated)
                {
                    gene.Merge(frm.Gene);
                }
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

        private void btnResubmit_Click(object sender, EventArgs e)
        {
            IEnumerable<RequestHistoryRow> rows = ((SortableBindingList<RequestHistoryRow>)this.grdHistory.DataSource).Where(row => row.LastStatus != ChangLab.NCBI.RequestStatus.Ready);

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

    public class RequestHistoryRow : RowDataItem
    {
        public override string ID
        {
            // NCBI.Request has been converted from uniqueidentifier to int for its ID
            get { throw new NotImplementedException(); }
        }

        public int DatabaseID { get; private set; }
        public string RequestID { get; set; }
        public DateTime StartedAt { get; private set; }
        public DateTime EndedAt { get; private set; }
        public ChangLab.NCBI.RequestStatus LastStatus { get; set; }
        public string StatusInformation { get; set; }
        public string TargetDatabase { get; set; }
        public string Algorithm { get; set; }
        public int GeneCount { get; set; }

        public RequestHistoryRow() { }

        public static RequestHistoryRow FromDatabaseRow(DataRow Row)
        {
            return new RequestHistoryRow()
            {
                DatabaseID = (int)Row["ID"],
                RequestID = Row.ToSafeString("RequestID"),
                StartedAt = Row.ToSafeDateTime("StartTime"),
                EndedAt = Row.ToSafeDateTime("EndTime"),
                LastStatus = ChangLab.NCBI.Request.ParseRequestStatus((string)Row["LastStatus"]),
                StatusInformation = (string)Row["StatusInformation"],
                TargetDatabase = Row.ToSafeString("TargetDatabase"),
                Algorithm = Row.ToSafeString("Algorithm"),
                GeneCount = (int)Row["GeneCount"]
            };
        }
    }
}