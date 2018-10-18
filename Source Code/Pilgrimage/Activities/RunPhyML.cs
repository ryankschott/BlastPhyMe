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
    public class RunPhyML : Activity
    {
        internal GenerateTreeWithPhyML PhyMLJob { get; set; }

        public RunPhyML(IWin32Window CallingForm) : base(CallingForm) { }

        public void GenerateTree(List<Gene> Genes, PhyMLOptions Options, string SequenceHeaderFormat, string PhyMLPath, string WorkingDirectory, bool KeepOutputFiles, RecordSet RecordSet, SubSet SubSet)
        {
            PhyMLJob = new GenerateTreeWithPhyML(Options, SequenceHeaderFormat, PhyMLPath, KeepOutputFiles, RecordSet, SubSet);
            PhyMLJob.InputGenes.AddRange(Genes);
            PhyMLJob.CreateJobDirectoryByName(WorkingDirectory, PhyMLJob.SourceName + " - PhyML");
            this.CurrentJob = PhyMLJob;

            PhyMLJob.Initialize();
            PhyMLJob.ProgressUpdate += new ProgressUpdateEventHandler(Job_ProgressUpdate);
            PhyMLJob.StatusUpdate += new StatusUpdateEventHandler(Job_StatusUpdate);

            Worker.RunWorkerAsync();
        }

        protected internal override void Worker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            PhyMLJob.GenerateTree();

            if (this.CurrentJob.CancellationPending || Worker.CancellationPending) { e.Cancel = true; }
            else { e.Result = PhyMLJob; }
        }
    }
}
