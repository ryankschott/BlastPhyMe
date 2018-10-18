using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using ChangLab.Common;
using ChangLab.Genes;
using ChangLab.Jobs;
using ChangLab.PAML.CodeML;

namespace Pilgrimage
{
    public class GenericGeneRowDataItem : RowDataItem
    {
        public override string ID
        {
            get { return Gene.ID; }
        }
        public Gene Gene { get; private set; }

        public string GeneName { get { return Gene.GeneName; } }
        public string Organism { get { return Gene.Organism; } }
        public string Definition { get { return Gene.Definition; } }
        public string Accession { get { return Gene.Accession; } }
        public string Locus { get { return Gene.Locus; } }
        public DateTime LastUpdatedAt { get { return Gene.LastUpdatedAt; } }

        private int _length = -1;
        public int Length
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(Gene.Nucleotides))
                {
                    return Gene.Nucleotides.Length;
                }
                else if (_length != -1) { return _length; }
                else
                {
                    // If we don't have nucleotides, because the Gene is just a shell from, for example, a GenBank search, the SequenceRange property
                    // should have been populated with the Length from the search result (same goes for BLASTN hits).  If not, this will return 0
                    // because SequenceRange is instantiated by Gene's constructor, and that's good enough to say we don't have anything for a length
                    // statistic for this gene.
                    return Gene.SequenceRange.Length;
                }
            }
        }

        public string GenBankID { get { return Gene.GenBankID.ToSafeString(); } }
        public string GenBankUrl { get { return Gene.GenBankUrl; } }

        public string SourceName { get { return GeneSource.NameByID(Gene.SourceID); } }
        public string LastUpdateSourceName { get { return GeneSource.NameByID(Gene.LastUpdateSourceID); } }
        public DateTime ModifiedAt { get; internal set; }

        public string TaxonomyHierarchy { get { return Gene.TaxonomyHierarchy; } }
        public string Nucleotides { get { return Gene.Nucleotides; } }
        public string SourceSequenceString { get { return (Gene.SourceSequence != null ? Gene.SourceSequence.ToString() : string.Empty); } }

        public bool ProcessedThroughBLASTNAtNCBI { get; internal set; }
        public bool HasAlignedQuerySequences { get; internal set; }
        public bool HasBlastNResults { get { return ProcessedThroughBLASTNAtNCBI; } }

        public int DuplicateSequenceKey { get; internal set; }
        public System.Drawing.Bitmap DuplicateSequenceImage { get; internal set; }

        public bool InRecordSet { get; set; }

        public GenericGeneRowDataItem(Gene Gene)
        {
            this.Gene = Gene;
        }
    }

    public class JobRowDataItem : RowDataItem
    {
        private string _id;
        public override string ID { get { return _id; } }

        public Job Job { get; internal set; }

        public string Title { get; private set; }
        public DateTime StartedAt { get; private set; }
        public DateTime EndedAt { get; internal set; }
        public JobStatuses Status { get; internal set; }

        public JobRowDataItem(DataRow Row)
        {
            this._id = Row["ID"].ToString();
            this.Title = Row.ToSafeString("Title");
            this.StartedAt = (DateTime)Row["StartedAt"];
            this.EndedAt = Row.ToSafeDateTime("EndedAt");
            this.Status = (JobStatuses)Enum.Parse(typeof(JobStatuses), (string)Row["JobStatusName"]);
        }
    }

    public class GeneProcessingJobHistoryRow : JobRowDataItem
    {
        public string InputSubSetName { get; private set; }
        public int InputGeneCount { get; private set; }

        public string SubSetNameAndSequenceCount
        {
            get { return InputSubSetName + " (" + InputGeneCount.ToString("N0") + ")"; }
        }

        public virtual string OptionsString
        {
            get { return string.Empty; }
        }

        public GeneProcessingJobHistoryRow(DataRow Row)
            : base(Row)
        {
            this.InputSubSetName = (string)Row["InputSubSetName"];
            this.InputGeneCount = (int)Row["InputGeneCount"];
        }
    }

    public class ThirdPartyComponentReferenceRowDataItem : RowDataItem
    {
        internal ThirdPartyComponentReference ComponentReference { get; private set; }
        public override string ID { get { return ComponentReference.ID.ToString(); } }

        public string Name { get { return this.ComponentReference.Name; } }
        public string Version { get { return this.ComponentReference.Version; } }
        public string Creator { get { return this.ComponentReference.Creator; } }
        public string Copyright { get { return this.ComponentReference.Copyright; } }
        public string ProductURL { get { return this.ComponentReference.ProductURL; } }
        public System.Drawing.Bitmap ComponentLogo { get; set; }

        public ThirdPartyComponentReferenceRowDataItem(ThirdPartyComponentReference ComponentReference)
        {
            this.ComponentReference = ComponentReference;
        }
    }

    public class ImportedFromFileGeneRow : GenericGeneRowDataItem
    {
        public override string ID { get { return this.Gene.ID; } }
        public string Header { get; private set; }
        public bool HasExceptions { get { return (this.Gene.Exceptions.Count != 0); } }
        public System.Drawing.Bitmap ExceptionsImage { get; internal set; }

        public ImportedFromFileGeneRow(Gene Gene, string Header)
            : base(Gene)
        {
            this.Header = Header;
        }
    }

    public class ExceptionRowDataItem
    {
        public int ID { get; private set; }
        public Exception Exception { get; private set; }
        public DateTime OccuredAt { get; private set; }

        // For use when the object is constructed from the database. 
        public string Message { get; private set; }
        public string ExceptionType { get; private set; }

        public System.Drawing.Bitmap ExceptionImage
        { 
            get 
            {
                return ((this.Exception != null && this.Exception.GetType() == typeof(WarningException)) || this.ExceptionType == "WarningException")
                    ? Properties.Resources.Warning_16
                    : Properties.Resources.Error_16;
            }
        }

        public ExceptionRowDataItem(Exception Exception)
        {
            this.Exception = Exception;
            this.Message = Exception.Message;
            this.ExceptionType = Exception.GetType().Name;
            
            if (this.Exception.GetType() == typeof(JobException))
            { 
                this.ID = ((JobException)this.Exception).ID;
                this.OccuredAt = ((JobException)this.Exception).ExceptionAt;
            }
        }

        public ExceptionRowDataItem(DataRow Row)
        {
            this.ID = Row.ToSafeInt("ID");
            this.OccuredAt = Row.ToSafeDateTime("ExceptionAt");
            this.Message = Row.ToSafeString("Message");
            this.ExceptionType = Row.ToSafeString("ExceptionType");
        }
    }
}

