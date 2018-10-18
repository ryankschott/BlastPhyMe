using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChangLab.Common;
using ChangLab.Jobs;
using ChangLab.Genes;
using ChangLab.RecordSets;
using Pilgrimage.Activities;

namespace Pilgrimage
{
    public class DataGridViewHelper
    {
        internal bool Loaded { get; set; }
        internal bool Editing { get; set; }
        /// <remarks>
        /// This property exists to deal with an issue where pressing the Enter key on a modal dialog form to close it (via the AcceptButton) would
        /// result in the KeyUp event being triggered in the datagrid on the underlying form, possibly because the dialog form was closing just by
        /// KeyDown event on its button.  The KeyDown property is set on the DataGrid_KeyDown event so that DataGrid_KeyUp can check to make sure
        /// that a key was pressed down within the focus of the DataGrid.  DataGrid_KeyUp will reset this property to None.
        /// </remarks>
        private Keys KeyDown { get; set; }

        internal static string DefaultDateTimeFormatString { get { return "yyyy-MM-dd hh:mm tt"; } }
        internal static string DateTimeFormatStringWithSeconds { get { return "yyyy-MM-dd hh:mm:ss tt"; } }
        internal static string DefaultDoubleFormatString { get { return "0.00"; } }

        internal DataSourceTypes DataSourceType { get; set; }
        public enum DataSourceTypes
        {
            Other = 0,
            Genes = 1,
            BLASTNResultsHistory = 2,
            Taxa = 3
        }
        internal bool AllowViewDetails { get; set; }

        private Form ParentForm { get; set; }
        private DataGridView DataGrid { get; set; }
        private ContextMenuStrip ContextMenu { get; set; }
        private CheckBox ToggleAllCheckBox { get; set; }

        private List<DataGridViewColumn> DateTimeColumns { get; set; }

        #region Short-Term Key Press Disabler
        private Timer KeyPressEnabler { get; set; }
        private bool KeyPressDisabled { get; set; }

        /// <summary>
        /// Disables the key-press events for the DataGrid for 100 milliseconds to avoid issues where pressing a key on a modal dialog passes through
        /// to the underlying form and triggers and event on its grid.
        /// </summary>
        internal void ShortTermDisableKeyPress()
        {
            if (KeyPressEnabler == null)
            {
                KeyPressEnabler = new Timer();
                KeyPressEnabler.Tick += new EventHandler(KeyPressEnabler_Tick);
            }

            KeyPressDisabled = true;
            KeyPressEnabler.Start();
        }

        void KeyPressEnabler_Tick(object sender, EventArgs e)
        {
            KeyPressEnabler.Stop();
            KeyPressDisabled = false;
        }
        #endregion

        private int _urlColumnIndex = -1;
        internal int URLColumnIndex
        {
            get
            {
                if (_urlColumnIndex == -1)
                {
                    DataGridViewColumn urlColumn = DataGrid.Columns.Cast<DataGridViewColumn>().FirstOrDefault(clm => clm.GetType() == typeof(DataGridViewLinkColumn)
                                                                                                                        && clm.DataPropertyName.ToLower().Contains(""));
                    if (urlColumn != null)
                    {
                        _urlColumnIndex = urlColumn.Index;
                    }
                }
                return _urlColumnIndex;
            }
        }

        private int _lengthColumnIndex = -1;
        private int LengthColumnIndex
        {
            get
            {
                if (_lengthColumnIndex == -1)
                {
                    DataGridViewColumn column = DataGrid.Columns.Cast<DataGridViewColumn>().FirstOrDefault(clm => clm.GetType() == typeof(DataGridViewTextBoxColumn)
                                                                                                                        && clm.DataPropertyName == "Length");
                    if (column != null)
                    {
                        _lengthColumnIndex = column.Index;
                    }
                }
                return _lengthColumnIndex;
            }
        }

