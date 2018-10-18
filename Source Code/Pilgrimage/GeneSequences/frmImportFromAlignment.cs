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
using ChangLab.RecordSets;

namespace Pilgrimage.GeneSequences
{
    public partial class frmImportFromAlignment : DialogForm
    {
        private string OpenOnLoad { get; set; }
        private List<Gene> AllGenes { get; set; }
        private Dictionary<Gene, string> AllGeneHeaders { get; set; }
        private List<string> FileContents { get; set; }
        
        private string _selectedSubSetID;
        public string SelectedSubSetID
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_selectedSubSetID))
                { return (string)cmbSubSets.SelectedValue; }
                else
                { return _selectedSubSetID; }
            }
        }

        public frmImportFromAlignment() : this(string.Empty) { }

        public frmImportFromAlignment(string FileName)
        {
            InitializeComponent();
            lblInstructions.Text = string.Format(lblInstructions.Text, Program.ProductName);

            if (Program.DatabaseSettings.ImportAlignedSequences_ShowInstructions)
            {
                lnkInstructions.Text = "Hide instructions";
            }
            else
            {
                lnkInstructions.Text = "Show instructions";
                lblInstructions.Parent.Controls.Remove(lblInstructions);
            }

            txtFormatString.Text = Program.DatabaseSettings.FASTAHeaderFormatString;
            SetButtonImage(btnRefresh, "Refresh");
            SetButtonImage(btnImport, "Import");
            SetButtonImage(btnCancel, "Cancel");

            List<SubSet> subSets = Program.Settings.AllSubSets(DataTypes.GeneSequence).OrderBy(sub => sub.Name).ToList();
            subSets.Insert(0, new SubSet(DataTypes.GeneSequence) { Name = "--- New Dataset ---" });
            
            cmbSubSets.DisplayMember = "Name";
            cmbSubSets.ValueMember = "ID";
            cmbSubSets.DataSource = new BindingSource(subSets, null);
            cmbSubSets.SelectedIndex = 0;

            this.FocusOnLoad = grdImport;
            this.OpenOnLoad = FileName;
        }

        private void frmImportFromFASTA_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this.OpenOnLoad) && System.IO.File.Exists(this.OpenOnLoad))
            {
                FileContents = System.IO.File.ReadAllLines(this.OpenOnLoad).ToList();
                LineUpGenes(FileContents);
                return;
            }
            
            System.IO.FileInfo file = null;
            if (IODialogHelper.OpenFile(IODialogHelper.DialogPresets.FASTA, this, ref file))
            {
                FileContents = System.IO.File.ReadAllLines(file.FullName).ToList();
                LineUpGenes(FileContents);
            }
            else { this.DialogResult = System.Windows.Forms.DialogResult.Cancel; }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFormatString.Text))
            {
                Utility.ShowMessage(this, "Header format cannot be empty.");
            }
            else
            {
                LineUpGenes(FileContents);
            }
        }

        private void lnkFormatString_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (frmFormatFieldNames frm = new frmFormatFieldNames(txtFormatString.Text, AllGenes.ElementAt((new Random()).Next(0, AllGenes.Count - 1))))
            {
                if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    txtFormatString.Text = frm.FormatString;
                }
            }
        }

        private void LineUpGenes(List<string> Lines)
        {
            AllGenes = Program.Settings.CurrentRecordSet
                .ListAllGenes()
                .OrderBy(g => g.Definition)
                .ToList();
            AllGeneHeaders = AllGenes.ToDictionary(g => g, g => System.Text.RegularExpressions.Regex.Replace(g.ToFASTAHeader(txtFormatString.Text), "[^A-Za-z0-9]", ""));
            AllGenes.Insert(0, new Gene(string.Empty) { Definition = string.Empty, Description = string.Empty });
            
            clmGeneName.DisplayMember = "Definition";
            clmGeneName.ValueMember = "ID";
            clmGeneName.DataSource = new BindingSource(AllGenes, null);
            
            List<Gene> importGenes = new List<Gene>();
            Gene importGene = null;
            for (int i = 0; i < Lines.Count; i++)
            {
                if (Lines[i].TrimStart().StartsWith(">"))
                {
                    importGene = new Gene()
                    {
                        Definition = Lines[i].Substring(1),
                        // Description is just being used as a placeholder for the Regex'd version of the header that we'll be matching with.
                        Description = System.Text.RegularExpressions.Regex.Replace(Lines[i].Substring(1), "[^A-Za-z0-9]", "")
                    };
                    importGenes.Add(importGene);
                }
                else
                {
                    if (importGene != null)
                    {
                        importGene.Nucleotides += Lines[i];
                    }
                }
            }

            grdImport.AutoGenerateColumns = false;
            grdImport.DataSource = new BindingSource(importGenes, null);
        }

        private void grdImport_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in grdImport.Rows)
            {
                Gene sourceGene = ((Gene)row.DataBoundItem);
                Gene match = null; // AllGenes.FirstOrDefault(g => g.Organism == sourceGene.Organism);
                double matchPercentage = 0;
                for (int i = 0; i < sourceGene.Description.Length; i += 5)
                {
                    match = AllGenes.Skip(1).FirstOrDefault(g =>
                    {
                        string header = AllGeneHeaders[g];
                        if (header.Length > i)
                        {
                            return sourceGene.Description.Substring(0, (sourceGene.Description.Length - i)).StartsWith(header.Substring(0, header.Length - i));
                        }
                        else { return false; }
                    });

                    if (match != null)
                    {
                        matchPercentage = Math.Round(Convert.ToDouble(sourceGene.Description.Length - i) / Convert.ToDouble(sourceGene.Description.Length), 2);
                        break;
                    }
                }

                if (match != null)
                {
                    ((DataGridViewComboBoxCell)row.Cells[clmGeneName.Name]).Value = match.ID;
                    row.Cells[clmMatchPercentage.Name].Value = matchPercentage; //.ToString() + "%";
                }
                else
                {
                    row.Cells[clmMatchPercentage.Name].Value = 0D; // "0%";
                }
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SelectedSubSetID))
            {
                using (RecordSets.frmEditSubSet frm = new RecordSets.frmEditSubSet(new SubSet(DataTypes.GeneSequence)))
                {
                    if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        _selectedSubSetID = frm.CurrentSubSet.ID;
                    }
                    else { return; }
                }
            }
            
            List<Gene> importGenes = new List<Gene>();

            foreach (DataGridViewRow row in grdImport.Rows)
            {
                string id = ((DataGridViewComboBoxCell)row.Cells[clmGeneName.Name]).Value.ToSafeString();
                if (!string.IsNullOrWhiteSpace(id))
                {
                    Gene match = AllGenes.FirstOrDefault(g => GuidCompare.Equals(g.ID, id));
                    if (match != null)
                    {
                        Gene newGene = new Gene();
                        newGene.SourceID = GeneSource.IDByKey(GeneSources.MEGA);
                        newGene.Merge(match);

                        Gene sourceGene = ((Gene)row.DataBoundItem);
                        newGene.Nucleotides = sourceGene.Nucleotides;
                        newGene.SourceSequence = new NucleotideSequence(sourceGene.Nucleotides, 1);

                        // Wipe whatever features were in the existing Gene to replace with a simple annotation.
                        newGene.Features = new List<Feature>();
                        newGene.Features.Add(new Feature() { FeatureKey = GeneFeatureKeyCollection.Get(GeneFeatureKeys.gene), Rank = 1 });
                        newGene.Features[0].Intervals.Add(new FeatureInterval() { Start = newGene.SourceSequence.Start, End = newGene.SourceSequence.End });

                        importGenes.Add(newGene);
                    }
                }
            }

            if (Utility.ShowMessage(this, importGenes.Count.ToString() + " project genes matched to " + grdImport.Rows.Count.ToString() + " genes in the selected file."
                                        + "\r\n" + "The " + importGenes.Count.ToString() + " matched genes will be created as new sequence records using the nucleotide sequences from the selected file.",
                                        MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.OK)
            {
                // Create new Gene.Gene records using the imported nucleotide sequence data.
                importGenes.ForEach(g =>
                {
                    // Create the new records.
                    g.Save(true, true);
                    // Associate them with the selected subset.
                    Program.Settings.CurrentRecordSet.AddGene(g, SelectedSubSetID);
                    // Update the recordset's ModifiedAt value.
                    Program.Settings.CurrentRecordSet.Save();
                });

                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }          
        }

        private void lnkInstructions_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (Program.DatabaseSettings.ImportAlignedSequences_ShowInstructions)
            {
                lnkInstructions.Text = "Show instructions";
                lblInstructions.Parent.Controls.Remove(lblInstructions);
            }
            else
            {
                lnkInstructions.Text = "Hide instructions";
                tblForm.Controls.Add(lblInstructions, 0, 0);
                tblForm.SetColumnSpan(lblInstructions, tblForm.ColumnCount);
            }

            Program.DatabaseSettings.ImportAlignedSequences_ShowInstructions = !Program.DatabaseSettings.ImportAlignedSequences_ShowInstructions;
        }
    }
}
