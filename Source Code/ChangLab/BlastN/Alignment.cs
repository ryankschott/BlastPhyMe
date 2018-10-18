/*
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * Next step is to re-import the procedure for CompileSequenceFromBLASTNExe() from blastn_similarity, inclusive of the new mismatched overlap logic
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChangLab.Common;
using ChangLab.Genes;

namespace ChangLab.BlastN
{
    public class Alignment
    {
        #region Properties
        public int ID { get; set; }
        /// <summary>
        /// The gene that that was used as the source to align against.
        /// </summary>
        public string QueryID { get; set; }
        /// <summary>
        /// The gene that was aligned against the Query gene.
        /// </summary>
        public string SubjectID { get; set; }
        public int Rank { get; set; }

        public List<AlignmentExon> Exons { get; private set; }
        public List<Exception> Warnings { get; private set; }

        public string Nucleotides { get; set; }
        public Range AlignmentRange { get; set; }
        #endregion

        public Alignment()
        {
            this.Exons = new List<AlignmentExon>();
            this.Warnings = new List<Exception>();
            this.AlignmentRange = new Range();
        }

        public void Save(bool Exons)
        {
            using (LocalDatabase.DataAccess da = new LocalDatabase.DataAccess("BlastN.Alignment_Edit"))
            {
                da.AddParameter("ID", System.Data.SqlDbType.Int, 0, 0, System.Data.ParameterDirection.InputOutput, true);
                da.AddParameter("QueryID", System.Data.SqlDbType.UniqueIdentifier, this.QueryID);
                da.AddParameter("SubjectID", System.Data.SqlDbType.UniqueIdentifier, this.SubjectID);
                da.AddParameter("Rank", System.Data.SqlDbType.Int, this.Rank);
                // If this alignment already exists, we're going to replace the existing exons with this new data.
                da.AddParameter("ClearExons", Exons);

                this.ID = (int)da.ExecuteParameter("ID");
                if (this.ID == 0)
                {
                    // Just a safeguard to make sure this doesn't happen.
                    throw new Exception("Alignment ID should not be null.");
                }
            }

            if (Exons)
            {
                SaveExons();
            }
        }

        public void SaveExons()
        {
            this.Exons.ForEach(exon =>
            {
                using (LocalDatabase.DataAccess da = new LocalDatabase.DataAccess("BlastN.AlignmentExon_Edit"))
                {
                    da.AddParameter("AlignmentID", this.ID);
                    da.AddParameter("OrientationID", ExonOrientation.IDByKey(exon.Orientation));
                    da.AddParameter("BitScore", System.Data.SqlDbType.Float, exon.BitScore);
                    da.AddParameter("AlignmentLength", System.Data.SqlDbType.Int, exon.AlignmentLength);
                    da.AddParameter("IdentitiesCount", System.Data.SqlDbType.Int, exon.IdentitiesCount);
                    da.AddParameter("Gaps", System.Data.SqlDbType.Int, exon.Gaps);
                    da.AddParameter("QueryRangeStart", System.Data.SqlDbType.Int, exon.QueryRangeStart);
                    da.AddParameter("QueryRangeEnd", System.Data.SqlDbType.Int, exon.QueryRangeEnd);
                    da.AddParameter("SubjectRangeStart", System.Data.SqlDbType.Int, exon.SubjectRangeStart);
                    da.AddParameter("SubjectRangeEnd", System.Data.SqlDbType.Int, exon.SubjectRangeEnd);

                    da.ExecuteCommand();
                }
            });
        }

        public void PopulateFromHit(Bio.Web.Blast.Hit Hit)
        {
            try
            {
                this.Exons.AddRange(Hit.Hsps.Select(h =>
                {
                    AlignmentExon ex = new AlignmentExon()
                    {
                        AlignmentLength = h.AlignmentLength.ToInt(),
                        BitScore = h.BitScore,
                        IdentitiesCount = h.IdentitiesCount.ToInt(),
                        Gaps = h.Gaps,
                        Orientation = (h.HitStart < h.HitEnd ? ExonOrientations.PlusPlus : ExonOrientations.PlusMinus)
                    };

                    ex.SequenceFragments.Add(new SequenceFragment()
                    {
                        SubjectSequence = h.HitSequence,
                        SubjectRange = new Range(h.HitStart.ToInt(), h.HitEnd.ToInt()),
                        QueryRange = new Range(h.QueryStart.ToInt(), h.QueryEnd.ToInt())
                    });

                    return ex;
                }));

                if (this.Exons.Count != 0)
                {
                    this.CompileSequenceFromNCBI();
                }
                else
                {
                    this.Warnings.Add(new Exception("Result contained no alignments."));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CompileSequenceFromNCBI()
        {
            string sequence = string.Empty;

            Exons.OrderBy(exon => exon.QueryRangeStart).ToList().ForEach(exon =>
            {
                int exonIndex = 0;
                string exonSequence = string.Empty;

                var orderedFragments = exon.SequenceFragments.OrderBy(fragment => fragment.QueryRange.Start).ToList();
                orderedFragments.ForEach(fragment =>
                {
                    if (exonIndex == 0)
                    {
                        exonSequence += fragment.SubjectSequence;
                    }
                    else
                    {
                        if (fragment.QueryRange.Start > (exonIndex + 1))
                        {
                            // There's a gap between the preceding query sequence and the current one, so we fill it with hyphens.
                            exonSequence += string.Empty.Replicate("-", (fragment.QueryRange.Start - (exonIndex + 1))) + fragment.SubjectSequence;

                            // Report it out as a soft warning.
                            this.Warnings.Add(new Exception(string.Format("Gap filled from {0}-{1}", (exonIndex + 1), fragment.QueryRange.Start)));
                        }
                        else if (fragment.QueryRange.Start < exonIndex)
                        {
                            // There's an overlap.

                            // Check the overlap to see if the two segments match.
                            string priorOverlap = exonSequence.Reverse().AsString().Substring(0, (exonIndex + 1) - fragment.QueryRange.Start).Reverse().AsString();
                            string currentOverlap = fragment.TakeSubjectByQueryIndex(exonIndex);
                            if (priorOverlap == currentOverlap)
                            {
                                // We can remove the overlapping nucleotides from the current fragment.
                                exonSequence += fragment.SubstringSubjectByQueryIndex(exonIndex);
                                // Report it out as a soft warning.
                                this.Warnings.Add(new Exception(string.Format("Overlap removed from {0}-{1}", fragment.QueryRange.Start, (exonIndex + 1))));
                            }
                            else
                            {
                                // This is an unresolvable error; the sequence can't be run.
                                throw new Exception(string.Format("Mismatched overlap from {0}-{1}", fragment.QueryRange.Start, (exonIndex + 1)));
                            }
                        }
                        else
                        {
                            // This is the normal situation; the current fragment starts immediately after the preceding one ended.
                            exonSequence += fragment.SubjectSequence;
                        }
                    }
                    exonIndex = fragment.QueryRange.End;
                });

                sequence += exonSequence;
            });

            this.Nucleotides = sequence;

            // Derive the sequence range from the exons, based on orientation
            switch (this.Exons.First().Orientation)
            {
                case ExonOrientations.PlusPlus:
                    this.AlignmentRange.Start = this.Exons.Min(e => e.SubjectRangeStart);
                    this.AlignmentRange.End = this.Exons.Max(e => e.SubjectRangeEnd);
                    break;
                case ExonOrientations.PlusMinus:
                default:
                    this.AlignmentRange.Start = this.Exons.Max(e => e.SubjectRangeStart);
                    this.AlignmentRange.End = this.Exons.Min(e => e.SubjectRangeEnd);
                    break;
            }
        }

        public void CompileSequenceFromBLASTNExe()
        {
            string sequence = string.Empty;
            int lastWithinExonQueryEndIndex = 0;
            int lastFragmentQueryEndIndex = 0;

            Exons.OrderBy(exon => exon.QueryRangeStart).ToList().ForEach(exon =>
            {
                lastWithinExonQueryEndIndex = 0;

                var orderedFragments = exon.SequenceFragments.OrderBy(fragment => fragment.QueryRange.Start).ToList();
                orderedFragments.ForEach(fragment =>
                {
                    // The ordering of this if statement is critical - don't change it without thinking it through

                    // First check to see if the current fragment overlaps with the previous fragment, regardless of whether we're in the next exon
                    if (fragment.QueryRange.Start <= lastFragmentQueryEndIndex)
                    {
                        // There's an overlap.

                        // Check the overlap to see if the two segments match.
                        string priorOverlap = sequence.Reverse().AsString().Substring(0, (lastFragmentQueryEndIndex + 1) - fragment.QueryRange.Start).Reverse().AsString();
                        string currentOverlap = fragment.TakeSubjectByQueryIndex(lastFragmentQueryEndIndex);
                        if (priorOverlap == currentOverlap)
                        {
                            // We can remove the overlapping nucleotides from the current fragment.
                            sequence += fragment.SubstringSubjectByQueryIndex(lastFragmentQueryEndIndex);
                            // Report it out as a soft warning.
                            this.Warnings.Add(new Exception(string.Format("Overlap removed from {0}-{1}", fragment.QueryRange.Start, lastFragmentQueryEndIndex)));
                        }
                        else
                        {
                            // This is an unresolvable error; the sequence can't be run.
                            throw new SequenceParsingWarningException(string.Format("Mismatched overlap from query index {0}-{1}", fragment.QueryRange.Start, lastFragmentQueryEndIndex)) { HandleAsWarning = false, Exons = Exons };
                        }
                    }
                    // If there's no overlap, now check to see if the current fragment starts later than the last fragment within the same exon ended
                    else if ((fragment.QueryRange.Start > (lastWithinExonQueryEndIndex + 1)) && (lastWithinExonQueryEndIndex != 0))
                    {
                        // There's a gap between the preceding query sequence and the current one, so we fill it with hyphens.
                        sequence += string.Empty.Replicate("-", (fragment.QueryRange.Start - (lastWithinExonQueryEndIndex + 1))) + fragment.SubjectSequence;

                        // Report it out as a soft warning.
                        this.Warnings.Add(new Exception(string.Format("Gap filled from {0}-{1}", (lastWithinExonQueryEndIndex + 1), fragment.QueryRange.Start)));
                    }
                    // If there's no within-exon gap, we can just tack the fragment onto the sequence because it's either a new exon or a succeeding
                    // fragment that's starting immediately after the preceding one ended.
                    else
                    {
                        sequence += fragment.SubjectSequence;
                    }

                    lastFragmentQueryEndIndex = fragment.QueryRange.End;
                    lastWithinExonQueryEndIndex = fragment.QueryRange.End;
                });
            });

            this.Nucleotides = sequence;

            // Derive the sequence range from the exons, based on orientation
            switch (this.Exons.First().Orientation)
            {
                case ExonOrientations.PlusPlus:
                    this.AlignmentRange.Start = this.Exons.Min(e => e.SubjectRangeStart);
                    this.AlignmentRange.End = this.Exons.Max(e => e.SubjectRangeEnd);
                    break;
                case ExonOrientations.PlusMinus:
                default:
                    this.AlignmentRange.Start = this.Exons.Max(e => e.SubjectRangeStart);
                    this.AlignmentRange.End = this.Exons.Min(e => e.SubjectRangeEnd);
                    break;
            }
        }

        #region Database
        
        #endregion
    }

    public class SequenceParsingWarningException : WarningException
    {
        public List<AlignmentExon> Exons { get; set; }

        public SequenceParsingWarningException(string Message) : base(Message) { }
    }
}
