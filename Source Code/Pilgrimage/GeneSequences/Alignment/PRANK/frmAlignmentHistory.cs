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

namespace Pilgrimage.GeneSequences.PRANK
{
    public partial class frmAlignmentHistory<T> : JobHistoryForm where T : Activity
    {
        internal override DataGridView HistoryGridView { get { return grdJobHistory; } }

        public frmAlignmentHistory(JobTargets Target) : base(Target, DataGridViewHelper.DataSourceTypes.BLASTNResultsHistory /* Close enough... */)
        {
            InitializeComponent();
            SetButtonImage(btnClose, DialogButtonPresets.Close);

#if DOCUMENTATION
            this.Size = new System.Drawing.Size(806, 600);
#endif
        }

        protected override void JobHistoryForm_Load(object sender, EventArgs e)
        {
            if (this.DesignMode) { return; }

            base.JobHistoryForm_Load(sender, e);
            RefreshHistory(new SortableBindingList<GeneProcessingJobHistoryRow>(JobHistory.Cast<GeneProcessingJobHistoryRow>()));

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

            // If the job is still running, intercept and go to frmJobProgress first.
            Activity activity = Program.InProgressActivities.GetActivityByJobID(row.ID);
            if (activity != null && activity.CurrentJob.LastCompletedStep != Job.RunStep.Complete)
            {
                using (frmActivityProgress frm = new frmActivityProgress() { Activity = activity })
                { if (frm.ShowDialog(this) != DialogResult.OK) { e.Cancel = true; return; /* Cancel or Ignore will come back depending on the button selected */ } }
            }

            using (frmAlignmentResults frm = new frmAlignmentResults(row.ID))
            {
                frm.ShowDialog();
                if (frm.EditedSubSetIDs.Count != 0)
                { this.EditedSubSetIDs.AddRange(frm.EditedSubSetIDs.Where(id => !this.EditedSubSetIDs.Contains(id))); }
            }
        }
    }
}
