namespace Pilgrimage.GeneSequences.BlastN
{
    partial class frmAnnotatedGeneDetails
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAnnotatedGeneDetails));
            this.tblDetails = new System.Windows.Forms.TableLayoutPanel();
            this.grdResults = new System.Windows.Forms.DataGridView();
            this.clmDefinition = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmGenBankUrl = new System.Windows.Forms.DataGridViewLinkColumn();
            this.clmBitScore = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmQueryCover = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmLengthDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmIdentities = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label7 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.txtDefinition = new System.Windows.Forms.TextBox();
            this.txtOriginalSequence = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtMergedDescription = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.lblDefinition = new System.Windows.Forms.Label();
            this.txtOriginalDefinition = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.txtOriginalOrganism = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txtOriginalTaxonomy = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtOriginalGeneName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtOriginalAccession = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtOriginalLocus = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tblButtons = new System.Windows.Forms.TableLayoutPanel();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.txtCodingSequence = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tblDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdResults)).BeginInit();
            this.tblButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // tblDetails
            // 
            this.tblDetails.ColumnCount = 6;
            this.tblDetails.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tblDetails.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tblDetails.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tblDetails.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.34F));
            this.tblDetails.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tblDetails.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tblDetails.Controls.Add(this.grdResults, 0, 1);
            this.tblDetails.Controls.Add(this.label7, 0, 10);
            this.tblDetails.Controls.Add(this.btnClose, 5, 13);
            this.tblDetails.Controls.Add(this.txtDefinition, 1, 3);
            this.tblDetails.Controls.Add(this.txtOriginalSequence, 1, 10);
            this.tblDetails.Controls.Add(this.label12, 0, 12);
            this.tblDetails.Controls.Add(this.txtMergedDescription, 1, 12);
            this.tblDetails.Controls.Add(this.label10, 0, 0);
            this.tblDetails.Controls.Add(this.lblDefinition, 0, 2);
            this.tblDetails.Controls.Add(this.txtOriginalDefinition, 1, 2);
            this.tblDetails.Controls.Add(this.label11, 0, 3);
            this.tblDetails.Controls.Add(this.label14, 0, 4);
            this.tblDetails.Controls.Add(this.txtOriginalOrganism, 1, 4);
            this.tblDetails.Controls.Add(this.label15, 2, 4);
            this.tblDetails.Controls.Add(this.txtOriginalTaxonomy, 3, 4);
            this.tblDetails.Controls.Add(this.label2, 0, 7);
            this.tblDetails.Controls.Add(this.txtOriginalGeneName, 1, 7);
            this.tblDetails.Controls.Add(this.label4, 2, 7);
            this.tblDetails.Controls.Add(this.txtOriginalAccession, 3, 7);
            this.tblDetails.Controls.Add(this.label3, 4, 7);
            this.tblDetails.Controls.Add(this.txtOriginalLocus, 5, 7);
            this.tblDetails.Controls.Add(this.groupBox2, 0, 9);
            this.tblDetails.Controls.Add(this.tblButtons, 0, 13);
            this.tblDetails.Controls.Add(this.txtCodingSequence, 1, 11);
            this.tblDetails.Controls.Add(this.label1, 0, 11);
            this.tblDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblDetails.Location = new System.Drawing.Point(0, 0);
            this.tblDetails.Name = "tblDetails";
            this.tblDetails.RowCount = 14;
            this.tblDetails.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tblDetails.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblDetails.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblDetails.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblDetails.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblDetails.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblDetails.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblDetails.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblDetails.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tblDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tblDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tblDetails.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tblDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tblDetails.Size = new System.Drawing.Size(784, 566);
            this.tblDetails.TabIndex = 1;
            // 
            // grdResults
            // 
            this.grdResults.AllowUserToAddRows = false;
            this.grdResults.AllowUserToDeleteRows = false;
            this.grdResults.AllowUserToResizeRows = false;
            this.grdResults.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grdResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdResults.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmDefinition,
            this.clmGenBankUrl,
            this.clmBitScore,
            this.clmQueryCover,
            this.clmLengthDescription,
            this.clmIdentities});
            this.tblDetails.SetColumnSpan(this.grdResults, 6);
            this.grdResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdResults.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.grdResults.Location = new System.Drawing.Point(3, 26);
            this.grdResults.MultiSelect = false;
            this.grdResults.Name = "grdResults";
            this.grdResults.RowHeadersVisible = false;
            this.grdResults.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdResults.Size = new System.Drawing.Size(778, 147);
            this.grdResults.TabIndex = 27;
            this.grdResults.SelectionChanged += new System.EventHandler(this.grdResults_SelectionChanged);
            // 
            // clmDefinition
            // 
            this.clmDefinition.DataPropertyName = "Definition";
            this.clmDefinition.FillWeight = 55F;
            this.clmDefinition.HeaderText = "Definition";
            this.clmDefinition.Name = "clmDefinition";
            this.clmDefinition.ReadOnly = true;
            // 
            // clmGenBankUrl
            // 
            this.clmGenBankUrl.DataPropertyName = "GenBankUrl";
            this.clmGenBankUrl.FillWeight = 17F;
            this.clmGenBankUrl.HeaderText = "GenBank";
            this.clmGenBankUrl.Name = "clmGenBankUrl";
            this.clmGenBankUrl.ReadOnly = true;
            // 
            // clmBitScore
            // 
            this.clmBitScore.DataPropertyName = "MaxScore";
            dataGridViewCellStyle1.Format = "F0";
            this.clmBitScore.DefaultCellStyle = dataGridViewCellStyle1;
            this.clmBitScore.FillWeight = 7F;
            this.clmBitScore.HeaderText = "Max Score";
            this.clmBitScore.Name = "clmBitScore";
            this.clmBitScore.ReadOnly = true;
            // 
            // clmQueryCover
            // 
            this.clmQueryCover.DataPropertyName = "QueryCover";
            dataGridViewCellStyle2.Format = "P0";
            this.clmQueryCover.DefaultCellStyle = dataGridViewCellStyle2;
            this.clmQueryCover.FillWeight = 7F;
            this.clmQueryCover.HeaderText = "Query Cover";
            this.clmQueryCover.Name = "clmQueryCover";
            this.clmQueryCover.ReadOnly = true;
            // 
            // clmLengthDescription
            // 
            this.clmLengthDescription.DataPropertyName = "AlignmentLength";
            dataGridViewCellStyle3.Format = "N0";
            this.clmLengthDescription.DefaultCellStyle = dataGridViewCellStyle3;
            this.clmLengthDescription.FillWeight = 7F;
            this.clmLengthDescription.HeaderText = "Aligned Length";
            this.clmLengthDescription.Name = "clmLengthDescription";
            this.clmLengthDescription.ReadOnly = true;
            // 
            // clmIdentities
            // 
            this.clmIdentities.DataPropertyName = "AlignmentPercentage";
            dataGridViewCellStyle4.Format = "P0";
            this.clmIdentities.DefaultCellStyle = dataGridViewCellStyle4;
            this.clmIdentities.FillWeight = 7F;
            this.clmIdentities.HeaderText = "Identities Match";
            this.clmIdentities.Name = "clmIdentities";
            this.clmIdentities.ReadOnly = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(5, 310);
            this.label7.Margin = new System.Windows.Forms.Padding(5);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(96, 13);
            this.label7.TabIndex = 18;
            this.label7.Text = "Source Sequence:";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.ImageKey = "Cancel";
            this.btnClose.Location = new System.Drawing.Point(701, 537);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 24);
            this.btnClose.TabIndex = 26;
            this.btnClose.Text = "&Cancel";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // txtDefinition
            // 
            this.txtDefinition.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tblDetails.SetColumnSpan(this.txtDefinition, 5);
            this.txtDefinition.Location = new System.Drawing.Point(120, 211);
            this.txtDefinition.Margin = new System.Windows.Forms.Padding(5);
            this.txtDefinition.MaxLength = 1000;
            this.txtDefinition.Name = "txtDefinition";
            this.txtDefinition.Size = new System.Drawing.Size(659, 20);
            this.txtDefinition.TabIndex = 1;
            // 
            // txtOriginalSequence
            // 
            this.tblDetails.SetColumnSpan(this.txtOriginalSequence, 5);
            this.txtOriginalSequence.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtOriginalSequence.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOriginalSequence.Location = new System.Drawing.Point(120, 310);
            this.txtOriginalSequence.Margin = new System.Windows.Forms.Padding(5);
            this.txtOriginalSequence.MaxLength = 100000;
            this.txtOriginalSequence.Multiline = true;
            this.txtOriginalSequence.Name = "txtOriginalSequence";
            this.txtOriginalSequence.ReadOnly = true;
            this.txtOriginalSequence.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtOriginalSequence.Size = new System.Drawing.Size(659, 66);
            this.txtOriginalSequence.TabIndex = 19;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(5, 462);
            this.label12.Margin = new System.Windows.Forms.Padding(5);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(38, 13);
            this.label12.TabIndex = 21;
            this.label12.Text = "Notes:";
            // 
            // txtMergedDescription
            // 
            this.tblDetails.SetColumnSpan(this.txtMergedDescription, 5);
            this.txtMergedDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMergedDescription.Location = new System.Drawing.Point(120, 462);
            this.txtMergedDescription.Margin = new System.Windows.Forms.Padding(5);
            this.txtMergedDescription.MaxLength = 100000;
            this.txtMergedDescription.Multiline = true;
            this.txtMergedDescription.Name = "txtMergedDescription";
            this.txtMergedDescription.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtMergedDescription.Size = new System.Drawing.Size(659, 66);
            this.txtMergedDescription.TabIndex = 22;
            // 
            // label10
            // 
            this.label10.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(5, 5);
            this.label10.Margin = new System.Windows.Forms.Padding(5);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(105, 13);
            this.label10.TabIndex = 0;
            this.label10.Text = "Results from BLAST:";
            // 
            // lblDefinition
            // 
            this.lblDefinition.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblDefinition.AutoSize = true;
            this.lblDefinition.Location = new System.Drawing.Point(5, 184);
            this.lblDefinition.Margin = new System.Windows.Forms.Padding(5);
            this.lblDefinition.Name = "lblDefinition";
            this.lblDefinition.Size = new System.Drawing.Size(92, 13);
            this.lblDefinition.TabIndex = 0;
            this.lblDefinition.Text = "Original Definition:";
            // 
            // txtOriginalDefinition
            // 
            this.txtOriginalDefinition.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tblDetails.SetColumnSpan(this.txtOriginalDefinition, 5);
            this.txtOriginalDefinition.Location = new System.Drawing.Point(120, 181);
            this.txtOriginalDefinition.Margin = new System.Windows.Forms.Padding(5);
            this.txtOriginalDefinition.MaxLength = 1000;
            this.txtOriginalDefinition.Name = "txtOriginalDefinition";
            this.txtOriginalDefinition.ReadOnly = true;
            this.txtOriginalDefinition.Size = new System.Drawing.Size(659, 20);
            this.txtOriginalDefinition.TabIndex = 1;
            // 
            // label11
            // 
            this.label11.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(5, 214);
            this.label11.Margin = new System.Windows.Forms.Padding(5);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(67, 13);
            this.label11.TabIndex = 0;
            this.label11.Text = "Updated To:";
            // 
            // label14
            // 
            this.label14.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(5, 244);
            this.label14.Margin = new System.Windows.Forms.Padding(5);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(54, 13);
            this.label14.TabIndex = 2;
            this.label14.Text = "Organism:";
            // 
            // txtOriginalOrganism
            // 
            this.txtOriginalOrganism.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOriginalOrganism.Location = new System.Drawing.Point(120, 241);
            this.txtOriginalOrganism.Margin = new System.Windows.Forms.Padding(5);
            this.txtOriginalOrganism.MaxLength = 250;
            this.txtOriginalOrganism.Name = "txtOriginalOrganism";
            this.txtOriginalOrganism.ReadOnly = true;
            this.txtOriginalOrganism.Size = new System.Drawing.Size(173, 20);
            this.txtOriginalOrganism.TabIndex = 3;
            // 
            // label15
            // 
            this.label15.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(303, 244);
            this.label15.Margin = new System.Windows.Forms.Padding(5);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(59, 13);
            this.label15.TabIndex = 4;
            this.label15.Text = "Taxonomy:";
            // 
            // txtOriginalTaxonomy
            // 
            this.txtOriginalTaxonomy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tblDetails.SetColumnSpan(this.txtOriginalTaxonomy, 3);
            this.txtOriginalTaxonomy.Location = new System.Drawing.Point(372, 241);
            this.txtOriginalTaxonomy.Margin = new System.Windows.Forms.Padding(5);
            this.txtOriginalTaxonomy.MaxLength = 4000;
            this.txtOriginalTaxonomy.Name = "txtOriginalTaxonomy";
            this.txtOriginalTaxonomy.ReadOnly = true;
            this.txtOriginalTaxonomy.Size = new System.Drawing.Size(407, 20);
            this.txtOriginalTaxonomy.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 274);
            this.label2.Margin = new System.Windows.Forms.Padding(5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Gene:";
            // 
            // txtOriginalGeneName
            // 
            this.txtOriginalGeneName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOriginalGeneName.Location = new System.Drawing.Point(120, 271);
            this.txtOriginalGeneName.Margin = new System.Windows.Forms.Padding(5);
            this.txtOriginalGeneName.MaxLength = 100;
            this.txtOriginalGeneName.Name = "txtOriginalGeneName";
            this.txtOriginalGeneName.ReadOnly = true;
            this.txtOriginalGeneName.Size = new System.Drawing.Size(173, 20);
            this.txtOriginalGeneName.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(303, 274);
            this.label4.Margin = new System.Windows.Forms.Padding(5);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Accession:";
            // 
            // txtOriginalAccession
            // 
            this.txtOriginalAccession.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOriginalAccession.Location = new System.Drawing.Point(372, 271);
            this.txtOriginalAccession.Margin = new System.Windows.Forms.Padding(5);
            this.txtOriginalAccession.MaxLength = 20;
            this.txtOriginalAccession.Name = "txtOriginalAccession";
            this.txtOriginalAccession.ReadOnly = true;
            this.txtOriginalAccession.Size = new System.Drawing.Size(173, 20);
            this.txtOriginalAccession.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(555, 274);
            this.label3.Margin = new System.Windows.Forms.Padding(5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Locus:";
            // 
            // txtOriginalLocus
            // 
            this.txtOriginalLocus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOriginalLocus.Location = new System.Drawing.Point(604, 271);
            this.txtOriginalLocus.Margin = new System.Windows.Forms.Padding(5);
            this.txtOriginalLocus.MaxLength = 100;
            this.txtOriginalLocus.Name = "txtOriginalLocus";
            this.txtOriginalLocus.ReadOnly = true;
            this.txtOriginalLocus.Size = new System.Drawing.Size(175, 20);
            this.txtOriginalLocus.TabIndex = 12;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tblDetails.SetColumnSpan(this.groupBox2, 6);
            this.groupBox2.Location = new System.Drawing.Point(3, 299);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(778, 3);
            this.groupBox2.TabIndex = 28;
            this.groupBox2.TabStop = false;
            // 
            // tblButtons
            // 
            this.tblButtons.AutoSize = true;
            this.tblButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tblButtons.ColumnCount = 2;
            this.tblDetails.SetColumnSpan(this.tblButtons, 3);
            this.tblButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tblButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tblButtons.Controls.Add(this.btnSave, 0, 0);
            this.tblButtons.Controls.Add(this.btnClear, 1, 0);
            this.tblButtons.Location = new System.Drawing.Point(0, 533);
            this.tblButtons.Margin = new System.Windows.Forms.Padding(0);
            this.tblButtons.Name = "tblButtons";
            this.tblButtons.RowCount = 1;
            this.tblButtons.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblButtons.Size = new System.Drawing.Size(182, 30);
            this.tblButtons.TabIndex = 29;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.ImageKey = "Cancel";
            this.btnSave.Location = new System.Drawing.Point(3, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 24);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "&Update";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClear
            // 
            this.btnClear.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnClear.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClear.ImageKey = "Cancel";
            this.btnClear.Location = new System.Drawing.Point(89, 3);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(90, 24);
            this.btnClear.TabIndex = 0;
            this.btnClear.Text = "&Clear All";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Visible = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // txtCodingSequence
            // 
            this.tblDetails.SetColumnSpan(this.txtCodingSequence, 5);
            this.txtCodingSequence.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCodingSequence.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCodingSequence.Location = new System.Drawing.Point(120, 386);
            this.txtCodingSequence.Margin = new System.Windows.Forms.Padding(5);
            this.txtCodingSequence.MaxLength = 100000;
            this.txtCodingSequence.Multiline = true;
            this.txtCodingSequence.Name = "txtCodingSequence";
            this.txtCodingSequence.ReadOnly = true;
            this.txtCodingSequence.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtCodingSequence.Size = new System.Drawing.Size(659, 66);
            this.txtCodingSequence.TabIndex = 19;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 386);
            this.label1.Margin = new System.Windows.Forms.Padding(5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "Match Sequence:";
            // 
            // frmAnnotatedGeneDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(784, 566);
            this.Controls.Add(this.tblDetails);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(500, 450);
            this.Name = "frmAnnotatedGeneDetails";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Details";
            this.Load += new System.EventHandler(this.frmAnnotatedGeneDetails_Load);
            this.tblDetails.ResumeLayout(false);
            this.tblDetails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdResults)).EndInit();
            this.tblButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tblDetails;
        private System.Windows.Forms.Label lblDefinition;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TextBox txtDefinition;
        private System.Windows.Forms.TextBox txtOriginalOrganism;
        private System.Windows.Forms.TextBox txtOriginalSequence;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtMergedDescription;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView grdResults;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtOriginalDefinition;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtOriginalTaxonomy;
        private System.Windows.Forms.TextBox txtOriginalGeneName;
        private System.Windows.Forms.TextBox txtOriginalAccession;
        private System.Windows.Forms.TextBox txtOriginalLocus;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TableLayoutPanel tblButtons;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmDefinition;
        private System.Windows.Forms.DataGridViewLinkColumn clmGenBankUrl;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmBitScore;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmQueryCover;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmLengthDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmIdentities;
        private System.Windows.Forms.TextBox txtCodingSequence;
        private System.Windows.Forms.Label label1;
    }
}