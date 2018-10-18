using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChangLab.Common;
using ChangLab.Jobs;
using ChangLab.Genes;
using ChangLab.RecordSets;
using Pilgrimage.Activities;

namespace Pilgrimage.GeneSequences
{
    public partial class frmExportToFlatFile : DialogForm
    {
        private List<Gene> SelectedGenes { get; set; }
        private ExportFileFormats FileFormat { get; set; }
        internal bool OpenInMEGA { get; set; }

        public frmExportToFlatFile(List<Gene> SelectedGenes, ExportFileFormats FileFormat)
        {
            InitializeComponent();
            this.SelectedGenes = SelectedGenes;
            this.FileFormat = FileFormat;

            this.Text = string.Format(this.Text, this.FileFormat);
            lblHeaderFormat.Text = string.Format(lblHeaderFormat.Text, this.FileFormat);

            SetButtonImage(btnExport, "Export");
            SetButtonImage(btnCancel, "Cancel");

            if (OpenInMEGA || FileFormat != ExportFileFormats.FASTA)
            {
                // Clear out the "per file" options.
                foreach (Control ctrl in tblOutput_FASTA.Controls.Cast<Control>().ToList())
                {
                    if (tblOutput_FASTA.GetRow(ctrl) > 1 && tblOutput_FASTA.GetRow(ctrl) < 5)
                    {
                        ctrl.Parent.Controls.Remove(ctrl);
                    }
                }
                lnkFileNameFieldNames.Parent.Controls.Remove(lnkFileNameFieldNames);
                chkOpenInExplorer.Parent.Controls.Remove(chkOpenInExplorer);
            }
            this.OpenInMEGA = OpenInMEGA;
        }

        private void frmExportToFASTA_Load(object sender, EventArgs e)
        {
            txtFileNameFormat.Text = Program.DatabaseSettings.FASTAFileNameFormatString;
            txtHeaderFormat.Text = Program.DatabaseSettings.FASTAHeaderFormatString;
            chkOpenInExplorer.Checked = Program.DatabaseSettings.FASTAExportOpenOnCreate;
            rbCheckedChanged(null, EventArgs.Empty);
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtHeaderFormat.Text))
            {
                Utility.ShowMessage(this, "Header format cannot be empty.");
                return;
            }
            if (rbPerSequence.Checked && string.IsNullOrWhiteSpace(txtFileNameFormat.Text))
            {
                Utility.ShowMessage(this, "File name format cannot be empty.");
                return;
            }

