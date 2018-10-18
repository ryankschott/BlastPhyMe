namespace Pilgrimage.Utilities
{
    partial class frmEUtilitiesTest
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEUtilitiesTest));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.txtURL = new System.Windows.Forms.TextBox();
            this.rbEFetch = new System.Windows.Forms.RadioButton();
            this.txtEFetchID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbDatabase = new System.Windows.Forms.ComboBox();
            this.rbURL = new System.Windows.Forms.RadioButton();
            this.bwRequest = new System.ComponentModel.BackgroundWorker();
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
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.btnCancel, 4, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnSubmit, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtURL, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.rbEFetch, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtEFetchID, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.cmbDatabase, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.rbURL, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(584, 123);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(506, 97);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnSubmit
            // 
            this.btnSubmit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSubmit.Location = new System.Drawing.Point(425, 97);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(75, 23);
            this.btnSubmit.TabIndex = 2;
            this.btnSubmit.Text = "&Submit";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // txtURL
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.txtURL, 4);
            this.txtURL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtURL.Location = new System.Drawing.Point(89, 3);
            this.txtURL.Multiline = true;
            this.txtURL.Name = "txtURL";
            this.txtURL.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtURL.Size = new System.Drawing.Size(492, 61);
            this.txtURL.TabIndex = 1;
            // 
            // rbEFetch
            // 
            this.rbEFetch.AutoSize = true;
            this.rbEFetch.Location = new System.Drawing.Point(5, 72);
            this.rbEFetch.Margin = new System.Windows.Forms.Padding(5);
            this.rbEFetch.Name = "rbEFetch";
            this.rbEFetch.Size = new System.Drawing.Size(76, 17);
            this.rbEFetch.TabIndex = 4;
            this.rbEFetch.Text = "EFetch ID:";
            this.rbEFetch.UseVisualStyleBackColor = true;
            // 
            // txtEFetchID
            // 
            this.txtEFetchID.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEFetchID.Location = new System.Drawing.Point(89, 70);
            this.txtEFetchID.Name = "txtEFetchID";
            this.txtEFetchID.Size = new System.Drawing.Size(150, 20);
            this.txtEFetchID.TabIndex = 5;
            this.txtEFetchID.TextChanged += new System.EventHandler(this.txtEFetchID_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(247, 72);
            this.label2.Margin = new System.Windows.Forms.Padding(5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Database:";
            // 
            // cmbDatabase
            // 
            this.cmbDatabase.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.cmbDatabase, 2);
            this.cmbDatabase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDatabase.FormattingEnabled = true;
            this.cmbDatabase.Location = new System.Drawing.Point(311, 70);
            this.cmbDatabase.Name = "cmbDatabase";
            this.cmbDatabase.Size = new System.Drawing.Size(270, 21);
            this.cmbDatabase.TabIndex = 7;
            // 
            // rbURL
            // 
            this.rbURL.AutoSize = true;
            this.rbURL.Checked = true;
            this.rbURL.Location = new System.Drawing.Point(5, 5);
            this.rbURL.Margin = new System.Windows.Forms.Padding(5);
            this.rbURL.Name = "rbURL";
            this.rbURL.Size = new System.Drawing.Size(50, 17);
            this.rbURL.TabIndex = 4;
            this.rbURL.TabStop = true;
            this.rbURL.Text = "URL:";
            this.rbURL.UseVisualStyleBackColor = true;
            // 
            // bwRequest
            // 
            this.bwRequest.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwRequest_DoWork);
            this.bwRequest.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwRequest_RunWorkerCompleted);
            // 
            // frmEUtilitiesTest
            // 
            this.AcceptButton = this.btnSubmit;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(584, 123);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(450, 120);
            this.Name = "frmEUtilitiesTest";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "E-Utilities Test";
            this.Load += new System.EventHandler(this.frmEUtilitiesTest_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.TextBox txtURL;
        private System.ComponentModel.BackgroundWorker bwRequest;
        private System.Windows.Forms.RadioButton rbEFetch;
        private System.Windows.Forms.TextBox txtEFetchID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbDatabase;
        private System.Windows.Forms.RadioButton rbURL;
    }
}