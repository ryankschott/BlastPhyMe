using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ChangLab.BlastN;
using ChangLab.Common;
using ChangLab.Genes;
using ChangLab.NCBI;
using ChangLab.NCBI.LocalDatabase;

namespace ChangLab.Jobs
{
    public class BlastNAtLocalDatabase : GeneProcessingJob
    {
        private BlastNLocalDatabase LocalDatabase = null;
        public string DatabaseFilePath { get; set; }
        public string BlastNExePath { get; set; }
        public string OutputDirectoryPath { get; set; }

        protected internal BlastNAtLocalDatabase() : this(string.Empty) { return; /* Any constructor code should go in the primary constructor */ }
        public BlastNAtLocalDatabase(string SubSetID) : base(JobTargets.BLASTN_Local, SubSetID) { }

        public void QueryDatabase(List<Gene> InputGenes)
        {
            // Validation
            if (!File.Exists(DatabaseFilePath))
            {
                throw new ArgumentException("Database file not found (" + DatabaseFilePath + ")", "DatabaseFilePath");
            }
            else if (!Directory.Exists(BlastNExePath))
            {
                throw new ArgumentException("BLASTN.exe directory not found (" + BlastNExePath + ")", "BlastNExePath");
            }
            else if (!File.Exists(BlastNExePath + "\\blastn.exe"))
            {
                throw new ArgumentException("BLASTN.exe not found in directory (" + BlastNExePath + ")", "BlastNExePath");
            }

            try
            {
                this.InputGenes.Clear(); this.InputGenes.AddRange(InputGenes);
                // Record the InputGenes against the Job
                InputGenes.ForEach(g => AddGene(g, GeneDirections.Input));

                LocalDatabase = new BlastNLocalDatabase(DatabaseFilePath, BlastNExePath, OutputDirectoryPath, this.ID);
                LocalDatabase.ProgressUpdate += new ProgressUpdateEventHandler(LocalDatabase_ProgressUpdate);
                LocalDatabase.ResultsProcessed += new BlastNLocalDatabase.ResultsEventHandler(LocalDatabase_ResultsProcessed);

                this.UpdateStatus(JobStatuses.Running);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SaveAlignments(Query Result)
        {
            try
            {
                //Gene inputGene = Result.InputGene InputGenes.First(g => GuidCompare.Equals(g.ID, Result.IterationQueryDefinition));
                int sourceId = GeneSource.IDByKey(GeneSources.BLASTN_Local);
                KeyValuePair<Gene, List<Gene>> alignments = new KeyValuePair<Gene, List<Gene>>(Result.InputGene, new List<Gene>());

                foreach (var hit in Result.LocalAlignments)
                {
                    Gene outputGene = new Gene()
                    {
                        Definition = hit.OutputGene.Definition,
                        SourceID = sourceId,
                        
                        Nucleotides = hit.Alignment.Nucleotides,
                        SequenceType = GeneSequenceTypes.Alignment,
                        SourceSequence = new NucleotideSequence(hit.Alignment.Nucleotides, hit.Alignment.AlignmentRange.Start)
                    };

                    outputGene.Save(true, true); // Save the gene to the master Gene table.
                    hit.Alignment.SubjectID = outputGene.ID;
                    AddGene(outputGene, GeneDirections.Output); // Record the output genes against the Job.
                    hit.Alignment.Save(true); // Save the alignment data.

                    alignments.Value.Add(outputGene); // Pilgrimage.frmMain uses this to update the HasBlastNAlignments column in subset views.
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LocalDatabase_ResultsProcessed(BlastNLocalDatabase.ResultsEventArgs e)
        {
            OnProgressUpdate(new ProgressUpdateEventArgs() { ProgressMessage = "Processing results..." });
            foreach (var result in e.Results)
            {
                // Save the alignment to the database and tag it against the Job as an output.
                SaveAlignments(result);
            }
            
            if (this.CancellationPending && !this.LocalDatabase.CancellationPending) { this.LocalDatabase.CancelAsync(); return; }
        }

        private void LocalDatabase_ProgressUpdate(ProgressUpdateEventArgs e)
        {
            this.OnProgressUpdate(e);
        }

    }
}
