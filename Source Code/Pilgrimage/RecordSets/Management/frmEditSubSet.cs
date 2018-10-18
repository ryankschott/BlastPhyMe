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
    public partial class frmEditSubSet : DialogForm
    {
        public SubSet CurrentSubSet { get; private set; }

        public frmEditSubSet(SubSet CurrentSubSet)
        {
            InitializeComponent();
            this.CurrentSubSet = CurrentSubSet;

            if (string.IsNullOrWhiteSpace(CurrentSubSet.ID))
            {
                this.Text = "Add Dataset";
                btnSave.Text = "&Create";
                SetButtonImage(btnSave, "Add");
            }
            else
            {
                this.Text = "Edit Dataset";
                txtName.Text = CurrentSubSet.Name;
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

            CurrentSubSet.Name = txtName.Text;
            try
            {
                CurrentSubSet.DisplayIndex = Program.Settings.CurrentRecordSet.MaxSubSetDisplayIndex(this.CurrentSubSet.DataType.Key) + 1;
                CurrentSubSet.Save(Program.Settings.CurrentRecordSet.ID);
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            catch (Exception ex)
            {
                if (ex.Message.ToLower().Contains("duplicate key row"))
                {
                    Utility.ShowMessage(this, "A dataset with this name already exists.");
                    return;
                }
                else { throw ex; }
            }
        }
    }
}