            if (rbOneFile.Checked)
            {
                FileInfo file = null;

                IODialogHelper.DialogPresets preset = IODialogHelper.DialogPresets.Text;
                switch (this.FileFormat)
                {
                    case ExportFileFormats.FASTA:
                        preset = IODialogHelper.DialogPresets.FASTA;
                        break;
                    case ExportFileFormats.PHYLIP:
                        preset = IODialogHelper.DialogPresets.PHYLIP;
                        break;
                    case ExportFileFormats.NEXUS:
                        preset = IODialogHelper.DialogPresets.NEXUS;
                        break;
                    case ExportFileFormats.PAML:
                        preset = IODialogHelper.DialogPresets.FASTA;
                        break;
                }

                if (IODialogHelper.SaveFile(preset, Program.Settings.CurrentRecordSet.Name.ToSafeFileName() + " - " + Program.Settings.CurrentSubSet_GeneSequences.Name.ToSafeFileName(), this, ref file))
                {
                    StreamSingleFile(file.FullName);

                    if (OpenInMEGA)
                    {
                        if (System.IO.File.Exists(Program.Settings.MEGAFullPath)) // One way or another, we found it.
                        {
                            OpenFileInMEGA(file.FullName);
                        }
                        else
                        {
                            Program.Settings.MEGAFullPath = "C:\\Program Files (x86)\\MEGA6\\MEGA6.exe";
                            if (!System.IO.File.Exists(Program.Settings.MEGAFullPath))
                            {
                                Program.Settings.MEGAFullPath = "C:\\Program Files\\MEGA6\\MEGA6.exe";
                                if (!System.IO.File.Exists(Program.Settings.MEGAFullPath))
                                {
                                    if (Utility.ShowMessage(this, "MEGA could not be found on this computer.  Would you like to browse for the MEGA application?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                                    {
                                        string megaFullPath = string.Empty;
                                        if (IODialogHelper.OpenFile("MEGA|*.exe", "exe", this, ref megaFullPath, false, false))
                                        {
                                            Program.Settings.MEGAFullPath = megaFullPath;
                                        }
                                    }
                                }
                            }

                            if (System.IO.File.Exists(Program.Settings.MEGAFullPath)) // One way or another, we found it.
                            {
                                OpenFileInMEGA(file.FullName);
                            }
                        }
                    }
                    else if (chkOpenInExplorer.Checked)
                    {
                        System.Diagnostics.Process.Start("explorer", "/select, \"" + file.FullName + "\"");

                        //// DG 2014-11-13: This feature used to open the file itself by first figuring out what the software was associated with the
                        //// file extension, but then I added the Export -> MEGA feature, which is what this was really going for anyway.
                        //// The AssocQueryString() code might be useful for something else, though, so this is being kept here for now.
                        //try
                        //{
                        //    string assocProgram = string.Empty;
                        //    if (IODialogHelper.AssocQueryString(file.Extension, ref assocProgram))
                        //    {
                        //        System.Diagnostics.Process.Start(assocProgram, file.FullName);
                        //    }
                        //    else
                        //    { System.Diagnostics.Process.Start(file.FullName); }
                        //}
                        //catch (Win32Exception ex)
                        //{
                        //    try
                        //    {
                        //        System.Diagnostics.Process.Start("notepad", file.FullName);
                        //    }
                        //    catch (Exception inner_ex)
                        //    {
                        //        Utility.ShowErrorMessage(this, inner_ex);
                        //    }
                        //}
                        //catch (Exception other_ex)
                        //{
                        //    Utility.ShowErrorMessage(this, other_ex);
                        //}
                    }
                }
                else { return; /* To skip over Program.Settings sets */ }
            }
            else if (rbPerSequence.Checked)
            {
                string directory = string.Empty;
                if (IODialogHelper.FolderBrowse(ref directory))
                {
                    // First check to make sure that all of the file names will work; that none will be too long.
                    bool allFilesValid = true;
                    string newFilePath = string.Empty;
                    Exception fileException = null;
                    foreach (Gene g in SelectedGenes)
                    {
                        newFilePath = directory + "\\" + g.ToFASTAHeader(txtFileNameFormat.Text).ToSafeFileName() + ".fas";
                        if (IODialogHelper.CanCreate(newFilePath, out fileException) != IODialogHelper.FileAccessResults.ReadWrite)
                        {
                            allFilesValid = false;
                            break;
                        }
                    }

                    if (allFilesValid)
                    {
                        SelectedGenes.ForEach(g =>
                        {
                            StreamFASTAFile(g, directory);
                        });

                        if (chkOpenInExplorer.Checked)
                        {
                            System.Diagnostics.Process.Start("explorer", directory);
                        }
                    }
                    else
                    {
                        Utility.ShowErrorMessage(this, new Exception(fileException.Message + " [" + newFilePath + "]", fileException));
                        return; // Cycle out early so that the form doesn't close.
                    }
                }
                else { return; }
            }

            Program.DatabaseSettings.FASTAFileNameFormatString = txtFileNameFormat.Text;
            Program.DatabaseSettings.FASTAHeaderFormatString = txtHeaderFormat.Text;
            Program.DatabaseSettings.FASTAExportOpenOnCreate = chkOpenInExplorer.Checked;

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void OpenFileInMEGA(string FileName)
        {
            Process process = Process.Start(new ProcessStartInfo("\"" + Program.Settings.MEGAFullPath + "\"", "\"" + FileName + "\""));
        }

        private void StreamSingleFile(string FileName)
        {
            using (FileStream stream = new FileStream(FileName, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    switch (this.FileFormat)
                    {
                        case ExportFileFormats.FASTA:
                            StreamFASTAFile(writer);
                            break;
                        case ExportFileFormats.NEXUS:
                            StreamNEXUSFile(writer);
                            break;
                        case ExportFileFormats.PHYLIP:
                            StreamPHYLIPFile(writer);
                            break;
                        case ExportFileFormats.PAML:
                            StreamPHYLIPFile(writer);
                            break;
                    }
                }
            }
        }

        private void StreamFASTAFile(StreamWriter writer)
        {
            SelectedGenes.ForEach(g =>
            {
                writer.WriteLine(">" + g.ToFASTAHeader(txtHeaderFormat.Text));
                writer.WriteLine(g.Nucleotides);
            });
        }

        private void StreamNEXUSFile(StreamWriter writer)
        {
            List<string> taxLabels = SelectedGenes.Select(g => g.ToFASTAHeader(txtHeaderFormat.Text).ToSafeTaxaLabel()).ToList();

            writer.WriteLine("#NEXUS");

            // TAXA block
            writer.WriteLine("begin taxa;");
            writer.WriteLine("\tdimensions ntax=" + SelectedGenes.Count.ToString() + ";");
            writer.WriteLine("\ttaxlabels");
            taxLabels.ForEach(g => writer.WriteLine("\t\t" + g));
            writer.WriteLine(";");
            writer.WriteLine("end;");

            // Nucleotides block

            // Make all of the sequences the same length; the user will have received a warning before opening this screen if they weren't.
            int maxLength = SelectedGenes.Max(g => g.Nucleotides.Length);
            List<string> nucleotides = SelectedGenes.Select(g => g.Nucleotides.PadRight(maxLength, '-')).ToList();

            writer.WriteLine("begin characters;");
            writer.WriteLine("\tdimensions nchar=" + maxLength.ToString() + ";");
            writer.WriteLine("\tformat missing=? gap=- matchchar=. datatype=nucleotide;");
            writer.WriteLine("\tmatrix");
            writer.WriteLine("");

            // Now print each sequence 60 nucelotides per line.
            int fragmentLength = 60;
            int nucleotideFragments = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(maxLength) / Convert.ToDouble(fragmentLength)));
            for (int i = 0; i < taxLabels.Count; i++)
            {
                writer.WriteLine(taxLabels[i]);
                for (int nBlock = 0; nBlock < nucleotideFragments; nBlock++)
                {
                    writer.WriteLine(nucleotides[i].SafeSubstring(nBlock * fragmentLength, fragmentLength));
                }
                writer.WriteLine();
            }

