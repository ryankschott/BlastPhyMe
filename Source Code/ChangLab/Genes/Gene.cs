using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using ChangLab.Common;
using ChangLab.LocalDatabase;
using ChangLab.BlastN;

namespace ChangLab.Genes
{
    public class Gene
    {
        #region Properties
        public string ID { get; private set; }
        public bool New { get; set; }
        public bool RefreshedFromDatabase { get; set; }

        public string GeneName { get; set; }
        public string Definition { get; set; }
        public int SourceID { get; set; }
        public GeneSources Source
        {
            get { return GeneSource.KeyByID(this.SourceID); }
            set { this.SourceID = GeneSource.IDByKey(value); }
        }
        public bool SourceIsNCBI
        {
            get { return (new GeneSources[] { GeneSources.BLASTN_NCBI, GeneSources.GenBank }).Contains(this.Source); }
        }

        public DateTime LastUpdatedAt { get; set; }
        public int LastUpdateSourceID { get; set; }
        public GeneSources LastUpdateSource
        {
            get { return GeneSource.KeyByID(this.LastUpdateSourceID); }
            set { this.LastUpdateSourceID = GeneSource.IDByKey(value); }
        }

        public bool NeedsUpdateFromGenBank { get { return ((this.GenBankID != 0 || !string.IsNullOrWhiteSpace(this.Accession)) && this.LastUpdatedAt == DateTime.MinValue); } }

