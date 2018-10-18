using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using ChangLab.Common;

namespace ChangLab.PAML.CodeML
{
    public class CodeMLProcess
    {
        private List<Tree> Trees { get; set; }
        private int ConcurrentProcesses { get; set; }
        private ProcessPriorityClass Priority { get; set; }
        private string CodeMLExecutablePath { get; set; }
        private Jobs.Job CurrentJob { get; set; }
        public string JobDirectory { get { return this.CurrentJob.JobDirectory; } set { this.CurrentJob.JobDirectory = value; } }
        private CodeMLProcessOptions Options { get; set; }

        private Dictionary<BackgroundWorker, CodeMLAnalysisOption> WorkerThreads { get; set; }

        public List<CodeMLAnalysisOption> Analyses { get; private set; }
        public List<Result> Results { get; private set; }

        public bool CancellationPending { get; private set; }
        public void CancelAsync() { this.CancellationPending = true; }

        public CodeMLProcess(CodeMLProcessOptions Options, Jobs.Job CurrentJob)
        {
            this.CurrentJob = CurrentJob;
            this.Options = Options;
            
            this.Trees = Options.Trees;
            this.ConcurrentProcesses = Options.ConcurrentProcesses;
            this.Priority = Options.Priority;
            this.CodeMLExecutablePath = Options.CodeMLExecutablePath;
            this.JobDirectory = Options.WorkingDirectory;
            this.Results = new List<Result>();
        }

