using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using ChangLab.Common;

namespace ChangLab.NCBI
{
    public abstract class ServiceSearch<T>
    {
        public virtual ESearchHistory SearchResult { get; set; }
        public abstract EUtilities.Databases Database { get; }
        public abstract EUtilitiesXMLParser<T> XMLParser { get; }

        public int RequestDelayMilliseconds { get; internal set; }
        public bool CancellationPending { get; internal set; }
        public void CancelAsync() { CancellationPending = true; }

        public int Count { get { return Results.Count; } }
        public List<T> Results { get { return IndexedResults.Select(kv => kv.Value).ToList(); } }
        protected internal Dictionary<int, T> IndexedResults { get; set; }
        public IEnumerable<T> GetRange(int StartIndex)
        {
            return IndexedResults.Where(kv => kv.Key >= StartIndex && kv.Key < StartIndex + SearchResult.ReturnMaximum).Select(kv => kv.Value).ToList();
        }

        public ServiceSearch()
        {
            this.IndexedResults = new Dictionary<int, T>();
            this.RequestDelayMilliseconds = 2000;
        }

        public virtual void Search(string Term, string RecordSetID, bool FetchFirstSummary = true)
        {
            this.SearchResult = EUtilities.Search(this.Database, Term, RecordSetID);

            if (FetchFirstSummary && this.SearchResult.IDList.Count != 0)
            {
                this.ResultsSummary(0);
            }
        }

        public virtual void ResultsSummary(int ReturnStart, int ReturnMaxmium = 0)
        {
            try
            {
                int targetMaximum = (ReturnMaxmium != 0 ? (this.SearchResult.ResultCount > ReturnMaxmium ? ReturnMaxmium : this.SearchResult.ResultCount) : this.SearchResult.ReturnMaximum);
                bool showProgress = (targetMaximum > this.SearchResult.ReturnMaximum); // Because if otherwise then we'll be able to get everything in one request.
                if (showProgress)
                {
                    OnProgressUpdate(new ProgressUpdateEventArgs()
                    {
                        Setup = true,
                        CurrentMax = targetMaximum,
                        StatusMessage = "Downloading " + targetMaximum.ToString("N0") + " summary records..."
                    });
                }

                for (int i = 0; i < targetMaximum; i += this.SearchResult.ReturnMaximum)
                {
                    if (this.CancellationPending) { return; }
                    int startIndex = i + ReturnStart;

                    if (showProgress)
                    {
                        OnProgressUpdate(new ProgressUpdateEventArgs()
                        {
                            CurrentProgress = startIndex,
                            StatusMessage = "Downloading summary records " + (startIndex + 1).ToString("N0")
                                                + "-"
                                                + (startIndex + this.SearchResult.ReturnMaximum + (startIndex + this.SearchResult.ReturnMaximum < targetMaximum ? 1 : 0)).ToString("N0")
                                                + " of " + targetMaximum.ToString("N0")
                        });
                    }

                    List<T> records = XMLParser.ParseDocSum(NCBI.EUtilities.Summary(this.Database, this.SearchResult, startIndex, this.SearchResult.ReturnMaximum));
                    foreach (var record in records.Select((g, index) => new { Record = g, Index = index }))
                    {
                        IndexedResults.Add(record.Index + startIndex, record.Record);
                    }

                    if (this.CancellationPending) { return; }

                    if (showProgress)
                    {
                        System.Threading.Thread.Sleep(RequestDelayMilliseconds);
                    }
                }

                if (showProgress) { OnProgressUpdate(new ProgressUpdateEventArgs() { CurrentProgress = targetMaximum, StatusMessage = "All summary records downloaded" }); }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected virtual void OnProgressUpdate(ProgressUpdateEventArgs e)
        {
            if (ProgressUpdate != null)
            {
                ProgressUpdate(e);
            }
        }
        public event ProgressUpdateEventHandler ProgressUpdate;
    }
}
