using System;
using System.Collections.Generic;
using System.Linq;
using ChangLab.Common;
using ChangLab.Taxonomy;

namespace ChangLab.NCBI.Taxonomy
{
    public class TaxonomyFetch : ServiceFetch<Taxon>
    {
        protected internal override EUtilities.Databases Database
        {
            get { return EUtilities.Databases.Taxonomy; }
        }

        protected internal override EUtilities.ReturnTypes ReturnType
        {
            get { return EUtilities.ReturnTypes.NotSpecified; }
        }

        protected internal override EUtilitiesXMLParser<Taxon> XMLParser
        {
            get { return new TaxonomyXMLParser(); }
        }
    }
}