        public void RunAnalyses()
        {
            // this.Trees should be checked to see if it's already been populated, and if so the List<CodeMLAnalysisOption> instance below hsould be
            // set up as the delta of what's not marked as Completed in the source this.Trees collection, for all options and configurations.
            // This means we'd need to pass a List<Result> collection into the constructor as well, to check to see what anaylsis options have
            // already been run.

            // For each tree we need to run each analysis configuration, therefore I can collapse the hierarchy
            // At the same time, analysis configurations with intervals for kappa and/or omega need to be split by their interval value, generating
            // a different Analysis instance (because each Result object will have different k/w values).
            Analyses = Trees
                .Aggregate(new List<CodeMLAnalysisOption>(), (current, t) =>
                    {
                        current.AddRange(t.AnalysisConfigurations.Aggregate(new List<CodeMLAnalysisOption>(), (tree_configs, cf) =>
                            {
                                cf.Analyses = cf.GenerateAnalysisShells(t).Select(a =>
                                    {
                                        a.Tree = t;
                                        a.TreeFilePath = t.TreeFilePath;
                                        a.SequencesFilePath = t.SequencesFilePath;
                                        a.Status = Jobs.JobStatuses.Pending;
                                        a.OutputData = "Queued for processing\r\n";
                                        return a;
                                    }).ToList();

                                tree_configs.AddRange(cf.Analyses);
                                return tree_configs;
                            }));
                        return current;
                    });
            // Set the index values
            Analyses.Select((a, i) => a.Index = i);

            OnProgressUpdate(new ProgressUpdateEventArgs()
            {
                CurrentMax = Analyses.Count,
                ProgressMessage = "Processing " + Analyses.Count + " PAML requests",
                Source = Analyses
            });

            // Now start running the analyses.
            try
            {
                // Set up a thread for each CPU
                WorkerThreads = Enumerable.Range(1, ConcurrentProcesses).ToList().Select(i => new KeyValuePair<BackgroundWorker, CodeMLAnalysisOption>(NewWorkerThread(), null)).ToDictionary(kv => kv.Key, kv => kv.Value);

                // Assign analyses to worker threads and wait for the threads to complete
                // Checking IsBusy isn't quite good enough, because once the worker hits RunWorkerCompleted() it's no longer Busy, but there is still
                // code that needs to be executed to wrap up the Analysis object.  The last line of that code sets the Status to Completed.
                while (Analyses.Any(a => a.Status == Jobs.JobStatuses.Pending || a.Status == Jobs.JobStatuses.Running))
                {
                    if (this.CancellationPending)
                    {
                        foreach (var worker in WorkerThreads.Where(kv => kv.Value != null && kv.Value.Status == Jobs.JobStatuses.Running))
                        {
                            // Terminate the running codeml processes.
                            if (worker.Value.Process != null) { try { worker.Value.Process.Kill(); } catch { } }
                        }

                        // Mark the not-yet-finished trees and configurations as Cancelled. Failed takes precedence over Cancelled as a final status.
                        foreach (Tree tree in this.Trees.Where(t => !Jobs.JobStatusCollection.CompletedOrFailed(t.Status.Key)))
                        {
                            tree.UpdateStatus(Jobs.JobStatuses.Cancelled);

                            tree.AnalysisConfigurations
                                .Where(cf => !Jobs.JobStatusCollection.CompletedOrFailed(cf.Status.Key))
                                .ToList()
                                .ForEach(cf => cf.UpdateStatus(Jobs.JobStatuses.Cancelled));
                        }

                        // Mark the pending analyses as cancelled
                        this.Analyses.Where(a => a.Status == Jobs.JobStatuses.Pending).ToList().ForEach(a => a.Status = Jobs.JobStatuses.Cancelled);
                        break;
                    }
                    else
                    {
                        CodeMLAnalysisOption unprocessed = Analyses.FirstOrDefault(a => a.Status == Jobs.JobStatuses.Pending);
                        BackgroundWorker worker = WorkerThreads.FirstOrDefault(kv => !kv.Key.IsBusy && (kv.Value == null || kv.Value.Status != Jobs.JobStatuses.Running)).Key;
                        if (unprocessed != null && worker != null)
                        {
                            // If there is an unprocessed analysis and an available worker thread, start the analysis
                            unprocessed.Status = Jobs.JobStatuses.Running;
                            WorkerThreads[worker] = unprocessed;
                            worker.RunWorkerAsync(unprocessed);
                        }
                        else
                        {
                            // All of the threads are busy, so we'll go to sleep for a bit and then loop back through the while{} to check again.
                            System.Threading.Thread.Sleep(500);
                            OnProgressUpdate(new ProgressUpdateEventArgs() { });
                        }
                    }
                }

                // Final progress update
                OnProgressUpdate(new ProgressUpdateEventArgs() { Source = Analyses });

                // Capture any errors that occured for any of the analysis options.
                this.CurrentJob.Exceptions.AddRange(Analyses
                    .Where(a => a.Exceptions.Count != 0)
                    .Select(a => 
                        {
                            Jobs.JobException jex = new Jobs.JobException(CurrentJob.ID, 0, "An error occured attempting to run PAML");
                            jex.Save(); this.CurrentJob.Exceptions.Add(jex);
                            return jex;
                        }));
            }
            catch (Exception ex)
            {
                Jobs.JobException jex = new Jobs.JobException(CurrentJob.ID, 0, "An error occured attempting to run PAML", ex);
                jex.Save(); this.CurrentJob.Exceptions.Add(jex);
            }
        }

        /// <summary>
        /// Used to cancel a single codeml instance.
        /// </summary>
        public void CancelProcess(Guid OptionID)
        {
            if (WorkerThreads.Any(kv => GuidCompare.Equals(kv.Value.ID.ToString(), OptionID.ToString())))
            {
                KeyValuePair<BackgroundWorker, CodeMLAnalysisOption> worker = WorkerThreads.First(kv => GuidCompare.Equals(kv.Value.ID.ToString(), OptionID.ToString()));
                if (worker.Value != null)
                {
                    switch (worker.Value.Status)
                    {
                        case Jobs.JobStatuses.Running:
                            // Terminate the running codeml process.
                            if (worker.Value.Process != null) { try { worker.Key.CancelAsync(); worker.Value.Process.Kill(); } catch { } }
                            break;
                        case Jobs.JobStatuses.Pending:
                            // Set the status to cancelled so that the cycler in RunAnalyses() won't pick it up and start it
                            // We'll only step into here if the worker thread got created but the process hasn't been started yet (unlikely).
                            worker.Value.Status = Jobs.JobStatuses.Cancelled;
                            OnProgressUpdate(new ProgressUpdateEventArgs() { Source = worker.Value });
                            break;
                        default:
                            // For all other possibilities, do nothing
                            break;
                    }
                }
            }
            else if (Analyses.Any(o => GuidCompare.Equals(o.ID.ToString(), OptionID.ToString())))
            {
                // Set the status to cancelled so that the cycler in RunAnalyses() won't pick it up and start it
                CodeMLAnalysisOption option = Analyses.First(o => GuidCompare.Equals(o.ID.ToString(), OptionID.ToString()));
                option.Status = Jobs.JobStatuses.Cancelled;
                OnProgressUpdate(new ProgressUpdateEventArgs() { Source = option });
            }
        }

