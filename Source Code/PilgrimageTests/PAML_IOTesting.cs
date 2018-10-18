using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using ChangLab.Common;
using ChangLab.Jobs;
using ChangLab.PAML.CodeML;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PilgrimageTests
{
    [TestClass]
    public class PAML_Testing
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

        [ClassInitialize()]
        public static void TestInitialize(TestContext testContext)
        {
            StartTime = DateTime.Now;
            ChangLab.LocalDatabase.DataAccess.SetConnectionString("C:\\Data\\ChangLab\\db\\Debug_1_2_1_0.mdf");
        }

        [TestMethod]
        public void PAML_ParsingTests()
        {
            RunTreesAtCodeML codeml = new RunTreesAtCodeML(new CodeMLProcessOptions()
                {
                    ConcurrentProcesses = Environment.ProcessorCount,
                    Priority = System.Diagnostics.ProcessPriorityClass.BelowNormal,
                    CodeMLExecutablePath = "C:\\Data\\ChangLab\\paml4.8\\bin\\codeml.exe",
                    WorkingDirectory = "C:\\Data\\ChangLab\\PAML\\Pilgrimage"
                });
            codeml.Initialize();
            codeml.ProgressUpdate += new ProgressUpdateEventHandler(codeml_ProgressUpdate);

            Tree tree = new Tree()
            {
                TreeFilePath = "C:\\Data\\ChangLab\\PAML\\Pilgrimage\\AfR.tre",
                SequencesFilePath = "C:\\Data\\ChangLab\\PAML\\Pilgrimage\\AfR_cpt.txt",
                Status = JobStatusCollection.Get(JobStatuses.New),
                Rank = 1,
                JobID = codeml.ID
            };
            tree.Save();

            AnalysisConfiguration config = new AnalysisConfiguration()
            {
                TreeID = tree.ID,
                Model = 0,
                NCatG = 10,
                Rank = 1,
                KStart = 2,
                KEnd = 2,
                WStart = 2,
                WEnd = 2
            };
            config.NSSites.AddRange(new int[] { 0, 1 });
            config.Save();

            tree.AnalysisConfigurations.Add(config);
            
            codeml.Options.Trees = (new Tree[] { tree }).ToList();
            codeml.RunAnalyses();

            Assert.IsTrue(codeml.Process.Results.Count > 0, "No results!");
        }

        private void codeml_ProgressUpdate(ProgressUpdateEventArgs e)
        {
            OutputProgressMessage(e.ProgressMessage);
        }

        private void OutputProgressMessage(string Message)
        {
            System.Diagnostics.Debug.WriteLine(Message);
            TestContext.WriteLine(Message, new object[] { });
        }
    }
}
