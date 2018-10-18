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
    public class CommandLineAlignmentJob : CommandLineGeneProcessingJob
    {
        internal virtual GeneSources Source { get { throw new NotImplementedException(); } }
        internal FileInfo InputFile { get; private set; }

        public CommandLineAlignmentJob(JobTargets Target, string SubSetID, string CommandLinePath, bool KeepOutputFiles)
            : base(Target, SubSetID, CommandLinePath, KeepOutputFiles)
        {
        }

        public CommandLineAlignmentJob(string JobID)
            : base(JobID)
        {
        }

        public void AlignSequences()
        {
            try
            {
                UpdateStatus(JobStatuses.Running);

                // Record the InputGenes against the Job; it is assumed these were added before Align() is called...
                this.InputGenes.ForEach(g => AddGene(g, GeneDirections.Input, false));

                OnAlignmentStatusUpdate(new AlignmentJobEventArgs(AlignmentStatuses.WritingInputFile));

                // Write the sequences as a FASTA file
                string inputFileName = this.JobDirectory + "\\input.fas";
                using (FileStream stream = new FileStream(inputFileName, FileMode.Create, FileAccess.Write))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        foreach (Gene g in this.InputGenes)
                        {
                            AlignmentJobEventArgs writingFASTAEntryArgs = new AlignmentJobEventArgs(AlignmentStatuses.WritingFASTAEntry);
                            writingFASTAEntryArgs.Data = g;
                            OnAlignmentStatusUpdate(writingFASTAEntryArgs);

                            writer.WriteLine(((string[])writingFASTAEntryArgs.Data)[0]);
                            writer.WriteLine(((string[])writingFASTAEntryArgs.Data)[1]);
                        }
                    }
                }
                InputFile = new FileInfo(inputFileName);

                AlignmentJobEventArgs initializingCommandLineArgs = new AlignmentJobEventArgs(AlignmentStatuses.InitializingCommandLine);
                OnAlignmentStatusUpdate(initializingCommandLineArgs);

                // Set up the PRANK process
                string arguments = (string)initializingCommandLineArgs.Data;
                using (CommandLineProcess = new Process() { EnableRaisingEvents = true })
                {
                    CommandLineProcess.StartInfo = new ProcessStartInfo("\"" + this.CommandLinePath + "\"", arguments)
                    {
                        CreateNoWindow = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        WorkingDirectory = InputFile.DirectoryName
                    };
                    CommandLineProcess.OutputDataReceived += new DataReceivedEventHandler(Process_OutputDataReceived);
                    CommandLineProcess.ErrorDataReceived += new DataReceivedEventHandler(Process_ErrorDataReceived);
                    CommandLineProcess.Start();
                    CommandLineProcess.BeginOutputReadLine();
                    CommandLineProcess.BeginErrorReadLine();
                    CommandLineProcess.WaitForExit();
                }
                if (this.CancellationPending) { return; }

                // Any custom post-command line code can be run by the inheritor class
                AlignmentJobEventArgs commandLineExitedArgs = new AlignmentJobEventArgs(AlignmentStatuses.CommandLineExited);
                OnAlignmentStatusUpdate(commandLineExitedArgs);
                if (commandLineExitedArgs.Error != null) { throw commandLineExitedArgs.Error; }
                else if (commandLineExitedArgs.Cancel) { return; }

                // Now capture the output sequences into the database. The inheritor class will need to provide the output file name.
                AlignmentJobEventArgs parsingOutputArgs = new AlignmentJobEventArgs(AlignmentStatuses.ParsingOutput);
                OnAlignmentStatusUpdate(parsingOutputArgs);
                ParseOutput(InputFile.DirectoryName + "\\" + (string)parsingOutputArgs.Data);

                // Clean-up
                if (!this.KeepOutputFiles)
                {
                    try
                    {
                        OnProgressUpdate(new ProgressUpdateEventArgs() { ProgressMessage = "Cleaning up process files" });
                        Directory.Delete(this.JobDirectory);
                    }
                    catch (Exception ex)
                    {
                        OnProgressUpdate(new ProgressUpdateEventArgs() { ProgressMessage = "Unable to delete process files (" + ex.ToString() + ")" });
                    }
                }

                OnAlignmentStatusUpdate(new AlignmentJobEventArgs(AlignmentStatuses.Completed));
            }
            catch (Exception ex)
            {
                Jobs.JobException jex = new Jobs.JobException(this.ID, 0, "An error occured attempting to run " + this.Source.ToString() + ". Review the " + this.Source.ToString() + " output for additional details.", ex);
                jex.Save(); this.Exceptions.Add(jex);
            }
            finally
            {
                try
                {
                    Complete(); // Close out the job's final status.

                    if (string.IsNullOrWhiteSpace(this.Output) && this.ProgressMessages.Count != 0)
                    {
                        this.Output = this.ProgressMessages.Select(msg => msg.Message).Concatenate("\r\n");
                        this.LogOutput();
                    }
                }
                catch (Exception ex)
                {
                    UnhandledJobException(ex);
                }
            }
        }

        /// <remarks>
        /// We're saving these directly to the database much like we do with NCBI results where it's regardless of what the user ultimately decides
        /// to do with the results.  This facilitates the ability to close a running alignment to the background and continue to use Pilgrimage until
        /// the alignment is finished, after which the output is accessed via a results form.  The user can then decide what to do with the aligned
        /// sequences, even if they've closed Pilgrimage after the alignment finished and then re-opened Pilgrimage to view the results.
        /// </remarks>
        internal virtual void ParseOutput(string OutputFileName)
        {
            this.OutputGenes.Clear();
            List<Gene> alignedSequences = new List<Gene>();
            Gene gene = null;

            foreach (string line in File.ReadAllLines(OutputFileName))
            {
                if (line.StartsWith(">"))
                {
                    gene = new Gene(System.Text.RegularExpressions.Regex.Match(line.ToUpper(), "^\\>+[A-Z0-9\\-]{36}").Groups[0].Value.Substring(1));
                    alignedSequences.Add(gene);
                }
                else
                {
                    gene.Nucleotides += line;
                }
            }

            alignedSequences.ForEach(seq =>
            {
                // seq.ID is the input gene's ID
                Gene outputGene = new Gene() { SourceID = GeneSource.IDByKey(Source) };

                outputGene.Merge(this.InputGenes.First(inputGene => GuidCompare.Equals(inputGene.ID, seq.ID)));
                outputGene.Nucleotides = seq.Nucleotides;
                outputGene.SourceSequence = new NucleotideSequence(seq.Nucleotides, 1);
                outputGene.Features.Clear();
                outputGene.LastUpdatedAt = DateTime.Now;
                outputGene.LastUpdateSourceID = outputGene.SourceID;
                outputGene.Save(true, true); // The output gene now exists in the database as a standalone record.

                OutputGenes.Add(outputGene);
                AddGene(outputGene, GeneDirections.Output, false);
                Genes.AlignedGeneSource.Add(this.ID, seq.ID, outputGene.ID);
            });
        }

        internal virtual void Process_OutputDataReceived(object sender, DataReceivedEventArgs e) { }

        internal virtual void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e) { }

        #region Events
        public enum AlignmentStatuses
        {
            Undefined,
            WritingInputFile,
            WritingFASTAEntry,
            InitializingCommandLine,
            CommandLineExited,
            ParsingOutput,
            Completed
        }

        protected virtual void OnAlignmentStatusUpdate(AlignmentJobEventArgs e)
        {
            if (AlignmentStatusUpdate != null)
            {
                AlignmentStatusUpdate(e);
            }
        }
        public event AlignmentStatusUpdateEventHandler AlignmentStatusUpdate;

        public delegate void AlignmentStatusUpdateEventHandler(AlignmentJobEventArgs e);
        public class AlignmentJobEventArgs : EventArgs
        {
            public AlignmentStatuses Status { get; set; }
            public object Data { get; set; }
            public bool Cancel { get; set; }
            public Exception Error { get; set; }

            public AlignmentJobEventArgs(AlignmentStatuses Status)
            {
                this.Status = Status;
                this.Cancel = false;
            }
        }
        #endregion
    }
}
