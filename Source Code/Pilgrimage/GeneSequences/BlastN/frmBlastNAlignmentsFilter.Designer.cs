namespace Pilgrimage.GeneSequences.BlastN
{
    partial class frmBlastNAlignmentsFilter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBlastNAlignmentsFilter));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.rbFilterByGenBankID = new System.Windows.Forms.RadioButton();
            this.rbFilterByOrganism = new System.Windows.Forms.RadioButton();
            this.rbNoFilter = new System.Windows.Forms.RadioButton();
            this.btnSave = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtDefinition = new System.Windows.Forms.TextBox();
            this.btnDefault = new System.Windows.Forms.Button();
            this.cmbDefinitionLogic = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.btnCancel, 3, 6);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.rbFilterByGenBankID, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.rbFilterByOrganism, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.rbNoFilter, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.btnSave, 2, 6);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtDefinition, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnDefault, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.cmbDefinitionLogic, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(6);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(876, 373);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(710, 314);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(160, 44);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label1, 2);
            this.label1.Location = new System.Drawing.Point(10, 117);
            this.label1.Margin = new System.Windows.Forms.Padding(10, 19, 10, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(165, 25);
            this.label1.TabIndex = 4;
            this.label1.Text = "Exclude results:";
            // 
            // rbFilterByGenBankID
            // 
            this.rbFilterByGenBankID.AutoSize = true;
            this.rbFilterByGenBankID.Checked = true;
            this.tableLayoutPanel1.SetColumnSpan(this.rbFilterByGenBankID, 4);
            this.rbFilterByGenBankID.Location = new System.Drawing.Point(10, 162);
            this.rbFilterByGenBankID.Margin = new System.Windows.Forms.Padding(10);
            this.rbFilterByGenBankID.Name = "rbFilterByGenBankID";
            this.rbFilterByGenBankID.Size = new System.Drawing.Size(807, 29);
            this.rbFilterByGenBankID.TabIndex = 5;
            this.rbFilterByGenBankID.TabStop = true;
            this.rbFilterByGenBankID.Text = "Only show results that do not already exist in the project, matched by Accession";
            this.rbFilterByGenBankID.UseVisualStyleBackColor = true;
            // 
            // rbFilterByOrganism
            // 
            this.rbFilterByOrganism.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.rbFilterByOrganism, 4);
            this.rbFilterByOrganism.Location = new System.Drawing.Point(10, 211);
            this.rbFilterByOrganism.Margin = new System.Windows.Forms.Padding(10);
            this.rbFilterByOrganism.Name = "rbFilterByOrganism";
            this.rbFilterByOrganism.Size = new System.Drawing.Size(855, 29);
            this.rbFilterByOrganism.TabIndex = 6;
            this.rbFilterByOrganism.Text = "Only show results that do not already exist in the project, matched by organism n" +
    "ame";
            this.rbFilterByOrganism.UseVisualStyleBackColor = true;
            this.rbFilterByOrganism.Visible = false;
            // 
            // rbNoFilter
            // 
            this.rbNoFilter.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.rbNoFilter, 2);
            this.rbNoFilter.Location = new System.Drawing.Point(10, 260);
            this.rbNoFilter.Margin = new System.Windows.Forms.Padding(10);
            this.rbNoFilter.Name = "rbNoFilter";
            this.rbNoFilter.Size = new System.Drawing.Size(194, 29);
            this.rbNoFilter.TabIndex = 7;
            this.rbNoFilter.Text = "Show all results";
            this.rbNoFilter.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnSave.Location = new System.Drawing.Point(538, 314);
            this.btnSave.Margin = new System.Windows.Forms.Padding(6);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(160, 44);
            this.btnSave.TabIndex = 9;
            this.btnSave.Text = "&Apply";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 59);
            this.label2.Margin = new System.Windows.Forms.Padding(10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(108, 25);
            this.label2.TabIndex = 1;
            this.label2.Text = "Definition:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 10);
            this.label6.Margin = new System.Windows.Forms.Padding(10);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(95, 25);
            this.label6.TabIndex = 0;
            this.label6.Text = "Filter by:";
            // 
            // txtDefinition
            // 
            this.txtDefinition.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.txtDefinition, 2);
            this.txtDefinition.Location = new System.Drawing.Point(338, 56);
            this.txtDefinition.Margin = new System.Windows.Forms.Padding(10);
            this.txtDefinition.Name = "txtDefinition";
            this.txtDefinition.Size = new System.Drawing.Size(528, 31);
            this.txtDefinition.TabIndex = 3;
            // 
            // btnDefault
            // 
            this.btnDefault.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tableLayoutPanel1.SetColumnSpan(this.btnDefault, 2);
            this.btnDefault.Location = new System.Drawing.Point(6, 314);
            this.btnDefault.Margin = new System.Windows.Forms.Padding(6);
            this.btnDefault.Name = "btnDefault";
            this.btnDefault.Size = new System.Drawing.Size(160, 44);
            this.btnDefault.TabIndex = 8;
            this.btnDefault.Text = "&Default";
            this.btnDefault.UseVisualStyleBackColor = true;
            this.btnDefault.Click += new System.EventHandler(this.btnDefault_Click);
            // 
            // cmbDefinitionLogic
            // 
            this.cmbDefinitionLogic.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cmbDefinitionLogic.DisplayMember = "Value";
            this.cmbDefinitionLogic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDefinitionLogic.FormattingEnabled = true;
            this.cmbDefinitionLogic.Location = new System.Drawing.Point(138, 55);
            this.cmbDefinitionLogic.Margin = new System.Windows.Forms.Padding(10);
            this.cmbDefinitionLogic.Name = "cmbDefinitionLogic";
            this.cmbDefinitionLogic.Size = new System.Drawing.Size(176, 33);
            this.cmbDefinitionLogic.TabIndex = 2;
            this.cmbDefinitionLogic.ValueMember = "Key";
            // 
            // frmBlastNAlignmentsFilter
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(876, 373);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(12);
            this.MinimumSize = new System.Drawing.Size(882, 203);
            this.Name = "frmBlastNAlignmentsFilter";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Filter BLASTN Alignments";
            this.Load += new System.EventHandler(this.frmBlastNAlignmentsFilter_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rbFilterByGenBankID;
        private System.Windows.Forms.RadioButton rbFilterByOrganism;
        private System.Windows.Forms.RadioButton rbNoFilter;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        internal System.Windows.Forms.TextBox txtDefinition;
        private System.Windows.Forms.Button btnDefault;
        private System.Windows.Forms.ComboBox cmbDefinitionLogic;
    }
}