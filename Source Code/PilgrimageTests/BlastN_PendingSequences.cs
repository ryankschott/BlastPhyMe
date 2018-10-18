using System;
using System.Collections.Generic;
using System.Linq;
using ChangLab.Common;
using ChangLab.Jobs;
using ChangLab.RecordSets;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PilgrimageTests
{
    [TestClass]
    public class BlastN_PendingSequences
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

        private static DateTime StartTime { get; set; }
        private static string StartTimeStamp { get { return StartTime.ToString("yyyyMMdd_hhmmss"); } }
        
        private RecordSet CurrentRecordSet { get; set; }
        private SubSet CurrentSubSet { get; set; }
        
        [ClassInitialize()]
        public static void TestInitialize(TestContext testContext)
        {
            StartTime = DateTime.Now;

            ChangLab.LocalDatabase.DataAccess.SetConnectionString("C:\\Data\\ChangLab\\db\\LocalDB.mdf");
        }
        
        [TestMethod]
        public void BlastN_PendingSequencesTest()
        {
            CurrentRecordSet = RecordSet.List().FirstOrDefault(rs => rs.Name.StartsWith("Test "));
            Assert.IsTrue(CurrentRecordSet != null, "Could not find test RecordSet.");

            List<SubSet> subSets = CurrentRecordSet.ListSubSets(DataTypes.GeneSequence);
            Assert.AreNotEqual<int>(0, subSets.Count, "No SubSets found.");
            CurrentSubSet = subSets.First();

            CurrentRecordSet.RefreshGenesFromDatabase(CurrentSubSet.ID);
            Assert.AreNotEqual(0, CurrentRecordSet.Count, "RecordSet not populated with genes.");

            BlastNAtNCBI job = new BlastNAtNCBI(new ChangLab.NCBI.BlastNWebServiceConfigurationSettings() { DatabaseName = "nr", Service = ChangLab.NCBI.BlastNServices.blastn }, CurrentSubSet.ID);
            job.ProgressUpdate += new ProgressUpdateEventHandler(job_ProgressUpdate);

            job.Initialize();
            job.SetupWebService(CurrentRecordSet.Genes(CurrentSubSet.ID));
            job.SubmitRequest();
        }

        void job_ProgressUpdate(ProgressUpdateEventArgs e)
        {
            string message = (!string.IsNullOrWhiteSpace(e.ProgressMessage) ? e.ProgressMessage : (!string.IsNullOrWhiteSpace(e.StatusMessage) ? e.StatusMessage : string.Empty));
            System.Diagnostics.Debug.WriteLine(message);
            TestContext.WriteLine(message, new object[] { });
        }
    }
}
