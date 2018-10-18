namespace Pilgrimage.Search
{
    partial class frmTaxonomyDetails
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTaxonomyDetails));
            this.tblForm = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtScientificName = new System.Windows.Forms.TextBox();
            this.txtCommonName = new System.Windows.Forms.TextBox();
            this.txtRank = new System.Windows.Forms.TextBox();
            this.txtDivision = new System.Windows.Forms.TextBox();
            this.pnlLineage = new System.Windows.Forms.FlowLayoutPanel();
            this.label6 = new System.Windows.Forms.Label();
            this.lnkNCBI = new System.Windows.Forms.LinkLabel();
            this.btnClose = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tblForm.SuspendLayout();
            this.SuspendLayout();
            // 
            // tblForm
            // 
            this.tblForm.AutoSize = true;
            this.tblForm.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tblForm.ColumnCount = 4;
            this.tblForm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tblForm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblForm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tblForm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblForm.Controls.Add(this.label1, 0, 0);
            this.tblForm.Controls.Add(this.label2, 2, 0);
            this.tblForm.Controls.Add(this.label3, 0, 1);
            this.tblForm.Controls.Add(this.label4, 2, 1);
            this.tblForm.Controls.Add(this.label5, 0, 3);
            this.tblForm.Controls.Add(this.txtScientificName, 1, 0);
            this.tblForm.Controls.Add(this.txtCommonName, 3, 0);
            this.tblForm.Controls.Add(this.txtRank, 1, 1);
            this.tblForm.Controls.Add(this.txtDivision, 3, 1);
            this.tblForm.Controls.Add(this.pnlLineage, 1, 3);
            this.tblForm.Controls.Add(this.label6, 0, 2);
            this.tblForm.Controls.Add(this.lnkNCBI, 1, 2);
            this.tblForm.Controls.Add(this.btnClose, 3, 4);
            this.tblForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblForm.Location = new System.Drawing.Point(0, 0);
            this.tblForm.Name = "tblForm";
            this.tblForm.RowCount = 5;
            this.tblForm.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblForm.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblForm.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblForm.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblForm.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblForm.Size = new System.Drawing.Size(544, 135);
            this.tblForm.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 8);
            this.label1.Margin = new System.Windows.Forms.Padding(5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Scientific Name:";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(285, 8);
            this.label2.Margin = new System.Windows.Forms.Padding(5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Other Name:";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 38);
            this.label3.Margin = new System.Windows.Forms.Padding(5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Rank:";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(285, 38);
            this.label4.Margin = new System.Windows.Forms.Padding(5);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Division:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(5, 88);
            this.label5.Margin = new System.Windows.Forms.Padding(5);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Lineage:";
            // 
            // txtScientificName
            // 
            this.txtScientificName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtScientificName.Location = new System.Drawing.Point(99, 5);
            this.txtScientificName.Margin = new System.Windows.Forms.Padding(5);
            this.txtScientificName.Name = "txtScientificName";
            this.txtScientificName.Size = new System.Drawing.Size(176, 20);
            this.txtScientificName.TabIndex = 1;
            // 
            // txtCommonName
            // 
            this.txtCommonName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCommonName.Location = new System.Drawing.Point(362, 5);
            this.txtCommonName.Margin = new System.Windows.Forms.Padding(5);
            this.txtCommonName.Name = "txtCommonName";
            this.txtCommonName.Size = new System.Drawing.Size(177, 20);
            this.txtCommonName.TabIndex = 3;
            // 
            // txtRank
            // 
            this.txtRank.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRank.Location = new System.Drawing.Point(99, 35);
            this.txtRank.Margin = new System.Windows.Forms.Padding(5);
            this.txtRank.Name = "txtRank";
            this.txtRank.Size = new System.Drawing.Size(176, 20);
            this.txtRank.TabIndex = 5;
            // 
            // txtDivision
            // 
            this.txtDivision.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDivision.Location = new System.Drawing.Point(362, 35);
            this.txtDivision.Margin = new System.Windows.Forms.Padding(5);
            this.txtDivision.Name = "txtDivision";
            this.txtDivision.Size = new System.Drawing.Size(177, 20);
            this.txtDivision.TabIndex = 7;
            // 
            // pnlLineage
            // 
            this.pnlLineage.AutoSize = true;
            this.pnlLineage.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tblForm.SetColumnSpan(this.pnlLineage, 3);
            this.pnlLineage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLineage.Location = new System.Drawing.Point(94, 83);
            this.pnlLineage.Margin = new System.Windows.Forms.Padding(0);
            this.pnlLineage.Name = "pnlLineage";
            this.pnlLineage.Size = new System.Drawing.Size(450, 23);
            this.pnlLineage.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(5, 65);
            this.label6.Margin = new System.Windows.Forms.Padding(5);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "NCBI URL:";
            // 
            // lnkNCBI
            // 
            this.lnkNCBI.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lnkNCBI.AutoSize = true;
            this.tblForm.SetColumnSpan(this.lnkNCBI, 3);
            this.lnkNCBI.Location = new System.Drawing.Point(99, 65);
            this.lnkNCBI.Margin = new System.Windows.Forms.Padding(5);
            this.lnkNCBI.Name = "lnkNCBI";
            this.lnkNCBI.Size = new System.Drawing.Size(38, 13);
            this.lnkNCBI.TabIndex = 9;
            this.lnkNCBI.TabStop = true;
            this.lnkNCBI.Text = "https://";
            this.lnkNCBI.UseMnemonic = false;
            this.lnkNCBI.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnk_LinkClicked);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(466, 109);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 12;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // frmTaxonomyDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(544, 135);
            this.Controls.Add(this.tblForm);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmTaxonomyDetails";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Details";
            this.Load += new System.EventHandler(this.frmTaxonomyDetails_Load);
            this.tblForm.ResumeLayout(false);
            this.tblForm.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tblForm;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtScientificName;
        private System.Windows.Forms.TextBox txtCommonName;
        private System.Windows.Forms.TextBox txtRank;
        private System.Windows.Forms.TextBox txtDivision;
        private System.Windows.Forms.FlowLayoutPanel pnlLineage;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.LinkLabel lnkNCBI;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ToolTip toolTip;
    }
}