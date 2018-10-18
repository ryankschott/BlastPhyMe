using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using ChangLab.Common;
using ChangLab.Genes;

namespace ChangLab.Jobs
{
    public class Job
    {
        /// <summary>
        /// This property is only held in-memory, it is not saved to the database and should not be used to determine if a job record completed.
        /// The HasCompleted property is based on the job status, which is saved to the database and is a reliable measure of the job's completion.
        /// </summary>
        public RunStep LastCompletedStep { get; set; }
        public enum RunStep
        {
            None,
            Initialize,
            Run,
            Complete
        }
        public bool HasCompleted
        {
            get
            {
                switch (this.Status)
                {
                    case JobStatuses.Completed:
                    case JobStatuses.Cancelled:
                    case JobStatuses.Failed:
                        return true;
                    default:
                        return false;
                }
            }
        }

        public bool CancellationPending { get; set; }
        public bool SaveResultsAfterCancel { get; set; }
        public List<JobException> Exceptions { get; private set; }
        public string Output { get; set; }
        public string JobDirectory
        {
            get { return this.GetAdditionalProperty("JobDirectory"); }
            internal set { this.SetAdditionalProperty("JobDirectory", value); }
        }
        public void CreateJobDirectoryByDateTime(string JobDirectory)
        {
            // Set up the working directory for this job.  This has to be done before Initialize() is called to ensure that this directory path is
            // the one saved to the database.
            this.JobDirectory = (new System.IO.DirectoryInfo(JobDirectory)).CreateSafeDirectory("Job-" + DateTime.Now.ToString("yyyyMMdd-hhmmss")).FullName;
        }
        public void CreateJobDirectoryByName(string ParentDirectory, string Name)
        {
            // Set up the working directory for this job.  This has to be done before Initialize() is called to ensure that this directory path is
            // the one saved to the database.
            this.JobDirectory = (new System.IO.DirectoryInfo(ParentDirectory)).CreateSafeDirectory(Name).FullName;
        }

        public ChangLab.RecordSets.RecordSet SourceRecordSet { get; set; }
        public ChangLab.RecordSets.SubSet SourceSubSet { get; set; }
        public string SourceName { get { return this.SourceRecordSet.Name + " - " + this.SourceSubSet.Name; } }
        
        #region Progress Tracking
        public List<ProgressMessage> ProgressMessages { get; set; }
        public int CurrentProgressMaximum { get; private set; }
        public int CurrentProgressValue { get; private set; }
        public string LastStatusMessage { get; private set; }
        #endregion

        #region Database Properties
        public string ID { get; private set; }
        public string RecordSetID { get; set; }
        public string SubSetID { get; set; }
        public string Title { get; set; }
        public DateTime StartTime { get; internal set; }
        public DateTime EndTime { get; private set; }

        private int StatusID { get; set; }
        public JobStatuses Status
        {
            get { return JobStatus.KeyByID(StatusID); }
            private set { StatusID = JobStatus.IDByKey(value); }
        }
        public string StatusName
        {
            get { return JobStatus.NameByID(StatusID); }
        }

        private int TargetID { get; set; }
        public JobTargets Target
        {
            get { return JobTargetCollection.KeyByID(TargetID); }
            private set { TargetID = JobTargetCollection.IDByKey(value); }
        }

        public string StartTimeStamp
        {
            get
            {
                if (DateTime.Now.Subtract(StartTime).TotalDays > 0)
                {
                    return StartTime.ToString("yyyyMMdd_hhmmss");
                }
                else
                {
                    return StartTime.ToString("hhmmss");
                }
            }
        }

        public string StartTimeString
        {
            get
            {
                return StartTime.ToStandardTimeStringWithDateIfNotToday();
            }
        }

        private Dictionary<string, string> _additionalProperties;
        internal Dictionary<string, string> AdditionalProperties
        {
            get { if (_additionalProperties == null) { _additionalProperties = new Dictionary<string, string>(); } return _additionalProperties; }
            private set { _additionalProperties = value; }
        }
        internal XDocument AdditionalPropertiesXml
        {
            get
            {
                if (_additionalProperties == null)
                {
                    return null;
                }
                else
                {
                    XDocument xdoc = new XDocument(new XElement("Additional"));
                    xdoc.Document.Root.Add(_additionalProperties.Select(kv => new XElement("Property", new object[] { new XElement("Name") { Value = kv.Key }, new XElement("Value") { Value = kv.Value } })).ToArray());
                    return xdoc;
                }
            }
        }

