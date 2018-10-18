using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using ChangLab.Common;
using ChangLab.LocalDatabase;

namespace ChangLab.PAML.CodeML
{
    public class Tree
    {
        #region Properties
        public int ID { get; private set; }
        public string JobID { get; set; }
        public string TreeFilePath { get; set; }
        public string SequencesFilePath { get; set; }

        public string Title { get; set; }
        public int Rank { get; set; }
        public int ParentID { get; set; }
        public ReferenceItem2<Jobs.JobStatuses> Status { get; set; }
        public int SequenceCount { get; set; }
        public int SequenceLength { get; set; }
        public ControlConfiguration Configuration { get; set; }

        /// <remarks>
        /// For some reason, just returning Configuration.Document was being seen as of type ControlConfiguration, not XDocument, despite the
        /// Document property being of type XDocument. This is a work-around so that we can give the SqlParameter an XDocument type.
        /// </remarks>
        private XDocument ControlConfigurationXml 
        {
            get
            {
                return XDocument.Parse(Configuration.Document.ToString());
            }
        }

        public List<AnalysisConfiguration> AnalysisConfigurations { get; set; }
        #endregion

        public Tree() : this(new ControlConfiguration()) { }

        public Tree(ControlConfiguration Configuration)
        {
            this.ID = 0;
            this.Status = Jobs.JobStatusCollection.Get(Jobs.JobStatuses.New);
            this.AnalysisConfigurations = new List<AnalysisConfiguration>();
            this.Configuration = Configuration;
        }

        public Tree Copy()
        {
            return Copy(this);
        }

        public static Tree Copy(Tree Source, bool ClearTitle = true, bool New = false)
        {
            Tree tree = new Tree()
                {
                    ID = Source.ID,
                    JobID = Source.JobID,
                    TreeFilePath = Source.TreeFilePath,
                    SequencesFilePath = Source.SequencesFilePath,
                    Title = (ClearTitle ? string.Empty : Source.Title),
                    Rank = Source.Rank,
                    ParentID = Source.ParentID,
                    Status = Jobs.JobStatusCollection.Get(Source.Status.Key)
                };
            tree.AnalysisConfigurations.AddRange(Source.AnalysisConfigurations.Select(cf => cf.Copy()));
            tree.Configuration = Source.Configuration.Copy();
            return tree;
        }

        #region Database
        public void Save()
        {
            using (DataAccess da = new DataAccess("PAML.Tree_Edit"))
            {
                da.AddParameter("JobID", System.Data.SqlDbType.UniqueIdentifier, this.JobID);
                da.AddParameter("TreeFilePath", System.Data.SqlDbType.VarChar, 250, this.TreeFilePath);
                da.AddParameter("SequencesFilePath", System.Data.SqlDbType.VarChar, 250, this.SequencesFilePath);
                da.AddParameter("Title", SqlDbType.VarChar, 250, this.Title);
                da.AddParameter("Rank", this.Rank);
                da.AddParameter("ParentID", System.Data.SqlDbType.Int, this.ParentID, true);
                da.AddParameter("StatusID", this.Status.ID);
                da.AddParameter("SequenceCount", this.SequenceCount);
                da.AddParameter("SequenceLength", this.SequenceLength);
                da.AddParameter("ControlConfiguration", SqlDbType.Xml, this.ControlConfigurationXml, true);
                da.AddParameter("ID", System.Data.SqlDbType.Int, 0, 0, System.Data.ParameterDirection.InputOutput, true);

                this.ID = (int)da.ExecuteParameter("ID");
            }
        }

        public void UpdateStatus(Jobs.JobStatuses Status)
        {
            using (DataAccess da = new DataAccess("PAML.Tree_UpdateStatus"))
            {
                da.AddParameter("ID", this.ID);
                da.AddParameter("StatusID", Jobs.JobStatusCollection.Get(Status).ID);
                da.ExecuteCommand();

                this.Status = Jobs.JobStatusCollection.Get(Status);
            }
        }

        public static List<Tree> ListForJob(string JobID)
        {
            List<Tree> trees = new List<Tree>();

            using (DataAccess da = new DataAccess("PAML.Tree_List"))
            {
                da.AddParameter("JobID", SqlDbType.UniqueIdentifier, JobID);

                int treeId = 0;
                Tree tree = null;
                using (System.Data.SqlClient.SqlDataReader reader = da.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if ((int)reader["ID"] != treeId)
                        {
                            treeId = (int)reader["ID"];

                            tree = new Tree()
                                {
                                    ID = treeId,
                                    JobID = JobID,
                                    TreeFilePath = reader.ToSafeString("TreeFilePath"),
                                    SequencesFilePath = reader.ToSafeString("SequencesFilePath"),
                                    Title = reader.ToSafeString("Title"),
                                    Rank = reader.ToSafeInt("Rank"),
                                    ParentID = reader.ToSafeInt("ParentID"),
                                    Status = Jobs.JobStatusCollection.Get(reader.ToSafeInt("StatusID")),
                                    SequenceCount = reader.ToSafeInt("SequenceCount"),
                                    SequenceLength = reader.ToSafeInt("SequenceLength"),
                                    Configuration = new ControlConfiguration(reader.ToSafeString("ControlConfiguration"))
                                };
                            trees.Add(tree);
                        }

                        tree.AnalysisConfigurations.Add(new AnalysisConfiguration()
                            {
                                ID = (int)reader["AnalysisConfigurationID"],
                                TreeID = treeId,
                                Model = (int)reader["Model"],
                                ModelPresetID = (int)reader["ModelPresetID"],
                                NCatG = (int)reader["NCatG"],
                                KStart = reader.ToSafeDouble("KStart"),
                                KEnd = reader.ToSafeDouble("KEnd"),
                                KInterval = reader.ToSafeDouble("KInterval"),
                                FixedKappa = (bool)reader["KFixed"],
                                WStart = reader.ToSafeDouble("WStart"),
                                WEnd = reader.ToSafeDouble("WEnd"),
                                WInterval = reader.ToSafeDouble("WInterval"),
                                FixedOmega = (bool)reader["WFixed"],
                                NSSites = ((string)reader["NSSites"]).Split(new char[] { ',' }).Select(s => int.Parse(s)).ToList()
                            });
                    }
                }
            }

            return trees;
        }
        #endregion
    }
}
