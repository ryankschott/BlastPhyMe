using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChangLab.Common;
using ChangLab.Genes;

namespace ChangLab.NCBI.GenBank
{
    public class GenBankSearch : ServiceSearch<Gene>
    {
        public override EUtilities.Databases Database
        {
            get { return EUtilities.Databases.NucCore; }
        }

        public override EUtilitiesXMLParser<Gene> XMLParser
        {
            get { return new GenBankXMLParser(); }
        }

        public static Dictionary<int, bool> InRecordSet_ByGenBankID(IEnumerable<int> GenBankIDs, string RecordSetID)
        {
            Dictionary<int, bool> results = GenBankIDs.Select(id => new KeyValuePair<int, bool>(id, false)).ToDictionary(kv => kv.Key, kv => kv.Value);

            using (ChangLab.LocalDatabase.DataAccess da = new ChangLab.LocalDatabase.DataAccess("Gene.Gene_ExistsByGenBankID"))
            {
                da.AddListParameter("GenBankIDs", GenBankIDs);
                da.AddParameter("RecordSetID", System.Data.SqlDbType.UniqueIdentifier, RecordSetID);
                using (System.Data.SqlClient.SqlDataReader reader = da.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        results[(int)reader["ID"]] = (bool)reader["Exists"];
                    }
                }
            }

            return results;
        }
    }
}
