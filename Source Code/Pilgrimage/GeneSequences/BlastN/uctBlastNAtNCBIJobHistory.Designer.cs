namespace Pilgrimage
{
    partial class uctBlastNAtNCBIJobHistory
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.grdJobHistory = new System.Windows.Forms.DataGridView();
            this.clmBlastNHistory_Selected = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.clmBlastNHistory_StartedAt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmBlastNHistory_Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmBlastNHistory_InputGenes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmBlastNHistory_EndedAt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.grdJobHistory)).BeginInit();
            this.SuspendLayout();
            // 
            // grdBlastNHistory
            // 
            this.grdJobHistory.AllowUserToAddRows = false;
            this.grdJobHistory.AllowUserToDeleteRows = false;
            this.grdJobHistory.AllowUserToResizeRows = false;
            this.grdJobHistory.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grdJobHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdJobHistory.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmBlastNHistory_Selected,
            this.clmBlastNHistory_StartedAt,
            this.clmBlastNHistory_Status,
            this.clmBlastNHistory_InputGenes,
            this.clmBlastNHistory_EndedAt});
            this.grdJobHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdJobHistory.Location = new System.Drawing.Point(0, 0);
            this.grdJobHistory.Name = "grdBlastNHistory";
            this.grdJobHistory.RowHeadersVisible = false;
            this.grdJobHistory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdJobHistory.Size = new System.Drawing.Size(1009, 330);
            this.grdJobHistory.TabIndex = 1;
            this.grdJobHistory.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.grdJobHistory_CellFormatting);
            // 
            // clmBlastNHistory_Selected
            // 
            this.clmBlastNHistory_Selected.DataPropertyName = "Selected";
            this.clmBlastNHistory_Selected.FillWeight = 3F;
            this.clmBlastNHistory_Selected.HeaderText = "";
            this.clmBlastNHistory_Selected.Name = "clmBlastNHistory_Selected";
            // 
            // clmBlastNHistory_StartedAt
            // 
            this.clmBlastNHistory_StartedAt.DataPropertyName = "StartedAt";
            dataGridViewCellStyle1.Format = "G";
            dataGridViewCellStyle1.NullValue = null;
            this.clmBlastNHistory_StartedAt.DefaultCellStyle = dataGridViewCellStyle1;
            this.clmBlastNHistory_StartedAt.FillWeight = 15F;
            this.clmBlastNHistory_StartedAt.HeaderText = "Started At";
            this.clmBlastNHistory_StartedAt.Name = "clmBlastNHistory_StartedAt";
            this.clmBlastNHistory_StartedAt.ReadOnly = true;
            // 
            // clmBlastNHistory_Status
            // 
            this.clmBlastNHistory_Status.DataPropertyName = "StatusName";
            this.clmBlastNHistory_Status.FillWeight = 15F;
            this.clmBlastNHistory_Status.HeaderText = "Status";
            this.clmBlastNHistory_Status.Name = "clmBlastNHistory_Status";
            this.clmBlastNHistory_Status.ReadOnly = true;
            // 
            // clmBlastNHistory_InputGenes
            // 
            this.clmBlastNHistory_InputGenes.DataPropertyName = "InputDescription";
            this.clmBlastNHistory_InputGenes.FillWeight = 21F;
            this.clmBlastNHistory_InputGenes.HeaderText = "Query Sequences";
            this.clmBlastNHistory_InputGenes.Name = "clmBlastNHistory_InputGenes";
            this.clmBlastNHistory_InputGenes.ReadOnly = true;
            // 
            // clmBlastNHistory_EndedAt
            // 
            this.clmBlastNHistory_EndedAt.DataPropertyName = "EndedAt";
            dataGridViewCellStyle2.Format = "G";
            this.clmBlastNHistory_EndedAt.DefaultCellStyle = dataGridViewCellStyle2;
            this.clmBlastNHistory_EndedAt.FillWeight = 46F;
            this.clmBlastNHistory_EndedAt.HeaderText = "Ended At";
            this.clmBlastNHistory_EndedAt.Name = "clmBlastNHistory_EndedAt";
            this.clmBlastNHistory_EndedAt.ReadOnly = true;
            // 
            // uctBlastNAtNCBIJobHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grdJobHistory);
            this.Name = "uctBlastNAtNCBIJobHistory";
            this.Size = new System.Drawing.Size(1009, 330);
            ((System.ComponentModel.ISupportInitialize)(this.grdJobHistory)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView grdJobHistory;
        private System.Windows.Forms.DataGridViewCheckBoxColumn clmBlastNHistory_Selected;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmBlastNHistory_StartedAt;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmBlastNHistory_Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmBlastNHistory_InputGenes;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmBlastNHistory_EndedAt;
    }
}
