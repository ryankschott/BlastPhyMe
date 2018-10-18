using System.ComponentModel;
using System.Windows.Forms;
using ChangLab.NCBI;
using ChangLab.NCBI.GenBank;
using Pilgrimage.GeneSequences.Search;

namespace Pilgrimage.Activities
{
    public class SearchGenBankForNucleotides : Activity
    {
        public SearchGenBankForNucleotides(IWin32Window OwnerWindow) : base(OwnerWindow) { }
        private string SearchQuery { get; set; }
        private bool UseHistory { get; set; }
        private ESearchHistory SelectedHistory { get; set; }
        
        #region GenBank
        public void Search()
        {
            using (frmSearchGenBank frm = new frmSearchGenBank())
            {
                if (frm.ShowDialog(this.OwnerWindow) == System.Windows.Forms.DialogResult.OK)
                {
                    this.SearchQuery = frm.txtSearchQuery.Text;
                    this.UseHistory = frm.UseHistory;
                    this.SelectedHistory = frm.SelectedHistory;

                    Worker.RunWorkerAsync();
                    using (ProgressForm = new frmProgress("Searching GenBank...", false, true, false, true))
                    {
                        ProgressForm.Cancelled += new frmProgress.CancelledEventHandler(ProgressForm_Cancelled);
                        ProgressForm.ShowDialog();
                    }
                }
            }
        }

        protected internal override void ProgressForm_Cancelled(RunWorkerCompletedEventArgs e)
        {
            Worker.CancelAsync();
        }

        protected internal override void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            GenBankSearch search = null;

            if (this.UseHistory)
            {
                search = new GenBankSearch()
                {
                    SearchResult = new ESearchHistory()
                    {
                        ResultCount = this.SelectedHistory.ResultCount,
                        QueryKey = this.SelectedHistory.QueryKey,
                        WebEnvironment = this.SelectedHistory.WebEnvironment,
                        ReturnMaximum = this.SelectedHistory.ReturnMaximum
                    }
                };

                if (this.SelectedHistory.ResultCount > 0)
                {
                    search.ResultsSummary(0);
                    if (search.Count == 0)
                    {
                        // The results have expired.
                        this.SearchQuery = this.SelectedHistory.Term;
                        search = null;
                    }
                }
            }

            if (search == null)
            {
                search = new GenBankSearch();
                search.Search(this.SearchQuery, Program.Settings.CurrentRecordSet.ID);
            }
            e.Result = search;

            if (Worker.CancellationPending) { e.Cancel = true; }
        }

        protected internal override void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Completed = true;
            CloseProgressForm(DialogResult.OK);

            if ((e.Error != null) || (e.Cancelled))
            {
                OnActivityCompleted(new ActivityCompletedEventArgs(this, null, e.Error, e.Cancelled));
            }
            else
            {
                using (frmSearchResultsGenBank frm = new frmSearchResultsGenBank((GenBankSearch)e.Result))
                {
                    frm.ShowDialog(this.OwnerWindow);
                    OnActivityCompleted(new ActivityCompletedEventArgs(this, frm.EditedSubSetIDs, e.Error, false));
                }
            }
        }
        #endregion
    }
}
