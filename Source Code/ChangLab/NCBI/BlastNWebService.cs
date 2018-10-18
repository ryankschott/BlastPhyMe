using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bio;
using Bio.Web;
using Bio.Web.Blast;
using ChangLab.Common;
using ChangLab.Genes;

namespace ChangLab.NCBI
{
    public class BlastNWebService
    {
        public BlastNWebServiceConfigurationSettings ConfigurationSettings { get; private set; }
        private Jobs.Job CurrentJob { get; set; }

        private NCBIBlastHandler NCBIBlastService;
        public List<Request> Requests { get; private set; }
        internal Dictionary<Request, GeneBatch> BatchedRequests { get; private set; }
        public List<BlastSearchRecord> Results { get; private set; }

        public static int SequenceBatchSize { get { return 10; } }
        public static int AggregateBatchNucletoideLengthLimit { get { return 100000; } }
#if !PASSTHROUGH_BLASTN
        private int RequestDelay = 60;
#else
        private int RequestDelay = 5;
#endif
        private int RetryLimit = 3;

        public BlastNWebService(BlastNWebServiceConfigurationSettings ConfigurationSettings, Jobs.Job Job)
        {
            this.ConfigurationSettings = ConfigurationSettings;
            this.CurrentJob = Job;
            this.Requests = new List<Request>();

            // Set up web service proxy class
            NCBIBlastService = new NCBIBlastHandler(new ConfigParameters
            {
                UseBrowserProxy = false,
                UseAsyncMode = true,
                Connection = new Uri("https://www.ncbi.nlm.nih.gov/blast/Blast.cgi"),
                DefaultTimeout = ConfigurationSettings.RequestTimeout
            });
        }

        public bool CancellationPending { get; private set; }
        public void CancelAsync()
        {
            CancellationPending = true;
            OnProgressUpdate(new ProgressUpdateEventArgs() { ProgressMessage = "Job is being cancelled..." });
        }

        /// <summary>
        /// Primary entry point for submitting a list of sequences to NCBI.
        /// </summary>
        public void BatchGenesIntoRequests(List<Gene> Genes)
        {
            // Split the input genes into batches instead of submitting everything
            // Originally this was a simple batch by SequenceBatchSize, but if the sequences are too large then we run into the CPU error.
            // Now we're limiting a batch by either aggregate base pair total or the SequenceBatchSize; whichever comes first.

            // Old code:
            //var geneBatches = Genes.Select((gene, index) => new { Gene = gene, Index = index })
            //    .GroupBy(g => g.Index / SequenceBatchSize)
            //    .Select(grp => grp.ToList());

            List<GeneBatch> geneBatches =
                Genes
                .Select((gene, index) => new BatchedGene() { Gene = gene, Index = index })
                .Aggregate(new List<GeneBatch>(), (current, gene) =>
                {
                    if ((current.Count == 0)
                        || (current.Last().Count == SequenceBatchSize)
                        || ((current.Last().NucleotideCount + gene.Gene.Nucleotides.Length) > AggregateBatchNucletoideLengthLimit))
                    {
                        current.Add(new GeneBatch());
                    }

                    current.Last().Add(gene);
                    return current;
                });

            this.BatchedRequests =
                geneBatches
                .ToDictionary(batch =>
                {
                    Request req = new Request()
                    {
                        JobID = CurrentJob.ID,
                        LastStatus = RequestStatus.Pending,
                        TargetDatabase = ConfigurationSettings.DatabaseName,
                        Algorithm = ConfigurationSettings.Service.ToString()
                    };
                    req.Save();
                    req.SetGenesStatus(batch.Select(g => g.Gene.ID), Jobs.JobGeneStatuses.NotSubmitted);

                    return req;
                }, batch => batch);
        }

