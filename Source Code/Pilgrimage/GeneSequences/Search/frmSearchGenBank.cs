using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ChangLab.NCBI;

namespace Pilgrimage.GeneSequences.Search
{
    public partial class frmSearchGenBank : DialogForm
    {
        private List<ESearchHistory> SearchHistory { get; set; }
        public bool UseHistory { get; private set; }
        public ESearchHistory SelectedHistory { get; private set; }

        public frmSearchGenBank()
        {
            InitializeComponent();
            this.Icon = Properties.Resources.DefaultIcon;

            SetButtonImage(btnTaxonomy, "Filter");
            SetButtonImage(btnSearch, "Search");
            SetButtonImage(btnCancel, "Cancel");
            this.SearchHistory = ESearchHistory.List(Program.Settings.CurrentRecordSet.ID, EUtilities.Databases.NucCore);

            this.FocusOnLoad = txtSearchQuery;
        }

        private void frmSearchGenBank_Load(object sender, EventArgs e)
        {
            if (this.SearchHistory.Count == 0)
            {
                lblSearchHistory.Parent.Controls.Remove(lblSearchHistory);
                tblSearchHistory.Parent.Controls.Remove(tblSearchHistory);
            }
            else
            {
                tblSearchHistory.RowStyles.Clear();
                tblSearchHistory.RowCount = this.SearchHistory.Count;

                for (int i = 0; i < this.SearchHistory.Count; i++)
                {
                    ESearchHistory history = this.SearchHistory[i];
                    tblSearchHistory.RowStyles.Add(new RowStyle() { SizeType = SizeType.AutoSize });

                    LinkLabel lnk = new LinkLabel()
                    {
                        Anchor = (AnchorStyles)(AnchorStyles.Left | AnchorStyles.Right),
                        AutoSize = true,
                        Margin = new System.Windows.Forms.Padding(5),
                        Text = (history.Term.Length > 100 ? history.Term.Substring(0, 97) + "..." : history.Term) + " (" + history.ResultCount.ToString("N0") + " results)",
                        Tag = history
                    };

                    lnk.LinkClicked += new LinkLabelLinkClickedEventHandler(lnk_LinkClicked);
                    tblSearchHistory.Controls.Add(lnk, 0, i);
                    this.FormToolTip.SetToolTip(lnk, history.Term);
                }

                this.Location = new System.Drawing.Point(this.Location.X, this.Location.Y - (this.tblSearchHistory.Height / 2));
            }
        }

        private void lnk_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.SelectedHistory = (ESearchHistory)((LinkLabel)sender).Tag;
            this.UseHistory = true;
            txtSearchQuery.Text = this.SelectedHistory.Term;
            txtSearchQuery.ScrollToEnd();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearchQuery.Text))
            {
                Utility.ShowMessage(this, "Search query cannot be empty.");
            }
            else
            {
                if (this.UseHistory && this.txtSearchQuery.Text == this.SelectedHistory.Term)
                {
                    // The user didn't change the query, we can go ahead and use its history.
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    return;
                }

                // Either the user hadn't clicked a recent link, or they modified the query...

                // Check to see if the user's searched this recently.
                ESearchHistory match = this.SearchHistory.FirstOrDefault(h => h.Term.ToLower() == txtSearchQuery.Text.ToLower());
                if (match != null)
                {
                    this.UseHistory = true;
                    this.SelectedHistory = match;
                }
                else
                {
                    this.UseHistory = false;
                }
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }

        private void btnTaxonomy_Click(object sender, EventArgs e)
        {
            using (Pilgrimage.Search.frmSearchForTaxonomy frm = new Pilgrimage.Search.frmSearchForTaxonomy())
            {
                if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (this.txtSearchQuery.Text.Length != 0 && !this.txtSearchQuery.Text.EndsWith(" "))
                    {
                        this.txtSearchQuery.Text += " ";
                    }

                    this.txtSearchQuery.Text += string.Format("Txid{0}[Organism:exp]", frm.SelectedTaxon.TaxonomyDatabaseID);
                }
            }
        }
    }
}
