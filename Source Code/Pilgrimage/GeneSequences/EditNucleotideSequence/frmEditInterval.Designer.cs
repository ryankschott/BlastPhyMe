namespace Pilgrimage.GeneSequences.EditNucleotideSequence
{
    partial class frmEditInterval
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
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.rtfNucleotideSequence = new System.Windows.Forms.RichTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.chkComplement = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.numEndIndex = new System.Windows.Forms.NumericUpDown();
            this.numStartIndex = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.txtIntervalSequence = new System.Windows.Forms.TextBox();
            this.lnkCaptureSelection = new System.Windows.Forms.LinkLabel();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numEndIndex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStartIndex)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Start:";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 6;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.34F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tableLayoutPanel1.Controls.Add(this.rtfNucleotideSequence, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.chkComplement, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.label3, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 4, 5);
            this.tableLayoutPanel1.Controls.Add(this.numEndIndex, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.numStartIndex, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.txtIntervalSequence, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.lnkCaptureSelection, 3, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(484, 408);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // rtfNucleotideSequence
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.rtfNucleotideSequence, 9);
            this.rtfNucleotideSequence.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtfNucleotideSequence.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtfNucleotideSequence.Location = new System.Drawing.Point(3, 52);
            this.rtfNucleotideSequence.Name = "rtfNucleotideSequence";
            this.rtfNucleotideSequence.ReadOnly = true;
            this.rtfNucleotideSequence.Size = new System.Drawing.Size(478, 208);
            this.rtfNucleotideSequence.TabIndex = 7;
            this.rtfNucleotideSequence.Text = "";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label4, 2);
            this.label4.Location = new System.Drawing.Point(3, 33);
            this.label4.Margin = new System.Windows.Forms.Padding(3, 7, 3, 3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Source Sequence:";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(149, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "End:";
            // 
            // chkComplement
            // 
            this.chkComplement.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkComplement.AutoSize = true;
            this.chkComplement.Location = new System.Drawing.Point(377, 7);
            this.chkComplement.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.chkComplement.Name = "chkComplement";
            this.chkComplement.Size = new System.Drawing.Size(15, 14);
            this.chkComplement.TabIndex = 5;
            this.chkComplement.UseVisualStyleBackColor = true;
            this.chkComplement.CheckedChanged += new System.EventHandler(this.chkComplement_ValueChanged);
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(292, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Is Complement:";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel1.SetColumnSpan(this.tableLayoutPanel2, 2);
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.btnSave, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnCancel, 1, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(322, 378);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(162, 29);
            this.tableLayoutPanel2.TabIndex = 11;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnSave.Location = new System.Drawing.Point(3, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnCancel.Location = new System.Drawing.Point(84, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // numEndIndex
            // 
            this.numEndIndex.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.numEndIndex.Location = new System.Drawing.Point(184, 3);
            this.numEndIndex.Name = "numEndIndex";
            this.numEndIndex.Size = new System.Drawing.Size(52, 20);
            this.numEndIndex.TabIndex = 3;
            this.numEndIndex.ValueChanged += new System.EventHandler(this.numEndIndex_ValueChanged);
            // 
            // numStartIndex
            // 
            this.numStartIndex.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.numStartIndex.Location = new System.Drawing.Point(41, 3);
            this.numStartIndex.Name = "numStartIndex";
            this.numStartIndex.Size = new System.Drawing.Size(52, 20);
            this.numStartIndex.TabIndex = 1;
            this.numStartIndex.ValueChanged += new System.EventHandler(this.numStartIndex_ValueChanged);
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label5, 2);
            this.label5.Location = new System.Drawing.Point(3, 270);
            this.label5.Margin = new System.Windows.Forms.Padding(3, 7, 3, 3);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(97, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Interval Sequence:";
            // 
            // txtIntervalSequence
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.txtIntervalSequence, 6);
            this.txtIntervalSequence.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtIntervalSequence.Location = new System.Drawing.Point(3, 289);
            this.txtIntervalSequence.Multiline = true;
            this.txtIntervalSequence.Name = "txtIntervalSequence";
            this.txtIntervalSequence.ReadOnly = true;
            this.txtIntervalSequence.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtIntervalSequence.Size = new System.Drawing.Size(478, 86);
            this.txtIntervalSequence.TabIndex = 10;
            // 
            // lnkCaptureSelection
            // 
            this.lnkCaptureSelection.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lnkCaptureSelection.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lnkCaptureSelection, 3);
            this.lnkCaptureSelection.Location = new System.Drawing.Point(338, 268);
            this.lnkCaptureSelection.Name = "lnkCaptureSelection";
            this.lnkCaptureSelection.Size = new System.Drawing.Size(143, 13);
            this.lnkCaptureSelection.TabIndex = 9;
            this.lnkCaptureSelection.TabStop = true;
            this.lnkCaptureSelection.Text = "Capture Selection as Interval";
            this.lnkCaptureSelection.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkCaptureSelection_LinkClicked);
            // 
            // frmEditInterval
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(484, 408);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(500, 250);
            this.Name = "frmEditInterval";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add/Edit Interval";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numEndIndex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numStartIndex)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox chkComplement;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.RichTextBox rtfNucleotideSequence;
        private System.Windows.Forms.NumericUpDown numEndIndex;
        private System.Windows.Forms.NumericUpDown numStartIndex;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtIntervalSequence;
        private System.Windows.Forms.LinkLabel lnkCaptureSelection;
    }
}