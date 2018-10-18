using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChangLab.Common;
using ChangLab.RecordSets;

namespace Pilgrimage.UserControls
{
    public partial class uctSubSetsManager : UserControl
    {
        // These must be overridden.
        protected internal virtual ChangLab.RecordSets.DataTypes DataType { get { throw new NotImplementedException(); } }
        protected internal virtual DraggableTabControl.DraggableTabControl SubSetViewsControl { get { throw new NotImplementedException(); } }
        protected internal virtual ToolStripContainer FormToolStripContainer { get { throw new NotImplementedException(); } }
        protected internal virtual List<ToolStripItem> tsActions { get; set; }

        protected internal ChangLab.RecordSets.RecordSet CurrentRecordSet { get { return Program.Settings.CurrentRecordSet; } }
        internal SubSet CurrentSubSet
        {
            get { return Program.Settings.GetCurrentSubSet(this.DataType); }
            set { Program.Settings.SetCurrentSubSet(this.DataType, value); }
        }
        internal SubSet LastOpenedSubSet
        {
            get
            {
                return AllSubSets.Where(sub => sub.Open).OrderByDescending(sub => sub.LastOpenedAt).First();
            }
        }
        internal List<SubSet> AllSubSets
        {
            get
            {
                return (Program.Settings != null ? Program.Settings.AllSubSets(this.DataType).Where(sub => sub.DataType.Key == this.DataType).ToList() : null);
            }
        }
        
        protected internal TabControl.TabPageCollection TabPages
        {
            get { return this.SubSetViewsControl.TabPages; }
        }

        protected internal bool LoadingSubSetPages { get; set; }

        protected internal virtual uctSubSetView SelectedSubSetView
        {
            get
            {
                return (uctSubSetView)SubSetViewsControl.SelectedTab.Controls[0];
            }
        }
        protected internal virtual uctSubSetView SubSetRecords(string SubSetID)
        {
            TabPage page = SubSetViewsControl.TabPages.Cast<TabPage>().FirstOrDefault(pg => GuidCompare.Equals(((SubSet)pg.Tag).ID, SubSetID));
            if (page != null)
            { return (uctSubSetView)page.Controls[0]; }
            else
            { return null; }
        }

        public uctSubSetsManager()
        {
            InitializeComponent();
        }

        protected internal virtual void RestoreUserLayout()
        {
            FormToolStripContainer.TopToolStripPanel.SuspendLayout();

            List<ToolStrip> toolStrips = new List<ToolStrip>(FormToolStripContainer.TopToolStripPanel.Controls.Cast<ToolStrip>());

            foreach (ToolStrip ts in toolStrips)
            {
                ts.Parent.Controls.Remove(ts);
            }

            //FormToolStripContainer.TopToolStripPanel.ResumeLayout();
            //FormToolStripContainer.TopToolStripPanel.SuspendLayout();

            foreach (ToolStrip ts in toolStrips)
            {
                FormToolStripContainer.TopToolStripPanel.Controls.Add(ts);
                ts.Location = Program.DatabaseSettings.GetToolStripLocation(this.DataType, ts);
            }

            FormToolStripContainer.TopToolStripPanel.ResumeLayout();
        }

        protected internal virtual void ToolStrip_EndDrag(object sender, EventArgs e)
        {
            if (Program.DatabaseSettings != null)
            {
                Program.DatabaseSettings.SetToolStripLocation(this.DataType, (ToolStrip)sender);
            }
        }
        
        protected internal void SubSetsViewControl_Reordered(object sender, TabControlEventArgs e)
        {
            // Save the new index order for the open subsets.
            CurrentRecordSet.ReorderSubSets(SubSetViewsControl.TabPages
                                                        .Cast<TabPage>()
                                                        .Select((tp, index) => new { TabPage = tp, Index = index })
                                                        .ToDictionary(tp => (SubSet)tp.TabPage.Tag, tp => tp.Index));
        }

        internal void RefreshSubSets(string SelectedSubSetID = "", string SelectedSubSetName = "")
        {
            RefreshSubSetsFromDatabase();
            Program.DebugStartupTime("SubSets refreshed from database");

            LoadingSubSetPages = true;
            SubSetViewsControl.TabPages.Clear();
            foreach (SubSet set in AllSubSets.Where(sub => sub.Open).OrderBy(sub => sub.DisplayIndex))
            {
                AddSubSet(set);
            }
            LoadingSubSetPages = false;

            if (AllSubSets.Any(sub => sub.DisplayIndex == 0))
            {
                // Backwards compatibility for recordsets created before DisplayIndex was introduced.
                SubSetsViewControl_Reordered(SubSetViewsControl, null);
            }

            ReselectSubSet(SelectedSubSetID, SelectedSubSetName);
        }

        internal void RefreshSubSetsFromDatabase()
        {
            this.CurrentRecordSet.ListSubSets(this.DataType);
        }

