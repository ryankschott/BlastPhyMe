using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using ChangLab.Common;
using ChangLab.Genes;
using ChangLab.Jobs;
using Pilgrimage.Activities;

namespace Pilgrimage
{
    public class Utility
    {
        public static DialogResult ShowMessage(IWin32Window Owner, string Message, MessageBoxButtons Buttons, MessageBoxIcon Icon)
        {
            return MessageBox.Show(Owner, Message, Program.ProductName, Buttons, Icon);
        }

        public static DialogResult ShowMessage(IWin32Window Owner, string Message)
        {
            return MessageBox.Show(Owner, Message, Program.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void ShowErrorMessage(IWin32Window Owner, string Message)
        {
            MessageBox.Show(Owner, Message, Program.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void ShowErrorMessage(IWin32Window Owner, Exception Error)
        {
            using (frmMessageBox msg = new frmMessageBox(Error.Message, ErrorMessage(Error), MessageBoxButtons.OK, MessageBoxIcon.Error, true, true))
            {
                if (Owner != null)
                { msg.ShowDialog(Owner); }
                else
                { msg.ShowDialog(); }
            }
        }

        private static string ErrorMessage(Exception Error)
        {
            string message = Error.ToString();
            if (Error.InnerException != null)
            {
                message += "\r\n\r\nAdditional details: " + Error.InnerException.ToString();
            }
            return message;
        }

        public static string GetEntryAssemblyProductName()
        {
            return Assembly.GetEntryAssembly().GetAssemblyAttribute<AssemblyProductAttribute>(a => a.Product);
        }

        public static string GetEntryAssemblyTitle()
        {
            return Assembly.GetEntryAssembly().GetAssemblyAttribute<AssemblyTitleAttribute>(a => a.Title);
        }

        public static string GetEntryAssemblyConfiguration()
        {
            return Assembly.GetEntryAssembly().GetAssemblyAttribute<AssemblyConfigurationAttribute>(a => a.Configuration);
        }

        public static Dictionary<string, string> GeneDataFields()
        {
            return (new KeyValuePair<string, string>[] { 
                        new KeyValuePair<string, string>("Definition", ""),
                        new KeyValuePair<string, string>("Organism", ""),
                        new KeyValuePair<string, string>("Taxonomy", ""),
                        new KeyValuePair<string, string>("Locus", ""),
                        new KeyValuePair<string, string>("Accession", ""),
                        new KeyValuePair<string, string>("GenBankID", "GenBank ID"),
                        new KeyValuePair<string, string>("Source", ""),
                        new KeyValuePair<string, string>("Nucleotides", "Nucleotide Sequence")
                })
                .ToDictionary(kv => kv.Key, kv => (string.IsNullOrWhiteSpace(kv.Value) ? kv.Key : kv.Value));
        }
    }

    public static class ExtensionMethods
    {
        internal static List<GenericGeneRowDataItem> ToRowDataItemList(this List<Gene> Value)
        {
            return Value.Select(g => new GenericGeneRowDataItem(g)).ToList();
        }

        internal static List<GenericGeneRowDataItem> ToRowDataItemList(this DataTable Value, bool PopulateWithSequenceData = false)
        {
            return Value
                .Rows
                .Cast<DataRow>()
                .Select(row => 
                    new GenericGeneRowDataItem(Gene.FromDatabaseRow(row, PopulateWithSequenceData)) 
                                                {
                                                    ModifiedAt = row.ToSafeDateTime("ModifiedAt"),
                                                    ProcessedThroughBLASTNAtNCBI = row.ToSafeBoolean("ProcessedThroughBLASTNAtNCBI", false),
                                                    InRecordSet = row.ToSafeBoolean("InRecordSet", false)
                                                })
                .ToList();
        }

        internal static List<BlastNAlignmentRow> ToBlastNAlignmentRowList(this DataTable Value)
        {
            return Value
                .Rows
                .Cast<DataRow>()
                .Select(row => new BlastNAlignmentRow(row))
                .ToList();
        }

        internal static string GetString(this List<ApplicationProperty> List, string Key)
        {
            ApplicationProperty test = List.FirstOrDefault(ap => ap.Key == Key);
            if (test != null) { return test.Value; }
            else { return string.Empty; }
        }

        internal static bool GetBool(this List<ApplicationProperty> List, string Key, bool Default = true)
        {
            bool parsedValue = true;

            if (bool.TryParse(GetString(List, Key), out parsedValue))
            {
                return parsedValue;
            }
            else { return Default; }
        }

        internal static void SetString(this List<ApplicationProperty> List, string Key, string Value)
        {
            ApplicationProperty test = List.FirstOrDefault(ap => ap.Key == Key);
            if (test != null) { test.Value = Value; }
            else { List.Add(new ApplicationProperty(Key) { Value = Value }); }
        }
        
        internal static bool Match(this string SearchingIn, string SearchingFor, FilterLogicOptions Logic)
        {
            switch (Logic)
            {
                case FilterLogicOptions.Equals:
                    return SearchingIn.ToLower() == SearchingFor.ToLower();
                case FilterLogicOptions.Contains:
                    return SearchingIn.ToLower().Contains(SearchingFor.ToLower());
                case FilterLogicOptions.DoesNotContain:
                    return !(SearchingIn.ToLower().Contains(SearchingFor.ToLower()));
                case FilterLogicOptions.StartsWith:
                    return SearchingIn.ToLower().StartsWith(SearchingFor.ToLower());
                case FilterLogicOptions.EndsWith:
                    return SearchingIn.ToLower().EndsWith(SearchingFor.ToLower());
            }

            return false;
        }

        internal static void ScrollToEnd(this TextBox TextBox, bool Focus = true)
        {
            if (TextBox.Text.Length > 0)
            {
                TextBox.Select(TextBox.Text.Length, 0);
                TextBox.ScrollToCaret();
            }

            if (Focus) { TextBox.Focus(); }
        }

        internal static bool IsEmailAddress(this string EmailAddress)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(EmailAddress.ToUpper(), "^[A-Z0-9._%+-]+@(?:[A-Z0-9-]+\\.)+[A-Z]{2,4}$");
        }

        internal static string ElapsedTimeStamp(this TimeSpan Elapsed)
        {
            return Elapsed.ToString((Elapsed.Days > 0 ? "d\\.h\\:" : (Elapsed.Hours > 0 ? "h\\:" : "")) + "mm\\:ss");
        }
    }

    public enum FilterLogicOptions : int
    {
        Equals = 1,
        Contains = 2,
        DoesNotContain = 3,
        StartsWith = 4,
        EndsWith = 5
    }

    internal class ValidationMessage
    {
        internal string Message { get; set; }
        internal MessageBoxIcon Level { get; set; }

        internal ValidationMessage(string Message, MessageBoxIcon Level)
        {
            this.Message = Message;
            this.Level = Level;
        }

        internal static bool Prompt(List<ValidationMessage> Messages, IWin32Window OwnerWindow)
        {
            if (Messages.Count != 0)
            {
                if (Messages.Any(msg => msg.Level == MessageBoxIcon.Error))
                {
                    Utility.ShowMessage(OwnerWindow, Messages.Select(msg => msg.Message).Concatenate("\r\n"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                else
                {
                    // If there are messages but none of them are errors, then all of them are warnings.
                    if (Utility.ShowMessage(OwnerWindow, Messages.Select(msg => msg.Message).Concatenate("\r\n"), MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Cancel)
                    { return false; }
                }
            }

            return true;
        }
    }
}