namespace Pilgrimage.GeneSequences.BlastN
{
    partial class frmRequestDetails
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRequestDetails));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.grdQuerySequences = new System.Windows.Forms.DataGridView();
            this.clmOrganism = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmLength = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmGenBankLink_Committed = new System.Windows.Forms.DataGridViewLinkColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.txtRequestID = new System.Windows.Forms.TextBox();
            this.txtLastStatus = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtAlgorithm = new System.Windows.Forms.TextBox();
            this.txtTargetDatabase = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtStartedAt = new System.Windows.Forms.TextBox();
            this.txtEndedAt = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblStatusInformation = new System.Windows.Forms.Label();
            this.txtStatusInformation = new System.Windows.Forms.TextBox();
            this.lblSequences = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdQuerySequences)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 6;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.grdQuerySequences, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtRequestID, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtLastStatus, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label5, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtAlgorithm, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtTargetDatabase, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.label6, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtStartedAt, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtEndedAt, 5, 1);
            this.tableLayoutPanel1.Controls.Add(this.label4, 4, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblStatusInformation, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtStatusInformation, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblSequences, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.btnClose, 5, 6);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(734, 431);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // grdQuerySequences
            // 
            this.grdQuerySequences.AllowUserToAddRows = false;
            this.grdQuerySequences.AllowUserToDeleteRows = false;
            this.grdQuerySequences.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grdQuerySequences.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdQuerySequences.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmOrganism,
            this.dataGridViewTextBoxColumn1,
            this.clmLength,
            this.clmGenBankLink_Committed});
            this.tableLayoutPanel1.SetColumnSpan(this.grdQuerySequences, 6);
            this.grdQuerySequences.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdQuerySequences.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.grdQuerySequences.Location = new System.Drawing.Point(3, 154);
            this.grdQuerySequences.Name = "grdQuerySequences";
            this.grdQuerySequences.ReadOnly = true;
            this.grdQuerySequences.RowHeadersVisible = false;
            this.grdQuerySequences.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdQuerySequences.Size = new System.Drawing.Size(728, 245);
            this.grdQuerySequences.TabIndex = 15;
            // 
            // clmOrganism
            // 
            this.clmOrganism.DataPropertyName = "Organism";
            this.clmOrganism.FillWeight = 20F;
            this.clmOrganism.HeaderText = "Organism";
            this.clmOrganism.Name = "clmOrganism";
            this.clmOrganism.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Definition";
            this.dataGridViewTextBoxColumn1.FillWeight = 55F;
            this.dataGridViewTextBoxColumn1.HeaderText = "Definition";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // clmLength
            // 
            this.clmLength.DataPropertyName = "Length";
            dataGridViewCellStyle1.Format = "N0";
            this.clmLength.DefaultCellStyle = dataGridViewCellStyle1;
            this.clmLength.FillWeight = 10F;
            this.clmLength.HeaderText = "Length";
            this.clmLength.Name = "clmLength";
            this.clmLength.ReadOnly = true;
            // 
            // clmGenBankLink_Committed
            // 
            this.clmGenBankLink_Committed.DataPropertyName = "GenBankUrl";
            this.clmGenBankLink_Committed.FillWeight = 15F;
            this.clmGenBankLink_Committed.HeaderText = "GenBank";
            this.clmGenBankLink_Committed.Name = "clmGenBankLink_Committed";
            this.clmGenBankLink_Committed.ReadOnly = true;
            this.clmGenBankLink_Committed.Text = "Open GenBank Page";
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 8);
            this.label1.Margin = new System.Windows.Forms.Padding(5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Request ID:";
            // 
            // txtRequestID
            // 
            this.txtRequestID.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtRequestID.Location = new System.Drawing.Point(79, 5);
            this.txtRequestID.Margin = new System.Windows.Forms.Padding(5);
            this.txtRequestID.Name = "txtRequestID";
            this.txtRequestID.Size = new System.Drawing.Size(125, 20);
            this.txtRequestID.TabIndex = 1;
            // 
            // txtLastStatus
            // 
            this.txtLastStatus.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtLastStatus.Location = new System.Drawing.Point(79, 35);
            this.txtLastStatus.Margin = new System.Windows.Forms.Padding(5);
            this.txtLastStatus.Name = "txtLastStatus";
            this.txtLastStatus.Size = new System.Drawing.Size(125, 20);
            this.txtLastStatus.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 38);
            this.label2.Margin = new System.Windows.Forms.Padding(5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Status:";
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(444, 8);
            this.label5.Margin = new System.Windows.Forms.Padding(5);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Optimized for:";
            // 
            // txtAlgorithm
            // 
            this.txtAlgorithm.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtAlgorithm.Location = new System.Drawing.Point(525, 5);
            this.txtAlgorithm.Margin = new System.Windows.Forms.Padding(5);
            this.txtAlgorithm.Name = "txtAlgorithm";
            this.txtAlgorithm.Size = new System.Drawing.Size(125, 20);
            this.txtAlgorithm.TabIndex = 5;
            // 
            // txtTargetDatabase
            // 
            this.txtTargetDatabase.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtTargetDatabase.Location = new System.Drawing.Point(309, 5);
            this.txtTargetDatabase.Margin = new System.Windows.Forms.Padding(5);
            this.txtTargetDatabase.Name = "txtTargetDatabase";
            this.txtTargetDatabase.Size = new System.Drawing.Size(125, 20);
            this.txtTargetDatabase.TabIndex = 3;
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(214, 8);
            this.label6.Margin = new System.Windows.Forms.Padding(5);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(85, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "Database name:";
            // 
            // txtStartedAt
            // 
            this.txtStartedAt.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtStartedAt.Location = new System.Drawing.Point(309, 35);
            this.txtStartedAt.Margin = new System.Windows.Forms.Padding(5);
            this.txtStartedAt.Name = "txtStartedAt";
            this.txtStartedAt.Size = new System.Drawing.Size(125, 20);
            this.txtStartedAt.TabIndex = 9;
            // 
            // txtEndedAt
            // 
            this.txtEndedAt.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtEndedAt.Location = new System.Drawing.Point(525, 35);
            this.txtEndedAt.Margin = new System.Windows.Forms.Padding(5);
            this.txtEndedAt.Name = "txtEndedAt";
            this.txtEndedAt.Size = new System.Drawing.Size(125, 20);
            this.txtEndedAt.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(444, 38);
            this.label4.Margin = new System.Windows.Forms.Padding(5);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Ended At:";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(214, 38);
            this.label3.Margin = new System.Windows.Forms.Padding(5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Started At:";
            // 
            // lblStatusInformation
            // 
            this.lblStatusInformation.AutoSize = true;
            this.lblStatusInformation.Location = new System.Drawing.Point(5, 65);
            this.lblStatusInformation.Margin = new System.Windows.Forms.Padding(5);
            this.lblStatusInformation.Name = "lblStatusInformation";
            this.lblStatusInformation.Size = new System.Drawing.Size(62, 13);
            this.lblStatusInformation.TabIndex = 12;
            this.lblStatusInformation.Text = "Information:";
            // 
            // txtStatusInformation
            // 
            this.txtStatusInformation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.txtStatusInformation, 5);
            this.txtStatusInformation.Location = new System.Drawing.Point(79, 65);
            this.txtStatusInformation.Margin = new System.Windows.Forms.Padding(5);
            this.txtStatusInformation.Multiline = true;
            this.txtStatusInformation.Name = "txtStatusInformation";
            this.txtStatusInformation.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtStatusInformation.Size = new System.Drawing.Size(650, 58);
            this.txtStatusInformation.TabIndex = 13;
            // 
            // lblSequences
            // 
            this.lblSequences.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblSequences.AutoSize = true;
            this.lblSequences.Location = new System.Drawing.Point(5, 133);
            this.lblSequences.Margin = new System.Windows.Forms.Padding(5);
            this.lblSequences.Name = "lblSequences";
            this.lblSequences.Size = new System.Drawing.Size(64, 13);
            this.lblSequences.TabIndex = 14;
            this.lblSequences.Text = "Sequences:";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(651, 405);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 23);
            this.btnClose.TabIndex = 16;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // frmRequestDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(734, 431);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(656, 400);
            this.Name = "frmRequestDetails";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Request Details";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdQuerySequences)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtRequestID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtLastStatus;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtEndedAt;
        private System.Windows.Forms.TextBox txtStartedAt;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtAlgorithm;
        private System.Windows.Forms.TextBox txtTargetDatabase;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblStatusInformation;
        private System.Windows.Forms.TextBox txtStatusInformation;
        private System.Windows.Forms.DataGridView grdQuerySequences;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmOrganism;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmLength;
        private System.Windows.Forms.DataGridViewLinkColumn clmGenBankLink_Committed;
        private System.Windows.Forms.Label lblSequences;
    }
}