namespace Pilgrimage.GeneSequences.BlastN
{
    public class BlastNHistoryRow : GeneProcessingJobHistoryRow
    {
        public override string OptionsString
        {
            get
            {
                string options = string.Empty;
                BlastNAtNCBI ncbi = (BlastNAtNCBI)this.Job;

                if (ncbi.Options != null)
                {
                    switch (ncbi.Purpose)
                    {
                        case BlastNAtNCBI.BLASTPurposes.SimilarCodingSequences:
                            options = "Find similar sequences, ";
                            break;
                        case BlastNAtNCBI.BLASTPurposes.AnnotateUnknownGenes:
                            options = "Annotate sequences, ";
                            break;
                    }
                    return options
                        + "database: " + ncbi.Options.DatabaseName
                        + ", service: " + ncbi.Options.Service.ToString();
                }
                else { return string.Empty; }
            }
        }

        public BlastNHistoryRow(DataRow Row)
            : base(Row)
        {
            this.Job = new BlastNAtNCBI(string.Empty);

            string additionalProperties = Row["AdditionalProperties"].ToString();
            if (!string.IsNullOrWhiteSpace(additionalProperties))
            { ((BlastNAtNCBI)this.Job).Options = new ChangLab.NCBI.BlastNWebServiceConfigurationSettings(additionalProperties); }

            this.Job.FromDataRow(Row);
        }
    }
}

