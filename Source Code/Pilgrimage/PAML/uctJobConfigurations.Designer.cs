namespace Pilgrimage.PAML
{
    partial class uctJobConfigurations
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
            this.grdConfiguration = new System.Windows.Forms.DataGridView();
            this.clmTree = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmSequences = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmModel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmNSSites = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmNCatG = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmKappa = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmOmega = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.grdConfiguration)).BeginInit();
            this.SuspendLayout();
            // 
            // grdConfiguration
            // 
            this.grdConfiguration.AllowUserToAddRows = false;
            this.grdConfiguration.AllowUserToDeleteRows = false;
            this.grdConfiguration.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grdConfiguration.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdConfiguration.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmTree,
            this.clmSequences,
            this.clmTitle,
            this.clmModel,
            this.clmNSSites,
            this.clmNCatG,
            this.clmKappa,
            this.clmOmega});
            this.grdConfiguration.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdConfiguration.Location = new System.Drawing.Point(0, 0);
            this.grdConfiguration.Name = "grdConfiguration";
            this.grdConfiguration.ReadOnly = true;
            this.grdConfiguration.RowHeadersVisible = false;
            this.grdConfiguration.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdConfiguration.Size = new System.Drawing.Size(743, 309);
            this.grdConfiguration.TabIndex = 1;
            // 
            // clmTree
            // 
            this.clmTree.DataPropertyName = "TreeFileName";
            this.clmTree.FillWeight = 16F;
            this.clmTree.HeaderText = "Tree";
            this.clmTree.Name = "clmTree";
            this.clmTree.ReadOnly = true;
            // 
            // clmSequences
            // 
            this.clmSequences.DataPropertyName = "SequencesFileName";
            this.clmSequences.FillWeight = 16F;
            this.clmSequences.HeaderText = "Sequences";
            this.clmSequences.Name = "clmSequences";
            this.clmSequences.ReadOnly = true;
            // 
            // clmTitle
            // 
            this.clmTitle.DataPropertyName = "Title";
            this.clmTitle.FillWeight = 16F;
            this.clmTitle.HeaderText = "Title";
            this.clmTitle.Name = "clmTitle";
            this.clmTitle.ReadOnly = true;
            // 
            // clmModel
            // 
            this.clmModel.DataPropertyName = "Model";
            this.clmModel.FillWeight = 12F;
            this.clmModel.HeaderText = "Model";
            this.clmModel.Name = "clmModel";
            this.clmModel.ReadOnly = true;
            // 
            // clmNSSites
            // 
            this.clmNSSites.DataPropertyName = "NSSites";
            this.clmNSSites.FillWeight = 12F;
            this.clmNSSites.HeaderText = "Site Models";
            this.clmNSSites.Name = "clmNSSites";
            this.clmNSSites.ReadOnly = true;
            // 
            // clmNCatG
            // 
            this.clmNCatG.DataPropertyName = "NCatG";
            this.clmNCatG.FillWeight = 8F;
            this.clmNCatG.HeaderText = "Categories";
            this.clmNCatG.Name = "clmNCatG";
            this.clmNCatG.ReadOnly = true;
            // 
            // clmKappa
            // 
            this.clmKappa.DataPropertyName = "KappaDescription";
            this.clmKappa.FillWeight = 10F;
            this.clmKappa.HeaderText = "Kappa";
            this.clmKappa.Name = "clmKappa";
            this.clmKappa.ReadOnly = true;
            // 
            // clmOmega
            // 
            this.clmOmega.DataPropertyName = "OmegaDescription";
            this.clmOmega.FillWeight = 10F;
            this.clmOmega.HeaderText = "Omega";
            this.clmOmega.Name = "clmOmega";
            this.clmOmega.ReadOnly = true;
            // 
            // uctJobConfigurations
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grdConfiguration);
            this.Name = "uctJobConfigurations";
            this.Size = new System.Drawing.Size(743, 309);
            ((System.ComponentModel.ISupportInitialize)(this.grdConfiguration)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView grdConfiguration;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmTree;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmSequences;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmTitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmModel;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmNSSites;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmNCatG;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmKappa;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmOmega;
    }
}
