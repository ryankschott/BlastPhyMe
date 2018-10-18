namespace Pilgrimage.GeneSequences.Search
{
    partial class frmSearchGenBank
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSearchGenBank));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tblSearchHistory = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSearchQuery = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblSearchHistory = new System.Windows.Forms.Label();
            this.btnTaxonomy = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.tblSearchHistory, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtSearchQuery, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnSearch, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnCancel, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblSearchHistory, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnTaxonomy, 1, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(584, 122);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tblSearchHistory
            // 
            this.tblSearchHistory.AutoSize = true;
            this.tblSearchHistory.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tblSearchHistory.ColumnCount = 1;
            this.tableLayoutPanel1.SetColumnSpan(this.tblSearchHistory, 3);
            this.tblSearchHistory.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblSearchHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblSearchHistory.Location = new System.Drawing.Point(106, 69);
            this.tblSearchHistory.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.tblSearchHistory.Name = "tblSearchHistory";
            this.tblSearchHistory.RowCount = 1;
            this.tblSearchHistory.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblSearchHistory.Size = new System.Drawing.Size(478, 18);
            this.tblSearchHistory.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 5);
            this.label1.Margin = new System.Windows.Forms.Padding(5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Nucleotide search:";
            // 
            // txtSearchQuery
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.txtSearchQuery, 3);
            this.txtSearchQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSearchQuery.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearchQuery.Location = new System.Drawing.Point(111, 5);
            this.txtSearchQuery.Margin = new System.Windows.Forms.Padding(5);
            this.txtSearchQuery.MaxLength = 2000;
            this.txtSearchQuery.Multiline = true;
            this.txtSearchQuery.Name = "txtSearchQuery";
            this.txtSearchQuery.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtSearchQuery.Size = new System.Drawing.Size(468, 59);
            this.txtSearchQuery.TabIndex = 1;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSearch.Location = new System.Drawing.Point(415, 95);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(80, 24);
            this.btnSearch.TabIndex = 5;
            this.btnSearch.Text = "&Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.ImageKey = "Cancel";
            this.btnCancel.Location = new System.Drawing.Point(501, 95);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 24);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // lblSearchHistory
            // 
            this.lblSearchHistory.AutoSize = true;
            this.lblSearchHistory.Location = new System.Drawing.Point(5, 74);
            this.lblSearchHistory.Margin = new System.Windows.Forms.Padding(5);
            this.lblSearchHistory.Name = "lblSearchHistory";
            this.lblSearchHistory.Size = new System.Drawing.Size(77, 13);
            this.lblSearchHistory.TabIndex = 2;
            this.lblSearchHistory.Text = "Search history:";
            // 
            // btnTaxonomy
            // 
            this.btnTaxonomy.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnTaxonomy.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTaxonomy.Location = new System.Drawing.Point(109, 95);
            this.btnTaxonomy.Name = "btnTaxonomy";
            this.btnTaxonomy.Size = new System.Drawing.Size(130, 24);
            this.btnTaxonomy.TabIndex = 4;
            this.btnTaxonomy.Text = "&Filter by Taxonomy";
            this.btnTaxonomy.UseVisualStyleBackColor = true;
            this.btnTaxonomy.Click += new System.EventHandler(this.btnTaxonomy_Click);
            // 
            // frmSearchGenBank
            // 
            this.AcceptButton = this.btnSearch;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(584, 122);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(1000, 700);
            this.MinimumSize = new System.Drawing.Size(500, 128);
            this.Name = "frmSearchGenBank";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Search GenBank for Nucleotides";
            this.Load += new System.EventHandler(this.frmSearchGenBank_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnCancel;
        internal System.Windows.Forms.TextBox txtSearchQuery;
        private System.Windows.Forms.Label lblSearchHistory;
        private System.Windows.Forms.TableLayoutPanel tblSearchHistory;
        private System.Windows.Forms.Button btnTaxonomy;
    }
}