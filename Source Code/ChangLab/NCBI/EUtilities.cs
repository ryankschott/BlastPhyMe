using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using ChangLab.Common;

namespace ChangLab.NCBI
{
    public class EUtilities
    {
        #region Properties and Enumerations
#if DEBUG
        private static string _urlBase = "https://eutils.ncbi.nlm.nih.gov/entrez/eutils/";
#else
        private static string _urlBase = "https://eutils.ncbi.nlm.nih.gov/entrez/eutils/";
#endif

        private static string _productName = string.Empty;
        public static string ProductName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_productName)) { throw new Exception("Product name has not been configured."); }
                else { return _productName; }
            }
            set { _productName = value; }
        }

        private static string _email = string.Empty;
        public static string Email
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_email)) { throw new Exception("Email has not been configured."); }
                else { return _email; }
            }
            set { _email = value; }
        }

        public enum Services
        {
            ESearch = 1,
            ESummary = 2,
            EFetch = 3
        }

        public enum Databases
        {
            NucCore = 1,
            Taxonomy = 2
        }

        public enum ReturnTypes
        {
            NotSpecified = 0,
            DocSum = 1,
            UIList = 2,
            XML = 3,
            Full = 4,
            Summary = 5,
            Gene_Table = 6,
            Native = 7,
            Acc = 8,
            FASTA = 9,
            SeqId = 10,
            GB = 11,
            GBC = 12
        }
        #endregion

        public static string GetUrl(Services Service, Databases Database, bool UseHistory, ESearchResult History)
        {
            return _urlBase
                + Service.ToString().ToLower() + ".fcgi?"
                + "tool=" + EUtilities.ProductName
                + "&email=" + EUtilities.Email
                + "&db=" + Database.ToString().ToLower()
                + "&retmode=xml"
                + "&usehistory=" + (UseHistory ? "y" : "n")
                + (History != null ? "&WebEnv=" + History.WebEnvironment + "&query_key=" + History.QueryKey : string.Empty);
        }

        #region EUtilities Services
        public static ESearchHistory Search(EUtilities.Databases Database, string Term, string RecordSetID)
        {
            if (string.IsNullOrWhiteSpace(Term)) { throw new ArgumentNullException("Term", "Term cannot be empty."); }
            else
            {
                try
                {
                    string url = EUtilities.GetUrl(EUtilities.Services.ESearch, Database, true, null)
                        + "&term=" + Term.Replace(" ", "+");
                    XmlDocument response = XMLWebRequest.RequestDocument(url);

                    ESearchHistory result = new ESearchHistory()
                    {
                        Term = Term,
                        TargetDatabase = Database.ToString().ToLower(),
                        ResultCount = response.SelectSingleNode("./eSearchResult/Count").SafeInnerText().ToSafeInt(),
                        QueryKey = response.SelectSingleNode("./eSearchResult/QueryKey").SafeInnerText(),
                        WebEnvironment = response.SelectSingleNode("./eSearchResult/WebEnv").SafeInnerText(),
                        ReturnMaximum = response.SelectSingleNode("./eSearchResult/RetMax").SafeInnerText().ToSafeInt()
                    };
                    result.Save(RecordSetID); // We don't save the IdList results; for now we just want to capture the WebEnv and QueryKey for reuse.

                    XmlNode idListNode = response.SelectSingleNode("./eSearchResult/IdList");
                    if (idListNode != null && idListNode.HasChildNodes)
                    {
                        result.IDList.AddRange(idListNode.ChildNodes.Cast<XmlNode>().Select(node => int.Parse(node.InnerText)));
                    }

                    return result;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public static XmlDocument Summary(EUtilities.Databases Database, ESearchResult History, int ReturnStart, int ReturnMaximum = 0)
        {
            string url = EUtilities.GetUrl(Services.ESummary, Database, true, History)
                + "&retstart=" + ReturnStart.ToString()
                + "&retmax=" + (ReturnMaximum == 0 ? History.ReturnMaximum : ReturnMaximum).ToString();

            return XMLWebRequest.RequestDocument(url);
        }
        #endregion
    }

    public interface EUtilitiesXMLParser<T>
    {
        /// <summary>
        /// Should pass through to whatever the most detailed parsing method is; i.e.: INSDSeq for nuccore database records.
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        List<T> ParseFullRecord(XmlDocument doc);

        List<T> ParseTSeq(XmlDocument doc);
        List<T> ParseDocSum(XmlDocument doc);
    }

    public enum EUtilitiesXMLDocumentTypes
    {
        FullRecord,
        TSeq,
        DocSum
    }
}
