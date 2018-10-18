using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChangLab.Genes;
using ChangLab.Jobs;
using ChangLab.RecordSets;

namespace Pilgrimage.GeneSequences.PhyML
{
    public partial class uctPhyMLOptions : UserControl
    {
        private List<RadioButton> FormRadioButtons { get; set; }
        internal bool LoadingDefaults { get; set; }

        internal PhyMLOptions Options
        {
            get
            {
                return new PhyMLOptions()
                {
                    NucleotideSubstitutionModel = (PhyMLNucleotideSubstitutionModels)cmbSubstitutionModel.SelectedValue,
                    EmpiricalEquilibriumFrequencies = rbEquilibriumFrequenciesEmpirical.Checked,
                    NucleotideFrequencies = (rbEquilibriumFrequenciesFixed.Checked ? new Tuple<float, float, float, float>(Convert.ToSingle(numFrequencyA.Value), Convert.ToSingle(numFrequencyC.Value), Convert.ToSingle(numFrequencyG.Value), Convert.ToSingle(numFrequencyT.Value)) : null),
                    EstimateTransitionTransversionRatio = rbTransitionTransversionEstimated.Checked,
                    TransitionTransversionRatio = Convert.ToSingle(numTransitionTransversionRatio.Value),
                    EstimateProportionOfInvariableSites = rbProportionOfInvariableSitesEstimated.Checked,
                    ProportionOfInvariableSites = Convert.ToSingle(numProportionOfInvariableSites.Value),
                    NumberOfSubstitutionRateCategories = Convert.ToInt32(numNumberOfSubstitutionRateCategories.Value),
                    EstimateGammaShapeParameter = rbGammaShapeParameterEstimated.Checked,
                    GammaShapeParameter = Convert.ToSingle(numGammaShapeParameter.Value.ToString()),

                    UseStartingTree = !rbUserTreeBIONJ.Checked,
                    UserTreeFile = (File.Exists(txtUserTreeFilePath.Text) ? txtUserTreeFilePath.Text : string.Empty),
                    TreeTopologySearchOption = (PhyMLTreeTopologySearchOptions)cmbTreeTopologySearchOption.SelectedValue,
                    NumberOfInitialRandomStartingTrees = Convert.ToInt32(numNumberOfInitialRandomStartingTrees.Value),
                    OptimizeTopology = chkOptimizeTopology.Checked,
                    OptimizeBranchLengths = chkOptimizeBranchLengths.Checked,
                    OptimizeSubstitutionRateParameters = chkOptimizeSubstitutionRateParameters.Checked,

                    PerformFastLikelihoodBasedMethod = chkPerformFastLikelihoodBasedMethod.Checked,
                    FastLikelihoodBasedMethod = (PhyMLFastLikelihoodBasedMethods)cmbFastLikelihoodBasedMethod.SelectedValue,
                    PerformBootstrap = chkPerformBootstrap.Checked,
                    NumberOfBootstrapReplicates = Convert.ToInt32(numNumberOfBootstrapReplicates.Value)
                };
            }
        }

        public uctPhyMLOptions()
        {
            InitializeComponent();

            cmbSubstitutionModel.DataSource = new BindingSource(PhyMLOptions.NucleotideSubstitutionModelsList(), null);
            cmbTreeTopologySearchOption.DataSource = new BindingSource(PhyMLOptions.TreeTopologySearchOptionsList(), null);
            cmbFastLikelihoodBasedMethod.DataSource = new BindingSource(PhyMLOptions.FastLikelihoodBasedMethodsList(), null);

            this.FormRadioButtons = new List<RadioButton>();
            FindAllRadioButtons(tblForm);
        }

        private void FindAllRadioButtons(Control Parent)
        {
            foreach (Control ctrl in Parent.Controls)
            {
                if (ctrl.GetType() == typeof(RadioButton)) { FormRadioButtons.Add((RadioButton)ctrl); }
                FindAllRadioButtons(ctrl);
            }
        }

        private void ToggleAutoCheckForAllRadioButtons(bool AutoCheck)
        {
            FormRadioButtons.ForEach(rb => { rb.AutoCheck = AutoCheck; });
        }

