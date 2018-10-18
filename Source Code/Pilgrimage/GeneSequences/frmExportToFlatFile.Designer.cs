namespace Pilgrimage.GeneSequences
{
    partial class frmExportToFlatFile
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmExportToFlatFile));
            this.tblOutput_FASTA = new System.Windows.Forms.TableLayoutPanel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblHeaderFormat = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.rbPerSequence = new System.Windows.Forms.RadioButton();
            this.rbOneFile = new System.Windows.Forms.RadioButton();
            this.lnkFASTAHeaderFieldNames = new System.Windows.Forms.LinkLabel();
            this.txtHeaderFormat = new System.Windows.Forms.TextBox();
            this.txtFileNameFormat = new System.Windows.Forms.TextBox();
            this.lnkFileNameFieldNames = new System.Windows.Forms.LinkLabel();
            this.btnExport = new System.Windows.Forms.Button();
            this.chkOpenInExplorer = new System.Windows.Forms.CheckBox();
            this.tblOutput_FASTA.SuspendLayout();
            this.SuspendLayout();
            // 
            // tblOutput_FASTA
            // 
            this.tblOutput_FASTA.AutoSize = true;
            this.tblOutput_FASTA.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tblOutput_FASTA.ColumnCount = 5;
            this.tblOutput_FASTA.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tblOutput_FASTA.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tblOutput_FASTA.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblOutput_FASTA.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tblOutput_FASTA.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tblOutput_FASTA.Controls.Add(this.btnCancel, 4, 6);
            this.tblOutput_FASTA.Controls.Add(this.lblHeaderFormat, 0, 0);
            this.tblOutput_FASTA.Controls.Add(this.label3, 0, 4);
            this.tblOutput_FASTA.Controls.Add(this.rbPerSequence, 0, 3);
            this.tblOutput_FASTA.Controls.Add(this.rbOneFile, 0, 2);
            this.tblOutput_FASTA.Controls.Add(this.lnkFASTAHeaderFieldNames, 1, 1);
            this.tblOutput_FASTA.Controls.Add(this.txtHeaderFormat, 1, 0);
            this.tblOutput_FASTA.Controls.Add(this.txtFileNameFormat, 1, 4);
            this.tblOutput_FASTA.Controls.Add(this.lnkFileNameFieldNames, 1, 5);
            this.tblOutput_FASTA.Controls.Add(this.btnExport, 3, 6);
            this.tblOutput_FASTA.Controls.Add(this.chkOpenInExplorer, 1, 6);
            this.tblOutput_FASTA.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblOutput_FASTA.Location = new System.Drawing.Point(0, 0);
            this.tblOutput_FASTA.Name = "tblOutput_FASTA";
            this.tblOutput_FASTA.RowCount = 7;
            this.tblOutput_FASTA.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblOutput_FASTA.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblOutput_FASTA.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblOutput_FASTA.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblOutput_FASTA.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblOutput_FASTA.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblOutput_FASTA.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblOutput_FASTA.Size = new System.Drawing.Size(594, 189);
            this.tblOutput_FASTA.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(511, 163);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 24);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // lblHeaderFormat
            // 
            this.lblHeaderFormat.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblHeaderFormat.AutoSize = true;
            this.lblHeaderFormat.Location = new System.Drawing.Point(5, 8);
            this.lblHeaderFormat.Margin = new System.Windows.Forms.Padding(5);
            this.lblHeaderFormat.Name = "lblHeaderFormat";
            this.lblHeaderFormat.Size = new System.Drawing.Size(92, 13);
            this.lblHeaderFormat.TabIndex = 0;
            this.lblHeaderFormat.Text = "{0} header format:";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 115);
            this.label3.Margin = new System.Windows.Forms.Padding(10, 5, 5, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "File name format:";
            // 
            // rbPerSequence
            // 
            this.rbPerSequence.AutoSize = true;
            this.tblOutput_FASTA.SetColumnSpan(this.rbPerSequence, 4);
            this.rbPerSequence.Location = new System.Drawing.Point(5, 85);
            this.rbPerSequence.Margin = new System.Windows.Forms.Padding(5);
            this.rbPerSequence.Name = "rbPerSequence";
            this.rbPerSequence.Size = new System.Drawing.Size(129, 17);
            this.rbPerSequence.TabIndex = 4;
            this.rbPerSequence.Text = "One file per sequence";
            this.rbPerSequence.UseVisualStyleBackColor = true;
            this.rbPerSequence.CheckedChanged += new System.EventHandler(this.rbCheckedChanged);
            // 
            // rbOneFile
            // 
            this.rbOneFile.AutoSize = true;
            this.rbOneFile.Checked = true;
            this.tblOutput_FASTA.SetColumnSpan(this.rbOneFile, 2);
            this.rbOneFile.Location = new System.Drawing.Point(5, 58);
            this.rbOneFile.Margin = new System.Windows.Forms.Padding(5);
            this.rbOneFile.Name = "rbOneFile";
            this.rbOneFile.Size = new System.Drawing.Size(144, 17);
            this.rbOneFile.TabIndex = 3;
            this.rbOneFile.TabStop = true;
            this.rbOneFile.Text = "One file for all sequences";
            this.rbOneFile.UseVisualStyleBackColor = true;
            this.rbOneFile.CheckedChanged += new System.EventHandler(this.rbCheckedChanged);
            // 
            // lnkFASTAHeaderFieldNames
            // 
            this.lnkFASTAHeaderFieldNames.AutoSize = true;
            this.lnkFASTAHeaderFieldNames.Location = new System.Drawing.Point(107, 35);
            this.lnkFASTAHeaderFieldNames.Margin = new System.Windows.Forms.Padding(5);
            this.lnkFASTAHeaderFieldNames.Name = "lnkFASTAHeaderFieldNames";
            this.lnkFASTAHeaderFieldNames.Size = new System.Drawing.Size(90, 13);
            this.lnkFASTAHeaderFieldNames.TabIndex = 2;
            this.lnkFASTAHeaderFieldNames.TabStop = true;
            this.lnkFASTAHeaderFieldNames.Text = "Build format string";
            this.lnkFASTAHeaderFieldNames.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkFieldNames_LinkClicked);
            // 
            // txtHeaderFormat
            // 
            this.txtHeaderFormat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tblOutput_FASTA.SetColumnSpan(this.txtHeaderFormat, 4);
            this.txtHeaderFormat.Location = new System.Drawing.Point(107, 5);
            this.txtHeaderFormat.Margin = new System.Windows.Forms.Padding(5);
            this.txtHeaderFormat.Name = "txtHeaderFormat";
            this.txtHeaderFormat.Size = new System.Drawing.Size(482, 20);
            this.txtHeaderFormat.TabIndex = 1;
            // 
            // txtFileNameFormat
            // 
            this.txtFileNameFormat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tblOutput_FASTA.SetColumnSpan(this.txtFileNameFormat, 4);
            this.txtFileNameFormat.Location = new System.Drawing.Point(107, 112);
            this.txtFileNameFormat.Margin = new System.Windows.Forms.Padding(5);
            this.txtFileNameFormat.Name = "txtFileNameFormat";
            this.txtFileNameFormat.Size = new System.Drawing.Size(482, 20);
            this.txtFileNameFormat.TabIndex = 6;
            // 
            // lnkFileNameFieldNames
            // 
            this.lnkFileNameFieldNames.AutoSize = true;
            this.lnkFileNameFieldNames.Location = new System.Drawing.Point(107, 142);
            this.lnkFileNameFieldNames.Margin = new System.Windows.Forms.Padding(5);
            this.lnkFileNameFieldNames.Name = "lnkFileNameFieldNames";
            this.lnkFileNameFieldNames.Size = new System.Drawing.Size(90, 13);
            this.lnkFileNameFieldNames.TabIndex = 7;
            this.lnkFileNameFieldNames.TabStop = true;
            this.lnkFileNameFieldNames.Text = "Build format string";
            this.lnkFileNameFieldNames.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkFieldNames_LinkClicked);
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.Location = new System.Drawing.Point(425, 163);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(80, 24);
            this.btnExport.TabIndex = 8;
            this.btnExport.Text = "&Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // chkOpenInExplorer
            // 
            this.chkOpenInExplorer.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.chkOpenInExplorer.AutoSize = true;
            this.tblOutput_FASTA.SetColumnSpan(this.chkOpenInExplorer, 2);
            this.chkOpenInExplorer.Location = new System.Drawing.Point(239, 166);
            this.chkOpenInExplorer.Name = "chkOpenInExplorer";
            this.chkOpenInExplorer.Size = new System.Drawing.Size(180, 17);
            this.chkOpenInExplorer.TabIndex = 10;
            this.chkOpenInExplorer.Text = "Open in Explorer after exporting?";
            this.chkOpenInExplorer.UseVisualStyleBackColor = true;
            // 
            // frmExportToFASTA
            // 
            this.AcceptButton = this.btnExport;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(594, 189);
            this.Controls.Add(this.tblOutput_FASTA);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(610, 60);
            this.Name = "frmExportToFASTA";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Export to {0}";
            this.Load += new System.EventHandler(this.frmExportToFASTA_Load);
            this.tblOutput_FASTA.ResumeLayout(false);
            this.tblOutput_FASTA.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tblOutput_FASTA;
        private System.Windows.Forms.TextBox txtFileNameFormat;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblHeaderFormat;
        private System.Windows.Forms.RadioButton rbOneFile;
        private System.Windows.Forms.LinkLabel lnkFASTAHeaderFieldNames;
        private System.Windows.Forms.TextBox txtHeaderFormat;
        private System.Windows.Forms.RadioButton rbPerSequence;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.LinkLabel lnkFileNameFieldNames;
        private System.Windows.Forms.CheckBox chkOpenInExplorer;
    }
}