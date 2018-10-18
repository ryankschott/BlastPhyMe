using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChangLab.Common;
using ChangLab.Reference;

namespace ChangLab.Jobs
{
    public class JobGeneStatus : ReferenceItem
    {
        internal override string ListProcedure
        {
            get { return "Job.GeneStatus_List"; }
        }

        public static int IDByKey(JobGeneStatuses Key)
        {
            return ReferenceItemCollection<JobGeneStatus>.GetInstance(typeof(JobGeneStatus)).GetIDByKey(Key);
        }
    }

    public enum JobGeneStatuses
    {
        Undefined = 0,
        NotSubmitted = 1,
        Submitted = 2,
        Processed = 3
    }
}
