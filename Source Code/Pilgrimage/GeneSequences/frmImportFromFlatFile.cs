using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChangLab.Common;
using ChangLab.Genes;

namespace Pilgrimage.GeneSequences
{
    public partial class frmImportFromFlatFile : AddGeneSequencesToRecordSetForm
    {
        internal GeneSources FileSource { get; set; }
        private Dictionary<GeneSources, int> SourceIDs { get; set; }

        private IODialogHelper.DialogPresets DialogPreset
        {
            get
            {
                switch (this.FileSource)
                {
                    case GeneSources.FASTA:
                    case GeneSources.Trinity:
                        return IODialogHelper.DialogPresets.FASTA;
                    case GeneSources.NEXUS:
                        return IODialogHelper.DialogPresets.Text;
                    default:
                        return IODialogHelper.DialogPresets.All;
                }
            }
        }

        private FileInfo ImportFile { get; set; }

        /// <remarks>
        /// The string value will populate the Header property in ImportedFromFileGeneRow, corresponding to the header that was extracted from the
        /// file as opposed to a header set by the user.
        /// </remarks>
        private Dictionary<Gene, string> ImportGenes { get; set; }

        public frmImportFromFlatFile() : this(GeneSources.FASTA) { }

        public frmImportFromFlatFile(GeneSources FileSource)
        {
            InitializeComponent();
            this.FileSource = FileSource;

            SourceIDs = new Dictionary<GeneSources, int>();
            SourceIDs.Add(GeneSources.FASTA, GeneSource.IDByKey(GeneSources.FASTA));
            SourceIDs.Add(GeneSources.Trinity, GeneSource.IDByKey(GeneSources.Trinity));
            SourceIDs.Add(GeneSources.NEXUS, GeneSource.IDByKey(GeneSources.NEXUS));
            SourceIDs.Add(GeneSources.PHYLIP, GeneSource.IDByKey(GeneSources.PHYLIP));

            txtFormatString.Text = Program.DatabaseSettings.FASTAHeaderFormatString;

            Configure(grdResults, cmbSubSets, btnSave, ChangLab.RecordSets.DataTypes.GeneSequence);
            cmbSubSets.SelectedIndex = 0;
            // At the moment we're avoiding overwriting the user's data with GenBank data, even if there's a GenBank ID in the FASTA header.
            this.PerformUpdateFromGenBank = false;
            this.AddedGenes += new AddedGeneUpdatesEventHandler((CallerForm, AddedGenesArgs) => { UpdateGridControls(); });

            SetButtonImage(btnSave, "Import");
            SetButtonImage(btnCancel, DialogButtonPresets.Close);
        }

