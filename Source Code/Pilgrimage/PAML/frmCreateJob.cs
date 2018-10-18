/*
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * // TODO: Stop codon validation 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChangLab.PAML.CodeML;
using ChangLab.Common;
using ChangLab.Jobs;

namespace Pilgrimage.PAML
{
    public partial class frmCreateJob : DialogForm
    {
        internal Activities.RunCodeMLAnalysis Analysis { get; private set; }
        internal List<Tree> Trees
        {
            get
            {
                List<Tree> trees = new List<Tree>();

                trees.AddRange(this.ConfiguredTrees.ToList());
                trees.Select((t, i) => new { Tree = t, Index = i }).ToList().ForEach(t => t.Tree.Rank = (t.Index + 1));

                return trees;
            }
        }
        
        private Dictionary<Button, TextBox> BrowseButtons { get; set; }
        private DataGridViewHelper TreeGridHelper { get; set; }
        private SortableBindingList<Tree> ConfiguredTrees { get; set; }

        public frmCreateJob()
        {
            InitializeComponent();
            
            // Reset the Omega and Kappa values to their defaults.
            Program.Settings.PAML_KappaDefault = null;
            Program.Settings.PAML_OmegaDefault = null;
            
            txtCodeMLPath.Text = Program.Settings.CodeMLFullPath;
            txtWorkingDirectory.Text = Program.DatabaseSettings.PAML.WorkingDirectory;
            chkKeepFolders.Checked = Program.DatabaseSettings.PAML.KeepFolders;
            txtCPUs.Maximum = Environment.ProcessorCount * 4; // This is an entirely arbitrary number, but allows a single-CPU computer (or VM) to run multiple instances.
            txtCPUs.Value = Program.DatabaseSettings.PAML.CodeMLConcurrentProcesses;

            ProcessPriorityClass[] values = (ProcessPriorityClass[])Enum.GetValues(typeof(ProcessPriorityClass));
            Dictionary<ProcessPriorityClass, string> priorities = values.Select(p => new KeyValuePair<ProcessPriorityClass, string>(p, p.SeparateFriendlyCase())).ToDictionary(kv => kv.Key, kv => kv.Value);
            cmbPriority.DataSource = new BindingSource(priorities.OrderBy(kv =>
            {
                switch (kv.Key)
                {
                    case ProcessPriorityClass.Idle: return 0;
                    case ProcessPriorityClass.BelowNormal: return 1;
                    case ProcessPriorityClass.Normal: return 2;
                    case ProcessPriorityClass.AboveNormal: return 3;
                    case ProcessPriorityClass.High: return 4;
                    case ProcessPriorityClass.RealTime: return 5;
                    default: return -1;
                }
            }), null);
            cmbPriority.SelectedValue = Program.DatabaseSettings.PAML.CodeMLPriority;

            BrowseButtons = new Dictionary<Button, TextBox>();
            BrowseButtons.Add(btnBrowse_CodeML, txtCodeMLPath);

            SetButtonImage(btnAdd, "Add");
            SetButtonImage(btnEdit, "Rename");
            SetButtonImage(btnRemove, "Delete");
            SetButtonImage(btnCopy, "Copy");
            SetButtonImage(btnCancel, DialogButtonPresets.Cancel);
            SetButtonImage(btnRun, "Run");

            this.TreeGridHelper = new DataGridViewHelper(this, grdTrees, DataGridViewHelper.DataSourceTypes.Other);
            this.TreeGridHelper.ViewDetails += new DataGridViewHelper.ViewDetailsEventHandler(TreeGridHelper_ViewDetails);

            ConfiguredTrees = new SortableBindingList<Tree>(new List<Tree>());
            this.TreeGridHelper.Loaded = false;
            grdTrees.AutoGenerateColumns = false;
            grdTrees.DataSource = null;
            grdTrees.DataSource = ConfiguredTrees;
            this.TreeGridHelper.Loaded = true;
        }

        /// <summary>
        /// Pre-populate the form with as much configuration from the given Job ID as possible.
        /// </summary>
        public frmCreateJob(string JobID) : this()
        {
            RunTreesAtCodeML job = new RunTreesAtCodeML(JobID);

            this.txtJob.Text = job.Title;

            List<ValidationMessage> messages = new List<ValidationMessage>();
            Tree.ListForJob(job.ID).ForEach(tree => {
                this.ConfiguredTrees.Add(Tree.Copy(tree, false, true));

                // TODO: Stop codon validation

                if (!File.Exists(tree.TreeFilePath)) { messages.Add(new ValidationMessage("Tree file for tree configuration \"" + tree.Title + "\" does not exist.", MessageBoxIcon.Error)); }
                if (!File.Exists(tree.SequencesFilePath)) { messages.Add(new ValidationMessage("Sequences file for tree configuration \"" + tree.Title + "\" does not exist.", MessageBoxIcon.Error)); }
            });

            ValidationMessage.Prompt(messages, this);
        }

        private void frmCreateJob_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Save the relevant defaults
            Program.Settings.CodeMLFullPath = txtCodeMLPath.Text;
            Program.DatabaseSettings.PAML.WorkingDirectory = txtWorkingDirectory.Text;
            Program.DatabaseSettings.PAML.KeepFolders = chkKeepFolders.Checked;
            Program.DatabaseSettings.PAML.CodeMLConcurrentProcesses = Convert.ToInt32(txtCPUs.Value);
            Program.DatabaseSettings.PAML.CodeMLPriority = (ProcessPriorityClass)cmbPriority.SelectedValue;
        }

        private void btnBrowse_CodeML_Click(object sender, EventArgs e)
        {
            string filePath = BrowseButtons[(Button)sender].Text;
            if (IODialogHelper.OpenFile("codeml.exe|*.exe", "exe", this, ref filePath))
            {
                BrowseButtons[(Button)sender].Text = filePath;
            }
        }

        private void btnBrowse_WorkingDirectory_Click(object sender, EventArgs e)
        {
            string directory = txtWorkingDirectory.Text;
            if (IODialogHelper.FolderBrowse(ref directory))
            {
                txtWorkingDirectory.Text = directory;
            }
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            if (Validation())
            {
                // Don't save anything to the database; RunAnalyses() does that.
                this.Analysis = new Activities.RunCodeMLAnalysis(null);
                Analysis.RunAnalyses(
                    this.Trees,
                    Convert.ToInt32(txtCPUs.Value),
                    (ProcessPriorityClass)cmbPriority.SelectedValue,
                    txtCodeMLPath.Text,
                    txtWorkingDirectory.Text,
                    chkKeepFolders.Checked,
                    txtJob.Text,
                    Program.Settings.CurrentRecordSet,
                    Program.Settings.GetCurrentSubSet(ChangLab.RecordSets.DataTypes.CodeMLResult));
                Program.InProgressActivities.AddActivity(Analysis);

                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                // The calling form will take the Analysis property and pop a frmJobProgress form for it.
            }
        }

        private bool Validation()
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();

            // CodeML options
            if (string.IsNullOrWhiteSpace(txtCodeMLPath.Text)) { messages.Add(new ValidationMessage("Location of codeml.exe has not been provided.", MessageBoxIcon.Error)); }
            else if (!File.Exists(txtCodeMLPath.Text)) { messages.Add(new ValidationMessage("codeml.exe could not be found at the location provided.", MessageBoxIcon.Error)); }
            if (string.IsNullOrWhiteSpace(txtWorkingDirectory.Text)) { messages.Add(new ValidationMessage("A directory for processing codeml output files has not been provided.", MessageBoxIcon.Error)); }
            else if (!Directory.Exists(txtWorkingDirectory.Text)) { messages.Add(new ValidationMessage("Working directory could not be found.", MessageBoxIcon.Error)); }
            if (string.IsNullOrWhiteSpace(txtJob.Text)) { messages.Add(new ValidationMessage("Job title cannot be empty.", MessageBoxIcon.Error)); }

            // Check again that all of the tree and sequence files still exist.
            // Unlikely to be a problem for a job being created from scratch, but for a re-generated job from history this could come up.
            this.ConfiguredTrees.ToList().ForEach(tree => {
                if (!File.Exists(tree.TreeFilePath)) { messages.Add(new ValidationMessage("Tree file for tree configuration \"" + tree.Title + "\" does not exist.", MessageBoxIcon.Error)); }
                if (!File.Exists(tree.SequencesFilePath)) { messages.Add(new ValidationMessage("Sequences file for tree configuration \"" + tree.Title + "\" does not exist.", MessageBoxIcon.Error)); }
            });

            return ValidationMessage.Prompt(messages, this);
        }

        #region Trees
        private void TreeGridHelper_ViewDetails(DataGridViewHelper.ViewDetailsEventArgs e)
        {
            EditTree((Tree)e.Row.DataBoundItem, false);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            EditTree(new Tree(Program.DatabaseSettings.PAML.Configuration), true);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (grdTrees.SelectedRows.Count == 0)
            {
                Utility.ShowMessage(this.ParentForm, "Please select a tree to edit.");
                return;
            }
            else
            {
                EditTree((Tree)grdTrees.SelectedRows[0].DataBoundItem, false);
            }
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (grdTrees.SelectedRows.Count == 0)
            {
                Utility.ShowMessage(this.ParentForm, "Please select a tree to copy.");
                return;
            }
            else
            {
                EditTree(((Tree)grdTrees.SelectedRows[0].DataBoundItem).Copy(), true);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (grdTrees.SelectedRows.Count == 0)
            {
                Utility.ShowMessage(this.ParentForm, "Please select a tree to edit.");
                return;
            }
            else
            {
                if (Utility.ShowMessage(this.Parent, "Are you sure you want to remove this tree?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.ConfiguredTrees.Remove((Tree)grdTrees.SelectedRows[0].DataBoundItem);
                }
                grdTrees.Focus();
            }
        }

        private void SelectTree(Tree Tree)
        {
            grdTrees.SelectedRows.Cast<DataGridViewRow>().ToList().ForEach(row => row.Selected = false);
            grdTrees.Rows.Cast<DataGridViewRow>().First(row => (Tree)row.DataBoundItem == Tree).Selected = true;
        }

        private void EditTree(Tree Tree, bool New)
        {
            using (frmEditTreeConfiguration frm = new frmEditTreeConfiguration(Tree, this.Trees))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    if (New)
                    {
                        this.ConfiguredTrees.Add(frm.EditTree);
                        if (string.IsNullOrWhiteSpace(txtJob.Text)) { txtJob.Text = frm.EditTree.Title; }
                    }
                    else 
                    { Tree = frm.EditTree; }

                    SelectTree(Tree);
                }
                grdTrees.Focus();
            }
        }
        #endregion
    }
}