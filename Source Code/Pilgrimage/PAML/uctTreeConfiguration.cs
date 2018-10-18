using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChangLab.Common;
using ChangLab.PAML;
using ChangLab.PAML.CodeML;

namespace Pilgrimage.PAML
{
    public partial class uctTreeConfiguration : UserControl
    {
        internal Tree Tree { get; private set; }
        private List<Tree> TreeCollection { get; set; }
        private Dictionary<Button, TextBox> BrowseButtons { get; set; }

        private bool ReadOnly { get; set; }

        public uctTreeConfiguration()
        {
            InitializeComponent();

            BrowseButtons = new Dictionary<Button, TextBox>();
            BrowseButtons.Add(btnBrowse_TreeFile, txtTreeFile);
            BrowseButtons.Add(btnBrowse_SequencesFile, txtSequencesFile);
        }

        internal void Initialize(Tree Tree, List<Tree> TreeCollection, bool ShowButtons = true)
        {
            this.Tree = Tree;
            this.TreeCollection = TreeCollection;

            ((DialogForm)this.ParentForm).SetButtonImage(btnAdd, "Add");
            ((DialogForm)this.ParentForm).SetButtonImage(btnEdit, "Rename");
            ((DialogForm)this.ParentForm).SetButtonImage(btnCopy, "Copy");
            ((DialogForm)this.ParentForm).SetButtonImage(btnRemove, "Delete");

            txtTreeFile.Text = this.Tree.TreeFilePath;
            txtSequencesFile.Text = this.Tree.SequencesFilePath;
            txtTitle.Text = this.Tree.Title;
            lnkAdditionalOptions.Tag = this.Tree.Configuration;

            this.ReadOnly = !ShowButtons;
            if (!ShowButtons)
            {
                btnBrowse_TreeFile.Parent.Controls.Remove(btnBrowse_TreeFile);
                tblForm.SetColumnSpan(txtTreeFile, 3);
                btnBrowse_SequencesFile.Parent.Controls.Remove(btnBrowse_SequencesFile);
                tblForm.SetColumnSpan(txtSequencesFile, 3);
                tblControlButtons.Parent.Controls.Remove(tblControlButtons);

                lnkCustomControlFile.Parent.Controls.Remove(lnkCustomControlFile);
                tblForm.SetColumnSpan(lnkAdditionalOptions, 4);
            }

            uctAnalysisConfigurations1.Initialize(this.Tree.AnalysisConfigurations);
        }

        internal Tree GetTree()
        {
            this.Tree.TreeFilePath = txtTreeFile.Text;
            this.Tree.SequencesFilePath = txtSequencesFile.Text;

            try
            {
                string[] pieces = System.IO.File.ReadLines(txtSequencesFile.Text).First().SplitByEmptySpace();
                this.Tree.SequenceCount = int.Parse(pieces[0]);
                this.Tree.SequenceLength = int.Parse(pieces[1]);
            }
            catch { throw new Exception("Unable to parse sequences file."); }
            
            this.Tree.Title = txtTitle.Text;
            this.Tree.Configuration = (ControlConfiguration)lnkAdditionalOptions.Tag;
            this.Tree.AnalysisConfigurations.Clear();
            this.Tree.AnalysisConfigurations.AddRange(uctAnalysisConfigurations1.Configurations);
            return this.Tree;
        }

