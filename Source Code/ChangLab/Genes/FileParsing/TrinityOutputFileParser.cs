using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ChangLab.Common;
using ChangLab.Jobs;

namespace ChangLab.Genes.FileParsing
{
    public class TrinityOutputFileParser
    {
        public bool Cancelled { get; private set; }
        private int _sourceID { get; set; }

        public List<Gene> ParseTrinityOutputFile(string FilePath)
        {
            FileStream fs = null;
            StreamReader sr = null;
            FileInfo
            outputFile = new FileInfo(FilePath);
            this._sourceID = GeneSource.IDByKey(GeneSources.Trinity);

            try
            {
                using (fs = File.OpenRead(outputFile.FullName))
                {
                    using (sr = new StreamReader(fs))
                    {
                        string queriedDatabaseName = outputFile.Name;
                        if (queriedDatabaseName.EndsWith(outputFile.Extension)) { queriedDatabaseName = queriedDatabaseName.Remove(queriedDatabaseName.LastIndexOf(outputFile.Extension)); }
                        if (queriedDatabaseName.EndsWith(".")) { queriedDatabaseName = queriedDatabaseName.Substring(0, queriedDatabaseName.Length - 1); }

                        List<Gene> genes = new List<Gene>();
                        Gene gene = null;

                        ProgressUpdateEventArgs args = new ProgressUpdateEventArgs() { Setup = true, CurrentMax = Convert.ToInt32(fs.Length), CurrentProgress = 0 };
                        OnProgressUpdate(args);

                        string line = string.Empty;
                        while (!sr.EndOfStream)
                        {
                            line = sr.ReadLine();

                            try
                            {
                                if (line.TrimStart().StartsWith(">"))
                                {
                                    args = new ProgressUpdateEventArgs() { CurrentProgress = Convert.ToInt32(fs.Position) };
                                    ProgressUpdate(args);
                                    if (args.Cancel) { this.Cancelled = true; return null; }

                                    int alignmentLength = 0;
                                    if (line.Contains("len="))
                                    {
                                        System.Text.RegularExpressions.Match lengthMatch = System.Text.RegularExpressions.Regex.Match(line, "len=+[0-9]{1,}");
                                        if (lengthMatch.Success)
                                        {
                                            string lengthString = lengthMatch.Groups[0].Value.Replace("len=", "");
                                            int.TryParse(lengthString.Replace("len=", ""), out alignmentLength);
                                        }
                                    }

                                    int start = 0;
                                    int end = (alignmentLength != 0 ? alignmentLength - 1 : 0);
                                    if (line.Contains("path="))
                                    {
                                        System.Text.RegularExpressions.Match pathMatch = System.Text.RegularExpressions.Regex.Match(line, "path=+\\[+[^\\]]{1,}\\]");
                                        if (pathMatch.Success)
                                        {
                                            string pathString = pathMatch.Groups[0].Value.Replace("path=[", "").Replace("]", "");
                                            string[] pathPieces = pathString.Split(new char[] { ' ' });
                                            // There's undoubtedly a regular expression that could do this.
                                            start = int.Parse(pathPieces.First().Split(new char[] { ':' })[1].Split(new char[] { '-' })[0]);
                                            end = int.Parse(pathPieces.Last().Split(new char[] { ':' })[1].Split(new char[] { '-' })[1]);
                                        }
                                    }

                                    gene = new Gene()
                                    {
                                        SourceID = this._sourceID,
                                        Definition = line.Substring(1),
                                        Organism = queriedDatabaseName,
                                        Locus = line.Substring(0, line.IndexOf(" ")),
                                        SequenceRange = new Range(start, end)
                                    };
                                    genes.Add(gene);
                                }
                                else if (!string.IsNullOrWhiteSpace(line)) // Nucleotides
                                {
                                    gene.Nucleotides += line;
                                }
                            }
                            catch (Exception ex)
                            {
                                gene.Exceptions.Add(ex);
                                break;
                            }
                        }

                        genes.ForEach(g =>
                            {
                                g.SourceSequence = new NucleotideSequence(g.Nucleotides, g.SequenceRange.Start);
                            });

                        return genes;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sr != null)
                {
                    try { sr.Close(); sr.Dispose(); }
                    catch { }
                }
                if (fs != null)
                {
                    try { fs.Close(); fs.Dispose(); }
                    catch { }
                }
            }
        }

        protected virtual void OnProgressUpdate(ProgressUpdateEventArgs e)
        {
            if (ProgressUpdate != null)
            {
                ProgressUpdate(e);
            }
        }
        public event ProgressUpdateEventHandler ProgressUpdate;
    }
}