        public void SetAdditionalProperty(string Name, string Value)
        {
            if (AdditionalProperties.ContainsKey(Name)) { AdditionalProperties[Name] = Value; }
            else { AdditionalProperties.Add(Name, Value); }
        }
        public string GetAdditionalProperty(string Name, string Default = "")
        {
            if (AdditionalProperties.ContainsKey(Name)) { return AdditionalProperties[Name]; }
            else { return Default; }
        }

        public bool HasDatabaseExceptions { get; private set; }
        #endregion

        protected internal Job()
        {
            this.Exceptions = new List<JobException>();
            this.LastCompletedStep = RunStep.None;
            this.ProgressMessages = new List<ProgressMessage>();
        }

        public Job(JobTargets Target, string SubSetID) : this()
        {
            this.Target = Target;
            this.Status = JobStatuses.New;
            this.SubSetID = SubSetID;
        }

        public Job(string ID) : this()
        {
            if (!string.IsNullOrWhiteSpace(ID))
            {
                using (LocalDatabase.DataAccess da = new LocalDatabase.DataAccess("Job.Job_List"))
                {
                    da.AddParameter("JobID", SqlDbType.UniqueIdentifier, ID);

                    using (DataTable results = da.ExecuteDataTable())
                    {
                        this.FromDataRow(results.Rows[0]);
                    }
                }
            }
        }

        public virtual void Initialize()
        {
            this.StartTime = DateTime.Now;
            this.CancellationPending = false;
            this.SaveResultsAfterCancel = false;

            OnProgressUpdate(new ProgressUpdateEventArgs()
            {
                ProgressMessage = "Starting job at " + this.StartTime.ToStandardTimeString() + "\r\n",
                Setup = true,
                CurrentMax = 100,
                CurrentProgress = 0
            });

            CreateDatabaseRecord();

            this.LastCompletedStep = RunStep.Initialize;
        }

        /// <summary>
        /// Sets the final status of the job and performs any related cleanup. Should be called as the last line within the try...catch block of a
        /// job's publically exposed "run" function.
        /// </summary>
        internal void Complete()
        {
            if (this.CancellationPending)
            {
                Complete(JobStatuses.Cancelled);
            }
            else if (this.Exceptions.Count == 0)
            {
                Complete(JobStatuses.Completed);
            }
            else
            {
                Complete(JobStatuses.Failed);
            }
        }

        public void Complete(JobStatuses Status)
        {
            this.EndTime = DateTime.Now;
            this.LastCompletedStep = RunStep.Complete;

            OnProgressUpdate(new ProgressUpdateEventArgs()
            {
                ProgressMessage = "Job "
                                + (Status == JobStatuses.Cancelled ? "cancelled" : (Status == JobStatuses.Failed ? "ended in error" : "ended"))
                                + " at " + this.EndTime.ToStandardTimeStringWithDateIfNotToday()
            });

            UpdateStatus(Status);
        }

        internal void UnhandledJobException(Exception ex)
        {
            Complete(JobStatuses.Failed);

            JobException jex = new JobException(this.ID, 0, "Job failed: " + ex.Message, ex);
            jex.Save(); this.Exceptions.Add(jex);
        }

        public virtual void CancelAsync()
        {
            CancellationPending = true;
            OnProgressUpdate(new ProgressUpdateEventArgs() { ProgressMessage = "Job is being cancelled..." });
        }

        #region Database Methods
        protected internal void CreateDatabaseRecord()
        {
            using (LocalDatabase.DataAccess da = new LocalDatabase.DataAccess("Job.Job_Edit"))
            {
                da.AddParameter("ID", SqlDbType.UniqueIdentifier, 0, DBNull.Value, ParameterDirection.InputOutput);
                da.AddParameter("RecordSetID", SqlDbType.UniqueIdentifier, this.RecordSetID, true);
                da.AddParameter("SubSetID", SqlDbType.UniqueIdentifier, this.SubSetID, true);
                da.AddParameter("TargetID", this.TargetID);
                da.AddParameter("Title", SqlDbType.VarChar, 250, this.Title);
                da.AddParameter("StatusID", this.StatusID);
                da.AddParameter("StartedAt", SqlDbType.DateTime2, this.StartTime, true);
                da.AddParameter("EndedAt", SqlDbType.DateTime2, this.EndTime, true);
                da.AddParameter("AdditionalProperties", SqlDbType.Xml, this.AdditionalPropertiesXml, true);

                this.ID = da.ExecuteParameter("ID").ToString();
            }
        }

