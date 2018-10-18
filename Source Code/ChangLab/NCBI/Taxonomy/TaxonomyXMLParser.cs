using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using ChangLab.Common;
using ChangLab.Taxonomy;

namespace ChangLab.NCBI.Taxonomy
{
    public class TaxonomyXMLParser : EUtilitiesXMLParser<Taxon>
    {
        public List<Taxon> ParseFullRecord(XmlDocument doc)
        {
            return ParseTaxaSet(doc);
        }

        public List<Taxon> ParseTaxaSet(XmlDocument doc)
        {
            try
            {
                List<Taxon> records = new List<Taxon>();

                foreach (XmlNode taxonDoc in doc.SelectNodes("./TaxaSet/Taxon"))
                {
                    Taxon taxon = new Taxon()
                    {
                        TaxonomyDatabaseID = taxonDoc.SelectSingleNode("TaxId").SafeInnerText().ToSafeInt(),
                        Name = taxonDoc.SelectSingleNode("ScientificName").SafeInnerText(),
                        Rank = taxonDoc.SelectSingleNode("Rank").SafeInnerText(),
                        Division = taxonDoc.SelectSingleNode("Division").SafeInnerText(),
                        Lineage = taxonDoc.SelectSingleNode("Lineage").SafeInnerText(),
                        LineageList = new List<Taxon>()
                    };

                    XmlNode test = taxonDoc.SelectSingleNode("OtherNames");
                    if (test != null)
                    {
                        foreach (string nodeName in new string[] { "GenbankCommonName", "EquivalentName", "BlastName", "Name/DispName" })
                        {
                            taxon.OtherName = test.SelectSingleNode(nodeName).SafeInnerText();
                            if (!string.IsNullOrWhiteSpace(taxon.OtherName))
                            {
                                if (nodeName == "Name/DispName") { taxon.OtherName = "Authority: " + taxon.OtherName; }
                                break;
                            }
                        }
                    }

                    test = taxonDoc.SelectSingleNode("LineageEx");
                    if (test != null)
                    {
                        taxon.LineageList.AddRange(
                            test.SelectNodes("Taxon").Cast<XmlNode>().Select(lineageNode =>
                                new Taxon()
                                {
                                    TaxonomyDatabaseID = lineageNode.SelectSingleNode("TaxId").SafeInnerText().ToSafeInt(),
                                    Name = lineageNode.SelectSingleNode("ScientificName").SafeInnerText(),
                                    Rank = lineageNode.SelectSingleNode("Rank").SafeInnerText()
                                }
                            ));
                    }

                    records.Add(taxon);
                }

                return records;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Taxon> ParseDocSum(XmlDocument doc)
        {
            try
            {
                List<Taxon> records = new List<Taxon>();

                foreach (XmlNode seqDoc in doc.SelectNodes("./eSummaryResult/DocSum"))
                {
                    records.Add(new Taxon()
                    {
                        Rank = seqDoc.SelectSingleNode("Item[@Name=\"Rank\"]").SafeInnerText(),
                        Division = seqDoc.SelectSingleNode("Item[@Name=\"Division\"]").SafeInnerText(),
                        Name = seqDoc.SelectSingleNode("Item[@Name=\"ScientificName\"]").SafeInnerText(),
                        OtherName = seqDoc.SelectSingleNode("Item[@Name=\"CommonName\"]").SafeInnerText(),
                        TaxonomyDatabaseID = seqDoc.SelectSingleNode("Item[@Name=\"TaxId\"]").SafeInnerText().ToSafeInt()
                    });
                }

                return records;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Taxon> ParseTSeq(XmlDocument doc)
        {
            throw new NotImplementedException();
        }
    }
}
