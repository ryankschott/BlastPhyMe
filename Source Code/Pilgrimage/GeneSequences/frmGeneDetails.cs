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
using ChangLab.NCBI.GenBank;
using ChangLab.RecordSets;
using Pilgrimage.Activities;

namespace Pilgrimage.GeneSequences
{
    public partial class frmGeneDetails : AddGeneSequencesToRecordSetForm
    {
        public Gene Gene { get; set; }
        private bool PopulateOnLoad { get; set; }
        private bool Editable { get; set; }
        private bool EditInMemoryOnly { get; set; }
        private GenBankSearch SearchResult { get; set; }
        /// <summary>
        /// If the Gene needs to be populated on load (LastUpdatedAt hasn't been set), any updates will be saved back to the database.
        /// </summary>
        public bool SaveUpdates { get; set; }
        /// <summary>
        /// True if the Gene was updated via download from GenBank.
        /// </summary>
        public bool Updated { get; private set; }

        private EditNucleotideSequence.frmNucleotideSequenceFeatures FeaturesForm { get; set; }
        private Dictionary<TabPage, Button> CancelButtons { get; set; }

        #region Constructors
        private bool ShowBlastNResults { get; set; }

        public frmGeneDetails(string GeneID, bool ShowBlastNResults = false) : this(null, false, null, ShowBlastNResults)
        {
            if (!string.IsNullOrEmpty(GeneID))
            {
                this.Gene = Gene.Get(GeneID, false);
                if (this.Gene.NeedsUpdateFromGenBank)
                {
                    this.PopulateOnLoad = true;
                    this.SaveUpdates = true;
                }
            }
        }

        public frmGeneDetails(Gene Gene, bool PopulateFromGenBank, GenBankSearch SearchResult, bool ShowBlastNResults = false) : base()
        {
            InitializeComponent();
            this.Gene = Gene;
            this.Editable = false;
            this.PopulateOnLoad = PopulateFromGenBank;
            this.SearchResult = SearchResult;
            this.ShowBlastNResults = ShowBlastNResults;

            // Setup Add To functionality
            this.Configure(grdSubjectSequences, cmbSubSets, btnAddTo, chkUpdateFromGenBank);

            SetButtonImage(btnEdit, "Rename");
            SetButtonImage(btnReset, "Undo");
            SetButtonImage(btnClose, DialogButtonPresets.Cancel);
            SetButtonImage(btnCloseSubject, DialogButtonPresets.Cancel);
            SetButtonImage(btnCloseQuery, DialogButtonPresets.Cancel);
            SetButtonImage(btnMerge, "Copy");
            SetButtonImage(btnAddTo, "Add");

            CancelButtons = new Dictionary<TabPage, Button>();
            CancelButtons.Add(pgDetails, btnClose);
            CancelButtons.Add(pgSubjectSequences, btnCloseSubject);
            CancelButtons.Add(pgQuerySequences, btnCloseQuery);
        }

        internal void InitalizeAsEditable(bool EditInMemoryOnly = true)
        {
            if (this.Gene.SourceIsNCBI) { return; }

            btnEdit_Click(null, EventArgs.Empty);
            this.EditInMemoryOnly = EditInMemoryOnly;
        }

        private void frmGeneDetails_Load(object sender, EventArgs e)
        {
            if (this.PopulateOnLoad)
            {
                if (PopulateFromGenBank.PopulateSync(Gene, SearchResult, this))
                {
                    if (SaveUpdates)
                    {
                        Gene.Save(true, true);
                        this.Updated = true;
                    }
                }
                else { this.DialogResult = System.Windows.Forms.DialogResult.Cancel; return; }
            }

            if (!this.Gene.NeedsUpdateFromGenBank && this.Gene.Features.Count == 0)
            {
                this.Gene.GetSequenceData();
            }
            frmGeneDetails.PopulateGenBankDetailControls(this.Gene, this);

            if (!ShowBlastNResults)
            {
                tbForm.TabPages.Remove(pgSubjectSequences);
            }
            if (this.Gene.Source != GeneSources.BLASTN_NCBI)
            {
                tbForm.TabPages.Remove(pgQuerySequences);
            }

            // At present, due to the whole issue surrounding being able to aggregate based on GenBankID for NCBI records, any record with a source
            // of GenBank or BLASTN_NCBI is not editable.
            if (this.Gene.SourceIsNCBI) { tblEditButtons.Visible = false; }

            if (tbForm.TabPages.Count == 1) // No need to show tabs if there's only the Details page visible
            {
                tblDetails.Parent.Controls.Remove(tblDetails);
                tbForm.Parent.Controls.Remove(tbForm);
                this.Controls.Add(tblDetails);
            }

            this.SetFocusOnControl(lblDefinition);
        }
        #endregion

