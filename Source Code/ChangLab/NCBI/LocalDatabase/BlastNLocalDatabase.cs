using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using ChangLab.BlastN;
using ChangLab.Common;
using ChangLab.Genes;

namespace ChangLab.NCBI.LocalDatabase
{
    public class BlastNLocalDatabase
    {
        public static int SequenceBatchSize { get { return 10; } }

        private string DatabaseFilePath { get; set; }
        private string BlastNExePath { get; set; }
        private string OutputDirectoryPath { get; set; }
        
        private string JobID { get; set; }
        public bool CancellationPending { get; private set; }
        public void CancelAsync()
        {
            CancellationPending = true;
            OnProgressUpdate(new ProgressUpdateEventArgs() { ProgressMessage = "Job is being cancelled..." });
        }

        private List<Query> Queries { get; set; }
        private List<Gene> InputGenes { get; set; }

        public BlastNLocalDatabase()
        {
            this.Queries = new List<Query>();
        }

        public BlastNLocalDatabase(string DatabaseFilePath, string BlastNExePath, string OutputDirectoryPath, string JobID) : this()
        {
            this.OutputDirectoryPath = OutputDirectoryPath;
            this.DatabaseFilePath = DatabaseFilePath;
            this.BlastNExePath = BlastNExePath;
        }

