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
    public partial class frmManageRecordSets : DialogForm
    {
        public bool CurrentRecordSetDeleted { get; private set; }
        private string LastNewRecordSetID { get; set; }
        public bool IsNew { get; private set; }

        public frmManageRecordSets()
        {
            InitializeComponent();

            SetButtonImage(btnAdd, "New");
            SetButtonImage(btnRename, "Rename");
            SetButtonImage(btnDelete, "Delete");
            SetButtonImage(btnOpen, "Open_RecordSet");
            SetButtonImage(btnClose, "Cancel");

            this.CurrentRecordSetDeleted = false;
            this.LastNewRecordSetID = string.Empty;
            this.IsNew = false;
        }

        private void frmManageRecordSets_Load(object sender, EventArgs e)
        {
            RefreshRecordSets(string.Empty);
        }

        private void RefreshRecordSets(string ReselectID)
        {
            lstRecordSets.Items.Clear();

            List<RecordSet> recordSets = RecordSet.List(true);
            recordSets.ForEach(rs =>
            {
                lstRecordSets.Items.Add(
                    new ListViewItem(new string[] { rs.Name, rs.ModifiedAt.ToStandardDateTimeString() }) { Tag = rs }
                );
            }
            );

            if (recordSets.Any(rs => GuidCompare.Equals(rs.ID, ReselectID)))
            {
                lstRecordSets.Items[lstRecordSets.Items.Cast<ListViewItem>().First(lv => GuidCompare.Equals(((RecordSet)lv.Tag).ID, ReselectID)).Index].Selected = true;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (frmEditRecordSet frm = new frmEditRecordSet(new RecordSet()))
            {
                if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    this.LastNewRecordSetID = frm.CurrentRecordSet.ID;
                    RefreshRecordSets(frm.CurrentRecordSet.ID);
                }
            }
        }

        private void btnRename_Click(object sender, EventArgs e)
        {
            if (lstRecordSets.SelectedItems.Count == 0)
            {
                Utility.ShowMessage(this, Properties.Resources.Messages_NoResultSetSelected);
            }
            else
            {
                RenameRecordSet(lstRecordSets.SelectedItems[0]);
            }
        }

        private void RenameRecordSet(ListViewItem SelectedItem)
        {
            using (frmEditRecordSet frm = new frmEditRecordSet((RecordSet)SelectedItem.Tag))
            {
                if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    RefreshRecordSets(frm.CurrentRecordSet.ID);
                    if (Program.Settings.CurrentRecordSet != null && GuidCompare.Equals(Program.Settings.CurrentRecordSet.ID, frm.CurrentRecordSet.ID))
                    { Program.Settings.CurrentRecordSet.Name = frm.CurrentRecordSet.Name; }
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lstRecordSets.SelectedItems.Count == 0)
            {
                Utility.ShowMessage(this, Properties.Resources.Messages_NoResultSetSelected);
            }
            else
            {
                RecordSet recordSet = (RecordSet)lstRecordSets.SelectedItems[0].Tag;
                if (Utility.ShowMessage(this, "Are you sure you want to delete the \"" + recordSet.Name + "\" project?"
                                            + "\r\n\r\n" + "This cannot easily be undone.",
                                            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    recordSet.Delete();
                    if (Program.Settings.CurrentRecordSet != null && GuidCompare.Equals(Program.Settings.CurrentRecordSet.ID, recordSet.ID)) { CurrentRecordSetDeleted = true; }

                    RefreshRecordSets(string.Empty);
                }
            }
        }

        void export_ActivityCompleted(Activities.ActivityCompletedEventArgs e)
        {
            if (e.Error != null) { Utility.ShowErrorMessage(this, e.Error); }
            else if (e.Cancelled) { return; }
            else { Program.Settings.LastWorkingDirectory = (new System.IO.FileInfo((string)e.Result)).Directory.FullName; }
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (lstRecordSets.SelectedItems.Count == 0)
            {
                Utility.ShowMessage(this, Properties.Resources.Messages_NoResultSetSelected);
            }
            else
            {
                Program.Settings.CurrentRecordSet = (RecordSet)lstRecordSets.SelectedItems[0].Tag;
                this.IsNew = GuidCompare.Equals(Program.Settings.CurrentRecordSet.ID, this.LastNewRecordSetID);
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }

        private void lstRecordSets_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewHitTestInfo test = lstRecordSets.HitTest(e.X, e.Y);
            if (test != null && test.Item != null)
            {
                Program.Settings.CurrentRecordSet = (RecordSet)test.Item.Tag;
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }
    }
}
