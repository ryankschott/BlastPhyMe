using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using ChangLab.RecordSets;
using ChangLab.Common;
using Pilgrimage.IO;

namespace Pilgrimage
{
    internal class AppSettings
    {
        private string FilePath { get { return "settings.ini"; } }

        internal string LastWorkingDirectory { get; set; }
        internal string LastOutputDirectory { get; set; }
        internal string LastDatabaseDirectory { get; set; }

        public List<FileInfo> RecentDatabases { get; private set; }

        internal string MEGAFullPath { get; set; }
        
        private string _codeMLFullPath;
        internal string CodeMLFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(_codeMLFullPath))
                {
                    _codeMLFullPath = "C:\\Program Files (x86)\\Paml4.8\\bin\\codeml.exe";

                    if (!File.Exists(_codeMLFullPath))
                    {
                        _codeMLFullPath = "C:\\Program Files\\Paml4.8\\bin\\codeml.exe";

                        if (!File.Exists(_codeMLFullPath)) { _codeMLFullPath = string.Empty; }
                    }
                }

                return _codeMLFullPath;
            }
            set { _codeMLFullPath = value; }
        }
        
        internal string PRANKFullPath { get; set; }
        
        private string _phyMLFullPath;
        internal string PhyMLFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(_phyMLFullPath))
                {
                    _phyMLFullPath = "C:\\Program Files (x86)\\PhyML-3.1\\PhyML-3.1_win32.exe";

                    if (!File.Exists(_phyMLFullPath))
                    {
                        _phyMLFullPath = "C:\\Program Files\\PhyML-3.1\\PhyML-3.1_win32.exe";

                        if (!File.Exists(_phyMLFullPath)) { _phyMLFullPath = string.Empty; }
                    }
                }

                return _phyMLFullPath;
            }
            set { _phyMLFullPath = value; }
        }

        private string _treeViewFullPath;
        internal string TreeViewFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(_treeViewFullPath))
                {
                    _treeViewFullPath = "C:\\Program Files (x86)\\TreeView\\treev32.exe";
                    if (!File.Exists(_treeViewFullPath))
                    {
                        _treeViewFullPath = "C:\\Program Files\\TreeView\\treev32.exe";
                        if (!File.Exists(_treeViewFullPath)) { _treeViewFullPath = string.Empty; }
                    }
                }

                return _treeViewFullPath;
            }
            set { _treeViewFullPath = value; }
        }

        private string _figTreeFullPath;
        internal string FigTreeFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(_figTreeFullPath))
                {
                    _figTreeFullPath = "C:\\Program Files (x86)\\FigTree v1.4.2\\FigTree v1.4.2.exe";

                    if (!File.Exists(_figTreeFullPath))
                    {
                        _figTreeFullPath = "C:\\Program Files\\FigTree v1.4.2\\FigTree v1.4.2.exe";

                        if (!File.Exists(_figTreeFullPath)) { _figTreeFullPath = string.Empty; }
                    }
                }

                return _figTreeFullPath;
            }
            set
            {
                _figTreeFullPath = value;
            }
        }
        
        internal string MUSCLEFullPath { get; set; }

        #region Session Properties
        internal string CurrentDatabaseFilePath { get; set; }

        private RecordSet _currentRecordSet = null;
        internal RecordSet CurrentRecordSet
        {
            get
            {
                return _currentRecordSet;
            }
            set
            {
                if (_currentRecordSet != null && !string.IsNullOrEmpty(_currentRecordSet.ID))
                {
                    Program.RecordSetSettings.Save(_currentRecordSet.ID);
                }

                _currentRecordSet = value;

                if (_currentRecordSet != null && !string.IsNullOrEmpty(_currentRecordSet.ID))
                {
                    Program.RecordSetSettings.Refresh(_currentRecordSet.ID);
                }
            }
        }

        private Dictionary<DataTypes, SubSet> _currentSubSets = null;
        internal SubSet GetCurrentSubSet(DataTypes DataType)
        {
            if (_currentSubSets == null || !_currentSubSets.ContainsKey(DataType)) { return null; }
            else { return _currentSubSets[DataType]; }
        }
        internal void SetCurrentSubSet(DataTypes DataType, SubSet CurrentSubSet)
        {
            if (_currentSubSets == null) { _currentSubSets = new Dictionary<DataTypes, SubSet>(); }
            if (!_currentSubSets.ContainsKey(DataType)) { _currentSubSets.Add(DataType, null); }

            _currentSubSets[DataType] = CurrentSubSet;
        }

        internal SubSet CurrentSubSet_GeneSequences
        {
            get { return GetCurrentSubSet(DataTypes.GeneSequence); }
            set { SetCurrentSubSet(DataTypes.GeneSequence, value); }
        }

        internal SubSet CurrentSubSet_CodeMLResults
        {
            get { return GetCurrentSubSet(DataTypes.CodeMLResult); }
            set { SetCurrentSubSet(DataTypes.CodeMLResult, value); }
        }

        internal List<SubSet> AllSubSets(DataTypes DataType)
        {
            return CurrentRecordSet.SubSets[DataType].Where(sub => sub.DataType.Key == DataType).ToList();
        }
        internal string LastSubSetID { get; set; }

        private RangeWithInterval _paml_KappaDefault;
        internal RangeWithInterval PAML_KappaDefault
        {
            get
            {
                if (_paml_KappaDefault == null)
                {
#if !EEB460
                    _paml_KappaDefault = new RangeWithInterval(2.00, 3.00, 1.00, false);
#else
                    _paml_KappaDefault = new RangeWithInterval(3.00, 3.00, 1.00, false);
#endif
                }
                return _paml_KappaDefault;
            }
            set { _paml_KappaDefault = value; }
        }
        private RangeWithInterval _paml_OmegaDefault;
        internal RangeWithInterval PAML_OmegaDefault
        {
            get
            {
                if (_paml_OmegaDefault == null)
                {
#if !EEB460
                    _paml_OmegaDefault = new RangeWithInterval(0.00, 2.00, 1.00, false);
#else
                    _paml_OmegaDefault = new RangeWithInterval(0.00, 0.00, 1.00, false);
#endif
                }
                return _paml_OmegaDefault;
            }
            set { _paml_OmegaDefault = value; }
        }

        internal string GeneSequences_FilterTab { get; set; }
        #endregion

        internal AppSettings()
        {
            RecentDatabases = new List<FileInfo>();
        }

        internal void Load()
        {
            try
            {
                if (IsolatedStorageHelper.Exists(FilePath))
                {
                    List<string> allLines = IsolatedStorageHelper.ReadAllLines(FilePath);
                    for (int i = 0; i < allLines.Count; i++)
                    {
                        string line = allLines[i];
                        if (line.IndexOf("=") > 0)
                        {
                            string setting = line.Substring(0, line.IndexOf("="));
                            string value = line.Substring(line.IndexOf("=") + 1);
                            switch (setting)
                            {
                                case "last_directory":
                                    this.LastWorkingDirectory = value;
                                    break;
                                case "last_output_directory":
                                    this.LastOutputDirectory = value;
                                    break;
                                case "last_databasefile_directory":
                                    this.LastDatabaseDirectory = value;
                                    break;

                                case "recent_databases":
                                    string[] recentDatabases = value.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                                    recentDatabases.Select(s => s.Replace("\"", "")).Where(s => File.Exists(s)).ToList().ForEach(s =>
                                    {
                                        this.RecentDatabases.Add(new FileInfo(s));
                                    });
                                    break;

                                case "mega_fullpath":
                                    this.MEGAFullPath = value;
                                    break;
                                case "codeml_fullpath":
                                    this.CodeMLFullPath = value;
                                    break;
                                case "prank_fullpath":
                                    this.PRANKFullPath = value;
                                    break;
                                case "phyml_fullpath":
                                    this.PhyMLFullPath = value;
                                    break;
                                case "treeview_fullpath":
                                    this.TreeViewFullPath = value;
                                    break;
                                case "muscle_fullpath":
                                    this.MUSCLEFullPath = value;
                                    break;
                                case "figtree_fullpath":
                                    this.FigTreeFullPath = value;
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to read settings file [" + FilePath + "]: " + ex.Message);
            }
        }

        internal void Save()
        {
            try
            {
                List<string> lines = new List<string>();
                lines.Add("last_directory=" + this.LastWorkingDirectory);
                lines.Add("last_output_directory=" + this.LastOutputDirectory);
                lines.Add("last_databasefile_directory=" + this.LastDatabaseDirectory);

                if (this.RecentDatabases.Count != 0)
                { lines.Add("recent_databases=" + this.RecentDatabases.Aggregate(string.Empty, (current, fi) => current += "\"" + fi.FullName + "\";")); }

                lines.Add("mega_fullpath=" + this.MEGAFullPath);
                lines.Add("codeml_fullpath=" + this.CodeMLFullPath);
                lines.Add("prank_fullpath=" + this.PRANKFullPath);
                lines.Add("phyml_fullpath=" + this.PhyMLFullPath);
                lines.Add("treeview_fullpath=" + this.TreeViewFullPath);
                lines.Add("muscle_fullpath=" + this.MUSCLEFullPath);
                lines.Add("figtree_fullpath=" + this.FigTreeFullPath);

                IsolatedStorageHelper.WriteAllLines(FilePath, lines);
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to write settings file to [" + FilePath + "]: " + ex.Message);
            }
        }
    }

    internal class RecordSetSettings
    {
        private List<ApplicationProperty> RecordSetProperties { get; set; }

        internal string LastBlastNTarget
        {
            get { return RecordSetProperties.GetString("LastBlastNTarget"); }
            set { RecordSetProperties.SetString("LastBlastNTarget", value); }
        }

        internal string LastNCBIBlastNDatabaseName
        {
            get { return RecordSetProperties.GetString("LastNCBIBlastNDatabaseName"); }
            set { RecordSetProperties.SetString("LastNCBIBlastNDatabaseName", value); }
        }

        internal ChangLab.NCBI.BlastNServices LastNCBIBlastNService
        {
            get
            {
                ChangLab.NCBI.BlastNServices service = ChangLab.NCBI.BlastNServices.blastn;
                Enum.TryParse<ChangLab.NCBI.BlastNServices>(RecordSetProperties.GetString("LastNCBIBlastNService"), out service);

                return service;
            }
            set { RecordSetProperties.SetString("LastNCBIBlastNService", value.ToString()); }
        }

        internal string LastLocalBlastNDatabaseFile
        {
            get { return RecordSetProperties.GetString("LastLocalBlastNDatabaseFile"); }
            set { RecordSetProperties.SetString("LastLocalBlastNDatabaseFile", value); }
        }

        internal string LastLocalBlastNExeDirectory
        {
            get { return RecordSetProperties.GetString("LastLocalBlastNExeDirectory"); }
            set { RecordSetProperties.SetString("LastLocalBlastNExeDirectory", value); }
        }

        internal string LastLocalBlastNOutputDirectory
        {
            get { return RecordSetProperties.GetString("LastLocalBlastNOutputDirectory"); }
            set { RecordSetProperties.SetString("LastLocalBlastNOutputDirectory", value); }
        }

        internal string LastModuleOpen
        {
            get { return RecordSetProperties.GetString("LastModuleOpen"); }
            set { RecordSetProperties.SetString("LastModuleOpen", value); }
        }

        internal void Refresh(string RecordSetID)
        {
            RecordSetProperties = ApplicationProperty.ListForRecordSet(RecordSetID);
        }

        internal void Save(string RecordSetID)
        {
            RecordSetProperties.ForEach(ap => ap.SaveForRecordSet(RecordSetID));
        }
    }

    /// <remarks>
    /// Okay, so, since this is steadily expanding with added modules and features, at some point we'll need to go XML on the database and make this
    /// hierarchical by module.  The PAML properties under a PAML sub-class, PRAN under a PRAN sub-class, etc.  There'd be a root "PAML" key in
    /// Common.ApplicationProperty but it'd have an XMLValue column populated with key/value pairs for each of the properties.
    /// </remarks>
    internal class DatabaseSettings
    {
        private List<ApplicationProperty> DatabaseProperties { get; set; }
        internal bool Loaded { get { return this.DatabaseProperties != null; } }

        internal string FASTAFileNameFormatString
        {
            get { return DatabaseProperties.First(ap => ap.Key == "FASTAFileNameFormatString").Value; }
            set { DatabaseProperties.First(ap => ap.Key == "FASTAFileNameFormatString").Value = value; }
        }
        internal string FASTAHeaderFormatString
        {
            get { return DatabaseProperties.First(ap => ap.Key == "FASTAHeaderFormatString").Value; }
            set { DatabaseProperties.First(ap => ap.Key == "FASTAHeaderFormatString").Value = value; }
        }
        internal bool FASTAExportOpenOnCreate
        {
            get { return DatabaseProperties.GetBool("FASTAExportOpenOnCreate", true); }
            set { DatabaseProperties.SetString("FASTAExportOpenOnCreate", value.ToString()); }
        }

        internal bool ImportAlignedSequences_ShowInstructions
        {
            get { return DatabaseProperties.GetBool("ImportAlignedSequences_ShowInstructions", true); }
            set { DatabaseProperties.SetString("ImportAlignedSequences_ShowInstructions", value.ToString()); }
        }

        internal string NCBIProductName
        {
            get
            {
                return "BlastPhyMe_ChangLab_UniversityOfToronto";
            }
        }
        internal string NCBIEmailAddress
        {
            get { return DatabaseProperties.First(ap => ap.Key == "NCBIEmailAddress").Value; }
            set { DatabaseProperties.First(ap => ap.Key == "NCBIEmailAddress").Value = value; }
        }

        internal List<string> ExportToExcelColumnKeys
        {
            get { return DatabaseProperties.GetString("ExportToExcelColumnKeys").Split(new char[] { ',' }).ToList(); }
            set { DatabaseProperties.SetString("ExportToExcelColumnKeys", value.Concatenate(",")); }
        }
        internal bool ExportToExcelOpenOnCreate
        {
            get { return DatabaseProperties.GetBool("ExportToExcelOpenOnCreate", true); }
            set { DatabaseProperties.SetString("ExportToExcelOpenOnCreate", value.ToString()); }
        }

        private Dictionary<DataTypes, Dictionary<string, System.Drawing.Point>> ToolStripsLocations { get; set; }

        internal System.Drawing.Point GetToolStripLocation(DataTypes DataType, System.Windows.Forms.ToolStrip ToolStrip)
        {
            if (ToolStripsLocations[DataType].ContainsKey(ToolStrip.Name)) { return ToolStripsLocations[DataType][ToolStrip.Name]; }
            else { return ToolStrip.Location; }
        }
        internal void SetToolStripLocation(DataTypes DataType, System.Windows.Forms.ToolStrip ToolStrip)
        {
            if (!ToolStripsLocations[DataType].ContainsKey(ToolStrip.Name))
            { ToolStripsLocations[DataType].Add(ToolStrip.Name, ToolStrip.Location); }
            else
            { ToolStripsLocations[DataType][ToolStrip.Name] = ToolStrip.Location; }
        }

        #region BlastNAtNCBI
        internal BlastNAtNCBISettings BlastNAtNCBI { get; private set; }

        internal class BlastNAtNCBISettings : SubSettings
        {
            internal BlastNAtNCBISettings(List<ApplicationProperty> DatabaseProperties) : base(DatabaseProperties) { }

            internal bool Annotate_ReplaceAllSequences
            {
                get { return DatabaseProperties.GetBool("BlastNAtNCBI_AnnotateRepSeq", false); }
                set { DatabaseProperties.SetString("BlastNAtNCBI_AnnotateRepSeq", value.ToString()); }
            }

            /// <summary>
            /// Must be set and retrieved whole, if the properties within are set individually they will not be saved to the database.
            /// </summary>
            internal ChangLab.NCBI.BlastNWebServiceConfigurationSettings Options_SimilarSequences
            {
                get { return new ChangLab.NCBI.BlastNWebServiceConfigurationSettings(DatabaseProperties.GetString("BlastNAtNCBI_OptionsSimilar")); }
                set { DatabaseProperties.SetString("BlastNAtNCBI_OptionsSimilar", value.ToString()); }
            }

            /// <summary>
            /// Must be set and retrieved whole, if the properties within are set individually they will not be saved to the database.
            /// </summary>
            internal ChangLab.NCBI.BlastNWebServiceConfigurationSettings Options_AnnotateSequences
            {
                get
                {
                    string options = DatabaseProperties.GetString("BlastNAtNCBI_OptionsAnnotate");

                    ChangLab.NCBI.BlastNWebServiceConfigurationSettings settings = null;
                    if (!string.IsNullOrWhiteSpace(options))
                    { settings = new ChangLab.NCBI.BlastNWebServiceConfigurationSettings(options); }
                    else
                    { settings = new ChangLab.NCBI.BlastNWebServiceConfigurationSettings() { MaximumTargetSequences = 10 }; }

                    return settings;
                }
                set { DatabaseProperties.SetString("BlastNAtNCBI_OptionsAnnotate", value.ToString()); }
            }
        }
        #endregion

        #region PAML
        internal PAMLSettings PAML { get; private set; }

        internal class PAMLSettings : SubSettings
        {
            internal PAMLSettings(List<ApplicationProperty> DatabaseProperties) : base(DatabaseProperties) { }

            internal string WorkingDirectory
            {
                get { return GetDirectoryPath(DatabaseProperties, "PAML_WorkingDirectory"); }
                set { DatabaseProperties.SetString("PAML_WorkingDirectory", value); }
            }

            internal bool KeepFolders
            {
                get { return DatabaseProperties.GetBool("PAML_KeepFolders", true); }
                set { DatabaseProperties.SetString("PAML_KeepFolders", value.ToString()); }
            }

            internal int CodeMLConcurrentProcesses
            {
                get
                {
                    int processes = Environment.ProcessorCount;
                    int.TryParse(DatabaseProperties.GetString("PAML_CodeMLConcurrentProcesses"), out processes);
                    if (processes > Environment.ProcessorCount || processes == 0) { processes = Environment.ProcessorCount; }
                    return processes;
                }
                set { DatabaseProperties.SetString("PAML_CodeMLConcurrentProcesses", value.ToString()); }
            }

            internal System.Diagnostics.ProcessPriorityClass CodeMLPriority
            {
                get
                {
                    System.Diagnostics.ProcessPriorityClass priority = System.Diagnostics.ProcessPriorityClass.BelowNormal;
                    string priorityString = DatabaseProperties.GetString("PAML_CodeMLPriority");
                    if (Enum.TryParse<System.Diagnostics.ProcessPriorityClass>(priorityString, out priority))
                    {
                        return priority;
                    }
                    else { return System.Diagnostics.ProcessPriorityClass.BelowNormal; }
                }
                set { DatabaseProperties.SetString("PAML_CodeMLPriority", value.ToString()); }
            }

            /// <summary>
            /// Must be set and retrieved whole, if the properties within are set individually they will not be saved to the database.
            /// </summary>
            internal ChangLab.PAML.ControlConfiguration Configuration
            {
                get { return new ChangLab.PAML.ControlConfiguration(DatabaseProperties.GetString("PAML_Configuration")); }
                set { DatabaseProperties.SetString("PAML_Configuration", value.ToString()); }
            }
        }
        #endregion

        #region PRANK
        internal PRANKSettings PRANK { get; private set; }

        internal class PRANKSettings : SubSettings
        {
            internal PRANKSettings(List<ApplicationProperty> DatabaseProperties) : base(DatabaseProperties) { }

            internal string WorkingDirectory
            {
                get { return GetDirectoryPath(DatabaseProperties, "PRANK_WorkingDirectory"); }
                set { DatabaseProperties.SetString("PRANK_WorkingDirectory", value); }
            }

            internal bool KeepOutputFiles
            {
                get { return DatabaseProperties.GetBool("PRANK_KeepOutput", true); }
                set { DatabaseProperties.SetString("PRANK_KeepOutput", value.ToString()); }
            }

            /// <summary>
            /// Must be set and retrieved whole, if the properties within are set individually they will not be saved to the database.
            /// </summary>
            internal ChangLab.Jobs.PRANKOptions Options
            {
                get { return new ChangLab.Jobs.PRANKOptions(DatabaseProperties.GetString("PRANK_Options")); }
                set { DatabaseProperties.SetString("PRANK_Options", value.ToString()); }
            }
        }
        #endregion

        #region PhyML
        internal PhyMLSettings PhyML { get; private set; }

        internal class PhyMLSettings : SubSettings
        {
            internal PhyMLSettings(List<ApplicationProperty> DatabaseProperties) : base(DatabaseProperties) { }

            internal string SequenceHeaderFormat
            {
                get { return GetDirectoryPath(DatabaseProperties, "PhyML_SequenceHeaderFormat"); }
                set { DatabaseProperties.SetString("PhyML_SequenceHeaderFormat", value); }
            }

            internal string WorkingDirectory
            {
                get { return GetDirectoryPath(DatabaseProperties, "PhyML_WorkingDirectory"); }
                set { DatabaseProperties.SetString("PhyML_WorkingDirectory", value); }
            }

            internal bool KeepOutput
            {
                get { return DatabaseProperties.GetBool("PhyML_KeepOutput", true); }
                set { DatabaseProperties.SetString("PhyML_KeepOutput", value.ToString()); }
            }

            /// <summary>
            /// Must be set and retrieved whole, if the properties within are set individually they will not be saved to the database.
            /// </summary>
            internal ChangLab.Jobs.PhyMLOptions Options
            {
                get { return new ChangLab.Jobs.PhyMLOptions(DatabaseProperties.GetString("PhyML_Options")); }
                set { DatabaseProperties.SetString("PhyML_Options", value.ToString()); }
            }
        }
        #endregion

        #region MUSCLE
        internal MUSCLESettings MUSCLE { get; private set; }

        internal class MUSCLESettings : SubSettings
        {
            internal MUSCLESettings(List<ApplicationProperty> DatabaseProperties) : base(DatabaseProperties) { }

            internal string WorkingDirectory
            {
                get { return GetDirectoryPath(DatabaseProperties, "MUSCLE_WorkingDirectory"); }
                set { DatabaseProperties.SetString("MUSCLE_WorkingDirectory", value); }
            }

            internal bool KeepOutputFiles
            {
                get { return DatabaseProperties.GetBool("MUSCLE_KeepOutputFiles", true); }
                set { DatabaseProperties.SetString("MUSCLE_KeepOutputFiles", value.ToString()); }
            }

            /// <summary>
            /// Must be set and retrieved whole, if the properties within are set individually they will not be saved to the database.
            /// </summary>
            internal ChangLab.Jobs.MUSCLEOptions Options
            {
                get { return new ChangLab.Jobs.MUSCLEOptions(DatabaseProperties.GetString("MUSCLE_Options")); }
                set { DatabaseProperties.SetString("MUSCLE_Options", value.ToString()); }
            }
        }
        #endregion

        internal DatabaseSettings()
        {
            this.ToolStripsLocations = new Dictionary<DataTypes, Dictionary<string, System.Drawing.Point>>();
            this.ToolStripsLocations.Add(DataTypes.GeneSequence, new Dictionary<string, System.Drawing.Point>());
            this.ToolStripsLocations.Add(DataTypes.CodeMLResult, new Dictionary<string, System.Drawing.Point>());
        }

        internal void RefreshDatabaseProperties()
        {
            DatabaseProperties = ApplicationProperty.List();

            string toolStripsLocationsXML = DatabaseProperties.GetString("ToolStripsLocations");
            if (!string.IsNullOrWhiteSpace(toolStripsLocationsXML))
            {
                try
                {
                    XDocument doc = XDocument.Parse(toolStripsLocationsXML);
                    foreach (XElement moduleElement in doc.Root.Elements("Module"))
                    {
                        DataTypes module = DataTypes.Undefined;
                        if (Enum.TryParse<DataTypes>(moduleElement.Attribute("DataType").Value, out module))
                        {
                            foreach (XElement tsElement in moduleElement.Elements("ToolStrip"))
                            {
                                if (!this.ToolStripsLocations[module].ContainsKey(tsElement.Attribute("Name").Value))
                                { this.ToolStripsLocations[module].Add(tsElement.Attribute("Name").Value, System.Drawing.Point.Empty); }

                                this.ToolStripsLocations[module][tsElement.Attribute("Name").Value] = new System.Drawing.Point(int.Parse(tsElement.Attribute("X").Value), int.Parse(tsElement.Attribute("Y").Value));
                            }
                        }
                    }
                }
                catch { }
            }

            BlastNAtNCBI = new BlastNAtNCBISettings(DatabaseProperties);
            PAML = new PAMLSettings(DatabaseProperties);
            PRANK = new PRANKSettings(DatabaseProperties);
            PhyML = new PhyMLSettings(DatabaseProperties);
            MUSCLE = new MUSCLESettings(DatabaseProperties);
        }

        internal void SaveDatabaseProperties()
        {
            // Capture the ToolStrips Locations as XML
            XDocument doc = new XDocument();
            doc.Add(new XElement("ToolStrips"));
            foreach (var module in this.ToolStripsLocations)
            {
                XElement moduleElement = new XElement("Module");
                moduleElement.Add(new object[] { new XAttribute("DataType", module.Key.ToString()) });
                doc.Root.Add(moduleElement);

                foreach (var ts in module.Value)
                {
                    XElement tsElement = new XElement("ToolStrip");
                    tsElement.Add(new object[] { new XAttribute("Name", ts.Key), new XAttribute("X", ts.Value.X), new XAttribute("Y", ts.Value.Y) });
                    moduleElement.Add(tsElement);
                }
            }
            DatabaseProperties.SetString("ToolStripsLocations", doc.ToString());

            DatabaseProperties.ForEach(ap => ap.Save());
        }

        internal void SaveDatabaseProperty(string Key)
        {
            DatabaseProperties.First(ap => ap.Key == Key).Save();
        }

        internal static string GetDirectoryPath(List<ApplicationProperty> DatabaseProperties, string Key)
        {
            string path = DatabaseProperties.GetString(Key);
            if (!string.IsNullOrWhiteSpace(path) && !System.IO.Directory.Exists(path))
            {
                path = string.Empty;
                DatabaseProperties.SetString(Key, path);
            }

            return path;
        }

        internal class SubSettings
        {
            protected List<ApplicationProperty> DatabaseProperties { get; set; }
            internal SubSettings(List<ApplicationProperty> DatabaseProperties)
            {
                this.DatabaseProperties = DatabaseProperties;
            }
        }
    }
}