using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChangLab.Common;
using ChangLab.Jobs;
using ChangLab.Genes;
using ChangLab.NCBI.GenBank;
using ChangLab.RecordSets;
using Pilgrimage.Activities;

namespace Pilgrimage.GeneSequences
{
    public partial class uctGeneSequencesMain : Pilgrimage.UserControls.uctSubSetsManager
    {
        protected internal override DataTypes DataType { get { return DataTypes.GeneSequence; } }
        protected internal override DraggableTabControl.DraggableTabControl SubSetViewsControl { get { return tbSubSets; } }
        protected internal override ToolStripContainer FormToolStripContainer { get { return tscForm; } }

        public uctGeneSequencesMain()
        {
            InitializeComponent();
            this.tsbExportToPilgrimage.Text = Program.ProductName + " Data File";
            this.tsbImportFromPilgrimageDataFile.Text = Program.ProductName + " Data File";

            if (this.DesignMode) { return; }

            tbSubSets.Reordered += new TabControlEventHandler(SubSetsViewControl_Reordered);
            tsbNewSubSet.Click += new EventHandler(NewSubset_Click);
            tsbOpenSubSet.Click += new EventHandler(OpenSubSet_Click);
            tsbCloseSubSet.Click += new EventHandler(CloseSubSet_Click);
            tsbImportFromPilgrimageDataFile.Click += new System.EventHandler(ImportFromPilgrimageDataFile_Click);

            tsSubSetActions.EndDrag += new EventHandler(ToolStrip_EndDrag);
            tsGeneActions.EndDrag += new EventHandler(ToolStrip_EndDrag);
            tsForm.EndDrag += new EventHandler(ToolStrip_EndDrag);

            tsActions = (new ToolStripItem[] { tsbCopySelected, tsbMoveSelected, tsbDeleteGene, tsbSetGeneName, tsbBlastNSelectedSequences, tsbUpdateFromGenBank, tsbExportTo }).ToList();

#if EEB460
            tsbSearchGenBank.Visible = false;
            tsbBlastNSelectedSequences.Visible = false;
            tsbUpdateFromGenBank.Visible = false;
            tsbAnnotateFromBLASTNCBI.Visible = false;
            tsbAlignWith.Visible = false;

            sepExport.Visible = false;
            tsbExportToPilgrimage.Visible = false;
            sepImport.Visible = false;
            tsbImportFromBLASTNExeOutput.Visible = false;
            tsbImportFromTrinityFASTAFile.Visible = false;
            tsbImportFromPilgrimageDataFile.Visible = false;
#endif
        }

