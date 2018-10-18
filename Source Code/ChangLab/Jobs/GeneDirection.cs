using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChangLab.Common;
using ChangLab.Reference;

namespace ChangLab.Jobs
{
    public class GeneDirection : ReferenceItem
    {
        internal override string ListProcedure
        {
            get { return "Job.GeneDirection_List"; }
        }

        public static int IDByKey(GeneDirections Key)
        {
            return ReferenceItemCollection<GeneDirection>.GetInstance(typeof(GeneDirection)).GetIDByKey(Key);
        }
    }

    public enum GeneDirections
    {
        Input = 1,
        Output = 2
    }
}