        internal DataGridViewHelper(Form ParentForm, DataGridView DataGrid, CheckBox ToggleAllCheckBox, DataSourceTypes DataSourceType = DataSourceTypes.Genes, bool AllowViewDetails = true, ContextMenuStrip CustomContextMenu = null)
        {
            this.DateTimeColumns = new List<DataGridViewColumn>();

            this.ParentForm = ParentForm;
            this.DataGrid = DataGrid;
            this.DataGrid.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(DataGrid_DataBindingComplete);
            this.DataGrid.Columns.Cast<DataGridViewColumn>().Where(clm => clm.Name.EndsWith("StartedAt") || clm.Name.EndsWith("EndedAt")).ToList()
                .ForEach(clm => ConfigureDateTimeColumn(clm));

            if (ToggleAllCheckBox != null)
            {
                this.ToggleAllCheckBox = ToggleAllCheckBox;
                this.ToggleAllCheckBox.CheckedChanged += new EventHandler(ToggleAllCheckBox_CheckedChanged);
            }

            this.DataSourceType = DataSourceType;
            this.AllowViewDetails = AllowViewDetails;

            if (this.AllowViewDetails)
            {
                if (CustomContextMenu != null)
                {
                    this.ContextMenu = CustomContextMenu;

                    ToolStripMenuItem detailsMenuItem = this.ContextMenu.Items.OfType<ToolStripMenuItem>().SingleOrDefault(item => item.Tag.ToSafeString() == "Details");
                    if (detailsMenuItem != null) { detailsMenuItem.Click += new EventHandler(detailsMenuItem_Click); }
                }

                if (this.ContextMenu == null)
                {
                    this.ContextMenu = new ContextMenuStrip();
                    ToolStripMenuItem detailsMenuItem = new ToolStripMenuItem("&Details");
                    detailsMenuItem.Click += new EventHandler(detailsMenuItem_Click);
                    this.ContextMenu.Items.Add(detailsMenuItem);
                }

                switch (this.DataSourceType)
                {
                    case DataSourceTypes.Other:
                    case DataSourceTypes.Genes:
                    case DataSourceTypes.Taxa:
                    case DataSourceTypes.BLASTNResultsHistory:
                        this.DataGrid.CellMouseDoubleClick += new DataGridViewCellMouseEventHandler(DataGrid_CellMouseDoubleClick);
                        this.DataGrid.KeyDown += new KeyEventHandler(DataGrid_KeyDown);
                        if (SelectedColumnIndex == -1) { this.DataGrid.KeyUp += new KeyEventHandler(DataGrid_KeyUp); }
                        this.DataGrid.MouseUp += new System.Windows.Forms.MouseEventHandler(DataGrid_MouseUp);
                        break;
                }
            }

            if (SelectedColumnIndex != -1)
            {
                ((DataGridViewCheckBoxColumn)this.DataGrid.Columns[SelectedColumnIndex]).TrueValue = true;
                ((DataGridViewCheckBoxColumn)this.DataGrid.Columns[SelectedColumnIndex]).FalseValue = false;
                ((DataGridViewCheckBoxColumn)this.DataGrid.Columns[SelectedColumnIndex]).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                this.DataGrid.CellContentClick += new DataGridViewCellEventHandler(DataGrid_CellContentClick);
                this.DataGrid.KeyUp += new KeyEventHandler(DataGrid_KeyUp);
                this.DataGrid.CellLeave += new DataGridViewCellEventHandler(DataGrid_CellLeave);
            }
            else if (URLColumnIndex != -1)
            {
                this.DataGrid.CellContentClick += new DataGridViewCellEventHandler(DataGrid_CellContentClick);
            }
        }

        internal DataGridViewHelper(Form ParentForm, DataGridView DataGrid, DataSourceTypes DataSourceType = DataSourceTypes.Genes, bool AllowViewDetails = true) : this(ParentForm, DataGrid, null, DataSourceType, AllowViewDetails) { }

        private void DataGrid_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            this.DataGrid.Columns
                .Cast<DataGridViewColumn>()
                .Where(clm => !(clm.GetType() == typeof(DataGridViewCheckBoxColumn) && clm.DataPropertyName == "Selected"))
                .ToList()
                .ForEach(clm => clm.SortMode = DataGridViewColumnSortMode.Automatic);

            this.SetToggleAllCheckBoxText();
        }

        #region Selected Column
        private int _selectedColumnIndex = -1;
        public int SelectedColumnIndex
        {
            get
            {
                if (_selectedColumnIndex == -1)
                {
                    DataGridViewColumn selectedColumn = DataGrid.Columns.Cast<DataGridViewColumn>().FirstOrDefault(clm => clm.GetType() == typeof(DataGridViewCheckBoxColumn)
                                                                                                                              && clm.DataPropertyName == "Selected");
                    if (selectedColumn != null)
                    {
                        _selectedColumnIndex = selectedColumn.Index;
                    }
                }
                return _selectedColumnIndex;
            }
        }

        private string ToggleAll_SelectAll = "Select all";
        private string ToggleAll_DeselectAll = "Deselect all";
        
        private void DataGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (Editing) { return; }

