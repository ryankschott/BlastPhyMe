using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using Bio;
using Bio.Web;
using Bio.Web.Blast;
using ChangLab.BlastN;
using ChangLab.Common;
using ChangLab.Genes;
using ChangLab.NCBI;

namespace ChangLab.Jobs
{
    public class BlastNAtNCBI : GeneProcessingJob
    {
        private BlastNWebService WebService = null;
        public BlastNWebServiceConfigurationSettings Options { get; set; }
        public BLASTPurposes Purpose
        {
            get { return (BLASTPurposes)Enum.Parse(typeof(BLASTPurposes), this.GetAdditionalProperty("Purpose", "SimilarCodingSequences")); }
            set { this.SetAdditionalProperty("Purpose", value.ToString()); }
        }

        public List<Request> Requests { get { return WebService.Requests; } }
        public int BatchedRequestsCount { get { return WebService.BatchedRequests.Count; } }

        protected internal BlastNAtNCBI() : this(string.Empty) { return; /* Any constructor code should go in the primary constructor */ }

        public BlastNAtNCBI(BlastNWebServiceConfigurationSettings Options, string SubSetID) : base(JobTargets.BLASTN_NCBI, SubSetID)
        {
            this.Options = Options;
            this.SetAdditionalProperty("Options", Options.ToString());
        }

        public BlastNAtNCBI(string JobID) : base(JobID)
        {
            if (this.AdditionalPropertiesXml != null)
            { this.Options = new BlastNWebServiceConfigurationSettings(this.AdditionalPropertiesXml.ToString()); }
        }

        public void SetupWebService(List<Gene> InputGenes)
        {
            this.InputGenes.Clear(); this.InputGenes.AddRange(InputGenes);
            // Record the InputGenes against the Job
            InputGenes.ForEach(g => AddGene(g, GeneDirections.Input));

            WebService = new BlastNWebService(this.Options, this);
            WebService.ProgressUpdate += new ProgressUpdateEventHandler(innerProcess_ProgressUpdate);
            WebService.ResultsDownloaded += new BlastNWebService.ResultsEventHandler(WebService_ResultsDownloaded);
            WebService.BatchGenesIntoRequests(this.InputGenes);            
        }

