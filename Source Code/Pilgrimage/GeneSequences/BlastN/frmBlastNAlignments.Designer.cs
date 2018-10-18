namespace Pilgrimage.GeneSequences.BlastN
{
    partial class frmBlastNAlignments
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBlastNAlignments));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.grdResults = new System.Windows.Forms.DataGridView();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose_Results = new System.Windows.Forms.Button();
            this.cmbSubSets = new System.Windows.Forms.ComboBox();
            this.chkUpdateFromGenBank = new System.Windows.Forms.CheckBox();
            this.lnkClearFilter = new System.Windows.Forms.LinkLabel();
            this.chkToggleSelected = new System.Windows.Forms.CheckBox();
            this.lnkFilter = new System.Windows.Forms.LinkLabel();
            this.lblSelectedRows = new System.Windows.Forms.Label();
            this.lblTotalRows = new System.Windows.Forms.Label();
            this.tbForm = new System.Windows.Forms.TabControl();
            this.pgResults = new System.Windows.Forms.TabPage();
            this.pgQuerySequences = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnClose_QuerySequences = new System.Windows.Forms.Button();
            this.grdQuerySequences = new System.Windows.Forms.DataGridView();
            this.clmQueryGeneName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmOrganism = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmLength = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmGenBankLink_Committed = new System.Windows.Forms.DataGridViewLinkColumn();
            this.pgHistory = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.grdHistory = new System.Windows.Forms.DataGridView();
            this.clmRequestID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmSubmittedGenes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmStartedAt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmLastStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmEndedAt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnClose_History = new System.Windows.Forms.Button();
            this.btnResubmit = new System.Windows.Forms.Button();
            this.pgProgress = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.btnClose_Progress = new System.Windows.Forms.Button();
            this.txtProgress = new System.Windows.Forms.TextBox();
            this.btnSaveProgress = new System.Windows.Forms.Button();
            this.bwAlignments = new System.ComponentModel.BackgroundWorker();
            this.clmInclude = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.clmDefinition = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmLengthDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmMaxScore = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmQueryCover = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmAlignmentLength = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmAverageMaxAlignmentPercentage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmGenBankUrl = new System.Windows.Forms.DataGridViewLinkColumn();
            this.clmInRecordSet = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdResults)).BeginInit();
            this.tbForm.SuspendLayout();
            this.pgResults.SuspendLayout();
            this.pgQuerySequences.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdQuerySequences)).BeginInit();
            this.pgHistory.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdHistory)).BeginInit();
            this.pgProgress.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 6;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.grdResults, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnSave, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnClose_Results, 3, 3);
            this.tableLayoutPanel1.Controls.Add(this.cmbSubSets, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.chkUpdateFromGenBank, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.lnkClearFilter, 5, 1);
            this.tableLayoutPanel1.Controls.Add(this.chkToggleSelected, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lnkFilter, 4, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblSelectedRows, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblTotalRows, 2, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(876, 536);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label1, 6);
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(5, 5);
            this.label1.Margin = new System.Windows.Forms.Padding(5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(825, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select alignments to add to your dataset.  To view additional details for an align" +
    "ment, double-click on a row or right-click and select \"Details\"";
            // 
            // grdResults
            // 
            this.grdResults.AllowUserToAddRows = false;
            this.grdResults.AllowUserToDeleteRows = false;
            this.grdResults.AllowUserToResizeRows = false;
            this.grdResults.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grdResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdResults.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmInclude,
            this.clmDefinition,
            this.clmLengthDescription,
            this.clmMaxScore,
            this.clmQueryCover,
            this.clmAlignmentLength,
            this.clmAverageMaxAlignmentPercentage,
            this.clmGenBankUrl,
            this.clmInRecordSet});
            this.tableLayoutPanel1.SetColumnSpan(this.grdResults, 6);
            this.grdResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdResults.Location = new System.Drawing.Point(3, 51);
            this.grdResults.Name = "grdResults";
            this.grdResults.RowHeadersVisible = false;
            this.grdResults.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdResults.Size = new System.Drawing.Size(870, 452);
            this.grdResults.TabIndex = 4;
            // 
            // btnSave
            // 
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.ImageKey = "Add";
            this.btnSave.Location = new System.Drawing.Point(3, 509);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(90, 24);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "&Add to:";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // btnClose_Results
            // 
            this.btnClose_Results.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.btnClose_Results, 3);
            this.btnClose_Results.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose_Results.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose_Results.ImageKey = "Cancel";
            this.btnClose_Results.Location = new System.Drawing.Point(793, 509);
            this.btnClose_Results.Name = "btnClose_Results";
            this.btnClose_Results.Size = new System.Drawing.Size(80, 24);
            this.btnClose_Results.TabIndex = 9;
            this.btnClose_Results.Text = "&Close";
            this.btnClose_Results.UseVisualStyleBackColor = true;
            // 
            // cmbSubSets
            // 
            this.cmbSubSets.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cmbSubSets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSubSets.FormattingEnabled = true;
            this.cmbSubSets.Location = new System.Drawing.Point(99, 510);
            this.cmbSubSets.Name = "cmbSubSets";
            this.cmbSubSets.Size = new System.Drawing.Size(200, 21);
            this.cmbSubSets.TabIndex = 7;
            // 
            // chkUpdateFromGenBank
            // 
            this.chkUpdateFromGenBank.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkUpdateFromGenBank.AutoSize = true;
            this.chkUpdateFromGenBank.Checked = true;
            this.chkUpdateFromGenBank.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkUpdateFromGenBank.Location = new System.Drawing.Point(307, 512);
            this.chkUpdateFromGenBank.Margin = new System.Windows.Forms.Padding(5);
            this.chkUpdateFromGenBank.Name = "chkUpdateFromGenBank";
            this.chkUpdateFromGenBank.Size = new System.Drawing.Size(132, 17);
            this.chkUpdateFromGenBank.TabIndex = 8;
            this.chkUpdateFromGenBank.Text = "Update from GenBank";
            this.chkUpdateFromGenBank.UseVisualStyleBackColor = true;
            // 
            // lnkClearFilter
            // 
            this.lnkClearFilter.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lnkClearFilter.AutoSize = true;
            this.lnkClearFilter.Location = new System.Drawing.Point(815, 33);
            this.lnkClearFilter.Margin = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.lnkClearFilter.Name = "lnkClearFilter";
            this.lnkClearFilter.Size = new System.Drawing.Size(56, 13);
            this.lnkClearFilter.TabIndex = 3;
            this.lnkClearFilter.TabStop = true;
            this.lnkClearFilter.Text = "Clear Filter";
            this.lnkClearFilter.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkFilter_LinkClicked);
            // 
            // chkToggleSelected
            // 
            this.chkToggleSelected.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkToggleSelected.AutoSize = true;
            this.chkToggleSelected.Location = new System.Drawing.Point(5, 31);
            this.chkToggleSelected.Margin = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.chkToggleSelected.Name = "chkToggleSelected";
            this.chkToggleSelected.Size = new System.Drawing.Size(70, 17);
            this.chkToggleSelected.TabIndex = 10;
            this.chkToggleSelected.Text = "Select All";
            this.chkToggleSelected.UseVisualStyleBackColor = true;
            this.chkToggleSelected.CheckedChanged += new System.EventHandler(this.chkToggleSelected_CheckedChanged);
            // 
            // lnkFilter
            // 
            this.lnkFilter.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lnkFilter.AutoSize = true;
            this.lnkFilter.Location = new System.Drawing.Point(747, 33);
            this.lnkFilter.Margin = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.lnkFilter.Name = "lnkFilter";
            this.lnkFilter.Size = new System.Drawing.Size(58, 13);
            this.lnkFilter.TabIndex = 2;
            this.lnkFilter.TabStop = true;
            this.lnkFilter.Text = "Apply Filter";
            this.lnkFilter.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkFilter_LinkClicked);
            // 
            // lblSelectedRows
            // 
            this.lblSelectedRows.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblSelectedRows.AutoSize = true;
            this.lblSelectedRows.Location = new System.Drawing.Point(101, 33);
            this.lblSelectedRows.Margin = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.lblSelectedRows.Name = "lblSelectedRows";
            this.lblSelectedRows.Size = new System.Drawing.Size(95, 13);
            this.lblSelectedRows.TabIndex = 5;
            this.lblSelectedRows.Text = "# records selected";
            // 
            // lblTotalRows
            // 
            this.lblTotalRows.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblTotalRows.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblTotalRows, 2);
            this.lblTotalRows.Location = new System.Drawing.Point(590, 33);
            this.lblTotalRows.Margin = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.lblTotalRows.Name = "lblTotalRows";
            this.lblTotalRows.Size = new System.Drawing.Size(147, 13);
            this.lblTotalRows.TabIndex = 1;
            this.lblTotalRows.Text = "Showing {0} of {1} alignments";
            // 
            // tbForm
            // 
            this.tbForm.Controls.Add(this.pgResults);
            this.tbForm.Controls.Add(this.pgQuerySequences);
            this.tbForm.Controls.Add(this.pgHistory);
            this.tbForm.Controls.Add(this.pgProgress);
            this.tbForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbForm.Location = new System.Drawing.Point(0, 0);
            this.tbForm.Name = "tbForm";
            this.tbForm.SelectedIndex = 0;
            this.tbForm.Size = new System.Drawing.Size(884, 562);
            this.tbForm.TabIndex = 0;
            this.tbForm.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tbForm_Selecting);
            // 
            // pgResults
            // 
            this.pgResults.Controls.Add(this.tableLayoutPanel1);
            this.pgResults.Location = new System.Drawing.Point(4, 22);
            this.pgResults.Name = "pgResults";
            this.pgResults.Size = new System.Drawing.Size(876, 536);
            this.pgResults.TabIndex = 0;
            this.pgResults.Text = "Results";
            this.pgResults.UseVisualStyleBackColor = true;
            // 
            // pgQuerySequences
            // 
            this.pgQuerySequences.Controls.Add(this.tableLayoutPanel2);
            this.pgQuerySequences.Location = new System.Drawing.Point(4, 22);
            this.pgQuerySequences.Name = "pgQuerySequences";
            this.pgQuerySequences.Size = new System.Drawing.Size(876, 536);
            this.pgQuerySequences.TabIndex = 1;
            this.pgQuerySequences.Text = "Query Sequences";
            this.pgQuerySequences.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.btnClose_QuerySequences, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.grdQuerySequences, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(876, 536);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // btnClose_QuerySequences
            // 
            this.btnClose_QuerySequences.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose_QuerySequences.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose_QuerySequences.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose_QuerySequences.ImageKey = "Cancel";
            this.btnClose_QuerySequences.Location = new System.Drawing.Point(793, 509);
            this.btnClose_QuerySequences.Name = "btnClose_QuerySequences";
            this.btnClose_QuerySequences.Size = new System.Drawing.Size(80, 24);
            this.btnClose_QuerySequences.TabIndex = 1;
            this.btnClose_QuerySequences.Text = "&Close";
            this.btnClose_QuerySequences.UseVisualStyleBackColor = true;
            // 
            // grdQuerySequences
            // 
            this.grdQuerySequences.AllowUserToAddRows = false;
            this.grdQuerySequences.AllowUserToDeleteRows = false;
            this.grdQuerySequences.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grdQuerySequences.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdQuerySequences.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmQueryGeneName,
            this.clmOrganism,
            this.dataGridViewTextBoxColumn1,
            this.clmLength,
            this.clmGenBankLink_Committed});
            this.grdQuerySequences.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdQuerySequences.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.grdQuerySequences.Location = new System.Drawing.Point(3, 3);
            this.grdQuerySequences.Name = "grdQuerySequences";
            this.grdQuerySequences.ReadOnly = true;
            this.grdQuerySequences.RowHeadersVisible = false;
            this.grdQuerySequences.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdQuerySequences.Size = new System.Drawing.Size(870, 500);
            this.grdQuerySequences.TabIndex = 0;
            // 
            // clmQueryGeneName
            // 
            this.clmQueryGeneName.DataPropertyName = "GeneName";
            this.clmQueryGeneName.FillWeight = 5F;
            this.clmQueryGeneName.HeaderText = "Gene";
            this.clmQueryGeneName.Name = "clmQueryGeneName";
            this.clmQueryGeneName.ReadOnly = true;
            // 
            // clmOrganism
            // 
            this.clmOrganism.DataPropertyName = "Organism";
            this.clmOrganism.FillWeight = 20F;
            this.clmOrganism.HeaderText = "Organism";
            this.clmOrganism.Name = "clmOrganism";
            this.clmOrganism.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Definition";
            this.dataGridViewTextBoxColumn1.FillWeight = 50F;
            this.dataGridViewTextBoxColumn1.HeaderText = "Definition";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // clmLength
            // 
            this.clmLength.DataPropertyName = "Length";
            dataGridViewCellStyle6.Format = "N0";
            this.clmLength.DefaultCellStyle = dataGridViewCellStyle6;
            this.clmLength.FillWeight = 10F;
            this.clmLength.HeaderText = "Length";
            this.clmLength.Name = "clmLength";
            this.clmLength.ReadOnly = true;
            // 
            // clmGenBankLink_Committed
            // 
            this.clmGenBankLink_Committed.DataPropertyName = "GenBankUrl";
            this.clmGenBankLink_Committed.FillWeight = 15F;
            this.clmGenBankLink_Committed.HeaderText = "GenBank";
            this.clmGenBankLink_Committed.Name = "clmGenBankLink_Committed";
            this.clmGenBankLink_Committed.ReadOnly = true;
            this.clmGenBankLink_Committed.Text = "Open GenBank Page";
            // 
            // pgHistory
            // 
            this.pgHistory.Controls.Add(this.tableLayoutPanel3);
            this.pgHistory.Location = new System.Drawing.Point(4, 22);
            this.pgHistory.Name = "pgHistory";
            this.pgHistory.Padding = new System.Windows.Forms.Padding(3);
            this.pgHistory.Size = new System.Drawing.Size(876, 536);
            this.pgHistory.TabIndex = 2;
            this.pgHistory.Text = "Request History";
            this.pgHistory.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.Controls.Add(this.grdHistory, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnClose_History, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.btnResubmit, 0, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(870, 530);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // grdHistory
            // 
            this.grdHistory.AllowUserToAddRows = false;
            this.grdHistory.AllowUserToDeleteRows = false;
            this.grdHistory.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grdHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdHistory.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmRequestID,
            this.clmSubmittedGenes,
            this.clmStartedAt,
            this.clmLastStatus,
            this.clmEndedAt});
            this.tableLayoutPanel3.SetColumnSpan(this.grdHistory, 2);
            this.grdHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdHistory.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.grdHistory.Location = new System.Drawing.Point(3, 3);
            this.grdHistory.Name = "grdHistory";
            this.grdHistory.ReadOnly = true;
            this.grdHistory.RowHeadersVisible = false;
            this.grdHistory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdHistory.Size = new System.Drawing.Size(864, 494);
            this.grdHistory.TabIndex = 0;
            this.grdHistory.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.grdHistory_CellFormatting);
            // 
            // clmRequestID
            // 
            this.clmRequestID.DataPropertyName = "RequestID";
            this.clmRequestID.FillWeight = 15F;
            this.clmRequestID.HeaderText = "Request ID";
            this.clmRequestID.Name = "clmRequestID";
            this.clmRequestID.ReadOnly = true;
            // 
            // clmSubmittedGenes
            // 
            this.clmSubmittedGenes.DataPropertyName = "GeneCount";
            dataGridViewCellStyle7.Format = "N0";
            this.clmSubmittedGenes.DefaultCellStyle = dataGridViewCellStyle7;
            this.clmSubmittedGenes.FillWeight = 14F;
            this.clmSubmittedGenes.HeaderText = "Gene Sequences";
            this.clmSubmittedGenes.Name = "clmSubmittedGenes";
            this.clmSubmittedGenes.ReadOnly = true;
            // 
            // clmStartedAt
            // 
            this.clmStartedAt.DataPropertyName = "StartedAt";
            dataGridViewCellStyle8.Format = "G";
            this.clmStartedAt.DefaultCellStyle = dataGridViewCellStyle8;
            this.clmStartedAt.FillWeight = 20F;
            this.clmStartedAt.HeaderText = "Started At";
            this.clmStartedAt.Name = "clmStartedAt";
            this.clmStartedAt.ReadOnly = true;
            // 
            // clmLastStatus
            // 
            this.clmLastStatus.DataPropertyName = "LastStatus";
            this.clmLastStatus.FillWeight = 15F;
            this.clmLastStatus.HeaderText = "Request Status";
            this.clmLastStatus.Name = "clmLastStatus";
            this.clmLastStatus.ReadOnly = true;
            // 
            // clmEndedAt
            // 
            this.clmEndedAt.DataPropertyName = "EndedAt";
            dataGridViewCellStyle9.Format = "G";
            dataGridViewCellStyle9.NullValue = " ";
            this.clmEndedAt.DefaultCellStyle = dataGridViewCellStyle9;
            this.clmEndedAt.FillWeight = 36F;
            this.clmEndedAt.HeaderText = "Ended At";
            this.clmEndedAt.Name = "clmEndedAt";
            this.clmEndedAt.ReadOnly = true;
            // 
            // btnClose_History
            // 
            this.btnClose_History.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnClose_History.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose_History.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose_History.ImageKey = "Cancel";
            this.btnClose_History.Location = new System.Drawing.Point(787, 503);
            this.btnClose_History.Name = "btnClose_History";
            this.btnClose_History.Size = new System.Drawing.Size(80, 24);
            this.btnClose_History.TabIndex = 2;
            this.btnClose_History.Text = "&Close";
            this.btnClose_History.UseVisualStyleBackColor = true;
            // 
            // btnResubmit
            // 
            this.btnResubmit.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnResubmit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnResubmit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnResubmit.ImageKey = "Cancel";
            this.btnResubmit.Location = new System.Drawing.Point(3, 503);
            this.btnResubmit.Name = "btnResubmit";
            this.btnResubmit.Size = new System.Drawing.Size(185, 24);
            this.btnResubmit.TabIndex = 1;
            this.btnResubmit.Text = "&Submit Not Processed Genes";
            this.btnResubmit.UseVisualStyleBackColor = true;
            this.btnResubmit.Click += new System.EventHandler(this.btnResubmit_Click);
            // 
            // pgProgress
            // 
            this.pgProgress.Controls.Add(this.tableLayoutPanel4);
            this.pgProgress.Location = new System.Drawing.Point(4, 22);
            this.pgProgress.Name = "pgProgress";
            this.pgProgress.Padding = new System.Windows.Forms.Padding(3);
            this.pgProgress.Size = new System.Drawing.Size(876, 536);
            this.pgProgress.TabIndex = 3;
            this.pgProgress.Text = "Progress Messages";
            this.pgProgress.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.Controls.Add(this.btnClose_Progress, 1, 1);
            this.tableLayoutPanel4.Controls.Add(this.txtProgress, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.btnSaveProgress, 0, 1);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(870, 530);
            this.tableLayoutPanel4.TabIndex = 1;
            // 
            // btnClose_Progress
            // 
            this.btnClose_Progress.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnClose_Progress.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose_Progress.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose_Progress.ImageKey = "Cancel";
            this.btnClose_Progress.Location = new System.Drawing.Point(787, 503);
            this.btnClose_Progress.Name = "btnClose_Progress";
            this.btnClose_Progress.Size = new System.Drawing.Size(80, 24);
            this.btnClose_Progress.TabIndex = 2;
            this.btnClose_Progress.Text = "&Close";
            this.btnClose_Progress.UseVisualStyleBackColor = true;
            // 
            // txtProgress
            // 
            this.tableLayoutPanel4.SetColumnSpan(this.txtProgress, 2);
            this.txtProgress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtProgress.Location = new System.Drawing.Point(3, 3);
            this.txtProgress.Multiline = true;
            this.txtProgress.Name = "txtProgress";
            this.txtProgress.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtProgress.Size = new System.Drawing.Size(864, 494);
            this.txtProgress.TabIndex = 3;
            // 
            // btnSaveProgress
            // 
            this.btnSaveProgress.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnSaveProgress.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnSaveProgress.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSaveProgress.ImageKey = "Cancel";
            this.btnSaveProgress.Location = new System.Drawing.Point(3, 503);
            this.btnSaveProgress.Name = "btnSaveProgress";
            this.btnSaveProgress.Size = new System.Drawing.Size(80, 24);
            this.btnSaveProgress.TabIndex = 2;
            this.btnSaveProgress.Text = "&Save";
            this.btnSaveProgress.UseVisualStyleBackColor = true;
            this.btnSaveProgress.Click += new System.EventHandler(this.btnSaveProgress_Click);
            // 
            // bwAlignments
            // 
            this.bwAlignments.WorkerSupportsCancellation = true;
            this.bwAlignments.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwAlignments_DoWork);
            this.bwAlignments.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwAlignments_RunWorkerCompleted);
            // 
            // clmInclude
            // 
            this.clmInclude.DataPropertyName = "Selected";
            this.clmInclude.FillWeight = 3F;
            this.clmInclude.HeaderText = "";
            this.clmInclude.Name = "clmInclude";
            this.clmInclude.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // clmDefinition
            // 
            this.clmDefinition.DataPropertyName = "Definition";
            this.clmDefinition.FillWeight = 47F;
            this.clmDefinition.HeaderText = "Definition";
            this.clmDefinition.Name = "clmDefinition";
            // 
            // clmLengthDescription
            // 
            this.clmLengthDescription.DataPropertyName = "Length";
            dataGridViewCellStyle1.Format = "N0";
            this.clmLengthDescription.DefaultCellStyle = dataGridViewCellStyle1;
            this.clmLengthDescription.FillWeight = 7F;
            this.clmLengthDescription.HeaderText = "Sequence Length";
            this.clmLengthDescription.Name = "clmLengthDescription";
            // 
            // clmMaxScore
            // 
            this.clmMaxScore.DataPropertyName = "MaxScore";
            dataGridViewCellStyle2.Format = "F0";
            this.clmMaxScore.DefaultCellStyle = dataGridViewCellStyle2;
            this.clmMaxScore.FillWeight = 6F;
            this.clmMaxScore.HeaderText = "Max Score";
            this.clmMaxScore.Name = "clmMaxScore";
            this.clmMaxScore.ReadOnly = true;
            // 
            // clmQueryCover
            // 
            this.clmQueryCover.DataPropertyName = "QueryCover";
            dataGridViewCellStyle3.Format = "P0";
            dataGridViewCellStyle3.NullValue = null;
            this.clmQueryCover.DefaultCellStyle = dataGridViewCellStyle3;
            this.clmQueryCover.FillWeight = 6F;
            this.clmQueryCover.HeaderText = "Query Cover";
            this.clmQueryCover.Name = "clmQueryCover";
            this.clmQueryCover.ReadOnly = true;
            // 
            // clmAlignmentLength
            // 
            this.clmAlignmentLength.DataPropertyName = "AlignmentLength";
            dataGridViewCellStyle4.Format = "N0";
            this.clmAlignmentLength.DefaultCellStyle = dataGridViewCellStyle4;
            this.clmAlignmentLength.FillWeight = 7F;
            this.clmAlignmentLength.HeaderText = "Aligned Length";
            this.clmAlignmentLength.Name = "clmAlignmentLength";
            this.clmAlignmentLength.ReadOnly = true;
            // 
            // clmAverageMaxAlignmentPercentage
            // 
            this.clmAverageMaxAlignmentPercentage.DataPropertyName = "AlignmentPercentage";
            dataGridViewCellStyle5.Format = "P0";
            this.clmAverageMaxAlignmentPercentage.DefaultCellStyle = dataGridViewCellStyle5;
            this.clmAverageMaxAlignmentPercentage.FillWeight = 6F;
            this.clmAverageMaxAlignmentPercentage.HeaderText = "Identities Match";
            this.clmAverageMaxAlignmentPercentage.Name = "clmAverageMaxAlignmentPercentage";
            this.clmAverageMaxAlignmentPercentage.ReadOnly = true;
            // 
            // clmGenBankUrl
            // 
            this.clmGenBankUrl.DataPropertyName = "GenBankUrl";
            this.clmGenBankUrl.FillWeight = 8F;
            this.clmGenBankUrl.HeaderText = "GenBank";
            this.clmGenBankUrl.Name = "clmGenBankUrl";
            this.clmGenBankUrl.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.clmGenBankUrl.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // clmInRecordSet
            // 
            this.clmInRecordSet.DataPropertyName = "InRecordSet";
            this.clmInRecordSet.FillWeight = 5F;
            this.clmInRecordSet.HeaderText = "In Project";
            this.clmInRecordSet.Name = "clmInRecordSet";
            this.clmInRecordSet.ReadOnly = true;
            // 
            // frmBlastNAlignments
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose_Results;
            this.ClientSize = new System.Drawing.Size(884, 562);
            this.Controls.Add(this.tbForm);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(440, 440);
            this.Name = "frmBlastNAlignments";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Alignments";
            this.Load += new System.EventHandler(this.frmBlastNAlignments_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdResults)).EndInit();
            this.tbForm.ResumeLayout(false);
            this.pgResults.ResumeLayout(false);
            this.pgQuerySequences.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdQuerySequences)).EndInit();
            this.pgHistory.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdHistory)).EndInit();
            this.pgProgress.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView grdResults;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose_Results;
        private System.Windows.Forms.Label lblSelectedRows;
        private System.Windows.Forms.Label lblTotalRows;
        private System.Windows.Forms.ComboBox cmbSubSets;
        private System.Windows.Forms.LinkLabel lnkFilter;
        private System.Windows.Forms.CheckBox chkUpdateFromGenBank;
        private System.Windows.Forms.TabControl tbForm;
        private System.Windows.Forms.TabPage pgResults;
        private System.Windows.Forms.TabPage pgQuerySequences;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.DataGridView grdQuerySequences;
        private System.ComponentModel.BackgroundWorker bwAlignments;
        private System.Windows.Forms.LinkLabel lnkClearFilter;
        private System.Windows.Forms.Button btnClose_QuerySequences;
        private System.Windows.Forms.TabPage pgHistory;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.DataGridView grdHistory;
        private System.Windows.Forms.Button btnClose_History;
        private System.Windows.Forms.Button btnResubmit;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmRequestID;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmSubmittedGenes;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmStartedAt;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmLastStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmEndedAt;
        private System.Windows.Forms.CheckBox chkToggleSelected;
        private System.Windows.Forms.TabPage pgProgress;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Button btnClose_Progress;
        private System.Windows.Forms.TextBox txtProgress;
        private System.Windows.Forms.Button btnSaveProgress;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmQueryGeneName;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmOrganism;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmLength;
        private System.Windows.Forms.DataGridViewLinkColumn clmGenBankLink_Committed;
        private System.Windows.Forms.DataGridViewCheckBoxColumn clmInclude;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmDefinition;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmLengthDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmMaxScore;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmQueryCover;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmAlignmentLength;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmAverageMaxAlignmentPercentage;
        private System.Windows.Forms.DataGridViewLinkColumn clmGenBankUrl;
        private System.Windows.Forms.DataGridViewCheckBoxColumn clmInRecordSet;
    }
}