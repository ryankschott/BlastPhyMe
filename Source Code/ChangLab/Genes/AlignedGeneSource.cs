using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChangLab.LocalDatabase;

namespace ChangLab.Genes
{
    public class AlignedGeneSource
    {
        public string JobID { get; set; }
        public string InputGeneID { get; set; }
        public string OutputGeneID { get; set; }

        public static void Add(string JobID, string InputGeneID, string OutputGeneID)
        {
            using (DataAccess da = new DataAccess("Gene.AlignedGeneSource_Edit"))
            {
                da.AddParameter("JobID", System.Data.SqlDbType.UniqueIdentifier, 0, JobID);
                da.AddParameter("InputGeneID", System.Data.SqlDbType.UniqueIdentifier, 0, InputGeneID);
                da.AddParameter("OutputGeneID", System.Data.SqlDbType.UniqueIdentifier, 0, OutputGeneID);

                da.ExecuteCommand();
            }
        }
    }
}
