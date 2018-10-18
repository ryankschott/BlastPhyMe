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
using ChangLab.Jobs;
using Pilgrimage.Activities;

namespace Pilgrimage.GeneSequences.BlastN
{
    public partial class frmAnnotatedGeneDetails : DialogForm
    {
        internal BlastSequencesAtNCBI.Alignment Alignment { get; set; }
        internal string JobID { get; set; }
        internal bool ConfirmUpdate { get; set; }
        private DataGridViewHelper DataGridHelper { get; set; }
        private bool PopulateOnSelect { get; set; }

        public frmAnnotatedGeneDetails()
        {
            InitializeComponent();
            ConfirmUpdate = false;
            PopulateOnSelect = true;

            SetButtonImage(btnSave, "Rename");
            SetButtonImage(btnClear, "Undo");
            SetButtonImage(btnClose, DialogButtonPresets.Cancel);
        }

        private void frmAnnotatedGeneDetails_Load(object sender, EventArgs e)
        {
            // Populate the source record's fields
            txtOriginalDefinition.Text = this.Alignment.Query.Definition;
            txtOriginalOrganism.Text = this.Alignment.Query.Organism;
            txtOriginalTaxonomy.Text = this.Alignment.Query.Taxonomy;
            txtOriginalGeneName.Text = this.Alignment.Query.GeneName;
            txtOriginalAccession.Text = this.Alignment.Query.Accession;
            txtOriginalLocus.Text = this.Alignment.Query.Locus;
            txtOriginalSequence.Text = (this.Alignment.Query.Nucleotides.Length > 100000 ? this.Alignment.Query.Nucleotides.Substring(0, 99985) + "... (truncated)" : this.Alignment.Query.Nucleotides);

            // Download alignment results
            DataGridHelper = new DataGridViewHelper(this, grdResults);
            DataGridHelper.ViewDetails += new DataGridViewHelper.ViewDetailsEventHandler(DataGridHelper_ViewDetails);
            DataGridHelper.Loaded = false;
            grdResults.AutoGenerateColumns = false;

            if (this.Alignment.Alignments == null) // The user hasn't opened this screen for this Query gene yet.
            {
                this.Alignment.Alignments = BlastNAtNCBI.ListAlignedGenes(this.Alignment.Query.ID, Program.Settings.CurrentRecordSet.ID, this.JobID).ToBlastNAlignmentRowList();
            }

            grdResults.DataSource = new SortableBindingList<Activities.BlastNAlignmentRow>(this.Alignment.Alignments);
            grdResults.ClearSelection();
            DataGridHelper.Loaded = true;

            if (this.Alignment.HasMerged)
            {
                // Load up from Merged.
                Pilgrimage.GeneSequences.frmGeneDetails.PopulateGenBankDetailControls(this.Alignment.Merged, this);
                txtMergedDescription.Text = this.Alignment.Merged.Description;
                
                // Re-select based on the GenBankID but without repopulating the form, just to show the user the source Subject record.
                DataGridViewRow mergedRow = grdResults.Rows.Cast<DataGridViewRow>().FirstOrDefault(row => ((Activities.BlastNAlignmentRow)row.DataBoundItem).Gene.GenBankID == this.Alignment.Merged.GenBankID);
                if (mergedRow != null)
                {
                    this.PopulateOnSelect = false;
                    mergedRow.Selected = true;
                    this.PopulateOnSelect = true;
                }
            }
            else
            {
                txtMergedDescription.Text = this.Alignment.Query.Description;

                if (this.Alignment.Merged.GenBankID != 0)
                {
                    // This assumes that the caller (such as the Subject Sequences tab in frmGeneDetails) has a pre-selected record to merge with and
                    // has also pre-populated this.Alignment.Alignments, so we know that we'll have a match and can pre-populate the form.
                    grdResults.Rows.Cast<DataGridViewRow>()
                        .First(row => ((Activities.BlastNAlignmentRow)row.DataBoundItem).Gene.GenBankID == this.Alignment.Merged.GenBankID)
                        .Selected = true;
                }
                else
                {
                    // Load up the top-ranked result if there were hits
                    if (this.Alignment.Alignments.Count > 0)
                    { grdResults.Rows[0].Selected = true; }
                }
            }
        }

