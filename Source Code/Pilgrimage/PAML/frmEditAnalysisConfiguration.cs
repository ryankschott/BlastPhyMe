using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChangLab.PAML.CodeML;
using ChangLab.Common;

namespace Pilgrimage.PAML
{
    public partial class frmEditAnalysisConfiguration : DialogForm
    {
        internal AnalysisConfiguration Configuration { get; private set; }

        public frmEditAnalysisConfiguration(AnalysisConfiguration Configuration, bool IsNew = true)
        {
            InitializeComponent();
            this.Configuration = Configuration;

            if (IsNew)
            {
                this.Text = "Add Analysis Configuration";
                btnSave.Text = "&Add";
                SetButtonImage(btnSave, "Add");
            }
            else
            {
                this.Text = "Edit Analysis Configuration";
                btnSave.Text = "&Edit";
                SetButtonImage(btnSave, "Save");
            }

            SetButtonImage(btnCancel, "Cancel");
        }

        private void frmEditAnalysisConfiguration_Load(object sender, EventArgs e)
        {
            if (!this.DesignMode)
            {
                if (this.Configuration != null) { uctAnalysisConfiguration1.Populate(this.Configuration); }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            List<ValidationMessage> messages = null;
            uctAnalysisConfiguration1.Validate(out messages);
            if (ValidationMessage.Prompt(messages, this))
            {
                this.Configuration = uctAnalysisConfiguration1.GetConfiguration();
                Program.Settings.PAML_KappaDefault = new RangeWithInterval(this.Configuration.KStart, this.Configuration.KEnd, this.Configuration.KInterval, this.Configuration.FixedKappa);
                Program.Settings.PAML_OmegaDefault = new RangeWithInterval(this.Configuration.WStart, this.Configuration.WEnd, this.Configuration.WInterval, this.Configuration.FixedOmega);
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }
    }
}
