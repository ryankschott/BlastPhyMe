using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChangLab.Common;
using ChangLab.Genes;
using ChangLab.NCBI;
using Pilgrimage.Activities;

namespace Pilgrimage.GeneSequences.BlastN
{
    public partial class frmBlastNOptions : DialogForm
    {
        public List<Gene> SelectedGenes { get; set; }
        public frmMain MainForm { get; set; }
        internal BlastSequencesAtNCBI Activity { get; private set; }
        internal ChangLab.Jobs.BlastNAtNCBI.BLASTPurposes Purpose { get; private set; }

        private BlastNServices SelectedService
        {
            get
            {
                return (BlastNServices)(new RadioButton[] { rbServiceBlastN, rbServiceMegablast, rbServiceDCMegablast }).First(rb => rb.Checked).Tag;
            }
        }
        private BlastNWebServiceConfigurationSettings Options
        {
            get
            {
                BlastNServices service = (BlastNServices)(new RadioButton[] { rbServiceBlastN, rbServiceMegablast, rbServiceDCMegablast }).First(rb => rb.Checked).Tag;
                string[] matchMistmatchScores = ((string)cmbMatchMismatchScores.SelectedValue).Split(new char[] { ',' });

                return new BlastNWebServiceConfigurationSettings()
                {
                    DatabaseName = txtNCBIDatabaseName.Text,
                    Service = service,
                    MaximumTargetSequences = Convert.ToInt32(numMaxTargetSequences.Value),
                    ExpectThreshold = Convert.ToDouble(numExpect.Value),
                    WordSize = Convert.ToInt32(numWordSize.Value),
                    NucleotideMatchReward = int.Parse(matchMistmatchScores[0]),
                    NucleotideMismatchPenalty = int.Parse(matchMistmatchScores[1]),
                    GapCosts = (int[])cmbGapCosts.SelectedValue
                };
            }
        }

        Dictionary<int[], string> GapCostsOptions_Megablast;
        Dictionary<int[], string> GapCostsOptions_NotMegablast;

        public frmBlastNOptions(ChangLab.Jobs.BlastNAtNCBI.BLASTPurposes Purpose)
        {
            InitializeComponent();
            this.Purpose = Purpose;
            
            rbServiceBlastN.Tag = BlastNServices.blastn;
            rbServiceMegablast.Tag = BlastNServices.megablast;
            rbServiceDCMegablast.Tag = BlastNServices.dc_megablast;

            cmbMatchMismatchScores.DataSource = new BindingSource(new string[] 
                { "1,-1", "1,-2", "1,-3", "1,-4", "2,-3", "4,-5" }, null);
            
            List<int[]> gapCostsOptions_Megablast = new List<int[]>();
            gapCostsOptions_Megablast.AddRange(new int[][] { 
                new int[] { 0, 0 }, 
                new int[] { 4, 4 }, new int[] { 2, 4 }, new int[] { 0, 4 }, new int[] { 3, 3 }, 
                new int[] { 6, 2 }, new int[] { 5, 2 }, new int[] { 4, 2 }, new int[] { 2, 2 } });
            GapCostsOptions_Megablast = gapCostsOptions_Megablast.ToDictionary(
                i => i,
                i => (i[0] == 0 ? "Linear" : "Existence: " + i[0] + " Extension: " + i[1]));
            GapCostsOptions_NotMegablast = gapCostsOptions_Megablast.Where(i => i[0] != 0).ToDictionary(
                i => i,
                i => "Existence: " + i[0] + " Extension: " + i[1]);

            SetButtonImage(btnOK, "OK");
            SetButtonImage(btnCancel, "Cancel");

            this.FocusOnLoad = rbServiceBlastN;
        }

