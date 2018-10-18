using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChangLab.Common;
using ChangLab.PAML.CodeML;

namespace ChangLab.Jobs
{
    public class RunTreesAtCodeML : Job
    {
        //public List<Result> Results { get { return  }
        public CodeMLProcessOptions Options { get; private set; }
        public CodeMLProcess Process { get; private set; }

        public RunTreesAtCodeML(CodeMLProcessOptions Options) : base(JobTargets.CodeML, string.Empty)
        {
            this.Options = Options;
            this.SetAdditionalProperty("KeepFolders", Options.KeepFolders.ToSafeString());
        }

        public RunTreesAtCodeML(string JobID) : base(JobID)
        {
            this.Options = new CodeMLProcessOptions() { KeepFolders = bool.Parse(this.GetAdditionalProperty("KeepFolders", false.ToString())) };
        }

        public void RunAnalyses()
        {
            try
            {
                Process = new CodeMLProcess(this.Options, this);
                Process.ProgressUpdate += new ProgressUpdateEventHandler(innerProcess_ProgressUpdate);
                Process.ResultsParsed += new CodeMLProcess.ResultsEventHandler(Process_ResultsParsed);

                this.UpdateStatus(JobStatuses.Running);
                Process.RunAnalyses();

                Complete();
            }
            catch (Exception ex)
            {
                UnhandledJobException(ex);
            }
        }

        private void Process_ResultsParsed(CodeMLProcess.ResultsEventArgs e)
        {
            // Commit results to the database.
            e.Results.ForEach(r => r.Save());
        }

        public override void CancelAsync()
        {
            CancellationPending = true;
            Process.CancelAsync();
        }

        #region Database
        public static System.Data.DataSet ListTopResults(string RecordSetID, string JobID = "")
        {
            using (LocalDatabase.DataAccess da = new LocalDatabase.DataAccess("PAML.Job_ListTopResults"))
            {
                da.AddParameter("RecordSetID", System.Data.SqlDbType.UniqueIdentifier, RecordSetID);
                da.AddParameter("JobID", System.Data.SqlDbType.UniqueIdentifier, JobID, true);
                da.AddListParameter("ResultIDs", new List<int>());
                return da.ExecuteDataSet();
            }
        }

        public static System.Data.DataSet ListTopResults(string RecordSetID, List<int> ResultIDs)
        {
            using (LocalDatabase.DataAccess da = new LocalDatabase.DataAccess("PAML.Job_ListTopResults"))
            {
                da.AddParameter("RecordSetID", System.Data.SqlDbType.UniqueIdentifier, RecordSetID);
                da.AddListParameter("ResultIDs", ResultIDs);
                return da.ExecuteDataSet();
            }
        }
        #endregion
    }
}
