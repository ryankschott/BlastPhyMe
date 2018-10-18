using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChangLab.Genes;

namespace Pilgrimage.GeneSequences.EditNucleotideSequence
{
    public partial class frmNucleotideSequenceFeatures : DialogForm
    {
        internal Gene Gene { get; set; }
        private bool Editable { get; set; }

        internal List<Feature> Features { get; private set; }
        internal NucleotideSequence SourceSequence
        {
            get
            {
                int start = int.Parse(txtStartIndex.Text); if (start == 0) { start = 1; }
                return new NucleotideSequence(rtfNucleotideSequence.Text, start);
            }
        }

        private Feature SelectedFeature
        {
            get
            {
                return this.Features.FirstOrDefault(f => f.FeatureKey.Key == ((FeatureRowDataItem)grdFeature.SelectedRows[0].DataBoundItem).Key);
            }
        }
        private DataGridViewHelper IntervalsDataGridHelper { get; set; }

        public frmNucleotideSequenceFeatures(bool Editable = false)
        {
            InitializeComponent();
            
            this.IntervalsDataGridHelper = new DataGridViewHelper(this, grdFeatureInterval, DataGridViewHelper.DataSourceTypes.Other);
            this.IntervalsDataGridHelper.ViewDetails += new DataGridViewHelper.ViewDetailsEventHandler(IntervalsDataGridHelper_ViewDetails);

            SetButtonImage(btnClose, DialogButtonPresets.Close);
            SetButtonImage(btnSave, DialogButtonPresets.Save);

            SetButtonImage(btnAddFeature, DialogButtonPresets.Add);
            SetButtonImage(btnRemoveFeature, DialogButtonPresets.Delete);
            SetButtonImage(btnAddInterval, DialogButtonPresets.Add);
            SetButtonImage(btnEditInterval, DialogButtonPresets.Edit);
            SetButtonImage(btnAddIntervalFromSelection, DialogButtonPresets.Add);
            SetButtonImage(btnRemoveInterval, DialogButtonPresets.Delete);

            ToggleForm(Editable);
        }

        public frmNucleotideSequenceFeatures(Gene Gene, bool Editable) : this(Editable)
        {
            this.Gene = Gene.Copy(); // This way we won't be touching the Feature objects in the original Gene passed up from frmGeneDetails
        }

        private void frmNucleotideSequenceFeatures_Load(object sender, EventArgs e)
        {
            rtfNucleotideSequence.Text = this.Gene.SourceSequence.ToString();
            txtStartIndex.Text = this.Gene.SourceSequence.Start.ToString();

            rtfNucleotideSequence.ReadOnly = (this.Gene.Features.Count != 0);
            lnkEditSourceSequence.Visible = this.Editable && (this.Gene.Features.Count != 0);

            // Aggregate the sequence annotation data from the gene sequence record as follows:
            //
            //  For each type of feature (Gene.FeatureKey) create a list of the intervals that correspond to that FeatureKey
            //
            // Some sequences are annotated such that the same feature key exists across multiple features, so instead of listing the features and 
            // their keys, we're listing the keys and then aggregating their intervals across all features, because that's a more logical way for the
            // user to look at it.
            // If a user edits and then saves changes to the gene sequence, we'll have restructured the features and intervals in the database such
            // that there's only one Feature for each type (FeatureKey), and one or more Intervals per Feature.

            // Load() could be run more than once if we're using an instance of this form as a container for a Gene instance's edited Features
            // (see frmGeneDetails.lnkEditNucleotideSequence_LinkClicked() for more info.
            if (this.Features == null)
            {
                this.Features = this.Gene.Features
                    .OrderBy(f => f.Rank)
                    .GroupBy(f => f.FeatureKey.ID)
                    .Aggregate(new List<Feature>(), (current, fgroup) =>
                                {
                                    Feature feature = new Feature() { FeatureKey = GeneFeatureKeyCollection.Get(fgroup.Key) };
                                    feature.Intervals.AddRange(fgroup.Aggregate(new List<FeatureInterval>(), (list_intervals, f) => { list_intervals.AddRange(f.Intervals); return list_intervals; }));
                                    current.Add(feature);
                                    return current;
                                });
            }
            RefreshFeaturesGrid();
        }

