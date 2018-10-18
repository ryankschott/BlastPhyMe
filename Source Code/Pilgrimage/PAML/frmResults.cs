using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChangLab.PAML.CodeML;

namespace Pilgrimage.PAML
{
    public partial class frmResults : AddRecordsToRecordSetForm
    {
        internal string JobID { get; set; }
        internal bool JobOnly { get; set; }
        private ChangLab.Jobs.RunTreesAtCodeML Job { get; set; }
        private DataSet ResultsData { get; set; }

        private DataGridViewHelper DataGridHelper { get; set; }

        public List<ResultRow> SelectedResults
        {
            get
            {
                return SelectedRows.Cast<ResultRow>().ToList();
            }
        }

        public frmResults(string JobID)
        {
            InitializeComponent();
            this.JobID = JobID;
            this.JobOnly = false;

            Configure(grdResults, cmbSubSets, btnSave, ChangLab.RecordSets.DataTypes.CodeMLResult);
            SetButtonImage(btnSave, "Add");
            SetButtonImage(btnReRunJob, DialogButtonPresets.Run);
            SetButtonImage(btnClose_Results, DialogButtonPresets.Close);
            SetButtonImage(btnClose_Configuration, DialogButtonPresets.Close);

            Activities.RunCodeMLAnalysis analysisInMemory = Program.InProgressActivities.GetActivityByJobID<Activities.RunCodeMLAnalysis>(JobID);
            if (analysisInMemory != null) 
            {
                Program.InProgressActivities.RemoveActivity(analysisInMemory); // We no longer need to track it because it has completed.
            }

            // Cell style configuration
            grdResults.Columns.Cast<DataGridViewColumn>()
                .TakeWhile(clm => clm != clmModel)
                .ToList()
                .ForEach(clm => clm.DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft);

            grdResults.Columns.Cast<DataGridViewColumn>()
                .SkipWhile(clm => clm != clmModel)
                .Where(clm => clm != clmValueType)
                .ToList()
                .ForEach(clm =>
                {
                    clm.DefaultCellStyle.Alignment = (clm.Name.StartsWith("Value") ? DataGridViewContentAlignment.MiddleCenter : DataGridViewContentAlignment.TopCenter);
                    clm.DefaultCellStyle.WrapMode = DataGridViewTriState.True; // To allow multiline text.
                });

            clmK.DefaultCellStyle.Format = DataGridViewHelper.DefaultDoubleFormatString;
            clmlnL.DefaultCellStyle.Format = DataGridViewHelper.DefaultDoubleFormatString;
            clmValueType.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            clmValueType.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            this.DataGridHelper = new DataGridViewHelper(this.ParentForm, grdResults, DataGridViewHelper.DataSourceTypes.Other);
            this.DataGridHelper.SelectedRowsChanged += new DataGridViewHelper.SelectedRowsChangedEventHandler(DataGridHelper_SelectedRowsChanged);
            this.DataGridHelper.ViewDetails += new DataGridViewHelper.ViewDetailsEventHandler(DataGridHelper_ViewDetails);
            
            if (!this.DesignMode)
            {
                tbForm.SelectedTab = pgResults;
                tbForm_Selected(tbForm, new TabControlEventArgs(pgResults, 0, TabControlAction.Selected));
            }
        }

        private void frmResults_Load(object sender, EventArgs e)
        {
#if DOCUMENTATION
            this.Size = new System.Drawing.Size(806, 600);
#endif

            RefreshFromDatabase(this.JobID);
        }

