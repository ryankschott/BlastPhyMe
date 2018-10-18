using System.ComponentModel;
using System.Windows.Forms;
using ChangLab.NCBI;
using ChangLab.NCBI.Taxonomy;
using Pilgrimage.Search;

namespace Pilgrimage.Activities
{
    public class SearchNCBIForTaxa : Activity
    {
        public SearchNCBIForTaxa(IWin32Window OwnerWindow) : base(OwnerWindow) { }

        /// <summary>
        /// The maximum number of search results the will be fetched, regardless of how many records NCBI matched for the search query.
        /// </summary>
        public int ResultMaximum { get { return 500; } }
        
        private string Term { get; set; }
        private TaxonomyServiceSearch Search { get; set; }

        public void SubmitSearch(string Term)
        {
            this.Term = Term;
            this.Search = null;

            using (ProgressForm = new frmProgress("Searching NCBI...", new frmProgress.ProgressOptions() { AllowCancellation = false, UseNeverEndingTimer = true }))
            {
                Worker.RunWorkerAsync();
                ProgressForm.ShowDialog(OwnerWindow);
            }
        }

        protected internal override void ProgressForm_Cancelled(RunWorkerCompletedEventArgs e)
        {
            Worker.CancelAsync();
            this.Search.CancelAsync();
        }

        protected internal override void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (this.Search == null)
            {
                this.Search = new TaxonomyServiceSearch();
                this.Search.Search(this.Term, Program.Settings.CurrentRecordSet.ID, false);
                e.Result = null;
            }
            else
            {
                e.Result = DownloadSummaries();
            }

            if (Worker.CancellationPending) { e.Cancel = true; }
        }

        private TaxonomyServiceSearch DownloadSummaries()
        {
            this.Search.ProgressUpdate += new ChangLab.Common.ProgressUpdateEventHandler(Job_ProgressUpdate);
            this.Search.ResultsSummary(0, ResultMaximum);
            return this.Search;
        }

        protected internal override void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Completed = true;
            CloseProgressForm(DialogResult.OK);

            if (e.Error != null) { OnActivityCompleted(new ActivityCompletedEventArgs(this, null, e.Error, false)); }
            else if (e.Cancelled)
            {
                // Whatever was downloaded before the cancellation was requested can be sent back.
                OnActivityCompleted(new ActivityCompletedEventArgs(this, this.Search, null, true));
            }
            else
            {
                if (e.Result == null)
                {
                    if (this.Search.SearchResult.ResultCount > this.Search.SearchResult.ReturnMaximum)
                    {
                        using (ProgressForm = new frmProgress("Downloading taxonomy summary records...", new frmProgress.ProgressOptions() { }))
                        {
                            Worker.RunWorkerAsync();
                            ProgressForm.Cancelled += new frmProgress.CancelledEventHandler(ProgressForm_Cancelled);
                            ProgressForm.ShowDialog(OwnerWindow);
                        }
                    }
                    else
                    {
                        // We don't need the progress form unless we'll be downloading summaries in batch.
                        Worker.RunWorkerAsync();
                    }
                }
                else { OnActivityCompleted(new ActivityCompletedEventArgs(this, this.Search, null, false)); }
            }
        }
    }
}
