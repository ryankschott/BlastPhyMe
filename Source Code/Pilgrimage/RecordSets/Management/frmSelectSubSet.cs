using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChangLab.Common;
using ChangLab.RecordSets;

namespace Pilgrimage.RecordSets
{
    public partial class frmSelectSubSet : DialogForm
    {
        private DataTypes DataType { get; set; }
        private bool MoveGenes { get; set; }

        public string SelectedRecordSetID { get; private set; }

        private string _selectedSubSetID;
        public string SelectedSubSetID
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_selectedSubSetID))
                { return (string)cmbSubSets.SelectedValue; }
                else
                { return _selectedSubSetID; }
            }
        }

        public frmSelectSubSet(DataTypes DataType, SubSet CurrentSubSet, bool MoveGenes) : this (DataType, CurrentSubSet, (MoveGenes ? "Move" : "Copy")) { }

        public frmSelectSubSet(DataTypes DataType, SubSet CurrentSubSet, string ButtonType, bool AllowOtherRecordSet = true, bool IncludeCurrentSubSet = false)
        {
            InitializeComponent();
            this.DataType = DataType;
            this.MoveGenes = MoveGenes;

            List<SubSet> subSets = Program.Settings.AllSubSets(DataType)
                .Where(sub => IncludeCurrentSubSet || !GuidCompare.Equals(sub.ID, CurrentSubSet.ID)) // No sense in moving or copying to the same subset.
                .OrderBy(sub => sub.Name)
                .ToList();
            subSets.Insert(0, new SubSet(this.DataType) { Name = "--- New Dataset ---" });

            cmbSubSets.DisplayMember = "Name";
            cmbSubSets.ValueMember = "ID";
            cmbSubSets.DataSource = new BindingSource(subSets, null);

            SubSet selected = subSets.FirstOrDefault(sub => GuidCompare.Equals(sub.ID, Program.Settings.LastSubSetID));
            if (selected != null)
            { cmbSubSets.SelectedValue = selected.ID; }
            else
            { cmbSubSets.SelectedIndex = 0; }

            btnSave.Text = "&" + ButtonType;
            SetButtonImage(btnSave, ButtonType);

            if (AllowOtherRecordSet) { SetButtonImage(btnOtherRecordSet, "RecordSets"); }
            else { btnOtherRecordSet.Parent.Controls.Remove(btnOtherRecordSet); }
            SetButtonImage(btnCancel, "Cancel");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SelectedSubSetID))
            {
                using (RecordSets.frmEditSubSet frm = new RecordSets.frmEditSubSet(new SubSet(this.DataType)))
                {
                    if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        _selectedSubSetID = frm.CurrentSubSet.ID;
                    }
                    else { return; }
                }
            }

            Program.Settings.LastSubSetID = SelectedSubSetID;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void btnOtherRecordSet_Click(object sender, EventArgs e)
        {
            using (RecordSets.frmSelectSubSetInRecordSets frm = new frmSelectSubSetInRecordSets(this.DataType, this.MoveGenes))
            {
                if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    _selectedSubSetID = frm.SelectedSubSetID;
                    SelectedRecordSetID = frm.SelectedRecordSetID;

                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                }
            }
        }
    }
}