        #region Worker Threads
        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = (BackgroundWorker)sender;
            CodeMLAnalysisOption analysis = (CodeMLAnalysisOption)e.Argument;
            worker.ReportProgress(0, new ProgressUpdateEventArgs() { ProgressMessage = "Processing " + analysis.Description, Source = analysis });

            // Set up the working directory
            int directoryCopy = 0;
            string analysisTitle = (analysis.Tree.Title
                                        + " Model-" + analysis.Configuration.Model
                                        + " NSSites-" + analysis.Configuration.NSSites.Concatenate("_")
                                        + " K-" + analysis.Kappa.ToString().Replace(".", "_") + (analysis.Configuration.FixedKappa ? "-Fixed" : string.Empty)
                                        + " W-" + analysis.Omega.ToString().Replace(".", "_") + (analysis.Configuration.FixedOmega ? "-Fixed" : string.Empty)
                                        + " NCatG-" + analysis.Configuration.NCatG.ToString()
                                    ).ToSafeFileName();
            analysis.ProcessDirectory = analysisTitle;
            while (Directory.Exists(this.JobDirectory + "\\" + analysis.ProcessDirectory))
            {
                directoryCopy++;
                analysis.ProcessDirectory = analysisTitle + "_" + directoryCopy.ToString();
            }
            analysis.ProcessDirectory = Directory.CreateDirectory(this.JobDirectory + "\\" + analysis.ProcessDirectory).FullName;
            
            // Copy the tree and sequence files to the working directory
            File.Copy(analysis.TreeFilePath, analysis.ProcessDirectory + "\\tree.tre");
            File.Copy(analysis.SequencesFilePath, analysis.ProcessDirectory + "\\sequences.txt");

            // Create the control file
            string controlContents = Properties.Resources.codeml_template
                .Replace("{out_file}", "out.txt")
                .Replace("{seq_file}", "sequences.txt")
                .Replace("{tree_file}", "tree.tre")

                .Replace("{verbose}", analysis.Tree.Configuration.Verbose.ToString())
                .Replace("{runmode}", analysis.Tree.Configuration.RunMode.ToString())
                .Replace("{seqtype}", analysis.Tree.Configuration.SequenceType.ToString())
                .Replace("{codonfreq}", analysis.Tree.Configuration.CodonFrequency.ToString())
                .Replace("{clock}", analysis.Tree.Configuration.Clock.ToString())
                .Replace("{icode}", analysis.Tree.Configuration.ICode.ToString())
                .Replace("{mgene}", analysis.Tree.Configuration.MGene.ToString())
                .Replace("{method}", analysis.Tree.Configuration.Method.ToString())

                .Replace("{malpha}", (analysis.Tree.Configuration.MAlpha ? "1" : "0"))
                .Replace("{getse}", (analysis.Tree.Configuration.GetSE ? "1" : "0"))
                .Replace("{rateancestor}", (analysis.Tree.Configuration.RateAncestor ? "1" : "0"))
                .Replace("{cleandata}", (analysis.Tree.Configuration.CleanData ? "1" : "0"))

                .Replace("{model}", analysis.Configuration.Model.ToString())
                .Replace("{ns_sites}", analysis.Configuration.NSSites.Concatenate(" "))
                .Replace("{kappa_fixed}", (analysis.Configuration.FixedKappa ? "1" : "0"))
                .Replace("{kappa}", analysis.Kappa.ToString())
                .Replace("{omega_fixed}", (analysis.Configuration.FixedOmega ? "1" : "0"))
                .Replace("{omega}", analysis.Omega.ToString())
                .Replace("{ncatg}", analysis.Configuration.NCatG.ToString());
            File.WriteAllText(analysis.ProcessDirectory + "\\codeml.ctl", controlContents);

