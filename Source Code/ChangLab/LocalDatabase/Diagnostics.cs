using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace ChangLab.LocalDatabase
{
    /// <remarks>
    /// This class could be where I put the Backup and Restore functionality.
    /// </remarks>
    public class Diagnostics
    {
        /// <summary>
        /// Validates that the client can communicate with the database configured in DataAccess.ConnectionString.
        /// </summary>
        public static bool VerifyDatabaseConnectivity()
        {
            try
            {
                try
                {
                    using (DataAccess da = new DataAccess("Common.VerifyDatabaseConnectivity"))
                    {
                        using (SqlDataReader reader = da.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DataAccess._connectedDatabaseVersion = (string)reader["DatabaseVersion"];
                            }
                        }
                    }
                }
                catch (SqlException sqlEx)
                {
                    if (sqlEx.Message.ToLower().Contains("could not find stored procedure 'common.verifydatabaseconnectivity'"))
                    {
                        // TODO: If before going public we require that all ChangLab users upgrade their databases to the latest, after that upgrade we
                        // can remove this catch, which is accomodating a user upgrading to a new version of Pilgrimage when they're running a
                        // database that predates 1.4.0.11, because that older database would not have had the new stored procedure used above.
                        // 1.4.0.10 and earlier also didn't have Common.ApplicationProperty_GetByKey.
                        using (DataAccess da = new DataAccess("Common.ApplicationProperty_List"))
                        {
                            using (SqlDataReader reader = da.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    if ((string)reader["Key"] == "DatabaseVersion")
                                    {
                                        DataAccess._connectedDatabaseVersion = (string)reader["Value"];
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    else { throw sqlEx; }
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
