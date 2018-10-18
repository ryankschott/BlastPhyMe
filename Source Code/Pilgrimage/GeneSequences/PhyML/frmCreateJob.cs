using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChangLab.Genes;
using ChangLab.Jobs;
using ChangLab.RecordSets;

namespace Pilgrimage.GeneSequences.PhyML
{
    public partial class frmCreateJob : DialogForm
    {
        internal Activities.RunPhyML Activity { get; private set; }
        private List<Gene> InputGenes { get; set; }
        private PhyMLOptions Options
        {
            get
            {
                PhyMLOptions options = this.uctPhyMLOptions1.Options;
                options.CopyWithoutSupportValues = chkCopyWithoutSupportValues.Checked;
                return options;
            }
        }

        private List<RadioButton> FormRadioButtons { get; set; }
        private bool LoadingDefaults { get; set; }
        
        public frmCreateJob()
        {
            InitializeComponent();
            SetButtonImage(btnRun, "Run");
            SetButtonImage(btnResetDefaults, "Undo");
            SetButtonImage(btnCancel, DialogButtonPresets.Cancel);
        }

        public frmCreateJob(List<Gene> InputGenes) : this()
        {
            this.InputGenes = InputGenes;
            
            // Load up the defaults
            txtSequenceHeaderFormat.Text = Program.DatabaseSettings.PhyML.SequenceHeaderFormat;
            if (string.IsNullOrWhiteSpace(txtSequenceHeaderFormat.Text))
            {
#if !EEB460
                txtSequenceHeaderFormat.Text = "{Definition}";
#else
                txtSequenceHeaderFormat.Text = "{Organism}";
#endif
            }

            if (File.Exists(Program.Settings.PhyMLFullPath)) { txtPhyMLPath.Text = Program.Settings.PhyMLFullPath; }
            else { txtPhyMLPath.Text = string.Empty; }
            // PhyML is not traditionally installed with an installation package, so we can't effectively guess at where it could be.

            if (Directory.Exists(Program.DatabaseSettings.PhyML.WorkingDirectory)) { txtWorkingDirectory.Text = Program.DatabaseSettings.PhyML.WorkingDirectory; }
            else { txtWorkingDirectory.Text = string.Empty; }

            chkKeepOutput.Checked = Program.DatabaseSettings.PhyML.KeepOutput;

            LoadDefaultOptions();
        }

        private void frmCreateJob_Load(object sender, EventArgs e)
        {
            SetFocusOnControl(this.uctPhyMLOptions1.cmbSubstitutionModel);
        }

        private void frmCreateJob_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void LoadDefaultOptions()
        {
            LoadingDefaults = true;

            this.uctPhyMLOptions1.LoadOptions(Program.DatabaseSettings.PhyML.Options);
            chkCopyWithoutSupportValues.Checked = Program.DatabaseSettings.PhyML.Options.CopyWithoutSupportValues;

            LoadingDefaults = false;
        }

        private bool Validation()
        {
            List<ValidationMessage> messages = null;
            this.uctPhyMLOptions1.Validation(out messages);

            if (string.IsNullOrWhiteSpace(txtSequenceHeaderFormat.Text)) { messages.Add(new ValidationMessage("Format for sequence labels has not been provided.", MessageBoxIcon.Error)); }

            if (string.IsNullOrWhiteSpace(txtPhyMLPath.Text)) { messages.Add(new ValidationMessage("Location of phyml.exe has not been provided.", MessageBoxIcon.Error)); }
            else if (!File.Exists(txtPhyMLPath.Text)) { messages.Add(new ValidationMessage("phyml.exe could not be found at the location provided.", MessageBoxIcon.Error)); }
            
            if (string.IsNullOrWhiteSpace(txtWorkingDirectory.Text)) { messages.Add(new ValidationMessage("A directory for processing PhyML output files has not been provided.", MessageBoxIcon.Error)); }
            else if (!Directory.Exists(txtWorkingDirectory.Text)) { messages.Add(new ValidationMessage("Working directory could not be found.", MessageBoxIcon.Error)); }
            
            return ValidationMessage.Prompt(messages, this);
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            if (Validation())
            {
                // Save the selected options as the last-used settings
                Program.DatabaseSettings.PhyML.SequenceHeaderFormat = txtSequenceHeaderFormat.Text;

                if (File.Exists(txtPhyMLPath.Text)) { Program.Settings.PhyMLFullPath = txtPhyMLPath.Text; }
                else { Program.Settings.PhyMLFullPath = string.Empty; }

                if (Directory.Exists(txtWorkingDirectory.Text)) { Program.DatabaseSettings.PhyML.WorkingDirectory = txtWorkingDirectory.Text; }
                else { Program.DatabaseSettings.PhyML.WorkingDirectory = string.Empty; }

                Program.DatabaseSettings.PhyML.KeepOutput = chkKeepOutput.Checked;
                Program.DatabaseSettings.PhyML.Options = this.Options;

                // Set up and run the activity

                this.Activity = new Activities.RunPhyML(this);
                this.Activity.GenerateTree(InputGenes, this.Options, txtSequenceHeaderFormat.Text, txtPhyMLPath.Text, txtWorkingDirectory.Text, chkKeepOutput.Checked, 
                                            Program.Settings.CurrentRecordSet, Program.Settings.GetCurrentSubSet(DataTypes.GeneSequence));
                Program.InProgressActivities.AddActivity(this.Activity);

                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                // The calling form will take the PhyML property and pop a frmJobProgress form for it.
            }
        }

        private void btnSequenceHeaderFormat_Click(object sender, EventArgs e)
        {
            using (frmFormatFieldNames frm = new frmFormatFieldNames(txtSequenceHeaderFormat.Text, this.InputGenes.ElementAt((new Random()).Next(0, this.InputGenes.Count - 1))))
            {
                if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    txtSequenceHeaderFormat.Text = frm.FormatString;
                }
            }
        }

        private void btnBrowsePRANK_Click(object sender, EventArgs e)
        {
            string filePath = txtPhyMLPath.Text;
            if (IODialogHelper.OpenFile("phyml.exe|*.exe", "exe", this, ref filePath))
            {
                txtPhyMLPath.Text = filePath;
            }
        }

        private void btnBrowseWorkingDirectory_Click(object sender, EventArgs e)
        {
            string directory = txtWorkingDirectory.Text;
            if (IODialogHelper.FolderBrowse(ref directory))
            {
                txtWorkingDirectory.Text = directory;
            }
        }

        private void btnResetDefaults_Click(object sender, EventArgs e)
        {
            this.uctPhyMLOptions1.LoadOptions(new PhyMLOptions());
        }
    }
}
