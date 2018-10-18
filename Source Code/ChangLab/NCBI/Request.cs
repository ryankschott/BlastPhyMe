using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bio.Web;
using ChangLab.Common;
using ChangLab.LocalDatabase;

namespace ChangLab.NCBI
{
    public class Request
    {
        #region Properties
        public int ID { get; set; }
        public string RequestID { get; set; }
        public string JobID { get; set; }
        public string TargetDatabase { get; set; }
        public string Algorithm { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public RequestStatus LastStatus { get; set; }
        public string StatusInformation { get; set; }
        #endregion

        public void Save()
        {
            using (DataAccess da = new DataAccess("NCBI.Request_Edit"))
            {
                da.AddParameter("ID", System.Data.SqlDbType.Int, 0, this.ID, System.Data.ParameterDirection.InputOutput);
                da.AddParameter("JobID", System.Data.SqlDbType.UniqueIdentifier, 0, this.JobID);
                da.AddParameter("TargetDatabase", System.Data.SqlDbType.VarChar, 250, this.TargetDatabase, true);
                da.AddParameter("Algorithm", System.Data.SqlDbType.VarChar, 20, this.Algorithm, true);
                da.AddParameter("RequestID", System.Data.SqlDbType.VarChar, 20, this.RequestID);
                da.AddParameter("StartTime", System.Data.SqlDbType.DateTime2, 0, this.StartTime, true);
                da.AddParameter("EndTime", System.Data.SqlDbType.DateTime2, 0, this.EndTime, true);
                da.AddParameter("LastStatus", System.Data.SqlDbType.VarChar, 8, this.LastStatus.ToString(), true);
                da.AddParameter("StatusInformation", System.Data.SqlDbType.VarChar, 0, this.StatusInformation, true);

                this.ID = (int)da.ExecuteParameter("ID");
            }
        }

        public void AddAlignment(BlastN.Alignment Alignment)
        {
            using (DataAccess da = new DataAccess("NCBI.BlastNAlignment_Edit"))
            {
                da.AddParameter("RequestID", this.ID);
                da.AddParameter("AlignmentID", Alignment.ID);

                da.ExecuteCommand();
            }
        }

        public void SetGeneStatus(Genes.Gene Gene, Jobs.JobGeneStatuses Status)
        {
            using (DataAccess da = new DataAccess("NCBI.Gene_Edit"))
            {
                da.AddParameter("RequestID", this.ID);
                da.AddParameter("GeneID", System.Data.SqlDbType.UniqueIdentifier, 0, Gene.ID);
                da.AddParameter("StatusID", System.Data.SqlDbType.Int, Jobs.JobGeneStatus.IDByKey(Status));

                da.ExecuteCommand();
            }
        }

        public void SetGenesStatus(IEnumerable<string> GeneIDs, Jobs.JobGeneStatuses Status)
        {
            using (DataAccess da = new DataAccess("NCBI.Gene_EditMultiple"))
            {
                da.AddParameter("RequestID", this.ID);
                da.AddListParameter("GeneIDs", GeneIDs.Select(g => Guid.Parse(g)));
                da.AddParameter("StatusID", System.Data.SqlDbType.Int, Jobs.JobGeneStatus.IDByKey(Status));

                da.ExecuteCommand();
            }
        }

        #region Static
        public static RequestStatus ConvertServiceRequestStatus(ServiceRequestStatus Status)
        {
            RequestStatus status = RequestStatus.Pending;
            Enum.TryParse<RequestStatus>(Status.ToString(), out status);
            return status;
        }

        public static RequestStatus ParseRequestStatus(string Status)
        {
            RequestStatus status = RequestStatus.Undefined;
            Enum.TryParse<RequestStatus>(Status, out status);
            return status;
        }
        #endregion
    }

    public enum RequestStatus
    {
        Undefined = 0,
        Pending = 1,
        Queued = 2,
        Waiting = 3,
        Ready = 4,
        Error = 5,
        Canceled = 6
    }
}