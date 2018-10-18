using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChangLab.Common;

namespace ChangLab.Genes
{
    public class NucleotideSequence
    {
        public string GeneID { get; set; }
        private Dictionary<int, char> Nucleotides { get; set; }
        public int Length { get { return Nucleotides.Count; } }
        
        /// <remarks>
        /// These don't mean much now that we're recording all of the features of a source sequence.
        /// </remarks>
        public int Start { get { return (Nucleotides.Count == 0 ? 0 : Nucleotides.First().Key); } }
        public int End { get { return (Nucleotides.Count == 0 ? 0 : Nucleotides.Last().Key); } }
        
        public NucleotideSequence()
        {
            Nucleotides = new Dictionary<int, char>();
        }

        /// <param name="Start">The index number of the first nucleotide in Sequence.</param>
        public NucleotideSequence(string Sequence, int Start)
        {
            Nucleotides = Sequence.Select((c, index) => new KeyValuePair<int, char>(index + Start, c)).ToDictionary(kv => kv.Key, kv => kv.Value);
        }

        public void Save()
        {
            using (LocalDatabase.DataAccess da = new LocalDatabase.DataAccess("Gene.NucleotideSequence_Edit"))
            {
                da.AddParameter("GeneID", System.Data.SqlDbType.UniqueIdentifier, this.GeneID);
                da.AddParameter("Nucleotides", this.ToString());
                da.AddParameter("Start", this.Start);
                da.AddParameter("End", this.End);

                da.ExecuteCommand();
            }
        }

        public override string ToString()
        {
            return Nucleotides.Aggregate(string.Empty, (current, c) => current += c.Value);
        }

        /// <summary>
        /// Concatenates a fragment of nucleotides onto the end of the current sequence, starting at the last index + 1.
        /// </summary>
        public void Add(string Fragment)
        {
            int startingIndex = End + 1;
            for (int i = 0; i < Fragment.Length; i++)
            {
                Nucleotides.Add(i + startingIndex, Fragment[i]);
            }
        }

        /// <summary>
        /// Returns a segment of the nucleotide sequence based on the given start/end index values.
        /// </summary>
        public string SubSequence(int Start, int End)
        {
            string sequence = string.Empty;
            
            if (Nucleotides != null && Nucleotides.First().Key <= Start && Nucleotides.Last().Key >= End)
            {
                sequence = Nucleotides.SkipWhile(kv => kv.Key < Start).TakeWhile(kv => kv.Key <= End).Aggregate(string.Empty, (current, c) => current += c.Value);
            }
            
            return sequence;
        }

        /// <summary>
        /// Returns a segment of the nucleotide sequence based on the given start/end index values.
        /// </summary>
        public string SubSequence(int Start, int End, bool IsComplement = false)
        {
            string sequence = string.Empty;

            if (Nucleotides != null && Nucleotides.First().Key <= Start && Nucleotides.Last().Key >= End)
            {
                if (!IsComplement)
                {
                    sequence = Nucleotides.SkipWhile(kv => kv.Key < Start).TakeWhile(kv => kv.Key <= End).Aggregate(string.Empty, (current, c) => current += c.Value);
                }
                else
                {
                    sequence = Nucleotides.SkipWhile(kv => kv.Key < Start).TakeWhile(kv => kv.Key <= End).Reverse().Aggregate(string.Empty, (current, c) => current += c.Value.ToComplement());
                }
            }

            return sequence;
        }

        public NucleotideSequence Splice(List<List<Interval>> Intervals)
        {
            NucleotideSequence spliced = new NucleotideSequence();

            if (Nucleotides != null)
            {
                Intervals.ForEach(intervals =>
                    {
                        // For each set of intervals, splice out that whole set from the original sequence and then stack it onto the output
                        // sequence.  That preserves the logic of a feature representing a continuous set of nucleotides.
                        // The indexing within spliced is going to get thrown off, but that's okay; Start and End don't mean much when you're joining
                        // together disparate segments of the source nucleotide sequence.
                        spliced.Add(
                            intervals
                            .Aggregate(string.Empty, (current, interval) =>
                            {
                                bool reverse = interval.From > interval.To;
                                string fragment = this.SubSequence((reverse ? interval.To : interval.From), (reverse ? interval.From : interval.To));
                                if (reverse) { fragment = fragment.Reverse().Aggregate(string.Empty, (frag, c) => frag += c); }
                                if (interval.IsComplement) { fragment = fragment.Select(c => c.ToComplement()).Aggregate(string.Empty, (frag, c) => frag += c); }

                                return current += fragment;
                            }));
                    });
            }

            return spliced;
        }

