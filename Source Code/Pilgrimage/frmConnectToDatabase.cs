using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChangLab.Common;
using ChangLab.LocalDatabase;

namespace Pilgrimage
{
    public partial class frmConnectToDatabase : DialogForm
    {
        private string OriginalConnectionString { get; set; }

        internal bool NewDatabase { get; private set; }
        private FileInfo DatabaseFile { get; set; }
        private IWin32Window ParentWindow { get; set; }

        private BackgroundWorker AutoUpdateWorker { get; set; }
        public ConnectionStates State { get; set; }

        public enum ConnectionStates
        {
            New = 0,
            Connecting = 1,
            Connected = 2,
            Upgrading = 3,
            Upgraded = 4,
            Cancelled = 5,
            Failed = 6,
            Ready = 7
        }

        public frmConnectToDatabase() : this(string.Empty, null) { }

        public frmConnectToDatabase(string FilePath, IWin32Window ParentWindow)
        {
            InitializeComponent();
            // If the user is changing databases and for whatever reason the database the user selects doesn't open and the user cancels out, we need
            // to be able to revert to the originally open database.
            OriginalConnectionString = DataAccess.ConnectionString;

            SetButtonImage(btnNewDatabase, "New");
            SetButtonImage(btnOpen, "DB");
            SetButtonImage(btnCancel, "Cancel");

            if (!string.IsNullOrWhiteSpace(FilePath))
            {
                this.DatabaseFile = new FileInfo(FilePath);
            }
            this.ParentWindow = (ParentWindow == null ? this : ParentWindow);
            this.State = ConnectionStates.New;
            this.NewDatabase = false;
        }

        private void frmConnectToDatabase_Load(object sender, EventArgs e)
        {
            int fileCount = Program.Settings.RecentDatabases.Count;
            if (fileCount != 0)
            {
                tblRecent.RowStyles.Clear();
                tblRecent.RowCount = fileCount;

                for (int i = 0; i < fileCount; i++)
                {
                    FileInfo file = Program.Settings.RecentDatabases[i];

                    tblRecent.RowStyles.Add(new RowStyle() { SizeType = SizeType.AutoSize });

                    LinkLabel lnk = new LinkLabel()
                        {
                            Anchor = (AnchorStyles)(AnchorStyles.Left | AnchorStyles.Right),
                            AutoSize = true,
                            Margin = new System.Windows.Forms.Padding(5),
                            Text = file.FullName,
                            Tag = file
                        };
                    lnk.LinkClicked += new LinkLabelLinkClickedEventHandler(lnk_LinkClicked);
                    tblRecent.Controls.Add(lnk, 0, i);
                }
            }
            else
            {
                lblRecent.Parent.Controls.Remove(lblRecent);
                tblRecent.Parent.Controls.Remove(tblRecent);
            }

#if EEB460 && !DEBUG
            if (lblRecent.Parent != null)
            {
                lblRecent.Parent.Controls.Remove(lblRecent);
                tblRecent.Parent.Controls.Remove(tblRecent);
            }
#endif
        }