        internal void ToggleForm(bool Editable)
        {
            this.Editable = Editable;

            txtStartIndex.ReadOnly = !Editable;
            btnSave.Visible = Editable;
            btnAddFeature.Visible = Editable;
            btnRemoveFeature.Visible = Editable;
            btnAddInterval.Visible = Editable;
            btnAddIntervalFromSelection.Visible = Editable;
            btnEditInterval.Visible = Editable;
            btnRemoveInterval.Visible = Editable;
            btnClose.Text = (Editable ? "&Cancel" : "&Close");
        }

        private void RefreshFeaturesGrid(GeneFeatureKeys SelectedFeature = GeneFeatureKeys.Undefined)
        {
            grdFeature.AutoGenerateColumns = false;
            grdFeature.DataSource = null;
            grdFeature.DataSource = new SortableBindingList<FeatureRowDataItem>(this.Features.Select(f => new FeatureRowDataItem(f.FeatureKey.ID, f.Intervals)).OrderByDescending(fr => fr.Rank));

            if (SelectedFeature != GeneFeatureKeys.Undefined)
            {
                DataGridViewRow selectedRow = grdFeature.Rows.Cast<DataGridViewRow>().FirstOrDefault(r => ((FeatureRowDataItem)r.DataBoundItem).Key == SelectedFeature);
                if (selectedRow != null) { selectedRow.Selected = true; }
            }

            // Okay, so this might seem a little sketchy, but a feature can't have a key that isn't in the GeneFeatureKeys enum because 
            // ChangLab.NCBI.GenBank.GenBankXMLParser will only add features to a gene if their feature key is in the enum.
            btnAddFeature.Enabled = (grdFeature.Rows.Count != GeneFeatureKeyCollection.Count);
            btnRemoveFeature.Enabled = (grdFeature.Rows.Count != 0);
            btnAddInterval.Enabled = (grdFeature.Rows.Count != 0);
            if (grdFeature.Rows.Count == 0)
            {
                // No rows means no selected rows means no refreshing of the intervals grid and no disabling of these buttons.
                btnEditInterval.Enabled = false;
                btnRemoveInterval.Enabled = false;
            }
        }

        private void grdFeature_SelectionChanged(object sender, EventArgs e)
        {
            RefreshFeatureIntervalsGrid();
        }

        private void RefreshFeatureIntervalsGrid(FeatureInterval SelectedInterval = null)
        {
            grdFeatureInterval.AutoGenerateColumns = false;
            grdFeatureInterval.DataSource = null;

            if (grdFeature.SelectedRows.Count != 0)
            {
                grdFeatureInterval.DataSource = new SortableBindingList<FeatureIntervalRowDataItem>(((FeatureRowDataItem)grdFeature.SelectedRows[0].DataBoundItem).Intervals.Select(i => new FeatureIntervalRowDataItem(i, this.Gene.SourceSequence)));

                if (SelectedInterval != null)
                {
                    DataGridViewRow selectedRow = grdFeatureInterval.Rows.Cast<DataGridViewRow>().FirstOrDefault(r => r.DataBoundItem == SelectedInterval);
                    if (selectedRow != null) { selectedRow.Selected = true; }
                }

                btnEditInterval.Enabled = (grdFeatureInterval.Rows.Count != 0);
                btnRemoveInterval.Enabled = (grdFeatureInterval.Rows.Count != 0);
            }
        }

