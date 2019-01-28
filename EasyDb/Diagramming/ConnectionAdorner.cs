namespace EasyDb.Diagramming
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;

    /// <summary>
    /// Defines the <see cref="ConnectionAdorner" />
    /// </summary>
    public class ConnectionAdorner : Adorner
    {
        /// <summary>
        /// Defines the designerCanvas
        /// </summary>
        private EasyDb.Diagramming.DesignerCanvas designerCanvas;

        /// <summary>
        /// Defines the adornerCanvas
        /// </summary>
        private Canvas adornerCanvas;

        /// <summary>
        /// Defines the connection
        /// </summary>
        private Connection connection;

        /// <summary>
        /// Defines the pathGeometry
        /// </summary>
        private PathGeometry pathGeometry;

        /// <summary>
        /// Defines the fixConnector, dragConnector
        /// </summary>
        private Connector fixConnector, dragConnector;

        /// <summary>
        /// Defines the sourceDragThumb, sinkDragThumb
        /// </summary>
        private Thumb sourceDragThumb, sinkDragThumb;

        /// <summary>
        /// Defines the drawingPen
        /// </summary>
        private Pen drawingPen;

        /// <summary>
        /// Defines the hitDesignerItem
        /// </summary>
        private DesignerItem hitDesignerItem;

        /// <summary>
        /// Gets or sets the HitDesignerItem
        /// </summary>
        private DesignerItem HitDesignerItem
        {
            get { return hitDesignerItem; }
            set
            {
                if (hitDesignerItem != value)
                {
                    if (hitDesignerItem != null)
                        hitDesignerItem.IsDragConnectionOver = false;

                    hitDesignerItem = value;

                    if (hitDesignerItem != null)
                        hitDesignerItem.IsDragConnectionOver = true;
                }
            }
        }

        /// <summary>
        /// Defines the hitConnector
        /// </summary>
        private Connector hitConnector;

        /// <summary>
        /// Gets or sets the HitConnector
        /// </summary>
        private Connector HitConnector
        {
            get { return hitConnector; }
            set
            {
                if (hitConnector != value)
                {
                    hitConnector = value;
                }
            }
        }

        /// <summary>
        /// Defines the visualChildren
        /// </summary>
        private VisualCollection visualChildren;

        /// <summary>
        /// Gets the VisualChildrenCount
        /// </summary>
        protected override int VisualChildrenCount
        {
            get
            {
                return this.visualChildren.Count;
            }
        }

        /// <summary>
        /// The GetVisualChild
        /// </summary>
        /// <param name="index">The index<see cref="int"/></param>
        /// <returns>The <see cref="Visual"/></returns>
        protected override Visual GetVisualChild(int index)
        {
            return this.visualChildren[index];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionAdorner"/> class.
        /// </summary>
        /// <param name="designer">The designer<see cref="EasyDb.Diagramming.DesignerCanvas"/></param>
        /// <param name="connection">The connection<see cref="Connection"/></param>
        public ConnectionAdorner(EasyDb.Diagramming.DesignerCanvas designer, Connection connection)
            : base(designer)
        {
            this.designerCanvas = designer;
            adornerCanvas = new Canvas();
            this.visualChildren = new VisualCollection(this);
            this.visualChildren.Add(adornerCanvas);

            this.connection = connection;
            this.connection.PropertyChanged += new PropertyChangedEventHandler(AnchorPositionChanged);

            InitializeDragThumbs();

            drawingPen = new Pen(Brushes.LightSlateGray, 1);
            drawingPen.LineJoin = PenLineJoin.Round;

            base.Unloaded += new RoutedEventHandler(ConnectionAdorner_Unloaded);
        }

        /// <summary>
        /// The AnchorPositionChanged
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="PropertyChangedEventArgs"/></param>
        internal void AnchorPositionChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("AnchorPositionSource"))
            {
                Canvas.SetLeft(sourceDragThumb, connection.AnchorPositionSource.X);
                Canvas.SetTop(sourceDragThumb, connection.AnchorPositionSource.Y);
            }

            if (e.PropertyName.Equals("AnchorPositionSink"))
            {
                Canvas.SetLeft(sinkDragThumb, connection.AnchorPositionSink.X);
                Canvas.SetTop(sinkDragThumb, connection.AnchorPositionSink.Y);
            }
        }

        /// <summary>
        /// The thumbDragThumb_DragCompleted
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="DragCompletedEventArgs"/></param>
        internal void thumbDragThumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            if (HitConnector != null)
            {
                if (connection != null)
                {
                    if (connection.Source == fixConnector)
                        connection.Sink = this.HitConnector;
                    else
                        connection.Source = this.HitConnector;
                }
            }

            this.HitDesignerItem = null;
            this.HitConnector = null;
            this.pathGeometry = null;
            this.connection.StrokeDashArray = null;
            this.InvalidateVisual();
        }

        /// <summary>
        /// The thumbDragThumb_DragStarted
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="DragStartedEventArgs"/></param>
        internal void thumbDragThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            this.HitDesignerItem = null;
            this.HitConnector = null;
            this.pathGeometry = null;
            this.Cursor = Cursors.Cross;
            this.connection.StrokeDashArray = new DoubleCollection(new double[] { 1, 2 });

            if (sender == sourceDragThumb)
            {
                fixConnector = connection.Sink;
                dragConnector = connection.Source;
            }
            else if (sender == sinkDragThumb)
            {
                dragConnector = connection.Sink;
                fixConnector = connection.Source;
            }
        }

        /// <summary>
        /// The thumbDragThumb_DragDelta
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="DragDeltaEventArgs"/></param>
        internal void thumbDragThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            Point currentPosition = Mouse.GetPosition(this);
            this.HitTesting(currentPosition);
            this.pathGeometry = UpdatePathGeometry(currentPosition);
            this.InvalidateVisual();
        }

        /// <summary>
        /// The OnRender
        /// </summary>
        /// <param name="dc">The dc<see cref="DrawingContext"/></param>
        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            dc.DrawGeometry(null, drawingPen, this.pathGeometry);
        }

        /// <summary>
        /// The ArrangeOverride
        /// </summary>
        /// <param name="finalSize">The finalSize<see cref="Size"/></param>
        /// <returns>The <see cref="Size"/></returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            adornerCanvas.Arrange(new Rect(0, 0, this.designerCanvas.ActualWidth, this.designerCanvas.ActualHeight));
            return finalSize;
        }

        /// <summary>
        /// The ConnectionAdorner_Unloaded
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="RoutedEventArgs"/></param>
        private void ConnectionAdorner_Unloaded(object sender, RoutedEventArgs e)
        {
            sourceDragThumb.DragDelta -= new DragDeltaEventHandler(thumbDragThumb_DragDelta);
            sourceDragThumb.DragStarted -= new DragStartedEventHandler(thumbDragThumb_DragStarted);
            sourceDragThumb.DragCompleted -= new DragCompletedEventHandler(thumbDragThumb_DragCompleted);

            sinkDragThumb.DragDelta -= new DragDeltaEventHandler(thumbDragThumb_DragDelta);
            sinkDragThumb.DragStarted -= new DragStartedEventHandler(thumbDragThumb_DragStarted);
            sinkDragThumb.DragCompleted -= new DragCompletedEventHandler(thumbDragThumb_DragCompleted);
        }

        /// <summary>
        /// The InitializeDragThumbs
        /// </summary>
        private void InitializeDragThumbs()
        {
            Style dragThumbStyle = connection.FindResource("ConnectionAdornerThumbStyle") as Style;

            //source drag thumb
            sourceDragThumb = new Thumb();
            Canvas.SetLeft(sourceDragThumb, connection.AnchorPositionSource.X);
            Canvas.SetTop(sourceDragThumb, connection.AnchorPositionSource.Y);
            this.adornerCanvas.Children.Add(sourceDragThumb);
            if (dragThumbStyle != null)
                sourceDragThumb.Style = dragThumbStyle;

            sourceDragThumb.DragDelta += new DragDeltaEventHandler(thumbDragThumb_DragDelta);
            sourceDragThumb.DragStarted += new DragStartedEventHandler(thumbDragThumb_DragStarted);
            sourceDragThumb.DragCompleted += new DragCompletedEventHandler(thumbDragThumb_DragCompleted);

            // sink drag thumb
            sinkDragThumb = new Thumb();
            Canvas.SetLeft(sinkDragThumb, connection.AnchorPositionSink.X);
            Canvas.SetTop(sinkDragThumb, connection.AnchorPositionSink.Y);
            this.adornerCanvas.Children.Add(sinkDragThumb);
            if (dragThumbStyle != null)
                sinkDragThumb.Style = dragThumbStyle;

            sinkDragThumb.DragDelta += new DragDeltaEventHandler(thumbDragThumb_DragDelta);
            sinkDragThumb.DragStarted += new DragStartedEventHandler(thumbDragThumb_DragStarted);
            sinkDragThumb.DragCompleted += new DragCompletedEventHandler(thumbDragThumb_DragCompleted);
        }

        /// <summary>
        /// The UpdatePathGeometry
        /// </summary>
        /// <param name="position">The position<see cref="Point"/></param>
        /// <returns>The <see cref="PathGeometry"/></returns>
        private PathGeometry UpdatePathGeometry(Point position)
        {
            PathGeometry geometry = new PathGeometry();

            ConnectorOrientation targetOrientation;
            if (HitConnector != null)
                targetOrientation = HitConnector.Orientation;
            else
                targetOrientation = dragConnector.Orientation;

            List<Point> linePoints = PathFinder.GetConnectionLine(fixConnector.GetInfo(), position, targetOrientation);

            if (linePoints.Count > 0)
            {
                PathFigure figure = new PathFigure();
                figure.StartPoint = linePoints[0];
                linePoints.Remove(linePoints[0]);
                figure.Segments.Add(new PolyLineSegment(linePoints, true));
                geometry.Figures.Add(figure);
            }

            return geometry;
        }

        /// <summary>
        /// The HitTesting
        /// </summary>
        /// <param name="hitPoint">The hitPoint<see cref="Point"/></param>
        private void HitTesting(Point hitPoint)
        {
            bool hitConnectorFlag = false;

            DependencyObject hitObject = designerCanvas.InputHitTest(hitPoint) as DependencyObject;
            while (hitObject != null &&
                   hitObject != fixConnector.ParentDesignerItem &&
                   hitObject.GetType() != typeof(EasyDb.Diagramming.DesignerCanvas))
            {
                if (hitObject is Connector)
                {
                    HitConnector = hitObject as Connector;
                    hitConnectorFlag = true;
                }

                if (hitObject is DesignerItem)
                {
                    HitDesignerItem = hitObject as DesignerItem;
                    if (!hitConnectorFlag)
                        HitConnector = null;
                    return;
                }
                hitObject = VisualTreeHelper.GetParent(hitObject);
            }

            HitConnector = null;
            HitDesignerItem = null;
        }
    }
}
