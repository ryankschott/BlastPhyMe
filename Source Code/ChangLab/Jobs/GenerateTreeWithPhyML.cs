using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using ChangLab.Common;
using ChangLab.Genes;
using ChangLab.RecordSets;

namespace ChangLab.Jobs
{
    public class GenerateTreeWithPhyML : CommandLineGeneProcessingJob
    {
        public PhyMLOptions Options { get; private set; }
        
        public string OriginalTreeFilePath
        {
            get { return this.GetAdditionalProperty("OriginalTreeFile"); }
            set { this.SetAdditionalProperty("OriginalTreeFile", value); }
        }
        public string UnlabeledTreeFilePath
        {
            get { return this.GetAdditionalProperty("UnlabeledTreeFile"); }
            set { this.SetAdditionalProperty("UnlabeledTreeFile", value); }
        }

        internal string SequenceHeaderFormat { get; private set; }

        public GenerateTreeWithPhyML(PhyMLOptions Options, string SequenceHeaderFormat, string PhyMLPath, bool KeepOutputFiles, RecordSet SourceRecordSet, SubSet SourceSubSet)
            : base(JobTargets.PhyML, SourceSubSet.ID, PhyMLPath, KeepOutputFiles)
        {
            this.Options = Options;
            this.SetAdditionalProperty("Options", Options.ToString());
            this.SequenceHeaderFormat = SequenceHeaderFormat;

            this.SourceRecordSet = SourceRecordSet;
            this.SourceSubSet = SourceSubSet;
        }

        public GenerateTreeWithPhyML(string JobID) : base(JobID)
        {
            if (!string.IsNullOrWhiteSpace(JobID))
            {
                this.Options = new PhyMLOptions(this.AdditionalPropertiesXml.ToString());
            }
        }

        public void GenerateTree()
        {
            try
            {
                UpdateStatus(JobStatuses.Running);

                // Record the InputGenes against the Job; it is assumed these were added to the local collection before GenerateTree() is called.
                this.InputGenes.ForEach(g => AddGene(g, GeneDirections.Input, false));

                // Clean up any invalid characters in the sequence headers
                Dictionary<string, string> sequenceHeaders = new Dictionary<string, string>();
                int arbitrarySequenceHeaderLength = 30;
                this.InputGenes.ForEach(g =>
                {
                    string sequenceHeader = Regex.Replace(g.ToFASTAHeader(this.SequenceHeaderFormat), "[\\\"\\\"\\,:()]", "")
                                                    .Replace(" ", "_");
                    if (sequenceHeader.Length > arbitrarySequenceHeaderLength) { sequenceHeader = sequenceHeader.Substring(0, arbitrarySequenceHeaderLength); }
                    sequenceHeaders.Add(g.ID, sequenceHeader);
                });

                // Sequence headers are capped in length, so we might end up with repeats, in which case we do this acrobatic grouping 
                // exercise to relabel any that repeat. This is problematic however, because the user doesn't have any way of knowing which
                // one is which.
                sequenceHeaders = sequenceHeaders
                    .GroupBy(kv => kv.Value)
                    .ToList()
                    .Select(grp => grp.Select((kv, index) => new { Values = kv, Index = (index + 1) }).ToList())
                    .Aggregate(new Dictionary<string, string>(), (current, grp) =>
                    {
                        if (grp.Count > 1)
                        {
                            grp.ForEach(kv => current.Add(kv.Values.Key, kv.Values.Value.Substring(0, kv.Values.Value.Length - (kv.Index.ToString().Length + 1)) + kv.Index.ToString()));
                        }
                        else
                        {
                            current.Add(grp[0].Values.Key, grp[0].Values.Value);
                        }

                        return current;
                    })
                    .ToDictionary(kv => kv.Key, kv => kv.Value);

                OnProgressUpdate(new ProgressUpdateEventArgs() { ProgressMessage = "Writing gene sequences to PHYLIP and FASTA files" });

                // Write a FASTA file in case that's something the user might want to use for troubleshooting
                string inputFileName = this.JobDirectory + "\\input.fas";
                using (System.IO.FileStream fs = new System.IO.FileStream(inputFileName, System.IO.FileMode.Create, System.IO.FileAccess.Write))
                {
                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(fs))
                    {
                        this.InputGenes.ForEach(g => sw.WriteLine(">" + sequenceHeaders[g.ID] + "\r\n" + g.Nucleotides));
                    }
                }

                // Write the sequences in this quasi-PHYLIP format that PAML prefers
                inputFileName = this.JobDirectory + "\\" + (this.SourceName + " - alignment for PAML.txt").ToSafeFileName();
                using (System.IO.FileStream fs = new System.IO.FileStream(inputFileName, System.IO.FileMode.Create, System.IO.FileAccess.Write))
                {
                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(fs))
                    {
                        sw.WriteLine(this.InputGenes.Count().ToString() + " " + this.InputGenes.First().Nucleotides.Length.ToString());
                        sw.WriteLine();

                        int leafLength = 60; // Arbitrarily chosen fragment length
                        int leafCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(this.InputGenes.First().Nucleotides.Length) / Convert.ToDouble(leafLength)));