namespace Pilgrimage.GeneSequences.PRANK
{
    public class PRANKHistoryRow : GeneProcessingJobHistoryRow
    {
        private PRANKOptions _options;
        public override string OptionsString
        {
            get { return (_options != null ? _options.OptionSwitches : string.Empty); }
        }

        public PRANKHistoryRow(DataRow Row)
            : base(Row)
        {
            string additionalProperties = Row["AdditionalProperties"].ToString();
            if (!string.IsNullOrWhiteSpace(additionalProperties)) { this._options = new PRANKOptions(additionalProperties); }
            else { this._options = new PRANKOptions(); }
        }
    }
}

namespace Pilgrimage.GeneSequences.MUSCLE
{
    public class MUSCLEHistoryRow : GeneProcessingJobHistoryRow
    {
        private MUSCLEOptions _options;
        public override string OptionsString
        {
            get { return (_options != null ? _options.OptionSwitches : string.Empty); }
        }

        public MUSCLEHistoryRow(DataRow Row)
            : base(Row)
        {
            string additionalProperties = Row["AdditionalProperties"].ToString();
            if (!string.IsNullOrWhiteSpace(additionalProperties)) { this._options = new MUSCLEOptions(additionalProperties); }
            else { this._options = new MUSCLEOptions(); }
        }
    }
}

namespace Pilgrimage.GeneSequences.PhyML
{
    public class PhyMLHistoryRow : GeneProcessingJobHistoryRow
    {
        private PhyMLOptions _options;
        public override string OptionsString
        {
            get { return (_options != null ? _options.OptionSwitches : string.Empty); }
        }

        public PhyMLHistoryRow(DataRow Row)
            : base(Row)
        {
            this.Job = (new GenerateTreeWithPhyML(string.Empty)); this.Job.FromDataRow(Row);
            string additionalProperties = Row["AdditionalProperties"].ToString();
            if (!string.IsNullOrWhiteSpace(additionalProperties)) { this._options = new PhyMLOptions(additionalProperties); }
        }
    }
}

namespace Pilgrimage.PAML
{
    public class TreeConfigurationRowDataItem : AnalysisConfigurationRowDataItem
    {
        public Tree Tree { get; internal set; }

        public string Title { get { return Tree.Title; } }
        public string TreeFileName { get { return Tree.TreeFilePath.Substring(Tree.TreeFilePath.LastIndexOf("\\") + 1); } }
        public string SequencesFileName { get { return Tree.SequencesFilePath.Substring(Tree.SequencesFilePath.LastIndexOf("\\") + 1); } }

        public TreeConfigurationRowDataItem(Tree Tree, AnalysisConfiguration Configuration)
            : base(Configuration)
        {
            this.Tree = Tree;
        }
    }

    public class AnalysisConfigurationRowDataItem
    {
        private AnalysisConfiguration _configuration;
        public AnalysisConfiguration Configuration
        {
            get { return _configuration; }
            set
            {
                _configuration = value;
                this.Preset = ModelPreset.Derive(_configuration);
            }
        }
        private ModelPreset Preset { get; set; }

        public string Model { get { return Preset.Name; } }
        public string NSSites { get { return Configuration.NSSites.Concatenate(", "); } }
        public int NCatG { get { return Configuration.NCatG; } }

        public string KappaDescription
        {
            get
            {
                return AnalysisConfiguration.KappaOmegaDescription(Configuration.KStart, Configuration.KEnd, Configuration.KInterval, Configuration.FixedKappa, DataGridViewHelper.DefaultDoubleFormatString);
            }
        }
        public string OmegaDescription
        {
            get
            {
                return AnalysisConfiguration.KappaOmegaDescription(Configuration.WStart, Configuration.WEnd, Configuration.WInterval, Configuration.FixedOmega, DataGridViewHelper.DefaultDoubleFormatString);
            }
        }

