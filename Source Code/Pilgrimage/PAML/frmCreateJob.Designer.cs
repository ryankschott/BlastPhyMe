namespace Pilgrimage.PAML
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
            this.label4 = new System.Windows.Forms.Label();
            this.grdTrees = new System.Windows.Forms.DataGridView();
            this.clmTreeFile = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmSequencesFile = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmSequencesCoun = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmLength = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.label10 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btnBrowse_CodeML = new System.Windows.Forms.Button();
            this.txtCodeMLPath = new System.Windows.Forms.TextBox();
            this.txtJob = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnRun = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.txtWorkingDirectory = new System.Windows.Forms.TextBox();
            this.btnBrowse_WorkingDirectory = new System.Windows.Forms.Button();
            this.cmbPriority = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCPUs = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.chkKeepFolders = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.grdTrees)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCPUs)).BeginInit();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.tableLayoutPanel4.SetColumnSpan(this.label4, 6);
            this.label4.Location = new System.Drawing.Point(5, 5);
            this.label4.Margin = new System.Windows.Forms.Padding(5);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(724, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Configure models to run for each tree and sequence file:";
            // 
            // grdTrees
            // 
            this.grdTrees.AllowUserToAddRows = false;
            this.grdTrees.AllowUserToDeleteRows = false;
            this.grdTrees.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grdTrees.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdTrees.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmTreeFile,
            this.clmSequencesFile,
            this.clmSequencesCoun,
            this.clmLength,
            this.clmTitle});
            this.tableLayoutPanel4.SetColumnSpan(this.grdTrees, 6);
            this.grdTrees.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdTrees.Location = new System.Drawing.Point(3, 26);
            this.grdTrees.Name = "grdTrees";
            this.grdTrees.ReadOnly = true;
            this.grdTrees.RowHeadersVisible = false;
            this.grdTrees.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdTrees.Size = new System.Drawing.Size(728, 216);
            this.grdTrees.TabIndex = 1;
            // 
            // clmTreeFile
            // 
            this.clmTreeFile.DataPropertyName = "TreeFilePath";
            this.clmTreeFile.FillWeight = 32F;
            this.clmTreeFile.HeaderText = "Tree File";
            this.clmTreeFile.Name = "clmTreeFile";
            this.clmTreeFile.ReadOnly = true;
            // 
            // clmSequencesFile
            // 
            this.clmSequencesFile.DataPropertyName = "SequencesFilePath";
            this.clmSequencesFile.FillWeight = 32F;
            this.clmSequencesFile.HeaderText = "Sequences File";
            this.clmSequencesFile.Name = "clmSequencesFile";
            this.clmSequencesFile.ReadOnly = true;
            // 
            // clmSequencesCoun
            // 
            this.clmSequencesCoun.DataPropertyName = "SequenceCount";
            this.clmSequencesCoun.FillWeight = 7F;
            this.clmSequencesCoun.HeaderText = "# Seq.";
            this.clmSequencesCoun.Name = "clmSequencesCoun";
            this.clmSequencesCoun.ReadOnly = true;
            // 
            // clmLength
            // 
            this.clmLength.DataPropertyName = "SequenceLength";
            this.clmLength.FillWeight = 7F;
            this.clmLength.HeaderText = "Length";
            this.clmLength.Name = "clmLength";
            this.clmLength.ReadOnly = true;
            // 
            // clmTitle
            // 
            this.clmTitle.DataPropertyName = "Title";
            this.clmTitle.FillWeight = 22F;
            this.clmTitle.HeaderText = "Title";
            this.clmTitle.Name = "clmTitle";
            this.clmTitle.ReadOnly = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel2.ColumnCount = 5;
            this.tableLayoutPanel4.SetColumnSpan(this.tableLayoutPanel2, 6);
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Controls.Add(this.btnAdd, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnEdit, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnRemove, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnCopy, 2, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 245);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(734, 29);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnAdd.Location = new System.Drawing.Point(3, 3);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(80, 23);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "&Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnEdit.Location = new System.Drawing.Point(89, 3);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(80, 23);
            this.btnEdit.TabIndex = 1;
            this.btnEdit.Text = "&Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnRemove.Location = new System.Drawing.Point(261, 3);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(80, 23);
            this.btnRemove.TabIndex = 3;
            this.btnRemove.Text = "&Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnCopy
            // 
            this.btnCopy.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnCopy.Location = new System.Drawing.Point(175, 3);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(80, 23);
            this.btnCopy.TabIndex = 2;
            this.btnCopy.Text = "&Copy";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 6;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel2, 0, 2);
            this.tableLayoutPanel4.Controls.Add(this.grdTrees, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.label10, 0, 9);
            this.tableLayoutPanel4.Controls.Add(this.label1, 0, 4);
            this.tableLayoutPanel4.Controls.Add(this.label7, 0, 5);
            this.tableLayoutPanel4.Controls.Add(this.btnBrowse_CodeML, 5, 5);
            this.tableLayoutPanel4.Controls.Add(this.txtCodeMLPath, 1, 5);
            this.tableLayoutPanel4.Controls.Add(this.txtJob, 1, 9);
            this.tableLayoutPanel4.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.groupBox1, 0, 3);
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel1, 1, 10);
            this.tableLayoutPanel4.Controls.Add(this.label8, 0, 6);
            this.tableLayoutPanel4.Controls.Add(this.txtWorkingDirectory, 1, 6);
            this.tableLayoutPanel4.Controls.Add(this.btnBrowse_WorkingDirectory, 5, 6);
            this.tableLayoutPanel4.Controls.Add(this.cmbPriority, 3, 7);
            this.tableLayoutPanel4.Controls.Add(this.label2, 2, 7);
            this.tableLayoutPanel4.Controls.Add(this.txtCPUs, 1, 7);
            this.tableLayoutPanel4.Controls.Add(this.label9, 0, 7);
            this.tableLayoutPanel4.Controls.Add(this.chkKeepFolders, 4, 7);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 11;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.Size = new System.Drawing.Size(734, 466);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // label10
            // 
            this.label10.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(5, 410);
            this.label10.Margin = new System.Windows.Forms.Padding(5);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(50, 13);
            this.label10.TabIndex = 15;
            this.label10.Text = "Job Title:";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.tableLayoutPanel4.SetColumnSpan(this.label1, 2);
            this.label1.Location = new System.Drawing.Point(5, 301);
            this.label1.Margin = new System.Windows.Forms.Padding(5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(141, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Configure job options:";
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(5, 327);
            this.label7.Margin = new System.Windows.Forms.Padding(5);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(70, 13);
            this.label7.TabIndex = 5;
            this.label7.Text = "CodeML.exe:";
            // 
            // btnBrowse_CodeML
            // 
            this.btnBrowse_CodeML.Location = new System.Drawing.Point(707, 322);
            this.btnBrowse_CodeML.Name = "btnBrowse_CodeML";
            this.btnBrowse_CodeML.Size = new System.Drawing.Size(24, 23);
            this.btnBrowse_CodeML.TabIndex = 7;
            this.btnBrowse_CodeML.Text = "...";
            this.btnBrowse_CodeML.UseVisualStyleBackColor = true;
            this.btnBrowse_CodeML.Click += new System.EventHandler(this.btnBrowse_CodeML_Click);
            // 
            // txtCodeMLPath
            // 
            this.txtCodeMLPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel4.SetColumnSpan(this.txtCodeMLPath, 4);
            this.txtCodeMLPath.Location = new System.Drawing.Point(108, 323);
            this.txtCodeMLPath.Name = "txtCodeMLPath";
            this.txtCodeMLPath.Size = new System.Drawing.Size(593, 20);
            this.txtCodeMLPath.TabIndex = 6;
            // 
            // txtJob
            // 
            this.txtJob.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel4.SetColumnSpan(this.txtJob, 5);
            this.txtJob.Location = new System.Drawing.Point(108, 407);
            this.txtJob.Name = "txtJob";
            this.txtJob.Size = new System.Drawing.Size(623, 20);
            this.txtJob.TabIndex = 16;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel4.SetColumnSpan(this.groupBox1, 6);
            this.groupBox1.Location = new System.Drawing.Point(3, 284);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 10, 3, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(728, 2);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel4.SetColumnSpan(this.tableLayoutPanel1, 5);
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.btnRun, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnCancel, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(562, 430);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(172, 36);
            this.tableLayoutPanel1.TabIndex = 18;
            // 
            // btnRun
            // 
            this.btnRun.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnRun.Location = new System.Drawing.Point(3, 10);
            this.btnRun.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
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
            this.btnCancel.Location = new System.Drawing.Point(89, 10);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(5, 356);
            this.label8.Margin = new System.Windows.Forms.Padding(5);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(95, 13);
            this.label8.TabIndex = 8;
            this.label8.Text = "Working Directory:";
            // 
            // txtWorkingDirectory
            // 
            this.txtWorkingDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel4.SetColumnSpan(this.txtWorkingDirectory, 4);
            this.txtWorkingDirectory.Location = new System.Drawing.Point(108, 352);
            this.txtWorkingDirectory.Name = "txtWorkingDirectory";
            this.txtWorkingDirectory.Size = new System.Drawing.Size(593, 20);
            this.txtWorkingDirectory.TabIndex = 9;
            // 
            // btnBrowse_WorkingDirectory
            // 
            this.btnBrowse_WorkingDirectory.Location = new System.Drawing.Point(707, 351);
            this.btnBrowse_WorkingDirectory.Name = "btnBrowse_WorkingDirectory";
            this.btnBrowse_WorkingDirectory.Size = new System.Drawing.Size(24, 23);
            this.btnBrowse_WorkingDirectory.TabIndex = 10;
            this.btnBrowse_WorkingDirectory.Text = "...";
            this.btnBrowse_WorkingDirectory.UseVisualStyleBackColor = true;
            this.btnBrowse_WorkingDirectory.Click += new System.EventHandler(this.btnBrowse_WorkingDirectory_Click);
            // 
            // cmbPriority
            // 
            this.cmbPriority.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cmbPriority.DisplayMember = "Value";
            this.cmbPriority.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPriority.FormattingEnabled = true;
            this.cmbPriority.Location = new System.Drawing.Point(230, 380);
            this.cmbPriority.Name = "cmbPriority";
            this.cmbPriority.Size = new System.Drawing.Size(120, 21);
            this.cmbPriority.TabIndex = 14;
            this.cmbPriority.ValueMember = "Key";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(156, 384);
            this.label2.Margin = new System.Windows.Forms.Padding(5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "CPU Priority:";
            // 
            // txtCPUs
            // 
            this.txtCPUs.Location = new System.Drawing.Point(108, 380);
            this.txtCPUs.Margin = new System.Windows.Forms.Padding(3, 3, 13, 3);
            this.txtCPUs.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtCPUs.Name = "txtCPUs";
            this.txtCPUs.Size = new System.Drawing.Size(30, 20);
            this.txtCPUs.TabIndex = 12;
            this.txtCPUs.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label9
            // 
            this.label9.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(5, 384);
            this.label9.Margin = new System.Windows.Forms.Padding(5);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(81, 13);
            this.label9.TabIndex = 11;
            this.label9.Text = "# of Processes:";
            // 
            // chkKeepFolders
            // 
            this.chkKeepFolders.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.chkKeepFolders.AutoSize = true;
            this.tableLayoutPanel4.SetColumnSpan(this.chkKeepFolders, 2);
            this.chkKeepFolders.Location = new System.Drawing.Point(581, 382);
            this.chkKeepFolders.Margin = new System.Windows.Forms.Padding(5);
            this.chkKeepFolders.Name = "chkKeepFolders";
            this.chkKeepFolders.Size = new System.Drawing.Size(148, 17);
            this.chkKeepFolders.TabIndex = 19;
            this.chkKeepFolders.Text = "Preserve Working Folders";
            this.chkKeepFolders.UseVisualStyleBackColor = true;
            // 
            // frmCreateJob
            // 
            this.AcceptButton = this.btnRun;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(734, 466);
            this.Controls.Add(this.tableLayoutPanel4);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmCreateJob";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Configure PAML";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmCreateJob_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.grdTrees)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtCPUs)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView grdTrees;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btnBrowse_CodeML;
        private System.Windows.Forms.Button btnBrowse_WorkingDirectory;
        private System.Windows.Forms.TextBox txtCodeMLPath;
        private System.Windows.Forms.TextBox txtWorkingDirectory;
        private System.Windows.Forms.TextBox txtJob;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.NumericUpDown txtCPUs;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbPriority;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmTreeFile;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmSequencesFile;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmSequencesCoun;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmLength;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmTitle;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox chkKeepFolders;
    }
}