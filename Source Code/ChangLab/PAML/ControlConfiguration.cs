using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChangLab.Common;
using ChangLab.Jobs;
using ChangLab.PAML.CodeML;

namespace ChangLab.PAML
{
    /// <summary>
    /// A repository for the default settings for a codeml control file.
    /// </summary>
    /// <remarks>
    /// In Pilgrimage the control file settings are split into a hierarchy such that the same tree and sequence file combination can be run with
    /// multiple configurations.  As such, when configuring the default settings for a ChangLab.PAML.CodeML.Tree class's Configuration property, only
    /// those properties that are applicable across a tree and sequence file combination should be taken from a new instance of this file (or from 
    /// the application/database-level default copy).
    /// </remarks>
    public class ControlConfiguration : JobOptions
    {
        public int Verbose
        {
            get { return _optionsRoot.SafeAttributeValueAsInt("Verbose", 1); }
            set
            {
                if (!(new int[] { 0, 1, 2 }).Contains(value)) { throw new ArgumentOutOfRangeException("Verbose", "Invalid value for Verbose property."); }
                else { _optionsRoot.SetAttributeValue("Verbose", value); }
            }
        }

        public int RunMode
        {
            get { return _optionsRoot.SafeAttributeValueAsInt("RunMode", 0); }
            set
            {
                if (!(new int[] { -2, 0, 1, 2, 3, 4, 5 }).Contains(value)) { throw new ArgumentOutOfRangeException("RunMode", "Invalid value for runmode property."); }
                else { _optionsRoot.SetAttributeValue("RunMode", value); }
            }
        }

        public int SequenceType
        {
            get { return _optionsRoot.SafeAttributeValueAsInt("SequenceType", 1); }
            set
            {
                if (!(new int[] { 1, 2, 3 }).Contains(value)) { throw new ArgumentOutOfRangeException("SequenceType", "Invalid value for seqtype property."); }
                else { _optionsRoot.SetAttributeValue("SequenceType", value); }
            }
        }

        public int CodonFrequency
        {
            get { return _optionsRoot.SafeAttributeValueAsInt("CodonFrequency", 2); }
            set
            {
                if (!(new int[] { 0, 1, 2, 3 }).Contains(value)) { throw new ArgumentOutOfRangeException("CodonFrequency", "Invalid value for codon frequency property."); }
                else { _optionsRoot.SetAttributeValue("CodonFrequency", value); }
            }
        }

        public int Clock
        {
            get { return _optionsRoot.SafeAttributeValueAsInt("Clock", 0); }
            set
            {
                if (!(new int[] { 0, 1, 2, 3 }).Contains(value)) { throw new ArgumentOutOfRangeException("Clock", "Invalid value for clock property."); }
                else { _optionsRoot.SetAttributeValue("Clock", value); }
            }
        }

        public ModelPresets Model
        {
            get { return _optionsRoot.SafeAttributeValueAsEnum<ModelPresets>("Model", ModelPresets.Model0); }
            set { _optionsRoot.SetAttributeValue("Model", value.ToString()); }
        }

        /// <remarks>
        /// The default value of -1 indicates that these values should take on the first allowable value of the default model.
        /// </remarks>
        public int NSSites
        {
            get { return _optionsRoot.SafeAttributeValueAsInt("NSSites", -1); }
            set { _optionsRoot.SetAttributeValue("NSSites", value); }
        }

        public int ICode
        {
            get { return _optionsRoot.SafeAttributeValueAsInt("ICode", 0); }
            set
            {
                if (!(Enumerable.Range(0, 11)).Contains(value)) { throw new ArgumentOutOfRangeException("ICode", "Invalid value for icode property."); }
                else { _optionsRoot.SetAttributeValue("ICode", value); }
            }
        }

