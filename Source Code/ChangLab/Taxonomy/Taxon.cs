using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using ChangLab.Common;
using ChangLab.RecordSets;
using ChangLab.LocalDatabase;

namespace ChangLab.Taxonomy
{
    public class Taxon
    {
        #region Properties
        public int ID { get; private set; }
        public string Name { get; internal set; }
        public string Hierarchy { get; private set; }

        public int ParentID { get; private set; }
        #endregion

        #region NCBI Properties
        public int TaxonomyDatabaseID { get; internal set; }
        
        public string OtherName { get; internal set; }
        public string Rank { get; internal set; }
        public string Division { get; internal set; }
        public string Lineage { get; internal set; }

        public List<Taxon> LineageList { get; internal set; }
        #endregion

        /// <summary>
        /// Intended for updating an existing instance after performing an EFetch to pull additional information.
        /// </summary>
        public void Merge(Taxon UpdateWith)
        {
            this.Name = UpdateWith.Name;
            this.OtherName = UpdateWith.OtherName;
            this.Rank = UpdateWith.Rank;
            this.Division = UpdateWith.Division;
            this.Lineage = UpdateWith.Lineage;
            
            if (this.LineageList == null) { this.LineageList = new List<Taxon>(); }
            else { this.LineageList.Clear(); }
            this.LineageList.AddRange(UpdateWith.LineageList);
        }

        public static List<Taxon> List(string RecordSetID, string SubSetID)
        {
            List<Taxon> taxa = new List<Taxon>();

            using (DataAccess da = new DataAccess("Taxonomy.Taxon_ListTreeView_ForRecordSet"))
            {
                da.AddParameter("RecordSetID", SqlDbType.UniqueIdentifier, RecordSetID);
                da.AddParameter("SubSetID", SqlDbType.UniqueIdentifier, SubSetID);
                
                using (DataTable results = da.ExecuteDataTable())
                {
                    foreach (DataRow row in results.Rows)
                    {
                        taxa.Add(Taxon.FromDatabaseRow(row));
                    }
                }
            }

            return taxa;
        }

        public static List<Taxon> ListByTaxaID(IEnumerable<int> TaxaIDs)
        {
            List<Taxon> taxa = new List<Taxon>();

            using (DataAccess da = new DataAccess("Taxonomy.Taxon_ListTreeView_ForTaxa"))
            {
                da.AddListParameter("TaxaIDs", TaxaIDs);

                using (DataTable results = da.ExecuteDataTable())
                {
                    foreach (DataRow row in results.Rows)
                    {
                        taxa.Add(Taxon.FromDatabaseRow(row));
                    }
                }
            }

            return taxa;
        }

        public static List<Taxon> ListByGeneID(IEnumerable<string> GeneIDs)
        {
            List<Taxon> taxa = new List<Taxon>();

            using (DataAccess da = new DataAccess("Taxonomy.Taxon_ListTreeView_ForGenes"))
            {
                da.AddListParameter("GeneIDs", GeneIDs.Select(g => Guid.Parse(g)));

                using (DataTable results = da.ExecuteDataTable())
                {
                    foreach (DataRow row in results.Rows)
                    {
                        taxa.Add(Taxon.FromDatabaseRow(row));
                    }
                }
            }

            return taxa;
        }

        public static List<Taxon> List(string NameSearch)
        {
            List<Taxon> taxa = new List<Taxon>();

            using (DataAccess da = new DataAccess("Taxonomy.Taxon_ListTreeView"))
            {
                da.AddParameter("NameSearch", SqlDbType.VarChar, 200, NameSearch);

                using (DataTable results = da.ExecuteDataTable())
                {
                    foreach (DataRow row in results.Rows)
                    {
                        taxa.Add(Taxon.FromDatabaseRow(row));
                    }
                }
            }

            return taxa;
        }

        public static Taxon FromDatabaseRow(DataRow Row)
        {
            return new Taxon()
                {
                    ID = Row.ToSafeInt("ID"),
                    Name = Row.ToSafeString("Name"),
                    Hierarchy = Row.ToSafeString("Hierarchy"),
                    ParentID = Row.ToSafeInt("ParentID"),
                    Lineage = Row.ToSafeString("Lineage")
                };
        }
    }

    public class TaxonComparer : IEqualityComparer<Taxon>
    {
        // Products are equal if their names and product numbers are equal.
        public bool Equals(Taxon x, Taxon y)
        {
            //Check whether the compared objects reference the same data.
            if (Object.ReferenceEquals(x, y)) return true;

            //Check whether any of the compared objects is null.
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            //Check whether the taxon's properties are equal.
            return x.ID == y.ID;
        }

        // If Equals() returns true for a pair of objects 
        // then GetHashCode() must return the same value for these objects.
        public int GetHashCode(Taxon taxon)
        {
            //Check whether the object is null
            if (Object.ReferenceEquals(taxon, null)) return 0;

            //Get hash code for the ID field.
            return taxon.ID.GetHashCode();
        }
    }
}