using System;
using System.Collections.Generic;
using System.Linq;
using ChangLab.Common;
using ChangLab.Genes;
using ChangLab.NCBI.GenBank;
using ChangLab.RecordSets;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PilgrimageTests
{
    [TestClass]
    public class GenBank_Search
    {
        private TestContext testContextInstance;
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        private static bool UseExisting { get; set; }
        private static DateTime StartTime { get; set; }
        private static string StartTimeStamp { get { return StartTime.ToString("yyyyMMdd_hhmmss"); } }

        private RecordSet CurrentRecordSet { get; set; }
        private SubSet CurrentSubSet { get; set; }
        private GenBankSearch Search { get; set; }
        private List<Gene> FetchedGenes { get; set; }

        [ClassInitialize()]
        public static void TestInitialize(TestContext testContext)
        {
            UseExisting = false;
            StartTime = DateTime.Now;

            ChangLab.LocalDatabase.DataAccess.SetConnectionString("C:\\Data\\ChangLab\\db\\LocalDB.mdf");

            ChangLab.NCBI.EUtilities.ProductName = Utility.NCBIProductName;
            ChangLab.NCBI.EUtilities.Email = Utility.NCBIEmailAddress;
        }

        [TestMethod]
        public void GenBank_SearchTests()
        {
            SetupRecordSet();
            ListRecordSets();

            SearchGenBank_ForRPE65();
            DownloadMoreSearchResults();
            FetchFullRecords();

            SaveSelectedRecords();
        }

        public void SetupRecordSet()
        {
            CurrentRecordSet = new RecordSet() { Name = "Test " + StartTimeStamp };
            CurrentRecordSet.Save();
            Assert.AreNotEqual(string.Empty, CurrentRecordSet.ID, "ID not generated for RecordSet.");

            List<SubSet> subSets = CurrentRecordSet.ListSubSets(DataTypes.GeneSequence);
            Assert.AreEqual<int>(1, subSets.Count, "Less than or more than one SubSet was automatically generated for the new RecordSet.");
            CurrentSubSet = subSets[0];
        }

        public void ListRecordSets()
        {
            List<RecordSet> recordSets = RecordSet.List();
            
            Assert.AreNotEqual(0, recordSets.Count, "No RecordSets found.");

            if (UseExisting) { CurrentRecordSet = recordSets.FirstOrDefault(rs => rs.Name.StartsWith("Test ")); }

            Assert.IsTrue(recordSets.Any(rs => rs.ID == CurrentRecordSet.ID), CurrentRecordSet.Name + " not found.");

            List<SubSet> subSets = CurrentRecordSet.ListSubSets(DataTypes.GeneSequence);
            Assert.AreNotEqual<int>(0, subSets.Count, "No SubSets found.");
            
            CurrentSubSet = subSets.First();
            CurrentSubSet.ListAllGenes().ForEach(g =>
                {
                    CurrentRecordSet.RemoveGene(g, CurrentSubSet.ID);
                }
            );
        }

        public void SearchGenBank_ForRPE65()
        {
            //if (UseExisting)
            //{
            //    SearchResults = new GenBankSearchResult()
            //    {
            //        WebEnvironment = "NCID_1_392226652_130.14.22.215_9001_1400862098_885530396",
            //        QueryKey = "1",
            //        ResultCount = 316,
            //        ReturnMaximum = 20
            //    };
            //    SearchResults.FetchResults(0);
            //}
            //else
            //{
            Search = new GenBankSearch();
            Search.Search(CurrentRecordSet.ID, "RPE65[Gene Name]");
            //}

            Assert.AreNotEqual(string.Empty, Search.SearchResult.WebEnvironment, "Search results not stored on history server.");
            Assert.AreEqual(3, GeneSource.IDByKey(GeneSources.GenBank), "GenBank Source record not found as ID [3].");
        }

        public void DownloadMoreSearchResults()
        {
            int moreRecordCount = 40;

            for (int i = 20; i < moreRecordCount; i += 20)
            {
                System.Threading.Thread.Sleep(10000);
                Search.ResultsSummary(i);
            }

            Assert.AreEqual(moreRecordCount, Search.Count, "Not all search results were downloaded.");
        }

        public void FetchFullRecords()
        {
            List<int> genBankIds = new List<int>(new int[]
            {
                Search.Results[0].GenBankID,
                Search.Results[2].GenBankID,
                Search.Results[3].GenBankID,
                Search.Results[8].GenBankID,
                Search.Results[16].GenBankID,
                Search.Results[18].GenBankID,
                Search.Results[19].GenBankID,
                Search.Results[20].GenBankID,
                Search.Results[24].GenBankID,
                Search.Results[33].GenBankID,
                Search.Results[38].GenBankID,
                Search.Results[39].GenBankID
            });

            GenBankFetch fetch = new GenBankFetch();
            fetch.ProgressUpdate += new ProgressUpdateEventHandler(currentProgress_ProgressUpdate);
            fetch.ResultsDownloaded += new GenBankFetch.ResultsEventHandler(fetch_ResultsDownloaded);

            FetchedGenes = new List<Gene>();
            fetch.FetchRecords(genBankIds, Search.SearchResult);

            Assert.IsTrue(FetchedGenes.All(g => !string.IsNullOrWhiteSpace(g.Organism)), "Organism annotation was not retrieved for all genes.");
        }

        private void fetch_ResultsDownloaded(GenBankFetch.ResultsEventArgs e)
        {
            FetchedGenes.AddRange(e.Results);
        }

        private void SaveSelectedRecords()
        {
            FetchedGenes.ForEach(g =>
                {
                    g.Save();
                    CurrentRecordSet.AddGene(g, CurrentSubSet.ID);
                });

            CurrentRecordSet.RefreshGenesFromDatabase(CurrentSubSet.ID);
            Assert.AreEqual(FetchedGenes.Count, CurrentRecordSet.Genes(CurrentSubSet.ID).Count, "Not all fetched genes were saved as Pending.");
        }

        private void currentProgress_ProgressUpdate(ProgressUpdateEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(e.ProgressMessage);
            TestContext.WriteLine(e.ProgressMessage, new object[] { });
        }
    }
}