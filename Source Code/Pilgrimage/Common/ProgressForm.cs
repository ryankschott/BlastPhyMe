using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChangLab.Common;

namespace Pilgrimage
{
    public partial class ProgressForm : DialogForm
    {
        internal ProgressOptions Options { get; set; }
        protected DateTime OpenedAt { get; set; }
        protected Timer NeverEndingTimer { get; set; }
        public string Output { get { return ProgressBox.Text; } }

        public virtual TextBox ProgressBox { get { throw new NotImplementedException(); } }
        public virtual Label StatusLabel { get { return null; } }
        public virtual ProgressBar CurrentProgressBar { get { throw new NotImplementedException(); } }
        public virtual Label TotalProgressLabel { get { return null; } }
        public virtual ProgressBar TotalProgressBar { get { return null; } }
        public virtual Button CancellationButton { get { throw new NotImplementedException(); } }
        
        public ProgressForm()
        {
            InitializeComponent();
        }

        private void ProgressForm_Load(object sender, EventArgs e)
        {
            if (this.DesignMode) { return; }

            this.OpenedAt = DateTime.Now;

            if (Options.UseNeverEndingTimer)
            {
                this.NeverEndingTimer = new Timer();
                this.NeverEndingTimer.Tick += new EventHandler(NeverEndingTimer_Tick);
                
                CurrentProgressBar.Maximum = 1000;
                CurrentProgressBar.Minimum = 0;
            }

            if (NeverEndingTimer != null)
            {
                NeverEndingTimer.Start();
            }

            if (!Options.ShowTotalProgress && this.TotalProgressBar != null)
            {
                ////We don't need to show the "Current Progress:" label if there's only one progress bar
                //if (lblCurrent.Parent != null) { lblCurrent.Parent.Controls.Remove(lblCurrent); }
                this.TotalProgressLabel.Parent.Controls.Remove(this.TotalProgressLabel);
                this.TotalProgressBar.Parent.Controls.Remove(this.TotalProgressBar);
            }
        }

        private void ProgressForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (NeverEndingTimer != null)
            {
                NeverEndingTimer.Stop();
                NeverEndingTimer.Tick -= NeverEndingTimer_Tick;
                NeverEndingTimer.Dispose();
                NeverEndingTimer = null;
            }
        }

        public void UpdateProgress(ProgressUpdateEventArgs args)
        {
            if (args.Setup && (NeverEndingTimer == null))
            {
                if (args.CurrentChanged) { CurrentProgressBar.Maximum = args.CurrentMax; }
                if (args.TotalChanged && TotalProgressBar != null) { TotalProgressBar.Maximum = args.TotalMax; }
            }

            if (!string.IsNullOrWhiteSpace(args.ProgressMessage))
            {
                if (Options.IncludeElapsedTimeInProgressMessage)
                {
                    TimeSpan elapsed = DateTime.Now.Subtract(this.OpenedAt);
                    ProgressBox.Text += elapsed.ElapsedTimeStamp() + ": ";
                }

                ProgressBox.Text += args.ProgressMessage + "\r\n";
                ProgressBox.ScrollToEnd(false);
            }
            if (StatusLabel != null)
            {
                if (!string.IsNullOrWhiteSpace(args.StatusMessage))
                {
                    StatusLabel.Text = args.StatusMessage;
                    StatusLabel.Visible = true;
                }
                else
                {
                    StatusLabel.Text = string.Empty;
                    StatusLabel.Visible = false;
                }
            }

            if (NeverEndingTimer == null)
            {
                if (args.CurrentChanged) { CurrentProgressBar.Value = args.CurrentProgress; }
                if (args.TotalChanged && TotalProgressBar != null) { TotalProgressBar.Value = args.TotalProgress; }
            }
        }

        public void PerformStep()
        {
            if (CurrentProgressBar.Parent != null)
            {
                if (CurrentProgressBar.Value == CurrentProgressBar.Maximum)
                { CurrentProgressBar.Value = CurrentProgressBar.Minimum; }
                else
                { CurrentProgressBar.PerformStep(); }
            }
        }

        private void NeverEndingTimer_Tick(object sender, EventArgs e)
        {
            PerformStep();
        }

        internal void SetCancelling()
        {
            CancellationButton.Text = "Cancelling...";
            CancellationButton.Width = 90;
            CancellationButton.TextAlign = ContentAlignment.MiddleRight;
            CancellationButton.Enabled = false;
        }

        internal void SetNeverEnding()
        {
            if (this.NeverEndingTimer == null)
            {
                this.NeverEndingTimer = new Timer();
                this.NeverEndingTimer.Tick += new EventHandler(NeverEndingTimer_Tick);

                CurrentProgressBar.Maximum = 1000;
                CurrentProgressBar.Minimum = 0;
            }

            this.NeverEndingTimer.Start();
        }

        internal void SetComplete()
        {
            if (this.NeverEndingTimer != null && this.NeverEndingTimer.Enabled) { this.NeverEndingTimer.Stop(); }
            if (this.Options.ShowCurrentProgress && this.CurrentProgressBar != null) { this.CurrentProgressBar.Value = this.CurrentProgressBar.Maximum; }
            if (this.Options.ShowTotalProgress && this.TotalProgressBar != null) { this.TotalProgressBar.Value = this.TotalProgressBar.Maximum; }
        }

        #region Events
        protected virtual void OnUserClosed(EventArgs e)
        {
            if (UserClosed != null)
            {
                UserClosed(e);
            }
        }
        public delegate void UserClosedEventHandler(EventArgs e);
        public event UserClosedEventHandler UserClosed;

        protected virtual void OnCancelled(RunWorkerCompletedEventArgs e)
        {
            if (Cancelled != null)
            {
                Cancelled(e);
            }
        }
        public delegate void CancelledEventHandler(RunWorkerCompletedEventArgs e);
        public event CancelledEventHandler Cancelled;

        public class ProgressFormEventArgs : EventArgs
        {
            internal ProgressForm Form { get; set; }
            internal Activities.Activity Activity { get; set; }
            internal bool Cancel { get; set; }
        }
        #endregion

        public class ProgressOptions
        {
            /// <summary>
            /// Default is false.
            /// </summary>
            public bool PrintProgressMessages { get; set; }

            /// <summary>
            /// Default is true.
            /// </summary>
            public bool IncludeElapsedTimeInProgressMessage { get; set; }

            /// <summary>
            /// Default is true.
            /// </summary>
            public bool ShowCurrentProgress { get; set; }

            /// <summary>
            /// Default is false.
            /// </summary>
            public bool ShowTotalProgress { get; set; }

            /// <summary>
            /// Default is false.
            /// </summary>
            public bool UseNeverEndingTimer { get; set; }

            /// <summary>
            /// Default is true.
            /// </summary>
            public bool AllowCancellation { get; set; }

            /// <summary>
            /// Default is Cancel.
            /// </summary>
            public DialogButtonPresets FormCloseButtonMode { get; set; }

            public ProgressOptions()
            {
                PrintProgressMessages = false;
                ShowTotalProgress = false;
                UseNeverEndingTimer = false;
                IncludeElapsedTimeInProgressMessage = true;
                ShowCurrentProgress = true;
                AllowCancellation = true;
                FormCloseButtonMode = DialogButtonPresets.Cancel;
            }
        }
    }
}
