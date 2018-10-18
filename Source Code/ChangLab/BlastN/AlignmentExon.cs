using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChangLab.Common;
using ChangLab.Genes;

namespace ChangLab.BlastN
{
    public class AlignmentExon
    {
        public List<SequenceFragment> SequenceFragments { get; set; }
        public ExonOrientations Orientation { get; set; }

        public double BitScore { get; set; }
        public int AlignmentLength { get; set; }
        public int IdentitiesCount { get; set; }
        public int IdentityMatchPercentage
        {
            get
            {
                return Convert.ToInt32((Math.Round((Convert.ToDouble(this.IdentitiesCount) / Convert.ToDouble(this.AlignmentLength)), 2) * 100));
            }
        }
        public int Gaps { get; set; }

        public int QueryRangeStart
        {
            get
            {
                return this.SequenceFragments.Min(fragment => fragment.QueryRange.Start);
            }
        }
        public int QueryRangeEnd
        {
            get
            {
                return this.SequenceFragments.Max(fragment => fragment.QueryRange.End);
            }
        }

        public int SubjectRangeStart
        {
            get
            {
                switch (this.Orientation)
                {
                    case ExonOrientations.PlusPlus:
                        return this.SequenceFragments.Min(sf => sf.SubjectRange.Start);
                    case ExonOrientations.PlusMinus:
                    default:
                        return this.SequenceFragments.Max(sf => sf.SubjectRange.Start);
                }
            }
        }
        public int SubjectRangeEnd
        {
            get
            {
                switch (this.Orientation)
                {
                    case ExonOrientations.PlusPlus:
                        return this.SequenceFragments.Max(sf => sf.SubjectRange.End);
                    case ExonOrientations.PlusMinus:
                    default:
                        return this.SequenceFragments.Min(sf => sf.SubjectRange.End);
                }
            }
        }

        public AlignmentExon() { SequenceFragments = new List<SequenceFragment>(); }

        #region Database
        public static System.Data.DataTable ListForGenes(Gene Query, Gene Subject)
        {
            using (LocalDatabase.DataAccess da = new LocalDatabase.DataAccess("BlastN.AlignmentExon_ListForGenes"))
            {
                da.AddParameter("QueryID", System.Data.SqlDbType.UniqueIdentifier, Query.ID);
                da.AddParameter("SubjectID", System.Data.SqlDbType.UniqueIdentifier, Subject.ID);

                return da.ExecuteDataTable();
            }
        }
        #endregion
    }

    public class SequenceFragment
    {
        public Range QueryRange { get; set; }
        public Range SubjectRange { get; set; }

        public SequenceFragment()
        {
            QueryRange = new Range(false);
            SubjectRange = new Range(false);
        }

        private string _subjectSequence = string.Empty;
        public string SubjectSequence
        {
            get
            {
                return _subjectSequence;
            }
            set
            {
                _subjectSequence = value;
                _subjectSequenceIndexedByQuery = null;
            }
        }

        private Dictionary<int, char> _subjectSequenceIndexedByQuery;
        public Dictionary<int, char> SubjectSequenceIndexedByQuery
        {
            get
            {
                if (_subjectSequenceIndexedByQuery == null)
                {
                    _subjectSequenceIndexedByQuery = new Dictionary<int, char>();
                    for (int i = 0; i < _subjectSequence.Length; i++)
                    {
                        _subjectSequenceIndexedByQuery.Add(QueryRange.Start + i, _subjectSequence[i]);
                    }
                }

                return _subjectSequenceIndexedByQuery;
            }
        }

        /// <summary>
        /// Returns the remainder of the nucleotide sequence after the given query index.
        /// </summary>
        public string SubstringSubjectByQueryIndex(int QueryIndex)
        {
            return this.SubjectSequenceIndexedByQuery.SkipWhile(n => n.Key <= QueryIndex).Aggregate(string.Empty, (current, s) => current += s.Value);
        }

        /// <summary>
        /// Returns the nucleotide sequence up to and including the given query index.
        /// </summary>
        public string TakeSubjectByQueryIndex(int QueryIndex)
        {
            return this.SubjectSequenceIndexedByQuery.TakeWhile(n => n.Key <= QueryIndex).Aggregate(string.Empty, (current, s) => current += s.Value);
        }
    }
}
