namespace Pilgrimage.GeneSequences
{
    partial class frmFilterGenes
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmFilterGenes));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tvTaxonomy = new System.Windows.Forms.TreeView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDefinition = new System.Windows.Forms.TextBox();
            this.cmbOrganism = new System.Windows.Forms.ComboBox();
            this.cmbOrganismLogic = new System.Windows.Forms.ComboBox();
            this.cmbDefinitionLogic = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbGeneNameLogic = new System.Windows.Forms.ComboBox();
            this.cmbGeneName = new System.Windows.Forms.ComboBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.chkDuplicatesByOrganism = new System.Windows.Forms.CheckBox();
            this.chkBLASTN = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.rbDuplicates_Have = new System.Windows.Forms.RadioButton();
            this.rbDuplicates_HaveNot = new System.Windows.Forms.RadioButton();
            this.tblBLASTN = new System.Windows.Forms.TableLayoutPanel();
            this.rbBLASTN_Results = new System.Windows.Forms.RadioButton();
            this.rbBLASTN_NotSubmitted = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.chkSequenceMatch = new System.Windows.Forms.CheckBox();
            this.tblSequenceMatch = new System.Windows.Forms.TableLayoutPanel();
            this.rbSequenceMatch_CDS = new System.Windows.Forms.RadioButton();
            this.rbSequenceMatch_Whole = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.pgRecordFilters = new System.Windows.Forms.TabPage();
            this.pgSequenceFilters = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tblBLASTN.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tblSequenceMatch.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.pgRecordFilters.SuspendLayout();
            this.pgSequenceFilters.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.tvTaxonomy, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtDefinition, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.cmbOrganism, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.cmbOrganismLogic, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.cmbDefinitionLogic, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.cmbGeneNameLogic, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.cmbGeneName, 2, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(395, 354);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tvTaxonomy
            // 
            this.tvTaxonomy.CheckBoxes = true;
            this.tableLayoutPanel1.SetColumnSpan(this.tvTaxonomy, 4);
            this.tvTaxonomy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvTaxonomy.Location = new System.Drawing.Point(3, 117);
            this.tvTaxonomy.Name = "tvTaxonomy";
            this.tvTaxonomy.Size = new System.Drawing.Size(389, 234);
            this.tvTaxonomy.TabIndex = 10;
            this.tvTaxonomy.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tvTaxonomy_AfterCheck);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Organism Name:";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 40);
            this.label2.Margin = new System.Windows.Forms.Padding(5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Definition:";
            // 
            // txtDefinition
            // 
            this.txtDefinition.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.txtDefinition, 2);
            this.txtDefinition.Location = new System.Drawing.Point(200, 36);
            this.txtDefinition.Margin = new System.Windows.Forms.Padding(5);
            this.txtDefinition.Name = "txtDefinition";
            this.txtDefinition.Size = new System.Drawing.Size(190, 20);
            this.txtDefinition.TabIndex = 5;
            // 
            // cmbOrganism
            // 
            this.cmbOrganism.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbOrganism.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbOrganism.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.tableLayoutPanel1.SetColumnSpan(this.cmbOrganism, 2);
            this.cmbOrganism.FormattingEnabled = true;
            this.cmbOrganism.Location = new System.Drawing.Point(200, 5);
            this.cmbOrganism.Margin = new System.Windows.Forms.Padding(5);
            this.cmbOrganism.Name = "cmbOrganism";
            this.cmbOrganism.Size = new System.Drawing.Size(190, 21);
            this.cmbOrganism.TabIndex = 2;
            // 
            // cmbOrganismLogic
            // 
            this.cmbOrganismLogic.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cmbOrganismLogic.DisplayMember = "Value";
            this.cmbOrganismLogic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOrganismLogic.FormattingEnabled = true;
            this.cmbOrganismLogic.Location = new System.Drawing.Point(100, 5);
            this.cmbOrganismLogic.Margin = new System.Windows.Forms.Padding(5);
            this.cmbOrganismLogic.Name = "cmbOrganismLogic";
            this.cmbOrganismLogic.Size = new System.Drawing.Size(90, 21);
            this.cmbOrganismLogic.TabIndex = 1;
            this.cmbOrganismLogic.ValueMember = "Key";
            // 
            // cmbDefinitionLogic
            // 
            this.cmbDefinitionLogic.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cmbDefinitionLogic.DisplayMember = "Value";
            this.cmbDefinitionLogic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDefinitionLogic.FormattingEnabled = true;
            this.cmbDefinitionLogic.Location = new System.Drawing.Point(100, 36);
            this.cmbDefinitionLogic.Margin = new System.Windows.Forms.Padding(5);
            this.cmbDefinitionLogic.Name = "cmbDefinitionLogic";
            this.cmbDefinitionLogic.Size = new System.Drawing.Size(90, 21);
            this.cmbDefinitionLogic.TabIndex = 4;
            this.cmbDefinitionLogic.ValueMember = "Key";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 98);
            this.label3.Margin = new System.Windows.Forms.Padding(5, 5, 5, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Taxonomy:";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 72);
            this.label4.Margin = new System.Windows.Forms.Padding(5, 5, 5, 3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Gene:";
            // 
            // cmbGeneNameLogic
            // 
            this.cmbGeneNameLogic.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cmbGeneNameLogic.DisplayMember = "Value";
            this.cmbGeneNameLogic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGeneNameLogic.FormattingEnabled = true;
            this.cmbGeneNameLogic.Location = new System.Drawing.Point(100, 67);
            this.cmbGeneNameLogic.Margin = new System.Windows.Forms.Padding(5);
            this.cmbGeneNameLogic.Name = "cmbGeneNameLogic";
            this.cmbGeneNameLogic.Size = new System.Drawing.Size(90, 21);
            this.cmbGeneNameLogic.TabIndex = 7;
            this.cmbGeneNameLogic.ValueMember = "Key";
            // 
            // cmbGeneName
            // 
            this.cmbGeneName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbGeneName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbGeneName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbGeneName.FormattingEnabled = true;
            this.cmbGeneName.Location = new System.Drawing.Point(200, 67);
            this.cmbGeneName.Margin = new System.Windows.Forms.Padding(5);
            this.cmbGeneName.Name = "cmbGeneName";
            this.cmbGeneName.Size = new System.Drawing.Size(190, 21);
            this.cmbGeneName.TabIndex = 8;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.ImageKey = "Cancel";
            this.btnCancel.Location = new System.Drawing.Point(326, 389);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 24);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnApply
            // 
            this.btnApply.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnApply.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnApply.ImageKey = "Filter";
            this.btnApply.Location = new System.Drawing.Point(240, 389);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(80, 24);
            this.btnApply.TabIndex = 3;
            this.btnApply.Text = "&Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnClear
            // 
            this.btnClear.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnClear.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClear.ImageKey = "Filter";
            this.btnClear.Location = new System.Drawing.Point(3, 389);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(80, 24);
            this.btnClear.TabIndex = 2;
            this.btnClear.Text = "&Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // chkDuplicatesByOrganism
            // 
            this.chkDuplicatesByOrganism.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkDuplicatesByOrganism.AutoSize = true;
            this.chkDuplicatesByOrganism.Location = new System.Drawing.Point(10, 5);
            this.chkDuplicatesByOrganism.Margin = new System.Windows.Forms.Padding(10, 5, 5, 5);
            this.chkDuplicatesByOrganism.Name = "chkDuplicatesByOrganism";
            this.chkDuplicatesByOrganism.Size = new System.Drawing.Size(195, 17);
            this.chkDuplicatesByOrganism.TabIndex = 0;
            this.chkDuplicatesByOrganism.Text = "Only show sequences for organisms";
            this.chkDuplicatesByOrganism.UseVisualStyleBackColor = true;
            // 
            // chkBLASTN
            // 
            this.chkBLASTN.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkBLASTN.AutoSize = true;
            this.chkBLASTN.Location = new System.Drawing.Point(10, 157);
            this.chkBLASTN.Margin = new System.Windows.Forms.Padding(10, 5, 5, 5);
            this.chkBLASTN.Name = "chkBLASTN";
            this.chkBLASTN.Size = new System.Drawing.Size(151, 17);
            this.chkBLASTN.TabIndex = 4;
            this.chkBLASTN.Text = "Only show sequences that";
            this.chkBLASTN.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.rbDuplicates_Have, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.rbDuplicates_HaveNot, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 27);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(395, 49);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // rbDuplicates_Have
            // 
            this.rbDuplicates_Have.AutoSize = true;
            this.rbDuplicates_Have.Checked = true;
            this.rbDuplicates_Have.Location = new System.Drawing.Point(20, 5);
            this.rbDuplicates_Have.Margin = new System.Windows.Forms.Padding(20, 5, 5, 5);
            this.rbDuplicates_Have.Name = "rbDuplicates_Have";
            this.rbDuplicates_Have.Size = new System.Drawing.Size(165, 17);
            this.rbDuplicates_Have.TabIndex = 0;
            this.rbDuplicates_Have.TabStop = true;
            this.rbDuplicates_Have.Text = "with more than one sequence";
            this.rbDuplicates_Have.UseVisualStyleBackColor = true;
            // 
            // rbDuplicates_HaveNot
            // 
            this.rbDuplicates_HaveNot.AutoSize = true;
            this.rbDuplicates_HaveNot.Location = new System.Drawing.Point(20, 27);
            this.rbDuplicates_HaveNot.Margin = new System.Windows.Forms.Padding(20, 0, 5, 5);
            this.rbDuplicates_HaveNot.Name = "rbDuplicates_HaveNot";
            this.rbDuplicates_HaveNot.Size = new System.Drawing.Size(163, 17);
            this.rbDuplicates_HaveNot.TabIndex = 1;
            this.rbDuplicates_HaveNot.Text = "that have only one sequence";
            this.rbDuplicates_HaveNot.UseVisualStyleBackColor = true;
            // 
            // tblBLASTN
            // 
            this.tblBLASTN.AutoSize = true;
            this.tblBLASTN.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tblBLASTN.ColumnCount = 1;
            this.tblBLASTN.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tblBLASTN.Controls.Add(this.rbBLASTN_Results, 0, 0);
            this.tblBLASTN.Controls.Add(this.rbBLASTN_NotSubmitted, 0, 1);
            this.tblBLASTN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblBLASTN.Location = new System.Drawing.Point(0, 179);
            this.tblBLASTN.Margin = new System.Windows.Forms.Padding(0);
            this.tblBLASTN.Name = "tblBLASTN";
            this.tblBLASTN.RowCount = 2;
            this.tblBLASTN.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblBLASTN.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblBLASTN.Size = new System.Drawing.Size(395, 49);
            this.tblBLASTN.TabIndex = 5;
            // 
            // rbBLASTN_Results
            // 
            this.rbBLASTN_Results.AutoSize = true;
            this.rbBLASTN_Results.Checked = true;
            this.rbBLASTN_Results.Location = new System.Drawing.Point(20, 5);
            this.rbBLASTN_Results.Margin = new System.Windows.Forms.Padding(20, 5, 5, 5);
            this.rbBLASTN_Results.Name = "rbBLASTN_Results";
            this.rbBLASTN_Results.Size = new System.Drawing.Size(317, 17);
            this.rbBLASTN_Results.TabIndex = 0;
            this.rbBLASTN_Results.TabStop = true;
            this.rbBLASTN_Results.Text = "have been processed by BLASTN (NCBI) for alignment results";
            this.rbBLASTN_Results.UseVisualStyleBackColor = true;
            // 
            // rbBLASTN_NotSubmitted
            // 
            this.rbBLASTN_NotSubmitted.AutoSize = true;
            this.rbBLASTN_NotSubmitted.Location = new System.Drawing.Point(20, 27);
            this.rbBLASTN_NotSubmitted.Margin = new System.Windows.Forms.Padding(20, 0, 5, 5);
            this.rbBLASTN_NotSubmitted.Name = "rbBLASTN_NotSubmitted";
            this.rbBLASTN_NotSubmitted.Size = new System.Drawing.Size(146, 17);
            this.rbBLASTN_NotSubmitted.TabIndex = 1;
            this.rbBLASTN_NotSubmitted.Text = "have not been processed";
            this.rbBLASTN_NotSubmitted.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.chkDuplicatesByOrganism, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.tblBLASTN, 0, 5);
            this.tableLayoutPanel4.Controls.Add(this.chkBLASTN, 0, 4);
            this.tableLayoutPanel4.Controls.Add(this.chkSequenceMatch, 0, 2);
            this.tableLayoutPanel4.Controls.Add(this.tblSequenceMatch, 0, 3);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 7;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(395, 354);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // chkSequenceMatch
            // 
            this.chkSequenceMatch.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkSequenceMatch.AutoSize = true;
            this.chkSequenceMatch.Location = new System.Drawing.Point(10, 81);
            this.chkSequenceMatch.Margin = new System.Windows.Forms.Padding(10, 5, 5, 5);
            this.chkSequenceMatch.Name = "chkSequenceMatch";
            this.chkSequenceMatch.Size = new System.Drawing.Size(172, 17);
            this.chkSequenceMatch.TabIndex = 2;
            this.chkSequenceMatch.Text = "Only show identical sequences";
            this.chkSequenceMatch.UseVisualStyleBackColor = true;
            // 
            // tblSequenceMatch
            // 
            this.tblSequenceMatch.AutoSize = true;
            this.tblSequenceMatch.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tblSequenceMatch.ColumnCount = 1;
            this.tblSequenceMatch.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tblSequenceMatch.Controls.Add(this.rbSequenceMatch_CDS, 0, 0);
            this.tblSequenceMatch.Controls.Add(this.rbSequenceMatch_Whole, 0, 1);
            this.tblSequenceMatch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblSequenceMatch.Location = new System.Drawing.Point(0, 103);
            this.tblSequenceMatch.Margin = new System.Windows.Forms.Padding(0);
            this.tblSequenceMatch.Name = "tblSequenceMatch";
            this.tblSequenceMatch.RowCount = 2;
            this.tblSequenceMatch.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblSequenceMatch.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblSequenceMatch.Size = new System.Drawing.Size(395, 49);
            this.tblSequenceMatch.TabIndex = 3;
            // 
            // rbSequenceMatch_CDS
            // 
            this.rbSequenceMatch_CDS.AutoSize = true;
            this.rbSequenceMatch_CDS.Checked = true;
            this.rbSequenceMatch_CDS.Location = new System.Drawing.Point(20, 5);
            this.rbSequenceMatch_CDS.Margin = new System.Windows.Forms.Padding(20, 5, 5, 5);
            this.rbSequenceMatch_CDS.Name = "rbSequenceMatch_CDS";
            this.rbSequenceMatch_CDS.Size = new System.Drawing.Size(185, 17);
            this.rbSequenceMatch_CDS.TabIndex = 0;
            this.rbSequenceMatch_CDS.TabStop = true;
            this.rbSequenceMatch_CDS.Text = "by matching the coding sequence";
            this.rbSequenceMatch_CDS.UseVisualStyleBackColor = true;
            // 
            // rbSequenceMatch_Whole
            // 
            this.rbSequenceMatch_Whole.AutoSize = true;
            this.rbSequenceMatch_Whole.Location = new System.Drawing.Point(20, 27);
            this.rbSequenceMatch_Whole.Margin = new System.Windows.Forms.Padding(20, 0, 5, 5);
            this.rbSequenceMatch_Whole.Name = "rbSequenceMatch_Whole";
            this.rbSequenceMatch_Whole.Size = new System.Drawing.Size(231, 17);
            this.rbSequenceMatch_Whole.TabIndex = 1;
            this.rbSequenceMatch_Whole.Text = "by matching the entire nucleotide sequence";
            this.rbSequenceMatch_Whole.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 4;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.Controls.Add(this.tabControl1, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.btnApply, 2, 1);
            this.tableLayoutPanel5.Controls.Add(this.btnCancel, 3, 1);
            this.tableLayoutPanel5.Controls.Add(this.btnClear, 0, 1);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 2;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(409, 416);
            this.tableLayoutPanel5.TabIndex = 1;
            // 
            // tabControl1
            // 
            this.tableLayoutPanel5.SetColumnSpan(this.tabControl1, 4);
            this.tabControl1.Controls.Add(this.pgRecordFilters);
            this.tabControl1.Controls.Add(this.pgSequenceFilters);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(409, 386);
            this.tabControl1.TabIndex = 1;
            // 
            // pgRecordFilters
            // 
            this.pgRecordFilters.Controls.Add(this.tableLayoutPanel1);
            this.pgRecordFilters.Location = new System.Drawing.Point(4, 22);
            this.pgRecordFilters.Name = "pgRecordFilters";
            this.pgRecordFilters.Padding = new System.Windows.Forms.Padding(3);
            this.pgRecordFilters.Size = new System.Drawing.Size(401, 360);
            this.pgRecordFilters.TabIndex = 0;
            this.pgRecordFilters.Text = "Gene Sequence Record Filters";
            this.pgRecordFilters.UseVisualStyleBackColor = true;
            // 
            // pgSequenceFilters
            // 
            this.pgSequenceFilters.Controls.Add(this.tableLayoutPanel4);
            this.pgSequenceFilters.Location = new System.Drawing.Point(4, 22);
            this.pgSequenceFilters.Name = "pgSequenceFilters";
            this.pgSequenceFilters.Padding = new System.Windows.Forms.Padding(3);
            this.pgSequenceFilters.Size = new System.Drawing.Size(401, 360);
            this.pgSequenceFilters.TabIndex = 1;
            this.pgSequenceFilters.Text = "Nucleotide Sequence Filters";
            this.pgSequenceFilters.UseVisualStyleBackColor = true;
            // 
            // frmFilterGenes
            // 
            this.AcceptButton = this.btnApply;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(409, 416);
            this.Controls.Add(this.tableLayoutPanel5);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(425, 450);
            this.Name = "frmFilterGenes";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Filter Gene Sequences";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmFilterGenes_FormClosing);
            this.Load += new System.EventHandler(this.frmFilterGenes_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tblBLASTN.ResumeLayout(false);
            this.tblBLASTN.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.tblSequenceMatch.ResumeLayout(false);
            this.tblSequenceMatch.PerformLayout();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.pgRecordFilters.ResumeLayout(false);
            this.pgSequenceFilters.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TreeView tvTaxonomy;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtDefinition;
        private System.Windows.Forms.ComboBox cmbOrganism;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.CheckBox chkDuplicatesByOrganism;
        private System.Windows.Forms.CheckBox chkBLASTN;
        private System.Windows.Forms.RadioButton rbBLASTN_Results;
        private System.Windows.Forms.RadioButton rbDuplicates_Have;
        private System.Windows.Forms.RadioButton rbDuplicates_HaveNot;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tblBLASTN;
        private System.Windows.Forms.RadioButton rbBLASTN_NotSubmitted;
        private System.Windows.Forms.ComboBox cmbOrganismLogic;
        private System.Windows.Forms.ComboBox cmbDefinitionLogic;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.CheckBox chkSequenceMatch;
        private System.Windows.Forms.TableLayoutPanel tblSequenceMatch;
        private System.Windows.Forms.RadioButton rbSequenceMatch_CDS;
        private System.Windows.Forms.RadioButton rbSequenceMatch_Whole;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage pgRecordFilters;
        private System.Windows.Forms.TabPage pgSequenceFilters;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbGeneNameLogic;
        private System.Windows.Forms.ComboBox cmbGeneName;
    }
}