        public AnalysisConfigurationRowDataItem(AnalysisConfiguration Configuration)
        {
            this.Configuration = Configuration;
        }
    }

    public class CodeMLAnalysisOptionRowDataItem
    {
        public CodeMLAnalysisOption Option { get; private set; }
        public int ProcessOutputID { get; private set; }

        public string Title { get; private set; }
        public string Status { get; internal set; }
        public string TreeFileName { get; private set; }
        public string SequencesFileName { get; private set; }
        public string Model { get; private set; }
        public string Kappa { get; private set; }
        public string Omega { get; private set; }
        public string NSSites { get; private set; }
        public int NCatG { get; private set; }
        public string ProcessDirectory { get; private set; }

        public string OutputData { get; private set; }
        public string ErrorData { get; private set; }
        public bool HasExceptions { get; private set; }
        public List<Exception> Exceptions { get; private set; }
        public System.Drawing.Bitmap ExceptionsImage
        { get { return (this.HasExceptions ? Properties.Resources.Warning_16 : Properties.Resources.Transparent_16); } }

        public CodeMLAnalysisOptionRowDataItem()
        {
            this.Exceptions = new List<Exception>();
        }

        public CodeMLAnalysisOptionRowDataItem(CodeMLAnalysisOption Option)
            : this()
        {
            this.Option = Option;
            this.Status = Option.Status.ToString();

            this.Title = Option.Tree.Title;
            this.TreeFileName = Option.Tree.TreeFilePath.FileNameFromPath();
            this.SequencesFileName = Option.Tree.SequencesFilePath.FileNameFromPath();
            this.Model = ModelPreset.Derive(Option.Configuration).Name;
            this.Kappa = Option.Kappa.ToString(DataGridViewHelper.DefaultDoubleFormatString) + (Option.Configuration.FixedKappa ? " (fixed)" : string.Empty);
            this.Omega = Option.Omega.ToString(DataGridViewHelper.DefaultDoubleFormatString) + (Option.Configuration.FixedOmega ? " (fixed)" : string.Empty);
            this.NSSites = Option.Configuration.NSSites.Concatenate(", ");
            this.NCatG = Option.Configuration.NCatG;
        }

        public CodeMLAnalysisOptionRowDataItem(DataRow Row)
            : this()
        {
            this.ProcessOutputID = (int)Row["ID"];
            this.Title = (string)Row["Title"];
            this.Status = (string)Row["Status"];
            this.TreeFileName = ((string)Row["TreeFilePath"]).FileNameFromPath();
            this.SequencesFileName = ((string)Row["SequencesFilePath"]).FileNameFromPath();
            this.Model = ModelPreset.Derive(ModelPresetCollection.KeyByID((int)Row["ModelPresetID"])).Name;
            this.Kappa = Row.ToSafeDouble("Kappa").ToString(DataGridViewHelper.DefaultDoubleFormatString);
            this.Omega = Row.ToSafeDouble("Omega").ToString(DataGridViewHelper.DefaultDoubleFormatString);
            this.NSSites = (string)Row["NSSites"];
            this.NCatG = (int)Row["NCatG"];
            this.ProcessDirectory = Row.ToSafeString("ProcessDirectory");
            this.OutputData = Row.ToSafeString("OutputData");
            this.ErrorData = Row.ToSafeString("ErrorData");
            this.HasExceptions = Row.ToSafeBoolean("HasExceptions");
        }
    }

    public class PAMLHistoryRow : JobRowDataItem
    {
        public string TreeFileName { get; private set; }
        public string SequencesFileName { get; private set; }
        public string TreeTitle { get; private set; }
        public int TreeFileCount { get; private set; }
        public bool InRecordSet { get; internal set; }

