namespace Pilgrimage.GeneSequences.BlastN
{
    partial class frmBlastNOptions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBlastNOptions));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.numWordSize = new System.Windows.Forms.NumericUpDown();
            this.cmbMatchMismatchScores = new System.Windows.Forms.ComboBox();
            this.cmbGapCosts = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.numMaxTargetSequences = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.numExpect = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.grpNCBI = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.txtNCBIDatabaseName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.rbServiceBlastN = new System.Windows.Forms.RadioButton();
            this.rbServiceMegablast = new System.Windows.Forms.RadioButton();
            this.rbServiceDCMegablast = new System.Windows.Forms.RadioButton();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numWordSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxTargetSequences)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numExpect)).BeginInit();
            this.grpNCBI.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.grpNCBI, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnOK, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnCancel, 2, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(390, 301);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSize = true;
            this.groupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.SetColumnSpan(this.groupBox1, 3);
            this.groupBox1.Controls.Add(this.tableLayoutPanel3);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 118);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(384, 151);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Algorithm Parameters";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.AutoSize = true;
            this.tableLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.Controls.Add(this.numWordSize, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.cmbMatchMismatchScores, 1, 3);
            this.tableLayoutPanel3.Controls.Add(this.cmbGapCosts, 1, 4);
            this.tableLayoutPanel3.Controls.Add(this.label6, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.numMaxTargetSequences, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.label4, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.label9, 0, 4);
            this.tableLayoutPanel3.Controls.Add(this.numExpect, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.label8, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.label5, 0, 2);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 5;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Size = new System.Drawing.Size(378, 132);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // numWordSize
            // 
            this.numWordSize.Location = new System.Drawing.Point(137, 55);
            this.numWordSize.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numWordSize.Name = "numWordSize";
            this.numWordSize.Size = new System.Drawing.Size(50, 20);
            this.numWordSize.TabIndex = 5;
            // 
            // cmbMatchMismatchScores
            // 
            this.cmbMatchMismatchScores.DisplayMember = "Value";
            this.cmbMatchMismatchScores.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMatchMismatchScores.FormattingEnabled = true;
            this.cmbMatchMismatchScores.Location = new System.Drawing.Point(137, 81);
            this.cmbMatchMismatchScores.Name = "cmbMatchMismatchScores";
            this.cmbMatchMismatchScores.Size = new System.Drawing.Size(60, 21);
            this.cmbMatchMismatchScores.TabIndex = 7;
            this.cmbMatchMismatchScores.ValueMember = "Value";
            // 
            // cmbGapCosts
            // 
            this.cmbGapCosts.DisplayMember = "Value";
            this.cmbGapCosts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGapCosts.FormattingEnabled = true;
            this.cmbGapCosts.Location = new System.Drawing.Point(137, 108);
            this.cmbGapCosts.Name = "cmbGapCosts";
            this.cmbGapCosts.Size = new System.Drawing.Size(145, 21);
            this.cmbGapCosts.TabIndex = 9;
            this.cmbGapCosts.ValueMember = "Key";
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(5, 6);
            this.label6.Margin = new System.Windows.Forms.Padding(5);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(115, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Max target sequences:";
            // 
            // numMaxTargetSequences
            // 
            this.numMaxTargetSequences.Location = new System.Drawing.Point(137, 3);
            this.numMaxTargetSequences.Maximum = new decimal(new int[] {
            20000,
            0,
            0,
            0});
            this.numMaxTargetSequences.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMaxTargetSequences.Name = "numMaxTargetSequences";
            this.numMaxTargetSequences.Size = new System.Drawing.Size(50, 20);
            this.numMaxTargetSequences.TabIndex = 1;
            this.numMaxTargetSequences.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 31);
            this.label4.Margin = new System.Windows.Forms.Padding(5);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Expect threshold:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(5, 110);
            this.label9.Margin = new System.Windows.Forms.Padding(5);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(58, 13);
            this.label9.TabIndex = 8;
            this.label9.Text = "Gap costs:";
            // 
            // numExpect
            // 
            this.numExpect.DecimalPlaces = 2;
            this.numExpect.Location = new System.Drawing.Point(137, 29);
            this.numExpect.Name = "numExpect";
            this.numExpect.Size = new System.Drawing.Size(50, 20);
            this.numExpect.TabIndex = 3;
            this.numExpect.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(5, 83);
            this.label8.Margin = new System.Windows.Forms.Padding(5);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(124, 13);
            this.label8.TabIndex = 6;
            this.label8.Text = "Match/Mismatch scores:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(5, 57);
            this.label5.Margin = new System.Windows.Forms.Padding(5);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Word size:";
            // 
            // grpNCBI
            // 
            this.grpNCBI.AutoSize = true;
            this.grpNCBI.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.SetColumnSpan(this.grpNCBI, 3);
            this.grpNCBI.Controls.Add(this.tableLayoutPanel2);
            this.grpNCBI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpNCBI.Location = new System.Drawing.Point(3, 0);
            this.grpNCBI.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.grpNCBI.Name = "grpNCBI";
            this.grpNCBI.Size = new System.Drawing.Size(384, 112);
            this.grpNCBI.TabIndex = 1;
            this.grpNCBI.TabStop = false;
            this.grpNCBI.Text = "Search Program";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.txtNCBIDatabaseName, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.rbServiceBlastN, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.rbServiceMegablast, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.rbServiceDCMegablast, 1, 2);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(378, 93);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 6);
            this.label1.Margin = new System.Windows.Forms.Padding(5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Database name:";
            // 
            // txtNCBIDatabaseName
            // 
            this.txtNCBIDatabaseName.Location = new System.Drawing.Point(98, 3);
            this.txtNCBIDatabaseName.MaxLength = 250;
            this.txtNCBIDatabaseName.Name = "txtNCBIDatabaseName";
            this.txtNCBIDatabaseName.Size = new System.Drawing.Size(100, 20);
            this.txtNCBIDatabaseName.TabIndex = 1;
            this.txtNCBIDatabaseName.Text = "nr";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 31);
            this.label2.Margin = new System.Windows.Forms.Padding(5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Optimize for:";
            // 
            // rbServiceBlastN
            // 
            this.rbServiceBlastN.AutoSize = true;
            this.rbServiceBlastN.Checked = true;
            this.rbServiceBlastN.Location = new System.Drawing.Point(100, 71);
            this.rbServiceBlastN.Margin = new System.Windows.Forms.Padding(5, 0, 5, 5);
            this.rbServiceBlastN.Name = "rbServiceBlastN";
            this.rbServiceBlastN.Size = new System.Drawing.Size(198, 17);
            this.rbServiceBlastN.TabIndex = 5;
            this.rbServiceBlastN.TabStop = true;
            this.rbServiceBlastN.Text = "Somewhat similar sequences (blastn)";
            this.rbServiceBlastN.UseVisualStyleBackColor = true;
            // 
            // rbServiceMegablast
            // 
            this.rbServiceMegablast.AutoSize = true;
            this.rbServiceMegablast.Location = new System.Drawing.Point(100, 26);
            this.rbServiceMegablast.Margin = new System.Windows.Forms.Padding(5, 0, 5, 5);
            this.rbServiceMegablast.Name = "rbServiceMegablast";
            this.rbServiceMegablast.Size = new System.Drawing.Size(197, 17);
            this.rbServiceMegablast.TabIndex = 3;
            this.rbServiceMegablast.Text = "Highly similar sequences (megablast)";
            this.rbServiceMegablast.UseVisualStyleBackColor = true;
            // 
            // rbServiceDCMegablast
            // 
            this.rbServiceDCMegablast.AutoSize = true;
            this.rbServiceDCMegablast.Location = new System.Drawing.Point(100, 49);
            this.rbServiceDCMegablast.Margin = new System.Windows.Forms.Padding(5, 0, 5, 5);
            this.rbServiceDCMegablast.Name = "rbServiceDCMegablast";
            this.rbServiceDCMegablast.Size = new System.Drawing.Size(273, 17);
            this.rbServiceDCMegablast.TabIndex = 4;
            this.rbServiceDCMegablast.Text = "More dissimilar sequences (discontinuous megablast)";
            this.rbServiceDCMegablast.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(221, 275);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 23);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "&Submit";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(307, 275);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // frmBlastNOptions
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(390, 301);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmBlastNOptions";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "BLASTN Options";
            this.Load += new System.EventHandler(this.frmBlastNOptions_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numWordSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxTargetSequences)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numExpect)).EndInit();
            this.grpNCBI.ResumeLayout(false);
            this.grpNCBI.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox grpNCBI;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TextBox txtNCBIDatabaseName;
        private System.Windows.Forms.RadioButton rbServiceMegablast;
        private System.Windows.Forms.RadioButton rbServiceBlastN;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numMaxTargetSequences;
        private System.Windows.Forms.RadioButton rbServiceDCMegablast;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.NumericUpDown numWordSize;
        private System.Windows.Forms.ComboBox cmbMatchMismatchScores;
        private System.Windows.Forms.ComboBox cmbGapCosts;
        private System.Windows.Forms.NumericUpDown numExpect;
    }
}