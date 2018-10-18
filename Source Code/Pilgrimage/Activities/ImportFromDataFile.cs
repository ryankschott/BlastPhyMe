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
    public class ImportFromDataFile : Activity
    {
        private string FilePath { get; set; }
        private string RecordSetName { get; set; }
        private string CurrentRecordSetID { get; set; }
        internal string CurrentSubSetID { get; set; }
        private IOPilgrimageDataFile Importer { get; set; }

        public string NewRecordSetID { get; set; }
        public string ImportJobID { get; set; }

        public string RecordSetID { get; private set; }

        public ImportFromDataFile(IWin32Window OwnerWindow) : base(OwnerWindow) { }

        public void Import(string FilePath, string RecordSetName, string CurrentRecordSetID, string CurrentSubSetID)
        {
            this.FilePath = FilePath;
            this.RecordSetName = RecordSetName;
            this.CurrentRecordSetID = CurrentRecordSetID;
            this.CurrentSubSetID = CurrentSubSetID;

            using (ProgressForm = new frmProgress("Importing...", new frmProgress.ProgressOptions() { AllowCancellation = false, ShowTotalProgress = false, ShowCurrentProgress = false, UseNeverEndingTimer = true }))
            {
                ProgressForm.Cancelled += new frmProgress.CancelledEventHandler(ProgressForm_Cancelled);

                Worker.RunWorkerAsync();
                ProgressForm.ShowDialog(OwnerWindow);
            }
        }

        //public void Import(string FilePath, string RecordSetName)
        //{
        //    this.FilePath = FilePath;
        //    this.RecordSetName = RecordSetName;
        //    this.Importer = new IOPilgrimageDataFile();
        //    this.CurrentJob = this.Importer;

        //    using (ProgressForm = new frmProgress("Importing Recordset", new frmProgress.ProgressOptions() { AllowCancellation = true, PrintProgressMessages = true, ShowTotalProgress = true }))
        //    {
        //        ProgressForm.Cancelled += new frmProgress.CancelledEventHandler(ProgressForm_Cancelled);

        //        Worker.RunWorkerAsync();
        //        ProgressForm.ShowDialog(OwnerWindow);
        //    }
        //}

        protected internal override void Worker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            string newRecordSetID = string.Empty;
            string jobId = string.Empty;

            IOPilgrimageDataFile.Import(this.FilePath, this.RecordSetName, this.CurrentRecordSetID, this.CurrentSubSetID, out newRecordSetID, out jobId);
            this.ImportJobID = jobId;

            Job job = new Job(this.ImportJobID);
            e.Result = job.Status;

            switch (job.Status)
            {
                case JobStatuses.Completed:
                    this.NewRecordSetID = newRecordSetID;
                    break;
                case JobStatuses.Failed:
                    throw new JobException(jobId, 0, "Job failed");
                case JobStatuses.Cancelled:
                    e.Cancel = true;
                    break;
            }

            //Importer.ProgressUpdate += new ProgressUpdateEventHandler(Job_ProgressUpdate);
            //Importer.Import(this.FilePath, this.RecordSetName);

            //if (Importer.CancellationPending) { e.Cancel = true; }
            //else { e.Result = Importer.RecordSetID; }
        }
    }
}
