using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChangLab.Common;
using ChangLab.BlastN;
using ChangLab.Jobs;
using ChangLab.Genes;
using Pilgrimage.Activities;

namespace Pilgrimage.GeneSequences.BlastN
{
    public partial class frmSubjectGeneDetails : Form
    {
        private BlastNAtNCBI Job { get; set; }
        public Gene SubjectGene { get; set; }
        private bool PopulateOnLoad { get; set; }

        public frmSubjectGeneDetails(BlastNAtNCBI Job, Gene SubjectGene, bool PopulateFromGenBank)
        {
            InitializeComponent();

            this.Job = Job;
            this.SubjectGene = SubjectGene;
            this.PopulateOnLoad = PopulateFromGenBank;
        }

        public frmSubjectGeneDetails(BlastNAtNCBI Job, string GeneID) : this(Job, null, false)
        {
            if (!string.IsNullOrEmpty(GeneID))
            {
                this.SubjectGene = Gene.Get(GeneID, false);
                if (this.SubjectGene.NeedsUpdateFromGenBank)
                {
                    this.PopulateOnLoad = true;
                }
            }
        }

        private void frmAlignmentDetails_Load(object sender, EventArgs e)
        {
            if (this.PopulateOnLoad)
            {
                PopulateFromGenBank.PopulateSync(this.SubjectGene, null, this);
                this.SubjectGene.Save(true, true);
            }

            Pilgrimage.GeneSequences.frmGeneDetails.PopulateGenBankDetailControls(this.SubjectGene, this);

            // Download query genes
            DataGridViewHelper DataGridHelper = new DataGridViewHelper(this, grdResults);
            DataGridHelper.ViewDetails += new DataGridViewHelper.ViewDetailsEventHandler(DataGridHelper_ViewDetails);
            grdResults.AutoGenerateColumns = false;
            grdResults.DataSource = new BindingSource(BlastNAtNCBI.ListInputGenes(this.SubjectGene.ID, 0, (this.Job != null ? this.Job.ID : string.Empty), Program.Settings.CurrentRecordSet.ID).ToBlastNAlignmentRowList(), null);
        }

        private void txtAccession_TextChanged(object sender, EventArgs e)
        {
            Pilgrimage.GeneSequences.frmGeneDetails.UpdateGenBankURL(txtAccession, lnkGenBankURL);
        }

        private void lnkGenBankURL_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(((LinkLabel)sender).Text);
        }

        private void txtCodingSequence_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A && e.Modifiers == Keys.Control)
            {
                txtCodingSequence.SelectAll();
            }
        }

        private void DataGridHelper_ViewDetails(DataGridViewHelper.ViewDetailsEventArgs e)
        {
            GenericGeneRowDataItem row = (GenericGeneRowDataItem)e.Row.DataBoundItem;
            using (GeneSequences.frmGeneDetails frm = new GeneSequences.frmGeneDetails(row.Gene.ID))
            {
                frm.ShowDialog(this);
            }
        }
    }
}
