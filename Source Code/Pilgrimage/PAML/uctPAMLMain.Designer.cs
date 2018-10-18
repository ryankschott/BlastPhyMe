namespace Pilgrimage.PAML
{
    partial class uctPAMLMain
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
            this.tscForm = new System.Windows.Forms.ToolStripContainer();
            this.tbSubSets = new DraggableTabControl.DraggableTabControl();
            this.tsSubSetActions = new System.Windows.Forms.ToolStrip();
            this.tsbNewSubSet = new System.Windows.Forms.ToolStripButton();
            this.tsbOpenSubSet = new System.Windows.Forms.ToolStripButton();
            this.tsbCloseSubSet = new System.Windows.Forms.ToolStripButton();
            this.tsPAML = new System.Windows.Forms.ToolStrip();
            this.tsbNewPAMLJob = new System.Windows.Forms.ToolStripButton();
            this.tsbJobHistory = new System.Windows.Forms.ToolStripButton();
            this.tsForm = new System.Windows.Forms.ToolStrip();
            this.tsbMoveResults = new System.Windows.Forms.ToolStripButton();
            this.tsbCopyResults = new System.Windows.Forms.ToolStripButton();
            this.tsbDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbExportTo = new System.Windows.Forms.ToolStripDropDownButton();
            this.excelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sepExport = new System.Windows.Forms.ToolStripSeparator();
            this.tsbExportToPilgrimage = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbImportFrom = new System.Windows.Forms.ToolStripDropDownButton();
            this.tsbImportFromPilgrimageDataFile = new System.Windows.Forms.ToolStripMenuItem();
            this.tscForm.ContentPanel.SuspendLayout();
            this.tscForm.TopToolStripPanel.SuspendLayout();
            this.tscForm.SuspendLayout();
            this.tsSubSetActions.SuspendLayout();
            this.tsPAML.SuspendLayout();
            this.tsForm.SuspendLayout();
            this.SuspendLayout();
            // 
            // tscForm
            // 
            // 
            // tscForm.ContentPanel
            // 
            this.tscForm.ContentPanel.Controls.Add(this.tbSubSets);
            this.tscForm.ContentPanel.Size = new System.Drawing.Size(1007, 334);
            this.tscForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tscForm.Location = new System.Drawing.Point(0, 0);
            this.tscForm.Name = "tscForm";
            this.tscForm.Size = new System.Drawing.Size(1007, 409);
            this.tscForm.TabIndex = 0;
            this.tscForm.Text = "toolStripContainer1";
            // 
            // tscForm.TopToolStripPanel
            // 
            this.tscForm.TopToolStripPanel.Controls.Add(this.tsPAML);
            this.tscForm.TopToolStripPanel.Controls.Add(this.tsForm);
            this.tscForm.TopToolStripPanel.Controls.Add(this.tsSubSetActions);
            // 
            // tbSubSets
            // 
            this.tbSubSets.AllowDrop = true;
            this.tbSubSets.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbSubSets.Location = new System.Drawing.Point(0, 0);
            this.tbSubSets.Margin = new System.Windows.Forms.Padding(0);
            this.tbSubSets.Name = "tbSubSets";
            this.tbSubSets.SelectedIndex = 0;
            this.tbSubSets.Size = new System.Drawing.Size(1007, 334);
            this.tbSubSets.TabIndex = 1;
            this.tbSubSets.Selected += new System.Windows.Forms.TabControlEventHandler(this.tbSubSets_Selected);
            // 
            // tsSubSetActions
            // 
            this.tsSubSetActions.Dock = System.Windows.Forms.DockStyle.None;
            this.tsSubSetActions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbNewSubSet,
            this.tsbOpenSubSet,
            this.tsbCloseSubSet});
            this.tsSubSetActions.Location = new System.Drawing.Point(3, 50);
            this.tsSubSetActions.Name = "tsSubSetActions";
            this.tsSubSetActions.Size = new System.Drawing.Size(301, 25);
            this.tsSubSetActions.TabIndex = 0;
            // 
            // tsbNewSubSet
            // 
            this.tsbNewSubSet.Image = global::Pilgrimage.Properties.Resources.New;
            this.tsbNewSubSet.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbNewSubSet.Name = "tsbNewSubSet";
            this.tsbNewSubSet.Size = new System.Drawing.Size(93, 22);
            this.tsbNewSubSet.Text = "New Dataset";
            // 
            // tsbOpenSubSet
            // 
            this.tsbOpenSubSet.Image = global::Pilgrimage.Properties.Resources.Open;
            this.tsbOpenSubSet.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbOpenSubSet.Name = "tsbOpenSubSet";
            this.tsbOpenSubSet.Size = new System.Drawing.Size(98, 22);
            this.tsbOpenSubSet.Text = "Open Dataset";
            // 
            // tsbCloseSubSet
            // 
            this.tsbCloseSubSet.Image = global::Pilgrimage.Properties.Resources.Close;
            this.tsbCloseSubSet.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCloseSubSet.Name = "tsbCloseSubSet";
            this.tsbCloseSubSet.Size = new System.Drawing.Size(98, 22);
            this.tsbCloseSubSet.Text = "Close Dataset";
            // 
            // tsPAML
            // 
            this.tsPAML.Dock = System.Windows.Forms.DockStyle.None;
            this.tsPAML.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbNewPAMLJob,
            this.tsbJobHistory});
            this.tsPAML.Location = new System.Drawing.Point(3, 0);
            this.tsPAML.Name = "tsPAML";
            this.tsPAML.Size = new System.Drawing.Size(240, 25);
            this.tsPAML.TabIndex = 2;
            // 
            // tsbNewPAMLJob
            // 
            this.tsbNewPAMLJob.Image = global::Pilgrimage.Properties.Resources.Gears;
            this.tsbNewPAMLJob.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbNewPAMLJob.Name = "tsbNewPAMLJob";
            this.tsbNewPAMLJob.Size = new System.Drawing.Size(107, 22);
            this.tsbNewPAMLJob.Text = "New PAML Job";
            this.tsbNewPAMLJob.Click += new System.EventHandler(this.tsbNewPAMLJob_Click);
            // 
            // tsbJobHistory
            // 
            this.tsbJobHistory.Image = global::Pilgrimage.Properties.Resources.TextDocument;
            this.tsbJobHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbJobHistory.Name = "tsbJobHistory";
            this.tsbJobHistory.Size = new System.Drawing.Size(121, 22);
            this.tsbJobHistory.Text = "PAML Job History";
            this.tsbJobHistory.Click += new System.EventHandler(this.tsbJobHistory_Click);
            // 
            // tsForm
            // 
            this.tsForm.Dock = System.Windows.Forms.DockStyle.None;
            this.tsForm.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbMoveResults,
            this.tsbCopyResults,
            this.tsbDelete,
            this.toolStripSeparator1,
            this.tsbExportTo,
            this.tsbImportFrom});
            this.tsForm.Location = new System.Drawing.Point(3, 25);
            this.tsForm.Name = "tsForm";
            this.tsForm.Size = new System.Drawing.Size(469, 25);
            this.tsForm.TabIndex = 1;
            // 
            // tsbMoveResults
            // 
            this.tsbMoveResults.Image = global::Pilgrimage.Properties.Resources.Move;
            this.tsbMoveResults.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbMoveResults.Name = "tsbMoveResults";
            this.tsbMoveResults.Size = new System.Drawing.Size(80, 22);
            this.tsbMoveResults.Text = "Move to...";
            this.tsbMoveResults.Click += new System.EventHandler(this.tsbMoveOrCopyResults_Click);
            // 
            // tsbCopyResults
            // 
            this.tsbCopyResults.Image = global::Pilgrimage.Properties.Resources.Copy;
            this.tsbCopyResults.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCopyResults.Name = "tsbCopyResults";
            this.tsbCopyResults.Size = new System.Drawing.Size(78, 22);
            this.tsbCopyResults.Text = "Copy to...";
            this.tsbCopyResults.Click += new System.EventHandler(this.tsbMoveOrCopyResults_Click);
            // 
            // tsbDelete
            // 
            this.tsbDelete.Image = global::Pilgrimage.Properties.Resources.Delete;
            this.tsbDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDelete.Name = "tsbDelete";
            this.tsbDelete.Size = new System.Drawing.Size(60, 22);
            this.tsbDelete.Text = "Delete";
            this.tsbDelete.Click += new System.EventHandler(this.tsbDelete_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbExportTo
            // 
            this.tsbExportTo.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.excelToolStripMenuItem,
            this.sepExport,
            this.tsbExportToPilgrimage});
            this.tsbExportTo.Image = global::Pilgrimage.Properties.Resources.Export;
            this.tsbExportTo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbExportTo.Name = "tsbExportTo";
            this.tsbExportTo.Size = new System.Drawing.Size(92, 22);
            this.tsbExportTo.Text = "Export to...";
            // 
            // excelToolStripMenuItem
            // 
            this.excelToolStripMenuItem.Image = global::Pilgrimage.Properties.Resources.ExcelDocument;
            this.excelToolStripMenuItem.Name = "excelToolStripMenuItem";
            this.excelToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.excelToolStripMenuItem.Text = "&Excel";
            this.excelToolStripMenuItem.Click += new System.EventHandler(this.excelToolStripMenuItem_Click);
            // 
            // sepExport
            // 
            this.sepExport.Name = "sepExport";
            this.sepExport.Size = new System.Drawing.Size(176, 6);
            // 
            // tsbExportToPilgrimage
            // 
            this.tsbExportToPilgrimage.Image = global::Pilgrimage.Properties.Resources.Icon_16;
            this.tsbExportToPilgrimage.Name = "tsbExportToPilgrimage";
            this.tsbExportToPilgrimage.Size = new System.Drawing.Size(179, 22);
            this.tsbExportToPilgrimage.Text = "text set in constructor";
            this.tsbExportToPilgrimage.Click += new System.EventHandler(this.tsbExportToPilgrimage_Click);
            // 
            // tsbImportFrom
            // 
            this.tsbImportFrom.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbImportFromPilgrimageDataFile});
            this.tsbImportFrom.Image = global::Pilgrimage.Properties.Resources.Import;
            this.tsbImportFrom.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbImportFrom.Name = "tsbImportFrom";
            this.tsbImportFrom.Size = new System.Drawing.Size(110, 22);
            this.tsbImportFrom.Text = "Import from...";
            // 
            // tsbImportFromPilgrimageDataFile
            // 
            this.tsbImportFromPilgrimageDataFile.Image = global::Pilgrimage.Properties.Resources.Icon_16;
            this.tsbImportFromPilgrimageDataFile.Name = "tsbImportFromPilgrimageDataFile";
            this.tsbImportFromPilgrimageDataFile.Size = new System.Drawing.Size(179, 22);
            this.tsbImportFromPilgrimageDataFile.Text = "text set in constructor";
            // 
            // uctPAMLMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tscForm);
            this.Name = "uctPAMLMain";
            this.Size = new System.Drawing.Size(1007, 409);
            this.tscForm.ContentPanel.ResumeLayout(false);
            this.tscForm.TopToolStripPanel.ResumeLayout(false);
            this.tscForm.TopToolStripPanel.PerformLayout();
            this.tscForm.ResumeLayout(false);
            this.tscForm.PerformLayout();
            this.tsSubSetActions.ResumeLayout(false);
            this.tsSubSetActions.PerformLayout();
            this.tsPAML.ResumeLayout(false);
            this.tsPAML.PerformLayout();
            this.tsForm.ResumeLayout(false);
            this.tsForm.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripContainer tscForm;
        private System.Windows.Forms.ToolStrip tsSubSetActions;
        private System.Windows.Forms.ToolStripDropDownButton tsbExportTo;
        private System.Windows.Forms.ToolStripMenuItem excelToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton tsbJobHistory;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsbNewPAMLJob;
        private System.Windows.Forms.ToolStrip tsForm;
        private System.Windows.Forms.ToolStripButton tsbDelete;
        internal DraggableTabControl.DraggableTabControl tbSubSets;
        private System.Windows.Forms.ToolStripButton tsbNewSubSet;
        private System.Windows.Forms.ToolStripButton tsbOpenSubSet;
        private System.Windows.Forms.ToolStripButton tsbCloseSubSet;
        private System.Windows.Forms.ToolStripButton tsbMoveResults;
        private System.Windows.Forms.ToolStripButton tsbCopyResults;
        private System.Windows.Forms.ToolStrip tsPAML;
        private System.Windows.Forms.ToolStripMenuItem tsbExportToPilgrimage;
        private System.Windows.Forms.ToolStripDropDownButton tsbImportFrom;
        private System.Windows.Forms.ToolStripMenuItem tsbImportFromPilgrimageDataFile;
        private System.Windows.Forms.ToolStripSeparator sepExport;
    }
}
