namespace Pilgrimage.PAML
{
    partial class frmJobHistory
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmJobHistory));
            this.grdJobHistory = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnClose = new System.Windows.Forms.Button();
            this.clmTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmStartedAt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmEndedAt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmTreeFileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmSequencesFileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmTreeTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmTreeFileCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmResultsInProject = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.grdJobHistory)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grdJobHistory
            // 
            this.grdJobHistory.AllowUserToAddRows = false;
            this.grdJobHistory.AllowUserToDeleteRows = false;
            this.grdJobHistory.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grdJobHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdJobHistory.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmTitle,
            this.clmStartedAt,
            this.clmStatus,
            this.clmEndedAt,
            this.clmTreeFileName,
            this.clmSequencesFileName,
            this.clmTreeTitle,
            this.clmTreeFileCount,
            this.clmResultsInProject});
            this.grdJobHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdJobHistory.Location = new System.Drawing.Point(3, 3);
            this.grdJobHistory.Name = "grdJobHistory";
            this.grdJobHistory.ReadOnly = true;
            this.grdJobHistory.RowHeadersVisible = false;
            this.grdJobHistory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdJobHistory.Size = new System.Drawing.Size(878, 431);
            this.grdJobHistory.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.grdJobHistory, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnClose, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(884, 466);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(806, 440);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // clmTitle
            // 
            this.clmTitle.DataPropertyName = "Title";
            this.clmTitle.FillWeight = 15F;
            this.clmTitle.HeaderText = "Title";
            this.clmTitle.Name = "clmTitle";
            this.clmTitle.ReadOnly = true;
            // 
            // clmStartedAt
            // 
            this.clmStartedAt.DataPropertyName = "StartedAt";
            this.clmStartedAt.FillWeight = 12F;
            this.clmStartedAt.HeaderText = "Started At";
            this.clmStartedAt.Name = "clmStartedAt";
            this.clmStartedAt.ReadOnly = true;
            // 
            // clmStatus
            // 
            this.clmStatus.DataPropertyName = "Status";
            this.clmStatus.FillWeight = 8F;
            this.clmStatus.HeaderText = "Status";
            this.clmStatus.Name = "clmStatus";
            this.clmStatus.ReadOnly = true;
            // 
            // clmEndedAt
            // 
            this.clmEndedAt.DataPropertyName = "EndedAt";
            this.clmEndedAt.FillWeight = 12F;
            this.clmEndedAt.HeaderText = "Ended At";
            this.clmEndedAt.Name = "clmEndedAt";
            this.clmEndedAt.ReadOnly = true;
            // 
            // clmTreeFileName
            // 
            this.clmTreeFileName.DataPropertyName = "TreeFileName";
            this.clmTreeFileName.FillWeight = 14F;
            this.clmTreeFileName.HeaderText = "Tree File";
            this.clmTreeFileName.Name = "clmTreeFileName";
            this.clmTreeFileName.ReadOnly = true;
            // 
            // clmSequencesFileName
            // 
            this.clmSequencesFileName.DataPropertyName = "SequencesFileName";
            this.clmSequencesFileName.FillWeight = 14F;
            this.clmSequencesFileName.HeaderText = "Sequences File";
            this.clmSequencesFileName.Name = "clmSequencesFileName";
            this.clmSequencesFileName.ReadOnly = true;
            // 
            // clmTreeTitle
            // 
            this.clmTreeTitle.DataPropertyName = "TreeTitle";
            this.clmTreeTitle.FillWeight = 14F;
            this.clmTreeTitle.HeaderText = "Tree Title";
            this.clmTreeTitle.Name = "clmTreeTitle";
            this.clmTreeTitle.ReadOnly = true;
            // 
            // clmTreeFileCount
            // 
            this.clmTreeFileCount.DataPropertyName = "TreeFileCount";
            this.clmTreeFileCount.FillWeight = 6F;
            this.clmTreeFileCount.HeaderText = "Tree Count";
            this.clmTreeFileCount.Name = "clmTreeFileCount";
            this.clmTreeFileCount.ReadOnly = true;
            // 
            // clmResultsInProject
            // 
            this.clmResultsInProject.DataPropertyName = "InRecordSet";
            this.clmResultsInProject.FillWeight = 5F;
            this.clmResultsInProject.HeaderText = "In Project";
            this.clmResultsInProject.Name = "clmResultsInProject";
            this.clmResultsInProject.ReadOnly = true;
            // 
            // frmJobHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(884, 466);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmJobHistory";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Job History";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmJobHistory_FormClosing);
            this.Load += new System.EventHandler(this.frmJobHistory_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdJobHistory)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView grdJobHistory;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmTitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmStartedAt;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmEndedAt;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmTreeFileName;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmSequencesFileName;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmTreeTitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmTreeFileCount;
        private System.Windows.Forms.DataGridViewCheckBoxColumn clmResultsInProject;
    }
}