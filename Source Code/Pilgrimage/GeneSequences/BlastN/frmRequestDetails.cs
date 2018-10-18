using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChangLab.Genes;
using ChangLab.Jobs;
using ChangLab.Common;

namespace Pilgrimage.GeneSequences.BlastN
{
    public partial class frmRequestDetails : DialogForm
    {
        private RequestHistoryRow Request { get; set; }
        private string JobID { get; set; }
        private DataGridViewHelper SequencesGridHelper { get; set; }
        internal List<string> EditedSubSetIDs { get; set; }

        public frmRequestDetails(RequestHistoryRow Request, string JobID)
        {
            InitializeComponent();
            SetButtonImage(btnClose, "Cancel");
            
            this.Request = Request;
            this.JobID = JobID;
            this.EditedSubSetIDs = new List<string>();

            bool empty = (this.Request.DatabaseID == 0);
            if (!empty)
            {
                txtRequestID.Text = this.Request.RequestID;
                txtTargetDatabase.Text = this.Request.TargetDatabase;
                txtAlgorithm.Text = this.Request.Algorithm;
                txtLastStatus.Text = this.Request.LastStatus.ToString();
                txtStartedAt.Text = (this.Request.StartedAt != DateTime.MinValue ? this.Request.StartedAt.ToString(DataGridViewHelper.DateTimeFormatStringWithSeconds) : string.Empty);
                txtEndedAt.Text = (this.Request.EndedAt != DateTime.MinValue ? this.Request.EndedAt.ToString(DataGridViewHelper.DateTimeFormatStringWithSeconds) : string.Empty);
                txtStatusInformation.Text = this.Request.StatusInformation;
            }
            else
            {
                // Hide the header controls
                for (int row = 0; row < tableLayoutPanel1.GetRow(lblSequences); row++)
                {
                    for (int col = 0; col < tableLayoutPanel1.ColumnCount; col++)
                    {
                        Control ctrl = tableLayoutPanel1.GetControlFromPosition(col, row);
                        if (ctrl != null) { tableLayoutPanel1.Controls.Remove(ctrl); }
                    }
                }
            }

            this.SequencesGridHelper = new DataGridViewHelper(this, grdQuerySequences);
            this.SequencesGridHelper.ViewDetails += new DataGridViewHelper.ViewDetailsEventHandler(QuerySequencesGridHelper_ViewDetails);
            this.SequencesGridHelper.Loaded = false;
            this.grdQuerySequences.AutoGenerateColumns = false;
            if (!empty)
            { this.grdQuerySequences.DataSource = new SortableBindingList<GenericGeneRowDataItem>(BlastNAtNCBI.ListInputGenesForRequest(this.Request.DatabaseID).ToRowDataItemList()); }
            else
            { this.grdQuerySequences.DataSource = new SortableBindingList<GenericGeneRowDataItem>(BlastNAtNCBI.ListNotProcessedGenes(this.JobID).ToRowDataItemList()); }
            this.SequencesGridHelper.Loaded = true;

            List<JobException> exceptions = JobException.List(this.JobID, (empty ? 0 : Request.DatabaseID));
            if (exceptions.Count != 0)
            {
                txtStatusInformation.Text += (string.IsNullOrWhiteSpace(txtStatusInformation.Text) ? "" : ".  ")
                    + exceptions.Aggregate(string.Empty, (current, ex) => current += ex.Message + ".  ");
            }

            if (string.IsNullOrWhiteSpace(txtStatusInformation.Text))
            {
                lblStatusInformation.Parent.Controls.Remove(lblStatusInformation);
                txtStatusInformation.Parent.Controls.Remove(txtStatusInformation);
            }
        }

        private void QuerySequencesGridHelper_ViewDetails(DataGridViewHelper.ViewDetailsEventArgs e)
        {
            Gene gene = ((GenericGeneRowDataItem)e.Row.DataBoundItem).Gene;
            using (GeneSequences.frmGeneDetails frm = new GeneSequences.frmGeneDetails(gene.ID, true))
            {
                frm.ShowDialog(this);
                this.EditedSubSetIDs.AddRange(frm.EditedSubSetIDs);
                if (frm.Updated)
                {
                    gene.Merge(frm.Gene);
                }
            }
        }

        private void ExceptionsGridHelper_ViewDetails(DataGridViewHelper.ViewDetailsEventArgs e)
        {
            Gene gene = ((GenericGeneRowDataItem)e.Row.DataBoundItem).Gene;
            using (GeneSequences.frmGeneDetails frm = new GeneSequences.frmGeneDetails(gene.ID, true))
            {
                frm.ShowDialog(this);
                this.EditedSubSetIDs.AddRange(frm.EditedSubSetIDs);
                if (frm.Updated)
                {
                    gene.Merge(frm.Gene);
                }
            }
        }
    }
}
