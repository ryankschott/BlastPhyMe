namespace Pilgrimage.GeneSequences
{
    partial class uctRecordSetGenes
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tblForm = new System.Windows.Forms.TableLayoutPanel();
            this.grdResults = new System.Windows.Forms.DataGridView();
            this.chkToggleSelected = new System.Windows.Forms.CheckBox();
            this.lblTotalRows = new System.Windows.Forms.Label();
            this.lblFilterRows = new System.Windows.Forms.Label();
            this.lnkClearFilter = new System.Windows.Forms.LinkLabel();
            this.lnkFilter = new System.Windows.Forms.LinkLabel();
            this.bwRefreshGenes = new System.ComponentModel.BackgroundWorker();
            this.pbRefreshing = new System.Windows.Forms.ProgressBar();
            this.NeverEndingTimer = new System.Windows.Forms.Timer(this.components);
            this.clmSelected_Committed = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.clmName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmOrganism = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmDefinition = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmLength = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmGenBankUrl = new System.Windows.Forms.DataGridViewLinkColumn();
            this.clmSource = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmHasBlastNResults = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.clmModifiedAt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmLastUpdatedAt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmSequenceMatch = new System.Windows.Forms.DataGridViewImageColumn();
            this.tblForm.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdResults)).BeginInit();
            this.SuspendLayout();
            // 
            // tblForm
            // 
            this.tblForm.ColumnCount = 5;
            this.tblForm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tblForm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tblForm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblForm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tblForm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tblForm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tblForm.Controls.Add(this.grdResults, 0, 1);
            this.tblForm.Controls.Add(this.chkToggleSelected, 0, 0);
            this.tblForm.Controls.Add(this.lblTotalRows, 1, 0);
            this.tblForm.Controls.Add(this.lblFilterRows, 2, 0);
            this.tblForm.Controls.Add(this.lnkClearFilter, 4, 0);
            this.tblForm.Controls.Add(this.lnkFilter, 3, 0);
            this.tblForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblForm.Location = new System.Drawing.Point(0, 0);
            this.tblForm.Margin = new System.Windows.Forms.Padding(0);
            this.tblForm.Name = "tblForm";
            this.tblForm.RowCount = 2;
            this.tblForm.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblForm.Size = new System.Drawing.Size(995, 456);
            this.tblForm.TabIndex = 0;
            // 
            // grdResults
            // 
            this.grdResults.AllowUserToAddRows = false;
            this.grdResults.AllowUserToDeleteRows = false;
            this.grdResults.AllowUserToResizeRows = false;
            this.grdResults.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grdResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdResults.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmSelected_Committed,
            this.clmName,
            this.clmOrganism,
            this.clmDefinition,
            this.clmLength,
            this.clmGenBankUrl,
            this.clmSource,
            this.clmHasBlastNResults,
            this.clmModifiedAt,
            this.clmLastUpdatedAt,
            this.clmSequenceMatch});
            this.tblForm.SetColumnSpan(this.grdResults, 5);
            this.grdResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdResults.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.grdResults.Location = new System.Drawing.Point(3, 25);
            this.grdResults.Name = "grdResults";
            this.grdResults.ReadOnly = true;
            this.grdResults.RowHeadersVisible = false;
            this.grdResults.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdResults.Size = new System.Drawing.Size(989, 428);
            this.grdResults.TabIndex = 3;
            this.grdResults.SelectionChanged += new System.EventHandler(this.grdResults_SelectionChanged);
            // 
            // chkToggleSelected
            // 
            this.chkToggleSelected.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkToggleSelected.AutoSize = true;
            this.chkToggleSelected.Location = new System.Drawing.Point(5, 5);
            this.chkToggleSelected.Margin = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.chkToggleSelected.Name = "chkToggleSelected";
            this.chkToggleSelected.Size = new System.Drawing.Size(70, 17);
            this.chkToggleSelected.TabIndex = 0;
            this.chkToggleSelected.Text = "Select All";
            this.chkToggleSelected.UseVisualStyleBackColor = true;
            this.chkToggleSelected.CheckedChanged += new System.EventHandler(this.chkToggleSelected_CheckedChanged);
            // 
            // lblTotalRows
            // 
            this.lblTotalRows.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblTotalRows.AutoSize = true;
            this.lblTotalRows.Location = new System.Drawing.Point(85, 7);
            this.lblTotalRows.Margin = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.lblTotalRows.Name = "lblTotalRows";
            this.lblTotalRows.Size = new System.Drawing.Size(81, 13);
            this.lblTotalRows.TabIndex = 1;
            this.lblTotalRows.Text = "# records (total)";
            // 
            // lblFilterRows
            // 
            this.lblFilterRows.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblFilterRows.AutoSize = true;
            this.lblFilterRows.Location = new System.Drawing.Point(737, 7);
            this.lblFilterRows.Margin = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.lblFilterRows.Name = "lblFilterRows";
            this.lblFilterRows.Size = new System.Drawing.Size(119, 13);
            this.lblFilterRows.TabIndex = 1;
            this.lblFilterRows.Text = "Showing {0} of {1} rows";
            // 
            // lnkClearFilter
            // 
            this.lnkClearFilter.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lnkClearFilter.AutoSize = true;
            this.lnkClearFilter.Location = new System.Drawing.Point(934, 7);
            this.lnkClearFilter.Margin = new System.Windows.Forms.Padding(10, 5, 5, 0);
            this.lnkClearFilter.Name = "lnkClearFilter";
            this.lnkClearFilter.Size = new System.Drawing.Size(56, 13);
            this.lnkClearFilter.TabIndex = 2;
            this.lnkClearFilter.TabStop = true;
            this.lnkClearFilter.Text = "Clear Filter";
            this.lnkClearFilter.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkFilter_LinkClicked);
            // 
            // lnkFilter
            // 
            this.lnkFilter.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lnkFilter.AutoSize = true;
            this.lnkFilter.Location = new System.Drawing.Point(866, 7);
            this.lnkFilter.Margin = new System.Windows.Forms.Padding(5, 5, 0, 0);
            this.lnkFilter.Name = "lnkFilter";
            this.lnkFilter.Size = new System.Drawing.Size(58, 13);
            this.lnkFilter.TabIndex = 2;
            this.lnkFilter.TabStop = true;
            this.lnkFilter.Text = "Apply Filter";
            this.lnkFilter.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkFilter_LinkClicked);
            // 
            // bwRefreshGenes
            // 
            this.bwRefreshGenes.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwRefreshGenes_DoWork);
            this.bwRefreshGenes.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwRefreshGenes_RunWorkerCompleted);
            // 
            // pbRefreshing
            // 
            this.pbRefreshing.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pbRefreshing.Location = new System.Drawing.Point(397, 216);
            this.pbRefreshing.Maximum = 1000;
            this.pbRefreshing.Name = "pbRefreshing";
            this.pbRefreshing.Size = new System.Drawing.Size(200, 23);
            this.pbRefreshing.TabIndex = 1;
            // 
            // NeverEndingTimer
            // 
            this.NeverEndingTimer.Tick += new System.EventHandler(this.NeverEndingTimer_Tick);
            // 
            // clmSelected_Committed
            // 
            this.clmSelected_Committed.DataPropertyName = "Selected";
            this.clmSelected_Committed.FillWeight = 3F;
            this.clmSelected_Committed.HeaderText = "";
            this.clmSelected_Committed.Name = "clmSelected_Committed";
            this.clmSelected_Committed.ReadOnly = true;
            // 
            // clmName
            // 
            this.clmName.DataPropertyName = "GeneName";
            this.clmName.FillWeight = 8F;
            this.clmName.HeaderText = "Gene";
            this.clmName.Name = "clmName";
            this.clmName.ReadOnly = true;
            // 
            // clmOrganism
            // 
            this.clmOrganism.DataPropertyName = "Organism";
            this.clmOrganism.FillWeight = 13F;
            this.clmOrganism.HeaderText = "Organism";
            this.clmOrganism.Name = "clmOrganism";
            this.clmOrganism.ReadOnly = true;
            // 
            // clmDefinition
            // 
            this.clmDefinition.DataPropertyName = "Definition";
            this.clmDefinition.FillWeight = 33F;
            this.clmDefinition.HeaderText = "Definition";
            this.clmDefinition.Name = "clmDefinition";
            this.clmDefinition.ReadOnly = true;
            // 
            // clmLength
            // 
            this.clmLength.DataPropertyName = "Length";
            dataGridViewCellStyle1.Format = "N0";
            this.clmLength.DefaultCellStyle = dataGridViewCellStyle1;
            this.clmLength.FillWeight = 5F;
            this.clmLength.HeaderText = "Length";
            this.clmLength.Name = "clmLength";
            this.clmLength.ReadOnly = true;
            // 
            // clmGenBankUrl
            // 
            this.clmGenBankUrl.DataPropertyName = "GenBankUrl";
            this.clmGenBankUrl.FillWeight = 7F;
            this.clmGenBankUrl.HeaderText = "GenBank";
            this.clmGenBankUrl.Name = "clmGenBankUrl";
            this.clmGenBankUrl.ReadOnly = true;
            this.clmGenBankUrl.Text = "Open GenBank Page";
            // 
            // clmSource
            // 
            this.clmSource.DataPropertyName = "SourceName";
            this.clmSource.FillWeight = 7F;
            this.clmSource.HeaderText = "Source";
            this.clmSource.Name = "clmSource";
            this.clmSource.ReadOnly = true;
            // 
            // clmHasBlastNResults
            // 
            this.clmHasBlastNResults.DataPropertyName = "ProcessedThroughBLASTNAtNCBI";
            this.clmHasBlastNResults.FillWeight = 5F;
            this.clmHasBlastNResults.HeaderText = "Processed by BLASTN";
            this.clmHasBlastNResults.Name = "clmHasBlastNResults";
            this.clmHasBlastNResults.ReadOnly = true;
            // 
            // clmModifiedAt
            // 
            this.clmModifiedAt.DataPropertyName = "ModifiedAt";
            dataGridViewCellStyle2.Format = "G";
            this.clmModifiedAt.DefaultCellStyle = dataGridViewCellStyle2;
            this.clmModifiedAt.FillWeight = 8F;
            this.clmModifiedAt.HeaderText = "Added At";
            this.clmModifiedAt.Name = "clmModifiedAt";
            this.clmModifiedAt.ReadOnly = true;
            // 
            // clmLastUpdatedAt
            // 
            this.clmLastUpdatedAt.DataPropertyName = "LastUpdatedAt";
            dataGridViewCellStyle3.Format = "G";
            this.clmLastUpdatedAt.DefaultCellStyle = dataGridViewCellStyle3;
            this.clmLastUpdatedAt.FillWeight = 8F;
            this.clmLastUpdatedAt.HeaderText = "Updated At";
            this.clmLastUpdatedAt.Name = "clmLastUpdatedAt";
            this.clmLastUpdatedAt.ReadOnly = true;
            // 
            // clmSequenceMatch
            // 
            this.clmSequenceMatch.DataPropertyName = "DuplicateSequenceImage";
            this.clmSequenceMatch.FillWeight = 3F;
            this.clmSequenceMatch.HeaderText = "";
            this.clmSequenceMatch.Name = "clmSequenceMatch";
            this.clmSequenceMatch.ReadOnly = true;
            this.clmSequenceMatch.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.clmSequenceMatch.ToolTipText = "Sequence match";
            this.clmSequenceMatch.Visible = false;
            // 
            // uctRecordSetGenes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tblForm);
            this.Controls.Add(this.pbRefreshing);
            this.Name = "uctRecordSetGenes";
            this.Size = new System.Drawing.Size(995, 456);
            this.tblForm.ResumeLayout(false);
            this.tblForm.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdResults)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tblForm;
        private System.Windows.Forms.DataGridView grdResults;
        private System.Windows.Forms.CheckBox chkToggleSelected;
        private System.Windows.Forms.LinkLabel lnkFilter;
        private System.Windows.Forms.Label lblTotalRows;
        private System.Windows.Forms.Label lblFilterRows;
        private System.ComponentModel.BackgroundWorker bwRefreshGenes;
        private System.Windows.Forms.ProgressBar pbRefreshing;
        private System.Windows.Forms.Timer NeverEndingTimer;
        private System.Windows.Forms.LinkLabel lnkClearFilter;
        private System.Windows.Forms.DataGridViewCheckBoxColumn clmSelected_Committed;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmName;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmOrganism;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmDefinition;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmLength;
        private System.Windows.Forms.DataGridViewLinkColumn clmGenBankUrl;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmSource;
        private System.Windows.Forms.DataGridViewCheckBoxColumn clmHasBlastNResults;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmModifiedAt;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmLastUpdatedAt;
        private System.Windows.Forms.DataGridViewImageColumn clmSequenceMatch;
    }
}
