using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using ChangLab.Common;
using ChangLab.Jobs;
using Microsoft.SqlServer.Management.Smo;

namespace ChangLab.LocalDatabase
{
    public class AutoUpdate : IDisposable
    {
        public string ServerName { get; set; }
        private string FilePath { get; set; }
        private Version CurrentVersion { get; set; }
        
        private Dictionary<Version, string> UpdateScripts { get; set; }
        public Version LatestVersion { get { return UpdateScripts.Last().Key; } }

        public bool UpgradesExist
        {
            get { return (this.LatestVersion > this.CurrentVersion); }
        }

        public bool DatabaseExceedsApplicationVersion
        {
            get { return (this.CurrentVersion > this.LatestVersion); }
        }

        /// <param name="CurrentVersion">
        /// Pass as empty to generate a new database at the file path given by the constuctor.  An error will be thrown if the file already exists.
        /// </param>
        public AutoUpdate(string DatabaseFilePath, Version CurrentVersion)
        {
            this.ServerName = "(localDB)\\MSSQLLocalDB";
            this.FilePath = DatabaseFilePath;
            this.CurrentVersion = CurrentVersion;

            this.UpdateScripts = System.Reflection.Assembly
                .GetExecutingAssembly()
                .GetManifestResourceNames()
                .Where(s => s.StartsWith("ChangLab.LocalDatabase.Upgrade"))
                .Select(s => new { Version = new Version(s.Substring(s.IndexOf("_") + 1, (s.LastIndexOf(".") - (s.IndexOf("_") + 1))).Replace("_", ".")), ResourceName = s })
                .OrderBy(kv => kv.Version)
                .ToDictionary(kv => kv.Version, kv => kv.ResourceName);
        }