        #region Details
        public static void PopulateGenBankDetailControls(Gene Gene, Form Form)
        {
            SetTextBoxValue(Form, "txtGenBankID", (Gene.GenBankID != 0 ? Gene.GenBankID.ToString() : string.Empty));
            SetTextBoxValue(Form, "txtGeneName", Gene.GeneName);
            SetTextBoxValue(Form, "txtDefinition", Gene.Definition);
            SetTextBoxValue(Form, "txtOrganism", Gene.Organism);
            SetTextBoxValue(Form, "txtTaxonomy", Gene.Taxonomy);
            SetTextBoxValue(Form, "txtLocus", Gene.Locus);
            SetTextBoxValue(Form, "txtLength", Gene.LengthDescription());
            SetTextBoxValue(Form, "txtAccession", Gene.Accession);
            SetTextBoxValue(Form, "txtCodingSequence", (Gene.Nucleotides.Length > 100000 ? Gene.Nucleotides.Substring(0, 99985) + "... (truncated)" : Gene.Nucleotides));
            SetTextBoxValue(Form, "txtDescription", Gene.Description);
            SetTextBoxValue(Form, "txtLastUpdated", Gene.LastUpdatedAt.ToStandardDateTimeString() + " by " + ChangLab.Genes.GeneSource.NameByID(Gene.LastUpdateSourceID));
            SetTextBoxValue(Form, "txtSource", GeneSource.NameByID(Gene.SourceID));

            if (Gene.GenBankID == 0)
            {
                // No value will have been set in txtGenBankID, so its TextChanged event (below) won't have been triggered.
                Control[] test = Form.Controls.Find("lnkGenBankURL", true);
                if (test.Length == 1) { ((Control)test[0]).Visible = false; }
            }

            Form.Text = Gene.Definition;
        }

        private static void SetTextBoxValue(Form Form, string Name, string Value)
        {
            Control[] test = Form.Controls.Find(Name, true);
            if (test.Length == 1) { ((TextBox)test[0]).Text = Value; }
        }

        private void txtAccession_TextChanged(object sender, EventArgs e)
        {
            UpdateGenBankURL(txtAccession, lnkGenBankURL);
        }

        internal static void UpdateGenBankURL(TextBox AccessionBox, LinkLabel GenBankURLLink)
        {
            string url = Gene.GetGenBankUrl(AccessionBox.Text);

            if (!string.IsNullOrWhiteSpace(url))
            {
                GenBankURLLink.Text = url;
                if (!GenBankURLLink.Visible) { GenBankURLLink.Visible = true; }
            }
            else
            { if (GenBankURLLink.Visible) { GenBankURLLink.Visible = false; } }
        }

        private void lnkGenBankURL_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(((LinkLabel)sender).Text);
        }

