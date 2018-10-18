using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ChangLab.LocalDatabase;

namespace ChangLab.Common
{
    public class ThirdPartyComponentReference
    {
        public int ID { get; private set; }
        public string Name { get; private set; }
        public string LongName { get; private set; }
        public string Version { get; private set; }
        public string Creator { get; private set; }
        public string ProductURL { get; private set; }
        public DateTime LastUpdatedAt { get; private set; }
        public DateTime LastRetrievedAt { get; private set; }
        public string Copyright { get; private set; }
        public string LicenseType { get; private set; }
        public string LicenseURL { get; private set; }
        public string LicenseText { get; private set; }
        public bool Modified { get; private set; }
        public string Logo { get; private set; }
        public bool Packaged { get; private set; }
        public string Citation { get; private set; }

        public static List<ThirdPartyComponentReference> List()
        {
            List<ThirdPartyComponentReference> references = new List<ThirdPartyComponentReference>();

            using (DataAccess da = new DataAccess("Common.ThirdPartyComponentReference_List"))
            {
                using (SqlDataReader reader = da.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        references.Add(new ThirdPartyComponentReference() {
                            ID = (int)reader["ID"],
                            Name = (string)reader["Name"],
                            LongName = reader.ToSafeString("LongName"),
                            Version = (string)reader["Version"],
                            Creator = (string)reader["Creator"],
                            ProductURL = (string)reader["ProductURL"],
                            LastUpdatedAt = (DateTime)reader["LastUpdatedAt"],
                            LastRetrievedAt = (DateTime)reader["LastRetrievedAt"],
                            Copyright = reader.ToSafeString("Copyright"),
                            LicenseType = reader.ToSafeString("LicenseType"),
                            LicenseURL = reader.ToSafeString("LicenseURL"),
                            LicenseText = reader.ToSafeString("LicenseText"),
                            Modified = (bool)reader["Modified"],
                            Logo = reader.ToSafeString("Logo"),
                            Packaged = (bool)reader["Packaged"],
                            Citation = reader.ToSafeString("Citation"),
                        });
                    }
                }
            }

            return references;
        }
    }
}