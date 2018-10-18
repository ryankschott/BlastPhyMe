using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChangLab.PAML;

namespace Pilgrimage.PAML
{
    public partial class frmEditTreeAdditionalOptions : DialogForm
    {
        public ControlConfiguration Configuration { get; set; }
        private ControlConfiguration _defaultConfiguration = new ControlConfiguration();

        public bool ReadOnly { get; set; }

        private ControlConfiguration GetConfiguration()
        {
            return new ControlConfiguration()
            {
                Verbose = (int)cmbVerbose.SelectedValue,
                RunMode = (int)cmbRunMode.SelectedValue,
                SequenceType = (int)cmbSequenceType.SelectedValue,
                CodonFrequency = (int)cmbCodonFrequencies.SelectedValue,
                Clock = (int)cmbClock.SelectedValue,
                ICode = (int)cmbICode.SelectedValue,
                MGene = (int)cmbMGene.SelectedValue,
                Method = (int)cmbOptimizationMethod.SelectedValue,

                MAlpha = chkMAlpha.Checked,
                GetSE = chkGetSE.Checked,
                RateAncestor = chkRateAncestor.Checked,
                CleanData = chkCleanData.Checked
            };
        }

        public frmEditTreeAdditionalOptions()
        {
            InitializeComponent();
        }

        public frmEditTreeAdditionalOptions(ControlConfiguration Configuration, bool ReadOnly) : this()
        {
            this.Configuration = Configuration;
            this.ReadOnly = ReadOnly;

            cmbVerbose.DataSource = new BindingSource(ControlConfiguration.VerboseModes, null);
            cmbRunMode.DataSource = new BindingSource(ControlConfiguration.RunModes, null);
            cmbSequenceType.DataSource = new BindingSource(ControlConfiguration.SequenceTypes, null);
            cmbCodonFrequencies.DataSource = new BindingSource(ControlConfiguration.CodonFrequencies, null);
            cmbClock.DataSource = new BindingSource(ControlConfiguration.Clocks, null);
            cmbICode.DataSource = new BindingSource(ControlConfiguration.ICodes, null);
            cmbMGene.DataSource = new BindingSource(ControlConfiguration.MGene_Codon, null);
            cmbOptimizationMethod.DataSource = new BindingSource(ControlConfiguration.Methods, null);

            if (ReadOnly)
            {
                btnReset.Visible = false;
                btnSave.Visible = false;
                tblAdditionalOptions.Enabled = false;
            }

            this.FocusOnLoad = cmbVerbose;
        }

        private void frmEditTreeAdditionalOptions_Load(object sender, EventArgs e)
        {
            if (this.Configuration == null) { this.Configuration = Program.DatabaseSettings.PAML.Configuration; }

            cmbVerbose.SelectedValue = this.Configuration.Verbose;
            cmbRunMode.SelectedValue = this.Configuration.RunMode;
            cmbSequenceType.SelectedValue = this.Configuration.SequenceType;
            cmbSequenceType.SelectedIndexChanged += new System.EventHandler(cmbSequenceType_SelectedIndexChanged);
            cmbCodonFrequencies.SelectedValue = this.Configuration.CodonFrequency;
            cmbClock.SelectedValue = this.Configuration.Clock;
            cmbICode.SelectedValue = this.Configuration.ICode;
            cmbMGene.SelectedValue = this.Configuration.MGene;
            cmbOptimizationMethod.SelectedValue = this.Configuration.Method;

            chkMAlpha.Checked = this.Configuration.MAlpha;
            chkGetSE.Checked = this.Configuration.GetSE;
            chkRateAncestor.Checked = this.Configuration.RateAncestor;
            chkCleanData.Checked = this.Configuration.CleanData;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.Configuration = GetConfiguration();
            Program.DatabaseSettings.PAML.Configuration = GetConfiguration();

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            cmbVerbose.SelectedValue = _defaultConfiguration.Verbose;
            cmbRunMode.SelectedValue = _defaultConfiguration.RunMode;
            cmbSequenceType.SelectedValue = _defaultConfiguration.SequenceType;
            cmbCodonFrequencies.SelectedValue = _defaultConfiguration.CodonFrequency;
            cmbClock.SelectedValue = _defaultConfiguration.Clock;
            cmbICode.SelectedValue = _defaultConfiguration.ICode;
            cmbMGene.SelectedValue = _defaultConfiguration.MGene;
            cmbOptimizationMethod.SelectedValue = _defaultConfiguration.Method;

            chkMAlpha.Checked = _defaultConfiguration.MAlpha;
            chkGetSE.Checked = _defaultConfiguration.GetSE;
            chkRateAncestor.Checked = _defaultConfiguration.RateAncestor;
            chkCleanData.Checked = _defaultConfiguration.CleanData;
        }

        private void cmbSequenceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            int mGene = (int)cmbMGene.SelectedValue;
            int defaultMGene = _defaultConfiguration.MGene;
            Dictionary<int, string> dataSource = null;

            if ((int)cmbSequenceType.SelectedValue == 1)
            { dataSource = ControlConfiguration.MGene_Codon; }
            else
            { dataSource = ControlConfiguration.MGene_AA; }

            cmbMGene.DataSource = new BindingSource(dataSource, null);
            cmbMGene.SelectedValue = (dataSource.ContainsKey(mGene) ? mGene : defaultMGene);
        }
    }
}