        private void frmImportFromFASTA_Load(object sender, EventArgs e)
        {
            switch (this.FileSource)
            {
                case GeneSources.Trinity:
                    lblHeaderFormat.Parent.Controls.Remove(lblHeaderFormat);
                    txtFormatString.Parent.Controls.Remove(txtFormatString);
                    lnkFormatString.Parent.Controls.Remove(lnkFormatString);
                    btnLoadFromFile.Parent.Controls.Remove(btnLoadFromFile);
                    clmOrganism.Visible = false;
                    clmLocus.Visible = true;

                    this.Text = "Import from Trinity FASTA file";
                    break;
                case GeneSources.NEXUS:
                    this.Text = "Import from NEXUS file";
                    break;
                case GeneSources.PHYLIP:
                    this.Text = "Import from PHYLIP file";
                    break;
            }

            this.SubjectDataGridHelper = new DataGridViewHelper(this, grdResults, chkToggleSelected);
            this.SubjectDataGridHelper.ViewDetails += new DataGridViewHelper.ViewDetailsEventHandler(DataGridHelper_ViewDetails);
            this.SubjectDataGridHelper.SelectedRowsChanged += new DataGridViewHelper.SelectedRowsChangedEventHandler(DataGridHelper_SelectedRowsChanged);

            UpdateGridControls();

            btnBrowseForFile.PerformClick();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (grdResults.Rows.Cast<DataGridViewRow>().Any(row => !((ImportedFromFileGeneRow)row.DataBoundItem).InRecordSet))
            {
                if (Utility.ShowMessage(this, "All results parsed from the selected file have not yet been imported.\r\n\r\nAre you sure you want to close?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    == System.Windows.Forms.DialogResult.No)
                { return; }
            }

            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void btnBrowseForFile_Click(object sender, EventArgs e)
        {
            FileInfo file = null;
            if (IODialogHelper.OpenFile(this.DialogPreset, this, ref file))
            {
                ImportFile = file;
                txtFilePath.Text = file.FullName;

                if (this.FileSource == GeneSources.Trinity)
                {
                    btnLoadFromFile_Click(btnBrowseForFile, EventArgs.Empty);
                }
            }
        }

        private void btnLoadFromFile_Click(object sender, EventArgs e)
        {
            if (!File.Exists(txtFilePath.Text)) { Utility.ShowMessage(this, "File not found.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
            else
            {
                ImportGenes = new Dictionary<Gene, string>();

                switch (FileSource)
                {
                    case GeneSources.FASTA:
                    case GeneSources.Trinity:
                        List<string> Lines = System.IO.File.ReadAllLines(txtFilePath.Text).ToList();

                        Gene gene = null; string nucleotides = string.Empty;
                        for (int i = 0; i < Lines.Count; i++)
                        {
                            if (Lines[i].StartsWith(">"))
                            {
                                if (gene != null)
                                {
                                    // Any changes to this must be replicated below the for{} loop.
                                    gene.Nucleotides = nucleotides;
                                    if (!string.IsNullOrEmpty(nucleotides))
                                    {
                                        gene.SourceSequence = new NucleotideSequence(nucleotides, 1);
                                        gene.Features.Add(gene.CreateFeatureFromSequence(GeneFeatureKeys.gene));
                                    }
                                }

                                //if (txtFormatString.Text.Contains("{Definition}")
                                string header = Lines[i];
                                gene = ExtractGeneFromHeader(ref header);
                                ImportGenes.Add(gene, header);
                                nucleotides = string.Empty;
                            }
                            else
                            {
                                if (gene != null)
                                {
                                    nucleotides += Lines[i];
                                }
                            }
                        }
                        if (gene != null)
                        {
                            // Any changes to this must be replicated above in the for{} loop.
                            gene.Nucleotides = nucleotides;
                            if (!string.IsNullOrEmpty(nucleotides))
                            {
                                gene.SourceSequence = new NucleotideSequence(nucleotides, 1);
                            }
                        }
                        break;

                    case GeneSources.NEXUS:
                        ChangLab.Genes.FileParsing.NexusFileParser nexus = new ChangLab.Genes.FileParsing.NexusFileParser();
                        List<Gene> nexusGenes = nexus.ParseFile(txtFilePath.Text);

                        nexusGenes.ForEach(ng =>
                            {
                                string header = ng.Definition;
                                Gene g = ExtractGeneFromHeader(ref header);

                                g.Nucleotides = ng.Nucleotides;
                                g.SourceSequence = ng.SourceSequence;
                                g.SequenceTypeID = ng.SequenceTypeID;

                                ImportGenes.Add(g, header);
                            }
                        );
                        break;
                    case GeneSources.PHYLIP:
                        ChangLab.Genes.FileParsing.PhylipFileParser phylip = new ChangLab.Genes.FileParsing.PhylipFileParser();
                        phylip.ParseFile(txtFilePath.Text).ForEach(g => ImportGenes.Add(g, g.Definition));
                        break;
                }

                RefreshGrid(null);
                Program.DatabaseSettings.FASTAHeaderFormatString = txtFormatString.Text;
            }
        }

        private Gene ExtractGeneFromHeader(ref string Header)
        {
            if (Header.StartsWith(">")) { Header = Header.Substring(1); }

            if (this.FileSource == GeneSources.Trinity)
            { return ExtractGeneFromHeaderAsTrinity(ref Header); }

            string headerMask = txtFormatString.Text.Trim();

            Dictionary<string, string> fields = NewFieldDataCollection();
            string description = string.Empty; // Holding this field separately for convenience.

            bool headerConformsToMask = true;
            if (!string.IsNullOrWhiteSpace(headerMask))
            {
                int headerIndex = 0;
                int closeIndex = -1;
                int afterFieldCharacterIndexInHeader = -1;
                string fieldName = string.Empty;
                string afterFieldCharacter = string.Empty;
                string fieldData = string.Empty;

                for (int i = 0; i < headerMask.Length; i++)
                {
                    if (headerMask[i] == '{')
                    {
                        closeIndex = headerMask.IndexOf('}', i);
                        if (closeIndex != -1) // Making sure this isn't just an open curly bracket
                        {
                            fieldName = headerMask.Substring(i, (closeIndex - i) + 1);

                            if (closeIndex == (headerMask.Length - 1)) // The close bracket is at the end of the mask
                            {
                                fieldData = Header.Substring(headerIndex);
                                if (fields.ContainsKey(fieldName)) { fields[fieldName] = fieldData; }
                                else
                                {
                                    description += (description.Length == 0 ? string.Empty : "\r\n") + fieldName.Replace("{", "").Replace("}", "") + ": " + fieldData;
                                }
                                i = closeIndex + 1;
                            }
                            else
                            {
                                afterFieldCharacter = headerMask[closeIndex + 1].ToString();
                                afterFieldCharacterIndexInHeader = Header.IndexOf(afterFieldCharacter, headerIndex);

                                // TODO: What if the afterField character is a "{", in other words the user didn't delimit the fields?

                                if (afterFieldCharacterIndexInHeader != -1)
                                {
                                    fieldData = Header.Substring(headerIndex, (afterFieldCharacterIndexInHeader - headerIndex));
                                    if (fields.ContainsKey(fieldName)) { fields[fieldName] = fieldData; }
                                    else
                                    {
                                        description += (description.Length == 0 ? string.Empty : "\r\n") + fieldName.Replace("{", "").Replace("}", "") + ": " + fieldData;
                                    }

                                    headerIndex = afterFieldCharacterIndexInHeader + 1;
                                    i = closeIndex + 1;
                                }
                                else
                                {
                                    // The header doesn't match the mask format.
                                    headerConformsToMask = false;
                                    break;
                                }
                            }
                        }
                        else { headerIndex++; }
                    }
                    else
                    {
                        headerIndex++;
                    }
                }
            }

            if (!headerConformsToMask)
            {
                fields = NewFieldDataCollection(); // Reset the collection to scrap whatever was extracted.
                fields["{Definition}"] = Header;
            }

            description += (description.Length == 0 ? string.Empty : "\r\n\r\n") + FileSource.ToString() + " header: " + Header;

            Gene gene = new Gene()
            {
                SourceID = SourceIDs[FileSource],
                // Source is not a maskable field because then someone could potentially import from a flat file a record sourced as BLASTN or 
                // GenBank, which would conflict with the notion of NCBI-sourced records being unique.  In order for the whole aggregation of BLASTN
                // alignment results thing to work we have to have uniqueness for GenBankID and source of BLASTN/GenBank.
                GeneName = fields["{Gene}"],
                Definition = fields["{Definition}"],
                GenBankID = fields["{GenBank ID}"].ToSafeInt(),
                Locus = fields["{Locus}"],
                Accession = fields["{Accession}"],
                Organism = fields["{Organism}"],
                Description = description,

                LastUpdatedAt = ImportFile.LastWriteTime,
                LastUpdateSourceID = SourceIDs[FileSource],
                SequenceType = GeneSequenceTypes.NotDefined
            };

            return gene;
        }

        private Gene ExtractGeneFromHeaderAsTrinity(ref string Header)
        {
            string description = "FASTA header: " + Header;

            Gene gene = new Gene()
            {
                SourceID = SourceIDs[GeneSources.Trinity],
                Definition = Header,
                Locus = Header.Substring(0, Header.IndexOf(" ")),
                Description = description,

                LastUpdatedAt = ImportFile.LastWriteTime,
                LastUpdateSourceID = SourceIDs[GeneSources.FASTA],
                SequenceType = GeneSequenceTypes.NotDefined
            };

            return gene;
        }

        private Dictionary<string, string> NewFieldDataCollection()
        {
            Dictionary<string, string> fields = new Dictionary<string, string>();
            fields.Add("{Definition}", string.Empty);
            fields.Add("{Source}", string.Empty);
            fields.Add("{GenBank ID}", string.Empty);
            fields.Add("{Locus}", string.Empty);
            fields.Add("{Accession}", string.Empty);
            fields.Add("{Organism}", string.Empty);
            fields.Add("{Gene}", string.Empty);
            return fields;
        }

        private void RefreshGrid(Gene SelectedGene)
        {
            grdResults.AutoGenerateColumns = false;
            grdResults.DataSource = null;
            grdResults.DataSource = new SortableBindingList<ImportedFromFileGeneRow>(ImportGenes.Select(g => new ImportedFromFileGeneRow(g.Key, g.Value)));

            if (SelectedGene != null)
            {
                DataGridViewRow selectedRow = grdResults.Rows.Cast<DataGridViewRow>().FirstOrDefault(r => ((ImportedFromFileGeneRow)r.DataBoundItem).Gene == SelectedGene);
                if (selectedRow != null) { selectedRow.Selected = true; }
            }

            UpdateGridControls();
        }

        private void DataGridHelper_ViewDetails(DataGridViewHelper.ViewDetailsEventArgs e)
        {
            Gene gene = ((ImportedFromFileGeneRow)e.Row.DataBoundItem).Gene;
            using (GeneSequences.frmGeneDetails frm = new GeneSequences.frmGeneDetails(gene, false, null))
            {
                frm.InitalizeAsEditable();
                frm.ShowDialog(this.OwnerForm);

                if (frm.Updated)
                {
                    gene.LastUpdatedAt = DateTime.Now;
                    gene.LastUpdateSource = GeneSources.User;
                }

                DataGridViewRow selectedRow = grdResults.Rows.Cast<DataGridViewRow>().FirstOrDefault(r => ((ImportedFromFileGeneRow)r.DataBoundItem).Gene == gene);
                if (selectedRow != null) { selectedRow.Selected = false; selectedRow.Selected = true; /* Allows for the row to be redrawn and updated */ };
            }
        }

        private void DataGridHelper_SelectedRowsChanged(DataGridViewHelper.SelectedRowEventArgs e)
        {
            UpdateGridControls();
        }

        private void UpdateGridControls()
        {
            lblSelectedRows.Text = this.SelectedGenes.Count.ToString("N0") + " of " + grdResults.Rows.Count.ToString("N0") + " records selected";
            btnSave.Enabled = (SelectedGeneRows.Count != 0);
        }

        private void lnkFormatString_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (frmFormatFieldNames frm = new frmFormatFieldNames(txtFormatString.Text, null))
            {
                if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    txtFormatString.Text = frm.FormatString;
                }
            }
        }
    }
}