using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChangLab.Common
{
    public class ReferenceItem
    {
        #region Properties
        public int ID { get; internal set; }
        public string Name { get; internal set; }
        public string Key { get; internal set; }
        public int Rank { get; internal set; }
        #endregion

        internal virtual string ListProcedure { get { throw new NotImplementedException(); } }
        
        public static int GetIDByKey<E>(Type ReferenceItemType, E Key) where E : struct
        {
            return (int)GetInstance(ReferenceItemType).GetIDByKey(Key);
        }

        public static E GetKeyByID<E>(Type ReferenceItemType, int ID) where E : struct
        {
            return (E)GetInstance(ReferenceItemType).GetKeyByID<E>(ID);
        }

        public static string GetNameByID(Type ReferenceItemType, int ID)
        {
            return GetInstance(ReferenceItemType).GetNameByID(ID);
        }

        public static List<T> ListAll<T>() where T : ReferenceItem
        {
            return GetInstance(typeof(T)).All;
        }

        private static dynamic GetInstance(Type ReferenceItemType)
        {
            Type constructedClass = typeof(ReferenceItemCollection<>).MakeGenericType(ReferenceItemType);
            return constructedClass.GetProperty("Instance").GetValue(null, null);
        }
    }

    public class ReferenceItemCollection<T> where T : ReferenceItem, new()
    {
        private string ListProcedure { get; set; }

        private List<T> _all;
        internal List<T> All
        {
            get
            {
                if (_all == null)
                {
                    _all = List().Cast<T>().ToList();
                }

                return _all;
            }
        }

        protected internal int GetIDByKey<E>(E Key) where E : struct
        {
            T record = All.FirstOrDefault(r => r.Key.ToUpper() == Key.ToString().ToUpper());
            if (record != null)
            {
                return record.ID;
            }
            else
            {
                return 0;
            }
        }

        protected internal E GetKeyByID<E>(int ID)
        {
            T record = All.FirstOrDefault(r => r.ID == ID);
            if (record != null)
            {
                return (E)Enum.Parse(typeof(E), record.Key);
            }
            else
            {
                throw new ArgumentOutOfRangeException("ID", "Key not found in " + typeof(E).Name + " for ID " + ID.ToString());
            }
        }

        protected internal string GetNameByID(int ID)
        {
            T record = All.FirstOrDefault(r => r.ID == ID);
            if (record != null)
            {
                return record.Name;
            }
            else
            {
                throw new ArgumentOutOfRangeException("ID", "Name not found for ID " + ID.ToString());
            }
        }

        protected internal bool TryGetIDByName(string Name, out int ID)
        {
            T record = All.FirstOrDefault(r => r.Name.ToLower() == Name.ToLower());
            if (record != null)
            {
                ID = record.ID;
                return true;
            }
            else
            {
                ID = 0;
                return false;
            }
        }

        protected internal int GetRankByID(int ID)
        {
            T record = All.FirstOrDefault(r => r.ID == ID);
            if (record != null)
            {
                return record.Rank;
            }
            else
            {
                throw new ArgumentOutOfRangeException("ID", "Rank not found for ID " + ID.ToString());
            }
        }

        private List<T> List()
        {
            List<T> list = new List<T>();
            using (LocalDatabase.DataAccess da = new LocalDatabase.DataAccess(ListProcedure))
            {
                using (System.Data.SqlClient.SqlDataReader reader = da.ExecuteReader())
                {
                    bool hasRank = Enumerable.Range(0, reader.FieldCount).Any(i => reader.GetName(i) == "Rank");
                    while (reader.Read())
                    {
                        list.Add(new T()
                        {
                            ID = (int)reader["ID"],
                            Name = (string)reader["Name"],
                            Key = (string)reader["Key"],
                            Rank = (hasRank ? (int)reader["Rank"] : 0)
                        });
                    }
                }
            }
            return list;
        }

        private static Dictionary<Type, ReferenceItemCollection<ReferenceItem>> _instances;
        public static ReferenceItemCollection<ReferenceItem> GetInstance(Type CollectionType)
        {
            if (_instances == null)
            {
                _instances = new Dictionary<Type, ReferenceItemCollection<ReferenceItem>>();
                _instances.Add(CollectionType, new ReferenceItemCollection<ReferenceItem>() { ListProcedure = Activator.CreateInstance<T>().ListProcedure });
            }

            return _instances.First(kv => kv.Key == CollectionType).Value;
        }

        private static ReferenceItemCollection<T> _instance;
        public static ReferenceItemCollection<T> Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ReferenceItemCollection<T>() { ListProcedure = Activator.CreateInstance<T>().ListProcedure };
                }

                return _instance;
            }
        }
    }
}
