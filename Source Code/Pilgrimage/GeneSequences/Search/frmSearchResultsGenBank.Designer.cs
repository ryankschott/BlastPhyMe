namespace Pilgrimage.GeneSequences.Search
{
    partial class frmSearchResultsGenBank
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSearchResultsGenBank));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.grdResults = new System.Windows.Forms.DataGridView();
            this.clmInclude = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.clmDefinition = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmLengthDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmAccession = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmGenBankURL = new System.Windows.Forms.DataGridViewLinkColumn();
            this.clmInRecordSet = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblTotalRows = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnFirstPage = new System.Windows.Forms.Button();
            this.imgButtons = new System.Windows.Forms.ImageList(this.components);
            this.btnPreviousPage = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPages = new System.Windows.Forms.MaskedTextBox();
            this.lblSelectedRows = new System.Windows.Forms.Label();
            this.btnLastPage = new System.Windows.Forms.Button();
            this.lblPageCount = new System.Windows.Forms.Label();
            this.btnNextPage = new System.Windows.Forms.Button();
            this.cmbSubSets = new System.Windows.Forms.ComboBox();
            this.chkUpdateFromGenBank = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdResults)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.grdResults, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnSave, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.btnClose, 3, 4);
            this.tableLayoutPanel1.Controls.Add(this.lblTotalRows, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.cmbSubSets, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.chkUpdateFromGenBank, 2, 4);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(884, 592);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label1, 4);
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(5, 5);
            this.label1.Margin = new System.Windows.Forms.Padding(5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(773, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select results to add to your dataset.  To view additional details for a record, d" +
    "ouble-click on a row or right-click and select \"Details\"";
            // 
            // grdResults
            // 
            this.grdResults.AllowUserToAddRows = false;
            this.grdResults.AllowUserToDeleteRows = false;
            this.grdResults.AllowUserToResizeRows = false;
            this.grdResults.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grdResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdResults.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmInclude,
            this.clmDefinition,
            this.clmLengthDescription,
            this.clmAccession,
            this.clmGenBankURL,
            this.clmInRecordSet});
            this.tableLayoutPanel1.SetColumnSpan(this.grdResults, 4);
            this.grdResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdResults.Location = new System.Drawing.Point(3, 52);
            this.grdResults.Name = "grdResults";
            this.grdResults.RowHeadersVisible = false;
            this.grdResults.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdResults.Size = new System.Drawing.Size(878, 477);
            this.grdResults.TabIndex = 2;
            // 
            // clmInclude
            // 
            this.clmInclude.DataPropertyName = "Selected";
            this.clmInclude.FillWeight = 3F;
            this.clmInclude.HeaderText = "";
            this.clmInclude.MinimumWidth = 20;
            this.clmInclude.Name = "clmInclude";
            this.clmInclude.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // clmDefinition
            // 
            this.clmDefinition.DataPropertyName = "Definition";
            this.clmDefinition.FillWeight = 56F;
            this.clmDefinition.HeaderText = "Definition";
            this.clmDefinition.Name = "clmDefinition";
            this.clmDefinition.ReadOnly = true;
            // 
            // clmLengthDescription
            // 
            this.clmLengthDescription.DataPropertyName = "Length";
            dataGridViewCellStyle1.Format = "N0";
            this.clmLengthDescription.DefaultCellStyle = dataGridViewCellStyle1;
            this.clmLengthDescription.FillWeight = 10F;
            this.clmLengthDescription.HeaderText = "Length";
            this.clmLengthDescription.Name = "clmLengthDescription";
            this.clmLengthDescription.ReadOnly = true;
            // 
            // clmAccession
            // 
            this.clmAccession.DataPropertyName = "Accession";
            this.clmAccession.FillWeight = 12F;
            this.clmAccession.HeaderText = "Accession";
            this.clmAccession.Name = "clmAccession";
            this.clmAccession.ReadOnly = true;
            // 
            // clmGenBankURL
            // 
            this.clmGenBankURL.DataPropertyName = "GenBankUrl";
            this.clmGenBankURL.FillWeight = 12F;
            this.clmGenBankURL.HeaderText = "GenBank";
            this.clmGenBankURL.Name = "clmGenBankURL";
            this.clmGenBankURL.ReadOnly = true;
            // 
            // clmInRecordSet
            // 
            this.clmInRecordSet.DataPropertyName = "InRecordSet";
            this.clmInRecordSet.FillWeight = 7F;
            this.clmInRecordSet.HeaderText = "In Project";
            this.clmInRecordSet.Name = "clmInRecordSet";
            this.clmInRecordSet.ReadOnly = true;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(3, 565);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 24);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "&Add to:";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(801, 565);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 24);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblTotalRows
            // 
            this.lblTotalRows.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblTotalRows.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblTotalRows, 4);
            this.lblTotalRows.Location = new System.Drawing.Point(5, 31);
            this.lblTotalRows.Margin = new System.Windows.Forms.Padding(5);
            this.lblTotalRows.Name = "lblTotalRows";
            this.lblTotalRows.Size = new System.Drawing.Size(94, 13);
            this.lblTotalRows.TabIndex = 1;
            this.lblTotalRows.Text = "# records returned";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel2.ColumnCount = 9;
            this.tableLayoutPanel1.SetColumnSpan(this.tableLayoutPanel2, 4);
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.btnFirstPage, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnPreviousPage, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.label2, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.txtPages, 4, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblSelectedRows, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnLastPage, 7, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblPageCount, 5, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnNextPage, 6, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 532);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(884, 30);
            this.tableLayoutPanel2.TabIndex = 3;
            // 
            // btnFirstPage
            // 
            this.btnFirstPage.ImageKey = "LeftArrowShort";
            this.btnFirstPage.ImageList = this.imgButtons;
            this.btnFirstPage.Location = new System.Drawing.Point(329, 3);
            this.btnFirstPage.Name = "btnFirstPage";
            this.btnFirstPage.Size = new System.Drawing.Size(24, 24);
            this.btnFirstPage.TabIndex = 2;
            this.btnFirstPage.UseVisualStyleBackColor = true;
            this.btnFirstPage.Click += new System.EventHandler(this.btnFirstPage_Click);
            // 
            // imgButtons
            // 
            this.imgButtons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgButtons.ImageStream")));
            this.imgButtons.TransparentColor = System.Drawing.Color.Transparent;
            this.imgButtons.Images.SetKeyName(0, "LeftArrowLong");
            this.imgButtons.Images.SetKeyName(1, "LeftArrowShort");
            this.imgButtons.Images.SetKeyName(2, "RightArrowLong");
            this.imgButtons.Images.SetKeyName(3, "RightArrowShort");
            // 
            // btnPreviousPage
            // 
            this.btnPreviousPage.ImageKey = "LeftArrowLong";
            this.btnPreviousPage.ImageList = this.imgButtons;
            this.btnPreviousPage.Location = new System.Drawing.Point(359, 3);
            this.btnPreviousPage.Name = "btnPreviousPage";
            this.btnPreviousPage.Size = new System.Drawing.Size(24, 24);
            this.btnPreviousPage.TabIndex = 3;
            this.btnPreviousPage.UseVisualStyleBackColor = true;
            this.btnPreviousPage.Click += new System.EventHandler(this.btnPreviousPage_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(389, 8);
            this.label2.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Page:";
            // 
            // txtPages
            // 
            this.txtPages.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPages.Location = new System.Drawing.Point(427, 5);
            this.txtPages.Mask = "000";
            this.txtPages.Name = "txtPages";
            this.txtPages.PromptChar = ' ';
            this.txtPages.Size = new System.Drawing.Size(24, 20);
            this.txtPages.TabIndex = 5;
            this.txtPages.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtPages_KeyUp);
            // 
            // lblSelectedRows
            // 
            this.lblSelectedRows.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblSelectedRows.AutoSize = true;
            this.lblSelectedRows.Location = new System.Drawing.Point(5, 8);
            this.lblSelectedRows.Margin = new System.Windows.Forms.Padding(5);
            this.lblSelectedRows.Name = "lblSelectedRows";
            this.lblSelectedRows.Size = new System.Drawing.Size(95, 13);
            this.lblSelectedRows.TabIndex = 1;
            this.lblSelectedRows.Text = "# records selected";
            // 
            // btnLastPage
            // 
            this.btnLastPage.ImageKey = "RightArrowShort";
            this.btnLastPage.ImageList = this.imgButtons;
            this.btnLastPage.Location = new System.Drawing.Point(530, 3);
            this.btnLastPage.Name = "btnLastPage";
            this.btnLastPage.Size = new System.Drawing.Size(24, 24);
            this.btnLastPage.TabIndex = 0;
            this.btnLastPage.UseVisualStyleBackColor = true;
            this.btnLastPage.Click += new System.EventHandler(this.btnLastPage_Click);
            // 
            // lblPageCount
            // 
            this.lblPageCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPageCount.AutoSize = true;
            this.lblPageCount.Location = new System.Drawing.Point(457, 8);
            this.lblPageCount.Name = "lblPageCount";
            this.lblPageCount.Size = new System.Drawing.Size(37, 13);
            this.lblPageCount.TabIndex = 6;
            this.lblPageCount.Text = "of 000";
            // 
            // btnNextPage
            // 
            this.btnNextPage.ImageKey = "RightArrowLong";
            this.btnNextPage.ImageList = this.imgButtons;
            this.btnNextPage.Location = new System.Drawing.Point(500, 3);
            this.btnNextPage.Name = "btnNextPage";
            this.btnNextPage.Size = new System.Drawing.Size(24, 24);
            this.btnNextPage.TabIndex = 7;
            this.btnNextPage.UseVisualStyleBackColor = true;
            this.btnNextPage.Click += new System.EventHandler(this.btnNextPage_Click);
            // 
            // cmbSubSets
            // 
            this.cmbSubSets.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cmbSubSets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSubSets.FormattingEnabled = true;
            this.cmbSubSets.Location = new System.Drawing.Point(89, 566);
            this.cmbSubSets.Name = "cmbSubSets";
            this.cmbSubSets.Size = new System.Drawing.Size(200, 21);
            this.cmbSubSets.TabIndex = 5;
            // 
            // chkUpdateFromGenBank
            // 
            this.chkUpdateFromGenBank.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkUpdateFromGenBank.AutoSize = true;
            this.chkUpdateFromGenBank.Checked = true;
            this.chkUpdateFromGenBank.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkUpdateFromGenBank.Location = new System.Drawing.Point(297, 568);
            this.chkUpdateFromGenBank.Margin = new System.Windows.Forms.Padding(5);
            this.chkUpdateFromGenBank.Name = "chkUpdateFromGenBank";
            this.chkUpdateFromGenBank.Size = new System.Drawing.Size(132, 17);
            this.chkUpdateFromGenBank.TabIndex = 6;
            this.chkUpdateFromGenBank.Text = "Update from GenBank";
            this.chkUpdateFromGenBank.UseVisualStyleBackColor = true;
            // 
            // frmSearchResultsGenBank
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(884, 592);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(440, 440);
            this.Name = "frmSearchResultsGenBank";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Search Results";
            this.Load += new System.EventHandler(this.frmSearchResultsGenBank_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdResults)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView grdResults;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnLastPage;
        private System.Windows.Forms.ImageList imgButtons;
        private System.Windows.Forms.Button btnPreviousPage;
        private System.Windows.Forms.Button btnFirstPage;
        private System.Windows.Forms.Button btnNextPage;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.MaskedTextBox txtPages;
        private System.Windows.Forms.Label lblSelectedRows;
        private System.Windows.Forms.Label lblTotalRows;
        private System.Windows.Forms.Label lblPageCount;
        private System.Windows.Forms.ComboBox cmbSubSets;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.CheckBox chkUpdateFromGenBank;
        private System.Windows.Forms.DataGridViewCheckBoxColumn clmInclude;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmDefinition;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmLengthDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmAccession;
        private System.Windows.Forms.DataGridViewLinkColumn clmGenBankURL;
        private System.Windows.Forms.DataGridViewCheckBoxColumn clmInRecordSet;
    }
}