        protected internal virtual void AddSubSet(SubSet Set, bool UpdateOpened = false) { throw new NotImplementedException(); }

        internal void ReselectSubSet(string SelectedSubSetID = "", string SelectedSubSetName = "")
        {
            if (string.IsNullOrWhiteSpace(SelectedSubSetID) && string.IsNullOrWhiteSpace(SelectedSubSetName))
            {
                if (Program.Settings.GetCurrentSubSet(this.DataType) != null)
                {
                    SelectedSubSetID = Program.Settings.GetCurrentSubSet(this.DataType).ID;
                    if (!this.CurrentRecordSet.SubSets[this.DataType].Any(sub => GuidCompare.Equals(sub.ID, SelectedSubSetID)))
                    {
                        // This will happen if a recordset (and subset) are open, and then a new recordset is created and opened.  The CurrentSubSet
                        // property will be set to the subset from the previously opened recordset, and so here we're acknowledging that the property
                        // is set to a subset that isn't in the newly opened recordset, and thus we're switching to use the most recently opened
                        // subset in the newly opened recordset.  For a newly created recordset, the last opened subset is configured in the
                        // RecordSet.RecordSet_Edit stored procedure such that 'All' is set to be the last opened by default.
                        SelectedSubSetID = this.LastOpenedSubSet.ID;
                    }
                }
                else if (AllSubSets.Where(sub => sub.Open).Count() != 0)
                {
                    SelectedSubSetID = this.LastOpenedSubSet.ID;
                }
            }

            SubSetViewsControl.SelectedTab = null;
            if (SubSetViewsControl.TabPages.Count != 0)
            {
                if (!string.IsNullOrWhiteSpace(SelectedSubSetID))
                {
                    SubSetViewsControl.SelectedTab = SubSetViewsControl.TabPages.Cast<TabPage>().FirstOrDefault(pg => GuidCompare.Equals(((SubSet)pg.Tag).ID, SelectedSubSetID));
                }
                else if (!string.IsNullOrWhiteSpace(SelectedSubSetName))
                {
                    SubSetViewsControl.SelectedTab = SubSetViewsControl.TabPages.Cast<TabPage>().FirstOrDefault(pg => ((SubSet)pg.Tag).Name.ToLower() == SelectedSubSetName.ToLower());
                }

                if (SubSetViewsControl.SelectedTab == null) { SubSetViewsControl.SelectedTab = SubSetViewsControl.TabPages[SubSetViewsControl.TabPages.Count - 1]; }
            }
            else { ToggleSubSetToolbarFunctions(); }
        }

        internal void ShowAndRefreshSubSet(string SelectedSubSetID, int? GeneCount)
        {
            if (!AllSubSets.Any(sub => GuidCompare.Equals(sub.ID, SelectedSubSetID)))
            {
                // The user created a new subset to store the records in.
                RefreshSubSetsFromDatabase();
                AddSubSet(AllSubSets.ElementByID(SelectedSubSetID), true);
            }
            else
            {
                if (SubSetRecords(SelectedSubSetID) != null)
                { SubSetRecords(SelectedSubSetID).RefreshRecords(); }
                else
                { AddSubSet(AllSubSets.ElementByID(SelectedSubSetID), true); }
            }
            ReselectSubSet(SelectedSubSetID);
            SubSetsViewControl_Reordered(SubSetViewsControl, null);
            this.CurrentRecordSet.Save(); // Update the ModifiedAt value.

            if (GeneCount != null)
            {
                Utility.ShowMessage(this, GeneCount + " records have been added to \"" + AllSubSets.ElementByID(SelectedSubSetID).Name + "\".");
            }
        }

        internal void RefreshSubSet(string SubSetID)
        {
            if (SubSetRecords(SubSetID) != null)
            {
                if (GuidCompare.Equals(SubSetID, CurrentSubSet.ID))
                {
                    // It is assumed that whatever edits were made to the subset record(s) would be fully effective in memory and thus we don't need
                    // to go back to the database for a refresh.  This opens up the potential for lack of concurrency between the database and the UI
                    // if the editing form does not fully update the in-memory copy of the record after committing its changes to the database.
                    SubSetRecords(SubSetID).RefreshRecords(false, true);
                    // The advantage is that the user's sorting and scrolling are not effected.
                }
                else
                {
                    // Whereas any subsets not currently visible need to go through a full database refresh considering that even if they contain the
                    // same record as the one being edited (based on database ID), it won't be the same in-memory copy and won't have been updated.
                    SubSetRecords(SubSetID).RefreshRecords();
                }
            }

            this.CurrentRecordSet.Save(); // Update the ModifiedAt value.
        }

        internal void ClearSubSets()
        {
            SubSetViewsControl.TabPages.Clear();
        }

