namespace Pilgrimage.RecordSets
{
    partial class frmExportToPilgrimageDataFile
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmExportToPilgrimageDataFile));
            this.chkIncludeAlignedSequences = new System.Windows.Forms.CheckBox();
            this.chkGeneSequencesJobHistories = new System.Windows.Forms.CheckBox();
            this.chkSelectionAnalysesJobHistories = new System.Windows.Forms.CheckBox();
            this.lblSelectionAnalysesSubSets = new System.Windows.Forms.Label();
            this.tvSelectionAnalysesSubSets = new System.Windows.Forms.TreeView();
            this.ilForm = new System.Windows.Forms.ImageList(this.components);
            this.tbForm = new System.Windows.Forms.TabControl();
            this.pgGeneSequences = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.lblGeneSequenceSubSets = new System.Windows.Forms.Label();
            this.tvGeneSequenceSubSets = new System.Windows.Forms.TreeView();
            this.tvJobTargets = new System.Windows.Forms.TreeView();
            this.chkGeneSequenceToggleSubSets = new System.Windows.Forms.CheckBox();
            this.pgSelectionAnalyses = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.chkSelectionAnalysesToggleSubSets = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tbForm.SuspendLayout();
            this.pgGeneSequences.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.pgSelectionAnalyses.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkIncludeAlignedSequences
            // 
            this.chkIncludeAlignedSequences.AutoSize = true;
            this.tableLayoutPanel2.SetColumnSpan(this.chkIncludeAlignedSequences, 2);
            this.chkIncludeAlignedSequences.Enabled = false;
            this.chkIncludeAlignedSequences.Location = new System.Drawing.Point(5, 5);
            this.chkIncludeAlignedSequences.Margin = new System.Windows.Forms.Padding(5);
            this.chkIncludeAlignedSequences.Name = "chkIncludeAlignedSequences";
            this.chkIncludeAlignedSequences.Size = new System.Drawing.Size(345, 17);
            this.chkIncludeAlignedSequences.TabIndex = 0;
            this.chkIncludeAlignedSequences.Text = "Include gene sequences aligned with the selected gene sequences";
            this.chkIncludeAlignedSequences.UseVisualStyleBackColor = true;
            // 
            // chkGeneSequencesJobHistories
            // 
            this.chkGeneSequencesJobHistories.AutoSize = true;
            this.tableLayoutPanel2.SetColumnSpan(this.chkGeneSequencesJobHistories, 2);
            this.chkGeneSequencesJobHistories.Enabled = false;
            this.chkGeneSequencesJobHistories.Location = new System.Drawing.Point(5, 32);
            this.chkGeneSequencesJobHistories.Margin = new System.Windows.Forms.Padding(5);
            this.chkGeneSequencesJobHistories.Name = "chkGeneSequencesJobHistories";
            this.chkGeneSequencesJobHistories.Size = new System.Drawing.Size(277, 17);
            this.chkGeneSequencesJobHistories.TabIndex = 0;
            this.chkGeneSequencesJobHistories.Text = "Include job histories for the selected gene sequences";
            this.chkGeneSequencesJobHistories.UseVisualStyleBackColor = true;
            this.chkGeneSequencesJobHistories.CheckedChanged += new System.EventHandler(this.chkGeneSequencesJobHistories_CheckedChanged);
            // 
            // chkSelectionAnalysesJobHistories
            // 
            this.chkSelectionAnalysesJobHistories.AutoSize = true;
            this.chkSelectionAnalysesJobHistories.Checked = true;
            this.chkSelectionAnalysesJobHistories.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tableLayoutPanel1.SetColumnSpan(this.chkSelectionAnalysesJobHistories, 2);
            this.chkSelectionAnalysesJobHistories.Enabled = false;
            this.chkSelectionAnalysesJobHistories.Location = new System.Drawing.Point(5, 5);
            this.chkSelectionAnalysesJobHistories.Margin = new System.Windows.Forms.Padding(5);
            this.chkSelectionAnalysesJobHistories.Name = "chkSelectionAnalysesJobHistories";
            this.chkSelectionAnalysesJobHistories.Size = new System.Drawing.Size(260, 17);
            this.chkSelectionAnalysesJobHistories.TabIndex = 0;
            this.chkSelectionAnalysesJobHistories.Text = "Include job histories for the selected PAML results";
            this.chkSelectionAnalysesJobHistories.UseVisualStyleBackColor = true;
            // 
            // lblSelectionAnalysesSubSets
            // 
            this.lblSelectionAnalysesSubSets.AutoSize = true;
            this.lblSelectionAnalysesSubSets.Location = new System.Drawing.Point(2, 32);
            this.lblSelectionAnalysesSubSets.Margin = new System.Windows.Forms.Padding(2, 5, 5, 5);
            this.lblSelectionAnalysesSubSets.Name = "lblSelectionAnalysesSubSets";
            this.lblSelectionAnalysesSubSets.Size = new System.Drawing.Size(52, 13);
            this.lblSelectionAnalysesSubSets.TabIndex = 1;
            this.lblSelectionAnalysesSubSets.Text = "Datasets:";
            // 
            // tvSelectionAnalysesSubSets
            // 
            this.tvSelectionAnalysesSubSets.CheckBoxes = true;
            this.tableLayoutPanel1.SetColumnSpan(this.tvSelectionAnalysesSubSets, 2);
            this.tvSelectionAnalysesSubSets.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvSelectionAnalysesSubSets.ImageIndex = 0;
            this.tvSelectionAnalysesSubSets.ImageList = this.ilForm;
            this.tvSelectionAnalysesSubSets.Location = new System.Drawing.Point(3, 53);
            this.tvSelectionAnalysesSubSets.Name = "tvSelectionAnalysesSubSets";
            this.tvSelectionAnalysesSubSets.SelectedImageIndex = 0;
            this.tvSelectionAnalysesSubSets.ShowLines = false;
            this.tvSelectionAnalysesSubSets.ShowRootLines = false;
            this.tvSelectionAnalysesSubSets.Size = new System.Drawing.Size(364, 248);
            this.tvSelectionAnalysesSubSets.TabIndex = 4;
            this.tvSelectionAnalysesSubSets.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tvSubSets_AfterCheck);
            // 
            // ilForm
            // 
            this.ilForm.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilForm.ImageStream")));
            this.ilForm.TransparentColor = System.Drawing.Color.Transparent;
            this.ilForm.Images.SetKeyName(0, "RecordSet");
            this.ilForm.Images.SetKeyName(1, "SubSet");
            // 
            // tbForm
            // 
            this.tableLayoutPanel3.SetColumnSpan(this.tbForm, 2);
            this.tbForm.Controls.Add(this.pgGeneSequences);
            this.tbForm.Controls.Add(this.pgSelectionAnalyses);
            this.tbForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbForm.Location = new System.Drawing.Point(0, 0);
            this.tbForm.Margin = new System.Windows.Forms.Padding(0);
            this.tbForm.Name = "tbForm";
            this.tbForm.SelectedIndex = 0;
            this.tbForm.Size = new System.Drawing.Size(384, 336);
            this.tbForm.TabIndex = 3;
            // 
            // pgGeneSequences
            // 
            this.pgGeneSequences.Controls.Add(this.tableLayoutPanel2);
            this.pgGeneSequences.Location = new System.Drawing.Point(4, 22);
            this.pgGeneSequences.Name = "pgGeneSequences";
            this.pgGeneSequences.Padding = new System.Windows.Forms.Padding(3);
            this.pgGeneSequences.Size = new System.Drawing.Size(376, 310);
            this.pgGeneSequences.TabIndex = 0;
            this.pgGeneSequences.Text = "Gene Sequences";
            this.pgGeneSequences.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel2.Controls.Add(this.chkIncludeAlignedSequences, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblGeneSequenceSubSets, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.chkGeneSequencesJobHistories, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.tvGeneSequenceSubSets, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.tvJobTargets, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.chkGeneSequenceToggleSubSets, 1, 3);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 5;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(370, 304);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // lblGeneSequenceSubSets
            // 
            this.lblGeneSequenceSubSets.AutoSize = true;
            this.lblGeneSequenceSubSets.Location = new System.Drawing.Point(2, 139);
            this.lblGeneSequenceSubSets.Margin = new System.Windows.Forms.Padding(2, 5, 5, 5);
            this.lblGeneSequenceSubSets.Name = "lblGeneSequenceSubSets";
            this.lblGeneSequenceSubSets.Size = new System.Drawing.Size(52, 13);
            this.lblGeneSequenceSubSets.TabIndex = 2;
            this.lblGeneSequenceSubSets.Text = "Datasets:";
            // 
            // tvGeneSequenceSubSets
            // 
            this.tvGeneSequenceSubSets.CheckBoxes = true;
            this.tableLayoutPanel2.SetColumnSpan(this.tvGeneSequenceSubSets, 2);
            this.tvGeneSequenceSubSets.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvGeneSequenceSubSets.ImageIndex = 0;
            this.tvGeneSequenceSubSets.ImageList = this.ilForm;
            this.tvGeneSequenceSubSets.Location = new System.Drawing.Point(3, 160);
            this.tvGeneSequenceSubSets.Name = "tvGeneSequenceSubSets";
            this.tvGeneSequenceSubSets.SelectedImageIndex = 0;
            this.tvGeneSequenceSubSets.ShowLines = false;
            this.tvGeneSequenceSubSets.ShowRootLines = false;
            this.tvGeneSequenceSubSets.Size = new System.Drawing.Size(364, 141);
            this.tvGeneSequenceSubSets.TabIndex = 5;
            this.tvGeneSequenceSubSets.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tvSubSets_AfterCheck);
            // 
            // tvJobTargets
            // 
            this.tvJobTargets.CheckBoxes = true;
            this.tableLayoutPanel2.SetColumnSpan(this.tvJobTargets, 2);
            this.tvJobTargets.Location = new System.Drawing.Point(25, 59);
            this.tvJobTargets.Margin = new System.Windows.Forms.Padding(25, 5, 5, 5);
            this.tvJobTargets.Name = "tvJobTargets";
            this.tvJobTargets.ShowLines = false;
            this.tvJobTargets.ShowRootLines = false;
            this.tvJobTargets.Size = new System.Drawing.Size(120, 70);
            this.tvJobTargets.TabIndex = 6;
            // 
            // chkGeneSequenceToggleSubSets
            // 
            this.chkGeneSequenceToggleSubSets.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.chkGeneSequenceToggleSubSets.AutoSize = true;
            this.chkGeneSequenceToggleSubSets.Checked = true;
            this.chkGeneSequenceToggleSubSets.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkGeneSequenceToggleSubSets.Location = new System.Drawing.Point(285, 137);
            this.chkGeneSequenceToggleSubSets.Name = "chkGeneSequenceToggleSubSets";
            this.chkGeneSequenceToggleSubSets.Size = new System.Drawing.Size(82, 17);
            this.chkGeneSequenceToggleSubSets.TabIndex = 7;
            this.chkGeneSequenceToggleSubSets.Text = "Deselect All";
            this.chkGeneSequenceToggleSubSets.UseVisualStyleBackColor = true;
            this.chkGeneSequenceToggleSubSets.CheckedChanged += new System.EventHandler(this.chkToggleSubSets_CheckedChanged);
            // 
            // pgSelectionAnalyses
            // 
            this.pgSelectionAnalyses.Controls.Add(this.tableLayoutPanel1);
            this.pgSelectionAnalyses.Location = new System.Drawing.Point(4, 22);
            this.pgSelectionAnalyses.Name = "pgSelectionAnalyses";
            this.pgSelectionAnalyses.Padding = new System.Windows.Forms.Padding(3);
            this.pgSelectionAnalyses.Size = new System.Drawing.Size(376, 310);
            this.pgSelectionAnalyses.TabIndex = 1;
            this.pgSelectionAnalyses.Text = "Selection Analyses (PAML)";
            this.pgSelectionAnalyses.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel1.Controls.Add(this.tvSelectionAnalysesSubSets, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.chkSelectionAnalysesJobHistories, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblSelectionAnalysesSubSets, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.chkSelectionAnalysesToggleSubSets, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(370, 304);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // chkSelectionAnalysesToggleSubSets
            // 
            this.chkSelectionAnalysesToggleSubSets.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.chkSelectionAnalysesToggleSubSets.AutoSize = true;
            this.chkSelectionAnalysesToggleSubSets.Checked = true;
            this.chkSelectionAnalysesToggleSubSets.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSelectionAnalysesToggleSubSets.Location = new System.Drawing.Point(285, 30);
            this.chkSelectionAnalysesToggleSubSets.Name = "chkSelectionAnalysesToggleSubSets";
            this.chkSelectionAnalysesToggleSubSets.Size = new System.Drawing.Size(82, 17);
            this.chkSelectionAnalysesToggleSubSets.TabIndex = 5;
            this.chkSelectionAnalysesToggleSubSets.Text = "Deselect All";
            this.chkSelectionAnalysesToggleSubSets.UseVisualStyleBackColor = true;
            this.chkSelectionAnalysesToggleSubSets.CheckedChanged += new System.EventHandler(this.chkToggleSubSets_CheckedChanged);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.Controls.Add(this.tbForm, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnExport, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.btnCancel, 1, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(384, 366);
            this.tableLayoutPanel3.TabIndex = 4;
            // 
            // btnExport
            // 
            this.btnExport.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnExport.Location = new System.Drawing.Point(225, 339);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 23);
            this.btnExport.TabIndex = 4;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(306, 339);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // frmExportToPilgrimageDataFile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(384, 366);
            this.Controls.Add(this.tableLayoutPanel3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(400, 400);
            this.Name = "frmExportToPilgrimageDataFile";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.frmExportToPilgrimageDataFile_Load);
            this.tbForm.ResumeLayout(false);
            this.pgGeneSequences.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.pgSelectionAnalyses.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox chkIncludeAlignedSequences;
        private System.Windows.Forms.CheckBox chkGeneSequencesJobHistories;
        private System.Windows.Forms.CheckBox chkSelectionAnalysesJobHistories;
        private System.Windows.Forms.Label lblSelectionAnalysesSubSets;
        private System.Windows.Forms.TreeView tvSelectionAnalysesSubSets;
        private System.Windows.Forms.TabControl tbForm;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TabPage pgGeneSequences;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label lblGeneSequenceSubSets;
        private System.Windows.Forms.TreeView tvGeneSequenceSubSets;
        private System.Windows.Forms.TabPage pgSelectionAnalyses;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ImageList ilForm;
        private System.Windows.Forms.TreeView tvJobTargets;
        private System.Windows.Forms.CheckBox chkGeneSequenceToggleSubSets;
        private System.Windows.Forms.CheckBox chkSelectionAnalysesToggleSubSets;
    }
}