using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChangLab.Common;

namespace ChangLab.Genes
{
    public class Feature
    {
        public int ID { get; set; }
        public string GeneID { get; set; }
        public int Rank { get; set; }
        public ReferenceItem2<GeneFeatureKeys> FeatureKey { get; set; }
        public string GeneQualifier { get; set; }
        public int GeneIDQualifier { get; set; }

        public List<FeatureInterval> Intervals { get; set; }
        
        public Feature()
        {
            this.Intervals = new List<FeatureInterval>();
        }

        public void Add()
        {
            using (LocalDatabase.DataAccess da = new LocalDatabase.DataAccess("Gene.Feature_Add"))
            {
                da.AddParameter("GeneID", System.Data.SqlDbType.UniqueIdentifier, this.GeneID);
                da.AddParameter("Rank", System.Data.SqlDbType.Int, this.Rank);
                da.AddParameter("FeatureKeyID", this.FeatureKey.ID);
                da.AddParameter("GeneQualifier", System.Data.SqlDbType.VarChar, 250, this.GeneQualifier);
                da.AddParameter("GeneIDQualifier", this.GeneIDQualifier);
                da.AddParameter("ID", System.Data.SqlDbType.Int, 0, 0, System.Data.ParameterDirection.InputOutput, true);
                
                this.ID = (int)da.ExecuteParameter("ID");

                if (this.Intervals.Count != 0)
                {
                    this.Intervals.ForEach(i => 
                        {
                            i.FeatureID = this.ID;
                            i.Add();
                        });
                }
            }
        }

        public static void DeleteAll(string GeneID)
        {
            using (LocalDatabase.DataAccess da = new LocalDatabase.DataAccess("Gene.Feature_Delete"))
            {
                da.AddParameter("GeneID", System.Data.SqlDbType.UniqueIdentifier, GeneID);

                da.ExecuteCommand();
            }
        }

        /// <summary>
        /// Returns a deep copy of the current instance.
        /// </summary>
        public Feature Copy()
        {
            Feature copy = (Feature)this.MemberwiseClone();
            copy.FeatureKey = this.FeatureKey.Copy();
            return copy;
        }
    }

    public class FeatureInterval
    {
        public int ID { get; set; }
        public int FeatureID { get; set; }
        
        public int Start { get; set; }
        public int End { get; set; }
        public int Length { get { return Math.Abs(Start - End) + 1; } }
        public bool IsComplement { get; set; }
        public string StartModifier { get; set; }
        public string EndModifier { get; set; }
        public string Accession { get; set; }

        public FeatureInterval() { }

        public void Add()
        {
            using (LocalDatabase.DataAccess da = new LocalDatabase.DataAccess("Gene.FeatureInterval_Add"))
            {
                da.AddParameter("ID", this.ID);
                da.AddParameter("FeatureID", this.FeatureID);
                da.AddParameter("Start", this.Start);
                da.AddParameter("End", this.End);
                da.AddParameter("IsComplement", this.IsComplement);
                da.AddParameter("StartModifier", System.Data.SqlDbType.Char, this.StartModifier, true);
                da.AddParameter("EndModifier", System.Data.SqlDbType.Char, this.EndModifier, true);
                da.AddParameter("Accession", System.Data.SqlDbType.VarChar, 20, this.Accession, true);

                da.ExecuteCommand();
            }
        }

        public void Merge(FeatureInterval CopyFrom)
        {
            this.ID = CopyFrom.ID;
            this.FeatureID = CopyFrom.FeatureID;
            this.Start = CopyFrom.Start;
            this.End = CopyFrom.End;
            this.IsComplement = CopyFrom.IsComplement;
            this.StartModifier = CopyFrom.StartModifier;
            this.EndModifier = CopyFrom.EndModifier;
            this.Accession = CopyFrom.Accession;
        }
    }
}
