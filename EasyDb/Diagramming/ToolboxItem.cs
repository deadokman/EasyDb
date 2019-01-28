namespace EasyDb.Diagramming
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Markup;
    using System.Windows.Media;

    // Represents a selectable item in the Toolbox/>.
    /// <summary>
    /// Defines the <see cref="ToolboxItem" />
    /// </summary>
    public class ToolboxItem : ContentControl
    {
        // caches the start point of the drag operation
        /// <summary>
        /// Defines the dragStartPoint
        /// </summary>
        private Point? dragStartPoint = null;

        /// <summary>
        /// Initializes static members of the <see cref="ToolboxItem"/> class.
        /// </summary>
        static ToolboxItem()
        {
            // set the key to reference the style for this control
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(
                typeof(ToolboxItem), new FrameworkPropertyMetadata(typeof(ToolboxItem)));
        }

        /// <summary>
        /// The OnPreviewMouseDown
        /// </summary>
        /// <param name="e">The e<see cref="MouseButtonEventArgs"/></param>
        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(e);
            this.dragStartPoint = new Point?(e.GetPosition(this));
        }

        /// <summary>
        /// The OnMouseMove
        /// </summary>
        /// <param name="e">The e<see cref="MouseEventArgs"/></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.LeftButton != MouseButtonState.Pressed)
                this.dragStartPoint = null;

            if (this.dragStartPoint.HasValue)
            {
                // XamlWriter.Save() has limitations in exactly what is serialized,
                // see SDK documentation; short term solution only;
                string xamlString = XamlWriter.Save(this.Content);
                DragObject dataObject = new DragObject();
                dataObject.Xaml = xamlString;

                WrapPanel panel = VisualTreeHelper.GetParent(this) as WrapPanel;
                if (panel != null)
                {
                    // desired size for DesignerCanvas is the stretched Toolbox item size
                    double scale = 1.3;
                    dataObject.DesiredSize = new Size(panel.ItemWidth * scale, panel.ItemHeight * scale);
                }

                DragDrop.DoDragDrop(this, dataObject, DragDropEffects.Copy);

                e.Handled = true;
            }
        }
    }

    // Wraps info of the dragged object into a class
    /// <summary>
    /// Defines the <see cref="DragObject" />
    /// </summary>
    public class DragObject
    {
        // Xaml string that represents the serialized content
        /// <summary>
        /// Gets or sets the Xaml
        /// </summary>
        public String Xaml { get; set; }

        // Defines width and height of the DesignerItem
        // when this DragObject is dropped on the DesignerCanvas
        /// <summary>
        /// Gets or sets the DesiredSize
        /// </summary>
        public Size? DesiredSize { get; set; }
    }
}
