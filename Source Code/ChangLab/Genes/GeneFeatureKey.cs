using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChangLab.Common;

namespace ChangLab.Genes
{
    public class GeneFeatureKey : ReferenceItem2<GeneFeatureKeys>
    {
        //internal override string ListProcedure { get { return "Gene.FeatureKey_List"; } }

        //public static int IDByKey(GeneFeatureKeys Key)
        //{
        //    return ReferenceItemCollection<GeneFeatureKey>.GetInstance(typeof(GeneFeatureKey)).GetIDByKey(Key);
        //}

        //public static GeneFeatureKeys KeyByID(int ID)
        //{
        //    return ReferenceItemCollection<GeneFeatureKey>.GetInstance(typeof(GeneFeatureKey)).GetKeyByID<GeneFeatureKeys>(ID);
        //}

        //public static string NameByID(int ID)
        //{
        //    return ReferenceItemCollection<GeneFeatureKey>.GetInstance(typeof(GeneFeatureKey)).GetNameByID(ID);
        //}

        //public static int RankByID(int ID)
        //{
        //    return ReferenceItemCollection<GeneFeatureKey>.GetInstance(typeof(GeneFeatureKey)).GetRankByID(ID);
        //}

        public static void Survey(int GenBankID, string FeatureKey)
        {
            using (LocalDatabase.DataAccess da = new LocalDatabase.DataAccess("Gene.FeatureKeySurvey_Add"))
            {
                da.AddParameter("GenBankID", GenBankID);
                da.AddParameter("FeatureKey", System.Data.SqlDbType.VarChar, 250, FeatureKey);

                da.ExecuteCommand();
            }
        }

        /// <summary>
        /// In place of the generic Enum.TryParse, necessary because 3'UTR and 5'UTR are feature keys and those don't translate easily into enum
        /// values (can't lead an enum value with a number for whatever reason).
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        public static bool TryParse(string Value, out GeneFeatureKeys Key)
        {
            switch (Value)
            {
                case "3'UTR":
                    Key = GeneFeatureKeys.UTR_3; return true;
                case "5'UTR":
                    Key = GeneFeatureKeys.UTR_5; return true;
                default:
                    return Enum.TryParse<GeneFeatureKeys>(Value, out Key);
            }
        }
    }

    public class GeneFeatureKeyCollection : ReferenceItemCollection2<GeneFeatureKeys> { }

    /// <remarks>
    /// Case sensitivity matches GenBank INSD or GB standard for XML output
    /// </remarks>
    [ChangLab.Common.ReferenceItemAttribute(ListProcedure = "Gene.FeatureKey_List")]
    public enum GeneFeatureKeys
    {
        Undefined = 0,
        gene = 1,
        mRNA = 2,
        exon = 3,
        CDS = 4,
        STS = 5,
        source = 6,
        misc_RNA = 7,
        misc_feature = 8,
        polyA_site = 9,
        UTR_3 = 10,
        UTR_5 = 11,
        unsure = 12 // No really, this was used as a feature key (example GI: 410068626)
    }
}
