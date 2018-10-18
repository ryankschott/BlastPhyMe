namespace Pilgrimage.GeneSequences
{
    partial class frmImportFromFlatFile
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lnkFormatString = new System.Windows.Forms.LinkLabel();
            this.grdResults = new System.Windows.Forms.DataGridView();
            this.clmSelected_Committed = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.clmHeader = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmOrganism = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmLocus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmDefinition = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmLength = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmInRecordSet = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.label2 = new System.Windows.Forms.Label();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.btnLoadFromFile = new System.Windows.Forms.Button();
            this.btnBrowseForFile = new System.Windows.Forms.Button();
            this.lblHeaderFormat = new System.Windows.Forms.Label();
            this.txtFormatString = new System.Windows.Forms.TextBox();
            this.lblSelectedRows = new System.Windows.Forms.Label();
            this.chkToggleSelected = new System.Windows.Forms.CheckBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.cmbSubSets = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdResults)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.btnCancel, 3, 6);
            this.tableLayoutPanel1.Controls.Add(this.lnkFormatString, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.grdResults, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtFilePath, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnLoadFromFile, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnBrowseForFile, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblHeaderFormat, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtFormatString, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblSelectedRows, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.chkToggleSelected, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnSave, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.cmbSubSets, 1, 6);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(884, 566);
            this.tableLayoutPanel1.TabIndex = 11;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.tableLayoutPanel1.SetColumnSpan(this.btnCancel, 2);
            this.btnCancel.Location = new System.Drawing.Point(801, 539);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 24);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "&Close";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lnkFormatString
            // 
            this.lnkFormatString.AutoSize = true;
            this.lnkFormatString.Location = new System.Drawing.Point(101, 61);
            this.lnkFormatString.Margin = new System.Windows.Forms.Padding(5);
            this.lnkFormatString.Name = "lnkFormatString";
            this.lnkFormatString.Size = new System.Drawing.Size(90, 13);
            this.lnkFormatString.TabIndex = 2;
            this.lnkFormatString.TabStop = true;
            this.lnkFormatString.Text = "Build format string";
            this.lnkFormatString.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkFormatString_LinkClicked);
            // 
            // grdResults
            // 
            this.grdResults.AllowUserToAddRows = false;
            this.grdResults.AllowUserToDeleteRows = false;
            this.grdResults.AllowUserToResizeRows = false;
            this.grdResults.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grdResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdResults.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmSelected_Committed,
            this.clmHeader,
            this.clmOrganism,
            this.clmLocus,
            this.clmDefinition,
            this.clmLength,
            this.clmInRecordSet});
            this.tableLayoutPanel1.SetColumnSpan(this.grdResults, 5);
            this.grdResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdResults.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.grdResults.Location = new System.Drawing.Point(3, 111);
            this.grdResults.Name = "grdResults";
            this.grdResults.ReadOnly = true;
            this.grdResults.RowHeadersVisible = false;
            this.grdResults.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdResults.Size = new System.Drawing.Size(878, 399);
            this.grdResults.TabIndex = 7;
            // 
            // clmSelected_Committed
            // 
            this.clmSelected_Committed.DataPropertyName = "Selected";
            this.clmSelected_Committed.FillWeight = 3F;
            this.clmSelected_Committed.HeaderText = "";
            this.clmSelected_Committed.Name = "clmSelected_Committed";
            this.clmSelected_Committed.ReadOnly = true;
            // 
            // clmHeader
            // 
            this.clmHeader.DataPropertyName = "Header";
            this.clmHeader.FillWeight = 32F;
            this.clmHeader.HeaderText = "Header from file";
            this.clmHeader.Name = "clmHeader";
            this.clmHeader.ReadOnly = true;
            // 
            // clmOrganism
            // 
            this.clmOrganism.DataPropertyName = "Organism";
            this.clmOrganism.FillWeight = 16F;
            this.clmOrganism.HeaderText = "Organism";
            this.clmOrganism.Name = "clmOrganism";
            this.clmOrganism.ReadOnly = true;
            // 
            // clmLocus
            // 
            this.clmLocus.DataPropertyName = "Locus";
            this.clmLocus.FillWeight = 16F;
            this.clmLocus.HeaderText = "Locus (Contig #)";
            this.clmLocus.Name = "clmLocus";
            this.clmLocus.ReadOnly = true;
            this.clmLocus.Visible = false;
            // 
            // clmDefinition
            // 
            this.clmDefinition.DataPropertyName = "Definition";
            this.clmDefinition.FillWeight = 32F;
            this.clmDefinition.HeaderText = "Definition";
            this.clmDefinition.Name = "clmDefinition";
            this.clmDefinition.ReadOnly = true;
            // 
            // clmLength
            // 
            this.clmLength.DataPropertyName = "Length";
            dataGridViewCellStyle3.Format = "N0";
            this.clmLength.DefaultCellStyle = dataGridViewCellStyle3;
            this.clmLength.FillWeight = 10F;
            this.clmLength.HeaderText = "Length";
            this.clmLength.Name = "clmLength";
            this.clmLength.ReadOnly = true;
            // 
            // clmInRecordSet
            // 
            this.clmInRecordSet.DataPropertyName = "InRecordSet";
            this.clmInRecordSet.FillWeight = 7F;
            this.clmInRecordSet.HeaderText = "Imported";
            this.clmInRecordSet.Name = "clmInRecordSet";
            this.clmInRecordSet.ReadOnly = true;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 8);
            this.label2.Margin = new System.Windows.Forms.Padding(5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "FASTA file:";
            // 
            // txtFilePath
            // 
            this.txtFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.txtFilePath, 3);
            this.txtFilePath.Location = new System.Drawing.Point(99, 5);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.Size = new System.Drawing.Size(752, 20);
            this.txtFilePath.TabIndex = 1;
            // 
            // btnLoadFromFile
            // 
            this.btnLoadFromFile.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.tableLayoutPanel1.SetColumnSpan(this.btnLoadFromFile, 3);
            this.btnLoadFromFile.Location = new System.Drawing.Point(766, 59);
            this.btnLoadFromFile.Name = "btnLoadFromFile";
            this.btnLoadFromFile.Size = new System.Drawing.Size(115, 24);
            this.btnLoadFromFile.TabIndex = 3;
            this.btnLoadFromFile.Text = "&Load from File";
            this.btnLoadFromFile.UseVisualStyleBackColor = true;
            this.btnLoadFromFile.Click += new System.EventHandler(this.btnLoadFromFile_Click);
            // 
            // btnBrowseForFile
            // 
            this.btnBrowseForFile.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnBrowseForFile.Location = new System.Drawing.Point(857, 3);
            this.btnBrowseForFile.Name = "btnBrowseForFile";
            this.btnBrowseForFile.Size = new System.Drawing.Size(24, 24);
            this.btnBrowseForFile.TabIndex = 3;
            this.btnBrowseForFile.Text = "...";
            this.btnBrowseForFile.UseVisualStyleBackColor = true;
            this.btnBrowseForFile.Click += new System.EventHandler(this.btnBrowseForFile_Click);
            // 
            // lblHeaderFormat
            // 
            this.lblHeaderFormat.AutoSize = true;
            this.lblHeaderFormat.Location = new System.Drawing.Point(5, 35);
            this.lblHeaderFormat.Margin = new System.Windows.Forms.Padding(5);
            this.lblHeaderFormat.Name = "lblHeaderFormat";
            this.lblHeaderFormat.Size = new System.Drawing.Size(77, 13);
            this.lblHeaderFormat.TabIndex = 0;
            this.lblHeaderFormat.Text = "Header format:";
            // 
            // txtFormatString
            // 
            this.txtFormatString.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.txtFormatString, 4);
            this.txtFormatString.Location = new System.Drawing.Point(99, 33);
            this.txtFormatString.Name = "txtFormatString";
            this.txtFormatString.Size = new System.Drawing.Size(782, 20);
            this.txtFormatString.TabIndex = 1;
            // 
            // lblSelectedRows
            // 
            this.lblSelectedRows.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblSelectedRows.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblSelectedRows, 2);
            this.lblSelectedRows.Location = new System.Drawing.Point(5, 518);
            this.lblSelectedRows.Margin = new System.Windows.Forms.Padding(5);
            this.lblSelectedRows.Name = "lblSelectedRows";
            this.lblSelectedRows.Size = new System.Drawing.Size(95, 13);
            this.lblSelectedRows.TabIndex = 12;
            this.lblSelectedRows.Text = "# records selected";
            // 
            // chkToggleSelected
            // 
            this.chkToggleSelected.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkToggleSelected.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.chkToggleSelected, 2);
            this.chkToggleSelected.Location = new System.Drawing.Point(5, 91);
            this.chkToggleSelected.Margin = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.chkToggleSelected.Name = "chkToggleSelected";
            this.chkToggleSelected.Size = new System.Drawing.Size(70, 17);
            this.chkToggleSelected.TabIndex = 13;
            this.chkToggleSelected.Text = "Select All";
            this.chkToggleSelected.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnSave.Location = new System.Drawing.Point(3, 539);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(90, 24);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "&Import to:";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // cmbSubSets
            // 
            this.cmbSubSets.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cmbSubSets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSubSets.FormattingEnabled = true;
            this.cmbSubSets.Location = new System.Drawing.Point(99, 540);
            this.cmbSubSets.Name = "cmbSubSets";
            this.cmbSubSets.Size = new System.Drawing.Size(200, 21);
            this.cmbSubSets.TabIndex = 1;
            // 
            // frmImportFromFASTA
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(884, 566);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(800, 400);
            this.Name = "frmImportFromFASTA";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Import from FASTA";
            this.Load += new System.EventHandler(this.frmImportFromFASTA_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdResults)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblHeaderFormat;
        private System.Windows.Forms.TextBox txtFormatString;
        private System.Windows.Forms.LinkLabel lnkFormatString;
        private System.Windows.Forms.Button btnLoadFromFile;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ComboBox cmbSubSets;
        private System.Windows.Forms.DataGridView grdResults;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.Button btnBrowseForFile;
        private System.Windows.Forms.Label lblSelectedRows;
        private System.Windows.Forms.CheckBox chkToggleSelected;
        private System.Windows.Forms.DataGridViewCheckBoxColumn clmSelected_Committed;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmHeader;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmOrganism;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmLocus;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmDefinition;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmLength;
        private System.Windows.Forms.DataGridViewCheckBoxColumn clmInRecordSet;

    }
}