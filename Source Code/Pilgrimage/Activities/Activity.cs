using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChangLab.Common;

namespace Pilgrimage.Activities
{
    public class Activity
    {
        protected internal IWin32Window OwnerWindow { get; set; }
        protected internal ProgressForm ProgressForm { get; set; }
        protected internal BackgroundWorker Worker { get; set; }

        public ChangLab.Jobs.Job CurrentJob { get; set; }
        public object Result { get; private set; }
        public Exception Error { get; private set; }
        public bool Cancelled { get; private set; }
        public bool Completed { get; internal set; }

        public Activity(IWin32Window OwnerWindow)
        {
            this.OwnerWindow = OwnerWindow;
            this.Cancelled = false;
            this.Completed = false;

            Worker = new BackgroundWorker()
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };

            Worker.DoWork += new DoWorkEventHandler(Worker_DoWork);
            Worker.ProgressChanged += new ProgressChangedEventHandler(Worker_ProgressChanged);
            Worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Worker_RunWorkerCompleted);
        }

        protected internal string CloseProgressForm(DialogResult Result)
        {
            if (ProgressForm != null)
            {
                string output = ProgressForm.Output;
                ProgressForm.DialogResult = Result;
                ProgressForm.Close();
                return output;
            }
            else { return string.Empty; }
        }

        protected internal virtual void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            throw new NotImplementedException();
        }

        public virtual void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // In order to deal with popping out of the Worker's thread, all events that could be triggerd within the Worker thread (most likely by
            // the underlying Job instance) pipe out through here, either via Worker.ReportProgress() or calling this method directly.
            // The Job_EventName() event handler method is responsible for determining whether the Worker is still running and whether to pipe
            // through Worker.ReportProgress(), and thus pop out into the Activity instance's owner thread, or if it can just go straight to that
            // thread via calling this method directly.
            // With all events on the worker thread routing through here, we determine what event to re-trigger based on the args passed to here.

            if (e.UserState.GetType().Equals(typeof(ProgressUpdateEventArgs)))
            {
                ProgressUpdateEventArgs args = (ProgressUpdateEventArgs)e.UserState;

                if (this.CurrentJob != null) // Not all activities wrap around a job (e.g.: PopulateFromGenBank)
                {
                    // Store all progress message events against the Job with an in-memory list of a class that holds the elapsed time (as TimeSpan) and
                    // the message.  This can be used after the fact to reconstruct an output log, and it could also be saved to the database.
                    if (!string.IsNullOrWhiteSpace(args.ProgressMessage))
                    {
                        this.CurrentJob.ProgressMessages.Add(new ChangLab.Jobs.ProgressMessage() { Elapsed = DateTime.Now.Subtract(this.CurrentJob.StartTime), Message = args.ProgressMessage });
                    }
                }

                if (this.ProgressForm != null) { ProgressForm.UpdateProgress(args); }
                else { OnProgressUpdate(args); }
            }
            else if (e.UserState.GetType().Equals(typeof(StatusUpdateEventArgs)))
            {
                OnStatusUpdate((StatusUpdateEventArgs)e.UserState);
            }
        }

        protected internal virtual void ProgressForm_Cancelled(RunWorkerCompletedEventArgs e)
        {
            if (this.CurrentJob != null) { this.CurrentJob.CancelAsync(); }
            Worker.CancelAsync();
        }

        protected internal virtual void Job_ProgressUpdate(ProgressUpdateEventArgs e)
        {
            if ((this.Completed || (this.CurrentJob != null && this.CurrentJob.LastCompletedStep == ChangLab.Jobs.Job.RunStep.Complete)) && !(Worker != null && Worker.IsBusy))
            { Worker_ProgressChanged(null, new ProgressChangedEventArgs(0, e)); }
            else
            {
                try
                { Worker.ReportProgress(0, e); }
                catch
                {
                    try
                    {
                        // Progress forms should handle updates via Invoke, just in case we skip into here and end up in a cross-thread situation.
                        Worker_ProgressChanged(null, new ProgressChangedEventArgs(0, e));
                    }
                    catch { }
                }
            }
        }

        /// <remarks>
        /// Worker_ProgressChanged() will take care of triggering the Activity.OnStatusUpdate event.
        /// </remarks>
        protected internal virtual void Job_StatusUpdate(ChangLab.Jobs.Job sender, StatusUpdateEventArgs e)
        {
            if (Worker == null || !Worker.IsBusy)
            { Worker_ProgressChanged(null, new ProgressChangedEventArgs(0, e)); }
            else
            {
                try
                { Worker.ReportProgress(0, e); }
                catch
                {
                    try
                    {
                        // Progress forms should handle updates via Invoke, just in case we skip into here and end up in a cross-thread situation.
                        Worker_ProgressChanged(null, new ProgressChangedEventArgs(0, e));
                    }
                    catch { }
                }
            }
        }

        protected internal virtual void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Completed = true;
            string output = CloseProgressForm(DialogResult.OK);
            if (this.CurrentJob != null && !string.IsNullOrWhiteSpace(output)) { this.CurrentJob.Output = output; }

            if (e.Error != null)
            {
                this.Result = null;
                this.Error = e.Error;
                this.Cancelled = false;
            }
            else if (e.Cancelled)
            {
                this.Result = null; // e.Result cannot be accessed when e.Cancelled = true
                this.Error = null;
                this.Cancelled = true;
            }
            else
            {
                this.Result = e.Result;
                this.Error = null;
                this.Cancelled = false;
            }

            OnActivityCompleted(new ActivityCompletedEventArgs(this, this.Result, this.Error, this.Cancelled) { Sender = this });
        }

        protected virtual void OnActivityCompleted(ActivityCompletedEventArgs e)
        {
            if (ActivityCompleted != null)
            {
                ActivityCompleted(e);
            }
        }
        public delegate void ActivityCompletedEventHandler(ActivityCompletedEventArgs e);
        public event ActivityCompletedEventHandler ActivityCompleted;

        protected virtual void OnProgressUpdate(ProgressUpdateEventArgs e)
        {
            if (ProgressUpdate != null)
            {
                ProgressUpdate(e);
            }
        }
        public event ProgressUpdateEventHandler ProgressUpdate;

        protected virtual void OnStatusUpdate(StatusUpdateEventArgs e)
        {
            if (StatusUpdate != null)
            {
                StatusUpdate(this.CurrentJob, e);
            }
        }
        public event StatusUpdateEventHandler StatusUpdate;
    }

    public class ActivityCompletedEventArgs : RunWorkerCompletedEventArgs
    {
        public Activity Sender { get; set; }
        
        /// <summary>
        /// A replacement for the Result property that can be retrieved even if Cancelled == true.  Will be set with Result via the Constructor only.
        /// </summary>
        public object ActivityResult { get; set; }
        public string SelectedRecordSetID { get; set; }
        public string SelectedSubSetID { get; set; }
        
        public ActivityCompletedEventArgs() : this(null, null, null, false) { }
        public ActivityCompletedEventArgs(Activity sender, object result, Exception error, bool cancelled) : base(result, error, cancelled)
        {
            this.Sender = sender;
            this.ActivityResult = result;
        }
    }

    internal class InProgress
    {
        internal List<RunAlignment> PRANKAlignments
        {
            get { return this.Activities.Where(a => a.GetType() == typeof(RunAlignment) && ((RunAlignment)a).Target == ChangLab.Jobs.JobTargets.PRANK).Cast<RunAlignment>().ToList(); }
        }
        internal List<RunAlignment> MUSCLEAlignments
        {
            get { return this.Activities.Where(a => a.GetType() == typeof(RunAlignment) && ((RunAlignment)a).Target == ChangLab.Jobs.JobTargets.MUSCLE).Cast<RunAlignment>().ToList(); }
        }
        internal List<RunPhyML> PhyMLs
        {
            get { return this.Activities.Where(a => a.GetType() == typeof(RunPhyML)).Cast<RunPhyML>().ToList(); }
        }
        internal List<RunCodeMLAnalysis> CodeMLAnalyses
        {
            get { return this.Activities.Where(a => a.GetType() == typeof(RunCodeMLAnalysis)).Cast<RunCodeMLAnalysis>().ToList(); }
        }
        internal List<BlastSequencesAtNCBI> BlastNAtNCBIs
        {
            get { return this.Activities.Where(a => a.GetType() == typeof(BlastSequencesAtNCBI)).Cast<BlastSequencesAtNCBI>().ToList(); }
        }
        
        private List<Activity> Activities { get; set; }

        internal InProgress()
        {
            this.Activities = new List<Activity>();
        }

        internal void AddActivity(Activity Activity)
        {
            this.Activities.Add(Activity);
            OnActivityAdded(new ActivityEventArgs(Activity));
        }

        internal void RemoveActivity(Activity Activity)
        {
            this.Activities.Remove(Activity);
            OnActivityRemoved(new ActivityEventArgs(Activity));
        }

        internal Activity GetActivityByJobID(string JobID)
        {
            return this.Activities.FirstOrDefault(al => GuidCompare.Equals(al.CurrentJob.ID, JobID));
        }

        internal T GetActivityByJobID<T>(string JobID) where T : Activity
        {
            return this.Activities.FirstOrDefault(al => GuidCompare.Equals(al.CurrentJob.ID, JobID)) as T;
        }

        internal List<T> ListActivities<T>() where T : Activity
        {
            return this.Activities.Where(a => a.GetType() == typeof(T)).Cast<T>().ToList();
        }

        protected virtual void OnActivityAdded(ActivityEventArgs e)
        {
            if (ActivityAdded != null)
            {
                ActivityAdded(e);
            }
        }
        internal delegate void ActivityAddedEventHandler(ActivityEventArgs e);
        internal event ActivityAddedEventHandler ActivityAdded;

        protected virtual void OnActivityRemoved(ActivityEventArgs e)
        {
            if (ActivityRemoved != null)
            {
                ActivityRemoved(e);
            }
        }
        internal delegate void ActivityRemovedEventHandler(ActivityEventArgs e);
        internal event ActivityRemovedEventHandler ActivityRemoved;

        internal class ActivityEventArgs : EventArgs
        {
            internal Activity Activity { get; private set; }

            internal ActivityEventArgs(Activity Activity)
            {
                this.Activity = Activity;
            }
        }
    }
}