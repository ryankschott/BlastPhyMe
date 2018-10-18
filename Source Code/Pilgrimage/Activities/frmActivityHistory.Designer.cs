namespace Pilgrimage.Activities
{
    partial class frmActivityHistory<T>
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.grdJobHistory = new System.Windows.Forms.DataGridView();
            this.clmTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmStartedAt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmEndedAt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmOptions = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnClose = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdJobHistory)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.grdJobHistory, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnClose, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(784, 381);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // grdJobHistory
            // 
            this.grdJobHistory.AllowUserToAddRows = false;
            this.grdJobHistory.AllowUserToDeleteRows = false;
            this.grdJobHistory.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grdJobHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdJobHistory.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmTitle,
            this.clmStartedAt,
            this.clmStatus,
            this.clmEndedAt,
            this.clmOptions});
            this.grdJobHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdJobHistory.Location = new System.Drawing.Point(3, 3);
            this.grdJobHistory.Name = "grdJobHistory";
            this.grdJobHistory.ReadOnly = true;
            this.grdJobHistory.RowHeadersVisible = false;
            this.grdJobHistory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdJobHistory.Size = new System.Drawing.Size(778, 346);
            this.grdJobHistory.TabIndex = 1;
            // 
            // clmTitle
            // 
            this.clmTitle.DataPropertyName = "SubSetNameAndSequenceCount";
            this.clmTitle.FillWeight = 35F;
            this.clmTitle.HeaderText = "Input Sequences From";
            this.clmTitle.Name = "clmTitle";
            this.clmTitle.ReadOnly = true;
            // 
            // clmStartedAt
            // 
            this.clmStartedAt.DataPropertyName = "StartedAt";
            this.clmStartedAt.FillWeight = 15F;
            this.clmStartedAt.HeaderText = "Started At";
            this.clmStartedAt.Name = "clmStartedAt";
            this.clmStartedAt.ReadOnly = true;
            // 
            // clmStatus
            // 
            this.clmStatus.DataPropertyName = "Status";
            this.clmStatus.FillWeight = 10F;
            this.clmStatus.HeaderText = "Status";
            this.clmStatus.Name = "clmStatus";
            this.clmStatus.ReadOnly = true;
            // 
            // clmEndedAt
            // 
            this.clmEndedAt.DataPropertyName = "EndedAt";
            this.clmEndedAt.FillWeight = 15F;
            this.clmEndedAt.HeaderText = "Ended At";
            this.clmEndedAt.Name = "clmEndedAt";
            this.clmEndedAt.ReadOnly = true;
            // 
            // clmOptions
            // 
            this.clmOptions.DataPropertyName = "OptionsString";
            this.clmOptions.FillWeight = 25F;
            this.clmOptions.HeaderText = "Options";
            this.clmOptions.Name = "clmOptions";
            this.clmOptions.ReadOnly = true;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(706, 355);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // frmActivityHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(784, 381);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(600, 300);
            this.Name = "frmActivityHistory";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmAlignmentHistory_FormClosing);
            this.Load += new System.EventHandler(this.JobHistoryForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdJobHistory)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridView grdJobHistory;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmTitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmStartedAt;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmEndedAt;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmOptions;
    }
}