        #region Toolbar Functions
        protected internal virtual void ToggleSubSetToolbarFunctions()
        {
            bool hasTabPages = (SubSetViewsControl.TabPages.Count != 0);
            tsActions.ForEach(tsb => tsb.Enabled = hasTabPages);
            SubSetViewsControl.Enabled = hasTabPages;
        }

        protected internal virtual void NewSubset_Click(object sender, EventArgs e)
        {
            using (RecordSets.frmEditSubSet frm = new RecordSets.frmEditSubSet(new SubSet(this.DataType)))
            {
                if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    AddSubSet(frm.CurrentSubSet);
                    ReselectSubSet(frm.CurrentSubSet.ID);
                    RefreshSubSetsFromDatabase();
                    ToggleSubSetToolbarFunctions();
                }
            }
        }

        protected internal virtual void OpenSubSet_Click(object sender, EventArgs e)
        {
            using (RecordSets.frmManageSubSets frm = new RecordSets.frmManageSubSets(this.DataType))
            {
                DialogResult result = frm.ShowDialog(this);
                if (frm.Added.Count != 0 || frm.Renamed.Count != 0 || frm.Deleted.Count != 0)
                {
                    RefreshSubSetsFromDatabase();
                }

                if (frm.Added.Count != 0)
                {
                    LoadingSubSetPages = true;
                    frm.Added.ForEach(sub => {
                        AddSubSet(sub);
                    });
                    LoadingSubSetPages = false;
                }
                if (frm.Renamed.Count != 0)
                {
                    frm.Renamed.ForEach(sub =>
                    {
                        TabPage page = SubSetViewsControl.TabPages.Cast<TabPage>().FirstOrDefault(pg => GuidCompare.Equals(sub.ID, ((SubSet)pg.Tag).ID));
                        if (page != null)
                        {
                            page.Text = sub.Name;
                        }
                    }
                    );
                }
                if (frm.Deleted.Count != 0)
                {
                    LoadingSubSetPages = true;
                    frm.Deleted.ForEach(sub =>
                    {
                        TabPage page = SubSetViewsControl.TabPages.Cast<TabPage>().FirstOrDefault(pg => GuidCompare.Equals(sub.ID, ((SubSet)pg.Tag).ID));
                        if (page != null)
                        {
                            SubSetViewsControl.TabPages.Remove(page);
                        }
                    }
                    );
                    LoadingSubSetPages = false;
                }
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    TabPage page = SubSetViewsControl.TabPages.Cast<TabPage>().FirstOrDefault(pg => GuidCompare.Equals(CurrentSubSet.ID, ((SubSet)pg.Tag).ID));
                    if (page == null)
                    {
                        LoadingSubSetPages = true;
                        AddSubSet(CurrentSubSet);
                        LoadingSubSetPages = false;
                    }
                }

                SubSetsViewControl_Reordered(SubSetViewsControl, null);
                ToggleSubSetToolbarFunctions();
                if (SubSetViewsControl.TabPages.Count != 0) { ReselectSubSet(); }
            }
        }

        protected internal virtual void CloseSubSet_Click(object sender, EventArgs e)
        {
            CurrentSubSet.Toggle(false);
            RefreshSubSetsFromDatabase();

            LoadingSubSetPages = true;
            SubSetViewsControl.TabPages.Remove(SubSetViewsControl.SelectedTab);
            LoadingSubSetPages = false;

            SubSetViewsControl.SelectedTab = null; // This will have set CurrentSubSet = null
            ReselectSubSet(string.Empty);
        }

        protected internal virtual void ImportFromPilgrimageDataFile_Click(object sender, EventArgs e)
        {
            System.IO.FileInfo file = null;
            if (IODialogHelper.OpenFile(IODialogHelper.DialogPresets.PilgrimageDataFile, this, ref file))
            {
                using (RecordSets.frmSelectSubSet frm = new RecordSets.frmSelectSubSet(this.DataType, this.CurrentSubSet, "Import", false, true))
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        Activities.ImportFromDataFile Importer = new Activities.ImportFromDataFile(this);
                        Importer.ActivityCompleted += new Activities.Activity.ActivityCompletedEventHandler(ImportFromPilgrimageDataFile_ActivityCompleted);
                        Importer.Import(file.FullName, string.Empty, this.CurrentRecordSet.ID, frm.SelectedSubSetID);
                    }
                }
            }
        }

        private void ImportFromPilgrimageDataFile_ActivityCompleted(Activities.ActivityCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                using (frmMessageBox msg = new frmMessageBox("Import Failed", ChangLab.Jobs.Job.ImportDataFile_ListProgressMessages(e.Sender.CurrentJob.ID).Concatenate("\r\n"), MessageBoxButtons.OK, MessageBoxIcon.Warning, true))
                {
                    msg.ShowDialog();
                }
            }
            else if (e.Cancelled) { return; }
            else
            {
                ShowAndRefreshSubSet(((Activities.ImportFromDataFile)e.Sender).CurrentSubSetID, null);
            }
        }
        #endregion
    }
}
