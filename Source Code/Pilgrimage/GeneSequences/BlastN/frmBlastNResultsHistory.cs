using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Pilgrimage.GeneSequences.BlastN
{
    public partial class frmBlastNResultsHistory : DialogForm
    {
        internal List<string> EditedSubSetIDs { get; set; }

        public frmBlastNResultsHistory()
        {
            InitializeComponent();
            EditedSubSetIDs = new List<string>();
            blastNHistory.Initialize(this);
            SetButtonImage(btnClose, "Cancel");
        }

        private void frmBlastNResultsHistory_Load(object sender, EventArgs e)
        {
            blastNHistory.RefreshHistory();
        }

        private void frmBlastNResultsHistory_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.EditedSubSetIDs.AddRange(blastNHistory.EditedSubSetIDs.Where(id => !this.EditedSubSetIDs.Contains(id)));
        }

        private void tsbArchiveResults_Click(object sender, EventArgs e)
        {
            blastNHistory.ArchiveSelected();
        }

        private void tsbViewAllHistory_Click(object sender, EventArgs e)
        {
            using (BlastN.frmBlastNAlignments frm = new BlastN.frmBlastNAlignments(string.Empty))
            {
                frm.ShowDialog(this);
                if (frm.EditedSubSetIDs.Count != 0)
                {
                    this.EditedSubSetIDs.AddRange(frm.EditedSubSetIDs.Where(id => !this.EditedSubSetIDs.Contains(id)));
                }
            }
        }
    }
}
