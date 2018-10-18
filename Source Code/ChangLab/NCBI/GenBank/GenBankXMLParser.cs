using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using ChangLab.Common;
using ChangLab.Genes;

namespace ChangLab.NCBI.GenBank
{
    public class GenBankXMLParser : EUtilitiesXMLParser<Gene>
    {
        /// <summary>
        /// If True, the AdditionalRecordNeeded event will be triggered to fetch the sequence data for each segment of a segmented record, which will
        /// be compiled into the Gene instance returned.  True by default, set False when AdditionalRecordNeeded is being called to avoid a cascading
        /// fetch of segments.
        /// </summary>
        public bool CompileSegments { get; set; }

        public GenBankXMLParser()
        {
            CompileSegments = true;
        }

        public List<Gene> ParseFullRecord(XmlDocument doc)
        {
            return ParseINSDSeq(doc);
        }

        /// <remarks>
        /// Example: http://eutils.ncbi.nlm.nih.gov/entrez/eutils/efetch.fcgi?db=nuccore&retmode=xml&rettype=gbc&id=198386323
        /// </remarks>
        public List<Gene> ParseINSDSeq(XmlDocument doc)
        {
            try
            {
                List<Gene> records = new List<Gene>();

                foreach (XmlNode seqDoc in doc.SelectNodes("./INSDSet/INSDSeq"))
                {
                    string locus = seqDoc.SelectSingleNode("INSDSeq_locus").SafeInnerText();
                    string definition = seqDoc.SelectSingleNode("INSDSeq_definition").SafeInnerText();
                    string accession = seqDoc.SelectSingleNode("INSDSeq_accession-version").SafeInnerText();
                    string organism = seqDoc.SelectSingleNode("INSDSeq_organism").SafeInnerText();
                    string taxonomy = seqDoc.SelectSingleNode("INSDSeq_taxonomy").SafeInnerText();
                    DateTime lastUpdatedAt = DateTime.MinValue; string lastUpdatedAtString = seqDoc.SelectSingleNode("INSDSeq_update-date").SafeInnerText();
                    DateTime.TryParse(lastUpdatedAtString, out lastUpdatedAt);

                    int genBankId = 0;
                    XmlNode seqIdsNode = seqDoc.SelectSingleNode("INSDSeq_other-seqids");
                    if (seqIdsNode != null)
                    { genBankId = seqIdsNode.ChildNodes.Cast<XmlNode>().FirstOrDefault(n => n.InnerText.StartsWith("gi|")).SafeInnerText().Replace("gi|", "").ToSafeInt(); }

                    // Check to see if this is a segmented record.  If so, we're going to need another fetch to get all of the segments and then
                    // merge their sequence data together into this record.
                    int segmentNumber = 0; Dictionary<string, Gene> segments = null;
                    List<string> accessionsInIntervals = seqDoc.SelectNodes("./INSDSeq_feature-table/INSDFeature/INSDFeature_intervals/INSDInterval/INSDInterval_accession")
                                                            .Cast<XmlElement>()
                                                            .Select(el => el.InnerText)
                                                            .Distinct().ToList();
                    bool segmented = (accessionsInIntervals.Count > 1);
                    if (segmented)
                    {
                        string segmentDefinition = seqDoc.SafeChildNodeInnerText("INSDSeq_segment");
                        if (!string.IsNullOrWhiteSpace(segmentDefinition))
                        { segmentNumber = int.Parse(segmentDefinition.Substring(0, segmentDefinition.IndexOf(" "))); }
                        // We still need to capture the segment number because the parent gene record will need to sort by it, but we don't need to do
                        // anything else related to gathering segment data.
                        if (this.CompileSegments)
                        {
                            segments = new Dictionary<string, Gene>();
                            segments.Add(accession, null);
                            // The INSDSeq_sequence node for this INSD record will only contain this segment's sequence, not the whole sequence, so after
                            // parsing out the sequence info from this segment we can go after the other segments.  We have to parse the intervals anyway
                            // to get at the other accession IDs, so that we can use them for EFetch-ing.
                            segments = segments.Union(accessionsInIntervals
                                                            .Select(s => new KeyValuePair<string, Gene>(s, null)))
                                                            .ToDictionary(kv => kv.Key, kv => kv.Value);
                        }
                    }

                    // The source key provides us the indexing range for the sequence.  Typically this starts at 1 and ends at the length of the sequence.
                    Range sourceRange = new Range(false);
                    XmlElement sourceKeyElement = seqDoc.SelectNodes("./INSDSeq_feature-table/INSDFeature/INSDFeature_key").Cast<XmlElement>().FirstOrDefault(el => el.InnerText == "source");
                    if (sourceKeyElement != null)
                    {
                        XmlNode sourceIntervalNode = sourceKeyElement.ParentNode.SelectSingleNode("./INSDFeature_intervals/INSDInterval");
                        if (sourceIntervalNode != null)
                        {
                            sourceRange.Start = sourceIntervalNode.SelectSingleNode("INSDInterval_from").SafeInnerText().ToSafeInt();
                            sourceRange.End = sourceIntervalNode.SelectSingleNode("INSDInterval_to").SafeInnerText().ToSafeInt();
                        }
                    }

                    NucleotideSequence sequence = null;
                    string sourceSequence = string.Empty;
                    XmlNode sequenceNode = seqDoc.SelectSingleNode("(./INSDSeq_sequence)");
                    if (sequenceNode != null)
                    {
                        sourceSequence = sequenceNode.InnerText.ToUpper();
                    }
                    else if (AdditionalRecordNeeded != null)
                    {
                        AdditionalRecordNeededEventArgs args = new AdditionalRecordNeededEventArgs(genBankId, EUtilitiesXMLDocumentTypes.TSeq);
                        AdditionalRecordNeeded(args);
                        if (args.Result != null)
                        {
                            sourceSequence = args.Result[0].Nucleotides;
                        }
                    }
                    if (!string.IsNullOrEmpty(sourceSequence))
                    {
                        sequence = new NucleotideSequence(sourceSequence, (sourceRange.Length != 0 ? sourceRange.Start : 1));

                        if (sourceRange.Length == 0)
                        {
                            // Otherwise just use a 1-based index so we've got something for the gene and CDS ranges to work with.
                            sourceRange.Start = 1;
                            sourceRange.End = sourceSequence.Length;
                        }
                    }

                    string recordedCodingSequence = string.Empty;
                    Range recordedRange = new Range(false);
                    GeneSequenceTypes sequenceType = GeneSequenceTypes.NotDefined;
                    if (sequence != null)
                    {
                        // There can be multiple CDS features and each CDS feature can have multiple intervals
                        List<XmlElement> cdsKeyElements = seqDoc.SelectNodes("./INSDSeq_feature-table/INSDFeature/INSDFeature_key").Cast<XmlElement>().Where(el => el.InnerText == "CDS").ToList();
                        if (cdsKeyElements.Count != 0)
                        {
                            NucleotideSequence cdSequence = ExtractFromFeatureNodes(cdsKeyElements, sequence, accession);
                            recordedRange.Start = cdSequence.Start;
                            recordedRange.End = cdSequence.End;
                            recordedCodingSequence = cdSequence.ToString();
                            sequenceType = GeneSequenceTypes.Coding;
                        }
                        else
                        {
                            // Look for an exon feature element as an alternative; same multiple of multiples logic applies
                            List<XmlElement> exonKeyElements = seqDoc.SelectNodes("./INSDSeq_feature-table/INSDFeature/INSDFeature_key").Cast<XmlElement>().Where(el => el.InnerText == "exon").ToList();
                            if (exonKeyElements.Count != 0)
                            {
                                NucleotideSequence exSequence = ExtractFromFeatureNodes(exonKeyElements, sequence, accession);
                                recordedRange.Start = exSequence.Start;
                                recordedRange.End = exSequence.End;
                                recordedCodingSequence = exSequence.ToString();
                                sequenceType = GeneSequenceTypes.Coding;
                            }
                            else
                            {
                                // Try for a gene feature as a final alternative
                                List<XmlElement> geneKeyElements = seqDoc.SelectNodes("./INSDSeq_feature-table/INSDFeature/INSDFeature_key").Cast<XmlElement>().Where(el => el.InnerText == "gene").ToList();
                                if (geneKeyElements.Count != 0)
                                {
                                    NucleotideSequence geneSequence = ExtractFromFeatureNodes(geneKeyElements, sequence, accession);
                                    recordedRange.Start = geneSequence.Start;
                                    recordedRange.End = geneSequence.End;
                                    recordedCodingSequence = geneSequence.ToString();
                                    sequenceType = GeneSequenceTypes.Gene;
                                }
                            }
                        }
                    }

                    // Convert all of the sequence features into Feature objects
                    List<Feature> features = seqDoc
                                                .SelectNodes("./INSDSeq_feature-table/INSDFeature/INSDFeature_key")
                                                .Cast<XmlElement>()
                                                .Aggregate(new Dictionary<XmlNode, List<XmlNode>>(),
                                                            (current, el) =>
                                                            {
                                                                current.Add(el, el.ParentNode
                                                                                    .SelectNodes("./INSDFeature_intervals/INSDInterval")
                                                                                    .Cast<XmlNode>()
                                                                                    .Where(n => string.IsNullOrWhiteSpace(accession) || n.SelectSingleNode("INSDInterval_accession") == null || n.SelectSingleNode("INSDInterval_accession").SafeInnerText().ToLower() == accession.ToLower())
                                                                                    .ToList());
                                                                return current;
                                                            })
                                                .Aggregate(new List<Feature>(), (currentFeatureList, featureNodes) =>
                                                    {
                                                        GeneFeatureKeys key = GeneFeatureKeys.Undefined;
                                                        if (GeneFeatureKey.TryParse(featureNodes.Key.InnerText, out key))
                                                        {
                                                            Feature newFeature = new Feature()
                                                                {
                                                                    FeatureKey = GeneFeatureKeyCollection.Get(key),
                                                                    GeneQualifier = featureNodes.Key.ParentNode.SafeChildNodeInnerText("./INSDFeature_quals/INSDQualifier/INSDQualifier_name[text()='gene']/../INSDQualifier_value"),
                                                                    GeneIDQualifier = featureNodes.Key.ParentNode.SafeChildNodeInnerText("./INSDFeature_quals/INSDQualifier/INSDQualifier_name[text()='db_xref']/../INSDQualifier_value[contains(.,'GeneID:')]").Replace("GeneID:", "").ToSafeInt()
                                                                };

                                                            newFeature.Intervals.AddRange(
                                                                featureNodes.Value.Select((interval, index) =>
                                                                {
                                                                    FeatureInterval newInterval = new FeatureInterval()
                                                                        {
                                                                            ID = index + 1,
                                                                            Start = interval.SelectSingleNode("INSDInterval_from").SafeInnerText().ToSafeInt(),
                                                                            End = interval.SelectSingleNode("INSDInterval_to").SafeInnerText().ToSafeInt(),
                                                                            IsComplement = interval.SelectSingleNode("INSDInterval_iscomp") != null && interval.SelectSingleNode("INSDInterval_iscomp").SafeAttributeValue("value").ToSafeBoolean(),
                                                                            Accession = interval.SafeChildNodeInnerText("INSDInterval_accession")
                                                                        };
                                                                    if (string.IsNullOrWhiteSpace(newInterval.Accession)) { newInterval.Accession = accession; }
                                                                    return newInterval;
                                                                }));

                                                            currentFeatureList.Add(newFeature);
                                                        }
                                                        else
                                                        {
                                                            // If TryParse() fails then this is not a feature that we're tracking, but we at least
                                                            // want to capture it so I can check to see if it's something worth adding.
                                                            GeneFeatureKey.Survey(genBankId, featureNodes.Key.InnerText);
                                                        }

                                                        return currentFeatureList;
                                                    })
                                                .Select((f, index) => { f.Rank = (index + 1); return f; }) // Set as sequential by document order
                                                .ToList();

                    string geneName = string.Empty;
                    if (features.Count != 0)
                    {
                        // On the off-chance that there's more than one gene name given, pick one used the most.
                        geneName = features.GroupBy(f => f.GeneQualifier).Select(g => new { GeneName = g.Key, Count = g.Count() }).OrderByDescending(g => g.Count).First().GeneName;
                    }

                    Gene gene = new Gene()
                        {
                            GeneName = geneName,
                            Definition = definition,
                            SourceID = GeneSource.IDByKey(GeneSources.GenBank),
                            GenBankID = genBankId,
                            Accession = accession,
                            Locus = locus,
                            Organism = organism,
                            Taxonomy = taxonomy,
                            Nucleotides = recordedCodingSequence,
                            SequenceType = sequenceType,
                            SourceSequence = sequence,
                            Features = features,
                            SegmentNumber = segmentNumber,
                            LastUpdatedAt = lastUpdatedAt,
                            LastUpdateSource = GeneSources.GenBank
                        };

                    if (this.CompileSegments && segmented && segments.Count > 1 && AdditionalRecordNeeded != null)
                    {
                        segments[accession] = gene;
                        string[] segmentsNeeded = segments.Where(s => s.Key != accession).Select(kv => kv.Key).ToArray();

                        // Fetch the other segments by accession
                        AdditionalRecordNeededEventArgs segmentArgs = new AdditionalRecordNeededEventArgs(segmentsNeeded.ToList(), EUtilitiesXMLDocumentTypes.FullRecord);
                        AdditionalRecordNeeded(segmentArgs);

                        if (segmentArgs.Result != null)
                        {
                            segmentArgs.Result.ForEach(g =>
                                {
                                    if (segments.ContainsKey(g.Accession)) { segments[g.Accession] = g; }
                                });
                        }

                        // Merge the sequence data together, ordered by segment number (and accession in case there was no segment number)
                        IEnumerable<Gene> orderedSegments = segments.Select(kv => kv.Value).OrderBy(g => g.SegmentNumber).OrderBy(g => g.Accession.ExtractDouble());
                        gene.Features = orderedSegments.Aggregate(new List<Feature>(), (current, segment) => { current.AddRange(segment.Features); return current; }).ToList();
                        gene.Nucleotides = orderedSegments.Aggregate(string.Empty, (current, segment) => current += segment.Nucleotides);
                        gene.SourceSequence = orderedSegments.Aggregate(new NucleotideSequence(), (current, segment) => { current.Add(segment.SourceSequence.ToString()); return current; });
                    }

                    records.Add(gene);
                }

                return records;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <remarks>
        /// A given feature element can contain multiple intervals, so to splice out the specified sequence from the source whole sequence we first
        /// aggregate all of the intervals together and then pass them on to Splice(), which will loop through them.
        /// </remarks>
        private NucleotideSequence ExtractFromFeatureNodes(List<XmlElement> Elements, NucleotideSequence Sequence, string Accession)
        {
            return Sequence.Splice(
                Elements
                .Aggregate(new List<List<Interval>>(), (current, key) =>
                    {
                        current.Add(key
                                    .ParentNode
                                    .SelectNodes("./INSDFeature_intervals/INSDInterval")
                                    .Cast<XmlNode>()
                                    .Where(n => string.IsNullOrWhiteSpace(Accession) || n.SelectSingleNode("INSDInterval_accession") == null || n.SelectSingleNode("INSDInterval_accession").SafeInnerText().ToLower() == Accession.ToLower())
                                    .Select(node =>
                                        new Interval()
                                        {
                                            From = node.SafeChildNodeInnerText("INSDInterval_from").ToSafeInt(),
                                            To = node.SafeChildNodeInnerText("INSDInterval_to").ToSafeInt(),
                                            IsComplement = node.SafeChildNodeAttributeValue("INSDInterval_iscomp", "value").ToSafeBoolean(),
                                            Accession = node.SafeChildNodeInnerText("INSDInterval_accession")
                                        })
                                    .ToList());
                        return current;
                    }));

            //return Sequence.Splice(new List<Tuple<int, int, bool>>(
            //                    Elements
            //                    .Aggregate(new List<XmlNode>(), (current, el) => { current.AddRange(el.ParentNode.SelectNodes("./INSDFeature_intervals/INSDInterval").Cast<XmlNode>()); return current; })
            //                    .Where(n => string.IsNullOrWhiteSpace(Accession) || n.SelectSingleNode("INSDInterval_accession") == null || n.SelectSingleNode("INSDInterval_accession").SafeInnerText().ToLower() == Accession.ToLower())
            //                    .Select(n => new Tuple<int, int, bool>(
            //                        n.SelectSingleNode("INSDInterval_from").SafeInnerText().ToSafeInt(),
            //                        n.SelectSingleNode("INSDInterval_to").SafeInnerText().ToSafeInt(),
            //                        n.SelectSingleNode("INSDInterval_iscomp") != null && n.SelectSingleNode("INSDInterval_iscomp").SafeAttributeValue("value").ToSafeBoolean()
            //                    ))));
        }

        public List<Gene> ParseTSeq(XmlDocument doc)
        {
            try
            {
                List<Gene> records = new List<Gene>();

                foreach (XmlNode seqDoc in doc.SelectNodes("./TSeqSet/TSeq"))
                {
                    string definition = seqDoc.SelectSingleNode("TSeq_defline").SafeInnerText();
                    string accession = seqDoc.SelectSingleNode("TSeq_accver").SafeInnerText();
                    string organism = seqDoc.SelectSingleNode("TSeq_orgname").SafeInnerText();
                    int genBankId = seqDoc.SelectSingleNode("TSeq_gi").SafeInnerText().ToSafeInt();
                    string sequence = seqDoc.SelectSingleNode("TSeq_sequence").SafeInnerText();

                    records.Add(new Gene()
                    {
                        Definition = definition,
                        SourceID = GeneSource.IDByKey(GeneSources.GenBank),
                        GenBankID = genBankId,
                        Accession = accession,
                        Organism = organism,
                        Nucleotides = sequence,
                        SequenceType = GeneSequenceTypes.Source
                    });
                }

                return records;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Gene> ParseDocSum(XmlDocument doc)
        {
            try
            {
                List<Gene> records = new List<Gene>();

                foreach (XmlNode seqDoc in doc.SelectNodes("./eSummaryResult/DocSum"))
                {
                    string definition = seqDoc.SelectSingleNode("Item[@Name=\"Title\"]").SafeInnerText();
                    int genBankId = seqDoc.SelectSingleNode("Id").SafeInnerText().ToSafeInt();
                    int length = seqDoc.SelectSingleNode("Item[@Name=\"Length\"]").SafeInnerText().ToSafeInt();

                    string accession = string.Empty;
                    string extra = seqDoc.SelectSingleNode("Item[@Name=\"Extra\"]").SafeInnerText();
                    if (extra.Contains("|"))
                    {
                        string[] extraPieces = extra.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < extraPieces.Length; i++)
                        {
                            string piece = extraPieces[i];
                            switch (piece)
                            {
                                // These are all used for the accession number or accession version number
                                case "ref":
                                case "gb":
                                case "emb":
                                    if ((i + 1) < extraPieces.Length)
                                    {
                                        accession = extraPieces[i + 1];
                                        break;
                                    }
                                    break;
                            }
                        }
                    }

                    records.Add(new Gene()
                    {
                        Definition = definition,
                        SourceID = GeneSource.IDByKey(GeneSources.GenBank),
                        GenBankID = genBankId,
                        Accession = accession,
                        SequenceRange = new Range(1, length)
                    });
                }

                return records;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Events
        protected virtual void OnAdditionalRecordNeeded(AdditionalRecordNeededEventArgs e) { if (AdditionalRecordNeeded != null) { AdditionalRecordNeeded(e); } }
        public event AdditionalRecordNeededEventHandler AdditionalRecordNeeded;

        public delegate void AdditionalRecordNeededEventHandler(AdditionalRecordNeededEventArgs e);
        public class AdditionalRecordNeededEventArgs : EventArgs
        {
            public List<int> GenBankIDs { get; private set; }
            public List<string> Accessions { get; private set; }
            public EUtilitiesXMLDocumentTypes DocumentNeeded { get; private set; }
            public ESearchResult SearchResult { get; set; }
            public bool CompileSegments { get; set; }

            public List<Gene> Result { get; set; }

            public AdditionalRecordNeededEventArgs(int GenBankID, EUtilitiesXMLDocumentTypes DocumentNeeded, ESearchResult SearchResult = null)
                : this((new int[] { GenBankID }).ToList(), null, DocumentNeeded, SearchResult) { }
            public AdditionalRecordNeededEventArgs(string Accession, EUtilitiesXMLDocumentTypes DocumentNeeded, ESearchResult SearchResult = null)
                : this(null, (new string[] { Accession }).ToList(), DocumentNeeded, SearchResult) { }

            public AdditionalRecordNeededEventArgs(List<int> GenBankIDs, EUtilitiesXMLDocumentTypes DocumentNeeded, ESearchResult SearchResult = null)
                : this(GenBankIDs, null, DocumentNeeded, SearchResult) { }
            public AdditionalRecordNeededEventArgs(List<string> Accessions, EUtilitiesXMLDocumentTypes DocumentNeeded, ESearchResult SearchResult = null)
                : this(null, Accessions, DocumentNeeded, SearchResult) { }

            private AdditionalRecordNeededEventArgs(List<int> GenBankIDs, List<string> Accessions, EUtilitiesXMLDocumentTypes DocumentNeeded, ESearchResult SearchResult = null)
            {
                this.GenBankIDs = GenBankIDs;
                this.Accessions = Accessions;
                this.DocumentNeeded = DocumentNeeded;
                this.SearchResult = SearchResult;
                this.CompileSegments = false;
            }

            public string IDs(string Delimeter = ",")
            {
                if (this.GenBankIDs != null && this.GenBankIDs.Count != 0)
                {
                    return this.GenBankIDs.Concatenate<int>(Delimeter);
                }
                else if (this.Accessions != null && this.Accessions.Count != 0)
                {
                    return this.Accessions.Concatenate<string>(Delimeter);
                }
                else
                {
                    throw new Exception("No IDs have been configured.");
                }
            }
        }
        #endregion
    }
}