            writer.WriteLine(";");
            writer.WriteLine("end;");
        }

        private void StreamPHYLIPFile(StreamWriter writer)
        {
            List<string> taxLabels = SelectedGenes.Select(g => g.ToFASTAHeader(txtHeaderFormat.Text).ToSafeTaxaLabel()).ToList();
            // Make all of the sequences the same length; the user will have received a warning before opening this screen if they weren't.
            int maxLength = SelectedGenes.Max(g => g.Nucleotides.Length);
            int fragmentLength = 50;
            List<string> nucleotides = SelectedGenes.Select(g => g.Nucleotides.PadRight(maxLength, '-')).ToList();

            writer.WriteLine(SelectedGenes.Count.ToString() + " " + maxLength.ToString());
            writer.WriteLine();
            int nucleotideFragments = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(maxLength) / Convert.ToDouble(fragmentLength)));
                
            for (int i = 0; i < taxLabels.Count; i++)
            {
                writer.WriteLine(taxLabels[i].SafeSubstring(0, fragmentLength)); // "seq" + i.ToString().PadRight(3, ' '));

                // Split into <fragmentLength>bp strings
                for (int nBlock = 0; nBlock < nucleotideFragments; nBlock++)
                {
                    writer.WriteLine(nucleotides[i].SafeSubstring(nBlock * fragmentLength, fragmentLength));
                }

                writer.WriteLine();
            }

            //// Now interleave per 100 nucelotides.
            //int nucleotideFragments = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(maxLength) / 100.0D));
            //for (int nBlock = 0; nBlock < nucleotideFragments; nBlock++)
            //{
            //    for (int i = 0; i < taxLabels.Count; i++)
            //    {
            //        writer.WriteLine((nBlock == 0 ? taxLabels[i] + "  " : string.Empty) + nucleotides[i].SafeSubstring(nBlock * 100, 100));
            //    }

            //    writer.WriteLine();
            //}
        }

        private void StreamFASTAFile(Gene Gene, string OutputDirectory)
        {
            using (FileStream stream = new FileStream(OutputDirectory + "\\" + Gene.ToFASTAHeader(txtFileNameFormat.Text).ToSafeFileName() + ".fas", FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.WriteLine(">" + Gene.ToFASTAHeader(txtHeaderFormat.Text));
                    writer.WriteLine(Gene.Nucleotides);
                }
            }
        }

        private void rbCheckedChanged(object sender, EventArgs e)
        {
            txtFileNameFormat.Enabled = rbPerSequence.Checked;
            lnkFileNameFieldNames.Enabled = rbPerSequence.Checked;
        }

        private void lnkFieldNames_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            TextBox target = (sender == lnkFASTAHeaderFieldNames ? txtHeaderFormat : txtFileNameFormat);

            using (frmFormatFieldNames frm = new frmFormatFieldNames(target.Text, SelectedGenes.ElementAt((new Random()).Next(0, SelectedGenes.Count - 1))))
            {
                if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    target.Text = frm.FormatString;
                }
            }
        }
    }

    public enum ExportFileFormats
    {
        FASTA,
        NEXUS,
        PHYLIP,
        PAML
    }

    public static class ExtensionMethods
    {
        public static string ToSafeTaxaLabel(this string Header)
        {
            return System.Text.RegularExpressions.Regex.Replace(Header.Replace(" ", "_"), "[^A-Za-z0-9_\\-]", "");
        }
    }
}