        public void UpdateStatus(JobStatuses Status)
        {
            this.Status = Status;

            using (LocalDatabase.DataAccess da = new LocalDatabase.DataAccess("Job.Job_Edit"))
            {
                da.AddParameter("ID", SqlDbType.UniqueIdentifier, this.ID);
                da.AddParameter("StatusID", this.StatusID);
                da.AddParameter("EndedAt", SqlDbType.DateTime2, this.EndTime, true);

                da.ExecuteCommand();
            }

            this.OnStatusUpdate(new StatusUpdateEventArgs(Status));
        }

        public void UpdateAdditionalProperties()
        {
            using (LocalDatabase.DataAccess da = new LocalDatabase.DataAccess("Job.Job_Edit"))
            {
                da.AddParameter("ID", SqlDbType.UniqueIdentifier, this.ID);
                da.AddParameter("AdditionalProperties", SqlDbType.Xml, this.AdditionalPropertiesXml, true);
                // All the other parameters will be ignored either because they're explicitly not addressed in an UPDATE operation or because having
                // a NULL value causes them to be ignored.

                da.ExecuteCommand();
            }
        }

        public void LogOutput()
        {
            using (LocalDatabase.DataAccess da = new LocalDatabase.DataAccess("Job.OutputText_Edit"))
            {
                da.AddParameter("JobID", SqlDbType.UniqueIdentifier, this.ID);
                da.AddParameter("OutputText", this.Output);

                da.ExecuteCommand();
            }

            this.OnStatusUpdate(new StatusUpdateEventArgs(Status));
        }

        public void Archive()
        {
            using (LocalDatabase.DataAccess da = new LocalDatabase.DataAccess("Job.Job_Edit"))
            {
                da.AddParameter("ID", SqlDbType.UniqueIdentifier, this.ID);
                da.AddParameter("Active", SqlDbType.Bit, false);

                da.ExecuteCommand();
            }
        }

        public static DataTable ListAsDataTable(string RecordSetID, JobTargets Target)
        {
            using (LocalDatabase.DataAccess da = new LocalDatabase.DataAccess())
            {
                switch (Target)
                {
                    case JobTargets.CodeML:
                        da.ChangeCommand("PAML.Job_List");
                        break;
                    case JobTargets.PRANK:
                        da.ChangeCommand("PRANK.Job_List");
                        break;
                    default:
                        da.ChangeCommand("Job.Job_List");
                        da.AddParameter("TargetID", JobTargetCollection.IDByKey(Target));
                        break;
                }

                da.AddParameter("RecordSetID", SqlDbType.UniqueIdentifier, RecordSetID);

                return da.ExecuteDataTable();
            }
        }

        //public static Job FromDataRow<T>(DataRow row) where T : Job
        //{
        //    Job job = null;

        //    if (typeof(T) == typeof(BlastNAtNCBI))
        //    {
        //        job = new BlastNAtNCBI() { ID = row["ID"].ToString() };
        //    }
        //    else if (typeof(T) == typeof(RunTreesAtCodeML))
        //    {
        //        job = new RunTreesAtCodeML(new PAML.CodeML.CodeMLProcessOptions()) { ID = row["ID"].ToString() };
        //    }
        //    else if (typeof(T) == typeof(AlignSequencesWithPRANK))
        //    {
        //        job = new AlignSequencesWithPRANK(new PRANKOptions(), false, string.Empty, false, string.Empty) { ID = row["ID"].ToString() };
        //    }
        //    else if (typeof(T) == typeof(GenerateTreeWithPhyML))
        //    {
        //        job = new GenerateTreeWithPhyML(new PhyMLOptions(), string.Empty, string.Empty, false, string.Empty) { ID = row["ID"].ToString() };
        //    }
        //    else
        //    {
        //        job = new Job() { ID = row["ID"].ToString() };
        //    }