        internal void LoadOptions(PhyMLOptions Options = null)
        {
            LoadingDefaults = true;
            ToggleAutoCheckForAllRadioButtons(false);

            PhyMLOptions defaultOptions = (Options == null ? new PhyMLOptions() : Options);
            cmbSubstitutionModel.SelectedValue = defaultOptions.NucleotideSubstitutionModel;
            rbEquilibriumFrequenciesEmpirical.Checked = defaultOptions.EmpiricalEquilibriumFrequencies && (defaultOptions.NucleotideFrequencies == null);
            rbEquilibriumFrequenciesOptimized.Checked = !defaultOptions.EmpiricalEquilibriumFrequencies && (defaultOptions.NucleotideFrequencies == null);
            if (defaultOptions.NucleotideFrequencies != null)
            {
                rbEquilibriumFrequenciesFixed.Checked = true;
                numFrequencyA.Value = Convert.ToDecimal(defaultOptions.NucleotideFrequencies.Item1);
                numFrequencyC.Value = Convert.ToDecimal(defaultOptions.NucleotideFrequencies.Item2);
                numFrequencyG.Value = Convert.ToDecimal(defaultOptions.NucleotideFrequencies.Item3);
                numFrequencyT.Value = Convert.ToDecimal(defaultOptions.NucleotideFrequencies.Item4);
            }
            rbTransitionTransversionEstimated.Checked = defaultOptions.EstimateTransitionTransversionRatio;
            rbTransitionTransversionFixed.Checked = !defaultOptions.EstimateTransitionTransversionRatio;
            numTransitionTransversionRatio.Value = Convert.ToDecimal(defaultOptions.TransitionTransversionRatio);
            rbProportionOfInvariableSitesEstimated.Checked = defaultOptions.EstimateProportionOfInvariableSites;
            rbProportionOfInvariableSitesFixed.Checked = !defaultOptions.EstimateProportionOfInvariableSites;
            numProportionOfInvariableSites.Value = Convert.ToDecimal(defaultOptions.ProportionOfInvariableSites);
            numNumberOfSubstitutionRateCategories.Value = Convert.ToDecimal(defaultOptions.NumberOfSubstitutionRateCategories);
            rbGammaShapeParameterEstimated.Checked = defaultOptions.EstimateGammaShapeParameter;
            rbGammaShapeParameterFixed.Checked = !defaultOptions.EstimateGammaShapeParameter;
            numGammaShapeParameter.Value = Convert.ToDecimal(defaultOptions.GammaShapeParameter);

            rbUserTreeBIONJ.Checked = !defaultOptions.UseStartingTree;
            rbUserTreeFile.Checked = defaultOptions.UseStartingTree;
            txtUserTreeFilePath.Text = defaultOptions.UserTreeFile;
            cmbTreeTopologySearchOption.SelectedValue = defaultOptions.TreeTopologySearchOption;
            numNumberOfInitialRandomStartingTrees.Value = Convert.ToDecimal(defaultOptions.NumberOfInitialRandomStartingTrees);
            chkOptimizeTopology.Checked = defaultOptions.OptimizeTopology;
            chkOptimizeBranchLengths.Checked = defaultOptions.OptimizeBranchLengths;
            chkOptimizeSubstitutionRateParameters.Checked = defaultOptions.OptimizeSubstitutionRateParameters;

            chkPerformFastLikelihoodBasedMethod.Checked = defaultOptions.PerformFastLikelihoodBasedMethod;
            cmbFastLikelihoodBasedMethod.SelectedValue = defaultOptions.FastLikelihoodBasedMethod;
            chkPerformBootstrap.Checked = defaultOptions.PerformBootstrap;
            numNumberOfBootstrapReplicates.Value = Convert.ToDecimal(defaultOptions.NumberOfBootstrapReplicates);

            LoadingDefaults = false;
            ToggleAutoCheckForAllRadioButtons(true);

            rbEquilibriumFrequenciesFixed_CheckedChanged(rbEquilibriumFrequenciesFixed, EventArgs.Empty);
            rbTransitionTransversionFixed_CheckedChanged(rbTransitionTransversionFixed, EventArgs.Empty);
            rbProportionOfInvariableSitesFixed_CheckedChanged(rbProportionOfInvariableSitesFixed, EventArgs.Empty);
            rbGammaShapeParameterFixed_CheckedChanged(rbGammaShapeParameterFixed, EventArgs.Empty);

            rbUserTreeBIONJ_CheckedChanged(rbUserTreeBIONJ, EventArgs.Empty);
            cmbTreeTopologySearchOption_SelectedIndexChanged(cmbTreeTopologySearchOption, EventArgs.Empty);

            chkPerformFastLikelihoodBasedMethod_CheckedChanged(chkPerformFastLikelihoodBasedMethod, EventArgs.Empty);
            chkPerformBootstrap_CheckedChanged(chkPerformBootstrap, EventArgs.Empty);
        }

