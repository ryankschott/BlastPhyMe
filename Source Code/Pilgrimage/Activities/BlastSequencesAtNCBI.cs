using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using ChangLab.Common;
using ChangLab.Genes;
using ChangLab.Jobs;
using ChangLab.NCBI;
using ChangLab.RecordSets;

namespace Pilgrimage.Activities
{
    public class BlastSequencesAtNCBI : Activity
    {
        private SubSet SubSet { get; set; }
        private BlastNAtNCBI NCBIJob { get; set; }
        internal int BatchedRequestCount { get { return this.NCBIJob.BatchedRequestsCount; } }

        public BlastSequencesAtNCBI(SubSet SubSet, IWin32Window CallingForm) : base(CallingForm)
        {
            this.SubSet = SubSet;
        }

        public void Submit(List<Gene> Genes, BlastNWebServiceConfigurationSettings Options, BlastNAtNCBI.BLASTPurposes Purpose)
        {
            NCBIJob = new BlastNAtNCBI(Options, SubSet.ID) { Purpose = Purpose };
            NCBIJob.ProgressUpdate += new ProgressUpdateEventHandler(Job_ProgressUpdate);
            NCBIJob.StatusUpdate += new StatusUpdateEventHandler(Job_StatusUpdate);
            NCBIJob.ResultsSaved += new BlastNAtNCBI.ResultsSavedEventHandler(NCBIJob_ResultsSaved);

            NCBIJob.Initialize();
            // We're doing this setup now so that by the time we kick off the background worker, the caller of the activity can know things like
            // how many requests are going to be submitted to the web service.
            NCBIJob.SetupWebService(Genes);
            
            this.CurrentJob = NCBIJob;

            Worker.RunWorkerAsync();
        }

        protected internal override void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            NCBIJob.SubmitRequest();

            e.Result = NCBIJob;
            if (NCBIJob.CancellationPending || Worker.CancellationPending) { e.Cancel = true; }
        }

        protected internal override void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Completed = true;

            if (e.Error != null)
            {
                OnActivityCompleted(new ActivityCompletedEventArgs(this, null, e.Error, e.Cancelled));
                return;
            }
            else if (e.Cancelled && !NCBIJob.Requests.Any(req => req.LastStatus != RequestStatus.Pending) && NCBIJob.Exceptions.Count == 0)
            {
                // Cancelled before any genes were submitted and no exceptions occured.
                // The second clause makes this very unlikely; the first submission usually makes it in very fast.
                OnActivityCompleted(new ActivityCompletedEventArgs(this, null, e.Error, e.Cancelled));
            }
            else
            {
                OnActivityCompleted(new ActivityCompletedEventArgs(this, NCBIJob, e.Error, false));
            }
        }

        private void NCBIJob_ResultsSaved(BlastNAtNCBI.ResultsEventArgs e)
        {
            // frmMain subscribes to this event so that it can update the HasBlastNResults columns of its subset views.
            OnResultsSaved(e);
        }

        protected virtual void OnResultsSaved(BlastNAtNCBI.ResultsEventArgs e)
        {
            if (ResultsSaved != null)
            {
                ResultsSaved(e);
            }
        }
        public delegate void ResultsSavedEventHandler(BlastNAtNCBI.ResultsEventArgs e);
        public event ResultsSavedEventHandler ResultsSaved;

        public class Alignment
        {
            internal Gene Query { get; set; }
            internal Gene Merged { get; set; }

            /// <summary>
            /// Indicates that the user has accepted edits from one of the aligned Gene records, and those edits are now stored in Merged.
            /// </summary>
            internal bool HasMerged { get; set; }
            internal bool ReplaceSequence { get; set; }

            internal bool HasAlignments { get; set; }
            internal List<Activities.BlastNAlignmentRow> Alignments { get; set; }
        }
    }

    public class BlastNAlignmentRow : GenericGeneRowDataItem
    {
        public override string ID { get { return this.Gene.ID; } }

        public int Rank { get; private set; }
        public double MaxScore { get; private set; }
        public double TotalScore { get; private set; }
        public int AlignmentLength { get; private set; }
        public double AlignmentPercentage { get; private set; }
        public double QueryCover { get; private set; }
        
        public BlastNAlignmentRow(DataRow Row) : base (Gene.FromDatabaseRow(Row))
        {
            this.Rank = (Row.ToSafeInt("Rank") + 1);
            this.MaxScore = Row.ToSafeDouble("MaxScore");
            this.TotalScore = Row.ToSafeDouble("TotalScore");
            this.AlignmentLength = Row.ToSafeInt("AlignmentLength");
            this.AlignmentPercentage = Row.ToSafeDouble("AlignmentPercentage");
            this.QueryCover = Row.ToSafeDouble("QueryCover");
            this.InRecordSet = Row.ToSafeBoolean("InRecordSet", false);

            if (this.Gene.SourceID == 0) { this.Gene.SourceID = GeneSource.IDByKey(GeneSources.BLASTN_NCBI); }
        }

        public BlastNAlignmentRow(Gene Gene) : base(Gene)
        {
            this.Rank = 0;
        }
    }
}