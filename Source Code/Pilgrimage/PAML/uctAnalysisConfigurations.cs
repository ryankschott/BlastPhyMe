using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChangLab.Common;
using ChangLab.PAML.CodeML;

namespace Pilgrimage.PAML
{
    public partial class uctAnalysisConfigurations : UserControl
    {
        internal List<AnalysisConfiguration> Configurations
        {
            get
            {
                return DataSource.Select(row => row.Configuration).ToList();
            }
        }
        internal SortableBindingList<AnalysisConfigurationRowDataItem> DataSource { get; set; }
        private DataGridViewHelper DataGridHelper { get; set; }

        public uctAnalysisConfigurations()
        {
            InitializeComponent();
        }

        public void Initialize(List<AnalysisConfiguration> Configurations)
        {
            this.DataSource = new SortableBindingList<AnalysisConfigurationRowDataItem>(Configurations.ToRowDataItemList());
            this.DataGridHelper = new DataGridViewHelper(this.ParentForm, grdConfigurations, DataGridViewHelper.DataSourceTypes.Other, false);

            this.DataGridHelper.Loaded = false;
            grdConfigurations.AutoGenerateColumns = false;
            grdConfigurations.DataSource = null;
            grdConfigurations.DataSource = DataSource;
            this.DataGridHelper.Loaded = true;
        }

        internal void Add()
        {
            EditConfiguration(null, null);
        }

        internal void Edit()
        {
            if (grdConfigurations.SelectedRows.Count == 0)
            {
                Utility.ShowMessage(this.ParentForm, "Please select a configuration to edit.");
                return;
            }
            else
            {
                DataGridViewRow row = grdConfigurations.SelectedRows[0];
                EditConfiguration(((AnalysisConfigurationRowDataItem)row.DataBoundItem).Configuration, row);
            }
        }

        internal void Copy()
        {
            if (grdConfigurations.SelectedRows.Count == 0)
            {
                Utility.ShowMessage(this.ParentForm, "Please select a configuration to copy.");
                return;
            }
            else
            {
                EditConfiguration(((AnalysisConfigurationRowDataItem)grdConfigurations.SelectedRows[0].DataBoundItem).Configuration.Copy(), null);
            }
        }

        internal void Remove()
        {
            if (grdConfigurations.SelectedRows.Count == 0)
            {
                Utility.ShowMessage(this.ParentForm, "Please select a configuration to remove.");
                return;
            }
            else
            {
                if (Utility.ShowMessage(this.Parent, "Are you sure you want to remove this configuration?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DataSource.Remove((AnalysisConfigurationRowDataItem)grdConfigurations.SelectedRows[0].DataBoundItem);
                }
                grdConfigurations.Focus();
            }
        }

        private void SelectConfiguration(AnalysisConfigurationRowDataItem Configuration)
        {
            grdConfigurations.SelectedRows.Cast<DataGridViewRow>().ToList().ForEach(row => row.Selected = false);
            grdConfigurations.Rows.Cast<DataGridViewRow>().First(row => (AnalysisConfigurationRowDataItem)row.DataBoundItem == Configuration).Selected = true;
        }

        internal void EditConfiguration(AnalysisConfiguration Configuration, DataGridViewRow Row)
        {
            using (frmEditAnalysisConfiguration frm = new frmEditAnalysisConfiguration(Configuration))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    AnalysisConfigurationRowDataItem editedConfiguration = null;
                    if (Row == null)
                    {
                        editedConfiguration = new AnalysisConfigurationRowDataItem(frm.Configuration);
                        DataSource.Add(editedConfiguration);
                    }
                    else
                    {
                        editedConfiguration = (AnalysisConfigurationRowDataItem)Row.DataBoundItem;
                        editedConfiguration.Configuration = frm.Configuration;
                    }

                    SelectConfiguration(editedConfiguration);
                }

                grdConfigurations.Focus();
            }
        }
    }
}
