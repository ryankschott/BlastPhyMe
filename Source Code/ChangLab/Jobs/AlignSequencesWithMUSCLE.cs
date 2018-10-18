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
    public class AlignSequencesWithMUSCLE : CommandLineAlignmentJob
    {
        internal override GeneSources Source { get { return GeneSources.MUSCLE; } }
        public MUSCLEOptions Options { get; private set; }

        public AlignSequencesWithMUSCLE(MUSCLEOptions Options, string MUSCLEPath, bool KeepOutputFiles, string SubSetID)
            : base(JobTargets.MUSCLE, SubSetID, MUSCLEPath, KeepOutputFiles)
        {
            this.Options = Options;
            this.SetAdditionalProperty("Options", Options.ToString());

            this.AlignmentStatusUpdate += new AlignmentStatusUpdateEventHandler(AlignSequencesWithPRANK_AlignmentStatusUpdate);
        }

        public AlignSequencesWithMUSCLE(string JobID) : base(JobID)
        {
            this.Options = new MUSCLEOptions(this.AdditionalPropertiesXml.ToString());
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
                    // If I make source sequence an option, I'll have to call GetSequenceData() on the input genes.
                    fasta[1] = g.Nucleotides;

                    e.Data = fasta;
                    break;

                case AlignmentStatuses.InitializingCommandLine:
                    OnProgressUpdate(new ProgressUpdateEventArgs() { Setup = true, ProgressMessage = "Inititalizing MUSCLE", CurrentMax = (this.Options.MaximumIterations + 1), CurrentProgress = 0 });

                    e.Data = "-in input.fas -out output.fas " + Options.OptionSwitches;
                    break;

                case AlignmentStatuses.CommandLineExited:
                    //// Check for an error stack dump
                    //if (File.Exists(InputFile.Directory + "\\prank.exe.stackdump"))
                    //{
                    //    // Now capture the output sequences into the database
                    //    OnProgressUpdate(new ProgressUpdateEventArgs() { ProgressMessage = "Error occurred in PRANK.exe" });

                    //    List<string> errorLines = File.ReadAllLines(InputFile.Directory + "\\prank.exe.stackdump").ToList();
                    //    if (errorLines.First().StartsWith("Exception: "))
                    //    {
                    //        OnProgressUpdate(new ProgressUpdateEventArgs() { ProgressMessage = errorLines.First().Substring(11) });
                    //    }

                    //    e.Error = new Exception(errorLines.Concatenate("\r\n"));
                    //    e.Cancel = true;
                    //}
                    break;

                case AlignmentStatuses.ParsingOutput:
                    OnProgressUpdate(new ProgressUpdateEventArgs() { ProgressMessage = "Parsing aligned sequences" });

                    e.Data = "output.fas";
                    break;

                case AlignmentStatuses.Completed:
                    OnProgressUpdate(new ProgressUpdateEventArgs() { Setup = true, CurrentMax = 10, CurrentProgress = 10 });
                    break;
            }
        }

        internal override void Process_OutputDataReceived(object sender, DataReceivedEventArgs e) { }

        internal override void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                ProgressUpdateEventArgs args = new ProgressUpdateEventArgs();

                if (System.Text.RegularExpressions.Regex.IsMatch(e.Data, "^[0-9]{2}:[0-9]{2}:[0-9]{2}.{1,}$"))
                {
                    args.ProgressMessage = e.Data.Substring(8).TrimStart();
                }
                else
                {
                    args.ProgressMessage = e.Data;
                }

                if (e.Data.Contains("Iter"))
                {
                    string[] pieces = e.Data.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    args.CurrentProgress = int.Parse(pieces[4]);
                }

                OnProgressUpdate(args);
            }
        }
    }

    public class MUSCLEOptions : JobOptions
    {
        public bool FindDiagonals
        {
            get { return _optionsRoot.SafeAttributeValueAsBool("FindDiagonals", false); }
            set { _optionsRoot.SetAttributeValue("FindDiagonals", value); }
        }

        public int MaximumIterations
        {
            get { return _optionsRoot.SafeAttributeValueAsInt("MaximumIterations", 16); }
            set { _optionsRoot.SetAttributeValue("MaximumIterations", value); }
        }
        
        public int MaximumHours
        {
            get { return _optionsRoot.SafeAttributeValueAsInt("MaximumHours", 0); }
            set { _optionsRoot.SetAttributeValue("MaximumHours", value); }
        }

        public override string OptionSwitches
        {
            get
            {
                string options = (FindDiagonals ? " -diags" : "")
                    + " -maxiters " + MaximumIterations.ToString()
                    + (MaximumHours != 0 ? " -maxhours " + MaximumIterations.ToString() : "");

                return options.TrimStart();
            }
        }

        public MUSCLEOptions() : base() { }

        public MUSCLEOptions(string XML) : base(XML) { }
    }
}