        private void grdResults_SelectionChanged(object sender, EventArgs e)
        {
            if (!DataGridHelper.Loaded || grdResults.SelectedRows.Count == 0 || grdResults.SelectedRows[0].Index == -1) { return; }
            
            Gene selectedGene = ((Activities.BlastNAlignmentRow)grdResults.SelectedRows[0].DataBoundItem).Gene;
            if (selectedGene.NeedsUpdateFromGenBank)
            {
                if (PopulateFromGenBank.PopulateSync(selectedGene, null, this))
                {
                    // Refresh the selected row's cells
                    this.DataGridHelper.Loaded = false;
                    DataGridViewRow row = grdResults.SelectedRows[0];
                    row.Selected = false; row.Selected = true;
                    this.DataGridHelper.Loaded = true;
                }
            }
            else
            {
                if (!selectedGene.RefreshedFromDatabase)
                {
                    // This tells us that we did, at one point, update this record from GenBank, so all we need to do is fetch the details.
                    selectedGene.Merge(Gene.Get(selectedGene.ID, true), true, true, true);
                }
            }

            if (PopulateOnSelect)
            {
                // Populate the "replace with" fields
                txtDefinition.Text = txtOriginalDefinition.Text + " similar to " + selectedGene.Definition;
            }

            // This happens regardless of whether we need to populate the "replace with" fields, because we're using it as a storage container for
            // the nucleotide sequence data points.
            txtCodingSequence.Tag = selectedGene;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //if (ConfirmUpdate)
            //{
            //    if (Utility.ShowMessage(this, "Are you sure you want to annotate this record with these values?\r\nThis update will occur immediately and cannot be undone.", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
            //    { return; }
            //}

            //this.Alignment.Merged.Definition = txtDefinition.Text;
            //this.Alignment.Merged.Organism = txtOrganism.Text;
            //this.Alignment.Merged.Taxonomy = txtTaxonomy.Text;
            //this.Alignment.Merged.GeneName = txtGeneName.Text;
            //this.Alignment.Merged.Accession = txtAccession.Text;
            //this.Alignment.Merged.Locus = txtLocus.Text;
            //if (grdResults.SelectedRows.Count != 0)
            //{ this.Alignment.Merged.GenBankID = ((Activities.BlastNAlignmentRow)grdResults.SelectedRows[0].DataBoundItem).Gene.GenBankID; }
            //else
            //{ this.Alignment.Merged.GenBankID = 0; }
            //this.Alignment.Merged.LastUpdatedAt = DateTime.Now;
            //this.Alignment.Merged.LastUpdateSource = GeneSources.User;
            //this.Alignment.HasMerged = true;

            //if (chkReplaceSequence.Enabled && chkReplaceSequence.Checked)
            //{
            //    Gene selectedGene = (Gene)txtCodingSequence.Tag;
            //    this.Alignment.Merged.Nucleotides = selectedGene.Nucleotides;
            //    this.Alignment.Merged.SequenceTypeID = selectedGene.SequenceTypeID;
            //    this.Alignment.Merged.SourceSequence = selectedGene.SourceSequence;
            //    this.Alignment.Merged.Features = selectedGene.Features;
            //    this.Alignment.ReplaceSequence = true;
            //}
            //else
            //{
            //    this.Alignment.Merged.Nucleotides = this.Alignment.Query.Nucleotides;
            //    this.Alignment.Merged.SequenceTypeID = this.Alignment.Query.SequenceTypeID;
            //    this.Alignment.Merged.SourceSequence = this.Alignment.Query.SourceSequence;
            //    this.Alignment.Merged.Features = this.Alignment.Query.Features;
            //    this.Alignment.ReplaceSequence = false;
            //}

            //this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            //txtDefinition.Text = string.Empty;
            //txtOrganism.Text = string.Empty;
            //txtTaxonomy.Text = string.Empty;
            //txtGeneName.Text = string.Empty;
            //txtAccession.Text = string.Empty;
            //txtLocus.Text = string.Empty;
            //txtMergedDescription.Text = string.Empty;
            //chkReplaceSequence.Checked = false;
            //txtCodingSequence.Text = string.Empty;
            //txtCodingSequence.Tag = null;

            //grdResults.ClearSelection();
        }

        private void DataGridHelper_ViewDetails(DataGridViewHelper.ViewDetailsEventArgs e)
        {
            using (GeneSequences.frmGeneDetails frm = new GeneSequences.frmGeneDetails(((GenericGeneRowDataItem)e.Row.DataBoundItem).Gene.ID))
            {
                frm.ShowDialog(this);
            }
        }
    }
}
