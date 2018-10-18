using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using ChangLab.Common;
using ChangLab.Genes;
using ChangLab.LocalDatabase;

namespace ChangLab.RecordSets
{
    public class SubSet
    {
        #region Properties
        public string ID { get; private set; }
        public string Name { get; set; }

        public DateTime LastOpenedAt { get; set; }
        public bool Open { get; set; }
        public int DisplayIndex { get; set; }
        public ReferenceItem2<DataTypes> DataType { get; set; }

        /// <summary>
        /// Initially populated when the SubSet was instantiated via the List() procedure.
        /// </summary>
        public int GeneCount { get; set; }
        #endregion

        public SubSet(ReferenceItem2<DataTypes> DataType)
        {
            this.DataType = DataType;
        }

        public SubSet(DataTypes DataType) : this(DataTypeCollection.Get(DataType)) { }

        public void Save(string RecordSetID)
        {
            using (DataAccess da = new DataAccess("RecordSet.SubSet_Edit"))
            {
                da.AddParameter("ID", System.Data.SqlDbType.UniqueIdentifier, 0, this.ID, ParameterDirection.InputOutput, true);
                da.AddParameter("RecordSetID", SqlDbType.UniqueIdentifier, RecordSetID);
                da.AddParameter("Name", System.Data.SqlDbType.VarChar, 200, this.Name);
                da.AddParameter("DataTypeID", SqlDbType.Int, this.DataType.ID);

                this.ID = da.ExecuteParameter("ID").ToString();
            }
        }

        public void Opened()
        {
            using (DataAccess da = new DataAccess("RecordSet.SubSet_Opened"))
            {
                da.AddParameter("ID", System.Data.SqlDbType.UniqueIdentifier, 0, this.ID);
                da.ExecuteCommand();

                this.Open = true;
            }
        }

        public void Toggle(bool? Open = null, bool? Active = null)
        {
            if (Open != null) { this.Open = Convert.ToBoolean(Open); }

            using (DataAccess da = new DataAccess("RecordSet.SubSet_Toggle"))
            {
                da.AddParameter("ID", System.Data.SqlDbType.UniqueIdentifier, this.ID);
                da.AddParameter("Open", SqlDbType.Bit, Open, true);
                da.AddParameter("Active", SqlDbType.Bit, Active, true);

                da.ExecuteCommand();
            }
        }

        public List<Gene> ListAllGenes()
        {
            using (DataAccess da = new DataAccess("RecordSet.SubSetGene_List"))
            {
                da.AddParameter("SubSetID", SqlDbType.UniqueIdentifier, this.ID);

                DataTable table = da.ExecuteDataTable();
                return table.Rows.Cast<DataRow>().Select(row => Gene.FromDatabaseRow(row)).ToList();
            }
        }

        #region Database
        internal static SubSet FromDatabaseRow(DataRow Row, ReferenceItem2<DataTypes> DataType)
        {
            SubSet rs = new SubSet(DataType)
            {
                ID = Row["ID"].ToString(),
                Name = Row["Name"].ToSafeString(),
                LastOpenedAt = Row.ToSafeDateTime("LastOpenedAt"),
                Open = Row.ToSafeBoolean("Open"),
                DisplayIndex = Row.ToSafeInt("DisplayIndex"),
                GeneCount = Row.ToSafeInt("GeneCount"),
            };
            
            return rs;
        }

        public static void AllGeneReferenceNames(string SubSetID, out List<string> OrganismNames, out List<string> GeneNames)
        {
            using (DataAccess da = new DataAccess("RecordSet.SubSetGene_ListReferenceNames"))
            {
                da.AddParameter("SubSetID", SqlDbType.UniqueIdentifier, SubSetID, true);

                using (DataSet results = da.ExecuteDataSet())
                {
                    OrganismNames = results.Tables[0].Rows.Cast<DataRow>().Select(row => (string)row["Organism"]).ToList();
                    GeneNames = results.Tables[1].Rows.Cast<DataRow>().Select(row => (string)row["GeneName"]).ToList();
                }
            }
        }

        public static List<string> ListSubSetIDsForGeneIDs(IEnumerable<string> GeneIDs)
        {
            List<string> subSetIDs = new List<string>();

            using (DataAccess da = new DataAccess("RecordSet.SubSet_ListSubSetIDsForGeneIDs"))
            {
                da.AddListParameter("GeneIDs", GeneIDs.Select(g => Guid.Parse(g)));

                using (var reader = da.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        subSetIDs.Add(reader.ToSafeString("ID"));
                    }
                }
            }

            return subSetIDs;
        }
        #endregion
    }

    public class DataTypeCollection : ReferenceItemCollection2<DataTypes> { }

    [ChangLab.Common.ReferenceItemAttribute(ListProcedure = "RecordSet.DataType_List")]
    public enum DataTypes
    {
        Undefined = 0,
        GeneSequence = 1,
        CodeMLResult = 2
    }
}