        public void QueryLocalDB(List<Gene> Genes)
        {
            try
            {
                this.InputGenes = Genes;
                
                // One option would be to only create this directory if the user has opted to output the result files, but then if they don't select
                // that option and something goes wrong and the job crashes, the orphaned output files would be sitting in the main output directory.
                string outputDirectoryPath = Directory.CreateDirectory(OutputDirectoryPath + "\\" + JobID).FullName;

                // Split the input genes into batches instead of submitting everything
                var geneBatches = Genes.Select((gene, index) => new { Gene = gene, Index = index })
                    .GroupBy(g => g.Index / SequenceBatchSize)
                    .Select(grp => grp.ToList());

                OnProgressUpdate(new ProgressUpdateEventArgs() { Setup = true, TotalMax = geneBatches.Count(), TotalProgress = 0, CurrentMax = 2, CurrentProgress = 0 });
                ProgressUpdateEventArgs totalProgress = new ProgressUpdateEventArgs() { TotalProgress = 0 };

                foreach (var batch in geneBatches)
                {
                    if (CancellationPending) { return; }

                    int lBound = batch.First().Index + 1; int uBound = batch.Last().Index + 1;
                    string fastaSequences = batch.Aggregate(string.Empty, (current, g) => current += ">" + g.Gene.ID + "\r\n" + g.Gene.Nucleotides + "\r\n");

                    string progressMessage = "Querying local database with ";
                    if (Genes.Count > SequenceBatchSize)
                    { progressMessage += "sequences (" + lBound.ToString("N0") + "-" + uBound.ToString("N0") + " of " + Genes.Count.ToString() + ") "; }
                    else
                    { progressMessage += "all sequences (" + Genes.Count.ToString() + ") "; }
                    OnProgressUpdate(new ProgressUpdateEventArgs() { ProgressMessage = progressMessage });

                    string requestId = Guid.NewGuid().ToString();
                    string tmpFilePath = outputDirectoryPath + "\\sequences_" + lBound.ToString() + "_" + uBound.ToString() + ".fsa";
                    File.WriteAllText(tmpFilePath, fastaSequences);
                    string outFilePath = outputDirectoryPath + "\\results\\out_" + lBound.ToString() + "_" + uBound.ToString() + ".log";

                    string args = "-db \"" + DatabaseFilePath + "\" -query \"" + tmpFilePath + "\" -out \"" + outFilePath + "\"";

                    Process process = Process.Start(new ProcessStartInfo(this.BlastNExePath + "\\blastn.exe", args) { WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden });
                    process.WaitForExit();

                    if (CancellationPending) { return; }

                    OnProgressUpdate(new ProgressUpdateEventArgs() { ProgressMessage = "Parsing results...", CurrentProgress = 1 });

                    // Now parse the output file.
                    List<Query> results = ParseOutputFile(batch.Select(b => b.Gene).ToList(), outFilePath);
                    OnResultsProcessed(new ResultsEventArgs() { Results = results });
                    
                    File.Delete(tmpFilePath);
#if !DEBUG
                    File.Delete(outFilePath);
#endif
                    if (CancellationPending) { return; }
                    totalProgress.TotalProgress++; OnProgressUpdate(totalProgress);
                }

#if !DEBUG
                Directory.Delete(outputDirectoryPath, true);
#endif
                OnProgressUpdate(new ProgressUpdateEventArgs() { TotalProgress = geneBatches.Count(), CurrentProgress = 2, ProgressMessage = "Completed" });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Query> ParseOutputFile(List<Gene> Genes, string FilePath)
        {
            try
            {
                List<Query> queries = new List<Query>();
                FileInfo inputFile = new FileInfo(FilePath);

                using (FileStream stream = new FileStream(inputFile.FullName, FileMode.Open, FileAccess.Read))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string line = string.Empty;
                        bool readLine = true;

                        string targetDatabase = string.Empty;

                        Query query = null;
                        LocalAlignment alignment = null; 
                        int alignmentIndex = 0;
                        AlignmentExon exon = null;
                        SequenceFragment fragment = null;

                        while (!reader.EndOfStream)
                        {
                            if (readLine) { line = reader.ReadLine(); }
                            else
                            {
                                // This means that in one of the blocks below we had cause to read ahead to the next line, but whatever was on that 
                                // line wasn't what we were looking for and we want to continue and check it against the list of headers of inerest.
                                readLine = true;
                            }

                            if (line.StartsWith("Database:"))
                            {
                                if (string.IsNullOrWhiteSpace(targetDatabase))
                                {
                                    targetDatabase = line.Replace("Database: ", "");
                                    if (targetDatabase.LastIndexOf(".") > -1)
                                    {
                                        targetDatabase = targetDatabase.Remove(targetDatabase.LastIndexOf("."));
                                    }
                                }
                            }
                            else if (line.StartsWith("Query="))
                            {
                                // This should match the uniqueidentifier for one of the input genes.
                                string queryGeneId = line.Substring(7);
                                if (Genes != null)
                                {
                                    query = new Query() { InputGene = Genes.First(q => GuidCompare.Equals(q.ID, queryGeneId)) };
                                }
                                else
                                {
                                    query = new Query()
                                    {
                                        InputGene = new Gene()
                                        {
                                            Definition = queryGeneId,
                                            Organism = inputFile.Name.Substring(0, inputFile.Name.Length - inputFile.Extension.Length)
                                        }
                                    };
                                }
                                queries.Add(query);

                                alignmentIndex = 1;
                            }
                            else if (line.StartsWith("Lambda"))
                            {
                                // We've reached the end of the current query.
                                // Now attempt to compile sequences from the fragments for each match.
                                if (query.LocalAlignments.Count != 0)
                                {
                                    query.LocalAlignments.ForEach(lal =>
                                    {
                                        try
                                        {
                                            lal.Alignment = new Alignment() { QueryID = query.InputGene.ID, Rank = lal.Rank };
                                            lal.Alignment.Exons.AddRange(lal.Exons);
                                            lal.Alignment.CompileSequenceFromBLASTNExe();
                                        }
                                        catch (Exception ex)
                                        {
                                            lal.Alignment = null;
                                            lal.Exceptions.Add(ex);
                                        }
                                    });
                                }
                            }

                            try
                            {
                                if (line.StartsWith(">"))
                                {
                                    alignment = new LocalAlignment() 
                                        { 
                                            OutputGene = new Gene() { Definition = line.Substring(2), GeneName = query.InputGene.Definition, Accession = targetDatabase },
                                            Rank = alignmentIndex 
                                        };
                                    alignment.OutputGene.Locus = DeriveContigNumberFromHeader(alignment.OutputGene.Definition);
                                    query.LocalAlignments.Add(alignment);

                                    alignmentIndex++;
                                }

                                // This indicates a new sequence fragment; each fragment will have three lines for the Score, Identities, and Strand fields.
                                // We can grab all of these in one block by advancing the row 
                                if (line.TrimStart().StartsWith("Score ="))
                                {
                                    try
                                    {
                                        string score = line.Replace("Score =", "").TrimStart();
                                        score = score.Substring(0, score.IndexOf("bits")).TrimEnd();

                                        line = reader.ReadLine();
                                        string identities = line.Replace("Identities =", "").TrimStart();
                                        identities = identities.Substring(0, identities.IndexOf(" ")).TrimEnd();

                                        string gaps = line.Substring(line.IndexOf("Gaps =")).Replace("Gaps =", "").TrimStart();
                                        gaps = gaps.Substring(0, gaps.IndexOf(" ")).TrimEnd();

                                        line = reader.ReadLine();
                                        ExonOrientations orientation = (ExonOrientations)Enum.Parse(typeof(ExonOrientations), line.Trim().Replace("Strand=", "").Replace("/", ""));

                                        exon = new AlignmentExon()
                                        {
                                            BitScore = double.Parse(score),
                                            IdentitiesCount = int.Parse(identities.Split(new char[] { '/' })[0]),
                                            AlignmentLength = int.Parse(identities.Split(new char[] { '/' })[1]),
                                            Gaps = int.Parse(gaps.Split(new char[] { '/' })[0]),

                                            Orientation = orientation
                                        };
                                        alignment.Exons.Add(exon);

                                        continue;
                                    }
                                    catch (Exception ex)
                                    {
                                        if (query != null) { query.ParsingException = ex; } else { throw ex; }
                                        continue;
                                    }
                                }

                                if (line.TrimStart().StartsWith("Query") && !line.TrimStart().StartsWith("Query="))
                                {
                                    // This is a sequence fragment, not a new input query.
                                    fragment = new SequenceFragment();

                                    string[] pieces = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                                    fragment.QueryRange.Start = int.Parse(pieces[1]);
                                    fragment.QueryRange.End = int.Parse(pieces.Last());

                                    // Skip two lines to reach the subject sequence line.
                                    line = reader.ReadLine();
                                    line = reader.ReadLine();

                                    pieces = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                                    fragment.SubjectSequence = pieces[2];
                                    fragment.SubjectRange.Start = int.Parse(pieces[1]);
                                    fragment.SubjectRange.End = int.Parse(pieces.Last());

                                    if (alignment.Exons.Any(e => e.SequenceFragments.Any(sf => sf.QueryRange.Start == fragment.QueryRange.Start)))
                                    {
                                        alignment.Exceptions.Add(new Exception(string.Format("Duplicate query index ({0})", fragment.QueryRange.Start)));
                                    }
                                    exon.SequenceFragments.Add(fragment);
                                }
                            }
                            catch (Exception ex)
                            {
                                if (query != null) { query.ParsingException = ex; } else { throw ex; }
                                continue;
                            }
                        }
                    }
                }

                this.Queries.AddRange(queries);
                return queries;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string DeriveContigNumberFromHeader(string Header)
        {
            if (Header.ToLower().Contains("contig"))
            {
                return Header.Substring(Header.IndexOf("contig") + 6).Aggregate(string.Empty, (current, c) => current += ContigCharConversion(c));
            }
            else if (System.Text.RegularExpressions.Regex.IsMatch(Header, "^.{1,}(len=).{1,}(path=\\[).{1,}$"))
            {
                return Header.Substring(0, Header.IndexOf(" "));
            }
            else { return "0"; }
        }

        private string ContigCharConversion(char c)
        {
            if (char.IsNumber(c))
            {
                return c.ToString();
            }
            else if (char.IsWhiteSpace(c))
            {
                return " ";
            }
            else
            {
                return string.Empty;
            }
        }

        #region Events
        protected virtual void OnProgressUpdate(ProgressUpdateEventArgs e) { if (ProgressUpdate != null) { ProgressUpdate(e); } }
        public event ProgressUpdateEventHandler ProgressUpdate;

        protected virtual void OnResultsProcessed(ResultsEventArgs e) { if (ResultsProcessed != null) { ResultsProcessed(e); } }
        public delegate void ResultsEventHandler(ResultsEventArgs e);
        public event ResultsEventHandler ResultsProcessed;
        public class ResultsEventArgs : EventArgs
        {
            public List<Query> Results { get; set; }

            public ResultsEventArgs() { }
        }
        #endregion
    }
}
