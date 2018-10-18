using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChangLab.PAML.CodeML;
using ChangLab.Common;

namespace Pilgrimage.PAML
{
    public partial class uctAnalysisConfiguration : UserControl
    {
        private List<ModelPreset> Presets { get; set; }

        internal AnalysisConfiguration GetConfiguration()
        {
            ModelPreset preset = (ModelPreset)cmbModel.SelectedItem;

            AnalysisConfiguration configuration = new AnalysisConfiguration()
                {
                    Model = preset.Model,
                    ModelPresetID = preset.ID,
                    NCatG = int.Parse((string.IsNullOrWhiteSpace(txtNCatG.Text) ? "-1" : txtNCatG.Text)),
                    KStart = double.Parse(txtKappaStart.Text),
                    KEnd = double.Parse(txtKappaEnd.Text),
                    KInterval = double.Parse(txtKappaInterval.Text),
                    FixedKappa = chkFixedKappa.Checked,
                    WStart = double.Parse(txtOmegaStart.Text),
                    WEnd = double.Parse(txtOmegaEnd.Text),
                    WInterval = double.Parse(txtOmegaInterval.Text),
                    FixedOmega = chkFixedOmega.Checked
                };
            configuration.NSSites.AddRange(pnlNSSites.Controls.Cast<CheckBox>().Where(chk => chk.Checked).Select(chk => (int)chk.Tag));

            return configuration;
        }

        public uctAnalysisConfiguration()
        {
            InitializeComponent();
            if (this.DesignMode) { return; }

            if (Program.Settings != null) // It'll be null in design mode.
            {
                RangeWithInterval kappa = Program.Settings.PAML_KappaDefault;
                txtKappaStart.Text = kappa.Start.ToString();
                txtKappaEnd.Text = kappa.End.ToString();
                txtKappaInterval.Text = kappa.Interval.ToString();
                chkFixedKappa.Checked = kappa.Fixed;

                RangeWithInterval omega = Program.Settings.PAML_OmegaDefault;
                txtOmegaStart.Text = omega.Start.ToString();
                txtOmegaEnd.Text = omega.End.ToString();
                txtOmegaInterval.Text = omega.Interval.ToString();
                chkFixedOmega.Checked = omega.Fixed;
            }

            txtInterval_Leave(txtKappaInterval, EventArgs.Empty);
            txtInterval_Leave(txtOmegaInterval, EventArgs.Empty);

            this.Presets = ModelPreset.All;
            cmbModel.DisplayMember = "Name";
            cmbModel.ValueMember = "Key";
            cmbModel.DataSource = new BindingSource(this.Presets, null);
        }

        internal void Populate(AnalysisConfiguration Configuration)
        {
            ModelPreset preset = ModelPreset.Derive(Configuration);
            cmbModel.SelectedValue = preset.Key;
            if (preset.Key == ModelPresets.Model0)
            {
                pnlNSSites.Controls.Cast<CheckBox>().ToList().ForEach(chk =>
                    chk.Checked = Configuration.NSSites.Contains((int)chk.Tag)
                );
            }
            
            txtNCatG.Text = Configuration.NCatG.ToString();
            txtKappaStart.Text = Configuration.KStart.ToString();
            txtKappaEnd.Text = Configuration.KEnd.ToString();
            txtKappaInterval.Text = (Configuration.KInterval != 0 ? Configuration.KInterval : AnalysisConfiguration.KIntervalDefault).ToString();
            chkFixedKappa.Checked = Configuration.FixedKappa;
            txtOmegaStart.Text = Configuration.WStart.ToString();
            txtOmegaEnd.Text = Configuration.WEnd.ToString();
            txtOmegaInterval.Text = (Configuration.WInterval != 0 ? Configuration.WInterval : AnalysisConfiguration.WIntervalDefault).ToString();
            chkFixedOmega.Checked = Configuration.FixedOmega;
        }

