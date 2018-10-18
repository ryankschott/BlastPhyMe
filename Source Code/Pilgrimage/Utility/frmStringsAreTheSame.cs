using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Pilgrimage.Utilities
{
    public partial class frmStringsAreTheSame : Form
    {
        public frmStringsAreTheSame()
        {
            InitializeComponent();
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            btnClean.PerformClick();
            bool test = (txtString1.Text.ToUpper() == txtString2.Text.ToUpper());

            Utility.ShowMessage(this, (test).ToString());
            if (test) { Clipboard.SetText(txtString1.Text); }
        }

        private void btnClean_Click(object sender, EventArgs e)
        {
            txtString1.Text = System.Text.RegularExpressions.Regex.Replace(txtString1.Text, "[^\\w]", "");
            txtString2.Text = System.Text.RegularExpressions.Regex.Replace(txtString2.Text, "[^\\w]", "");
        }
    }
}
