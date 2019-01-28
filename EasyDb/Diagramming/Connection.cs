namespace EasyDb.Diagramming
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;

    /// <summary>
    /// Defines the <see cref="Connection" />
    /// </summary>
    public class Connection : Control, ISelectable, INotifyPropertyChanged
    {
        /// <summary>
        /// Defines the connectionAdorner
        /// </summary>
        private Adorner connectionAdorner;

        /// <summary>
        /// Gets or sets the ID
        /// </summary>
        public Guid ID { get; set; }

        // source connector
        /// <summary>
        /// Defines the source
        /// </summary>
        private Connector source;

        /// <summary>
        /// Gets or sets the Source
        /// </summary>
        public Connector Source
        {
            get
            {
                return source;
            }
            set
            {
                if (source != value)
                {
                    if (source != null)
                    {
                        source.PropertyChanged -= new PropertyChangedEventHandler(OnConnectorPositionChanged);
                        source.Connections.Remove(this);
                    }

                    source = value;

                    if (source != null)
                    {
                        source.Connections.Add(this);
                        source.PropertyChanged += new PropertyChangedEventHandler(OnConnectorPositionChanged);
                    }

                    UpdatePathGeometry();
                }
            }
        }

        // sink connector
        /// <summary>
        /// Defines the sink
        /// </summary>
        private Connector sink;

        /// <summary>
        /// Gets or sets the Sink
        /// </summary>
        public Connector Sink
        {
            get { return sink; }
            set
            {
                if (sink != value)
                {
                    if (sink != null)
                    {
                        sink.PropertyChanged -= new PropertyChangedEventHandler(OnConnectorPositionChanged);
                        sink.Connections.Remove(this);
                    }

                    sink = value;

                    if (sink != null)
                    {
                        sink.Connections.Add(this);
                        sink.PropertyChanged += new PropertyChangedEventHandler(OnConnectorPositionChanged);
                    }
                    UpdatePathGeometry();
                }
            }
        }

        // connection path geometry
        /// <summary>
        /// Defines the pathGeometry
        /// </summary>
        private PathGeometry pathGeometry;

        /// <summary>
        /// Gets or sets the PathGeometry
        /// </summary>
        public PathGeometry PathGeometry
        {
            get { return pathGeometry; }
            set
            {
                if (pathGeometry != value)
                {
                    pathGeometry = value;
                    UpdateAnchorPosition();
                    OnPropertyChanged("PathGeometry");
                }
            }
        }

        // between source connector position and the beginning 
        // of the path geometry we leave some space for visual reasons; 
        // so the anchor position source really marks the beginning 
        // of the path geometry on the source side
        /// <summary>
        /// Defines the anchorPositionSource
        /// </summary>
        private Point anchorPositionSource;

        /// <summary>
        /// Gets or sets the AnchorPositionSource
        /// </summary>
        public Point AnchorPositionSource
        {
            get { return anchorPositionSource; }
            set
            {
                if (anchorPositionSource != value)
                {
                    anchorPositionSource = value;
                    OnPropertyChanged("AnchorPositionSource");
                }
            }
        }

        // slope of the path at the anchor position
        // needed for the rotation angle of the arrow
        /// <summary>
        /// Defines the anchorAngleSource
        /// </summary>
        private double anchorAngleSource = 0;

        /// <summary>
        /// Gets or sets the AnchorAngleSource
        /// </summary>
        public double AnchorAngleSource
        {
            get { return anchorAngleSource; }
            set
            {
                if (anchorAngleSource != value)
                {
                    anchorAngleSource = value;
                    OnPropertyChanged("AnchorAngleSource");
                }
            }
        }

        // analogue to source side
        /// <summary>
        /// Defines the anchorPositionSink
        /// </summary>
        private Point anchorPositionSink;

        /// <summary>
        /// Gets or sets the AnchorPositionSink
        /// </summary>
        public Point AnchorPositionSink
        {
            get { return anchorPositionSink; }
            set
            {
                if (anchorPositionSink != value)
                {
                    anchorPositionSink = value;
                    OnPropertyChanged("AnchorPositionSink");
                }
            }
        }

        // analogue to source side
        /// <summary>
        /// Defines the anchorAngleSink
        /// </summary>
        private double anchorAngleSink = 0;

        /// <summary>
        /// Gets or sets the AnchorAngleSink
        /// </summary>
        public double AnchorAngleSink
        {
            get { return anchorAngleSink; }
            set
            {
                if (anchorAngleSink != value)
                {
                    anchorAngleSink = value;
                    OnPropertyChanged("AnchorAngleSink");
                }
            }
        }

        /// <summary>
        /// Defines the sourceArrowSymbol
        /// </summary>
        private ArrowSymbol sourceArrowSymbol = ArrowSymbol.None;

        /// <summary>
        /// Gets or sets the SourceArrowSymbol
        /// </summary>
        public ArrowSymbol SourceArrowSymbol
        {
            get { return sourceArrowSymbol; }
            set
            {
                if (sourceArrowSymbol != value)
                {
                    sourceArrowSymbol = value;
                    OnPropertyChanged("SourceArrowSymbol");
                }
            }
        }

        /// <summary>
        /// Defines the sinkArrowSymbol
        /// </summary>
        public ArrowSymbol sinkArrowSymbol = ArrowSymbol.Arrow;

        /// <summary>
        /// Gets or sets the SinkArrowSymbol
        /// </summary>
        public ArrowSymbol SinkArrowSymbol
        {
            get { return sinkArrowSymbol; }
            set
            {
                if (sinkArrowSymbol != value)
                {
                    sinkArrowSymbol = value;
                    OnPropertyChanged("SinkArrowSymbol");
                }
            }
        }

        // specifies a point at half path length
        /// <summary>
        /// Defines the labelPosition
        /// </summary>
        private Point labelPosition;

        /// <summary>
        /// Gets or sets the LabelPosition
        /// </summary>
        public Point LabelPosition
        {
            get { return labelPosition; }
            set
            {
                if (labelPosition != value)
                {
                    labelPosition = value;
                    OnPropertyChanged("LabelPosition");
                }
            }
        }

        // pattern of dashes and gaps that is used to outline the connection path
        /// <summary>
        /// Defines the strokeDashArray
        /// </summary>
        private DoubleCollection strokeDashArray;

        /// <summary>
        /// Gets or sets the StrokeDashArray
        /// </summary>
        public DoubleCollection StrokeDashArray
        {
            get
            {
                return strokeDashArray;
            }
            set
            {
                if (strokeDashArray != value)
                {
                    strokeDashArray = value;
                    OnPropertyChanged("StrokeDashArray");
                }
            }
        }

        // if connected, the ConnectionAdorner becomes visible
        /// <summary>
        /// Defines the isSelected
        /// </summary>
        private bool isSelected;

        /// <summary>
        /// Gets or sets a value indicating whether IsSelected
        /// </summary>
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                if (isSelected != value)
                {
                    isSelected = value;
                    OnPropertyChanged("IsSelected");
                    if (isSelected)
                        ShowAdorner();
                    else
                        HideAdorner();
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Connection"/> class.
        /// </summary>
        /// <param name="source">The source<see cref="Connector"/></param>
        /// <param name="sink">The sink<see cref="Connector"/></param>
        public Connection(Connector source, Connector sink)
        {
            this.ID = Guid.NewGuid();
            this.Source = source;
            this.Sink = sink;
            base.Unloaded += new RoutedEventHandler(Connection_Unloaded);
        }

        /// <summary>
        /// The OnMouseDown
        /// </summary>
        /// <param name="e">The e<see cref="System.Windows.Input.MouseButtonEventArgs"/></param>
        protected override void OnMouseDown(System.Windows.Input.MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            // usual selection business
            EasyDb.Diagramming.DesignerCanvas designer = VisualTreeHelper.GetParent(this) as EasyDb.Diagramming.DesignerCanvas;
            if (designer != null)
            {
                if ((Keyboard.Modifiers & (ModifierKeys.Shift | ModifierKeys.Control)) != ModifierKeys.None)
                    if (this.IsSelected)
                    {
                        designer.SelectionService.RemoveFromSelection(this);
                    }
                    else
                    {
                        designer.SelectionService.AddToSelection(this);
                    }
                else if (!this.IsSelected)
                {
                    designer.SelectionService.SelectItem(this);
                }

                Focus();
            }
            e.Handled = false;
        }

        /// <summary>
        /// The OnConnectorPositionChanged
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="PropertyChangedEventArgs"/></param>
        internal void OnConnectorPositionChanged(object sender, PropertyChangedEventArgs e)
        {
            // whenever the 'Position' property of the source or sink Connector 
            // changes we must update the connection path geometry
            if (e.PropertyName.Equals("Position"))
            {
                UpdatePathGeometry();
            }
        }

        /// <summary>
        /// The UpdatePathGeometry
        /// </summary>
        private void UpdatePathGeometry()
        {
            if (Source != null && Sink != null)
            {
                PathGeometry geometry = new PathGeometry();
                List<Point> linePoints = PathFinder.GetConnectionLine(Source.GetInfo(), Sink.GetInfo(), true);
                if (linePoints.Count > 0)
                {
                    PathFigure figure = new PathFigure();
                    figure.StartPoint = linePoints[0];
                    linePoints.Remove(linePoints[0]);
                    figure.Segments.Add(new PolyLineSegment(linePoints, true));
                    geometry.Figures.Add(figure);

                    this.PathGeometry = geometry;
                }
            }
        }

        /// <summary>
        /// The UpdateAnchorPosition
        /// </summary>
        private void UpdateAnchorPosition()
        {
            Point pathStartPoint, pathTangentAtStartPoint;
            Point pathEndPoint, pathTangentAtEndPoint;
            Point pathMidPoint, pathTangentAtMidPoint;

            // the PathGeometry.GetPointAtFractionLength method gets the point and a tangent vector 
            // on PathGeometry at the specified fraction of its length
            this.PathGeometry.GetPointAtFractionLength(0, out pathStartPoint, out pathTangentAtStartPoint);
            this.PathGeometry.GetPointAtFractionLength(1, out pathEndPoint, out pathTangentAtEndPoint);
            this.PathGeometry.GetPointAtFractionLength(0.5, out pathMidPoint, out pathTangentAtMidPoint);

            // get angle from tangent vector
            this.AnchorAngleSource = Math.Atan2(-pathTangentAtStartPoint.Y, -pathTangentAtStartPoint.X) * (180 / Math.PI);
            this.AnchorAngleSink = Math.Atan2(pathTangentAtEndPoint.Y, pathTangentAtEndPoint.X) * (180 / Math.PI);

            // add some margin on source and sink side for visual reasons only
            pathStartPoint.Offset(-pathTangentAtStartPoint.X * 5, -pathTangentAtStartPoint.Y * 5);
            pathEndPoint.Offset(pathTangentAtEndPoint.X * 5, pathTangentAtEndPoint.Y * 5);

            this.AnchorPositionSource = pathStartPoint;
            this.AnchorPositionSink = pathEndPoint;
            this.LabelPosition = pathMidPoint;
        }

        /// <summary>
        /// The ShowAdorner
        /// </summary>
        private void ShowAdorner()
        {
            // the ConnectionAdorner is created once for each Connection
            if (this.connectionAdorner == null)
            {
                EasyDb.Diagramming.DesignerCanvas designer = VisualTreeHelper.GetParent(this) as EasyDb.Diagramming.DesignerCanvas;

                AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this);
                if (adornerLayer != null)
                {
                    this.connectionAdorner = new ConnectionAdorner(designer, this);
                    adornerLayer.Add(this.connectionAdorner);
                }
            }
            this.connectionAdorner.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// The HideAdorner
        /// </summary>
        internal void HideAdorner()
        {
            if (this.connectionAdorner != null)
                this.connectionAdorner.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// The Connection_Unloaded
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="RoutedEventArgs"/></param>
        internal void Connection_Unloaded(object sender, RoutedEventArgs e)
        {
            // do some housekeeping when Connection is unloaded

            // remove event handler
            this.Source = null;
            this.Sink = null;

            // remove adorner
            if (this.connectionAdorner != null)
            {
                EasyDb.Diagramming.DesignerCanvas designer = VisualTreeHelper.GetParent(this) as EasyDb.Diagramming.DesignerCanvas;

                AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this);
                if (adornerLayer != null)
                {
                    adornerLayer.Remove(this.connectionAdorner);
                    this.connectionAdorner = null;
                }
            }
        }

        // we could use DependencyProperties as well to inform others of property changes
        /// <summary>
        /// Defines the PropertyChanged
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// The OnPropertyChanged
        /// </summary>
        /// <param name="name">The name<see cref="string"/></param>
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }

    /// <summary>
    /// Defines the ArrowSymbol
    /// </summary>
    public enum ArrowSymbol
    {
        /// <summary>
        /// Defines the None
        /// </summary>
        None,

        /// <summary>
        /// Defines the Arrow
        /// </summary>
        Arrow,

        /// <summary>
        /// Defines the Diamond
        /// </summary>
        Diamond
    }
}
