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
    public class CodeMLProcessOptions
    {
        public List<Tree> Trees { get; set; }
        public int ConcurrentProcesses { get; set; }
        public ProcessPriorityClass Priority { get; set; }
        public string CodeMLExecutablePath { get; set; }
        public string WorkingDirectory { get; set; }
        public bool KeepFolders { get; set; }
    }

    public class CodeMLAnalysisOption
    {
        public Guid ID { get; private set; }
        public int ProcessOutputID { get; private set; }
        public int Index { get; set; }
        public Jobs.JobStatuses Status { get; set; }
        public Process Process { get; set; }
        public List<Exception> Exceptions { get; set; }

        public Tree Tree { get; set; }
        public AnalysisConfiguration Configuration { get; set; }
        public double Kappa { get; set; }
        public double Omega { get; set; }
        public string TreeFilePath { get; set; }
        public string SequencesFilePath { get; set; }

        public string ProcessDirectory { get; set; }
        public DateTime ProcessStartTime { get; set; }
        internal TimeSpan Duration { get; set; }
        public string OutputData { get; set; }
        internal string ErrorData { get; set; }

        /// <remarks>
        /// The only reason this is plural is because of NSsites; codeml.exe will produce multiple results from a single input file for model 0.
        /// </remarks>
        internal List<Result> Results { get; set; }

        internal CodeMLAnalysisOption()
        {
            this.ID = Guid.NewGuid();
            this.Exceptions = new List<Exception>();
            this.ProcessStartTime = DateTime.MinValue;
        }

        internal string Description
        {
            get
            {
                return (new FileInfo(TreeFilePath)).Name
                    + " (Model: " + Configuration.Model.ToString()
                    + ", NSsites: " + Configuration.NSSites.Concatenate(", ")
                    + ", ncatG: " + Configuration.NCatG.ToString()
                    + ", kappa: " + Kappa.ToString() + (Configuration.FixedKappa ? " (fixed)" : string.Empty)
                    + ", omega: " + Omega.ToString() + (Configuration.FixedOmega ? " (fixed)" : string.Empty)
                    + ")";
            }
        }

        #region Database
        internal void LogProcessOutput(Jobs.JobStatuses FinalStatus)
        {
            using (ChangLab.LocalDatabase.DataAccess da = new ChangLab.LocalDatabase.DataAccess("PAML.ProcessOutput_Add"))
            {
                da.AddParameter("TreeID", this.Tree.ID);
                da.AddParameter("AnalysisConfigurationID", this.Configuration.ID);
                da.AddDoubleParameter("Kappa", 9, 3, this.Kappa);
                da.AddDoubleParameter("Omega", 9, 3, this.Omega);
                da.AddParameter("StatusID", Jobs.JobStatusCollection.IDByKey(FinalStatus));

                if (!string.IsNullOrWhiteSpace(this.ProcessDirectory))
                {
                    DirectoryInfo processDirectory = new DirectoryInfo(this.ProcessDirectory);
                    da.AddParameter("ProcessDirectory", (processDirectory.Exists ? processDirectory.Name : string.Empty), true);
                }
                else
                { da.AddParameter("ProcessDirectory", string.Empty, true); }

                da.AddParameter("OutputData", OutputData); // Output data will always have something in it.
                da.AddParameter("ErrorData", System.Data.SqlDbType.VarChar, -1, ErrorData, true); // Error data might not.
                da.AddOutputParameter("ID", this.ProcessOutputID);

                this.ProcessOutputID = (int)da.ExecuteParameter("ID");
            }

            this.Exceptions.ForEach(ex => {
                // The message is irrelevant because JobException's going to save the inner exception; the class is just serving as a wrapper to
                // capture the JobID.
                Jobs.JobException jex = new Jobs.JobException(this.Tree.JobID, 0, "PAML error", ex);
                jex.Save();

                using (ChangLab.LocalDatabase.DataAccess da = new LocalDatabase.DataAccess("PAML.ProcessException_Add"))
                {
                    da.AddParameter("ExceptionID", jex.ID);
                    da.AddParameter("ProcessOutputID", this.ProcessOutputID);

                    da.ExecuteCommand();
                }
            });
        }

        public static System.Data.DataTable ListOutput(string JobID, int TreeID = 0, int AnalysisConfigurationID = 0)
        {
            using (ChangLab.LocalDatabase.DataAccess da = new ChangLab.LocalDatabase.DataAccess("PAML.ProcessOutput_List"))
            {
                da.AddParameter("JobID", System.Data.SqlDbType.UniqueIdentifier, JobID);
                da.AddParameter("TreeID", TreeID, true);
                da.AddParameter("AnalysisConfigurationID", AnalysisConfigurationID, true);
                return da.ExecuteDataTable();
            }
        }

        public static System.Data.DataTable ListExceptions(int ProcessOutputID)
        {
            using (ChangLab.LocalDatabase.DataAccess da = new ChangLab.LocalDatabase.DataAccess("PAML.ProcessException_List"))
            {
                da.AddParameter("ProcessOutputID", ProcessOutputID);
                return da.ExecuteDataTable();
            }
        }
        #endregion
    }
}