        public int MGene
        {
            get { return _optionsRoot.SafeAttributeValueAsInt("MGene", 0); }
            set
            {
                if (!(Enumerable.Range(0, 4)).Contains(value)) { throw new ArgumentOutOfRangeException("MGene", "Invalid value for mgene property."); }
                else { _optionsRoot.SetAttributeValue("MGene", value); }
            }
        }

        public RangeWithInterval Kappa
        {
            get { return RangeWithInterval.FromString(_optionsRoot.SafeAttributeValue("Kappa", "2|3|1.0|false")); }
            set { _optionsRoot.SetAttributeValue("Kappa", value.ToString()); }
        }

        public RangeWithInterval Omega
        {
            get { return RangeWithInterval.FromString(_optionsRoot.SafeAttributeValue("Omega", "0|2|1.0|false")); }
            set { _optionsRoot.SetAttributeValue("Omega", value.ToString()); }
        }

        /// <summary>
        /// Different alphas for genes.
        /// </summary>
        public bool MAlpha
        {
            get { return _optionsRoot.SafeAttributeValueAsBool("MAlpha", false); }
            set { _optionsRoot.SetAttributeValue("MAlpha", value); }
        }

        /// <remarks>
        /// The default value of -1 indicates that these values should take on the first allowable value of the default model.
        /// </remarks>
        public int NCatG
        {
            get { return _optionsRoot.SafeAttributeValueAsInt("NCatG", -1); }
            set { _optionsRoot.SetAttributeValue("NCatG", value); }
        }

        /// <summary>
        /// Standard Errors of estimates.
        /// </summary>
        public bool GetSE
        {
            get { return _optionsRoot.SafeAttributeValueAsBool("GetSE", false); }
            set { _optionsRoot.SetAttributeValue("GetSE", value); }
        }

        /// <summary>
        /// If true, ancestral states are recorded, as are posterior estimates for site-specific rates.
        /// </summary>
        public bool RateAncestor
        {
            get { return _optionsRoot.SafeAttributeValueAsBool("RateAncestor", false); }
            set { _optionsRoot.SetAttributeValue("RateAncestor", value); }
        }

        public bool CleanData
        {
            get { return _optionsRoot.SafeAttributeValueAsBool("CleanData", false); }
            set { _optionsRoot.SetAttributeValue("CleanData", value); }
        }

        /// <summary>
        /// Optimization method.
        /// </summary>
        public int Method
        {
            get { return _optionsRoot.SafeAttributeValueAsInt("Method", 1); }
            set
            {
                if (!(new int[] { 0, 1 }).Contains(value)) { throw new ArgumentOutOfRangeException("Method", "Invalid value for method property."); }
                else { _optionsRoot.SetAttributeValue("Method", value); }
            }
        }

        public ControlConfiguration() : base() { }
        public ControlConfiguration(string XML) : base(XML) { }

        /// <summary>
        /// Returns a deep copy of the instance.
        /// </summary>
        public ControlConfiguration Copy()
        {
            ControlConfiguration copy = (ControlConfiguration)this.MemberwiseClone();
            copy.Kappa = this.Kappa.Copy();
            copy.Omega = this.Omega.Copy();
            return copy;
        }