            // Set up the PAML process
            Process codeml = new Process() { EnableRaisingEvents = true };
            
            codeml.StartInfo = new ProcessStartInfo(CodeMLExecutablePath)
                {
                    CreateNoWindow = true,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    WorkingDirectory = analysis.ProcessDirectory
                };
            codeml.OutputDataReceived += new DataReceivedEventHandler(codeml_OutputDataReceived);
            codeml.ErrorDataReceived += new DataReceivedEventHandler(codeml_ErrorDataReceived);
            analysis.Process = codeml;

            // Start PAML, set priority, and then wait for it to finish
            analysis.ProcessStartTime = DateTime.Now;
            analysis.OutputData += "codeml.exe started at " + analysis.ProcessStartTime.ToStandardTimeString() + "\r\n";
            
            codeml.Start();

            //StreamWriter inputStream = codeml.StandardInput;
            //StreamReader outputStream = codeml.StandardOutput;
            codeml.BeginOutputReadLine();
            codeml.BeginErrorReadLine();

            try
            { codeml.PriorityClass = this.Priority; }
            catch { } // We'll most likely have an exception because the process has already closed, in which case we don't particularly care.
            codeml.WaitForExit();
            
            analysis.Duration = DateTime.Now.Subtract(analysis.ProcessStartTime);

            // When CancelAsync() is recognized by the CodeMLProcess object's thread in the while{} loop of RunAnalyses, Kill() will be called on all
            // of the Process objects associated with the analysis.  The background worker's thread will no longer be blocking and it will hit this
            // line, where we cycle out.
            // Also, this is the only practical point to do a cancel check.  Everything else in here will run too quickly to bother checking.
            // worker.CancellationPending = true will occur if this specific codeml process instance has been cancelled from the caller (e.g.: UI).
            if (this.CancellationPending || worker.CancellationPending) { e.Cancel = true; }

            // Parse the results file
            if (!e.Cancel)
            {
                int nsSite = (analysis.Configuration.NSSites.Count == 1 ? analysis.Configuration.NSSites[0] : -1);

                if (File.Exists(analysis.ProcessDirectory + "\\out.txt"))
                {
                    OutputFile output = new OutputFile(analysis.ProcessDirectory + "\\out.txt",
                                                        analysis.Configuration.Model,
                                                        nsSite);
                    analysis.Results = output.Parse();
                    analysis.Results.ForEach(r =>
                        {
                            r.Kappa = analysis.Kappa;
                            r.Omega = analysis.Omega;
                            r.AnalysisConfigurationID = analysis.Configuration.ID;
                            r.TreeID = analysis.Tree.ID;
                            if (r.Duration == TimeSpan.MinValue)
                            {
                                // See: comments in OutputFile.Parse() around where the duration gets parsed as to why this might not have been set.
                                // The duration value up here is not as accurate as PAML's, because PAML can run multiple NSsite values within the same 
                                // process, so we prefer to use its value.
                                r.Duration = analysis.Duration;
                            }
                        });
                    analysis.Exceptions.AddRange(output.Exceptions);
                    // Superfluous, since we're not getting at it this way in RunWorkerCompleted()
                    e.Result = analysis;
                }
                else
                {
                    // If there's no output file we can assume something went wrong; the explanation should be in the process output.
                    throw new Exception(string.IsNullOrWhiteSpace(analysis.OutputData) ? "An error occured when running codeml.exe." : analysis.OutputData);
                }
            }

            if (!this.Options.KeepFolders)
            {
                try
                {
                    Directory.Delete(analysis.ProcessDirectory, true); // Recursive = true to allow us to delete even though it's not empty.
                }
                catch { }
            }
        }