        public PAMLHistoryRow(DataRow Row)
            : base(Row)
        {
            this.TreeFileName = (string)Row["TreeFileName"];
            this.SequencesFileName = (string)Row["SequencesFileName"];
            this.TreeTitle = (string)Row["TreeTitle"];
            this.TreeFileCount = (int)Row["TreeFileCount"];
            this.InRecordSet = Row.ToSafeBoolean("InRecordSet");
        }
    }

    public class ResultSummaryRow : RowDataItem
    {
        public override string ID { get { return string.Empty; } }
        public int TreeID { get; private set; }
        public int ResultID { get; private set; }
        public int AnalysisConfigurationID { get; private set; }

        public string JobTitle { get; private set; }
        public string TreeTitle { get; private set; }
        public string TreeFileName { get; private set; }
        public string TreeDescription
        {
            get { return TreeTitle + " (" + TreeFileName + ")"; }
        }
        public string SequencesFileName { get; private set; }

        private ModelPreset Model { get; set; }
        internal int NSSite { get; set; }
        public string ModelDescription
        {
            get
            {
                string description = string.Empty;

                switch (this.Model.Key)
                {
                    case ModelPresets.Model0:
                        description = this.Model.ShortName + " (M" + this.NSSite.ToString() + ")"; break;
                    default:
                        description = this.Model.ShortName; break;
                }

                return description;
            }
        }
        public ModelPresets ModelPresetKey { get { return Model.Key; } }

        private double Kappa { get; set; }
        private double Omega { get; set; }
        public string KappaOmegaStart
        {
            get { return Kappa.ToString(DataGridViewHelper.DefaultDoubleFormatString) + "/" + Omega.ToString(DataGridViewHelper.DefaultDoubleFormatString); }
        }

        public double lnL { get; private set; }
        public double k { get; private set; }

        public DateTime StartedAt { get; private set; }

        public ResultSummaryRow(DataRow Row)
        {
            this.TreeID = (int)Row["TreeID"];
            this.ResultID = (int)Row["ResultID"];
            this.AnalysisConfigurationID = (int)Row["AnalysisConfigurationID"];
            this.JobTitle = (string)Row["JobTitle"];
            this.TreeTitle = (string)Row["TreeTitle"];
            this.TreeFileName = (string)Row["TreeFileName"];
            this.SequencesFileName = (string)Row["SequencesFileName"];
            this.Model = ModelPreset.Derive((ModelPresets)Enum.Parse(typeof(ModelPresets), (string)Row["ModelPresetKey"]));
            this.NSSite = (int)Row["NSSite"];
            this.Kappa = Row.ToSafeDouble("Kappa");
            this.Omega = Row.ToSafeDouble("Omega");
            this.lnL = Row.ToSafeDouble("lnL");
            this.k = Row.ToSafeDouble("k");
            this.StartedAt = Row.ToSafeDateTime("StartedAt");
        }
    }

    public class ResultRow : RowDataItem
    {
        public override string ID { get { return string.Empty; } }
        public int ResultID { get; internal set; }
        public int AnalysisConfigurationID { get; internal set; }

        public string TreeTitle { get; internal set; }
        public string TreeFilePath { get; internal set; }
        public string SequencesFilePath { get; internal set; }
        public int SequenceCount { get; internal set; }
        public int SequenceLength { get; internal set; }

        internal ModelPreset Model { get; set; }
        public string ModelName
        {
            get
            {
                switch (this.Model.Key)
                {
                    case ModelPresets.Model0:
                        return this.Model.ShortName + " (M" + this.NSSite.ToString() + ")";
                    default:
                        return this.Model.ShortName;
                }
            }
        }
        public string ModelDescription
        {
            get
            {
                string description = string.Empty;

                switch (this.Model.Key)
                {
                    case ModelPresets.Model0:
                        description = this.Model.ShortName + " (M" + this.NSSite.ToString() + ")"; break;
                    default:
                        description = this.Model.ShortName; break;
                }

                return description;
            }
        }
        internal int NSSite { get; set; }

