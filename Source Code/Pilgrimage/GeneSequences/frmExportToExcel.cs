using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ChangLab.Genes;

namespace Pilgrimage.GeneSequences
{
    public partial class frmExportToExcel : DialogForm
    {
        private List<ListViewItem> AllItems { get; set; }
        private Dictionary<ListViewItem, int> AllItems_Ordered
        {
            get
            {
                return AllItems.Select((li, index) => new { Item = li, Index = index }).ToDictionary(li => li.Item, li => (li.Index * 2) - 1);
            }
        }
        private Dictionary<ListViewItem, int> SelectedItems_Ordered
        {
            get
            {
                return lvSelectedColumns
                        .Items
                        .Cast<ListViewItem>()
                        .ToDictionary(li => li, li => (li.Index * 2) - 1);
            }
        }
        internal List<Gene> SelectedGenes { get; private set; }

        public frmExportToExcel(List<Gene> SelectedGenes)
        {
            InitializeComponent();
            this.SelectedGenes = SelectedGenes;

            AllItems = Utility.GeneDataFields().Select(kv => new ListViewItem(kv.Value) { Tag = kv.Key }).ToList();

            // Restore the user's column preferences
            List<string> selectedColumnKeys = Program.DatabaseSettings.ExportToExcelColumnKeys;
            AllItems.Where(li => selectedColumnKeys.Contains((string)li.Tag)).ToList().ForEach(li => lvSelectedColumns.Items.Add(new ListViewItem(li.Text) { Tag = li.Tag }));
            AllItems.Where(li => !selectedColumnKeys.Contains((string)li.Tag)).ToList().ForEach(li => lvAvailableColumns.Items.Add(new ListViewItem(li.Text) { Tag = li.Tag }));

            chkOpenInExcel.Checked = Program.DatabaseSettings.ExportToExcelOpenOnCreate;
            SetButtonImage(btnExport, "Export");
            SetButtonImage(btnCancel, "Cancel");

            ToggleArrows();
        }

        private void frmExportToExcel_Load(object sender, EventArgs e)
        {
            frmExportToExcel_ResizeEnd(null, null);
            
            if (lvAvailableColumns.Items.Count != 0) { lvAvailableColumns.Items[0].Selected = true; }
            if (lvSelectedColumns.Items.Count != 0) { lvSelectedColumns.Items[0].Selected = true; }
        }

        private void frmExportToExcel_ResizeEnd(object sender, EventArgs e)
        {
            clmAvailable.Width = lvAvailableColumns.Width - 5;
            clmSelected.Width = lvSelectedColumns.Width - 5;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (lvSelectedColumns.Items.Count == 0)
            {
                Utility.ShowMessage(this, "No columns have been selected.");
                return;
            }

            string filePath = string.Empty;
            if (IODialogHelper.SaveFile(IODialogHelper.DialogPresets.Excel,
                                            Program.Settings.CurrentRecordSet.Name + " - " + Program.Settings.CurrentSubSet_GeneSequences.Name,
                                            this, ref filePath))
            {
                if (System.IO.File.Exists(filePath))
                {
                    IODialogHelper.FileAccessResults result = IODialogHelper.CanModify(filePath);
                    switch (result)
                    {
                        case IODialogHelper.FileAccessResults.ReadWrite:
                            System.IO.File.Delete(filePath); // Otherwise Excel is just going to ask us again if it can overwrite it.
                            break;
                        case IODialogHelper.FileAccessResults.LockedByProcess:
                            Utility.ShowMessage(this, "Unable to overwrite the selected file because it is currently open in another application.");
                            return;
                        case IODialogHelper.FileAccessResults.Denied:
                            Utility.ShowMessage(this, "Access denied");
                            return;
                    }
                }

                Activities.ExportToExcel export = new Activities.ExportToExcel(this);
                export.ActivityCompleted += new Activities.Activity.ActivityCompletedEventHandler(export_ActivityCompleted);
                export.Export(this.SelectedGenes.Select(g => g.ID), lvSelectedColumns.Items.Cast<ListViewItem>().ToDictionary(li => (string)li.Tag, li => li.Text), filePath);
            }
        }

        private void export_ActivityCompleted(Activities.ActivityCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Utility.ShowErrorMessage(this, e.Error);
            }
            else
            {
                if (chkOpenInExcel.Checked)
                {
                    System.Diagnostics.Process.Start((string)e.Result);
                }

                // Record the user's column preferences
                Program.DatabaseSettings.ExportToExcelColumnKeys = lvSelectedColumns.Items.Cast<ListViewItem>().Select(li => (string)li.Tag).ToList();
                Program.DatabaseSettings.ExportToExcelOpenOnCreate = chkOpenInExcel.Checked;
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }

        private void lstColumns_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToggleArrows();
        }

