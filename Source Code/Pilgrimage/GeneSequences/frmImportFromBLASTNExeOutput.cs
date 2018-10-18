using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChangLab.BlastN;
using ChangLab.Common;
using ChangLab.Genes;
using ChangLab.NCBI.LocalDatabase;

namespace Pilgrimage.GeneSequences
{
    public partial class frmImportFromBLASTNExeOutput : AddGeneSequencesToRecordSetForm
    {
        private FileInfo ImportFile { get; set; }
        /// <remarks>
        /// The string value will populate the Header property in ImportedFromFileGeneRow, corresponding to the header that was extracted from the
        /// file as opposed to a header set by the user.
        /// </remarks>
        private Dictionary<Gene, string> ImportGenes { get; set; }
        private List<ImportedFromFileGeneRow> DataSource { get; set; }
        private int SourceID_BLASTNExe { get; set; }

        public frmImportFromBLASTNExeOutput()
        {
            InitializeComponent();
            SourceID_BLASTNExe = GeneSource.IDByKey(GeneSources.BLASTN_Local);

            Configure(grdResults, cmbSubSets, btnSave, ChangLab.RecordSets.DataTypes.GeneSequence);
            cmbSubSets.SelectedIndex = 0;
            // At the moment we're avoiding overwriting the user's data with GenBank data, even if there's a GenBank ID in the FASTA header.
            this.PerformUpdateFromGenBank = false;
            this.AddedGenes += new AddedGeneUpdatesEventHandler((CallerForm, AddedGenesArgs) => { UpdateGridControls(); });

            SetButtonImage(btnSave, "Import");
            SetButtonImage(btnCancel, DialogButtonPresets.Close);
        }

        private void frmImportFromBLASTNExeOutput_Load(object sender, EventArgs e)
        {
            this.SubjectDataGridHelper = new DataGridViewHelper(this, grdResults, chkToggleSelected);
            this.SubjectDataGridHelper.ViewDetails += new DataGridViewHelper.ViewDetailsEventHandler(DataGridHelper_ViewDetails);
            this.SubjectDataGridHelper.SelectedRowsChanged += new DataGridViewHelper.SelectedRowsChangedEventHandler(DataGridHelper_SelectedRowsChanged);

            UpdateGridControls();

            btnBrowseForFile.PerformClick();
        }

        private void btnBrowseForFile_Click(object sender, EventArgs e)
        {
            FileInfo file = null;
            if (IODialogHelper.OpenFile("Text files (*.txt)|*.txt|FASTA files (*.fas*)|*.fas*", "txt", txtFilePath.Text, this.OwnerForm, ref file, true))
            {
                ImportFile = file;
                txtFilePath.Text = file.FullName;

                LoadFromFile();
            }
        }

