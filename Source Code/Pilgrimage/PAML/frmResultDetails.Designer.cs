namespace Pilgrimage.PAML
{
    partial class frmResultDetails
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmResultDetails));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.grdResults = new System.Windows.Forms.DataGridView();
            this.clmNP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmLnL = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmK = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmValueHeader = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmValue0 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmValue1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmValue2a = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmValue2b = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmKappaOmegaStart = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label7 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.txtTreeTitle = new System.Windows.Forms.TextBox();
            this.txtKappa = new System.Windows.Forms.TextBox();
            this.txtOmega = new System.Windows.Forms.TextBox();
            this.txtNCatG = new System.Windows.Forms.TextBox();
            this.txtModel = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtTreeFilePath = new System.Windows.Forms.TextBox();
            this.txtSequencesFilePath = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtSequenceCountAndLength = new System.Windows.Forms.TextBox();
            this.lnkViewJob = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.grdResults)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 6);
            this.label1.Margin = new System.Windows.Forms.Padding(5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Tree:";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 32);
            this.label2.Margin = new System.Windows.Forms.Padding(5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Sequences:";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 58);
            this.label3.Margin = new System.Windows.Forms.Padding(5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Model:";
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(187, 58);
            this.label8.Margin = new System.Windows.Forms.Padding(5);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(60, 13);
            this.label8.TabIndex = 10;
            this.label8.Text = "Categories:";
            // 
            // grdResults
            // 
            this.grdResults.AllowUserToAddRows = false;
            this.grdResults.AllowUserToDeleteRows = false;
            this.grdResults.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grdResults.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.grdResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdResults.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmNP,
            this.clmLnL,
            this.clmK,
            this.clmValueHeader,
            this.clmValue0,
            this.clmValue1,
            this.clmValue2a,
            this.clmValue2b,
            this.clmKappaOmegaStart});
            this.tableLayoutPanel1.SetColumnSpan(this.grdResults, 8);
            this.grdResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdResults.Location = new System.Drawing.Point(3, 107);
            this.grdResults.Name = "grdResults";
            this.grdResults.ReadOnly = true;
            this.grdResults.RowHeadersVisible = false;
            this.grdResults.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdResults.Size = new System.Drawing.Size(678, 195);
            this.grdResults.TabIndex = 17;
            // 
            // clmNP
            // 
            this.clmNP.DataPropertyName = "np";
            this.clmNP.FillWeight = 7F;
            this.clmNP.HeaderText = "np";
            this.clmNP.Name = "clmNP";
            this.clmNP.ReadOnly = true;
            // 
            // clmLnL
            // 
            this.clmLnL.DataPropertyName = "lnL";
            this.clmLnL.FillWeight = 10F;
            this.clmLnL.HeaderText = "lnL";
            this.clmLnL.Name = "clmLnL";
            this.clmLnL.ReadOnly = true;
            // 
            // clmK
            // 
            this.clmK.DataPropertyName = "k";
            this.clmK.FillWeight = 10F;
            this.clmK.HeaderText = "k";
            this.clmK.Name = "clmK";
            this.clmK.ReadOnly = true;
            // 
            // clmValueHeader
            // 
            this.clmValueHeader.DataPropertyName = "ValueHeader";
            this.clmValueHeader.FillWeight = 16F;
            this.clmValueHeader.HeaderText = "";
            this.clmValueHeader.Name = "clmValueHeader";
            this.clmValueHeader.ReadOnly = true;
            // 
            // clmValue0
            // 
            this.clmValue0.DataPropertyName = "Value0";
            this.clmValue0.FillWeight = 11F;
            this.clmValue0.HeaderText = "";
            this.clmValue0.Name = "clmValue0";
            this.clmValue0.ReadOnly = true;
            // 
            // clmValue1
            // 
            this.clmValue1.DataPropertyName = "Value1";
            this.clmValue1.FillWeight = 11F;
            this.clmValue1.HeaderText = "";
            this.clmValue1.Name = "clmValue1";
            this.clmValue1.ReadOnly = true;
            // 
            // clmValue2a
            // 
            this.clmValue2a.DataPropertyName = "Value2a";
            this.clmValue2a.FillWeight = 11F;
            this.clmValue2a.HeaderText = "";
            this.clmValue2a.Name = "clmValue2a";
            this.clmValue2a.ReadOnly = true;
            // 
            // clmValue2b
            // 
            this.clmValue2b.DataPropertyName = "Value2b";
            this.clmValue2b.FillWeight = 11F;
            this.clmValue2b.HeaderText = "";
            this.clmValue2b.Name = "clmValue2b";
            this.clmValue2b.ReadOnly = true;
            // 
            // clmKappaOmegaStart
            // 
            this.clmKappaOmegaStart.DataPropertyName = "KappaOmegaStart";
            this.clmKappaOmegaStart.FillWeight = 13F;
            this.clmKappaOmegaStart.HeaderText = "k/w Start";
            this.clmKappaOmegaStart.Name = "clmKappaOmegaStart";
            this.clmKappaOmegaStart.ReadOnly = true;
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(365, 58);
            this.label7.Margin = new System.Windows.Forms.Padding(5);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "Kappa:";
            // 
            // label9
            // 
            this.label9.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(524, 58);
            this.label9.Margin = new System.Windows.Forms.Padding(5);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(44, 13);
            this.label9.TabIndex = 14;
            this.label9.Text = "Omega:";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 8;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.grdResults, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnClose, 7, 5);
            this.tableLayoutPanel1.Controls.Add(this.txtTreeTitle, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label9, 6, 2);
            this.tableLayoutPanel1.Controls.Add(this.label7, 4, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtKappa, 5, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtOmega, 7, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtNCatG, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.label8, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtModel, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label6, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtTreeFilePath, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtSequencesFilePath, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label5, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtSequenceCountAndLength, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lnkViewJob, 0, 5);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(684, 334);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label4, 8);
            this.label4.Location = new System.Drawing.Point(3, 88);
            this.label4.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(552, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = "Multiple results will be displayed if a range of kappa and/or omega starting valu" +
    "es were configured for the PAML job:";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(606, 308);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 18;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // txtTreeTitle
            // 
            this.txtTreeTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTreeTitle.Location = new System.Drawing.Point(77, 3);
            this.txtTreeTitle.Name = "txtTreeTitle";
            this.txtTreeTitle.ReadOnly = true;
            this.txtTreeTitle.Size = new System.Drawing.Size(102, 20);
            this.txtTreeTitle.TabIndex = 1;
            // 
            // txtKappa
            // 
            this.txtKappa.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtKappa.Location = new System.Drawing.Point(414, 55);
            this.txtKappa.Name = "txtKappa";
            this.txtKappa.ReadOnly = true;
            this.txtKappa.Size = new System.Drawing.Size(102, 20);
            this.txtKappa.TabIndex = 13;
            // 
            // txtOmega
            // 
            this.txtOmega.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOmega.Location = new System.Drawing.Point(576, 55);
            this.txtOmega.Name = "txtOmega";
            this.txtOmega.ReadOnly = true;
            this.txtOmega.Size = new System.Drawing.Size(105, 20);
            this.txtOmega.TabIndex = 15;
            // 
            // txtNCatG
            // 
            this.txtNCatG.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNCatG.Location = new System.Drawing.Point(255, 55);
            this.txtNCatG.Name = "txtNCatG";
            this.txtNCatG.ReadOnly = true;
            this.txtNCatG.Size = new System.Drawing.Size(102, 20);
            this.txtNCatG.TabIndex = 11;
            // 
            // txtModel
            // 
            this.txtModel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtModel.Location = new System.Drawing.Point(77, 55);
            this.txtModel.Name = "txtModel";
            this.txtModel.ReadOnly = true;
            this.txtModel.Size = new System.Drawing.Size(102, 20);
            this.txtModel.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(187, 6);
            this.label6.Margin = new System.Windows.Forms.Padding(5);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(26, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "File:";
            // 
            // txtTreeFilePath
            // 
            this.txtTreeFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.txtTreeFilePath, 5);
            this.txtTreeFilePath.Location = new System.Drawing.Point(255, 3);
            this.txtTreeFilePath.Name = "txtTreeFilePath";
            this.txtTreeFilePath.ReadOnly = true;
            this.txtTreeFilePath.Size = new System.Drawing.Size(426, 20);
            this.txtTreeFilePath.TabIndex = 3;
            // 
            // txtSequencesFilePath
            // 
            this.txtSequencesFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.txtSequencesFilePath, 5);
            this.txtSequencesFilePath.Location = new System.Drawing.Point(255, 29);
            this.txtSequencesFilePath.Name = "txtSequencesFilePath";
            this.txtSequencesFilePath.ReadOnly = true;
            this.txtSequencesFilePath.Size = new System.Drawing.Size(426, 20);
            this.txtSequencesFilePath.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(187, 32);
            this.label5.Margin = new System.Windows.Forms.Padding(5);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(26, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "File:";
            // 
            // txtSequenceCountAndLength
            // 
            this.txtSequenceCountAndLength.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSequenceCountAndLength.Location = new System.Drawing.Point(77, 29);
            this.txtSequenceCountAndLength.Name = "txtSequenceCountAndLength";
            this.txtSequenceCountAndLength.ReadOnly = true;
            this.txtSequenceCountAndLength.Size = new System.Drawing.Size(102, 20);
            this.txtSequenceCountAndLength.TabIndex = 5;
            // 
            // lnkViewJob
            // 
            this.lnkViewJob.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lnkViewJob.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lnkViewJob, 7);
            this.lnkViewJob.Location = new System.Drawing.Point(3, 313);
            this.lnkViewJob.Name = "lnkViewJob";
            this.lnkViewJob.Size = new System.Drawing.Size(115, 13);
            this.lnkViewJob.TabIndex = 19;
            this.lnkViewJob.TabStop = true;
            this.lnkViewJob.Text = "View Job Configuration";
            this.lnkViewJob.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkViewJob_LinkClicked);
            // 
            // frmResultDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(684, 334);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmResultDetails";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Results";
            this.Load += new System.EventHandler(this.frmResultDetails_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdResults)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DataGridView grdResults;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmNP;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmLnL;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmK;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmValueHeader;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmValue0;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmValue1;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmValue2a;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmValue2b;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmKappaOmegaStart;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TextBox txtTreeTitle;
        private System.Windows.Forms.TextBox txtKappa;
        private System.Windows.Forms.TextBox txtOmega;
        private System.Windows.Forms.TextBox txtNCatG;
        private System.Windows.Forms.TextBox txtModel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtTreeFilePath;
        private System.Windows.Forms.TextBox txtSequencesFilePath;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtSequenceCountAndLength;
        private System.Windows.Forms.LinkLabel lnkViewJob;
    }
}