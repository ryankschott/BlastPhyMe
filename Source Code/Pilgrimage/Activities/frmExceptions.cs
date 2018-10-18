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

namespace Pilgrimage.Activities
{
    public partial class frmExceptions : DialogForm
    {
        public frmExceptions() : this(string.Empty, string.Empty, string.Empty) { }

        private frmExceptions(string Header, string Message, string Link)
        {
            InitializeComponent();

            this.SetButtonImage(btnSave, DialogButtonPresets.Save);
            this.SetButtonImage(btnCancel, DialogButtonPresets.Cancel);

            if (string.IsNullOrWhiteSpace(Header))
            { lblHeader.Parent.Controls.Remove(lblHeader); }
            else
            { lblHeader.Text = Header; }

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

            grdExceptions.AutoGenerateColumns = false;
            grdExceptions.DataSource = null;
        }

        public frmExceptions(string Message, string Link, List<Exception> Exceptions) : this(string.Empty, Message, Link)
        {
            Populate(Exceptions);
        }

        public frmExceptions(string Message, string Link, DataTable Exceptions) : this(string.Empty, Message, Link)
        {
            Populate(Exceptions);
        }

        private void Populate(List<Exception> Exceptions)
        {
            grdExceptions.DataSource = new SortableBindingList<ExceptionRowDataItem>(Exceptions.Select(ex => new ExceptionRowDataItem(ex)));
            grdExceptions.ClearSelection();
            HideExceptionsIfEmpty();
        }

        private void Populate(DataTable Exceptions)
        {
            grdExceptions.DataSource = new SortableBindingList<ExceptionRowDataItem>(Exceptions.Rows.Cast<DataRow>().Select(row => new ExceptionRowDataItem(row)));
            grdExceptions.ClearSelection();
            HideExceptionsIfEmpty();
        }

        private void HideExceptionsIfEmpty()
        {
            if (grdExceptions.Rows.Count == 0)
            {
                grdExceptions.Parent.Controls.Remove(grdExceptions);
                tblForm.RowStyles[2].SizeType = SizeType.AutoSize;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            System.IO.FileInfo file = null;
            if (IODialogHelper.SaveFile(IODialogHelper.DialogPresets.Text, "message.txt", this, ref file))
            {
                List<string> lines = txtMessage.Lines.ToList();
                if (grdExceptions.Rows.Count != 0)
                {
                    lines.Add("");
                    lines.Add("Error messages:"); 
                    lines.Add("");
                    lines.AddRange(((SortableBindingList<ExceptionRowDataItem>)grdExceptions.DataSource).Select(row => row.Message));
                }
                System.IO.File.WriteAllLines(file.FullName, lines.ToArray());
            }
        }

        private void lnkLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(lnkLink.Text);
        }
    }
}
