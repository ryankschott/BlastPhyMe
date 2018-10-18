using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChangLab.PAML.CodeML;

namespace Pilgrimage.PAML
{
    public partial class frmFilterResults : DialogForm
    {
        public FilterProperties Filter { get; set; }

        public frmFilterResults(FilterProperties Filter)
        {
            InitializeComponent();
            this.Filter = Filter;
            
            SetButtonImage(btnClear, "Filter_Clear");
            SetButtonImage(btnApply, "Filter");
            SetButtonImage(btnCancel, "Cancel");

            this.FocusOnLoad = txtJobTitle;
        }

        private void frmFilterResults_Load(object sender, EventArgs e)
        {
            txtJobTitle.Text = this.Filter.JobTitle;
            cmbJobLogic.DataSource = new BindingSource(this.FilterLogicDataSource, null);
            cmbJobLogic.SelectedValue = this.Filter.JobTitleMatchLogic;

            txtTreeTitle.Text = this.Filter.TreeTitle;
            cmbTreeLogic.DataSource = new BindingSource(this.FilterLogicDataSource, null);
            cmbTreeLogic.SelectedValue = this.Filter.TreeTitleMatchLogic;

            txtTreeFile.Text = this.Filter.TreeFile;
            cmbTreeFileLogic.DataSource = new BindingSource(this.FilterLogicDataSource, null);
            cmbTreeFileLogic.SelectedValue = this.Filter.TreeFileMatchLogic;

            txtSequencesFile.Text = this.Filter.SequencesFile;
            cmbSequencesFileLogic.DataSource = new BindingSource(this.FilterLogicDataSource, null);
            cmbSequencesFileLogic.SelectedValue = this.Filter.SequenceFileMatchLogic;
            
            ModelPreset.All.ForEach(mp => chkModelPresets.Nodes.Add(new TreeNode(mp.Name) { Tag = mp, Checked = this.Filter.Models.Contains(mp.Key) }));
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            this.Filter.JobTitle = txtJobTitle.Text;
            this.Filter.JobTitleMatchLogic = (FilterLogicOptions)cmbJobLogic.SelectedValue;
            this.Filter.TreeTitle = txtTreeTitle.Text;
            this.Filter.TreeTitleMatchLogic = (FilterLogicOptions)cmbTreeLogic.SelectedValue;
            this.Filter.TreeFile = txtTreeFile.Text;
            this.Filter.TreeFileMatchLogic = (FilterLogicOptions)cmbTreeFileLogic.SelectedValue;
            this.Filter.SequencesFile = txtSequencesFile.Text;
            this.Filter.SequenceFileMatchLogic = (FilterLogicOptions)cmbSequencesFileLogic.SelectedValue;
            this.Filter.Models = chkModelPresets.Nodes.Cast<TreeNode>().Where(node => node.Checked).Select(node => ((ModelPreset)node.Tag).Key).ToList();

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.Filter = new FilterProperties();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }

    public class FilterProperties
    {
        public string JobTitle { get; set; }
        public FilterLogicOptions JobTitleMatchLogic { get; set; }
        public string TreeTitle { get; set; }
        public FilterLogicOptions TreeTitleMatchLogic { get; set; }
        public string TreeFile { get; set; }
        public FilterLogicOptions TreeFileMatchLogic { get; set; }
        public string SequencesFile { get; set; }
        public FilterLogicOptions SequenceFileMatchLogic { get; set; }
        public List<ModelPresets> Models { get; set; }

        public FilterProperties()
        {
            JobTitleMatchLogic = FilterLogicOptions.Contains;
            TreeTitleMatchLogic = FilterLogicOptions.Contains;
            TreeFileMatchLogic = FilterLogicOptions.Contains;
            SequenceFileMatchLogic = FilterLogicOptions.Contains;
            Models = new List<ModelPresets>();
        }
    }
}
