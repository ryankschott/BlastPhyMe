using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChangLab.Common;
using ChangLab.Genes;
using ChangLab.NCBI.GenBank;

namespace Pilgrimage.Activities
{
    public class PopulateFromGenBank : Activity
    {
        private GenBankSearch Search { get; set; }
        private GenBankFetch Fetch { get; set; }
        public List<Gene> Results { get; set; }
        private bool Batched { get; set; }

        public PopulateFromGenBank(IWin32Window OwnerWindow) : base(OwnerWindow) { }

        public void Populate(IEnumerable<Gene> Genes, GenBankSearch SearchResult)
        {
            try
            {
                this.Search = SearchResult;
                this.Fetch = new GenBankFetch();
                Fetch.ProgressUpdate += new ProgressUpdateEventHandler(genBankFetch_ProgressUpdate);

                Batched = (Genes.Count() > this.Fetch.BatchSize);
                Worker.RunWorkerAsync(Genes);
                using (ProgressForm = new frmProgress("Downloading GenBank records for selected genes...", Batched, true, false, !Batched))
                {
                    ProgressForm.Cancelled += new frmProgress.CancelledEventHandler(ProgressForm_Cancelled);
                    ProgressForm.ShowDialog(this.OwnerWindow);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected internal override void ProgressForm_Cancelled(RunWorkerCompletedEventArgs e)
        {
            Fetch.CancelAsync();
            Worker.CancelAsync();
        }

        protected internal override void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            Fetch.FetchRecords(((IEnumerable<Gene>)e.Argument).Select(g => g.Accession).Distinct().ToList(), (this.Search != null ? this.Search.SearchResult : null));

            // FetchGenes() will return once all the results have been downloaded.
            Results = Fetch.Results;
            e.Result = Fetch.Results;
            if (Fetch.CancellationPending || Worker.CancellationPending) { e.Cancel = true; }
        }

        private void genBankFetch_ProgressUpdate(ProgressUpdateEventArgs e)
        {
            if (Worker != null && Worker.IsBusy)
            {
                if (!Batched)
                { e.StatusMessage = e.ProgressMessage; }
                else
                { if (e.StatusMessage == e.ProgressMessage) { e.ProgressMessage = string.Empty; } }
                Worker.ReportProgress(0, e);
            }
        }

        #region Static
        /// <summary>
        /// Quasi-synchronous because Populate() won't return until ProgressForm closes, and ProgressForm won't close until the Worker completes.
        /// </summary>
        /// <param name="Genes"></param>
        /// <param name="SearchResult"></param>
        public static bool PopulateSync(List<Gene> Genes, GenBankSearch SearchResult, IWin32Window OwnerWindow, bool BubbleUpError = false)
        {
            PopulateFromGenBank gb = new PopulateFromGenBank(OwnerWindow);
            gb.Populate(Genes, SearchResult);

            if (gb.Cancelled)
            { return false; }
            if (gb.Error != null)
            {
                if (BubbleUpError) { throw gb.Error; }
                else { Utility.ShowErrorMessage(OwnerWindow, gb.Error); return true; }
            }
            else
            {
                Genes.ForEach(g =>
                {
                    Gene match = gb.Results.FirstOrDefault(result => result.GenBankID == g.GenBankID);
                    g.Merge(match, true, true, false);
                });
                return true;
            }
        }

        public static bool PopulateSync(Gene Gene, GenBankSearch SearchResult, IWin32Window OwnerWindow)
        {
            return PopulateSync(new List<Gene>(new Gene[] { Gene }), SearchResult, OwnerWindow);
        }
        #endregion
    }
}
