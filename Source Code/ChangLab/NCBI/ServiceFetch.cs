using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using ChangLab.Common;

namespace ChangLab.NCBI
{
    public abstract class ServiceFetch<T>
    {
        protected internal abstract EUtilities.Databases Database { get; }
        protected internal abstract EUtilities.ReturnTypes ReturnType { get; }
        protected internal abstract EUtilitiesXMLParser<T> XMLParser { get; }
        internal ESearchHistory Search { get; private set; }

        public int BatchSize { get; internal set; }
        internal bool Batched { get; private set; }
        public int RequestDelayMilliseconds { get; internal set; }

        public bool CancellationPending { get; internal set; }
        public void CancelAsync() { CancellationPending = true; }

        public virtual List<T> Results { get; internal set; }

        public ServiceFetch()
        {
            BatchSize = 20;
            RequestDelayMilliseconds = 5000;
        }

        public List<T> FetchRecords(List<string> AccessionList, ESearchHistory Search = null)
        {
            this.Search = Search;
            
            string baseUrl = EUtilities.GetUrl(EUtilities.Services.EFetch, this.Database, (this.Search != null), this.Search)
                + (ReturnType != EUtilities.ReturnTypes.NotSpecified ? "&rettype=" + ReturnType.ToString().ToLower() : string.Empty);

            try
            {
                CancellationPending = false;
                Results = new List<T>();

                List<List<Tuple<string, int>>> accessionLists = AccessionList.Select((id, index) => new { id, index })
                    .GroupBy(id => id.index / BatchSize)
                    .Select(grp => grp.Select(g => new Tuple<string, int>(g.id, g.index)).ToList())
                    .ToList();
                Batched = accessionLists.Count > 1;

                OnProgressUpdate(new ProgressUpdateEventArgs()
                {
                    Setup = true,
                    CurrentMax = accessionLists.Count,
                    CurrentProgress = 0,
                    ProgressMessage = "Downloading " + AccessionList.Count.ToString() + " records in batches of " + BatchSize.ToString()
                                        + (Batched ? "\r\n" : string.Empty)
                });

                for (int i = 0; i < accessionLists.Count; i++)
                {
                    if (Batched && i > 0)
                    {
                        OnProgressUpdate(new ProgressUpdateEventArgs() { CurrentProgress = i, ProgressMessage = "Next request at " + DateTime.Now.AddMilliseconds(RequestDelayMilliseconds).ToStandardTimeString() });
                        System.Threading.Thread.Sleep(RequestDelayMilliseconds);
                    }

                    if (CancellationPending) { break; }

                    List<Tuple<string, int>> idBatch = accessionLists[i];
                    OnProgressUpdate(new ProgressUpdateEventArgs()
                    {
                        CurrentProgress = i,
                        ProgressMessage = "Downloading "
                                            + (accessionLists.Count == 1
                                                ? (AccessionList.Count == 1 ? "1 record" : AccessionList.Count.ToString() + " records")
                                                : "records " + (idBatch.Min(id => id.Item2) + 1).ToString()
                                                        + "-"
                                                        + (idBatch.Max(id => id.Item2) + 1).ToString()
                                                        + " of " + AccessionList.Count.ToString()
                                            )
                    });

                    string accessions = idBatch.Aggregate(string.Empty, (current, id) => current += (string.IsNullOrWhiteSpace(current) ? "" : ",") + id.Item1.ToString());
                    string url = baseUrl + "&id=" + accessions;

                    EUtilitiesXMLParser<T> parser = XMLParser;
                    List<T> results = parser.ParseFullRecord(XMLWebRequest.RequestDocument(url));
                    NormalizeResults(results);

                    // Validation
                    // Check to see if a result was compiled for everything that was submitted.
                    // Segmented GenBank records won't be, under the current methodology, when the segments sent back all have different GenBankIDs,
                    // which may always be the case for segmented records.  So when there's no result for an input gene, we need to resubmit an 
                    // individual EFetch request (from the same history cache) to check to see if it's segmented, and if so, compile a single record
                    // from the segments.

                    OnResultsDownloaded(new ResultsEventArgs() { Results = results });

                    Results.AddRange(results);
                    OnProgressUpdate(new ProgressUpdateEventArgs() { CurrentProgress = (i + 1), ProgressMessage = "Records downloaded" });

                    if (CancellationPending) { break; }
                }

                return Results;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected internal virtual void NormalizeResults(List<T> Results)
        {
            // Do nothing.
        }

        #region Events
        internal string LastProgressMessage { get; private set; }
        protected virtual void OnProgressUpdate(ProgressUpdateEventArgs e)
        {
            if (ProgressUpdate != null)
            {
                this.LastProgressMessage = e.ProgressMessage;
                ProgressUpdate(e);
            }
        }
        public event ProgressUpdateEventHandler ProgressUpdate;

        protected virtual void OnResultsDownloaded(ResultsEventArgs e) { if (ResultsDownloaded != null) { ResultsDownloaded(e); } }
        public delegate void ResultsEventHandler(ResultsEventArgs e);
        public event ResultsEventHandler ResultsDownloaded;
        public class ResultsEventArgs : EventArgs
        {
            public List<T> Results { get; set; }

            public ResultsEventArgs()
            {

            }
        }
        #endregion
    }
}
