using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using ChangLab.Common;

namespace Pilgrimage
{
    partial class frmAbout : Form
    {
        private DataGridViewHelper DataGridHelper { get; set; }

        public frmAbout()
        {
            InitializeComponent();
            this.lblThirdPartyComponents.Text = Program.ProductName + " implements the following third-party components:";

            this.lblProduct.Text = 
                Utility.GetEntryAssemblyTitle()
                + "\r\n"
                + string.Format("Version {0}", Assembly.GetEntryAssembly().GetName().Version)
                + "\r\n"
                + Assembly.GetEntryAssembly().GetAssemblyAttribute<AssemblyCopyrightAttribute>(a => a.Copyright);

            this.Text = String.Format("About {0}", Program.ProductName);

            this.DataGridHelper = new DataGridViewHelper(this, grdComponents, DataGridViewHelper.DataSourceTypes.Other, true);
            this.DataGridHelper.ViewDetails += new DataGridViewHelper.ViewDetailsEventHandler(DataGridHelper_ViewDetails);
        }

        private void frmAbout_Load(object sender, EventArgs e)
        {
            this.grdComponents.AutoGenerateColumns = false;
            this.grdComponents.DataSource = 
                new SortableBindingList<ThirdPartyComponentReferenceRowDataItem>(
                    ThirdPartyComponentReference.List()
                    .Select(r => new ThirdPartyComponentReferenceRowDataItem(r) 
                                { 
                                    ComponentLogo = (!string.IsNullOrWhiteSpace(r.Logo) ? (Bitmap)Properties.Resources.ResourceManager.GetObject(r.Logo + "_16") : Properties.Resources.CMD_16x16)
                                }));
        }

        private void lnkGPL_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(Application.StartupPath + "\\license.txt");
            }
            catch (Exception ex)
            {
                Utility.ShowErrorMessage(this, ex);
            }
        }

        private void DataGridHelper_ViewDetails(DataGridViewHelper.ViewDetailsEventArgs e)
        {
            using (frmThirdPartyComponentReferenceDetails details = new frmThirdPartyComponentReferenceDetails(((ThirdPartyComponentReferenceRowDataItem)e.Row.DataBoundItem).ComponentReference))
            {
                details.ShowDialog(this);
            }
        }
    }
}
