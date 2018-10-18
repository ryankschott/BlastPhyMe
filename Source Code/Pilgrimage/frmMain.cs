using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using ChangLab.Common;
using ChangLab.Genes;
using ChangLab.RecordSets;
using Pilgrimage.Activities;

namespace Pilgrimage
{
    public partial class frmMain : Form
    {
        internal RecordSet CurrentRecordSet
        {
            get
            {
                return (Program.Settings != null ? Program.Settings.CurrentRecordSet : null);
            }
            set
            {
                Program.Settings.CurrentRecordSet = value;
            }
        }

        private DataTypes CurrentModule { get { return (DataTypes)tbForm.SelectedTab.Tag; } }

        public frmMain()
        {
            InitializeComponent();
            this.aboutPilgrimageToolStripMenuItem.Text = "&About " + Program.ProductName;

            pgGenes.Tag = DataTypes.GeneSequence;
            pgTreeAnalysis.Tag = DataTypes.CodeMLResult;

            this.subsetsToolStripMenuItem.Click += new EventHandler(subsetsToolStripMenuItem_Click);

            this.Icon = Properties.Resources.DefaultIcon;
            RefreshTitleBar();

            newRecordsetToolStripMenuItem.Enabled = false;
            openRecordsetToolStripMenuItem.Enabled = false;
            recordsetsToolStripMenuItem.Enabled = false;
#if !DEBUG
            toolsToolStripMenuItem.Visible = false;
#endif
        }

