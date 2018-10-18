using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using ChangLab.RecordSets;

namespace Pilgrimage
{
    public partial class DialogForm : Form
    {
        protected internal Form OwnerForm { get { return (this.Owner != null ? this.Owner : this); } }
        protected internal frmProgress ProgressForm { get; set; }
        internal Control FocusOnLoad { get; set; }

        protected internal Dictionary<FilterLogicOptions, string> FilterLogicDataSource
        {
            get
            {
                Dictionary<FilterLogicOptions, string> options = new Dictionary<FilterLogicOptions, string>();

                options.Add(FilterLogicOptions.Equals, "Equals");
                options.Add(FilterLogicOptions.Contains, "Contains");
                options.Add(FilterLogicOptions.DoesNotContain, "Does Not Contain");
                options.Add(FilterLogicOptions.StartsWith, "Starts With");
                options.Add(FilterLogicOptions.EndsWith, "End With");

                return options;
            }
        }
        
        public DialogForm()
        {
            InitializeComponent();
        }

        protected internal virtual void DialogForm_Load(object sender, EventArgs e)
        {
            SetFocusOnControl(this.FocusOnLoad);
        }

        #region Buttons
        internal void SetButtonImage(Button Button, string ImageKey)
        {
            if (Button.Width == 75) { Button.Width = 80; } /* Override the default */
            Button.ImageList = this.ilButtons;
            Button.ImageAlign = ContentAlignment.MiddleLeft;
            Button.ImageKey = ImageKey;
        }

        internal void SetButtonImage(Button Button, DialogButtonPresets Preset)
        {
            if (Button.Width == 75) { Button.Width = 80; } /* Override the default */
            Button.ImageList = this.ilButtons;
            Button.ImageAlign = ContentAlignment.MiddleLeft;
            switch (Preset)
            {
                case DialogButtonPresets.Close:
                    Button.ImageKey = "Cancel";
                    break;
                case DialogButtonPresets.Edit:
                    Button.ImageKey = "Rename";
                    break;

                case DialogButtonPresets.Add:
                case DialogButtonPresets.Cancel:
                case DialogButtonPresets.Delete:
                case DialogButtonPresets.OK:
                case DialogButtonPresets.Rename:
                case DialogButtonPresets.Run:
                case DialogButtonPresets.Save:
                    Button.ImageKey = Preset.ToString(); 
                    break;
            }
        }

        public enum DialogButtonPresets
        {
            Cancel,
            Close,
            Add,
            Save,
            OK,
            Delete,
            Edit,
            Rename,
            Run
        }
        #endregion
        
        internal void ConfigureSubSetList(ComboBox List, DataTypes DataType, string SelectedSubSetID = "")
        {
            List<SubSet> subSets = Program.Settings.AllSubSets(DataType).OrderBy(sub => sub.Name).ToList();
            subSets.Insert(0, new SubSet(DataType) { Name = "--- New Dataset ---" });

            List.DisplayMember = "Name";
            List.ValueMember = "ID";
            List.DataSource = new BindingSource(subSets, null);
            if (!string.IsNullOrWhiteSpace(SelectedSubSetID) && subSets.Any(sub => ChangLab.Common.GuidCompare.Equals(sub.ID, SelectedSubSetID)))
            { List.SelectedValue = SelectedSubSetID; }
            else if (Program.Settings.GetCurrentSubSet(DataType) != null)
            { List.SelectedValue = Program.Settings.GetCurrentSubSet(DataType).ID; }
            else
            { List.SelectedIndex = 0; }
        }

        protected internal void CloseProgressForm(DialogResult Result)
        {
            ProgressForm.DialogResult = Result;
            ProgressForm.Close();
        }

        private void timFocus_Tick(object sender, EventArgs e)
        {
            ((Timer)sender).Stop();
            if (FocusOnLoad != null) { FocusOnLoad.Focus(); }
        }

        internal void SetFocusOnControl(Control FocusOn)
        {
            this.FocusOnLoad = FocusOn; // Outside of the if allows for nulling out the focus control; not sure why we need that, but you never know.

            if (this.FocusOnLoad != null)
            {
                timFocus.Start();
            }
        }
    }

