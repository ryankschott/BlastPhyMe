using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChangLab.PAML.CodeML;
using Pilgrimage.Activities;

namespace Pilgrimage.PAML
{
    public partial class uctJobConfigurations : UserControl
    {
        private DataGridViewHelper DataGridHelper { get; set; }
        internal bool DataSourceSet { get { return grdConfiguration.DataSource != null; } }

        public uctJobConfigurations()
        {
            InitializeComponent();

            this.DataGridHelper = new DataGridViewHelper(this.ParentForm, grdConfiguration, DataGridViewHelper.DataSourceTypes.Other);
            this.DataGridHelper.ViewDetails += new DataGridViewHelper.ViewDetailsEventHandler(DataGridHelper_ViewDetails);
            clmKappa.DefaultCellStyle.Format = DataGridViewHelper.DefaultDoubleFormatString;
            clmOmega.DefaultCellStyle.Format = DataGridViewHelper.DefaultDoubleFormatString;
        }

        public void Refresh(List<Tree> Trees)
        {
            this.DataGridHelper.Loaded = false;
            grdConfiguration.AutoGenerateColumns = false;
            grdConfiguration.DataSource = null;
            grdConfiguration.DataSource = new SortableBindingList<TreeConfigurationRowDataItem>(Trees.ToRowDataItemList());
            this.DataGridHelper.Loaded = true;
        }

        private void DataGridHelper_ViewDetails(DataGridViewHelper.ViewDetailsEventArgs e)
        {
            using (frmEditTreeConfiguration frm = new frmEditTreeConfiguration(((TreeConfigurationRowDataItem)e.Row.DataBoundItem).Tree, null, false))
            {
                frm.ShowDialog();
            }
        }
    }
}
