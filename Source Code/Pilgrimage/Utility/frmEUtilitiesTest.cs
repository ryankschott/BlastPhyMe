using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChangLab.Common;
using ChangLab.NCBI;

namespace Pilgrimage.Utilities
{
    public partial class frmEUtilitiesTest : Form
    {
        private string FilePath { get; set; }
        private frmProgress ProgressForm { get; set; }

        public class EUtilitiesTestParameters
        {
            public string URL { get; set; }
            public string EFetchID { get; set; }
            public EUtilities.Databases EFetchDatabase { get; set; }

            public TestModes Mode { get; set; }

            public enum TestModes
            {
                URL = 1,
                EFetch = 2
            }
        }

        public frmEUtilitiesTest()
        {
            InitializeComponent();
        }

        private void frmEUtilitiesTest_Load(object sender, EventArgs e)
        {
            cmbDatabase.DataSource = new BindingSource(Enum.GetNames(typeof(EUtilities.Databases)), null);
        }

        private void txtEFetchID_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtEFetchID.Text) && !rbEFetch.Checked) { rbEFetch.Checked = true; }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (rbEFetch.Checked && string.IsNullOrEmpty(txtEFetchID.Text))
            {
                Utility.ShowMessage(this, "EFetch ID is empty.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
                //int id = 0;
                //if (!int.TryParse(txtEFetchID.Text, out id))
                //{
                //    Utility.ShowMessage(this, "EFetch ID is not a number.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}
            }

            System.IO.FileInfo file = null;
            if (IODialogHelper.SaveFile(IODialogHelper.DialogPresets.XML, string.Empty, this, ref file))
            {
                this.FilePath = file.FullName;

                using (ProgressForm = new frmProgress("Downloading...", new frmProgress.ProgressOptions() { UseNeverEndingTimer = true, AllowCancellation = false }))
                {
                    bwRequest.RunWorkerAsync(
                        new EUtilitiesTestParameters()
                        {
                            URL = txtURL.Text,
                            EFetchID = txtEFetchID.Text,
                            EFetchDatabase = (EUtilities.Databases)Enum.Parse(typeof(EUtilities.Databases), (string)cmbDatabase.SelectedItem),
                            Mode = (rbURL.Checked ? EUtilitiesTestParameters.TestModes.URL : EUtilitiesTestParameters.TestModes.EFetch)
                        });
                    ProgressForm.ShowDialog(this);
                }
            }
        }

        private void bwRequest_DoWork(object sender, DoWorkEventArgs e)
        {
            EUtilitiesTestParameters args = (EUtilitiesTestParameters)e.Argument;

            string url = string.Empty;
            switch (args.Mode)
            {
                case EUtilitiesTestParameters.TestModes.URL:
                    url = args.URL;
                    break;
                case EUtilitiesTestParameters.TestModes.EFetch:
                    url = EUtilities.GetUrl(EUtilities.Services.EFetch, args.EFetchDatabase, false, null)
                        + "&rettype=gbc&id=" + args.EFetchID;
                    break;
            }

            if (string.IsNullOrEmpty(url)) { throw new ArgumentException("Test mode \"" + args.Mode.ToString() + "\" not configured.", "Mode"); }

            System.Xml.XmlDocument doc = XMLWebRequest.RequestDocument(url);
            doc.Save(this.FilePath);

            Program.Settings.LastWorkingDirectory = (new System.IO.FileInfo(this.FilePath)).Directory.FullName;
        }

        private void bwRequest_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ProgressForm.DialogResult = System.Windows.Forms.DialogResult.OK;
            ProgressForm.Close();

            if (e.Error != null)
            {
                Utility.ShowErrorMessage(this, e.Error);
            }
            else
            {
                Clipboard.SetText(this.FilePath);
            }
        }
    }
}
