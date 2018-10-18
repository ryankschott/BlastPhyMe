namespace Pilgrimage.Search
{
    partial class frmSearchForTaxonomy
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSearchForTaxonomy));
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSearchQuery = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.grdNCBIResults = new System.Windows.Forms.DataGridView();
            this.clmName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmRank = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmDivision = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblNCBIResults = new System.Windows.Forms.Label();
            this.grdLocalResults = new System.Windows.Forms.DataGridView();
            this.clmLocalName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmLineage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblLocalResults = new System.Windows.Forms.Label();
            this.lblTotalNCBIRows = new System.Windows.Forms.Label();
            this.btnAddFromLocal = new System.Windows.Forms.Button();
            this.btnAddFromNCBI = new System.Windows.Forms.Button();
            this.lblTotalLocalRows = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdNCBIResults)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdLocalResults)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.tableLayoutPanel1.SetColumnSpan(this.btnSearch, 2);
            this.btnSearch.Location = new System.Drawing.Point(465, 69);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(80, 23);
            this.btnSearch.TabIndex = 3;
            this.btnSearch.Text = "&Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(551, 69);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.btnCancel, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnSearch, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtSearchQuery, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.grdNCBIResults, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblNCBIResults, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.grdLocalResults, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.lblLocalResults, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.lblTotalNCBIRows, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.btnAddFromLocal, 2, 6);
            this.tableLayoutPanel1.Controls.Add(this.btnAddFromNCBI, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.lblTotalLocalRows, 1, 6);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(634, 366);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 5);
            this.label1.Margin = new System.Windows.Forms.Padding(5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Taxonomy search:";
            // 
            // txtSearchQuery
            // 
            this.txtSearchQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.txtSearchQuery, 3);
            this.txtSearchQuery.Location = new System.Drawing.Point(109, 5);
            this.txtSearchQuery.Margin = new System.Windows.Forms.Padding(5);
            this.txtSearchQuery.Name = "txtSearchQuery";
            this.txtSearchQuery.Size = new System.Drawing.Size(520, 20);
            this.txtSearchQuery.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label2, 3);
            this.label2.Location = new System.Drawing.Point(109, 35);
            this.label2.Margin = new System.Windows.Forms.Padding(5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(514, 26);
            this.label2.TabIndex = 2;
            this.label2.Text = resources.GetString("label2.Text");
            // 
            // grdNCBIResults
            // 
            this.grdNCBIResults.AllowUserToAddRows = false;
            this.grdNCBIResults.AllowUserToDeleteRows = false;
            this.grdNCBIResults.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grdNCBIResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdNCBIResults.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmName,
            this.clmRank,
            this.clmDivision});
            this.tableLayoutPanel1.SetColumnSpan(this.grdNCBIResults, 3);
            this.grdNCBIResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdNCBIResults.Location = new System.Drawing.Point(107, 98);
            this.grdNCBIResults.MultiSelect = false;
            this.grdNCBIResults.Name = "grdNCBIResults";
            this.grdNCBIResults.ReadOnly = true;
            this.grdNCBIResults.RowHeadersVisible = false;
            this.grdNCBIResults.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdNCBIResults.Size = new System.Drawing.Size(524, 100);
            this.grdNCBIResults.TabIndex = 6;
            // 
            // clmName
            // 
            this.clmName.DataPropertyName = "Name";
            this.clmName.FillWeight = 60F;
            this.clmName.HeaderText = "Name";
            this.clmName.Name = "clmName";
            this.clmName.ReadOnly = true;
            // 
            // clmRank
            // 
            this.clmRank.DataPropertyName = "Rank";
            this.clmRank.FillWeight = 20F;
            this.clmRank.HeaderText = "Rank";
            this.clmRank.Name = "clmRank";
            this.clmRank.ReadOnly = true;
            // 
            // clmDivision
            // 
            this.clmDivision.DataPropertyName = "Division";
            this.clmDivision.FillWeight = 20F;
            this.clmDivision.HeaderText = "Division";
            this.clmDivision.Name = "clmDivision";
            this.clmDivision.ReadOnly = true;
            // 
            // lblNCBIResults
            // 
            this.lblNCBIResults.AutoSize = true;
            this.lblNCBIResults.Location = new System.Drawing.Point(5, 100);
            this.lblNCBIResults.Margin = new System.Windows.Forms.Padding(5);
            this.lblNCBIResults.Name = "lblNCBIResults";
            this.lblNCBIResults.Size = new System.Drawing.Size(73, 13);
            this.lblNCBIResults.TabIndex = 5;
            this.lblNCBIResults.Text = "NCBI Results:";
            // 
            // grdLocalResults
            // 
            this.grdLocalResults.AllowUserToAddRows = false;
            this.grdLocalResults.AllowUserToDeleteRows = false;
            this.grdLocalResults.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grdLocalResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdLocalResults.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmLocalName,
            this.clmLineage});
            this.tableLayoutPanel1.SetColumnSpan(this.grdLocalResults, 3);
            this.grdLocalResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdLocalResults.Location = new System.Drawing.Point(107, 233);
            this.grdLocalResults.MultiSelect = false;
            this.grdLocalResults.Name = "grdLocalResults";
            this.grdLocalResults.ReadOnly = true;
            this.grdLocalResults.RowHeadersVisible = false;
            this.grdLocalResults.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdLocalResults.Size = new System.Drawing.Size(524, 100);
            this.grdLocalResults.TabIndex = 10;
            // 
            // clmLocalName
            // 
            this.clmLocalName.DataPropertyName = "Name";
            this.clmLocalName.FillWeight = 25F;
            this.clmLocalName.HeaderText = "Name";
            this.clmLocalName.Name = "clmLocalName";
            this.clmLocalName.ReadOnly = true;
            // 
            // clmLineage
            // 
            this.clmLineage.DataPropertyName = "Lineage";
            this.clmLineage.FillWeight = 75F;
            this.clmLineage.HeaderText = "Lineage";
            this.clmLineage.Name = "clmLineage";
            this.clmLineage.ReadOnly = true;
            // 
            // lblLocalResults
            // 
            this.lblLocalResults.AutoSize = true;
            this.lblLocalResults.Location = new System.Drawing.Point(5, 235);
            this.lblLocalResults.Margin = new System.Windows.Forms.Padding(5);
            this.lblLocalResults.Name = "lblLocalResults";
            this.lblLocalResults.Size = new System.Drawing.Size(82, 26);
            this.lblLocalResults.TabIndex = 9;
            this.lblLocalResults.Text = "Local Database\r\nResults:";
            // 
            // lblTotalNCBIRows
            // 
            this.lblTotalNCBIRows.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblTotalNCBIRows.AutoSize = true;
            this.lblTotalNCBIRows.Location = new System.Drawing.Point(104, 209);
            this.lblTotalNCBIRows.Margin = new System.Windows.Forms.Padding(0, 5, 5, 5);
            this.lblTotalNCBIRows.Name = "lblTotalNCBIRows";
            this.lblTotalNCBIRows.Size = new System.Drawing.Size(19, 13);
            this.lblTotalNCBIRows.TabIndex = 7;
            this.lblTotalNCBIRows.Text = "<>";
            // 
            // btnAddFromLocal
            // 
            this.btnAddFromLocal.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.tableLayoutPanel1.SetColumnSpan(this.btnAddFromLocal, 2);
            this.btnAddFromLocal.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnAddFromLocal.Location = new System.Drawing.Point(501, 339);
            this.btnAddFromLocal.Name = "btnAddFromLocal";
            this.btnAddFromLocal.Size = new System.Drawing.Size(130, 23);
            this.btnAddFromLocal.TabIndex = 12;
            this.btnAddFromLocal.Text = "Add from &Local";
            this.btnAddFromLocal.UseVisualStyleBackColor = true;
            this.btnAddFromLocal.Click += new System.EventHandler(this.btnAddFromLocal_Click);
            // 
            // btnAddFromNCBI
            // 
            this.btnAddFromNCBI.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.tableLayoutPanel1.SetColumnSpan(this.btnAddFromNCBI, 2);
            this.btnAddFromNCBI.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnAddFromNCBI.Location = new System.Drawing.Point(501, 204);
            this.btnAddFromNCBI.Name = "btnAddFromNCBI";
            this.btnAddFromNCBI.Size = new System.Drawing.Size(130, 23);
            this.btnAddFromNCBI.TabIndex = 8;
            this.btnAddFromNCBI.Text = "&Add";
            this.btnAddFromNCBI.UseVisualStyleBackColor = true;
            this.btnAddFromNCBI.Click += new System.EventHandler(this.btnAddFromNCBI_Click);
            // 
            // lblTotalLocalRows
            // 
            this.lblTotalLocalRows.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblTotalLocalRows.AutoSize = true;
            this.lblTotalLocalRows.Location = new System.Drawing.Point(104, 344);
            this.lblTotalLocalRows.Margin = new System.Windows.Forms.Padding(0, 5, 5, 5);
            this.lblTotalLocalRows.Name = "lblTotalLocalRows";
            this.lblTotalLocalRows.Size = new System.Drawing.Size(19, 13);
            this.lblTotalLocalRows.TabIndex = 11;
            this.lblTotalLocalRows.Text = "<>";
            // 
            // frmSearchForTaxonomy
            // 
            this.AcceptButton = this.btnSearch;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(634, 366);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(1000, 700);
            this.Name = "frmSearchForTaxonomy";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Search for Taxonomy";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdNCBIResults)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdLocalResults)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSearchQuery;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnAddFromNCBI;
        private System.Windows.Forms.DataGridView grdNCBIResults;
        private System.Windows.Forms.Label lblNCBIResults;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmName;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmRank;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmDivision;
        private System.Windows.Forms.Label lblTotalNCBIRows;
        private System.Windows.Forms.DataGridView grdLocalResults;
        private System.Windows.Forms.Label lblLocalResults;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmLocalName;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmLineage;
        private System.Windows.Forms.Button btnAddFromLocal;
        private System.Windows.Forms.Label lblTotalLocalRows;
    }
}