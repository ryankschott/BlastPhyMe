using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChangLab.Common;

namespace ChangLab.Jobs
{
    public class JobTarget : ReferenceItem2<JobTargets>
    {
        //internal override string ListProcedure
        //{
        //    get { return "Job.Target_List"; }
        //}
        
        //public static int IDByKey(JobTargets Key)
        //{
        //    return ReferenceItemCollection<JobTarget>.GetInstance(typeof(JobTarget)).GetIDByKey(Key);
        //}

        //public static JobTargets KeyByID(int ID)
        //{
        //    return ReferenceItemCollection<JobTarget>.GetInstance(typeof(JobTarget)).GetKeyByID<JobTargets>(ID);
        //}
    }

    public class JobTargetCollection : ReferenceItemCollection2<JobTargets> { }

    [ChangLab.Common.ReferenceItemAttribute(ListProcedure = "Job.Target_List")]
    public enum JobTargets
    {
        Undefined = 0,
        BLASTN_NCBI = 1,
        BLASTN_Local = 2,
        Ensembl = 3,
        CodeML = 4,
        PRANK = 5,
        PhyML = 6,
        MUSCLE = 7,
        Import_DataFile = 8
    }
}