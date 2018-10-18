using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChangLab.Common
{
    public class ChangLabException : Exception
    {
        public string ID { get; private set; }
        public string ObjectID { get; set; }
        public string ParentID { get; set; }

        public ChangLabException() : this(string.Empty, string.Empty, null) { }
        public ChangLabException(string ObjectID) : this(ObjectID, string.Empty, null) { }
        public ChangLabException(string ObjectID, string Message) : this(ObjectID, Message, null) { }

        public ChangLabException(string ObjectID, string Message, Exception InnerException) : base(Message, InnerException)
        {
            this.ID = Guid.NewGuid().ToString();
            this.ObjectID = ObjectID;
        }

        public void Save(bool AutoSaveInnerExceptions = true)
        {
            this.ID = Save(this.ID, this.ObjectID, this.ParentID, this);

            if (AutoSaveInnerExceptions)
            {
                Exception ex = this; string exceptionId = this.ID;
                // Automatically save the inner exceptions
                while (ex.InnerException != null)
                {
                    exceptionId = Save(string.Empty, this.ObjectID, exceptionId, ex);
                    ex = ex.InnerException;
                }
            }
        }

        private string Save(string ID, string ObjectID, string ParentID, Exception ex)
        {
            using (LocalDatabase.DataAccess da = new LocalDatabase.DataAccess("Common.Exception_Add"))
            {
                da.AddParameter("ID", System.Data.SqlDbType.UniqueIdentifier, 16, ID, System.Data.ParameterDirection.InputOutput, true);
                da.AddParameter("ObjectID", System.Data.SqlDbType.UniqueIdentifier, ObjectID);
                da.AddParameter("Message", ex.Message);
                da.AddParameter("Source", ex.Source);
                da.AddParameter("StackTrace", ex.StackTrace);
                da.AddParameter("ParentID", System.Data.SqlDbType.UniqueIdentifier, ParentID, true);

                return da.ExecuteParameter("ID").ToString();
            }
        }
    }

    /// <summary>
    /// Treated by default as a soft exception, alerting the user to a potential problem, instead of a hard fault that fails whatever the currently
    /// running activity is.
    /// </summary>
    public class WarningException : Exception
    {
        public bool HandleAsWarning { get; set; }

        public WarningException(string Message) : base(Message) { }
    }
}