        private void txtCodingSequence_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A && e.Modifiers == Keys.Control)
            {
                txtCodingSequence.SelectAll();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (!this.Editable)
            {
                ToggleForm(true);
                txtDefinition.Focus();
            }
            else
            {
                // Form validation
                if (Validation())
                {
                    this.Gene.GeneName = txtGeneName.Text;
                    this.Gene.Definition = txtDefinition.Text;
                    this.Gene.Organism = txtOrganism.Text;
                    this.Gene.Taxonomy = txtTaxonomy.Text;
                    this.Gene.Accession = txtAccession.Text;
                    this.Gene.Locus = txtLocus.Text;
                    if (!string.IsNullOrWhiteSpace(txtGenBankID.Text)) { this.Gene.GenBankID = int.Parse(txtGenBankID.Text); }

                    if (FeaturesForm != null)
                    {
                        this.Gene.Features = FeaturesForm.Features.Select(f => f.Copy()).OrderBy(f => f.FeatureKey.Rank).Select((f, index) => { f.Rank = (index + 1); return f; }).ToList();
                        this.Gene.SourceSequence = FeaturesForm.SourceSequence;
                        this.Gene.Nucleotides = txtCodingSequence.Text; // We've already re-spliced the gene after closing the FeaturesForm instance.
                        FeaturesForm = null; // Reset the features form so that next time it's opened it'll pick up a fresh copy of the this.Gene instance.
                    }

                    this.Gene.Description = txtDescription.Text;
                    this.Gene.LastUpdatedAt = DateTime.Now;
                    this.Gene.LastUpdateSource = GeneSources.User;

                    if (!EditInMemoryOnly)
                    {
                        this.Gene.Save(true, true);
                        this.Gene.Merge(Gene.Get(this.Gene.ID, true), true, true, true); // Refresh directly from the database to pick up things like TaxonomyHierarchy
                    }
                    this.Updated = true;

                    if (EditInMemoryOnly)
                    {
                        this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    }
                    else
                    {
                        btnReset.PerformClick();
                    }
                }
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            PopulateGenBankDetailControls(this.Gene, this);
            FeaturesForm = null;
            ToggleForm(false);
            txtDefinition.Focus();
        }

        private void ToggleForm(bool Editable)
        {
            txtGeneName.ReadOnly = !Editable;
            txtDefinition.ReadOnly = !Editable;
            txtOrganism.ReadOnly = !Editable;
            txtTaxonomy.ReadOnly = !Editable;
            txtAccession.ReadOnly = !Editable;
            txtLocus.ReadOnly = !Editable;
            if (!(new GeneSources[] { GeneSources.BLASTN_NCBI, GeneSources.GenBank }).Contains(this.Gene.Source))
            { txtGenBankID.ReadOnly = !Editable; }
            else
            { txtGenBankID.ReadOnly = true; }
            lnkSearchTaxonomy.Visible = Editable;
            lnkEditNucleotideSequence.Text = (Editable ? "Edit" : "View") + " Source Sequence";
            if (FeaturesForm != null) { FeaturesForm.ToggleForm(Editable); }
            txtDescription.ReadOnly = !Editable;
            btnReset.Visible = Editable;

            if (Editable)
            {
                SetButtonImage(btnEdit, "Save");
                btnEdit.Text = "&Save";
                this.AcceptButton = btnEdit;
            }
            else
            {
                SetButtonImage(btnEdit, "Rename");
                btnEdit.Text = "&Edit";
                this.AcceptButton = btnClose;
            }
            this.Editable = Editable;
        }

        private bool Validation()
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();

            if (string.IsNullOrWhiteSpace(txtDefinition.Text)) { messages.Add(new ValidationMessage("Definition is required.", MessageBoxIcon.Error)); }

            switch (this.Gene.Source)
            {
                case GeneSources.BLASTN_Local:
                case GeneSources.Trinity:
                    break;
                default:
                    if (string.IsNullOrWhiteSpace(txtOrganism.Text)) { messages.Add(new ValidationMessage("Organism is required.", MessageBoxIcon.Error)); }
                    break;
            }

            if (!string.IsNullOrWhiteSpace(txtGenBankID.Text))
            { int genBankId = 0; if (!int.TryParse(txtGenBankID.Text, out genBankId)) { messages.Add(new ValidationMessage("GenBank ID must be a number.", MessageBoxIcon.Error)); } }
                        
            return ValidationMessage.Prompt(messages, this);
        }

        private void lnkSearchTaxonomy_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string searchQuery = string.Empty;
            if (string.IsNullOrWhiteSpace(txtTaxonomy.Text) && !string.IsNullOrWhiteSpace(txtOrganism.Text))
            {
                // Try and help the user out by pre-populating the search query with the genus (or at least whatever's available).
                string[] pieces = txtOrganism.Text.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                if (pieces.Length > 1) { searchQuery = pieces[0]; }
                else { searchQuery = txtOrganism.Text; }
            }

            using (Pilgrimage.Search.frmSearchForTaxonomy frm = new Pilgrimage.Search.frmSearchForTaxonomy(DialogButtonPresets.OK, true, searchQuery) { PopulateDetailsForSelectedTaxon = true })
            {
                if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    txtTaxonomy.Text = frm.SelectedTaxon.Lineage.Replace("cellular organisms; ", "");
                }
            }
        }

        private void lnkEditNucleotideSequence_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Having this form as a property allows us to use it as a container for a seperate copy of the Gene.Features collection.
            // When features are being edited it's within the instance of that containing form, never the this.Gene object within frmGeneDetails,
            // which is a convenient way to keep the Reset button functionality (and the Cancel button, and everything really) clean and separate
            // such that the user can make a hundred edits throughout frmGeneDetails and frmNucleotideSequenceFeatures but nothing is affecting the
            // this.Gene properties (value or reference type) until the user commits a Save.
            if (FeaturesForm == null) { FeaturesForm = new EditNucleotideSequence.frmNucleotideSequenceFeatures(this.Gene, this.Editable); }

            // When this was coded you could only get an OK if this.Editable because that's the only way the Save button was visible, but just to be safe...
            if (FeaturesForm.ShowDialog() == System.Windows.Forms.DialogResult.OK && this.Editable)
            {
                // In btnSave_Click the FeaturesForm.Features property is accessed to grab the edited features and apply them to the Gene instance
                // we'll actually save to the database.
                txtLength.Text = Gene.LengthDescription(FeaturesForm.Features, FeaturesForm.SourceSequence);
                txtCodingSequence.Text = FeaturesForm.SourceSequence.SpliceByBestFeatures(FeaturesForm.Features).ToString();
            }

            //// <2015-02-03> The old code, in case I want to go back to the old way of handling all of this. </2015-02-03>
            //using (EditNucleotideSequence.frmNucleotideSequenceFeatures frm = new EditNucleotideSequence.frmNucleotideSequenceFeatures(this.Gene, this.Editable))
            //{
            //    // When this was coded you could only get an OK if this.Editable because that's the only way the Save button was visible, but just to be safe...
            //    if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK && this.Editable)
            //    {
            //        // Re-rank the features just so we're still doing something with ranking.
            //        this.Gene.Features = frm.Features.Select(f => f.Copy()).OrderBy(f => f.FeatureKey.Rank).Select((f, index) => { f.Rank = (index + 1); return f; }).ToList();
            //        // Update the length box to take into account something like changing the CDS annotation
            //        txtLength.Text = Gene.LengthDescription();
            //    }
            //}
        }
        #endregion

        #region BLASTN
        private DataGridViewHelper QueryDataGridHelper { get; set; }

        private void tbForm_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPage == pgSubjectSequences && grdSubjectSequences.DataSource == null)
            {
                // Download subject sequences
                this.SubjectDataGridHelper = new DataGridViewHelper(this, grdSubjectSequences);
                this.SubjectDataGridHelper.SelectedRowsChanged += new DataGridViewHelper.SelectedRowsChangedEventHandler(SubjectDataGridHelper_UpdateSelected);
                this.SubjectDataGridHelper.ViewDetails += new DataGridViewHelper.ViewDetailsEventHandler(DataGridHelper_ViewDetails);
                grdSubjectSequences.AutoGenerateColumns = false;
                grdSubjectSequences.DataSource = new SortableBindingList<BlastNAlignmentRow>(BlastNAtNCBI.ListAlignedGenes(this.Gene.ID, Program.Settings.CurrentRecordSet.ID).ToBlastNAlignmentRowList());
                btnMerge.Enabled = (grdSubjectSequences.Rows.Count != 0);
            }
            else if (e.TabPage == pgQuerySequences && grdQuerySequences.DataSource == null)
            {
                // Download query sequences
                this.QueryDataGridHelper = new DataGridViewHelper(this, grdQuerySequences);
                this.QueryDataGridHelper.ViewDetails += new DataGridViewHelper.ViewDetailsEventHandler(DataGridHelper_ViewDetails);
                grdQuerySequences.AutoGenerateColumns = false;
                grdQuerySequences.DataSource = new SortableBindingList<BlastNAlignmentRow>(BlastNAtNCBI.ListInputGenes(this.Gene.ID, 0, string.Empty, Program.Settings.CurrentRecordSet.ID).ToBlastNAlignmentRowList());
            }

            this.CancelButton = CancelButtons[e.TabPage];
        }

        private void SubjectDataGridHelper_UpdateSelected(DataGridViewHelper.SelectedRowEventArgs e)
        {
            btnAddTo.Enabled = SelectedGeneRows.Count != 0;
        }

        private void btnMerge_Click(object sender, EventArgs e)
        {
            BlastSequencesAtNCBI.Alignment alignment = new BlastSequencesAtNCBI.Alignment() {
                Query = this.Gene,
                HasAlignments = (grdSubjectSequences.Rows.Count != 0),
                Alignments = ((SortableBindingList<BlastNAlignmentRow>)grdSubjectSequences.DataSource).ToList(),
                Merged = ((BlastNAlignmentRow)grdSubjectSequences.SelectedRows[0].DataBoundItem).Gene,
                ReplaceSequence = Program.DatabaseSettings.BlastNAtNCBI.Annotate_ReplaceAllSequences
            };

            using (BlastN.frmAnnotatedGeneDetails frm = new BlastN.frmAnnotatedGeneDetails() { Alignment = alignment, ConfirmUpdate = true })
            {
                if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    this.Gene.Definition = alignment.Merged.Definition;
                    this.Gene.Organism = alignment.Merged.Organism;
                    this.Gene.Taxonomy = alignment.Merged.Taxonomy;
                    this.Gene.GeneName = alignment.Merged.GeneName;
                    this.Gene.Accession = alignment.Merged.Accession;
                    this.Gene.Locus = alignment.Merged.Locus;
                    if (alignment.Merged.GenBankID != 0)
                    { this.Gene.GenBankID = alignment.Merged.GenBankID; }
                    else
                    { this.Gene.GenBankID = 0; }
                    this.Gene.LastUpdatedAt = DateTime.Now;
                    this.Gene.LastUpdateSource = GeneSources.User;

                    if (alignment.ReplaceSequence)
                    {
                        this.Gene.Nucleotides = alignment.Merged.Nucleotides;
                        this.Gene.SequenceTypeID = alignment.Merged.SequenceTypeID;
                        this.Gene.SourceSequence = alignment.Merged.SourceSequence;
                        this.Gene.Features = alignment.Merged.Features;
                    }

                    this.Gene.Save(true, alignment.ReplaceSequence);
                    this.Gene.Merge(Gene.Get(this.Gene.ID, true), true, true, true); // Refresh directly from the database to pick up things like TaxonomyHierarchy
                    this.Updated = true;

                    this.UpdatedSubSetIDs.AddRange(ChangLab.RecordSets.SubSet.ListSubSetIDsForGeneIDs(new string[] { this.Gene.ID }).Where(id => !this.UpdatedSubSetIDs.Contains(id)));

                    btnReset_Click(btnReset, EventArgs.Empty);
                    tbForm.SelectedTab = pgDetails;
                }
            }
        }

        private void DataGridHelper_ViewDetails(DataGridViewHelper.ViewDetailsEventArgs e)
        {
            GenericGeneRowDataItem row = (GenericGeneRowDataItem)e.Row.DataBoundItem;
            using (frmGeneDetails frm = new frmGeneDetails(row.Gene.ID) { SaveUpdates = true })
            {
                frm.ShowDialog(this);
                if (frm.Updated)
                {
                    row.Gene.Merge(frm.Gene); // No need to save because frmGeneDetails will already have done so.
                    e.Row.Selected = false; e.Row.Selected = true; // Refresh the row
                }
            }
        }
        #endregion
    }
}
