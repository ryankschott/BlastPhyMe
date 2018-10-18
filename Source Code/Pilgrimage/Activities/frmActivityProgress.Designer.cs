namespace Pilgrimage.Activities
{
    partial class frmActivityProgress
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmActivityProgress));
            this.tblForm = new System.Windows.Forms.TableLayoutPanel();
            this.txtProgress = new System.Windows.Forms.TextBox();
            this.pbCurrent = new System.Windows.Forms.ProgressBar();
            this.lblCurrent = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.pbTotal = new System.Windows.Forms.ProgressBar();
            this.lblTotalProgress = new System.Windows.Forms.Label();
            this.tblForm.SuspendLayout();
            this.SuspendLayout();
            // 
            // tblForm
            // 
            this.tblForm.AutoSize = true;
            this.tblForm.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tblForm.ColumnCount = 2;
            this.tblForm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblForm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tblForm.Controls.Add(this.txtProgress, 0, 0);
            this.tblForm.Controls.Add(this.pbCurrent, 0, 3);
            this.tblForm.Controls.Add(this.lblCurrent, 0, 2);
            this.tblForm.Controls.Add(this.lblStatus, 0, 1);
            this.tblForm.Controls.Add(this.btnCancel, 0, 6);
            this.tblForm.Controls.Add(this.btnClose, 1, 6);
            this.tblForm.Controls.Add(this.pbTotal, 0, 5);
            this.tblForm.Controls.Add(this.lblTotalProgress, 0, 4);
            this.tblForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblForm.Location = new System.Drawing.Point(0, 0);
            this.tblForm.Name = "tblForm";
            this.tblForm.RowCount = 7;
            this.tblForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblForm.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblForm.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblForm.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblForm.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblForm.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblForm.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblForm.Size = new System.Drawing.Size(384, 466);
            this.tblForm.TabIndex = 1;
            // 
            // txtProgress
            // 
            this.tblForm.SetColumnSpan(this.txtProgress, 2);
            this.txtProgress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtProgress.Location = new System.Drawing.Point(3, 3);
            this.txtProgress.Multiline = true;
            this.txtProgress.Name = "txtProgress";
            this.txtProgress.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtProgress.Size = new System.Drawing.Size(378, 303);
            this.txtProgress.TabIndex = 1;
            // 
            // pbCurrent
            // 
            this.pbCurrent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tblForm.SetColumnSpan(this.pbCurrent, 2);
            this.pbCurrent.Location = new System.Drawing.Point(3, 358);
            this.pbCurrent.Name = "pbCurrent";
            this.pbCurrent.Size = new System.Drawing.Size(378, 23);
            this.pbCurrent.TabIndex = 3;
            // 
            // lblCurrent
            // 
            this.lblCurrent.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblCurrent.AutoSize = true;
            this.lblCurrent.Location = new System.Drawing.Point(5, 337);
            this.lblCurrent.Margin = new System.Windows.Forms.Padding(5);
            this.lblCurrent.Name = "lblCurrent";
            this.lblCurrent.Size = new System.Drawing.Size(88, 13);
            this.lblCurrent.TabIndex = 6;
            this.lblCurrent.Text = "Current Progress:";
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblStatus.AutoSize = true;
            this.tblForm.SetColumnSpan(this.lblStatus, 2);
            this.lblStatus.Location = new System.Drawing.Point(5, 314);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(5);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(37, 13);
            this.lblStatus.TabIndex = 6;
            this.lblStatus.Text = "Status";
            this.lblStatus.Visible = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnCancel.Location = new System.Drawing.Point(3, 439);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(110, 24);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "&Cancel Job";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Ignore;
            this.btnClose.Location = new System.Drawing.Point(301, 439);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 24);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // pbTotal
            // 
            this.pbTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tblForm.SetColumnSpan(this.pbTotal, 2);
            this.pbTotal.Location = new System.Drawing.Point(3, 410);
            this.pbTotal.Name = "pbTotal";
            this.pbTotal.Size = new System.Drawing.Size(378, 23);
            this.pbTotal.TabIndex = 3;
            // 
            // lblTotalProgress
            // 
            this.lblTotalProgress.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblTotalProgress.AutoSize = true;
            this.lblTotalProgress.Location = new System.Drawing.Point(5, 389);
            this.lblTotalProgress.Margin = new System.Windows.Forms.Padding(5);
            this.lblTotalProgress.Name = "lblTotalProgress";
            this.lblTotalProgress.Size = new System.Drawing.Size(78, 13);
            this.lblTotalProgress.TabIndex = 6;
            this.lblTotalProgress.Text = "Total Progress:";
            // 
            // frmActivityProgress
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 466);
            this.Controls.Add(this.tblForm);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(350, 350);
            this.Name = "frmActivityProgress";
            this.Text = "In progress...";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmJobProgress_FormClosing);
            this.Load += new System.EventHandler(this.frmJobProgress_Load);
            this.tblForm.ResumeLayout(false);
            this.tblForm.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tblForm;
        private System.Windows.Forms.TextBox txtProgress;
        private System.Windows.Forms.ProgressBar pbCurrent;
        private System.Windows.Forms.Label lblCurrent;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ProgressBar pbTotal;
        private System.Windows.Forms.Label lblTotalProgress;
    }
}