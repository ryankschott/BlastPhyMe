namespace Pilgrimage.GeneSequences.Alignment.MUSCLE
{
    partial class frmCreateJob
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCreateJob));
            this.btnBrowseExe = new System.Windows.Forms.Button();
            this.txtExecutablePath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnBrowseWorkingDirectory = new System.Windows.Forms.Button();
            this.txtWorkingDirectory = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.chkKeepOutputFiles = new System.Windows.Forms.CheckBox();
            this.chkFindDiagonals = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.numMaximumIterations = new System.Windows.Forms.NumericUpDown();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.btnRun = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.numMaximumHours = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMaximumIterations)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMaximumHours)).BeginInit();
            this.SuspendLayout();
            // 
            // btnBrowseExe
            // 
            this.btnBrowseExe.Location = new System.Drawing.Point(645, 3);
            this.btnBrowseExe.Name = "btnBrowseExe";
            this.btnBrowseExe.Size = new System.Drawing.Size(24, 23);
            this.btnBrowseExe.TabIndex = 2;
            this.btnBrowseExe.Text = "...";
            this.btnBrowseExe.UseVisualStyleBackColor = true;
            this.btnBrowseExe.Click += new System.EventHandler(this.btnBrowseExe_Click);
            // 
            // txtExecutablePath
            // 
            this.txtExecutablePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtExecutablePath.Location = new System.Drawing.Point(158, 4);
            this.txtExecutablePath.Name = "txtExecutablePath";
            this.txtExecutablePath.Size = new System.Drawing.Size(481, 20);
            this.txtExecutablePath.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 8);
            this.label1.Margin = new System.Windows.Forms.Padding(5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "MUSCLE.exe";
            // 
            // btnBrowseWorkingDirectory
            // 
            this.btnBrowseWorkingDirectory.Location = new System.Drawing.Point(645, 32);
            this.btnBrowseWorkingDirectory.Name = "btnBrowseWorkingDirectory";
            this.btnBrowseWorkingDirectory.Size = new System.Drawing.Size(24, 23);
            this.btnBrowseWorkingDirectory.TabIndex = 5;
            this.btnBrowseWorkingDirectory.Text = "...";
            this.btnBrowseWorkingDirectory.UseVisualStyleBackColor = true;
            this.btnBrowseWorkingDirectory.Click += new System.EventHandler(this.btnBrowseWorkingDirectory_Click);
            // 
            // txtWorkingDirectory
            // 
            this.txtWorkingDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtWorkingDirectory.Location = new System.Drawing.Point(158, 33);
            this.txtWorkingDirectory.Name = "txtWorkingDirectory";
            this.txtWorkingDirectory.Size = new System.Drawing.Size(481, 20);
            this.txtWorkingDirectory.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 37);
            this.label2.Margin = new System.Windows.Forms.Padding(5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Working Directory:";
            // 
            // chkKeepOutputFiles
            // 
            this.chkKeepOutputFiles.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.chkKeepOutputFiles.AutoSize = true;
            this.tableLayoutPanel2.SetColumnSpan(this.chkKeepOutputFiles, 2);
            this.chkKeepOutputFiles.Location = new System.Drawing.Point(540, 63);
            this.chkKeepOutputFiles.Margin = new System.Windows.Forms.Padding(5, 5, 5, 3);
            this.chkKeepOutputFiles.Name = "chkKeepOutputFiles";
            this.chkKeepOutputFiles.Size = new System.Drawing.Size(127, 17);
            this.chkKeepOutputFiles.TabIndex = 6;
            this.chkKeepOutputFiles.Text = "Preserve Output Files";
            this.chkKeepOutputFiles.UseVisualStyleBackColor = true;
            // 
            // chkFindDiagonals
            // 
            this.chkFindDiagonals.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkFindDiagonals.AutoSize = true;
            this.chkFindDiagonals.Location = new System.Drawing.Point(160, 5);
            this.chkFindDiagonals.Margin = new System.Windows.Forms.Padding(5, 5, 0, 5);
            this.chkFindDiagonals.Name = "chkFindDiagonals";
            this.chkFindDiagonals.Size = new System.Drawing.Size(15, 14);
            this.chkFindDiagonals.TabIndex = 0;
            this.chkFindDiagonals.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.AutoSize = true;
            this.groupBox2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel4.SetColumnSpan(this.groupBox2, 2);
            this.groupBox2.Controls.Add(this.tableLayoutPanel3);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(678, 95);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Alignment Options";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.AutoSize = true;
            this.tableLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 155F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.Controls.Add(this.chkFindDiagonals, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.label4, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.numMaximumIterations, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.numMaximumHours, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.label5, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(672, 76);
            this.tableLayoutPanel3.TabIndex = 28;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 30);
            this.label4.Margin = new System.Windows.Forms.Padding(5);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Maximum Iterations:";
            // 
            // numMaximumIterations
            // 
            this.numMaximumIterations.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.numMaximumIterations.Location = new System.Drawing.Point(158, 27);
            this.numMaximumIterations.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMaximumIterations.Name = "numMaximumIterations";
            this.numMaximumIterations.Size = new System.Drawing.Size(40, 20);
            this.numMaximumIterations.TabIndex = 2;
            this.numMaximumIterations.Value = new decimal(new int[] {
            16,
            0,
            0,
            0});
            // 
            // groupBox3
            // 
            this.groupBox3.AutoSize = true;
            this.groupBox3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel4.SetColumnSpan(this.groupBox3, 2);
            this.groupBox3.Controls.Add(this.tableLayoutPanel2);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(3, 104);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(678, 102);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Job Options";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 155F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnBrowseWorkingDirectory, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.btnBrowseExe, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.txtWorkingDirectory, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.txtExecutablePath, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.chkKeepOutputFiles, 1, 2);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(672, 83);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.AutoSize = true;
            this.tableLayoutPanel4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.Controls.Add(this.btnRun, 0, 2);
            this.tableLayoutPanel4.Controls.Add(this.groupBox3, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.btnCancel, 1, 2);
            this.tableLayoutPanel4.Controls.Add(this.groupBox2, 0, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 3;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.Size = new System.Drawing.Size(684, 238);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // btnRun
            // 
            this.btnRun.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnRun.Location = new System.Drawing.Point(515, 212);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(80, 23);
            this.btnRun.TabIndex = 2;
            this.btnRun.Text = "&Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(601, 212);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 56);
            this.label3.Margin = new System.Windows.Forms.Padding(5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Maximum Hours:";
            // 
            // numMaximumHours
            // 
            this.numMaximumHours.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.numMaximumHours.Location = new System.Drawing.Point(158, 53);
            this.numMaximumHours.Name = "numMaximumHours";
            this.numMaximumHours.Size = new System.Drawing.Size(40, 20);
            this.numMaximumHours.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(5, 5);
            this.label5.Margin = new System.Windows.Forms.Padding(5);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(130, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Use diagonal optimization:";
            // 
            // frmCreateJob
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(684, 238);
            this.Controls.Add(this.tableLayoutPanel4);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(700, 272);
            this.Name = "frmCreateJob";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Configure MUSCLE";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmCreateJob_FormClosing);
            this.Load += new System.EventHandler(this.frmCreateJob_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMaximumIterations)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMaximumHours)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnBrowseExe;
        private System.Windows.Forms.TextBox txtExecutablePath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnBrowseWorkingDirectory;
        private System.Windows.Forms.TextBox txtWorkingDirectory;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkKeepOutputFiles;
        private System.Windows.Forms.CheckBox chkFindDiagonals;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numMaximumIterations;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numMaximumHours;
        private System.Windows.Forms.Label label5;
    }
}