    public class IODialogHelper
    {
        #region File Dialogs
        /// <summary>
        /// Open a file with a preset and get back a FileInfo.
        /// </summary>
        internal static bool OpenFile(DialogPresets Settings, IWin32Window ParentForm, ref FileInfo File, bool? AllowAllFiles = null)
        {
            return OpenDialog(new OpenFileDialog(), Settings, string.Empty, string.Empty, string.Empty, ParentForm, ref File, AllowAllFiles);
        }

        /// <summary>
        /// Open a file with a preset and get back a string filepath.
        /// </summary>
        internal static bool OpenFile(DialogPresets Settings, IWin32Window ParentForm, ref string FilePath, bool? AllowAllFiles = null)
        {
            return OpenDialog(new OpenFileDialog(), Settings, string.Empty, string.Empty, string.Empty, ParentForm, ref FilePath, AllowAllFiles);
        }

        /// <summary>
        /// Open a file with a custom filter, extension, and input filepath and get back a FileInfo.
        /// </summary>
        internal static bool OpenFile(string Filter, string DefaultExtension, string FilePath, IWin32Window ParentForm, ref FileInfo File, bool? AllowAllFiles = null)
        {
            return OpenDialog(new OpenFileDialog(), DialogPresets.Custom, Filter, DefaultExtension, FilePath, ParentForm, ref File, AllowAllFiles);
        }

        /// <summary>
        /// Open a file with a custom filter and extension and get back a string filepath.
        /// </summary>
        internal static bool OpenFile(string Filter, string DefaultExtension, IWin32Window ParentForm, ref string FilePath, bool? AllowAllFiles = null, bool SetLastDirectory = true)
        {
            return OpenDialog(new OpenFileDialog(), DialogPresets.Custom, Filter, DefaultExtension, string.Empty, ParentForm, ref FilePath, AllowAllFiles, SetLastDirectory);
        }

        /// <summary>
        /// Save a file with a preset and filename and get back a FileInfo.
        /// </summary>
        internal static bool SaveFile(DialogPresets Settings, string FileName, IWin32Window ParentForm, ref FileInfo File, bool? AllowAllFiles = null)
        {
            return OpenDialog(new SaveFileDialog(), Settings, string.Empty, string.Empty, FileName, ParentForm, ref File, AllowAllFiles);
        }

        /// <summary>
        /// Save a file with a preset and filename and get back a string filepath.
        /// </summary>
        internal static bool SaveFile(DialogPresets Settings, string FileName, IWin32Window ParentForm, ref string FilePath, bool? AllowAllFiles = null)
        {
            return OpenDialog(new SaveFileDialog(), Settings, string.Empty, string.Empty, FileName, ParentForm, ref FilePath, AllowAllFiles);
        }

        /// <summary>
        /// Base private OpenDialog() overload for returning getting back a FileInfo.
        /// </summary>
        private static bool OpenDialog(FileDialog Dialog, DialogPresets Settings, string Filter, string DefaultExtension, string FileName, IWin32Window ParentForm, ref string FilePath, bool? AllowAllFiles = null, bool SetLastDirectory = true)
        {
            FileInfo fileInfo = null;
            if (File.Exists(FilePath)) { fileInfo = new FileInfo(FilePath); }
            bool result = OpenDialog(Dialog, Settings, Filter, DefaultExtension, FileName, ParentForm, ref fileInfo, AllowAllFiles, SetLastDirectory);

            if (result)
            {
                FilePath = fileInfo.FullName;
            }
            else
            {
                FilePath = string.Empty;
            }
            return result;
        }