        internal void RefreshFromDatabase(string JobID)
        {
            this.DataGridHelper.Loaded = false;

            this.Job = new ChangLab.Jobs.RunTreesAtCodeML(JobID);
            if (this.Job.Options.KeepFolders && System.IO.Directory.Exists(this.Job.JobDirectory))
            {
                lnkWorkingFolder.Text = this.Job.JobDirectory;
                lnkWorkingFolder_Configuration.Text = this.Job.JobDirectory;
            }
            else
            {
                lnkWorkingFolder.Parent.Controls.Remove(lnkWorkingFolder);
                lnkWorkingFolder_Configuration.Parent.Controls.Remove(lnkWorkingFolder_Configuration);
            }

            if (!this.JobOnly)
            {
                DataGridViewColumn sortColumn = grdResults.SortedColumn;
                ListSortDirection sortDirection = (grdResults.SortOrder == SortOrder.Ascending ? ListSortDirection.Ascending : ListSortDirection.Descending);

                grdResults.AutoGenerateColumns = false;
                grdResults.DataSource = null;
                ResultsData = ChangLab.Jobs.RunTreesAtCodeML.ListTopResults(Program.Settings.CurrentRecordSet.ID, JobID);
                grdResults.DataSource = new SortableBindingList<ResultRow>(ResultRow.ConvertToList(ResultsData));

                if (sortColumn != null)
                {
                    grdResults.Sort(sortColumn, sortDirection);
                }
                UpdateControlsForSelectedRows();
            }
            else
            {
                tbForm.TabPages.Remove(pgResults);
                tbForm.SelectedTab = pgConfiguration;
            }
            
            this.DataGridHelper.Loaded = true;

            // Load up CodeML.exe Output so we can check for errors.
            this.OutputDataGridHelper = new DataGridViewHelper(this.ParentForm, grdOutput, DataGridViewHelper.DataSourceTypes.Other);
            this.OutputDataGridHelper.ViewDetails += new DataGridViewHelper.ViewDetailsEventHandler(OutputDataGridHelper_ViewDetails);
            clmKappa.DefaultCellStyle.Format = DataGridViewHelper.DefaultDoubleFormatString;
            clmOmega.DefaultCellStyle.Format = DataGridViewHelper.DefaultDoubleFormatString;

            this.OutputDataGridHelper.Loaded = false;

            grdOutput.AutoGenerateColumns = false;
            grdOutput.DataSource = null;
            var qry = CodeMLAnalysisOption.ListOutput(this.JobID).Rows.Cast<DataRow>()
                .Select(row =>
                {
                    CodeMLAnalysisOptionRowDataItem item = new CodeMLAnalysisOptionRowDataItem(row);
                    return item;
                });
            grdOutput.DataSource = new SortableBindingList<CodeMLAnalysisOptionRowDataItem>(qry);
            clmExceptions.Visible = qry.Any(item => item.HasExceptions);

            this.OutputDataGridHelper.Loaded = true;

            if (!clmExceptions.Visible)
            {
                pbExceptions.Parent.Controls.Remove(pbExceptions);
                lblExceptions.Parent.Controls.Remove(lblExceptions);
                sepExceptions.Parent.Controls.Remove(sepExceptions);
            }
        }

        private void tbForm_Selected(object sender, TabControlEventArgs e)
        {
            if (e.TabPage == pgResults)
            {
                this.CancelButton = btnClose_Results;
            }
            else if (e.TabPage == pgConfiguration)
            {
                if (!uctJobConfigurations1.DataSourceSet)
                {
                    uctJobConfigurations1.Refresh(Tree.ListForJob(this.JobID));
                }

                this.CancelButton = btnClose_Configuration;
            }
            else if (e.TabPage == pgOutput)
            {
                this.CancelButton = btnClose_Output;
            }
        }

        private void DataGridHelper_ViewDetails(DataGridViewHelper.ViewDetailsEventArgs e)
        {
            using (frmResultDetails frm = new frmResultDetails(((ResultRow)e.Row.DataBoundItem).ResultID))
            {
                frm.ShowDialog(this);
            }
        }

        private void chkToggleSelected_CheckedChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in grdResults.Rows)
            {
                row.Cells[this.DataGridHelper.SelectedColumnIndex].Value = chkToggleSelected.Checked;
            }

