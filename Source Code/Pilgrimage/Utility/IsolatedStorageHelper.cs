using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;

namespace Pilgrimage.IO
{
    public class IsolatedStorageHelper
    {
        private static IsolatedStorageFile GetStore() { return IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly, null, null); }
        
        public static bool Exists(string RelativeFilePath)
        {
            using (IsolatedStorageFile isoStore = GetStore())
            {
                return isoStore.FileExists(RelativeFilePath);
            }
        }

        public static List<string> ReadAllLines(string RelativeFilePath)
        {
            using (IsolatedStorageFile isoStore = GetStore())
            {
                using (IsolatedStorageFileStream fs = isoStore.OpenFile(RelativeFilePath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(fs))
                    {
                        List<string> lines = new List<string>();
                        while (!reader.EndOfStream)
                        {
                            lines.Add(reader.ReadLine());
                        }
                        return lines;
                    }
                }
            }
        }

        public static void WriteAllLines(string RelativeFilePath, List<string> Lines)
        {
            using (IsolatedStorageFile isoStore = GetStore())
            {
                using (IsolatedStorageFileStream fs = isoStore.OpenFile(RelativeFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    using (StreamWriter writer = new StreamWriter(fs))
                    {
                        Lines.ForEach(s => writer.WriteLine(s));
                    }
                }
            }
        }
    }
}