        private void LoadFromFile()
        {
            FileInfo inputFile = new FileInfo(txtFilePath.Text);

            if (!inputFile.Exists) { Utility.ShowMessage(this, "File not found.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
            else
            {
                BlastNLocalDatabase local = new BlastNLocalDatabase();
                List<Query> queries = local.ParseOutputFile(null, inputFile.FullName);

                // Now convert the list of Query objects into a list of Gene objects based on what was parsed out of the file.
                // ParseOutputFile() is dual-purpose, originally for when Pilgrimage BLASTs a sequence against a local database, not for importing
                // gene sequence records from an output file.  In the former case it would already know the input (Query) genes, whereas in the
                // latter the input genes are unknown to the database.

                ImportGenes = new Dictionary<Gene, string>();
                Gene gene = null;
                foreach (var query in queries)
                {
                    foreach (var subject in query.LocalAlignments)
                    {
                        gene = new Gene();
                        gene.Merge(subject.OutputGene);
                        gene.SourceID = this.SourceID_BLASTNExe;
                        gene.LastUpdatedAt = inputFile.LastWriteTime;
                        gene.LastUpdateSourceID = this.SourceID_BLASTNExe;

                        if (subject.Alignment != null) { gene.Exceptions.AddRange(subject.Alignment.Warnings); }
                        gene.Exceptions.AddRange(subject.Exceptions);

                        gene.Description = "Imported from " + inputFile.FullName
                            + (!string.IsNullOrEmpty(gene.Locus) ? "\r\n" + "Contig number: " + gene.Locus : string.Empty);
                        if ((subject.Alignment != null) && (subject.Alignment.Warnings.Count != 0))
                        {
                            gene.Description += "\r\n\r\nWarnings from parsing sequence:\r\n"
                                + subject.Alignment.Warnings.Aggregate(string.Empty, (current, ex) => { current += ex.Message + "\r\n"; return current; });
                        }
                        if (subject.Exceptions.Count != 0)
                        {
                            gene.Description += "\r\n\r\nErrors from parsing sequence:\r\n"
                                + subject.Exceptions.Aggregate(string.Empty, (current, ex) => { current += ex.Message + "\r\n"; return current; });
                        }

                        if (subject.Alignment != null)
                        {
                            gene.Nucleotides = subject.Alignment.Nucleotides;
                            // If the query sequence used with BLASTN.exe was a CDS, then the output subject sequence will be as well, in which case
                            // the file will contain sequence fragments that are the exons, but not the whole source sequence.  Without the source
                            // sequence we can't effectively annotate, because all we have are exons without the introns in between.
                            // To deal with this, we take the subject sequence from BLASTN.exe and treat it as a CDS, with a single feature with one
                            // interval coded as such, and lose the data we have on the intervals and their lengths.
                            // A possible future solution would be to allow for a Gene.Gene record to have a backing sequence that is just an array of
                            // fragments, in which case the sequence editing screens would need to be modified to allow for that and not freak out
                            // when they don't have a source sequence that they're working with.
                            // Easy Mode on that would be to just fill in the introns with dashes and treat them as gaps.
                            gene.SourceSequence = new NucleotideSequence(gene.Nucleotides, 1);
                            gene.Features.Add(new Feature()
                            {
                                FeatureKey = GeneFeatureKeyCollection.Get(GeneFeatureKeys.CDS),
                                Intervals = new List<FeatureInterval>(new FeatureInterval[] { new FeatureInterval() { Start = 1, End = gene.Nucleotides.Length } }),
                                Rank = 1
                            });
                        }
                        else { gene.Nucleotides = string.Empty; }

                        //if (subject.Alignment.AlignmentRange.Start > subject.Alignment.AlignmentRange.End)
                        //{
                        //    // Flip the sequence
                        //    gene.Nucleotides = gene.Nucleotides.Reverse().Aggregate(string.Empty, (current, c) => current += c);
                        //}
                        //gene.SourceSequence = new NucleotideSequence(gene.Nucleotides, Math.Min(subject.Alignment.AlignmentRange.Start, subject.Alignment.AlignmentRange.End));

                        //gene.Features.AddRange(subject.Exons.OrderBy(exon => exon.QueryRangeStart).Select((exon, index) => new Feature()
                        //{
                        //    FeatureKey = GeneFeatureKeyCollection.Get(GeneFeatureKeys.CDS),
                        //    Intervals = new List<FeatureInterval>(exon.SequenceFragments.Select(frag => new FeatureInterval()
                        //        {
                        //            Start = frag.SubjectRange.Start,
                        //            End = frag.SubjectRange.End,
                        //        })),
                        //    Rank = (index + 1)
                        //}));

                        gene.SequenceType = GeneSequenceTypes.Alignment;
                        ImportGenes.Add(gene, subject.OutputGene.Definition);
                    }
                }

                this.DataSource = ImportGenes.Select(g => new ImportedFromFileGeneRow(g.Key, g.Value) { ExceptionsImage = (g.Key.Exceptions.Count != 0 ? Properties.Resources.Warning_16 : Properties.Resources.Transparent_16) }
                ).ToList();
                RefreshGrid();
            }
        }

        private void RefreshGrid()
        {
            grdResults.AutoGenerateColumns = false;
            grdResults.DataSource = null;

            bool showImported = chkToggleImported.Checked;
            var qry = this.DataSource.Where(row => (showImported ? true : !row.InRecordSet));
            grdResults.DataSource = new SortableBindingList<ImportedFromFileGeneRow>(qry);

            UpdateGridControls();
        }

        private void DataGridHelper_ViewDetails(DataGridViewHelper.ViewDetailsEventArgs e)
        {
            Gene gene = ((ImportedFromFileGeneRow)e.Row.DataBoundItem).Gene;
            using (GeneSequences.frmGeneDetails frm = new GeneSequences.frmGeneDetails(gene, false, null))
            {
                frm.InitalizeAsEditable();
                frm.ShowDialog(this.OwnerForm);

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
            this.SubjectDataGridHelper.ToggleToggleAllCheckBox();
            lblSelectedRows.Text = this.SelectedGenes.Count.ToString("N0") + " of " + grdResults.Rows.Count.ToString("N0") + " records selected";
            btnSave.Enabled = (SelectedGeneRows.Count != 0);
        }

        private void chkToggleImported_CheckedChanged(object sender, EventArgs e)
        {
            RefreshGrid();
        }
    }
}
