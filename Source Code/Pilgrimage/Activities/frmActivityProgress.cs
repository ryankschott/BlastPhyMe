using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChangLab.Common;

namespace Pilgrimage.Activities
{
    public partial class frmActivityProgress : ProgressForm
    {
        public Activities.Activity Activity { get; set; }
        public override TextBox ProgressBox { get { return txtProgress; } }
        public override Label StatusLabel { get { return lblStatus; } }
        public override ProgressBar CurrentProgressBar { get { return pbCurrent; } }
        public override Label TotalProgressLabel { get { return lblTotalProgress; } }
        public override ProgressBar TotalProgressBar { get { return pbTotal; } }
        public override Button CancellationButton { get { return btnCancel; } }

        public delegate void UpdateProgressDelegate(ProgressUpdateEventArgs args);
        public UpdateProgressDelegate updateProgressDelegate;

        public frmActivityProgress() : this(new ProgressOptions()) { }

        public frmActivityProgress(ProgressOptions Options) : base()
        {
            InitializeComponent();
            this.updateProgressDelegate = new UpdateProgressDelegate(UpdateProgress);
            this.Options = Options;

            SetButtonImage(btnCancel, "Stop");
            SetButtonImage(btnClose, DialogButtonPresets.Close);
        }

        private void frmJobProgress_Load(object sender, EventArgs e)
        {
            ProgressBox.Lines = this.Activity.CurrentJob.ProgressMessages.Select(msg => msg.Elapsed.ElapsedTimeStamp() + ": " + msg.Message).ToArray();
            ProgressBox.Text += "\r\n";
            ProgressBox.ScrollToEnd(false);

            CurrentProgressBar.Maximum = this.Activity.CurrentJob.CurrentProgressMaximum;
            CurrentProgressBar.Value = this.Activity.CurrentJob.CurrentProgressValue;
            StatusLabel.Text = this.Activity.CurrentJob.LastStatusMessage;

            if (this.Activity.CurrentJob.LastCompletedStep != ChangLab.Jobs.Job.RunStep.Complete)
            {
                this.Activity.ProgressUpdate += new ProgressUpdateEventHandler(Alignment_ProgressUpdate);
                this.Activity.Worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Worker_RunWorkerCompleted);
            }
            else
            {
                SetButtonImage(btnCancel, "OK");
                btnCancel.Text = "View &Results";
            }
        }

        private void Alignment_ProgressUpdate(ProgressUpdateEventArgs e)
        {
            // To get around any cross-thread issues with the underlying BackgroundWorker
            this.Invoke(updateProgressDelegate, new object[] { e });
        }

        private void frmJobProgress_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Activity.ProgressUpdate -= Alignment_ProgressUpdate;
            this.Activity.Worker.RunWorkerCompleted -= Worker_RunWorkerCompleted;
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!this.Activity.Cancelled)
            {
                SetButtonImage(btnCancel, DialogButtonPresets.OK);
                btnCancel.Text = "View &Results";
                btnCancel.Enabled = true;

                if (this.Activity.CurrentJob.Status == ChangLab.Jobs.JobStatuses.Failed)
                {
                    Utility.ShowMessage(this.OwnerForm, "The job ended in error.  Select \"View Results\" to view the details of the error" + (this.Activity.CurrentJob.Exceptions.Count > 1 ? "s" : "") + " that occured.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    SetFocusOnControl(btnCancel);
                }
            }
            else { this.DialogResult = System.Windows.Forms.DialogResult.Cancel; }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (this.Activity.CurrentJob.LastCompletedStep == ChangLab.Jobs.Job.RunStep.Complete)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            else
            {
                if (Utility.ShowMessage(this.OwnerForm, "Are you sure you want to cancel the job?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    // The job might finish while the user is deciding whether to click Yes or No, so we only want to call CancelAsync() if the job
                    // is still running.
                    if (this.Activity.CurrentJob.LastCompletedStep != ChangLab.Jobs.Job.RunStep.Complete)
                    {
                        this.Activity.CurrentJob.CancelAsync();
                        this.Activity.Worker.CancelAsync();
                        SetCancelling();
                    }
                    else
                    {
                        Utility.ShowMessage(this, "The job has already completed.");
                    }
                }   
            }
        }
    }
}
