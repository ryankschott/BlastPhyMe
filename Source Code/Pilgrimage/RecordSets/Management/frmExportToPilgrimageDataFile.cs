using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChangLab.Common;
using ChangLab.Jobs;
using ChangLab.RecordSets;

namespace Pilgrimage.RecordSets
{
    public partial class frmExportToPilgrimageDataFile : DialogForm
    {
        private List<string> SelectedGeneIDs { get; set; }
        private List<int> SelectedResultIDs { get; set; }
        private DataTypes CurrentSubSetDataType { get; set; }
        private bool ExportAsProjectFile { get { return (this.SelectedGeneIDs == null && this.SelectedResultIDs == null); } }

        public frmExportToPilgrimageDataFile(List<string> SelectedGeneIDs, List<int> SelectedResultIDs, DataTypes CurrentSubSetDataType)
        {
            InitializeComponent();
            this.Text = "Export to " + Program.ProductName + " Data File";

            SetButtonImage(btnExport, "Export");
            SetButtonImage(btnCancel, DialogButtonPresets.Cancel);

            if (SelectedGeneIDs != null)
            {
                this.SelectedGeneIDs = SelectedGeneIDs;

                lblGeneSequenceSubSets.Parent.Controls.Remove(lblGeneSequenceSubSets);
                chkGeneSequenceToggleSubSets.Parent.Controls.Remove(chkGeneSequenceToggleSubSets);
                tvGeneSequenceSubSets.Parent.Controls.Remove(tvGeneSequenceSubSets);

                tbForm.TabPages.Remove(pgSelectionAnalyses);

                this.MinimumSize = new Size(400, 230);
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
                this.Size = this.MinimumSize;
            }
            else if (SelectedResultIDs != null)
            {
                this.SelectedResultIDs = SelectedResultIDs;

                lblSelectionAnalysesSubSets.Parent.Controls.Remove(lblSelectionAnalysesSubSets);
                chkSelectionAnalysesToggleSubSets.Parent.Controls.Remove(chkSelectionAnalysesToggleSubSets);
                tvSelectionAnalysesSubSets.Parent.Controls.Remove(tvSelectionAnalysesSubSets);

                tbForm.TabPages.Remove(pgGeneSequences);

                this.MinimumSize = new Size(400, 120);
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
                this.Size = this.MinimumSize;
            }
            this.CurrentSubSetDataType = CurrentSubSetDataType;

            chkGeneSequencesJobHistories_CheckedChanged(chkGeneSequencesJobHistories, EventArgs.Empty);
        }

        private void frmExportToPilgrimageDataFile_Load(object sender, EventArgs e)
        {
            if (this.SelectedGeneIDs == null)
            {
                foreach (SubSet sub in Program.Settings.CurrentRecordSet.ListSubSets(DataTypes.GeneSequence).OrderBy(sub => sub.Name))
                {
                    tvGeneSequenceSubSets.Nodes.Add(new TreeNode(sub.Name) { ImageKey = "SubSet", SelectedImageKey = "SubSet", Tag = sub.ID, Checked = sub.Open });
                }
            }

            if (this.SelectedResultIDs == null)
            {
                foreach (SubSet sub in Program.Settings.CurrentRecordSet.ListSubSets(DataTypes.CodeMLResult).OrderBy(sub => sub.Name))
                {
                    tvSelectionAnalysesSubSets.Nodes.Add(new TreeNode(sub.Name) { ImageKey = "SubSet", SelectedImageKey = "SubSet", Tag = sub.ID, Checked = sub.Open });
                }
            }

            foreach (var target in JobTargetCollection.ListAll().Where(t => (new JobTargets[] { JobTargets.BLASTN_NCBI, JobTargets.PRANK, JobTargets.MUSCLE, JobTargets.PhyML }).Contains(t.Key)))
	        {
                tvJobTargets.Nodes.Add(new TreeNode(target.Name) { Tag = target.ID, Checked = true });
	        }
        }

