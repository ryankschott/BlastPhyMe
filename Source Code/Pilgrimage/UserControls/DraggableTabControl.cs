using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace DraggableTabControl
{
    /// <remarks>
    /// Huge thanks to Paul Auger for doing this for me:
    /// http://www.codeproject.com/Articles/2445/Drag-and-Drop-Tab-Control
    /// 
    /// NOTE: Implementation of any of the selected-tab-changed event handlers should cancel out of their code if IsDragging = true.
    /// </remarks>
    [ToolboxBitmap(typeof(DraggableTabControl))] 
    public class DraggableTabControl : System.Windows.Forms.TabControl
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        /// <remarks>
        /// Added by me so that I've got an on-drag-finished event to make use of.
        /// </remarks>
        private TabPage DragTab { get; set; }
        public bool IsDragging { get; private set; }
        public bool DragDropInitiated { get; private set; }
        public Point MouseDownAt { get; private set; }
        
        public DraggableTabControl()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        }
        #endregion

        /// <remarks>
        /// Modified by me to push DoDragDrop down into OnMouseMove.  This makes the initiation of a drag-drop a little less likely when the user is
        /// just clicking to select a tab vs actually intending to rearrange the tabs.
        /// </remarks>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button == System.Windows.Forms.MouseButtons.Left && e.Clicks == 1)
            {
                Point pt = new Point(e.X, e.Y);
                TabPage tp = GetTabPageByTab(pt);

                if (tp != null)
                {
#if DEBUG
                    System.Diagnostics.Debug.WriteLine("DraggableTabControl.OnMouseDown: " + tp.Text);
#endif

                    // Allow for suppression of drag-and-drop reordering
                    TabControlCancelEventArgs args = new TabControlCancelEventArgs(tp, FindIndex(tp), false, TabControlAction.Selecting);
                    OnReordering(args);
                    if (args.Cancel) { return; }

                    this.DragTab = tp;
                    this.DragDropInitiated = false;
                    this.IsDragging = false;
                    this.MouseDownAt = e.Location;
                    return;
                }
            }

            this.DragTab = null;
        }

        /// <remarks>
        /// Added by me.
        /// </remarks>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (!this.DragDropInitiated && this.DragTab != null && e.Button == System.Windows.Forms.MouseButtons.Left
                && (
                    (Math.Abs(this.MouseDownAt.X - e.Location.X) > 5)
                    || // These ensure that you've moved the cursor a bit, avoiding a drag-drop unless purposefully initiated.
                    (Math.Abs(this.MouseDownAt.Y - e.Location.Y) > 5)
                    )
                )
            {
                this.DragDropInitiated = true;
                DoDragDrop(DragTab, DragDropEffects.All);
            }
        }

        protected override void OnDragOver(System.Windows.Forms.DragEventArgs e)
        {
            base.OnDragOver(e);

            Point pt = new Point(e.X, e.Y);
            //We need client coordinates.
            pt = PointToClient(pt);

            //Get the tab we are hovering over.
            TabPage hover_tab = GetTabPageByTab(pt);

            //Make sure we are on a tab.
            if(hover_tab != null)
            {
                //Make sure there is a TabPage being dragged.
                if(e.Data.GetDataPresent(typeof(TabPage)))
                { 
                    e.Effect = DragDropEffects.Move;
                    //DragTab = (TabPage)e.Data.GetData(typeof(TabPage));
                    int item_drag_index = FindIndex(DragTab);
                    int drop_location_index= FindIndex(hover_tab);

                    //Don't do anything if we are hovering over ourself.
                    if (item_drag_index != drop_location_index)
                    {
                        IsDragging = true;
#if DEBUG
                        System.Diagnostics.Debug.WriteLine("DraggableTabControl.OnDragOver IsDragging: " + DragTab.Text);
#endif

                        ArrayList pages = new ArrayList();
                        //Put all tab pages into an array.
                        for (int i = 0; i < TabPages.Count; i++)
                        {
                            //Except the one we are dragging.
                            if (i != item_drag_index)
                                pages.Add(TabPages[i]);
                        }

                        //Now put the one we are dragging it at the proper location.
                        pages.Insert(drop_location_index, DragTab);

                        //Make them all go away for a nanosec.
                        TabPages.Clear();

                        //Add them all back in.
                        TabPages.AddRange((TabPage[])pages.ToArray(typeof(TabPage)));
                                                
                        //Make sure the drag tab is selected.
                        SelectedTab = DragTab;
                    }
                }
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        /// <remarks>
        /// Added by me to provide an event for when reordering is complete, and to support suppression of SelectedTab change events during dragging.
        /// </remarks>
        protected override void OnDragDrop(DragEventArgs drgevent)
        {
            base.OnDragDrop(drgevent);

            if (DragTab != null)
            {
                // DragTab will be set simply by MouseMove, wherein OnDragOver tests to see if the tab needs to be moved, but IsDragging is set true
                // only when the reordering occurs.  We want to null out DragTab for safety's sake, but only need to trigger these events if an
                // actual DragDrop reordering occured.
                if (IsDragging)
                {
                    IsDragging = false;
#if DEBUG
                    System.Diagnostics.Debug.WriteLine("DraggableTabControl.OnDragDrop: " + DragTab.Text);
#endif

                    TabControlEventArgs args = new TabControlEventArgs(DragTab, this.FindIndex(DragTab), TabControlAction.Selected);
                    OnReordered(args);
                    OnSelected(args);
                }

                DragDropInitiated = false;
                DragTab = null;
            }
        }
        
        /// <summary>
        /// Finds the TabPage whose tab is contains the given point.
        /// </summary>
        /// <param name="pt">
        /// The point (given in client coordinates) to look for a TabPage.
        /// </param>
        /// <returns>
        /// The TabPage whose tab is at the given point (null if there isn't one).
        /// </returns>
        private TabPage GetTabPageByTab(Point pt)
        {
            TabPage tp =

                null;
            for (int i =

                0; i < TabPages.Count; i++)
            {
                if (GetTabRect(i).Contains(pt))
                {
                    tp = TabPages[i];
                    break;
                }
            }

            return tp;
        }

        /// <summary>
        /// Loops over all the TabPages to find the index of the given TabPage.
        /// </summary>
        /// <param name="page">
        /// The TabPage we want the index for.
        /// </param>
        /// <returns>
        /// The index of the given TabPage (-1 if it isn't found.)
        /// </returns>
        private int FindIndex(TabPage page)
        {
            for (int i = 0; i < TabPages.Count; i++)
            {
                if (TabPages[i] == page)
                    return i;
            }

            return -1;
        }

        #region Events
        /// <remarks>
        /// Added by me.
        /// </remarks>
        protected virtual void OnReordering(TabControlCancelEventArgs e)
        {
            if (Reordering != null) { Reordering(this, e); }
        }
        public event TabControlCancelEventHandler Reordering;

        /// <remarks>
        /// Added by me.
        /// </remarks>
        protected virtual void OnReordered(TabControlEventArgs e)
        {
            if (Reordered != null) { Reordered(this, e); }
        }
        public event TabControlEventHandler Reordered;
        #endregion
    }
}