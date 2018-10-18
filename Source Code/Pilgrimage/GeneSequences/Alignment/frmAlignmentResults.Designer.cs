namespace Pilgrimage.GeneSequences.Alignment
{
    partial class frmAlignmentResults<T> where T : ChangLab.Jobs.CommandLineGeneProcessingJob
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
            this.tbForm = new System.Windows.Forms.TabControl();
            this.pgResults = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lnkWorkingFolder = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.grdResults = new System.Windows.Forms.DataGridView();
            this.clmInclude = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.clmSequenceMatch = new System.Windows.Forms.DataGridViewImageColumn();
            this.clmInRecordset = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.clmOrganism = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmDefinition = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmNucleotides = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose_Results = new System.Windows.Forms.Button();
            this.cmbSubSets = new System.Windows.Forms.ComboBox();
            this.lnkClearFilter = new System.Windows.Forms.LinkLabel();
            this.chkToggleSelected = new System.Windows.Forms.CheckBox();
            this.lnkFilter = new System.Windows.Forms.LinkLabel();
            this.lblSelectedRows = new System.Windows.Forms.Label();
            this.lblTotalRows = new System.Windows.Forms.Label();
            this.pgOutput = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.btnClose_Output = new System.Windows.Forms.Button();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.btnSaveOutput = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.tbForm.SuspendLayout();
            this.pgResults.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdResults)).BeginInit();
            this.pgOutput.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbForm
            // 
            this.tbForm.Controls.Add(this.pgResults);
            this.tbForm.Controls.Add(this.pgOutput);
            this.tbForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbForm.Location = new System.Drawing.Point(0, 0);
            this.tbForm.Name = "tbForm";
            this.tbForm.SelectedIndex = 0;
            this.tbForm.Size = new System.Drawing.Size(884, 562);
            this.tbForm.TabIndex = 1;
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
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.lnkWorkingFolder, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.grdResults, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnSave, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.btnClose_Results, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.cmbSubSets, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.lnkClearFilter, 4, 1);
            this.tableLayoutPanel1.Controls.Add(this.chkToggleSelected, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lnkFilter, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblSelectedRows, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblTotalRows, 2, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(876, 536);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lnkWorkingFolder
            // 
            this.lnkWorkingFolder.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lnkWorkingFolder.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lnkWorkingFolder, 3);
            this.lnkWorkingFolder.Location = new System.Drawing.Point(792, 488);
            this.lnkWorkingFolder.Margin = new System.Windows.Forms.Padding(5);
            this.lnkWorkingFolder.Name = "lnkWorkingFolder";
            this.lnkWorkingFolder.Size = new System.Drawing.Size(79, 13);
            this.lnkWorkingFolder.TabIndex = 14;
            this.lnkWorkingFolder.TabStop = true;
            this.lnkWorkingFolder.Text = "Working Folder";
            this.lnkWorkingFolder.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkWorkingFolder_LinkClicked);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label1, 5);
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(5, 5);
            this.label1.Margin = new System.Windows.Forms.Padding(5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(809, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select sequences to add to a dataset.  To view additional details for an alignment" +
    ", double-click on a row or right-click and select \"Details\"";
            // 
            // grdResults
            // 
            this.grdResults.AllowUserToAddRows = false;
            this.grdResults.AllowUserToDeleteRows = false;
            this.grdResults.AllowUserToResizeRows = false;
            this.grdResults.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.grdResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdResults.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmInclude,
            this.clmSequenceMatch,
            this.clmInRecordset,
            this.clmOrganism,
            this.clmDefinition,
            this.clmNucleotides});
            this.tableLayoutPanel1.SetColumnSpan(this.grdResults, 5);
            this.grdResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdResults.Location = new System.Drawing.Point(3, 51);
            this.grdResults.Name = "grdResults";
            this.grdResults.RowHeadersVisible = false;
            this.grdResults.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdResults.Size = new System.Drawing.Size(870, 429);
            this.grdResults.TabIndex = 4;
            this.grdResults.SelectionChanged += new System.EventHandler(this.grdResults_SelectionChanged);
            // 
            // clmInclude
            // 
            this.clmInclude.DataPropertyName = "Selected";
            this.clmInclude.FillWeight = 3F;
            this.clmInclude.HeaderText = "";
            this.clmInclude.MinimumWidth = 30;
            this.clmInclude.Name = "clmInclude";
            this.clmInclude.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.clmInclude.Width = 30;
            // 
            // clmSequenceMatch
            // 
            this.clmSequenceMatch.DataPropertyName = "DuplicateSequenceImage";
            this.clmSequenceMatch.HeaderText = "";
            this.clmSequenceMatch.MinimumWidth = 25;
            this.clmSequenceMatch.Name = "clmSequenceMatch";
            this.clmSequenceMatch.ReadOnly = true;
            this.clmSequenceMatch.ToolTipText = "Sequence match";
            this.clmSequenceMatch.Visible = false;
            this.clmSequenceMatch.Width = 25;
            // 
            // clmInRecordset
            // 
            this.clmInRecordset.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.clmInRecordset.DataPropertyName = "InRecordSet";
            this.clmInRecordset.FillWeight = 25F;
            this.clmInRecordset.HeaderText = "In Recordset";
            this.clmInRecordset.MinimumWidth = 60;
            this.clmInRecordset.Name = "clmInRecordset";
            this.clmInRecordset.ReadOnly = true;
            this.clmInRecordset.Width = 60;
            // 
            // clmOrganism
            // 
            this.clmOrganism.DataPropertyName = "Organism";
            this.clmOrganism.FillWeight = 30F;
            this.clmOrganism.HeaderText = "Organism";
            this.clmOrganism.MinimumWidth = 150;
            this.clmOrganism.Name = "clmOrganism";
            this.clmOrganism.ReadOnly = true;
            this.clmOrganism.Width = 150;
            // 
            // clmDefinition
            // 
            this.clmDefinition.DataPropertyName = "Definition";
            this.clmDefinition.FillWeight = 30F;
            this.clmDefinition.HeaderText = "Definition";
            this.clmDefinition.MinimumWidth = 250;
            this.clmDefinition.Name = "clmDefinition";
            this.clmDefinition.Width = 250;
            // 
            // clmNucleotides
            // 
            this.clmNucleotides.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.clmNucleotides.DataPropertyName = "SourceSequenceString";
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clmNucleotides.DefaultCellStyle = dataGridViewCellStyle1;
            this.clmNucleotides.FillWeight = 67F;
            this.clmNucleotides.HeaderText = "Aligned Sequence";
            this.clmNucleotides.MinimumWidth = 385;
            this.clmNucleotides.Name = "clmNucleotides";
            this.clmNucleotides.ReadOnly = true;
            this.clmNucleotides.Width = 385;
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
            this.lblTotalRows.Location = new System.Drawing.Point(590, 33);
            this.lblTotalRows.Margin = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.lblTotalRows.Name = "lblTotalRows";
            this.lblTotalRows.Size = new System.Drawing.Size(147, 13);
            this.lblTotalRows.TabIndex = 11;
            this.lblTotalRows.Text = "Showing {0} of {1} alignments";
            // 
            // pgOutput
            // 
            this.pgOutput.Controls.Add(this.tableLayoutPanel4);
            this.pgOutput.Location = new System.Drawing.Point(4, 22);
            this.pgOutput.Name = "pgOutput";
            this.pgOutput.Padding = new System.Windows.Forms.Padding(3);
            this.pgOutput.Size = new System.Drawing.Size(876, 536);
            this.pgOutput.TabIndex = 3;
            this.pgOutput.Text = "PRANK Output";
            this.pgOutput.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.Controls.Add(this.btnClose_Output, 1, 1);
            this.tableLayoutPanel4.Controls.Add(this.txtOutput, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.btnSaveOutput, 0, 1);
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
            // btnClose_Output
            // 
            this.btnClose_Output.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnClose_Output.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose_Output.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose_Output.ImageKey = "Cancel";
            this.btnClose_Output.Location = new System.Drawing.Point(787, 503);
            this.btnClose_Output.Name = "btnClose_Output";
            this.btnClose_Output.Size = new System.Drawing.Size(80, 24);
            this.btnClose_Output.TabIndex = 2;
            this.btnClose_Output.Text = "&Close";
            this.btnClose_Output.UseVisualStyleBackColor = true;
            // 
            // txtOutput
            // 
            this.tableLayoutPanel4.SetColumnSpan(this.txtOutput, 2);
            this.txtOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtOutput.Location = new System.Drawing.Point(3, 3);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtOutput.Size = new System.Drawing.Size(864, 494);
            this.txtOutput.TabIndex = 3;
            // 
            // btnSaveOutput
            // 
            this.btnSaveOutput.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnSaveOutput.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnSaveOutput.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSaveOutput.ImageKey = "Cancel";
            this.btnSaveOutput.Location = new System.Drawing.Point(3, 503);
            this.btnSaveOutput.Name = "btnSaveOutput";
            this.btnSaveOutput.Size = new System.Drawing.Size(80, 24);
            this.btnSaveOutput.TabIndex = 2;
            this.btnSaveOutput.Text = "&Save";
            this.btnSaveOutput.UseVisualStyleBackColor = true;
            this.btnSaveOutput.Click += new System.EventHandler(this.btnSaveOutput_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.ImageKey = "Cancel";
            this.btnClose.Location = new System.Drawing.Point(12, 12);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 24);
            this.btnClose.TabIndex = 9;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // frmAlignmentResults
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(884, 562);
            this.Controls.Add(this.tbForm);
            this.Controls.Add(this.btnClose);
            this.MinimumSize = new System.Drawing.Size(650, 450);
            this.Name = "frmAlignmentResults";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "PRANK Alignment Results";
            this.Load += new System.EventHandler(this.frmAlignmentResults_Load);
            this.tbForm.ResumeLayout(false);
            this.pgResults.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdResults)).EndInit();
            this.pgOutput.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tbForm;
        private System.Windows.Forms.TabPage pgResults;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView grdResults;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose_Results;
        private System.Windows.Forms.ComboBox cmbSubSets;
        private System.Windows.Forms.LinkLabel lnkClearFilter;
        private System.Windows.Forms.CheckBox chkToggleSelected;
        private System.Windows.Forms.LinkLabel lnkFilter;
        private System.Windows.Forms.Label lblSelectedRows;
        private System.Windows.Forms.TabPage pgOutput;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Button btnClose_Output;
        private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.Button btnSaveOutput;
        private System.Windows.Forms.Label lblTotalRows;
        private System.Windows.Forms.DataGridViewCheckBoxColumn clmInclude;
        private System.Windows.Forms.DataGridViewImageColumn clmSequenceMatch;
        private System.Windows.Forms.DataGridViewCheckBoxColumn clmInRecordset;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmOrganism;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmDefinition;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmNucleotides;
        private System.Windows.Forms.LinkLabel lnkWorkingFolder;
        private System.Windows.Forms.Button btnClose;
    }
}