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
    public partial class frmProgress : ProgressForm
    {
        public override TextBox ProgressBox { get { return txtProgress; } }
        public override Label StatusLabel { get { return lblStatus; } }
        public override ProgressBar CurrentProgressBar { get { return pbCurrent; } }
        public override Label TotalProgressLabel { get { return lblTotal; } }
        public override ProgressBar TotalProgressBar { get { return pbTotal; } }
        public override Button CancellationButton { get { return btnCancel; } }

        public frmProgress(string Header, bool ProgressMessages, bool CurrentProgress, bool TotalProgress, bool NeverEndingTimer)
            : this(Header, new ProgressOptions() { 
                            PrintProgressMessages = ProgressMessages,
                            ShowCurrentProgress = CurrentProgress,
                            ShowTotalProgress = TotalProgress,
                            UseNeverEndingTimer = NeverEndingTimer }) { }

        public frmProgress(string Header, ProgressOptions Options)
        {
            InitializeComponent();

            this.Text = Header;
            this.Options = Options;

            SetButtonImage(btnCancel, Options.FormCloseButtonMode);
            if (Options.FormCloseButtonMode == DialogButtonPresets.Close)
            {
                btnCancel.Text = "&Close";
                this.CancelButton = btnCancel;
            }
            lblHeader.Parent.Controls.Remove(lblHeader);

            if (!Options.PrintProgressMessages) { txtProgress.Parent.Controls.Remove(txtProgress); }
            if (!Options.ShowCurrentProgress && !Options.UseNeverEndingTimer)
            {
                lblCurrent.Parent.Controls.Remove(lblCurrent);
                pbCurrent.Parent.Controls.Remove(pbCurrent);
            }
            if (!Options.AllowCancellation) { btnCancel.Parent.Controls.Remove(btnCancel); }
        }

        private void frmProgress_Load(object sender, EventArgs e)
        {

        }

        private void frmProgress_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            switch (this.Options.FormCloseButtonMode)
            {
                case DialogButtonPresets.Cancel:
                    SetCancelling();
                    OnCancelled(new RunWorkerCompletedEventArgs(null, null, true));
                    break;
                case DialogButtonPresets.Close:
                    this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                    break;
            }
        }
    }
}