        /// <summary>
        /// Only used directly if restarting a job such that existing RequestIDs may be valid.
        /// </summary>
        public void SubmitToNCBI()
        {
            try
            {
                Results = new List<BlastSearchRecord>();
                int retry = 0;
                int geneCount = BatchedRequests.Aggregate(0, (current, batch) => current += batch.Value.Count);
                this.Requests.AddRange(BatchedRequests.Select(kv => kv.Key));

                OnProgressUpdate(new ProgressUpdateEventArgs() { Setup = true, TotalMax = BatchedRequests.Count(), TotalProgress = 0 });
                ProgressUpdateEventArgs totalProgress = new ProgressUpdateEventArgs() { TotalProgress = 0 };

                foreach (var batchedRequest in BatchedRequests)
                {
                    Request request = batchedRequest.Key;
                    GeneBatch batch = batchedRequest.Value;
                    ServiceRequestInformation info = null;

                    try
                    {
                        // Each batch is wrapped in a try {} catch {} so that we can have a batch fail but not kill the whole job because of it.
                        // The next batch will be attempted and may well succeed, as might the rest of the job, and then at the end the user should
                        // be shown the calculus of what succeeded and what needs to be resubmitted.
                        int lBound = batch.First().Index + 1; int uBound = batch.Last().Index + 1;
                        List<ISequence> sequenceBatch = new List<ISequence>(batch.Select(g => new Sequence(Alphabets.AmbiguousDNA, g.Gene.Nucleotides) { ID = g.Gene.ID }));

                        string progressMessage = "Submitting ";
                        if (geneCount > SequenceBatchSize)
                        { progressMessage += "sequences (" + lBound.ToString("N0") + "-" + uBound.ToString("N0") + " of " + geneCount.ToString() + ") "; }
                        else
                        { progressMessage += "all sequences (" + geneCount.ToString() + ") "; }
                        progressMessage += "to NCBI...";

                        OnProgressUpdate(new ProgressUpdateEventArgs() { ProgressMessage = progressMessage });
                        BlastParameters searchParams = null;

                        #region Submit Request
                        string requestId = request.RequestID;

                        if (!string.IsNullOrEmpty(requestId))
                        {
                            // This is a re-submit of a previously submitted request.  We need to check with the NCBI servers to see if results are
                            // available, and if so just download those instead of resubmitting the sequences.  If the results have expired then
                            // we'll need to resubmit the batch's sequences as a new request.

                            // First we associate the request with the current job.  For now this means a new entry in NCBI.Request, to simplify
                            // things instead of having to deal with one request belonging to two or more jobs.
                            request.ID = 0;
                            request.JobID = this.CurrentJob.ID;

                            CheckForResults(ref request, ref info);

                            if (CancellationPending) { return; }

                            if (!ServiceRequestInformation.Equals(info, null) && info.Status == ServiceRequestStatus.Ready)
                            {
                                // Skip to downloading the results.
                            }
                            else
                            {
                                // Empty out the request ID so we can re-submit the batch.
                                request.RequestID = string.Empty;
                                request.StartTime = DateTime.Now;
                                request.EndTime = DateTime.MinValue;
                                request.LastStatus = RequestStatus.Pending;
                                request.SetGenesStatus(batch.Select(g => g.Gene.ID), Jobs.JobGeneStatuses.NotSubmitted);
                                request.Save();
                            }
                        }

                        if (string.IsNullOrEmpty(request.RequestID))
                        {
                            retry = 0;
                            try
                            {
                                while (retry < RetryLimit)
                                {
                                    try
                                    {
                                        retry++;
                                        if (retry > 1) { OnProgressUpdate(new ProgressUpdateEventArgs() { ProgressMessage = retry.NumberWithSuffix() + " attempt..." }); }
                                        searchParams = ConfigurationSettings.NCBISearchParams();
#if !PASSTHROUGH_BLASTN
                                        requestId = NCBIBlastService.SubmitRequest(sequenceBatch, searchParams);
#else
                                        System.Threading.Thread.Sleep(500);
                                        if (totalProgress.TotalProgress == 1)
                                        {
                                            throw new Exception("Nope, not happening.");
                                        }
                                        requestId = "DEBUG_" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 14).ToUpper();
#endif
                                        break;
                                    }
                                    catch (Exception retryEx)
                                    {
                                        if (retry < RetryLimit)
                                        {
                                            if (!PauseForRetry("Failed to submit sequences on {0} attempt (limit of {1} attempts)", retry)) { return; }
                                            continue;
                                        }
                                        else { throw new Exception("Attempts to submit sequences exceeded the retry limit (" + RetryLimit.ToString() + ")", retryEx); }
                                    }
                                }
                                if (string.IsNullOrWhiteSpace(requestId))
                                {
                                    throw new Exception("A request ID was not received.");
                                }

                            }
                            catch (Exception ex)
                            {
                                request.RequestID = "ERROR"; // The database will convert this into a unique Request ID.
                                request.StartTime = DateTime.Now;

                                // Log the exception to the database and then move on to the next batch.
                                throw ex;
                            }

                            // We're using the time we receive the RequestID back as the start time because that seems more likely to be closer to when it
                            // was actually enqueued instead of recording a start time before submitting the request, which would include how long it took
                            // for the server to enqueue the request.
                            request.RequestID = requestId;
                            request.StartTime = DateTime.Now;
                            request.Save();
                            request.SetGenesStatus(batch.Select(g => g.Gene.ID), Jobs.JobGeneStatuses.Submitted);

                            OnProgressUpdate(new ProgressUpdateEventArgs() { ProgressMessage = "NCBI is now processing request ID " + requestId });
                        #endregion

                            if (CancellationPending) { return; }

                            // Start polling for results
                            CheckForResults(ref request, ref info);

                            if (CancellationPending) { return; }
                        }

                        #region Fetch Results
                        // So, yes, this looks a little weird for a null test, but the == operator has been overloaded in ServiceRequestInformation and
                        // you get a stack overflow error when trying to directly test for null.  This works just as well, though.
                        if (!ServiceRequestInformation.Equals(info, null) && info.Status == ServiceRequestStatus.Ready)
                        {
                            OnProgressUpdate(new ProgressUpdateEventArgs() { ProgressMessage = "Downloading results...", StatusMessage = string.Empty });

                            try
                            {
                                retry = 0;
                                IList<BlastResult> ncbiResults = null;
                                while (retry < RetryLimit)
                                {
                                    try
                                    {
                                        retry++;
                                        if (retry > 1) { OnProgressUpdate(new ProgressUpdateEventArgs() { ProgressMessage = retry.NumberWithSuffix() + " attempt..." }); }
#if !PASSTHROUGH_BLASTN
                                        ncbiResults = NCBIBlastService.FetchResultsSync(requestId, searchParams);
#endif
                                        break;
                                    }
                                    catch (Exception retryEx)
                                    {
                                        if (retry < RetryLimit)
                                        {
                                            if (!PauseForRetry("Failed to download results on {0} attempt (limit of {1} attempts)", retry)) { return; }
                                            continue;
                                        }
                                        else { throw retryEx; }
                                    }
                                }

#if !PASSTHROUGH_BLASTN
                                if (ncbiResults == null)
                                {
                                    OnProgressUpdate(new ProgressUpdateEventArgs() { ProgressMessage = "Not able to retrieve results for request ID " + requestId, StatusMessage = string.Empty });
                                }
                                else if (ncbiResults.Count == 0)
                                {
                                    OnProgressUpdate(new ProgressUpdateEventArgs() { ProgressMessage = "No results returned for request ID " + requestId, StatusMessage = string.Empty });
                                }
                                else
                                {
                                    if (CancellationPending) { return; }

                                    // We flag each submitted sequence for this request as having "been processed", to distinguish that the sequence was
                                    // sent to NCBI, processed, and we successfully performed a download of results.  There may be no results for some
                                    // sequences but we still need to flag that the record was successfully processed so that the user doesn't keep 
                                    // submitting a sequence that isn't going to bring any results back.
                                    request.SetGenesStatus(batch.Select(g => g.Gene.ID), Jobs.JobGeneStatuses.Processed);

                                    OnResultsDownloaded(new ResultsEventArgs() { Results = ncbiResults[0].Records, Request = request });
                                }
#else
                                OnProgressUpdate(new ProgressUpdateEventArgs() { ProgressMessage = "No results returned for request ID " + requestId, StatusMessage = string.Empty });
#endif
                            }
                            catch (Exception ex)
                            {
                                throw new Exception("Attempts to retrive results for request ID " + requestId + " exceeded the retry limit (" + RetryLimit.ToString() + ")", ex);
                            }
                        }
                        #endregion

                        if (CancellationPending) { return; }
                    }
                    catch (Exception ex)
                    {
                        request.LastStatus = RequestStatus.Error;
                        request.EndTime = DateTime.Now;
                        request.Save();

                        // Record the exception, but don't fail out of the job.
                        Jobs.JobException jex = new Jobs.JobException(CurrentJob.ID, request.ID, "An error occured attempting to process gene sequences through BLASTN at NCBI", ex);
                        jex.Save(); this.CurrentJob.Exceptions.Add(jex);

                        OnProgressUpdate(new ProgressUpdateEventArgs() { ProgressMessage = ex.Message, StatusMessage = string.Empty });
                    }
                    finally
                    {
                        totalProgress.TotalProgress++; OnProgressUpdate(totalProgress);

                        if (CancellationPending)
                        {
                            request.LastStatus = RequestStatus.Canceled;
                            request.StatusInformation = "Cancelled by user.";
                            request.EndTime = DateTime.Now;
                            request.Save();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CheckForResults(ref Request Request, ref ServiceRequestInformation Info)
        {
            bool completed = false;
            Info = null;

            while (!completed)
            {
                if (RequestNCBIResults(Request.RequestID, ref Info, false))
                {
                    try
                    {
                        completed = true;
                        switch (Info.Status)
                        {
                            case ServiceRequestStatus.Error:
                                // There's no retry logic in here because this is NCBI telling us there's something wrong with our request
                                // or otherwise on their end, which is different to the web service call itself failing, which would be
                                // caught by the logic within RequestNCBIResults().
                                throw new Exception(string.Format("Request failed.  {0}: {1}", Info.Status, Info.StatusInformation));
                            case ServiceRequestStatus.Canceled:
                                throw new Exception("Request canceled" + (string.IsNullOrWhiteSpace(Info.StatusInformation) ? ": " + Info.StatusInformation : string.Empty));
                            case ServiceRequestStatus.Ready:
                                OnProgressUpdate(new ProgressUpdateEventArgs() { ProgressMessage = "Results are ready", StatusMessage = string.Empty, CurrentProgress = RequestDelay });
                                break;
                            default: // Queued, Waiting
                                completed = false;
                                break;
                        }
                    }
                    finally
                    {
                        switch (Info.Status)
                        {
                            case ServiceRequestStatus.Error:
                            case ServiceRequestStatus.Canceled:
                                break;
                            default:
                                Request.LastStatus = Request.ConvertServiceRequestStatus(Info.Status);
                                if (completed) { Request.EndTime = DateTime.Now; }
                                Request.Save();
                                break;
                        }
                    }
                }
                else
                {
                    // RequestNCBIResults() will return false if the job has been cancelled.  We might want some additional cleanup here if
                    // there ever turns out to be a way to send a message to NCBI to cancel a request.
                    break;
                }
            }
        }

        private bool RequestNCBIResults(string RequestID, ref ServiceRequestInformation info, bool SkipDelay)
        {
            try
            {
                if (!SkipDelay)
                {
                    if (!Pause("Next status request will occur at {0}", "Waiting for results; next status request in {0} seconds"))
                    { return false; /* Pause() == false means the user cancelled */ }
                }

                OnProgressUpdate(new ProgressUpdateEventArgs() { StatusMessage = "Requesting status of results" });
                int retry = 0;
                while (retry < RetryLimit)
                {
                    try
                    {
                        retry++;
                        if (retry > 1) { OnProgressUpdate(new ProgressUpdateEventArgs() { ProgressMessage = retry.NumberWithSuffix() + " attempt..." }); }
#if !PASSTHROUGH_BLASTN
                        info = NCBIBlastService.GetRequestStatus(RequestID); 
#else
                        info = new ServiceRequestInformation() { Status = ServiceRequestStatus.Ready };
#endif
                        if (info.Status == ServiceRequestStatus.Error
                            && info.StatusInformation.Contains("existing connection was forcibly closed"))
                        {
                            // This is Bio.NET telling us there was a network failure; not NCBI telling us there's something wrong with our request.
                            throw new Exception(info.StatusInformation);
                        }
                        break;
                    }
                    catch (Exception retryEx)
                    {
                        if (retry < RetryLimit)
                        {
                            OnProgressUpdate(new ProgressUpdateEventArgs() { ProgressMessage = "Failed to request status on " + retry.NumberWithSuffix() + " attempt (limit of " + RetryLimit.ToString() + " attempts)" });
                            continue;
                        }
                        else { throw new Exception("Attempts to request status for request ID " + RequestID + " exceeded the retry limit (" + RetryLimit.ToString() + ")", retryEx); }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool Pause(string ProgressMessage, string StatusMessage)
        {
            DateTime requestStatusAt = DateTime.Now.AddSeconds(RequestDelay);

            OnProgressUpdate(new ProgressUpdateEventArgs()
            {
                ProgressMessage = string.Format(ProgressMessage, requestStatusAt.ToStandardTimeString()),
                Setup = true,
                CurrentMax = RequestDelay,
                CurrentProgress = 0
            });

            while (DateTime.Now < requestStatusAt)
            {
                int remainingSeconds = Convert.ToInt32(requestStatusAt.Subtract(DateTime.Now).TotalSeconds);
                OnProgressUpdate(new ProgressUpdateEventArgs()
                {
                    StatusMessage = string.Format(StatusMessage, remainingSeconds),
                    CurrentProgress = ((RequestDelay) - remainingSeconds)
                });
                System.Threading.Thread.Sleep(500);
                if (CancellationPending) { return false; }
            }

            return true;
        }

        private bool PauseForRetry(string RetryMessage, int RetryCount)
        {
            OnProgressUpdate(new ProgressUpdateEventArgs() { ProgressMessage = string.Format(RetryMessage, RetryCount, RetryLimit) });
            if (!Pause("Next attempt will occur at {0}", "Next attempt in {0} seconds"))
            { return false; /* Pause() == false means the user cancelled */ }

            return true;
        }

        #region Events
        protected virtual void OnProgressUpdate(ProgressUpdateEventArgs e) { if (ProgressUpdate != null) { ProgressUpdate(e); } }
        public event ProgressUpdateEventHandler ProgressUpdate;

        protected virtual void OnResultsDownloaded(ResultsEventArgs e) { if (ResultsDownloaded != null) { ResultsDownloaded(e); } }
        public delegate void ResultsEventHandler(ResultsEventArgs e);
        public event ResultsEventHandler ResultsDownloaded;
        public class ResultsEventArgs : EventArgs
        {
            public IList<BlastSearchRecord> Results { get; set; }
            public Request Request { get; set; }

            public ResultsEventArgs() { }
        }
        #endregion
    }

    public enum BlastNServices
    {
        blastn = 1,
        dc_megablast = 2,
        megablast = 3
    }

    public class BlastNWebServiceConfigurationSettings : Jobs.JobOptions
    {
        public string DatabaseName
        {
            get { return _optionsRoot.SafeAttributeValue("DatabaseName", "nr"); }
            set { _optionsRoot.SetAttributeValue("DatabaseName", value); }
        }

        public BlastNServices Service
        {
            get { return (BlastNServices)Enum.Parse(typeof(BlastNServices), _optionsRoot.SafeAttributeValue("Service", "blastn")); }
            set { _optionsRoot.SetAttributeValue("Service", value.ToString()); }
        }

        public int MaximumTargetSequences
        {
            get { return _optionsRoot.SafeAttributeValueAsInt("MaximumTargetSequences", 100); }
            set { _optionsRoot.SetAttributeValue("MaximumTargetSequences", value); }
        }

        public double ExpectThreshold
        {
            get { return _optionsRoot.SafeAttributeValueAsDouble("Expect", 100); }
            set { _optionsRoot.SetAttributeValue("Expect", value); }
        }

        public int WordSize
        {
            get { return _optionsRoot.SafeAttributeValueAsInt("WordSize", 11); }
            set { _optionsRoot.SetAttributeValue("WordSize", value); }
        }

        public int NucleotideMatchReward
        {
            get { return _optionsRoot.SafeAttributeValueAsInt("NucleotideMatchReward", 1); }
            set { _optionsRoot.SetAttributeValue("NucleotideMatchReward", value); }
        }

        public int NucleotideMismatchPenalty
        {
            get { return _optionsRoot.SafeAttributeValueAsInt("NucleotideMismatchPenalty", -3); }
            set { _optionsRoot.SetAttributeValue("NucleotideMismatchPenalty", value); }
        }

        public int[] GapCosts
        {
            get { return _optionsRoot.SafeAttributeValue("GapCosts", "5,2").Split(new char[] { ',' }).Select(s => int.Parse(s)).ToArray(); }
            set { _optionsRoot.SetAttributeValue("GapCosts", value.Concatenate(",")); }
        }

        public int RequestTimeout
        {
            get { return _optionsRoot.SafeAttributeValueAsInt("RequestTimeout", 300); }
            set { _optionsRoot.SetAttributeValue("RequestTimeout", value); }
        }

        public BlastNWebServiceConfigurationSettings() : base()
        {
            Service = BlastNServices.blastn;
        }

        public BlastNWebServiceConfigurationSettings(string XML) : base(XML) { }

        public static BlastNWebServiceConfigurationSettings DefaultAlgorithmParameters(BlastNServices Service)
        {
            BlastNWebServiceConfigurationSettings settings = new BlastNWebServiceConfigurationSettings();

            switch (Service)
            {
                case BlastNServices.blastn:
                case BlastNServices.dc_megablast:
                    settings.WordSize = 11;
                    settings.NucleotideMatchReward = 2;
                    settings.NucleotideMismatchPenalty = -3;
                    settings.GapCosts = new int[] { 5, 2 };
                    break;

                case BlastNServices.megablast:
                    settings.WordSize = 28;
                    settings.NucleotideMatchReward = 1;
                    settings.NucleotideMismatchPenalty = -2;
                    settings.GapCosts = new int[] { 0, 0 };
                    break;
            }

            return settings;
        }

        internal BlastParameters NCBISearchParams()
        {
            BlastParameters ncbiSearchParams = new BlastParameters();
            ncbiSearchParams.Add("Program", "blastn");
            ncbiSearchParams.Add("Database", DatabaseName);
            switch (Service)
            {
                case BlastNServices.blastn:
                    break;
                case BlastNServices.megablast:
                case BlastNServices.dc_megablast:
                    ncbiSearchParams.Add("Service", "megablast");
                    break;
            }
            ncbiSearchParams.Add("HitlistSize", this.MaximumTargetSequences.ToString());
            ncbiSearchParams.Add("Expect", this.ExpectThreshold.ToString());
            ncbiSearchParams.Add("WordSize", this.WordSize.ToString());
            ncbiSearchParams.Add("NucleotideMatchReward", this.NucleotideMatchReward.ToString());
            ncbiSearchParams.Add("NucleotideMismatchPenalty", this.NucleotideMismatchPenalty.ToString());
            if (this.GapCosts[0] != 0) { ncbiSearchParams.Add("GapCosts", this.GapCosts.Concatenate(" ")); }
            
            return ncbiSearchParams;
        }
    }
}