        public static Tree FromCustomControlFile(string FilePath)
        {
            Tree tree = new Tree();
            ModelPreset preset = ModelPreset.Derive(tree.Configuration.Model);
            AnalysisConfiguration analysis = new AnalysisConfiguration()
            {
                Model = preset.Model,
                ModelPresetID = preset.ID,
                NCatG = preset.NCatG,
                KStart = tree.Configuration.Kappa.Start,
                KEnd = tree.Configuration.Kappa.End,
                KInterval = tree.Configuration.Kappa.Interval,
                FixedKappa = tree.Configuration.Kappa.Fixed,
                WStart = tree.Configuration.Omega.Start,
                WEnd = tree.Configuration.Omega.End,
                WInterval = tree.Configuration.Omega.Interval,
                FixedOmega = tree.Configuration.Omega.Fixed
            };
            tree.AnalysisConfigurations.Add(analysis);

            List<string> lines = System.IO.File.ReadAllLines(FilePath).ToList();
            foreach (string line in lines)
            {
                string[] pieces = line.Split(new char[] { '=' });
                if (pieces.Length > 1)
                {
                    string value = pieces[1];
                    if (value.Contains("*")) { value = value.Substring(0, value.IndexOf("*")); }
                    value = System.Text.RegularExpressions.Regex.Replace(value.Trim(), "[^0-9\\ ]", "");

                    switch (pieces[0].Trim().ToLower())
                    {
                        // Analysis option properties
                        case "model": analysis.Model = value.ToSafeInt(analysis.Model); break;
                        case "nssites":
                            analysis.NSSites = value.Split(new char[] { ' ' }).Where(v => { int nsSite = 0; return int.TryParse(v, out nsSite); }).Select(v => int.Parse(v)).ToList();
                            if (analysis.NSSites.Count == 0) { analysis.NSSites = preset.NSSites; }
                            break;
                        case "ncatg": analysis.NCatG = value.ToSafeInt(analysis.NCatG); break;
                        
                        case "kappa":
                            analysis.KStart = value.ExtractDouble(analysis.KStart);
                            analysis.KEnd = value.ExtractDouble(analysis.KEnd);
                            break;
                        case "fix_kappa": analysis.FixedKappa = (value == "1"); break;
                        case "omega":
                            analysis.WStart = value.ExtractDouble(analysis.WStart);
                            analysis.WEnd = value.ExtractDouble(analysis.WEnd);
                            break;
                        case "fix_omega": analysis.FixedOmega = (value == "1"); break;

                        // Configuration properties
                        case "verbose": tree.Configuration.Verbose = value.ToSafeInt(tree.Configuration.Verbose); break;
                        case "runmode": tree.Configuration.RunMode = value.ToSafeInt(tree.Configuration.RunMode); break;
                        case "seqtype": tree.Configuration.SequenceType = value.ToSafeInt(tree.Configuration.SequenceType); break;
                        case "codonfreq": tree.Configuration.CodonFrequency = value.ToSafeInt(tree.Configuration.CodonFrequency); break;
                        case "clock": tree.Configuration.Clock = value.ToSafeInt(tree.Configuration.Clock); break;
                        case "icode": tree.Configuration.ICode = value.ToSafeInt(tree.Configuration.ICode); break;
                        case "mgene": tree.Configuration.MGene = value.ToSafeInt(tree.Configuration.MGene); break;
                        case "method": tree.Configuration.Method = value.ToSafeInt(tree.Configuration.Method); break;

                        case "malpha": tree.Configuration.MAlpha = (value == "1"); break;
                        case "getse": tree.Configuration.GetSE = (value == "1"); break;
                        case "rateancestor": tree.Configuration.RateAncestor = (value == "1"); break;
                        case "cleandata": tree.Configuration.CleanData = (value == "1"); break;
                    }
                }
            }

            return tree;
        }

        #region Static Collections
        private static Dictionary<int, string> _runModes;
        public static Dictionary<int, string> RunModes
        {
            get
            {
                if (_runModes == null)
                {
                    _runModes = new Dictionary<int, string>();
                    _runModes.Add(0, "User tree");
                    _runModes.Add(1, "Semi-automatic");
                    _runModes.Add(2, "Automatic");
                    _runModes.Add(3, "Stepwise addition");
                    _runModes.Add(4, "Perturbation NNI (4)");
                    _runModes.Add(5, "Perturbation NNI (5)");
                    _runModes.Add(-2, "Pairwise");
                }

                return _runModes;
            }
        }