        public double Kappa { get; internal set; }
        public double Omega { get; internal set; }
        public string KappaOmegaStart { get { return Kappa.ToString(DataGridViewHelper.DefaultDoubleFormatString) + "/" + Omega.ToString(DataGridViewHelper.DefaultDoubleFormatString); } }

        public int np { get; internal set; }
        public double lnL { get; internal set; }
        public double k { get; internal set; }

        public string ValueHeader { get; internal set; }
        public string Value0 { get; internal set; }
        public string Value1 { get; internal set; }
        public string Value2a { get; internal set; }
        public string Value2b { get; internal set; }

        public DateTime CompletedAt { get; internal set; }

        public bool InRecordSet { get; internal set; }

        public ResultRow() { }

        public ResultRow(DataRow Row)
        {
            this.ResultID = (int)Row["ResultID"];
            this.AnalysisConfigurationID = (int)Row["AnalysisConfigurationID"];
            this.TreeTitle = (string)Row["Title"];
            this.Model = ModelPreset.Derive((ModelPresets)Enum.Parse(typeof(ModelPresets), (string)Row["ModelPresetKey"]));
            this.NSSite = (int)Row["NSSite"];
            this.Kappa = Row.ToSafeDouble("Kappa");
            this.Omega = Row.ToSafeDouble("Omega");
            this.np = (int)Row["np"];
            this.lnL = Row.ToSafeDouble("lnL");
            this.k = Row.ToSafeDouble("k");
            this.CompletedAt = Row.ToSafeDateTime("CompletedAt");
            this.InRecordSet = Row.ToSafeBoolean("InRecordSet");
        }

        public static List<ResultRow> ConvertToList(DataSet Results)
        {
            List<ResultRow> results = new List<ResultRow>();

            Results.Relations.Add(new DataRelation("FK_ResultID", Results.Tables[0].Columns["ResultID"], Results.Tables[1].Columns["ResultID"]));

            foreach (DataRow headerRow in Results.Tables[0].Rows)
            {
                ResultRow result = new ResultRow(headerRow);
                SetValues(headerRow, ref result);
                results.Add(result);
            }

            return results;
        }

        internal static void SetValues(DataRow HeaderRow, ref ResultRow Result)
        {
            ResultValueRow[] values = HeaderRow.GetChildRows("FK_ResultID").Select(row => new ResultValueRow(row)).ToArray();

            if (values.Length != 0)
            {
                if ((Result.Model.Key == ModelPresets.Model0 && (Result.NSSite == 2 || Result.NSSite == 3)) || Result.Model.Key == ModelPresets.Model2a)
                {
                    Result.ValueHeader = "p\r\nw";
                    Result.Value0 = ValueColumnString(values, "0");
                    Result.Value1 = ValueColumnString(values, "1");
                    Result.Value2a = ValueColumnString(values, "2");
                }
                else if ((Result.Model.Key == ModelPresets.Model0 && Result.NSSite == 8) || Result.Model.Key == ModelPresets.Model8a)
                {
                    Result.ValueHeader = "p\r\np1";
                    Result.Value0 = values.Where(v => v.ValueTypeName == "p" || v.ValueTypeName == "p1").Select(v => v.Value.ToString(DataGridViewHelper.DefaultDoubleFormatString)).Concatenate("\r\n");
                    Result.Value1 = "q\r\nw";
                    Result.Value2a = values.Where(v => v.ValueTypeName == "q" || v.ValueTypeName == "w").Select(v => v.Value.ToString(DataGridViewHelper.DefaultDoubleFormatString)).Concatenate("\r\n");
                }
                else
                {
                    switch (Result.Model.Key)
                    {
                        case ModelPresets.Model0:
                            switch (Result.NSSite)
                            {
                                case 0:
                                    Result.ValueHeader = values[0].ValueTypeName;
                                    Result.Value0 = values[0].Value.ToString(DataGridViewHelper.DefaultDoubleFormatString);
                                    break;
                                case 1:
                                    Result.ValueHeader = "p\r\nw";
                                    Result.Value0 = ValueColumnString(values, "0");
                                    Result.Value1 = ValueColumnString(values, "1");
                                    break;
                                case 7:
                                    Result.ValueHeader = "p\r\nq";
                                    Result.Value0 = ValueColumnString(values, "0");
                                    Result.Value1 = ValueColumnString(values, "1");
                                    break;
                            }
                            break;
                        case ModelPresets.Branch:
                        case ModelPresets.BranchNull:
                            Result.ValueHeader = "Background w\r\nForeground w";
                            Result.Value0 = ValueColumnString(values, "0", false);
                            break;
                        case ModelPresets.BranchSite:
                        case ModelPresets.BranchSiteNull:
                            Result.ValueHeader = "Site\r\nProportion\r\nBackground w\r\nForeground w";
                            Result.Value0 = ValueColumnString(values, "0", true);
                            Result.Value1 = ValueColumnString(values, "1", true);
                            Result.Value2a = ValueColumnString(values, "2a", true);
                            Result.Value2b = ValueColumnString(values, "2b", true);
                            break;
                        case ModelPresets.CmC:
                        case ModelPresets.CmCNull:
                        case ModelPresets.CmD:
                        case ModelPresets.CmDNull:
                            Result.ValueHeader = "Site\r\nProportion\r\n" + values.Where(v => v.ValueTypeName == "Branch Type").Select(v => v.ValueTypeName + " " + v.ValueRank.ToString()).Distinct().Concatenate("\r\n");
                            Result.Value0 = ValueColumnString(values, "0", true);
                            Result.Value1 = ValueColumnString(values, "1", true);
                            Result.Value2a = ValueColumnString(values, "2", true);
                            break;
                    }
                }
            }
            else
            {
                Result.ValueHeader = "null";
            }
        }

