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
    public partial class frmSelectSubSetInRecordSets : DialogForm
    {
        public string SelectedRecordSetID { get; private set; }
        public string SelectedSubSetID { get; private set; }

        public frmSelectSubSetInRecordSets(DataTypes DataType, bool Move)
        {
            InitializeComponent();

            foreach (RecordSet rs in RecordSet.List(true).OrderBy(rs => rs.Name))
            {
                TreeNode rsNode = new TreeNode(rs.Name) { ImageKey = "RecordSet", SelectedImageKey = "RecordSet", Tag = rs.ID };
                tvSubSets.Nodes.Add(rsNode);

                foreach (SubSet sub in rs.ListSubSets(DataType).OrderBy(sub => sub.Name))
                {
                    rsNode.Nodes.Add(new TreeNode(sub.Name) { ImageKey = "SubSet", SelectedImageKey = "SubSet", Tag = sub.ID });
                }
            }
            tvSubSets.ExpandAll();

            if (Move)
            {
                btnSave.Text = "&Move";
                SetButtonImage(btnSave, "Move");
            }
            else
            {
                btnSave.Text = "&Copy";
                SetButtonImage(btnSave, "Copy");
            }
            SetButtonImage(btnCancel, "Cancel");

            this.FocusOnLoad = tvSubSets;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (tvSubSets.SelectedNode == null || tvSubSets.SelectedNode.Level == 0)
            {
                Utility.ShowMessage(this, "Please select a dataset.");
            }
            else
            {
                this.SelectedRecordSetID = (string)tvSubSets.SelectedNode.Parent.Tag;
                this.SelectedSubSetID = (string)tvSubSets.SelectedNode.Tag;

                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }
    }
}
