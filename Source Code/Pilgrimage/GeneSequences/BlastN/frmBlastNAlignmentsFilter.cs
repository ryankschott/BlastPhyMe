using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Pilgrimage.GeneSequences.BlastN
{
    public partial class frmBlastNAlignmentsFilter : DialogForm
    {
        public FilterProperties Filter { get; set; }

        public frmBlastNAlignmentsFilter(FilterProperties Filter)
        {
            InitializeComponent();
            this.Filter = new FilterProperties() { Definition = Filter.Definition, ResultsExclusion = Filter.ResultsExclusion };

            SetButtonImage(btnDefault, "Filter_Clear");
            SetButtonImage(btnSave, "Filter");
            SetButtonImage(btnCancel, "Cancel");

            rbFilterByGenBankID.Tag = BlastNAlignmentResultsExclusions.ExistsByAccessionVersion;
            rbFilterByOrganism.Tag = BlastNAlignmentResultsExclusions.ExistsByOrganismName;
            rbNoFilter.Tag = BlastNAlignmentResultsExclusions.None;

            this.FocusOnLoad = txtDefinition;
        }

        private void frmBlastNAlignmentsFilter_Load(object sender, EventArgs e)
        {
            (new RadioButton[] { rbFilterByGenBankID, rbFilterByOrganism, rbNoFilter }).First(rb => (BlastNAlignmentResultsExclusions)rb.Tag == this.Filter.ResultsExclusion).Checked = true;
            txtDefinition.Text = this.Filter.Definition;
            cmbDefinitionLogic.DataSource = new BindingSource(this.FilterLogicDataSource, null);
            cmbDefinitionLogic.SelectedValue = this.Filter.DefinitionMatchLogic;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.Filter.Definition = txtDefinition.Text;
            this.Filter.DefinitionMatchLogic = (FilterLogicOptions)cmbDefinitionLogic.SelectedValue;
            this.Filter.ResultsExclusion = (BlastNAlignmentResultsExclusions)(new RadioButton[] { rbFilterByGenBankID, rbFilterByOrganism, rbNoFilter }).First(rb => rb.Checked).Tag;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void btnDefault_Click(object sender, EventArgs e)
        {
            this.Filter = new FilterProperties();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }

    public enum BlastNAlignmentResultsExclusions
    {
        None = 1,
        ExistsByAccessionVersion = 2,
        ExistsByOrganismName = 3
    }

    public class FilterProperties
    {
        public string Definition { get; set; }
        public FilterLogicOptions DefinitionMatchLogic { get; set; }
        public BlastNAlignmentResultsExclusions ResultsExclusion { get; set; }

        public FilterProperties()
        {
            DefinitionMatchLogic = FilterLogicOptions.Contains;
            ResultsExclusion = BlastNAlignmentResultsExclusions.ExistsByAccessionVersion;
        }
    }
}
