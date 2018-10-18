namespace Pilgrimage
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.mnuForm = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.connectToDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.newRecordsetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openRecordsetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openOtherRecordsetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.geneSequencesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.searchGenBankToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bLASTNResultsHistoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sepGeneSequences1 = new System.Windows.Forms.ToolStripSeparator();
            this.viewPRANKAlignmentHistoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewMUSCLEAlignmentHistoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sepGeneSequences2 = new System.Windows.Forms.ToolStripSeparator();
            this.viewPhyMLHistoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectionAnalysesPAMLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewPAMLJobHistoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.manageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recordsetsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.manageRecordsetsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.exportRecordsetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importRecordsetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.subsetsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testEUtilitiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stringTestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutPilgrimageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ilPages = new System.Windows.Forms.ImageList(this.components);
            this.tscForm = new System.Windows.Forms.ToolStripContainer();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsbResolution = new System.Windows.Forms.ToolStripDropDownButton();
            this.fullScreenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.x800ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.x768ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.x600ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tslInProgressBlastNAtNCBIs = new System.Windows.Forms.ToolStripStatusLabel();
            this.tslInProgressPRANKAlignments = new System.Windows.Forms.ToolStripStatusLabel();
            this.tslInProgressMUSCLEAlignments = new System.Windows.Forms.ToolStripStatusLabel();
            this.tslInProgressPhyMLs = new System.Windows.Forms.ToolStripStatusLabel();
            this.tslInProgressCodeMLAnalyses = new System.Windows.Forms.ToolStripStatusLabel();
            this.tbForm = new System.Windows.Forms.TabControl();
            this.pgGenes = new System.Windows.Forms.TabPage();
            this.uctGeneSequencesMain1 = new Pilgrimage.GeneSequences.uctGeneSequencesMain();
            this.pgTreeAnalysis = new System.Windows.Forms.TabPage();
            this.uctPAMLMain1 = new Pilgrimage.PAML.uctPAMLMain();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.taskbarIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.mnuForm.SuspendLayout();
            this.tscForm.BottomToolStripPanel.SuspendLayout();
            this.tscForm.ContentPanel.SuspendLayout();
            this.tscForm.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.tbForm.SuspendLayout();
            this.pgGenes.SuspendLayout();
            this.pgTreeAnalysis.SuspendLayout();
            this.SuspendLayout();
            // 
            // mnuForm
            // 
            this.mnuForm.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.geneSequencesToolStripMenuItem,
            this.selectionAnalysesPAMLToolStripMenuItem,
            this.manageToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.mnuForm.Location = new System.Drawing.Point(0, 0);
            this.mnuForm.Name = "mnuForm";
            this.mnuForm.Size = new System.Drawing.Size(1222, 24);
            this.mnuForm.TabIndex = 0;
            this.mnuForm.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectToDatabaseToolStripMenuItem,
            this.toolStripMenuItem2,
            this.newRecordsetToolStripMenuItem,
            this.openRecordsetToolStripMenuItem,
            this.toolStripMenuItem1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // connectToDatabaseToolStripMenuItem
            // 
            this.connectToDatabaseToolStripMenuItem.Name = "connectToDatabaseToolStripMenuItem";
            this.connectToDatabaseToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.connectToDatabaseToolStripMenuItem.Text = "Connect to Database";
            this.connectToDatabaseToolStripMenuItem.Click += new System.EventHandler(this.connectToDatabaseToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(181, 6);
            // 
            // newRecordsetToolStripMenuItem
            // 
            this.newRecordsetToolStripMenuItem.Image = global::Pilgrimage.Properties.Resources.NewFolder;
            this.newRecordsetToolStripMenuItem.Name = "newRecordsetToolStripMenuItem";
            this.newRecordsetToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.newRecordsetToolStripMenuItem.Text = "&New Project";
            this.newRecordsetToolStripMenuItem.Click += new System.EventHandler(this.newRecordsetToolStripMenuItem_Click);
            // 
            // openRecordsetToolStripMenuItem
            // 
            this.openRecordsetToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openOtherRecordsetToolStripMenuItem});
            this.openRecordsetToolStripMenuItem.Image = global::Pilgrimage.Properties.Resources.OpenFolder;
            this.openRecordsetToolStripMenuItem.Name = "openRecordsetToolStripMenuItem";
            this.openRecordsetToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.openRecordsetToolStripMenuItem.Text = "&Open Recent Project";
            // 
            // openOtherRecordsetToolStripMenuItem
            // 
            this.openOtherRecordsetToolStripMenuItem.Name = "openOtherRecordsetToolStripMenuItem";
            this.openOtherRecordsetToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openOtherRecordsetToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.openOtherRecordsetToolStripMenuItem.Text = "Open...";
            this.openOtherRecordsetToolStripMenuItem.Click += new System.EventHandler(this.openRecordsetToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(181, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // geneSequencesToolStripMenuItem
            // 
            this.geneSequencesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.searchGenBankToolStripMenuItem,
            this.bLASTNResultsHistoryToolStripMenuItem,
            this.sepGeneSequences1,
            this.viewPRANKAlignmentHistoryToolStripMenuItem,
            this.viewMUSCLEAlignmentHistoryToolStripMenuItem,
            this.sepGeneSequences2,
            this.viewPhyMLHistoryToolStripMenuItem});
            this.geneSequencesToolStripMenuItem.Name = "geneSequencesToolStripMenuItem";
            this.geneSequencesToolStripMenuItem.Size = new System.Drawing.Size(105, 20);
            this.geneSequencesToolStripMenuItem.Text = "Gene Sequences";
            // 
            // searchGenBankToolStripMenuItem
            // 
            this.searchGenBankToolStripMenuItem.Image = global::Pilgrimage.Properties.Resources.ncbi_transparent;
            this.searchGenBankToolStripMenuItem.Name = "searchGenBankToolStripMenuItem";
            this.searchGenBankToolStripMenuItem.Size = new System.Drawing.Size(247, 22);
            this.searchGenBankToolStripMenuItem.Text = "Search GenBank";
            this.searchGenBankToolStripMenuItem.Click += new System.EventHandler(this.searchGenBankToolStripMenuItem_Click);
            // 
            // bLASTNResultsHistoryToolStripMenuItem
            // 
            this.bLASTNResultsHistoryToolStripMenuItem.Image = global::Pilgrimage.Properties.Resources.Table;
            this.bLASTNResultsHistoryToolStripMenuItem.Name = "bLASTNResultsHistoryToolStripMenuItem";
            this.bLASTNResultsHistoryToolStripMenuItem.Size = new System.Drawing.Size(247, 22);
            this.bLASTNResultsHistoryToolStripMenuItem.Text = "View BLASTN Results History";
            this.bLASTNResultsHistoryToolStripMenuItem.Click += new System.EventHandler(this.bLASTNResultsHistoryToolStripMenuItem_Click);
            // 
            // sepGeneSequences1
            // 
            this.sepGeneSequences1.Name = "sepGeneSequences1";
            this.sepGeneSequences1.Size = new System.Drawing.Size(244, 6);
            // 
            // viewPRANKAlignmentHistoryToolStripMenuItem
            // 
            this.viewPRANKAlignmentHistoryToolStripMenuItem.Image = global::Pilgrimage.Properties.Resources.ComponentLogo_PRANK_16;
            this.viewPRANKAlignmentHistoryToolStripMenuItem.Name = "viewPRANKAlignmentHistoryToolStripMenuItem";
            this.viewPRANKAlignmentHistoryToolStripMenuItem.Size = new System.Drawing.Size(247, 22);
            this.viewPRANKAlignmentHistoryToolStripMenuItem.Text = "View PRANK Alignment History";
            this.viewPRANKAlignmentHistoryToolStripMenuItem.Click += new System.EventHandler(this.viewPRANKAlignmentHistoryToolStripMenuItem_Click);
            // 
            // viewMUSCLEAlignmentHistoryToolStripMenuItem
            // 
            this.viewMUSCLEAlignmentHistoryToolStripMenuItem.Name = "viewMUSCLEAlignmentHistoryToolStripMenuItem";
            this.viewMUSCLEAlignmentHistoryToolStripMenuItem.Size = new System.Drawing.Size(247, 22);
            this.viewMUSCLEAlignmentHistoryToolStripMenuItem.Text = "View MUSCLE Alignment History";
            this.viewMUSCLEAlignmentHistoryToolStripMenuItem.Click += new System.EventHandler(this.viewMUSCLEAlignmentHistoryToolStripMenuItem_Click);
            // 
            // sepGeneSequences2
            // 
            this.sepGeneSequences2.Name = "sepGeneSequences2";
            this.sepGeneSequences2.Size = new System.Drawing.Size(244, 6);
            // 
            // viewPhyMLHistoryToolStripMenuItem
            // 
            this.viewPhyMLHistoryToolStripMenuItem.Image = global::Pilgrimage.Properties.Resources.ComponentLogo_PhyML_16;
            this.viewPhyMLHistoryToolStripMenuItem.Name = "viewPhyMLHistoryToolStripMenuItem";
            this.viewPhyMLHistoryToolStripMenuItem.Size = new System.Drawing.Size(247, 22);
            this.viewPhyMLHistoryToolStripMenuItem.Text = "View PhyML History";
            this.viewPhyMLHistoryToolStripMenuItem.Click += new System.EventHandler(this.viewPhyMLHistoryToolStripMenuItem_Click);
            // 
            // selectionAnalysesPAMLToolStripMenuItem
            // 
            this.selectionAnalysesPAMLToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewPAMLJobHistoryToolStripMenuItem});
            this.selectionAnalysesPAMLToolStripMenuItem.Name = "selectionAnalysesPAMLToolStripMenuItem";
            this.selectionAnalysesPAMLToolStripMenuItem.Size = new System.Drawing.Size(159, 20);
            this.selectionAnalysesPAMLToolStripMenuItem.Text = "Selection Analyses (PAML)";
            // 
            // viewPAMLJobHistoryToolStripMenuItem
            // 
            this.viewPAMLJobHistoryToolStripMenuItem.Image = global::Pilgrimage.Properties.Resources.Table;
            this.viewPAMLJobHistoryToolStripMenuItem.Name = "viewPAMLJobHistoryToolStripMenuItem";
            this.viewPAMLJobHistoryToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.viewPAMLJobHistoryToolStripMenuItem.Text = "View PAML Job History";
            this.viewPAMLJobHistoryToolStripMenuItem.Click += new System.EventHandler(this.viewPAMLJobHistoryToolStripMenuItem_Click);
            // 
            // manageToolStripMenuItem
            // 
            this.manageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.recordsetsToolStripMenuItem,
            this.subsetsToolStripMenuItem});
            this.manageToolStripMenuItem.Name = "manageToolStripMenuItem";
            this.manageToolStripMenuItem.Size = new System.Drawing.Size(62, 20);
            this.manageToolStripMenuItem.Text = "&Manage";
            // 
            // recordsetsToolStripMenuItem
            // 
            this.recordsetsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.manageRecordsetsToolStripMenuItem,
            this.toolStripMenuItem3,
            this.exportRecordsetToolStripMenuItem,
            this.importRecordsetToolStripMenuItem});
            this.recordsetsToolStripMenuItem.Image = global::Pilgrimage.Properties.Resources.Folder_stuffed;
            this.recordsetsToolStripMenuItem.Name = "recordsetsToolStripMenuItem";
            this.recordsetsToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.recordsetsToolStripMenuItem.Text = "&Projects";
            // 
            // manageRecordsetsToolStripMenuItem
            // 
            this.manageRecordsetsToolStripMenuItem.Image = global::Pilgrimage.Properties.Resources.Folder_stuffed;
            this.manageRecordsetsToolStripMenuItem.Name = "manageRecordsetsToolStripMenuItem";
            this.manageRecordsetsToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.manageRecordsetsToolStripMenuItem.Text = "Manage &Projects";
            this.manageRecordsetsToolStripMenuItem.Click += new System.EventHandler(this.openRecordsetToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(159, 6);
            // 
            // exportRecordsetToolStripMenuItem
            // 
            this.exportRecordsetToolStripMenuItem.Image = global::Pilgrimage.Properties.Resources.Export;
            this.exportRecordsetToolStripMenuItem.Name = "exportRecordsetToolStripMenuItem";
            this.exportRecordsetToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.exportRecordsetToolStripMenuItem.Text = "&Export Project";
            this.exportRecordsetToolStripMenuItem.Click += new System.EventHandler(this.exportRecordsetToolStripMenuItem_Click);
            // 
            // importRecordsetToolStripMenuItem
            // 
            this.importRecordsetToolStripMenuItem.Image = global::Pilgrimage.Properties.Resources.Import;
            this.importRecordsetToolStripMenuItem.Name = "importRecordsetToolStripMenuItem";
            this.importRecordsetToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.importRecordsetToolStripMenuItem.Text = "&Import Project";
            this.importRecordsetToolStripMenuItem.Click += new System.EventHandler(this.importRecordsetToolStripMenuItem_Click);
            // 
            // subsetsToolStripMenuItem
            // 
            this.subsetsToolStripMenuItem.Image = global::Pilgrimage.Properties.Resources.Copy;
            this.subsetsToolStripMenuItem.Name = "subsetsToolStripMenuItem";
            this.subsetsToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.subsetsToolStripMenuItem.Text = "&Datasets";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.testEUtilitiesToolStripMenuItem,
            this.stringTestToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // testEUtilitiesToolStripMenuItem
            // 
            this.testEUtilitiesToolStripMenuItem.Name = "testEUtilitiesToolStripMenuItem";
            this.testEUtilitiesToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.testEUtilitiesToolStripMenuItem.Text = "Test &E-Utilities";
            this.testEUtilitiesToolStripMenuItem.Click += new System.EventHandler(this.testEUtilitiesToolStripMenuItem_Click);
            // 
            // stringTestToolStripMenuItem
            // 
            this.stringTestToolStripMenuItem.Name = "stringTestToolStripMenuItem";
            this.stringTestToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.stringTestToolStripMenuItem.Text = "&String Test";
            this.stringTestToolStripMenuItem.Click += new System.EventHandler(this.stringTestToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutPilgrimageToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutPilgrimageToolStripMenuItem
            // 
            this.aboutPilgrimageToolStripMenuItem.Image = global::Pilgrimage.Properties.Resources.Icon_16;
            this.aboutPilgrimageToolStripMenuItem.Name = "aboutPilgrimageToolStripMenuItem";
            this.aboutPilgrimageToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.aboutPilgrimageToolStripMenuItem.Text = "text set in constructor";
            this.aboutPilgrimageToolStripMenuItem.Click += new System.EventHandler(this.aboutPilgrimageToolStripMenuItem_Click);
            // 
            // ilPages
            // 
            this.ilPages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilPages.ImageStream")));
            this.ilPages.TransparentColor = System.Drawing.Color.Transparent;
            this.ilPages.Images.SetKeyName(0, "Pending");
            this.ilPages.Images.SetKeyName(1, "Final");
            this.ilPages.Images.SetKeyName(2, "Cogs");
            this.ilPages.Images.SetKeyName(3, "NCBI");
            this.ilPages.Images.SetKeyName(4, "Tree");
            // 
            // tscForm
            // 
            // 
            // tscForm.BottomToolStripPanel
            // 
            this.tscForm.BottomToolStripPanel.Controls.Add(this.statusStrip1);
            // 
            // tscForm.ContentPanel
            // 
            this.tscForm.ContentPanel.Controls.Add(this.tbForm);
            this.tscForm.ContentPanel.Size = new System.Drawing.Size(1222, 437);
            this.tscForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tscForm.LeftToolStripPanelVisible = false;
            this.tscForm.Location = new System.Drawing.Point(0, 24);
            this.tscForm.Name = "tscForm";
            this.tscForm.RightToolStripPanelVisible = false;
            this.tscForm.Size = new System.Drawing.Size(1222, 484);
            this.tscForm.TabIndex = 1;
            this.tscForm.Text = "toolStripContainer1";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.tsbResolution,
            this.tslInProgressBlastNAtNCBIs,
            this.tslInProgressPRANKAlignments,
            this.tslInProgressMUSCLEAlignments,
            this.tslInProgressPhyMLs,
            this.tslInProgressCodeMLAnalyses});
            this.statusStrip1.Location = new System.Drawing.Point(0, 0);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1222, 22);
            this.statusStrip1.TabIndex = 0;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(1194, 17);
            this.toolStripStatusLabel1.Spring = true;
            // 
            // tsbResolution
            // 
            this.tsbResolution.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsbResolution.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbResolution.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fullScreenToolStripMenuItem,
            this.x800ToolStripMenuItem,
            this.x768ToolStripMenuItem,
            this.x600ToolStripMenuItem});
            this.tsbResolution.Image = ((System.Drawing.Image)(resources.GetObject("tsbResolution.Image")));
            this.tsbResolution.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbResolution.Name = "tsbResolution";
            this.tsbResolution.Size = new System.Drawing.Size(13, 20);
            // 
            // fullScreenToolStripMenuItem
            // 
            this.fullScreenToolStripMenuItem.Name = "fullScreenToolStripMenuItem";
            this.fullScreenToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.fullScreenToolStripMenuItem.Text = "Full Screen";
            this.fullScreenToolStripMenuItem.Click += new System.EventHandler(this.fullScreenToolStripMenuItem_Click);
            // 
            // x800ToolStripMenuItem
            // 
            this.x800ToolStripMenuItem.Name = "x800ToolStripMenuItem";
            this.x800ToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.x800ToolStripMenuItem.Text = "1200x800";
            this.x800ToolStripMenuItem.Click += new System.EventHandler(this.x800ToolStripMenuItem_Click);
            // 
            // x768ToolStripMenuItem
            // 
            this.x768ToolStripMenuItem.Name = "x768ToolStripMenuItem";
            this.x768ToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.x768ToolStripMenuItem.Text = "1024x768";
            this.x768ToolStripMenuItem.Click += new System.EventHandler(this.x768ToolStripMenuItem_Click);
            // 
            // x600ToolStripMenuItem
            // 
            this.x600ToolStripMenuItem.Name = "x600ToolStripMenuItem";
            this.x600ToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
            this.x600ToolStripMenuItem.Text = "800x600";
            this.x600ToolStripMenuItem.Click += new System.EventHandler(this.x600ToolStripMenuItem_Click);
            // 
            // tslInProgressBlastNAtNCBIs
            // 
            this.tslInProgressBlastNAtNCBIs.IsLink = true;
            this.tslInProgressBlastNAtNCBIs.Name = "tslInProgressBlastNAtNCBIs";
            this.tslInProgressBlastNAtNCBIs.Size = new System.Drawing.Size(0, 17);
            this.tslInProgressBlastNAtNCBIs.Click += new System.EventHandler(this.tslInProgressBlastNAtNCBIs_Click);
            // 
            // tslInProgressPRANKAlignments
            // 
            this.tslInProgressPRANKAlignments.IsLink = true;
            this.tslInProgressPRANKAlignments.Name = "tslInProgressPRANKAlignments";
            this.tslInProgressPRANKAlignments.Size = new System.Drawing.Size(0, 17);
            this.tslInProgressPRANKAlignments.Click += new System.EventHandler(this.tslInProgressPRANKAlignments_Click);
            // 
            // tslInProgressMUSCLEAlignments
            // 
            this.tslInProgressMUSCLEAlignments.IsLink = true;
            this.tslInProgressMUSCLEAlignments.Name = "tslInProgressMUSCLEAlignments";
            this.tslInProgressMUSCLEAlignments.Size = new System.Drawing.Size(0, 17);
            this.tslInProgressMUSCLEAlignments.Click += new System.EventHandler(this.tslInProgressMUSCLEAlignments_Click);
            // 
            // tslInProgressPhyMLs
            // 
            this.tslInProgressPhyMLs.IsLink = true;
            this.tslInProgressPhyMLs.Name = "tslInProgressPhyMLs";
            this.tslInProgressPhyMLs.Size = new System.Drawing.Size(0, 17);
            this.tslInProgressPhyMLs.Click += new System.EventHandler(this.tslInProgressPhyMLs_Click);
            // 
            // tslInProgressCodeMLAnalyses
            // 
            this.tslInProgressCodeMLAnalyses.IsLink = true;
            this.tslInProgressCodeMLAnalyses.Name = "tslInProgressCodeMLAnalyses";
            this.tslInProgressCodeMLAnalyses.Size = new System.Drawing.Size(0, 17);
            this.tslInProgressCodeMLAnalyses.Click += new System.EventHandler(this.tslInProgressCodeMLAnalyses_Click);
            // 
            // tbForm
            // 
            this.tbForm.Controls.Add(this.pgGenes);
            this.tbForm.Controls.Add(this.pgTreeAnalysis);
            this.tbForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbForm.ImageList = this.ilPages;
            this.tbForm.Location = new System.Drawing.Point(0, 0);
            this.tbForm.Name = "tbForm";
            this.tbForm.SelectedIndex = 0;
            this.tbForm.Size = new System.Drawing.Size(1222, 437);
            this.tbForm.TabIndex = 0;
            this.tbForm.Selected += new System.Windows.Forms.TabControlEventHandler(this.tbForm_Selected);
            // 
            // pgGenes
            // 
            this.pgGenes.Controls.Add(this.uctGeneSequencesMain1);
            this.pgGenes.ImageKey = "Final";
            this.pgGenes.Location = new System.Drawing.Point(4, 23);
            this.pgGenes.Name = "pgGenes";
            this.pgGenes.Size = new System.Drawing.Size(1214, 410);
            this.pgGenes.TabIndex = 0;
            this.pgGenes.Text = "Gene Sequences";
            this.pgGenes.UseVisualStyleBackColor = true;
            // 
            // uctGeneSequencesMain1
            // 
            this.uctGeneSequencesMain1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uctGeneSequencesMain1.Location = new System.Drawing.Point(0, 0);
            this.uctGeneSequencesMain1.Name = "uctGeneSequencesMain1";
            this.uctGeneSequencesMain1.Size = new System.Drawing.Size(1214, 410);
            this.uctGeneSequencesMain1.TabIndex = 0;
            // 
            // pgTreeAnalysis
            // 
            this.pgTreeAnalysis.Controls.Add(this.uctPAMLMain1);
            this.pgTreeAnalysis.ImageKey = "Tree";
            this.pgTreeAnalysis.Location = new System.Drawing.Point(4, 23);
            this.pgTreeAnalysis.Name = "pgTreeAnalysis";
            this.pgTreeAnalysis.Size = new System.Drawing.Size(1214, 410);
            this.pgTreeAnalysis.TabIndex = 3;
            this.pgTreeAnalysis.Text = "Selection Analyses (PAML)";
            this.pgTreeAnalysis.UseVisualStyleBackColor = true;
            // 
            // uctPAMLMain1
            // 
            this.uctPAMLMain1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uctPAMLMain1.Location = new System.Drawing.Point(0, 0);
            this.uctPAMLMain1.Name = "uctPAMLMain1";
            this.uctPAMLMain1.Size = new System.Drawing.Size(1214, 410);
            this.uctPAMLMain1.TabIndex = 0;
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "toolStripButton1";
            // 
            // taskbarIcon
            // 
            this.taskbarIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("taskbarIcon.Icon")));
            this.taskbarIcon.Visible = true;
            this.taskbarIcon.BalloonTipClicked += new System.EventHandler(this.taskbarIcon_BalloonTipClicked);
            this.taskbarIcon.BalloonTipClosed += new System.EventHandler(this.taskbarIcon_BalloonTipClosed);
            this.taskbarIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.taskbarIcon_MouseDoubleClick);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1222, 508);
            this.Controls.Add(this.tscForm);
            this.Controls.Add(this.mnuForm);
            this.MainMenuStrip = this.mnuForm;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.Resize += new System.EventHandler(this.frmMain_Resize);
            this.mnuForm.ResumeLayout(false);
            this.mnuForm.PerformLayout();
            this.tscForm.BottomToolStripPanel.ResumeLayout(false);
            this.tscForm.BottomToolStripPanel.PerformLayout();
            this.tscForm.ContentPanel.ResumeLayout(false);
            this.tscForm.ResumeLayout(false);
            this.tscForm.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tbForm.ResumeLayout(false);
            this.pgGenes.ResumeLayout(false);
            this.pgTreeAnalysis.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mnuForm;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newRecordsetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openRecordsetToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripContainer tscForm;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.TabControl tbForm;
        private System.Windows.Forms.TabPage pgGenes;
        private System.Windows.Forms.ToolStripMenuItem openOtherRecordsetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem connectToDatabaseToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ImageList ilPages;
        private System.Windows.Forms.ToolStripMenuItem manageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem recordsetsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem subsetsToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripDropDownButton tsbResolution;
        private System.Windows.Forms.ToolStripMenuItem fullScreenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x768ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x600ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem x800ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importRecordsetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportRecordsetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem manageRecordsetsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testEUtilitiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stringTestToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutPilgrimageToolStripMenuItem;
        private System.Windows.Forms.TabPage pgTreeAnalysis;
        private PAML.uctPAMLMain uctPAMLMain1;
        internal GeneSequences.uctGeneSequencesMain uctGeneSequencesMain1;
        internal System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripMenuItem geneSequencesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem searchGenBankToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bLASTNResultsHistoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator sepGeneSequences1;
        private System.Windows.Forms.ToolStripMenuItem viewPRANKAlignmentHistoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectionAnalysesPAMLToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewPAMLJobHistoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel tslInProgressPRANKAlignments;
        private System.Windows.Forms.ToolStripStatusLabel tslInProgressCodeMLAnalyses;
        private System.Windows.Forms.ToolStripStatusLabel tslInProgressPhyMLs;
        private System.Windows.Forms.ToolStripSeparator sepGeneSequences2;
        private System.Windows.Forms.ToolStripMenuItem viewPhyMLHistoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewMUSCLEAlignmentHistoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel tslInProgressMUSCLEAlignments;
        private System.Windows.Forms.ToolStripStatusLabel tslInProgressBlastNAtNCBIs;
        private System.Windows.Forms.NotifyIcon taskbarIcon;
    }
}

