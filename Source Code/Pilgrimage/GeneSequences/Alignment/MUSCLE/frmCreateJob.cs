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

namespace Pilgrimage.GeneSequences.Alignment.MUSCLE
{
    public partial class frmCreateJob : DialogForm
    {
        internal Activities.RunAlignment Alignment { get; private set; }
        private List<Gene> InputGenes { get; set; }
        private MUSCLEOptions Options
        {
            get
            {
                MUSCLEOptions options = new MUSCLEOptions()
                {
                    FindDiagonals = chkFindDiagonals.Checked,
                    MaximumIterations = Convert.ToInt32(numMaximumIterations.Value),
                    MaximumHours = Convert.ToInt32(numMaximumHours.Value)
                };

                return options;
            }
        }
        
        public frmCreateJob() 
        {
            InitializeComponent();
            SetButtonImage(btnRun, "Run");
            SetButtonImage(btnCancel, DialogButtonPresets.Cancel);
        }

        public frmCreateJob(List<Gene> InputGenes) : this()
        {
            this.InputGenes = InputGenes;

            // Load up the defaults
            if (File.Exists(Program.Settings.MUSCLEFullPath)) { txtExecutablePath.Text = Program.Settings.MUSCLEFullPath; }
            else { txtExecutablePath.Text = string.Empty; }

            if (Directory.Exists(Program.DatabaseSettings.MUSCLE.WorkingDirectory)) { txtWorkingDirectory.Text = Program.DatabaseSettings.MUSCLE.WorkingDirectory; }
            else { txtWorkingDirectory.Text = string.Empty; }

            chkKeepOutputFiles.Checked = Program.DatabaseSettings.MUSCLE.KeepOutputFiles;

            MUSCLEOptions defaultOptions = Program.DatabaseSettings.MUSCLE.Options;
            chkFindDiagonals.Checked = defaultOptions.FindDiagonals;
            numMaximumIterations.Value = defaultOptions.MaximumIterations;
            numMaximumHours.Value = defaultOptions.MaximumHours;
        }

        private void frmCreateJob_Load(object sender, EventArgs e)
        {
            SetFocusOnControl(txtWorkingDirectory);
        }

        private void frmCreateJob_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (File.Exists(txtExecutablePath.Text)) { Program.Settings.MUSCLEFullPath = txtExecutablePath.Text; }
            else { Program.Settings.MUSCLEFullPath = string.Empty; }

            if (Directory.Exists(txtWorkingDirectory.Text)) { Program.DatabaseSettings.MUSCLE.WorkingDirectory = txtWorkingDirectory.Text; }
            else { Program.DatabaseSettings.MUSCLE.WorkingDirectory = string.Empty; }

            Program.DatabaseSettings.MUSCLE.KeepOutputFiles = chkKeepOutputFiles.Checked;
            Program.DatabaseSettings.MUSCLE.Options = this.Options;
        }

        private bool Validation()
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();

            if (string.IsNullOrWhiteSpace(txtExecutablePath.Text)) { messages.Add(new ValidationMessage("Location of muscle.exe has not been provided.", MessageBoxIcon.Error)); }
            else if (!File.Exists(txtExecutablePath.Text)) { messages.Add(new ValidationMessage("muscle.exe could not be found at the location provided.", MessageBoxIcon.Error)); }
            
            if (string.IsNullOrWhiteSpace(txtWorkingDirectory.Text)) { messages.Add(new ValidationMessage("A directory for processing MUSCLE output files has not been provided.", MessageBoxIcon.Error)); }
            else if (!Directory.Exists(txtWorkingDirectory.Text)) { messages.Add(new ValidationMessage("Working directory could not be found.", MessageBoxIcon.Error)); }
            
            return ValidationMessage.Prompt(messages, this);
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            if (Validation())
            {
                this.Alignment = new Activities.RunAlignment(this, JobTargets.MUSCLE);
                AlignSequencesWithMUSCLE job = new AlignSequencesWithMUSCLE(Options, txtExecutablePath.Text, chkKeepOutputFiles.Checked, Program.Settings.GetCurrentSubSet(DataTypes.GeneSequence).ID);
                this.Alignment.RunAlignmentJob(job, InputGenes, txtWorkingDirectory.Text, Program.Settings.CurrentRecordSet, Program.Settings.GetCurrentSubSet(DataTypes.GeneSequence));
                Program.InProgressActivities.AddActivity(this.Alignment);

                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                // The calling form will take the Alignment property and pop a frmJobProgress form for it.
            }
        }
        
        private void btnBrowseExe_Click(object sender, EventArgs e)
        {
            string filePath = txtExecutablePath.Text;
            if (IODialogHelper.OpenFile("muscle.exe|*.exe", "exe", this, ref filePath))
            {
                txtExecutablePath.Text = filePath;
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
    }
}
