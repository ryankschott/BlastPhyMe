using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChangLab.Common;

namespace ChangLab.Jobs
{
    public class JobStatus : ReferenceItem
    {
        internal override string ListProcedure
        {
            get { return "Job.Status_List"; }
        }

        public static int IDByKey(JobStatuses Key)
        {
            return ReferenceItemCollection<JobStatus>.GetInstance(typeof(JobStatus)).GetIDByKey(Key);
        }

        public static JobStatuses KeyByID(int ID)
        {
            return ReferenceItemCollection<JobStatus>.GetInstance(typeof(JobStatus)).GetKeyByID<JobStatuses>(ID);
        }

        public static string NameByID(int ID)
        {
            return ReferenceItemCollection<JobStatus>.GetInstance(typeof(JobStatus)).GetNameByID(ID);
        }
    }

    public class JobStatusCollection : ReferenceItemCollection2<JobStatuses>
    {
        public static bool CompletedOrFailed(JobStatuses Status)
        {
            return ((Status == JobStatuses.Completed) || (Status == JobStatuses.Failed));
        }
    }

    [ChangLab.Common.ReferenceItemAttribute(ListProcedure="Job.Status_List")]
    public enum JobStatuses
    {
        Undefined = 0,
        New = 1,
        Running = 2,
        Completed = 3,
        Cancelled = 4,
        Reviewed = 5,
        Archived = 6,
        Failed = 7,
        Pending = 8
    }
}
