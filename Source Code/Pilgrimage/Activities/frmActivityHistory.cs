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
using Pilgrimage.Activities;

namespace Pilgrimage.Activities
{
    public partial class frmActivityHistory<T> : JobHistoryForm where T : Activity
    {
        internal override DataGridView HistoryGridView { get { return grdJobHistory; } }
        internal Activity SelectedActivity { get; set; }

        public frmActivityHistory(JobTargets Target) : base(Target, DataGridViewHelper.DataSourceTypes.BLASTNResultsHistory /* Works fine for the other types of jobs... */)
        {
            InitializeComponent();
            SetButtonImage(btnClose, DialogButtonPresets.Close);

            switch (Target)
            {
                case JobTargets.BLASTN_NCBI: this.Text = "BLASTN Alignment History"; break;
                case JobTargets.PRANK: this.Text = "PRANK Alignment History"; break;
                case JobTargets.MUSCLE: this.Text = "MUSCLE Alignment History"; break;
                case JobTargets.PhyML: this.Text = "PhyML History"; break;
                default: this.Text = "History"; break;
            }

#if DOCUMENTATION
            this.Size = new System.Drawing.Size(806, 600);
#endif
        }

        protected override void JobHistoryForm_Load(object sender, EventArgs e)
        {
            if (this.DesignMode) { return; }

            base.JobHistoryForm_Load(sender, e);
            RefreshHistory(new SortableBindingList<GeneProcessingJobHistoryRow>(JobHistory.Cast<GeneProcessingJobHistoryRow>()));

            if (this.SelectedActivity != null)
            {
                DataGridViewRow selectedRow = HistoryGridView.Rows.Cast<DataGridViewRow>().FirstOrDefault(row => GuidCompare.Equals(((GeneProcessingJobHistoryRow)row.DataBoundItem).ID, this.SelectedActivity.CurrentJob.ID));
                if (selectedRow != null)
                {
                    HistoryGridView.ClearSelection();
                    selectedRow.Selected = true;

                    DataGridHelper_ViewDetails(new DataGridViewHelper.ViewDetailsEventArgs(selectedRow));
                }
            }

            foreach (T activity in Program.InProgressActivities.ListActivities<T>())
            {
                if (!activity.Completed)
                {
                    activity.StatusUpdate += new StatusUpdateEventHandler(Job_StatusUpdate);
                }
            }
        }

        private void frmAlignmentHistory_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (T activity in Program.InProgressActivities.ListActivities<T>())
            {
                if (!activity.Completed)
                {
                    activity.StatusUpdate -= Job_StatusUpdate;
                }
            }
        }

        internal override void DataGridHelper_ViewDetails(DataGridViewHelper.ViewDetailsEventArgs e)
        {
            GeneProcessingJobHistoryRow row = (GeneProcessingJobHistoryRow)e.Row.DataBoundItem;
            e.Args = row.ID;

            // If the job is still running, intercept and go to frmJobProgress first.
            Activity activity = Program.InProgressActivities.GetActivityByJobID(row.ID);
            if (activity != null && activity.CurrentJob.LastCompletedStep != Job.RunStep.Complete)
            {
                using (frmActivityProgress frm = new frmActivityProgress() { Activity = activity })
                {
                    OnShowProgressForm(new ProgressForm.ProgressFormEventArgs() { Form = frm, Activity = activity });

                    if (frm.ShowDialog(this) != DialogResult.OK) { e.Cancel = true; return; /* Cancel or Ignore will come back depending on the button selected */ }
                }
            }

            ViewDetails(e);
        }

        protected virtual void OnViewDetails(DataGridViewHelper.ViewDetailsEventArgs e)
        {
            if (ViewDetails != null)
            {
                ViewDetails(e);
            }
        }
        public delegate void ViewDetailsEventHandler(DataGridViewHelper.ViewDetailsEventArgs e);
        public event ViewDetailsEventHandler ViewDetails;

        protected virtual void OnShowProgressForm(ProgressForm.ProgressFormEventArgs e)
        {
            if (ShowProgressForm != null)
            {
                ShowProgressForm(e);
            }
        }
        public delegate void ProgressFormEventHandler(ProgressForm.ProgressFormEventArgs e);
        public event ProgressFormEventHandler ShowProgressForm;

    }
}