        private void grdFeatureInterval_SelectionChanged(object sender, EventArgs e)
        {
            // Clear the selection and highlighting
            rtfNucleotideSequence.SelectAll();
            rtfNucleotideSequence.SelectionBackColor = rtfNucleotideSequence.BackColor;
            rtfNucleotideSequence.Select(0, 0);

            if (grdFeatureInterval.SelectedRows.Count != 0)
            {
                // Highlight the intervals in the source sequence
                var qry = grdFeatureInterval.SelectedRows.Cast<DataGridViewRow>().Select(row => ((FeatureIntervalRowDataItem)row.DataBoundItem).Interval).OrderBy(f => f.Start);
                foreach (FeatureInterval interval in qry)
                {
                    rtfNucleotideSequence.Select(interval.Start - this.Gene.SourceSequence.Start, interval.Length);
                    rtfNucleotideSequence.SelectionBackColor = Color.PaleGoldenrod;
                }

                rtfNucleotideSequence.Select(qry.First().Start - this.Gene.SourceSequence.Start, qry.First().Length);
                rtfNucleotideSequence.ScrollToCaret();
            }
        }

        private void lnkEditSourceSequence_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (Utility.ShowMessage(this, Properties.Resources.Messages_NucleotideSequenceFeatures_EditSourceSequenceWarning, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.OK)
            {
                rtfNucleotideSequence.ReadOnly = false;
            }
        }

        #region Edit Features and Intervals
        private void btnAddFeature_Click(object sender, EventArgs e)
        {
            using (frmAddFeature frm = new frmAddFeature(this.Features))
            {
                if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    this.Features.Add(new Feature() { FeatureKey = frm.SelectedKey });
                    RefreshFeaturesGrid(frm.SelectedKey.Key);
                }
            }
        }

        private void btnRemoveFeature_Click(object sender, EventArgs e)
        {
            if (grdFeature.SelectedRows.Count == 0) { Utility.ShowMessage(this, "No feature has been selected."); }
            else if (Utility.ShowMessage(this, "Removing this feature will remove all intervals associated with it.\r\n\r\nAre you sure you want to remove this feature?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                this.Features.Remove(this.Features.First(f => f.FeatureKey.Key == ((FeatureRowDataItem)grdFeature.SelectedRows[0].DataBoundItem).Key));
                RefreshFeaturesGrid();
            }
        }

        private void btnAddInterval_Click(object sender, EventArgs e)
        {
            using (frmEditInterval frm = new frmEditInterval(this.SourceSequence, null, true, this.Editable))
            {
                if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    this.SelectedFeature.Intervals.Add(frm.Interval);
                    RefreshFeatureIntervalsGrid(frm.Interval);
                }
            }
        }

