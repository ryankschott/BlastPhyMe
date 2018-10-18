using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using ChangLab.Common;
using ChangLab.Genes;

namespace ChangLab.Jobs
{
    public class GeneProcessingJob : Job
    {
        public List<Gene> InputGenes { get; private set; }
        public List<Gene> OutputGenes { get; private set; }

        public GeneProcessingJob() : this(JobTargets.Undefined, string.Empty) { }

        public GeneProcessingJob(JobTargets Target, string SubSetID) : base (Target, SubSetID)
        {
            this.InputGenes = new List<Gene>();
            this.OutputGenes = new List<Gene>();
        }

        public GeneProcessingJob(string JobID) : base(JobID) { }

        #region Database Methods
        /// <param name="AddToCollection">If you already know that it's in the collection, this can be set false.</param>
        public void AddGene(Gene Gene, GeneDirections DirectionKey, bool AddToCollection = true)
        {
            using (LocalDatabase.DataAccess da = new LocalDatabase.DataAccess("Job.Gene_Edit"))
            {
                da.AddParameter("JobID", SqlDbType.UniqueIdentifier, this.ID);
                da.AddParameter("GeneID", SqlDbType.UniqueIdentifier, Gene.ID);
                da.AddParameter("DirectionID", GeneDirection.IDByKey(DirectionKey));

                da.ExecuteCommand();

                if (AddToCollection)
                {
                    switch (DirectionKey)
                    {
                        case GeneDirections.Input:
                            if (!this.InputGenes.Any(g => GuidCompare.Equals(g.ID, Gene.ID)))
                            {
                                this.InputGenes.Add(Gene);
                            }
                            break;
                        case GeneDirections.Output:
                            if (!this.OutputGenes.Any(g => GuidCompare.Equals(g.ID, Gene.ID)))
                            {
                                this.OutputGenes.Add(Gene);
                            }
                            break;
                    }
                }
            }
        }

        public static DataTable ListGenesFromDatabase(string JobID, GeneDirections Direction)
        {
            using (LocalDatabase.DataAccess da = new LocalDatabase.DataAccess("Job.Gene_List"))
            {
                da.AddParameter("JobID", SqlDbType.UniqueIdentifier, JobID);
                da.AddParameter("GeneDirectionID", GeneDirection.IDByKey(Direction));

                return da.ExecuteDataTable();
                //List<Gene> genes = new List<Gene>();
                //using (DataTable results = da.ExecuteDataTable())
                //{
                //    foreach (DataRow row in results.Rows)
                //    {
                //        Gene gene = Gene.FromDatabaseRow(row);
                //        genes.Add(gene);

                //        if (IncludeSequenceData) { gene.GetSequenceData(); }
                //    }
                //}

                //switch (Direction)
                //{
                //    case GeneDirections.Input:
                //        this.InputGenes = genes;
                //        break;
                //    case GeneDirections.Output:
                //        this.OutputGenes = genes;
                //        break;
                //}
            }
        }
        #endregion
    }

    public class CommandLineGeneProcessingJob : GeneProcessingJob
    {
        internal string CommandLinePath { get; set; }
        public bool KeepOutputFiles { get; set; }
        internal Process CommandLineProcess { get; set; }
        
        public CommandLineGeneProcessingJob(JobTargets Target, string SubSetID, string CommandLinePath, bool KeepOutputFiles) : base(Target, SubSetID)
        {
            this.CommandLinePath = CommandLinePath;
            this.KeepOutputFiles = KeepOutputFiles;
        }

        public CommandLineGeneProcessingJob(string JobID) : base(JobID) { }

        public override void CancelAsync()
        {
            base.CancelAsync();
            CommandLineProcess.CancelOutputRead();
            CommandLineProcess.Kill();
        }
    }

    public class JobOptions : XDocument
    {
        protected XElement _optionsRoot { get { return this.Root; } } // In case I ever want to move the root node.

        public virtual string OptionSwitches { get { throw new NotImplementedException(); } }

        public JobOptions()
        {
            this.Add(new XElement("Options"));
        }

        public JobOptions(string XML)
        {
            if (!string.IsNullOrWhiteSpace(XML))
            {
                try
                {
                    XDocument doc = XDocument.Parse(XML);

                    if (doc.Root.Name == "Options")
                    {
                        this.Add(doc.Root);
                    }
                    else if (doc.Root.Name == "Additional")
                    {
                        XElement optionsRoot = doc.Root.Elements("Property").FirstOrDefault(el => el.Element("Name").Value == "Options");
                        if (optionsRoot != null)
                        {
                            this.Add(XDocument.Parse(optionsRoot.Element("Value").Value).Root);
                        }
                    }
                }
                catch { }
                finally
                {
                    if (this.Root == null) { this.Add(new XElement("Options")); }
                }
            }
            else
            { this.Add(new XElement("Options")); }
        }
    }
}
