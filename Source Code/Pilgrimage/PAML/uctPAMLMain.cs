using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChangLab.Common;
using ChangLab.Jobs;
using ChangLab.PAML.CodeML;
using ChangLab.RecordSets;

namespace Pilgrimage.PAML
{
    public partial class uctPAMLMain : Pilgrimage.UserControls.uctSubSetsManager
    {
        protected internal override DataTypes DataType { get { return DataTypes.CodeMLResult; } }
        protected internal override DraggableTabControl.DraggableTabControl SubSetViewsControl { get { return tbSubSets; } }
        protected internal override ToolStripContainer FormToolStripContainer { get { return tscForm; } }
        
        public uctPAMLMain()
        {
            InitializeComponent();
            this.tsbExportToPilgrimage.Text = Program.ProductName + " Data File";
            this.tsbImportFromPilgrimageDataFile.Text = Program.ProductName + " Data File";

            tbSubSets.Reordered += new TabControlEventHandler(SubSetsViewControl_Reordered);
            tsbNewSubSet.Click += new EventHandler(NewSubset_Click);
            tsbOpenSubSet.Click += new EventHandler(OpenSubSet_Click);
            tsbCloseSubSet.Click += new EventHandler(CloseSubSet_Click);
            tsbImportFromPilgrimageDataFile.Click += new System.EventHandler(ImportFromPilgrimageDataFile_Click);

            tsSubSetActions.EndDrag += new System.EventHandler(ToolStrip_EndDrag);
            tsForm.EndDrag += new System.EventHandler(ToolStrip_EndDrag);
            tsPAML.EndDrag += new System.EventHandler(ToolStrip_EndDrag);

            tsActions = (new ToolStripItem[] { tsbMoveResults, tsbCopyResults, tsbDelete, tsbExportTo }).ToList();

#if EEB460
            sepExport.Visible = false;
            tsbExportToPilgrimage.Visible = false;
            tsbImportFrom.Visible = false;
#endif
        }

        private void tbSubSets_Selected(object sender, TabControlEventArgs e)
        {
            if (LoadingSubSetPages || tbSubSets.IsDragging) { return; }

            CurrentSubSet = (e.TabPage == null ? null : (SubSet)e.TabPage.Tag);
            tsActions.ForEach(tsb => tsb.Enabled = (CurrentSubSet != null));
            tsbCloseSubSet.Enabled = (CurrentSubSet != null);
            
            if (CurrentSubSet != null)
            {
                if (!SelectedSubSetView.Loaded) { SelectedSubSetView.RefreshRecords(); } // Load on demand
                CurrentSubSet.Opened();
            }
        }

        protected internal override void AddSubSet(SubSet Set, bool UpdateOpened = false)
        {
            TabPage pg = new TabPage(Set.Name) { Tag = Set };

            uctResults rsControl = new uctResults() { Dock = DockStyle.Fill, Manager = this };
            pg.Controls.Add(rsControl);
            tbSubSets.TabPages.Add(pg);

            rsControl.Initialize(Set);
        }