        public NucleotideSequence SpliceByBestFeatures(List<Feature> Features)
        {
            NucleotideSequence spliced = new NucleotideSequence();

            if (Nucleotides != null)
            {
                IEnumerable<Feature> featuresTest = null;
                foreach (GeneFeatureKeys key in new GeneFeatureKeys[] { GeneFeatureKeys.CDS, GeneFeatureKeys.exon, GeneFeatureKeys.gene })
                {
                    featuresTest = Features.Where(f => f.FeatureKey.Key == key && f.Intervals.Count != 0);
                    if (featuresTest.Count() != 0) { break; }
                }

                if (featuresTest.Count() != 0)
                {
                    featuresTest.ToList().ForEach(feature =>
                    {
                        // For each set of intervals, splice out that whole set from the original sequence and then stack it onto the output
                        // sequence.  That preserves the logic of a feature representing a continuous set of nucleotides.
                        // The indexing within spliced is going to get thrown off, but that's okay; Start and End don't mean much when you're joining
                        // together disparate segments of the source nucleotide sequence.
                        spliced.Add(
                            feature
                            .Intervals
                            .Aggregate(string.Empty, (current, interval) =>
                            {
                                bool reverse = interval.Start > interval.End;
                                string fragment = this.SubSequence((reverse ? interval.End : interval.Start), (reverse ? interval.Start : interval.End));
                                if (reverse) { fragment = fragment.Reverse().Aggregate(string.Empty, (frag, c) => frag += c); }
                                if (interval.IsComplement) { fragment = fragment.Select(c => c.ToComplement()).Aggregate(string.Empty, (frag, c) => frag += c); }

                                return current += fragment;
                            }));
                    });
                }
                else { spliced.Add(this.ToString()); }
            }

            return spliced;
        }

        /// <summary>
        /// Splices the nucleotide sequence based on the given exon start/end index values.
        /// </summary>
        /// <param name="ExonRanges">
        /// From, To, IsComplementary
        /// </param>
        public NucleotideSequence Splice(List<Tuple<int, int, bool>> ExonRanges)
        {
            NucleotideSequence spliced = new NucleotideSequence();
            
            if (Nucleotides != null)
            {
                int startMin = Nucleotides.First().Key;
                int endMax = Nucleotides.Last().Key;
                
                foreach (var range in ExonRanges)
                {
                    if (!range.Item3)
                    {
                        if (startMin <= range.Item1 && endMax >= range.Item2)
                        {
                            spliced.Nucleotides = spliced.Nucleotides.Concat(
                                Nucleotides
                                .SkipWhile(kv => kv.Key < range.Item1)
                                .TakeWhile(kv => kv.Key <= range.Item2)
                                .Select(kv => kv))
                                .ToDictionary(kv => kv.Key, kv => kv.Value);
                        }
                    }
                    else
                    {
                        // Get the segment from the sequence, reverse it, complement it
                        if (startMin <= range.Item2 && endMax >= range.Item1)
                        {
                            spliced.Nucleotides = spliced.Nucleotides.Concat(
                                Nucleotides
                                .SkipWhile(kv => kv.Key < range.Item2)
                                .TakeWhile(kv => kv.Key <= range.Item1)
                                .Select(kv => kv)
                                .Reverse()
                                .Select(kv => new KeyValuePair<int, char>(kv.Key, kv.Value.ToComplement())))
                                .ToDictionary(kv => kv.Key, kv => kv.Value);
                        }
                    }
                }
            }

            return spliced;
        }

        /// <summary>
        /// Returns a deep copy of the current instance.
        /// </summary>
        public NucleotideSequence Copy()
        {
            NucleotideSequence clone = (NucleotideSequence)this.MemberwiseClone();
            clone.Nucleotides = new Dictionary<int, char>(this.Nucleotides.ToDictionary(kv => kv.Key, kv => kv.Value));
            return clone;
        }
    }

    public class Interval
    {
        public int From { get; set; }
        public int To { get; set; }
        public bool IsComplement { get; set; }
        public string Accession { get; set; }
    }
}
