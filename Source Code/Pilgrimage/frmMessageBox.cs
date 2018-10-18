using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace Pilgrimage
{
    public partial class frmMessageBox : DialogForm
    {
        public frmMessageBox() 
            : this(string.Empty, string.Empty, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information) { }

        public frmMessageBox(string Header, string Message, string Link = "") 
            : this(Header, Message, Link, MessageBoxButtons.OK, MessageBoxIcon.Information) { }

        public frmMessageBox(string Header, string Message, MessageBoxButtons Buttons, MessageBoxIcon Icon, bool ShowSaveButton = false, bool EnableWordWrap = false)
            : this(Header, Message, "", Buttons, Icon, ShowSaveButton, EnableWordWrap) { }

        public frmMessageBox(string Header, string Message, string Link, MessageBoxButtons Buttons, MessageBoxIcon Icon, bool ShowSaveButton = false, bool EnableWordWrap = false)
        {
            InitializeComponent();
            this.Text = Program.ProductName;

            if (string.IsNullOrWhiteSpace(Header))
            {
                tblForm.Controls.Remove(lblHeader);
                tblForm.SetRow(txtMessage, 0);
                tblForm.SetRowSpan(txtMessage, 2);
            }
            else
            {
                lblHeader.Text = Header;
            }

            txtMessage.Text = Message;
            txtMessage.Select(0, 0);

            if (string.IsNullOrWhiteSpace(Link))
            {
                lnkLink.Parent.Controls.Remove(lnkLink);
            }
            else
            {
                lnkLink.Text = Link;
            }

            switch (Buttons)
            {
                case MessageBoxButtons.OK:
                    btnCancel.Visible = false;
                    this.CancelButton = btnOK;
                    SetButtonImage(btnOK, "OK");
                    btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
                    break;
                case MessageBoxButtons.YesNo:
                    btnOK.Text = "&Yes";
                    btnOK.DialogResult = System.Windows.Forms.DialogResult.Yes;
                    btnCancel.Text = "&No";
                    btnCancel.DialogResult = System.Windows.Forms.DialogResult.No;
                    break;
            }

            switch (Icon)
            {
                case MessageBoxIcon.Warning:
                    pbIcon.Image = imgIcons.Images["warning"];
                    break;
                case MessageBoxIcon.Error:
                    pbIcon.Image = imgIcons.Images["error"];
                    break;
                case MessageBoxIcon.None:
                    pbIcon.Parent.Controls.Remove(pbIcon);
                    break;
                default:
                    pbIcon.Image = imgIcons.Images["info"];
                    break;
            }

            btnSave.Visible = ShowSaveButton;
            SetButtonImage(btnSave, "Save");
            txtMessage.WordWrap = EnableWordWrap;

            this.FocusOnLoad = btnOK;
        }

        private void lnkLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(lnkLink.Text);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            System.IO.FileInfo file = null;
            if (IODialogHelper.SaveFile(IODialogHelper.DialogPresets.Text, "message.txt", this, ref file))
            {
                System.IO.File.WriteAllLines(file.FullName, txtMessage.Lines);
            }
        }
    }
}