        public int GenBankID { get; set; }
        public string GenBankUrl
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(this.Accession))
                { return Gene.GetGenBankUrl(this.Accession); }
                else
                { return Gene.GetGenBankUrl(this.GenBankID); }
            }
        }

        public static string GetGenBankUrl(int GenBankID)
        {
            return ((GenBankID.ToSafeInt() != 0) ? "https://www.ncbi.nlm.nih.gov/nucleotide/" + GenBankID.ToString() : string.Empty);
        }

        public static string GetGenBankUrl(string AccessionVersion)
        {
            return (!string.IsNullOrWhiteSpace(AccessionVersion) ? "https://www.ncbi.nlm.nih.gov/nucleotide/" + AccessionVersion.ToString() : string.Empty);
        }

        public string Locus { get; set; }
        public string Accession { get; set; }
        public string Organism { get; set; }
        public string Taxonomy { get; set; }
        public string TaxonomyHierarchy { get; set; }
        public string Description { get; set; }

        public string Nucleotides { get; set; }
        public int SequenceTypeID { get; set; }
        public GeneSequenceTypes SequenceType
        {
            get { return GeneSequenceType.KeyByID(this.SequenceTypeID); }
            set { this.SequenceTypeID = GeneSequenceType.IDByKey(value); }
        }

        /// <remarks>
        /// Originally this was used to record the length of the source sequence, but that was prior to Pilgrimage capturing the sequence annotation
        /// from the GenBank record, and things like the SourceSequence and Feature table, which made this field all but obsolete.  The only place
        /// this is still being used is for shell GenBank search result records, for which there is no nucleotide sequence attached but there is a
        /// length value, and we want to be able to present that to the user.
        /// </remarks>
        public Range SequenceRange { get; set; }

        /// <summary>
        /// At present there's nothing apparent in an INSD record to link segments together, no common identifier, so we're only using this in a
        /// temporary sense when piecing together a segmented record in the GenBankXMLParser.  I don't yet have a means to link together a segmented
        /// record, so I'm just merging the other segments into whichever segment the user hit first and then calling that the whole thing.
        /// The only problem there is that then the user doesn't have the other GenBankIDs in their database, and could conceivably hit one of the
        /// other segments, thus merging in all of those segments again into the other segment.  At the moment that's not a big deal because the
        /// "merging" is just the sequence and features, but at some point we may want to acknowledge that records are segmented and link them
        /// together with my own ID.
        /// </summary>
        public int SegmentNumber { get; set; }
        public NucleotideSequence SourceSequence { get; set; }
        public List<Feature> Features { get; set; }

        public List<Exception> Exceptions { get; private set; }
        #endregion

        public Gene() : this(Guid.NewGuid().ToString()) { }

        public Gene(string ID)
        {
            // We're giving new Gene instances an ID because that allows for unique tracking on forms that use Gene instances that haven't come from
            // the database, such as Pilgrimage.Search.frmSearchResultsGenBank.  Gene provides the structure for the rows of search results in that
            // form and consistency with use of Pilgrimage.GeneSequences.frmGeneDetails.  Tracking of selected Gene records relies upon a unique ID value
            // for each row, so if this value was left empty, to indicate a "new" gene, that tracking wouldn't work.
            // Then there's an exception in Pilgrimage.RecordSets.frmImportFromFASTA where we want a completely "empty" Gene record to serve as a 
            // blank, "unselected" row.
            this.ID = ID;
            // To give us a way to know whether the Gene is in the database, we use the New flag.  True = no, False = yes.
            New = true;
            RefreshedFromDatabase = false;
            
            Initialize();
        }

        private void Initialize()
        {
            SequenceRange = new Range();
            Features = new List<Feature>();
            LastUpdatedAt = DateTime.MinValue;
            Exceptions = new List<Exception>();
        }

        /// <summary>
        /// Performs a local update of a Gene instance by overwriting editable values.  Intended to be used after downloading a full GenBank record
        /// via an EFetch.
        /// </summary>
        public Gene Merge(Gene UpdateWith, bool OverwriteNucleotides = true, bool OverwriteSequenceData = true, bool OverwriteDescription = true)
        {
            //this.SourceID = UpdateWith.SourceID; // This is purposefully not overwritten; the source should stay as the original source.
            this.GeneName = UpdateWith.GeneName;
            this.Definition = UpdateWith.Definition;
            this.LastUpdatedAt = UpdateWith.LastUpdatedAt;
            this.LastUpdateSourceID = UpdateWith.LastUpdateSourceID;
            this.GenBankID = UpdateWith.GenBankID;
            this.Locus = UpdateWith.Locus;
            this.Accession = UpdateWith.Accession;
            this.Organism = UpdateWith.Organism;
            this.Taxonomy = UpdateWith.Taxonomy;
            this.TaxonomyHierarchy = UpdateWith.TaxonomyHierarchy;
            
            if (OverwriteNucleotides)
            {
                this.Nucleotides = UpdateWith.Nucleotides;
                this.SequenceType = UpdateWith.SequenceType;
            }

            if (OverwriteSequenceData)
            {
                if (UpdateWith.SourceSequence != null) { this.SourceSequence = UpdateWith.SourceSequence.Copy(); }
                if (UpdateWith.Features.Count != 0) { this.Features = new List<Feature>(UpdateWith.Features.Select(f => f.Copy())); }
            }

            if (OverwriteDescription)
            {
                this.Description = UpdateWith.Description;
            }

            return this;
        }

        /// <summary>
        /// Returns a deep copy of the current instance.
        /// </summary>
        public Gene Copy()
        {
            Gene copy = (Gene)this.MemberwiseClone();
            copy.SourceSequence = this.SourceSequence.Copy();
            copy.Features = new List<Feature>(this.Features.Select(f => f.Copy()));
            return copy;
        }

        /// <summary>
        /// Convenience function to generate a feature with a single interval from the gene's source sequence.
        /// </summary>
        /// <remarks>
        /// Used by functionality that imports sequences from FASTA files and is thus has no annotation information to work with.
        /// </remarks>
        public Feature CreateFeatureFromSequence(GeneFeatureKeys FeatureKey)
        {
            Feature newFeature = new Feature() { GeneID = this.ID, FeatureKey = GeneFeatureKeyCollection.Get(FeatureKey), Rank = 1 };
            newFeature.Intervals.Add(new FeatureInterval() { Start = this.SourceSequence.Start, End = this.SourceSequence.End });
            return newFeature;
        }

        #region Database
        public static Gene Get(string GeneID, bool IncludeSequenceData = true)
        {
            using (DataAccess da = new DataAccess("Gene.Gene_Get"))
            {
                da.AddParameter("GeneID", System.Data.SqlDbType.UniqueIdentifier, GeneID);
                using (DataTable result = da.ExecuteDataTable())
                {
                    if (result.Rows.Count == 1)
                    {
                        Gene gene = Gene.FromDatabaseRow(result.Rows[0]);
                        if (IncludeSequenceData) { gene.GetSequenceData(); }
                        return gene;
                    }
                    else
                    { return null; }
                }
            }
        }

        public void GetSequenceData()
        {
            using (DataAccess da = new DataAccess("Gene.Gene_GetSequenceData"))
            {
                da.AddParameter("GeneID", System.Data.SqlDbType.UniqueIdentifier, this.ID);
                using (DataSet result = da.ExecuteDataSet())
                {
                    Features =
                        result
                        .Tables[0]
                        .Rows
                        .Cast<DataRow>()
                        .GroupBy(row => new Feature() 
                            {
                                ID = (int)row["FeatureID"],
                                FeatureKey = GeneFeatureKeyCollection.Get((int)row["FeatureKeyID"]),
                                Rank = (int)row["Rank"],
                                GeneQualifier = row.ToSafeString("GeneQualifier"),
                                GeneIDQualifier = row.ToSafeInt("GeneIDQualifier")
                            })
                        .Select(grp =>
                            {
                                grp.Key.Intervals.AddRange(
                                    grp.Select(intervalRow => 
                                        new FeatureInterval()
                                            {
                                                FeatureID = grp.Key.ID,
                                                ID = (int)intervalRow["ID"],
                                                Start = (int)intervalRow["Start"],
                                                StartModifier = intervalRow["StartModifier"].ToString(),
                                                End = (int)intervalRow["End"],
                                                EndModifier = intervalRow["EndModifier"].ToString(),
                                                IsComplement = (bool)intervalRow["IsComplement"]
                                            }));
                                return grp;
                            })
                        .Select(grp => grp.Key)
                        .ToList();

                    if (result.Tables[1].Rows.Count != 0)
                    {
                        SourceSequence = new NucleotideSequence((string)result.Tables[1].Rows[0]["Nucleotides"], (int)result.Tables[1].Rows[0]["Start"]);
                    }
                }
            }
        }

        public static Gene FromDatabaseRow(DataRow Row, bool PopulateWithSequenceData = false)
        {
            Gene gene = new Gene()
            {
                ID = Row["ID"].ToString(),
                New = false,
                RefreshedFromDatabase = true,
                GeneName = Row.ToSafeString("Name"),
                Definition = (string)Row["Definition"],
                SourceID = Row.ToSafeInt("SourceID"),
                LastUpdatedAt = Row.ToSafeDateTime("LastUpdatedAt"),
                LastUpdateSourceID = Row.ToSafeInt("LastUpdateSourceID"),

                GenBankID = Row.ToSafeInt("GenBankID"),
                Locus = Row.ToSafeString("Locus"),
                Accession = Row.ToSafeString("Accession"),
                Organism = Row.ToSafeString("Organism"),
                Taxonomy = Row.ToSafeString("Taxonomy"),
                TaxonomyHierarchy = Row.ToSafeString("TaxonomyHierarchy"),
                Description = Row.ToSafeString("Description"),
                Nucleotides = Row.ToSafeString("Nucleotides"),
                SequenceTypeID = Row.ToSafeInt("SequenceTypeID")
            };

            if (PopulateWithSequenceData) { gene.GetSequenceData(); }

            return gene;
        }

        public void Save(bool AllowOverwrite = false, bool SequenceData = false)
        {
            using (DataAccess da = new DataAccess("Gene.Gene_Edit"))
            {
                da.AddParameter("ID", System.Data.SqlDbType.UniqueIdentifier, 0, this.ID, ParameterDirection.InputOutput);
                da.AddParameter("Name", System.Data.SqlDbType.VarChar, 100, this.GeneName, true);
                da.AddParameter("Definition", System.Data.SqlDbType.VarChar, 1000, this.Definition);
                da.AddParameter("SourceID", this.SourceID);

                da.AddParameter("GenBankID", this.GenBankID, true);
                da.AddParameter("Locus", System.Data.SqlDbType.VarChar, 100, this.Locus, true);
                da.AddParameter("Accession", System.Data.SqlDbType.VarChar, 20, this.Accession, true);
                da.AddParameter("Organism", System.Data.SqlDbType.VarChar, 250, this.Organism, true);
                da.AddParameter("Taxonomy", System.Data.SqlDbType.VarChar, 4000, this.Taxonomy, true);
                da.AddParameter("Description", System.Data.SqlDbType.VarChar, -1, this.Description, true);

                da.AddParameter("Nucleotides", System.Data.SqlDbType.VarChar, this.Nucleotides, true);
                da.AddParameter("SequenceTypeID", System.Data.SqlDbType.Int, GeneSequenceType.IDByKey(this.SequenceType), true);
                da.AddParameter("LastUpdatedAt", System.Data.SqlDbType.DateTime2, this.LastUpdatedAt, true);
                da.AddParameter("LastUpdateSourceID", this.LastUpdateSourceID, true);

                da.AddParameter("AllowOverwrite", AllowOverwrite);

                // If the gene already exists in the database (by GenBankID) but we treated this as a new Gene record, such as by downloading search
                // results from GenBank, the database will do an update and give us back the already existing record's ID.
                this.ID = da.ExecuteParameter("ID").ToString();
                this.New = false;

                if (AllowOverwrite && SequenceData)
                {
                    if (SourceSequence != null)
                    {
                        SourceSequence.GeneID = this.ID;
                        SourceSequence.Save();
                    }
                    if (Features.Count != 0)
                    {
                        Feature.DeleteAll(this.ID);
                        Features.ForEach(f => { f.GeneID = this.ID; f.Add(); });
                    }
                }
            }
        }

        public static void EditName(IEnumerable<string> GeneIDs, string GeneName, GeneSources UpdatedBy)
        {
            using (DataAccess da = new DataAccess("Gene.Gene_EditName_Multiple"))
            {
                da.AddListParameter("GeneIDs", GeneIDs.Select(id => Guid.Parse(id)));
                da.AddParameter("Name", SqlDbType.VarChar, 100, GeneName, true);
                da.AddParameter("LastUpdateSourceID", GeneSource.IDByKey(UpdatedBy));

                da.ExecuteCommand();
            }
        }

        public static DataTable ForExport(IEnumerable<string> GeneIDs)
        {
            using (DataAccess da = new DataAccess("Gene.Gene_ForExport"))
            {
                da.AddListParameter("GeneIDs", GeneIDs.Select(id => Guid.Parse(id)));
                return da.ExecuteDataTable();
            }
        }

        public static List<KeyValuePair<string, int>> FilterAtDatabase(List<string> GeneIDs, bool DuplicateByOrganismName, bool DuplicateWholeSequences)
        {
            List<KeyValuePair<string, int>> filteredGeneIds = new List<KeyValuePair<string, int>>();

            using (DataAccess da = new DataAccess("Gene.Gene_FilterBy"))
            {
                da.AddListParameter("GeneIDs", GeneIDs, DataAccess.ListParameterTypes.ListVarCharIndentifier);
                da.AddParameter("DuplicateByOrganismName", SqlDbType.Bit, DuplicateByOrganismName);
                da.AddParameter("DuplicateWholeSequences", SqlDbType.Bit, DuplicateWholeSequences);
                
                using (System.Data.SqlClient.SqlDataReader reader = da.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        filteredGeneIds.Add(new KeyValuePair<string, int>((string)reader["GeneID"], (int)reader["DuplicateWholeSequenceGroupNumber"]));
                    }
                }
            }

            return filteredGeneIds;
        }
        #endregion
    }

    /// <summary>
    /// Used for grouping genes by something more complicated than a GroupBy() would handle.
    /// </summary>
    public class GeneBatch : List<BatchedGene>
    {
        /// <remarks>
        /// This could be calculated on the fly with Aggregate(), but that was grossly innefficient where this all gets used in SubmitToNCBI, so
        /// instead it's being set as genes are added to the collection.
        /// </remarks>
        internal int NucleotideCount { get; set; }

        public new void Add(BatchedGene Gene)
        {
            base.Add(Gene);
            NucleotideCount += Gene.Gene.Nucleotides.Length;
        }

        public GeneBatch() { }
    }

    public class BatchedGene
    {
        public Gene Gene { get; set; }
        public int Index { get; set; }
    }
}