        //    job.SubSetID = row["SubSetID"].ToSafeString();
        //    job.TargetID = (int)row["TargetID"];
        //    job.StartTime = (DateTime)row["StartedAt"];
        //    job.EndTime = row.ToSafeDateTime("EndedAt");
        //    job.StatusID = (int)row["StatusID"];
        //    if (row["AdditionalProperties"] != DBNull.Value)
        //    {
        //        job.AdditionalProperties = row.ToAdditionalPropertiesList("AdditionalProperties");

        //        if (typeof(T).IsSubclassOf(typeof(CommandLineGeneProcessingJob)))
        //        {
        //            ((CommandLineGeneProcessingJob)job).Options.FromString(job.AdditionalPropertiesXml.ToString());
        //        }
        //    }
        //    job.HasDatabaseExceptions = row.ToSafeBoolean("HasDatabaseExceptions", false);
        //    if (row.Table.Columns.Contains("OutputText") && !string.IsNullOrWhiteSpace(row.ToSafeString("OutputText")))
        //    { job.Output = row.ToSafeString("OutputText"); }

        //    return job;
        //}

        public void FromDataRow(DataRow row)
        {
            this.ID = ID = row["ID"].ToString();
            this.SubSetID = row["SubSetID"].ToSafeString();
            this.TargetID = (int)row["TargetID"];
            this.StartTime = (DateTime)row["StartedAt"];
            this.EndTime = row.ToSafeDateTime("EndedAt");
            this.StatusID = (int)row["StatusID"];
            this.Title = row.ToSafeString("Title");
            
            if (row["AdditionalProperties"] != DBNull.Value)
            { this.AdditionalProperties = row.ToAdditionalPropertiesList("AdditionalProperties"); }
            
            this.HasDatabaseExceptions = row.ToSafeBoolean("HasDatabaseExceptions", false);
            
            if (row.Table.Columns.Contains("OutputText") && !string.IsNullOrWhiteSpace(row.ToSafeString("OutputText")))
            { this.Output = row.ToSafeString("OutputText"); }
        }

        public void RefreshExceptionsFromDatabase()
        {
            using (LocalDatabase.DataAccess da = new LocalDatabase.DataAccess("Job.Exception_List"))
            {
                da.AddParameter("JobID", SqlDbType.UniqueIdentifier, this.ID);

                this.Exceptions.Clear();
                using (DataTable results = da.ExecuteDataTable())
                {
                    foreach (DataRow row in results.Rows.Cast<DataRow>().OrderBy(row => row.ToSafeInt("ParentID")))
                    {
                        JobException jex = new JobException(this.ID, row.ToSafeInt("RequestID"), (string)row["Message"])
                        {
                            Source = row.ToSafeString("Source"),
                            ParentID = row.ToSafeInt("ParentID"),
                            ExceptionAt = (DateTime)row["ExceptionAt"]
                        };
                        jex.SetStackTrace(row.ToSafeString("StackTrace"));
                        this.Exceptions.Add(jex);
                    }
                }
            }
        }

        public static List<string> ImportDataFile_ListProgressMessages(string JobID)
        {
            List<string> progressMessages = new List<string>();

            using (LocalDatabase.DataAccess da = new LocalDatabase.DataAccess("Job.Import_DataFile_Progress_List"))
            {
                da.AddParameter("JobID", SqlDbType.UniqueIdentifier, JobID);

                using (System.Data.SqlClient.SqlDataReader reader = da.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        progressMessages.Add((string)reader["Message"]);
                    }
                }
            }

            return progressMessages;
        }
        #endregion

        #region Events
        internal void innerProcess_ProgressUpdate(ProgressUpdateEventArgs e)
        {
            this.OnProgressUpdate(e);
        }

