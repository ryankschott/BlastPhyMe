namespace Pilgrimage.GeneSequences
{
    partial class frmFormatFieldNames
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmFormatFieldNames));
            this.lstFieldNames = new System.Windows.Forms.ListBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txtExample = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtFormat = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lstFieldNames
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.lstFieldNames, 2);
            this.lstFieldNames.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstFieldNames.FormattingEnabled = true;
            this.lstFieldNames.Items.AddRange(new object[] {
            "{Definition}",
            "{Source}",
            "{GenBank ID}",
            "{Locus}",
            "{Accession}",
            "{Organism}",
            "{Gene}"});
            this.lstFieldNames.Location = new System.Drawing.Point(3, 68);
            this.lstFieldNames.Name = "lstFieldNames";
            this.lstFieldNames.Size = new System.Drawing.Size(448, 98);
            this.lstFieldNames.TabIndex = 2;
            this.lstFieldNames.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstFieldNames_MouseDoubleClick);
            this.lstFieldNames.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lstFieldNames_MouseDown);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.txtExample, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.lstFieldNames, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtFormat, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.btnCancel, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.btnOK, 0, 7);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 8;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(454, 321);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // txtExample
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.txtExample, 2);
            this.txtExample.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtExample.Location = new System.Drawing.Point(3, 256);
            this.txtExample.Multiline = true;
            this.txtExample.Name = "txtExample";
            this.txtExample.ReadOnly = true;
            this.txtExample.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtExample.Size = new System.Drawing.Size(448, 32);
            this.txtExample.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 47);
            this.label1.Margin = new System.Windows.Forms.Padding(5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Field Names:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label2, 2);
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(5, 5);
            this.label2.Margin = new System.Windows.Forms.Padding(5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(442, 32);
            this.label2.TabIndex = 0;
            this.label2.Text = "Build a format string using pre-defined field names, which will be replaced with " +
    "their corresponding value from the gene sequence, and static text.";
            // 
            // txtFormat
            // 
            this.txtFormat.AllowDrop = true;
            this.tableLayoutPanel1.SetColumnSpan(this.txtFormat, 2);
            this.txtFormat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtFormat.Location = new System.Drawing.Point(3, 195);
            this.txtFormat.Multiline = true;
            this.txtFormat.Name = "txtFormat";
            this.txtFormat.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtFormat.Size = new System.Drawing.Size(448, 32);
            this.txtFormat.TabIndex = 4;
            this.txtFormat.TextChanged += new System.EventHandler(this.txtFormat_TextChanged);
            this.txtFormat.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtFormat_DragDrop);
            this.txtFormat.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtFormat_DragEnter);
            this.txtFormat.DragOver += new System.Windows.Forms.DragEventHandler(this.txtFormat_DragOver);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 174);
            this.label3.Margin = new System.Windows.Forms.Padding(5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Format string:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 235);
            this.label4.Margin = new System.Windows.Forms.Padding(5);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Example:";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(371, 294);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 24);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(285, 294);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 24);
            this.btnOK.TabIndex = 7;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // frmFormatFieldNames
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(454, 321);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(470, 355);
            this.Name = "frmFormatFieldNames";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Build Format String";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lstFieldNames;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtExample;
        private System.Windows.Forms.TextBox txtFormat;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
    }
}