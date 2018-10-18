using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChangLab.Common;

namespace ChangLab.Genes
{
    public class ExonOrientation : ReferenceItem
    {
        internal override string ListProcedure
        {
            get
            {
                return "Gene.ExonOrientation_List";
            }
        }

        public static int IDByKey(ExonOrientations Key)
        {
            return ReferenceItemCollection<ExonOrientation>.GetInstance(typeof(ExonOrientation)).GetIDByKey(Key);
        }
    }

    public enum ExonOrientations
    {
        PlusPlus = 1,
        PlusMinus = 2
    }
}