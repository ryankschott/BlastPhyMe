using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChangLab.Common;
using ChangLab.Jobs;

namespace Pilgrimage.Activities
{
    public class ExportToExcel : Activity
    {
        private ExportGeneSequencesToTableFile Exporter { get; set; }

        public ExportToExcel(IWin32Window OwnerWindow) : base(OwnerWindow) { }

        public void Export(IEnumerable<string> GeneIDs, Dictionary<string, string> Columns, string FilePath)
        {
            this.Exporter = new ExportGeneSequencesToTableFile(GeneIDs, Columns, FilePath);

            using (ProgressForm = new frmProgress("Exporting Sequences", new frmProgress.ProgressOptions() { AllowCancellation = false }))
            {
                Worker.RunWorkerAsync();
                ProgressForm.ShowDialog(OwnerWindow);
            }
        }

        protected internal override void Worker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            Exporter.ProgressUpdate += new ProgressUpdateEventHandler(Job_ProgressUpdate);
            Exporter.Export();

            if (Exporter.CancellationPending) { e.Cancel = true; }
            else { e.Result = Exporter.FilePath; }
        }
    }
}
