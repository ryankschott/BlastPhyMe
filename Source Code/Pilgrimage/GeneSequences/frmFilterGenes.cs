using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChangLab.Genes;
using ChangLab.RecordSets;
using ChangLab.Taxonomy;

namespace Pilgrimage.GeneSequences
{
    public partial class frmFilterGenes : DialogForm
    {
        public FilterProperties Filter { get; set; }
        internal bool ShowBLASTN { get; set; }
        /// <summary>
        /// If true, will simplify the options for matching based on identical sequence to just the checkbox, without the differentiation between
        /// coding sequence and whole nucleotide sequence (typically the source sequence).
        /// </summary>
        internal bool SimpleSequenceMatching { get; set; }

        private frmFilterGenes(FilterProperties Filter)
        {
            InitializeComponent();
            
            this.Filter = Filter;
            this.ShowBLASTN = true;
            this.SimpleSequenceMatching = false;
            this.AllNodes = new List<TreeNode>();
            
            SetButtonImage(btnClear, "Filter_Clear");
            SetButtonImage(btnApply, "Filter");
            SetButtonImage(btnCancel, "Cancel");

            this.FocusOnLoad = txtDefinition;
        }

        public frmFilterGenes(FilterProperties Filter, string SubSetID) : this(Filter)
        {
            this.SubSetID = SubSetID;
            this.Taxa = Taxon.List(Program.Settings.CurrentRecordSet.ID, this.SubSetID);
        }

        private void frmFilterGenes_Load(object sender, EventArgs e)
        {
            List<string> organismNames = null;
            List<string> geneNames = null;
            ChangLab.RecordSets.SubSet.AllGeneReferenceNames(this.SubSetID, out organismNames, out geneNames);

            cmbOrganism.DataSource = new BindingSource(organismNames, null);
            cmbOrganism.Text = this.Filter.OrganismName;
            cmbOrganismLogic.DataSource = new BindingSource(this.FilterLogicDataSource, null);
            cmbOrganismLogic.SelectedValue = this.Filter.OrganismMatchLogic;

            txtDefinition.Text = this.Filter.Definition;
            cmbDefinitionLogic.DataSource = new BindingSource(this.FilterLogicDataSource, null);
            cmbDefinitionLogic.SelectedValue = this.Filter.DefinitionMatchLogic;

            cmbGeneName.DataSource = new BindingSource(geneNames, null);
            cmbGeneName.Text = this.Filter.GeneName;
            cmbGeneNameLogic.DataSource = new BindingSource(this.FilterLogicDataSource, null);
            cmbGeneNameLogic.SelectedValue = this.Filter.GeneNameMatchLogic;

            chkDuplicatesByOrganism.Checked = this.Filter.Duplicates; chkDuplicatesByOrganism_CheckedChanged(chkDuplicatesByOrganism, EventArgs.Empty); chkDuplicatesByOrganism.CheckedChanged += new EventHandler(chkDuplicatesByOrganism_CheckedChanged);
            rbDuplicates_Have.Checked = this.Filter.Duplicates_Have;
            rbDuplicates_HaveNot.Checked = !this.Filter.Duplicates_Have;

            chkSequenceMatch.Checked = this.Filter.SequenceMatch; chkSequenceMatch_CheckedChanged(chkSequenceMatch, EventArgs.Empty); chkSequenceMatch.CheckedChanged += new EventHandler(chkSequenceMatch_CheckedChanged);
            if (this.SimpleSequenceMatching)
            {
                tblSequenceMatch.Parent.Controls.Remove(tblSequenceMatch);
            }
            else
            {
                rbSequenceMatch_CDS.Checked = this.Filter.SequenceMatch_CDS;
                rbSequenceMatch_Whole.Checked = !this.Filter.SequenceMatch_CDS;
            }

            if (this.ShowBLASTN)
            {
                chkBLASTN.Checked = this.Filter.BLASTNSubmission; chkBLASTN_CheckedChanged(chkBLASTN, EventArgs.Empty); chkBLASTN.CheckedChanged += new System.EventHandler(this.chkBLASTN_CheckedChanged);
                rbBLASTN_Results.Checked = this.Filter.BLASTN_HasResults;
                rbBLASTN_NotSubmitted.Checked = !this.Filter.BLASTN_HasResults;
            }
            else
            {
                chkBLASTN.Checked = false;
                chkBLASTN.Parent.Controls.Remove(chkBLASTN);
                tblBLASTN.Parent.Controls.Remove(tblBLASTN);
            }
            
            // Set up taxonomy tree
            Taxa.Where(t => t.ParentID == 0).ToList().ForEach(t =>
                {
                    TreeNode node = new TreeNode(t.Name) { Tag = t };
                    tvTaxonomy.Nodes.Add(node);
                    AllNodes.Add(node);
                    AddTaxa(node);
                    node.Expand();
                }
            );

            if (this.SelectedTaxa.Count != 0)
            {
                AllNodes.Where(tn => this.SelectedTaxa.Any(t => t.Hierarchy == ((Taxon)tn.Tag).Hierarchy)).ToList().ForEach(tn =>
                    {
                        tn.Checked = true;
                        ExpandToNode(tn);
                    }
                );

                // Now refresh the SelectedTaxa in case the user moved or deleted some genes between filtering.
                RefreshSelectedTaxa();
            }

            TabPage defaultPage = tabControl1.TabPages.Cast<TabPage>().FirstOrDefault(pg => pg.Name == Program.Settings.GeneSequences_FilterTab);
            tabControl1.SelectedTab = (defaultPage != null ? defaultPage : pgRecordFilters);
        }

