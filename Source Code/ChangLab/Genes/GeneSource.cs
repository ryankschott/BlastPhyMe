using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChangLab.Common;

namespace ChangLab.Genes
{
    public class GeneSource : ReferenceItem
    {
        internal override string ListProcedure { get { return "Gene.Source_List"; } }

        public static int IDByKey(GeneSources Key)
        {
            return ReferenceItemCollection<GeneSource>.Instance.GetIDByKey(Key);

            //return ReferenceItem.GetIDByKey(typeof(GeneSource), Key);

            //return ReferenceItemCollection<GeneSource>.GetInstance(typeof(GeneSource)).GetIDByKey(Key);
        }

        public static GeneSources KeyByID(int ID)
        {
            return ReferenceItemCollection<GeneSource>.GetInstance(typeof(GeneSource)).GetKeyByID<GeneSources>(ID);
        }

        public static string NameByID(int ID)
        {
            return ReferenceItemCollection<GeneSource>.GetInstance(typeof(GeneSource)).GetNameByID(ID);
        }

        public static int IDByName(string Name)
        {
            int id = 0;
            ReferenceItemCollection<GeneSource>.GetInstance(typeof(GeneSource)).TryGetIDByName(Name, out id);
            return id;
        }
    }

    public enum GeneSources
    {
        Undefined,
        FASTA,
        BLASTN_NCBI,
        GenBank,
        Ensembl,
        BLASTN_Local,
        User,
        MEGA,
        PRANK,
        MUSCLE,
        Trinity,
        NEXUS,
        PHYLIP
    }
}