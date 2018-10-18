using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChangLab.Common;
using ChangLab.LocalDatabase;

namespace ChangLab.PAML.CodeML
{
    public class Result
    {
        #region Properties
        public int ID { get; private set; }
        public int TreeID { get; set; }
        public int AnalysisConfigurationID { get; set; }
        public int NSSite { get; set; }
        public double Kappa { get; set; }
        public double Omega { get; set; }
        public int np { get; set; }
        public double lnL { get; set; }
        public double k { get; set; }
        public TimeSpan Duration { get; set; }

        public List<ResultdNdSValue> Values { get; private set; }
        #endregion

        public Result()
        {
            Values = new List<ResultdNdSValue>();
            this.Duration = TimeSpan.MinValue;
        }

        #region Database
        public void Save(bool SaveValues = true)
        {
            using (DataAccess da = new DataAccess("PAML.Result_Add"))
            {
                da.AddParameter("TreeID", System.Data.SqlDbType.Int, this.TreeID, true);
                da.AddParameter("AnalysisConfigurationID", this.AnalysisConfigurationID);
                da.AddParameter("NSSite", this.NSSite);
                da.AddDoubleParameter("Kappa", 9, 3, this.Kappa);
                da.AddDoubleParameter("Omega", 9, 3, this.Omega);
                da.AddParameter("np", this.np);
                da.AddDoubleParameter("lnL", 19, 8, this.lnL);
                da.AddDoubleParameter("k", 9, 6, this.k);
                da.AddParameter("Duration", System.Data.SqlDbType.Time, this.Duration, true);
                da.AddParameter("ID", System.Data.SqlDbType.Int, 0, this.ID, System.Data.ParameterDirection.InputOutput, true);

                this.ID = (int)da.ExecuteParameter("ID");
            }

            if (SaveValues)
            {
                Values.ForEach(val => 
                    {
                        val.ResultID = this.ID;
                        val.Save();
                    });
            }
        }

        public static void AddToSubSet(string SubSetID, int ResultID)
        {
            using (DataAccess da = new DataAccess("PAML.SubSetResult_Edit"))
            {
                da.AddParameter("SubSetID", System.Data.SqlDbType.UniqueIdentifier, SubSetID);
                da.AddParameter("ResultID", System.Data.SqlDbType.Int, ResultID);
                da.ExecuteCommand();
            }
        }

        public static void AddToSubSet(string SubSetID, List<int> ResultIDs)
        {
            using (DataAccess da = new DataAccess("PAML.SubSetResult_EditMultiple"))
            {
                da.AddParameter("SubSetID", System.Data.SqlDbType.UniqueIdentifier, SubSetID);
                da.AddListParameter("ResultIDs", ResultIDs);
                da.ExecuteCommand();
            }
        }

        public static void DeleteFromSubSet(string SubSetID, List<int> ResultIDs)
        {
            using (DataAccess da = new DataAccess("PAML.SubSetResult_DeleteMultiple"))
            {
                da.AddParameter("SubSetID", System.Data.SqlDbType.UniqueIdentifier, SubSetID);
                da.AddListParameter("ResultIDs", ResultIDs);
                da.ExecuteCommand();
            }
        }

        public static System.Data.DataTable List(string SubSetID)
        {
            using (DataAccess da = new DataAccess("PAML.Result_List"))
            {
                da.AddParameter("SubSetID", System.Data.SqlDbType.UniqueIdentifier, SubSetID);
                return da.ExecuteDataTable();
            }
        }

        public static System.Data.DataSet Details(int ResultID)
        {
            using (DataAccess da = new DataAccess("PAML.Result_Details"))
            {
                da.AddParameter("ResultID", System.Data.SqlDbType.Int, ResultID);
                return da.ExecuteDataSet();
            }
        }

        public static void Delete(int ResultID, bool DeleteRelated = true)
        {
            using (DataAccess da = new DataAccess("PAML.Result_Delete"))
            {
                da.AddParameter("ResultID", System.Data.SqlDbType.Int, ResultID);
                da.AddParameter("DeleteRelated", System.Data.SqlDbType.Bit, DeleteRelated);
                da.ExecuteCommand();
            }
        }
        #endregion
    }

    public class ResultdNdSValue
    {
        #region Properties
        public int ID { get; private set; }
        public int ResultID { get; set; }
        public string SiteClass { get; set; }
        public ReferenceItem2<ResultdNdSValueTypes> ValueType { get; set; }
        public int Rank { get; set; }
        public double Value { get; set; }
        #endregion

        public ResultdNdSValue()
        {
            this.ID = 0;
        }

        public ResultdNdSValue(ResultdNdSValueTypes ValueType, double Value)
        {
            this.ID = 0;
            this.ValueType = ResultdNdSValueTypeCollection.Get(ValueType);
            this.Value = Value;
        }

        #region Database
        public void Save()
        {
            using (DataAccess da = new DataAccess("PAML.ResultdNdSValue_Add"))
            {
                da.AddParameter("ResultID", this.ResultID);
                da.AddParameter("SiteClass", System.Data.SqlDbType.VarChar, 2, this.SiteClass);
                da.AddParameter("ValueTypeID", this.ValueType.ID);
                da.AddParameter("Rank", this.Rank);
                da.AddDoubleParameter("Value", 9, 6, this.Value);
                da.AddParameter("ID", System.Data.SqlDbType.Int, 0, this.ID, System.Data.ParameterDirection.InputOutput, true);
                //da.AddOutputParameter("ID", this.ID);

                this.ID = (int)da.ExecuteParameter("ID");
            }
        }
        #endregion
    }

    public class ResultdNdSValueTypeCollection : ReferenceItemCollection2<ResultdNdSValueTypes>
    {
    }

    [ChangLab.Common.ReferenceItemAttribute(ListProcedure="PAML.ResultdNdSValueType_List")]
    public enum ResultdNdSValueTypes
    {
        Undefined = 0,
        p_value = 1,
        w_value = 2,
        q_value = 3,
        p1_value = 4,
        background_w = 5,
        foreground_w = 6,
        branch_type = 7
    }
}
