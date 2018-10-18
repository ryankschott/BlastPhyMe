using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using ChangLab.Common;
using ChangLab.Genes;
using ChangLab.Jobs;
using ChangLab.NCBI.LocalDatabase;
using ChangLab.RecordSets;

namespace Pilgrimage.Activities
{
    public class BlastSequencesWithLocalDatabase : Activity
    {
        private SubSet SubSet { get; set; }
        private BlastNAtLocalDatabase DatabaseJob { get; set; }

        public BlastSequencesWithLocalDatabase(SubSet SubSet, IWin32Window CallingForm)
            : base(CallingForm)
        {
            this.SubSet = SubSet;
        }

        public void Submit(List<Gene> Genes, string DatabaseFilePath, string BlastNExePath, string OutputDirectoryPath)
        {
            DatabaseJob = new BlastNAtLocalDatabase(this.SubSet.ID);
            DatabaseJob.ProgressUpdate += new ProgressUpdateEventHandler(Job_ProgressUpdate);
            DatabaseJob.DatabaseFilePath = DatabaseFilePath;
            DatabaseJob.BlastNExePath = BlastNExePath;
            DatabaseJob.OutputDirectoryPath = OutputDirectoryPath;
            this.CurrentJob = this.DatabaseJob;
            
            Worker.RunWorkerAsync(Genes);
            using (ProgressForm = new frmProgress("Querying database with coding sequences...", true, true, (Genes.Count > BlastNLocalDatabase.SequenceBatchSize), false))
            {
                ProgressForm.Cancelled += new frmProgress.CancelledEventHandler(ProgressForm_Cancelled);
                ProgressForm.ShowDialog(OwnerWindow);
            }
        }

        protected internal override void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            DatabaseJob.Initialize();
            DatabaseJob.QueryDatabase((List<Gene>)e.Argument);

            e.Result = DatabaseJob;
            if (DatabaseJob.CancellationPending || Worker.CancellationPending) { e.Cancel = true; }
        }

        protected internal override void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Completed = true;
            CloseProgressForm(DialogResult.OK);

            if (e.Error != null)
            {
                OnActivityCompleted(new ActivityCompletedEventArgs(this, null, e.Error, e.Cancelled));
                return;
            }

            if (!e.Cancelled || DatabaseJob.OutputGenes.Count != 0)
            {
                // Pop open a results table that pulls merged alignments from the database.
                // If the request was cancelled but we had received partial results, we'll still show what we got.

                using (GeneSequences.BlastN.frmBlastNAlignments frm = new GeneSequences.BlastN.frmBlastNAlignments((ChangLab.Jobs.BlastNAtNCBI)e.Result))
                {
                    frm.ShowDialog(OwnerWindow);
                    OnActivityCompleted(new ActivityCompletedEventArgs(this, frm.EditedSubSetIDs, e.Error, false));
                    return;
                }
            }

            // Cancelled before any genes were downloaded.
            OnActivityCompleted(new ActivityCompletedEventArgs(this, null, e.Error, e.Cancelled));
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
    }
}
