using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChangLab.Common;
using ChangLab.Genes;

namespace Pilgrimage.GeneSequences.EditNucleotideSequence
{
    public partial class frmAddFeature : DialogForm
    {
        public ReferenceItem2<GeneFeatureKeys> SelectedKey { get { return (ReferenceItem2<GeneFeatureKeys>)cmbFeatureKeys.SelectedItem; } }

        public frmAddFeature()
        {
            InitializeComponent();

            SetButtonImage(btnSave, "Add");
            SetButtonImage(btnCancel, "Cancel");
        }

        public frmAddFeature(List<Feature> ExistingFeatures) : this()
        {
            List<ReferenceItem2<GeneFeatureKeys>> allKeys = GeneFeatureKeyCollection.ListAll();
            List<ReferenceItem2<GeneFeatureKeys>> allUnusedKeys =
                allKeys.Join(allKeys.Select(fk => fk.Key).Except(ExistingFeatures.Select(ef => ef.FeatureKey.Key)),
                                fk => fk.Key,
                                ef => ef,
                                (fk, ef) => fk)
                        .OrderBy(fk => fk.Name)
                        .ToList();

            cmbFeatureKeys.ValueMember = "ID";
            cmbFeatureKeys.DisplayMember = "Name";
            cmbFeatureKeys.DataSource = new BindingSource(allUnusedKeys, null);
            cmbFeatureKeys.SelectedIndex = 0;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
