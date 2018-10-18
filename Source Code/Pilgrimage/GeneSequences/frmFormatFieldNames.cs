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

namespace Pilgrimage.GeneSequences
{
    public partial class frmFormatFieldNames : Form
    {
        internal string FormatString
        {
            get { return txtFormat.Text; }
            set { txtFormat.Text = value; }
        }
        private Gene ExampleGene
        {
            get;
            set;
        }

        public frmFormatFieldNames(string FormatString, Gene ExampleGene)
        {
            InitializeComponent();

            lstFieldNames.Items.Clear();
            lstFieldNames.Items.AddRange(Utility.GeneDataFields().Where(kv => kv.Key != "Nucleotides").Select(kv => "{" + kv.Value + "}").ToArray());
            
            this.ExampleGene = ExampleGene;
            if (this.ExampleGene == null)
            {
                this.ExampleGene = new Gene()
                {
                    Accession = "NM_000329.2",
                    GeneName = "RPE65",
                    Definition = "Homo sapiens retinal pigment epithelium-specific protein 65kDa (RPE65), mRNA",
                    GenBankID = 67188783,
                    LastUpdatedAt = new DateTime(2014, 5, 15),
                    Locus = "NM_000329",
                    Organism = "Homo sapiens",
                    Nucleotides = "ATGTCTATCCAGGTTGAGCATCCTGCTGGTGGTTACAAGAAACTGTTTGAAACTGTGGAGGAACTGTCCTCGCCGCTCACAGCTCATGTAACAGGCAGGATCCCCCTCTGGCTCACCGGCAGTCTCCTTCGATGTGGGCCAGGACTCTTTGAAGTTGGATCTGAGCCATTTTACCACCTGTTTGATGGGCAAGCCCTCCTGCACAAGTTTGACTTTAAAGAAGGACATGTCACATACCACAGAAGGTTCATCCGCACTGATGCTTACGTACGGGCAATGACTGAGAAAAGGATCGTCATAACAGAATTTGGCACCTGTGCTTTCCCAGATCCCTGCAAGAATATATTTTCCAGGTTTTTTTCTTACTTTCGAGGAGTAGAGGTTACTGACAATGCCCTTGTTAATGTCTACCCAGTGGGGGAAGATTACTACGCTTGCACAGAGACCAACTTTATTACAAAGATTAATCCAGAGACCTTGGAGACAATTAAGCAGGTTGATCTTTGCAACTATGTCTCTGTCAATGGGGCCACTGCTCACCCCCACATTGAAAATGATGGAACCGTTTACAATATTGGTAATTGCTTTGGAAAAAATTTTTCAATTGCCTACAACATTGTAAAGATCCCACCACTGCAAGCAGACAAGGAAGATCCAATAAGCAAGTCAGAGATCGTTGTACAATTCCCCTGCAGTGACCGATTCAAGCCATCTTACGTTCATAGTTTTGGTCTGACTCCCAACTATATCGTTTTTGTGGAGACACCAGTCAAAATTAACCTGTTCAAGTTCCTTTCTTCATGGAGTCTTTGGGGAGCCAACTACATGGATTGTTTTGAGTCCAATGAAACCATGGGGGTTTGGCTTCATATTGCTGACAAAAAAAGGAAAAAGTACCTCAATAATAAATACAGAACTTCTCCTTTCAACCTCTTCCATCACATCAACACCTATGAAGACAATGGGTTTCTGATTGTGGATCTCTGCTGCTGGAAAGGATTTGAGTTTGTTTATAATTACTTATATTTAGCCAATTTACGTGAGAACTGGGAAGAGGTGAAAAAAAATGCCAGAAAGGCTCCCCAACCTGAAGTTAGGAGATATGTACTTCCTTTGAATATTGACAAGGCTGACACAGGCAAGAATTTAGTCACGCTCCCCAATACAACTGCCACTGCAATTCTGTGCAGTGACGAGACTATCTGGCTGGAGCCTGAAGTTCTCTTTTCAGGGCCTCGTCAAGCATTTGAGTTTCCTCAAATCAATTACCAGAAGTATTGTGGGAAACCTTACACATATGCGTATGGACTTGGCTTGAATCACTTTGTTCCAGATAGGCTCTGTAAGCTGAATGTCAAAACTAAAGAAACTTGGGTTTGGCAAGAGCCTGATTCATACCCATCAGAACCCATCTTTGTTTCTCACCCAGATGCCTTGGAAGAAGATGATGGTGTAGTTCTGAGTGTGGTGGTGAGCCCAGGAGCAGGACAAAAGCCTGCTTATCTCCTGATTCTGAATGCCAAGGACTTAAGTGAAGTTGCCCGGGCTGAAGTGGAGATTAACATCCCTGTCACCTTTCATGGACTGTTCAAAAAATCTTGA",
                    SequenceType = GeneSequenceTypes.Coding,
                    SourceID = 3,
                    Taxonomy = "Eukaryota; Metazoa; Chordata; Craniata; Vertebrata; Euteleostomi; Mammalia; Eutheria; Euarchontoglires; Primates; Haplorrhini; Catarrhini; Hominidae; Homo"
                };
            }

            this.FormatString = FormatString;
        }

        private void txtFormat_TextChanged(object sender, EventArgs e)
        {
            txtExample.Text = this.ExampleGene.ToFASTAHeader(txtFormat.Text);
        }

        /// <remarks>
        /// I never bother remembering how to do this; someone else always has the right steps:
        /// http://www.codeproject.com/Articles/2006/Drag-and-Drop-between-list-boxes-Beginner-s-Tutori
        /// </remarks>
        private void lstFieldNames_MouseDown(object sender, MouseEventArgs e)
        {
            if (lstFieldNames.Items.Count == 0 || e.Clicks == 2) { return; }

            int index = lstFieldNames.IndexFromPoint(e.X, e.Y);
            if (index != -1)
            {
                string item = lstFieldNames.Items[index].ToString();
                DoDragDrop(item, DragDropEffects.Move);
            }
        }

        private void lstFieldNames_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int itemIndex = lstFieldNames.IndexFromPoint(e.X, e.Y);
            if (itemIndex != -1)
            {
                string item = lstFieldNames.Items[itemIndex].ToString();
                int index = txtFormat.SelectionStart;
                txtFormat.Text = txtFormat.Text.Insert(index, item);

                txtFormat.Focus();
                txtFormat.SelectionStart = index + item.Length;
                txtFormat.SelectionLength = 0;
            }
        }

        private void txtFormat_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;

            txtFormat.SelectionStart = SetSelectionStart(e.X, e.Y);
            txtFormat.SelectionLength = 0;
        }

        private void txtFormat_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                string item = (string)e.Data.GetData(DataFormats.StringFormat);
                int index = txtFormat.SelectionStart;
                
                txtFormat.Text = txtFormat.Text.Insert(txtFormat.SelectionStart, item);
                txtFormat.SelectionStart = index + item.Length;
                txtFormat.SelectionLength = 0;
            }
        }

        private void txtFormat_DragEnter(object sender, DragEventArgs e)
        {
            txtFormat.Focus();
        }

        private int SetSelectionStart(int X, int Y)
        {
            Point location = txtFormat.PointToClient(new Point(X, Y));
            int index = txtFormat.GetCharIndexFromPosition(location);
            if (index == txtFormat.Text.Length - 1
                && (
                        (location.X) > txtFormat.GetPositionFromCharIndex(index).X
                ))
            {
                index += 1;
            }
            return index;
        }
    }
}
