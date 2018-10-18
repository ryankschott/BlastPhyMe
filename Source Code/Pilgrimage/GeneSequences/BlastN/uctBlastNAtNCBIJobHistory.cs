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
using ChangLab.NCBI.GenBank;
using ChangLab.RecordSets;
using Pilgrimage.Activities;

namespace Pilgrimage
{
    public partial class uctBlastNAtNCBIJobHistory : UserControl
    {
        private RecordSet CurrentRecordSet { get { return Program.Settings.CurrentRecordSet; } }
        private DataGridViewHelper DataGridHelper { get; set; }

        private List<BlastNHistoryRow> JobHistory { get; set; }
        public List<BlastNHistoryRow> SelectedJobRows
        {
            get
            {
                if (grdJobHistory.DataSource == null) { return new List<BlastNHistoryRow>(); }
                else
                {
                    return grdJobHistory
                        .Rows
                        .Cast<DataGridViewRow>()
                        .Where(row => ((BlastNHistoryRow)row.DataBoundItem).Selected)
                        .Select(row => (BlastNHistoryRow)row.DataBoundItem)
                        .ToList();
                }
            }
        }

        public List<BlastNAtNCBI> SelectedJobs
        {
            get
            {
                return this.SelectedJobRows.Select(row => row.Job).ToList();
            }
        }

        internal List<string> EditedSubSetIDs { get; set; }

        public uctBlastNAtNCBIJobHistory()
        {
            InitializeComponent();
            this.EditedSubSetIDs = new List<string>();
        }

        public void Initialize(Form ParentForm)
        {
            this.DataGridHelper = new DataGridViewHelper(this.ParentForm, grdJobHistory, DataGridViewHelper.DataSourceTypes.BLASTNResultsHistory);
            this.DataGridHelper.ViewDetails += new DataGridViewHelper.ViewDetailsEventHandler(DataGridHelper_ViewDetails);
        }

        public void Clear()
        {
            grdJobHistory.DataSource = null;
        }

        public void RefreshHistory()
        {
            try
            {
                this.DataGridHelper.Loaded = false;

                JobHistory = Job.ListAsDataTable(CurrentRecordSet.ID, JobTargets.BLASTN_NCBI)
                    .Rows.Cast<DataRow>()
                    .Select(row => {
                        BlastNAtNCBI job = new BlastNAtNCBI(string.Empty); job.FromDataRow(row);
                        BlastNHistoryRow historyRow = new BlastNHistoryRow(job)
                        {
                            InputGenesCount = (int)row["InputGeneCount"],
                            StatusName = (string)row["JobStatusName"],
                            SubSetName = (string)row["InputSubSetName"]
                        };
                        return historyRow;
                    }
                    ).ToList();

                grdJobHistory.AutoGenerateColumns = false;
                grdJobHistory.DataSource = null;
                grdJobHistory.DataSource = new SortableBindingList<BlastNHistoryRow>(JobHistory);

                this.DataGridHelper.Loaded = true;
            }
            catch (Exception ex)
            {
                Utility.ShowErrorMessage(this, ex);
            }
        }

        public void ArchiveSelected()
        {
            if (this.SelectedJobRows.Count == 0)
            {
                Utility.ShowMessage(this, Properties.Resources.Messages_NoResultsSelected);
            }
            else
            {
                if (Utility.ShowMessage(this, "Are you sure you want to archive the selected " + this.SelectedJobRows.Count.ToString()
                                                + " result" + (this.SelectedJobRows.Count == 1 ? string.Empty : "s") + "?",
                                                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    this.SelectedJobs.ForEach(j => j.Archive());
                    this.CurrentRecordSet.Save(); // Update the ModifiedAt value.
                    this.RefreshHistory();
                }
            }
        }

        private void DataGridHelper_ViewDetails(DataGridViewHelper.ViewDetailsEventArgs e)
        {
            BlastNAtNCBI selectedJob = ((BlastNHistoryRow)e.Row.DataBoundItem).Job;

            switch (selectedJob.Status)
            {
                case JobStatuses.Completed:
                case JobStatuses.Cancelled:
                case JobStatuses.Archived:
                case JobStatuses.Reviewed:
                case JobStatuses.Failed:
                    using (GeneSequences.BlastN.frmBlastNAlignments frm = new GeneSequences.BlastN.frmBlastNAlignments(selectedJob))
                    {
                        frm.ShowDialog(this);
                        if (frm.EditedSubSetIDs.Count != 0)
                        {
                            this.EditedSubSetIDs.AddRange(frm.EditedSubSetIDs.Where(id => !this.EditedSubSetIDs.Contains(id)));
                        }
                    }
                    break;
                case JobStatuses.New:
                case JobStatuses.Running:
                    break;
            }
        }

        private void grdJobHistory_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == grdJobHistory.Columns[clmBlastNHistory_EndedAt.Name].Index
                && e.RowIndex != -1)
            {
                if (e.Value != null && e.Value.GetType() == typeof(DateTime) && (DateTime)e.Value == DateTime.MinValue)
                {
                    e.Value = string.Empty;
                }
            }
        }

        public class BlastNHistoryRow : RowDataItem
        {
            public override string ID { get { return Job.ID; } }

            public BlastNAtNCBI Job { get; set; }
            public int InputGenesCount { get; set; }
            public string StatusName { get; set; }
            public string SubSetName { get; set; }
            public string InputDescription
            {
                get
                {
                    return InputGenesCount.ToString() 
                        + (string.IsNullOrWhiteSpace(SubSetName) ? string.Empty : " from \"" + SubSetName + "\"");
                }
            }

            public DateTime StartedAt { get { return Job.StartTime; } }
            public DateTime EndedAt { get { return Job.EndTime; } }

            public BlastNHistoryRow(BlastNAtNCBI Job)
            {
                this.Job = Job;
            }
        }
    }
}
