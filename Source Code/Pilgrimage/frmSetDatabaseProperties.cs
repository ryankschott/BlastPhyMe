using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Pilgrimage
{
    public partial class frmSetDatabaseProperties : DialogForm
    {
        internal string NCBIEmailAddress { get; set; }

        public frmSetDatabaseProperties()
        {
            InitializeComponent();
            txtEmailAddress.Text = this.NCBIEmailAddress;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string validation = string.Empty;
            if (string.IsNullOrWhiteSpace(txtEmailAddress.Text)) { validation = "Please provide an e-mail address."; }
            else if (!txtEmailAddress.Text.IsEmailAddress()) { validation = "Please provide a valid e-mail address."; }

            if (!string.IsNullOrWhiteSpace(validation))
            {
                Utility.ShowMessage(this.OwnerForm, validation, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                this.NCBIEmailAddress = txtEmailAddress.Text;
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }

        private void frmSetDatabaseProperties_Load(object sender, EventArgs e)
        {
#if EEB460
            this.NCBIEmailAddress = "sarah.dungan@mail.utoronto.ca";
            this.DialogResult = System.Windows.Forms.DialogResult.OK;            
#endif
        }
    }
}
