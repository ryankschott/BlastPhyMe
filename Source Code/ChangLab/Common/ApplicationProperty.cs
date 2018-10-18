using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ChangLab.LocalDatabase;

namespace ChangLab.Common
{
    public class ApplicationProperty
    {
        public int ID { get; private set; }
        public string Key { get; private set; }
        public string Value { get; set; }

        public static string GetEntryAssemblyProductName()
        {
            return System.Reflection.Assembly.GetEntryAssembly().GetAssemblyAttribute<System.Reflection.AssemblyProductAttribute>(a => a.Product);
        }

        public ApplicationProperty() { }

        public ApplicationProperty(string Key)
        {
            this.Key = Key;
        }

        public void Save()
        {
            using (DataAccess da = new DataAccess("Common.ApplicationProperty_Edit"))
            {
                da.AddParameter("Key", SqlDbType.VarChar, 30, Key);
                da.AddParameter("Value", SqlDbType.VarChar, Value);

                da.ExecuteCommand();
            }
        }

        public static List<ApplicationProperty> List()
        {
            List<ApplicationProperty> properties = new List<ApplicationProperty>();

            using (DataAccess da = new DataAccess("Common.ApplicationProperty_List"))
            {
                using (SqlDataReader reader = da.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        properties.Add(new ApplicationProperty() { ID = (int)reader["ID"], Key = (string)reader["Key"], Value = reader["Value"].ToSafeString() });
                    }
                }
            }

            return properties;
        }

        public static ApplicationProperty Get(string Key)
        {
            List<ApplicationProperty> properties = new List<ApplicationProperty>();

            using (DataAccess da = new DataAccess("Common.ApplicationProperty_GetByKey"))
            {
                da.AddParameter("Key", Key, true);

                using (SqlDataReader reader = da.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        properties.Add(new ApplicationProperty() { ID = (int)reader["ID"], Key = (string)reader["Key"], Value = reader["Value"].ToSafeString() });
                    }
                }
            }

            return (properties.Count == 0 ? properties[0] : null);
        }

        public void SaveForRecordSet(string RecordSetID)
        {
            using (DataAccess da = new DataAccess("RecordSet.ApplicationProperty_Edit"))
            {
                da.AddParameter("RecordSetID", SqlDbType.UniqueIdentifier, RecordSetID);
                da.AddParameter("Key", SqlDbType.VarChar, 30, Key);
                da.AddParameter("Value", SqlDbType.VarChar, Value);

                da.ExecuteCommand();
            }
        }

        public static List<ApplicationProperty> ListForRecordSet(string RecordSetID)
        {
            List<ApplicationProperty> properties = new List<ApplicationProperty>();

            using (DataAccess da = new DataAccess("RecordSet.ApplicationProperty_List"))
            {
                da.AddParameter("RecordSetID", SqlDbType.UniqueIdentifier, RecordSetID);

                using (SqlDataReader reader = da.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        properties.Add(new ApplicationProperty() { ID = (int)reader["ID"], Key = (string)reader["Key"], Value = reader["Value"].ToSafeString() });
                    }
                }
            }

            return properties;
        }
    }
}
