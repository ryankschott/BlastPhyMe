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
    public partial class uctResults : Pilgrimage.UserControls.uctSubSetView
    {
        protected internal override DataGridView RecordsGrid { get { return grdResults; } }
        private List<ResultSummaryRow> ResultsTable { get; set; }
        private FilterProperties Filter { get; set; }
        private bool Filtered { get; set; }

        public uctResults()
        {
            InitializeComponent();
            clmK.DefaultCellStyle.Format = DataGridViewHelper.DefaultDoubleFormatString;
            clmlnL.DefaultCellStyle.Format = DataGridViewHelper.DefaultDoubleFormatString;
            clmCompletedAt.DefaultCellStyle.Format = DataGridViewHelper.DefaultDateTimeFormatString;
            Filter = new FilterProperties();
            this.Loaded = false;
        }

        public void Initialize(ChangLab.RecordSets.SubSet SubSet)
        {
            this.CurrentSubSet = SubSet;

            this.DataGridHelper = new DataGridViewHelper(this.ParentForm, grdResults, DataGridViewHelper.DataSourceTypes.Other);
            this.DataGridHelper.SelectedRowsChanged += new DataGridViewHelper.SelectedRowsChangedEventHandler(DataGridHelper_SelectedRowsChanged);
            this.DataGridHelper.ViewDetails += new DataGridViewHelper.ViewDetailsEventHandler(DataGridHelper_ViewDetails);
        }

        public override void RefreshRecords(bool RefreshFromDatabase = true, bool SoftRefresh = false)
        {
            if (SoftRefresh) { grdResults.Refresh(); return; }

            Refreshing = true;

            if (RefreshFromDatabase)
            {
                this.ResultsTable = ChangLab.PAML.CodeML.Result.List(CurrentSubSet.ID).Rows.Cast<DataRow>().Select(row => new ResultSummaryRow(row)).ToList();
                RefreshRecords(false, false);
                return;
            }

            ResultSummaryRow[] copy = new ResultSummaryRow[ResultsTable.Count]; ResultsTable.CopyTo(copy);
            IEnumerable<ResultSummaryRow> query = copy.Select(r => r);
            int totalRecords = query.Count();
            ApplyFilter(ref query);
            int filteredRecords = query.Count();

            if (SelectedRows.Count != 0)
            {
                // Reselect the genes that were already selected.
                SelectedRows.Cast<ResultSummaryRow>().ToList().ForEach(srow =>
                {
                    ResultSummaryRow row = query.FirstOrDefault(qrow => qrow.ResultID == srow.ResultID);
                    if (row != null) { row.Selected = true; }
                }
                );
            }

            Filter_UpdateText(filteredRecords, totalRecords);

            this.DataGridHelper.Loaded = false;

            DataGridViewColumn sortColumn = grdResults.SortedColumn;
            SortOrder sortOrder = grdResults.SortOrder;
            
            grdResults.AutoGenerateColumns = false;
            grdResults.DataSource = null;
            grdResults.DataSource = new SortableBindingList<ResultSummaryRow>(query);

            if (sortColumn != null && sortOrder != SortOrder.None)
            {
                grdResults.Sort(sortColumn, (grdResults.SortOrder == SortOrder.Ascending ? ListSortDirection.Ascending : ListSortDirection.Descending));
            }

            this.DataGridHelper.Loaded = true;

            DataGridHelper_SelectedRowsChanged(null);

            Refreshing = false;
            if (!Loaded) { Loaded = true; }
        }

        private void ApplyFilter(ref IEnumerable<ResultSummaryRow> Records)
        {
            Filtered = false;

            if (!string.IsNullOrWhiteSpace(this.Filter.JobTitle))
            {
                Records = Records.Where(r => r.JobTitle.Match(this.Filter.JobTitle, this.Filter.JobTitleMatchLogic));
                Filtered = true;
            }
            if (!string.IsNullOrWhiteSpace(this.Filter.TreeTitle))
            {
                Records = Records.Where(r => r.TreeTitle.Match(this.Filter.TreeTitle, this.Filter.TreeTitleMatchLogic));
                Filtered = true;
            }
            if (!string.IsNullOrWhiteSpace(this.Filter.TreeFile))
            {
                Records = Records.Where(r => r.TreeFileName.Match(this.Filter.TreeFile, this.Filter.TreeFileMatchLogic));
                Filtered = true;
            }
            if (!string.IsNullOrWhiteSpace(this.Filter.SequencesFile))
            {
                Records = Records.Where(r => r.SequencesFileName.Match(this.Filter.SequencesFile, this.Filter.SequenceFileMatchLogic));
                Filtered = true;
            }
            if (Filter.Models.Count != 0)
            {
                Records = Records.Where(r => Filter.Models.Contains(r.ModelPresetKey));
                Filtered = true;
            }
        }

        private void Header_UpdateText()
        {
            chkToggleSelected.Text = (chkToggleSelected.Checked ? "Deselect all" : "Select all");
            lblTotalRows.Text = this.SelectedRows.Count.ToString("N0") + " of " + grdResults.Rows.Count.ToString("N0") + " records selected";
        }

        private void Filter_UpdateText(int FilteredRecords, int TotalRecords)
        {
            lblFilterRows.Text = string.Format("Showing {0} of {1} sequences", FilteredRecords.ToString("N0"), TotalRecords.ToString("N0"));
            lnkFilter.Text = (Filtered ? "Change" : "Apply") + " Filter";
            lnkClearFilter.Text = (Filtered ? "Clear Filter" : string.Empty);
        }

        private void DataGridHelper_SelectedRowsChanged(DataGridViewHelper.SelectedRowEventArgs e)
        {
            bool allSelected = (grdResults.Rows.Count > 0 && this.SelectedRows.Count == grdResults.Rows.Count);
            chkToggleSelected.CheckedChanged -= chkToggleSelected_CheckedChanged;

            chkToggleSelected.Checked = allSelected;
            Header_UpdateText();

            chkToggleSelected.CheckedChanged += new EventHandler(chkToggleSelected_CheckedChanged);
        }

        private void DataGridHelper_ViewDetails(DataGridViewHelper.ViewDetailsEventArgs e)
        {
            using (frmResultDetails frm = new frmResultDetails(((ResultSummaryRow)e.Row.DataBoundItem).ResultID))
            {
                frm.ShowDialog(this.ParentForm);

                if (frm.EditedSubSetIDs.Count != 0)
                { frm.EditedSubSetIDs.Distinct().ToList().ForEach(sub => this.Manager.ShowAndRefreshSubSet(sub, null)); }
            }
        }

        private void chkToggleSelected_CheckedChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in grdResults.Rows)
            {
                row.Cells[DataGridHelper.SelectedColumnIndex].Value = chkToggleSelected.Checked;
            }

            Header_UpdateText();
        }

        public override void RemoveRecords(List<RowDataItem> Results)
        {
            SortableBindingList<ResultSummaryRow> dataSource = (SortableBindingList<ResultSummaryRow>)grdResults.DataSource;
            Results.Cast<ResultSummaryRow>().Select(result => result.ResultID).ToList().ForEach(id =>
            {
                ResultSummaryRow match = dataSource.FirstOrDefault(row => row.ResultID == id);
                if (match != null) { dataSource.Remove(match); }
            });

            Filter_UpdateText(dataSource.Count, ResultsTable.Count);
            DataGridHelper_SelectedRowsChanged(null);
        }

        private void lnkFilter_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (sender == lnkFilter)
            {
                using (PAML.frmFilterResults frm = new PAML.frmFilterResults(this.Filter))
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        this.DataGridHelper.ShortTermDisableKeyPress();

                        this.Filter = frm.Filter;
                        RefreshRecords(false);
                    }
                }
            }
            else
            {
                this.Filter = new FilterProperties();
                RefreshRecords();
            }

            grdResults.Focus();
        }
    }
}
