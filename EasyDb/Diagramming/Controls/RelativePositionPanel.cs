namespace EasyDb.Diagramming.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    /// <summary>
    /// Defines the <see cref="RelativePositionPanel" />
    /// </summary>
    public class RelativePositionPanel : Panel
    {
        /// <summary>
        /// Defines the RelativePositionProperty
        /// </summary>
        public static readonly DependencyProperty RelativePositionProperty =
            DependencyProperty.RegisterAttached("RelativePosition", typeof(Point), typeof(RelativePositionPanel),
            new FrameworkPropertyMetadata(new Point(0, 0),
                                          new PropertyChangedCallback(RelativePositionPanel.OnRelativePositionChanged)));

        /// <summary>
        /// The GetRelativePosition
        /// </summary>
        /// <param name="element">The element<see cref="UIElement"/></param>
        /// <returns>The <see cref="Point"/></returns>
        public static Point GetRelativePosition(UIElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            return (Point)element.GetValue(RelativePositionProperty);
        }

        /// <summary>
        /// The SetRelativePosition
        /// </summary>
        /// <param name="element">The element<see cref="UIElement"/></param>
        /// <param name="value">The value<see cref="Point"/></param>
        public static void SetRelativePosition(UIElement element, Point value)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            element.SetValue(RelativePositionProperty, value);
        }

        /// <summary>
        /// The OnRelativePositionChanged
        /// </summary>
        /// <param name="d">The d<see cref="DependencyObject"/></param>
        /// <param name="e">The e<see cref="DependencyPropertyChangedEventArgs"/></param>
        private static void OnRelativePositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UIElement reference = d as UIElement;
            if (reference != null)
            {
                RelativePositionPanel parent = VisualTreeHelper.GetParent(reference) as RelativePositionPanel;
                if (parent != null)
                {
                    parent.InvalidateArrange();
                }
            }
        }

        /// <summary>
        /// The ArrangeOverride
        /// </summary>
        /// <param name="arrangeSize">The arrangeSize<see cref="Size"/></param>
        /// <returns>The <see cref="Size"/></returns>
        protected override Size ArrangeOverride(Size arrangeSize)
        {
            foreach (UIElement element in base.InternalChildren)
            {
                if (element != null)
                {
                    Point relPosition = GetRelativePosition(element);
                    double x = (arrangeSize.Width - element.DesiredSize.Width) * relPosition.X;
                    double y = (arrangeSize.Height - element.DesiredSize.Height) * relPosition.Y;

                    if (double.IsNaN(x)) x = 0;
                    if (double.IsNaN(y)) y = 0;

                    element.Arrange(new Rect(new Point(x, y), element.DesiredSize));
                }
            }
            return arrangeSize;
        }

        /// <summary>
        /// The MeasureOverride
        /// </summary>
        /// <param name="availableSize">The availableSize<see cref="Size"/></param>
        /// <returns>The <see cref="Size"/></returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            Size size = new Size(double.PositiveInfinity, double.PositiveInfinity);

            // SDK docu says about InternalChildren Property: 'Classes that are derived from Panel 
            // should use this property, instead of the Children property, for internal overrides 
            // such as MeasureCore and ArrangeCore.

            foreach (UIElement element in this.InternalChildren)
            {
                if (element != null)
                    element.Measure(size);
            }

            return base.MeasureOverride(availableSize);
        }
    }
}