        private void frmFilterGenes_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.Settings.GeneSequences_FilterTab = tabControl1.SelectedTab.Name;
            this.Filter.SimpleSequenceMatching = this.SimpleSequenceMatching;
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            this.Filter.OrganismName = cmbOrganism.Text;
            this.Filter.OrganismMatchLogic = (FilterLogicOptions)cmbOrganismLogic.SelectedValue;
            this.Filter.Definition = txtDefinition.Text;
            this.Filter.DefinitionMatchLogic = (FilterLogicOptions)cmbDefinitionLogic.SelectedValue;
            this.Filter.GeneName = cmbGeneName.Text;
            this.Filter.GeneNameMatchLogic = (FilterLogicOptions)cmbGeneNameLogic.SelectedValue;
            this.Filter.Duplicates = chkDuplicatesByOrganism.Checked;
            this.Filter.Duplicates_Have = rbDuplicates_Have.Checked;
            this.Filter.SequenceMatch = chkSequenceMatch.Checked;
            this.Filter.SequenceMatch_CDS = rbSequenceMatch_CDS.Checked;
            this.Filter.BLASTNSubmission = chkBLASTN.Checked;
            this.Filter.BLASTN_HasResults = rbBLASTN_Results.Checked;

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        #region Taxonomy
        private List<Taxon> Taxa { get; set; }
        private List<TreeNode> AllNodes { get; set; }

        private List<Taxon> SelectedTaxa { get { return this.Filter.Taxa; } set { this.Filter.Taxa = value; } }
        private string SubSetID { get; set; }

        private void AddTaxa(TreeNode ParentNode)
        {
            Taxa.Where(t => t.ParentID == ((Taxon)ParentNode.Tag).ID).ToList().ForEach(t =>
                {
                    TreeNode node = new TreeNode(t.Name) { Tag = t };
                    ParentNode.Nodes.Add(node);
                    AllNodes.Add(node);
                    AddTaxa(node);
                    if (node.Level < 6) { node.Expand(); }
                }
            );
        }

        private void ExpandToNode(TreeNode Node)
        {
            if (Node.Parent != null && !Node.Parent.IsExpanded)
            {
                Node.Parent.Expand();
                ExpandToNode(Node.Parent);
            }
        }

        private void tvTaxonomy_AfterCheck(object sender, TreeViewEventArgs e)
        {
            tvTaxonomy.AfterCheck -= tvTaxonomy_AfterCheck;

            if (e.Node.Checked)
            {
                // If checked, check all of the nodes above.
                ToggleParent(e.Node, true);
            }
            else
            {
                // If unchecked, uncheck all of the nodes below.
                ToggleChildren(e.Node, false);
            }

            RefreshSelectedTaxa();

            tvTaxonomy.AfterCheck += tvTaxonomy_AfterCheck;
        }

        private void RefreshSelectedTaxa()
        {
            // Instead of grabbing all selected nodes, we're only including those at the deepest level in each selected branch.
            // This makes the filtering simpler because it has fewer taxa to iterate through.
            SelectedTaxa =
                AllNodes
                // This translates to: All nodes that are checked but do not have any child nodes that are checked.
                .Where(tn => tn.Checked && !tn.Nodes.Cast<TreeNode>().Any(cn => cn.Checked))
                .Select(tn => (Taxon)tn.Tag)
                .ToList();
        }

