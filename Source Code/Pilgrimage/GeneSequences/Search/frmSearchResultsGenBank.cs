using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChangLab.Common;
using ChangLab.Genes;
using ChangLab.NCBI.GenBank;
using ChangLab.RecordSets;
using Pilgrimage.Activities;

namespace Pilgrimage.GeneSequences.Search
{
    public partial class frmSearchResultsGenBank : AddGeneSequencesToRecordSetForm
    {
        private int PageCount { get; set; }
        private int PageNumber { get; set; }

        private List<GenericGeneRowDataItem> _selectedGeneRows = null;
        internal override List<GenericGeneRowDataItem> SelectedGeneRows { get { return _selectedGeneRows; } }
        internal override List<RowDataItem> SelectedRows
        {
            get
            {
                return _selectedGeneRows.Cast<RowDataItem>().ToList();
            }
        }

        public frmSearchResultsGenBank(GenBankSearch Results)
        {
            InitializeComponent();
            SetButtonImage(btnSave, "Add");
            SetButtonImage(btnClose, "Cancel");

            Configure(grdResults, cmbSubSets, btnSave, chkUpdateFromGenBank);
            _selectedGeneRows = new List<GenericGeneRowDataItem>();

            this.Search = Results;
            btnSave.Text = "&Add to";

            this.FocusOnLoad = grdResults;
        }

