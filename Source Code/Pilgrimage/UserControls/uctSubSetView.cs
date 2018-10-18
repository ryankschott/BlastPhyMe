using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChangLab.RecordSets;

namespace Pilgrimage.UserControls
{
    public partial class uctSubSetView : UserControl
    {
        internal uctSubSetsManager Manager { get; set; }

        internal RecordSet CurrentRecordSet { get { return Program.Settings.CurrentRecordSet; } }
        public SubSet CurrentSubSet { get; set; }
        
        protected internal virtual DataGridView RecordsGrid { get { throw new NotImplementedException(); } }
        internal DataGridViewHelper DataGridHelper { get; set; }
        
        public bool Loaded { get; protected internal set; }
        public bool Refreshing { get; set; }

        public int RowCount { get { return RecordsGrid.Rows.Count; } }
        public virtual List<RowDataItem> SelectedRows
        {
            get
            {
                if (RecordsGrid.DataSource == null) { return new List<RowDataItem>(); }
                else
                {
                    return RecordsGrid
                        .Rows
                        .Cast<DataGridViewRow>()
                        .Where(row => ((RowDataItem)row.DataBoundItem).Selected)
                        .Select(row => (RowDataItem)row.DataBoundItem)
                        .ToList();
                }
            }
        }
        public virtual List<RowDataItem> AllRows
        {
            get
            {
                if (RecordsGrid.DataSource == null) { return new List<RowDataItem>(); }
                else
                {
                    return RecordsGrid
                        .Rows
                        .Cast<DataGridViewRow>()
                        .Select(row => (RowDataItem)row.DataBoundItem)
                        .ToList();
                }
            }
        }
        
        public uctSubSetView()
        {
            InitializeComponent();
        }

        public virtual void RefreshRecords(bool RefreshFromDatabase = true, bool SoftRefresh = false)
        {
            throw new NotImplementedException();
        }

        public virtual void RemoveRecords(List<RowDataItem> Records)
        {
            throw new NotImplementedException();
        }
    }
}
