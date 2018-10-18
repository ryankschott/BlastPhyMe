namespace Pilgrimage.GeneSequences.Alignment.PRANK
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
            this.btnBrowsePRANK = new System.Windows.Forms.Button();
            this.txtPRANKPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnBrowseWorkingDirectory = new System.Windows.Forms.Button();
            this.txtWorkingDirectory = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.chkKeepOutputFiles = new System.Windows.Forms.CheckBox();
            this.chkF = new System.Windows.Forms.CheckBox();
            this.chkCodon = new System.Windows.Forms.CheckBox();
            this.chkTranslate = new System.Windows.Forms.CheckBox();
            this.chkMTTranslate = new System.Windows.Forms.CheckBox();
            this.chkKeep = new System.Windows.Forms.CheckBox();
            this.chkShowAnc = new System.Windows.Forms.CheckBox();
            this.chkShowTree = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtGuideTreePath = new System.Windows.Forms.TextBox();
            this.btnBrowseGuideTree = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.chkShowEvents = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.numIterations = new System.Windows.Forms.NumericUpDown();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.btnRun = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numIterations)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnBrowsePRANK
            // 
            this.btnBrowsePRANK.Location = new System.Drawing.Point(645, 3);
            this.btnBrowsePRANK.Name = "btnBrowsePRANK";
            this.btnBrowsePRANK.Size = new System.Drawing.Size(24, 23);
            this.btnBrowsePRANK.TabIndex = 2;
            this.btnBrowsePRANK.Text = "...";
            this.btnBrowsePRANK.UseVisualStyleBackColor = true;
            this.btnBrowsePRANK.Click += new System.EventHandler(this.btnBrowsePRANK_Click);
            // 
            // txtPRANKPath
            // 
            this.txtPRANKPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPRANKPath.Location = new System.Drawing.Point(158, 4);
            this.txtPRANKPath.Name = "txtPRANKPath";
            this.txtPRANKPath.Size = new System.Drawing.Size(481, 20);
            this.txtPRANKPath.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 8);
            this.label1.Margin = new System.Windows.Forms.Padding(5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "PRANK.exe";
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
            // chkKeepOutput
            // 
            this.chkKeepOutputFiles.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.chkKeepOutputFiles.AutoSize = true;
            this.tableLayoutPanel2.SetColumnSpan(this.chkKeepOutputFiles, 2);
            this.chkKeepOutputFiles.Location = new System.Drawing.Point(540, 63);
            this.chkKeepOutputFiles.Margin = new System.Windows.Forms.Padding(5, 5, 5, 3);
            this.chkKeepOutputFiles.Name = "chkKeepOutput";
            this.chkKeepOutputFiles.Size = new System.Drawing.Size(127, 17);
            this.chkKeepOutputFiles.TabIndex = 6;
            this.chkKeepOutputFiles.Text = "Preserve Output Files";
            this.chkKeepOutputFiles.UseVisualStyleBackColor = true;
            // 
            // chkF
            // 
            this.chkF.AutoSize = true;
            this.chkF.Location = new System.Drawing.Point(5, 5);
            this.chkF.Margin = new System.Windows.Forms.Padding(5, 5, 0, 5);
            this.chkF.Name = "chkF";
            this.chkF.Size = new System.Drawing.Size(115, 17);
            this.chkF.TabIndex = 0;
            this.chkF.Text = "Trust insertions (-F)";
            this.chkF.UseVisualStyleBackColor = true;
            // 
            // chkCodon
            // 
            this.chkCodon.AutoSize = true;
            this.chkCodon.Location = new System.Drawing.Point(228, 5);
            this.chkCodon.Margin = new System.Windows.Forms.Padding(5, 5, 0, 5);
            this.chkCodon.Name = "chkCodon";
            this.chkCodon.Size = new System.Drawing.Size(147, 17);
            this.chkCodon.TabIndex = 2;
            this.chkCodon.Text = "Codon alignment (-codon)";
            this.chkCodon.UseVisualStyleBackColor = true;
            // 
            // chkTranslate
            // 
            this.chkTranslate.AutoSize = true;
            this.chkTranslate.Location = new System.Drawing.Point(228, 32);
            this.chkTranslate.Margin = new System.Windows.Forms.Padding(5, 5, 0, 5);
            this.chkTranslate.Name = "chkTranslate";
            this.chkTranslate.Size = new System.Drawing.Size(172, 17);
            this.chkTranslate.TabIndex = 3;
            this.chkTranslate.Text = "Standard translation (-translate)";
            this.chkTranslate.UseVisualStyleBackColor = true;
            // 
            // chkMTTranslate
            // 
            this.chkMTTranslate.AutoSize = true;
            this.chkMTTranslate.Location = new System.Drawing.Point(228, 59);
            this.chkMTTranslate.Margin = new System.Windows.Forms.Padding(5, 5, 0, 5);
            this.chkMTTranslate.Name = "chkMTTranslate";
            this.chkMTTranslate.Size = new System.Drawing.Size(203, 17);
            this.chkMTTranslate.TabIndex = 4;
            this.chkMTTranslate.Text = "Mitochondrial translation (-mttranslate)";
            this.chkMTTranslate.UseVisualStyleBackColor = true;
            // 
            // chkKeep
            // 
            this.chkKeep.AutoSize = true;
            this.chkKeep.Location = new System.Drawing.Point(5, 32);
            this.chkKeep.Margin = new System.Windows.Forms.Padding(5, 5, 0, 5);
            this.chkKeep.Name = "chkKeep";
            this.chkKeep.Size = new System.Drawing.Size(161, 17);
            this.chkKeep.TabIndex = 1;
            this.chkKeep.Text = "Keep gap characters (-keep)";
            this.chkKeep.UseVisualStyleBackColor = true;
            // 
            // chkShowAnc
            // 
            this.chkShowAnc.AutoSize = true;
            this.chkShowAnc.Location = new System.Drawing.Point(452, 32);
            this.chkShowAnc.Margin = new System.Windows.Forms.Padding(5, 5, 0, 5);
            this.chkShowAnc.Name = "chkShowAnc";
            this.chkShowAnc.Size = new System.Drawing.Size(214, 17);
            this.chkShowAnc.TabIndex = 6;
            this.chkShowAnc.Text = "Output ancestral sequences (-showanc)";
            this.chkShowAnc.UseVisualStyleBackColor = true;
            // 
            // chkShowTree
            // 
            this.chkShowTree.AutoSize = true;
            this.chkShowTree.Location = new System.Drawing.Point(452, 5);
            this.chkShowTree.Margin = new System.Windows.Forms.Padding(5, 5, 0, 5);
            this.chkShowTree.Name = "chkShowTree";
            this.chkShowTree.Size = new System.Drawing.Size(163, 17);
            this.chkShowTree.TabIndex = 5;
            this.chkShowTree.Text = "Output guide tree (-showtree)";
            this.chkShowTree.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(5, 115);
            this.label6.Margin = new System.Windows.Forms.Padding(5);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "Guide tree:";
            // 
            // txtGuideTreePath
            // 
            this.txtGuideTreePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtGuideTreePath.Location = new System.Drawing.Point(158, 111);
            this.txtGuideTreePath.Name = "txtGuideTreePath";
            this.txtGuideTreePath.Size = new System.Drawing.Size(481, 20);
            this.txtGuideTreePath.TabIndex = 4;
            // 
            // btnBrowseGuideTree
            // 
            this.btnBrowseGuideTree.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnBrowseGuideTree.Location = new System.Drawing.Point(645, 110);
            this.btnBrowseGuideTree.Name = "btnBrowseGuideTree";
            this.btnBrowseGuideTree.Size = new System.Drawing.Size(24, 23);
            this.btnBrowseGuideTree.TabIndex = 5;
            this.btnBrowseGuideTree.Text = "...";
            this.btnBrowseGuideTree.UseVisualStyleBackColor = true;
            this.btnBrowseGuideTree.Click += new System.EventHandler(this.btnBrowseGuideTree_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel3.SetColumnSpan(this.tableLayoutPanel1, 3);
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.34F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tableLayoutPanel1.Controls.Add(this.chkShowEvents, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.chkF, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.chkShowAnc, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.chkShowTree, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.chkKeep, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.chkCodon, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.chkMTTranslate, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.chkTranslate, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(672, 81);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // chkShowEvents
            // 
            this.chkShowEvents.AutoSize = true;
            this.chkShowEvents.Location = new System.Drawing.Point(452, 59);
            this.chkShowEvents.Margin = new System.Windows.Forms.Padding(5, 5, 0, 5);
            this.chkShowEvents.Name = "chkShowEvents";
            this.chkShowEvents.Size = new System.Drawing.Size(216, 17);
            this.chkShowEvents.TabIndex = 7;
            this.chkShowEvents.Text = "Output events per branch (-showevents)";
            this.chkShowEvents.UseVisualStyleBackColor = true;
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
            this.groupBox2.Size = new System.Drawing.Size(678, 155);
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
            this.tableLayoutPanel3.Controls.Add(this.btnBrowseGuideTree, 2, 2);
            this.tableLayoutPanel3.Controls.Add(this.txtGuideTreePath, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.label6, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel1, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.label4, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.numIterations, 1, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Size = new System.Drawing.Size(672, 136);
            this.tableLayoutPanel3.TabIndex = 28;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 87);
            this.label4.Margin = new System.Windows.Forms.Padding(5);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Iterations:";
            // 
            // numIterations
            // 
            this.numIterations.Location = new System.Drawing.Point(158, 84);
            this.numIterations.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numIterations.Name = "numIterations";
            this.numIterations.Size = new System.Drawing.Size(40, 20);
            this.numIterations.TabIndex = 2;
            this.numIterations.Value = new decimal(new int[] {
            5,
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
            this.groupBox3.Location = new System.Drawing.Point(3, 164);
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
            this.tableLayoutPanel2.Controls.Add(this.btnBrowsePRANK, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.txtWorkingDirectory, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.txtPRANKPath, 1, 0);
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
            this.tableLayoutPanel4.Size = new System.Drawing.Size(684, 298);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // btnRun
            // 
            this.btnRun.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnRun.Location = new System.Drawing.Point(515, 272);
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
            this.btnCancel.Location = new System.Drawing.Point(601, 272);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // frmCreateJob
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(684, 298);
            this.Controls.Add(this.tableLayoutPanel4);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(700, 332);
            this.Name = "frmCreateJob";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Configure PRANK";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmCreateJob_FormClosing);
            this.Load += new System.EventHandler(this.frmCreateJob_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numIterations)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnBrowsePRANK;
        private System.Windows.Forms.TextBox txtPRANKPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnBrowseWorkingDirectory;
        private System.Windows.Forms.TextBox txtWorkingDirectory;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkKeepOutputFiles;
        private System.Windows.Forms.CheckBox chkF;
        private System.Windows.Forms.CheckBox chkCodon;
        private System.Windows.Forms.CheckBox chkTranslate;
        private System.Windows.Forms.CheckBox chkMTTranslate;
        private System.Windows.Forms.CheckBox chkKeep;
        private System.Windows.Forms.CheckBox chkShowAnc;
        private System.Windows.Forms.CheckBox chkShowTree;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtGuideTreePath;
        private System.Windows.Forms.Button btnBrowseGuideTree;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.CheckBox chkShowEvents;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numIterations;
    }
}