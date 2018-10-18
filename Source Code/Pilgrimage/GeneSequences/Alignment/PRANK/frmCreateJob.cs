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

namespace Pilgrimage.GeneSequences.Alignment.PRANK
{
    public partial class frmCreateJob : DialogForm
    {
        internal Activities.RunAlignment Alignment { get; private set; }
        private List<Gene> InputGenes { get; set; }
        private PRANKOptions Options
        {
            get
            {
                PRANKOptions options = new PRANKOptions()
                {
                    F = chkF.Checked,
                    Keep = chkKeep.Checked,
                    Codon = chkCodon.Checked,
                    Translate = chkTranslate.Checked,
                    MTTranslate = chkMTTranslate.Checked,
                    ShowTree = chkShowTree.Checked,
                    ShowAnc = chkShowAnc.Checked,
                    ShowEvents = chkShowEvents.Checked,
                    Iterations = Convert.ToInt32(numIterations.Value)
                };
                if (File.Exists(txtGuideTreePath.Text)) { options.GuideTreeFile = txtGuideTreePath.Text; }

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
            if (File.Exists(Program.Settings.PRANKFullPath)) { txtPRANKPath.Text = Program.Settings.PRANKFullPath; }
            else { txtPRANKPath.Text = string.Empty; }
            if (string.IsNullOrWhiteSpace(txtPRANKPath.Text))
            {
                // Try and find PRANK if it's not been set or has gone missing.
                string testPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "PRANK\\bin\\prank.exe");
                if (File.Exists(testPath)) { txtPRANKPath.Text = testPath; }
                else
                {
                    testPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "PRANK\\bin\\prank.exe");
                    if (File.Exists(testPath)) { txtPRANKPath.Text = testPath; }
                    else
                    {
                        testPath = "C:\\Programs\\PRANK\\bin\\prank.exe";
                        if (File.Exists(testPath)) { txtPRANKPath.Text = testPath; }
                    }
                }
            }

            if (Directory.Exists(Program.DatabaseSettings.PRANK.WorkingDirectory)) { txtWorkingDirectory.Text = Program.DatabaseSettings.PRANK.WorkingDirectory; }
            else { txtWorkingDirectory.Text = string.Empty; }

            chkKeepOutputFiles.Checked = Program.DatabaseSettings.PRANK.KeepOutputFiles;
            
            PRANKOptions defaultOptions = Program.DatabaseSettings.PRANK.Options;
            chkF.Checked = defaultOptions.F;
            chkKeep.Checked = defaultOptions.Keep;
            chkCodon.Checked = defaultOptions.Codon;
            chkTranslate.Checked = defaultOptions.Translate;
            chkMTTranslate.Checked = defaultOptions.MTTranslate;
            chkShowTree.Checked = defaultOptions.ShowTree;
            chkShowAnc.Checked = defaultOptions.ShowAnc;
            chkShowEvents.Checked = defaultOptions.ShowEvents;
            numIterations.Value = defaultOptions.Iterations;

            if (File.Exists(defaultOptions.GuideTreeFile)) { txtGuideTreePath.Text = defaultOptions.GuideTreeFile; }
            else { txtGuideTreePath.Text = string.Empty; }
        }

        private void frmCreateJob_Load(object sender, EventArgs e)
        {
            SetFocusOnControl(txtWorkingDirectory);
        }

        private void frmCreateJob_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (File.Exists(txtPRANKPath.Text)) { Program.Settings.PRANKFullPath = txtPRANKPath.Text; }
            else { Program.Settings.PRANKFullPath = string.Empty; }

            if (Directory.Exists(txtWorkingDirectory.Text)) { Program.DatabaseSettings.PRANK.WorkingDirectory = txtWorkingDirectory.Text; }
            else { Program.DatabaseSettings.PRANK.WorkingDirectory = string.Empty; }

