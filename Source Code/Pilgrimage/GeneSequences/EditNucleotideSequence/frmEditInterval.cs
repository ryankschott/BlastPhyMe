using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChangLab.Genes;

namespace Pilgrimage.GeneSequences.EditNucleotideSequence
{
    public partial class frmEditInterval : DialogForm
    {
        private NucleotideSequence SourceSequence { get; set; }
        public FeatureInterval Interval { get; set; }
        private FeatureInterval OriginalIntervalSettings { get; set; }
        private bool SuspendEvents { get; set; }

        public frmEditInterval()
        {
            InitializeComponent();
            SetButtonImage(btnCancel, DialogButtonPresets.Cancel);
        }

        public frmEditInterval(NucleotideSequence SourceSequence, FeatureInterval Interval, bool Add, bool Editable) : this()
        {
            if (Add)
            {
                this.Text = "Add Interval";
                btnSave.Text = "&Add";
                SetButtonImage(btnSave, DialogButtonPresets.Add);
            }
            else
            {
                this.Text = "Edit Interval";
                btnSave.Text = "&Edit";
                SetButtonImage(btnSave, DialogButtonPresets.Edit);
            }

            this.SourceSequence = SourceSequence;
            rtfNucleotideSequence.Text = this.SourceSequence.ToString();

            SuspendEvents = true;
            if (Interval == null)
            {
                this.Interval = new FeatureInterval();
            
                numStartIndex.Minimum = this.SourceSequence.Start;
                numStartIndex.Maximum = this.SourceSequence.End;
                numStartIndex.Value = this.SourceSequence.Start;
                this.Interval.Start = Convert.ToInt32(numStartIndex.Value);

                numEndIndex.Minimum = this.SourceSequence.Start;
                numEndIndex.Maximum = this.SourceSequence.End;
                numEndIndex.Value = this.SourceSequence.Start;
                this.Interval.End = Convert.ToInt32(numEndIndex.Value);
                
                chkComplement.Checked = false;
                this.Interval.IsComplement = chkComplement.Checked;
            }
            else
            {
                this.Interval = Interval;
                OriginalIntervalSettings = new FeatureInterval(); OriginalIntervalSettings.Merge(this.Interval);
                
                numStartIndex.Minimum = this.SourceSequence.Start;
                numStartIndex.Maximum = this.SourceSequence.End;
                numStartIndex.Value = this.Interval.Start;

                numEndIndex.Minimum = this.SourceSequence.Start;
                numEndIndex.Maximum = this.SourceSequence.End;
                numEndIndex.Value = this.Interval.End;

                chkComplement.Checked = this.Interval.IsComplement;
            }
            SuspendEvents = false;

            numStartIndex.Enabled = Editable;
            numEndIndex.Enabled = Editable;
            chkComplement.Enabled = Editable;
            lnkCaptureSelection.Visible = Editable;
            btnSave.Visible = Editable;

            RefreshPreview();
        }

        private void RefreshPreview()
        {
            // Clear the selection and highlighting
            rtfNucleotideSequence.SelectAll();
            rtfNucleotideSequence.SelectionBackColor = rtfNucleotideSequence.BackColor;
            rtfNucleotideSequence.Select(0, 0);

            // Allow for reversed sequence annotation
            int start = 0; int end = 0;
            if (this.Interval.End >= this.Interval.Start) { start = this.Interval.Start; end = this.Interval.End; }
            else { start = this.Interval.End; end = this.Interval.Start; }

            rtfNucleotideSequence.Select(start - this.SourceSequence.Start, this.Interval.Length);
            rtfNucleotideSequence.SelectionBackColor = Color.PaleGoldenrod;
            rtfNucleotideSequence.Select(start - this.SourceSequence.Start, 0);
            rtfNucleotideSequence.ScrollToCaret();

            txtIntervalSequence.Text = this.SourceSequence.SubSequence(start, end, chkComplement.Checked);
        }

        private void numStartIndex_ValueChanged(object sender, EventArgs e)
        {
            if (SuspendEvents) { return; }

            this.Interval.Start = Convert.ToInt32(numStartIndex.Value);
            RefreshPreview();
        }

        private void numEndIndex_ValueChanged(object sender, EventArgs e)
        {
            if (SuspendEvents) { return; }

            this.Interval.End = Convert.ToInt32(numEndIndex.Value);
            RefreshPreview();
        }

        private void chkComplement_ValueChanged(object sender, EventArgs e)
        {
            this.Interval.IsComplement = chkComplement.Checked;
            RefreshPreview();
        }

        private void lnkCaptureSelection_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (rtfNucleotideSequence.SelectionLength == 0) { Utility.ShowMessage(this, "No nucleotides have been selected."); }
            else
            {
                SuspendEvents = true;
                numStartIndex.Value = (rtfNucleotideSequence.SelectionStart + this.SourceSequence.Start);
                this.Interval.Start = Convert.ToInt32(numStartIndex.Value); 
                numEndIndex.Value = (rtfNucleotideSequence.SelectionStart + (rtfNucleotideSequence.SelectionLength - 1) + this.SourceSequence.Start);
                this.Interval.End = Convert.ToInt32(numEndIndex.Value);
                SuspendEvents = false;
                RefreshPreview();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // See the comments in frmNucleotideSequenceFeatures.btnEditInterval_Click() as to what's going on here.
            this.Interval.Merge(this.OriginalIntervalSettings);
        }
    }
}