        private void frmBlastNOptions_Load(object sender, EventArgs e)
        {
            BlastNWebServiceConfigurationSettings defaultOptions = null;

            switch (this.Purpose)
            {
                case ChangLab.Jobs.BlastNAtNCBI.BLASTPurposes.SimilarCodingSequences:
                    defaultOptions = Program.DatabaseSettings.BlastNAtNCBI.Options_SimilarSequences;
                    break;
                case ChangLab.Jobs.BlastNAtNCBI.BLASTPurposes.AnnotateUnknownGenes:
                    defaultOptions = Program.DatabaseSettings.BlastNAtNCBI.Options_AnnotateSequences;
                    break;
            }
            
            txtNCBIDatabaseName.Text = defaultOptions.DatabaseName;
            rbServiceBlastN.Checked = (defaultOptions.Service == BlastNServices.blastn);
            rbServiceMegablast.Checked = (defaultOptions.Service == BlastNServices.megablast);
            rbServiceDCMegablast.Checked = (defaultOptions.Service == BlastNServices.dc_megablast);
            numMaxTargetSequences.Value = defaultOptions.MaximumTargetSequences;

            this.rbServiceBlastN.CheckedChanged += new System.EventHandler(this.rbService_CheckedChanged);
            this.rbServiceMegablast.CheckedChanged += new System.EventHandler(this.rbService_CheckedChanged);
            this.rbServiceDCMegablast.CheckedChanged += new System.EventHandler(this.rbService_CheckedChanged);
            rbService_CheckedChanged(null, EventArgs.Empty);
        }

        private void rbService_CheckedChanged(object sender, EventArgs e)
        {
            BlastNWebServiceConfigurationSettings defaultSettings = BlastNWebServiceConfigurationSettings.DefaultAlgorithmParameters(this.SelectedService);

            numWordSize.Value = defaultSettings.WordSize;
            cmbMatchMismatchScores.SelectedIndex = ((string[])((BindingSource)cmbMatchMismatchScores.DataSource).DataSource)
                .ToList().FindIndex(s => s == defaultSettings.NucleotideMatchReward + "," + defaultSettings.NucleotideMismatchPenalty);

            cmbGapCosts.DataSource = new BindingSource((this.SelectedService == BlastNServices.megablast ? GapCostsOptions_Megablast : GapCostsOptions_NotMegablast), null);
            cmbGapCosts.SelectedIndex = ((Dictionary<int[], string>)((BindingSource)cmbGapCosts.DataSource).DataSource)
                .ToList().FindIndex(kv => kv.Key[0] == defaultSettings.GapCosts[0] && kv.Key[1] == defaultSettings.GapCosts[1]);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNCBIDatabaseName.Text))
            {
                Utility.ShowMessage(this, "Database name cannot be empty.");
                return;
            }

            SaveSettings();
            SubmitToNCBI();
        }

        private void SubmitToNCBI()
        {
            this.Activity = new BlastSequencesAtNCBI(Program.Settings.CurrentSubSet_GeneSequences, this);
            this.Activity.Submit(SelectedGenes, this.Options, this.Purpose);
            Program.InProgressActivities.AddActivity(this.Activity);

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void blastn_ResultsSaved(ChangLab.Jobs.BlastNAtNCBI.ResultsEventArgs e)
        {
            // Update the Has BlastN Results columns in the open subset views for those sequences that were submitted and for which we got results.
            MainForm.uctGeneSequencesMain1.TabPages.Cast<TabPage>().ToList()
                .ForEach(pg => ((GeneSequences.uctRecordSetGenes)pg.Controls[0])
                                    .UpdateHasAlignedSubjectSequences(e.Alignments.Where(kv => kv.Value.Count != 0).Select(kv => kv.Key.ID).ToList()));
        }

        private void blastn_Completed(ActivityCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                this.Show();
                return;
            }
            if (e.Error != null)
            {
                Utility.ShowErrorMessage(this, e.Error);
                this.Show();
                return;
            }
            else
            {
                List<string> editedSubSetIDs = (List<string>)e.Result;
                if (editedSubSetIDs.Count != 0)
                {
                    editedSubSetIDs.Distinct().ToList().ForEach(id => MainForm.uctGeneSequencesMain1.ShowAndRefreshSubSet(id, null));
                }

                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }

        private void SaveSettings()
        {
            switch (this.Purpose)
            {
                case ChangLab.Jobs.BlastNAtNCBI.BLASTPurposes.SimilarCodingSequences:
                    Program.DatabaseSettings.BlastNAtNCBI.Options_SimilarSequences = Options;
                    break;
                case ChangLab.Jobs.BlastNAtNCBI.BLASTPurposes.AnnotateUnknownGenes:
                    Program.DatabaseSettings.BlastNAtNCBI.Options_AnnotateSequences = Options;
                    break;
            }
        }
    }
}