        private void ToggleParent(TreeNode Node, bool Checked)
        {
            if (Node.Parent != null)
            {
                Node.Parent.Checked = Checked;
                ToggleParent(Node.Parent, Checked);
            }
        }

        private void ToggleChildren(TreeNode Node, bool Checked)
        {
            Node.Nodes.Cast<TreeNode>().ToList().ForEach(tn =>
                {
                    tn.Checked = Checked;
                    ToggleChildren(tn, Checked);
                }
            );
        }
        #endregion

        private void chkDuplicatesByOrganism_CheckedChanged(object sender, EventArgs e)
        {
            rbDuplicates_Have.Enabled = chkDuplicatesByOrganism.Checked;
            rbDuplicates_HaveNot.Enabled = chkDuplicatesByOrganism.Checked;
        }

        private void chkSequenceMatch_CheckedChanged(object sender, EventArgs e)
        {
            rbSequenceMatch_CDS.Enabled = chkSequenceMatch.Checked;
            rbSequenceMatch_Whole.Enabled = chkSequenceMatch.Checked;
        }

        private void chkBLASTN_CheckedChanged(object sender, EventArgs e)
        {
            rbBLASTN_Results.Enabled = chkBLASTN.Checked;
            rbBLASTN_NotSubmitted.Enabled = chkBLASTN.Checked;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.Filter = new FilterProperties();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }

    public class FilterProperties
    {
        public string OrganismName { get; set; }
        public FilterLogicOptions OrganismMatchLogic { get; set; }
        public string Definition { get; set; }
        public FilterLogicOptions DefinitionMatchLogic { get; set; }
        public string GeneName { get; set; }
        public FilterLogicOptions GeneNameMatchLogic { get; set; }
        public List<Taxon> Taxa { get; set; }

        public bool Duplicates { get; set; }
        public bool Duplicates_Have { get; set; }
        public bool SequenceMatch { get; set; }
        public bool SequenceMatch_CDS { get; set; }
        public bool SimpleSequenceMatching { get; set; }
        public bool BLASTNSubmission { get; set; }
        public bool BLASTN_HasResults { get; set; }
        
        public FilterProperties()
        {
            OrganismMatchLogic = FilterLogicOptions.Contains;
            DefinitionMatchLogic = FilterLogicOptions.Contains;
            GeneNameMatchLogic = FilterLogicOptions.Contains;
            Taxa = new List<Taxon>();
            Duplicates_Have = true;
            SequenceMatch_CDS = true;
        }

