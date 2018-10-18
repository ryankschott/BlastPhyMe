using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using ChangLab.Common;
using ChangLab.Genes;

namespace ChangLab.Jobs
{
    public class AlignSequencesWithPRANK : CommandLineAlignmentJob
    {
        internal override GeneSources Source { get { return GeneSources.PRANK; } }

        public PRANKOptions Options { get; private set; }
        private bool TrimIfNecessary { get; set; }

        public AlignSequencesWithPRANK(PRANKOptions Options, bool TrimIfNecessary, string PRANKPath, bool KeepOutputFiles, string SubSetID) : base(JobTargets.PRANK, SubSetID, PRANKPath, KeepOutputFiles)
        {
            this.Options = Options;
            this.SetAdditionalProperty("Options", Options.ToString());
            this.TrimIfNecessary = TrimIfNecessary;
            
            this.AlignmentStatusUpdate += new AlignmentStatusUpdateEventHandler(AlignSequencesWithPRANK_AlignmentStatusUpdate);
        }

        public AlignSequencesWithPRANK(string JobID)
            : base(JobID)
        {
            this.Options = new PRANKOptions(this.AdditionalPropertiesXml.ToString());
        }

        private void AlignSequencesWithPRANK_AlignmentStatusUpdate(CommandLineAlignmentJob.AlignmentJobEventArgs e)
        {
            switch (e.Status)
            {
                case AlignmentStatuses.WritingInputFile:
                    OnProgressUpdate(new ProgressUpdateEventArgs() { ProgressMessage = "Writing gene sequences to FASTA file" });
                    break;

                case AlignmentStatuses.WritingFASTAEntry:
                    string[] fasta = new string[2];
                    Gene g = (Gene)e.Data;

                    // The ID is what we'll be matching up from the output file, the definition is there purely for the purposes of helping
                    // the user out if they open the files.
                    fasta[0] = ">" + g.ID.ToString() + " (" + g.Definition + ")";

                    // TODO: Give the user the choice of whether to use the CDS or source sequence
                    // If I make source sequence an option, the -codon validation back up in frmCreateJob is going to need to have the 
                    // SourceSequence available for checking to see if trimming is needed, so it'll have to call GetSequenceData() on the
                    // input genes.
                    string sequence = g.Nucleotides;
                    if (TrimIfNecessary)
                    {
                        int trimBy = Convert.ToInt32(Convert.ToDouble(sequence.Length) % 3.0);
                        sequence = sequence.Substring(0, sequence.Length - trimBy);
                        if ((new string[] { "TAA", "TAG", "TGA" }).Contains(sequence.Substring(sequence.Length - 3)))
                        { sequence = sequence.Substring(0, sequence.Length - 3); }
                    }
                    fasta[1] = sequence;

                    e.Data = fasta;
                    break;

                case AlignmentStatuses.InitializingCommandLine:
                    OnProgressUpdate(new ProgressUpdateEventArgs() { Setup = true, ProgressMessage = "Inititalizing PRANK", CurrentMax = (this.Options.Iterations + 1), CurrentProgress = 0 });

                    e.Data = "-d=input.fas -o=output_sequences " + Options.OptionSwitches;
                    break;

                case AlignmentStatuses.CommandLineExited:
                    // Check for an error stack dump
                    if (File.Exists(InputFile.Directory + "\\prank.exe.stackdump"))
                    {
                        // Now capture the output sequences into the database
                        OnProgressUpdate(new ProgressUpdateEventArgs() { ProgressMessage = "Error occurred in PRANK.exe" });

                        List<string> errorLines = File.ReadAllLines(InputFile.Directory + "\\prank.exe.stackdump").ToList();
                        if (errorLines.First().StartsWith("Exception: "))
                        {
                            OnProgressUpdate(new ProgressUpdateEventArgs() { ProgressMessage = errorLines.First().Substring(11) });
                        }

                        e.Error = new Exception(errorLines.Concatenate("\r\n"));
                        e.Cancel = true;
                    }
                    break;

                case AlignmentStatuses.ParsingOutput:
                    OnProgressUpdate(new ProgressUpdateEventArgs() { ProgressMessage = "Parsing aligned sequences" });

                    string outputFileName = string.Empty;
                    if (this.Options.Translate || this.Options.MTTranslate) { outputFileName = "output_sequences.best.nuc.fas"; }
                    else { outputFileName = "output_sequences.best.fas"; }
                    if (!File.Exists(InputFile.Directory + "\\" + outputFileName))
                    {
                        FileInfo[] test = InputFile.Directory.GetFiles("output_sequences*.fas");
                        if (test.Length != 0) { outputFileName = test[0].Name; } // Take the first and hope for the best.
                    }
                    e.Data = outputFileName;
                    break;
            }
        }

