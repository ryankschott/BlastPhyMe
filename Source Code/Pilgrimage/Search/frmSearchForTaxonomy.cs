using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChangLab.Common;
using ChangLab.NCBI.Taxonomy;
using ChangLab.Taxonomy;
using Pilgrimage.Activities;
using Pilgrimage;

namespace Pilgrimage.Search
{
    public partial class frmSearchForTaxonomy : DialogForm
    {
        private SearchNCBIForTaxa SearchNCBI { get; set; }
        private TaxonomyServiceSearch NCBITaxonomySearch { get; set; }

        private bool SearchLocal { get; set; }
        private List<Taxon> LocalTaxonomySearch { get; set; }

        internal Taxon SelectedTaxon { get; private set; }
        internal bool PopulateDetailsForSelectedTaxon { get; set; }

        private DataGridViewHelper NCBIDataGridHelper { get; set; }
        private DataGridViewHelper LocalDataGridHelper { get; set; }

        public frmSearchForTaxonomy(bool SearchLocal = false, string SearchQuery = "")
        {
            InitializeComponent();
            this.SearchLocal = SearchLocal;
            this.PopulateDetailsForSelectedTaxon = false;
            this.txtSearchQuery.Text = SearchQuery;

            SetButtonImage(btnSearch, "Search");
            SetButtonImage(btnCancel, "Cancel");
            SetButtonImage(btnAddFromNCBI, "Add");
            SetButtonImage(btnAddFromLocal, "Add");
            if (!SearchLocal) { btnAddFromNCBI.Width = 80; }

            lblNCBIResults.Parent.Controls.Remove(lblNCBIResults);
            grdNCBIResults.Parent.Controls.Remove(grdNCBIResults);
            lblTotalNCBIRows.Parent.Controls.Remove(lblTotalNCBIRows);
            btnAddFromNCBI.Parent.Controls.Remove(btnAddFromNCBI);
            lblLocalResults.Parent.Controls.Remove(lblLocalResults);
            grdLocalResults.Parent.Controls.Remove(grdLocalResults);
            lblTotalLocalRows.Parent.Controls.Remove(lblTotalLocalRows);
            btnAddFromLocal.Parent.Controls.Remove(btnAddFromLocal);
            this.Height = 128;

            this.FocusOnLoad = txtSearchQuery;
        }

        public frmSearchForTaxonomy(DialogButtonPresets AcceptButtonConfiguration, bool SearchLocal = false, string SearchQuery = "")
            : this(SearchLocal, SearchQuery)
        {
            SetButtonImage(btnAddFromNCBI, AcceptButtonConfiguration);
            SetButtonImage(btnAddFromLocal, AcceptButtonConfiguration);

            switch (AcceptButtonConfiguration)
            {
                case DialogButtonPresets.OK:
                    btnAddFromNCBI.Text = "Accept from &NCBI"; btnAddFromNCBI.Width = 135;
                    btnAddFromLocal.Text = "Accept from &Local"; btnAddFromLocal.Width = 135;
                    break;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearchQuery.Text))
            {
                Utility.ShowMessage(this, "Search query cannot be empty.");
            }
            else
            {
                SearchNCBI = new SearchNCBIForTaxa(this);
                SearchNCBI.ActivityCompleted += new Activity.ActivityCompletedEventHandler(Search_ActivityCompleted);
                SearchNCBI.SubmitSearch(txtSearchQuery.Text);
            }
        }

        private void Search_ActivityCompleted(ActivityCompletedEventArgs e)
        {
            if (e.Error != null) { Utility.ShowErrorMessage(this, e.Error); }
            else if (e.Cancelled && ((TaxonomyServiceSearch)e.ActivityResult).Count == 0) { return; }
            else
            {
                NCBITaxonomySearch = (TaxonomyServiceSearch)e.ActivityResult;

                grdNCBIResults.AutoGenerateColumns = false;
                grdNCBIResults.DataSource = new SortableBindingList<Taxon>(NCBITaxonomySearch.Results);

                if (SearchLocal)
                {
                    // This should be fast enough that we don't need an async search; maybe implement that way later if I ever find this comment again
                    LocalTaxonomySearch = Taxon.List(txtSearchQuery.Text.Replace("*", "%"));
                    
                    grdLocalResults.AutoGenerateColumns = false;
                    grdLocalResults.DataSource = new SortableBindingList<Taxon>(LocalTaxonomySearch);
                }

                ShowResults();
            }
        }

