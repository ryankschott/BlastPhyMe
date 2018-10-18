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
    public partial class frmManageSubSets : DialogForm
    {
        private DataTypes DataType { get; set; }

        public List<SubSet> Added { get; private set; }
        public List<SubSet> Renamed { get; private set; }
        public List<SubSet> Deleted { get; private set; }

        public frmManageSubSets(DataTypes DataType)
        {
            InitializeComponent();
            this.DataType = DataType;

            Added = new List<SubSet>();
            Renamed = new List<SubSet>();
            Deleted = new List<SubSet>();

            SetButtonImage(btnAdd, "New");
            SetButtonImage(btnRename, "Rename");
            SetButtonImage(btnDelete, "Delete");
            SetButtonImage(btnOpen, "Open");
            SetButtonImage(btnClose, "Cancel");
        }

        private void frmManageSubSets_Load(object sender, EventArgs e)
        {
            RefreshSubSets(string.Empty);
        }

        private void RefreshSubSets(string ReselectID)
        {
            lstSubSets.Items.Clear();

            List<SubSet> recordSets = Program.Settings.CurrentRecordSet.ListSubSets(this.DataType).OrderBy(sub => sub.Name).ToList();
            recordSets.ForEach(rs =>
            {
                lstSubSets.Items.Add(
                    new ListViewItem(new string[] { rs.Name, rs.GeneCount.ToSafeString() }) { Tag = rs }
                );
            });

            if (recordSets.Any(rs => GuidCompare.Equals(rs.ID, ReselectID)))
            {
                lstSubSets.Items[lstSubSets.Items.Cast<ListViewItem>().First(lv => GuidCompare.Equals(((SubSet)lv.Tag).ID, ReselectID)).Index].Selected = true;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (frmEditSubSet frm = new frmEditSubSet(new SubSet(this.DataType)))
            {
                if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (!Added.Contains(frm.CurrentSubSet)) { Added.Add(frm.CurrentSubSet); }
                    RefreshSubSets(frm.CurrentSubSet.ID);
                }
            }
        }

        private void btnRename_Click(object sender, EventArgs e)
        {
            if (lstSubSets.SelectedItems.Count == 0)
            {
                Utility.ShowMessage(this, Properties.Resources.Messages_NoSubSetSelected);
            }
            else
            {
                RenameSubSet(lstSubSets.SelectedItems[0]);
            }
        }

        private void RenameSubSet(ListViewItem SelectedItem)
        {
            using (frmEditSubSet frm = new frmEditSubSet((SubSet)SelectedItem.Tag))
            {
                if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (!Renamed.Contains(frm.CurrentSubSet)) { Renamed.Add(frm.CurrentSubSet); }
                    RefreshSubSets(frm.CurrentSubSet.ID);
                    if (Program.Settings.GetCurrentSubSet(this.DataType) != null && GuidCompare.Equals(Program.Settings.GetCurrentSubSet(this.DataType).ID, frm.CurrentSubSet.ID))
                    { Program.Settings.GetCurrentSubSet(this.DataType).Name = frm.CurrentSubSet.Name; }
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lstSubSets.SelectedItems.Count == 0)
            {
                Utility.ShowMessage(this, Properties.Resources.Messages_NoResultSetSelected);
            }
            else
            {
                SubSet SubSet = (SubSet)lstSubSets.SelectedItems[0].Tag;
                if (Utility.ShowMessage(this, "Are you sure you want to delete the \"" + SubSet.Name + "\" dataset?"
                                            + "\r\n\r\n" + "This cannot easily be undone.",
                                            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    SubSet.Toggle(null, false);
                    if (!Deleted.Contains(SubSet)) { Deleted.Add(SubSet); }
                    RefreshSubSets(string.Empty);
                }
            }
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (lstSubSets.SelectedItems.Count == 0)
            {
                Utility.ShowMessage(this, Properties.Resources.Messages_NoSubSetSelected);
            }
            else
            {
                switch (this.DataType)
                {
                    case DataTypes.GeneSequence:
                        Program.Settings.CurrentSubSet_GeneSequences = (SubSet)lstSubSets.SelectedItems[0].Tag;
                        Program.Settings.CurrentSubSet_GeneSequences.Toggle(true);
                        break;
                    case DataTypes.CodeMLResult:
                        Program.Settings.CurrentSubSet_CodeMLResults = (SubSet)lstSubSets.SelectedItems[0].Tag;
                        Program.Settings.CurrentSubSet_CodeMLResults.Toggle(true);
                        break;
                }

                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }

        private void lstSubSets_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewHitTestInfo test = lstSubSets.HitTest(e.X, e.Y);
            if (test != null && test.Item != null)
            {
                switch (this.DataType)
                {
                    case DataTypes.GeneSequence:
                        Program.Settings.CurrentSubSet_GeneSequences = (SubSet)test.Item.Tag;
                        Program.Settings.CurrentSubSet_GeneSequences.Toggle(true);
                        break;
                    case DataTypes.CodeMLResult:
                        Program.Settings.CurrentSubSet_CodeMLResults = (SubSet)test.Item.Tag;
                        Program.Settings.CurrentSubSet_CodeMLResults.Toggle(true);
                        break;
                }
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }
    }
}
