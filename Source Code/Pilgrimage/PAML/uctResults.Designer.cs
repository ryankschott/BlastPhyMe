namespace Pilgrimage.PAML
{
    partial class uctResults
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
            this.tblForm = new System.Windows.Forms.TableLayoutPanel();
            this.grdResults = new System.Windows.Forms.DataGridView();
            this.clmSelected = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.clmJobTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmTree = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmModel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmKStart = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmlnL = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmK = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmCompletedAt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmSequencesFileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chkToggleSelected = new System.Windows.Forms.CheckBox();
            this.lblTotalRows = new System.Windows.Forms.Label();
            this.lblFilterRows = new System.Windows.Forms.Label();
            this.lnkClearFilter = new System.Windows.Forms.LinkLabel();
            this.lnkFilter = new System.Windows.Forms.LinkLabel();
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
            this.tblForm.Size = new System.Drawing.Size(1046, 461);
            this.tblForm.TabIndex = 1;
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
            this.clmJobTitle,
            this.clmTree,
            this.clmModel,
            this.clmKStart,
            this.clmlnL,
            this.clmK,
            this.clmCompletedAt,
            this.clmSequencesFileName});
            this.tblForm.SetColumnSpan(this.grdResults, 5);
            this.grdResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdResults.Location = new System.Drawing.Point(3, 25);
            this.grdResults.Name = "grdResults";
            this.grdResults.ReadOnly = true;
            this.grdResults.RowHeadersVisible = false;
            this.grdResults.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdResults.Size = new System.Drawing.Size(1040, 433);
            this.grdResults.TabIndex = 4;
            // 
            // clmSelected
            // 
            this.clmSelected.DataPropertyName = "Selected";
            this.clmSelected.FillWeight = 3F;
            this.clmSelected.HeaderText = "";
            this.clmSelected.Name = "clmSelected";
            this.clmSelected.ReadOnly = true;
            // 
            // clmJobTitle
            // 
            this.clmJobTitle.DataPropertyName = "JobTitle";
            this.clmJobTitle.FillWeight = 13F;
            this.clmJobTitle.HeaderText = "Job Title";
            this.clmJobTitle.Name = "clmJobTitle";
            this.clmJobTitle.ReadOnly = true;
            // 
            // clmTree
            // 
            this.clmTree.DataPropertyName = "TreeDescription";
            this.clmTree.FillWeight = 20F;
            this.clmTree.HeaderText = "Tree";
            this.clmTree.Name = "clmTree";
            this.clmTree.ReadOnly = true;
            // 
            // clmModel
            // 
            this.clmModel.DataPropertyName = "ModelDescription";
            this.clmModel.FillWeight = 19F;
            this.clmModel.HeaderText = "Model";
            this.clmModel.Name = "clmModel";
            this.clmModel.ReadOnly = true;
            // 
            // clmKStart
            // 
            this.clmKStart.DataPropertyName = "KappaOmegaStart";
            this.clmKStart.FillWeight = 5F;
            this.clmKStart.HeaderText = "k/w Start";
            this.clmKStart.Name = "clmKStart";
            this.clmKStart.ReadOnly = true;
            // 
            // clmlnL
            // 
            this.clmlnL.DataPropertyName = "lnL";
            this.clmlnL.FillWeight = 5F;
            this.clmlnL.HeaderText = "lnL";
            this.clmlnL.Name = "clmlnL";
            this.clmlnL.ReadOnly = true;
            // 
            // clmK
            // 
            this.clmK.DataPropertyName = "k";
            this.clmK.FillWeight = 5F;
            this.clmK.HeaderText = "k";
            this.clmK.Name = "clmK";
            this.clmK.ReadOnly = true;
            // 
            // clmCompletedAt
            // 
            this.clmCompletedAt.DataPropertyName = "StartedAt";
            this.clmCompletedAt.FillWeight = 20F;
            this.clmCompletedAt.HeaderText = "Run At";
            this.clmCompletedAt.Name = "clmCompletedAt";
            this.clmCompletedAt.ReadOnly = true;
            // 
            // clmSequencesFileName
            // 
            this.clmSequencesFileName.DataPropertyName = "SequencesFileName";
            this.clmSequencesFileName.FillWeight = 14F;
            this.clmSequencesFileName.HeaderText = "Sequences File";
            this.clmSequencesFileName.Name = "clmSequencesFileName";
            this.clmSequencesFileName.ReadOnly = true;
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
            this.lblFilterRows.Location = new System.Drawing.Point(788, 7);
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
            this.lnkClearFilter.Location = new System.Drawing.Point(985, 7);
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
            this.lnkFilter.Location = new System.Drawing.Point(917, 7);
            this.lnkFilter.Margin = new System.Windows.Forms.Padding(5, 5, 0, 0);
            this.lnkFilter.Name = "lnkFilter";
            this.lnkFilter.Size = new System.Drawing.Size(58, 13);
            this.lnkFilter.TabIndex = 2;
            this.lnkFilter.TabStop = true;
            this.lnkFilter.Text = "Apply Filter";
            this.lnkFilter.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkFilter_LinkClicked);
            // 
            // uctResults
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tblForm);
            this.Name = "uctResults";
            this.Size = new System.Drawing.Size(1046, 461);
            this.tblForm.ResumeLayout(false);
            this.tblForm.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdResults)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tblForm;
        private System.Windows.Forms.CheckBox chkToggleSelected;
        private System.Windows.Forms.Label lblTotalRows;
        private System.Windows.Forms.Label lblFilterRows;
        private System.Windows.Forms.LinkLabel lnkClearFilter;
        private System.Windows.Forms.LinkLabel lnkFilter;
        private System.Windows.Forms.DataGridView grdResults;
        private System.Windows.Forms.DataGridViewCheckBoxColumn clmSelected;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmJobTitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmTree;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmModel;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmKStart;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmlnL;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmK;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmCompletedAt;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmSequencesFileName;
    }
}
