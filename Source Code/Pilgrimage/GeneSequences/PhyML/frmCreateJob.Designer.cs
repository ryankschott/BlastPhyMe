namespace Pilgrimage.GeneSequences.PhyML
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
            this.btnBrowsePhyML = new System.Windows.Forms.Button();
            this.txtPhyMLPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnBrowseWorkingDirectory = new System.Windows.Forms.Button();
            this.txtWorkingDirectory = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.chkKeepOutput = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label20 = new System.Windows.Forms.Label();
            this.chkCopyWithoutSupportValues = new System.Windows.Forms.CheckBox();
            this.label19 = new System.Windows.Forms.Label();
            this.txtSequenceHeaderFormat = new System.Windows.Forms.TextBox();
            this.btnSequenceHeaderFormat = new System.Windows.Forms.Button();
            this.tblForm = new System.Windows.Forms.TableLayoutPanel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.uctPhyMLOptions1 = new Pilgrimage.GeneSequences.PhyML.uctPhyMLOptions();
            this.btnResetDefaults = new System.Windows.Forms.Button();
            this.btnRun = new System.Windows.Forms.Button();
            this.groupBox3.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tblForm.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnBrowsePhyML
            // 
            this.btnBrowsePhyML.Location = new System.Drawing.Point(705, 62);
            this.btnBrowsePhyML.Name = "btnBrowsePhyML";
            this.btnBrowsePhyML.Size = new System.Drawing.Size(24, 23);
            this.btnBrowsePhyML.TabIndex = 7;
            this.btnBrowsePhyML.Text = "...";
            this.btnBrowsePhyML.UseVisualStyleBackColor = true;
            this.btnBrowsePhyML.Click += new System.EventHandler(this.btnBrowsePRANK_Click);
            // 
            // txtPhyMLPath
            // 
            this.txtPhyMLPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPhyMLPath.Location = new System.Drawing.Point(169, 63);
            this.txtPhyMLPath.Name = "txtPhyMLPath";
            this.txtPhyMLPath.Size = new System.Drawing.Size(530, 20);
            this.txtPhyMLPath.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 67);
            this.label1.Margin = new System.Windows.Forms.Padding(5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "PhyML.exe";
            // 
            // btnBrowseWorkingDirectory
            // 
            this.btnBrowseWorkingDirectory.Location = new System.Drawing.Point(705, 91);
            this.btnBrowseWorkingDirectory.Name = "btnBrowseWorkingDirectory";
            this.btnBrowseWorkingDirectory.Size = new System.Drawing.Size(24, 23);
            this.btnBrowseWorkingDirectory.TabIndex = 10;
            this.btnBrowseWorkingDirectory.Text = "...";
            this.btnBrowseWorkingDirectory.UseVisualStyleBackColor = true;
            this.btnBrowseWorkingDirectory.Click += new System.EventHandler(this.btnBrowseWorkingDirectory_Click);
            // 
            // txtWorkingDirectory
            // 
            this.txtWorkingDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtWorkingDirectory.Location = new System.Drawing.Point(169, 92);
            this.txtWorkingDirectory.Name = "txtWorkingDirectory";
            this.txtWorkingDirectory.Size = new System.Drawing.Size(530, 20);
            this.txtWorkingDirectory.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 96);
            this.label2.Margin = new System.Windows.Forms.Padding(5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Working Directory:";
            // 
            // chkKeepOutput
            // 
            this.chkKeepOutput.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.chkKeepOutput.AutoSize = true;
            this.tableLayoutPanel2.SetColumnSpan(this.chkKeepOutput, 2);
            this.chkKeepOutput.Enabled = false;
            this.chkKeepOutput.Location = new System.Drawing.Point(600, 122);
            this.chkKeepOutput.Margin = new System.Windows.Forms.Padding(5, 5, 5, 3);
            this.chkKeepOutput.Name = "chkKeepOutput";
            this.chkKeepOutput.Size = new System.Drawing.Size(127, 17);
            this.chkKeepOutput.TabIndex = 11;
            this.chkKeepOutput.Text = "Preserve Output Files";
            this.chkKeepOutput.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.AutoSize = true;
            this.groupBox3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tblForm.SetColumnSpan(this.groupBox3, 3);
            this.groupBox3.Controls.Add(this.tableLayoutPanel2);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(3, 477);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(738, 161);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Job Options";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 166F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.label20, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.btnBrowseWorkingDirectory, 2, 3);
            this.tableLayoutPanel2.Controls.Add(this.btnBrowsePhyML, 2, 2);
            this.tableLayoutPanel2.Controls.Add(this.label2, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.chkCopyWithoutSupportValues, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.txtWorkingDirectory, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.txtPhyMLPath, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.chkKeepOutput, 1, 4);
            this.tableLayoutPanel2.Controls.Add(this.label19, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.txtSequenceHeaderFormat, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnSequenceHeaderFormat, 2, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 5;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(732, 142);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // label20
            // 
            this.label20.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(5, 31);
            this.label20.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(143, 26);
            this.label20.TabIndex = 3;
            this.label20.Text = "Create a copy of the tree file without support values:";
            // 
            // chkCopyWithoutSupportValues
            // 
            this.chkCopyWithoutSupportValues.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkCopyWithoutSupportValues.AutoSize = true;
            this.chkCopyWithoutSupportValues.Location = new System.Drawing.Point(169, 37);
            this.chkCopyWithoutSupportValues.Margin = new System.Windows.Forms.Padding(3, 6, 6, 6);
            this.chkCopyWithoutSupportValues.Name = "chkCopyWithoutSupportValues";
            this.chkCopyWithoutSupportValues.Size = new System.Drawing.Size(15, 14);
            this.chkCopyWithoutSupportValues.TabIndex = 4;
            this.chkCopyWithoutSupportValues.UseVisualStyleBackColor = true;
            // 
            // label19
            // 
            this.label19.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(5, 8);
            this.label19.Margin = new System.Windows.Forms.Padding(5);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(116, 13);
            this.label19.TabIndex = 0;
            this.label19.Text = "Sequence label format:";
            // 
            // txtSequenceHeaderFormat
            // 
            this.txtSequenceHeaderFormat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSequenceHeaderFormat.Location = new System.Drawing.Point(169, 4);
            this.txtSequenceHeaderFormat.Name = "txtSequenceHeaderFormat";
            this.txtSequenceHeaderFormat.Size = new System.Drawing.Size(530, 20);
            this.txtSequenceHeaderFormat.TabIndex = 1;
            // 
            // btnSequenceHeaderFormat
            // 
            this.btnSequenceHeaderFormat.Location = new System.Drawing.Point(705, 3);
            this.btnSequenceHeaderFormat.Name = "btnSequenceHeaderFormat";
            this.btnSequenceHeaderFormat.Size = new System.Drawing.Size(24, 23);
            this.btnSequenceHeaderFormat.TabIndex = 2;
            this.btnSequenceHeaderFormat.Text = "...";
            this.btnSequenceHeaderFormat.UseVisualStyleBackColor = true;
            this.btnSequenceHeaderFormat.Click += new System.EventHandler(this.btnSequenceHeaderFormat_Click);
            // 
            // tblForm
            // 
            this.tblForm.AutoSize = true;
            this.tblForm.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tblForm.ColumnCount = 3;
            this.tblForm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tblForm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblForm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tblForm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tblForm.Controls.Add(this.groupBox3, 0, 1);
            this.tblForm.Controls.Add(this.btnCancel, 2, 2);
            this.tblForm.Controls.Add(this.uctPhyMLOptions1, 0, 0);
            this.tblForm.Controls.Add(this.btnResetDefaults, 0, 2);
            this.tblForm.Controls.Add(this.btnRun, 1, 2);
            this.tblForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblForm.Location = new System.Drawing.Point(0, 0);
            this.tblForm.Name = "tblForm";
            this.tblForm.RowCount = 3;
            this.tblForm.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblForm.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblForm.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblForm.Size = new System.Drawing.Size(744, 671);
            this.tblForm.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(661, 644);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // uctPhyMLOptions1
            // 
            this.uctPhyMLOptions1.AutoSize = true;
            this.uctPhyMLOptions1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tblForm.SetColumnSpan(this.uctPhyMLOptions1, 3);
            this.uctPhyMLOptions1.Location = new System.Drawing.Point(0, 0);
            this.uctPhyMLOptions1.Margin = new System.Windows.Forms.Padding(0);
            this.uctPhyMLOptions1.Name = "uctPhyMLOptions1";
            this.uctPhyMLOptions1.Size = new System.Drawing.Size(744, 474);
            this.uctPhyMLOptions1.TabIndex = 0;
            // 
            // btnResetDefaults
            // 
            this.btnResetDefaults.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnResetDefaults.Location = new System.Drawing.Point(3, 644);
            this.btnResetDefaults.Name = "btnResetDefaults";
            this.btnResetDefaults.Size = new System.Drawing.Size(165, 23);
            this.btnResetDefaults.TabIndex = 3;
            this.btnResetDefaults.Text = "&Reset to Default Options";
            this.btnResetDefaults.UseVisualStyleBackColor = true;
            this.btnResetDefaults.Click += new System.EventHandler(this.btnResetDefaults_Click);
            // 
            // btnRun
            // 
            this.btnRun.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnRun.Location = new System.Drawing.Point(575, 644);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(80, 23);
            this.btnRun.TabIndex = 2;
            this.btnRun.Text = "&Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // frmCreateJob
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(744, 671);
            this.Controls.Add(this.tblForm);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(750, 695);
            this.Name = "frmCreateJob";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Configure PhyML";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmCreateJob_FormClosing);
            this.Load += new System.EventHandler(this.frmCreateJob_Load);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tblForm.ResumeLayout(false);
            this.tblForm.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnBrowsePhyML;
        private System.Windows.Forms.TextBox txtPhyMLPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnBrowseWorkingDirectory;
        private System.Windows.Forms.TextBox txtWorkingDirectory;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkKeepOutput;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TableLayoutPanel tblForm;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnResetDefaults;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox txtSequenceHeaderFormat;
        private System.Windows.Forms.Button btnSequenceHeaderFormat;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.CheckBox chkCopyWithoutSupportValues;
        private uctPhyMLOptions uctPhyMLOptions1;
    }
}