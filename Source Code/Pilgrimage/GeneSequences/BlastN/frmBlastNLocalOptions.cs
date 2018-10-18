using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChangLab.Common;
using ChangLab.Genes;
using ChangLab.NCBI;
using Pilgrimage.Activities;

namespace Pilgrimage.GeneSequences.BlastN
{
    public partial class frmBlastNLocalOptions : Form
    {
        public List<Gene> SelectedGenes { get; set; }
        public frmMain MainForm { get; set; }
        internal BlastSequencesAtNCBI Activity { get; private set; }
        internal ChangLab.Jobs.BlastNAtNCBI.BLASTPurposes Purpose { get; private set; }

        public frmBlastNLocalOptions()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            List<string> validationMessages = new List<string>();
            if (string.IsNullOrWhiteSpace(txtLocal_DatabaseFile.Text)) { validationMessages.Add("A database file has not been provided."); }
            else if (!System.IO.File.Exists(txtLocal_DatabaseFile.Text)) { validationMessages.Add("The database file could not be found."); }

            if (string.IsNullOrWhiteSpace(txtLocal_BlastNExeDirectory.Text)) { validationMessages.Add("The directory for blastn.exe has not been provided."); }
            else if (!System.IO.Directory.Exists(txtLocal_BlastNExeDirectory.Text)) { validationMessages.Add("The directory for blastn.exe could not be found."); }
            else if (!System.IO.File.Exists(txtLocal_BlastNExeDirectory.Text + "\\blastn.exe")) { validationMessages.Add("blastn.exe could not be found in the provided directory."); }

            if (string.IsNullOrWhiteSpace(txtLocal_OutputDirectory.Text)) { validationMessages.Add("An output directory has not been provided."); }
            else if (!System.IO.Directory.Exists(txtLocal_OutputDirectory.Text))
            {
                if (Utility.ShowMessage(this, "The output directory could not be found.  Do you want to create it?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    try { System.IO.Directory.CreateDirectory(txtLocal_OutputDirectory.Text); }
                    catch (Exception ex) { Utility.ShowErrorMessage(this, ex); }
                }
                else { return; }
            }

            if (validationMessages.Count != 0)
            {
                Utility.ShowMessage(this, validationMessages.Concatenate("\r\n"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveSettings();
            QueryLocalDatabase();
        }

        private void QueryLocalDatabase()
        {
            BlastSequencesWithLocalDatabase blastn = new BlastSequencesWithLocalDatabase(Program.Settings.CurrentSubSet_GeneSequences, this);
            blastn.ResultsSaved += new BlastSequencesWithLocalDatabase.ResultsSavedEventHandler(blastn_ResultsSaved);
            blastn.ActivityCompleted += new Activity.ActivityCompletedEventHandler(blastn_Completed);

            this.Hide();
            blastn.Submit(SelectedGenes, txtLocal_DatabaseFile.Text, txtLocal_BlastNExeDirectory.Text, txtLocal_OutputDirectory.Text);
        }

        private void blastn_ResultsSaved(ChangLab.Jobs.BlastNAtNCBI.ResultsEventArgs e)
        {
            // Update the Has BlastN Results columns in the open subset views for those sequences that were submitted and for which we got results.
            MainForm.uctGeneSequencesMain1.TabPages.Cast<TabPage>().ToList()
                .ForEach(pg => ((GeneSequences.uctRecordSetGenes)pg.Controls[0])
                                    .UpdateHasAlignedSubjectSequences(e.Alignments.Where(kv => kv.Value.Count != 0).Select(kv => kv.Key.ID).ToList()));
        }

        private void blastn_Completed(ActivityCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                this.Show();
                return;
            }
            if (e.Error != null)
            {
                Utility.ShowErrorMessage(this, e.Error);
                this.Show();
                return;
            }
            else
            {
                List<string> editedSubSetIDs = (List<string>)e.Result;
                if (editedSubSetIDs.Count != 0)
                {
                    editedSubSetIDs.Distinct().ToList().ForEach(id => MainForm.uctGeneSequencesMain1.ShowAndRefreshSubSet(id, null));
                }

                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }

        private void SaveSettings()
        {
            Program.RecordSetSettings.LastLocalBlastNDatabaseFile = txtLocal_DatabaseFile.Text;
            Program.RecordSetSettings.LastLocalBlastNExeDirectory = txtLocal_BlastNExeDirectory.Text;
            Program.RecordSetSettings.LastLocalBlastNOutputDirectory = txtLocal_OutputDirectory.Text;
        }

        private void btnLocal_DatabaseFile_Click(object sender, EventArgs e)
        {
            string filePath = string.Empty;
            if (IODialogHelper.OpenFile(IODialogHelper.DialogPresets.All, this, ref filePath))
            {
                txtLocal_DatabaseFile.Text = filePath;
            }
        }

        private void btnLocal_BlastNExeDirectory_Click(object sender, EventArgs e)
        {
            string directoryPath = string.Empty;
            if (IODialogHelper.FolderBrowse(ref directoryPath))
            {
                txtLocal_BlastNExeDirectory.Text = directoryPath;
            }
        }

        private void btnLocal_OutputDirectory_Click(object sender, EventArgs e)
        {
            string directoryPath = string.Empty;
            if (IODialogHelper.FolderBrowse(ref directoryPath))
            {
                txtLocal_BlastNExeDirectory.Text = directoryPath;
            }
        }
    }
}
