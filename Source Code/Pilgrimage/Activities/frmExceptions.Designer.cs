namespace Pilgrimage.Activities
{
    partial class frmExceptions
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
            this.lnkLink = new System.Windows.Forms.LinkLabel();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.lblHeader = new System.Windows.Forms.Label();
            this.tblForm = new System.Windows.Forms.TableLayoutPanel();
            this.grdExceptions = new System.Windows.Forms.DataGridView();
            this.clmExceptionImage = new System.Windows.Forms.DataGridViewImageColumn();
            this.clmMessage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tblForm.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExceptions)).BeginInit();
            this.SuspendLayout();
            // 
            // lnkLink
            // 
            this.lnkLink.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lnkLink.AutoSize = true;
            this.tblForm.SetColumnSpan(this.lnkLink, 3);
            this.lnkLink.Location = new System.Drawing.Point(3, 367);
            this.lnkLink.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.lnkLink.Name = "lnkLink";
            this.lnkLink.Size = new System.Drawing.Size(19, 13);
            this.lnkLink.TabIndex = 3;
            this.lnkLink.TabStop = true;
            this.lnkLink.Text = "<>";
            this.lnkLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkLink_LinkClicked);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnSave.Location = new System.Drawing.Point(3, 388);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 24);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(546, 388);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 24);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "&Close";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // txtMessage
            // 
            this.tblForm.SetColumnSpan(this.txtMessage, 3);
            this.txtMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMessage.Location = new System.Drawing.Point(3, 29);
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.ReadOnly = true;
            this.txtMessage.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtMessage.Size = new System.Drawing.Size(623, 246);
            this.txtMessage.TabIndex = 1;
            this.txtMessage.WordWrap = false;
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.tblForm.SetColumnSpan(this.lblHeader, 3);
            this.lblHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.Location = new System.Drawing.Point(5, 5);
            this.lblHeader.Margin = new System.Windows.Forms.Padding(5);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(45, 16);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "label1";
            // 
            // tblForm
            // 
            this.tblForm.ColumnCount = 3;
            this.tblForm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tblForm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblForm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tblForm.Controls.Add(this.grdExceptions, 0, 2);
            this.tblForm.Controls.Add(this.btnCancel, 2, 4);
            this.tblForm.Controls.Add(this.lblHeader, 0, 0);
            this.tblForm.Controls.Add(this.txtMessage, 0, 1);
            this.tblForm.Controls.Add(this.lnkLink, 0, 3);
            this.tblForm.Controls.Add(this.btnSave, 0, 4);
            this.tblForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblForm.Location = new System.Drawing.Point(0, 0);
            this.tblForm.Margin = new System.Windows.Forms.Padding(0);
            this.tblForm.Name = "tblForm";
            this.tblForm.RowCount = 5;
            this.tblForm.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.tblForm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tblForm.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblForm.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblForm.Size = new System.Drawing.Size(629, 416);
            this.tblForm.TabIndex = 1;
            // 
            // grdExceptions
            // 
            this.grdExceptions.AllowUserToAddRows = false;
            this.grdExceptions.AllowUserToDeleteRows = false;
            this.grdExceptions.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grdExceptions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdExceptions.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmExceptionImage,
            this.clmMessage});
            this.tblForm.SetColumnSpan(this.grdExceptions, 3);
            this.grdExceptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdExceptions.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.grdExceptions.Location = new System.Drawing.Point(3, 281);
            this.grdExceptions.Name = "grdExceptions";
            this.grdExceptions.ReadOnly = true;
            this.grdExceptions.RowHeadersVisible = false;
            this.grdExceptions.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdExceptions.Size = new System.Drawing.Size(623, 78);
            this.grdExceptions.TabIndex = 4;
            // 
            // clmExceptionImage
            // 
            this.clmExceptionImage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.clmExceptionImage.DataPropertyName = "ExceptionImage";
            this.clmExceptionImage.FillWeight = 4F;
            this.clmExceptionImage.HeaderText = "";
            this.clmExceptionImage.Name = "clmExceptionImage";
            this.clmExceptionImage.ReadOnly = true;
            this.clmExceptionImage.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.clmExceptionImage.Width = 22;
            // 
            // clmMessage
            // 
            this.clmMessage.DataPropertyName = "Message";
            this.clmMessage.FillWeight = 96F;
            this.clmMessage.HeaderText = "Message";
            this.clmMessage.Name = "clmMessage";
            this.clmMessage.ReadOnly = true;
            // 
            // frmExceptions
            // 
            this.AcceptButton = this.btnCancel;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(629, 416);
            this.Controls.Add(this.tblForm);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(400, 400);
            this.Name = "frmExceptions";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Output";
            this.tblForm.ResumeLayout(false);
            this.tblForm.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExceptions)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.LinkLabel lnkLink;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.TableLayoutPanel tblForm;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.DataGridView grdExceptions;
        private System.Windows.Forms.DataGridViewImageColumn clmExceptionImage;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmMessage;
    }
}