        public bool ApplyFilter(ref IEnumerable<GenericGeneRowDataItem> Records)
        {
            bool filtered = false;

            if (!string.IsNullOrWhiteSpace(this.OrganismName))
            {
                Records = Records.Where(g => g.Organism.Match(this.OrganismName, this.OrganismMatchLogic));
                filtered = true;
            }
            if (!string.IsNullOrWhiteSpace(this.Definition))
            {
                Records = Records.Where(g => g.Definition.Match(this.Definition, this.DefinitionMatchLogic));
                filtered = true;
            }
            if (!string.IsNullOrWhiteSpace(this.GeneName))
            {
                Records = Records.Where(g => g.GeneName.Match(this.GeneName, this.GeneNameMatchLogic));
                filtered = true;
            }
            if (this.Taxa.Count != 0)
            {
                Records = Records.Where(g => this.Taxa.Any(t => g.TaxonomyHierarchy.StartsWith(t.Hierarchy)));
                filtered = true;
            }
            if (this.BLASTNSubmission)
            {
                Records = Records.Where(g => g.HasBlastNResults == this.BLASTN_HasResults);
                filtered = true;
            }
            // These ones should be done last because they use a group by.
            if (this.Duplicates)
            {
                List<string> duplicateOrganismNames = Records.GroupBy(g => g.Organism).Where(gb => gb.Count() > 1).Select(gb => gb.Key).Distinct().ToList();
                if (this.Duplicates_Have)
                {
                    Records = Records.Where(g => duplicateOrganismNames.Contains(g.Organism));
                }
                else
                {
                    Records = Records.Where(g => !duplicateOrganismNames.Contains(g.Organism));
                }
                filtered = true;
            }
            if (this.SequenceMatch)
            {
                MatchingSequencesEventArgs matchArgs = new MatchingSequencesEventArgs(this.SimpleSequenceMatching);
                OnMatchingSequences(matchArgs);
                
                if (this.SequenceMatch_CDS || this.SimpleSequenceMatching)
                {
                    List<KeyValuePair<string, int>> duplicateRecordsBySequenceMatch = 
                        Records
                        .GroupBy(g =>
                            {
                                switch (matchArgs.MatchWith)
                                {
                                    case MatchingSequencesEventArgs.MatchingSequencesTypes.Nucleotides: return g.Gene.Nucleotides;
                                    case MatchingSequencesEventArgs.MatchingSequencesTypes.SourceSequence: return g.Gene.SourceSequence.ToString();
                                    default: throw new NotImplementedException("You've created another possible way to match sequences without implementing it fully.");
                                }
                            })
                        .Where(gb => gb.Count() > 1)
                        .Select((gb, index) => new { Group = gb, Index = (index + 1) })
                        .Aggregate(new List<KeyValuePair<string, int>>(), 
                                    (current, gb) => { current.AddRange(gb.Group.Select(g => new KeyValuePair<string, int>(g.ID, gb.Index))); return current; });

                    // Now narrow down the Records to just those IDs we selected via the GroupBy (having Count() > 1)
                    Records =
                        Records
                        .Join(duplicateRecordsBySequenceMatch,
                                g => g.ID,
                                d => d.Key,
                                (g, d) => { g.DuplicateSequenceKey = d.Value; return g; });
                }
                else
                {
                    // If the user has filtered by the whole gene sequence we have to go out to the database to filter because we don't have the
                    // whole sequence in memory unless a record has been opened into detailed view.
                    List<KeyValuePair<string, int>> filteredGeneIds = ChangLab.Genes.Gene.FilterAtDatabase(Records.Select(g => g.ID).ToList(), false, true);
                    Records = Records.Join(filteredGeneIds, g => g.ID, fg => fg.Key, (g, fg) => { g.DuplicateSequenceKey = fg.Value; return g; });
                }
                filtered = true;
            }

            return filtered;
        }

        protected virtual void OnMatchingSequences(MatchingSequencesEventArgs e)
        {
            if (MatchingSequences != null)
            {
                MatchingSequences(e);
            }
        }
        public delegate void MatchingSequencesEventHandler(MatchingSequencesEventArgs e);
        public event MatchingSequencesEventHandler MatchingSequences;

        public class MatchingSequencesEventArgs : EventArgs
        {
            public MatchingSequencesTypes MatchWith { get; set; }
            public bool SimpleSequenceMatching { get; set; }

            public MatchingSequencesEventArgs(bool SimpleSequenceMatching)
            {
                this.MatchWith = MatchingSequencesTypes.Nucleotides;
                this.SimpleSequenceMatching = SimpleSequenceMatching;
            }

            public enum MatchingSequencesTypes
            {
                Nucleotides,
                SourceSequence
            }
        }

        //// I originally set this up to provide a way to check to see if any filter properties had been set by way of comparing an existing instance
        //// against a new instance, thus avoiding the need for "_set" variables for all of the properties, but then realized I could just use the
        //// code in uctRecordSetGenes that applies filters to indicate that filters were set.  However, this code has been preserved as an example
        //// of a custom Equals() override.  GetHashCode() might need help in this case (not sure).
        //public override bool Equals(object obj)
        //{
        //    if (obj.GetType() == typeof(FilterProperties))
        //    {
        //        FilterProperties test = (FilterProperties)obj;

        //        return (test.OrganismName == this.OrganismName
        //                && test.OrganismMatchLogic == this.OrganismMatchLogic
        //                && test.Definition == this.Definition
        //                && test.DefinitionMatchLogic == this.DefinitionMatchLogic
        //                && test.Taxa.SequenceEqual(this.Taxa, new TaxonComparer())

        //                && test.Duplicates == this.Duplicates
        //                && test.Duplicates_Have == this.Duplicates_Have
        //                && test.BLASTNSubmission == this.BLASTNSubmission
        //                && test.BLASTN_HasResults == this.BLASTN_HasResults);
        //    }
        //    else { return base.Equals(obj); }
        //}

        //public override int GetHashCode()
        //{
        //    return base.GetHashCode();
        //}
    }
}