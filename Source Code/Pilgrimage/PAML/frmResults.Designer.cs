namespace Pilgrimage.PAML
{
    partial class frmResults
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmResults));
            this.tbForm = new System.Windows.Forms.TabControl();
            this.pgResults = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.chkToggleSelected = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.grdResults = new System.Windows.Forms.DataGridView();
            this.clmSelected = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.clmTree = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmModel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmNP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmlnL = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmK = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmValueType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmSite0 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmSite1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmSite2a = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmSite2b = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmKStart = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmWStart = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmInRecordSet = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.btnClose_Results = new System.Windows.Forms.Button();
            this.lblSelectedRows = new System.Windows.Forms.Label();
            this.lnkWorkingFolder = new System.Windows.Forms.LinkLabel();
            this.cmbSubSets = new System.Windows.Forms.ComboBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.pbExceptions = new System.Windows.Forms.PictureBox();
            this.lblExceptions = new System.Windows.Forms.Label();
            this.sepExceptions = new System.Windows.Forms.GroupBox();
            this.pgConfiguration = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.uctJobConfigurations1 = new Pilgrimage.PAML.uctJobConfigurations();
            this.btnClose_Configuration = new System.Windows.Forms.Button();
            this.lnkWorkingFolder_Configuration = new System.Windows.Forms.LinkLabel();
            this.pgOutput = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.grdOutput = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmSequences = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmNSSites = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmNCatG = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmKappa = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmOmega = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmExceptions = new System.Windows.Forms.DataGridViewImageColumn();
            this.btnClose_Output = new System.Windows.Forms.Button();
            this.btnReRunJob = new System.Windows.Forms.Button();
            this.tbForm.SuspendLayout();
            this.pgResults.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdResults)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbExceptions)).BeginInit();
            this.pgConfiguration.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.pgOutput.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdOutput)).BeginInit();
            this.SuspendLayout();
            // 
            // tbForm
            // 
            this.tbForm.Controls.Add(this.pgResults);
            this.tbForm.Controls.Add(this.pgConfiguration);
            this.tbForm.Controls.Add(this.pgOutput);
            this.tbForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbForm.Location = new System.Drawing.Point(0, 0);
            this.tbForm.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.tbForm.Name = "tbForm";
            this.tbForm.SelectedIndex = 0;
            this.tbForm.Size = new System.Drawing.Size(1768, 1081);
            this.tbForm.TabIndex = 0;
            this.tbForm.Selected += new System.Windows.Forms.TabControlEventHandler(this.tbForm_Selected);
            // 
            // pgResults
            // 
            this.pgResults.Controls.Add(this.tableLayoutPanel2);
            this.pgResults.Location = new System.Drawing.Point(8, 39);
            this.pgResults.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.pgResults.Name = "pgResults";
            this.pgResults.Size = new System.Drawing.Size(1752, 1034);
            this.pgResults.TabIndex = 0;
            this.pgResults.Text = "Results";
            this.pgResults.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.chkToggleSelected, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.grdResults, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.btnClose_Results, 3, 6);
            this.tableLayoutPanel2.Controls.Add(this.lblSelectedRows, 2, 3);
            this.tableLayoutPanel2.Controls.Add(this.lnkWorkingFolder, 2, 5);
            this.tableLayoutPanel2.Controls.Add(this.cmbSubSets, 2, 6);
            this.tableLayoutPanel2.Controls.Add(this.btnSave, 0, 6);
            this.tableLayoutPanel2.Controls.Add(this.pbExceptions, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblExceptions, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.sepExceptions, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 7;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1752, 1034);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // chkToggleSelected
            // 
            this.chkToggleSelected.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkToggleSelected.AutoSize = true;
            this.tableLayoutPanel2.SetColumnSpan(this.chkToggleSelected, 2);
            this.chkToggleSelected.Location = new System.Drawing.Point(10, 136);
            this.chkToggleSelected.Margin = new System.Windows.Forms.Padding(10, 10, 10, 0);
            this.chkToggleSelected.Name = "chkToggleSelected";
            this.chkToggleSelected.Size = new System.Drawing.Size(134, 29);
            this.chkToggleSelected.TabIndex = 11;
            this.chkToggleSelected.Text = "Select All";
            this.chkToggleSelected.UseVisualStyleBackColor = true;
            this.chkToggleSelected.CheckedChanged += new System.EventHandler(this.chkToggleSelected_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.tableLayoutPanel2.SetColumnSpan(this.label1, 4);
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(10, 86);
            this.label1.Margin = new System.Windows.Forms.Padding(10, 10, 10, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1490, 30);
            this.label1.TabIndex = 9;
            this.label1.Text = "Select results to add to your dataset.  To view additional details for a result,," +
    " double-click on a row or right-click and select \"Details\"";
            // 
            // grdResults
            // 
            this.grdResults.AllowUserToAddRows = false;
            this.grdResults.AllowUserToDeleteRows = false;
            this.grdResults.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grdResults.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.grdResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdResults.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmSelected,
            this.clmTree,
            this.clmModel,
            this.clmNP,
            this.clmlnL,
            this.clmK,
            this.clmValueType,
            this.clmSite0,
            this.clmSite1,
            this.clmSite2a,
            this.clmSite2b,
            this.clmKStart,
            this.clmWStart,
            this.clmInRecordSet});
            this.tableLayoutPanel2.SetColumnSpan(this.grdResults, 4);
            this.grdResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdResults.Location = new System.Drawing.Point(6, 171);
            this.grdResults.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.grdResults.Name = "grdResults";
            this.grdResults.ReadOnly = true;
            this.grdResults.RowHeadersVisible = false;
            this.grdResults.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdResults.Size = new System.Drawing.Size(1740, 756);
            this.grdResults.TabIndex = 3;
            // 
            // clmSelected
            // 
            this.clmSelected.DataPropertyName = "Selected";
            this.clmSelected.FillWeight = 3F;
            this.clmSelected.HeaderText = "";
            this.clmSelected.Name = "clmSelected";
            this.clmSelected.ReadOnly = true;
            // 
            // clmTree
            // 
            this.clmTree.DataPropertyName = "TreeTitle";
            this.clmTree.FillWeight = 15F;
            this.clmTree.HeaderText = "Tree";
            this.clmTree.Name = "clmTree";
            this.clmTree.ReadOnly = true;
            // 
            // clmModel
            // 
            this.clmModel.DataPropertyName = "ModelName";
            this.clmModel.FillWeight = 14F;
            this.clmModel.HeaderText = "Model";
            this.clmModel.Name = "clmModel";
            this.clmModel.ReadOnly = true;
            // 
            // clmNP
            // 
            this.clmNP.DataPropertyName = "np";
            this.clmNP.FillWeight = 7F;
            this.clmNP.HeaderText = "np";
            this.clmNP.Name = "clmNP";
            this.clmNP.ReadOnly = true;
            // 
            // clmlnL
            // 
            this.clmlnL.DataPropertyName = "lnL";
            this.clmlnL.FillWeight = 7F;
            this.clmlnL.HeaderText = "lnL";
            this.clmlnL.Name = "clmlnL";
            this.clmlnL.ReadOnly = true;
            // 
            // clmK
            // 
            this.clmK.DataPropertyName = "k";
            this.clmK.FillWeight = 7F;
            this.clmK.HeaderText = "k";
            this.clmK.Name = "clmK";
            this.clmK.ReadOnly = true;
            // 
            // clmValueType
            // 
            this.clmValueType.DataPropertyName = "ValueHeader";
            this.clmValueType.FillWeight = 10F;
            this.clmValueType.HeaderText = "";
            this.clmValueType.Name = "clmValueType";
            this.clmValueType.ReadOnly = true;
            // 
            // clmSite0
            // 
            this.clmSite0.DataPropertyName = "Value0";
            this.clmSite0.FillWeight = 6F;
            this.clmSite0.HeaderText = "";
            this.clmSite0.Name = "clmSite0";
            this.clmSite0.ReadOnly = true;
            // 
            // clmSite1
            // 
            this.clmSite1.DataPropertyName = "Value1";
            this.clmSite1.FillWeight = 6F;
            this.clmSite1.HeaderText = "";
            this.clmSite1.Name = "clmSite1";
            this.clmSite1.ReadOnly = true;
            // 
            // clmSite2a
            // 
            this.clmSite2a.DataPropertyName = "Value2a";
            this.clmSite2a.FillWeight = 6F;
            this.clmSite2a.HeaderText = "";
            this.clmSite2a.Name = "clmSite2a";
            this.clmSite2a.ReadOnly = true;
            // 
            // clmSite2b
            // 
            this.clmSite2b.DataPropertyName = "Value2b";
            this.clmSite2b.FillWeight = 6F;
            this.clmSite2b.HeaderText = "";
            this.clmSite2b.Name = "clmSite2b";
            this.clmSite2b.ReadOnly = true;
            // 
            // clmKStart
            // 
            this.clmKStart.DataPropertyName = "Kappa";
            this.clmKStart.FillWeight = 4F;
            this.clmKStart.HeaderText = "k\rStart";
            this.clmKStart.Name = "clmKStart";
            this.clmKStart.ReadOnly = true;
            // 
            // clmWStart
            // 
            this.clmWStart.DataPropertyName = "Omega";
            this.clmWStart.FillWeight = 4F;
            this.clmWStart.HeaderText = "w\rStart";
            this.clmWStart.Name = "clmWStart";
            this.clmWStart.ReadOnly = true;
            // 
            // clmInRecordSet
            // 
            this.clmInRecordSet.DataPropertyName = "InRecordSet";
            this.clmInRecordSet.FillWeight = 5F;
            this.clmInRecordSet.HeaderText = "In Project";
            this.clmInRecordSet.Name = "clmInRecordSet";
            this.clmInRecordSet.ReadOnly = true;
            // 
            // btnClose_Results
            // 
            this.btnClose_Results.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnClose_Results.Location = new System.Drawing.Point(1596, 984);
            this.btnClose_Results.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnClose_Results.Name = "btnClose_Results";
            this.btnClose_Results.Size = new System.Drawing.Size(150, 44);
            this.btnClose_Results.TabIndex = 0;
            this.btnClose_Results.Text = "&Close";
            this.btnClose_Results.UseVisualStyleBackColor = true;
            // 
            // lblSelectedRows
            // 
            this.lblSelectedRows.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblSelectedRows.AutoSize = true;
            this.lblSelectedRows.Location = new System.Drawing.Point(172, 138);
            this.lblSelectedRows.Margin = new System.Windows.Forms.Padding(10, 10, 10, 0);
            this.lblSelectedRows.Name = "lblSelectedRows";
            this.lblSelectedRows.Size = new System.Drawing.Size(189, 25);
            this.lblSelectedRows.TabIndex = 12;
            this.lblSelectedRows.Text = "# records selected";
            // 
            // lnkWorkingFolder
            // 
            this.lnkWorkingFolder.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lnkWorkingFolder.AutoSize = true;
            this.tableLayoutPanel2.SetColumnSpan(this.lnkWorkingFolder, 2);
            this.lnkWorkingFolder.Location = new System.Drawing.Point(1584, 943);
            this.lnkWorkingFolder.Margin = new System.Windows.Forms.Padding(10, 10, 10, 10);
            this.lnkWorkingFolder.Name = "lnkWorkingFolder";
            this.lnkWorkingFolder.Size = new System.Drawing.Size(158, 25);
            this.lnkWorkingFolder.TabIndex = 13;
            this.lnkWorkingFolder.TabStop = true;
            this.lnkWorkingFolder.Text = "Working Folder";
            this.lnkWorkingFolder.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkWorkingFolder_LinkClicked);
            // 
            // cmbSubSets
            // 
            this.cmbSubSets.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cmbSubSets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSubSets.FormattingEnabled = true;
            this.cmbSubSets.Location = new System.Drawing.Point(168, 989);
            this.cmbSubSets.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.cmbSubSets.Name = "cmbSubSets";
            this.cmbSubSets.Size = new System.Drawing.Size(396, 33);
            this.cmbSubSets.TabIndex = 8;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tableLayoutPanel2.SetColumnSpan(this.btnSave, 2);
            this.btnSave.Location = new System.Drawing.Point(6, 984);
            this.btnSave.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(150, 44);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "&Add to:";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // pbExceptions
            // 
            this.pbExceptions.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pbExceptions.Image = global::Pilgrimage.Properties.Resources.Warning_24;
            this.pbExceptions.Location = new System.Drawing.Point(6, 6);
            this.pbExceptions.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.pbExceptions.Name = "pbExceptions";
            this.pbExceptions.Size = new System.Drawing.Size(48, 46);
            this.pbExceptions.TabIndex = 14;
            this.pbExceptions.TabStop = false;
            // 
            // lblExceptions
            // 
            this.lblExceptions.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblExceptions.AutoSize = true;
            this.tableLayoutPanel2.SetColumnSpan(this.lblExceptions, 3);
            this.lblExceptions.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExceptions.Location = new System.Drawing.Point(70, 14);
            this.lblExceptions.Margin = new System.Windows.Forms.Padding(10, 10, 10, 10);
            this.lblExceptions.Name = "lblExceptions";
            this.lblExceptions.Size = new System.Drawing.Size(1489, 30);
            this.lblExceptions.TabIndex = 9;
            this.lblExceptions.Text = "One or more configurations for codeml.exe resulted in a warning or error. Select " +
    "the \"CodeML.exe Output\" tab to review the output.";
            // 
            // sepExceptions
            // 
            this.sepExceptions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.SetColumnSpan(this.sepExceptions, 5);
            this.sepExceptions.Location = new System.Drawing.Point(6, 64);
            this.sepExceptions.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.sepExceptions.Name = "sepExceptions";
            this.sepExceptions.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.sepExceptions.Size = new System.Drawing.Size(1740, 6);
            this.sepExceptions.TabIndex = 15;
            this.sepExceptions.TabStop = false;
            // 
            // pgConfiguration
            // 
            this.pgConfiguration.Controls.Add(this.tableLayoutPanel1);
            this.pgConfiguration.Location = new System.Drawing.Point(8, 39);
            this.pgConfiguration.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.pgConfiguration.Name = "pgConfiguration";
            this.pgConfiguration.Size = new System.Drawing.Size(1752, 1034);
            this.pgConfiguration.TabIndex = 1;
            this.pgConfiguration.Text = "Job Configuration";
            this.pgConfiguration.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.uctJobConfigurations1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnClose_Configuration, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.lnkWorkingFolder_Configuration, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnReRunJob, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1752, 1034);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // uctJobConfigurations1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.uctJobConfigurations1, 3);
            this.uctJobConfigurations1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uctJobConfigurations1.Location = new System.Drawing.Point(12, 12);
            this.uctJobConfigurations1.Margin = new System.Windows.Forms.Padding(12, 12, 12, 12);
            this.uctJobConfigurations1.Name = "uctJobConfigurations1";
            this.uctJobConfigurations1.Size = new System.Drawing.Size(1728, 954);
            this.uctJobConfigurations1.TabIndex = 1;
            // 
            // btnClose_Configuration
            // 
            this.btnClose_Configuration.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnClose_Configuration.Location = new System.Drawing.Point(1596, 984);
            this.btnClose_Configuration.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnClose_Configuration.Name = "btnClose_Configuration";
            this.btnClose_Configuration.Size = new System.Drawing.Size(150, 44);
            this.btnClose_Configuration.TabIndex = 0;
            this.btnClose_Configuration.Text = "&Close";
            this.btnClose_Configuration.UseVisualStyleBackColor = true;
            // 
            // lnkWorkingFolder_Configuration
            // 
            this.lnkWorkingFolder_Configuration.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lnkWorkingFolder_Configuration.AutoSize = true;
            this.lnkWorkingFolder_Configuration.Location = new System.Drawing.Point(10, 993);
            this.lnkWorkingFolder_Configuration.Margin = new System.Windows.Forms.Padding(10, 10, 10, 10);
            this.lnkWorkingFolder_Configuration.Name = "lnkWorkingFolder_Configuration";
            this.lnkWorkingFolder_Configuration.Size = new System.Drawing.Size(158, 25);
            this.lnkWorkingFolder_Configuration.TabIndex = 14;
            this.lnkWorkingFolder_Configuration.TabStop = true;
            this.lnkWorkingFolder_Configuration.Text = "Working Folder";
            this.lnkWorkingFolder_Configuration.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkWorkingFolder_LinkClicked);
            // 
            // pgOutput
            // 
            this.pgOutput.Controls.Add(this.tableLayoutPanel3);
            this.pgOutput.Location = new System.Drawing.Point(8, 39);
            this.pgOutput.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.pgOutput.Name = "pgOutput";
            this.pgOutput.Size = new System.Drawing.Size(1752, 1034);
            this.pgOutput.TabIndex = 2;
            this.pgOutput.Text = "CodeML.exe Output";
            this.pgOutput.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.grdOutput, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnClose_Output, 0, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(1752, 1034);
            this.tableLayoutPanel3.TabIndex = 1;
            // 
            // grdOutput
            // 
            this.grdOutput.AllowUserToAddRows = false;
            this.grdOutput.AllowUserToDeleteRows = false;
            this.grdOutput.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grdOutput.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdOutput.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.clmSequences,
            this.clmTitle,
            this.dataGridViewTextBoxColumn2,
            this.clmNSSites,
            this.clmNCatG,
            this.clmKappa,
            this.clmOmega,
            this.clmStatus,
            this.clmExceptions});
            this.grdOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdOutput.Location = new System.Drawing.Point(6, 6);
            this.grdOutput.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.grdOutput.Name = "grdOutput";
            this.grdOutput.ReadOnly = true;
            this.grdOutput.RowHeadersVisible = false;
            this.grdOutput.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdOutput.Size = new System.Drawing.Size(1740, 966);
            this.grdOutput.TabIndex = 2;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "TreeFileName";
            this.dataGridViewTextBoxColumn1.FillWeight = 12.82118F;
            this.dataGridViewTextBoxColumn1.HeaderText = "Tree";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // clmSequences
            // 
            this.clmSequences.DataPropertyName = "SequencesFileName";
            this.clmSequences.FillWeight = 12.82118F;
            this.clmSequences.HeaderText = "Sequences";
            this.clmSequences.Name = "clmSequences";
            this.clmSequences.ReadOnly = true;
            // 
            // clmTitle
            // 
            this.clmTitle.DataPropertyName = "Title";
            this.clmTitle.FillWeight = 12.82118F;
            this.clmTitle.HeaderText = "Title";
            this.clmTitle.Name = "clmTitle";
            this.clmTitle.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "Model";
            this.dataGridViewTextBoxColumn2.FillWeight = 13.73698F;
            this.dataGridViewTextBoxColumn2.HeaderText = "Model";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // clmNSSites
            // 
            this.clmNSSites.DataPropertyName = "NSSites";
            this.clmNSSites.FillWeight = 10.98959F;
            this.clmNSSites.HeaderText = "Site Models";
            this.clmNSSites.Name = "clmNSSites";
            this.clmNSSites.ReadOnly = true;
            // 
            // clmNCatG
            // 
            this.clmNCatG.DataPropertyName = "NCatG";
            this.clmNCatG.FillWeight = 9.157989F;
            this.clmNCatG.HeaderText = "Categories";
            this.clmNCatG.Name = "clmNCatG";
            this.clmNCatG.ReadOnly = true;
            // 
            // clmKappa
            // 
            this.clmKappa.DataPropertyName = "Kappa";
            this.clmKappa.FillWeight = 3.663195F;
            this.clmKappa.HeaderText = "k Start";
            this.clmKappa.Name = "clmKappa";
            this.clmKappa.ReadOnly = true;
            // 
            // clmOmega
            // 
            this.clmOmega.DataPropertyName = "Omega";
            this.clmOmega.FillWeight = 3.663195F;
            this.clmOmega.HeaderText = "w Start";
            this.clmOmega.Name = "clmOmega";
            this.clmOmega.ReadOnly = true;
            // 
            // clmStatus
            // 
            this.clmStatus.DataPropertyName = "Status";
            this.clmStatus.FillWeight = 9.157989F;
            this.clmStatus.HeaderText = "Status";
            this.clmStatus.Name = "clmStatus";
            this.clmStatus.ReadOnly = true;
            // 
            // clmExceptions
            // 
            this.clmExceptions.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.clmExceptions.DataPropertyName = "ExceptionsImage";
            this.clmExceptions.FillWeight = 4F;
            this.clmExceptions.HeaderText = "";
            this.clmExceptions.Name = "clmExceptions";
            this.clmExceptions.ReadOnly = true;
            this.clmExceptions.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.clmExceptions.Width = 22;
            // 
            // btnClose_Output
            // 
            this.btnClose_Output.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnClose_Output.Location = new System.Drawing.Point(1596, 984);
            this.btnClose_Output.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnClose_Output.Name = "btnClose_Output";
            this.btnClose_Output.Size = new System.Drawing.Size(150, 44);
            this.btnClose_Output.TabIndex = 0;
            this.btnClose_Output.Text = "&Close";
            this.btnClose_Output.UseVisualStyleBackColor = true;
            // 
            // btnReRunJob
            // 
            this.btnReRunJob.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnReRunJob.Location = new System.Drawing.Point(1354, 984);
            this.btnReRunJob.Margin = new System.Windows.Forms.Padding(6);
            this.btnReRunJob.Name = "btnReRunJob";
            this.btnReRunJob.Size = new System.Drawing.Size(230, 44);
            this.btnReRunJob.TabIndex = 0;
            this.btnReRunJob.Text = "&Run Job Again";
            this.btnReRunJob.UseVisualStyleBackColor = true;
            this.btnReRunJob.Click += new System.EventHandler(this.btnReRunJob_Click);
            // 
            // frmResults
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1768, 1081);
            this.Controls.Add(this.tbForm);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(12, 12, 12, 12);
            this.Name = "frmResults";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Results";
            this.Load += new System.EventHandler(this.frmResults_Load);
            this.tbForm.ResumeLayout(false);
            this.pgResults.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdResults)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbExceptions)).EndInit();
            this.pgConfiguration.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.pgOutput.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdOutput)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tbForm;
        private System.Windows.Forms.TabPage pgResults;
        private System.Windows.Forms.TabPage pgConfiguration;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnClose_Configuration;
        private uctJobConfigurations uctJobConfigurations1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button btnClose_Results;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.DataGridView grdResults;
        private System.Windows.Forms.ComboBox cmbSubSets;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkToggleSelected;
        private System.Windows.Forms.Label lblSelectedRows;
        private System.Windows.Forms.LinkLabel lnkWorkingFolder;
        private System.Windows.Forms.TabPage pgOutput;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button btnClose_Output;
        private System.Windows.Forms.DataGridView grdOutput;
        private System.Windows.Forms.PictureBox pbExceptions;
        private System.Windows.Forms.Label lblExceptions;
        private System.Windows.Forms.GroupBox sepExceptions;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmSequences;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmTitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmNSSites;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmNCatG;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmKappa;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmOmega;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmStatus;
        private System.Windows.Forms.DataGridViewImageColumn clmExceptions;
        private System.Windows.Forms.LinkLabel lnkWorkingFolder_Configuration;
        private System.Windows.Forms.DataGridViewCheckBoxColumn clmSelected;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmTree;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmModel;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmNP;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmlnL;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmK;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmValueType;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmSite0;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmSite1;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmSite2a;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmSite2b;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmKStart;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmWStart;
        private System.Windows.Forms.DataGridViewCheckBoxColumn clmInRecordSet;
        private System.Windows.Forms.Button btnReRunJob;
    }
}