using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using ChangLab.Common;
using ChangLab.Taxonomy;

namespace ChangLab.NCBI.Taxonomy
{
    public class TaxonomyServiceSearch : ServiceSearch<Taxon>
    {
        public override EUtilities.Databases Database
        {
            get { return EUtilities.Databases.Taxonomy; }
        }

        public override EUtilitiesXMLParser<Taxon> XMLParser
        {
            get { return new TaxonomyXMLParser(); }
        }
    }
}