        protected virtual void OnProgressUpdate(ProgressUpdateEventArgs e)
        {
            // These variables allow a progress form to be closed and re-opened for a job, with the progress bar picking up where it last left off,
            // similarly to how we're now storing ProgressMessage values in ProgressMessages.
            if (e.Setup) { this.CurrentProgressMaximum = e.CurrentMax; }
            if (e.CurrentChanged) { this.CurrentProgressValue = e.CurrentProgress; }
            this.LastStatusMessage = e.StatusMessage;

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
                StatusUpdate(this, e);
            }
        }
        public event StatusUpdateEventHandler StatusUpdate;
        #endregion
    }

    public class JobException : Exception
    {
        public int ID { get; private set; }
        public string JobID { get; set; }
        public int RequestID { get; set; }
        public int ParentID { get; set; }
        public DateTime ExceptionAt { get; set; }

        private string _stackTrace = string.Empty;
        public override string StackTrace
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_stackTrace))
                { return _stackTrace; }
                else
                { return base.StackTrace; }
            }
        }
        /// <remarks>There's no set accessor to override for the StackTrace property so we do this instead.</remarks>
        internal void SetStackTrace(string StackTrace) { this._stackTrace = StackTrace; }

        public JobException() : this(string.Empty, 0, string.Empty, null) { }
        public JobException(string JobID, int RequestID) : this(JobID, RequestID, string.Empty, null) { }
        public JobException(string JobID, int RequestID, string Message) : this(JobID, RequestID, Message, null) { }

        public JobException(string JobID, int RequestID, string Message, Exception InnerException)
            : base(Message, InnerException)
        {
            this.ID = 0;
            this.JobID = JobID;
            this.RequestID = RequestID;
            this.ExceptionAt = DateTime.Now;
        }

        public void Save(bool AutoSaveInnerExceptions = true)
        {
            // If the root exception has an inner exception, we assume the root is just a wrapper to give us the Job and Request ID and that the real
            // root exception is the inner.  This allows for use of specific types of exceptions within the Job instead of genericising everything to
            // the JobException class in the working code.
            Exception ex = (this.InnerException != null ? this.InnerException : this);
            this.ID = Save(this.ID, this.JobID, this.RequestID, 0, ex);

            if (AutoSaveInnerExceptions)
            {
                int exceptionId = this.ID;
                // Automatically save the inner exceptions
                while (ex.InnerException != null)
                {
                    exceptionId = Save(0, this.JobID, this.RequestID, exceptionId, ex.InnerException);
                    ex = ex.InnerException;
                }
            }
        }

        private int Save(int ID, string JobID, int RequestID, int ParentID, Exception ex)
        {
            using (LocalDatabase.DataAccess da = new LocalDatabase.DataAccess("Job.Exception_Add"))
            {
                da.AddParameter("ID", System.Data.SqlDbType.Int, 0, ID, System.Data.ParameterDirection.InputOutput, true);
                da.AddParameter("JobID", System.Data.SqlDbType.UniqueIdentifier, JobID);
                da.AddParameter("RequestID", RequestID, true);
                da.AddParameter("Message", ex.Message);
                da.AddParameter("Source", SqlDbType.VarChar, ex.Source, true);
                da.AddParameter("StackTrace", SqlDbType.VarChar, ex.StackTrace, true);
                da.AddParameter("ExceptionType", SqlDbType.VarChar, 250, ex.GetType().Name);
                da.AddParameter("ParentID", System.Data.SqlDbType.Int, ParentID, true);

                return (int)da.ExecuteParameter("ID");
            }
        }

        public static List<JobException> List(string JobID, int RequestID = 0)
        {
            List<JobException> exceptions = new List<JobException>();

            using (LocalDatabase.DataAccess da = new LocalDatabase.DataAccess("Job.Exception_List"))
            {
                da.AddParameter("JobID", System.Data.SqlDbType.UniqueIdentifier, JobID);
                da.AddParameter("RequestID", RequestID, true);

                using (System.Data.SqlClient.SqlDataReader reader = da.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        exceptions.Add(new JobException(JobID, RequestID, (string)reader["Message"])
                            {
                                ID = (int)reader["ID"],
                                Source = reader.ToSafeString("Source"),
                                _stackTrace = reader.ToSafeString("StackTrace"),
                                ParentID = reader.ToSafeInt("ParentID"),
                                ExceptionAt = (DateTime)reader["ExceptionAt"]
                            });
                    }
                }
            }

            return exceptions;
        }
    }

    public class ProgressMessage
    {
        public TimeSpan Elapsed { get; set; }
        public string Message { get; set; }
    }
}
