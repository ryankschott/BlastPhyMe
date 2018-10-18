namespace Pilgrimage.GeneSequences
{
    partial class uctGeneSequencesMain
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
            this.tssGeneActions = new System.Windows.Forms.ToolStripSeparator();
            this.tsbSearchGenBank = new System.Windows.Forms.ToolStripButton();
            this.tsForm = new System.Windows.Forms.ToolStrip();
            this.tsbMoveSelected = new System.Windows.Forms.ToolStripButton();
            this.tsbCopySelected = new System.Windows.Forms.ToolStripButton();
            this.tsbDeleteGene = new System.Windows.Forms.ToolStripButton();
            this.tssGeneActions1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbExportTo = new System.Windows.Forms.ToolStripDropDownButton();
            this.tsbExportToMEGA = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbExportToFASTA = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbExportToNEXUS = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbExportToPHYLIP = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbExportToExcel = new System.Windows.Forms.ToolStripMenuItem();
            this.sepExport = new System.Windows.Forms.ToolStripSeparator();
            this.tsbExportToPilgrimage = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbImportFrom = new System.Windows.Forms.ToolStripDropDownButton();
            this.tsbImportFromFASTAFile = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbImportFromNEXUSFile = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbImportFromPHYLIPFile = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbImportFromAlignmentInMEGA = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbImportFromBLASTNExeOutput = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbImportFromTrinityFASTAFile = new System.Windows.Forms.ToolStripMenuItem();
            this.sepImport = new System.Windows.Forms.ToolStripSeparator();
            this.tsbImportFromPilgrimageDataFile = new System.Windows.Forms.ToolStripMenuItem();
            this.tsGeneActions = new System.Windows.Forms.ToolStrip();
            this.tsbSetGeneName = new System.Windows.Forms.ToolStripButton();
            this.tsbBlastNSelectedSequences = new System.Windows.Forms.ToolStripButton();
            this.tsbUpdateFromGenBank = new System.Windows.Forms.ToolStripButton();
            this.tsbAnnotateFromBLASTNCBI = new System.Windows.Forms.ToolStripButton();
            this.tsbAlignWith = new System.Windows.Forms.ToolStripDropDownButton();
            this.tsbAlignWithPRANK = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbAlignWithMUSCLE = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbGenerateTreeWithPhyML = new System.Windows.Forms.ToolStripButton();
            this.tsbExportToPAML = new System.Windows.Forms.ToolStripMenuItem();
            this.tscForm.ContentPanel.SuspendLayout();
            this.tscForm.TopToolStripPanel.SuspendLayout();
            this.tscForm.SuspendLayout();
            this.tsSubSetActions.SuspendLayout();
            this.tsForm.SuspendLayout();
            this.tsGeneActions.SuspendLayout();
            this.SuspendLayout();
            // 
            // tscForm
            // 
            this.tscForm.BottomToolStripPanelVisible = false;
            // 
            // tscForm.ContentPanel
            // 
            this.tscForm.ContentPanel.Controls.Add(this.tbSubSets);
            this.tscForm.ContentPanel.Size = new System.Drawing.Size(1200, 415);
            this.tscForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tscForm.LeftToolStripPanelVisible = false;
            this.tscForm.Location = new System.Drawing.Point(0, 0);
            this.tscForm.Name = "tscForm";
            this.tscForm.RightToolStripPanelVisible = false;
            this.tscForm.Size = new System.Drawing.Size(1200, 490);
            this.tscForm.TabIndex = 2;
            this.tscForm.Text = "toolStripContainer1";
            // 
            // tscForm.TopToolStripPanel
            // 
            this.tscForm.TopToolStripPanel.Controls.Add(this.tsSubSetActions);
            this.tscForm.TopToolStripPanel.Controls.Add(this.tsForm);
            this.tscForm.TopToolStripPanel.Controls.Add(this.tsGeneActions);
            // 
            // tbSubSets
            // 
            this.tbSubSets.AllowDrop = true;
            this.tbSubSets.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbSubSets.Location = new System.Drawing.Point(0, 0);
            this.tbSubSets.Margin = new System.Windows.Forms.Padding(0);
            this.tbSubSets.Name = "tbSubSets";
            this.tbSubSets.SelectedIndex = 0;
            this.tbSubSets.Size = new System.Drawing.Size(1200, 415);
            this.tbSubSets.TabIndex = 0;
            this.tbSubSets.Selected += new System.Windows.Forms.TabControlEventHandler(this.tbSubSets_Selected);
            // 
            // tsSubSetActions
            // 
            this.tsSubSetActions.Dock = System.Windows.Forms.DockStyle.None;
            this.tsSubSetActions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbNewSubSet,
            this.tsbOpenSubSet,
            this.tsbCloseSubSet,
            this.tssGeneActions,
            this.tsbSearchGenBank});
            this.tsSubSetActions.Location = new System.Drawing.Point(3, 0);
            this.tsSubSetActions.Name = "tsSubSetActions";
            this.tsSubSetActions.Size = new System.Drawing.Size(419, 25);
            this.tsSubSetActions.TabIndex = 1;
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
            // tssGeneActions
            // 
            this.tssGeneActions.Name = "tssGeneActions";
            this.tssGeneActions.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbSearchGenBank
            // 
            this.tsbSearchGenBank.Image = global::Pilgrimage.Properties.Resources.ncbi_transparent;
            this.tsbSearchGenBank.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSearchGenBank.Name = "tsbSearchGenBank";
            this.tsbSearchGenBank.Size = new System.Drawing.Size(112, 22);
            this.tsbSearchGenBank.Text = "Search GenBank";
            this.tsbSearchGenBank.Click += new System.EventHandler(this.tsbSearchGenBank_Click);
            // 
            // tsForm
            // 
            this.tsForm.Dock = System.Windows.Forms.DockStyle.None;
            this.tsForm.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbMoveSelected,
            this.tsbCopySelected,
            this.tsbDeleteGene,
            this.tssGeneActions1,
            this.tsbExportTo,
            this.tsbImportFrom});
            this.tsForm.Location = new System.Drawing.Point(3, 25);
            this.tsForm.Name = "tsForm";
            this.tsForm.Size = new System.Drawing.Size(469, 25);
            this.tsForm.TabIndex = 0;
            // 
            // tsbMoveSelected
            // 
            this.tsbMoveSelected.Image = global::Pilgrimage.Properties.Resources.Move;
            this.tsbMoveSelected.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbMoveSelected.Name = "tsbMoveSelected";
            this.tsbMoveSelected.Size = new System.Drawing.Size(80, 22);
            this.tsbMoveSelected.Text = "Move to...";
            this.tsbMoveSelected.Click += new System.EventHandler(this.tsbMoveOrCopySelected_Click);
            // 
            // tsbCopySelected
            // 
            this.tsbCopySelected.Image = global::Pilgrimage.Properties.Resources.Copy;
            this.tsbCopySelected.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCopySelected.Name = "tsbCopySelected";
            this.tsbCopySelected.Size = new System.Drawing.Size(78, 22);
            this.tsbCopySelected.Text = "Copy to...";
            this.tsbCopySelected.Click += new System.EventHandler(this.tsbMoveOrCopySelected_Click);
            // 
            // tsbDeleteGene
            // 
            this.tsbDeleteGene.Image = global::Pilgrimage.Properties.Resources.Delete;
            this.tsbDeleteGene.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDeleteGene.Name = "tsbDeleteGene";
            this.tsbDeleteGene.Size = new System.Drawing.Size(60, 22);
            this.tsbDeleteGene.Text = "Delete";
            this.tsbDeleteGene.Click += new System.EventHandler(this.tsbDeleteGene_Click);
            // 
            // tssGeneActions1
            // 
            this.tssGeneActions1.Name = "tssGeneActions1";
            this.tssGeneActions1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbExportTo
            // 
            this.tsbExportTo.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbExportToMEGA,
            this.tsbExportToFASTA,
            this.tsbExportToNEXUS,
            this.tsbExportToPHYLIP,
            this.tsbExportToPAML,
            this.tsbExportToExcel,
            this.sepExport,
            this.tsbExportToPilgrimage});
            this.tsbExportTo.Image = global::Pilgrimage.Properties.Resources.Export;
            this.tsbExportTo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbExportTo.Name = "tsbExportTo";
            this.tsbExportTo.Size = new System.Drawing.Size(92, 22);
            this.tsbExportTo.Text = "Export to...";
            // 
            // tsbExportToMEGA
            // 
            this.tsbExportToMEGA.Image = global::Pilgrimage.Properties.Resources.ComponentLogo_MEGA6_48;
            this.tsbExportToMEGA.Name = "tsbExportToMEGA";
            this.tsbExportToMEGA.Size = new System.Drawing.Size(186, 22);
            this.tsbExportToMEGA.Text = "MEGA";
            this.tsbExportToMEGA.Click += new System.EventHandler(this.tsbExportToMEGA_Click);
            // 
            // tsbExportToFASTA
            // 
            this.tsbExportToFASTA.Image = global::Pilgrimage.Properties.Resources.TextDocument;
            this.tsbExportToFASTA.Name = "tsbExportToFASTA";
            this.tsbExportToFASTA.Size = new System.Drawing.Size(186, 22);
            this.tsbExportToFASTA.Text = "FASTA";
            this.tsbExportToFASTA.Click += new System.EventHandler(this.tsbExportToFASTA_Click);
            // 
            // tsbExportToNEXUS
            // 
            this.tsbExportToNEXUS.Image = global::Pilgrimage.Properties.Resources.TextDocument;
            this.tsbExportToNEXUS.Name = "tsbExportToNEXUS";
            this.tsbExportToNEXUS.Size = new System.Drawing.Size(186, 22);
            this.tsbExportToNEXUS.Text = "NEXUS";
            this.tsbExportToNEXUS.Click += new System.EventHandler(this.tsbExportToNEXUS_Click);
            // 
            // tsbExportToPHYLIP
            // 
            this.tsbExportToPHYLIP.Image = global::Pilgrimage.Properties.Resources.ComponentLogo_PHYLIP_16;
            this.tsbExportToPHYLIP.Name = "tsbExportToPHYLIP";
            this.tsbExportToPHYLIP.Size = new System.Drawing.Size(186, 22);
            this.tsbExportToPHYLIP.Text = "PHYLIP";
            this.tsbExportToPHYLIP.Click += new System.EventHandler(this.tsbExportToPHYLIP_Click);
            // 
            // tsbExportToExcel
            // 
            this.tsbExportToExcel.Image = global::Pilgrimage.Properties.Resources.ExcelDocument;
            this.tsbExportToExcel.Name = "tsbExportToExcel";
            this.tsbExportToExcel.Size = new System.Drawing.Size(186, 22);
            this.tsbExportToExcel.Text = "Excel";
            this.tsbExportToExcel.Click += new System.EventHandler(this.tsbExportToExcel_Click);
            // 
            // sepExport
            // 
            this.sepExport.Name = "sepExport";
            this.sepExport.Size = new System.Drawing.Size(183, 6);
            // 
            // tsbExportToPilgrimage
            // 
            this.tsbExportToPilgrimage.Image = global::Pilgrimage.Properties.Resources.Icon_16;
            this.tsbExportToPilgrimage.Name = "tsbExportToPilgrimage";
            this.tsbExportToPilgrimage.Size = new System.Drawing.Size(186, 22);
            this.tsbExportToPilgrimage.Text = "text set in constructor";
            this.tsbExportToPilgrimage.Click += new System.EventHandler(this.tsbExportToPilgrimage_Click);
            // 
            // tsbImportFrom
            // 
            this.tsbImportFrom.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbImportFromFASTAFile,
            this.tsbImportFromNEXUSFile,
            this.tsbImportFromPHYLIPFile,
            this.tsbImportFromAlignmentInMEGA,
            this.tsbImportFromBLASTNExeOutput,
            this.tsbImportFromTrinityFASTAFile,
            this.sepImport,
            this.tsbImportFromPilgrimageDataFile});
            this.tsbImportFrom.Image = global::Pilgrimage.Properties.Resources.Import;
            this.tsbImportFrom.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbImportFrom.Name = "tsbImportFrom";
            this.tsbImportFrom.Size = new System.Drawing.Size(110, 22);
            this.tsbImportFrom.Text = "Import from...";
            this.tsbImportFrom.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.tsbImportFrom_DropDownItemClicked);
            this.tsbImportFrom.Click += new System.EventHandler(this.tsbImportFrom_Click);
            // 
            // tsbImportFromFASTAFile
            // 
            this.tsbImportFromFASTAFile.Image = global::Pilgrimage.Properties.Resources.TextDocument;
            this.tsbImportFromFASTAFile.Name = "tsbImportFromFASTAFile";
            this.tsbImportFromFASTAFile.Size = new System.Drawing.Size(187, 22);
            this.tsbImportFromFASTAFile.Text = "FASTA file";
            this.tsbImportFromFASTAFile.Click += new System.EventHandler(this.tsbImportFromFASTA_Click);
            // 
            // tsbImportFromNEXUSFile
            // 
            this.tsbImportFromNEXUSFile.Image = global::Pilgrimage.Properties.Resources.TextDocument;
            this.tsbImportFromNEXUSFile.Name = "tsbImportFromNEXUSFile";
            this.tsbImportFromNEXUSFile.Size = new System.Drawing.Size(187, 22);
            this.tsbImportFromNEXUSFile.Text = "NEXUS file";
            this.tsbImportFromNEXUSFile.Click += new System.EventHandler(this.tsbImportFromNEXUSFile_Click);
            // 
            // tsbImportFromPHYLIPFile
            // 
            this.tsbImportFromPHYLIPFile.Image = global::Pilgrimage.Properties.Resources.ComponentLogo_PHYLIP_16;
            this.tsbImportFromPHYLIPFile.Name = "tsbImportFromPHYLIPFile";
            this.tsbImportFromPHYLIPFile.Size = new System.Drawing.Size(187, 22);
            this.tsbImportFromPHYLIPFile.Text = "PHYLIP";
            this.tsbImportFromPHYLIPFile.Click += new System.EventHandler(this.tsbImportFromPHYLIPFile_Click);
            // 
            // tsbImportFromAlignmentInMEGA
            // 
            this.tsbImportFromAlignmentInMEGA.Image = global::Pilgrimage.Properties.Resources.ComponentLogo_MEGA6_48;
            this.tsbImportFromAlignmentInMEGA.Name = "tsbImportFromAlignmentInMEGA";
            this.tsbImportFromAlignmentInMEGA.Size = new System.Drawing.Size(187, 22);
            this.tsbImportFromAlignmentInMEGA.Text = "Alignment in MEGA";
            this.tsbImportFromAlignmentInMEGA.Click += new System.EventHandler(this.tsbImportFromAlignment_Click);
            // 
            // tsbImportFromBLASTNExeOutput
            // 
            this.tsbImportFromBLASTNExeOutput.Image = global::Pilgrimage.Properties.Resources.NCBI;
            this.tsbImportFromBLASTNExeOutput.Name = "tsbImportFromBLASTNExeOutput";
            this.tsbImportFromBLASTNExeOutput.Size = new System.Drawing.Size(187, 22);
            this.tsbImportFromBLASTNExeOutput.Text = "BLASTN.exe Output";
            this.tsbImportFromBLASTNExeOutput.Click += new System.EventHandler(this.tsbImportFromBLASTNExeOutput_Click);
            // 
            // tsbImportFromTrinityFASTAFile
            // 
            this.tsbImportFromTrinityFASTAFile.Image = global::Pilgrimage.Properties.Resources.ComponentLogo_Trinity_16;
            this.tsbImportFromTrinityFASTAFile.Name = "tsbImportFromTrinityFASTAFile";
            this.tsbImportFromTrinityFASTAFile.Size = new System.Drawing.Size(187, 22);
            this.tsbImportFromTrinityFASTAFile.Text = "Trinity FASTA Output";
            this.tsbImportFromTrinityFASTAFile.Click += new System.EventHandler(this.tsbImportFromTrinityFASTAFile_Click);
            // 
            // sepImport
            // 
            this.sepImport.Name = "sepImport";
            this.sepImport.Size = new System.Drawing.Size(184, 6);
            // 
            // tsbImportFromPilgrimageDataFile
            // 
            this.tsbImportFromPilgrimageDataFile.Image = global::Pilgrimage.Properties.Resources.Icon_16;
            this.tsbImportFromPilgrimageDataFile.Name = "tsbImportFromPilgrimageDataFile";
            this.tsbImportFromPilgrimageDataFile.Size = new System.Drawing.Size(187, 22);
            this.tsbImportFromPilgrimageDataFile.Text = "text set in constructor";
            // 
            // tsGeneActions
            // 
            this.tsGeneActions.Dock = System.Windows.Forms.DockStyle.None;
            this.tsGeneActions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbSetGeneName,
            this.tsbBlastNSelectedSequences,
            this.tsbUpdateFromGenBank,
            this.tsbAnnotateFromBLASTNCBI,
            this.tsbAlignWith,
            this.tsbGenerateTreeWithPhyML});
            this.tsGeneActions.Location = new System.Drawing.Point(3, 50);
            this.tsGeneActions.Name = "tsGeneActions";
            this.tsGeneActions.Size = new System.Drawing.Size(878, 25);
            this.tsGeneActions.TabIndex = 2;
            // 
            // tsbSetGeneName
            // 
            this.tsbSetGeneName.Image = global::Pilgrimage.Properties.Resources.Rename;
            this.tsbSetGeneName.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSetGeneName.Name = "tsbSetGeneName";
            this.tsbSetGeneName.Size = new System.Drawing.Size(108, 22);
            this.tsbSetGeneName.Text = "Set Gene Name";
            this.tsbSetGeneName.Click += new System.EventHandler(this.tsbSetGeneName_Click);
            // 
            // tsbBlastNSelectedSequences
            // 
            this.tsbBlastNSelectedSequences.Image = global::Pilgrimage.Properties.Resources.ncbi_transparent;
            this.tsbBlastNSelectedSequences.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbBlastNSelectedSequences.Name = "tsbBlastNSelectedSequences";
            this.tsbBlastNSelectedSequences.Size = new System.Drawing.Size(177, 22);
            this.tsbBlastNSelectedSequences.Text = "BLAST for Similar Sequences";
            this.tsbBlastNSelectedSequences.Click += new System.EventHandler(this.tsbBlastNSelectedSequences_Click);
            // 
            // tsbUpdateFromGenBank
            // 
            this.tsbUpdateFromGenBank.Image = global::Pilgrimage.Properties.Resources.Synchronize;
            this.tsbUpdateFromGenBank.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbUpdateFromGenBank.Name = "tsbUpdateFromGenBank";
            this.tsbUpdateFromGenBank.Size = new System.Drawing.Size(144, 22);
            this.tsbUpdateFromGenBank.Text = "Update from GenBank";
            this.tsbUpdateFromGenBank.Click += new System.EventHandler(this.tsbUpdateFromGenBank_Click);
            // 
            // tsbAnnotateFromBLASTNCBI
            // 
            this.tsbAnnotateFromBLASTNCBI.Image = global::Pilgrimage.Properties.Resources.ncbi_transparent;
            this.tsbAnnotateFromBLASTNCBI.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAnnotateFromBLASTNCBI.Name = "tsbAnnotateFromBLASTNCBI";
            this.tsbAnnotateFromBLASTNCBI.Size = new System.Drawing.Size(172, 22);
            this.tsbAnnotateFromBLASTNCBI.Text = "Annotate from BLAST NCBI";
            this.tsbAnnotateFromBLASTNCBI.Click += new System.EventHandler(this.tsbAnnotateFromBLASTNCBI_Click);
            // 
            // tsbAlignWith
            // 
            this.tsbAlignWith.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbAlignWithPRANK,
            this.tsbAlignWithMUSCLE});
            this.tsbAlignWith.Image = global::Pilgrimage.Properties.Resources.Ruler;
            this.tsbAlignWith.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAlignWith.Name = "tsbAlignWith";
            this.tsbAlignWith.Size = new System.Drawing.Size(99, 22);
            this.tsbAlignWith.Text = "Align with...";
            // 
            // tsbAlignWithPRANK
            // 
            this.tsbAlignWithPRANK.Image = global::Pilgrimage.Properties.Resources.ComponentLogo_PRANK_16;
            this.tsbAlignWithPRANK.Name = "tsbAlignWithPRANK";
            this.tsbAlignWithPRANK.Size = new System.Drawing.Size(119, 22);
            this.tsbAlignWithPRANK.Text = "PRANK";
            this.tsbAlignWithPRANK.Click += new System.EventHandler(this.tsbAlignWithPRANK_Click);
            // 
            // tsbAlignWithMUSCLE
            // 
            this.tsbAlignWithMUSCLE.Name = "tsbAlignWithMUSCLE";
            this.tsbAlignWithMUSCLE.Size = new System.Drawing.Size(119, 22);
            this.tsbAlignWithMUSCLE.Text = "MUSCLE";
            this.tsbAlignWithMUSCLE.Click += new System.EventHandler(this.tsbAlignWithMUSCLE_Click);
            // 
            // tsbGenerateTreeWithPhyML
            // 
            this.tsbGenerateTreeWithPhyML.Image = global::Pilgrimage.Properties.Resources.ComponentLogo_PhyML_16;
            this.tsbGenerateTreeWithPhyML.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbGenerateTreeWithPhyML.Name = "tsbGenerateTreeWithPhyML";
            this.tsbGenerateTreeWithPhyML.Size = new System.Drawing.Size(166, 22);
            this.tsbGenerateTreeWithPhyML.Text = "Generate Tree with PhyML";
            this.tsbGenerateTreeWithPhyML.Click += new System.EventHandler(this.tsbGenerateTreeWithPhyML_Click);
            // 
            // tsbExportToPAML
            // 
            this.tsbExportToPAML.Image = global::Pilgrimage.Properties.Resources.TextDocument;
            this.tsbExportToPAML.Name = "tsbExportToPAML";
            this.tsbExportToPAML.Size = new System.Drawing.Size(186, 22);
            this.tsbExportToPAML.Text = "PAML Sequences File";
            this.tsbExportToPAML.Click += new System.EventHandler(this.tsbExportToPAML_Click);
            // 
            // uctGeneSequencesMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tscForm);
            this.Name = "uctGeneSequencesMain";
            this.Size = new System.Drawing.Size(1200, 490);
            this.tscForm.ContentPanel.ResumeLayout(false);
            this.tscForm.TopToolStripPanel.ResumeLayout(false);
            this.tscForm.TopToolStripPanel.PerformLayout();
            this.tscForm.ResumeLayout(false);
            this.tscForm.PerformLayout();
            this.tsSubSetActions.ResumeLayout(false);
            this.tsSubSetActions.PerformLayout();
            this.tsForm.ResumeLayout(false);
            this.tsForm.PerformLayout();
            this.tsGeneActions.ResumeLayout(false);
            this.tsGeneActions.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripContainer tscForm;
        internal DraggableTabControl.DraggableTabControl tbSubSets;
        private System.Windows.Forms.ToolStrip tsSubSetActions;
        private System.Windows.Forms.ToolStripButton tsbNewSubSet;
        private System.Windows.Forms.ToolStripButton tsbOpenSubSet;
        private System.Windows.Forms.ToolStripButton tsbCloseSubSet;
        private System.Windows.Forms.ToolStripSeparator tssGeneActions;
        private System.Windows.Forms.ToolStripButton tsbSearchGenBank;
        private System.Windows.Forms.ToolStrip tsForm;
        private System.Windows.Forms.ToolStripButton tsbMoveSelected;
        private System.Windows.Forms.ToolStripButton tsbCopySelected;
        private System.Windows.Forms.ToolStripButton tsbDeleteGene;
        private System.Windows.Forms.ToolStripSeparator tssGeneActions1;
        private System.Windows.Forms.ToolStripButton tsbBlastNSelectedSequences;
        private System.Windows.Forms.ToolStripButton tsbUpdateFromGenBank;
        private System.Windows.Forms.ToolStripDropDownButton tsbExportTo;
        private System.Windows.Forms.ToolStripMenuItem tsbExportToFASTA;
        private System.Windows.Forms.ToolStripMenuItem tsbExportToExcel;
        private System.Windows.Forms.ToolStripMenuItem tsbExportToMEGA;
        private System.Windows.Forms.ToolStripDropDownButton tsbImportFrom;
        private System.Windows.Forms.ToolStripMenuItem tsbImportFromFASTAFile;
        private System.Windows.Forms.ToolStripMenuItem tsbImportFromAlignmentInMEGA;
        private System.Windows.Forms.ToolStrip tsGeneActions;
        private System.Windows.Forms.ToolStripButton tsbSetGeneName;
        private System.Windows.Forms.ToolStripButton tsbGenerateTreeWithPhyML;
        private System.Windows.Forms.ToolStripMenuItem tsbAlignWithMUSCLE;
        private System.Windows.Forms.ToolStripMenuItem tsbAlignWithPRANK;
        private System.Windows.Forms.ToolStripDropDownButton tsbAlignWith;
        private System.Windows.Forms.ToolStripMenuItem tsbImportFromBLASTNExeOutput;
        private System.Windows.Forms.ToolStripButton tsbAnnotateFromBLASTNCBI;
        private System.Windows.Forms.ToolStripMenuItem tsbImportFromTrinityFASTAFile;
        private System.Windows.Forms.ToolStripMenuItem tsbImportFromNEXUSFile;
        private System.Windows.Forms.ToolStripMenuItem tsbExportToNEXUS;
        private System.Windows.Forms.ToolStripMenuItem tsbExportToPHYLIP;
        private System.Windows.Forms.ToolStripMenuItem tsbImportFromPHYLIPFile;
        private System.Windows.Forms.ToolStripMenuItem tsbExportToPilgrimage;
        private System.Windows.Forms.ToolStripSeparator sepExport;
        private System.Windows.Forms.ToolStripSeparator sepImport;
        private System.Windows.Forms.ToolStripMenuItem tsbImportFromPilgrimageDataFile;
        private System.Windows.Forms.ToolStripMenuItem tsbExportToPAML;
    }
}
