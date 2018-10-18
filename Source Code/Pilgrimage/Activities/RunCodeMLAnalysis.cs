using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChangLab.Common;
using ChangLab.Jobs;
using ChangLab.PAML.CodeML;

namespace Pilgrimage.Activities
{
    public class RunCodeMLAnalysis : Activity
    {
        internal RunTreesAtCodeML CodeMLJob { get; set; }
        internal List<Tree> Trees { get; set; }

        public RunCodeMLAnalysis(IWin32Window CallingForm) : base(CallingForm) { }

        public void RunAnalyses(List<Tree> Trees, int ConcurrentProcesses, System.Diagnostics.ProcessPriorityClass Priority, string CodeMLExecutablePath, string WorkingDirectory, bool KeepFolders, string JobTitle,
            ChangLab.RecordSets.RecordSet SourceRecordSet, ChangLab.RecordSets.SubSet SourceSubSet)
        {
            // Initialize the job
            CodeMLJob = new RunTreesAtCodeML(
                    new CodeMLProcessOptions()
                    {
                        ConcurrentProcesses = ConcurrentProcesses,
                        Priority = Priority,
                        CodeMLExecutablePath = CodeMLExecutablePath,
                        KeepFolders = KeepFolders
                    })
                {
                    Title = JobTitle,
                    RecordSetID = SourceRecordSet.ID,
                    SourceRecordSet = SourceRecordSet,
                    SourceSubSet = SourceSubSet
                };
            // Set up the job's working folder
            CodeMLJob.CreateJobDirectoryByName(WorkingDirectory, CodeMLJob.SourceRecordSet.Name + " - " + JobTitle + " - PAML");
            CodeMLJob.Options.WorkingDirectory = CodeMLJob.JobDirectory;
            CodeMLJob.Initialize();
            CodeMLJob.ProgressUpdate += new ChangLab.Common.ProgressUpdateEventHandler(Job_ProgressUpdate);
            CodeMLJob.StatusUpdate += new StatusUpdateEventHandler(Job_StatusUpdate);
            this.CurrentJob = CodeMLJob;

            // Commit the trees and their configurations to the database.
            // At some point we'll have an overload of this method that just takes a JobID, extracts the Trees and Configurations for it, and then
            // passes that in to RunWorkerAsync() so that Jobs.RunTreeAtCodeML can do the delta on what's completed vs. what still needs to be run.
            Trees.ForEach(t =>
                {
                    t.JobID = CodeMLJob.ID;
                    t.Save();

                    t.AnalysisConfigurations.ForEach(cf =>
                        {
                            cf.TreeID = t.ID;
                            cf.Save();
                        });
                });
            this.Trees = Trees;
            this.CodeMLJob.Options.Trees = this.Trees;

            Worker.RunWorkerAsync();
        }

        protected internal override void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            CodeMLJob.RunAnalyses();

            e.Result = CodeMLJob;
            if (this.CurrentJob.CancellationPending || Worker.CancellationPending) { e.Cancel = true; }
        }

        protected internal override void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Completed = true;
            // That's all we need to do; frmJobProgress is also attached to this event and will do more processing.
        }
    }
}
