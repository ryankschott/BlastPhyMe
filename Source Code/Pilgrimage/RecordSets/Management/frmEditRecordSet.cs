using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChangLab.RecordSets;

namespace Pilgrimage.RecordSets
{
    public partial class frmEditRecordSet : DialogForm
    {
        public RecordSet CurrentRecordSet { get; private set; }

        public frmEditRecordSet(RecordSet CurrentRecordSet)
        {
            InitializeComponent();
            this.CurrentRecordSet = CurrentRecordSet;

            if (string.IsNullOrWhiteSpace(CurrentRecordSet.ID))
            {
                this.Text = "Add Project";
                btnSave.Text = "&Create";
                SetButtonImage(btnSave, "Add_RecordSet");
            }
            else
            {
                this.Text = "Edit Project";
                txtName.Text = CurrentRecordSet.Name;
                btnSave.Text = "&Rename";
                btnSave.TextAlign = ContentAlignment.MiddleRight;
                SetButtonImage(btnSave, "Save");
            }
            SetButtonImage(btnCancel, "Cancel");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                Utility.ShowMessage(this, "Name cannot be empty.");
                return;
            }

            CurrentRecordSet.Name = txtName.Text;
            try
            {
                CurrentRecordSet.Save();
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            catch (Exception ex)
            {
                if (ex.Message.ToLower().Contains("duplicate key row"))
                {
                    Utility.ShowMessage(this, "A project with this name already exists.");
                    return;
                }
                else { throw ex; }
            }
        }
    }
}