        private void chkGeneSequencesJobHistories_CheckedChanged(object sender, EventArgs e)
        {
            tvJobTargets.Enabled = chkGeneSequencesJobHistories.Checked;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            System.IO.FileInfo file = null;
            string fileName = Program.Settings.CurrentRecordSet.Name + (!this.ExportAsProjectFile ? " - " + Program.Settings.GetCurrentSubSet(this.CurrentSubSetDataType).Name : string.Empty);
            if (IODialogHelper.SaveFile((this.ExportAsProjectFile ? IODialogHelper.DialogPresets.PilgrimageProjectFile : IODialogHelper.DialogPresets.PilgrimageDataFile), fileName, this, ref file))
            {
                List<int> jobTargetIDs = new List<int>();
                if (chkGeneSequencesJobHistories.Checked)
                {
                    jobTargetIDs.AddRange(tvJobTargets.Nodes.Cast<TreeNode>().Where(tn => tn.Checked).Select(tn => (int)tn.Tag));
                }
                if (chkSelectionAnalysesJobHistories.Checked)
                {
                    jobTargetIDs.Add(JobTargetCollection.IDByKey(JobTargets.CodeML));
                }

                Activities.ExportToDataFile export = new Activities.ExportToDataFile(this);
                export.ActivityCompleted += new Activities.Activity.ActivityCompletedEventHandler(export_ActivityCompleted);
                
                if (this.SelectedGeneIDs == null && this.SelectedResultIDs == null)
                {
                    List<string> subSetIDs = new List<string>();
                    subSetIDs.AddRange(tvGeneSequenceSubSets.Nodes.Cast<TreeNode>().Where(tn => tn.Checked).Select(tn => (string)tn.Tag));
                    subSetIDs.AddRange(tvSelectionAnalysesSubSets.Nodes.Cast<TreeNode>().Where(tn => tn.Checked).Select(tn => (string)tn.Tag));

                    export.Export(Program.Settings.CurrentRecordSet.ID, file.FullName,
                        new IOPilgrimageDataFile.ExportOptions()
                        {
                            JobTargetIDs = jobTargetIDs,
                            IncludeAlignedSequences = chkIncludeAlignedSequences.Checked,
                            SubSetIDs = subSetIDs
                        });
                }
                else
                {
                    export.Export(Program.Settings.GetCurrentSubSet(this.CurrentSubSetDataType).ID, this.SelectedGeneIDs, this.SelectedResultIDs, file.FullName,
                        new IOPilgrimageDataFile.ExportOptions()
                        {
                            JobTargetIDs = jobTargetIDs,
                            IncludeAlignedSequences = chkIncludeAlignedSequences.Checked,
                            SubSetIDs = null
                        });
                }
            }
        }

        private void export_ActivityCompleted(Activities.ActivityCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Utility.ShowErrorMessage(this, e.Error);
            }
            else if (e.Cancelled) { return; }
            else
            {
                Program.Settings.LastWorkingDirectory = (new System.IO.FileInfo((string)e.Result)).Directory.FullName;
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }

        private void tvSubSets_AfterCheck(object sender, TreeViewEventArgs e)
        {
            CheckBox chkToggle = (sender == tvGeneSequenceSubSets ? chkGeneSequenceToggleSubSets : chkSelectionAnalysesToggleSubSets);
            TreeView tvSubSets = (TreeView)sender;

            chkToggle.CheckedChanged -= chkToggleSubSets_CheckedChanged;
            chkToggle.Checked = tvSubSets.Nodes.Cast<TreeNode>().All(tn => tn.Checked);
            UpdateToggleCheckBoxText(chkToggle);
            chkToggle.CheckedChanged += new EventHandler(chkToggleSubSets_CheckedChanged);
        }

        private void chkToggleSubSets_CheckedChanged(object sender, EventArgs e)
        {
            TreeView tvSubSets = (sender == chkGeneSequenceToggleSubSets ? tvGeneSequenceSubSets : tvSelectionAnalysesSubSets);
            CheckBox chkToggle = (CheckBox)sender;
            UpdateToggleCheckBoxText(chkToggle);

            tvSubSets.AfterCheck -= tvSubSets_AfterCheck;
            foreach (TreeNode node in tvSubSets.Nodes) { node.Checked = chkToggle.Checked; }
            tvSubSets.AfterCheck += new TreeViewEventHandler(tvSubSets_AfterCheck);
        }

        private void UpdateToggleCheckBoxText(CheckBox chkToggle)
        {
            chkToggle.Text = (chkToggle.Checked ? "Deselect All" : "Select All");
        }
    }
}
