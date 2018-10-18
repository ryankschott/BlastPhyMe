using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChangLab.Common;
using ChangLab.PAML.CodeML;
using Pilgrimage.Activities;

namespace Pilgrimage.PAML
{
    public partial class frmJobProgress : DialogForm
    {
        internal RunCodeMLAnalysis Analysis { get; private set; }
        private DataGridViewHelper DataGridHelper { get; set; }
        private string StartedAtText { get; set; }

        public delegate void RefreshDelegate();
        public RefreshDelegate refreshGridDelegate;
        public RefreshDelegate refreshLabelDelegate;
        public delegate void RefreshRowDelegate(CodeMLAnalysisOption Option);
        public RefreshRowDelegate refreshRowDelegate;
        public delegate void RefreshProgressFormDelegate(ProgressUpdateEventArgs e);
        public RefreshProgressFormDelegate refreshProgressFormDelegate;

        public frmJobProgress(RunCodeMLAnalysis Analysis)
        {
            InitializeComponent();
            this.Analysis = Analysis;
            this.refreshGridDelegate = new RefreshDelegate(RefreshGrid);
            this.refreshLabelDelegate = new RefreshDelegate(RefreshProgressLabel);
            this.refreshRowDelegate = new RefreshRowDelegate(RefreshRow);
            this.refreshProgressFormDelegate = new RefreshProgressFormDelegate(RefreshProgressForm);

            SetButtonImage(btnClose_Progress, DialogButtonPresets.Cancel);
            SetButtonImage(btnClose_Configuration, DialogButtonPresets.Cancel);
            SetButtonImage(btnReRunJob, DialogButtonPresets.Run);

            if (!this.DesignMode)
            {
                tbForm.SelectedTab = pgProgress;
                tbForm_Selected(tbForm, new TabControlEventArgs(pgProgress, 0, TabControlAction.Selected));
            }
        }

        private void frmJobProgress_Load(object sender, EventArgs e)
        {
#if DOCUMENTATION
            this.Size = new System.Drawing.Size(806, 600);
#endif

            this.Text = this.Analysis.CurrentJob.Title;
            this.StartedAtText = lblStartedAt.Text;

            if (this.Analysis.CurrentJob.LastCompletedStep != ChangLab.Jobs.Job.RunStep.Complete)
            {
                SetButtonImage(btnNext, "Stop");
                btnNext.Text = "Cancel &Job";

                this.Analysis.ProgressUpdate += new ProgressUpdateEventHandler(Analysis_ProgressUpdate);
                this.Analysis.CodeMLJob.Process.ProcessOutputUpdate += new ProgressUpdateEventHandler(Process_ProcessOutputUpdate);
                this.Analysis.Worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Worker_RunWorkerCompleted);
            }
            else
            {
                // The job has finished.
                SetButtonImage(btnNext, "OK");
                btnNext.Text = "View &Results";
            }
            RefreshProgressLabel();

            // Custom context menu for the data grid so that we can have the "Cancel codeml.exe" option
            ContextMenuStrip contextMenu = new ContextMenuStrip();
            ToolStripMenuItem detailsMenuItem = new ToolStripMenuItem("&Details") { Tag = "Details" };
            contextMenu.Items.Add(detailsMenuItem);
            ToolStripMenuItem cancelMenuItem = new ToolStripMenuItem("&Cancel");
            cancelMenuItem.Click += new EventHandler(cancelMenuItem_Click);
            contextMenu.Items.Add(cancelMenuItem);

            // Load up the data grid
            this.DataGridHelper = new DataGridViewHelper(this, grdAnalyses, null, DataGridViewHelper.DataSourceTypes.Other, true, contextMenu);
            this.DataGridHelper.ViewDetails += new DataGridViewHelper.ViewDetailsEventHandler(DataGridHelper_ViewDetails);
            clmKappa.DefaultCellStyle.Format = DataGridViewHelper.DefaultDoubleFormatString;
            clmOmega.DefaultCellStyle.Format = DataGridViewHelper.DefaultDoubleFormatString;

            if (this.Analysis.CodeMLJob.Process.Analyses != null)
            {
                RefreshGrid();
            }
        }

