namespace Pilgrimage.GeneSequences.EditNucleotideSequence
{
    partial class frmNucleotideSequenceFeatures
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
            this.grdFeature = new System.Windows.Forms.DataGridView();
            this.clmRank = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmFeatureKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grdFeatureInterval = new System.Windows.Forms.DataGridView();
            this.clmStart = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmEnd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmComplement = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.clmSequence = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.rtfNucleotideSequence = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnAddFeature = new System.Windows.Forms.Button();
            this.btnRemoveFeature = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.btnAddInterval = new System.Windows.Forms.Button();
            this.btnRemoveInterval = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lnkEditSourceSequence = new System.Windows.Forms.LinkLabel();
            this.btnClose = new System.Windows.Forms.Button();
            this.txtStartIndex = new System.Windows.Forms.MaskedTextBox();
            this.btnAddIntervalFromSelection = new System.Windows.Forms.Button();
            this.btnEditInterval = new System.Windows.Forms.Button();
            this.tipCommandButtons = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.grdFeature)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdFeatureInterval)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grdFeature
            // 
            this.grdFeature.AllowUserToAddRows = false;
            this.grdFeature.AllowUserToDeleteRows = false;
            this.grdFeature.AllowUserToResizeRows = false;
            this.grdFeature.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grdFeature.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdFeature.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmRank,
            this.clmFeatureKey});
            this.tableLayoutPanel1.SetColumnSpan(this.grdFeature, 3);
            this.grdFeature.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdFeature.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.grdFeature.Location = new System.Drawing.Point(3, 267);
            this.grdFeature.MultiSelect = false;
            this.grdFeature.Name = "grdFeature";
            this.grdFeature.ReadOnly = true;
            this.grdFeature.RowHeadersVisible = false;
            this.grdFeature.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdFeature.Size = new System.Drawing.Size(118, 116);
            this.grdFeature.TabIndex = 8;
            this.grdFeature.SelectionChanged += new System.EventHandler(this.grdFeature_SelectionChanged);
            // 
            // clmRank
            // 
            this.clmRank.DataPropertyName = "Rank";
            this.clmRank.FillWeight = 25F;
            this.clmRank.HeaderText = "Rank";
            this.clmRank.Name = "clmRank";
            this.clmRank.ReadOnly = true;
            this.clmRank.Visible = false;
            // 
            // clmFeatureKey
            // 
            this.clmFeatureKey.DataPropertyName = "Name";
            this.clmFeatureKey.FillWeight = 75F;
            this.clmFeatureKey.HeaderText = " ";
            this.clmFeatureKey.Name = "clmFeatureKey";
            this.clmFeatureKey.ReadOnly = true;
            // 
            // grdFeatureInterval
            // 
            this.grdFeatureInterval.AllowUserToAddRows = false;
            this.grdFeatureInterval.AllowUserToDeleteRows = false;
            this.grdFeatureInterval.AllowUserToResizeRows = false;
            this.grdFeatureInterval.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grdFeatureInterval.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdFeatureInterval.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmStart,
            this.clmEnd,
            this.clmComplement,
            this.clmSequence});
            this.tableLayoutPanel1.SetColumnSpan(this.grdFeatureInterval, 6);
            this.grdFeatureInterval.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdFeatureInterval.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.grdFeatureInterval.Location = new System.Drawing.Point(127, 267);
            this.grdFeatureInterval.Name = "grdFeatureInterval";
            this.grdFeatureInterval.ReadOnly = true;
            this.grdFeatureInterval.RowHeadersVisible = false;
            this.grdFeatureInterval.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdFeatureInterval.Size = new System.Drawing.Size(604, 116);
            this.grdFeatureInterval.TabIndex = 11;
            this.grdFeatureInterval.SelectionChanged += new System.EventHandler(this.grdFeatureInterval_SelectionChanged);
            // 
            // clmStart
            // 
            this.clmStart.DataPropertyName = "Start";
            this.clmStart.FillWeight = 13F;
            this.clmStart.HeaderText = "Start";
            this.clmStart.Name = "clmStart";
            this.clmStart.ReadOnly = true;
            // 
            // clmEnd
            // 
            this.clmEnd.DataPropertyName = "End";
            this.clmEnd.FillWeight = 13F;
            this.clmEnd.HeaderText = "End";
            this.clmEnd.Name = "clmEnd";
            this.clmEnd.ReadOnly = true;
            // 
            // clmComplement
            // 
            this.clmComplement.DataPropertyName = "IsComplement";
            this.clmComplement.FillWeight = 14F;
            this.clmComplement.HeaderText = "Complement";
            this.clmComplement.Name = "clmComplement";
            this.clmComplement.ReadOnly = true;
            // 
            // clmSequence
            // 
            this.clmSequence.DataPropertyName = "Sequence";
            this.clmSequence.FillWeight = 60F;
            this.clmSequence.HeaderText = "Sequence";
            this.clmSequence.Name = "clmSequence";
            this.clmSequence.ReadOnly = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 9;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 85F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.grdFeature, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.rtfNucleotideSequence, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnAddFeature, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.btnRemoveFeature, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.label3, 3, 3);
            this.tableLayoutPanel1.Controls.Add(this.grdFeatureInterval, 3, 4);
            this.tableLayoutPanel1.Controls.Add(this.btnAddInterval, 3, 5);
            this.tableLayoutPanel1.Controls.Add(this.btnRemoveInterval, 5, 5);
            this.tableLayoutPanel1.Controls.Add(this.btnSave, 6, 5);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 6, 0);
            this.tableLayoutPanel1.Controls.Add(this.lnkEditSourceSequence, 6, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnClose, 7, 5);
            this.tableLayoutPanel1.Controls.Add(this.txtStartIndex, 8, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnAddIntervalFromSelection, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnEditInterval, 4, 5);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(734, 416);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // rtfNucleotideSequence
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.rtfNucleotideSequence, 9);
            this.rtfNucleotideSequence.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtfNucleotideSequence.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtfNucleotideSequence.Location = new System.Drawing.Point(3, 29);
            this.rtfNucleotideSequence.Name = "rtfNucleotideSequence";
            this.rtfNucleotideSequence.Size = new System.Drawing.Size(728, 177);
            this.rtfNucleotideSequence.TabIndex = 3;
            this.rtfNucleotideSequence.Text = "";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label2, 3);
            this.label2.Location = new System.Drawing.Point(3, 248);
            this.label2.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Feature Types:";
            // 
            // btnAddFeature
            // 
            this.btnAddFeature.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnAddFeature.Location = new System.Drawing.Point(3, 389);
            this.btnAddFeature.Name = "btnAddFeature";
            this.btnAddFeature.Size = new System.Drawing.Size(23, 23);
            this.btnAddFeature.TabIndex = 9;
            this.tipCommandButtons.SetToolTip(this.btnAddFeature, "Add Feature");
            this.btnAddFeature.UseVisualStyleBackColor = true;
            this.btnAddFeature.Click += new System.EventHandler(this.btnAddFeature_Click);
            // 
            // btnRemoveFeature
            // 
            this.btnRemoveFeature.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnRemoveFeature.Location = new System.Drawing.Point(32, 389);
            this.btnRemoveFeature.Name = "btnRemoveFeature";
            this.btnRemoveFeature.Size = new System.Drawing.Size(23, 23);
            this.btnRemoveFeature.TabIndex = 10;
            this.tipCommandButtons.SetToolTip(this.btnRemoveFeature, "Remove Feature");
            this.btnRemoveFeature.UseVisualStyleBackColor = true;
            this.btnRemoveFeature.Click += new System.EventHandler(this.btnRemoveFeature_Click);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label3, 3);
            this.label3.Location = new System.Drawing.Point(127, 248);
            this.label3.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Intervals:";
            // 
            // btnAddInterval
            // 
            this.btnAddInterval.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnAddInterval.Location = new System.Drawing.Point(127, 389);
            this.btnAddInterval.Name = "btnAddInterval";
            this.btnAddInterval.Size = new System.Drawing.Size(23, 23);
            this.btnAddInterval.TabIndex = 12;
            this.tipCommandButtons.SetToolTip(this.btnAddInterval, "Add Interval");
            this.btnAddInterval.UseVisualStyleBackColor = true;
            this.btnAddInterval.Click += new System.EventHandler(this.btnAddInterval_Click);
            // 
            // btnRemoveInterval
            // 
            this.btnRemoveInterval.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnRemoveInterval.Location = new System.Drawing.Point(186, 389);
            this.btnRemoveInterval.Name = "btnRemoveInterval";
            this.btnRemoveInterval.Size = new System.Drawing.Size(23, 23);
            this.btnRemoveInterval.TabIndex = 14;
            this.tipCommandButtons.SetToolTip(this.btnRemoveInterval, "Remove Interval");
            this.btnRemoveInterval.UseVisualStyleBackColor = true;
            this.btnRemoveInterval.Click += new System.EventHandler(this.btnRemoveInterval_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.ImageKey = "Cancel";
            this.btnSave.Location = new System.Drawing.Point(564, 389);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 24);
            this.btnSave.TabIndex = 15;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label4, 4);
            this.label4.Location = new System.Drawing.Point(3, 6);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Source Sequence:";
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label1.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label1, 2);
            this.label1.Location = new System.Drawing.Point(623, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Start Index:";
            // 
            // lnkEditSourceSequence
            // 
            this.lnkEditSourceSequence.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lnkEditSourceSequence.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lnkEditSourceSequence, 3);
            this.lnkEditSourceSequence.Location = new System.Drawing.Point(617, 217);
            this.lnkEditSourceSequence.Margin = new System.Windows.Forms.Padding(3);
            this.lnkEditSourceSequence.Name = "lnkEditSourceSequence";
            this.lnkEditSourceSequence.Size = new System.Drawing.Size(114, 13);
            this.lnkEditSourceSequence.TabIndex = 5;
            this.lnkEditSourceSequence.TabStop = true;
            this.lnkEditSourceSequence.Text = "Edit Source Sequence";
            this.lnkEditSourceSequence.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkEditSourceSequence_LinkClicked);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.tableLayoutPanel1.SetColumnSpan(this.btnClose, 2);
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.ImageKey = "Cancel";
            this.btnClose.Location = new System.Drawing.Point(651, 389);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 24);
            this.btnClose.TabIndex = 16;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // txtStartIndex
            // 
            this.txtStartIndex.AllowPromptAsInput = false;
            this.txtStartIndex.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtStartIndex.Location = new System.Drawing.Point(690, 3);
            this.txtStartIndex.Mask = "######";
            this.txtStartIndex.Name = "txtStartIndex";
            this.txtStartIndex.PromptChar = ' ';
            this.txtStartIndex.Size = new System.Drawing.Size(40, 20);
            this.txtStartIndex.TabIndex = 2;
            // 
            // btnAddIntervalFromSelection
            // 
            this.btnAddIntervalFromSelection.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tableLayoutPanel1.SetColumnSpan(this.btnAddIntervalFromSelection, 3);
            this.btnAddIntervalFromSelection.Location = new System.Drawing.Point(127, 212);
            this.btnAddIntervalFromSelection.Name = "btnAddIntervalFromSelection";
            this.btnAddIntervalFromSelection.Size = new System.Drawing.Size(170, 23);
            this.btnAddIntervalFromSelection.TabIndex = 4;
            this.btnAddIntervalFromSelection.Text = "Add Interval from Selection";
            this.btnAddIntervalFromSelection.UseVisualStyleBackColor = true;
            this.btnAddIntervalFromSelection.Click += new System.EventHandler(this.btnAddIntervalFromSelection_Click);
            // 
            // btnEditInterval
            // 
            this.btnEditInterval.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnEditInterval.Location = new System.Drawing.Point(156, 389);
            this.btnEditInterval.Name = "btnEditInterval";
            this.btnEditInterval.Size = new System.Drawing.Size(23, 23);
            this.btnEditInterval.TabIndex = 13;
            this.tipCommandButtons.SetToolTip(this.btnEditInterval, "Edit Selected Interval");
            this.btnEditInterval.UseVisualStyleBackColor = true;
            this.btnEditInterval.Click += new System.EventHandler(this.btnEditInterval_Click);
            // 
            // frmNucleotideSequenceFeatures
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(734, 416);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "frmNucleotideSequenceFeatures";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Nucleotide Sequence";
            this.Load += new System.EventHandler(this.frmNucleotideSequenceFeatures_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdFeature)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdFeatureInterval)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView grdFeature;
        private System.Windows.Forms.DataGridView grdFeatureInterval;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmStart;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmEnd;
        private System.Windows.Forms.DataGridViewCheckBoxColumn clmComplement;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmSequence;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.RichTextBox rtfNucleotideSequence;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MaskedTextBox txtStartIndex;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnAddFeature;
        private System.Windows.Forms.Button btnRemoveFeature;
        private System.Windows.Forms.ToolTip tipCommandButtons;
        private System.Windows.Forms.Button btnAddInterval;
        private System.Windows.Forms.Button btnRemoveInterval;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.LinkLabel lnkEditSourceSequence;
        private System.Windows.Forms.Button btnAddIntervalFromSelection;
        private System.Windows.Forms.Button btnEditInterval;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmRank;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmFeatureKey;
    }
}