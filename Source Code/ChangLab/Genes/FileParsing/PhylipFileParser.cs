using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChangLab.Common;

namespace ChangLab.Genes.FileParsing
{
    public class PhylipFileParser
    {
        public List<Gene> ParseFile(string FilePath)
        {
            System.IO.FileInfo file = new System.IO.FileInfo(FilePath);
            List<Gene> genes = new List<Gene>();
            try
            {
                List<string> lines = System.IO.File.ReadAllLines(FilePath).ToList();
                
                int count = 0; int length = 0; bool valid = true;
                string[] dimensions = lines[0].Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (dimensions.Length == 2)
                {
                    if (!int.TryParse(dimensions[0], out count)) { valid = false; }
                    if (!int.TryParse(dimensions[1], out length)) { valid = false; }
                }
                if (!valid) { throw new Exception("Invalid header."); }

                bool interlevedNoGaps = (lines.Count == ((count * 2) + 2));

                for (int lineIndex = 1; lineIndex < lines.Count; lineIndex++)
                {
                    if (string.IsNullOrWhiteSpace(lines[lineIndex])) { continue; }

                    for (int taxaIndex = 0; taxaIndex < count; taxaIndex++)
                    {
                        if (string.IsNullOrWhiteSpace(lines[lineIndex])) { continue; }

                        // Currently we're assuming that it's formatted correctly...
                        if (genes.Count < count)
                        {
                            string[] firstLeaf = lines[lineIndex].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            string definition = string.Empty; string nucleotides = string.Empty;
                            if (firstLeaf.Length == 2)
                            {
                                // The header preceedes the first chunk of the sequence and the rest is interleved
                                definition = firstLeaf[0];
                                nucleotides = firstLeaf[1];
                            }
                            else if (interlevedNoGaps)
                            {
                                // The header is (presumably) above the whole sequence, which is entirely on the next line
                                definition = firstLeaf[0];
                                lineIndex++;
                                nucleotides = lines[lineIndex].Trim();
                            }
                            else if (firstLeaf.Length == 1)
                            {
                                // The header is (presumably) above the whole sequence, which is in fragments on multiple lines
                                definition = firstLeaf[0];
                                while (!string.IsNullOrWhiteSpace(lines[lineIndex + 1]))
                                {
                                    lineIndex++;
                                    nucleotides += lines[lineIndex].Trim();
                                }
                            }

                            genes.Add(new Gene() { 
                                Source = GeneSources.PHYLIP,
                                LastUpdateSource = GeneSources.PHYLIP,
                                LastUpdatedAt = file.LastWriteTime,
                                
                                Definition = definition, 
                                Nucleotides = nucleotides,
                                SequenceType = GeneSequenceTypes.Alignment
                            });
                        }
                        else
                        {
                            genes[taxaIndex].Nucleotides += lines[lineIndex].Trim();
                        }

                        lineIndex++;
                    }
                }

                genes.ForEach(g => g.SourceSequence = new NucleotideSequence(g.Nucleotides, 1));
            }
            catch (Exception ex)
            {
                throw new Exception("File is not a valid PHYLIP format.", ex);
            }

            return genes;
        }
    }
}
