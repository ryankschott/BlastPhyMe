namespace Pilgrimage.GeneSequences.PhyML
{
    partial class frmPhyMLResults
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPhyMLResults));
            this.tbForm = new System.Windows.Forms.TabControl();
            this.pgOutput = new System.Windows.Forms.TabPage();
            this.tblOutput = new System.Windows.Forms.TableLayoutPanel();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lnkWorkingDirectory = new System.Windows.Forms.LinkLabel();
            this.lnkOriginalTreeFile = new System.Windows.Forms.LinkLabel();
            this.lnkUnlabeledTreeFile = new System.Windows.Forms.LinkLabel();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose_Output = new System.Windows.Forms.Button();
            this.pgOptions = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.uctPhyMLOptions1 = new Pilgrimage.GeneSequences.PhyML.uctPhyMLOptions();
            this.btnClose_Options = new System.Windows.Forms.Button();
            this.tbForm.SuspendLayout();
            this.pgOutput.SuspendLayout();
            this.tblOutput.SuspendLayout();
            this.pgOptions.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbForm
            // 
            this.tbForm.Controls.Add(this.pgOutput);
            this.tbForm.Controls.Add(this.pgOptions);
            this.tbForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbForm.Location = new System.Drawing.Point(0, 0);
            this.tbForm.Margin = new System.Windows.Forms.Padding(0);
            this.tbForm.Name = "tbForm";
            this.tbForm.SelectedIndex = 0;
            this.tbForm.Size = new System.Drawing.Size(757, 534);
            this.tbForm.TabIndex = 0;
            this.tbForm.Selected += new System.Windows.Forms.TabControlEventHandler(this.tbForm_Selected);
            // 
            // pgOutput
            // 
            this.pgOutput.Controls.Add(this.tblOutput);
            this.pgOutput.Location = new System.Drawing.Point(4, 22);
            this.pgOutput.Name = "pgOutput";
            this.pgOutput.Padding = new System.Windows.Forms.Padding(3);
            this.pgOutput.Size = new System.Drawing.Size(749, 508);
            this.pgOutput.TabIndex = 0;
            this.pgOutput.Text = "PhyML Output";
            this.pgOutput.UseVisualStyleBackColor = true;
            // 
            // tblOutput
            // 
            this.tblOutput.ColumnCount = 3;
            this.tblOutput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tblOutput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblOutput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tblOutput.Controls.Add(this.txtOutput, 0, 0);
            this.tblOutput.Controls.Add(this.label1, 0, 1);
            this.tblOutput.Controls.Add(this.lnkWorkingDirectory, 1, 1);
            this.tblOutput.Controls.Add(this.lnkOriginalTreeFile, 1, 2);
            this.tblOutput.Controls.Add(this.lnkUnlabeledTreeFile, 1, 3);
            this.tblOutput.Controls.Add(this.btnSave, 2, 1);
            this.tblOutput.Controls.Add(this.btnClose_Output, 2, 4);
            this.tblOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblOutput.Location = new System.Drawing.Point(3, 3);
            this.tblOutput.Margin = new System.Windows.Forms.Padding(0);
            this.tblOutput.Name = "tblOutput";
            this.tblOutput.RowCount = 5;
            this.tblOutput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblOutput.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblOutput.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblOutput.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblOutput.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblOutput.Size = new System.Drawing.Size(743, 502);
            this.tblOutput.TabIndex = 0;
            // 
            // txtOutput
            // 
            this.tblOutput.SetColumnSpan(this.txtOutput, 3);
            this.txtOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtOutput.Location = new System.Drawing.Point(3, 3);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtOutput.Size = new System.Drawing.Size(737, 392);
            this.txtOutput.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 406);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Output files:";
            // 
            // lnkWorkingDirectory
            // 
            this.lnkWorkingDirectory.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lnkWorkingDirectory.AutoSize = true;
            this.lnkWorkingDirectory.Location = new System.Drawing.Point(74, 406);
            this.lnkWorkingDirectory.Margin = new System.Windows.Forms.Padding(5);
            this.lnkWorkingDirectory.Name = "lnkWorkingDirectory";
            this.lnkWorkingDirectory.Size = new System.Drawing.Size(22, 13);
            this.lnkWorkingDirectory.TabIndex = 1;
            this.lnkWorkingDirectory.TabStop = true;
            this.lnkWorkingDirectory.Text = "C:\\";
            this.lnkWorkingDirectory.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkWorkingDirectory_LinkClicked);
            // 
            // lnkOriginalTreeFile
            // 
            this.lnkOriginalTreeFile.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lnkOriginalTreeFile.AutoSize = true;
            this.lnkOriginalTreeFile.Location = new System.Drawing.Point(74, 432);
            this.lnkOriginalTreeFile.Margin = new System.Windows.Forms.Padding(5);
            this.lnkOriginalTreeFile.Name = "lnkOriginalTreeFile";
            this.lnkOriginalTreeFile.Size = new System.Drawing.Size(193, 13);
            this.lnkOriginalTreeFile.TabIndex = 1;
            this.lnkOriginalTreeFile.TabStop = true;
            this.lnkOriginalTreeFile.Text = "Open tree with support values in viewer";
            this.lnkOriginalTreeFile.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkOpenTreeFile_LinkClicked);
            // 
            // lnkUnlabeledTreeFile
            // 
            this.lnkUnlabeledTreeFile.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lnkUnlabeledTreeFile.AutoSize = true;
            this.lnkUnlabeledTreeFile.Location = new System.Drawing.Point(74, 455);
            this.lnkUnlabeledTreeFile.Margin = new System.Windows.Forms.Padding(5);
            this.lnkUnlabeledTreeFile.Name = "lnkUnlabeledTreeFile";
            this.lnkUnlabeledTreeFile.Size = new System.Drawing.Size(208, 13);
            this.lnkUnlabeledTreeFile.TabIndex = 1;
            this.lnkUnlabeledTreeFile.TabStop = true;
            this.lnkUnlabeledTreeFile.Text = "Open tree without support values in viewer";
            this.lnkUnlabeledTreeFile.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkOpenTreeFile_LinkClicked);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnSave.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnSave.Location = new System.Drawing.Point(665, 401);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose_Output
            // 
            this.btnClose_Output.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnClose_Output.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose_Output.Location = new System.Drawing.Point(665, 476);
            this.btnClose_Output.Name = "btnClose_Output";
            this.btnClose_Output.Size = new System.Drawing.Size(75, 23);
            this.btnClose_Output.TabIndex = 3;
            this.btnClose_Output.Text = "&Close";
            this.btnClose_Output.UseVisualStyleBackColor = true;
            // 
            // pgOptions
            // 
            this.pgOptions.Controls.Add(this.tableLayoutPanel2);
            this.pgOptions.Location = new System.Drawing.Point(4, 22);
            this.pgOptions.Name = "pgOptions";
            this.pgOptions.Padding = new System.Windows.Forms.Padding(3);
            this.pgOptions.Size = new System.Drawing.Size(749, 508);
            this.pgOptions.TabIndex = 1;
            this.pgOptions.Text = "Options";
            this.pgOptions.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.uctPhyMLOptions1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnClose_Options, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(743, 502);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // uctPhyMLOptions1
            // 
            this.uctPhyMLOptions1.AutoSize = true;
            this.uctPhyMLOptions1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.uctPhyMLOptions1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uctPhyMLOptions1.Location = new System.Drawing.Point(0, 0);
            this.uctPhyMLOptions1.Margin = new System.Windows.Forms.Padding(0);
            this.uctPhyMLOptions1.Name = "uctPhyMLOptions1";
            this.uctPhyMLOptions1.Size = new System.Drawing.Size(744, 473);
            this.uctPhyMLOptions1.TabIndex = 0;
            // 
            // btnClose_Options
            // 
            this.btnClose_Options.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnClose_Options.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose_Options.Location = new System.Drawing.Point(666, 476);
            this.btnClose_Options.Name = "btnClose_Options";
            this.btnClose_Options.Size = new System.Drawing.Size(75, 23);
            this.btnClose_Options.TabIndex = 1;
            this.btnClose_Options.Text = "Close";
            this.btnClose_Options.UseVisualStyleBackColor = true;
            // 
            // frmPhyMLResults
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose_Output;
            this.ClientSize = new System.Drawing.Size(757, 534);
            this.Controls.Add(this.tbForm);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(773, 568);
            this.Name = "frmPhyMLResults";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Results";
            this.tbForm.ResumeLayout(false);
            this.pgOutput.ResumeLayout(false);
            this.tblOutput.ResumeLayout(false);
            this.tblOutput.PerformLayout();
            this.pgOptions.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tbForm;
        private System.Windows.Forms.TabPage pgOutput;
        private System.Windows.Forms.TabPage pgOptions;
        private uctPhyMLOptions uctPhyMLOptions1;
        private System.Windows.Forms.TableLayoutPanel tblOutput;
        private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.LinkLabel lnkWorkingDirectory;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnClose_Output;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button btnClose_Options;
        private System.Windows.Forms.LinkLabel lnkOriginalTreeFile;
        private System.Windows.Forms.LinkLabel lnkUnlabeledTreeFile;
        private System.Windows.Forms.Button btnSave;
    }
}