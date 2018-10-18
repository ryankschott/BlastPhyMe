namespace Pilgrimage
{
    partial class frmConnectToDatabase
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmConnectToDatabase));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblRecent = new System.Windows.Forms.Label();
            this.tblRecent = new System.Windows.Forms.TableLayoutPanel();
            this.btnNewDatabase = new System.Windows.Forms.Button();
            this.btnOpen = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnBrowse, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtFilePath, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnCancel, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblRecent, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tblRecent, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnNewDatabase, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnOpen, 2, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(494, 81);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 5);
            this.label1.Margin = new System.Windows.Forms.Padding(5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Database:";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(467, 3);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(24, 23);
            this.btnBrowse.TabIndex = 2;
            this.btnBrowse.Text = "...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtFilePath
            // 
            this.txtFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.txtFilePath, 3);
            this.txtFilePath.Location = new System.Drawing.Point(69, 4);
            this.txtFilePath.MaxLength = 250;
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.Size = new System.Drawing.Size(392, 20);
            this.txtFilePath.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.btnCancel, 2);
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(411, 55);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 24);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // lblRecent
            // 
            this.lblRecent.AutoSize = true;
            this.lblRecent.Location = new System.Drawing.Point(5, 34);
            this.lblRecent.Margin = new System.Windows.Forms.Padding(5);
            this.lblRecent.Name = "lblRecent";
            this.lblRecent.Size = new System.Drawing.Size(45, 13);
            this.lblRecent.TabIndex = 3;
            this.lblRecent.Text = "Recent:";
            // 
            // tblRecent
            // 
            this.tblRecent.AutoSize = true;
            this.tblRecent.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tblRecent.ColumnCount = 1;
            this.tableLayoutPanel1.SetColumnSpan(this.tblRecent, 4);
            this.tblRecent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblRecent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblRecent.Location = new System.Drawing.Point(66, 29);
            this.tblRecent.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.tblRecent.Name = "tblRecent";
            this.tblRecent.RowCount = 1;
            this.tblRecent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblRecent.Size = new System.Drawing.Size(428, 18);
            this.tblRecent.TabIndex = 4;
            // 
            // btnNewDatabase
            // 
            this.btnNewDatabase.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tableLayoutPanel1.SetColumnSpan(this.btnNewDatabase, 2);
            this.btnNewDatabase.Location = new System.Drawing.Point(3, 55);
            this.btnNewDatabase.Name = "btnNewDatabase";
            this.btnNewDatabase.Size = new System.Drawing.Size(125, 24);
            this.btnNewDatabase.TabIndex = 5;
            this.btnNewDatabase.Text = "&New Database";
            this.btnNewDatabase.UseVisualStyleBackColor = true;
            this.btnNewDatabase.Click += new System.EventHandler(this.btnNewDatabase_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpen.Location = new System.Drawing.Point(325, 55);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(80, 24);
            this.btnOpen.TabIndex = 6;
            this.btnOpen.Text = "&Open";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // frmConnectToDatabase
            // 
            this.AcceptButton = this.btnOpen;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(494, 81);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(400, 85);
            this.Name = "frmConnectToDatabase";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Connect to Database";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmConnectToDatabase_FormClosing);
            this.Load += new System.EventHandler(this.frmConnectToDatabase_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblRecent;
        private System.Windows.Forms.TableLayoutPanel tblRecent;
        private System.Windows.Forms.Button btnNewDatabase;
    }
}