namespace Pilgrimage.GeneSequences.BlastN
{
    partial class frmBlastNResultsHistory
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBlastNResultsHistory));
            this.tscJobHistory = new System.Windows.Forms.ToolStripContainer();
            this.blastNHistory = new Pilgrimage.uctBlastNAtNCBIJobHistory();
            this.tsJobHistory = new System.Windows.Forms.ToolStrip();
            this.tsbArchiveResults = new System.Windows.Forms.ToolStripButton();
            this.tsbViewAllHistory = new System.Windows.Forms.ToolStripButton();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnClose = new System.Windows.Forms.Button();
            this.tscJobHistory.ContentPanel.SuspendLayout();
            this.tscJobHistory.TopToolStripPanel.SuspendLayout();
            this.tscJobHistory.SuspendLayout();
            this.tsJobHistory.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tscJobHistory
            // 
            this.tscJobHistory.BottomToolStripPanelVisible = false;
            // 
            // tscJobHistory.ContentPanel
            // 
            this.tscJobHistory.ContentPanel.Controls.Add(this.blastNHistory);
            this.tscJobHistory.ContentPanel.Size = new System.Drawing.Size(784, 308);
            this.tscJobHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tscJobHistory.LeftToolStripPanelVisible = false;
            this.tscJobHistory.Location = new System.Drawing.Point(0, 0);
            this.tscJobHistory.Margin = new System.Windows.Forms.Padding(0);
            this.tscJobHistory.Name = "tscJobHistory";
            this.tscJobHistory.RightToolStripPanelVisible = false;
            this.tscJobHistory.Size = new System.Drawing.Size(784, 333);
            this.tscJobHistory.TabIndex = 2;
            this.tscJobHistory.Text = "toolStripContainer1";
            // 
            // tscJobHistory.TopToolStripPanel
            // 
            this.tscJobHistory.TopToolStripPanel.Controls.Add(this.tsJobHistory);
            // 
            // blastNHistory
            // 
            this.blastNHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.blastNHistory.Location = new System.Drawing.Point(0, 0);
            this.blastNHistory.Name = "blastNHistory";
            this.blastNHistory.Size = new System.Drawing.Size(784, 308);
            this.blastNHistory.TabIndex = 0;
            // 
            // tsJobHistory
            // 
            this.tsJobHistory.Dock = System.Windows.Forms.DockStyle.None;
            this.tsJobHistory.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsJobHistory.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbArchiveResults,
            this.tsbViewAllHistory});
            this.tsJobHistory.Location = new System.Drawing.Point(3, 0);
            this.tsJobHistory.Name = "tsJobHistory";
            this.tsJobHistory.Size = new System.Drawing.Size(219, 25);
            this.tsJobHistory.TabIndex = 0;
            // 
            // tsbArchiveResults
            // 
            this.tsbArchiveResults.Image = global::Pilgrimage.Properties.Resources.Folder_stuffed;
            this.tsbArchiveResults.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbArchiveResults.Name = "tsbArchiveResults";
            this.tsbArchiveResults.Size = new System.Drawing.Size(107, 22);
            this.tsbArchiveResults.Text = "Archive Results";
            this.tsbArchiveResults.Click += new System.EventHandler(this.tsbArchiveResults_Click);
            // 
            // tsbViewAllHistory
            // 
            this.tsbViewAllHistory.Image = global::Pilgrimage.Properties.Resources.Copy;
            this.tsbViewAllHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbViewAllHistory.Name = "tsbViewAllHistory";
            this.tsbViewAllHistory.Size = new System.Drawing.Size(109, 22);
            this.tsbViewAllHistory.Text = "View All Results";
            this.tsbViewAllHistory.Click += new System.EventHandler(this.tsbViewAllHistory_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.btnClose, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tscJobHistory, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(784, 362);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(701, 336);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 23);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // frmBlastNResultsHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(784, 362);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmBlastNResultsHistory";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "BLASTN Results History";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmBlastNResultsHistory_FormClosing);
            this.Load += new System.EventHandler(this.frmBlastNResultsHistory_Load);
            this.tscJobHistory.ContentPanel.ResumeLayout(false);
            this.tscJobHistory.TopToolStripPanel.ResumeLayout(false);
            this.tscJobHistory.TopToolStripPanel.PerformLayout();
            this.tscJobHistory.ResumeLayout(false);
            this.tscJobHistory.PerformLayout();
            this.tsJobHistory.ResumeLayout(false);
            this.tsJobHistory.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripContainer tscJobHistory;
        internal uctBlastNAtNCBIJobHistory blastNHistory;
        private System.Windows.Forms.ToolStrip tsJobHistory;
        private System.Windows.Forms.ToolStripButton tsbArchiveResults;
        private System.Windows.Forms.ToolStripButton tsbViewAllHistory;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnClose;
    }
}