        #region Toolbar PAML
        private void tsbNewPAMLJob_Click(object sender, EventArgs e)
        {
            using (frmCreateJob frm = new frmCreateJob())
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    // Pop the job progress form with the newly created analsis wrapper.
                    using (frmJobProgress job = new frmJobProgress(frm.Analysis))
                    {
                        if (job.ShowDialog() == DialogResult.OK)
                        {
                            using (frmResults results = new frmResults(job.Analysis.CurrentJob.ID))
                            {
                                results.ShowDialog();
                                if (results.EditedSubSetIDs.Count != 0)
                                { results.EditedSubSetIDs.Distinct().ToList().ForEach(sub => this.ShowAndRefreshSubSet(sub, null)); }
                            }
                        }
                    }
                }
            }
        }

        internal void ViewJobHistory(Job SelectedJob = null)
        {
            using (frmJobHistory frm = new frmJobHistory() { SelectedJob = SelectedJob })
            {
                frm.ShowDialog();

                if (frm.EditedSubSetIDs.Count != 0)
                { frm.EditedSubSetIDs.Distinct().ToList().ForEach(sub => this.ShowAndRefreshSubSet(sub, null)); }

                if (frm.RunningAnalysis != null)
                {
                    // A job was re-run from the history screen.
                    // Pop the job progress form with the newly created analsis wrapper.
                    using (frmJobProgress job = new frmJobProgress(frm.RunningAnalysis))
                    {
                        if (job.ShowDialog() == DialogResult.OK)
                        {
                            using (frmResults results = new frmResults(job.Analysis.CurrentJob.ID))
                            {
                                results.ShowDialog();
                                if (results.EditedSubSetIDs.Count != 0)
                                { results.EditedSubSetIDs.Distinct().ToList().ForEach(sub => this.ShowAndRefreshSubSet(sub, null)); }
                            }
                        }
                    }
                }
            }
        }

        private void tsbJobHistory_Click(object sender, EventArgs e)
        {
            ViewJobHistory();
        }

        #endregion

        #region Toolbar Results
        public bool GetSelectedResults(out List<ResultSummaryRow> SelectedRows, bool ShowNoResultsMessage = true, bool PromptForAllIfNone = true)
        {
            SelectedRows = null;

            if (SelectedSubSetView.Refreshing) { return false; }
            SelectedRows = this.SelectedSubSetView.SelectedRows.Cast<ResultSummaryRow>().ToList();

            if (SelectedRows == null || SelectedRows.Count == 0)
            {
                if (PromptForAllIfNone)
                {
                    if (Utility.ShowMessage(this, Properties.Resources.Messages_NoResultsSelectAll, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                    {
                        SelectedRows = this.SelectedSubSetView.AllRows.Cast<ResultSummaryRow>().ToList();
                        return true;
                    }
                    else { return false; }
                }
                else
                {
                    Utility.ShowMessage(this, Properties.Resources.Messages_NoResultsSelected);
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        private void tsbMoveOrCopyResults_Click(object sender, EventArgs e)
        {
            List<ResultSummaryRow> selectedResults = null;
            if (GetSelectedResults(out selectedResults, true, false))
            {
                bool move = sender == tsbMoveResults;

                using (RecordSets.frmSelectSubSet frm = new RecordSets.frmSelectSubSet(this.DataType, this.CurrentSubSet, move))
                {
                    if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        SelectedSubSetView.DataGridHelper.ShortTermDisableKeyPress();

                        SubSet selectedSubSet = null;
                        if (string.IsNullOrEmpty(frm.SelectedRecordSetID))
                        {
                            bool newSubSet = (!AllSubSets.Any(sub => GuidCompare.Equals(sub.ID, frm.SelectedSubSetID)));
                            if (newSubSet)
                            {
                                RefreshSubSetsFromDatabase();
                            }

                            selectedSubSet = AllSubSets.First(sub => GuidCompare.Equals(sub.ID, frm.SelectedSubSetID));

                            Result.AddToSubSet(selectedSubSet.ID, selectedResults.Select(result => result.ResultID).ToList());
                            if (move) { Result.DeleteFromSubSet(CurrentSubSet.ID, selectedResults.Select(result => result.ResultID).ToList()); }

                            if (move)
                            {
                                // Remove them from the subset view via a soft update.
                                SelectedSubSetView.RemoveRecords(selectedResults.Cast<RowDataItem>().ToList());
                            }
                            if (newSubSet)
                            {
                                AddSubSet(selectedSubSet);
                            }
                            if (selectedSubSet.Open)
                            {
                                SubSetRecords(selectedSubSet.ID).RefreshRecords();
                            }
                        }
                        else
                        {
                            Result.AddToSubSet(frm.SelectedSubSetID, selectedResults.Select(result => result.ResultID).ToList());

                            if (move)
                            { 
                                Result.DeleteFromSubSet(CurrentSubSet.ID, selectedResults.Select(result => result.ResultID).ToList());
                                SelectedSubSetView.RemoveRecords(selectedResults.Cast<RowDataItem>().ToList());
                            }
                        }
                    }
                }
            }
        }

        private void tsbDelete_Click(object sender, EventArgs e)
        {
            List<ResultSummaryRow> selectedResults = null;
            if (GetSelectedResults(out selectedResults, true, false))
            {
                string plural = (selectedResults.Count == 1 ? string.Empty : "s");
                if (Utility.ShowMessage(this, 
                        "Deleting the selected result" + plural + " from this dataset will also delete any results from the same job that are related by tree, model, and if applicable, sites model, but which vary by starting kappa and omega values."
                        + "\r\n\r\n" + "Are you sure you want to delete the selected " + selectedResults.Count.ToString() + " result" + plural + " from this dataset?"
                        + "\r\n\r\n" + "This action cannot be undone because Dan hasn't yet programmed in Undo functionality.",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Yes)
                {
                    Result.DeleteFromSubSet(this.SelectedSubSetView.CurrentSubSet.ID, selectedResults.Select(result => result.ResultID).ToList());
                    this.CurrentRecordSet.Save(); // Update the ModifiedAt value.
                    this.SelectedSubSetView.RemoveRecords(selectedResults.Cast<RowDataItem>().ToList());
                }
            }
        }
        
        private void excelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<ResultSummaryRow> selectedResults = null;
            if (GetSelectedResults(out selectedResults))
            {
                using (frmExportToExcel frm = new frmExportToExcel(selectedResults))
                {
                    if (frm.UserInputNeeded)
                    {
                        if (frm.ShowDialog() != DialogResult.OK) { return; }
                    }

                    frm.Export();
                }
            }
        }

        private void tsbExportToPilgrimage_Click(object sender, EventArgs e)
        {
            List<ResultSummaryRow> selectedResults = null;
            if (GetSelectedResults(out selectedResults))
            {
                using (RecordSets.frmExportToPilgrimageDataFile frm = new RecordSets.frmExportToPilgrimageDataFile(null, selectedResults.Select(r => r.ResultID).ToList(), this.DataType))
                { frm.ShowDialog(); }
            }
        }
        #endregion
    }
}
