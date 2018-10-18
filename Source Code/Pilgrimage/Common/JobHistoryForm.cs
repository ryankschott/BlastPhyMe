using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChangLab.Common;
using ChangLab.Jobs;

namespace Pilgrimage
{
    public partial class JobHistoryForm : DialogForm
    {
        internal virtual DataGridView HistoryGridView { get { throw new NotImplementedException(); } }
        internal DataGridViewHelper DataGridHelper { get; set; }
        private DataGridViewHelper.DataSourceTypes DataSourceType { get; set; }
        internal List<string> EditedSubSetIDs { get; set; }
        internal ContextMenuStrip DataGridContextMenuStrip { get; set; }

        protected List<JobRowDataItem> JobHistory { get; set; }

        public JobHistoryForm()
        {
            InitializeComponent();
        }

        public JobHistoryForm(JobTargets Target, DataGridViewHelper.DataSourceTypes DataSourceType)
        {
            this.DataSourceType = DataSourceType;
            this.EditedSubSetIDs = new List<string>();
            this.JobHistory = ListJobHistory(Target);
        }

        protected virtual void JobHistoryForm_Load(object sender, EventArgs e)
        {
            if (this.DesignMode) { return; }

            this.DataGridHelper = new DataGridViewHelper(this, HistoryGridView, null, this.DataSourceType, true, DataGridContextMenuStrip);
            this.DataGridHelper.ViewDetails += new DataGridViewHelper.ViewDetailsEventHandler(DataGridHelper_ViewDetails);
            this.HistoryGridView.CellFormatting += new DataGridViewCellFormattingEventHandler(HistoryGridView_CellFormatting);
        }
        
        protected List<JobRowDataItem> ListJobHistory(JobTargets Target)
        {
            return Job.ListAsDataTable(Program.Settings.CurrentRecordSet.ID, Target)
                    .Rows.Cast<DataRow>()
                    .Select<DataRow, JobRowDataItem>(row =>
                    {
                        switch (Target)
                        {
                            case JobTargets.BLASTN_NCBI:
                                return new Pilgrimage.GeneSequences.BlastN.BlastNHistoryRow(row);
                            case JobTargets.CodeML:
                                return new Pilgrimage.PAML.PAMLHistoryRow(row);
                            case JobTargets.PRANK:
                                return new Pilgrimage.GeneSequences.PRANK.PRANKHistoryRow(row);
                            case JobTargets.MUSCLE:
                                return new Pilgrimage.GeneSequences.MUSCLE.MUSCLEHistoryRow(row);
                            case JobTargets.PhyML:
                                return new Pilgrimage.GeneSequences.PhyML.PhyMLHistoryRow(row);
                            default:
                                return null;
                        }
                    }).ToList();
        }

        protected void RefreshHistory<T>(SortableBindingList<T> DataSource) where T : JobRowDataItem
        {
            try
            {
                this.DataGridHelper.Loaded = false;
                HistoryGridView.AutoGenerateColumns = false;
                HistoryGridView.DataSource = null;
                HistoryGridView.DataSource = DataSource;
                this.DataGridHelper.Loaded = true;
            }
            catch (Exception ex)
            {
                Utility.ShowErrorMessage(this, ex);
            }
        }

        public void Clear()
        {
            HistoryGridView.DataSource = null;
        }

        private void HistoryGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == HistoryGridView.Columns["clmEndedAt"].Index && e.RowIndex != -1)
            {
                if (e.Value != null && e.Value.GetType() == typeof(DateTime) && (DateTime)e.Value == DateTime.MinValue)
                {
                    e.Value = string.Empty;
                }
            }
        }

        internal virtual void DataGridHelper_ViewDetails(DataGridViewHelper.ViewDetailsEventArgs e)
        {
            throw new NotImplementedException();
        }

        protected virtual void Job_StatusUpdate(Job sender, StatusUpdateEventArgs e)
        {
            DataGridViewRow match = HistoryGridView.Rows.Cast<DataGridViewRow>().FirstOrDefault(row => GuidCompare.Equals(((JobRowDataItem)row.DataBoundItem).ID, sender.ID));
            if (match != null)
            {
                ((JobRowDataItem)match.DataBoundItem).Status = sender.Status;
                ((JobRowDataItem)match.DataBoundItem).EndedAt = sender.EndTime;

                if (match.Selected) { match.Selected = false; match.Selected = true; }
                else { match.Selected = true; match.Selected = false; }

                // Reapply the current sorting.
                HistoryGridView.Sort(
                    (HistoryGridView.SortedColumn != null ? HistoryGridView.SortedColumn : HistoryGridView.Columns["clmStartedAt"]),
                    (HistoryGridView.SortOrder == SortOrder.Ascending ? ListSortDirection.Ascending : ListSortDirection.Descending));
            }
        }
    }
}