            Program.DatabaseSettings.PRANK.KeepOutputFiles = chkKeepOutputFiles.Checked;
            Program.DatabaseSettings.PRANK.Options = this.Options;
        }

        private bool Validation()
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();

            if (string.IsNullOrWhiteSpace(txtPRANKPath.Text)) { messages.Add(new ValidationMessage("Location of prank.exe has not been provided.", MessageBoxIcon.Error)); }
            else if (!File.Exists(txtPRANKPath.Text)) { messages.Add(new ValidationMessage("prank.exe could not be found at the location provided.", MessageBoxIcon.Error)); }
            
            if (string.IsNullOrWhiteSpace(txtWorkingDirectory.Text)) { messages.Add(new ValidationMessage("A directory for processing PRANK output files has not been provided.", MessageBoxIcon.Error)); }
            else if (!Directory.Exists(txtWorkingDirectory.Text)) { messages.Add(new ValidationMessage("Working directory could not be found.", MessageBoxIcon.Error)); }
            
            // Guide tree is allowed to be empty
            if (!string.IsNullOrWhiteSpace(txtGuideTreePath.Text) && !File.Exists(txtGuideTreePath.Text)) { messages.Add(new ValidationMessage("Guide tree file could not be found at the location provided.", MessageBoxIcon.Error)); }

            return ValidationMessage.Prompt(messages, this);
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            if (Validation())
            {
                bool trimIfNecessary = false;

                // Keeping this out of Validation() above because it's not part of the usual form fields validation.
                int needTrimming = InputGenes.Count(g => 
                    {
                        return (Convert.ToDouble(g.Nucleotides.Length) % 3.0) != 0.0;
                    });
                if (needTrimming != 0)
                {
                    string message = "{0} sequence{1} exact multiple{2} of codons and must be trimmed in order for PRANK to align {3} using the codon model."
                                    + "\r\n\r\n"
                                    + "Select \"Yes\" to automatically trim the end{4} of the sequence{4} by one or two codons as "
                                    + "necessary, including removing the STOP codon.\r\n\r\n"
                                    + "Select \"No\" to run PRANK without the codon model option selected.";

                    if (needTrimming > 1)
                    { message = string.Format(message, needTrimming, "s are not", "s", "them", "s"); }
                    else
                    { message = string.Format(message, needTrimming, " is not an", "", "it", ""); }

                    DialogResult result = Utility.ShowMessage(this.OwnerForm, message, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                    switch (result)
                    {
                        case System.Windows.Forms.DialogResult.Cancel: return;
                        case System.Windows.Forms.DialogResult.No:
                            chkCodon.Checked = false;
                            break;
                        case System.Windows.Forms.DialogResult.Yes:
                            trimIfNecessary = true;
                            break;
                    }
                }

                this.Alignment = new Activities.RunAlignment(this, JobTargets.PRANK);
                AlignSequencesWithPRANK job = new AlignSequencesWithPRANK(Options, trimIfNecessary, txtPRANKPath.Text, chkKeepOutputFiles.Checked, Program.Settings.GetCurrentSubSet(DataTypes.GeneSequence).ID);
                this.Alignment.RunAlignmentJob(job, InputGenes, txtWorkingDirectory.Text, Program.Settings.CurrentRecordSet, Program.Settings.GetCurrentSubSet(DataTypes.GeneSequence));
                Program.InProgressActivities.AddActivity(this.Alignment);

                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                // The calling form will take the PRANK property and pop a frmJobProgress form for it.
            }
        }

        private void btnBrowseGuideTree_Click(object sender, EventArgs e)
        {
            string filePath = txtGuideTreePath.Text;
            if (IODialogHelper.OpenFile("Tree file|*.tre", "tre", this, ref filePath))
            {
                txtGuideTreePath.Text = filePath;
            }
        }

        private void btnBrowsePRANK_Click(object sender, EventArgs e)
        {
            string filePath = txtPRANKPath.Text;
            if (IODialogHelper.OpenFile("prank.exe|*.exe", "exe", this, ref filePath))
            {
                txtPRANKPath.Text = filePath;
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
