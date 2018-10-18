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
using ChangLab.PAML.CodeML;
using Pilgrimage.Activities;

namespace Pilgrimage.PAML
{
    public partial class frmJobHistory : JobHistoryForm
    {
        internal override DataGridView HistoryGridView { get { return grdJobHistory; } }
        internal Job SelectedJob { get; set; }
        internal RunCodeMLAnalysis RunningAnalysis { get; private set; }
        private bool JobReRunFromResultsForm { get; set; }

        public frmJobHistory()
            : base(JobTargets.CodeML, DataGridViewHelper.DataSourceTypes.BLASTNResultsHistory)
        {
            InitializeComponent();
            SetButtonImage(btnClose, DialogButtonPresets.Close);

#if DOCUMENTATION
            this.Size = new System.Drawing.Size(806, 600);
#endif
        }

        private void frmJobHistory_Load(object sender, EventArgs e)
        {
            if (this.DesignMode) { return; }

            // Custom context menu for the data grid so that we can have the "Cancel codeml.exe" option
            ContextMenuStrip contextMenu = new ContextMenuStrip();
            ToolStripMenuItem detailsMenuItem = new ToolStripMenuItem("&Details") { Tag = "Details" };
            contextMenu.Items.Add(detailsMenuItem);
            ToolStripMenuItem reRunJobMenuItem = new ToolStripMenuItem("&Run Job Again");
            reRunJobMenuItem.Click += new EventHandler(reRunJobMenuItem_Click);
            contextMenu.Items.Add(reRunJobMenuItem);
            this.DataGridContextMenuStrip = contextMenu;

            base.JobHistoryForm_Load(sender, e);
            RefreshHistory(new SortableBindingList<PAMLHistoryRow>(JobHistory.Cast<PAMLHistoryRow>()));

            if (this.SelectedJob != null)
            {
                DataGridViewRow selectedRow = HistoryGridView.Rows.Cast<DataGridViewRow>().FirstOrDefault(row => GuidCompare.Equals(((JobRowDataItem)row.DataBoundItem).ID, this.SelectedJob.ID));
                if (selectedRow != null)
                {
                    HistoryGridView.ClearSelection();
                    selectedRow.Selected = true;

                    DataGridHelper_ViewDetails(new DataGridViewHelper.ViewDetailsEventArgs(selectedRow));
                }
            }

            foreach (RunCodeMLAnalysis analysis in Program.InProgressActivities.CodeMLAnalyses)
            {
                if (!analysis.Completed)
                {
                    analysis.StatusUpdate += new StatusUpdateEventHandler(Job_StatusUpdate);
                }
            }
        }

        private void reRunJobMenuItem_Click(object sender, EventArgs e)
        {
            PAMLHistoryRow row = (PAMLHistoryRow)grdJobHistory.SelectedRows[0].DataBoundItem;

            if (Utility.ShowMessage(this.OwnerForm, "Create a new PAML job using this job's configuration?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                using (frmCreateJob frm = new frmCreateJob(row.ID))
                {
                    if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                    {
                        this.RunningAnalysis = frm.Analysis;
                        this.DialogResult = System.Windows.Forms.DialogResult.OK;
                        // The calling form will take the RunningAnalysis property and pop a frmJobProgress form for it.
                    }
                }
            }
        }

        private void frmJobHistory_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (RunCodeMLAnalysis analysis in Program.InProgressActivities.CodeMLAnalyses)
            {
                analysis.StatusUpdate -= Job_StatusUpdate;
            }
        }

        internal override void DataGridHelper_ViewDetails(DataGridViewHelper.ViewDetailsEventArgs e)
        {
            PAMLHistoryRow row = (PAMLHistoryRow)e.Row.DataBoundItem;

            RunCodeMLAnalysis inMemory = Program.InProgressActivities.GetActivityByJobID<RunCodeMLAnalysis>(row.ID);
            if (inMemory != null && inMemory.CurrentJob.LastCompletedStep != Job.RunStep.Complete)
            {
                // The job is still running and we can get at its activity object in-memory
                using (frmJobProgress frm = new frmJobProgress(inMemory))
                {
                    if (frm.ShowDialog(this) != DialogResult.OK)
                    {
                        return; /* Cancel or Ignore will come back depending on the button selected */
                    }
                }
            }

            using (frmResults results = new frmResults(row.ID))
            {
                Program.InProgressActivities.ActivityAdded += new InProgress.ActivityAddedEventHandler(InProgressActivities_ActivityAdded);
                this.JobReRunFromResultsForm = false;

                results.ShowDialog();
                if (results.EditedSubSetIDs.Count != 0)
                {
                    this.EditedSubSetIDs.AddRange(results.EditedSubSetIDs.Where(id => !this.EditedSubSetIDs.Contains(id)));

                    row.InRecordSet = true;
                    e.Row.Selected = false; e.Row.Selected = true;
                }

                if (this.JobReRunFromResultsForm)
                {
                    this.JobHistory = ListJobHistory(JobTargets.CodeML);
                    RefreshHistory(new SortableBindingList<PAMLHistoryRow>(JobHistory.Cast<PAMLHistoryRow>()));
                }
            }
        }

        private void InProgressActivities_ActivityAdded(InProgress.ActivityEventArgs e)
        {
            this.JobReRunFromResultsForm = true;
            Program.InProgressActivities.ActivityAdded -= InProgressActivities_ActivityAdded;
        }
    }
}
