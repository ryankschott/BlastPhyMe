using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChangLab.BlastN;
using ChangLab.Common;
using ChangLab.Genes;

namespace ChangLab.NCBI.LocalDatabase
{
    public class Query
    {
        public Gene InputGene { get; internal set; }
        public List<LocalAlignment> LocalAlignments { get; internal set; }
        public Exception ParsingException { get; internal set; }

        public Query()
        {
            this.LocalAlignments = new List<LocalAlignment>();
        }
    }

    public class LocalAlignment
    {
        public Gene OutputGene { get; internal set; }
        public Alignment Alignment { get; internal set; }
        public int Rank { get; internal set; }
        public List<AlignmentExon> Exons { get; internal set; }

        public List<Exception> Exceptions { get; internal set; }

        public LocalAlignment()
        {
            this.Exons = new List<AlignmentExon>();
            this.Exceptions = new List<Exception>();
        }
    }
}