        private void ShowResults()
        {
            lblTotalNCBIRows.Text = NCBITaxonomySearch.SearchResult.ResultCount.ToString("N0") + " results found"
                    + (NCBITaxonomySearch.Count != NCBITaxonomySearch.SearchResult.ResultCount
                        ? ", " + (NCBITaxonomySearch.Count == SearchNCBI.ResultMaximum && NCBITaxonomySearch.SearchResult.ResultCount > SearchNCBI.ResultMaximum ? "maximum of " : string.Empty) + NCBITaxonomySearch.Count.ToString("N0") + " shown"
                        : string.Empty);
            if (SearchLocal)
            {
                lblTotalLocalRows.Text = LocalTaxonomySearch.Count.ToString("N0") + " results found";
            }

            if (grdNCBIResults.Parent == null)
            {
                tableLayoutPanel1.Controls.Add(lblNCBIResults, 0, 3);
                tableLayoutPanel1.Controls.Add(grdNCBIResults, 1, 3);
                tableLayoutPanel1.Controls.Add(lblTotalNCBIRows, 1, 4);
                tableLayoutPanel1.Controls.Add(btnAddFromNCBI, 2, 4);

                if (SearchLocal)
                {
                    tableLayoutPanel1.Controls.Add(lblLocalResults, 0, 5);
                    tableLayoutPanel1.Controls.Add(grdLocalResults, 1, 5);
                    tableLayoutPanel1.Controls.Add(lblTotalLocalRows, 1, 6);
                    tableLayoutPanel1.Controls.Add(btnAddFromLocal, 2, 6);
                }
                else
                {
                    tableLayoutPanel1.RowStyles[5].Height = 0F;
                    tableLayoutPanel1.RowStyles[3].Height = 100F;
                }

                this.NCBIDataGridHelper = new DataGridViewHelper(this, grdNCBIResults, DataGridViewHelper.DataSourceTypes.Taxa);
                this.NCBIDataGridHelper.ViewDetails += new DataGridViewHelper.ViewDetailsEventHandler(DataGridHelper_ViewDetails);

                this.Location = new System.Drawing.Point(this.Location.X, this.Location.Y - ((this.grdNCBIResults.Height / 2) + (SearchLocal ? this.grdLocalResults.Height / 2 : 0)));
            }
        }

        private void DataGridHelper_ViewDetails(DataGridViewHelper.ViewDetailsEventArgs e)
        {
            Taxon taxon = (Taxon)e.Row.DataBoundItem;
            if (string.IsNullOrWhiteSpace(taxon.Lineage))
            {
                taxon = FetchDetails(taxon);
            }

            using (frmTaxonomyDetails frm = new frmTaxonomyDetails() { Taxon = taxon })
            {
                frm.ShowDialog();
            }
        }

        private Taxon FetchDetails(Taxon Taxon)
        {
            List<Taxon> fetched = (new TaxonomyFetch()).FetchRecords((new string[] { Taxon.TaxonomyDatabaseID.ToString() }).ToList(), NCBITaxonomySearch.SearchResult);
            if (fetched.Any(t => t.TaxonomyDatabaseID == Taxon.TaxonomyDatabaseID))
            {
                Taxon.Merge(fetched.First(t => t.TaxonomyDatabaseID == Taxon.TaxonomyDatabaseID));
            }

            return Taxon;
        }

        private void btnAddFromNCBI_Click(object sender, EventArgs e)
        {
            if (grdNCBIResults.SelectedRows.Count == 0)
            {
                Utility.ShowMessage(this, Properties.Resources.Messages_NoResultsSelected);
            }
            else
            {
                this.SelectedTaxon = (Taxon)grdNCBIResults.SelectedRows[0].DataBoundItem;

                if (this.PopulateDetailsForSelectedTaxon && string.IsNullOrEmpty(this.SelectedTaxon.Lineage))
                {
                    this.SelectedTaxon = FetchDetails(this.SelectedTaxon);
                }

                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }

        private void btnAddFromLocal_Click(object sender, EventArgs e)
        {
            if (grdLocalResults.SelectedRows.Count == 0)
            {
                Utility.ShowMessage(this, Properties.Resources.Messages_NoResultsSelected);
            }
            else
            {
                this.SelectedTaxon = (Taxon)grdLocalResults.SelectedRows[0].DataBoundItem;
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }
    }
}
