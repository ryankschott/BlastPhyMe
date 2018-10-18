using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChangLab.PAML.CodeML;

namespace Pilgrimage.PAML
{
    public partial class frmEditTreeConfiguration : DialogForm
    {
        internal Tree EditTree { get; private set; }
        
        public frmEditTreeConfiguration(Tree EditTree, List<Tree> TreeCollection, bool ShowButtons = true)
        {
            InitializeComponent();
            uctTreeConfiguration1.Initialize(EditTree, TreeCollection, ShowButtons);

            SetButtonImage(btnSave, "Save");
            SetButtonImage(btnCancel, "Cancel");

            if (!ShowButtons) { btnSave.Parent.Controls.Remove(btnSave); }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            List<ValidationMessage> messages = new List<ValidationMessage>();
            Tree configuredTree = null;
            uctTreeConfiguration1.Validate(out messages, ref configuredTree);
            if (ValidationMessage.Prompt(messages, this))
            {
                this.EditTree = configuredTree;

                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }
    }
}