        /// <summary>
        /// Base OpenDialog() overload for all internal and private overloads.
        /// </summary>
        private static bool OpenDialog(FileDialog Dialog, DialogPresets Settings, string Filter, string DefaultExtension, string FileName, IWin32Window ParentForm, ref FileInfo File, bool? AllowAllFiles = null, bool SetLastDirectory = true)
        {
            using (Dialog)
            {
                Dialog.DefaultExt = (Settings != DialogPresets.Custom ? DeriveDefaultExtension(Settings) : DefaultExtension);
                if (!string.IsNullOrWhiteSpace(FileName))
                { Dialog.FileName = FileName; }
                else if (File != null && File.Exists)
                { Dialog.FileName = File.Name; }

                switch (Settings)
                {
                    case DialogPresets.Custom:
                        Dialog.Filter = Filter;
                        break;
                    case DialogPresets.Database:
                        Dialog.Filter = string.Format("Database files (*.{0})|*.{0}", Dialog.DefaultExt);
                        Dialog.ValidateNames = false;
                        break;
                    case DialogPresets.PilgrimageDataFile:
                        Dialog.Filter = string.Format(Program.ProductName + " Data files (*.{0})|*.{0}", Dialog.DefaultExt);
                        break;
                    case DialogPresets.PilgrimageProjectFile:
                        Dialog.Filter = string.Format(Program.ProductName + " Project files (*.{0})|*.{0}", Dialog.DefaultExt);
                        break;
                    case DialogPresets.FASTA:
                        Dialog.Filter = string.Format("FASTA files (*.{0})|*.{0}", Dialog.DefaultExt);
                        break;
                    case DialogPresets.Text:
                        Dialog.Filter = string.Format("Text files (*.{0})|*.{0}", Dialog.DefaultExt);
                        break;
                    case DialogPresets.XML:
                        Dialog.Filter = string.Format("XML files (*.{0})|*.{0}", Dialog.DefaultExt);
                        break;
                    case DialogPresets.Excel:
                        Dialog.Filter = "Excel workbook (*.xlsx)|*.xlsx";
                        break;
                    case DialogPresets.PHYLIP:
                        Dialog.Filter = "PHYLIP files (*.phylip,*.phyml)|*.phylip;*.phyml";
                        break;
                    case DialogPresets.NEXUS:
                        Dialog.Filter = string.Format("NEXUS files (*.{0})|*.{0}", Dialog.DefaultExt);
                        break;
                }

                switch (Settings)
                {
                    case DialogPresets.All:
                        Dialog.Filter = "All files|*.*";
                        break;
                    // Default AllFiles behavior for FASTA and Text is true
                    case DialogPresets.FASTA:
                    case DialogPresets.PHYLIP:
                    case DialogPresets.NEXUS:
                    case DialogPresets.Text:
                    case DialogPresets.PilgrimageDataFile: // Added because of the file extension change with renaming to BlastPhyMe
                    case DialogPresets.PilgrimageProjectFile: // Added because of the file extension change with renaming to BlastPhyMe
                        if (AllowAllFiles == null || AllowAllFiles == true) // Not set (null) assumed as defaulted to true
                        {
                            Dialog.Filter += "|All files|*.*";
                        }
                        break;
                    // For all other presets (except, obviously, All), default behavior is false
                    default:
                        if (AllowAllFiles != null && AllowAllFiles == true) // Must be explicitly set.
                        {
                            Dialog.Filter += "|All files|*.*";
                        }
                        break;
                }

                if (File != null && File.Exists)
                {
                    Dialog.InitialDirectory = File.Directory.FullName;
                }
                else
                {
                    switch (Settings)
                    {
                        case DialogPresets.Database:
                            if (Directory.Exists(Program.Settings.LastDatabaseDirectory))
                            { Dialog.InitialDirectory = Program.Settings.LastDatabaseDirectory; }
                            break;
                        default:
                            if (Directory.Exists(Program.Settings.LastWorkingDirectory))
                            { Dialog.InitialDirectory = Program.Settings.LastWorkingDirectory; }
                            break;
                    }
                }

                if (Dialog.ShowDialog(ParentForm) == System.Windows.Forms.DialogResult.OK)
                {
                    File = new FileInfo(Dialog.FileName);
                    if (SetLastDirectory)
                    {
                        switch (Settings)
                        {
                            case DialogPresets.Database:
                                Program.Settings.LastDatabaseDirectory = File.Directory.FullName;
                                break;
                            default:
                                Program.Settings.LastWorkingDirectory = File.Directory.FullName;
                                break;
                        }
                    }
                    return true;
                }
                else
                {
                    File = null;
                    return false;
                }
            }
        }

        internal enum DialogPresets
        {
            All,
            Custom,
            Database,
            PilgrimageDataFile,
            FASTA,
            Text,
            XML,
            Excel,
            PHYLIP,
            NEXUS,
            PilgrimageProjectFile
        }

