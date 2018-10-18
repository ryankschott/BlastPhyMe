using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;

namespace ChangLab.LocalDatabase
{
    public class DataAccess : IDisposable
    {
        private static string ConnectionStringTemplate { get { return "Data Source=(localDB)\\MSSQLLocalDB;AttachDBFileName={0};Integrated Security=True;"; } }
        public static string ConnectionString { get; set; }

        internal static string _connectedDatabaseVersion;
        public static Version ConnectedDatabaseVersion
        {
            get
            {
                if (_connectedDatabaseVersion == string.Empty)
                {
                    try
                    {
                        Diagnostics.VerifyDatabaseConnectivity();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Could not connect to database and retrieve database version.", ex);
                    }
                }

                Version connectedDatabaseVersion = null;
                if (Version.TryParse(_connectedDatabaseVersion, out connectedDatabaseVersion))
                {
                    return connectedDatabaseVersion;
                }
                else { throw new ArgumentException("Invalid version format", "ConnectedDatabaseVersion"); }
            }
        }

        public static void SetConnectionString(string FileName)
        {
            DataAccess.ConnectionString = string.Format(DataAccess.ConnectionStringTemplate, FileName);
        }

        private SqlConnection Connection { get; set; }
        //private SqlTransaction Transaction { get; set; }
        //private string TransactionID { get; set; }
        //private bool UseTransaction { get; set; }
        private bool KeepConnectionAlive { get; set; }

        private SqlCommand Command { get; set; }
        private CommandType CommandType { get; set; }
        internal int CommandTimeout { get { return Command.CommandTimeout; } set { Command.CommandTimeout = value; } }
        internal SqlParameterCollection Parameters { get { return Command.Parameters; } }

        public DataAccess(bool KeepConnectionAlive = false) : this(string.Empty, KeepConnectionAlive) { }

        public void Dispose()
        {
            if (Command != null)
            {
                try { Command.Dispose(); Command = null; }
                finally { }
            }
            if (Connection != null)
            {
                if (Connection.State == ConnectionState.Open)
                {
                    try { Connection.Close(); Connection.Dispose(); Connection = null; }
                    finally { }
                }
            }
        }

        public DataAccess(string StoredProcedureName, bool KeepConnectionAlive = false)
        {
            this.Command = new SqlCommand()
            {
                CommandText = StoredProcedureName,
                CommandType = CommandType.StoredProcedure
            };

            this.KeepConnectionAlive = KeepConnectionAlive;
            //this.UseTransaction = UseTransaction;
            //this.TransactionID = Guid.NewGuid().ToString().Replace("-", "");
        }

        public void ChangeCommand(string StoredProcedureName)
        {
            this.Command.CommandText = StoredProcedureName;
            this.Command.Parameters.Clear();
        }

        //public void Commit()
        //{
        //    this.Transaction.Commit();
        //}

        //public void Rollback()
        //{
        //    this.Transaction.Rollback();
        //}

        #region Add Parameter
        public SqlParameter AddParameter(string Name, object Value, bool EmptyIsNull = false)
        {
            SqlDbType dbType = SqlDbType.Variant;

            if (Value == null) { throw new ArgumentNullException("Value", "Type could not be derived for the parameter value because the value given is null."); }

            if (Value.GetType() == typeof(int)) { dbType = SqlDbType.Int; }
            else if (Value.GetType() == typeof(bool))  { dbType = SqlDbType.Bit; }
            else if (Value.GetType() == typeof(string)) { dbType = SqlDbType.VarChar; }
            else if (Value.GetType() == typeof(SqlDbType) && (SqlDbType)Value == SqlDbType.Bit)
            {
                // The one issue with using the EmptyIsNull default parameter is that it's a bool, so if you pass:
                //      AddParameter("TrueFalse", SqlDbType.Bit, bTrueFalse);
                // It would route in here, taking bTrueFalse as the value for the EmptyIsNull parameter.
                // This statement will catch that occurance, but ideally just don't pass the datatype and
                // let the else-if for typeof(bool) above handle it.
                return AddParameter(Name, SqlDbType.Bit, 0, 0, 0, EmptyIsNull, ParameterDirection.Input);
            }

            if (dbType != SqlDbType.Variant)
            {
                return AddParameter(Name, dbType, 0, 0, 0, Value, ParameterDirection.Input, EmptyIsNull);
            }
            else
            {
                throw new NotImplementedException("Deriving a SqlDbType for type " + (Value != null ? Value.GetType().Name : "null") + " has not been implemented.");
            }
        }