                        this.InputGenes.ForEach(g =>
                        {
                            sw.WriteLine(sequenceHeaders[g.ID]);

                            for (int i = 0; i < leafCount; i++)
                            {
                                sw.WriteLine(g.Nucleotides.Skip(i * leafLength).Take(leafLength).Aggregate(string.Empty, (current, c) => current += c));
                            }

                            sw.WriteLine();
                        });
                    }
                }

                // Write the sequences as an interleaved PHYLIP file (http://www.bioperl.org/wiki/PHYLIP_multiple_alignment_format)
                inputFileName = this.JobDirectory + "\\input.phylip";
                using (System.IO.FileStream fs = new System.IO.FileStream(inputFileName, System.IO.FileMode.Create, System.IO.FileAccess.Write))
                {
                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(fs))
                    {
                        sw.WriteLine(this.InputGenes.Count().ToString() + " " + this.InputGenes.First().Nucleotides.Length.ToString());
                        sw.WriteLine();

                        int leafLength = 60; // Arbitrarily chosen fragment length
                        int leafCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(this.InputGenes.First().Nucleotides.Length) / Convert.ToDouble(leafLength)));

                        for (int i = 0; i < leafCount; i++)
                        {
                            this.InputGenes.ForEach(g =>
                            {
                                if (i == 0)
                                {
                                    sw.WriteLine(sequenceHeaders[g.ID] + "  " + g.Nucleotides.Substring(0, leafLength));
                                }
                                else
                                {
                                    sw.WriteLine(g.Nucleotides.Skip(i * leafLength).Take(leafLength).Aggregate(string.Empty, (current, c) => current += c));
                                }
                            });

                            sw.WriteLine(string.Empty); // Separator line
                        }
                    }
                }
                FileInfo inputFile = new FileInfo(inputFileName);

                OnProgressUpdate(new ProgressUpdateEventArgs() { ProgressMessage = "Inititalizing PhyML", Setup = true, CurrentMax = (Options.NumberOfInitialRandomStartingTrees + 1) });

                // Set up the PhyML process
                string arguments = "-i " + inputFile.Name + this.Options.OptionSwitches;
                using (CommandLineProcess = new Process() { EnableRaisingEvents = true })
                {
                    CommandLineProcess.StartInfo = new ProcessStartInfo("\"" + this.CommandLinePath + "\"", arguments)
                    {
                        CreateNoWindow = true,
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        WorkingDirectory = inputFile.DirectoryName
                    };
                    CommandLineProcess.OutputDataReceived += new DataReceivedEventHandler(Process_OutputDataReceived);
                    CommandLineProcess.Start();
                    CommandLineProcess.BeginOutputReadLine();
                    CommandLineProcess.WaitForExit();
                }
                if (this.CancellationPending) { return; }

                // Remove the support values if the user opted to have them removed.
                FileInfo outputFile = new FileInfo(inputFile.FullName + "_phyml_tree.txt");
                if (outputFile.Exists)
                {
                    if (this.Options.CopyWithoutSupportValues)
                    {
                        string treeFileContents = File.ReadAllText(outputFile.FullName);

                        string regex = "([0-1]{1}\\.[0-9]{1,})\\:+([0-1]{1}\\.[0-9]{1,})";
                        Match match = Regex.Match(treeFileContents, regex);
                        while (match.Success)
                        {
                            treeFileContents = treeFileContents.Remove(match.Index, treeFileContents.IndexOf(":", match.Index) - match.Index);

                            // We can't use Match.NextMatch() because the length of the string has changed.
                            match = Regex.Match(treeFileContents, regex);
                        }
                        
                        this.UnlabeledTreeFilePath = outputFile.DirectoryName + "\\" + this.SourceName + ".tre";
                        File.WriteAllText(UnlabeledTreeFilePath, treeFileContents);

#if !EEB460
                        this.OriginalTreeFilePath = outputFile.DirectoryName + "\\" + this.SourceName + " (with support values).tre";
                        outputFile.MoveTo(OriginalTreeFilePath);
#else
                        outputFile.Delete(); // So as to not confuse the students
#endif
                    }
                    else { this.OriginalTreeFilePath = outputFile.FullName; }

                    this.UpdateAdditionalProperties();
                }

                outputFile = new FileInfo(inputFile.FullName + "_phyml_stats.txt");
                if (outputFile.Exists)
                {
                    this.Output = File.ReadAllText(outputFile.FullName);
                    this.LogOutput();
                }

                // Clean-up - does this even make sense, considering that we're not pulling the tree file into the database yet?
                if (!this.KeepOutputFiles)
                {
                    try
                    {
                        OnProgressUpdate(new ProgressUpdateEventArgs() { ProgressMessage = "Cleaning up process files" });
                        Directory.Delete(this.JobDirectory);
                    }
                    catch (Exception ex)
                    {
                        OnProgressUpdate(new ProgressUpdateEventArgs() { ProgressMessage = "Unable to delete process files (" + ex.ToString() + ")" });
                    }
                }
                else
                {
                    Directory.CreateDirectory(this.JobDirectory + "\\input");

                    foreach (FileInfo file in (new DirectoryInfo(this.JobDirectory)).GetFiles("input.*"))
                    {
                        file.MoveTo(this.JobDirectory + "\\input\\" + file.Name);
                    }
                }
            }
            catch (Exception ex)
            {
                Jobs.JobException jex = new Jobs.JobException(this.ID, 0, "An error occured attempting to run PhyML. Review the PhyML output for additional details.", ex);
                jex.Save(); this.Exceptions.Add(jex);
            }
            finally
            {
                try
                {
                    OnProgressUpdate(new ProgressUpdateEventArgs() { Setup = true, CurrentMax = 10, CurrentProgress = 10 });

                    Complete(); // Close out the job's final status.

                    // Preferably we store the contents of the stats file, because it looks nice, but if the user cancels r some kind of error occurs
                    // and we don't make it that far, we'll store as the output whatever was created for progress messages.
                    if (string.IsNullOrWhiteSpace(this.Output) && this.ProgressMessages.Count != 0)
                    {
                        this.Output = this.ProgressMessages.Select(msg => msg.Message).Concatenate("\r\n");
                        this.LogOutput();
                    }
                }
                catch (Exception ex)
                {
                    UnhandledJobException(ex);
                }
            }
        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null
                && !e.Data.Contains("\b")) // For now we're excluding the per-iteration flurry of alignment messages
            {
                ProgressUpdateEventArgs args = new ProgressUpdateEventArgs() { ProgressMessage = e.Data };

                if (e.Data.Contains("[Random start"))
                {
                    args.CurrentProgress = int.Parse(Regex.Replace(e.Data, "[^0-9/]", "").Split(new char[] { '/' })[0]);
                }

                OnProgressUpdate(args);
            }
        }
    }

    public class PhyMLOptions : JobOptions
    {
        #region Properties
        /// <summary>
        /// Set as "-d " and then "nt" or "aa" accordingly.
        /// </summary>
        public PhyMLDataTypeOptions DataType
        {
            get { return _optionsRoot.SafeAttributeValueAsEnum<PhyMLDataTypeOptions>("DataType", PhyMLDataTypeOptions.Nucleotide); }
            set { _optionsRoot.SetAttributeValue("DataType", value); }
        }

        /// <remarks>
        /// Only expose this in the UI if we're giving the user the ability to choose their own input file. We'd only provide that kind of an option
        /// just to give the user a way to run PhyML through Pilgrimage but effectively disconnected from their data. In other words, just creating a
        /// UI wrapper around the PhyML command-line utility for the user's convenience, because there'd be no way to link up the sequences in the
        /// input file with the database.
        /// </remarks>
        public bool SequentialInput
        {
            get { return _optionsRoot.SafeAttributeValueAsBool("SequentialInput", false); }
            set { _optionsRoot.SetAttributeValue("SequentialInput", value); }
        }

        /// <remarks>
        /// Similarly to <seealso cref="ChangLab.Jobs.PhyMLOptions.SequentialInput"/> this only applies if we're letting the user provide their own
        /// input file, because we're only ever going to run a single dataset of sequences per job.
        /// </remarks>
        public int NumberOfDataSets
        {
            get { return _optionsRoot.SafeAttributeValueAsInt("NumberOfDataSets", 1); }
            set { _optionsRoot.SetAttributeValue("NumberOfDataSets", value); }
        }

        /// <remarks>
        /// These two are separated, even though for PhyML itself it's one command-line switch, so that we can simply provide the user a combo-box
        /// that loads one or the other enum as its data source depending on the DataType selection.
        /// </remarks>
        public PhyMLNucleotideSubstitutionModels NucleotideSubstitutionModel
        {
            get { return _optionsRoot.SafeAttributeValueAsEnum<PhyMLNucleotideSubstitutionModels>("NucleotideSubstitutionModel", PhyMLNucleotideSubstitutionModels.HKY85); }
            set { _optionsRoot.SetAttributeValue("NucleotideSubstitutionModel", value); }
        }

        /// <remarks>
        /// These two are separated, even though for PhyML itself it's one command-line switch, so that we can simply provide the user a combo-box
        /// that loads one or the other enum as its data source depending on the DataType selection.
        /// </remarks>
        public PhyMLAminoAcidSubstitutionModels AminoAcidSubstitutionModel
        {
            get { return _optionsRoot.SafeAttributeValueAsEnum<PhyMLAminoAcidSubstitutionModels>("AminoAcidSubstitutionModel", PhyMLAminoAcidSubstitutionModels.LG); }
            set { _optionsRoot.SetAttributeValue("AminoAcidSubstitutionModel", value); }
        }

        /// <summary>
        /// If true, the option switch "-f e" should be employed for Empirical.  If false, "-f m" for maximum likelihood/substitution model.
        /// </summary>
        public bool EmpiricalEquilibriumFrequencies
        {
            get { return _optionsRoot.SafeAttributeValueAsBool("EmpiricalEquilibriumFrequencies", true); }
            set { _optionsRoot.SetAttributeValue("EmpiricalEquilibriumFrequencies", value); }
        }

        /// <summary>
        /// Should only be applied as "-f fA,fC,fG,fT" if not null.
        /// </summary>
        public Tuple<float, float, float, float> NucleotideFrequencies
        {
            get
            {
                string[] values = _optionsRoot.SafeAttributeValue("NucleotideFrequencies").Split(new char[] { ',' });
                if (values.Length == 4)
                {
                    try 
                    {
                        return new Tuple<float, float, float, float>(
                            float.Parse(values[0]),
                            float.Parse(values[1]),
                            float.Parse(values[2]),
                            float.Parse(values[3])
                        );
                    }
                    catch
                    {
                        _optionsRoot.SetAttributeValue("NucleotideFrequencies", string.Empty); // Clean up the XML for next time.
	                    return null;
                    }
                }
                else { return null; } // Not perfect, but if the attribute doesn't exist, is empty, or isn't four values, we assume it was not set.
            }
            set
            {
                if (value != null)
                { _optionsRoot.SetAttributeValue("NucleotideFrequencies", string.Format("{0},{1},{2},{3}", value.Item1, value.Item2, value.Item3, value.Item4)); }
                else
                { _optionsRoot.SetAttributeValue("NucleotideFrequencies", string.Empty); }
            }
        }

        /// <summary>
        /// If true, set "-t e", if false, use "-t" with the TransitionTransversionRatio property value, only if DataType == Nucleotide.
        /// </summary>
        public bool EstimateTransitionTransversionRatio
        {
            get { return _optionsRoot.SafeAttributeValueAsBool("EstimateTransitionTransversionRatio", true); }
            set { _optionsRoot.SetAttributeValue("EstimateTransitionTransversionRatio", value); }
        }

        /// <summary>
        /// If EstimateTransitionTransversionRatio is false, apply set "-t {value}", only if DataType == Nucleotide.
        /// </summary>
        public float TransitionTransversionRatio
        {
            get { return _optionsRoot.SafeAttributeValueAsFloat("TransitionTransversionRatio"); }
            set { _optionsRoot.SetAttributeValue("TransitionTransversionRatio", value); }
        }

        /// <summary>
        /// If true, set "-v e", if false, use "-v" with the ProportionOfInvariableSites property value.
        /// </summary>
        public bool EstimateProportionOfInvariableSites
        {
            get { return _optionsRoot.SafeAttributeValueAsBool("EstimateProportionOfInvariableSites", true); }
            set { _optionsRoot.SetAttributeValue("EstimateProportionOfInvariableSites", value); }
        }

        /// <summary>
        /// If EstimateProportionOfInvariableSites is false, apply set "-v {value}".
        /// </summary>
        public float ProportionOfInvariableSites
        {
            get { return _optionsRoot.SafeAttributeValueAsFloat("ProportionOfInvariableSites"); }
            set { _optionsRoot.SetAttributeValue("ProportionOfInvariableSites", value); }
        }

        /// <summary>
        /// Must be a positive integer. Set "-c {value}".
        /// </summary>
        public int NumberOfSubstitutionRateCategories
        {
            get { return _optionsRoot.SafeAttributeValueAsInt("NumberOfSubstitutionRateCategories", 5); }
            set
            {
                if (value > 0) { _optionsRoot.SetAttributeValue("NumberOfSubstitutionRateCategories", value); }
                else { throw new ArgumentOutOfRangeException("NumberOfSubstitutionRateCategories", value, "Number of relative substitution rate categories must be a positive integer."); }
            }
        }

        /// <summary>
        /// If true, set "-a e", if false, use "-a" with the GammaShapeParameter property value.
        /// </summary>
        public bool EstimateGammaShapeParameter
        {
            get { return _optionsRoot.SafeAttributeValueAsBool("EstimateGammaShapeParameter", true); }
            set { _optionsRoot.SetAttributeValue("EstimateGammaShapeParameter", value); }
        }

        /// <summary>
        /// If EstimateGammaShapeParameter is false, apply set "-a {value}".
        /// </summary>
        public float GammaShapeParameter
        {
            get { return _optionsRoot.SafeAttributeValueAsFloat("GammaShapeParameter"); }
            set { _optionsRoot.SetAttributeValue("GammaShapeParameter", value); }
        }

        public bool UseStartingTree
        {
            get { return _optionsRoot.SafeAttributeValueAsBool("UseStartingTree", false); }
            set { _optionsRoot.SetAttributeValue("UseStartingTree", value); }
        }

        /// <summary>
        /// If not empty, and the file exists, set "-u {value}".
        /// </summary>
        public string UserTreeFile
        {
            get { return _optionsRoot.SafeAttributeValue("UserTreeFile"); }
            set { _optionsRoot.SetAttributeValue("UserTreeFile", value); }
        }

        /// <summary>
        /// Set "-s {value}".
        /// </summary>
        public PhyMLTreeTopologySearchOptions TreeTopologySearchOption
        {
            get { return _optionsRoot.SafeAttributeValueAsEnum<PhyMLTreeTopologySearchOptions>("TreeTopologySearchOption", PhyMLTreeTopologySearchOptions.BEST); }
            set { _optionsRoot.SetAttributeValue("TreeTopologySearchOption", value); }
        }

        /// <summary>
        /// Must be a positive integer. Set "--n_randstars {value}". Do not set if TreeTopologySearchOption = NNI.
        /// </summary>
        public int NumberOfInitialRandomStartingTrees
        {
            get { return _optionsRoot.SafeAttributeValueAsInt("NumberOfInitialRandomStartingTrees", 5); }
            set
            {
                if (value > 0) { _optionsRoot.SetAttributeValue("NumberOfInitialRandomStartingTrees", value); }
                else { throw new ArgumentOutOfRangeException("NumberOfInitialRandomStartingTrees", value, "Number of initial random starting trees must be a positive integer."); }
            }
        }

        /// <summary>
        /// If true, set "-o params=t", taking into account OptimizeBranchLengths and OptimizeSubstitutionRateParameters, which share the same switch.
        /// If all three parameter optimization properties are set to false, set "-o params=n".
        /// </summary>
        public bool OptimizeTopology
        {
            get { return _optionsRoot.SafeAttributeValueAsBool("OptimizeTopology", true); }
            set { _optionsRoot.SetAttributeValue("OptimizeTopology", value); }
        }

        /// <summary>
        /// If true, set "-o params=l", taking into account OptimizeTopology and OptimizeSubstitutionRateParameters, which share the same switch.
        /// If all three parameter optimization properties are set to false, set "-o params=n".
        /// </summary>
        public bool OptimizeBranchLengths
        {
            get { return _optionsRoot.SafeAttributeValueAsBool("OptimizeBranchLength", true); }
            set { _optionsRoot.SetAttributeValue("OptimizeBranchLength", value); }
        }

        /// <summary>
        /// If true, set "-o params=r", taking into account OptimizeTopology and OptimizeBranchLengths, which share the same switch.
        /// If all three parameter optimization properties are set to false, set "-o params=n".
        /// </summary>
        public bool OptimizeSubstitutionRateParameters
        {
            get { return _optionsRoot.SafeAttributeValueAsBool("OptimizeSubstitutionRateParameters", false); }
            set { _optionsRoot.SetAttributeValue("OptimizeSubstitutionRateParameters", value); }
        }

        /// <summary>
        /// If true, PhyML will not stop to prompt the user to continue when memory usage exceeds its a certain amount.
        /// For Pilgrimage, since we're not talking back to the shell execution, this must be left as true.
        /// Set "--no_memory_check".
        /// </summary>
        public bool NoMemoryCheck
        {
            get { return _optionsRoot.SafeAttributeValueAsBool("NoMemoryCheck", true); }
            set { _optionsRoot.SetAttributeValue("NoMemoryCheck", value); }
        }

        /// <summary>
        /// Employed via "-b {BootstrapSwitchValue}".
        /// </summary>
        public bool PerformBootstrap
        {
            get { return _optionsRoot.SafeAttributeValueAsBool("PerformBootstrap", false); }
            set { _optionsRoot.SetAttributeValue("PerformBootstrap", value); }
        }

        /// <summary>
        /// Must be a positive integer. Employed via "-b {BootstrapSwitchValue}".
        /// </summary>
        public int NumberOfBootstrapReplicates
        {
            get { return _optionsRoot.SafeAttributeValueAsInt("NumberOfBootstrapReplicates", 1); }
            set
            {
                if (value > 0) { _optionsRoot.SetAttributeValue("NumberOfBootstrapReplicates", value); }
                else { throw new ArgumentOutOfRangeException("NumberOfBootstrapReplicates", value, "Number of bootstrap replicates must be a positive integer."); }
            }
        }

        /// <summary>
        /// Employed via "-b {BootstrapSwitchValue}".
        /// </summary>
        public bool PerformFastLikelihoodBasedMethod
        {
            get { return _optionsRoot.SafeAttributeValueAsBool("PerformApproximateLikelihoodRatioTest", true); }
            set { _optionsRoot.SetAttributeValue("PerformApproximateLikelihoodRatioTest", value); }
        }

        /// <summary>
        /// Employed via "-b {BootstrapSwitchValue}".
        /// </summary>
        public PhyMLFastLikelihoodBasedMethods FastLikelihoodBasedMethod
        {
            get { return _optionsRoot.SafeAttributeValueAsEnum<PhyMLFastLikelihoodBasedMethods>("FastLikelihoodBasedMethod", PhyMLFastLikelihoodBasedMethods.SHlike); }
            set { _optionsRoot.SetAttributeValue("FastLikelihoodBasedMethod", value); }
        }

        /// <summary>
        /// Computes the value for the "-b" switch based on the related options.
        /// </summary>
        public int BootstrapSwitchValue
        {
            get
            {
                if (PerformBootstrap)
                {
                    return this.NumberOfBootstrapReplicates;
                }
                else if (!PerformFastLikelihoodBasedMethod)
                {
                    return 0;
                }
                else
                {
                    switch (FastLikelihoodBasedMethod)
                    {
                        case PhyMLFastLikelihoodBasedMethods.aLRT: return -1;
                        case PhyMLFastLikelihoodBasedMethods.aLRTChi2: return -2;
                        case PhyMLFastLikelihoodBasedMethods.SHlike: return -4;
                        default: return 0;
                    }
                }
            }
        }

        /// <summary>
        /// If true, the job will create an unlabeled copy of the tree file.
        /// </summary>
        public bool CopyWithoutSupportValues
        {
            get { return _optionsRoot.SafeAttributeValueAsBool("CopyWithoutSupportValues", true); }
            set { _optionsRoot.SetAttributeValue("CopyWithoutSupportValues", value); }
        }

        public override string OptionSwitches
        {
            get
            {
                string switches =
                    " -d " + (this.DataType == PhyMLDataTypeOptions.Nucleotide ? "nt" : "aa")
                    + " -m " + (this.DataType == PhyMLDataTypeOptions.Nucleotide ? this.NucleotideSubstitutionModel.ToString() : this.AminoAcidSubstitutionModel.ToString())
                    + " -f " + (this.NucleotideFrequencies == null
                                ? (this.EmpiricalEquilibriumFrequencies ? "e" : "f")
                                : (string.Format("{0},{1},{2},{3}", this.NucleotideFrequencies.Item1, this.NucleotideFrequencies.Item2, this.NucleotideFrequencies.Item3, this.NucleotideFrequencies.Item4)))
                    + " -t " + (this.EstimateTransitionTransversionRatio ? "e" : this.TransitionTransversionRatio.ToString())
                    + " -c " + this.NumberOfSubstitutionRateCategories.ToString()
                    + " -v " + (this.EstimateProportionOfInvariableSites ? "e" : this.ProportionOfInvariableSites.ToString())
                    + " -a " + (this.EstimateGammaShapeParameter ? "e" : this.GammaShapeParameter.ToString())
                    + " " + (System.IO.File.Exists(this.UserTreeFile) ? "-u \"" + this.UserTreeFile + "\"" : string.Empty)
                    + " -s " + this.TreeTopologySearchOption.ToString()
                    + " " + (this.TreeTopologySearchOption != PhyMLTreeTopologySearchOptions.NNI ? "--n_rand_starts " + this.NumberOfInitialRandomStartingTrees.ToString() : "")
                    + " -o " + (
                        (!this.OptimizeTopology && !this.OptimizeBranchLengths && !this.OptimizeSubstitutionRateParameters)
                        ? "n"
                        : (this.OptimizeTopology ? "t" : "") + (this.OptimizeBranchLengths ? "l" : "") + (this.OptimizeSubstitutionRateParameters ? "r" : ""))
                    + " -b " + this.BootstrapSwitchValue.ToString()
                    + " " + (this.NoMemoryCheck ? "--no_memory_check" : "")
                ;

                return switches;
            }
        }
        #endregion

        public PhyMLOptions() : base() { }

        public PhyMLOptions(string XML) : base(XML) { }

        public override string ToString()
        {
            return base.ToString();
        }

        public static Dictionary<PhyMLNucleotideSubstitutionModels, string> NucleotideSubstitutionModelsList()
        {
            string[] names = Enum.GetNames(typeof(PhyMLNucleotideSubstitutionModels));

            return
                Enum.GetValues(typeof(PhyMLNucleotideSubstitutionModels))
                .Cast<PhyMLNucleotideSubstitutionModels>()
                .Select((model, index) => new { Model = model, Name = names[index] })
                .ToDictionary(kv => kv.Model, kv => kv.Name);
        }

        public static Dictionary<PhyMLTreeTopologySearchOptions, string> TreeTopologySearchOptionsList()
        {
            Dictionary<PhyMLTreeTopologySearchOptions, string> list = new Dictionary<PhyMLTreeTopologySearchOptions, string>();
            list.Add(PhyMLTreeTopologySearchOptions.NNI, "NNI");
            list.Add(PhyMLTreeTopologySearchOptions.SPR, "SPR");
            list.Add(PhyMLTreeTopologySearchOptions.BEST, "SPR & NNI");
            return list;
        }

        public static Dictionary<PhyMLFastLikelihoodBasedMethods, string> FastLikelihoodBasedMethodsList()
        {
            Dictionary<PhyMLFastLikelihoodBasedMethods, string> list = new Dictionary<PhyMLFastLikelihoodBasedMethods, string>();
            list.Add(PhyMLFastLikelihoodBasedMethods.aLRT, "aLRT");
            list.Add(PhyMLFastLikelihoodBasedMethods.aLRTChi2, "aLRT with Chi2-based branch supports");
            list.Add(PhyMLFastLikelihoodBasedMethods.SHlike, "SH-like branch supports alone");
            return list;
        }
    }

    public enum PhyMLDataTypeOptions
    {
        Nucleotide,
        AminoAcid
    }

    public enum PhyMLNucleotideSubstitutionModels
    {
        HKY85,
        JC69,
        K80,
        F81,
        F84,
        TN93,
        GTR,
        Custom
    }

    public enum PhyMLAminoAcidSubstitutionModels
    {
        LG,
        WAG,
        JTT,
        MtREV,
        Dayhoff,
        DCMut,
        RtREV,
        CpREV,
        VT,
        Blosum62,
        MtMam,
        MtArt,
        HIVw,
        HIVb,
        Custom
    }

    public enum PhyMLTreeTopologySearchOptions
    {
        NNI,
        SPR,
        BEST
    }

    public enum PhyMLFastLikelihoodBasedMethods
    {
        aLRT,
        aLRTChi2,
        SHlike
    }
}