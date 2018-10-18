using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChangLab.PAML.CodeML;
using ChangLab.Jobs;

namespace Pilgrimage.PAML
{
    public partial class frmExportToExcel : DialogForm
    {
        private List<ResultSummaryRow> ResultRows { get; set; }
        private List<string> SitesTrees { get; set; }
        internal bool UserInputNeeded { get; private set; }

        private string _sitesTreeForLRT;
        private string SitesTreeForLRT
        {
            get
            {
                if (!string.IsNullOrEmpty(_sitesTreeForLRT)) { return _sitesTreeForLRT; }
                else
                {
                    if (SitesTrees.Count > 1)
                    { return (string)cmbSitesTrees.SelectedValue; }
                    else
                    { return string.Empty; }
                }
            }
        }
        
        public frmExportToExcel(List<ResultSummaryRow> ResultRows)
        {
            InitializeComponent();
            SetButtonImage(btnExport, "Excel");
            SetButtonImage(btnCancel, DialogButtonPresets.Cancel);
            this.ResultRows = ResultRows;
            this.UserInputNeeded = false;

            Dictionary<ModelPresets, string> modelGroups = new Dictionary<ModelPresets,string>();
            modelGroups.Add(ModelPresets.Model0, "Sites"); modelGroups.Add(ModelPresets.Model2a, "Sites"); modelGroups.Add(ModelPresets.Model8a, "Sites");
            modelGroups.Add(ModelPresets.Branch, "Branch"); modelGroups.Add(ModelPresets.BranchNull, "Branch");
            modelGroups.Add(ModelPresets.BranchSite, "BranchSite"); modelGroups.Add(ModelPresets.BranchSiteNull, "BranchSite");
            modelGroups.Add(ModelPresets.CmC, "CmC"); modelGroups.Add(ModelPresets.CmCNull, "CmC");
            modelGroups.Add(ModelPresets.CmD, "CmD"); modelGroups.Add(ModelPresets.CmDNull, "CmD");

            bool hasNonSites = (ResultRows.Any(result => modelGroups[result.ModelPresetKey] != "Sites"));
            bool hasCmC = (ResultRows.Any(result => modelGroups[result.ModelPresetKey] == "CmC"));
            bool hasCmD = (ResultRows.Any(result => modelGroups[result.ModelPresetKey] == "CmD"));

            this.SitesTrees = ResultRows
                .Where(result => modelGroups[result.ModelPresetKey] == "Sites")
                .GroupBy(result => result.TreeTitle)
                .Where(g =>
                        g.Any(result => result.ModelPresetKey == ModelPresets.Model2a) // Needed for Br, BrS, CmC, and CmD
                        && ((!hasCmC && !hasCmD)
                                || (g.Any(result => result.ModelPresetKey == ModelPresets.Model0 && result.NSSite == 1)
                                        && // Needed only for CmC or CmD
                                    g.Any(result => result.ModelPresetKey == ModelPresets.Model0 && result.NSSite == 2))
                        ))
                .Select(g => g.Key).ToList();

            if (this.SitesTrees.Count != 0)
            {
                if (hasNonSites && this.SitesTrees.Count > 1)
                { this.UserInputNeeded = true; }
                else
                { this._sitesTreeForLRT = this.SitesTrees[0]; }
            }
        }

        private void frmExportToExcel_Load(object sender, EventArgs e)
        {
            cmbSitesTrees.DataSource = new BindingSource(this.SitesTrees, null);
            cmbSitesTrees.SelectedIndex = 0;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        internal void Export()
        {
            using (ProgressForm = new frmProgress("Exporting to Excel", new frmProgress.ProgressOptions() { AllowCancellation = false, UseNeverEndingTimer = true }))
            {
                bwExport.RunWorkerAsync();
                ProgressForm.ShowDialog();
            }
        }

        private void bwExport_DoWork(object sender, DoWorkEventArgs e)
        {
            using (DataSet results = RunTreesAtCodeML.ListTopResults(Program.Settings.CurrentRecordSet.ID, this.ResultRows.Select(result => result.ResultID).ToList()))
            {
                (new ExportToExcel(results, this.SitesTreeForLRT)).ExportAndOpen();
            }
        }

        private void bwExport_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            CloseProgressForm(System.Windows.Forms.DialogResult.OK);

            if (e.Error != null)
            {
                Utility.ShowErrorMessage(this.OwnerForm, e.Error);
            }

            // We don't really need to do anything else because Excel will open by itself at the end of ExportToExcel.ExportAndOpen()
        }
    }
}