        private static Dictionary<int, string> _sequenceTypes;
        public static Dictionary<int, string> SequenceTypes
        {
            get
            {
                if (_sequenceTypes == null)
                {
                    _sequenceTypes = new Dictionary<int, string>();
                    _sequenceTypes.Add(1, "Codons");
                    _sequenceTypes.Add(2, "Amino Acids");
                    _sequenceTypes.Add(3, "Translated Codons");
                }

                return _sequenceTypes;
            }
        }

        private static Dictionary<int, string> _codonFrequencies;
        public static Dictionary<int, string> CodonFrequencies
        {
            get
            {
                if (_codonFrequencies == null)
                {
                    _codonFrequencies = new Dictionary<int,string>();
                    _codonFrequencies.Add(0, "Equal");
                    _codonFrequencies.Add(1, "F1x4");
                    _codonFrequencies.Add(2, "F3x4");
                    _codonFrequencies.Add(3, "Fcodon/F61");
                }

                return _codonFrequencies;
            }
        }

        private static Dictionary<int, string> _clocks;
        public static Dictionary<int, string> Clocks
        {
            get
            {
                if (_clocks == null)
                {
                    _clocks = new Dictionary<int, string>();
                    _clocks.Add(0, "No clock");
                    _clocks.Add(1, "Clock");
                    _clocks.Add(2, "Local clock");
                    _clocks.Add(3, "Combined analysis");
                }

                return _clocks;
            }
        }
        
        private static Dictionary<int, string> _methods;
        public static Dictionary<int, string> Methods
        {
            get
            {
                if (_methods == null)
                {
                    _methods = new Dictionary<int, string>();
                    _methods.Add(0, "Simultaneous");
                    _methods.Add(1, "One branch");
                }

                return _methods;
            }
        }

        private static Dictionary<int, string> _verboseModes;
        public static Dictionary<int, string> VerboseModes
        {
            get
            {
                if (_verboseModes == null)
                {
                    _verboseModes = new Dictionary<int, string>();
                    _verboseModes.Add(0, "Concise");
                    _verboseModes.Add(1, "Detailed");
                    _verboseModes.Add(2, "All");
                }

                return _verboseModes;
            }
        }

        private static Dictionary<int, string> _iCodes;
        public static Dictionary<int, string> ICodes
        {
            get
            {
                if (_iCodes == null)
                {
                    _iCodes = new Dictionary<int, string>();
                    _iCodes.Add(0, "Universal");
                    _iCodes.Add(1, "Mammalian MT");
                    _iCodes.Add(2, "Yeast MT");
                    _iCodes.Add(3, "Mold MT");
                    _iCodes.Add(4, "Invertebrate MT");
                    _iCodes.Add(5, "Ciliate Nuclear");
                    _iCodes.Add(6, "Echinoderm MT");
                    _iCodes.Add(7, "Euplotid MT");
                    _iCodes.Add(8, "Alternative Yeast Nuclear");
                    _iCodes.Add(9, "Ascidian MT");
                    _iCodes.Add(10, "Blepharisma Nuclear");
                }

                return _iCodes;
            }
        }

        private static Dictionary<int, string> _mGene_Codon;
        public static Dictionary<int, string> MGene_Codon
        {
            get
            {
                if (_mGene_Codon == null)
                {
                    _mGene_Codon = new Dictionary<int, string>();
                    _mGene_Codon.Add(0, "Rates");
                    _mGene_Codon.Add(1, "Separate");
                    _mGene_Codon.Add(2, "Diff Pi");
                    _mGene_Codon.Add(3, "Diff Kappa");
                    _mGene_Codon.Add(4, "All Diff");
                }

                return _mGene_Codon;
            }
        }

        private static Dictionary<int, string> _mGene_AA;
        public static Dictionary<int, string> MGene_AA
        {
            get
            {
                if (_mGene_AA == null)
                {
                    _mGene_AA = new Dictionary<int, string>();
                    _mGene_AA.Add(0, "Rates");
                    _mGene_AA.Add(1, "Separate");
                }

                return _mGene_AA;
            }
        }
        #endregion
    }
}