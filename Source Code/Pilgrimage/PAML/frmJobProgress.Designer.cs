namespace Pilgrimage.PAML
{
    partial class frmJobProgress
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmJobProgress));
            this.grdAnalyses = new System.Windows.Forms.DataGridView();
            this.clmTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmTree = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmSequences = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmModel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmNSsites = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmNCatG = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmKappa = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmOmega = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblStartedAt = new System.Windows.Forms.Label();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnClose_Progress = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tbForm = new System.Windows.Forms.TabControl();
            this.pgProgress = new System.Windows.Forms.TabPage();
            this.pgConfiguration = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnClose_Configuration = new System.Windows.Forms.Button();
            this.uctJobConfigurations1 = new Pilgrimage.PAML.uctJobConfigurations();
            this.btnReRunJob = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.grdAnalyses)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.tbForm.SuspendLayout();
            this.pgProgress.SuspendLayout();
            this.pgConfiguration.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // grdAnalyses
            // 
            this.grdAnalyses.AllowUserToAddRows = false;
            this.grdAnalyses.AllowUserToDeleteRows = false;
            this.grdAnalyses.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grdAnalyses.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdAnalyses.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmTitle,
            this.clmTree,
            this.clmSequences,
            this.clmStatus,
            this.clmModel,
            this.clmNSsites,
            this.clmNCatG,
            this.clmKappa,
            this.clmOmega});
            this.tableLayoutPanel1.SetColumnSpan(this.grdAnalyses, 2);
            this.grdAnalyses.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdAnalyses.Location = new System.Drawing.Point(6, 51);
            this.grdAnalyses.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.grdAnalyses.Name = "grdAnalyses";
            this.grdAnalyses.ReadOnly = true;
            this.grdAnalyses.RowHeadersVisible = false;
            this.grdAnalyses.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdAnalyses.Size = new System.Drawing.Size(1728, 716);
            this.grdAnalyses.TabIndex = 1;
            // 
            // clmTitle
            // 
            this.clmTitle.DataPropertyName = "Title";
            this.clmTitle.FillWeight = 15F;
            this.clmTitle.HeaderText = "Title";
            this.clmTitle.Name = "clmTitle";
            this.clmTitle.ReadOnly = true;
            // 
            // clmTree
            // 
            this.clmTree.DataPropertyName = "TreeFileName";
            this.clmTree.FillWeight = 15F;
            this.clmTree.HeaderText = "Tree";
            this.clmTree.Name = "clmTree";
            this.clmTree.ReadOnly = true;
            // 
            // clmSequences
            // 
            this.clmSequences.DataPropertyName = "SequencesFileName";
            this.clmSequences.FillWeight = 15F;
            this.clmSequences.HeaderText = "Sequences";
            this.clmSequences.Name = "clmSequences";
            this.clmSequences.ReadOnly = true;
            // 
            // clmStatus
            // 
            this.clmStatus.DataPropertyName = "Status";
            this.clmStatus.FillWeight = 10F;
            this.clmStatus.HeaderText = "Status";
            this.clmStatus.Name = "clmStatus";
            this.clmStatus.ReadOnly = true;
            // 
            // clmModel
            // 
            this.clmModel.DataPropertyName = "Model";
            this.clmModel.FillWeight = 13F;
            this.clmModel.HeaderText = "Model";
            this.clmModel.Name = "clmModel";
            this.clmModel.ReadOnly = true;
            // 
            // clmNSsites
            // 
            this.clmNSsites.DataPropertyName = "NSSites";
            this.clmNSsites.FillWeight = 10F;
            this.clmNSsites.HeaderText = "Site Models";
            this.clmNSsites.Name = "clmNSsites";
            this.clmNSsites.ReadOnly = true;
            // 
            // clmNCatG
            // 
            this.clmNCatG.DataPropertyName = "NCatG";
            this.clmNCatG.FillWeight = 8F;
            this.clmNCatG.HeaderText = "Categories";
            this.clmNCatG.Name = "clmNCatG";
            this.clmNCatG.ReadOnly = true;
            // 
            // clmKappa
            // 
            this.clmKappa.DataPropertyName = "Kappa";
            this.clmKappa.FillWeight = 7F;
            this.clmKappa.HeaderText = "Kappa";
            this.clmKappa.Name = "clmKappa";
            this.clmKappa.ReadOnly = true;
            // 
            // clmOmega
            // 
            this.clmOmega.DataPropertyName = "Omega";
            this.clmOmega.FillWeight = 7F;
            this.clmOmega.HeaderText = "Omega";
            this.clmOmega.Name = "clmOmega";
            this.clmOmega.ReadOnly = true;
            // 
            // lblStartedAt
            // 
            this.lblStartedAt.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblStartedAt.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblStartedAt, 2);
            this.lblStartedAt.Location = new System.Drawing.Point(10, 10);
            this.lblStartedAt.Margin = new System.Windows.Forms.Padding(10, 10, 10, 10);
            this.lblStartedAt.Name = "lblStartedAt";
            this.lblStartedAt.Size = new System.Drawing.Size(271, 25);
            this.lblStartedAt.TabIndex = 0;
            this.lblStartedAt.Text = "Started at: {0} ({1} elapsed)";
            // 
            // btnNext
            // 
            this.btnNext.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnNext.Location = new System.Drawing.Point(6, 779);
            this.btnNext.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(220, 44);
            this.btnNext.TabIndex = 2;
            this.btnNext.Text = "Cancel &Job";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnClose_Progress
            // 
            this.btnClose_Progress.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnClose_Progress.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose_Progress.Location = new System.Drawing.Point(1584, 779);
            this.btnClose_Progress.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnClose_Progress.Name = "btnClose_Progress";
            this.btnClose_Progress.Size = new System.Drawing.Size(150, 44);
            this.btnClose_Progress.TabIndex = 3;
            this.btnClose_Progress.Text = "&Close";
            this.btnClose_Progress.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.btnNext, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblStartedAt, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnClose_Progress, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.grdAnalyses, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(6, 6);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1740, 829);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tbForm
            // 
            this.tbForm.Controls.Add(this.pgProgress);
            this.tbForm.Controls.Add(this.pgConfiguration);
            this.tbForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbForm.Location = new System.Drawing.Point(0, 0);
            this.tbForm.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.tbForm.Name = "tbForm";
            this.tbForm.SelectedIndex = 0;
            this.tbForm.Size = new System.Drawing.Size(1768, 888);
            this.tbForm.TabIndex = 0;
            this.tbForm.Selected += new System.Windows.Forms.TabControlEventHandler(this.tbForm_Selected);
            // 
            // pgProgress
            // 
            this.pgProgress.Controls.Add(this.tableLayoutPanel1);
            this.pgProgress.Location = new System.Drawing.Point(8, 39);
            this.pgProgress.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.pgProgress.Name = "pgProgress";
            this.pgProgress.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.pgProgress.Size = new System.Drawing.Size(1752, 841);
            this.pgProgress.TabIndex = 0;
            this.pgProgress.Text = "Progress";
            this.pgProgress.UseVisualStyleBackColor = true;
            // 
            // pgConfiguration
            // 
            this.pgConfiguration.Controls.Add(this.tableLayoutPanel2);
            this.pgConfiguration.Location = new System.Drawing.Point(8, 39);
            this.pgConfiguration.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.pgConfiguration.Name = "pgConfiguration";
            this.pgConfiguration.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.pgConfiguration.Size = new System.Drawing.Size(1752, 841);
            this.pgConfiguration.TabIndex = 1;
            this.pgConfiguration.Text = "Job Configuration";
            this.pgConfiguration.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.btnClose_Configuration, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.uctJobConfigurations1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnReRunJob, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(6, 6);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1740, 829);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // btnClose_Configuration
            // 
            this.btnClose_Configuration.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnClose_Configuration.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose_Configuration.Location = new System.Drawing.Point(1584, 779);
            this.btnClose_Configuration.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnClose_Configuration.Name = "btnClose_Configuration";
            this.btnClose_Configuration.Size = new System.Drawing.Size(150, 44);
            this.btnClose_Configuration.TabIndex = 1;
            this.btnClose_Configuration.Text = "&Close";
            this.btnClose_Configuration.UseVisualStyleBackColor = true;
            // 
            // uctJobConfigurations1
            // 
            this.tableLayoutPanel2.SetColumnSpan(this.uctJobConfigurations1, 2);
            this.uctJobConfigurations1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uctJobConfigurations1.Location = new System.Drawing.Point(0, 0);
            this.uctJobConfigurations1.Margin = new System.Windows.Forms.Padding(0);
            this.uctJobConfigurations1.Name = "uctJobConfigurations1";
            this.uctJobConfigurations1.Size = new System.Drawing.Size(1740, 773);
            this.uctJobConfigurations1.TabIndex = 2;
            // 
            // btnReRunJob
            // 
            this.btnReRunJob.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnReRunJob.Location = new System.Drawing.Point(1342, 779);
            this.btnReRunJob.Margin = new System.Windows.Forms.Padding(6);
            this.btnReRunJob.Name = "btnReRunJob";
            this.btnReRunJob.Size = new System.Drawing.Size(230, 44);
            this.btnReRunJob.TabIndex = 1;
            this.btnReRunJob.Text = "&Run Job Again";
            this.btnReRunJob.UseVisualStyleBackColor = true;
            this.btnReRunJob.Click += new System.EventHandler(this.btnReRunJob_Click);
            // 
            // frmJobProgress
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1768, 888);
            this.Controls.Add(this.tbForm);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(12, 12, 12, 12);
            this.Name = "frmJobProgress";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Progress";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmJobProgress_FormClosing);
            this.Load += new System.EventHandler(this.frmJobProgress_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdAnalyses)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tbForm.ResumeLayout(false);
            this.pgProgress.ResumeLayout(false);
            this.pgConfiguration.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView grdAnalyses;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Label lblStartedAt;
        private System.Windows.Forms.Button btnClose_Progress;
        private System.Windows.Forms.TabControl tbForm;
        private System.Windows.Forms.TabPage pgProgress;
        private System.Windows.Forms.TabPage pgConfiguration;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button btnClose_Configuration;
        private uctJobConfigurations uctJobConfigurations1;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmTitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmTree;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmSequences;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmModel;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmNSsites;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmNCatG;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmKappa;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmOmega;
        private System.Windows.Forms.Button btnReRunJob;
    }
}