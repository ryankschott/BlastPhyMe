namespace Pilgrimage.GeneSequences.BlastN
{
    partial class frmSubjectGeneDetails
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSubjectGeneDetails));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.grdResults = new System.Windows.Forms.DataGridView();
            this.clmDefinition = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmOrganism = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmBitScore = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmQueryCover = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmLengthDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmIdentities = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnClose = new System.Windows.Forms.Button();
            this.ilButtons = new System.Windows.Forms.ImageList(this.components);
            this.label10 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtGenBankID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtDefinition = new System.Windows.Forms.TextBox();
            this.txtOrganism = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtTaxonomy = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtCodingSequence = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtLastUpdated = new System.Windows.Forms.TextBox();
            this.lnkGenBankURL = new System.Windows.Forms.LinkLabel();
            this.label8 = new System.Windows.Forms.Label();
            this.txtLength = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtLocus = new System.Windows.Forms.TextBox();
            this.txtAccession = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtGeneName = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdResults)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 6;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.34F));
            this.tableLayoutPanel1.Controls.Add(this.grdResults, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.btnClose, 5, 9);
            this.tableLayoutPanel1.Controls.Add(this.label10, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtDefinition, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtOrganism, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label6, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtTaxonomy, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.label7, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.txtCodingSequence, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.label9, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.txtLastUpdated, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.lnkGenBankURL, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.label8, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.txtLength, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.label3, 4, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtLocus, 5, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtGeneName, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label11, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtGenBankID, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtAccession, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.label1, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 10;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1568, 1088);
            this.tableLayoutPanel1.TabIndex = 0;
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
            this.clmOrganism,
            this.clmBitScore,
            this.clmQueryCover,
            this.clmLengthDescription,
            this.clmIdentities});
            this.tableLayoutPanel1.SetColumnSpan(this.grdResults, 9);
            this.grdResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdResults.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.grdResults.Location = new System.Drawing.Point(6, 498);
            this.grdResults.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.grdResults.Name = "grdResults";
            this.grdResults.RowHeadersVisible = false;
            this.grdResults.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdResults.Size = new System.Drawing.Size(1556, 526);
            this.grdResults.TabIndex = 22;
            // 
            // clmDefinition
            // 
            this.clmDefinition.DataPropertyName = "Definition";
            this.clmDefinition.FillWeight = 54F;
            this.clmDefinition.HeaderText = "Definition";
            this.clmDefinition.Name = "clmDefinition";
            this.clmDefinition.ReadOnly = true;
            // 
            // clmOrganism
            // 
            this.clmOrganism.DataPropertyName = "Organism";
            this.clmOrganism.FillWeight = 18F;
            this.clmOrganism.HeaderText = "Organism";
            this.clmOrganism.Name = "clmOrganism";
            this.clmOrganism.ReadOnly = true;
            // 
            // clmBitScore
            // 
            this.clmBitScore.DataPropertyName = "MaxScore";
            dataGridViewCellStyle9.Format = "F0";
            this.clmBitScore.DefaultCellStyle = dataGridViewCellStyle9;
            this.clmBitScore.FillWeight = 7F;
            this.clmBitScore.HeaderText = "Max Score";
            this.clmBitScore.Name = "clmBitScore";
            this.clmBitScore.ReadOnly = true;
            // 
            // clmQueryCover
            // 
            this.clmQueryCover.DataPropertyName = "QueryCover";
            dataGridViewCellStyle10.Format = "P0";
            this.clmQueryCover.DefaultCellStyle = dataGridViewCellStyle10;
            this.clmQueryCover.FillWeight = 7F;
            this.clmQueryCover.HeaderText = "Query Cover";
            this.clmQueryCover.Name = "clmQueryCover";
            this.clmQueryCover.ReadOnly = true;
            // 
            // clmLengthDescription
            // 
            this.clmLengthDescription.DataPropertyName = "AlignmentLength";
            dataGridViewCellStyle11.Format = "N0";
            this.clmLengthDescription.DefaultCellStyle = dataGridViewCellStyle11;
            this.clmLengthDescription.FillWeight = 7F;
            this.clmLengthDescription.HeaderText = "Aligned Length";
            this.clmLengthDescription.Name = "clmLengthDescription";
            this.clmLengthDescription.ReadOnly = true;
            // 
            // clmIdentities
            // 
            this.clmIdentities.DataPropertyName = "AlignmentPercentage";
            dataGridViewCellStyle12.Format = "P0";
            this.clmIdentities.DefaultCellStyle = dataGridViewCellStyle12;
            this.clmIdentities.FillWeight = 7F;
            this.clmIdentities.HeaderText = "Identities Match";
            this.clmIdentities.Name = "clmIdentities";
            this.clmIdentities.ReadOnly = true;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.ImageKey = "Cancel";
            this.btnClose.ImageList = this.ilButtons;
            this.btnClose.Location = new System.Drawing.Point(1402, 1036);
            this.btnClose.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(160, 46);
            this.btnClose.TabIndex = 23;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // ilButtons
            // 
            this.ilButtons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilButtons.ImageStream")));
            this.ilButtons.TransparentColor = System.Drawing.Color.Transparent;
            this.ilButtons.Images.SetKeyName(0, "Add");
            this.ilButtons.Images.SetKeyName(1, "Cancel");
            // 
            // label10
            // 
            this.label10.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label10.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label10, 2);
            this.label10.Location = new System.Drawing.Point(10, 457);
            this.label10.Margin = new System.Windows.Forms.Padding(10, 19, 10, 10);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(391, 25);
            this.label10.TabIndex = 21;
            this.label10.Text = "Query genes resulting in this alignment:";
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(588, 115);
            this.label1.Margin = new System.Windows.Forms.Padding(10, 10, 10, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(133, 25);
            this.label1.TabIndex = 12;
            this.label1.Text = "GenBank ID:";
            // 
            // txtGenBankID
            // 
            this.txtGenBankID.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtGenBankID.Location = new System.Drawing.Point(741, 112);
            this.txtGenBankID.Margin = new System.Windows.Forms.Padding(10, 10, 10, 10);
            this.txtGenBankID.Name = "txtGenBankID";
            this.txtGenBankID.ReadOnly = true;
            this.txtGenBankID.Size = new System.Drawing.Size(349, 31);
            this.txtGenBankID.TabIndex = 13;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 13);
            this.label2.Margin = new System.Windows.Forms.Padding(10, 10, 10, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(108, 25);
            this.label2.TabIndex = 0;
            this.label2.Text = "Definition:";
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 64);
            this.label5.Margin = new System.Windows.Forms.Padding(10, 10, 10, 10);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(110, 25);
            this.label5.TabIndex = 2;
            this.label5.Text = "Organism:";
            // 
            // txtDefinition
            // 
            this.txtDefinition.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.txtDefinition, 5);
            this.txtDefinition.Location = new System.Drawing.Point(219, 10);
            this.txtDefinition.Margin = new System.Windows.Forms.Padding(10, 10, 10, 10);
            this.txtDefinition.Name = "txtDefinition";
            this.txtDefinition.ReadOnly = true;
            this.txtDefinition.Size = new System.Drawing.Size(1339, 31);
            this.txtDefinition.TabIndex = 1;
            // 
            // txtOrganism
            // 
            this.txtOrganism.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOrganism.Location = new System.Drawing.Point(219, 61);
            this.txtOrganism.Margin = new System.Windows.Forms.Padding(10, 10, 10, 10);
            this.txtOrganism.Name = "txtOrganism";
            this.txtOrganism.ReadOnly = true;
            this.txtOrganism.Size = new System.Drawing.Size(349, 31);
            this.txtOrganism.TabIndex = 3;
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(588, 64);
            this.label6.Margin = new System.Windows.Forms.Padding(10, 10, 10, 10);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(118, 25);
            this.label6.TabIndex = 4;
            this.label6.Text = "Taxonomy:";
            // 
            // txtTaxonomy
            // 
            this.txtTaxonomy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.txtTaxonomy, 3);
            this.txtTaxonomy.Location = new System.Drawing.Point(741, 61);
            this.txtTaxonomy.Margin = new System.Windows.Forms.Padding(10, 10, 10, 10);
            this.txtTaxonomy.Name = "txtTaxonomy";
            this.txtTaxonomy.ReadOnly = true;
            this.txtTaxonomy.Size = new System.Drawing.Size(817, 31);
            this.txtTaxonomy.TabIndex = 5;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 265);
            this.label7.Margin = new System.Windows.Forms.Padding(10, 10, 10, 10);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(189, 25);
            this.label7.TabIndex = 17;
            this.label7.Text = "Coding Sequence:";
            // 
            // txtCodingSequence
            // 
            this.txtCodingSequence.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.txtCodingSequence, 5);
            this.txtCodingSequence.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCodingSequence.Location = new System.Drawing.Point(219, 265);
            this.txtCodingSequence.Margin = new System.Windows.Forms.Padding(10, 10, 10, 10);
            this.txtCodingSequence.Multiline = true;
            this.txtCodingSequence.Name = "txtCodingSequence";
            this.txtCodingSequence.ReadOnly = true;
            this.txtCodingSequence.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtCodingSequence.Size = new System.Drawing.Size(1339, 112);
            this.txtCodingSequence.TabIndex = 18;
            this.txtCodingSequence.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtCodingSequence_KeyUp);
            // 
            // label9
            // 
            this.label9.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(10, 400);
            this.label9.Margin = new System.Windows.Forms.Padding(10, 10, 10, 10);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(172, 25);
            this.label9.TabIndex = 19;
            this.label9.Text = "Last Updated At:";
            // 
            // txtLastUpdated
            // 
            this.txtLastUpdated.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.txtLastUpdated, 2);
            this.txtLastUpdated.Location = new System.Drawing.Point(219, 397);
            this.txtLastUpdated.Margin = new System.Windows.Forms.Padding(10, 10, 10, 10);
            this.txtLastUpdated.Name = "txtLastUpdated";
            this.txtLastUpdated.ReadOnly = true;
            this.txtLastUpdated.Size = new System.Drawing.Size(502, 31);
            this.txtLastUpdated.TabIndex = 20;
            // 
            // lnkGenBankURL
            // 
            this.lnkGenBankURL.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lnkGenBankURL.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lnkGenBankURL, 4);
            this.lnkGenBankURL.Location = new System.Drawing.Point(588, 166);
            this.lnkGenBankURL.Margin = new System.Windows.Forms.Padding(10, 10, 10, 10);
            this.lnkGenBankURL.Name = "lnkGenBankURL";
            this.lnkGenBankURL.Size = new System.Drawing.Size(36, 25);
            this.lnkGenBankURL.TabIndex = 14;
            this.lnkGenBankURL.TabStop = true;
            this.lnkGenBankURL.Text = "<>";
            this.lnkGenBankURL.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkGenBankURL_LinkClicked);
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(10, 217);
            this.label8.Margin = new System.Windows.Forms.Padding(10, 10, 10, 10);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(84, 25);
            this.label8.TabIndex = 15;
            this.label8.Text = "Length:";
            // 
            // txtLength
            // 
            this.txtLength.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLength.Location = new System.Drawing.Point(219, 214);
            this.txtLength.Margin = new System.Windows.Forms.Padding(10, 10, 10, 10);
            this.txtLength.Name = "txtLength";
            this.txtLength.ReadOnly = true;
            this.txtLength.Size = new System.Drawing.Size(349, 31);
            this.txtLength.TabIndex = 16;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1110, 115);
            this.label3.Margin = new System.Windows.Forms.Padding(10, 10, 10, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 25);
            this.label3.TabIndex = 10;
            this.label3.Text = "Locus:";
            // 
            // txtLocus
            // 
            this.txtLocus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLocus.Location = new System.Drawing.Point(1206, 112);
            this.txtLocus.Margin = new System.Windows.Forms.Padding(10, 10, 10, 10);
            this.txtLocus.Name = "txtLocus";
            this.txtLocus.ReadOnly = true;
            this.txtLocus.Size = new System.Drawing.Size(352, 31);
            this.txtLocus.TabIndex = 11;
            // 
            // txtAccession
            // 
            this.txtAccession.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAccession.Location = new System.Drawing.Point(219, 163);
            this.txtAccession.Margin = new System.Windows.Forms.Padding(10, 10, 10, 10);
            this.txtAccession.Name = "txtAccession";
            this.txtAccession.ReadOnly = true;
            this.txtAccession.Size = new System.Drawing.Size(349, 31);
            this.txtAccession.TabIndex = 9;
            this.txtAccession.TextChanged += new System.EventHandler(this.txtAccession_TextChanged);
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 166);
            this.label4.Margin = new System.Windows.Forms.Padding(10, 10, 10, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(117, 25);
            this.label4.TabIndex = 8;
            this.label4.Text = "Accession:";
            // 
            // txtGeneName
            // 
            this.txtGeneName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtGeneName.Location = new System.Drawing.Point(219, 112);
            this.txtGeneName.Margin = new System.Windows.Forms.Padding(10, 10, 10, 10);
            this.txtGeneName.Name = "txtGeneName";
            this.txtGeneName.ReadOnly = true;
            this.txtGeneName.Size = new System.Drawing.Size(349, 31);
            this.txtGeneName.TabIndex = 7;
            // 
            // label11
            // 
            this.label11.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(10, 115);
            this.label11.Margin = new System.Windows.Forms.Padding(10, 10, 10, 10);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(70, 25);
            this.label11.TabIndex = 6;
            this.label11.Text = "Gene:";
            // 
            // frmSubjectGeneDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(1568, 1088);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.MinimumSize = new System.Drawing.Size(1474, 742);
            this.Name = "frmSubjectGeneDetails";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Alignment Details";
            this.Load += new System.EventHandler(this.frmAlignmentDetails_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdResults)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TextBox txtGenBankID;
        private System.Windows.Forms.TextBox txtDefinition;
        private System.Windows.Forms.TextBox txtLocus;
        private System.Windows.Forms.TextBox txtCodingSequence;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtAccession;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtLength;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.DataGridView grdResults;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtOrganism;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtTaxonomy;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtLastUpdated;
        private System.Windows.Forms.ImageList ilButtons;
        private System.Windows.Forms.LinkLabel lnkGenBankURL;
        private System.Windows.Forms.TextBox txtGeneName;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmDefinition;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmOrganism;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmBitScore;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmQueryCover;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmLengthDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmIdentities;

    }
}