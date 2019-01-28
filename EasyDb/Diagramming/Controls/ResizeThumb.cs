namespace EasyDb.Diagramming.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Media;

    /// <summary>
    /// Defines the <see cref="ResizeThumb" />
    /// </summary>
    public class ResizeThumb : Thumb
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResizeThumb"/> class.
        /// </summary>
        public ResizeThumb()
        {
            base.DragDelta += new DragDeltaEventHandler(ResizeThumb_DragDelta);
        }

        /// <summary>
        /// The ResizeThumb_DragDelta
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="DragDeltaEventArgs"/></param>
        internal void ResizeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            DesignerItem designerItem = this.DataContext as DesignerItem;
            EasyDb.Diagramming.DesignerCanvas designer = VisualTreeHelper.GetParent(designerItem) as EasyDb.Diagramming.DesignerCanvas;

            if (designerItem != null && designer != null && designerItem.IsSelected)
            {
                double minLeft, minTop, minDeltaHorizontal, minDeltaVertical;
                double dragDeltaVertical, dragDeltaHorizontal, scale;

                IEnumerable<DesignerItem> selectedDesignerItems = designer.SelectionService.CurrentSelection.OfType<DesignerItem>();

                CalculateDragLimits(selectedDesignerItems, out minLeft, out minTop,
                                    out minDeltaHorizontal, out minDeltaVertical);

                foreach (DesignerItem item in selectedDesignerItems)
                {
                    if (item != null && item.ParentID == Guid.Empty)
                    {
                        switch (base.VerticalAlignment)
                        {
                            case VerticalAlignment.Bottom:
                                dragDeltaVertical = Math.Min(-e.VerticalChange, minDeltaVertical);
                                scale = (item.ActualHeight - dragDeltaVertical) / item.ActualHeight;
                                DragBottom(scale, item, designer.SelectionService);
                                break;
                            case VerticalAlignment.Top:
                                double top = Canvas.GetTop(item);
                                dragDeltaVertical = Math.Min(Math.Max(-minTop, e.VerticalChange), minDeltaVertical);
                                scale = (item.ActualHeight - dragDeltaVertical) / item.ActualHeight;
                                DragTop(scale, item, designer.SelectionService);
                                break;
                            default:
                                break;
                        }

                        switch (base.HorizontalAlignment)
                        {
                            case HorizontalAlignment.Left:
                                double left = Canvas.GetLeft(item);
                                dragDeltaHorizontal = Math.Min(Math.Max(-minLeft, e.HorizontalChange), minDeltaHorizontal);
                                scale = (item.ActualWidth - dragDeltaHorizontal) / item.ActualWidth;
                                DragLeft(scale, item, designer.SelectionService);
                                break;
                            case HorizontalAlignment.Right:
                                dragDeltaHorizontal = Math.Min(-e.HorizontalChange, minDeltaHorizontal);
                                scale = (item.ActualWidth - dragDeltaHorizontal) / item.ActualWidth;
                                DragRight(scale, item, designer.SelectionService);
                                break;
                            default:
                                break;
                        }
                    }
                }
                e.Handled = true;
            }
        }

        /// <summary>
        /// The DragLeft
        /// </summary>
        /// <param name="scale">The scale<see cref="double"/></param>
        /// <param name="item">The item<see cref="DesignerItem"/></param>
        /// <param name="selectionService">The selectionService<see cref="SelectionService"/></param>
        private void DragLeft(double scale, DesignerItem item, SelectionService selectionService)
        {
            IEnumerable<DesignerItem> groupItems = selectionService.GetGroupMembers(item).Cast<DesignerItem>();

            double groupLeft = Canvas.GetLeft(item) + item.Width;
            foreach (DesignerItem groupItem in groupItems)
            {
                double groupItemLeft = Canvas.GetLeft(groupItem);
                double delta = (groupLeft - groupItemLeft) * (scale - 1);
                Canvas.SetLeft(groupItem, groupItemLeft - delta);
                groupItem.Width = groupItem.ActualWidth * scale;
            }
        }

        /// <summary>
        /// The DragTop
        /// </summary>
        /// <param name="scale">The scale<see cref="double"/></param>
        /// <param name="item">The item<see cref="DesignerItem"/></param>
        /// <param name="selectionService">The selectionService<see cref="SelectionService"/></param>
        private void DragTop(double scale, DesignerItem item, SelectionService selectionService)
        {
            IEnumerable<DesignerItem> groupItems = selectionService.GetGroupMembers(item).Cast<DesignerItem>();
            double groupBottom = Canvas.GetTop(item) + item.Height;
            foreach (DesignerItem groupItem in groupItems)
            {
                double groupItemTop = Canvas.GetTop(groupItem);
                double delta = (groupBottom - groupItemTop) * (scale - 1);
                Canvas.SetTop(groupItem, groupItemTop - delta);
                groupItem.Height = groupItem.ActualHeight * scale;
            }
        }

        /// <summary>
        /// The DragRight
        /// </summary>
        /// <param name="scale">The scale<see cref="double"/></param>
        /// <param name="item">The item<see cref="DesignerItem"/></param>
        /// <param name="selectionService">The selectionService<see cref="SelectionService"/></param>
        private void DragRight(double scale, DesignerItem item, SelectionService selectionService)
        {
            IEnumerable<DesignerItem> groupItems = selectionService.GetGroupMembers(item).Cast<DesignerItem>();

            double groupLeft = Canvas.GetLeft(item);
            foreach (DesignerItem groupItem in groupItems)
            {
                double groupItemLeft = Canvas.GetLeft(groupItem);
                double delta = (groupItemLeft - groupLeft) * (scale - 1);

                Canvas.SetLeft(groupItem, groupItemLeft + delta);
                groupItem.Width = groupItem.ActualWidth * scale;
            }
        }

        /// <summary>
        /// The DragBottom
        /// </summary>
        /// <param name="scale">The scale<see cref="double"/></param>
        /// <param name="item">The item<see cref="DesignerItem"/></param>
        /// <param name="selectionService">The selectionService<see cref="SelectionService"/></param>
        private void DragBottom(double scale, DesignerItem item, SelectionService selectionService)
        {
            IEnumerable<DesignerItem> groupItems = selectionService.GetGroupMembers(item).Cast<DesignerItem>();
            double groupTop = Canvas.GetTop(item);
            foreach (DesignerItem groupItem in groupItems)
            {
                double groupItemTop = Canvas.GetTop(groupItem);
                double delta = (groupItemTop - groupTop) * (scale - 1);

                Canvas.SetTop(groupItem, groupItemTop + delta);
                groupItem.Height = groupItem.ActualHeight * scale;
            }
        }

        /// <summary>
        /// The CalculateDragLimits
        /// </summary>
        /// <param name="selectedItems">The selectedItems<see cref="IEnumerable{DesignerItem}"/></param>
        /// <param name="minLeft">The minLeft<see cref="double"/></param>
        /// <param name="minTop">The minTop<see cref="double"/></param>
        /// <param name="minDeltaHorizontal">The minDeltaHorizontal<see cref="double"/></param>
        /// <param name="minDeltaVertical">The minDeltaVertical<see cref="double"/></param>
        private void CalculateDragLimits(IEnumerable<DesignerItem> selectedItems, out double minLeft, out double minTop, out double minDeltaHorizontal, out double minDeltaVertical)
        {
            minLeft = double.MaxValue;
            minTop = double.MaxValue;
            minDeltaHorizontal = double.MaxValue;
            minDeltaVertical = double.MaxValue;

            // drag limits are set by these parameters: canvas top, canvas left, minHeight, minWidth
            // calculate min value for each parameter for each item
            foreach (DesignerItem item in selectedItems)
            {
                double left = Canvas.GetLeft(item);
                double top = Canvas.GetTop(item);

                minLeft = double.IsNaN(left) ? 0 : Math.Min(left, minLeft);
                minTop = double.IsNaN(top) ? 0 : Math.Min(top, minTop);

                minDeltaVertical = Math.Min(minDeltaVertical, item.ActualHeight - item.MinHeight);
                minDeltaHorizontal = Math.Min(minDeltaHorizontal, item.ActualWidth - item.MinWidth);
            }
        }
    }
}