        private static string ValueColumnString(ResultValueRow[] Values, string SiteClass, bool IncludeSiteClass = false)
        {
            return (IncludeSiteClass ? SiteClass.ToString() + "\r\n" : string.Empty)
                + Values.Where(v => v.SiteClass == SiteClass).Select(v => v.Value.ToString(DataGridViewHelper.DefaultDoubleFormatString)).Concatenate("\r\n");
        }

        /// <summary>
        /// Facilitates building out the results strings for the "Value..." properties of a ResultsRow object.
        /// </summary>
        internal class ResultValueRow
        {
            internal int ResultID { get; set; }
            internal int TypeRank { get; set; }
            internal int ValueRank { get; set; }
            internal string SiteClass { get; set; }
            internal string ValueTypeName { get; set; }
            internal double Value { get; set; }

            internal ResultValueRow(DataRow Row)
            {
                this.ResultID = (int)Row["ResultID"];
                this.TypeRank = (int)Row["TypeRank"];
                this.ValueRank = (int)Row["ValueRank"];
                this.SiteClass = (string)Row["SiteClass"];
                this.ValueTypeName = (string)Row["ValueTypeName"];
                this.Value = Row.ToSafeDouble("Value");
            }
        }
    }

    public static class ExtensionMethods
    {
        internal static List<TreeConfigurationRowDataItem> ToRowDataItemList(this List<Tree> Value)
        {
            return Value.Aggregate(new List<TreeConfigurationRowDataItem>(), (current, tr) => { current.AddRange(tr.AnalysisConfigurations.Select(cf => new TreeConfigurationRowDataItem(tr, cf))); return current; });
        }

        internal static List<AnalysisConfigurationRowDataItem> ToRowDataItemList(this List<AnalysisConfiguration> Value)
        {
            return Value.Select(cf => new AnalysisConfigurationRowDataItem(cf)).ToList();
        }

        internal static List<CodeMLAnalysisOptionRowDataItem> ToRowDataItemList(this List<CodeMLAnalysisOption> Value)
        {
            return Value.Select(opt => new CodeMLAnalysisOptionRowDataItem(opt)).ToList();
        }
    }
}