        private void frmConnectToDatabase_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.State != ConnectionStates.Ready)
            {
                // Revert back so that the originally opened database is still the one being interacted with.
                // If no database was open, this would just set the connection string as empty.
                DataAccess.ConnectionString = OriginalConnectionString;
            }
        }

        private void lnk_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                this.DatabaseFile = (FileInfo)((LinkLabel)sender).Tag;
                OpenDatabase();
                if (this.State == ConnectionStates.Ready) { this.DialogResult = System.Windows.Forms.DialogResult.OK; }
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            FileInfo file = null;
            if (IODialogHelper.OpenFile(IODialogHelper.DialogPresets.Database, this, ref file))
            {
                txtFilePath.Text = file.FullName;
            }
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFilePath.Text))
            {
                Utility.ShowMessage(this, "Please select a database file.");
                return;
            }
            if (!File.Exists(txtFilePath.Text))
            {
                Utility.ShowMessage(this, "Could not find database file.");
                return;
            }

            this.DatabaseFile = new FileInfo(txtFilePath.Text);
            OpenDatabase();
            if (this.State == ConnectionStates.Ready)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }

        private void btnNewDatabase_Click(object sender, EventArgs e)
        {
            FileInfo file = null;
            if (IODialogHelper.SaveFile(IODialogHelper.DialogPresets.Database, string.Empty, this, ref file))
            {
                this.DatabaseFile = file;
                using (frmSetDatabaseProperties prop = new frmSetDatabaseProperties())
                {
                    if (prop.ShowDialog(this.OwnerForm) == System.Windows.Forms.DialogResult.OK)
                    {
                        CreateDatabase(this.DatabaseFile.FullName);
                        if (this.State == ConnectionStates.Ready)
                        {
                            NewDatabase = true;
                            // This property will get overwritten at the end of CreateDatabase() when ConnectToDatabase() gets called to refresh the
                            // in-memory properties using what's in the database (in this case configured by the scripts), so we preserve it via the
                            // frmSetDatabaseProperties instance.
                            Program.DatabaseSettings.NCBIEmailAddress = prop.NCBIEmailAddress; Program.DatabaseSettings.SaveDatabaseProperty("NCBIEmailAddress");
                            this.DialogResult = System.Windows.Forms.DialogResult.OK;
                        }
                    }
                }
            }
        }

        private void RefreshLocalPropertiesFromDatabase()
        {
            Program.DatabaseSettings.RefreshDatabaseProperties();

            if (Program.Settings.RecentDatabases.Any(f => f.FullName.ToLower() == DatabaseFile.FullName.ToLower()))
            {
                Program.Settings.RecentDatabases.Remove(
                    Program.Settings.RecentDatabases.First(f => f.FullName.ToLower() == DatabaseFile.FullName.ToLower()));
            }
            Program.Settings.RecentDatabases.Insert(0, DatabaseFile);

            Program.Settings.CurrentDatabaseFilePath = DatabaseFile.FullName;
            Program.Settings.LastDatabaseDirectory = DatabaseFile.Directory.FullName;

            ChangLab.NCBI.EUtilities.ProductName = Program.DatabaseSettings.NCBIProductName;
            ChangLab.NCBI.EUtilities.Email = Program.DatabaseSettings.NCBIEmailAddress;
        }

        #region New
        private void CreateDatabase(string FilePath)
        {
            using (AutoUpdateWorker = new BackgroundWorker())
            {
                AutoUpdateWorker.WorkerReportsProgress = true;
                AutoUpdateWorker.DoWork += new DoWorkEventHandler(AutoUpdateWorker_DoWork);
                AutoUpdateWorker.ProgressChanged += new ProgressChangedEventHandler(AutoUpdateWorker_ProgressChanged);
                AutoUpdateWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(AutoUpdateWorker_RunWorkerCompleted);

                using (ProgressForm = new frmProgress("Creating new database...", new frmProgress.ProgressOptions() { AllowCancellation = false }))
                {
                    ProgressForm.Cancelled += new frmProgress.CancelledEventHandler(ProgressForm_Cancelled);
                    AutoUpdateWorker.RunWorkerAsync();
                    ProgressForm.ShowDialog(ParentWindow);
                }
            }
        }

        private void AutoUpdateWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            this.State = ConnectionStates.Upgrading;

            using (AutoUpdate au = new AutoUpdate(this.DatabaseFile.FullName, new Version(0, 0)))
            {
                au.ProgressUpdate += new ProgressUpdateEventHandler(AutoUpdate_ProgressUpdate);
                au.UpdateToLatest();
            }

            ConnectToDatabase(); // Verify that we can still talk to the database.
            RefreshLocalPropertiesFromDatabase();
        }

        private void AutoUpdateWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressForm.UpdateProgress((ProgressUpdateEventArgs)e.UserState);
        }

        private void AutoUpdate_ProgressUpdate(ProgressUpdateEventArgs e)
        {
            if (AutoUpdateWorker != null && AutoUpdateWorker.IsBusy) { AutoUpdateWorker.ReportProgress(0, e); }
            else { ProgressForm.UpdateProgress(e); }
        }

        private void AutoUpdateWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Utility.ShowErrorMessage(ProgressForm, e.Error);
                this.State = ConnectionStates.Failed;
            }
            else
            {
                this.State = ConnectionStates.Ready;
            }

            this.CloseProgressForm(System.Windows.Forms.DialogResult.OK);
        }
        #endregion

        /// <remarks>
        /// If this succeeds, Program.DatabaseProperties will have been overwritten.
        /// </remarks>
        public void OpenDatabase()
        {
            ConnectToDatabaseAsync();

            if (this.State == ConnectionStates.Connected)
            {
                try
                {
                    // Check to see if the database needs to be upgraded, if so, prompt the user.
                    // The upgrade has to complete successfully before we can formally "open" the database, and thus include it as recent.
                    using (AutoUpdate au = new AutoUpdate(DatabaseFile.FullName, DataAccess.ConnectedDatabaseVersion))
                    {
                        if (au.DatabaseExceedsApplicationVersion)
                        {
                            if (AutoUpdatePrompt != null)
                            {
                                // Allow the caller to halt the process here instead of prompting the user.
                                CancelEventArgs cancelArgs = new CancelEventArgs(false);
                                OnAutoUpdatePrompt(cancelArgs);
                                if (cancelArgs.Cancel) { return; }
                            }
                            Utility.ShowMessage(ParentWindow, "The database file selected has been upgraded to be compatible with a newer version of the application."
                                                            + " To open this file, version " + DataAccess.ConnectedDatabaseVersion + " or greater of the application is required.", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            return;
                        }
                        else if (au.UpgradesExist)
                        {
                            if (AutoUpdatePrompt != null)
                            {
                                // Allow the caller to halt the process here instead of prompting the user.
                                CancelEventArgs cancelArgs = new CancelEventArgs(false);
                                OnAutoUpdatePrompt(cancelArgs);
                                if (cancelArgs.Cancel) { return; }
                            }
                            if (Utility.ShowMessage(ParentWindow, "The database file selected must be upgraded to be compatible with the application before it can be opened."
                                                                + " Select \"OK\" to upgrade the database, or \"Cancel\" to select a different database file.", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning)
                                == DialogResult.OK)
                            {
                                UpdateToLatest(au);

                                if (this.State == ConnectionStates.Failed) { return; }
                            }
                            else { return; }
                        }
                        else
                        {
                            // Grab the latest database properties.  UpdateToLatest() will do this so we don't need to add it in the else-if above.
                            RefreshLocalPropertiesFromDatabase();
                        }
                    }

                    // CR-043: Now users have to supply their own e-mail address instead of NCBI always having mine.
                    if (string.IsNullOrWhiteSpace(Program.DatabaseSettings.NCBIEmailAddress) || !Program.DatabaseSettings.NCBIEmailAddress.IsEmailAddress())
                    {
                        using (frmSetDatabaseProperties prop = new frmSetDatabaseProperties())
                        {
                            if (prop.ShowDialog(this) == System.Windows.Forms.DialogResult.Cancel)
                            { this.State = ConnectionStates.Cancelled; return; /* Cancel out if no valid e-mail address is provided. */ }
                        }
                    }

                    this.State = ConnectionStates.Ready;
                }
                catch (Exception ex)
                {
                    Utility.ShowErrorMessage(this, ex);
                }
            }
        }

        private void ProgressForm_Cancelled(RunWorkerCompletedEventArgs e)
        {
            this.State = ConnectionStates.Cancelled;
            this.CloseProgressForm(System.Windows.Forms.DialogResult.Cancel);
        }

        #region Connect
        /// <remarks>
        /// Attempts to connect to the database by calling a diagnostic procedure to verify that the database can be communicated with.
        /// </remarks>
        private void ConnectToDatabase()
        {
            ChangLab.LocalDatabase.DataAccess.SetConnectionString(DatabaseFile.FullName);

            this.State = ConnectionStates.Connecting;
            int retryLimit = 2;
            for (int retryCount = 0; retryCount < retryLimit; retryCount++)
            {
                try
                {
                    // This serves as a decent test as to whether we can connect to the chosen database.
                    Diagnostics.VerifyDatabaseConnectivity();
                    this.State = ConnectionStates.Connected;
                    break;
                }
                catch (Exception ex)
                {
                    // If connecting to the database fails with a Connection Timeout error on the first try, that's entirely likely due to it
                    // taking longer to boot up the user instance than the connection timeout allows for.  If it fails on the second attempt then
                    // there's more likely something else wrong.
                    if ((retryCount + 1) < retryLimit
                        && (ex.GetType() == typeof(SqlException)))
                    {
                        SqlException sqlEx = (SqlException)ex;
                        if (sqlEx.Message.Contains("Connection Timeout Expired")
                            && sqlEx.InnerException != null
                            && sqlEx.InnerException.Message.Contains("The wait operation timed out"))
                        {
                            continue;
                            /*
                                For reference, these are the expected error details:
                                Message: Connection Timeout Expired.  The timeout period elapsed while attempting to consume the pre-login handshake
                                            acknowledgement.  This could be because the pre-login handshake failed or the server was unable to
                                            respond back in time.  The duration spent while attempting to connect to this server was - [Pre-Login]
                                            initialization=21572; handshake=1; 
                                Server: (localDB)\MSSQLLocalDB
                                State: 0
                                ErrorCode: -2146232060

                                - Inner Exception -
                                Message: The wait operation timed out
                                NativeErrorCode: 258
                            */
                        }
                    }

                    throw ex;
                }
            }
        }

        /// <remarks>
        /// Calls ConnectToDatabase() "asynchronously" by putting a ProgressForm in the way of the user while the function does its thing.
        /// </remarks>
        private void ConnectToDatabaseAsync()
        {
            using (BackgroundWorker bwOpen = new BackgroundWorker())
            {
                bwOpen.DoWork += new DoWorkEventHandler(bwOpen_DoWork);
                bwOpen.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwOpen_RunWorkerCompleted);

                using (ProgressForm = new frmProgress("Connecting to database...", new frmProgress.ProgressOptions() { UseNeverEndingTimer = true, AllowCancellation = false }))
                {
                    ProgressForm.Cancelled += new frmProgress.CancelledEventHandler(ProgressForm_Cancelled);
                    bwOpen.RunWorkerAsync();
                    ProgressForm.ShowDialog(ParentWindow);
                }
            }
        }

        private void bwOpen_DoWork(object sender, DoWorkEventArgs e)
        {
            ConnectToDatabase();
        }

        private void bwOpen_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Utility.ShowErrorMessage(ProgressForm, e.Error);
                this.State = ConnectionStates.Failed;
            }

            this.CloseProgressForm(System.Windows.Forms.DialogResult.OK);
        }
        #endregion

        #region Upgrade
        private void UpdateToLatest(AutoUpdate Updater)
        {
            using (BackgroundWorker bwUpgrade = new BackgroundWorker())
            {
                bwUpgrade.DoWork += new DoWorkEventHandler(bwUpgrade_DoWork);
                bwUpgrade.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwUpgrade_RunWorkerCompleted);

                using (ProgressForm = new frmProgress("Upgrading database...", new frmProgress.ProgressOptions() { UseNeverEndingTimer = true, AllowCancellation = false }))
                {
                    ProgressForm.Cancelled += new frmProgress.CancelledEventHandler(ProgressForm_Cancelled);
                    bwUpgrade.RunWorkerAsync(Updater);
                    ProgressForm.ShowDialog(ParentWindow);
                }
            }
        }

        private void bwUpgrade_DoWork(object sender, DoWorkEventArgs e)
        {
            this.State = ConnectionStates.Upgrading;
            AutoUpdate au = (AutoUpdate)e.Argument;
            au.UpdateToLatest();

            ConnectToDatabase(); // Verify that we can still talk to the database.
            RefreshLocalPropertiesFromDatabase(); // Pick up any changes caused by upgrade scripts.
        }

        private void bwUpgrade_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Utility.ShowErrorMessage(ProgressForm, e.Error);
                this.State = ConnectionStates.Failed;
            }
            else
            {
                this.State = ConnectionStates.Upgraded;
            }

            this.CloseProgressForm(System.Windows.Forms.DialogResult.OK);
        }
        #endregion

        #region Events
        protected virtual void OnAutoUpdatePrompt(CancelEventArgs e)
        {
            if (AutoUpdatePrompt != null)
            {
                AutoUpdatePrompt(e);
            }
        }
        public delegate void ActivityCompletedEventHandler(CancelEventArgs e);
        public event ActivityCompletedEventHandler AutoUpdatePrompt;
        #endregion
    }
}
