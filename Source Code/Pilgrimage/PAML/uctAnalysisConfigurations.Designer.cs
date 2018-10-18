namespace Pilgrimage.PAML
{
    partial class uctAnalysisConfigurations
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
            this.grdConfigurations = new System.Windows.Forms.DataGridView();
            this.clmModel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmNSSites = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmNCatG = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmKappa = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmOmega = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.grdConfigurations)).BeginInit();
            this.SuspendLayout();
            // 
            // grdConfigurations
            // 
            this.grdConfigurations.AllowUserToAddRows = false;
            this.grdConfigurations.AllowUserToDeleteRows = false;
            this.grdConfigurations.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grdConfigurations.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdConfigurations.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmModel,
            this.clmNSSites,
            this.clmNCatG,
            this.clmKappa,
            this.clmOmega});
            this.grdConfigurations.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdConfigurations.Location = new System.Drawing.Point(0, 0);
            this.grdConfigurations.MultiSelect = false;
            this.grdConfigurations.Name = "grdConfigurations";
            this.grdConfigurations.ReadOnly = true;
            this.grdConfigurations.RowHeadersVisible = false;
            this.grdConfigurations.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdConfigurations.Size = new System.Drawing.Size(638, 391);
            this.grdConfigurations.TabIndex = 0;
            // 
            // clmModel
            // 
            this.clmModel.DataPropertyName = "Model";
            this.clmModel.FillWeight = 30F;
            this.clmModel.HeaderText = "Model";
            this.clmModel.Name = "clmModel";
            this.clmModel.ReadOnly = true;
            // 
            // clmNSSites
            // 
            this.clmNSSites.DataPropertyName = "NSSites";
            this.clmNSSites.FillWeight = 15F;
            this.clmNSSites.HeaderText = "Site Models";
            this.clmNSSites.Name = "clmNSSites";
            this.clmNSSites.ReadOnly = true;
            // 
            // clmNCatG
            // 
            this.clmNCatG.DataPropertyName = "NCatG";
            this.clmNCatG.FillWeight = 15F;
            this.clmNCatG.HeaderText = "Categories";
            this.clmNCatG.Name = "clmNCatG";
            this.clmNCatG.ReadOnly = true;
            // 
            // clmKappa
            // 
            this.clmKappa.DataPropertyName = "KappaDescription";
            this.clmKappa.FillWeight = 20F;
            this.clmKappa.HeaderText = "Kappa";
            this.clmKappa.Name = "clmKappa";
            this.clmKappa.ReadOnly = true;
            // 
            // clmOmega
            // 
            this.clmOmega.DataPropertyName = "OmegaDescription";
            this.clmOmega.FillWeight = 20F;
            this.clmOmega.HeaderText = "Omega";
            this.clmOmega.Name = "clmOmega";
            this.clmOmega.ReadOnly = true;
            // 
            // uctAnalysisConfigurations
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grdConfigurations);
            this.Name = "uctAnalysisConfigurations";
            this.Size = new System.Drawing.Size(638, 391);
            ((System.ComponentModel.ISupportInitialize)(this.grdConfigurations)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView grdConfigurations;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmModel;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmNSSites;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmNCatG;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmKappa;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmOmega;
    }
}