        private void frmJobProgress_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Analysis.ProgressUpdate -= Analysis_ProgressUpdate;
            this.Analysis.CodeMLJob.Process.ProcessOutputUpdate -= Process_ProcessOutputUpdate;
            this.Analysis.Worker.RunWorkerCompleted -= Worker_RunWorkerCompleted;
        }

        private void tbForm_Selected(object sender, TabControlEventArgs e)
        {
            if (e.TabPage == pgProgress)
            {
                this.CancelButton = btnClose_Progress;
            }
            else if (e.TabPage == pgConfiguration)
            {
                this.CancelButton = btnClose_Configuration;
                if (!uctJobConfigurations1.DataSourceSet) { uctJobConfigurations1.Refresh(this.Analysis.Trees); }
            }
        }

        private void Analysis_ProgressUpdate(ProgressUpdateEventArgs e)
        {
            if (e.Source != null)
            {
                if (e.Source.GetType() == typeof(CodeMLAnalysisOption))
                {
                    this.Invoke(refreshRowDelegate, new object[] { e.Source });
                }
                else if (e.Source.GetType() == typeof(List<CodeMLAnalysisOption>))
                {
                    this.Invoke(refreshGridDelegate);
                }
            }

            this.Invoke(refreshLabelDelegate);
        }

        private void Process_ProcessOutputUpdate(ProgressUpdateEventArgs e)
        {
            if (e.Source != null)
            {
                if (e.Source.GetType() == typeof(CodeMLAnalysisOption))
                {
                    this.Invoke(refreshProgressFormDelegate, new object[] { e });
                }
            }
        }

        private void RefreshRow(CodeMLAnalysisOption Option)
        {
            DataGridViewRow match = grdAnalyses.Rows.Cast<DataGridViewRow>().FirstOrDefault(row => ((CodeMLAnalysisOptionRowDataItem)row.DataBoundItem).Option.ID == Option.ID);
            if (match != null)
            {
                ((CodeMLAnalysisOptionRowDataItem)match.DataBoundItem).Status = Option.Status.ToString();
                if (match.Selected) { match.Selected = false; match.Selected = true; }
                else { match.Selected = true; match.Selected = false; }
            }

            if (ProgressForm != null && ProgressForm.Visible)
            {
                switch (Option.Status)
                {
                    case ChangLab.Jobs.JobStatuses.Running:
                        ProgressForm.SetNeverEnding();
                        break;
                    default: // Pending is a status we won't see because it doesn't trigger a ProgressUpdate event.
                        ProgressForm.SetComplete();
                        break;
                }
            }
        }

        private void RefreshGrid()
        {
            this.DataGridHelper.Loaded = false;
            grdAnalyses.AutoGenerateColumns = false;
            grdAnalyses.DataSource = null;
            grdAnalyses.DataSource = new SortableBindingList<CodeMLAnalysisOptionRowDataItem>(this.Analysis.CodeMLJob.Process.Analyses.ToRowDataItemList());
            this.DataGridHelper.Loaded = true;
        }

        private void RefreshProgressForm(ProgressUpdateEventArgs e)
        {
            if (ProgressForm != null && ProgressForm.Visible)
            {
                ProgressForm.UpdateProgress(e);
            }
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            SetButtonImage(btnNext, "OK");
            btnNext.Text = "View &Results";
            btnNext.Enabled = true;
            RefreshProgressLabel();
        }

        private void DataGridHelper_ViewDetails(DataGridViewHelper.ViewDetailsEventArgs e)
        {
            CodeMLAnalysisOptionRowDataItem row = (CodeMLAnalysisOptionRowDataItem)e.Row.DataBoundItem;

            using (ProgressForm = new frmProgress(row.Option.ProcessDirectory, new ProgressForm.ProgressOptions() { PrintProgressMessages = true, UseNeverEndingTimer = (row.Option.Status == ChangLab.Jobs.JobStatuses.Running), IncludeElapsedTimeInProgressMessage = false, FormCloseButtonMode = DialogButtonPresets.Close }))
            {
                ProgressForm.UpdateProgress(new ProgressUpdateEventArgs() { ProgressMessage = row.Option.OutputData });
                ProgressForm.ShowDialog();
            }
        }

        private void cancelMenuItem_Click(object sender, EventArgs e)
        {
            CodeMLAnalysisOptionRowDataItem row = (CodeMLAnalysisOptionRowDataItem)grdAnalyses.SelectedRows[0].DataBoundItem;

            if (row.Option.Status == ChangLab.Jobs.JobStatuses.Running || row.Option.Status == ChangLab.Jobs.JobStatuses.Pending)
            {
                if (Utility.ShowMessage(this, "Are you sure you want to cancel this instance of codeml.exe?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    Analysis.CodeMLJob.Process.CancelProcess(row.Option.ID);
                }
            }
            else { Utility.ShowMessage(this, "This instance of codeml.exe cannot be cancelled because it is not running or pending.", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        }

        private void RefreshProgressLabel()
        {
            lblStartedAt.Text = string.Format(
                this.StartedAtText,
                this.Analysis.CurrentJob.StartTimeString,
                (this.Analysis.CurrentJob.EndTime != DateTime.MinValue ? this.Analysis.CurrentJob.EndTime : DateTime.Now)
                    .Subtract(this.Analysis.CurrentJob.StartTime)
                    .ToString("hh\\:mm\\:ss")
            );
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (this.Analysis.CurrentJob.LastCompletedStep == ChangLab.Jobs.Job.RunStep.Complete)
            {
                // The caller should open frmResults if frmJobProgress returns OK.
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            else
            {
                // Prompt to cancel
                if (Utility.ShowMessage(this, "Are you sure you want to cancel this job?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    // The job might finish while the user is deciding whether to click Yes or No, so we only want to call CancelAsync() if the job
                    // is still running.
                    if (this.Analysis.CurrentJob.LastCompletedStep != ChangLab.Jobs.Job.RunStep.Complete)
                    {
                        this.Analysis.CurrentJob.CancelAsync();
                        btnNext.Enabled = false;
                    }
                    else
                    {
                        Utility.ShowMessage(this, "Job has already completed.");
                    }
                }
            }
        }

        private void btnReRunJob_Click(object sender, EventArgs e)
        {
            if (Utility.ShowMessage(this.OwnerForm, "Create a new PAML job using this job's configuration?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                using (frmCreateJob frm = new frmCreateJob(this.Analysis.CodeMLJob.ID))
                {
                    if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                    {
                        Utility.ShowMessage(this.OwnerForm, "Job \"" + frm.Analysis.CodeMLJob.Title + "\" is running.");
                    }
                }
            }
        }
    }
}