        private void frmSearchResultsGenBank_Load(object sender, EventArgs e)
        {
#if DOCUMENTATION
            this.Size = new System.Drawing.Size(806, 600);
#endif

            this.SubjectDataGridHelper = new DataGridViewHelper(this, grdResults);
            this.SubjectDataGridHelper.SelectedRowsChanged += new DataGridViewHelper.SelectedRowsChangedEventHandler(DataGridHelper_UpdateSelected);
            this.SubjectDataGridHelper.ViewDetails += new DataGridViewHelper.ViewDetailsEventHandler(DataGridHelper_ViewDetails);
            
            lblTotalRows.Text = "Found " + this.Search.SearchResult.ResultCount.ToString("N0") + " nucleotide sequences.";
            lblSelectedRows.Text = "0 records selected";
            btnSave.Enabled = false;

            if (this.Search.SearchResult.ResultCount > 0)
            {
                PageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(this.Search.SearchResult.ResultCount) / Convert.ToDouble(this.Search.SearchResult.ReturnMaximum)));
                txtPages.Text = "1";
                lblPageCount.Text = "of " + PageCount.ToString();
                RefreshGrid(1);
            }
            else
            {
                txtPages.Text = string.Empty;
                lblPageCount.Text = string.Empty;
                grdResults.DataSource = null;
                
                btnFirstPage.Enabled = false;
                btnPreviousPage.Enabled = false;
                btnNextPage.Enabled = false;
                btnLastPage.Enabled = false;
                txtPages.Enabled = false;
                btnSave.Enabled = false;
            }
        }

        private void DataGridHelper_UpdateSelected(DataGridViewHelper.SelectedRowEventArgs e)
        {
            if (e.UpdatedRow.Selected)
            {
                if (!SelectedGeneRows.Any(row => GuidCompare.Equals(row.ID, e.UpdatedRow.ID)))
                { SelectedGeneRows.Add((GenericGeneRowDataItem)e.UpdatedRow); }
            }
            else
            {
                GenericGeneRowDataItem match = SelectedGeneRows.FirstOrDefault(row => GuidCompare.Equals(row.ID, e.UpdatedRow.ID));
                if (match != null) { SelectedGeneRows.Remove(match); }
            }

            UpdateControlsForSelectedRows();
        }

        private void DataGridHelper_ViewDetails(DataGridViewHelper.ViewDetailsEventArgs e)
        {
            Gene gene = ((GenericGeneRowDataItem)e.Row.DataBoundItem).Gene;
            bool populate = (gene.NeedsUpdateFromGenBank);

            using (GeneSequences.frmGeneDetails frm = new GeneSequences.frmGeneDetails(gene, populate, this.Search))
            {
                frm.ShowDialog(this.OwnerForm);
                if (populate)
                {
                    // Update the instance of the Gene on this form, so that if the user selects it and adds it to their recordset we've already
                    // got the additional data points and don't need to EFetch them again.
                    this.Search.Results[this.Search.Results.IndexOf(this.Search.Results.First(g => g.GenBankID == frm.Gene.GenBankID))].Merge(frm.Gene);
                }
            }
        }

        private void UpdateControlsForSelectedRows()
        {
            lblSelectedRows.Text = SelectedGeneRows.Count.ToString() + " records selected";
            btnSave.Enabled = (SelectedGeneRows.Count != 0);
        }

        private void RefreshGrid(int PageNumber)
        {
            this.SubjectDataGridHelper.Loaded = false;
            grdResults.AutoGenerateColumns = false;

            int lBound = (PageNumber - 1) * this.Search.SearchResult.ReturnMaximum;
            if (this.Search.GetRange(lBound).Count() == 0)
            {
                this.Enabled = false;
                this.Search.ResultsSummary(lBound);
                this.Enabled = true;
            }

            IEnumerable<Gene> range = this.Search.GetRange(lBound);
            Dictionary<int, bool> inRecordSet = GenBankSearch.InRecordSet_ByGenBankID(range.Select(g => g.GenBankID), Program.Settings.CurrentRecordSet.ID);
            List<GenericGeneRowDataItem> genes = range
                .Select(g => new GenericGeneRowDataItem(g)
                {
                    Selected = SelectedGeneRows.Any(row => GuidCompare.Equals(row.ID, g.ID)),
                    InRecordSet = inRecordSet[g.GenBankID]
                }).ToList();
            if (genes.Count() != 0)
            { grdResults.DataSource = genes; }
            else
            { grdResults.DataSource = null; }
            this.SubjectDataGridHelper.Loaded = true;

            this.PageNumber = PageNumber;
            txtPages.Text = PageNumber.ToString();

            btnFirstPage.Enabled = this.PageNumber > 1;
            btnPreviousPage.Enabled = this.PageNumber > 1;
            btnNextPage.Enabled = this.PageNumber < this.PageCount;
            btnLastPage.Enabled = this.PageNumber < this.PageCount;

            grdResults.Focus();
        }

        #region Navigation
        private void txtPages_KeyUp(object sender, KeyEventArgs e)
        {
            if (!e.Alt && !e.Control && !e.Shift && e.KeyCode == Keys.Enter)
            {
                int page = 0;
                if (int.TryParse(txtPages.Text.Replace(" ", ""), out page))
                {
                    if (page < 1) { page = 1; }
                    else if (page > PageCount) { page = PageCount; }

                    RefreshGrid(page);
                }
            }
        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            RefreshGrid(1);
        }

        private void btnPreviousPage_Click(object sender, EventArgs e)
        {
            RefreshGrid(PageNumber - 1);
        }

        private void btnNextPage_Click(object sender, EventArgs e)
        {
            RefreshGrid(PageNumber + 1);
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            RefreshGrid(PageCount);
        }
        #endregion

        #region Details
        //private void grdResults_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        //{
        //    if (e.Button == System.Windows.Forms.MouseButtons.Left)
        //    {
        //        ViewDetails();
        //    }
        //}

        //private void detailsToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    ViewDetails();
        //}
        #endregion

        protected internal override void GenBankCompleted(RunWorkerCompletedEventArgs e)
        {
            base.GenBankCompleted(e);
            if (e.Error == null && !e.Cancelled)
            {
                _selectedGeneRows.Clear();
                UpdateControlsForSelectedRows();
                grdResults.Focus();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (SelectedGeneRows.Count != 0)
            {
                if (Utility.ShowMessage(this, "Are you sure you want to discard the " + SelectedGeneRows.Count.ToString() + " selected results?",
                                            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                }
            }
        }
    }
}
