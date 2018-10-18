namespace Pilgrimage.GeneSequences.BlastN
{
    partial class frmBlastNAnnotations
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBlastNAnnotations));
            this.tbForm = new System.Windows.Forms.TabControl();
            this.pgResults = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.grdResults = new System.Windows.Forms.DataGridView();
            this.btnSave = new System.Windows.Forms.Button();
            this.chkToggleSelected = new System.Windows.Forms.CheckBox();
            this.lblSelectedRows = new System.Windows.Forms.Label();
            this.btnClose_Results = new System.Windows.Forms.Button();
            this.pgHistory = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.grdHistory = new System.Windows.Forms.DataGridView();
            this.clmRequestID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmSubmittedGenes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmStartedAt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmLastStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmEndedAt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnClose_History = new System.Windows.Forms.Button();
            this.pgProgress = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.btnClose_Progress = new System.Windows.Forms.Button();
            this.txtProgress = new System.Windows.Forms.TextBox();
            this.btnSaveProgress = new System.Windows.Forms.Button();
            this.bwAlignments = new System.ComponentModel.BackgroundWorker();
            this.clmInclude = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.clmOriginalSource = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmDefinition = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmNewDefinition = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmPercentageMatch = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmGenBankUrl = new System.Windows.Forms.DataGridViewLinkColumn();
            this.tbForm.SuspendLayout();
            this.pgResults.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdResults)).BeginInit();
            this.pgHistory.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdHistory)).BeginInit();
            this.pgProgress.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbForm
            // 
            this.tbForm.Controls.Add(this.pgResults);
            this.tbForm.Controls.Add(this.pgHistory);
            this.tbForm.Controls.Add(this.pgProgress);
            this.tbForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbForm.Location = new System.Drawing.Point(0, 0);
            this.tbForm.Name = "tbForm";
            this.tbForm.SelectedIndex = 0;
            this.tbForm.Size = new System.Drawing.Size(884, 562);
            this.tbForm.TabIndex = 1;
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
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.grdResults, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnSave, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.chkToggleSelected, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblSelectedRows, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnClose_Results, 3, 4);
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label1, 4);
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(5, 5);
            this.label1.Margin = new System.Windows.Forms.Padding(5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(770, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "To view additional details, and select a different alignment to annotate with, do" +
    "uble-click on a row or right-click and select \"Details\"";
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
            this.clmOriginalSource,
            this.clmDefinition,
            this.clmNewDefinition,
            this.clmPercentageMatch,
            this.clmGenBankUrl});
            this.tableLayoutPanel1.SetColumnSpan(this.grdResults, 4);
            this.grdResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdResults.Location = new System.Drawing.Point(3, 51);
            this.grdResults.Name = "grdResults";
            this.grdResults.ReadOnly = true;
            this.grdResults.RowHeadersVisible = false;
            this.grdResults.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdResults.Size = new System.Drawing.Size(870, 429);
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
            this.btnSave.Text = "&Update";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // chkToggleSelected
            // 
            this.chkToggleSelected.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkToggleSelected.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.chkToggleSelected, 2);
            this.chkToggleSelected.Location = new System.Drawing.Point(5, 31);
            this.chkToggleSelected.Margin = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.chkToggleSelected.Name = "chkToggleSelected";
            this.chkToggleSelected.Size = new System.Drawing.Size(70, 17);
            this.chkToggleSelected.TabIndex = 10;
            this.chkToggleSelected.Text = "Select All";
            this.chkToggleSelected.UseVisualStyleBackColor = true;
            // 
            // lblSelectedRows
            // 
            this.lblSelectedRows.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblSelectedRows.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblSelectedRows, 2);
            this.lblSelectedRows.Location = new System.Drawing.Point(5, 488);
            this.lblSelectedRows.Margin = new System.Windows.Forms.Padding(5);
            this.lblSelectedRows.Name = "lblSelectedRows";
            this.lblSelectedRows.Size = new System.Drawing.Size(95, 13);
            this.lblSelectedRows.TabIndex = 5;
            this.lblSelectedRows.Text = "# records selected";
            // 
            // btnClose_Results
            // 
            this.btnClose_Results.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
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
            dataGridViewCellStyle1.Format = "N0";
            this.clmSubmittedGenes.DefaultCellStyle = dataGridViewCellStyle1;
            this.clmSubmittedGenes.FillWeight = 14F;
            this.clmSubmittedGenes.HeaderText = "Gene Sequences";
            this.clmSubmittedGenes.Name = "clmSubmittedGenes";
            this.clmSubmittedGenes.ReadOnly = true;
            // 
            // clmStartedAt
            // 
            this.clmStartedAt.DataPropertyName = "StartedAt";
            dataGridViewCellStyle2.Format = "G";
            this.clmStartedAt.DefaultCellStyle = dataGridViewCellStyle2;
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
            dataGridViewCellStyle3.Format = "G";
            dataGridViewCellStyle3.NullValue = " ";
            this.clmEndedAt.DefaultCellStyle = dataGridViewCellStyle3;
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
            this.clmInclude.ReadOnly = true;
            this.clmInclude.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // clmOriginalSource
            // 
            this.clmOriginalSource.DataPropertyName = "Accession";
            this.clmOriginalSource.FillWeight = 16F;
            this.clmOriginalSource.HeaderText = "Original Source";
            this.clmOriginalSource.Name = "clmOriginalSource";
            this.clmOriginalSource.ReadOnly = true;
            // 
            // clmDefinition
            // 
            this.clmDefinition.DataPropertyName = "OriginalDefinition";
            this.clmDefinition.FillWeight = 32F;
            this.clmDefinition.HeaderText = "Original Definition";
            this.clmDefinition.Name = "clmDefinition";
            this.clmDefinition.ReadOnly = true;
            // 
            // clmNewDefinition
            // 
            this.clmNewDefinition.DataPropertyName = "NewDefinition";
            this.clmNewDefinition.FillWeight = 32F;
            this.clmNewDefinition.HeaderText = "Closest Match in GenBank";
            this.clmNewDefinition.Name = "clmNewDefinition";
            this.clmNewDefinition.ReadOnly = true;
            // 
            // clmPercentageMatch
            // 
            this.clmPercentageMatch.DataPropertyName = "SequenceIdentityMatchPercentage";
            this.clmPercentageMatch.FillWeight = 5F;
            this.clmPercentageMatch.HeaderText = "% Match";
            this.clmPercentageMatch.Name = "clmPercentageMatch";
            this.clmPercentageMatch.ReadOnly = true;
            // 
            // clmGenBankUrl
            // 
            this.clmGenBankUrl.DataPropertyName = "NewGenBankUrl";
            this.clmGenBankUrl.FillWeight = 12F;
            this.clmGenBankUrl.HeaderText = "GenBank";
            this.clmGenBankUrl.Name = "clmGenBankUrl";
            this.clmGenBankUrl.ReadOnly = true;
            // 
            // frmBlastNAnnotations
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose_Results;
            this.ClientSize = new System.Drawing.Size(884, 562);
            this.Controls.Add(this.tbForm);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmBlastNAnnotations";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BLAST Annotations";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmBlastNAnnotations_FormClosing);
            this.Load += new System.EventHandler(this.frmBlastNAnnotations_Load);
            this.tbForm.ResumeLayout(false);
            this.pgResults.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdResults)).EndInit();
            this.pgHistory.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdHistory)).EndInit();
            this.pgProgress.ResumeLayout(false);
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
        private System.Windows.Forms.CheckBox chkToggleSelected;
        private System.Windows.Forms.Label lblSelectedRows;
        private System.Windows.Forms.TabPage pgHistory;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.DataGridView grdHistory;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmRequestID;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmSubmittedGenes;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmStartedAt;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmLastStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmEndedAt;
        private System.Windows.Forms.Button btnClose_History;
        private System.Windows.Forms.TabPage pgProgress;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Button btnClose_Progress;
        private System.Windows.Forms.TextBox txtProgress;
        private System.Windows.Forms.Button btnSaveProgress;
        private System.ComponentModel.BackgroundWorker bwAlignments;
        private System.Windows.Forms.DataGridViewCheckBoxColumn clmInclude;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmOriginalSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmDefinition;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmNewDefinition;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmPercentageMatch;
        private System.Windows.Forms.DataGridViewLinkColumn clmGenBankUrl;
    }
}