        private void btnAddIntervalFromSelection_Click(object sender, EventArgs e)
        {
            if (grdFeature.Rows.Count == 0) { Utility.ShowMessage(this, "A Feature Type must be added and selected before an interval can be defined."); }
            else if (grdFeature.SelectedRows.Count == 0) { Utility.ShowMessage(this, "A Feature Type must be selected before an interval can be defined."); }
            else if (rtfNucleotideSequence.SelectionLength == 0) { Utility.ShowMessage(this, "No nucleotides have been selected."); }
            else
            {
                FeatureInterval interval = new FeatureInterval()
                {
                    Start = (rtfNucleotideSequence.SelectionStart + this.SourceSequence.Start),
                    End = (rtfNucleotideSequence.SelectionStart + (rtfNucleotideSequence.SelectionLength - 1)+ this.SourceSequence.Start),
                    IsComplement = false
                };

                using (frmEditInterval frm = new frmEditInterval(this.SourceSequence, interval, true, this.Editable))
                {
                    if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        this.SelectedFeature.Intervals.Add(frm.Interval);
                        RefreshFeatureIntervalsGrid(frm.Interval);
                    }
                }
            }
        }

        private void btnEditInterval_Click(object sender, EventArgs e)
        {
            if (grdFeatureInterval.SelectedRows.Count == 0) { Utility.ShowMessage(this, "No interval has been selected."); return;  }
            if (grdFeatureInterval.SelectedRows.Count > 1) { if (Utility.ShowMessage(this, "The first of the selected intervals will be edited.", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Cancel) { return; } }

            FeatureInterval selectedInterval = ((FeatureIntervalRowDataItem)grdFeatureInterval.SelectedRows[0].DataBoundItem).Interval;

            using (frmEditInterval frm = new frmEditInterval(this.SourceSequence, selectedInterval, false, this.Editable))
            {
                frm.ShowDialog();
                // We're just passing around the same FeatureInterval object instances, between this.Features and the grdFeatureInterval data source.
                // When we pass an existing instance up to the form, it gets set as the form's Interval property, which is then directly edited by
                // changing the interval properties using the form controls.  That's all affecting the same instance down here in this form, thus
                // there's no code here after frm.ShowDialog() to set the edited properties - they've already been set.
                // This makes it up to frmEditInterval to reset the FeatureInterval property values if the user cancels.
                RefreshFeatureIntervalsGrid(selectedInterval);
            }
        }

        private void IntervalsDataGridHelper_ViewDetails(DataGridViewHelper.ViewDetailsEventArgs e)
        {
            // Calling the event directly instead of using btnEditInterval.PerformClick() so that this will still work if the button is not visible.
            btnEditInterval_Click(btnEditInterval, EventArgs.Empty);
        }

        private void btnRemoveInterval_Click(object sender, EventArgs e)
        {
            if (grdFeatureInterval.SelectedRows.Count == 0) { Utility.ShowMessage(this, "No interval has been selected."); }
            else if (Utility.ShowMessage(this, "Are you sure you want to remove this interval?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                this.SelectedFeature.Intervals.Remove(((FeatureIntervalRowDataItem)grdFeatureInterval.SelectedRows[0].DataBoundItem).Interval);
                RefreshFeatureIntervalsGrid();
            }
        }
        #endregion
    }

    public class FeatureRowDataItem : RowDataItem
    {
        public override string ID { get { throw new NotImplementedException(); } }

        private ChangLab.Common.ReferenceItem2<GeneFeatureKeys> FeatureKey { get; set; }
        public int Rank { get { return FeatureKey.Rank; } }
        public string Name { get { return FeatureKey.Name; } }
        public GeneFeatureKeys Key { get { return FeatureKey.Key; } }

        public List<FeatureInterval> Intervals { get; private set; }

        public FeatureRowDataItem(int FeatureKeyID, List<FeatureInterval> Intervals)
        {
            this.FeatureKey = GeneFeatureKeyCollection.Get(FeatureKeyID);
            this.Intervals = Intervals;
        }
        
        //public int DatabaseID { get { return this.Feature.ID; } }
        //public int Rank { get { return this.Feature.Rank; } }
        //public string FeatureKeyName { get; private set; }

        //private Feature _feature;
        //public Feature Feature
        //{
        //    get { return _feature; }
        //    set
        //    {
        //        _feature = value;
        //        this.FeatureKeyName = GeneFeatureKey.NameByID(_feature.FeatureKeyID);
        //    }
        //}

        //public FeatureRowDataItem(Feature Feature)
        //{
        //    this.Feature = Feature;
        //}
    }

    public class FeatureIntervalRowDataItem : RowDataItem
    {
        public override string ID { get { throw new NotImplementedException(); } }

        public FeatureInterval Interval { get; private set; }

        public int Start { get { return this.Interval.Start; } }
        public int End { get { return this.Interval.End; } }
        public bool IsComplement { get { return this.Interval.IsComplement; } }
        public string Sequence { get; private set; }

        public FeatureIntervalRowDataItem(FeatureInterval FeatureInterval, NucleotideSequence SourceSequence)
        {
            this.Interval = FeatureInterval;
            this.Sequence = SourceSequence.SubSequence(this.Start, this.End, this.IsComplement);
        }
    }
}