        internal static string DeriveDefaultExtension(DialogPresets Settings)
        {
            switch (Settings)
            {
                case DialogPresets.Database: return "mdf";
                case DialogPresets.PilgrimageDataFile: return "bpmd";
                case DialogPresets.PilgrimageProjectFile: return "bmpp";
                case DialogPresets.FASTA: return "fas";
                case DialogPresets.Text: return "txt";
                case DialogPresets.XML: return "xml";
                case DialogPresets.Excel: return "xlsx";
                case DialogPresets.PHYLIP: return "phylip";
                case DialogPresets.NEXUS: return "nexus";
                default: return "";
            }
        }
        #endregion

        #region Folder Dialogs
        internal static bool FolderBrowse(out DirectoryInfo SelectedDirectory)
        {
            string directory = string.Empty;
            bool result = FolderBrowse(ref directory);

            if (result)
            {
                SelectedDirectory = new DirectoryInfo(directory);
            }
            else
            {
                SelectedDirectory = null;
            }
            return result;
        }

        internal static bool FolderBrowse(ref string SelectedDirectoryPath)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.RootFolder = Environment.SpecialFolder.Desktop;
                if (!string.IsNullOrWhiteSpace(SelectedDirectoryPath) && Directory.Exists(SelectedDirectoryPath))
                { dialog.SelectedPath = SelectedDirectoryPath; }
                else if (Directory.Exists(Program.Settings.LastWorkingDirectory))
                { dialog.SelectedPath = Program.Settings.LastWorkingDirectory; }

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    SelectedDirectoryPath = dialog.SelectedPath;
                    Program.Settings.LastWorkingDirectory = SelectedDirectoryPath;
                    return true;
                }
                else
                {
                    SelectedDirectoryPath = string.Empty;
                    return false;
                }
            }
        }
        #endregion

        #region File Access
        internal static FileAccessResults CanModify(string FilePath)
        {
            try
            {
                using (FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.ReadWrite))
                {
                    return FileAccessResults.ReadWrite;
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.ToLower().Contains("another process"))
                {
                    return FileAccessResults.LockedByProcess;
                }
                else
                {
                    return FileAccessResults.Denied;
                }
            }
        }

        internal static FileAccessResults CanCreate(string FilePath)
        {
            Exception fileException = null;
            return IODialogHelper.CanCreate(FilePath, out fileException);
        }

        internal static FileAccessResults CanCreate(string FilePath, out Exception FileException)
        {
            try
            {
                using (FileStream fs = new FileStream(FilePath, FileMode.Create, FileAccess.ReadWrite))
                {
                    FileException = null;
                    return FileAccessResults.ReadWrite;
                }
            }
            catch (PathTooLongException pathEx)
            {
                FileException = pathEx;
                return FileAccessResults.InvalidFileName;
            }
            catch (Exception ex)
            {
                FileException = ex;

                if (ex.Message.ToLower().Contains("another process"))
                {
                    return FileAccessResults.LockedByProcess;
                }
                else
                {
                    return FileAccessResults.Denied;
                }
            }
        }

        internal enum FileAccessResults
        {
            ReadWrite,
            LockedByProcess,
            Denied,
            InvalidFileName
        }
        #endregion

        #region Default Applications for File Extensions
        /*
         * Courtesy of Ohad Schneider: http://stackoverflow.com/questions/162331/finding-the-default-application-for-opening-a-particular-file-type-on-windows 
        */

        [DllImport("Shlwapi.dll", CharSet = CharSet.Unicode)]
        public static extern uint AssocQueryString(AssocF flags, AssocStr str, string pszAssoc, string pszExtra, [Out] StringBuilder pszOut, ref uint pcchOut);

        public enum AssocF
        {
            None = 0x00000000
        }

        public enum AssocStr
        {
            Command = 1
        }

        public const int S_OK = 0;
        public const int S_FALSE = 1;
        
        public static bool AssocQueryString(string Extension, ref string ApplicationFilePath)
        {
            uint length = 0;
            uint ret = AssocQueryString(AssocF.None, AssocStr.Command, Extension, null, null, ref length);
            if (ret != S_FALSE)
            {
                return false;
            }

            var sb = new StringBuilder((int)length); // (length-1) will probably work too as the marshaller adds null termination
            ret = AssocQueryString(AssocF.None, AssocStr.Command, Extension, null, sb, ref length);
            if (ret != S_OK)
            {
                return false;
            }

            ApplicationFilePath = sb.ToString();
            return true;
        }
        #endregion
    }
}