        public Version UpdateToLatest()
        {
            if (CurrentVersion.Major != 0)
            {
                if (!File.Exists(FilePath))
                { throw new ArgumentException("Database file not found", "FilePath"); }
                //else if (!FilePath.CanReadWrite())
                //{ throw new ArgumentException("Database file not accessible", "FilePath"); }
            }
            else
            {
                // Create a new database.
                if (File.Exists(FilePath))
                { throw new ArgumentException("File already exists at the path specified for a new database.", "FilePath"); }
                else
                {
                    CreateDatabase();
                    // The upgrade path will start at script 1.0.0.0, so CreateDatabase() just generates an empty database.
                }
            }

            if (!UpdateScripts.Any(kv => kv.Key > CurrentVersion))
            {
                // Nothing to upgrade.
                return CurrentVersion;
            }

            try
            {
                string dbName = FilePath.Substring(FilePath.LastIndexOf("\\") + 1).Replace(".mdf", "");

                Server server = new Server(ServerName);
                server.ConnectionContext.LoginSecure = true;

                // By virtue of connecting to an existing database to check its version number, the database has been attached by filename to the
                // user instance.  In order to attach it the file needs to not be in use, thus we detach first (after dropping connections).
                if (server.Databases.Cast<Database>().Any(db => db.Name.ToUpper() == FilePath.ToUpper()))
                {
                    server.ConnectionContext.ExecuteNonQuery("ALTER DATABASE [" + FilePath + "] SET SINGLE_USER WITH ROLLBACK IMMEDIATE");
                    server.DetachDatabase(FilePath, false);
                }

                // Attach the database to the server if it's not already there.
                // The SMO method for running scripts requires that the database be attached to the instance.  The rest of the application's calls
                // will use ADO.Net and the AttachDBFileName configuration in the connection string, such that while the instance is up it will know
                // about the database by its full file path.  That way, should it come up that the user has two databases with the same dbname but
                // different file paths, they can run concurrently on the same user instance.
                if (!server.Databases.Cast<Database>().Any(db => db.Name == dbName))
                {
                    System.Collections.Specialized.StringCollection files = new System.Collections.Specialized.StringCollection();
                    files.Add(FilePath);
                    files.Add(FilePath.Replace(".mdf", "_log.ldf"));
                    server.AttachDatabase(dbName, files, AttachOptions.None);
                }

                Version lastVersion = null;

                List<KeyValuePair<Version, string>> scripts = UpdateScripts.Where(kv => kv.Key > CurrentVersion).ToList();
                OnProgressUpdate(new ProgressUpdateEventArgs()
                    {
                        StatusMessage = "Configuring database...",
                        Setup = true,
                        CurrentMax = scripts.Count,
                        CurrentProgress = 0
                    });
                ProgressUpdateEventArgs args = new ProgressUpdateEventArgs();

                foreach (KeyValuePair<Version, string> script in scripts)
                {
                    args.StatusMessage = "Applying updates for version " + script.Key.ToString(VersionDepth.Build);
                    OnProgressUpdate(args); args.CurrentProgress++;

                    using (Stream stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(script.Value))
                    {
                        using (StreamReader reader = new StreamReader(stream, System.Text.Encoding.Default))
                        {
                            string command = "USE [" + dbName + "]\r\nGO\r\n" + reader.ReadToEnd();
                            server.ConnectionContext.ExecuteNonQuery(command);

                            lastVersion = script.Key;
                        }
                    }
                }

                // We're showing the application version here so that the user doesn't get confused if the latest database version is lower.
                args.CurrentProgress = scripts.Count; args.StatusMessage = "Database configured to version " + System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString(VersionDepth.Build);
                OnProgressUpdate(args);

                try
                {
                    server.ConnectionContext.ExecuteNonQuery("ALTER DATABASE [" + dbName + "] SET SINGLE_USER WITH ROLLBACK IMMEDIATE");
                    server.DetachDatabase(dbName, false);
                    // It'd be nice if this detached, so that we don't run into duplicate name issues, but it's not a big problem if it doesn't.
                    // The only time I had this failing was when I had the database open in SSMS and then let AutoUpdate run a script.
                }
                catch { }
                
                return lastVersion;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        private void CreateDatabase()
        {
            using (SqlConnection conn = new SqlConnection("Data Source=" + this.ServerName + ";Integrated Security=True;"))
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    string dbName = FilePath.Substring(FilePath.LastIndexOf("\\") + 1).Replace(".mdf", "");

                    cmd.CommandText = "CREATE DATABASE [" + dbName + "] ON (name = '" + dbName + "', filename = '" + FilePath + "'); ";
                    cmd.CommandType = CommandType.Text;

                    try
                    {
                        int retryLimit = 2;
                        for (int retryCount = 0; retryCount < retryLimit; retryCount++)
                        {
                            try
                            {
                                OnProgressUpdate(new ProgressUpdateEventArgs() { StatusMessage = "Connecting to local instance of SQL Server LocalDB..." });

                                conn.Open();
                                break;
                            }
                            catch (Exception ex)
                            {
                                // If connecting to the database fails with a Connection Timeout error on the first try, that's entirely likely due
                                // to it taking longer to boot up the user instance than the connection timeout allows for.  If it fails on the 
                                // second attempt then there's more likely something else wrong.
                                if ((retryCount + 1) < retryLimit
                                    && (ex.GetType() == typeof(SqlException)))
                                {
                                    SqlException sqlEx = (SqlException)ex;
                                    if (sqlEx.Message.Contains("Connection Timeout Expired")
                                        && sqlEx.InnerException != null
                                        && sqlEx.InnerException.Message.Contains("The wait operation timed out"))
                                    {
                                        continue;
                                    }
                                }

                                throw ex;
                            }
                        }

                        OnProgressUpdate(new ProgressUpdateEventArgs() { StatusMessage = "Creating database file..." });
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }

        public void Dispose()
        {
            
        }

        protected virtual void OnProgressUpdate(ProgressUpdateEventArgs e)
        {
            if (ProgressUpdate != null)
            {
                ProgressUpdate(e);
            }
        }
        public event ProgressUpdateEventHandler ProgressUpdate;
    }
}
