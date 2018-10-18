using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using ChangLab.Common;
using ChangLab.Jobs;

namespace Pilgrimage.Activities
{
    public class ExportToDataFile : Activity
    {
        private string RecordSetID { get; set; }
        private string SourceSubSetID { get; set; }
        private List<string> SelectedGeneIDs { get; set; }
        private List<int> SelectedResultIDs { get; set; }

        private string FilePath { get; set; }
        private IOPilgrimageDataFile.ExportOptions Options { get; set; }
        private bool IncludeHistory { get; set; }
        private IOPilgrimageDataFile Exporter { get; set; }
        
        public ExportToDataFile(IWin32Window OwnerWindow) : base(OwnerWindow) { }

        public void Export(string RecordSetID, string FilePath, IOPilgrimageDataFile.ExportOptions Options)
        {
            this.RecordSetID = RecordSetID;
            Export(FilePath, Options);
        }

        public void Export(string SourceSubSetID, List<string> SelectedGeneIDs, List<int> SelectedResultIDs, string FilePath, IOPilgrimageDataFile.ExportOptions Options)
        {
            this.SourceSubSetID = SourceSubSetID;
            this.SelectedGeneIDs = SelectedGeneIDs;
            this.SelectedResultIDs = SelectedResultIDs;
            Export(FilePath, Options);
        }

        private void Export(string FilePath, IOPilgrimageDataFile.ExportOptions Options)
        {
            this.FilePath = FilePath;
            this.Options = Options;

            StartExport("Exporting...");
        }

        private void StartExport(string HeaderText)
        {
            using (ProgressForm = new frmProgress(HeaderText, new frmProgress.ProgressOptions() { AllowCancellation = false, ShowTotalProgress = false, ShowCurrentProgress = false, UseNeverEndingTimer = true }))
            {
                Worker.RunWorkerAsync();
                ProgressForm.ShowDialog(OwnerWindow);
            }
        }

        //public void Export(string RecordSetID, string FilePath, bool IncludeHistory)
        //{
        //    this.RecordSetID = RecordSetID;
        //    this.FilePath = FilePath;
        //    this.IncludeHistory = IncludeHistory;
        //    this.Exporter = new IOPilgrimageDataFile();

        //    using (ProgressForm = new frmProgress("Exporting Recordset", new frmProgress.ProgressOptions() { AllowCancellation = false }))
        //    {
        //        Worker.RunWorkerAsync();
        //        ProgressForm.ShowDialog(OwnerWindow);
        //    }
        //}

        protected internal override void Worker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.RecordSetID))
            { IOPilgrimageDataFile.Export(this.RecordSetID, this.FilePath, this.Options); }
            else
            { IOPilgrimageDataFile.Export(this.SourceSubSetID, this.SelectedGeneIDs, this.SelectedResultIDs, this.FilePath, this.Options); }

            e.Result = this.FilePath;

            //Exporter.ProgressUpdate += new ProgressUpdateEventHandler(Job_ProgressUpdate);
            //Exporter.Export(this.RecordSetID, this.FilePath, this.IncludeHistory);

            //if (Exporter.CancellationPending) { e.Cancel = true; }
            //else { e.Result = this.FilePath; }
        }
    }
}
