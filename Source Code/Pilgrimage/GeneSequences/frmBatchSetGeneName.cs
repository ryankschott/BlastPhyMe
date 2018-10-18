using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChangLab.Genes;

namespace Pilgrimage.GeneSequences
{
    public partial class frmBatchSetGeneName : DialogForm
    {
        private List<Gene> SelectedGenes { get; set; }
        
        public frmBatchSetGeneName()
        {
            InitializeComponent();
        }

        public frmBatchSetGeneName(IEnumerable<Gene> SelectedGenes)
            : this()
        {
            SetButtonImage(btnSave, DialogButtonPresets.Rename);
            SetButtonImage(btnCancel, DialogButtonPresets.Cancel);

            this.SelectedGenes = SelectedGenes.ToList();
            IEnumerable<string> distinctNames = this.SelectedGenes.Select(g => g.GeneName).Distinct();
            cmbGeneName.DataSource = new BindingSource(distinctNames, null);
            cmbGeneName.Text = distinctNames.First();

            this.FocusOnLoad = cmbGeneName;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cmbGeneName.Text))
            {
                if (Utility.ShowMessage(this.OwnerForm, "Are you sure you want to remove the gene name for all of the selected genes?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                { return; }
            }

            this.SelectedGenes.ForEach(g =>
                {
                    g.GeneName = cmbGeneName.Text.Trim();
                    g.LastUpdatedAt = DateTime.Now;
                    g.LastUpdateSource = GeneSources.User;
                });

            Gene.EditName(this.SelectedGenes.Select(g => g.ID), cmbGeneName.Text.Trim(), GeneSources.User);
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