        public SqlParameter AddParameter(string Name, SqlDbType DbType, object Value, bool EmptyIsNull = false)
        {
            return AddParameter(Name, DbType, 0, 0, 0, Value, ParameterDirection.Input, EmptyIsNull);
        }

        public SqlParameter AddParameter(string Name, SqlDbType DbType, int Size, object Value, bool EmptyIsNull = false)
        {
            return AddParameter(Name, DbType, Size, 0, 0, Value, ParameterDirection.Input, EmptyIsNull);
        }

        public SqlParameter AddParameter(string Name, SqlDbType DbType, int Size, object Value, ParameterDirection Direction, bool EmptyIsNull = false)
        {
            return AddParameter(Name, DbType, Size, 0, 0, Value, Direction, EmptyIsNull);
        }

        private SqlParameter AddParameter(string Name, SqlDbType DbType, int Size, byte Precision, byte Scale, object Value, ParameterDirection Direction, bool EmptyIsNull = false)
        {
            object value = null;
            if (Value == null)
            {
                value = DBNull.Value;
            }
            else
            {
                if (Value.GetType() == typeof(string) && DbType == SqlDbType.UniqueIdentifier)
                {
                    if (string.IsNullOrWhiteSpace((string)Value) && EmptyIsNull) { value = DBNull.Value; }
                    else
                    {
                        value = Guid.Parse((string)Value);
                    }
                }
                else if (DbType == SqlDbType.Xml && Value.GetType() == typeof(System.Xml.Linq.XDocument))
                {
                    value = ((System.Xml.Linq.XDocument)Value).ToString();
                }
                else
                {
                    if (EmptyIsNull)
                    {
                        if (Value.GetType() == typeof(string))
                        {
                            value = (string.IsNullOrWhiteSpace((string)Value) ? DBNull.Value : Value);
                        }
                        else if (Value.GetType() == typeof(int))
                        {
                            value = ((int)Value == 0 ? DBNull.Value : Value);
                        }
                        else if (Value.GetType() == typeof(DateTime))
                        {
                            value = ((DateTime)Value == DateTime.MinValue ? DBNull.Value : Value);
                        }
                        else if (Value.GetType() == typeof(TimeSpan))
                        {
                            value = ((TimeSpan)Value == TimeSpan.MinValue ? DBNull.Value : Value);
                        }
                        else if (Value.GetType() == typeof(Guid))
                        {
                            value = ((Guid)Value == Guid.Empty ? DBNull.Value : Value);
                        }
                        else if (Value.GetType() == typeof(short))
                        {
                            value = ((short)Value == 0 ? DBNull.Value : Value);
                        }
                        else if (Value.GetType() == typeof(byte))
                        {
                            value = ((byte)Value == 0 ? DBNull.Value : Value);
                        }
                        else if (Value.GetType() == typeof(double))
                        {
                            value = ((double)Value == 0 ? DBNull.Value : Value);
                        }
                        else
                        {
                            value = Value;
                        }
                    }
                    else { value = Value; }
                }
            }

            SqlParameter parameter = new SqlParameter(Name, value)
            {
                SqlDbType = DbType,
                Size = (Size == 0 ? DefaultSize(DbType) : Size),
                Direction = Direction
            };
            if (Precision != 0 && Scale != 0)
            {
                parameter.Precision = Precision;
                parameter.Scale = Scale;
                if (Size == 0)
                {
                    switch (DbType)
                    {
                        case SqlDbType.Decimal:
                            if (Precision <= 9)
                            { parameter.Size = 5; }
                            else if (Precision <= 19)
                            { parameter.Size = 9; }
                            else if (Precision <= 28)
                            { parameter.Size = 13; }
                            else if (Precision <= 38)
                            { parameter.Size = 17; }
                            break;
                    }
                }
            }

            Command.Parameters.Add(parameter);
            return parameter;
        }

