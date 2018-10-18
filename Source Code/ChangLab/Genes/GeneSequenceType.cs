using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChangLab.Common;

namespace ChangLab.Genes
{
    public class GeneSequenceType : ReferenceItem
    {
        internal override string ListProcedure { get { return "Gene.SequenceType_List"; } }

        public static int IDByKey(GeneSequenceTypes Key)
        {
            return ReferenceItemCollection<GeneSequenceType>.GetInstance(typeof(GeneSequenceType)).GetIDByKey(Key);
        }

        public static GeneSequenceTypes KeyByID(int ID)
        {
            return ReferenceItemCollection<GeneSequenceType>.GetInstance(typeof(GeneSequenceType)).GetKeyByID<GeneSequenceTypes>(ID);
        }

        public static string NameByID(int ID)
        {
            return ReferenceItemCollection<GeneSequenceType>.GetInstance(typeof(GeneSequenceType)).GetNameByID(ID);
        }
    }

    public enum GeneSequenceTypes
    {
        NotDefined = 0,
        Source = 1,
        Gene = 2,
        Coding = 3,
        Alignment = 4
    }
}