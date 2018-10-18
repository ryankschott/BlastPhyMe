using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChangLab.Common
{
    /// <remarks>
    /// Credit for restricting E to enums: http://stackoverflow.com/questions/6438352/using-enum-as-generic-type-parameter-in-c-sharp
    /// </remarks>
    public class ReferenceItem2<E> where E : struct
    {
        #region Properties
        private int _id;
        public int ID
        {
            get { return _id; }
            set { Sync(value); }
        }

        private E _key;
        public E Key
        {
            get { return _key; }
            set { Sync(value); }
        }

        public string Name { get; internal set; }
        public string ShortName { get; internal set; }
        public int Rank { get; internal set; }

        /// <summary>
        /// Returns a deep copy of the current instance.
        /// </summary>
        public ReferenceItem2<E> Copy()
        {
            return (ReferenceItem2<E>)this.MemberwiseClone();
        }
        #endregion

        internal ReferenceItem2()
        {
            if (!typeof(E).IsEnum) { throw new NotSupportedException("Generic type must be an enumeration."); }
        }

        public ReferenceItem2(int ID)
            : this()
        {
            Sync(ID);
        }

        public ReferenceItem2(E Key)
            : this()
        {
            Sync(Key);
        }

        internal ReferenceItem2(int ID, E Key, string Name, string ShortName, int Rank) : this()
        {
            this._id = ID;
            this._key = Key;
            this.Name = Name;
            this.ShortName = ShortName;
            this.Rank = Rank;
        }

        internal static ReferenceItem2<E> FromReaderRow(Dictionary<string, object> Row, bool HasRank, bool HasShortName)
        {
            return new ReferenceItem2<E>()
            {
                _id = (int)Row["ID"],
                _key = (E)Enum.Parse(typeof(E), (string)Row["Key"]),
                Name = (string)Row["Name"],
                ShortName = (HasShortName ? (string)Row["ShortName"] : string.Empty),
                Rank = (HasRank ? (int)Row["Rank"] : 0)
            };
        }

        private void Sync(int ID)
        {
            Sync(ReferenceItemCollection2<E>.Instance.GetByID(ID));
        }

        private void Sync(E Key)
        {
            Sync(ReferenceItemCollection2<E>.Instance.GetByKey(Key));
        }

        private void Sync(ReferenceItem2<E> Source)
        {
            if (Source != null)
            {
                this._id = Source.ID;
                this._key = Source.Key;
                this.Name = Source.Name;
                this.Key = Source.Key;
                this.Rank = Source.Rank;
            }
            else { throw new ArgumentOutOfRangeException("ID", "Invalid ID or Key for " + typeof(E).Name); }
        }

        public override string ToString()
        {
            return this.Name;
        }

        public static bool operator ==(ReferenceItem2<E> object1, E object2)
        {
            if (object1 == null) { return false; }

            return object1.Key.ToString() == object2.ToString();
        }

        public static bool operator !=(ReferenceItem2<E> object1, E object2)
        {
            if (object1 == null) { return false; }

            return object1.Key.ToString() != object2.ToString();
        }
    }

    public class ReferenceItemAttribute : Attribute
    {
        public string ListProcedure { get; set; }
    }

    public class ReferenceItemCollection2<E> where E : struct
    {
        internal string ListProcedure
        {
            get
            {
                Attribute attr = System.Attribute.GetCustomAttribute(typeof(E), typeof(ReferenceItemAttribute));
                if (attr != null)
                {
                    return ((ReferenceItemAttribute)attr).ListProcedure;
                }
                else
                {
                    throw new NotImplementedException("Reference item enumeration has not been configured with a List procedure.");
                }
            }
        }

        private static ReferenceItemCollection2<E> _instance;
        public static ReferenceItemCollection2<E> Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ReferenceItemCollection2<E>();
                }

                return _instance;
            }
        }

        private List<ReferenceItem2<E>> _all;
        internal List<ReferenceItem2<E>> All
        {
            get
            {
                if (_all == null)
                {
                    _all = List().Cast<ReferenceItem2<E>>().ToList();
                }

                return _all;
            }
        }

        internal ReferenceItem2<E> GetByID(int ID)
        {
            return All.FirstOrDefault(r => r.ID == ID);
        }

        internal ReferenceItem2<E> GetByKey(E Key)
        {
            return All.FirstOrDefault(r => r.Key.ToString() == Key.ToString());
        }

        private int GetIDByKey(E Key)
        {
            ReferenceItem2<E> record = All.FirstOrDefault(r => r.Key.ToString() == Key.ToString());
            if (record != null)
            {
                return record.ID;
            }
            else
            {
                return 0;
            }
        }

        private E GetKeyByID(int ID)
        {
            ReferenceItem2<E> record = All.FirstOrDefault(r => r.ID == ID);
            if (record != null)
            {
                return record.Key;
            }
            else
            {
                throw new ArgumentOutOfRangeException("ID", "Key not found in " + typeof(E).Name + " for ID " + ID.ToString());
            }
        }

        private string GetNameByID(int ID)
        {
            ReferenceItem2<E> record = All.FirstOrDefault(r => r.ID == ID);
            if (record != null)
            {
                return record.Name;
            }
            else
            {
                throw new ArgumentOutOfRangeException("ID", "Name not found for ID " + ID.ToString());
            }
        }

        private string GetNameByKey(E Key)
        {
            ReferenceItem2<E> record = All.FirstOrDefault(r => r.Key.ToString() == Key.ToString());
            if (record != null)
            {
                return record.Name;
            }
            else
            {
                throw new ArgumentOutOfRangeException("Key", "Name not found for key " + Key.ToString());
            }
        }

        private int GetRankByID(int ID)
        {
            ReferenceItem2<E> record = All.FirstOrDefault(r => r.ID == ID);
            if (record != null)
            {
                return record.Rank;
            }
            else
            {
                throw new ArgumentOutOfRangeException("ID", "Rank not found for ID " + ID.ToString());
            }
        }

        private List<ReferenceItem2<E>> List()
        {
            List<ReferenceItem2<E>> list = new List<ReferenceItem2<E>>();
            using (LocalDatabase.DataAccess da = new LocalDatabase.DataAccess(ListProcedure))
            {
                using (System.Data.SqlClient.SqlDataReader reader = da.ExecuteReader())
                {
                    Dictionary<int, string> fields = Enumerable.Range(0, reader.FieldCount).Select(i => new KeyValuePair<int, string>(i, reader.GetName(i))).ToDictionary(kv => kv.Key, kv => kv.Value);
                    bool hasRank = fields.Any(kv => kv.Value == "Rank");
                    bool hasShortName = fields.Any(kv => kv.Value == "ShortName");

                    while (reader.Read())
                    {
                        Dictionary<string, object> row = fields.Select(f => new KeyValuePair<string, object>(f.Value, reader.GetValue(f.Key))).ToDictionary(kv => kv.Key, kv => kv.Value);
                        list.Add(ReferenceItem2<E>.FromReaderRow(row, hasRank, hasShortName));
                    }
                }
            }
            return list;
        }

        #region Static
        // Static counterparts for the methods that derive properties of the reference item

        public static ReferenceItem2<E> Get(int ID)
        {
            return Instance.GetByID(ID);
        }

        public static ReferenceItem2<E> Get(E Key)
        {
            return Instance.GetByKey(Key);
        }

        public static int IDByKey(E Key)
        {
            return Instance.GetIDByKey(Key);
        }

        public static E KeyByID(int ID)
        {
            return Instance.GetKeyByID(ID);
        }

        public static string NameByID(int ID)
        {
            return Instance.GetNameByID(ID);
        }

        public static string NameByKey(E Key)
        {
            return Instance.GetNameByKey(Key);
        }

        public static int RankByID(int ID)
        {
            return Instance.GetRankByID(ID);
        }

        public static List<ReferenceItem2<E>> ListAll()
        {
            return Instance.All;
        }

        public static int Count
        {
            get { return Instance.All.Count; }
        }
        #endregion
    }
}
