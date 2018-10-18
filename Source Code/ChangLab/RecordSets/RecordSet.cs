using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using ChangLab.Common;
using ChangLab.Genes;
using ChangLab.LocalDatabase;

namespace ChangLab.RecordSets
{
    public class RecordSet
    {
        #region Properties
        public string ID { get; private set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime LastOpenedAt { get; private set; }
        public DateTime ModifiedAt { get; private set; }
        //public string SelectedSubSetID { get; private set; }

        private Dictionary<string, List<Gene>> AllGenes { get; set; }
        private Dictionary<string, DataTable> AllGeneTables { get; set; }
        public List<Gene> Genes(string SubSetID)
        {
            if (!AllGenes.ContainsKey(SubSetID)) { AllGenes.Add(SubSetID, new List<Gene>()); }

            return AllGenes[SubSetID];
        }
        public DataTable GeneTable(string SubSetID)
        {
            if (!AllGeneTables.ContainsKey(SubSetID)) { throw new ArgumentOutOfRangeException("SubSetID", SubSetID + " has not been retrieved from the database."); }

            return AllGeneTables[SubSetID];
        }

        public int Count
        {
            get { return AllGenes.Aggregate(0, (current, g) => current += g.Value.Count); }
        }

        /// <summary>
        /// Initially populated when the RecordSet was instantiated via the List() procedure.
        /// </summary>
        public int GeneCountFinal { get; set; }

        public Dictionary<DataTypes, List<SubSet>> SubSets { get; private set; }
        public int MaxSubSetDisplayIndex(DataTypes DataType)
        {
            if (SubSets[DataType].Count == 0) { return 0; }
            else { return SubSets[DataType].Max(sub => sub.DisplayIndex); }
        }
        #endregion

        public RecordSet()
        {
            this.AllGenes = new Dictionary<string, List<Gene>>();
            this.AllGeneTables = new Dictionary<string, DataTable>();
            this.SubSets = new Dictionary<DataTypes, List<SubSet>>();
            this.SubSets.Add(DataTypes.GeneSequence, new List<SubSet>());
            this.SubSets.Add(DataTypes.CodeMLResult, new List<SubSet>());
        }

        public void Save()
        {
            using (DataAccess da = new DataAccess("RecordSet.RecordSet_Edit"))
            {
                da.AddParameter("ID", System.Data.SqlDbType.UniqueIdentifier, 0, this.ID, ParameterDirection.InputOutput, true);
                da.AddParameter("Name", System.Data.SqlDbType.VarChar, 200, this.Name);
                
                string id = da.ExecuteParameter("ID").ToString();
                if (id.ToUpper() != this.ID.ToSafeString().ToUpper()) { this.CreatedAt = DateTime.Now; } /* A new RecordSet was created */
                this.ID = id;

                this.ModifiedAt = DateTime.Now;
            }
        }

        public void Opened()
        {
            using (DataAccess da = new DataAccess("RecordSet.RecordSet_Opened"))
            {
                da.AddParameter("ID", System.Data.SqlDbType.UniqueIdentifier, 0, this.ID);
                da.ExecuteCommand();
            }
        }

        public void Delete()
        {
            using (DataAccess da = new DataAccess("RecordSet.RecordSet_Edit"))
            {
                da.AddParameter("ID", System.Data.SqlDbType.UniqueIdentifier, 0, this.ID);
                da.AddParameter("Name", System.Data.SqlDbType.VarChar, 200, this.Name);
                da.AddParameter("Active", SqlDbType.Bit, false);

                da.ExecuteCommand();
            }
        }

        public void AddGene(Gene Gene, string SubSetID)
        {
            using (DataAccess da = new DataAccess("RecordSet.Gene_Edit"))
            {
                da.AddParameter("RecordSetID", SqlDbType.UniqueIdentifier, this.ID);
                da.AddParameter("GeneID", SqlDbType.UniqueIdentifier, Gene.ID);
                da.AddParameter("SubSetID", SqlDbType.UniqueIdentifier, SubSetID);

                da.ExecuteCommand();
            }

            AddGeneToAllGenes(Gene, SubSetID);
        }

        public void AddGenes(IEnumerable<Gene> Genes, string SubSetID)
        {
            AddGenes(Genes, this.ID, SubSetID);

            Genes.ToList().ForEach(g => AddGeneToAllGenes(g, SubSetID));
        }

        public static void AddGenes(IEnumerable<Gene> Genes, string RecordSetID, string SubSetID)
        {
            using (DataAccess da = new DataAccess("RecordSet.Gene_EditMultiple"))
            {
                da.AddParameter("RecordSetID", SqlDbType.UniqueIdentifier, RecordSetID);
                da.AddParameter("SubSetID", SqlDbType.UniqueIdentifier, SubSetID);
                da.AddListParameter("GeneIDs", Genes.Select(g => Guid.Parse(g.ID)));

                da.ExecuteCommand();
            }
        }

        private void AddGeneToAllGenes(Gene Gene, string SubSetID)
        {
            if (AllGenes == null)
            {
                throw new Exception("How is this possible?");
            }

            if (!AllGenes.ContainsKey(SubSetID))
            {
                AllGenes.Add(SubSetID, new List<Gene>());
            }

            if (!AllGenes[SubSetID].Any(g => GuidCompare.Equals(g.ID, Gene.ID)))
            {
                AllGenes[SubSetID].Add(Gene);
            }
        }

        public void RemoveGene(Gene Gene, string SubSetID)
        {
            using (DataAccess da = new DataAccess("RecordSet.Gene_Delete"))
            {
                da.AddParameter("RecordSetID", SqlDbType.UniqueIdentifier, this.ID);
                da.AddParameter("GeneID", SqlDbType.UniqueIdentifier, Gene.ID);
                da.AddParameter("SubSetID", SqlDbType.UniqueIdentifier, SubSetID);

                da.ExecuteCommand();
            }

            Gene remove = AllGenes[SubSetID].FirstOrDefault(g => GuidCompare.Equals(g.ID, Gene.ID));
            if (remove != null)
            {
                AllGenes[SubSetID].Remove(remove);
            }
        }

        public void RemoveGenes(IEnumerable<Gene> Genes, string SubSetID)
        {
            RemoveGenes(Genes, this.ID, SubSetID);

            Genes.ToList().ForEach(gene =>
                {
                    Gene remove = AllGenes[SubSetID].FirstOrDefault(g => GuidCompare.Equals(g.ID, gene.ID));
                    if (remove != null)
                    {
                        AllGenes[SubSetID].Remove(remove);
                    }
                });
        }

        public static void RemoveGenes(IEnumerable<Gene> Genes, string RecordSetID, string SubSetID)
        {
            using (DataAccess da = new DataAccess("RecordSet.Gene_DeleteMultiple"))
            {
                da.AddParameter("RecordSetID", SqlDbType.UniqueIdentifier, RecordSetID);
                da.AddParameter("SubSetID", SqlDbType.UniqueIdentifier, SubSetID);
                da.AddListParameter("GeneIDs", Genes.Select(g => Guid.Parse(g.ID)));

                da.ExecuteCommand();
            }
        }

        public void RefreshGenesFromDatabase(string SubSetID)
        {
            if (AllGenes != null && AllGenes.ContainsKey(SubSetID) && AllGenes[SubSetID] != null) { AllGenes[SubSetID].Clear(); }
            
            using (DataAccess da = new DataAccess("RecordSet.SubSetGene_List"))
            {
                da.AddParameter("SubSetID", SqlDbType.UniqueIdentifier, SubSetID, true);

                DataTable table = da.ExecuteDataTable();

                if (!AllGeneTables.ContainsKey(SubSetID)) { AllGeneTables.Add(SubSetID, table); }
                else { AllGeneTables[SubSetID] = table; }
                
                foreach (DataRow row in table.Rows)
                {
                    AddGeneToAllGenes(Gene.FromDatabaseRow(row), SubSetID);
                }
            }
        }

        public List<SubSet> ListSubSets(DataTypes DataType, bool Active = true)
        {
            ReferenceItem2<DataTypes> dataType = DataTypeCollection.Get(DataType);

            using (DataAccess da = new DataAccess("RecordSet.SubSet_List"))
            {
                da.AddParameter("RecordSetID", SqlDbType.UniqueIdentifier, this.ID);
                da.AddParameter("DataTypeID", SqlDbType.Int, dataType.ID);

                this.SubSets[DataType] = da.ExecuteDataTable().Rows.Cast<DataRow>().Select(row => SubSet.FromDatabaseRow(row, dataType)).ToList();
                return this.SubSets[DataType];
            }
        }

        public List<Gene> ListAllGenes()
        {
            using (DataAccess da = new DataAccess("RecordSet.Gene_List"))
            {
                da.AddParameter("RecordSetID", SqlDbType.UniqueIdentifier, this.ID);

                DataTable table = da.ExecuteDataTable();
                return table.Rows.Cast<DataRow>().Select(row => Gene.FromDatabaseRow(row)).ToList();
            }
        }

        public void ReorderSubSets(Dictionary<SubSet, int> DisplayIndexes)
        {
            using (DataAccess da = new DataAccess("RecordSet.SubSets_Reorder"))
            {
                da.AddDictionaryParameter("SubSets", DisplayIndexes.ToDictionary(kv => Guid.Parse(kv.Key.ID), kv => kv.Value));
                da.ExecuteCommand();
            }
        }

        #region Static
        public static List<RecordSet> List(bool? Active = null)
        {
            List<RecordSet> recordsets = new List<RecordSet>();

            using (DataAccess da = new DataAccess("RecordSet.RecordSet_List"))
            {
                da.AddParameter("Active", SqlDbType.Bit, Active, true);

                using (DataTable dt = da.ExecuteDataTable())
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        recordsets.Add(RecordSet.FromDatabaseRow(row));
                    }
                }
            }

            return recordsets;
        }

        private static RecordSet FromDatabaseRow(DataRow Row)
        {
            RecordSet rs = new RecordSet()
            {
                ID = Row["ID"].ToString(),
                Name = Row["Name"].ToSafeString(),
                CreatedAt = (DateTime)Row["CreatedAt"],
                LastOpenedAt = Row.ToSafeDateTime("LastOpenedAt"),
                ModifiedAt = (DateTime)Row["ModifiedAt"]
            };
            if (Row.Table.Columns.Contains("GeneCountFinal"))
            {
                rs.GeneCountFinal = Row["GeneCountFinal"].ToSafeInt();
            }

            return rs;
        }
        #endregion
    }
}