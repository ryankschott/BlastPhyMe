using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChangLab.Common;
using ChangLab.PAML.CodeML;

namespace Pilgrimage.PAML
{
    public partial class frmResultDetails : DialogForm
    {
        private DataGridViewHelper DataGridHelper { get; set; }
        private List<ResultRow> Results { get; set; }
        internal List<string> EditedSubSetIDs { get; set; }

        public frmResultDetails(int ResultID)
        {
            InitializeComponent();
            SetButtonImage(btnClose, DialogButtonPresets.Close);
            EditedSubSetIDs = new List<string>();

            DataSet details = Result.Details(ResultID);
            
            DataRow header = details.Tables[0].Rows[0];
            txtTreeTitle.Text = (string)header["TreeTitle"];
            txtTreeFilePath.Text = (string)header["TreeFilePath"];
            txtSequenceCountAndLength.Text = ((int)header["SequenceCount"]).ToString() + " @ " + ((int)header["SequenceLength"]).ToString() + "bp";
            txtSequencesFilePath.Text = (string)header["SequencesFilePath"];

            if (this.OwnerForm.GetType() == typeof(frmResults)) { lnkViewJob.Visible = false; }
            else { lnkViewJob.Tag = header["JobID"].ToString(); }

            ModelPreset preset = ModelPreset.Derive((ModelPresets)Enum.Parse(typeof(ModelPresets), (string)header["ModelPresetKey"]));
            txtModel.Text = preset.ShortName + (preset.Key == ModelPresets.Model0 ? " (M" + ((int)header["NSSite"]).ToString() + ")" : string.Empty);
            txtNCatG.Text = ((int)header["NCatG"]).ToString();
            txtKappa.Text = AnalysisConfiguration.KappaOmegaDescription(header.ToSafeDouble("KStart"), header.ToSafeDouble("KEnd"), header.ToSafeDouble("KInterval"), (bool)header["KFixed"], DataGridViewHelper.DefaultDoubleFormatString);
            txtOmega.Text = AnalysisConfiguration.KappaOmegaDescription(header.ToSafeDouble("WStart"), header.ToSafeDouble("WEnd"), header.ToSafeDouble("WInterval"), (bool)header["WFixed"], DataGridViewHelper.DefaultDoubleFormatString);

            // grdResults
            details.Relations.Add(new DataRelation("FK_ResultID", details.Tables[1].Columns["ResultID"], details.Tables[2].Columns["ResultID"]));
            Results =
                details.Tables[1].Rows.Cast<DataRow>().Select(headerRow =>
                {
                    ResultRow result = new ResultRow() { 
                        ResultID = (int)headerRow["ResultID"],
                        Model = preset,
                        NSSite = (int)header["NSSite"],
                        np = (int)headerRow["np"],
                        lnL = headerRow.ToSafeDouble("lnL"),
                        k = headerRow.ToSafeDouble("k"),
                        Kappa = headerRow.ToSafeDouble("Kappa"),
                        Omega = headerRow.ToSafeDouble("Omega")
                    };
                    ResultRow.SetValues(headerRow, ref result);
                    return result;
                }).ToList();

            // Cell style configuration
            grdResults.Columns.Cast<DataGridViewColumn>()
                .ToList()
                .ForEach(clm =>
                {
                    clm.DefaultCellStyle.Alignment = (clm != clmValueHeader ? DataGridViewContentAlignment.TopCenter : DataGridViewContentAlignment.TopRight);
                    clm.DefaultCellStyle.WrapMode = DataGridViewTriState.True; // To allow multiline text.
                });
            clmK.DefaultCellStyle.Format = DataGridViewHelper.DefaultDoubleFormatString;
            clmLnL.DefaultCellStyle.Format = DataGridViewHelper.DefaultDoubleFormatString;

            this.DataGridHelper = new DataGridViewHelper(this.ParentForm, grdResults, DataGridViewHelper.DataSourceTypes.Other);
        }

        private void frmResultDetails_Load(object sender, EventArgs e)
        {
            this.DataGridHelper.Loaded = false;
            grdResults.AutoGenerateColumns = false;
            grdResults.DataSource = null;
            grdResults.DataSource = new SortableBindingList<ResultRow>(Results);
            grdResults.SelectedRows.Cast<DataGridViewRow>().ToList().ForEach(row => row.Selected = false);
            this.DataGridHelper.Loaded = true;
        }

        private void lnkViewJob_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (frmResults frm = new frmResults((string)lnkViewJob.Tag) { JobOnly = true })
            {
                frm.ShowDialog(this);
                this.EditedSubSetIDs.AddRange(frm.EditedSubSetIDs.Where(id => !this.EditedSubSetIDs.Contains(id)));
            }
        }
    }
}