            UpdateControlsForSelectedRows();
        }

        private void UpdateControlsForSelectedRows()
        {
            chkToggleSelected.Text = (chkToggleSelected.Checked ? "Deselect all" : "Select all");
            lblSelectedRows.Text = this.SelectedRows.Count.ToString("N0") + " of " + grdResults.Rows.Count.ToString("N0") + " records selected";
            btnSave.Enabled = (SelectedRows.Count != 0);
        }

        private void DataGridHelper_SelectedRowsChanged(DataGridViewHelper.SelectedRowEventArgs e)
        {
            bool allSelected = (grdResults.Rows.Count > 0 && this.SelectedRows.Count == grdResults.Rows.Count);
            chkToggleSelected.CheckedChanged -= chkToggleSelected_CheckedChanged;

            chkToggleSelected.Checked = allSelected;
            UpdateControlsForSelectedRows();

            chkToggleSelected.CheckedChanged += new EventHandler(chkToggleSelected_CheckedChanged);
        }

        private void lnkWorkingFolder_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(lnkWorkingFolder.Text);
        }

        protected internal override void AddToButton_Click(object sender, EventArgs e)
        {
            base.AddToButton_Click(sender, e);
            if (AddRecordsAfterClick)
            {
                SelectedRows.ForEach(row =>
                {
                    Result.AddToSubSet(SelectedSubSetID, ((ResultRow)row).ResultID);
                    ((ResultRow)row).InRecordSet = true;                    
                });
                EditedSubSetIDs.Add(SelectedSubSetID);
                _selectedSubSetID = string.Empty; // Reset so as to not affect a subsequent addition to a new subset.

                Program.Settings.CurrentRecordSet.Save(); // Update the ModifiedAt value.

                if (RemoveAfterAdd)
                {
                    List<DataGridViewRow> rowsForRemoval = ResultsGrid.Rows.Cast<DataGridViewRow>().ToList().Where(row => ((RowDataItem)row.DataBoundItem).Selected).ToList();

                    DataGridViewRowsActionEventArgs args = new DataGridViewRowsActionEventArgs(rowsForRemoval);
                    OnRemovingRows(args);
                    if (!args.Cancel)
                    {
                        rowsForRemoval.ForEach(row => ResultsGrid.Rows.Remove(row));
                    }
                }
                else
                {
                    ResultsGrid.Rows.Cast<DataGridViewRow>().ToList().ForEach(row => ((RowDataItem)row.DataBoundItem).Selected = false);
                }
                ResultsGrid.Refresh();

                DataGridHelper_SelectedRowsChanged(null);
                UpdateControlsForSelectedRows();
            }
        }

        private void btnReRunJob_Click(object sender, EventArgs e)
        {
            if (Utility.ShowMessage(this.OwnerForm, "Create a new PAML job using this job's configuration?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                using (frmCreateJob frm = new frmCreateJob(JobID))
                {
                    if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                    {
                        Utility.ShowMessage(this.OwnerForm, "Job \"" + frm.Analysis.CodeMLJob.Title + "\" is running.");
                    }
                }
            }
        }

        #region Output TabPage
        private DataGridViewHelper OutputDataGridHelper { get; set; }

        private void OutputDataGridHelper_ViewDetails(DataGridViewHelper.ViewDetailsEventArgs e)
        {
            CodeMLAnalysisOptionRowDataItem row = (CodeMLAnalysisOptionRowDataItem)e.Row.DataBoundItem;

            string message = string.Empty;
            if (!string.IsNullOrWhiteSpace(row.ErrorData))
            {
                if (row.ErrorData.Trim() == row.OutputData.Trim())
                {
                    message = "An error occured when running codeml.exe:\r\n" + row.OutputData;
                }
                else
                {
                    message = "An error occured when running codeml.exe:\r\n" + row.ErrorData
                        + "\r\n" + "codeml.exe output:\r\n" + row.OutputData;
                }
            }
            else
            {
                message = row.OutputData;
            }


            using (Pilgrimage.Activities.frmExceptions frm = new Activities.frmExceptions(
                message,
                (this.Job.Options.KeepFolders && System.IO.Directory.Exists(this.Job.JobDirectory) ? this.Job.JobDirectory + "\\" + row.ProcessDirectory : string.Empty),
                CodeMLAnalysisOption.ListExceptions(row.ProcessOutputID)) { Text = "CodeML.exe Output" })
            {
                frm.ShowDialog();
            }
        }
        #endregion
    }
}