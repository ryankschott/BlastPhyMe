using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using ChangLab.Common;
using ChangLab.LocalDatabase;

namespace ChangLab.NCBI
{
    public class ESearchResult
    {
        public string WebEnvironment { get; set; }
        public string QueryKey { get; set; }
        public int ResultCount { get; set; }
        public int ReturnMaximum { get; set; }

        public List<int> IDList { get; private set; }

        public ESearchResult()
        {
            IDList = new List<int>();
        }
    }

    public class ESearchHistory : ESearchResult
    {
        #region Properties
        public string ID { get; private set; }
        public string TargetDatabase { get; set; }
        public string Term { get; set; }
        public DateTime QueryAt { get; set; }
        #endregion

        public void Save(string RecordSetID)
        {
            using (DataAccess da = new DataAccess("NCBI.ESearchHistory_Edit"))
            {
                da.AddParameter("RecordSetID", System.Data.SqlDbType.UniqueIdentifier, RecordSetID);
                da.AddParameter("TargetDatabase", System.Data.SqlDbType.VarChar, 20, this.TargetDatabase);
                da.AddParameter("Term", System.Data.SqlDbType.VarChar, 2000, this.Term);
                da.AddParameter("WebEnvironment", System.Data.SqlDbType.VarChar, 200, this.WebEnvironment);
                da.AddParameter("QueryKey", System.Data.SqlDbType.VarChar, 10, this.QueryKey);
                da.AddParameter("ResultCount", System.Data.SqlDbType.Int, this.ResultCount);
                da.AddParameter("ReturnMaximum", System.Data.SqlDbType.Int, this.ReturnMaximum);
                da.AddParameter("ID", System.Data.SqlDbType.UniqueIdentifier, 16, this.ID, ParameterDirection.InputOutput, true);

                this.ID = da.ExecuteParameter("ID").ToString();
            }
        }

        public static List<ESearchHistory> List(string RecordSetID, EUtilities.Databases Database)
        {
            List<ESearchHistory> results = new List<ESearchHistory>();

            using (DataAccess da = new DataAccess("NCBI.ESearchHistory_List"))
            {
                da.AddParameter("RecordSetID", System.Data.SqlDbType.UniqueIdentifier, RecordSetID);
                da.AddParameter("TargetDatabase", System.Data.SqlDbType.VarChar, 20, Database);

                using (DataTable records = da.ExecuteDataTable())
                {
                    records.Rows.Cast<DataRow>().ToList().ForEach(row =>
                        {
                            results.Add(new ESearchHistory()
                            {
                                ID = row["ID"].ToString(),
                                Term = (string)row["Term"],
                                WebEnvironment = (string)row["WebEnvironment"],
                                QueryKey = (string)row["QueryKey"],
                                ResultCount = (int)row["ResultCount"],
                                ReturnMaximum = (int)row["ReturnMaximum"],
                                QueryAt = row.ToSafeDateTime("QueryAt")
                            });
                        });
                }
            }

            return results;
        }
    }
}
