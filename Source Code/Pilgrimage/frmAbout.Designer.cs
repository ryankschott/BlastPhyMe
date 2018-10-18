namespace Pilgrimage
{
    partial class frmAbout
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAbout));
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.grdComponents = new System.Windows.Forms.DataGridView();
            this.pbLogo = new System.Windows.Forms.PictureBox();
            this.lblProduct = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.lnkGPL = new System.Windows.Forms.LinkLabel();
            this.lblThirdPartyComponents = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.clmLogo = new System.Windows.Forms.DataGridViewImageColumn();
            this.clmName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmVersion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmCreator = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmCopyright = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmProductURL = new System.Windows.Forms.DataGridViewLinkColumn();
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdComponents)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.grdComponents, 0, 5);
            this.tableLayoutPanel.Controls.Add(this.pbLogo, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.lblProduct, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.txtDescription, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.btnOK, 1, 6);
            this.tableLayoutPanel.Controls.Add(this.lnkGPL, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.lblThirdPartyComponents, 0, 4);
            this.tableLayoutPanel.Controls.Add(this.groupBox1, 0, 3);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 7;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(634, 372);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // grdComponents
            // 
            this.grdComponents.AllowUserToAddRows = false;
            this.grdComponents.AllowUserToDeleteRows = false;
            this.grdComponents.AllowUserToResizeRows = false;
            this.grdComponents.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grdComponents.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdComponents.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmLogo,
            this.clmName,
            this.clmVersion,
            this.clmCreator,
            this.clmCopyright,
            this.clmProductURL});
            this.tableLayoutPanel.SetColumnSpan(this.grdComponents, 2);
            this.grdComponents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdComponents.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.grdComponents.Location = new System.Drawing.Point(3, 195);
            this.grdComponents.Name = "grdComponents";
            this.grdComponents.ReadOnly = true;
            this.grdComponents.RowHeadersVisible = false;
            this.grdComponents.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdComponents.Size = new System.Drawing.Size(628, 145);
            this.grdComponents.TabIndex = 27;
            // 
            // pbLogo
            // 
            this.pbLogo.Image = ((System.Drawing.Image)(resources.GetObject("pbLogo.Image")));
            this.pbLogo.InitialImage = null;
            this.pbLogo.Location = new System.Drawing.Point(3, 3);
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.Size = new System.Drawing.Size(48, 48);
            this.pbLogo.TabIndex = 12;
            this.pbLogo.TabStop = false;
            // 
            // lblProduct
            // 
            this.lblProduct.AutoSize = true;
            this.lblProduct.Location = new System.Drawing.Point(59, 5);
            this.lblProduct.Margin = new System.Windows.Forms.Padding(5);
            this.lblProduct.Name = "lblProduct";
            this.lblProduct.Size = new System.Drawing.Size(75, 13);
            this.lblProduct.TabIndex = 19;
            this.lblProduct.Text = "Product Name";
            this.lblProduct.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtDescription
            // 
            this.tableLayoutPanel.SetColumnSpan(this.txtDescription, 2);
            this.txtDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDescription.Location = new System.Drawing.Point(3, 57);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ReadOnly = true;
            this.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtDescription.Size = new System.Drawing.Size(628, 74);
            this.txtDescription.TabIndex = 23;
            this.txtDescription.TabStop = false;
            this.txtDescription.Text = resources.GetString("txtDescription.Text");
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnOK.Location = new System.Drawing.Point(556, 346);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 24;
            this.btnOK.Text = "&OK";
            // 
            // lnkGPL
            // 
            this.lnkGPL.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lnkGPL.AutoSize = true;
            this.tableLayoutPanel.SetColumnSpan(this.lnkGPL, 2);
            this.lnkGPL.Location = new System.Drawing.Point(245, 139);
            this.lnkGPL.Margin = new System.Windows.Forms.Padding(5);
            this.lnkGPL.Name = "lnkGPL";
            this.lnkGPL.Size = new System.Drawing.Size(143, 13);
            this.lnkGPL.TabIndex = 25;
            this.lnkGPL.TabStop = true;
            this.lnkGPL.Text = "GNU General Public License";
            this.lnkGPL.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkGPL_LinkClicked);
            // 
            // lblThirdPartyComponents
            // 
            this.lblThirdPartyComponents.AutoSize = true;
            this.tableLayoutPanel.SetColumnSpan(this.lblThirdPartyComponents, 2);
            this.lblThirdPartyComponents.Location = new System.Drawing.Point(5, 174);
            this.lblThirdPartyComponents.Margin = new System.Windows.Forms.Padding(5);
            this.lblThirdPartyComponents.MaximumSize = new System.Drawing.Size(0, 17);
            this.lblThirdPartyComponents.Name = "lblThirdPartyComponents";
            this.lblThirdPartyComponents.Size = new System.Drawing.Size(285, 13);
            this.lblThirdPartyComponents.TabIndex = 21;
            this.lblThirdPartyComponents.Text = "text set in constructor";
            this.lblThirdPartyComponents.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel.SetColumnSpan(this.groupBox1, 2);
            this.groupBox1.Location = new System.Drawing.Point(5, 162);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(624, 2);
            this.groupBox1.TabIndex = 26;
            this.groupBox1.TabStop = false;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(12, 12);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // clmLogo
            // 
            this.clmLogo.DataPropertyName = "ComponentLogo";
            this.clmLogo.FillWeight = 4F;
            this.clmLogo.HeaderText = "";
            this.clmLogo.Name = "clmLogo";
            this.clmLogo.ReadOnly = true;
            // 
            // clmName
            // 
            this.clmName.DataPropertyName = "Name";
            this.clmName.FillWeight = 24F;
            this.clmName.HeaderText = "Component";
            this.clmName.Name = "clmName";
            this.clmName.ReadOnly = true;
            // 
            // clmVersion
            // 
            this.clmVersion.DataPropertyName = "Version";
            this.clmVersion.FillWeight = 12F;
            this.clmVersion.HeaderText = "Version";
            this.clmVersion.Name = "clmVersion";
            this.clmVersion.ReadOnly = true;
            // 
            // clmCreator
            // 
            this.clmCreator.DataPropertyName = "Creator";
            this.clmCreator.FillWeight = 24F;
            this.clmCreator.HeaderText = "Created By";
            this.clmCreator.Name = "clmCreator";
            this.clmCreator.ReadOnly = true;
            // 
            // clmCopyright
            // 
            this.clmCopyright.DataPropertyName = "Copyright";
            this.clmCopyright.FillWeight = 12F;
            this.clmCopyright.HeaderText = "Copyright";
            this.clmCopyright.Name = "clmCopyright";
            this.clmCopyright.ReadOnly = true;
            // 
            // clmProductURL
            // 
            this.clmProductURL.DataPropertyName = "ProductURL";
            this.clmProductURL.FillWeight = 24F;
            this.clmProductURL.HeaderText = "URL";
            this.clmProductURL.Name = "clmProductURL";
            this.clmProductURL.ReadOnly = true;
            // 
            // frmAbout
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(634, 372);
            this.Controls.Add(this.tableLayoutPanel);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(640, 400);
            this.Name = "frmAbout";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmAbout";
            this.Load += new System.EventHandler(this.frmAbout_Load);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdComponents)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label lblProduct;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.LinkLabel lnkGPL;
        private System.Windows.Forms.Label lblThirdPartyComponents;
        private System.Windows.Forms.PictureBox pbLogo;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView grdComponents;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.DataGridViewImageColumn clmLogo;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmName;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmVersion;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmCreator;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmCopyright;
        private System.Windows.Forms.DataGridViewLinkColumn clmProductURL;
    }
}
