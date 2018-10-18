using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using ChangLab.Common;
using ChangLab.LocalDatabase;

namespace ChangLab.Jobs
{
    public class IOPilgrimageDataFile : Job
    {
        #region Import
        public static void Import(string FilePath, string RecordSetName, string CurrentRecordSetID, string CurrentSubSetID, out string NewRecordSetID, out string JobID)
        {
            using (System.IO.FileStream fileStream = new System.IO.FileStream(FilePath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                using (System.IO.Compression.GZipStream gzip = new System.IO.Compression.GZipStream(fileStream, System.IO.Compression.CompressionMode.Decompress))
                {
                    XDocument doc = XDocument.Load(gzip);

                    using (DataAccess da = new DataAccess("RecordSet.Import_DataFile"))
                    {
                        da.CommandTimeout = 300;
                        da.AddParameter("x", SqlDbType.Xml, -1, doc);
                        da.AddParameter("JobRecordSetID", SqlDbType.UniqueIdentifier, CurrentRecordSetID);
                        da.AddParameter("RecordSetName", SqlDbType.VarChar, 200, RecordSetName, true);
                        da.AddParameter("TargetSubSetID", SqlDbType.UniqueIdentifier, CurrentSubSetID, true);
                        da.AddParameter("NewRecordSetID", SqlDbType.UniqueIdentifier, 16, null, ParameterDirection.InputOutput);
                        da.AddParameter("JobID", SqlDbType.UniqueIdentifier, 16, null, ParameterDirection.InputOutput);

                        da.ExecuteCommand();

                        NewRecordSetID = da.Parameters["NewRecordSetID"].Value.ToString();
                        JobID = da.Parameters["JobID"].Value.ToString();
                    }
                }
            }
        }

        private int TotalMax { get; set; }
        private int TotalProgress { get; set; }

        public void Import(string FilePath, string RecordSetName)
        {
            try
            {
                this.TotalMax = 9; this.TotalProgress = 0;
                ProgressUpdateEventArgs progress = null;
                this.OnProgressUpdate(new ProgressUpdateEventArgs()
                {
                    Setup = true,
                    ProgressMessage = "Parsing data file...",
                    TotalMax = this.TotalMax
                });

                XDocument doc = null;
                using (System.IO.FileStream fileStream = System.IO.File.OpenRead(FilePath))
                {
                    using (System.IO.Compression.GZipStream gzip = new System.IO.Compression.GZipStream(fileStream, System.IO.Compression.CompressionMode.Decompress))
                    {
                        doc = XDocument.Load(gzip);
                    }
                }

                IEnumerable<XElement> test = null;
                string value = string.Empty;

                Dictionary<string, string> SubSetIDs = new Dictionary<string, string>();
                Dictionary<string, string> GeneIDs = new Dictionary<string, string>();
                Dictionary<string, string> JobIDs = new Dictionary<string, string>();
                Dictionary<int, int> RequestIDs = new Dictionary<int, int>();
                Dictionary<int, int> AlignmentIDs = new Dictionary<int, int>();

                // Source database version validation
                test = doc.Root.Elements("Properties");
                if (test == null || test.Count() == 0) { throw new Exception("no properties specified"); }
                else
                {
                    test = test.First().Elements("DatabaseVersion");
                    if (test == null || test.Count() == 0) { throw new Exception("no version specified"); }
                    else
                    {
                        value = test.First().Value;
                        Version dataFileVersion = null;
                        if (Version.TryParse(value, out dataFileVersion))
                        {
                            // The data file has a valid database version stamp.
                            // Now we need to check to see if the version can be imported into the current database.
                            Version lastImportableDataFileVersion = Version.Parse(ChangLab.Common.ApplicationProperty.Get("LastImportableDataFileVersion").Value);
                            if (dataFileVersion < lastImportableDataFileVersion)
                            { throw new Exception("data file exported from an incompatible database version"); }

                            /*
                            using (ChangLab.LocalDatabase.AutoUpdate au = new ChangLab.LocalDatabase.AutoUpdate(string.Empty, dataFileVersion.ToString()))
                            { if (au.DatabaseExceedsApplicationVersion || au.UpgradesExist) { throw new Exception("database version mismatch"); } }
                            */
                        }
                        else { throw new Exception("invalid version format"); }
                    }
                }

                if (CancellationPending) { return; }

                using (DataAccess da = new DataAccess(true))
                {
                    try
                    {
                        #region RecordSet
                        this.RecordSetID = string.Empty;

                        test = doc.Root.Elements("RecordSet");
                        if (test == null || test.Count() != 1) { throw new Exception("fewer than, or more than, one project found"); } // Multiple RecordSet nodes would also be a problem.
                        else
                        {
                            this.OnProgressUpdate(new ProgressUpdateEventArgs()
                            {
                                ProgressMessage = "Importing project...",
                                TotalProgress = this.TotalProgress++
                            });

                            XElement recordSet = test.First();
                            da.ChangeCommand("RecordSet.Import_RecordSet");
                            da.AddParameter("RecordSetID", System.Data.SqlDbType.UniqueIdentifier, 16, string.Empty, System.Data.ParameterDirection.InputOutput, true);
                            da.AddParameter("Name", System.Data.SqlDbType.VarChar, 200, RecordSetName);
                            da.AddParameter("CreatedAt", System.Data.SqlDbType.DateTime2, 7, recordSet.Element("CreatedAt").ToDateTime());
                            da.AddParameter("LastOpenedAt", System.Data.SqlDbType.DateTime2, 7, recordSet.Element("LastOpenedAt").ToDateTime());

                            RecordSetID = da.ExecuteParameter("RecordSetID").ToString();

                            if (string.IsNullOrWhiteSpace(RecordSetID)) { throw new Exception("failed to create project"); }

                            // RecordSet-Property
                            // These don't have to exist, thus there's no validation.
                            foreach (XElement element in recordSet.Elements("Properties"))
                            {
                                da.ChangeCommand("RecordSet.Import_RecordSet_Property");
                                da.AddParameter("RecordSetID", System.Data.SqlDbType.UniqueIdentifier, RecordSetID);
                                da.AddParameter("Key", System.Data.SqlDbType.VarChar, 30, element.Element("Key").Value);
                                da.AddParameter("Value", System.Data.SqlDbType.VarChar, -1, element.Element("Value").Value);

                                da.ExecuteCommand();
                            }

                            if (CancellationPending) { Rollback(RecordSetID); return; }
                        }
                        #endregion

                        #region RecordSet-Gene
                        List<XElement> recordSetGeneElements = new List<XElement>();

                        if (TestForElements(doc, "RecordSet-Gene", "Gene", out test)) { recordSetGeneElements.AddRange(test.ToList()); }

                        if (recordSetGeneElements.Count != 0)
                        {
                            ResetCurrentProgress("Importing gene records...", (recordSetGeneElements.Count));
                            progress = new ProgressUpdateEventArgs() { CurrentProgress = 0 };

                            foreach (XElement element in recordSetGeneElements)
                            {
                                da.ChangeCommand("RecordSet.Import_RecordSet_Gene");
                                SetupGeneInsert(da, element, this.RecordSetID);
                                GeneIDs.Add(element.Element("ID").Value, da.ExecuteParameter("ID").ToString());

                                if (CancellationPending) { Rollback(RecordSetID); return; }

                                progress.CurrentProgress++;
                                this.OnProgressUpdate(progress);
                            }
                        }

                        if (TestForElements(doc, "RecordSet-Gene-Sequence", "Sequence", out test))
                        {
                            ResetCurrentProgress("Importing gene sequences...", test.Count());
                            progress = new ProgressUpdateEventArgs() { CurrentProgress = 0 };

                            foreach (XElement element in test)
                            {
                                da.ChangeCommand("Gene.NucleotideSequence_Edit");
                                da.AddParameter("GeneID", System.Data.SqlDbType.UniqueIdentifier, GeneIDs[element.Element("GeneID").Value]);
                                da.AddParameter("Nucleotides", element.Element("Nucleotides").Value);
                                da.AddParameter("Start", int.Parse(element.Element("Start").Value));
                                da.AddParameter("End", int.Parse(element.Element("End").Value));
                                da.ExecuteCommand();

                                if (CancellationPending) { Rollback(RecordSetID); return; }

                                progress.CurrentProgress++;
                                this.OnProgressUpdate(progress);
                            }
                        }

                        if (TestForElements(doc, "RecordSet-Gene-Feature", "Feature", out test))
                        {
                            ResetCurrentProgress("Importing gene sequence features...", test.Count());
                            progress = new ProgressUpdateEventArgs() { CurrentProgress = 0 };

                            foreach (XElement element in test)
                            {
                                da.ChangeCommand("Gene.Feature_Add");
                                da.AddParameter("GeneID", System.Data.SqlDbType.UniqueIdentifier, GeneIDs[element.Element("GeneID").Value]);
                                da.AddParameter("Rank", int.Parse(element.Element("Rank").Value));
                                da.AddParameter("FeatureKeyID", int.Parse(element.Element("FeatureKeyID").Value));
                                da.AddParameter("GeneQualifier", SqlDbType.VarChar, 250, element.ElementToString("GeneQualifier"), true);
                                da.AddParameter("GeneIDQualifier", SqlDbType.Int, 0, element.ElementToInt("GeneIDQualifier"), true);
                                da.AddParameter("ID", System.Data.SqlDbType.Int, 0, 0, System.Data.ParameterDirection.InputOutput, true);

                                int featureId = (int)da.ExecuteParameter("ID");

                                if (CancellationPending) { Rollback(RecordSetID); return; }

                                foreach (XElement intervalElement in element.Elements("Feature-Interval"))
                                {
                                    da.ChangeCommand("Gene.FeatureInterval_Add");
                                    da.AddParameter("ID", int.Parse(intervalElement.Element("ID").Value));
                                    da.AddParameter("FeatureID", featureId);
                                    da.AddParameter("Start", int.Parse(intervalElement.Element("Start").Value));
                                    da.AddParameter("End", int.Parse(intervalElement.Element("End").Value));
                                    da.AddParameter("IsComplement", SqlDbType.Bit, intervalElement.Element("IsComplement").Value == "1");
                                    da.AddParameter("StartModifier", SqlDbType.Char, 1, intervalElement.Element("StartModifier").Value);
                                    da.AddParameter("EndModifier", SqlDbType.Char, 1, intervalElement.Element("EndModifier").Value);
                                    da.AddParameter("Accession", SqlDbType.VarChar, 20, intervalElement.ElementToString("Accession"), true);

                                    da.ExecuteCommand();

                                    if (CancellationPending) { Rollback(RecordSetID); return; }
                                }

                                progress.CurrentProgress++;
                                this.OnProgressUpdate(progress);
                            }
                        }
                        #endregion

                        #region RecordSet-SubSet
                        if (TestForElements(doc, "RecordSet-SubSet", "SubSet", out test))
                        {
                            ResetCurrentProgress("Importing datasets and assigning genes to datasets...", test.Count());

                            progress = new ProgressUpdateEventArgs() { CurrentProgress = 0 };
                            foreach (XElement element in test)
                            {
                                da.ChangeCommand("RecordSet.Import_SubSet");
                                da.AddParameter("RecordSetID", System.Data.SqlDbType.UniqueIdentifier, RecordSetID);
                                da.AddParameter("ID", System.Data.SqlDbType.UniqueIdentifier, 16, string.Empty, System.Data.ParameterDirection.InputOutput, true);
                                da.AddParameter("Name", System.Data.SqlDbType.VarChar, 100, element.Element("Name").Value);
                                da.AddParameter("DataTypeID", System.Data.SqlDbType.Int, element.ElementToInt("DataTypeID"));
                                da.AddParameter("LastOpenedAt", System.Data.SqlDbType.DateTime2, 7, element.ElementToDateTime("LastOpenedAt"));
                                da.AddParameter("Open", System.Data.SqlDbType.Bit, element.Element("Open").Value == "1");
                                da.AddParameter("DisplayIndex", System.Data.SqlDbType.Int, element.ElementToInt("DisplayIndex"));

                                string subSetID = da.ExecuteParameter("ID").ToString();
                                SubSetIDs.Add(element.Element("ID").Value, subSetID);

                                if (CancellationPending) { Rollback(RecordSetID); return; }

                                foreach (XElement geneElement in element.Elements("SubSet-Gene"))
                                {
                                    da.ChangeCommand("RecordSet.Import_SubSet_Gene");
                                    da.AddParameter("SubSetID", System.Data.SqlDbType.UniqueIdentifier, subSetID);
                                    da.AddParameter("GeneID", System.Data.SqlDbType.UniqueIdentifier, GeneIDs[geneElement.Element("GeneID").Value]);
                                    da.AddParameter("ModifiedAt", System.Data.SqlDbType.DateTime2, 7, geneElement.ElementToDateTime("ModifiedAt"));
                                    da.ExecuteCommand();

                                    if (CancellationPending) { Rollback(RecordSetID); return; }
                                }

                                progress.CurrentProgress++;
                                this.OnProgressUpdate(progress);
                            }
                        }
                        #endregion

                        #region Job
                        if (TestForElements(doc, "RecordSet-Job", "Job", out test))
                        {
                            ResetCurrentProgress("Importing job history...", test.Count());

                            progress = new ProgressUpdateEventArgs() { CurrentProgress = 0 };
                            foreach (XElement element in test)
                            {
                                da.ChangeCommand("RecordSet.Import_Job");
                                da.AddParameter("RecordSetID", System.Data.SqlDbType.UniqueIdentifier, RecordSetID);
                                da.AddParameter("ID", System.Data.SqlDbType.UniqueIdentifier, 16, string.Empty, System.Data.ParameterDirection.InputOutput, true);
                                da.AddParameter("SubSetID", System.Data.SqlDbType.UniqueIdentifier, element.ElementToString("SubSetID"), true);
                                da.AddParameter("TargetID", System.Data.SqlDbType.Int, element.ElementToInt("TargetID"));
                                da.AddParameter("StatusID", System.Data.SqlDbType.Int, element.ElementToInt("StatusID"));
                                da.AddParameter("StartedAt", System.Data.SqlDbType.DateTime2, 7, element.ElementToDateTime("StartedAt"));
                                da.AddParameter("EndedAt", System.Data.SqlDbType.DateTime2, 7, element.ElementToDateTime("EndedAt"));

                                string jobId = da.ExecuteParameter("ID").ToString();
                                JobIDs.Add(element.Element("ID").Value, jobId);

                                if (CancellationPending) { Rollback(RecordSetID); return; }

                                foreach (XElement geneElement in element.Elements("Job-Gene"))
                                {
                                    da.ChangeCommand("RecordSet.Import_Job_Gene");
                                    da.AddParameter("JobID", System.Data.SqlDbType.UniqueIdentifier, jobId);
                                    da.AddParameter("GeneID", System.Data.SqlDbType.UniqueIdentifier, GeneIDs[geneElement.Element("GeneID").Value]);
                                    da.AddParameter("DirectionID", System.Data.SqlDbType.Int, geneElement.ElementToInt("DirectionID"));
                                    da.ExecuteCommand();

                                    if (CancellationPending) { Rollback(RecordSetID); return; }
                                }

                                progress.CurrentProgress++;
                                this.OnProgressUpdate(progress);
                            }
                        }
                        #endregion

                        #region NCBI-Request
                        if (TestForElements(doc, "RecordSet-NCBI-Request", "Request", out test))
                        {
                            ResetCurrentProgress("Importing NCBI requests...", test.Count());

                            progress = new ProgressUpdateEventArgs() { CurrentProgress = 0 };
                            foreach (XElement element in test)
                            {
                                da.ChangeCommand("NCBI.Request_Edit");
                                da.AddParameter("ID", System.Data.SqlDbType.Int, 0, 0, System.Data.ParameterDirection.InputOutput, true);
                                da.AddParameter("RequestID", System.Data.SqlDbType.VarChar, 20, element.ElementToString("RequestID"));
                                da.AddParameter("JobID", System.Data.SqlDbType.UniqueIdentifier, JobIDs[element.Element("JobID").Value]);
                                da.AddParameter("TargetDatabase", System.Data.SqlDbType.VarChar, 250, element.ElementToString("TargetDatabase"), true);
                                da.AddParameter("Algorithm", System.Data.SqlDbType.VarChar, 20, element.ElementToString("Algorithm"), true);
                                da.AddParameter("StartTime", System.Data.SqlDbType.DateTime2, 7, element.ElementToDateTime("StartTime"));
                                da.AddParameter("EndTime", System.Data.SqlDbType.DateTime2, 7, element.ElementToDateTime("EndTime"));
                                da.AddParameter("LastStatus", System.Data.SqlDbType.VarChar, 8, element.ElementToString("LastStatus"), true);
                                da.AddParameter("StatusInformation", System.Data.SqlDbType.VarChar, -1, element.ElementToString("StatusInformation"), true);
                                da.AddParameter("LastUpdatedAt", System.Data.SqlDbType.DateTime2, 7, element.ElementToString("LastUpdatedAt"), true);

                                int requestId = (int)da.ExecuteParameter("ID");
                                RequestIDs.Add(int.Parse(element.Element("ID").Value), requestId);

                                if (CancellationPending) { Rollback(RecordSetID); return; }

                                foreach (XElement geneElement in element.Elements("Request-Gene"))
                                {
                                    da.ChangeCommand("NCBI.Gene_Edit");
                                    da.AddParameter("RequestID", requestId);
                                    da.AddParameter("GeneID", System.Data.SqlDbType.UniqueIdentifier, GeneIDs[geneElement.Element("GeneID").Value]);
                                    da.AddParameter("StatusID", System.Data.SqlDbType.Int, geneElement.ElementToInt("StatusID"));
                                    da.ExecuteCommand();

                                    if (CancellationPending) { Rollback(RecordSetID); return; }
                                }

                                progress.CurrentProgress++;
                                this.OnProgressUpdate(progress);
                            }
                        }
                        #endregion

                        #region BlastN-Alignment
                        if (TestForElements(doc, "RecordSet-Alignment", "Alignment", out test))
                        {
                            ResetCurrentProgress("Importing BLASTN alignment results...", test.Count());

                            progress = new ProgressUpdateEventArgs() { CurrentProgress = 0 };
                            foreach (XElement element in test)
                            {
                                da.ChangeCommand("RecordSet.Import_BlastN_Alignment");
                                da.AddParameter("ID", System.Data.SqlDbType.Int, 0, 0, System.Data.ParameterDirection.InputOutput, true);
                                da.AddParameter("NeedsExons", System.Data.SqlDbType.Bit, 1, false, System.Data.ParameterDirection.InputOutput);
                                da.AddParameter("QueryID", System.Data.SqlDbType.UniqueIdentifier, GeneIDs[element.Element("QueryID").Value]);
                                da.AddParameter("SubjectID", System.Data.SqlDbType.UniqueIdentifier, GeneIDs[element.Element("SubjectID").Value]);
                                da.AddParameter("Rank", System.Data.SqlDbType.Int, element.ElementToInt("Rank"));
                                da.AddParameter("LastUpdatedAt", System.Data.SqlDbType.DateTime2, 7, element.ElementToDateTime("LastUpdatedAt"));

                                int alignmentId = (int)da.ExecuteParameter("ID");
                                if (!AlignmentIDs.ContainsKey(int.Parse(element.Element("ID").Value)))
                                {
                                    // If both the genes already existed (by GenBankID) and the alignment already existed (by QueryID and SubjectID),
                                    // and we're importing an export from the same database, the ID that's in the export will match what 
                                    // RecordSet.Import_BlastN_Alignment returns because the procedure will return the existing alignment ID.
                                    AlignmentIDs.Add(int.Parse(element.Element("ID").Value), alignmentId);
                                }

                                if (CancellationPending) { Rollback(RecordSetID); return; }

                                if ((bool)da.Parameters["NeedsExons"].Value)
                                {
                                    foreach (XElement exonElement in element.Elements("Alignment-Exon"))
                                    {
                                        da.ChangeCommand("RecordSet.Import_BlastN_AlignmentExon");
                                        da.AddParameter("AlignmentID", alignmentId);
                                        da.AddParameter("OrientationID", System.Data.SqlDbType.Int, exonElement.ElementToInt("OrientationID"));
                                        da.AddParameter("BitScore", System.Data.SqlDbType.Float, exonElement.ElementToDouble("BitScore"));
                                        da.AddParameter("AlignmentLength", System.Data.SqlDbType.Int, exonElement.ElementToInt("AlignmentLength"));
                                        da.AddParameter("IdentitiesCount", System.Data.SqlDbType.Int, exonElement.ElementToInt("IdentitiesCount"));
                                        da.AddParameter("Gaps", System.Data.SqlDbType.Int, exonElement.ElementToInt("Gaps"));
                                        da.AddParameter("QueryRangeStart", System.Data.SqlDbType.Int, exonElement.ElementToInt("QueryRangeStart"));
                                        da.AddParameter("QueryRangeEnd", System.Data.SqlDbType.Int, exonElement.ElementToInt("QueryRangeEnd"));
                                        da.AddParameter("SubjectRangeStart", System.Data.SqlDbType.Int, exonElement.ElementToInt("SubjectRangeStart"));
                                        da.AddParameter("SubjectRangeEnd", System.Data.SqlDbType.Int, exonElement.ElementToInt("SubjectRangeEnd"));
                                        da.ExecuteCommand();

                                        if (CancellationPending) { Rollback(RecordSetID); return; }
                                    }
                                }

                                progress.CurrentProgress++;
                                this.OnProgressUpdate(progress);
                            }
                        }
                        #endregion

                        #region Request-Alignment
                        if (TestForElements(doc, "RecordSet-Request-Alignment", "Request", out test))
                        {
                            ResetCurrentProgress("Assigning alignments to NCBI requests...", test.Count());

                            progress = new ProgressUpdateEventArgs() { CurrentProgress = 0 };
                            foreach (XElement element in test)
                            {
                                da.ChangeCommand("RecordSet.Import_NCBI_BlastNAlignment");
                                da.AddParameter("RequestID", RequestIDs[int.Parse(element.Element("ID").Value)]);
                                da.AddListParameter("AlignmentIDs", element.Element("Alignments").Elements("AlignmentID").Select(ele => AlignmentIDs[int.Parse(ele.Value)]));
                                da.ExecuteCommand();

                                if (CancellationPending) { Rollback(RecordSetID); return; }

                                progress.CurrentProgress++;
                                this.OnProgressUpdate(progress);
                            }
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        if (!string.IsNullOrWhiteSpace(this.RecordSetID)) { Rollback(RecordSetID, ex); }
                        throw ex;
                    }
                }

                this.OnProgressUpdate(new ProgressUpdateEventArgs()
                {
                    Setup = true,
                    ProgressMessage = "Import completed",
                    CurrentMax = 100,
                    CurrentProgress = 100,
                    TotalMax = 100,
                    TotalProgress = 100
                });
            }
            catch (Exception ex)
            {
                throw new Exception("Invalid data file: " + ex.Message, ex);
            }
        }

        private void ResetCurrentProgress(string ProgressMessage, int Max)
        {
            this.OnProgressUpdate(new ProgressUpdateEventArgs()
            {
                Setup = true,
                ProgressMessage = ProgressMessage,
                CurrentMax = Max,
                CurrentProgress = 0,
                TotalMax = this.TotalMax,
                TotalProgress = this.TotalProgress++
            });
        }

        private bool TestForElements(XDocument Document, string ParentName, string CollectionName, out IEnumerable<XElement> Test)
        {
            Test = Document.Root.Elements(ParentName);
            if (Test != null && Test.Count() != 0)
            {
                Test = Test.Elements(CollectionName);
                if (Test != null && Test.Count() != 0)
                {
                    return true;
                }
            }

            return false;
        }

        private void SetupGeneInsert(DataAccess da, XElement element, string RecordSetID)
        {
            da.AddParameter("RecordSetID", System.Data.SqlDbType.UniqueIdentifier, RecordSetID, true);
            da.AddParameter("ID", System.Data.SqlDbType.UniqueIdentifier, 16, string.Empty, System.Data.ParameterDirection.InputOutput, true);
            da.AddParameter("Definition", System.Data.SqlDbType.VarChar, 1000, element.Element("Definition").Value);
            da.AddParameter("SourceID", System.Data.SqlDbType.Int, element.Element("SourceID").Value);
            da.AddParameter("GenBankID", System.Data.SqlDbType.Int, element.ElementToInt("GenBankID"), true);
            da.AddParameter("Locus", System.Data.SqlDbType.VarChar, 100, element.ElementToString("Locus"), true);
            da.AddParameter("Accession", System.Data.SqlDbType.VarChar, 20, element.ElementToString("Accession"), true);
            da.AddParameter("Organism", System.Data.SqlDbType.VarChar, 250, element.ElementToString("Organism"), true);
            da.AddParameter("Taxonomy", System.Data.SqlDbType.VarChar, 4000, element.ElementToString("Taxonomy"), true);
            da.AddParameter("Nucleotides", System.Data.SqlDbType.VarChar, -1, element.ElementToString("Nucleotides"), true);
            da.AddParameter("SequenceTypeID", System.Data.SqlDbType.Int, element.ElementToString("SequenceTypeID"), true);
            da.AddParameter("SequenceStart", System.Data.SqlDbType.Int, element.ElementToString("SequenceStart"), true);
            da.AddParameter("SequenceEnd", System.Data.SqlDbType.Int, element.ElementToString("SequenceEnd"), true);
            da.AddParameter("CodingSequenceStart", System.Data.SqlDbType.Int, element.ElementToString("CodingSequenceStart"), true);
            da.AddParameter("CodingSequenceEnd", System.Data.SqlDbType.Int, element.ElementToString("CodingSequenceEnd"), true);
            da.AddParameter("LastUpdatedAt", System.Data.SqlDbType.DateTime2, 7, element.ElementToDateTime("LastUpdatedAt"), true);
        }

        private void Rollback(string RecordSetID, Exception Error = null)
        {
            this.OnProgressUpdate(new ProgressUpdateEventArgs()
            {
                Setup = true,
                ProgressMessage = (Error != null ? "Error: " + Error.Message + "\r\n" : string.Empty) + "Removing imported records...",
                CurrentMax = 100,
                CurrentProgress = 100,
                TotalMax = 100,
                TotalProgress = 100
            });

            using (DataAccess da = new DataAccess("RecordSet.Import_Rollback"))
            {
                da.AddParameter("RecordSetID", System.Data.SqlDbType.UniqueIdentifier, RecordSetID);
                da.ExecuteCommand();
            }
        }
        #endregion

        #region Export
        public static void Export(string SourceSubSetID, List<string> SelectedGeneIDs, List<int> SelectedResultIDs, string FilePath, ExportOptions Options)
        {
            using (DataAccess da = new DataAccess("RecordSet.RecordSet_Export"))
            {
                da.CommandTimeout = 300;
                da.AddParameter("SourceSubSetID_ForSelectedRecords", SqlDbType.UniqueIdentifier, SourceSubSetID);
                if (SelectedGeneIDs != null && SelectedGeneIDs.Count != 0)
                {
                    da.AddListParameter("SelectedGeneIDs", SelectedGeneIDs.Select(id => Guid.Parse(id)));
                }
                else if (SelectedResultIDs != null && SelectedResultIDs.Count != 0)
                {
                    da.AddListParameter("SelectedResultIDs", SelectedResultIDs);
                }
                da.AddListParameter("IncludeJobHistory_TargetIDs", Options.JobTargetIDs);
                da.AddParameter("GeneOptions_IncludeAlignedSequences", Options.IncludeAlignedSequences);
                da.AddParameter("GeneOptions_IncludeGeneSequenceAnnotations", true);

                StreamToFile(da.ExecuteReader(), FilePath);
            }
        }

        public static void Export(string RecordSetID, string FilePath, ExportOptions Options)
        {
            using (DataAccess da = new DataAccess("RecordSet.RecordSet_Export"))
            {
                da.CommandTimeout = 300;
                da.AddParameter("RecordSetID", SqlDbType.UniqueIdentifier, RecordSetID);
                da.AddListParameter("SelectedSubSetIDs", Options.SubSetIDs.Select(s => Guid.Parse(s)));
                da.AddListParameter("IncludeJobHistory_TargetIDs", Options.JobTargetIDs);
                da.AddParameter("GeneOptions_IncludeAlignedSequences", Options.IncludeAlignedSequences);
                da.AddParameter("GeneOptions_IncludeGeneSequenceAnnotations", true);

                StreamToFile(da.ExecuteReader(), FilePath);
            }
        }

        private static void StreamToFile(System.Data.SqlClient.SqlDataReader Reader, string FilePath)
        {
            try
            {
                using (Reader)
                {
                    using (System.IO.FileStream fileStream = System.IO.File.Create(FilePath))
                    {
#if DEBUG
                        using (System.IO.FileStream uncompressedFileStream = System.IO.File.Create(FilePath.Substring(0, FilePath.LastIndexOf(".")) + ".xml"))
                        {
                            using (System.IO.StreamWriter uncompressedWriter = new System.IO.StreamWriter(uncompressedFileStream))
                            {
#endif
                                using (System.IO.Compression.GZipStream gzip = new System.IO.Compression.GZipStream(fileStream, System.IO.Compression.CompressionMode.Compress))
                                {
                                    using (System.IO.StreamWriter writer = new System.IO.StreamWriter(gzip))
                                    {
                                        writer.WriteLine("<Pilgrimage>");
#if DEBUG
                                        uncompressedWriter.WriteLine("<Pilgrimage>");
#endif

                                        while (Reader.Read())
                                        {
                                            if (!Reader.IsDBNull(1))
                                            {
                                                writer.Write((string)Reader[1]);
#if DEBUG
                                                uncompressedWriter.Write((string)Reader[1]);
#endif
                                            }
                                        }

                                        writer.WriteLine("</Pilgrimage>");
#if DEBUG
                                        uncompressedWriter.WriteLine("</Pilgrimage>");
#endif
                                    }
                                }
#if DEBUG
                            }
                        }
#endif
                    }
                }
            }
            catch (Exception ex) { throw ex; }
        }

        public class ExportOptions
        {
            public bool IncludeAlignedSequences { get; set; }
            public List<int> JobTargetIDs { get; set; }
            public List<string> SubSetIDs { get; set; }

            public List<string> GeneIDs { get; set; }
            public List<int> PAMLResultIDs { get; set; }

            public ExportOptions()
            {
                this.JobTargetIDs = new List<int>();
                this.SubSetIDs = new List<string>();
                this.GeneIDs = new List<string>();
                this.PAMLResultIDs = new List<int>();
            }
        }

        /*
        public void Export(string RecordSetID, string FilePath, bool IncludeJobHistory)
        {
            int expectedFragments = 9 + 1; // + 1 for the compressing step
            this.OnProgressUpdate(new ProgressUpdateEventArgs()
            {
                Setup = true,
                StatusMessage = "Exporting to data file...",
                CurrentMax = expectedFragments
            });
            ProgressUpdateEventArgs progress = new ProgressUpdateEventArgs() { CurrentProgress = 0 };
            
            using (DataAccess da = new DataAccess("RecordSet.RecordSet_Export"))
            {
                XDocument doc = new XDocument(new object[] { new XElement("Export") });
                XElement root = doc.Root;

                da.AddParameter("RecordSetID", SqlDbType.UniqueIdentifier, RecordSetID);
                da.AddParameter("JobHistory", SqlDbType.Bit, false);
                using (System.Data.SqlClient.SqlDataReader reader = da.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        try
                        {
                            if (reader[0] != DBNull.Value)
                            {
                                XElement element = XElement.Parse((string)reader[0]);
                                switch (element.Name.ToString())
                                {
                                    case "RecordSet": progress.StatusMessage = "Recordset properties"; break;
                                    case "RecordSet-SubSet": progress.StatusMessage = "Subsets"; break;
                                    case "RecordSet-Gene": progress.StatusMessage = "Genes"; break;
                                }
                                root.Add(element);
                            }
                        }
                        catch (Exception ex) { throw ex; }

                        progress.CurrentProgress++;
                        this.OnProgressUpdate(progress);
                    }
                }

                if (IncludeJobHistory)
                {
                    da.Parameters["JobHistory"].Value = true;
                    using (System.Data.SqlClient.SqlDataReader reader = da.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            try
                            {
                                if (reader[0] != DBNull.Value)
                                {
                                    XElement element = XElement.Parse((string)reader[0]);
                                    switch (element.Name.ToString())
                                    {
                                        case "RecordSet-Job": progress.StatusMessage = "BLASTN submission history"; break;
                                        case "RecordSet-NCBI-Request": progress.StatusMessage = "NCBI request history"; break;
                                        case "RecordSet-Alignment": progress.StatusMessage = "BLASTN alignment statistics"; break;
                                    }
                                    root.Add(element);
                                }
                            }
                            catch (Exception ex) { throw ex; }

                            progress.CurrentProgress++;
                            this.OnProgressUpdate(progress);
                        }
                    }
                }

                progress.StatusMessage = "Compressing data file...";
                progress.CurrentProgress = expectedFragments;
                this.OnProgressUpdate(progress);

                using (System.IO.FileStream fileStream = System.IO.File.Create(FilePath))
                {
                    using (System.IO.Compression.GZipStream gzip = new System.IO.Compression.GZipStream(fileStream, System.IO.Compression.CompressionMode.Compress))
                    {
                        doc.Save(gzip, SaveOptions.DisableFormatting);
                    }
                }
            }
        }
        */
        #endregion
    }
}
