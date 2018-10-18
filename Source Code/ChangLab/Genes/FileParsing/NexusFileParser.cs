using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChangLab.Common;

namespace ChangLab.Genes.FileParsing
{
    public class NexusFileParser
    {
        private List<string> _lines;
        private NexusFileContent _content;

        public List<Gene> ParseFile(string FilePath)
        {
            System.IO.FileInfo file = new System.IO.FileInfo(FilePath);
            List<Gene> genes = new List<Gene>();
            try
            {
                _lines = System.IO.File.ReadAllLines(FilePath).ToList();
                if (_lines[0].TrimEnd() != "#NEXUS") { throw new Exception("File is not a valid NEXUS format."); }

                _content = new NexusFileContent();
                NexusBlockTypes currentBlockType = NexusBlockTypes.Unknown;
                
                List<string> taxaLabels = new List<string>();

                for (int i = 0; i < _lines.Count; i++)
                {
                    if (_lines[i].Trim().StartsWith("[")) { continue; } // Assume this is a 

                    if (_lines[i].ToLower().StartsWith("begin "))
                    {
                        // New block
                        if (Enum.TryParse<NexusBlockTypes>(_lines[i].Substring(6).Replace(";", ""), true, out currentBlockType))
                        {
                            // It's a valid block of a type that we care about parsing. The next few lines should be properties of the block' data.
                            i++;
                        }
                        else
                        {
                            // It's not a block that we're interested in, so skip ahead to when it closes
                            while (_lines[i] != "end;")
                            {
                                if (i + 1 == _lines.Count) { break; }
                                else { i++; }
                            }
                        }
                    }

                    if (currentBlockType != NexusBlockTypes.Unknown)
                    {
                        switch (currentBlockType)
                        {
                            case NexusBlockTypes.Taxa:
                                if (_lines[i].Trim().ToLower().StartsWith("dimensions "))
                                {
                                    ParseDimensions(ref i);
                                }
                                else if (_lines[i].Trim().ToLower().StartsWith("format "))
                                {
                                    ParseFormat(ref i);
                                }
                                else if (_lines[i].Trim().ToLower() == "taxlabels")
                                {
                                    i++;
                                    // Until the closing semi-colon this should all be labels.
                                    while (_lines[i].Trim() != ";" && _lines[i].Trim() != "end;")
                                    {
                                        _content.TaxaNames.Add(_lines[i].Trim());
                                        i++;
                                    }
                                }

                                break;

                            case NexusBlockTypes.Characters:
                            case NexusBlockTypes.Data:
                                if (_lines[i].Trim().ToLower().StartsWith("dimensions "))
                                {
                                    ParseDimensions(ref i);
                                }
                                else if (_lines[i].Trim().ToLower().StartsWith("format "))
                                {
                                    ParseFormat(ref i);
                                }
                                else if (_lines[i].Trim().ToLower() == "matrix")
                                {
                                    i++;
                                    // Until the closing semi-colon this should all be sequences.
                                    ParseSequences(ref i);
                                }

                                break;

                            case NexusBlockTypes.Trees:
                                break;
                        }
                    }

                    if (_lines[i].ToLower() == "end;")
                    {
                        currentBlockType = NexusBlockTypes.Unknown;
                        continue;
                    }
                }

                genes.AddRange(
                    _content.TaxaNames.Select((name, index) =>
                        {
                            return new Gene()
                            {
                                Source = GeneSources.NEXUS,
                                LastUpdateSource = GeneSources.NEXUS,
                                LastUpdatedAt = file.LastWriteTime,

                                Definition = name,
                                Nucleotides = _content.NucleotideSequences[index],
                                SequenceType = GeneSequenceTypes.Alignment,
                                SourceSequence = new NucleotideSequence(_content.NucleotideSequences[index], 1)
                            };
                        })
                );
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return genes;
        }

        private void ParseDimensions(ref int lineIndex)
        {
            string[] arguments = _lines[lineIndex].TrimStart().Substring(11).ToLower().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            for (int argumentIndex = 0; argumentIndex < arguments.Length; argumentIndex++)
            {
                int argumentValue = 0;
                string argumentName = string.Empty;
                bool valueParsed = false;

                if (arguments[argumentIndex].StartsWith("ntax")) { argumentName = "ntax"; }
                if (arguments[argumentIndex].StartsWith("nchar")) { argumentName = "nchar"; }

                if (!string.IsNullOrWhiteSpace(argumentName))
                {
                    // Number of taxa in the file
                    if (arguments[argumentIndex].EndsWith("="))
                    {
                        // The argument value is in the next entry in the array
                        argumentIndex++;
                        if (argumentIndex < arguments.Length && int.TryParse(arguments[argumentIndex].Replace(";", ""), out argumentValue))
                        { valueParsed = true; }
                    }
                    else
                    {
                        // The argument value is part of the current array entry
                        try
                        {
                            argumentValue = int.Parse(arguments[argumentIndex].Substring(arguments[argumentIndex].IndexOf("=") + 1).Replace(";", ""));
                            valueParsed = true;
                        }
                        catch { } // Yup, just being lazy.
                    }

                    if (valueParsed)
                    {
                        switch (argumentName)
                        {
                            case "ntax": _content.NumberOfTaxa = argumentValue; break;
                            case "nchar": _content.NumberOfCharacters = argumentValue; break;
                        }
                    }
                }
            }
        }

        private void ParseFormat(ref int lineIndex)
        {
            string[] arguments = _lines[lineIndex].TrimStart().Substring(7).ToLower().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            for (int argumentIndex = 0; argumentIndex < arguments.Length; argumentIndex++)
            {
                string[] argumentPieces = arguments[argumentIndex].Split(new char[] { '=' });
                string argumentValue = string.Empty;
                string argumentName = string.Empty;

                argumentName = argumentPieces[0];
                if (argumentPieces.Length > 1)
                { argumentValue = argumentPieces[1]; }
                else
                {
                    // Value is in the next entry in arguments
                    argumentIndex++;
                    if (argumentIndex < arguments.Length)
                    { argumentValue = arguments[argumentIndex]; }
                }

                if (!string.IsNullOrWhiteSpace(argumentValue))
                {
                    switch (argumentName.ToLower())
                    {
                        case "missing": _content.MissingCharacter = argumentValue[0]; break;
                        case "gap": _content.GapCharacter = argumentValue[0]; break;
                        case "matchchar":
                        case "match": 
                            _content.MatchCharacter = argumentValue[0]; break;
                        case "datatype": 
                            if (NexusFileContent.CharactersDataTypesDictionary.ContainsValue(argumentValue))
                            { _content.DataType = NexusFileContent.CharactersDataTypesDictionary.First(kv => kv.Value.ToLower() == argumentValue).Key; }
                            break;
                        case "interleave": _content.InterleaveSequences = (argumentValue.ToLower() == "yes"); break;
                    }
                }
            }
        }

        private void ParseSequences(ref int lineIndex)
        {
            List<string> sequences = _content.TaxaNames.Select(n => string.Empty).ToList();

            while (_lines[lineIndex].Trim().ToLower() != "end;")
            {
                if (string.IsNullOrWhiteSpace(_lines[lineIndex]) || _lines[lineIndex].StartsWith("[") || _lines[lineIndex].Trim() == ";") { lineIndex++; continue; }

                for (int i = 0; i < _content.TaxaNames.Count; i++)
                {
                    string[] pieces = _lines[lineIndex].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    sequences[i] += pieces[1];
                    lineIndex++;
                }
            }

            // The first sequence is treated as the "master" sequence, and all the others will use a combination of key characters to indicate 
            // whether at the same position as in the first sequence they match or have a gap.  If neither, it will be the non-matching nucleotide.
            // Pilgrimage doesn't think that way, though, each sequence is on its own and we're certainly not going to track back to an original
            // master sequence for each record, so we need to translate each sequence into its own, replacing match characters with the appropriate
            // nucleotide from the master sequence.
            string master = sequences.First();
            _content.NucleotideSequences = sequences.Select((seq, index) =>
                {
                    StringBuilder sb = new StringBuilder(seq);

                    if (index > 0)
                    {
                        for (int i = 0; i < seq.Length; i++)
                        {
                            if (seq[i] == _content.MatchCharacter)
                            {
                                sb.Replace(seq[i], master[i], i, 1);
                            }
                        }
                    }

                    return sb.ToString();
                }).ToList();
        }
    }

