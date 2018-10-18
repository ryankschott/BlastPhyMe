namespace Pilgrimage.PAML
{
    partial class uctAnalysisConfiguration
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cmbModel = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtKappaInterval = new System.Windows.Forms.NumericUpDown();
            this.txtKappaStart = new System.Windows.Forms.NumericUpDown();
            this.txtKappaEnd = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtOmegaInterval = new System.Windows.Forms.NumericUpDown();
            this.txtOmegaStart = new System.Windows.Forms.NumericUpDown();
            this.txtOmegaEnd = new System.Windows.Forms.NumericUpDown();
            this.pnlNSSites = new System.Windows.Forms.FlowLayoutPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txtNCatG = new System.Windows.Forms.NumericUpDown();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.chkFixedOmega = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.chkFixedKappa = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.txtKappaInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKappaStart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKappaEnd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOmegaInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOmegaStart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOmegaEnd)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtNCatG)).BeginInit();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbModel
            // 
            this.cmbModel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbModel.FormattingEnabled = true;
            this.cmbModel.Location = new System.Drawing.Point(82, 3);
            this.cmbModel.Name = "cmbModel";
            this.cmbModel.Size = new System.Drawing.Size(235, 21);
            this.cmbModel.TabIndex = 1;
            this.cmbModel.SelectedValueChanged += new System.EventHandler(this.cmbModel_SelectedValueChanged);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 7);
            this.label1.Margin = new System.Windows.Forms.Padding(5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Model:";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 32);
            this.label2.Margin = new System.Windows.Forms.Padding(5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Site models:";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 56);
            this.label3.Margin = new System.Windows.Forms.Padding(5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Categories:";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 82);
            this.label4.Margin = new System.Windows.Forms.Padding(5);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Kappa Start:";
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(61, 6);
            this.label5.Margin = new System.Windows.Forms.Padding(5);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "End:";
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(156, 6);
            this.label6.Margin = new System.Windows.Forms.Padding(5);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(45, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "Interval:";
            // 
            // txtKappaInterval
            // 
            this.txtKappaInterval.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtKappaInterval.DecimalPlaces = 2;
            this.txtKappaInterval.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.txtKappaInterval.Location = new System.Drawing.Point(209, 3);
            this.txtKappaInterval.Name = "txtKappaInterval";
            this.txtKappaInterval.Size = new System.Drawing.Size(50, 20);
            this.txtKappaInterval.TabIndex = 4;
            this.txtKappaInterval.Value = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.txtKappaInterval.Leave += new System.EventHandler(this.txtInterval_Leave);
            // 
            // txtKappaStart
            // 
            this.txtKappaStart.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtKappaStart.DecimalPlaces = 2;
            this.txtKappaStart.Location = new System.Drawing.Point(3, 3);
            this.txtKappaStart.Name = "txtKappaStart";
            this.txtKappaStart.Size = new System.Drawing.Size(50, 20);
            this.txtKappaStart.TabIndex = 0;
            this.txtKappaStart.Value = new decimal(new int[] {
            20,
            0,
            0,
            65536});
            // 
            // txtKappaEnd
            // 
            this.txtKappaEnd.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtKappaEnd.DecimalPlaces = 2;
            this.txtKappaEnd.Location = new System.Drawing.Point(98, 3);
            this.txtKappaEnd.Name = "txtKappaEnd";
            this.txtKappaEnd.Size = new System.Drawing.Size(50, 20);
            this.txtKappaEnd.TabIndex = 2;
            this.txtKappaEnd.Value = new decimal(new int[] {
            20,
            0,
            0,
            65536});
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(5, 108);
            this.label7.Margin = new System.Windows.Forms.Padding(5);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(69, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "Omega Start:";
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(61, 6);
            this.label8.Margin = new System.Windows.Forms.Padding(5);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 13);
            this.label8.TabIndex = 1;
            this.label8.Text = "End:";
            // 
            // label9
            // 
            this.label9.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(156, 6);
            this.label9.Margin = new System.Windows.Forms.Padding(5);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(45, 13);
            this.label9.TabIndex = 3;
            this.label9.Text = "Interval:";
            // 
            // txtOmegaInterval
            // 
            this.txtOmegaInterval.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtOmegaInterval.DecimalPlaces = 2;
            this.txtOmegaInterval.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.txtOmegaInterval.Location = new System.Drawing.Point(209, 3);
            this.txtOmegaInterval.Name = "txtOmegaInterval";
            this.txtOmegaInterval.Size = new System.Drawing.Size(50, 20);
            this.txtOmegaInterval.TabIndex = 4;
            this.txtOmegaInterval.Value = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.txtOmegaInterval.Leave += new System.EventHandler(this.txtInterval_Leave);
            // 
            // txtOmegaStart
            // 
            this.txtOmegaStart.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtOmegaStart.DecimalPlaces = 2;
            this.txtOmegaStart.Location = new System.Drawing.Point(3, 3);
            this.txtOmegaStart.Name = "txtOmegaStart";
            this.txtOmegaStart.Size = new System.Drawing.Size(50, 20);
            this.txtOmegaStart.TabIndex = 0;
            this.txtOmegaStart.Value = new decimal(new int[] {
            20,
            0,
            0,
            65536});
            // 
            // txtOmegaEnd
            // 
            this.txtOmegaEnd.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtOmegaEnd.DecimalPlaces = 2;
            this.txtOmegaEnd.Location = new System.Drawing.Point(98, 3);
            this.txtOmegaEnd.Name = "txtOmegaEnd";
            this.txtOmegaEnd.Size = new System.Drawing.Size(50, 20);
            this.txtOmegaEnd.TabIndex = 2;
            this.txtOmegaEnd.Value = new decimal(new int[] {
            20,
            0,
            0,
            65536});
            // 
            // pnlNSSites
            // 
            this.pnlNSSites.AutoSize = true;
            this.pnlNSSites.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlNSSites.Location = new System.Drawing.Point(79, 27);
            this.pnlNSSites.Margin = new System.Windows.Forms.Padding(0);
            this.pnlNSSites.Name = "pnlNSSites";
            this.pnlNSSites.Size = new System.Drawing.Size(0, 0);
            this.pnlNSSites.TabIndex = 3;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.txtNCatG, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.cmbModel, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.pnlNSSites, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label7, 0, 4);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(398, 128);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // txtNCatG
            // 
            this.txtNCatG.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtNCatG.Location = new System.Drawing.Point(82, 53);
            this.txtNCatG.Name = "txtNCatG";
            this.txtNCatG.Size = new System.Drawing.Size(50, 20);
            this.txtNCatG.TabIndex = 5;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.AutoSize = true;
            this.tableLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel3.ColumnCount = 6;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.Controls.Add(this.chkFixedOmega, 5, 0);
            this.tableLayoutPanel3.Controls.Add(this.txtOmegaInterval, 4, 0);
            this.tableLayoutPanel3.Controls.Add(this.label9, 3, 0);
            this.tableLayoutPanel3.Controls.Add(this.txtOmegaEnd, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.txtOmegaStart, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.label8, 1, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(79, 102);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Size = new System.Drawing.Size(319, 26);
            this.tableLayoutPanel3.TabIndex = 9;
            // 
            // chkFixedOmega
            // 
            this.chkFixedOmega.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkFixedOmega.AutoSize = true;
            this.chkFixedOmega.Location = new System.Drawing.Point(265, 4);
            this.chkFixedOmega.Name = "chkFixedOmega";
            this.chkFixedOmega.Size = new System.Drawing.Size(51, 17);
            this.chkFixedOmega.TabIndex = 5;
            this.chkFixedOmega.Text = "Fixed";
            this.chkFixedOmega.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel2.ColumnCount = 6;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.txtKappaStart, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label5, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.txtKappaEnd, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.label6, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.txtKappaInterval, 4, 0);
            this.tableLayoutPanel2.Controls.Add(this.chkFixedKappa, 5, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(79, 76);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(319, 26);
            this.tableLayoutPanel2.TabIndex = 7;
            // 
            // chkFixedKappa
            // 
            this.chkFixedKappa.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkFixedKappa.AutoSize = true;
            this.chkFixedKappa.Location = new System.Drawing.Point(265, 4);
            this.chkFixedKappa.Name = "chkFixedKappa";
            this.chkFixedKappa.Size = new System.Drawing.Size(51, 17);
            this.chkFixedKappa.TabIndex = 5;
            this.chkFixedKappa.Text = "Fixed";
            this.chkFixedKappa.UseVisualStyleBackColor = true;
            // 
            // uctAnalysisConfiguration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "uctAnalysisConfiguration";
            this.Size = new System.Drawing.Size(398, 128);
            ((System.ComponentModel.ISupportInitialize)(this.txtKappaInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKappaStart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtKappaEnd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOmegaInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOmegaStart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOmegaEnd)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtNCatG)).EndInit();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbModel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown txtKappaInterval;
        private System.Windows.Forms.NumericUpDown txtKappaStart;
        private System.Windows.Forms.NumericUpDown txtKappaEnd;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown txtOmegaInterval;
        private System.Windows.Forms.NumericUpDown txtOmegaStart;
        private System.Windows.Forms.NumericUpDown txtOmegaEnd;
        private System.Windows.Forms.FlowLayoutPanel pnlNSSites;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.CheckBox chkFixedOmega;
        private System.Windows.Forms.CheckBox chkFixedKappa;
        private System.Windows.Forms.NumericUpDown txtNCatG;
    }
}
