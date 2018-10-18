using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChangLab.Jobs;

namespace Pilgrimage.GeneSequences.PhyML
{
    public partial class frmPhyMLResults : DialogForm
    {
        private GenerateTreeWithPhyML Job { get; set; }

        public frmPhyMLResults()
        {
            InitializeComponent();
        }

        public frmPhyMLResults(GenerateTreeWithPhyML Job) : this()
        {
            this.Job = Job;

            this.SetButtonImage(btnClose_Output, DialogButtonPresets.Close);
            this.SetButtonImage(btnClose_Options, DialogButtonPresets.Close);
            this.SetButtonImage(btnSave, DialogButtonPresets.Save);

            Pilgrimage.Activities.RunPhyML phymlInMemory = Program.InProgressActivities.GetActivityByJobID<Pilgrimage.Activities.RunPhyML>(Job.ID);
            if (phymlInMemory != null) // We no longer need to track it because it has completed. 
            { Program.InProgressActivities.RemoveActivity(phymlInMemory); }
            
            if (Job.Output != null) { this.txtOutput.Lines = Job.Output.Split(new string[] { "\r\n" }, StringSplitOptions.None); }
            this.lnkWorkingDirectory.Text = Job.JobDirectory;

            if (!string.IsNullOrWhiteSpace(Job.OriginalTreeFilePath))
            { lnkOriginalTreeFile.Tag = Job.OriginalTreeFilePath; }
            else
            { lnkOriginalTreeFile.Parent.Controls.Remove(lnkOriginalTreeFile); }

            if (!string.IsNullOrWhiteSpace(Job.UnlabeledTreeFilePath))
            { lnkUnlabeledTreeFile.Tag = Job.UnlabeledTreeFilePath; }
            else
            { lnkUnlabeledTreeFile.Parent.Controls.Remove(lnkUnlabeledTreeFile); }
            
            this.uctPhyMLOptions1.LoadOptions(Job.Options);
            this.uctPhyMLOptions1.Enabled = false;

#if EEB460
            if (lnkOriginalTreeFile.Parent != null)
            { lnkOriginalTreeFile.Parent.Controls.Remove(lnkOriginalTreeFile); }

            lnkUnlabeledTreeFile.Text = "Open tree in viewer";
#endif
        }

        public frmPhyMLResults(string JobID) : this(new GenerateTreeWithPhyML(JobID)) { }

        private void lnkWorkingDirectory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (System.IO.Directory.Exists(lnkWorkingDirectory.Text))
            {
                System.Diagnostics.Process.Start(lnkWorkingDirectory.Text);
            }
            else
            {
                Utility.ShowMessage(this.Owner, "The output directory could not be found on this computer.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void lnkOpenTreeFile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.IO.FileInfo file = new System.IO.FileInfo((string)((LinkLabel)sender).Tag);
            if (file.Exists)
            {
                // This is more than a little hackish; what we need is to offer the user their preference of viewer and accomodate that cleanly.
#if EEB460
                if (System.IO.File.Exists(Program.Settings.FigTreeFullPath)) // One way or another, we found it.
                {
                    OpenFileInFigTree(file.FullName);
                }
                else
                {
                    if (Utility.ShowMessage(this, "FigTree could not be found on this computer.  Would you like to browse for the application?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                    {
                        string figTreeFullPath = string.Empty;
                        if (IODialogHelper.OpenFile("FigTree|*.exe", "exe", this, ref figTreeFullPath, false, false))
                        {
                            Program.Settings.FigTreeFullPath = figTreeFullPath;
                        }
                    }

                    if (System.IO.File.Exists(Program.Settings.FigTreeFullPath)) // One way or another, we found it.
                    {
                        OpenFileInFigTree(file.FullName);
                    }
                }
#else
                if (System.IO.File.Exists(Program.Settings.TreeViewFullPath)) // One way or another, we found it.
                {
                    OpenFileInTreeView(file.FullName);
                }
                else
                {
                    if (Utility.ShowMessage(this, "TreeView could not be found on this computer.  Would you like to browse for the application?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                    {
                        string treeViewFullPath = string.Empty;
                        if (IODialogHelper.OpenFile("TreeView|*.exe", "exe", this, ref treeViewFullPath, false, false))
                        {
                            Program.Settings.TreeViewFullPath = treeViewFullPath;
                        }
                    }

                    if (System.IO.File.Exists(Program.Settings.TreeViewFullPath)) // One way or another, we found it.
                    {
                        OpenFileInTreeView(file.FullName);
                    }
                }
#endif
            }
            else
            {
                Utility.ShowMessage(this.Owner, "The tree file could not be found on this computer.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void OpenFileInTreeView(string FilePath)
        {
            System.IO.FileInfo treeViewExe = new System.IO.FileInfo(Program.Settings.TreeViewFullPath);

            if (System.Diagnostics.Process.GetProcesses().Any(p => p.ProcessName == treeViewExe.Name.Substring(0, treeViewExe.Name.Length - treeViewExe.Extension.Length)))
            {
                Utility.ShowMessage(this.Owner, "TreeView is already running and only one instance of TreeView can be open at any time.  Please close TreeView and then select this link to open the tree file again.", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("\"" + Program.Settings.TreeViewFullPath + "\"", "\"" + FilePath + "\""));
            }
        }

        private void OpenFileInFigTree(string FilePath)
        {
            System.IO.FileInfo figTreeExe = new System.IO.FileInfo(Program.Settings.FigTreeFullPath);

            //if (System.Diagnostics.Process.GetProcesses().Any(p => p.ProcessName == figTreeExe.Name.Substring(0, figTreeExe.Name.Length - figTreeExe.Extension.Length)))
            //{
            //    Utility.ShowMessage(this.Owner, "FigTree is already running and only one instance of FigTree can be open at any time.  Please close FigTree and then select this link to open the tree file again.", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
            //else
            //{
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("\"" + Program.Settings.FigTreeFullPath + "\"", "\"" + FilePath + "\""));
            //}
        }

        private void tbForm_Selected(object sender, TabControlEventArgs e)
        {
            if (e.TabPage == pgOutput)
            { this.CancelButton = btnClose_Output; }
            else if (e.TabPage == pgOptions)
            { this.CancelButton = btnClose_Options; }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string filePath = string.Empty;
            if (IODialogHelper.SaveFile(IODialogHelper.DialogPresets.Text, "output.txt", this, ref filePath))
            {
                System.IO.File.WriteAllLines(filePath, txtOutput.Lines);
            }
        }
    }
}
