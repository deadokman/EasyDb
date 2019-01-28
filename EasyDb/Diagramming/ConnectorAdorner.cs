namespace EasyDb.Diagramming
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;

    /// <summary>
    /// Defines the <see cref="ConnectorAdorner" />
    /// </summary>
    public class ConnectorAdorner : Adorner
    {
        /// <summary>
        /// Defines the pathGeometry
        /// </summary>
        private PathGeometry pathGeometry;

        /// <summary>
        /// Defines the designerCanvas
        /// </summary>
        private EasyDb.Diagramming.DesignerCanvas designerCanvas;

        /// <summary>
        /// Defines the sourceConnector
        /// </summary>
        private Connector sourceConnector;

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
        /// Initializes a new instance of the <see cref="ConnectorAdorner"/> class.
        /// </summary>
        /// <param name="designer">The designer<see cref="EasyDb.Diagramming.DesignerCanvas"/></param>
        /// <param name="sourceConnector">The sourceConnector<see cref="Connector"/></param>
        public ConnectorAdorner(EasyDb.Diagramming.DesignerCanvas designer, Connector sourceConnector)
            : base(designer)
        {
            this.designerCanvas = designer;
            this.sourceConnector = sourceConnector;
            drawingPen = new Pen(Brushes.LightSlateGray, 1);
            drawingPen.LineJoin = PenLineJoin.Round;
            this.Cursor = Cursors.Cross;
        }

        /// <summary>
        /// The OnMouseUp
        /// </summary>
        /// <param name="e">The e<see cref="MouseButtonEventArgs"/></param>
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            if (HitConnector != null)
            {
                Connector sourceConnector = this.sourceConnector;
                Connector sinkConnector = this.HitConnector;
                Connection newConnection = new Connection(sourceConnector, sinkConnector);

                Canvas.SetZIndex(newConnection, designerCanvas.Children.Count);
                this.designerCanvas.Children.Add(newConnection);

            }
            if (HitDesignerItem != null)
            {
                this.HitDesignerItem.IsDragConnectionOver = false;
            }

            if (this.IsMouseCaptured) this.ReleaseMouseCapture();

            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this.designerCanvas);
            if (adornerLayer != null)
            {
                adornerLayer.Remove(this);
            }
        }

        /// <summary>
        /// The OnMouseMove
        /// </summary>
        /// <param name="e">The e<see cref="MouseEventArgs"/></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (!this.IsMouseCaptured) this.CaptureMouse();
                HitTesting(e.GetPosition(this));
                this.pathGeometry = GetPathGeometry(e.GetPosition(this));
                this.InvalidateVisual();
            }
            else
            {
                if (this.IsMouseCaptured) this.ReleaseMouseCapture();
            }
        }

        /// <summary>
        /// The OnRender
        /// </summary>
        /// <param name="dc">The dc<see cref="DrawingContext"/></param>
        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            dc.DrawGeometry(null, drawingPen, this.pathGeometry);

            // without a background the OnMouseMove event would not be fired
            // Alternative: implement a Canvas as a child of this adorner, like
            // the ConnectionAdorner does.
            dc.DrawRectangle(Brushes.Transparent, null, new Rect(RenderSize));
        }

        /// <summary>
        /// The GetPathGeometry
        /// </summary>
        /// <param name="position">The position<see cref="Point"/></param>
        /// <returns>The <see cref="PathGeometry"/></returns>
        private PathGeometry GetPathGeometry(Point position)
        {
            PathGeometry geometry = new PathGeometry();

            ConnectorOrientation targetOrientation;
            if (HitConnector != null)
                targetOrientation = HitConnector.Orientation;
            else
                targetOrientation = ConnectorOrientation.None;

            List<Point> pathPoints = PathFinder.GetConnectionLine(sourceConnector.GetInfo(), position, targetOrientation);

            if (pathPoints.Count > 0)
            {
                PathFigure figure = new PathFigure();
                figure.StartPoint = pathPoints[0];
                pathPoints.Remove(pathPoints[0]);
                figure.Segments.Add(new PolyLineSegment(pathPoints, true));
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
                   hitObject != sourceConnector.ParentDesignerItem &&
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