        public void SubmitRequest()
        {
            try
            {
                this.UpdateStatus(JobStatuses.Running);
                WebService.SubmitToNCBI();
            }
            catch (Exception ex)
            {
                UnhandledJobException(ex);
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

        private KeyValuePair<Gene, List<Gene>> SaveAlignments(BlastSearchRecord Result, Request Request)
        {
            try
            {
                Gene inputGene = InputGenes.First(g => GuidCompare.Equals(g.ID, Result.IterationQueryDefinition));
                int sourceId = GeneSource.IDByKey(GeneSources.BLASTN_NCBI);
                KeyValuePair<Gene, List<Gene>> alignments = new KeyValuePair<Gene, List<Gene>>(inputGene, new List<Gene>());
                
                foreach (var hit in Result.Hits.Select((h, i) => new { Hit = h, Rank = i }))
                {
                    Alignment alignment = new Alignment() { QueryID = inputGene.ID, Rank = hit.Rank };
                    alignment.PopulateFromHit(hit.Hit);

                    string accession = hit.Hit.ExtractAccessionFromID();
                    
                    Gene outputGene = new Gene()
                    {
                        Accession = (!string.IsNullOrWhiteSpace(accession) ? accession : hit.Hit.Accession.ToSafeString()),
                        Definition = hit.Hit.Def,
                        SourceID = sourceId,
                        GenBankID = hit.Hit.ExtractGIFromID().ToSafeInt(),
                        
                        Nucleotides = alignment.Nucleotides,
                        SequenceType = GeneSequenceTypes.Alignment
                    };

                    outputGene.Save(); // Save the gene to the master Gene table.
                    alignment.SubjectID = outputGene.ID;
                    AddGene(outputGene, GeneDirections.Output); // Record the output genes against the Job.
                    alignment.Save(true); // Save the alignment data.
                    Request.AddAlignment(alignment); // Link the alignment to the Request (so we could pull statistics on NCBI requests later).

                    alignments.Value.Add(outputGene); // Pilgrimage.frmMain uses this to update the HasBlastNAlignments column in subset views.
                }

                return alignments;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void WebService_ResultsDownloaded(BlastNWebService.ResultsEventArgs e)
        {
            WebService.Results.AddRange(e.Results);
            Dictionary<Gene, List<Gene>> resultAlignments = new Dictionary<Gene, List<Gene>>();

            OnProgressUpdate(new ProgressUpdateEventArgs() { ProgressMessage = "Processing results..." });
            foreach (var result in e.Results)
            {
                // Save the alignment to the database and tag it against the Job as an output so we've at least stored the results and can retrieve them again later,
                // and show the user which of the genes they sent off were processed by the job so that they can just run the ones that weren't.
                KeyValuePair<Gene, List<Gene>> alignments = SaveAlignments(result, e.Request);
                resultAlignments.Add(alignments.Key, alignments.Value);
            }
            OnResultsSaved(new ResultsEventArgs(resultAlignments));

            if (this.CancellationPending && !this.WebService.CancellationPending) { this.WebService.CancelAsync(); return; }
        }

        public override void CancelAsync()
        {
            CancellationPending = true;
            WebService.CancelAsync();
        }

        protected virtual void OnResultsSaved(ResultsEventArgs e)
        {
            if (ResultsSaved != null)
            {
                ResultsSaved(e);
            }
        }
        public delegate void ResultsSavedEventHandler(ResultsEventArgs e);
        public event ResultsSavedEventHandler ResultsSaved;

        public class ResultsEventArgs : EventArgs
        {
            public Dictionary<Gene, List<Gene>> Alignments { get; private set; }

            public ResultsEventArgs(Dictionary<Gene, List<Gene>> Alignments)
            {
                this.Alignments = Alignments;
            }

            public ResultsEventArgs()
            {
                this.Alignments = new Dictionary<Gene, List<Gene>>();
            }
        }

        #region Speciality Database Functions
        public static DataTable ListAlignments(int FilterByID, string JobID, string RecordSetID)
        {
            return ListAlignments(FilterByID, 0, JobID, RecordSetID);
        }

        public static DataTable ListAlignments(int FilterByID, int RequestID, string JobID, string RecordSetID)
        {
            using (LocalDatabase.DataAccess da = new LocalDatabase.DataAccess("Job.BlastN_ListAlignmentsForJob"))
            {
                da.CommandTimeout = 120;
                da.AddParameter("RequestID", RequestID, true);
                da.AddParameter("JobID", SqlDbType.UniqueIdentifier, JobID, true);
                da.AddParameter("RecordSetID", SqlDbType.UniqueIdentifier, RecordSetID, true);
                da.AddParameter("FilterByID", SqlDbType.Int, FilterByID);
                return da.ExecuteDataTable();
            }
        }

        public static DataSet ListAnnotationGenes(string JobID)
        {
            using (LocalDatabase.DataAccess da = new LocalDatabase.DataAccess("Job.BlastN_ListAnnotationGenesForJob"))
            {
                da.CommandTimeout = 120;
                da.AddParameter("JobID", SqlDbType.UniqueIdentifier, JobID, true);
                return da.ExecuteDataSet();
            }
        }

        public DataTable ListInputGenesForAlignment(string SubjectGeneID)
        {
            return BlastNAtNCBI.ListInputGenes(SubjectGeneID, 0, this.ID, string.Empty);
        }

        public static DataTable ListInputGenesForRequest(int RequestID)
        {
            return BlastNAtNCBI.ListInputGenes(string.Empty, RequestID, string.Empty, string.Empty);
        }

        public static DataTable ListInputGenesForJob(string JobID, string RecordSetID)
        {
            return BlastNAtNCBI.ListInputGenes(string.Empty, 0, JobID, RecordSetID);
        }

        public static DataTable ListInputGenes(string SubjectGeneID, int RequestID, string JobID, string RecordSetID)
        {
            using (LocalDatabase.DataAccess da = new LocalDatabase.DataAccess("Job.BlastN_ListQueryGenesForAlignment"))
            {
                da.AddParameter("SubjectGeneID", SqlDbType.UniqueIdentifier, SubjectGeneID, true);
                da.AddParameter("RequestID", RequestID, true);
                da.AddParameter("JobID", SqlDbType.UniqueIdentifier, JobID, true);
                da.AddParameter("RecordSetID", SqlDbType.UniqueIdentifier, RecordSetID, true);
                return da.ExecuteDataTable();
            }
        }

        public static DataTable ListNotProcessedGenes(string JobID)
        {
            using (LocalDatabase.DataAccess da = new LocalDatabase.DataAccess("Job.BlastN_ListNotProcessedGenes"))
            {
                da.AddParameter("JobID", SqlDbType.UniqueIdentifier, JobID);
                return da.ExecuteDataTable();
            }
        }

        public static DataTable ListAlignedGenes(string QueryGeneID, string RecordSetID, string JobID = "")
        {
            using (LocalDatabase.DataAccess da = new LocalDatabase.DataAccess("Job.BlastN_ListSubjectGenesForQueryGene"))
            {
                da.AddParameter("QueryGeneID", SqlDbType.UniqueIdentifier, QueryGeneID);
                da.AddParameter("RecordSetID", SqlDbType.UniqueIdentifier, RecordSetID);
                da.AddParameter("JobID", SqlDbType.UniqueIdentifier, JobID, true);
                return da.ExecuteDataTable();
            }
        }

        public static DataTable ListRequests(string JobID)
        {
            using (LocalDatabase.DataAccess da = new LocalDatabase.DataAccess("Job.BLASTN_ListRequests"))
            {
                da.AddParameter("JobID", SqlDbType.UniqueIdentifier, JobID);
                return da.ExecuteDataTable();
            }
        }
        #endregion

        public enum BLASTPurposes
        {
            SimilarCodingSequences,
            AnnotateUnknownGenes
        }
    }
}