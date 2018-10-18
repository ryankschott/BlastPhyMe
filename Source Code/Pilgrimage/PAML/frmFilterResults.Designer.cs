namespace Pilgrimage.PAML
{
    partial class frmFilterResults
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmFilterResults));
            this.btnClear = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbJobLogic = new System.Windows.Forms.ComboBox();
            this.txtJobTitle = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cmbTreeLogic = new System.Windows.Forms.ComboBox();
            this.cmbTreeFileLogic = new System.Windows.Forms.ComboBox();
            this.cmbSequencesFileLogic = new System.Windows.Forms.ComboBox();
            this.txtTreeTitle = new System.Windows.Forms.TextBox();
            this.txtTreeFile = new System.Windows.Forms.TextBox();
            this.txtSequencesFile = new System.Windows.Forms.TextBox();
            this.chkModelPresets = new System.Windows.Forms.TreeView();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClear
            // 
            this.btnClear.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnClear.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClear.ImageKey = "Filter";
            this.btnClear.Location = new System.Drawing.Point(3, 284);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(80, 24);
            this.btnClear.TabIndex = 14;
            this.btnClear.Text = "&Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnApply
            // 
            this.btnApply.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnApply.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnApply.ImageKey = "Filter";
            this.btnApply.Location = new System.Drawing.Point(265, 284);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(80, 24);
            this.btnApply.TabIndex = 15;
            this.btnApply.Text = "&Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.ImageKey = "Cancel";
            this.btnCancel.Location = new System.Drawing.Point(351, 284);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 24);
            this.btnCancel.TabIndex = 16;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Job Title:";
            // 
            // cmbJobLogic
            // 
            this.cmbJobLogic.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cmbJobLogic.DisplayMember = "Value";
            this.cmbJobLogic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbJobLogic.FormattingEnabled = true;
            this.cmbJobLogic.Location = new System.Drawing.Point(98, 5);
            this.cmbJobLogic.Margin = new System.Windows.Forms.Padding(5);
            this.cmbJobLogic.Name = "cmbJobLogic";
            this.cmbJobLogic.Size = new System.Drawing.Size(90, 21);
            this.cmbJobLogic.TabIndex = 1;
            this.cmbJobLogic.ValueMember = "Key";
            // 
            // txtJobTitle
            // 
            this.txtJobTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel4.SetColumnSpan(this.txtJobTitle, 2);
            this.txtJobTitle.Location = new System.Drawing.Point(198, 5);
            this.txtJobTitle.Margin = new System.Windows.Forms.Padding(5);
            this.txtJobTitle.Name = "txtJobTitle";
            this.txtJobTitle.Size = new System.Drawing.Size(231, 20);
            this.txtJobTitle.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(5, 132);
            this.label5.Margin = new System.Windows.Forms.Padding(5, 8, 5, 5);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Models:";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 4;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.Controls.Add(this.btnClear, 0, 5);
            this.tableLayoutPanel4.Controls.Add(this.cmbJobLogic, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.btnApply, 2, 5);
            this.tableLayoutPanel4.Controls.Add(this.btnCancel, 3, 5);
            this.tableLayoutPanel4.Controls.Add(this.txtJobTitle, 2, 0);
            this.tableLayoutPanel4.Controls.Add(this.label6, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.label7, 0, 2);
            this.tableLayoutPanel4.Controls.Add(this.label8, 0, 3);
            this.tableLayoutPanel4.Controls.Add(this.cmbTreeLogic, 1, 1);
            this.tableLayoutPanel4.Controls.Add(this.cmbTreeFileLogic, 1, 2);
            this.tableLayoutPanel4.Controls.Add(this.cmbSequencesFileLogic, 1, 3);
            this.tableLayoutPanel4.Controls.Add(this.txtTreeTitle, 2, 1);
            this.tableLayoutPanel4.Controls.Add(this.txtTreeFile, 2, 2);
            this.tableLayoutPanel4.Controls.Add(this.txtSequencesFile, 2, 3);
            this.tableLayoutPanel4.Controls.Add(this.label5, 0, 4);
            this.tableLayoutPanel4.Controls.Add(this.chkModelPresets, 1, 4);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 6;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(434, 311);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(5, 40);
            this.label6.Margin = new System.Windows.Forms.Padding(5);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(55, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "Tree Title:";
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(5, 71);
            this.label7.Margin = new System.Windows.Forms.Padding(5);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(51, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Tree File:";
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(5, 102);
            this.label8.Margin = new System.Windows.Forms.Padding(5);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(83, 13);
            this.label8.TabIndex = 9;
            this.label8.Text = "Sequences File:";
            // 
            // cmbTreeLogic
            // 
            this.cmbTreeLogic.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cmbTreeLogic.DisplayMember = "Value";
            this.cmbTreeLogic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTreeLogic.FormattingEnabled = true;
            this.cmbTreeLogic.Location = new System.Drawing.Point(98, 36);
            this.cmbTreeLogic.Margin = new System.Windows.Forms.Padding(5);
            this.cmbTreeLogic.Name = "cmbTreeLogic";
            this.cmbTreeLogic.Size = new System.Drawing.Size(90, 21);
            this.cmbTreeLogic.TabIndex = 4;
            this.cmbTreeLogic.ValueMember = "Key";
            // 
            // cmbTreeFileLogic
            // 
            this.cmbTreeFileLogic.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cmbTreeFileLogic.DisplayMember = "Value";
            this.cmbTreeFileLogic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTreeFileLogic.FormattingEnabled = true;
            this.cmbTreeFileLogic.Location = new System.Drawing.Point(98, 67);
            this.cmbTreeFileLogic.Margin = new System.Windows.Forms.Padding(5);
            this.cmbTreeFileLogic.Name = "cmbTreeFileLogic";
            this.cmbTreeFileLogic.Size = new System.Drawing.Size(90, 21);
            this.cmbTreeFileLogic.TabIndex = 7;
            this.cmbTreeFileLogic.ValueMember = "Key";
            // 
            // cmbSequencesFileLogic
            // 
            this.cmbSequencesFileLogic.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cmbSequencesFileLogic.DisplayMember = "Value";
            this.cmbSequencesFileLogic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSequencesFileLogic.FormattingEnabled = true;
            this.cmbSequencesFileLogic.Location = new System.Drawing.Point(98, 98);
            this.cmbSequencesFileLogic.Margin = new System.Windows.Forms.Padding(5);
            this.cmbSequencesFileLogic.Name = "cmbSequencesFileLogic";
            this.cmbSequencesFileLogic.Size = new System.Drawing.Size(90, 21);
            this.cmbSequencesFileLogic.TabIndex = 10;
            this.cmbSequencesFileLogic.ValueMember = "Key";
            // 
            // txtTreeTitle
            // 
            this.txtTreeTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel4.SetColumnSpan(this.txtTreeTitle, 2);
            this.txtTreeTitle.Location = new System.Drawing.Point(198, 36);
            this.txtTreeTitle.Margin = new System.Windows.Forms.Padding(5);
            this.txtTreeTitle.Name = "txtTreeTitle";
            this.txtTreeTitle.Size = new System.Drawing.Size(231, 20);
            this.txtTreeTitle.TabIndex = 5;
            // 
            // txtTreeFile
            // 
            this.txtTreeFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel4.SetColumnSpan(this.txtTreeFile, 2);
            this.txtTreeFile.Location = new System.Drawing.Point(198, 67);
            this.txtTreeFile.Margin = new System.Windows.Forms.Padding(5);
            this.txtTreeFile.Name = "txtTreeFile";
            this.txtTreeFile.Size = new System.Drawing.Size(231, 20);
            this.txtTreeFile.TabIndex = 8;
            // 
            // txtSequencesFile
            // 
            this.txtSequencesFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel4.SetColumnSpan(this.txtSequencesFile, 2);
            this.txtSequencesFile.Location = new System.Drawing.Point(198, 98);
            this.txtSequencesFile.Margin = new System.Windows.Forms.Padding(5);
            this.txtSequencesFile.Name = "txtSequencesFile";
            this.txtSequencesFile.Size = new System.Drawing.Size(231, 20);
            this.txtSequencesFile.TabIndex = 11;
            // 
            // chkModelPresets
            // 
            this.chkModelPresets.CheckBoxes = true;
            this.tableLayoutPanel4.SetColumnSpan(this.chkModelPresets, 3);
            this.chkModelPresets.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkModelPresets.Location = new System.Drawing.Point(96, 127);
            this.chkModelPresets.Name = "chkModelPresets";
            this.chkModelPresets.ShowLines = false;
            this.chkModelPresets.ShowPlusMinus = false;
            this.chkModelPresets.ShowRootLines = false;
            this.chkModelPresets.Size = new System.Drawing.Size(335, 151);
            this.chkModelPresets.TabIndex = 17;
            // 
            // frmFilterResults
            // 
            this.AcceptButton = this.btnApply;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(434, 311);
            this.Controls.Add(this.tableLayoutPanel4);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmFilterResults";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Filter";
            this.Load += new System.EventHandler(this.frmFilterResults_Load);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbJobLogic;
        private System.Windows.Forms.TextBox txtJobTitle;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cmbTreeLogic;
        private System.Windows.Forms.ComboBox cmbTreeFileLogic;
        private System.Windows.Forms.ComboBox cmbSequencesFileLogic;
        private System.Windows.Forms.TextBox txtTreeTitle;
        private System.Windows.Forms.TextBox txtTreeFile;
        private System.Windows.Forms.TextBox txtSequencesFile;
        private System.Windows.Forms.TreeView chkModelPresets;
    }
}