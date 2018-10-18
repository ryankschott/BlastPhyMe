using System;
using System.Collections.Generic;
using System.Linq;
using ChangLab.Common;
using ChangLab.Genes;

namespace ChangLab.NCBI.GenBank
{
    public class GenBankFetch : ServiceFetch<Gene>
    {
        protected internal override EUtilities.Databases Database
        {
            get { return EUtilities.Databases.NucCore; }
        }

        protected internal override EUtilities.ReturnTypes ReturnType
        {
            get { return EUtilities.ReturnTypes.GBC; }
        }

        protected internal override EUtilitiesXMLParser<Gene> XMLParser
        {
            get
            {
                GenBankXMLParser parser = new GenBankXMLParser();
                parser.AdditionalRecordNeeded += new GenBankXMLParser.AdditionalRecordNeededEventHandler(parser_SequenceNeeded);
                return parser;
            }
        }
        
        protected internal override void NormalizeResults(List<Gene> Results)
        {
            // To catch those that didn't have a parsable updated-at date
            Results.Where(g => g.LastUpdatedAt == DateTime.MinValue).ToList().ForEach(g => g.LastUpdatedAt = DateTime.Now);
        }

        private void parser_SequenceNeeded(GenBankXMLParser.AdditionalRecordNeededEventArgs e)
        {
            if (e.DocumentNeeded == EUtilitiesXMLDocumentTypes.DocSum)
            {
                // This would be an ESummary call, not EFetch
            }
            else
            {
                string retType = string.Empty;
                switch (e.DocumentNeeded)
                {
                    case EUtilitiesXMLDocumentTypes.FullRecord: retType = "gbc"; break;
                    case EUtilitiesXMLDocumentTypes.TSeq: retType = "fasta"; break;
                }

                string message = "Downloading segmented records (" + e.IDs(", ") + ")";
                string lastProgressMessage = this.LastProgressMessage;
                OnProgressUpdate(new ProgressUpdateEventArgs() { StatusMessage = (this.Batched ? message : string.Empty), ProgressMessage = (this.Batched ? string.Empty : message) });

                GenBankXMLParser parser = new GenBankXMLParser() { CompileSegments = e.CompileSegments };
                string url = EUtilities.GetUrl(EUtilities.Services.EFetch, EUtilities.Databases.NucCore, (this.Search != null), this.Search)
                    + "&rettype=" + retType
                    + "&id=" + e.IDs();
                List<Gene> results = parser.ParseFullRecord(XMLWebRequest.RequestDocument(url));
                if (results.Count != 0)
                {
                    e.Result = results;
                }
            }
        }
    }
}