    internal class NexusFileContent
    {
        internal int NumberOfTaxa { get; set; }
        internal int NumberOfCharacters { get; set; }

        #region Sequence Block
        internal char MissingCharacter { get; set; }
        internal char GapCharacter { get; set; }
        internal char MatchCharacter { get; set; }

        internal CharactersDataTypes DataType { get; set; }
        internal bool InterleaveSequences { get; set; }

        internal List<string> TaxaNames { get; set; }
        internal List<string> NucleotideSequences { get; set; }
        #endregion

        internal NexusFileContent()
        {
            TaxaNames = new List<string>();
        }

        private static Dictionary<NexusFileContent.CharactersDataTypes, string> _charactersDataTypesDictionary;
        internal static Dictionary<NexusFileContent.CharactersDataTypes, string> CharactersDataTypesDictionary
        {
            get
            {
                if (_charactersDataTypesDictionary == null)
                { _charactersDataTypesDictionary = ChangLab.Common.ExtensionMethods.ToDictionary<NexusFileContent.CharactersDataTypes>(); }
                return _charactersDataTypesDictionary;
            }
        }

        internal enum CharactersDataTypes
        {
            Nucleotide,
            Protein
        }
    }

    internal enum NexusBlockTypes
    {
        Unknown,
        Taxa,
        Data,
        Trees,
        Characters
    }
}
