namespace Pilgrimage
{
    partial class DialogForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogForm));
            this.ilButtons = new System.Windows.Forms.ImageList(this.components);
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.FormToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.timFocus = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // ilButtons
            // 
            this.ilButtons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilButtons.ImageStream")));
            this.ilButtons.TransparentColor = System.Drawing.Color.Transparent;
            this.ilButtons.Images.SetKeyName(0, "Add");
            this.ilButtons.Images.SetKeyName(1, "Add_RecordSet");
            this.ilButtons.Images.SetKeyName(2, "Back");
            this.ilButtons.Images.SetKeyName(3, "Cancel");
            this.ilButtons.Images.SetKeyName(4, "Copy");
            this.ilButtons.Images.SetKeyName(5, "DB");
            this.ilButtons.Images.SetKeyName(6, "Delete");
            this.ilButtons.Images.SetKeyName(7, "Excel");
            this.ilButtons.Images.SetKeyName(8, "Export");
            this.ilButtons.Images.SetKeyName(9, "Filter");
            this.ilButtons.Images.SetKeyName(10, "Filter_Clear");
            this.ilButtons.Images.SetKeyName(11, "Import");
            this.ilButtons.Images.SetKeyName(12, "Move");
            this.ilButtons.Images.SetKeyName(13, "New");
            this.ilButtons.Images.SetKeyName(14, "OK");
            this.ilButtons.Images.SetKeyName(15, "Open");
            this.ilButtons.Images.SetKeyName(16, "Open_RecordSet");
            this.ilButtons.Images.SetKeyName(17, "RecordSets");
            this.ilButtons.Images.SetKeyName(18, "Refresh");
            this.ilButtons.Images.SetKeyName(19, "Rename");
            this.ilButtons.Images.SetKeyName(20, "Run");
            this.ilButtons.Images.SetKeyName(21, "Save");
            this.ilButtons.Images.SetKeyName(22, "Search");
            this.ilButtons.Images.SetKeyName(23, "Stop");
            this.ilButtons.Images.SetKeyName(24, "Undo");
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Add");
            this.imageList1.Images.SetKeyName(1, "Cancel");
            this.imageList1.Images.SetKeyName(2, "Save");
            this.imageList1.Images.SetKeyName(3, "Filter");
            this.imageList1.Images.SetKeyName(4, "Search");
            this.imageList1.Images.SetKeyName(5, "Add_RecordSet");
            this.imageList1.Images.SetKeyName(6, "Move");
            this.imageList1.Images.SetKeyName(7, "Copy");
            this.imageList1.Images.SetKeyName(8, "New");
            this.imageList1.Images.SetKeyName(9, "Rename");
            this.imageList1.Images.SetKeyName(10, "Delete");
            this.imageList1.Images.SetKeyName(11, "Open");
            this.imageList1.Images.SetKeyName(12, "Open_RecordSet");
            this.imageList1.Images.SetKeyName(13, "DB");
            this.imageList1.Images.SetKeyName(14, "Export");
            this.imageList1.Images.SetKeyName(15, "Import");
            this.imageList1.Images.SetKeyName(16, "OK");
            this.imageList1.Images.SetKeyName(17, "Refresh");
            // 
            // timFocus
            // 
            this.timFocus.Interval = 10;
            this.timFocus.Tick += new System.EventHandler(this.timFocus_Tick);
            // 
            // DialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(568, 504);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "DialogForm";
            this.Text = "DialogForm";
            this.Load += new System.EventHandler(this.DialogForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.ImageList imageList1;
        internal System.Windows.Forms.ToolTip FormToolTip;
        internal System.Windows.Forms.ImageList ilButtons;
        private System.Windows.Forms.Timer timFocus;

    }
}