        #region Arrows
        private void btnAdd_Click(object sender, EventArgs e)
        {
            lvAvailableColumns.SelectedItems.Cast<ListViewItem>().Select(li => (string)li.Tag).ToList().ForEach(key =>
                {
                    ListViewItem item = lvAvailableColumns.Items.Cast<ListViewItem>().First(li => (string)li.Tag == key);
                    lvAvailableColumns.Items.Remove(item);
                    lvSelectedColumns.Items.Add(item);
                });
            
            ToggleArrows();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            lvSelectedColumns.SelectedItems.Cast<ListViewItem>().Select(li => (string)li.Tag).ToList().ForEach(key =>
            {
                ListViewItem item = lvSelectedColumns.Items.Cast<ListViewItem>().First(li => (string)li.Tag == key);
                lvSelectedColumns.Items.Remove(item);
                lvAvailableColumns.Items.Add(item);
            });

            ToggleArrows();
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            ListViewItem selected = lvSelectedColumns.SelectedItems[0];
            Dictionary<ListViewItem, int> items = SelectedItems_Ordered;
            items[selected] -= 3;

            lvSelectedColumns.Items.Clear();
            lvSelectedColumns.Items.AddRange(items.OrderBy(kv => kv.Value).Select(kv => kv.Key).ToArray());

            ToggleArrows();

            selected.Selected = true;
            btnUp.Focus();
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            ListViewItem selected = lvSelectedColumns.SelectedItems[0];
            Dictionary<ListViewItem, int> items = SelectedItems_Ordered;
            items[selected] += 3;

            lvSelectedColumns.Items.Clear();
            lvSelectedColumns.Items.AddRange(items.OrderBy(kv => kv.Value).Select(kv => kv.Key).ToArray());

            ToggleArrows();

            selected.Selected = true;
            btnDown.Focus();
        }

        private void ToggleArrows()
        {
            btnAdd.Enabled = (lvAvailableColumns.SelectedItems.Count != 0);
            btnRemove.Enabled = (lvSelectedColumns.SelectedItems.Count != 0);
            btnUp.Enabled = (lvSelectedColumns.SelectedItems.Count != 0 && lvSelectedColumns.SelectedItems[0].Index != 0);
            btnDown.Enabled = (lvSelectedColumns.SelectedItems.Count != 0 && lvSelectedColumns.SelectedItems[0].Index != (lvSelectedColumns.Items.Count - 1));
        }
        #endregion

        #region Drag and Drop
        private ListView DragSource { get; set; }
        private ListView DropTarget { get; set; }
        private bool Dragging { get; set; }
        private Point MouseDownAt { get; set; }

        private void lvColumns_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left && e.Clicks == 1)
            {
                ListViewItem item = ((ListView)sender).HitTest(e.X, e.Y).Item;
                if (item != null)
                {
                    this.DragSource = (ListView)sender;
                    this.Dragging = false;
                    this.MouseDownAt = e.Location;
                    return;
                }
            }

            DragSource = null;
        }

        private void lvColumns_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left
                && DragSource != null
                && !Dragging
                && (
                    (Math.Abs(this.MouseDownAt.X - e.Location.X) > 5)
                    || // These ensure that you've moved the cursor a bit, avoiding a drag-drop unless purposefully initiated.
                    (Math.Abs(this.MouseDownAt.Y - e.Location.Y) > 5)
                    ))
            {
                Dragging = true;
                ListViewItem item = ((ListView)sender).HitTest(e.X, e.Y).Item;
                if (item != null)
                {
                    DragSource = (ListView)sender;
                    DoDragDrop(DragSource.SelectedItems.Cast<ListViewItem>().ToArray(), DragDropEffects.Move);
                }
            }
        }

        private void lvColumns_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ListViewItem[]))
                && !(DragSource == lvAvailableColumns && sender == lvAvailableColumns)) // lvAvailableColumns has no reason to support rearranging
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void lvColumns_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ListViewItem[])))
            {
                ListView dropTarget = ((ListView)sender);

                e.Effect = DragDropEffects.Move;
                ListViewItem hover_item = dropTarget.HitTest(dropTarget.PointToClient(new Point(e.X, e.Y))).Item;
                List<ListViewItem> drag_items = ((ListViewItem[])e.Data.GetData(typeof(ListViewItem[]))).ToList();

                if (DragSource == lvAvailableColumns && dropTarget == lvSelectedColumns)
                {
                    if (hover_item != null)
                    {
                        drag_items.Select((item, index) => new { Item = item, Index = (index + 1) }).ToList().ForEach(item =>
                            {
                                DragSource.Items.Remove(item.Item);
                                dropTarget.Items.Insert(hover_item.Index + item.Index, item.Item);
                            });
                    }
                    else
                    {
                        // Tack on the end
                        drag_items.ForEach(item =>
                        {
                            DragSource.Items.Remove(item);
                            dropTarget.Items.Add(item);
                        });
                    }
                }
                else if (DragSource == lvSelectedColumns && dropTarget == lvSelectedColumns)
                {
                    if (!drag_items.Contains(hover_item))
                    {
                        drag_items.Select((item, index) => new { Item = item, Index = (index + 1) }).ToList().ForEach(item =>
                        {
                            DragSource.Items.Remove(item.Item);
                            dropTarget.Items.Insert(hover_item.Index + item.Index, item.Item);
                        });
                    }
                }
                else if (DragSource == lvSelectedColumns && dropTarget == lvAvailableColumns)
                {
                    drag_items.ForEach(item =>
                    {
                        DragSource.Items.Remove(item);
                        dropTarget.Items.Add(item);
                    });

                    // Restore the item to its original sort order
                    ListViewItem[] reordered = lvAvailableColumns
                                                .Items
                                                .Cast<ListViewItem>()
                                                .Join(AllItems_Ordered, outer => (string)outer.Tag, inner => (string)inner.Key.Tag, (outer, inner) => new { Item = outer, Index = inner.Value })
                                                .OrderBy(item => item.Index)
                                                .Select(item => item.Item)
                                                .ToArray();

                    reordered.ToList().ForEach(li => lvAvailableColumns.Items.Remove(li));
                    lvAvailableColumns.Items.AddRange(reordered);
                }

                ToggleArrows();
            }
        }
        #endregion
    }
}
