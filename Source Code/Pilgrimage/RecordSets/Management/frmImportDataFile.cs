using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChangLab.Common;
using ChangLab.RecordSets;
using Pilgrimage.Activities;

namespace Pilgrimage.RecordSets
{
    public partial class frmImportDataFile : DialogForm
    {
        private List<RecordSet> AllRecordSets { get; set; }
        private ImportFromDataFile Importer { get; set; }
        public string RecordSetID { get; set; }

        public frmImportDataFile()
        {
            InitializeComponent();
            this.Text = "Import from " + Program.ProductName + " Data File";

            AllRecordSets = RecordSet.List();

            SetButtonImage(btnImport, "Import");
            SetButtonImage(btnCancel, "Cancel");
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            FileInfo file = null;
            if (IODialogHelper.OpenFile(IODialogHelper.DialogPresets.PilgrimageProjectFile, this, ref file))
            {
                txtFilePath.Text = file.FullName;
                txtRecordSetName.Text = file.Name.Replace("." + IODialogHelper.DeriveDefaultExtension(IODialogHelper.DialogPresets.PilgrimageProjectFile), string.Empty);
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (!File.Exists(txtFilePath.Text))
            {
                Utility.ShowMessage(this, "Data file not found.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtRecordSetName.Text))
            {
                Utility.ShowMessage(this, "Project name must be provided.");
                return;
            }
            if (AllRecordSets.Any(rs => rs.Name.ToLower() == txtRecordSetName.Text.ToLower()))
            {
                Utility.ShowMessage(this, "A project with the name \"" + txtRecordSetName.Text + "\" already exists in this database.");
                return;
            }

            Importer = new ImportFromDataFile(this);
            Importer.ActivityCompleted += new Activity.ActivityCompletedEventHandler(import_ActivityCompleted);
            Importer.Import(txtFilePath.Text, txtRecordSetName.Text, Program.Settings.CurrentRecordSet.ID, string.Empty);
        }

        private void import_ActivityCompleted(ActivityCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                using (frmMessageBox msg = new frmMessageBox("Import Failed", ChangLab.Jobs.Job.ImportDataFile_ListProgressMessages(Importer.ImportJobID).Concatenate("\r\n"), MessageBoxButtons.OK, MessageBoxIcon.Warning, true))
                {
                    msg.ShowDialog();
                }
            }
            else if (e.Cancelled) { return; }
            else
            {
                this.RecordSetID = Importer.NewRecordSetID;
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }
    }
}
