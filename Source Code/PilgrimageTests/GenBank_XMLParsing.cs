using System;
using System.Collections.Generic;
using System.Xml;
using ChangLab.Common;
using ChangLab.Genes;
using ChangLab.NCBI;
using ChangLab.NCBI.GenBank;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PilgrimageTests
{
    [TestClass]
    public class GenBank_XMLParsing
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
        private static string EFetchUrl_FASTA { get { return "https://eutils.ncbi.nlm.nih.gov/entrez/eutils/efetch.fcgi?db=nuccore&retmode=xml&rettype=fasta&id={0}"; } }

        [ClassInitialize()]
        public static void TestInitialize(TestContext testContext)
        {
            StartTime = DateTime.Now;
            ChangLab.LocalDatabase.DataAccess.SetConnectionString("C:\\Data\\ChangLab\\db\\Debug_1_2_1_0.mdf");
            EUtilities.ProductName = "Pilgrimage_ChangLab_UniversityOfToronto";
            EUtilities.Email = "daniel.gow@mail.utoronto.ca";
        }

        [TestMethod]
        public void GenBank_XMLParsingTests()
        {
            ParseXML_SingleCDSRange();
            ParseXML_MultipleCDSRanges();
            ParseXML_MultipleCDSNodes();
            ParseXML_MultipleExonNodes();
            ParseXML_NoSequenceProvided();
            ParseXML_OverlappingCDSRanges();
            ParseXML_Segmented();
        }

        public void ParseXML_SingleCDSRange()
        {
            TestXML("EFetch_641757509_SingleCDSRange.xml", 641757509, 0, 1602, Properties.Resources.GenBank_XMLParsing_SingleCDSRange_CDS);
        }

        public void ParseXML_MultipleCDSRanges()
        {
            TestXML("EFetch_198386323_MultipleCDSRanges.xml", 198386323, 0, 1602, Properties.Resources.GenBank_XMLParsing_MultipleCDSRanges_CDS);
        }

        public void ParseXML_MultipleCDSNodes()
        {
            TestXML("EFetch_1209649_MultipleCDSFeatureNodes.xml", 1209649, 0, 95, Properties.Resources.GenBank_XMLParsing_MultipleCDSFeatureNodes_CDS, 2);
        }

        public void ParseXML_MultipleExonNodes()
        {
            TestXML("EFetch_374081841_MultipleExonFeatureNodes.xml", 374081841, 0, 4080, Properties.Resources.GenBank_XMLParsing_MultipleExonFeatureNodes_CDS);
        }

        public void ParseXML_NoSequenceProvided()
        {
            TestXML("EFetch_634586027_NoSequence.xml", 634586027, 0, 3949, Properties.Resources.GenBank_XMLParsing_NoSequence_CDS);
        }

        public void ParseXML_OverlappingCDSRanges()
        {
            TestXML("EFetch_224028255_ComplementAndJoin_InCDSFeatures.xml", 224028255, 0, 4331, Properties.Resources.GenBank_XMLParsing_OverlappingCDSRanges_CDS);
        }

        public void ParseXML_Segmented()
        {
            TestXML("EFetch_5020190_Segmented.xml", 5020190, 2044, 1098, Properties.Resources.GenBank_XMLParsing_Segmented_CDS);
        }

        public void TestXML(string FileName, int GenBankID, int SourceSequenceLength, int NucleotidesLength, string Nucleotides, int ExpectedGenes = 1)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load((new System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location)).Directory.FullName + "\\TestFiles\\" + FileName);
            GenBankXMLParser parser = new GenBankXMLParser(); parser.AdditionalRecordNeeded += new GenBankXMLParser.AdditionalRecordNeededEventHandler(parser_SequenceNeeded);
            List<Gene> genes = parser.ParseINSDSeq(doc);

            Assert.AreEqual<int>(ExpectedGenes, genes.Count, "Incorrect number of genes parsed from XML.");
            Assert.AreEqual<int>(GenBankID, genes[0].GenBankID, "Incorrect GenBankID parsed from XML.");
            if (SourceSequenceLength != 0) { Assert.AreEqual<int>(SourceSequenceLength, genes[0].SourceSequence.Length, "Incorrect sequence range length."); }
            Assert.AreEqual<int>(NucleotidesLength, genes[0].Nucleotides.Length, "Incorrect nucleotide sequence length.");
            Assert.AreEqual<string>(Nucleotides, genes[0].Nucleotides, "Incorrect coding sequence.");

            OutputProgressMessage("Successfully parsed XML for [" + genes[0].GenBankID.ToString() + "]");
        }

        private void parser_SequenceNeeded(GenBankXMLParser.AdditionalRecordNeededEventArgs e)
        {
            if (e.DocumentNeeded != EUtilitiesXMLDocumentTypes.DocSum)
            {
                string retType = string.Empty;
                switch (e.DocumentNeeded)
                {
                    case EUtilitiesXMLDocumentTypes.FullRecord: retType = "gbc"; break;
                    case EUtilitiesXMLDocumentTypes.TSeq: retType = "fasta"; break;
                }

                GenBankXMLParser parser = new GenBankXMLParser() { CompileSegments = e.CompileSegments };
                string url = EUtilities.GetUrl(EUtilities.Services.EFetch, EUtilities.Databases.NucCore, (e.SearchResult != null), e.SearchResult)
                    + "&rettype=" + retType
                    + "&id=" + e.IDs();

                List<Gene> results = new List<Gene>();
                switch (e.DocumentNeeded)
                {
                    case EUtilitiesXMLDocumentTypes.FullRecord:
                        results.AddRange(parser.ParseFullRecord(XMLWebRequest.RequestDocument(url)));
                        break;
                    case EUtilitiesXMLDocumentTypes.TSeq:
                        results.AddRange(parser.ParseTSeq(XMLWebRequest.RequestDocument(url)));
                        break;
                }

                if (results.Count != 0)
                {
                    e.Result = results;
                }
            }
        }

        private void currentProgress_ProgressUpdate(ProgressUpdateEventArgs e)
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