        internal override void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null
                && !e.Data.Contains("\b")) // For now we're excluding the per-iteration flurry of alignment messages
            {
                ProgressUpdateEventArgs args = new ProgressUpdateEventArgs() { ProgressMessage = e.Data };

                if (e.Data.StartsWith("Generating multiple alignment"))
                {
                    args.CurrentProgress = int.Parse(System.Text.RegularExpressions.Regex.Replace(e.Data, "[^0-9]", ""));
                }
                else if (e.Data.StartsWith("Analysis done"))
                {
                    args.Setup = true;
                    args.CurrentMax = 10;
                    args.CurrentProgress = 10;
                }

                OnProgressUpdate(args);
            }
        }

        //public override void AlignSequences()
        //{
        //    try
        //    {
        //        UpdateStatus(JobStatuses.Running);

        //        // Record the InputGenes against the Job; it is assumed these were added before Align() is called...
        //        this.InputGenes.ForEach(g => AddGene(g, GeneDirections.Input, false));

        //        OnProgressUpdate(new ProgressUpdateEventArgs() { ProgressMessage = "Writing gene sequences to FASTA file" });

        //        // Write the sequences as a FASTA file
        //        string inputFileName = this.JobDirectory + "\\input.fas";
        //        using (FileStream stream = new FileStream(inputFileName, FileMode.Create, FileAccess.Write))
        //        {
        //            using (StreamWriter writer = new StreamWriter(stream))
        //            {
        //                foreach (Gene g in this.InputGenes)
        //                {
        //                    //if (g.Features.Count == 0) { g.GetSequenceData(); }

        //                    // The ID is what we'll be matching up from the output file, the definition is there purely for the purposes of helping
        //                    // the user out if they open the files.
        //                    writer.WriteLine(">" + g.ID.ToString() + " (" + g.Definition + ")");

        //                    // TODO: Give the user the choice of whether to use the CDS or source sequence
        //                    // If I make source sequence an option, the -codon validation back up in frmCreateJob is going to need to have the 
        //                    // SourceSequence available for checking to see if trimming is needed, so it'll have to call GetSequenceData() on the
        //                    // input genes.
        //                    string sequence = g.Nucleotides;
        //                    if (TrimIfNecessary)
        //                    {
        //                        int trimBy = Convert.ToInt32(Convert.ToDouble(sequence.Length) % 3.0);
        //                        sequence = sequence.Substring(0, sequence.Length - trimBy);
        //                        if ((new string[] { "TAA", "TAG", "TGA" }).Contains(sequence.Substring(sequence.Length - 3)))
        //                        { sequence = sequence.Substring(0, sequence.Length - 3); }
        //                    }

        //                    writer.WriteLine(sequence);
        //                }
        //            }
        //        }
        //        FileInfo inputFile = new FileInfo(inputFileName);

        //        OnProgressUpdate(new ProgressUpdateEventArgs() { Setup = true, ProgressMessage = "Inititalizing PRANK", CurrentMax = this.Options.Iterations, CurrentProgress = 0 });

        //        // Set up the PRANK process
        //        string arguments = "-d=input.fas -o=output_sequences " + Options.OptionSwitches;
        //        using (CommandLineProcess = new Process() { EnableRaisingEvents = true })
        //        {
        //            CommandLineProcess.StartInfo = new ProcessStartInfo("\"" + this.CommandLinePath + "\"", arguments)
        //            {
        //                CreateNoWindow = true,
        //                RedirectStandardOutput = true,
        //                UseShellExecute = false,
        //                WorkingDirectory = inputFile.DirectoryName
        //            };
        //            CommandLineProcess.OutputDataReceived += new DataReceivedEventHandler(Process_OutputDataReceived);
        //            CommandLineProcess.Start();
        //            CommandLineProcess.BeginOutputReadLine();
        //            CommandLineProcess.WaitForExit();
        //        }
        //        if (this.CancellationPending) { return; }

        //        // Check for an error stack dump
        //        if (File.Exists(inputFile.Directory + "\\prank.exe.stackdump"))
        //        {
        //            // Now capture the output sequences into the database
        //            OnProgressUpdate(new ProgressUpdateEventArgs() { ProgressMessage = "Error occurred in PRANK.exe" });

        //            List<string> errorLines = File.ReadAllLines(inputFile.Directory + "\\prank.exe.stackdump").ToList();
        //            if (errorLines.First().StartsWith("Exception: "))
        //            {
        //                OnProgressUpdate(new ProgressUpdateEventArgs() { ProgressMessage = errorLines.First().Substring(11) });
        //            }

        //            throw new Exception(errorLines.Concatenate("\r\n"));
        //        }

        //        // Now capture the output sequences into the database
        //        OnProgressUpdate(new ProgressUpdateEventArgs() { ProgressMessage = "Parsing aligned sequences" });

        //        string outputFileName = string.Empty;
        //        if (this.Options.Translate || this.Options.MTTranslate) { outputFileName = "output_sequences.best.nuc.fas"; }
        //        else { outputFileName = "output_sequences.best.fas"; }
        //        if (!File.Exists(inputFile.Directory + "\\" + outputFileName))
        //        {
        //            FileInfo[] test = inputFile.Directory.GetFiles("output_sequences*.fas");
        //            if (test.Length != 0) { outputFileName = test[0].Name; } // Take the first and hope for the best.
        //        }
        //        ParseOutput(inputFile.DirectoryName + "\\" + outputFileName);

        //        // Clean-up
        //        if (!this.KeepOutputFiles)
        //        {
        //            try
        //            {
        //                OnProgressUpdate(new ProgressUpdateEventArgs() { ProgressMessage = "Cleaning up process files" });
        //                Directory.Delete(this.JobDirectory);
        //            }
        //            catch (Exception ex)
        //            {
        //                OnProgressUpdate(new ProgressUpdateEventArgs() { ProgressMessage = "Unable to delete process files (" + ex.ToString() + ")" });
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Jobs.JobException jex = new Jobs.JobException(this.ID, 0, "An error occured attempting to run PRANK. Review the PRANK output for additional details.", ex);
        //        jex.Save(); this.Exceptions.Add(jex);
        //    }
        //    finally
        //    {
        //        try
        //        {
        //            Complete(); // Close out the job's final status.
        //        }
        //        catch (Exception ex)
        //        {
        //            UnhandledJobException(ex);
        //        }
        //    }
        //}
    }

    public class PRANKOptions : JobOptions
    {
        public bool F
        {
            get { return _optionsRoot.SafeAttributeValueAsBool("F", false); }
            set { _optionsRoot.SetAttributeValue("F", value); }
        }
        public bool Keep
        {
            get { return _optionsRoot.SafeAttributeValueAsBool("Keep", false); }
            set { _optionsRoot.SetAttributeValue("Keep", value); }
        }
        public bool Codon
        {
            get { return _optionsRoot.SafeAttributeValueAsBool("Codon", true); }
            set { _optionsRoot.SetAttributeValue("Codon", value); }
        }
        public bool Translate
        {
            get { return _optionsRoot.SafeAttributeValueAsBool("Translate", false); }
            set { _optionsRoot.SetAttributeValue("Translate", value); }
        }
        public bool MTTranslate
        {
            get { return _optionsRoot.SafeAttributeValueAsBool("MTTranslate", false); }
            set { _optionsRoot.SetAttributeValue("MTTranslate", value); }
        }
        public bool ShowTree
        {
            get { return _optionsRoot.SafeAttributeValueAsBool("ShowTree", false); }
            set { _optionsRoot.SetAttributeValue("ShowTree", value); }
        }
        public bool ShowAnc
        {
            get { return _optionsRoot.SafeAttributeValueAsBool("ShowAnc", false); }
            set { _optionsRoot.SetAttributeValue("ShowAnc", value); }
        }
        public bool ShowEvents
        {
            get { return _optionsRoot.SafeAttributeValueAsBool("ShowEvents", false); }
            set { _optionsRoot.SetAttributeValue("ShowEvents", value); }
        }

        public int Iterations
        {
            get { return _optionsRoot.SafeAttributeValueAsInt("Iterations", 5); }
            set { _optionsRoot.SetAttributeValue("Iterations", value); }
        }
        public string GuideTreeFile
        {
            get { return _optionsRoot.SafeAttributeValue("GuideTreeFile"); }
            set { _optionsRoot.SetAttributeValue("GuideTreeFile", value); }
        }

        public override string OptionSwitches
        {
            get
            {
                string options = (!string.IsNullOrWhiteSpace(GuideTreeFile) ? " -t=\"" + GuideTreeFile + "\"" : "")
                    + " -iterate=" + Iterations.ToString()
                    + (F ? " -F" : "")
                    + (Keep ? " -keep" : "")
                    + (ShowTree ? " -showtree" : "")
                    + (ShowAnc ? " -showanc" : "")
                    + (ShowEvents ? " -showevents" : "");

                if (Codon) { options += " -codon"; }
                else if (Translate) { options += " -translate"; }
                else if (MTTranslate) { options += " -mttranslate"; }

                return options.TrimStart();
            }
        }

        public PRANKOptions()
            : base()
        {
            Codon = true;
        }

        public PRANKOptions(string XML) : base(XML) { }

        public PRANKOptions(bool SetAllOptions)
        {
            this.F = SetAllOptions;
            this.Keep = SetAllOptions;
            this.Codon = SetAllOptions;
            this.Translate = SetAllOptions;
            this.MTTranslate = SetAllOptions;
            this.ShowTree = SetAllOptions;
            this.ShowAnc = SetAllOptions;
            this.ShowEvents = SetAllOptions;
        }
    }
}
