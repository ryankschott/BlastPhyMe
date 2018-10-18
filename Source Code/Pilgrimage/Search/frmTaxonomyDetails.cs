using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChangLab.Taxonomy;

namespace Pilgrimage.Search
{
    public partial class frmTaxonomyDetails : DialogForm
    {
        internal Taxon Taxon { get; set; }
        private string BaseUrl { get { return "https://www.ncbi.nlm.nih.gov/Taxonomy/Browser/wwwtax.cgi?mode=Info&id="; } }

        public frmTaxonomyDetails()
        {
            InitializeComponent();
        }

        private void frmTaxonomyDetails_Load(object sender, EventArgs e)
        {
            txtScientificName.Text = this.Taxon.Name;
            txtCommonName.Text = this.Taxon.OtherName;
            txtRank.Text = this.Taxon.Rank;
            txtDivision.Text = this.Taxon.Division;
            lnkNCBI.Text = this.BaseUrl + this.Taxon.TaxonomyDatabaseID.ToString();
            lnkNCBI.Tag = lnkNCBI.Text;

            if (this.Taxon.LineageList != null && this.Taxon.LineageList.Count != 0)
            {
                for (int i = 0; i < this.Taxon.LineageList.Count; i++)
                {
                    Taxon lineage = this.Taxon.LineageList[i];
                    LinkLabel lnk = new LinkLabel()
                    {
                        AutoSize = true,
                        Margin = new Padding(3, 3, 0, 3),
                        Text = lineage.Name + ((i + 1) < this.Taxon.LineageList.Count ? ";" : string.Empty),
                        Tag = this.BaseUrl + lineage.TaxonomyDatabaseID.ToString()
                    };
                    lnk.LinkClicked += new LinkLabelLinkClickedEventHandler(lnk_LinkClicked);

                    pnlLineage.Controls.Add(lnk);
                    toolTip.SetToolTip(lnk, lineage.Rank);
                }
            }
            else
            {
                // Show whatever we've got for lineage as a textbox.
                pnlLineage.Parent.Controls.Remove(pnlLineage);

                TextBox txtLineage = new TextBox()
                    {
                        Anchor = (AnchorStyles)(AnchorStyles.Left | AnchorStyles.Right),
                        Margin = new Padding(5),
                        TabIndex = 9,
                        Text = this.Taxon.Lineage
                    };
                tblForm.Controls.Add(txtLineage, 1, 3);
                tblForm.SetColumnSpan(txtLineage, 3);
            }
        }

        private void lnk_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start((string)((LinkLabel)sender).Tag);
        }
    }
}