            if (e.RowIndex != -1 && e.ColumnIndex != -1)
            {
                if (e.ColumnIndex == SelectedColumnIndex)
                {
                    RowDataItem result = (RowDataItem)DataGrid.Rows[e.RowIndex].DataBoundItem;
                    result.Selected = !result.Selected;

                    DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)DataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    chk.Value = result.Selected;

                    OnSelectedRowsChanged(new SelectedRowEventArgs(result));
                    if (this.ToggleAllCheckBox != null) { ToggleToggleAllCheckBox(); }

                    DataGrid.EndEdit();
                }
                else if (DataGrid.Columns[e.ColumnIndex].GetType() == typeof(DataGridViewCheckBoxColumn))
                {
                    // Any editable check-box column except for Selected
                    OnCheckBoxChanged(new SelectedRowEventArgs((RowDataItem)DataGrid.Rows[e.RowIndex].DataBoundItem, (DataGridViewCheckBoxCell)DataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex]));
                    DataGrid.EndEdit();
                }

                if (e.ColumnIndex == URLColumnIndex)
                {
                    object url = DataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
                    System.Diagnostics.Process.Start(url.ToSafeString());
                }
            }
        }

        private void DataGrid_KeyUp(object sender, KeyEventArgs e)
        {
            if (this.KeyPressDisabled) { return; }

            if (DataGrid.SelectedRows.Count != 0 && !(e.Alt || e.Control || e.Shift) && this.KeyDown != Keys.None)
            {
                if (e.KeyCode == Keys.Space)
                {
                    this.KeyDown = Keys.None;
                    Editing = true;

                    bool allTheSame = DataGrid.SelectedRows.Cast<DataGridViewRow>().All(row => ((RowDataItem)row.DataBoundItem).Selected)
                                        || DataGrid.SelectedRows.Cast<DataGridViewRow>().All(row => !((RowDataItem)row.DataBoundItem).Selected);

                    bool newValue = false;
                    if (allTheSame)
                    {
                        // Set all values to be the opposite of whatever they currently are.
                        newValue = !((RowDataItem)DataGrid.SelectedRows.Cast<DataGridViewRow>().First().DataBoundItem).Selected;
                    }
                    else
                    {
                        // When there's a mix of selected and not selected, select everything.
                        newValue = true;
                    }

                    DataGrid.SelectedRows.Cast<DataGridViewRow>().ToList().ForEach(row =>
                    {
                        RowDataItem result = (RowDataItem)row.DataBoundItem;
                        result.Selected = newValue;
                        OnSelectedRowsChanged(new SelectedRowEventArgs(result));

                        row.Selected = false; row.Selected = true;
                    });

                    if (ToggleAllCheckBox != null) { ToggleToggleAllCheckBox(); }
                }

                if (e.KeyCode == Keys.Enter && this.AllowViewDetails)
                {
                    this.KeyDown = Keys.None;

                    OnViewDetails(new ViewDetailsEventArgs(DataGrid.SelectedRows[0]));
                }
            }
        }

        private void DataGrid_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (Editing) { Editing = false; }
        }

        #region Toggle All CheckBox
        internal void ToggleToggleAllCheckBox()
        {
            bool allSelected = (this.DataGrid.Rows.Cast<DataGridViewRow>().All(row => (bool)row.Cells[this.SelectedColumnIndex].Value));
            this.ToggleAllCheckBox.CheckedChanged -= ToggleAllCheckBox_CheckedChanged;
            this.ToggleAllCheckBox.Checked = allSelected;
            SetToggleAllCheckBoxText();
            this.ToggleAllCheckBox.CheckedChanged += new EventHandler(ToggleAllCheckBox_CheckedChanged);
        }

        private void ToggleAllCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in this.DataGrid.Rows)
            {
                row.Cells[this.SelectedColumnIndex].Value = this.ToggleAllCheckBox.Checked;
            }

            SetToggleAllCheckBoxText();
            OnSelectedRowsChanged(new SelectedRowEventArgs(null));
        }

        internal void SetToggleAllCheckBoxText()
        {
            if (this.ToggleAllCheckBox != null)
            {
                this.ToggleAllCheckBox.Text = (this.ToggleAllCheckBox.Checked ? ToggleAll_DeselectAll : ToggleAll_SelectAll);
                this.ToggleAllCheckBox.Text += " " + this.DataGrid.Rows.Count.ToString("N0") + " rows";
            }
        }
        #endregion

        protected virtual void OnSelectedRowsChanged(SelectedRowEventArgs e) { if (SelectedRowsChanged != null) { SelectedRowsChanged(e); } }
        public event SelectedRowsChangedEventHandler SelectedRowsChanged;

        protected virtual void OnCheckBoxChanged(SelectedRowEventArgs e) { if (CheckBoxChanged != null) { CheckBoxChanged(e); } }
        public event SelectedRowsChangedEventHandler CheckBoxChanged;

        public delegate void SelectedRowsChangedEventHandler(SelectedRowEventArgs e);
        public class SelectedRowEventArgs : EventArgs
        {
            public RowDataItem UpdatedRow { get; set; }
            public DataGridViewCheckBoxCell CheckBox { get; set; }

            public SelectedRowEventArgs(RowDataItem UpdatedRow) : this(UpdatedRow, null) { }

            public SelectedRowEventArgs(RowDataItem UpdatedRow, DataGridViewCheckBoxCell CheckBox)
            {
                this.UpdatedRow = UpdatedRow;
                this.CheckBox = CheckBox;
            }
        }
        #endregion

        #region Context Menu
        private void DataGrid_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left
                && e.RowIndex != -1)
            {
                OnViewDetails(new ViewDetailsEventArgs(DataGrid.Rows[e.RowIndex]));
            }
        }

        /// <remarks>
        /// As-is, courtesy of Mr. Kiddo: http://social.msdn.microsoft.com/Forums/en-US/1bbb07c2-d368-4443-ac2f-04c6138d965c/context-menu-on-a-datagridview
        /// </remarks>
        private void DataGrid_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                DataGridView.HitTestInfo hit = DataGrid.HitTest(e.X, e.Y);

                // Only RowHeader and Cells (which make up a row) should fire a menu
                if (hit.Type == DataGridViewHitTestType.RowHeader
                    || hit.Type == DataGridViewHitTestType.Cell
                    && hit.RowIndex >= 0)
                {
                    // Notice how I assign the menu to the dataGridView below...

                    DataGrid.ClearSelection();
                    DataGrid.Rows[hit.RowIndex].Selected = true;
                    DataGrid.ContextMenuStrip = ContextMenu;
                    DataGrid.ContextMenuStrip.Show(DataGrid, new Point(e.X, e.Y));
                }

                // and then I remove the menu here...
                // This is because after we assign it to the dataGridView
                // it allows you to right-click anywhere on the control to
                // get a menu.  Try removing this line and expirement.
                DataGrid.ContextMenuStrip = null;
            }
        }

        /// <summary>
        /// Suppresses the default behavior of shifting to the next row so that KeyUp can pop-up the details instead.
        /// </summary>
        private void DataGrid_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.KeyPressDisabled) { return; }
            this.KeyDown = e.KeyCode;

            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
            }
        }

        internal void detailsMenuItem_Click(object sender, EventArgs e)
        {
            OnViewDetails(new ViewDetailsEventArgs(DataGrid.SelectedRows[0]));
        }

        protected virtual void OnViewDetails(ViewDetailsEventArgs e)
        {
            if (ViewDetails != null)
            {
                ViewDetails(e);
            }
        }
        public delegate void ViewDetailsEventHandler(ViewDetailsEventArgs e);
        public event ViewDetailsEventHandler ViewDetails;

        public class ViewDetailsEventArgs : EventArgs
        {
            public DataGridViewRow Row { get; set; }
            public object Args { get; set; }
            public bool Cancel { get; set; }

            public ViewDetailsEventArgs(DataGridViewRow Row)
            {
                this.Row = Row;
                this.Cancel = false;
            }
        }
        #endregion

        #region Cell Formatting
        private void DataGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (LengthColumnIndex != -1
                && e.ColumnIndex == LengthColumnIndex
                && e.RowIndex != -1)
            {
                if (e.Value != null && e.Value.GetType() == typeof(int) && (int)e.Value == 0)
                {
                    e.Value = string.Empty;
                }
            }

            if (this.DateTimeColumns.Any(col => col.Index == e.ColumnIndex))
            {
                if (e.Value != null && (DateTime)e.Value == DateTime.MinValue)
                {
                    e.Value = null;
                }
            }
        }

        internal void ConfigureDateTimeColumn(DataGridViewColumn Column)
        {
            Column.DefaultCellStyle.Format = DataGridViewHelper.DefaultDateTimeFormatString;
            Column.DefaultCellStyle.NullValue = string.Empty;

            if (!this.DateTimeColumns.Contains(Column)) { this.DateTimeColumns.Add(Column); }
        }
        #endregion
    }

    public abstract class RowDataItem
    {
        public bool Selected { get; set; }
        public abstract string ID { get; }
    }

    public delegate void DataGridViewRowsActionEventHandler(DataGridViewRowsActionEventArgs e);
    public class DataGridViewRowsActionEventArgs : EventArgs
    {
        public List<DataGridViewRow> Rows { get; set; }
        public bool Cancel { get; set; }

        public DataGridViewRowsActionEventArgs(List<DataGridViewRow> Rows)
        {
            this.Rows = Rows;
            this.Cancel = false;
        }
    }
}