        internal bool Validate(out List<ValidationMessage> Messages)
        {
            Messages = new List<ValidationMessage>();

            AnalysisConfiguration validate = GetConfiguration();

            if (validate.NSSites.Count == 0)
            { Messages.Add(new ValidationMessage("At least one site model must be selected.", MessageBoxIcon.Error)); }

            if (validate.NCatG == -1)
            { Messages.Add(new ValidationMessage("Categories cannot be empty.", MessageBoxIcon.Error)); }

            if (validate.KEnd > 0 && validate.KEnd < validate.KStart)
            { Messages.Add(new ValidationMessage("Kappa end value must be 0 or greater than start value.", MessageBoxIcon.Error)); }
            if (validate.KInterval <= 0)
            { Messages.Add(new ValidationMessage("Kappa interval must be greater than 0. To configure a single kappa value, set the start and end values to be the same.", MessageBoxIcon.Error)); }
            else
            {
                if ((((validate.KEnd - validate.KStart) / validate.KInterval) % 1.0D) != 0)
                { Messages.Add(new ValidationMessage("Kappa range does not evenly divide by interval value.", MessageBoxIcon.Warning)); }
            }

            if (validate.WEnd > 0 && validate.WEnd < validate.WStart)
            { Messages.Add(new ValidationMessage("Omega end value must be 0 or greater than start value.", MessageBoxIcon.Error)); }
            if (validate.WInterval <= 0)
            { Messages.Add(new ValidationMessage("Omega interval must be greater than 0. To configure a single omega value, set the start and end values to be the same.", MessageBoxIcon.Error)); }
            {
                if ((((validate.WEnd - validate.WStart) / validate.WInterval) % 1.0D) != 0)
                { Messages.Add(new ValidationMessage("Omega range does not evenly divide by interval value.", MessageBoxIcon.Warning)); }
            }

            return Messages.Count == 0;
        }

        private void cmbModel_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cmbModel.SelectedValue != null)
            {
                ModelPreset preset = (ModelPreset)cmbModel.SelectedItem;

                pnlNSSites.Controls.Clear();
                preset.NSSites.ForEach(ns =>
                    {
                        CheckBox chkNS = new CheckBox()
                            {
                                Text = ns.ToString(),
                                Tag = ns,
                                AutoSize = true,
                                Checked = true,
                                Margin = new Padding(3)
                            };
                        chkNS.CheckedChanged += new EventHandler(chkNS_CheckedChanged);
                        pnlNSSites.Controls.Add(chkNS);
                    });
                pnlNSSites.Enabled = (preset.NSSites.Count > 1);

                txtNCatG.Text = preset.NCatG.ToString();
                txtNCatG.Enabled = !preset.NCatGFixed;
                chkNS_CheckedChanged(null, EventArgs.Empty); // If Sites 0 is selected, whether or not txtNCatG is enabled is dependent on the number of sites models selected.

                if (preset.Omega.Fixed)
                {
                    txtOmegaStart.Text = preset.Omega.Start.ToString();
                    txtOmegaEnd.Text = preset.Omega.End.ToString();
                    txtOmegaInterval.Text = preset.Omega.Interval.ToString();
                    chkFixedOmega.Checked = true;
                    chkFixedOmega.Enabled = false;
                }
                else
                {
                    chkFixedOmega.Checked = false;
                    chkFixedOmega.Enabled = true;
                }
            }
        }

        private void chkNS_CheckedChanged(object sender, EventArgs e)
        {
            List<int> sitesModels = pnlNSSites.Controls.Cast<CheckBox>().Where(chk => chk.Checked).Select(chk => (int)chk.Tag).ToList();
            
            if (sitesModels.Count > 1)
            {
                txtNCatG.Value = 10;
                txtNCatG.Enabled = false;
            }
            else
            {
                txtNCatG.Enabled = true;
            }
            
            if (sitesModels.Count == 1)
            {
                switch (sitesModels[0])
                {
                    case 3: 
                        txtNCatG.Value = 3;
                        break;
                    case 7:
                    case 8:
                        txtNCatG.Value = 10;
                        break;
                }
            }
        }
        
        private void txtInterval_Leave(object sender, EventArgs e)
        {
            if (sender == txtKappaInterval)
            {
                txtKappaStart.Increment = ((NumericUpDown)sender).Value;
                txtKappaEnd.Increment = ((NumericUpDown)sender).Value;
            }
            else if (sender == txtOmegaInterval)
            {
                txtOmegaStart.Increment = ((NumericUpDown)sender).Value;
                txtOmegaEnd.Increment = ((NumericUpDown)sender).Value;
            }

            // If I decide to go back to having the interval be dependent on what the remainder is in the individual NumericUpDown:
            /*
                NumericUpDown ctrl = (NumericUpDown)sender;
                double increment = ((double)ctrl.Value % 1.0D);
                if (increment == 0) { increment = 1.0D; }
                ctrl.Increment = (decimal)increment;
            */
        }
    }
}