        private void codeml_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data == null) { return; }

            CodeMLAnalysisOption analysis = Analyses.FirstOrDefault(a => a.Process == (Process)sender);
            if (analysis != null)
            {
                analysis.OutputData += e.Data + "\r\n";
                OnProcessOutputUpdate(new ProgressUpdateEventArgs() { Source = analysis, ProgressMessage = e.Data });
            }
        }

        private void codeml_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data == null) { return; }

            CodeMLAnalysisOption analysis = Analyses.FirstOrDefault(a => a.Process == (Process)sender);
            if (analysis != null)
            {
                analysis.ErrorData += e.Data + "\r\n";
            }
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            OnProgressUpdate((ProgressUpdateEventArgs)e.UserState);
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            CodeMLAnalysisOption analysis = WorkerThreads[(BackgroundWorker)sender];
            Jobs.JobStatuses finalStatus = Jobs.JobStatuses.Undefined;

            if (e.Error != null)
            {
                analysis.Exceptions.Add(e.Error);
                analysis.ErrorData = e.Error.Message;

                // One option within the configuration failing is good enough to count the whole thing as failed, so that the user can readily see in
                // the results that there was a problem.
                analysis.Configuration.UpdateStatus(Jobs.JobStatuses.Failed);
                analysis.Tree.UpdateStatus(Jobs.JobStatuses.Failed);

                finalStatus = Jobs.JobStatuses.Failed;
            }
            else if (e.Cancelled)
            {
                finalStatus = Jobs.JobStatuses.Cancelled;
                // Not doing anything else here yet, since we're not running the Option objects into the database.  Trees and Configurations will get
                // set to Cancelled if needs be by the while{} loop after it's killed all of the pending processes.
            }
            else
            {
                this.Results.AddRange(analysis.Results);
                OnResultsParsed(new ResultsEventArgs() { Results = analysis.Results });

                // Check to see if this was the last analysis option in the configuration, in which case we can close out its status as completed and
                // possibly do the same for the tree.

                // It might seem odd that we do this down here, but in order to run a while{} loop that allocates analysis options to threads as they
                // become available we're collaping the Tree-Configuration-Analysis hierarchy into a simple List<CodeMLAnalysisOption>.  That makes
                // this the best point where we can check to see if a configuration or tree can be marked as Completed.

                if (analysis.Configuration.Analyses // Equivalent to: this.Parent.Children
                        .Where(a => a != analysis) // This worker's analysis hasn't been marked Completed, but it's about to be.
                        .All(a => a.Status == Jobs.JobStatuses.Completed))
                {
                    // Set the analysis to completed.  If any of the analysis options were marked as Failed, we won't have made it here because the
                    // All() check will have failed, so the configuration will hold its status as Failed.
                    analysis.Configuration.UpdateStatus(Jobs.JobStatuses.Completed);

                    // Now check to see if we can mark the whole tree as completed.
                    if (analysis.Tree.AnalysisConfigurations.All(cf => cf.Status == Jobs.JobStatuses.Completed))
                    {
                        analysis.Tree.UpdateStatus(Jobs.JobStatuses.Completed);
                    }
                }
                finalStatus = Jobs.JobStatuses.Completed;
            }

            // Log to the database whatever the executable was outputting and any exceptions.
            analysis.LogProcessOutput(finalStatus);
            
            // Once we set this status (changing from Running), the outer while{} loop will carry on to the next analysis, so this has to be the
            // very last thing that happens in RunWorkerCompleted.
            analysis.Status = finalStatus;
            OnProgressUpdate(new ProgressUpdateEventArgs() { Source = analysis });
        }

        private BackgroundWorker NewWorkerThread()
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            worker.ProgressChanged += new ProgressChangedEventHandler(worker_ProgressChanged);
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
            return worker;
        }
        #endregion

        #region Events
        protected virtual void OnProgressUpdate(ProgressUpdateEventArgs e) { if (ProgressUpdate != null) { ProgressUpdate(e); } }
        internal event ProgressUpdateEventHandler ProgressUpdate;

        protected virtual void OnProcessOutputUpdate(ProgressUpdateEventArgs e) { if (ProcessOutputUpdate != null) { ProcessOutputUpdate(e); } }
        public event ProgressUpdateEventHandler ProcessOutputUpdate;

        protected virtual void OnResultsParsed(ResultsEventArgs e) { if (ResultsParsed != null) { ResultsParsed(e); } }
        public delegate void ResultsEventHandler(ResultsEventArgs e);
        internal event ResultsEventHandler ResultsParsed;
        public class ResultsEventArgs : EventArgs
        {
            public List<Result> Results { get; set; }
            
            public ResultsEventArgs() { }
        }
        #endregion
    }
}