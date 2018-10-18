namespace Pilgrimage.GeneSequences
{
    partial class frmExportToExcel
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmExportToExcel));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnAdd = new System.Windows.Forms.Button();
            this.ilArrows = new System.Windows.Forms.ImageList(this.components);
            this.btnRemove = new System.Windows.Forms.Button();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnDown = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lvSelectedColumns = new System.Windows.Forms.ListView();
            this.clmSelected = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvAvailableColumns = new System.Windows.Forms.ListView();
            this.clmAvailable = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.chkOpenInExcel = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.lvSelectedColumns, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.lvAvailableColumns, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.chkOpenInExcel, 0, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(354, 276);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.btnAdd, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnRemove, 0, 1);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(143, 98);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(34, 68);
            this.tableLayoutPanel2.TabIndex = 4;
            // 
            // btnAdd
            // 
            this.btnAdd.ImageKey = "Right";
            this.btnAdd.ImageList = this.ilArrows;
            this.btnAdd.Location = new System.Drawing.Point(5, 5);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(5);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(24, 24);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // ilArrows
            // 
            this.ilArrows.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilArrows.ImageStream")));
            this.ilArrows.TransparentColor = System.Drawing.Color.Transparent;
            this.ilArrows.Images.SetKeyName(0, "Left");
            this.ilArrows.Images.SetKeyName(1, "Right");
            this.ilArrows.Images.SetKeyName(2, "Up");
            this.ilArrows.Images.SetKeyName(3, "Down");
            // 
            // btnRemove
            // 
            this.btnRemove.ImageKey = "Left";
            this.btnRemove.ImageList = this.ilArrows;
            this.btnRemove.Location = new System.Drawing.Point(5, 39);
            this.btnRemove.Margin = new System.Windows.Forms.Padding(5);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(24, 24);
            this.btnRemove.TabIndex = 1;
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tableLayoutPanel3.AutoSize = true;
            this.tableLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.Controls.Add(this.btnUp, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnDown, 0, 1);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(320, 98);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Size = new System.Drawing.Size(34, 68);
            this.tableLayoutPanel3.TabIndex = 6;
            // 
            // btnUp
            // 
            this.btnUp.ImageKey = "Up";
            this.btnUp.ImageList = this.ilArrows;
            this.btnUp.Location = new System.Drawing.Point(5, 5);
            this.btnUp.Margin = new System.Windows.Forms.Padding(5);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(24, 24);
            this.btnUp.TabIndex = 0;
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnDown
            // 
            this.btnDown.ImageKey = "Down";
            this.btnDown.ImageList = this.ilArrows;
            this.btnDown.Location = new System.Drawing.Point(5, 39);
            this.btnDown.Margin = new System.Windows.Forms.Padding(5);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(24, 24);
            this.btnDown.TabIndex = 1;
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label1, 4);
            this.label1.Location = new System.Drawing.Point(5, 5);
            this.label1.Margin = new System.Windows.Forms.Padding(5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(268, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select the columns to include in the Excel spreadsheet:";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.tableLayoutPanel4.AutoSize = true;
            this.tableLayoutPanel4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel1.SetColumnSpan(this.tableLayoutPanel4, 4);
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.Controls.Add(this.btnExport, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.btnCancel, 1, 0);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(182, 247);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.Size = new System.Drawing.Size(172, 29);
            this.tableLayoutPanel4.TabIndex = 7;
            // 
            // btnExport
            // 
            this.btnExport.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnExport.Location = new System.Drawing.Point(3, 3);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(80, 23);
            this.btnExport.TabIndex = 0;
            this.btnExport.Text = "&Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(89, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // lvSelectedColumns
            // 
            this.lvSelectedColumns.AllowDrop = true;
            this.lvSelectedColumns.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clmSelected});
            this.lvSelectedColumns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvSelectedColumns.FullRowSelect = true;
            this.lvSelectedColumns.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvSelectedColumns.Location = new System.Drawing.Point(180, 44);
            this.lvSelectedColumns.Name = "lvSelectedColumns";
            this.lvSelectedColumns.Size = new System.Drawing.Size(137, 177);
            this.lvSelectedColumns.TabIndex = 5;
            this.lvSelectedColumns.UseCompatibleStateImageBehavior = false;
            this.lvSelectedColumns.View = System.Windows.Forms.View.Details;
            this.lvSelectedColumns.SelectedIndexChanged += new System.EventHandler(this.lstColumns_SelectedIndexChanged);
            this.lvSelectedColumns.DragDrop += new System.Windows.Forms.DragEventHandler(this.lvColumns_DragDrop);
            this.lvSelectedColumns.DragOver += new System.Windows.Forms.DragEventHandler(this.lvColumns_DragOver);
            this.lvSelectedColumns.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lvColumns_MouseDown);
            this.lvSelectedColumns.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lvColumns_MouseMove);
            // 
            // clmSelected
            // 
            this.clmSelected.Text = "Column";
            // 
            // lvAvailableColumns
            // 
            this.lvAvailableColumns.AllowDrop = true;
            this.lvAvailableColumns.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clmAvailable});
            this.lvAvailableColumns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvAvailableColumns.FullRowSelect = true;
            this.lvAvailableColumns.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvAvailableColumns.Location = new System.Drawing.Point(3, 44);
            this.lvAvailableColumns.Name = "lvAvailableColumns";
            this.lvAvailableColumns.Size = new System.Drawing.Size(137, 177);
            this.lvAvailableColumns.TabIndex = 3;
            this.lvAvailableColumns.UseCompatibleStateImageBehavior = false;
            this.lvAvailableColumns.View = System.Windows.Forms.View.Details;
            this.lvAvailableColumns.SelectedIndexChanged += new System.EventHandler(this.lstColumns_SelectedIndexChanged);
            this.lvAvailableColumns.DragDrop += new System.Windows.Forms.DragEventHandler(this.lvColumns_DragDrop);
            this.lvAvailableColumns.DragOver += new System.Windows.Forms.DragEventHandler(this.lvColumns_DragOver);
            this.lvAvailableColumns.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lvColumns_MouseDown);
            this.lvAvailableColumns.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lvColumns_MouseMove);
            // 
            // clmAvailable
            // 
            this.clmAvailable.Text = "Column";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 28);
            this.label2.Margin = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Available:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(182, 28);
            this.label3.Margin = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Selected:";
            // 
            // chkOpenInExcel
            // 
            this.chkOpenInExcel.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.chkOpenInExcel, 4);
            this.chkOpenInExcel.Location = new System.Drawing.Point(5, 227);
            this.chkOpenInExcel.Margin = new System.Windows.Forms.Padding(5, 3, 3, 3);
            this.chkOpenInExcel.Name = "chkOpenInExcel";
            this.chkOpenInExcel.Size = new System.Drawing.Size(168, 17);
            this.chkOpenInExcel.TabIndex = 8;
            this.chkOpenInExcel.Text = "Open in Excel after exporting?";
            this.chkOpenInExcel.UseVisualStyleBackColor = true;
            // 
            // frmExportToExcel
            // 
            this.AcceptButton = this.btnExport;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(354, 276);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(370, 310);
            this.Name = "frmExportToExcel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Export to Excel";
            this.Load += new System.EventHandler(this.frmExportToExcel_Load);
            this.ResizeEnd += new System.EventHandler(this.frmExportToExcel_ResizeEnd);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.ImageList ilArrows;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ListView lvSelectedColumns;
        private System.Windows.Forms.ColumnHeader clmSelected;
        private System.Windows.Forms.ListView lvAvailableColumns;
        private System.Windows.Forms.ColumnHeader clmAvailable;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkOpenInExcel;
    }
}