        public bool GetSelectedGenes(out List<GenericGeneRowDataItem> SelectedGeneRows, bool ShowNoGenesMessage, bool PromptForAllIfNone = true)
        {
            SelectedGeneRows = null;

            if (SelectedSubSetView.Refreshing) { return false; }
            SelectedGeneRows = SelectedSubSetView.SelectedRows.Cast<GenericGeneRowDataItem>().ToList();

            if (SelectedGeneRows == null || SelectedGeneRows.Count == 0)
            {
                if (PromptForAllIfNone)
                {
                    if (Utility.ShowMessage(this, Properties.Resources.Messages_NoGenesSelectAll, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                    {
                        SelectedGeneRows = SelectedSubSetView.AllRows.Cast<GenericGeneRowDataItem>().ToList();
                        return true;
                    }
                    else { return false; }
                }
                else
                {
                    Utility.ShowMessage(this, Properties.Resources.Messages_NoGenesSelected);
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        protected internal override void AddSubSet(SubSet Set, bool UpdateOpened = false)
        {
            Program.DebugStartupTime("AddSubSet - Start");
            TabPage pg = new TabPage(Set.Name) { Tag = Set };

            uctRecordSetGenes rsControl = new uctRecordSetGenes() { Dock = DockStyle.Fill, Manager = this };
            pg.Controls.Add(rsControl);
            tbSubSets.TabPages.Add(pg);

            rsControl.Initialize(this, Set);
            Program.DebugStartupTime("AddSubSet - End");
        }

        private void tbSubSets_Selected(object sender, TabControlEventArgs e)
        {
            if (LoadingSubSetPages || tbSubSets.IsDragging) { return; }

            CurrentSubSet = (e.TabPage == null ? null : (SubSet)e.TabPage.Tag);
            tsActions.ForEach(tsb => tsb.Enabled = (CurrentSubSet != null));
            tsbCloseSubSet.Enabled = (CurrentSubSet != null);

            if (CurrentSubSet != null)
            {
                if (!SelectedSubSetView.Loaded) { SelectedSubSetView.RefreshRecords(); } // Load on demand
                CurrentSubSet.Opened();
            }

#if DEBUG
            System.Diagnostics.Debug.WriteLine("tbSubSets_Selected: " + (CurrentSubSet != null ? CurrentSubSet.Name : "null"));
#endif
        }

        #region Toolbar Buttons
        internal void SearchGenBank()
        {
            SearchGenBankForNucleotides search = new SearchGenBankForNucleotides(this);
            search.ActivityCompleted += new Activity.ActivityCompletedEventHandler(SearchCompleted);
            search.Search();
        }

        private void tsbSearchGenBank_Click(object sender, EventArgs e)
        {
            SearchGenBank();
        }

        private void SearchCompleted(ActivityCompletedEventArgs e)
        {
            if (e.Cancelled)
            { return; }
            else if (e.Error != null)
            { Utility.ShowErrorMessage(this, e.Error); }
            else
            {
                List<string> editedSubSetIDs = (List<string>)e.Result;
                if (editedSubSetIDs.Count != 0)
                {
                    editedSubSetIDs.Distinct().ToList().ForEach(id => ShowAndRefreshSubSet(id, null));
                }
            }
        }

        internal void ViewBLASTNResultsHistory(Activity SelectedActivity = null)
        {
            //using (BlastN.frmBlastNResultsHistory frm = new BlastN.frmBlastNResultsHistory())
            //{
            //    frm.ShowDialog();
            //    if (frm.EditedSubSetIDs.Count != 0)
            //    {
            //        frm.EditedSubSetIDs.Distinct().ToList().ForEach(sub => this.ShowAndRefreshSubset(sub, null));
            //    }
            //}

            using (frmActivityHistory<BlastSequencesAtNCBI> frm = new frmActivityHistory<BlastSequencesAtNCBI>(JobTargets.BLASTN_NCBI) { SelectedActivity = SelectedActivity })
            {
                frm.ShowProgressForm += ((args) =>
                {
                    args.Form.Options.ShowTotalProgress = (((BlastSequencesAtNCBI)args.Activity).BatchedRequestCount > 1);
                });

                frm.ViewDetails += ((args) =>
                {
                    if (args.Cancel) { return; }
                    else
                    {
                        BlastNAtNCBI job = new BlastNAtNCBI((string)args.Args);
                        switch (job.Purpose)
                        {
                            case BlastNAtNCBI.BLASTPurposes.SimilarCodingSequences:
                                using (BlastN.frmBlastNAlignments results = new BlastN.frmBlastNAlignments(job))
                                {
                                    results.ShowDialog(this);
                                    if (results.EditedSubSetIDs.Count != 0)
                                    { results.EditedSubSetIDs.Distinct().ToList().ForEach(sub => this.ShowAndRefreshSubSet(sub, null)); }
                                }
                                break;
                            case BlastNAtNCBI.BLASTPurposes.AnnotateUnknownGenes:
                                using (BlastN.frmBlastNAnnotations results = new BlastN.frmBlastNAnnotations(job))
                                {
                                    results.ShowDialog(this);
                                    if (results.EditedSubSetIDs.Count != 0)
                                    { results.EditedSubSetIDs.Distinct().ToList().ForEach(sub => this.ShowAndRefreshSubSet(sub, null)); }
                                    if (results.UpdatedSubSetIDs.Count != 0)
                                    { results.UpdatedSubSetIDs.ForEach(id => this.ShowAndRefreshSubSet(id, null)); }
                                }
                                break;
                        }
                    }
                });

                frm.ShowDialog(this);
                if (frm.EditedSubSetIDs.Count != 0)
                { frm.EditedSubSetIDs.Distinct().ToList().ForEach(sub => this.ShowAndRefreshSubSet(sub, null)); }
            }
        }

        private void tsbBLASTNResultsHistory_Click(object sender, EventArgs e)
        {
            ViewBLASTNResultsHistory();
        }

        private void tsbMoveOrCopySelected_Click(object sender, EventArgs e)
        {
            List<GenericGeneRowDataItem> selectedGenes = null;
            if (GetSelectedGenes(out selectedGenes, true))
            {
                bool move = sender == tsbMoveSelected;

                using (RecordSets.frmSelectSubSet frm = new RecordSets.frmSelectSubSet(this.DataType, this.CurrentSubSet, move))
                {
                    if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        SelectedSubSetView.DataGridHelper.ShortTermDisableKeyPress();

                        System.Diagnostics.Debug.WriteLine("tsbMoveSelectedGenes_Click_frmSelectSubSet_Closed");

                        SubSet selectedSubSet = null;
                        if (string.IsNullOrEmpty(frm.SelectedRecordSetID))
                        {
                            System.Diagnostics.Debug.WriteLine("tsbMoveSelectedGenes_Click_frmSelectSubSet_Closed");

                            bool newSubSet = (!AllSubSets.Any(sub => GuidCompare.Equals(sub.ID, frm.SelectedSubSetID)));
                            if (newSubSet)
                            {
                                RefreshSubSetsFromDatabase();
                            }

                            selectedSubSet = AllSubSets.First(sub => GuidCompare.Equals(sub.ID, frm.SelectedSubSetID));

                            CurrentRecordSet.AddGenes(selectedGenes.Select(g => g.Gene), selectedSubSet.ID);
                            if (move) { CurrentRecordSet.RemoveGenes(selectedGenes.Select(g => g.Gene), CurrentSubSet.ID); }

                            if (move)
                            {
                                // Remove them from the subset view via a soft update.
                                SelectedSubSetView.RemoveRecords(selectedGenes.Cast<RowDataItem>().ToList());
                            }
                            if (newSubSet)
                            {
                                AddSubSet(selectedSubSet);
                            }
                            if (selectedSubSet.Open)
                            {
                                SubSetRecords(selectedSubSet.ID).RefreshRecords();
                            }
                        }
                        else
                        {
                            RecordSet.AddGenes(selectedGenes.Select(g => g.Gene), frm.SelectedRecordSetID, frm.SelectedSubSetID);
                            if (move)
                            {
                                RecordSet.RemoveGenes(selectedGenes.Select(g => g.Gene), CurrentRecordSet.ID, CurrentSubSet.ID);
                                SelectedSubSetView.RemoveRecords(selectedGenes.Cast<RowDataItem>().ToList());
                            }
                        }
                    }
                }
            }
        }

        private void tsbDeleteGene_Click(object sender, EventArgs e)
        {
            List<GenericGeneRowDataItem> selectedGenes = null;
            if (GetSelectedGenes(out selectedGenes, true, false))
            {
                if (Utility.ShowMessage(this, "Are you sure you want to remove the selected " + selectedGenes.Count.ToString()
                                                + " gene" + (selectedGenes.Count == 1 ? string.Empty : "s") + " from this dataset?"
                                                + "\r\n\r\n" + "This action cannot be undone because Dan hasn't yet programmed in Undo functionality.",
                                                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Yes)
                {
                    selectedGenes.ForEach(g =>
                    {
                        CurrentRecordSet.RemoveGene(g.Gene, CurrentSubSet.ID);
                    });
                    this.CurrentRecordSet.Save(); // Update the ModifiedAt value.
                    SelectedSubSetView.RemoveRecords(selectedGenes.Cast<RowDataItem>().ToList());
                }
            }
        }

        private void tsbSetGeneName_Click(object sender, EventArgs e)
        {
            List<GenericGeneRowDataItem> selectedGenes = null;
            if (GetSelectedGenes(out selectedGenes, true))
            {
                using (frmBatchSetGeneName frm = new frmBatchSetGeneName(selectedGenes.Select(g => g.Gene)))
                {
                    if (frm.ShowDialog(this) == DialogResult.OK)
                    {
                        this.CurrentRecordSet.Save();
                        SelectedSubSetView.DataGridHelper.ShortTermDisableKeyPress();
                        SelectedSubSetView.RefreshRecords(false, true);
                    }
                }
            }
        }

        private void tsbBlastNSelectedSequences_Click(object sender, EventArgs e)
        {
            if (Program.InProgressActivities.BlastNAtNCBIs.Count(a => !a.CurrentJob.HasCompleted) > 0)
            {
                Utility.ShowMessage(this, "Concurrent submissions to NCBI are not supported per NCBI guidelines.  Please wait until the current submission has completed.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            List<GenericGeneRowDataItem> selectedGenes = null;
            if (GetSelectedGenes(out selectedGenes, true))
            {
                List<Gene> genes = null;

                int previouslySubmitted = selectedGenes.Count<GenericGeneRowDataItem>(g => g.HasBlastNResults);
                if (previouslySubmitted == selectedGenes.Count)
                {
                    if (Utility.ShowMessage(this, "All of the selected genes have previously been submited and results were downloaded."
                                                    + "\r\n"
                                                    + "\r\n" + "Select \"OK\" to resubmit the sequences.",
                                                    MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == System.Windows.Forms.DialogResult.Cancel)
                    { return; }
                }
                else if (previouslySubmitted != 0)
                {
                    DialogResult result = Utility.ShowMessage(this, previouslySubmitted.ToString() + " of the selected sequences have previously been submitted and results were downloaded."
                                                                    + "\r\n"
                                                                    + "\r\n" + "Select \"Yes\" to include these sequences in this submission."
                                                                    + "\r\n" + "Select \"No\" to exclude these sequences from submission and only submit those sequences that have not previously been submitted.",
                                                                    MessageBoxButtons.YesNoCancel,
                                                                    MessageBoxIcon.Information);
                    switch (result)
                    {
                        case System.Windows.Forms.DialogResult.No:
                            genes = selectedGenes.Where(g => !g.HasBlastNResults).Select(g => g.Gene).ToList();
                            break;
                        case System.Windows.Forms.DialogResult.Cancel:
                            return;
                    }
                }

                if (genes == null) { genes = selectedGenes.Select(g => g.Gene).ToList(); }

                BlastSequencesAtNCBI blastn = null;
                using (BlastN.frmBlastNOptions frm = new BlastN.frmBlastNOptions(BlastNAtNCBI.BLASTPurposes.SimilarCodingSequences) 
                    { SelectedGenes = genes, MainForm = (frmMain)this.ParentForm })
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        // If DialogResult.OK, the BlastSequencesAtNCBI activity instance is underway.
                        blastn = frm.Activity;
                        blastn.ResultsSaved += new BlastSequencesAtNCBI.ResultsSavedEventHandler(BlastSequencesAtNCBI_ResultsSaved);
                    }
                    else { return; }   
                }

                // We'll make it here if the job was initialized without any issues.
                using (frmActivityProgress frm = new frmActivityProgress(new ProgressForm.ProgressOptions() { ShowTotalProgress = (blastn.BatchedRequestCount > 1) })
                        { Activity = blastn, Text = "Submitting coding sequences to BLASTN (NCBI)..." })
                { if (frm.ShowDialog(this) != DialogResult.OK) { return; /* Progress screen was closed (Ignore) or the job was cancelled (Cancel) */ } }

                // We'll make it this far if the job was allowed to complete while the progress screen was open and the user selected "View Results"
                using (BlastN.frmBlastNAlignments frm = new BlastN.frmBlastNAlignments((BlastNAtNCBI)blastn.CurrentJob))
                {
                    frm.ShowDialog(this);
                    if (frm.EditedSubSetIDs.Count != 0)
                    { frm.EditedSubSetIDs.Distinct().ToList().ForEach(sub => this.ShowAndRefreshSubSet(sub, null)); }
                }

                //using (frmProgress progressForm = new frmProgress("Submitting coding sequences to BLASTN (NCBI)...", true, true, (Genes.Count > BlastNWebService.SequenceBatchSize), false))
                //{
                //    progressForm.Cancelled += new frmProgress.CancelledEventHandler(ProgressForm_Cancelled);
                //    progressForm.ShowDialog(OwnerWindow);
                //}
            }
        }

        private void BlastSequencesAtNCBI_ResultsSaved(BlastNAtNCBI.ResultsEventArgs e)
        {
            this.TabPages.Cast<TabPage>().ToList()
                .ForEach(pg => ((GeneSequences.uctRecordSetGenes)pg.Controls[0])
                                    .UpdateHasAlignedSubjectSequences(e.Alignments.Where(kv => kv.Value.Count != 0).Select(kv => kv.Key.ID).ToList()));
        }

        //private void BlastSequencesAtNCBI_ActivityCompleted(ActivityCompletedEventArgs e)
        //{
        //    if (e.Cancelled) { return; }
        //    if (e.Error != null)
        //    {
        //        Utility.ShowErrorMessage(this, e.Error);
        //        return;
        //    }
        //}

        private List<GenericGeneRowDataItem> selectedGenesForUpdateFromGenBank;
        private void tsbUpdateFromGenBank_Click(object sender, EventArgs e)
        {
            selectedGenesForUpdateFromGenBank = null;
            if (GetSelectedGenes(out selectedGenesForUpdateFromGenBank, true))
            {
                int withoutGenBankId = selectedGenesForUpdateFromGenBank.Count(g => g.GenBankID.ToSafeInt() == 0);
                if (withoutGenBankId == selectedGenesForUpdateFromGenBank.Count)
                {
                    Utility.ShowMessage(this, "All of the selected genes do not have a GenBank ID and cannot be updated from GenBank.");
                    return;
                }
                else if (withoutGenBankId > 0)
                {
                    Utility.ShowMessage(this, withoutGenBankId.ToString() + " of the selected " + selectedGenesForUpdateFromGenBank.Count.ToString()
                                                + " genes do not have a GenBank ID and will not be updated.  The remaining "
                                                + (selectedGenesForUpdateFromGenBank.Count - withoutGenBankId).ToString() + " genes will be updated.");
                }

                PopulateFromGenBank gb = new PopulateFromGenBank(this);
                gb.ActivityCompleted += new Activity.ActivityCompletedEventHandler(UpdateFromGenBankCompleted);
                gb.Populate(selectedGenesForUpdateFromGenBank.Where(g => g.Gene.GenBankID.ToSafeInt() != 0).Select(g => g.Gene), null);
            }
        }

        private void UpdateFromGenBankCompleted(RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Utility.ShowErrorMessage(this, e.Error);
            }
            else if (!e.Cancelled)
            {
                List<Gene> PopulatedGenes = (List<Gene>)e.Result;
                PopulatedGenes.ForEach(pg =>
                {
                    GenericGeneRowDataItem match = selectedGenesForUpdateFromGenBank.FirstOrDefault(sg => sg.Gene.GenBankID == pg.GenBankID);
                    if (match != null)
                    {
                        match.Gene.Merge(pg, true, true, false);
                        match.Gene.LastUpdateSource = GeneSources.GenBank;
                        match.Gene.Save(true, true);
                    }
                });
                this.CurrentRecordSet.Save(); // Update the ModifiedAt value.

                Utility.ShowMessage(this, PopulatedGenes.Count.ToString() + " genes have been updated from GenBank.");
                SelectedSubSetView.DataGridHelper.ShortTermDisableKeyPress();

                SelectedSubSetView.RefreshRecords(false, true);
            }
        }

        private void tsbAnnotateFromBLASTNCBI_Click(object sender, EventArgs e)
        {
            if (Program.InProgressActivities.BlastNAtNCBIs.Count(a => !a.CurrentJob.HasCompleted) > 0)
            {
                Utility.ShowMessage(this, "Concurrent submissions to NCBI are not supported per NCBI guidelines.  Please wait until the current submission has completed.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            List<GenericGeneRowDataItem> selectedGenes = null;
            if (GetSelectedGenes(out selectedGenes, true))
            {
                BlastSequencesAtNCBI blastn = null;
                using (BlastN.frmBlastNOptions frm = new BlastN.frmBlastNOptions(BlastNAtNCBI.BLASTPurposes.AnnotateUnknownGenes) 
                    { SelectedGenes = selectedGenes.Select(g => g.Gene).ToList(), MainForm = (frmMain)this.ParentForm })
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        // If DialogResult.OK, the BlastSequencesAtNCBI activity instance is underway.
                        blastn = frm.Activity;
                        blastn.ResultsSaved += new BlastSequencesAtNCBI.ResultsSavedEventHandler(BlastSequencesAtNCBI_ResultsSaved);
                    }
                    else { return; }
                }

                // We'll make it here if the job was initialized without any issues.
                using (frmActivityProgress frm = new frmActivityProgress(new ProgressForm.ProgressOptions() { ShowTotalProgress = (blastn.BatchedRequestCount > 1) }) 
                    { Activity = blastn, Text = "Submitting sequences to BLASTN (NCBI)..." })
                { if (frm.ShowDialog(this) != DialogResult.OK) { return; /* Progress screen was closed (Ignore) or the job was cancelled (Cancel) */ } }
                
                // We'll make it this far if the job was allowed to complete while the progress screen was open and the user selected "View Results"
                using (BlastN.frmBlastNAnnotations frm = new BlastN.frmBlastNAnnotations((BlastNAtNCBI)blastn.CurrentJob))
                {
                    frm.ShowDialog(this);
                    if (frm.EditedSubSetIDs.Count != 0)
                    { frm.EditedSubSetIDs.Distinct().ToList().ForEach(sub => this.ShowAndRefreshSubSet(sub, null)); }
                    if (frm.UpdatedSubSetIDs.Count != 0)
                    { frm.UpdatedSubSetIDs.ForEach(id => this.ShowAndRefreshSubSet(id, null)); }

                    //if (frm.UpdatedSubSetIDs.Count != 0)
                    //{ frm.UpdatedSubSetIDs.ForEach(id => this.RefreshSubSet(id)); }
                }
            }
        }

        private void tsbExportToMEGA_Click(object sender, EventArgs e)
        {
            List<GenericGeneRowDataItem> selectedGenes = null;
            if (GetSelectedGenes(out selectedGenes, true))
            {
                using (frmExportToFlatFile frm = new frmExportToFlatFile(selectedGenes.Select(g => g.Gene).ToList(), ExportFileFormats.FASTA) { OpenInMEGA = true })
                {
                    frm.ShowDialog(this);
                    SelectedSubSetView.DataGridHelper.ShortTermDisableKeyPress();
                }
            }
        }

        private void tsbExportToFASTA_Click(object sender, EventArgs e)
        {
            List<GenericGeneRowDataItem> selectedGenes = null;
            if (GetSelectedGenes(out selectedGenes, true))
            {
                using (frmExportToFlatFile frm = new frmExportToFlatFile(selectedGenes.Select(g => g.Gene).ToList(), ExportFileFormats.FASTA))
                {
                    frm.ShowDialog(this);
                    SelectedSubSetView.DataGridHelper.ShortTermDisableKeyPress();
                }
            }
        }

        private void tsbExportToNEXUS_Click(object sender, EventArgs e)
        {
            tsbExportToAlignedFormat_Click(ExportFileFormats.NEXUS);
        }

        private void tsbExportToPHYLIP_Click(object sender, EventArgs e)
        {
            tsbExportToAlignedFormat_Click(ExportFileFormats.PHYLIP);
        }

        private void tsbExportToPAML_Click(object sender, EventArgs e)
        {
            tsbExportToAlignedFormat_Click(ExportFileFormats.PAML);
        }

        private void tsbExportToAlignedFormat_Click(ExportFileFormats FileFormat)
        {
            List<GenericGeneRowDataItem> selectedGenes = null;
            if (GetSelectedGenes(out selectedGenes, true))
            {
                int sequenceLength = selectedGenes.First().Gene.Nucleotides.Length;
                if (selectedGenes.Any(g => g.Gene.Nucleotides.Length != sequenceLength))
                {
                    // NEXUS requires them to be all the same length; presumably the user does an alignment before trying to export.
                    if (Utility.ShowMessage(this,
                            "One or more of the selected sequences are not the same length."
                            + "\r\n"
                            + "Select \"OK\" to pad the end of the shorter sequences with \"-\" to match the same length as the longest sequence.",
                            MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
                    { return; }
                }

                using (frmExportToFlatFile frm = new frmExportToFlatFile(selectedGenes.Select(g => g.Gene).ToList(), FileFormat))
                {
                    frm.ShowDialog(this);
                    SelectedSubSetView.DataGridHelper.ShortTermDisableKeyPress();
                }
            }
        }

        private void tsbExportToExcel_Click(object sender, EventArgs e)
        {
            List<GenericGeneRowDataItem> selectedGenes = null;
            if (GetSelectedGenes(out selectedGenes, true))
            {
                using (frmExportToExcel frm = new frmExportToExcel(selectedGenes.Select(g => g.Gene).ToList()))
                {
                    frm.ShowDialog(this);
                    SelectedSubSetView.DataGridHelper.ShortTermDisableKeyPress();
                }
            }
        }

        private void tsbExportToPilgrimage_Click(object sender, EventArgs e)
        {
            List<GenericGeneRowDataItem> selectedGenes = null;
            if (GetSelectedGenes(out selectedGenes, true))
            {
                using (RecordSets.frmExportToPilgrimageDataFile frm = new RecordSets.frmExportToPilgrimageDataFile(selectedGenes.Select(g => g.ID).ToList(), null, this.DataType))
                { frm.ShowDialog(); }
            }
        }

        private void tsbImportFromFASTA_Click(object sender, EventArgs e)
        {
            tsbImportFromFlatFile(GeneSources.FASTA);
        }

        private void tsbImportFromNEXUSFile_Click(object sender, EventArgs e)
        {
            tsbImportFromFlatFile(GeneSources.NEXUS);
        }

        private void tsbImportFromPHYLIPFile_Click(object sender, EventArgs e)
        {
            tsbImportFromFlatFile(GeneSources.PHYLIP);
        }

        private void tsbImportFromAlignment_Click(object sender, EventArgs e)
        {
            using (frmImportFromAlignment frm = new frmImportFromAlignment())
            {
                if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    ShowAndRefreshSubSet(frm.SelectedSubSetID, null);
                }
            }
        }

        private void tsbImportFromBLASTNExeOutput_Click(object sender, EventArgs e)
        {
            using (frmImportFromBLASTNExeOutput frm = new frmImportFromBLASTNExeOutput())
            {
                frm.ShowDialog(this);
                if (frm.EditedSubSetIDs.Count != 0)
                { frm.EditedSubSetIDs.Distinct().ToList().ForEach(id => ShowAndRefreshSubSet(id, null)); }
            }
        }

        private void tsbImportFromTrinityFASTAFile_Click(object sender, EventArgs e)
        {
            tsbImportFromFlatFile(GeneSources.Trinity);
        }

        private void tsbImportFromFlatFile(GeneSources FileSource)
        {
            using (frmImportFromFlatFile frm = new frmImportFromFlatFile(FileSource))
            {
                frm.ShowDialog(this);
                if (frm.EditedSubSetIDs.Count != 0)
                { frm.EditedSubSetIDs.Distinct().ToList().ForEach(id => ShowAndRefreshSubSet(id, null)); }
            }
        }

        private void tsbAlignWithPRANK_Click(object sender, EventArgs e)
        {
            tsbAlignWith_Click<AlignSequencesWithPRANK>(JobTargets.PRANK);
        }

        private void tsbAlignWithMUSCLE_Click(object sender, EventArgs e)
        {
            tsbAlignWith_Click<AlignSequencesWithMUSCLE>(JobTargets.MUSCLE);
        }

        private void tsbAlignWith_Click<T>(JobTargets Target) where T : CommandLineAlignmentJob
        {
            List<GenericGeneRowDataItem> selectedGenes = null;
            if (GetSelectedGenes(out selectedGenes, true))
            {
                RunAlignment alignment = null;
                switch (Target)
                {
                    case JobTargets.PRANK:
                        using (Alignment.PRANK.frmCreateJob frm = new Alignment.PRANK.frmCreateJob(selectedGenes.Select(g => g.Gene).ToList()))
                        {
                            if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK) { alignment = frm.Alignment; }
                            else { return; }
                        }
                        break;

                    case JobTargets.MUSCLE:
                        using (Alignment.MUSCLE.frmCreateJob frm = new Alignment.MUSCLE.frmCreateJob(selectedGenes.Select(g => g.Gene).ToList()))
                        {
                            if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK) { alignment = frm.Alignment; }
                            else { return; }
                        }
                        break;
                }
                
                // We'll make it here if the job was initialized without any issues.
                using (frmActivityProgress frm = new frmActivityProgress() { Activity = alignment })
                { if (frm.ShowDialog(this) != DialogResult.OK) { return; /* Progress screen was closed (Ignore) or the job was cancelled (Cancel) */ } }

                // We'll make it here if the job completed without any issues and the user selected "View Results".
                using (Alignment.frmAlignmentResults<T> frm = new Alignment.frmAlignmentResults<T>(alignment.AlignmentJob.ID))
                {
                    frm.ShowDialog(this);
                    if (frm.EditedSubSetIDs.Count != 0)
                    { frm.EditedSubSetIDs.Distinct().ToList().ForEach(sub => this.ShowAndRefreshSubSet(sub, null)); }
                }
            }
        }

        internal void ViewPRANKAlignmentHistory(Activity SelectedActivity = null)
        {
            ViewAlignmentHistory<AlignSequencesWithPRANK>(JobTargets.PRANK, SelectedActivity);
        }

        internal void ViewMUSCLEAlignmentHistory(Activity SelectedActivity = null)
        {
            ViewAlignmentHistory<AlignSequencesWithMUSCLE>(JobTargets.MUSCLE, SelectedActivity);
        }

        private void ViewAlignmentHistory<T>(JobTargets Target, Activity SelectedActivity = null) where T : CommandLineAlignmentJob
        {
            using (frmActivityHistory<RunAlignment> frm = new frmActivityHistory<RunAlignment>(Target) { SelectedActivity = SelectedActivity })
            {
                frm.ViewDetails += ((args) =>
                {
                    if (args.Cancel) { return; }
                    else
                    {
                        using (Alignment.frmAlignmentResults<T> results = new Alignment.frmAlignmentResults<T>((string)args.Args))
                        {
                            results.ShowDialog();
                            if (results.EditedSubSetIDs.Count != 0)
                            { frm.EditedSubSetIDs.AddRange(results.EditedSubSetIDs.Where(id => !frm.EditedSubSetIDs.Contains(id))); }
                        }
                    }
                });

                frm.ShowDialog(this);
                if (frm.EditedSubSetIDs.Count != 0)
                { frm.EditedSubSetIDs.Distinct().ToList().ForEach(sub => this.ShowAndRefreshSubSet(sub, null)); }
            }
        }

        private void tsbGenerateTreeWithPhyML_Click(object sender, EventArgs e)
        {
            List<GenericGeneRowDataItem> selectedGenes = null;
            if (GetSelectedGenes(out selectedGenes, true))
            {
                int sequenceLength = selectedGenes.First().Gene.Nucleotides.Length;
                if (selectedGenes.Any(g => g.Gene.Nucleotides.Length != sequenceLength))
                {
                    // PhyML requires them to be all the same length; presumably the user does an alignment before trying to make a tree.
                    Utility.ShowMessage(this, "One or more of the selected sequences are not the same length.", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                RunPhyML phyml = null;
                using (PhyML.frmCreateJob frm = new PhyML.frmCreateJob(selectedGenes.Select(g => g.Gene).ToList()))
                {
                    if (frm.ShowDialog(this) == DialogResult.OK) { phyml = frm.Activity; }
                    else { return; }
                }

                // We'll make it here if the job was initialized without any issues.
                using (frmActivityProgress frm = new frmActivityProgress() { Activity = phyml })
                { if (frm.ShowDialog(this) != DialogResult.OK) { return; /* Progress screen was closed (Ignore) or the job was cancelled (Cancel) */ } }

                // We'll make it here if the job completed without any issues and the user selected "View Output".
                using (PhyML.frmPhyMLResults results = new PhyML.frmPhyMLResults(phyml.PhyMLJob))
                { results.ShowDialog(); }
            }
        }

        internal void ViewPhyMLHistory(Activity SelectedActivity = null)
        {
            using (frmActivityHistory<RunPhyML> frm = new frmActivityHistory<RunPhyML>(JobTargets.PhyML) { SelectedActivity = SelectedActivity })
            {
                frm.ViewDetails += ((args) =>
                {
                    if (args.Cancel) { return; }
                    else
                    {
                        using (PhyML.frmPhyMLResults results = new PhyML.frmPhyMLResults((string)args.Args))
                        { results.ShowDialog(); }
                    }
                });

                frm.ShowDialog(this);
                if (frm.EditedSubSetIDs.Count != 0)
                { frm.EditedSubSetIDs.Distinct().ToList().ForEach(sub => this.ShowAndRefreshSubSet(sub, null)); }
            }
        }

        private void tsbImportFrom_Click(object sender, EventArgs e)
        {
            ((frmMain)this.ParentForm).toolStripStatusLabel1.Text = "tsbImportFrom_Click";
        }

        private void tsbImportFrom_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ((frmMain)this.ParentForm).toolStripStatusLabel1.Text = "tsbImportFrom_DropDownItemClicked";
        }
        #endregion
    }
}