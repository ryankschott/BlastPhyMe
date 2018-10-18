namespace Pilgrimage.PAML
{
    partial class uctTreeConfiguration
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
            this.label3 = new System.Windows.Forms.Label();
            this.btnBrowse_SequencesFile = new System.Windows.Forms.Button();
            this.btnBrowse_TreeFile = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTreeFile = new System.Windows.Forms.TextBox();
            this.uctAnalysisConfigurations1 = new Pilgrimage.PAML.uctAnalysisConfigurations();
            this.tblControlButtons = new System.Windows.Forms.TableLayoutPanel();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.txtSequencesFile = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lnkAdditionalOptions = new System.Windows.Forms.LinkLabel();
            this.label6 = new System.Windows.Forms.Label();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.lnkCustomControlFile = new System.Windows.Forms.LinkLabel();
            this.tblForm.SuspendLayout();
            this.tblControlButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // tblForm
            // 
            this.tblForm.AutoSize = true;
            this.tblForm.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tblForm.ColumnCount = 4;
            this.tblForm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tblForm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblForm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tblForm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tblForm.Controls.Add(this.label3, 0, 1);
            this.tblForm.Controls.Add(this.btnBrowse_SequencesFile, 3, 1);
            this.tblForm.Controls.Add(this.btnBrowse_TreeFile, 3, 0);
            this.tblForm.Controls.Add(this.label2, 0, 0);
            this.tblForm.Controls.Add(this.txtTreeFile, 1, 0);
            this.tblForm.Controls.Add(this.uctAnalysisConfigurations1, 1, 4);
            this.tblForm.Controls.Add(this.tblControlButtons, 1, 5);
            this.tblForm.Controls.Add(this.txtSequencesFile, 1, 1);
            this.tblForm.Controls.Add(this.label1, 0, 4);
            this.tblForm.Controls.Add(this.lnkAdditionalOptions, 0, 3);
            this.tblForm.Controls.Add(this.label6, 0, 2);
            this.tblForm.Controls.Add(this.txtTitle, 1, 2);
            this.tblForm.Controls.Add(this.lnkCustomControlFile, 2, 3);
            this.tblForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblForm.Location = new System.Drawing.Point(0, 0);
            this.tblForm.Name = "tblForm";
            this.tblForm.RowCount = 6;
            this.tblForm.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblForm.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblForm.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblForm.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblForm.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblForm.Size = new System.Drawing.Size(734, 300);
            this.tblForm.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 37);
            this.label3.Margin = new System.Windows.Forms.Padding(5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Sequences File:";
            // 
            // btnBrowse_SequencesFile
            // 
            this.btnBrowse_SequencesFile.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnBrowse_SequencesFile.Location = new System.Drawing.Point(706, 32);
            this.btnBrowse_SequencesFile.Name = "btnBrowse_SequencesFile";
            this.btnBrowse_SequencesFile.Size = new System.Drawing.Size(24, 23);
            this.btnBrowse_SequencesFile.TabIndex = 5;
            this.btnBrowse_SequencesFile.Text = "...";
            this.btnBrowse_SequencesFile.UseVisualStyleBackColor = true;
            this.btnBrowse_SequencesFile.Click += new System.EventHandler(this.btnBrowse_File_Click);
            // 
            // btnBrowse_TreeFile
            // 
            this.btnBrowse_TreeFile.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnBrowse_TreeFile.Location = new System.Drawing.Point(706, 3);
            this.btnBrowse_TreeFile.Name = "btnBrowse_TreeFile";
            this.btnBrowse_TreeFile.Size = new System.Drawing.Size(25, 23);
            this.btnBrowse_TreeFile.TabIndex = 2;
            this.btnBrowse_TreeFile.Text = "...";
            this.btnBrowse_TreeFile.UseVisualStyleBackColor = true;
            this.btnBrowse_TreeFile.Click += new System.EventHandler(this.btnBrowse_File_Click);
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 8);
            this.label2.Margin = new System.Windows.Forms.Padding(5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Tree File:";
            // 
            // txtTreeFile
            // 
            this.txtTreeFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tblForm.SetColumnSpan(this.txtTreeFile, 2);
            this.txtTreeFile.Location = new System.Drawing.Point(96, 4);
            this.txtTreeFile.Name = "txtTreeFile";
            this.txtTreeFile.Size = new System.Drawing.Size(604, 20);
            this.txtTreeFile.TabIndex = 1;
            // 
            // uctAnalysisConfigurations1
            // 
            this.tblForm.SetColumnSpan(this.uctAnalysisConfigurations1, 3);
            this.uctAnalysisConfigurations1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uctAnalysisConfigurations1.Location = new System.Drawing.Point(96, 110);
            this.uctAnalysisConfigurations1.Name = "uctAnalysisConfigurations1";
            this.uctAnalysisConfigurations1.Size = new System.Drawing.Size(635, 158);
            this.uctAnalysisConfigurations1.TabIndex = 10;
            // 
            // tblControlButtons
            // 
            this.tblControlButtons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tblControlButtons.AutoSize = true;
            this.tblControlButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tblControlButtons.ColumnCount = 4;
            this.tblForm.SetColumnSpan(this.tblControlButtons, 2);
            this.tblControlButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tblControlButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tblControlButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tblControlButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tblControlButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tblControlButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tblControlButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tblControlButtons.Controls.Add(this.btnAdd, 0, 0);
            this.tblControlButtons.Controls.Add(this.btnEdit, 1, 0);
            this.tblControlButtons.Controls.Add(this.btnRemove, 3, 0);
            this.tblControlButtons.Controls.Add(this.btnCopy, 2, 0);
            this.tblControlButtons.Location = new System.Drawing.Point(93, 271);
            this.tblControlButtons.Margin = new System.Windows.Forms.Padding(0);
            this.tblControlButtons.Name = "tblControlButtons";
            this.tblControlButtons.RowCount = 1;
            this.tblControlButtons.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblControlButtons.Size = new System.Drawing.Size(610, 29);
            this.tblControlButtons.TabIndex = 11;
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnAdd.Location = new System.Drawing.Point(3, 3);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(80, 23);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "&Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnEdit.Location = new System.Drawing.Point(89, 3);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(80, 23);
            this.btnEdit.TabIndex = 1;
            this.btnEdit.Text = "&Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnRemove.Location = new System.Drawing.Point(261, 3);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(80, 23);
            this.btnRemove.TabIndex = 3;
            this.btnRemove.Text = "&Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnCopy
            // 
            this.btnCopy.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnCopy.Location = new System.Drawing.Point(175, 3);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(80, 23);
            this.btnCopy.TabIndex = 2;
            this.btnCopy.Text = "&Copy";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // txtSequencesFile
            // 
            this.txtSequencesFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tblForm.SetColumnSpan(this.txtSequencesFile, 2);
            this.txtSequencesFile.Location = new System.Drawing.Point(96, 33);
            this.txtSequencesFile.Name = "txtSequencesFile";
            this.txtSequencesFile.Size = new System.Drawing.Size(604, 20);
            this.txtSequencesFile.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 112);
            this.label1.Margin = new System.Windows.Forms.Padding(5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Analyses:";
            // 
            // lnkAdditionalOptions
            // 
            this.lnkAdditionalOptions.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lnkAdditionalOptions.AutoSize = true;
            this.tblForm.SetColumnSpan(this.lnkAdditionalOptions, 2);
            this.lnkAdditionalOptions.Location = new System.Drawing.Point(5, 89);
            this.lnkAdditionalOptions.Margin = new System.Windows.Forms.Padding(5);
            this.lnkAdditionalOptions.Name = "lnkAdditionalOptions";
            this.lnkAdditionalOptions.Size = new System.Drawing.Size(122, 13);
            this.lnkAdditionalOptions.TabIndex = 8;
            this.lnkAdditionalOptions.TabStop = true;
            this.lnkAdditionalOptions.Text = "Show Additional Options";
            this.lnkAdditionalOptions.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkAdditionalOptions_LinkClicked);
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(5, 64);
            this.label6.Margin = new System.Windows.Forms.Padding(5);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(30, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Title:";
            // 
            // txtTitle
            // 
            this.txtTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tblForm.SetColumnSpan(this.txtTitle, 3);
            this.txtTitle.Location = new System.Drawing.Point(96, 61);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(635, 20);
            this.txtTitle.TabIndex = 7;
            // 
            // lnkCustomControlFile
            // 
            this.lnkCustomControlFile.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lnkCustomControlFile.AutoSize = true;
            this.tblForm.SetColumnSpan(this.lnkCustomControlFile, 2);
            this.lnkCustomControlFile.Location = new System.Drawing.Point(611, 89);
            this.lnkCustomControlFile.Margin = new System.Windows.Forms.Padding(5);
            this.lnkCustomControlFile.Name = "lnkCustomControlFile";
            this.lnkCustomControlFile.Size = new System.Drawing.Size(118, 13);
            this.lnkCustomControlFile.TabIndex = 8;
            this.lnkCustomControlFile.TabStop = true;
            this.lnkCustomControlFile.Text = "Extract from Control File";
            this.lnkCustomControlFile.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkCustomControlFile_LinkClicked);
            // 
            // uctTreeConfiguration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.tblForm);
            this.Name = "uctTreeConfiguration";
            this.Size = new System.Drawing.Size(734, 300);
            this.tblForm.ResumeLayout(false);
            this.tblForm.PerformLayout();
            this.tblControlButtons.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tblForm;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.TextBox txtSequencesFile;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnBrowse_SequencesFile;
        private System.Windows.Forms.Button btnBrowse_TreeFile;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtTreeFile;
        private uctAnalysisConfigurations uctAnalysisConfigurations1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tblControlButtons;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.LinkLabel lnkAdditionalOptions;
        private System.Windows.Forms.LinkLabel lnkCustomControlFile;
    }
}
