using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using Bio.Web.Blast;

namespace ChangLab.Common
{
    public static class ExtensionMethods
    {
        public static int SequenceIdentityMatchPercentage(this Hit hit)
        {
            try
            {
                if (hit.Hsps.Count > 0)
                {
                    return Convert.ToInt32((Math.Round((Convert.ToDouble(hit.Hsps[0].IdentitiesCount) / Convert.ToDouble(hit.Hsps[0].AlignmentLength)), 2) * 100));
                }
                else { return 0; }
            }
            catch
            {
                return 0;
            }
        }

        public static string ExtractGIFromID(this Hit hit)
        {
            try
            {
                return hit.Id.Split(new string[] { "|" }, StringSplitOptions.None)[1];
            }
            catch
            {
                return hit.Id;
            }
        }

        public static string ExtractAccessionFromID(this Hit hit)
        {
            try
            {
                //gi|755756130|ref|XM_003990180.3|

                List<string> parts = hit.Id.Split(new string[] { "|" }, StringSplitOptions.None).ToList();
                if (parts.First().ToLower() == "gi") { return parts[3]; }
                else { return parts[1]; }
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string ToStandardDateString(this DateTime Value)
        {
            return Value.ToString("yyyy-MM-dd");
        }

        public static string ToStandardFriendlyDateString(this DateTime Value)
        {
            return Value.ToString("MMMM d, yyyy");
        }

        public static string ToStandardDateTimeString(this DateTime Value)
        {
            return Value.ToString("yyyy-MM-dd hh:mm");
        }

        public static string ToStandardTimeString(this DateTime Value)
        {
            return Value.ToString("hh:mm:ss");
        }

        public static string ToStandardTimeStringWithDateIfNotToday(this DateTime Value)
        {
            if (DateTime.Now.Date != Value.Date)
            {
                return Value.ToString("yyyy-MM-dd hh:mm:ss");
            }
            else
            {
                return Value.ToStandardTimeString();
            }
        }

        public static char ToComplement(this Char Value)
        {
            switch (Value)
            {
                case 'A': return 'T';
                case 'T': return 'A';
                case 'C': return 'G';
                case 'G': return 'C';
                default: return Value;
            }
        }

        public static string ToString(this Version Value, VersionDepth Depth)
        {
            switch (Depth)
            {
                case VersionDepth.Major: return Value.Major.ToString();
                case VersionDepth.Minor: return Value.Major.ToString() + "." + Value.Minor.ToString();
                case VersionDepth.Build: return Value.Major.ToString() + "." + Value.Minor.ToString() + "." + Value.Build.ToString();
                case VersionDepth.Revision: return Value.Major.ToString() + "." + Value.Minor.ToString() + "." + Value.Build.ToString() + "." + Value.Revision.ToString();
            }
            return string.Empty;
        }

        #region Collections
        public static string AsString(this IEnumerable<Char> Value)
        {
            return Value.Aggregate(string.Empty, (current, c) => current += c);
        }

        public static string AggregateMessages(this List<Jobs.JobException> Value, string Delimeter)
        {
            return Value.Select(ex => ex.Message).Distinct().Aggregate(string.Empty, (current, message) => current += (current.Length == 0 ? "" : Delimeter) + message);
        }

        public static string AggregateInnerMessages(this List<Jobs.JobException> Value, string Delimeter)
        {
            return Value.Select(ex => {
                if (ex.InnerException != null) { return ex.InnerException.Message; }
                else { return ex.Message; }
            }).Distinct().Aggregate(string.Empty, (current, message) => current += (current.Length == 0 ? "" : Delimeter) + message);
        }

        public static RecordSets.SubSet ElementByID(this List<RecordSets.SubSet> Value, string ID)
        {
            return Value.FirstOrDefault(sub => GuidCompare.Equals(sub.ID, ID));
        }

        public static string Concatenate(this List<string> Value, string Delimeter)
        {
            return Value.Aggregate(string.Empty, (current, message) => current += (current.Length == 0 ? "" : Delimeter) + message);
        }

        public static string Concatenate<T>(this IEnumerable<T> Value, string Delimeter)
        {
            return Value.Aggregate(string.Empty, (current, message) => current += (current.Length == 0 ? "" : Delimeter) + message.ToString());
        }

        /// <summary>
        /// Returns true if the collection contains only one item and that item matches the Value parameter.
        /// </summary>
        public static bool Only<T>(this IEnumerable<T> Collection, T Value)
        {
            return (Collection.Count() == 1) && object.Equals(Collection.ElementAt(0), Value);
        }

        public static string Mid(this string Value, int Start, int End)
        {
            return Value.Select((c, i) => new { Char = c, Index = i }).Skip(Start).TakeWhile(c => c.Index < End).Aggregate(string.Empty, (current, s) => current += s.Char);
        }

        public static string[] SplitByEmptySpace(this string Value)
        {
            return Value.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static string[] SeparateFriendlyCase(this string[] Values)
        {
            return Values.Select(s => s.SeparateFriendlyCase()).ToArray();
        }

        public static Dictionary<T, string> ToDictionary<T>()
        {
            return
            Enum.GetNames(typeof(T))
                .Select((name, index) => new { Name = name, Index = index })
                .Join(Enum.GetValues(typeof(T)).Cast<T>()
                        .Select((value, index) => new { Value = value, Index = index }),
                        (n => n.Index),
                        (v => v.Index),
                        (n, v) => new KeyValuePair<T, string>(v.Value, n.Name))
                .ToDictionary(kv => kv.Key, kv => kv.Value);
        }
        #endregion

        #region DataTable and DataReader
        public static string ToCSV(this DataTable Table)
        {
            string csvOutputContents = string.Empty;
            // Header row with column names
            foreach (DataColumn column in Table.Columns)
            {
                csvOutputContents += (string.IsNullOrWhiteSpace(csvOutputContents) ? string.Empty : ",") + column.ColumnName;
            }
            // Data rows
            foreach (DataRow row in Table.Rows)
            {
                csvOutputContents += "\r\n";

                for (int i = 0; i < Table.Columns.Count; i++)
                {
                    if (i > 0) { csvOutputContents += ","; }
                    csvOutputContents += "\"" + (DBNull.Value.Equals(row[i]) ? string.Empty : (string)row[i]) + "\"";
                }
            }

            return csvOutputContents;
        }

        public static string ToSafeString(this DataRow Row, int Index)
        {
            return (Row[Index] == DBNull.Value ? string.Empty : Row[Index].ToString());
        }

        public static string ToSafeString(this DataRow Row, string ColumnName)
        {
            return (!Row.Table.Columns.Contains(ColumnName) || Row[ColumnName] == DBNull.Value ? string.Empty : Row[ColumnName].ToString());
        }

        public static bool ToSafeBoolean(this DataRow Row, string ColumnName, bool Default = true)
        {
            return (!Row.Table.Columns.Contains(ColumnName) || Row[ColumnName] == DBNull.Value ? Default : (bool)Row[ColumnName]);
        }

        public static int ToSafeInt(this DataRow Row, string ColumnName)
        {
            return (!Row.Table.Columns.Contains(ColumnName) || Row[ColumnName] == DBNull.Value ? 0 : (int)Row[ColumnName]);
        }

        public static DateTime ToSafeDateTime(this DataRow Value, string ColumnName)
        {
            return (Value == null || !Value.Table.Columns.Contains(ColumnName) || Value[ColumnName] == DBNull.Value ? DateTime.MinValue : (DateTime)Value[ColumnName]);
        }

        public static double ToSafeDouble(this DataRow Row, string ColumnName)
        {
            return (!Row.Table.Columns.Contains(ColumnName) || Row[ColumnName] == DBNull.Value 
                    ? 0 
                    : (Row[ColumnName].GetType() == typeof(decimal)
                        ? Decimal.ToDouble((decimal)Row[ColumnName])
                        : (double)Row[ColumnName]));
        }

        public static string ToSafeString(this System.Data.SqlClient.SqlDataReader Reader, string ColumnName)
        {
            return (Reader[ColumnName] == DBNull.Value ? string.Empty : Reader[ColumnName].ToString());
        }

        public static int ToSafeInt(this System.Data.SqlClient.SqlDataReader Reader, string ColumnName)
        {
            return (Reader[ColumnName] == DBNull.Value ? 0 : (int)Reader[ColumnName]);
        }

        public static double ToSafeDouble(this System.Data.SqlClient.SqlDataReader Reader, string ColumnName, double Default = 0)
        {
            return (Reader[ColumnName] == DBNull.Value 
                    ? Default
                    : (Reader[ColumnName].GetType() == typeof(decimal) 
                        ? Decimal.ToDouble((decimal)Reader[ColumnName]) 
                        : (double)Reader[ColumnName]));
        }

        public static Dictionary<string, string> ToAdditionalPropertiesList(this DataRow Row, string ColumnName)
        {
            if (Row[ColumnName] == DBNull.Value) { return null; }
            else
            {
                try
                {
                    XDocument xdoc = XDocument.Parse(Row[ColumnName].ToString());
                    return xdoc.Document.Root.Elements().Select(xel => new KeyValuePair<string, string>(xel.Elements("Name").First().Value, xel.Elements("Value").First().Value)).ToDictionary(kv => kv.Key, kv => kv.Value);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        #endregion

        #region Genes
        public static string LengthDescription(this Genes.Gene Value)
        {
            return LengthDescription(Value, Value.Features, Value.SourceSequence);
        }

        public static string LengthDescription(this Genes.Gene Value, List<Genes.Feature> Features, Genes.NucleotideSequence SourceSequence)
        {
            if (Features.Any(f => f.Intervals.Count != 0) && SourceSequence != null)
            {
                Genes.Feature topRankedFeature = Features.Where(f => f.Intervals.Count != 0).OrderBy(f => f.FeatureKey.Rank).First();
                return SourceSequence.ToString().Length.ToString("N0")
                        + (topRankedFeature.FeatureKey != Genes.GeneFeatureKeys.source
                            ? " ("
                                + topRankedFeature.FeatureKey.Name
                                + ": "
                                + Features
                                    .Where(f => f.FeatureKey.ID == topRankedFeature.FeatureKey.ID)
                                    .Aggregate(0, (current, f) => current += f.Intervals.Aggregate(0, (intervalLength, i) => intervalLength += i.Length))
                                    .ToString("N0")
                                + ")"
                            : string.Empty);
            }
            else
            {
                int length = 0;

                if (SourceSequence != null)
                { length = SourceSequence.Length; }
                else if (!string.IsNullOrWhiteSpace(Value.Nucleotides))
                {  length = Value.Nucleotides.Length; }
                else
                { length = Value.SequenceRange.Length; }

                return length.ToString("N0");
            }
        }

        public static string ToFASTAHeader(this Genes.Gene Value, string FormatString)
        {
            return FormatString
                .Replace("{Gene}", Value.GeneName)
                .Replace("{Definition}", Value.Definition)
                .Replace("{Source}", Genes.GeneSource.NameByID(Value.SourceID))
                .Replace("{GenBank ID}", Value.GenBankID.ToSafeString())

                .Replace("{Locus}", Value.Locus)
                .Replace("{Accession}", Value.Accession)
                .Replace("{Organism}", Value.Organism);
        }
        #endregion

        #region IO
        public static string ToSafeFileName(this string FilePath)
        {
            return Path.GetInvalidFileNameChars().Aggregate(FilePath, (current, c) => current.Replace(c.ToString(), string.Empty));
        }

        public static bool CanReadWrite(this string FilePath)
        {
            using (FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.ReadWrite))
            {
                return (fs.CanRead && fs.CanWrite);
            }
        }

        public static DirectoryInfo CreateSafeDirectory(this DirectoryInfo Parent, string Name)
        {
            int directoryCopy = 1;
            string name = Name.ToSafeFileName();
            string newPath = Parent.FullName + "\\" + name;
            while (Directory.Exists(newPath))
            {
                directoryCopy++; // Copy number starts at 2, indicating that the first one (the one with the same name) already exists.
                newPath = Parent.FullName + "\\" + name + " - (" + directoryCopy.ToString() + ")";
            }
            return Directory.CreateDirectory(newPath);
        }

        public static string FileNameFromPath(this string FilePath)
        {
            return FilePath.Substring(FilePath.LastIndexOf("\\") + 1);
        }
        #endregion

        #region Primitive Types
        #region string
        public static string Replicate(this string Value, string Replicon, int Count)
        {
            for (int i = 0; i < Count; i++)
            {
                Value += Replicon;
            }

            return Value;
        }

        public static string ToSafeString(this object Value)
        {
            return (Value == null || Value == DBNull.Value ? string.Empty : Value.ToString());
        }

        public static string NumberWithSuffix(this int Value)
        {
            string suffix = "th";
            switch (Value)
            {
                case 1: suffix = "st"; break;
                case 2: suffix = "nd"; break;
                case 3: suffix = "rd"; break;
            }

            return Value.ToString() + suffix;
        }

        public static string SeparateFriendlyCase(this object Value)
        {
            string value = Value.ToString();
            Match match = Regex.Match(value, "[a-z]{1}[A-Z]{1}");
            while (match.Success)
            {
                value = value.Insert(match.Index + 1, " ");
                match = Regex.Match(value, "[a-z]{1}[A-Z]{1}");
            }
            return value;
        }

        public static string SafeSubstring(this string Value, int Index, int Count)
        {
            if ((Index + Count) > Value.Length) { Count = Value.Length - Index; }
            return Value.Substring(Index, Count);
        }
        #endregion

        public static int ToSafeInt(this object Value)
        {
            return (Value == null || Value == DBNull.Value || Value.GetType() != typeof(int) ? 0 : (int)Value);
        }

        public static int ToSafeInt(this string Value, int Default = 0)
        {
            int tryParse = Default;
            // If TryParse() fails, tryParse will still equal Default
            int.TryParse(Value, out tryParse);
            return tryParse;
        }
        
        public static bool ToSafeBoolean(this object Value, bool Default = false)
        {
            if (Value == null || Value == DBNull.Value) { return Default; }
            else
            {
                bool value = Default;
                bool.TryParse(Value.ToString(), out value);
                return value;
            }
        }

        public static object SafeValue<TKey, TValue>(this Dictionary<TKey, TValue> Source, TKey Key, object Default = null)
        {
            if (Source.ContainsKey(Key))
            {
                return Source[Key];
            }
            else
            {
                return Default;
            }
        }
        
        public static int ToInt(this long Value)
        {
            return Convert.ToInt32(Value);
        }
        
        public static string[] ToStringArray(this object[] Value)
        {
            List<string> output = new List<string>();
            Value.ToList().ForEach(o => output.Add(o.ToString()));
            return output.ToArray();
        }

        public static double ExtractDouble(this string Value, double Default = 0.0D)
        {
            double tryParse = Default;
            // If TryParse() fails, tryParse will still equal Default
            double.TryParse(System.Text.RegularExpressions.Regex.Replace(Value, "[^0-9\\.]", ""), out tryParse);
            return tryParse;
        }
        #endregion

        #region Reflection
        public static string GetAssemblyAttribute<T>(this Assembly assembly, Func<T, string> value) where T : Attribute
        {
            T attribute = (T)Attribute.GetCustomAttribute(assembly, typeof(T));
            return value.Invoke(attribute);
        }
        #endregion

        #region XML
        public static string SafeInnerText(this XmlNode Node)
        {
            return (Node == null ? string.Empty : Node.InnerText);
        }

        public static string SafeChildNodeInnerText(this XmlNode Node, string ChildNodePath)
        {
            if (Node != null)
            {
                XmlNode match = Node.SelectSingleNode(ChildNodePath);
                if (match != null)
                {
                    return match.InnerText;
                }
            }

            return string.Empty;
        }

        #region Attribute Values
        public static string SafeAttributeValue(this XmlNode Node, string AttributeName)
        {
            if (Node == null || !Node.Attributes.Cast<XmlAttribute>().Any(att => att.Name == AttributeName))
            { return string.Empty; }
            else
            {
                return Node.Attributes[AttributeName].Value;
            }
        }

        public static string SafeAttributeValue(this XElement Element, string AttributeName, string Default = "")
        {
            if (Element == null || Element.Attribute(AttributeName) == null)
            { return Default; }
            else
            { return Element.Attribute(AttributeName).Value; }
        }
        
        public static E SafeAttributeValueAsEnum<E>(this XElement Element, string Key, E Default)
        {
            return (E)Enum.Parse(typeof(E), Element.SafeAttributeValue(Key, Default.ToString()));
        }

        public static bool SafeAttributeValueAsBool(this XElement Element, string Key, bool Default = false)
        {
            bool value = false;
            if (bool.TryParse(Element.SafeAttributeValue(Key), out value))
            { return value; }
            else
            { return Default; }
        }

        public static int SafeAttributeValueAsInt(this XElement Element, string Key, int Default = 0)
        {
            int value = 0;
            if (int.TryParse(Element.SafeAttributeValue(Key), out value))
            { return value; }
            else
            { return Default; }
        }

        public static float SafeAttributeValueAsFloat(this XElement Element, string Key, float Default = 0.0F)
        {
            float value = 0.0F;
            if (float.TryParse(Element.SafeAttributeValue(Key), out value))
            { return value; }
            else
            { return Default; }
        }

        public static string SafeChildNodeAttributeValue(this XmlNode Node, string ChildNodePath, string AttributeName)
        {
            return (Node == null ? string.Empty : Node.SelectSingleNode(ChildNodePath).SafeAttributeValue(AttributeName));
        }
        
        public static double SafeAttributeValueAsDouble(this XElement Element, string Key, double Default = 0D)
        {
            double value = 0D;
            if (double.TryParse(Element.SafeAttributeValue(Key), out value))
            { return value; }
            else
            { return Default; }
        }
        #endregion

        public static DateTime ToDateTime(this XElement Element)
        {
            DateTime value = DateTime.MinValue;
            if (Element != null)
            {
                // If this fails, value won't have been changed, so we'll still end up returning DateTime.MinValue.
                DateTime.TryParse(Element.Value, out value);
            }

            return value;
        }

        public static string ElementToString(this XElement ParentElement, string ElementName)
        {
            string value = string.Empty;

            if (ParentElement != null && ParentElement.Elements(ElementName).Count() != 0)
            {
                value = ParentElement.Element(ElementName).Value;
            }

            return value;
        }

        public static int ElementToInt(this XElement ParentElement, string ElementName)
        {
            int value = 0;

            if (ParentElement != null && ParentElement.Elements(ElementName).Count() != 0)
            {
                int.TryParse(ParentElement.Element(ElementName).Value, out value);
            }

            return value;
        }

        public static DateTime ElementToDateTime(this XElement ParentElement, string ElementName)
        {
            DateTime value = DateTime.MinValue;

            if (ParentElement != null && ParentElement.Elements(ElementName).Count() != 0)
            {
                DateTime.TryParse(ParentElement.Element(ElementName).Value, out value);
            }

            return value;
        }

        public static double ElementToDouble(this XElement ParentElement, string ElementName)
        {
            double value = 0;

            if (ParentElement != null && ParentElement.Elements(ElementName).Count() != 0)
            {
                double.TryParse(ParentElement.Element(ElementName).Value, out value);
            }

            return value;   
        }
        #endregion
    }

    public class GuidCompare
    {
        public static bool Equals(string Guid1, string Guid2)
        {
            if (string.IsNullOrWhiteSpace(Guid1) || string.IsNullOrWhiteSpace(Guid2)) { return false; } /* One or both are not Guids, and this is a Guid equality test, not a string equality test. */
            return (Guid.Parse(Guid1) == Guid.Parse(Guid2));
        }
    }

    public enum VersionDepth
    {
        Major,
        Minor,
        Build,
        Revision
    }
}