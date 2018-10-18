namespace Pilgrimage.GeneSequences.BlastN
{
    partial class frmBlastNLocalOptions
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.rbTargetLocal = new System.Windows.Forms.RadioButton();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.grpLocal = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.txtLocal_DatabaseFile = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtLocal_BlastNExeDirectory = new System.Windows.Forms.TextBox();
            this.txtLocal_OutputDirectory = new System.Windows.Forms.TextBox();
            this.btnLocal_DatabaseFile = new System.Windows.Forms.Button();
            this.btnLocal_BlastNExeDirectory = new System.Windows.Forms.Button();
            this.btnLocal_OutputDirectory = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.grpLocal.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
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
            this.tableLayoutPanel1.Controls.Add(this.rbTargetLocal, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnOK, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnCancel, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.grpLocal, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(590, 166);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // rbTargetLocal
            // 
            this.rbTargetLocal.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.rbTargetLocal, 3);
            this.rbTargetLocal.Location = new System.Drawing.Point(5, 5);
            this.rbTargetLocal.Margin = new System.Windows.Forms.Padding(5);
            this.rbTargetLocal.Name = "rbTargetLocal";
            this.rbTargetLocal.Size = new System.Drawing.Size(265, 17);
            this.rbTargetLocal.TabIndex = 2;
            this.rbTargetLocal.Text = "Query a local nucleotide database using blastn.exe";
            this.rbTargetLocal.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(421, 140);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "&Submit";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(507, 140);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // grpLocal
            // 
            this.grpLocal.AutoSize = true;
            this.grpLocal.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.SetColumnSpan(this.grpLocal, 3);
            this.grpLocal.Controls.Add(this.tableLayoutPanel3);
            this.grpLocal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpLocal.Location = new System.Drawing.Point(3, 27);
            this.grpLocal.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.grpLocal.Name = "grpLocal";
            this.grpLocal.Size = new System.Drawing.Size(584, 107);
            this.grpLocal.TabIndex = 3;
            this.grpLocal.TabStop = false;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.AutoSize = true;
            this.tableLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.txtLocal_DatabaseFile, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.label4, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.label5, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.txtLocal_BlastNExeDirectory, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.txtLocal_OutputDirectory, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.btnLocal_DatabaseFile, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnLocal_BlastNExeDirectory, 2, 1);
            this.tableLayoutPanel3.Controls.Add(this.btnLocal_OutputDirectory, 2, 2);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Size = new System.Drawing.Size(578, 88);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 8);
            this.label3.Margin = new System.Windows.Forms.Padding(5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Database file:";
            // 
            // txtLocal_DatabaseFile
            // 
            this.txtLocal_DatabaseFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLocal_DatabaseFile.Location = new System.Drawing.Point(131, 4);
            this.txtLocal_DatabaseFile.MaxLength = 250;
            this.txtLocal_DatabaseFile.Name = "txtLocal_DatabaseFile";
            this.txtLocal_DatabaseFile.Size = new System.Drawing.Size(414, 20);
            this.txtLocal_DatabaseFile.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 37);
            this.label4.Margin = new System.Windows.Forms.Padding(5);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(118, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Directory for blastn.exe:";
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(5, 66);
            this.label5.Margin = new System.Windows.Forms.Padding(5);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Output file directory:";
            // 
            // txtLocal_BlastNExeDirectory
            // 
            this.txtLocal_BlastNExeDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLocal_BlastNExeDirectory.Location = new System.Drawing.Point(131, 33);
            this.txtLocal_BlastNExeDirectory.MaxLength = 250;
            this.txtLocal_BlastNExeDirectory.Name = "txtLocal_BlastNExeDirectory";
            this.txtLocal_BlastNExeDirectory.Size = new System.Drawing.Size(414, 20);
            this.txtLocal_BlastNExeDirectory.TabIndex = 4;
            // 
            // txtLocal_OutputDirectory
            // 
            this.txtLocal_OutputDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLocal_OutputDirectory.Location = new System.Drawing.Point(131, 63);
            this.txtLocal_OutputDirectory.MaxLength = 250;
            this.txtLocal_OutputDirectory.Name = "txtLocal_OutputDirectory";
            this.txtLocal_OutputDirectory.Size = new System.Drawing.Size(414, 20);
            this.txtLocal_OutputDirectory.TabIndex = 7;
            // 
            // btnLocal_DatabaseFile
            // 
            this.btnLocal_DatabaseFile.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnLocal_DatabaseFile.Location = new System.Drawing.Point(551, 3);
            this.btnLocal_DatabaseFile.Name = "btnLocal_DatabaseFile";
            this.btnLocal_DatabaseFile.Size = new System.Drawing.Size(24, 23);
            this.btnLocal_DatabaseFile.TabIndex = 2;
            this.btnLocal_DatabaseFile.Text = "...";
            this.btnLocal_DatabaseFile.UseVisualStyleBackColor = true;
            // 
            // btnLocal_BlastNExeDirectory
            // 
            this.btnLocal_BlastNExeDirectory.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnLocal_BlastNExeDirectory.Location = new System.Drawing.Point(551, 32);
            this.btnLocal_BlastNExeDirectory.Name = "btnLocal_BlastNExeDirectory";
            this.btnLocal_BlastNExeDirectory.Size = new System.Drawing.Size(24, 23);
            this.btnLocal_BlastNExeDirectory.TabIndex = 5;
            this.btnLocal_BlastNExeDirectory.Text = "...";
            this.btnLocal_BlastNExeDirectory.UseVisualStyleBackColor = true;
            // 
            // btnLocal_OutputDirectory
            // 
            this.btnLocal_OutputDirectory.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnLocal_OutputDirectory.Location = new System.Drawing.Point(551, 61);
            this.btnLocal_OutputDirectory.Name = "btnLocal_OutputDirectory";
            this.btnLocal_OutputDirectory.Size = new System.Drawing.Size(24, 23);
            this.btnLocal_OutputDirectory.TabIndex = 8;
            this.btnLocal_OutputDirectory.Text = "...";
            this.btnLocal_OutputDirectory.UseVisualStyleBackColor = true;
            // 
            // frmBlastNLocalOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(590, 166);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "frmBlastNLocalOptions";
            this.Text = "frmBlastNLocalOptions";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.grpLocal.ResumeLayout(false);
            this.grpLocal.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.RadioButton rbTargetLocal;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox grpLocal;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtLocal_DatabaseFile;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtLocal_BlastNExeDirectory;
        private System.Windows.Forms.TextBox txtLocal_OutputDirectory;
        private System.Windows.Forms.Button btnLocal_DatabaseFile;
        private System.Windows.Forms.Button btnLocal_BlastNExeDirectory;
        private System.Windows.Forms.Button btnLocal_OutputDirectory;
    }
}