        #region Form Events
        private void frmMain_Load(object sender, EventArgs e)
        {
            Program.Settings = new AppSettings(); Program.Settings.Load();
            Program.DatabaseSettings = new DatabaseSettings();
            Program.RecordSetSettings = new RecordSetSettings();
            Program.InProgressActivities.ActivityAdded += new InProgress.ActivityAddedEventHandler(InProgressActivities_ActivityAdded);
            Program.InProgressActivities.ActivityRemoved += new InProgress.ActivityRemovedEventHandler(InProgressActivities_ActivityRemoved);

#if DEBUG
            tsbResolution.Text = this.Size.ToString();
#endif
#if DOCUMENTATION
            this.WindowState = FormWindowState.Normal;
            this.Size = new System.Drawing.Size(806, 600);
#endif

#if !EEB460 || DEBUG
            if (Program.Settings.RecentDatabases.Count != 0
                && System.IO.File.Exists(Program.Settings.RecentDatabases.First().FullName))
            {
                try
                {
                    using (frmConnectToDatabase frm = new frmConnectToDatabase(Program.Settings.RecentDatabases.First().FullName, this))
                    {
                        // This will trap an upgrade or incompatibility prompt.  If the most recent database needs auto-upgrading or is at a higher 
                        // version, we don't automatically prompt because the user might not know which database the app is trying to open.
                        frm.AutoUpdatePrompt += new frmConnectToDatabase.ActivityCompletedEventHandler((cancelArgs) => { cancelArgs.Cancel = true; });
                        frm.OpenDatabase();
                        if (frm.State == frmConnectToDatabase.ConnectionStates.Ready)
                        {
                            newRecordsetToolStripMenuItem.Enabled = true;
                            openRecordsetToolStripMenuItem.Enabled = true;
                            recordsetsToolStripMenuItem.Enabled = true;

                            OpeningDatabase();
                            if (openRecordsetToolStripMenuItem.DropDownItems[0] != openOtherRecordsetToolStripMenuItem)
                            {
                                // In other words, only open the most recent if there is a most recent to open.
                                // The reason there would not be is that the opened database doesn't have any active recordsets.
                                openRecordsetToolStripMenuItem.DropDownItems[0].PerformClick();
                            }
                            else
                            {
                                CloseCurrentRecordSet();
                            }
                            RefreshTitleBar();
                        }
                        else { Program.Settings.CurrentDatabaseFilePath = string.Empty; }
                    }
                }
                catch (Exception ex)
                {
                    Utility.ShowErrorMessage(this, ex);
                    Program.Settings.CurrentDatabaseFilePath = string.Empty;
                }
            }

            if (string.IsNullOrEmpty(Program.Settings.CurrentDatabaseFilePath))
            {
                connectToDatabaseToolStripMenuItem.PerformClick();
            }
#else
            connectToDatabaseToolStripMenuItem.PerformClick();
#endif

#if EEB460
            searchGenBankToolStripMenuItem.Visible = false;
            bLASTNResultsHistoryToolStripMenuItem.Visible = false;
            sepGeneSequences1.Visible = false;
            viewMUSCLEAlignmentHistoryToolStripMenuItem.Visible = false;
            viewPRANKAlignmentHistoryToolStripMenuItem.Visible = false;
            sepGeneSequences2.Visible = false;
#endif
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Program.Settings.CurrentDatabaseFilePath))
            {
                try
                {
                    Program.DatabaseSettings.SaveDatabaseProperties();

                    if (CurrentRecordSet != null && !string.IsNullOrWhiteSpace(CurrentRecordSet.ID))
                    {
                        Program.RecordSetSettings.Save(CurrentRecordSet.ID);
                    }
                }
                finally { }
            }
        }

        private void frmMain_Resize(object sender, EventArgs e)
        {
#if DEBUG
            tsbResolution.Text = this.Size.ToString();
#endif
        }

        private void fullScreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void tbForm_Selected(object sender, TabControlEventArgs e)
        {
            if (this.CurrentRecordSet != null && e.TabPage != null)
            {
                Program.RecordSetSettings.LastModuleOpen = e.TabPage.Name;
                ToggleModuleSpecificMenuItems();
            }
        }

        private Dictionary<ToolStripMenuItem, DataTypes> _moduleSpecificMenuItems { get; set; }
        private void ToggleModuleSpecificMenuItems()
        {
            if (_moduleSpecificMenuItems == null)
            {
                _moduleSpecificMenuItems = new Dictionary<ToolStripMenuItem, DataTypes>();
                _moduleSpecificMenuItems.Add(geneSequencesToolStripMenuItem, DataTypes.GeneSequence);
                _moduleSpecificMenuItems.Add(selectionAnalysesPAMLToolStripMenuItem, DataTypes.CodeMLResult);
            }

            mnuForm.SuspendLayout();
            foreach (var item in _moduleSpecificMenuItems) { item.Key.Visible = (item.Value == this.CurrentModule); }
            mnuForm.ResumeLayout();
        }

        private void x800ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.Size = new System.Drawing.Size(1210, 800);
        }

        private void x768ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.Size = new System.Drawing.Size(1024, 768);
        }

        private void x600ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.Size = new System.Drawing.Size(800, 600);
        }
        #endregion

        /// <summary>
        /// These things happen immediately after a database has been selected and opened.
        /// </summary>
        private void OpeningDatabase()
        {
            ListRecentRecordSets();
            uctGeneSequencesMain1.RestoreUserLayout();
            uctPAMLMain1.RestoreUserLayout();
        }

        private void RefreshTitleBar()
        {
            this.Text = Application.ProductName + " - v" + Application.ProductVersion + " " + Utility.GetEntryAssemblyConfiguration()
                + (Program.Settings != null && !string.IsNullOrWhiteSpace(Program.Settings.CurrentDatabaseFilePath) ? " - " + Program.Settings.CurrentDatabaseFilePath : string.Empty)
                + (CurrentRecordSet != null ? " - " + CurrentRecordSet.Name : string.Empty);
        }

        private void OpenCurrentRecordSet(string SelectedSubSetName = "")
        {
            Program.DebugStartupTime("OpenCurrentRecordSet - Start");
            try
            {
                uctGeneSequencesMain1.RefreshSubSets();
                uctPAMLMain1.RefreshSubSets();

                tbForm.Enabled = true;

                subsetsToolStripMenuItem.Enabled = true;
                newRecordsetToolStripMenuItem.Enabled = true;
                openRecordsetToolStripMenuItem.Enabled = true;
                geneSequencesToolStripMenuItem.Enabled = true;
                selectionAnalysesPAMLToolStripMenuItem.Enabled = true;

                tbForm.SelectedTab = null;
                TabPage lastPage = tbForm.TabPages.Cast<TabPage>().FirstOrDefault(pg => pg.Name == Program.RecordSetSettings.LastModuleOpen);
                if (lastPage != null) { tbForm.SelectedTab = lastPage; }
                else { tbForm.SelectedTab = pgGenes; }

                CurrentRecordSet.Opened();
                ListRecentRecordSets();

                RefreshTitleBar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            Program.DebugStartupTime("OpenCurrentRecordSet - End");
        }

        private void CloseCurrentRecordSet()
        {
            try
            {
                tbForm.Enabled = false;

                uctGeneSequencesMain1.ClearSubSets();
                subsetsToolStripMenuItem.Enabled = false;
                geneSequencesToolStripMenuItem.Enabled = false;
                selectionAnalysesPAMLToolStripMenuItem.Enabled = false;

                tbForm.SelectedTab = null;
                tbForm.SelectedTab = pgGenes;

                CurrentRecordSet = null;
                RefreshTitleBar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Menu
        private void connectToDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmConnectToDatabase frm = new frmConnectToDatabase())
            {
                if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    newRecordsetToolStripMenuItem.Enabled = true;
                    openRecordsetToolStripMenuItem.Enabled = true;
                    recordsetsToolStripMenuItem.Enabled = true;

                    if (frm.NewDatabase)
                    {
                        if (NewRecordSet() == System.Windows.Forms.DialogResult.Cancel)
                        {
                            CloseCurrentRecordSet();
                        }
                    }
                    else
                    {
                        OpeningDatabase();

                        if (openRecordsetToolStripMenuItem.DropDownItems[0] != openOtherRecordsetToolStripMenuItem)
                        {
                            // In other words, only open the most recent if there is a most recent to open.
                            // The reason there would not be is that the opened database doesn't have any active recordsets.
                            openRecordsetToolStripMenuItem.DropDownItems[0].PerformClick();
                        }
                        else
                        {
                            CloseCurrentRecordSet();
                        }
                    }
                }
                else
                {
                    // If a database is already open, leave it open, otherwise make sure things are closed up.
                    if (string.IsNullOrWhiteSpace(Program.Settings.CurrentDatabaseFilePath))
                    {
                        CloseCurrentRecordSet();

                        newRecordsetToolStripMenuItem.Enabled = false;
                        openRecordsetToolStripMenuItem.Enabled = false;
                        recordsetsToolStripMenuItem.Enabled = false;
                    }
                }
            }

            RefreshTitleBar();
        }

        private DialogResult NewRecordSet()
        {
            DialogResult result = System.Windows.Forms.DialogResult.Cancel;

            using (RecordSets.frmEditRecordSet frm = new RecordSets.frmEditRecordSet(new RecordSet()))
            {
                result = frm.ShowDialog(this);
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    Program.Settings.CurrentRecordSet = frm.CurrentRecordSet;
                    OpenCurrentRecordSet("All");
                }
            }

            return result;
        }

        private void newRecordsetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewRecordSet();
        }

        private void ListRecentRecordSets()
        {
            while (openRecordsetToolStripMenuItem.DropDownItems[0] != openOtherRecordsetToolStripMenuItem)
            {
                openRecordsetToolStripMenuItem.DropDownItems.RemoveAt(0);
            }

            // Pull the top 5 recordsets to add to the Open dialog as recent.
            List<RecordSet> recentRecordSets = RecordSet.List(true);
            if (recentRecordSets.Count != 0)
            {
                openRecordsetToolStripMenuItem.DropDownItems.Insert(0, new ToolStripSeparator());

                recentRecordSets.OrderByDescending(rs => rs.LastOpenedAt).Take(5).Reverse().ToList().ForEach(rs =>
                {
                    ToolStripMenuItem mnu = new ToolStripMenuItem(rs.Name + " (" + rs.ModifiedAt.ToStandardDateTimeString() + ")") { Tag = rs };
                    mnu.Click += new EventHandler(openRecentRecordsetToolStripMenuItem_Click);
                    openRecordsetToolStripMenuItem.DropDownItems.Insert(0, mnu);
                });
            }
        }

        private void openRecentRecordsetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurrentRecordSet = (RecordSet)((ToolStripMenuItem)sender).Tag;
            OpenCurrentRecordSet();
        }

        private void openRecordsetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (RecordSets.frmManageRecordSets frm = new RecordSets.frmManageRecordSets())
            {
                if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    OpenCurrentRecordSet(frm.IsNew ? "All" : string.Empty);
                }
                else
                {
                    if (frm.CurrentRecordSetDeleted)
                    {
                        Program.Settings.CurrentRecordSet = null;
                        CloseCurrentRecordSet();
                    }
                }

                ListRecentRecordSets();
                RefreshTitleBar();
            }
        }

        private void subsetsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            switch (CurrentModule)
            {
                case DataTypes.GeneSequence:
                    uctGeneSequencesMain1.OpenSubSet_Click(subsetsToolStripMenuItem, EventArgs.Empty);
                    break;
                case DataTypes.CodeMLResult:
                    uctPAMLMain1.OpenSubSet_Click(subsetsToolStripMenuItem, EventArgs.Empty);
                    break;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region Module-Specific
        // Gene Sequences
        private void searchGenBankToolStripMenuItem_Click(object sender, EventArgs e)
        {
            uctGeneSequencesMain1.SearchGenBank();
        }

        private void bLASTNResultsHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            uctGeneSequencesMain1.ViewBLASTNResultsHistory();
        }

        private void viewPRANKAlignmentHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            uctGeneSequencesMain1.ViewPRANKAlignmentHistory();
        }

        private void viewMUSCLEAlignmentHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            uctGeneSequencesMain1.ViewMUSCLEAlignmentHistory();
        }

        private void viewPhyMLHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            uctGeneSequencesMain1.ViewPhyMLHistory();
        }

        // PAML
        private void viewPAMLJobHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            uctPAMLMain1.ViewJobHistory();
        }
        #endregion

        #region Tools
        private void testEUtilitiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (Utilities.frmEUtilitiesTest frm = new Utilities.frmEUtilitiesTest())
            {
                frm.ShowDialog();
            }
        }

        private void stringTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (Utilities.frmStringsAreTheSame frm = new Utilities.frmStringsAreTheSame())
            {
                frm.ShowDialog();
            }
        }
        #endregion

        private void aboutPilgrimageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmAbout frm = new frmAbout()) { frm.ShowDialog(); }
        }
        #endregion

        #region RecordSet Actions
        private void exportRecordsetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (RecordSets.frmExportToPilgrimageDataFile frm = new RecordSets.frmExportToPilgrimageDataFile(null, null, DataTypes.Undefined))
            { frm.ShowDialog(); }

            //System.IO.FileInfo file = null;
            //if (IODialogHelper.SaveFile(IODialogHelper.DialogPresets.PilgrimageDataFile, CurrentRecordSet.Name, this, ref file))
            //{
            //    Activities.ExportToDataFile export = new Activities.ExportToDataFile(this);
            //    export.ActivityCompleted += new Activities.Activity.ActivityCompletedEventHandler(exportRecordSet_ActivityCompleted);
            //    export.Export(CurrentRecordSet.ID, file.FullName, true);
            //}
        }

        private void exportRecordSet_ActivityCompleted(Activities.ActivityCompletedEventArgs e)
        {
            if (e.Error != null) { Utility.ShowErrorMessage(this, e.Error); }
            else if (e.Cancelled) { return; }
            else { Program.Settings.LastWorkingDirectory = (new System.IO.FileInfo((string)e.Result)).Directory.FullName; }
        }

        private void importRecordsetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (RecordSets.frmImportDataFile frm = new RecordSets.frmImportDataFile())
            {
                if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (Utility.ShowMessage(this, "Open imported project?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                    {
                        CurrentRecordSet = RecordSet.List(true).First(rs => GuidCompare.Equals(rs.ID, frm.RecordSetID));
                        OpenCurrentRecordSet();
                    }
                }
            }
        }
        #endregion

        #region In-Progress Activities
        private void InProgressActivities_ActivityAdded(InProgress.ActivityEventArgs e)
        {
            e.Activity.StatusUpdate += new StatusUpdateEventHandler(Activity_StatusUpdate);
            UpdateInProgressStatusLabels();
        }

        private void InProgressActivities_ActivityRemoved(InProgress.ActivityEventArgs e)
        {
            UpdateInProgressStatusLabels();
        }

        private void Activity_StatusUpdate(ChangLab.Jobs.Job sender, StatusUpdateEventArgs e)
        {
            if (sender.HasCompleted)
            {
                Activity activity = Program.InProgressActivities.GetActivityByJobID<Activity>(sender.ID);
                activity.StatusUpdate -= Activity_StatusUpdate;

                if (e.Status != ChangLab.Jobs.JobStatuses.Cancelled)
                {
                    string messageText = string.Empty;
                    switch (activity.CurrentJob.Target)
                    {
                        case ChangLab.Jobs.JobTargets.BLASTN_NCBI: messageText = "BLASTN alignment"; break;
                        case ChangLab.Jobs.JobTargets.PRANK: messageText = "PRANK alignment"; break;
                        case ChangLab.Jobs.JobTargets.MUSCLE: messageText = "MUSCLE alignment"; break;
                        case ChangLab.Jobs.JobTargets.PhyML: messageText = "PhyML tree"; break;
                        case ChangLab.Jobs.JobTargets.CodeML: messageText = "CodeML analysis"; break;
                    }

                    ShowBalloonMessage(messageText + " " + ChangLab.Jobs.JobStatusCollection.NameByKey(activity.CurrentJob.Status).ToLower() + " and awaiting review", ToolTipIcon.Info, activity);
                }
            }

            UpdateInProgressStatusLabels();
        }

        private void UpdateInProgressStatusLabels()
        {
            tslInProgressBlastNAtNCBIs.Text = InProgressStatusLabelText(Program.InProgressActivities.BlastNAtNCBIs.Cast<Activity>(), "BLASTN alignment", "BLASTN alignments");
            tslInProgressPRANKAlignments.Text = InProgressStatusLabelText(Program.InProgressActivities.PRANKAlignments.Cast<Activity>(), "PRANK alignment", "PRANK alignments");
            tslInProgressMUSCLEAlignments.Text = InProgressStatusLabelText(Program.InProgressActivities.MUSCLEAlignments.Cast<Activity>(), "MUSCLE alignment", "MUSCLE alignments");
            tslInProgressPhyMLs.Text = InProgressStatusLabelText(Program.InProgressActivities.PhyMLs.Cast<Activity>(), "PhyML tree", "PhyML trees");
            tslInProgressCodeMLAnalyses.Text = InProgressStatusLabelText(Program.InProgressActivities.CodeMLAnalyses.Cast<Activity>(), "CodeML analysis", "CodeML analyses");
        }

        private string InProgressStatusLabelText(IEnumerable<Activity> Activities, string ActivityName, string ActivitiesName)
        {
            string labelText = string.Empty;

            int inProgressCount = Activities.Where(a => !a.CurrentJob.HasCompleted).Count();
            if (inProgressCount > 0)
            { labelText = inProgressCount.ToString() + " " + (inProgressCount > 1 ? ActivitiesName : ActivityName) + " in-progress"; }

            int completedCount = Activities.Where(a => a.CurrentJob.HasCompleted).Count();
            if (completedCount > 0)
            {
                labelText +=
                    (!string.IsNullOrEmpty(labelText) ? ", " : "")
                    + completedCount.ToString()
                    + (!string.IsNullOrEmpty(labelText) ? "" : " " + (completedCount > 1 ? ActivitiesName : ActivityName))
                    + " completed and awaiting review";
            }

            return labelText;
        }

        private void tslInProgressCodeMLAnalyses_Click(object sender, EventArgs e)
        {
            viewPAMLJobHistoryToolStripMenuItem.PerformClick();
        }

        private void tslInProgressPRANKAlignments_Click(object sender, EventArgs e)
        {
            viewPRANKAlignmentHistoryToolStripMenuItem.PerformClick();
        }

        private void tslInProgressMUSCLEAlignments_Click(object sender, EventArgs e)
        {
            viewMUSCLEAlignmentHistoryToolStripMenuItem.PerformClick();
        }

        private void tslInProgressPhyMLs_Click(object sender, EventArgs e)
        {
            viewPhyMLHistoryToolStripMenuItem.PerformClick();
        }

        private void tslInProgressBlastNAtNCBIs_Click(object sender, EventArgs e)
        {
            bLASTNResultsHistoryToolStripMenuItem.PerformClick();
        }
        #endregion

        #region Taskbar Icon
        private int taskbarIcon_DefaultTimeout = 3000;

        internal void ShowBalloonMessage(string Text, ToolTipIcon Icon, object Tag)
        {
            taskbarIcon.Tag = Tag;
            taskbarIcon.ShowBalloonTip(taskbarIcon_DefaultTimeout, Application.ProductName, Text, Icon);
        }

        private void taskbarIcon_BalloonTipClicked(object sender, EventArgs e)
        {
            if (taskbarIcon.Tag != null)
            {
                if (taskbarIcon.Tag.GetType().BaseType == typeof(Activity))
                {
                    this.BringToFront();
                    this.Activate();

                    if (this.Visible && this.CanFocus) // CanFocus == true tells us that no modal dialogs are open.
                    {
                        Activity activity = (Activity)taskbarIcon.Tag;
                        switch (activity.CurrentJob.Target)
                        {
                            case ChangLab.Jobs.JobTargets.BLASTN_NCBI: uctGeneSequencesMain1.ViewBLASTNResultsHistory(activity); break;
                            case ChangLab.Jobs.JobTargets.PRANK: uctGeneSequencesMain1.ViewPRANKAlignmentHistory(activity); break;
                            case ChangLab.Jobs.JobTargets.MUSCLE: uctGeneSequencesMain1.ViewMUSCLEAlignmentHistory(activity); break;
                            case ChangLab.Jobs.JobTargets.PhyML: uctGeneSequencesMain1.ViewPhyMLHistory(activity); break;
                            case ChangLab.Jobs.JobTargets.CodeML: uctPAMLMain1.ViewJobHistory(activity.CurrentJob); break;
                        }
                    }
                }
            }
        }

        private void taskbarIcon_BalloonTipClosed(object sender, EventArgs e)
        {
            taskbarIcon.Tag = null;
        }

        private void taskbarIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.BringToFront();
            this.Activate();
        }
        #endregion
    }
}