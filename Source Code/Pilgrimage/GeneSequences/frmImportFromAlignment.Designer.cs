namespace Pilgrimage.GeneSequences
{
    partial class frmImportFromAlignment
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmImportFromAlignment));
            this.btnCancel = new System.Windows.Forms.Button();
            this.tblForm = new System.Windows.Forms.TableLayoutPanel();
            this.grdImport = new System.Windows.Forms.DataGridView();
            this.clmSourceName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmGeneName = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.clmMatchPercentage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.txtFormatString = new System.Windows.Forms.TextBox();
            this.lnkFormatString = new System.Windows.Forms.LinkLabel();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnImport = new System.Windows.Forms.Button();
            this.cmbSubSets = new System.Windows.Forms.ComboBox();
            this.lblInstructions = new System.Windows.Forms.Label();
            this.lnkInstructions = new System.Windows.Forms.LinkLabel();
            this.tblForm.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdImport)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(801, 539);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 24);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // tblForm
            // 
            this.tblForm.ColumnCount = 5;
            this.tblForm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tblForm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tblForm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblForm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tblForm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tblForm.Controls.Add(this.btnCancel, 4, 5);
            this.tblForm.Controls.Add(this.grdImport, 0, 4);
            this.tblForm.Controls.Add(this.label1, 0, 2);
            this.tblForm.Controls.Add(this.txtFormatString, 2, 2);
            this.tblForm.Controls.Add(this.lnkFormatString, 2, 3);
            this.tblForm.Controls.Add(this.btnRefresh, 4, 3);
            this.tblForm.Controls.Add(this.tableLayoutPanel2, 0, 5);
            this.tblForm.Controls.Add(this.lblInstructions, 0, 0);
            this.tblForm.Controls.Add(this.lnkInstructions, 2, 1);
            this.tblForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblForm.Location = new System.Drawing.Point(0, 0);
            this.tblForm.Name = "tblForm";
            this.tblForm.RowCount = 6;
            this.tblForm.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblForm.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblForm.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblForm.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblForm.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblForm.Size = new System.Drawing.Size(884, 566);
            this.tblForm.TabIndex = 0;
            // 
            // grdImport
            // 
            this.grdImport.AllowUserToAddRows = false;
            this.grdImport.AllowUserToDeleteRows = false;
            this.grdImport.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grdImport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdImport.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmSourceName,
            this.clmGeneName,
            this.clmMatchPercentage});
            this.tblForm.SetColumnSpan(this.grdImport, 5);
            this.grdImport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdImport.Location = new System.Drawing.Point(3, 220);
            this.grdImport.Name = "grdImport";
            this.grdImport.RowHeadersVisible = false;
            this.grdImport.Size = new System.Drawing.Size(878, 313);
            this.grdImport.TabIndex = 4;
            this.grdImport.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.grdImport_DataBindingComplete);
            // 
            // clmSourceName
            // 
            this.clmSourceName.DataPropertyName = "Definition";
            this.clmSourceName.FillWeight = 47F;
            this.clmSourceName.HeaderText = "Imported Gene Sequence From File";
            this.clmSourceName.Name = "clmSourceName";
            this.clmSourceName.ReadOnly = true;
            // 
            // clmGeneName
            // 
            this.clmGeneName.FillWeight = 47F;
            this.clmGeneName.HeaderText = "Originally Exported Gene Sequence";
            this.clmGeneName.Name = "clmGeneName";
            // 
            // clmMatchPercentage
            // 
            dataGridViewCellStyle1.Format = "P0";
            this.clmMatchPercentage.DefaultCellStyle = dataGridViewCellStyle1;
            this.clmMatchPercentage.FillWeight = 6F;
            this.clmMatchPercentage.HeaderText = "Match";
            this.clmMatchPercentage.Name = "clmMatchPercentage";
            this.clmMatchPercentage.ReadOnly = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.tblForm.SetColumnSpan(this.label1, 2);
            this.label1.Location = new System.Drawing.Point(5, 166);
            this.label1.Margin = new System.Windows.Forms.Padding(5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "FASTA header format:";
            // 
            // txtFormatString
            // 
            this.tblForm.SetColumnSpan(this.txtFormatString, 3);
            this.txtFormatString.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtFormatString.Location = new System.Drawing.Point(125, 164);
            this.txtFormatString.Name = "txtFormatString";
            this.txtFormatString.Size = new System.Drawing.Size(756, 20);
            this.txtFormatString.TabIndex = 1;
            // 
            // lnkFormatString
            // 
            this.lnkFormatString.AutoSize = true;
            this.lnkFormatString.Location = new System.Drawing.Point(127, 192);
            this.lnkFormatString.Margin = new System.Windows.Forms.Padding(5);
            this.lnkFormatString.Name = "lnkFormatString";
            this.lnkFormatString.Size = new System.Drawing.Size(90, 13);
            this.lnkFormatString.TabIndex = 2;
            this.lnkFormatString.TabStop = true;
            this.lnkFormatString.Text = "Build format string";
            this.lnkFormatString.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkFormatString_LinkClicked);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnRefresh.Location = new System.Drawing.Point(801, 190);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(80, 24);
            this.btnRefresh.TabIndex = 3;
            this.btnRefresh.Text = "&Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tblForm.SetColumnSpan(this.tableLayoutPanel2, 3);
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.btnImport, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.cmbSubSets, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 536);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(778, 30);
            this.tableLayoutPanel2.TabIndex = 5;
            // 
            // btnImport
            // 
            this.btnImport.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnImport.Location = new System.Drawing.Point(3, 3);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(90, 24);
            this.btnImport.TabIndex = 0;
            this.btnImport.Text = "&Import to";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // cmbSubSets
            // 
            this.cmbSubSets.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cmbSubSets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSubSets.FormattingEnabled = true;
            this.cmbSubSets.Location = new System.Drawing.Point(99, 4);
            this.cmbSubSets.Name = "cmbSubSets";
            this.cmbSubSets.Size = new System.Drawing.Size(200, 21);
            this.cmbSubSets.TabIndex = 1;
            // 
            // lblInstructions
            // 
            this.lblInstructions.AutoSize = true;
            this.tblForm.SetColumnSpan(this.lblInstructions, 5);
            this.lblInstructions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblInstructions.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInstructions.Location = new System.Drawing.Point(5, 5);
            this.lblInstructions.Margin = new System.Windows.Forms.Padding(5);
            this.lblInstructions.Name = "lblInstructions";
            this.lblInstructions.Size = new System.Drawing.Size(874, 128);
            this.lblInstructions.TabIndex = 7;
            this.lblInstructions.Text = resources.GetString("lblInstructions.Text");
            // 
            // lnkInstructions
            // 
            this.lnkInstructions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkInstructions.AutoSize = true;
            this.tblForm.SetColumnSpan(this.lnkInstructions, 3);
            this.lnkInstructions.Location = new System.Drawing.Point(762, 143);
            this.lnkInstructions.Margin = new System.Windows.Forms.Padding(5);
            this.lnkInstructions.Name = "lnkInstructions";
            this.lnkInstructions.Size = new System.Drawing.Size(117, 13);
            this.lnkInstructions.TabIndex = 2;
            this.lnkInstructions.TabStop = true;
            this.lnkInstructions.Text = "Show/Hide instructions";
            this.lnkInstructions.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkInstructions_LinkClicked);
            // 
            // frmImportFromAlignment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(884, 566);
            this.Controls.Add(this.tblForm);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmImportFromAlignment";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Import Aligned Sequences";
            this.Load += new System.EventHandler(this.frmImportFromFASTA_Load);
            this.tblForm.ResumeLayout(false);
            this.tblForm.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdImport)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tblForm;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.DataGridView grdImport;
        private System.Windows.Forms.ComboBox cmbSubSets;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtFormatString;
        private System.Windows.Forms.LinkLabel lnkFormatString;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label lblInstructions;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmSourceName;
        private System.Windows.Forms.DataGridViewComboBoxColumn clmGeneName;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmMatchPercentage;
        private System.Windows.Forms.LinkLabel lnkInstructions;
    }
}