        internal bool Validate(out List<ValidationMessage> Messages, ref Tree ConfiguredTree, string MessageModifier = "", bool ConfigurationsRequired = false)
        {
            Messages = new List<ValidationMessage>();
            if (!string.IsNullOrWhiteSpace(txtSequencesFile.Text))
            {
                try
                { ConfiguredTree = GetTree(); }
                catch (Exception ex)
                {
                    Messages.Add(new ValidationMessage(ex.Message, MessageBoxIcon.Error));
                    ConfiguredTree = null;
                }
            }
            else { ConfiguredTree = null; }

            if (string.IsNullOrWhiteSpace(txtTitle.Text)) { Messages.Add(new ValidationMessage("Title" + MessageModifier + " cannot be empty.", MessageBoxIcon.Error)); }
            else if (TreeCollection != null && TreeCollection.Any(t => t.Title.ToLower() == txtTitle.Text.ToLower() && t != this.Tree)) 
            { Messages.Add(new ValidationMessage("Titles for individual analyses must be unique.", MessageBoxIcon.Error)); }
            if (string.IsNullOrWhiteSpace(txtTreeFile.Text)) { Messages.Add(new ValidationMessage("Tree file" + MessageModifier + " has not been provided.", MessageBoxIcon.Error)); }
            else if (!System.IO.File.Exists(txtTreeFile.Text)) { Messages.Add(new ValidationMessage("Tree file" + MessageModifier + " could not be found.", MessageBoxIcon.Error)); }
            if (string.IsNullOrWhiteSpace(txtSequencesFile.Text)) { Messages.Add(new ValidationMessage("Sequences file" + MessageModifier + " has not been provided.", MessageBoxIcon.Error)); }
            else if (!System.IO.File.Exists(txtSequencesFile.Text)) { Messages.Add(new ValidationMessage("Sequences file" + MessageModifier + " could not be found.", MessageBoxIcon.Error)); }
            if (uctAnalysisConfigurations1.Configurations.Count == 0) { Messages.Add(new ValidationMessage("No analyses have been configured" + MessageModifier + ".", ConfigurationsRequired ? MessageBoxIcon.Error : MessageBoxIcon.Warning)); }

            return Messages.Count == 0;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            uctAnalysisConfigurations1.Add();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            uctAnalysisConfigurations1.Edit();
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            uctAnalysisConfigurations1.Copy();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            uctAnalysisConfigurations1.Remove();
        }

        private void btnBrowse_File_Click(object sender, EventArgs e)
        {
            System.IO.FileInfo fileInfo = null;
            string filePath = BrowseButtons[(Button)sender].Text;
            bool openedFile = false;
            if (sender == btnBrowse_TreeFile)
            {
                openedFile = IODialogHelper.OpenFile("Tree File (*.tre,*.phy)|*.tre;*.phy", "tre", filePath, this, ref fileInfo, true);
                if (openedFile && string.IsNullOrWhiteSpace(txtTitle.Text))
                { txtTitle.Text = fileInfo.Name.Replace(fileInfo.Extension, string.Empty); }
            }
            else
            {
                openedFile = IODialogHelper.OpenFile(IODialogHelper.DialogPresets.All, this, ref fileInfo);
            }

            if (openedFile)
            {
                BrowseButtons[(Button)sender].Text = fileInfo.FullName;
            }
        }

        private void lnkAdditionalOptions_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (frmEditTreeAdditionalOptions frm = new frmEditTreeAdditionalOptions((ControlConfiguration)lnkAdditionalOptions.Tag, this.ReadOnly))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                { lnkAdditionalOptions.Tag = frm.Configuration; }
            }
        }

        private void lnkCustomControlFile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.IO.FileInfo controlFile = null;
            if (IODialogHelper.OpenFile("codeml Control File (*.ctl)|*.ctl", "ctl", string.Empty, this, ref controlFile, true))
            {
                try
                {
                    bool overwriteAdditionalOptions = false;
                    switch (Utility.ShowMessage(this, "Set additional options using the custom control file?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                    {
                        case DialogResult.Yes: overwriteAdditionalOptions = true; break;
                        case DialogResult.Cancel: return;
                    }

                    Tree custom = ControlConfiguration.FromCustomControlFile(controlFile.FullName);
                    uctAnalysisConfigurations1.DataSource.Add(new AnalysisConfigurationRowDataItem(custom.AnalysisConfigurations[0]));
                    if (overwriteAdditionalOptions) { lnkAdditionalOptions.Tag = custom.Configuration; }
                }
                catch (Exception ex)
                {
                    Utility.ShowErrorMessage(this, ex);
                }
            }
        }
    }
}