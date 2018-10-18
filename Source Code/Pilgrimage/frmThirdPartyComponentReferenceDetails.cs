using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChangLab.Common;

namespace Pilgrimage
{
    public partial class frmThirdPartyComponentReferenceDetails : DialogForm
    {
        private ThirdPartyComponentReference Component { get; set; }

        public frmThirdPartyComponentReferenceDetails() : this(null) { }

        public frmThirdPartyComponentReferenceDetails(ThirdPartyComponentReference Component)
        {
            InitializeComponent();
            this.lblPackaged.Text = "This component is not included in the " + Program.ProductName + " installation and must be separately installed.";

            if (Component != null)
            {
                this.Text = (string.IsNullOrWhiteSpace(Component.LongName) ? Component.Name : Component.LongName);
                txtNameAndVersion.Text = this.Text + " v" + Component.Version;
                txtCreatedByAndCopyright.Text = Component.Creator + (!string.IsNullOrWhiteSpace(Component.Copyright) ? " (c) " + Component.Copyright : "");

                if (string.IsNullOrEmpty(Component.Logo))
                { pbLogo.Image = Properties.Resources.CMD_48x48; }
                else
                { pbLogo.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject(Component.Logo + "_48"); }

                if (string.IsNullOrWhiteSpace(Component.Citation))
                {
                    lblCitation.Parent.Controls.Remove(lblCitation);
                    txtCitation.Parent.Controls.Remove(txtCitation);
                }
                else
                { txtCitation.Text = Component.Citation; }

                lnkProductURL.Text = Component.ProductURL;
                lnkProductURL.Tag = Component.ProductURL;
                txtLastUpdatedAt.Text = Component.LastUpdatedAt.ToStandardFriendlyDateString();
                txtLastRetrievedAt.Text = Component.LastRetrievedAt.ToStandardFriendlyDateString();

                if (Component.Packaged)
                { lblPackaged.Parent.Controls.Remove(lblPackaged); }

                if (!Component.Modified)
                { lblModified.Parent.Controls.Remove(lblModified); }

                if (string.IsNullOrEmpty(Component.LicenseType))
                {
                    lblLicenseType.Parent.Controls.Remove(lblLicenseType);
                    txtLicenseType.Parent.Controls.Remove(txtLicenseType);
                }
                else
                { 
                    txtLicenseType.Text = Component.LicenseType;
                    lblLicenseText.Parent.Controls.Remove(lblLicenseText);
                }

                if (string.IsNullOrEmpty(Component.LicenseURL))
                { lnkLicenseURL.Parent.Controls.Remove(lnkLicenseURL); }
                else
                {
                    lnkLicenseURL.Text = Component.LicenseURL;
                    lnkLicenseURL.Tag = Component.LicenseURL;
                }

                if (string.IsNullOrEmpty(Component.LicenseText))
                { txtLicenseText.Parent.Controls.Remove(txtLicenseText); }
                else
                {
                    txtLicenseText.Lines = Component.LicenseText.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                    txtLicenseText.Select(0, 0);
                    txtLicenseText.ScrollToCaret();
                }
            }
        }

        private void frmThirdPartyComponentReferenceDetails_Load(object sender, EventArgs e)
        {
            this.SetFocusOnControl(lblName);
        }

        private void lnkURL_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start((string)((LinkLabel)sender).Tag);
            }
            catch (Exception ex)
            {
                Utility.ShowErrorMessage(this, ex);
            }
        }
    }
}
