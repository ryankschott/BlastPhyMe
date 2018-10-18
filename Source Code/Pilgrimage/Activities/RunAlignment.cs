using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChangLab.Common;
using ChangLab.Genes;
using ChangLab.Jobs;
using ChangLab.RecordSets;

namespace Pilgrimage.Activities
{
    public class RunAlignment : Activity
    {
        internal CommandLineAlignmentJob AlignmentJob { get { return (CommandLineAlignmentJob)this.CurrentJob; } set { this.CurrentJob = value; } }
        internal JobTargets Target { get; private set; }

        public RunAlignment(IWin32Window CallingForm, JobTargets Target) : base(CallingForm)
        {
            this.Target = Target;
        }

        public void RunAlignmentJob(CommandLineAlignmentJob AlignmentJob, List<Gene> Genes, string WorkingDirectory, RecordSet RecordSet, SubSet SubSet)
        {
            this.CurrentJob = AlignmentJob;
            AlignmentJob.SourceRecordSet = RecordSet;
            AlignmentJob.SourceSubSet = SubSet;
            AlignmentJob.InputGenes.AddRange(Genes);
            AlignmentJob.CreateJobDirectoryByName(WorkingDirectory, AlignmentJob.SourceName + " - " + this.Target.ToString());

            AlignmentJob.Initialize();
            AlignmentJob.ProgressUpdate += new ProgressUpdateEventHandler(Job_ProgressUpdate);
            AlignmentJob.StatusUpdate += new StatusUpdateEventHandler(Job_StatusUpdate);

            Worker.RunWorkerAsync();
        }

        ///// <param name="SubSetID">The subset for the input genes, not the destination subset for the aligned results.</param>
        //public void RunPRANKAlignment(List<Gene> Genes, PRANKOptions Options, bool TrimIfNecessary, string WorkingDirectory, string PRANKPath, bool KeepOutputFiles, string SubSetID)
        //{
        //    AlignmentJob = new AlignSequencesWithPRANK(Options, TrimIfNecessary, PRANKPath, KeepOutputFiles, SubSetID);
        //    AlignmentJob.InputGenes.AddRange(Genes);
        //    AlignmentJob.CreateJobDirectory(WorkingDirectory);
        //    this.CurrentJob = AlignmentJob;
            
        //    AlignmentJob.Initialize();
        //    AlignmentJob.ProgressUpdate += new ProgressUpdateEventHandler(Job_ProgressUpdate);
        //    AlignmentJob.StatusUpdate += new StatusUpdateEventHandler(Job_StatusUpdate);
            
        //    Worker.RunWorkerAsync();
        //}

        protected internal override void Worker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            AlignmentJob.AlignSequences();

            if (this.CurrentJob.CancellationPending || Worker.CancellationPending) { e.Cancel = true; }
            else { e.Result = AlignmentJob; }
        }
    }
}