        private int DefaultSize(SqlDbType DbType)
        {
            switch (DbType)
            {
                case SqlDbType.Int:
                    return 8;
                case SqlDbType.UniqueIdentifier:
                    return 16;
                case SqlDbType.DateTime2:
                    return 8;
                case SqlDbType.SmallInt:
                    return 4;
                case SqlDbType.TinyInt:
                    return 2;
                case SqlDbType.Bit:
                    return 1;
                case SqlDbType.VarChar:
                    return -1;
                case SqlDbType.Xml:
                    return -1;
                default:
                    return 0;
            }
        }

        /// <remarks>
        /// A helper version, to accomodate the stored procedures that send back as an output parameter the ID for a newly created record.
        /// Technically it's an InputOutput parameter, but I don't use strictly Output parameters anywhere.
        /// </remarks>
        public SqlParameter AddOutputParameter(string Name, int Value)
        {
            return AddParameter(Name, SqlDbType.Int, 0, 0, 0, Value, ParameterDirection.InputOutput, true);
        }

        /// <remarks>
        /// A special case because it's not often used and adding the Precision parameter up in the overloads above makes a mess of things.
        /// </remarks>
        public SqlParameter AddDoubleParameter(string Name, byte Precision, byte Scale, double Value, bool EmptyIsNull = false)
        {
            return AddParameter(Name, SqlDbType.Decimal, 0, Precision, Scale, Value, ParameterDirection.Input, EmptyIsNull);
        }

        public SqlParameter AddListParameter(string Name, IEnumerable<int> Values)
        {
            IEnumerable<SqlDataRecord> table = null;
            if (Values.Count() != 0)
            {
                SqlMetaData[] meta = new SqlMetaData[1];
                meta[0] = new SqlMetaData("Value", SqlDbType.Int);

                table = Values.Select(v =>
                {
                    SqlDataRecord record = new SqlDataRecord(meta);
                    record.SetInt32(0, v);
                    return record;
                });
            }

            SqlParameter parameter = new SqlParameter(Name, table)
            {
                SqlDbType = System.Data.SqlDbType.Structured,
                TypeName = "Common.ListInt",
                Direction = ParameterDirection.Input,
            };

            Command.Parameters.Add(parameter);
            return parameter;
        }

        public SqlParameter AddListParameter(string Name, IEnumerable<Guid> Values)
        {
            SqlMetaData[] meta = new SqlMetaData[1];
            meta[0] = new SqlMetaData("Value", SqlDbType.UniqueIdentifier);

            IEnumerable<SqlDataRecord> table = Values.Select(v =>
            {
                SqlDataRecord record = new SqlDataRecord(meta);
                record.SetGuid(0, v);
                return record;
            });

            SqlParameter parameter = new SqlParameter(Name, table)
            {
                SqlDbType = System.Data.SqlDbType.Structured,
                TypeName = "Common.ListUniqueIdentifier",
                Direction = ParameterDirection.Input
            };

            Command.Parameters.Add(parameter);
            return parameter;
        }

        public SqlParameter AddListParameter(string Name, IEnumerable<string> Values, ListParameterTypes ListParameterType)
        {
            SqlMetaData[] meta = new SqlMetaData[1];
            string typeName = string.Empty;
            switch (ListParameterType)
            {
                case ListParameterTypes.ListVarCharIndentifier:
                    meta[0] = new SqlMetaData("Value", SqlDbType.VarChar, 36);
                    typeName = "Common.ListVarCharIdentifier";
                    break;
            }

            IEnumerable<SqlDataRecord> table = Values.Select(v =>
            {
                SqlDataRecord record = new SqlDataRecord(meta);
                record.SetString(0, v);
                return record;
            });

            SqlParameter parameter = new SqlParameter(Name, table)
            {
                SqlDbType = System.Data.SqlDbType.Structured,
                TypeName = typeName,
                Direction = ParameterDirection.Input
            };

            Command.Parameters.Add(parameter);
            return parameter;
        }