        internal void Validation(out List<ValidationMessage> Messages)
        {
            Messages = new List<ValidationMessage>();

            // Guide tree is allowed to be empty
            if (!string.IsNullOrWhiteSpace(txtUserTreeFilePath.Text) && !File.Exists(txtUserTreeFilePath.Text)) { Messages.Add(new ValidationMessage("Starting tree file could not be found at the location provided.", MessageBoxIcon.Error)); }
        }

        private void btnBrowseGuideTree_Click(object sender, EventArgs e)
        {
            string filePath = txtUserTreeFilePath.Text;
            if (IODialogHelper.OpenFile("Tree file|*.tre|Text file|*.txt|All files|*.*", "tre", this, ref filePath))
            {
                txtUserTreeFilePath.Text = filePath;
            }
        }
        private void rbEquilibriumFrequenciesFixed_CheckedChanged(object sender, EventArgs e)
        {
            if (LoadingDefaults) { return; }
            tblNucleotideFrequencies.Visible = rbEquilibriumFrequenciesFixed.Checked;
        }

        private void rbTransitionTransversionFixed_CheckedChanged(object sender, EventArgs e)
        {
            if (LoadingDefaults) { return; }
            numTransitionTransversionRatio.Visible = rbTransitionTransversionFixed.Checked;
        }

        private void rbProportionOfInvariableSitesFixed_CheckedChanged(object sender, EventArgs e)
        {
            if (LoadingDefaults) { return; }
            numProportionOfInvariableSites.Visible = rbProportionOfInvariableSitesFixed.Checked;
        }

        private void rbGammaShapeParameterFixed_CheckedChanged(object sender, EventArgs e)
        {
            if (LoadingDefaults) { return; }
            numGammaShapeParameter.Visible = rbGammaShapeParameterFixed.Checked;
        }

        private void chkPerformFastLikelihoodBasedMethod_CheckedChanged(object sender, EventArgs e)
        {
            if (LoadingDefaults) { return; }
            cmbFastLikelihoodBasedMethod.Enabled = chkPerformFastLikelihoodBasedMethod.Checked;
            if (chkPerformFastLikelihoodBasedMethod.Checked)
            { chkPerformBootstrap.Checked = false; }
        }

        private void chkPerformBootstrap_CheckedChanged(object sender, EventArgs e)
        {
            if (LoadingDefaults) { return; }
            numNumberOfBootstrapReplicates.Enabled = chkPerformBootstrap.Checked;
            if (chkPerformBootstrap.Checked)
            { chkPerformFastLikelihoodBasedMethod.Checked = false; }
        }

        private void rbUserTreeBIONJ_CheckedChanged(object sender, EventArgs e)
        {
            if (LoadingDefaults) { return; }
            txtUserTreeFilePath.Enabled = !rbUserTreeBIONJ.Checked;
            btnUserTreeFileBrowse.Enabled = !rbUserTreeBIONJ.Checked;
        }

        private void cmbTreeTopologySearchOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LoadingDefaults) { return; }
            numNumberOfInitialRandomStartingTrees.Enabled = ((PhyMLTreeTopologySearchOptions)cmbTreeTopologySearchOption.SelectedValue != PhyMLTreeTopologySearchOptions.NNI);
        }
    }
}