        public enum ListParameterTypes
        {
            ListInt,
            ListUniqueIdentifier,
            ListVarCharIndentifier,
            ListVarChar8000,
            ListVarCharMax
        }

        public SqlParameter AddDictionaryParameter(string Name, Dictionary<Guid, int> Values)
        {
            SqlMetaData[] meta = new SqlMetaData[2];
            meta[0] = new SqlMetaData("Key", SqlDbType.UniqueIdentifier);
            meta[1] = new SqlMetaData("Value", SqlDbType.Int);

            IEnumerable<SqlDataRecord> table = Values.Select(v =>
            {
                SqlDataRecord record = new SqlDataRecord(meta);
                record.SetGuid(0, v.Key);
                record.SetInt32(1, v.Value);
                return record;
            });

            SqlParameter parameter = new SqlParameter(Name, table)
            {
                SqlDbType = System.Data.SqlDbType.Structured,
                TypeName = "Common.DictionaryUniqueIdentifierInt",
                Direction = ParameterDirection.Input
            };

            Command.Parameters.Add(parameter);
            return parameter;
        }
        #endregion

        #region Execute
        private ExecuteModes _executeMode;
        private string _outputParameterName;

        public int ExecuteCommand()
        {
            _executeMode = ExecuteModes.NonQuery;
            return (int)Execute();
        }

        public object ExecuteParameter(string ParameterName)
        {
            _executeMode = ExecuteModes.Parameter;
            _outputParameterName = ParameterName;
            return Execute();
        }

        public DataTable ExecuteDataTable()
        {
            _executeMode = ExecuteModes.DataTable;
            return (DataTable)Execute();
        }

        public DataSet ExecuteDataSet()
        {
            _executeMode = ExecuteModes.DataSet;
            return (DataSet)Execute();
        }

        public SqlDataReader ExecuteReader()
        {
            _executeMode = ExecuteModes.DataReader;
            return (SqlDataReader)Execute();
        }

        private object Execute()
        {
            // We're not using a using block because otherwise the KeepConnectionAlive logic would be overwridden by the Dispose() behavior.
            if (Connection == null) { Connection = new SqlConnection(ConnectionString); }
            try
            {
                Command.Connection = Connection;
                if (Connection.State != ConnectionState.Open) { Connection.Open(); }

                //if (this.UseTransaction)
                //{
                //    if (this.Transaction == null)
                //    {
                //        this.Transaction = Connection.BeginTransaction(this.TransactionID);
                //        this.KeepConnectionAlive = true;
                //        this.Command.Transaction = this.Transaction;
                //    }
                //}

                switch (_executeMode)
                {
                    case ExecuteModes.NonQuery:
                        return Command.ExecuteNonQuery();
                    case ExecuteModes.Parameter:
                        Command.ExecuteNonQuery();
                        return Command.Parameters[_outputParameterName].Value;
                    case ExecuteModes.DataReader:
                        this.KeepConnectionAlive = true;
                        return Command.ExecuteReader();
                    case ExecuteModes.DataSet:
                    case ExecuteModes.DataTable:
                    default:
                        using (SqlDataAdapter da = new SqlDataAdapter(Command))
                        {
                            switch (_executeMode)
                            {
                                case ExecuteModes.DataSet:
                                    DataSet ds = new DataSet();
                                    da.Fill(ds);
                                    return ds;
                                case ExecuteModes.DataTable:
                                    DataTable dt = new DataTable();
                                    da.Fill(dt);
                                    return dt;
                            }
                        }
                        return null;
                }
            }
            catch (Exception ex)
            {
                //try
                //{
                //    if (this.Transaction != null) { this.Transaction.Rollback(); }
                //}
                //catch { }
                
                throw ex;
            }
            finally
            {
                if (!KeepConnectionAlive)
                {
                    if (Connection != null && Connection.State == ConnectionState.Open)
                    {
                        try { Connection.Close(); Connection.Dispose(); Connection = null; }
                        catch { }
                    }
                }
            }
        }

        private enum ExecuteModes
        {
            NonQuery,
            Parameter,
            ReturnValue,
            DataReader,
            DataTable,
            DataSet
        }
        #endregion
    }
}
