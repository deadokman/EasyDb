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
    /// Defines the <see cref="Connector" />
    /// </summary>
    public class Connector : Control, INotifyPropertyChanged
    {
        // drag start point, relative to the DesignerCanvas
        /// <summary>
        /// Defines the dragStartPoint
        /// </summary>
        private Point? dragStartPoint = null;

        /// <summary>
        /// Gets or sets the Orientation
        /// </summary>
        public ConnectorOrientation Orientation { get; set; }

        // center position of this Connector relative to the DesignerCanvas
        /// <summary>
        /// Defines the position
        /// </summary>
        private Point position;

        /// <summary>
        /// Gets or sets the Position
        /// </summary>
        public Point Position
        {
            get { return position; }
            set
            {
                if (position != value)
                {
                    position = value;
                    OnPropertyChanged("Position");
                }
            }
        }

        // the DesignerItem this Connector belongs to;
        // retrieved from DataContext, which is set in the
        // DesignerItem template
        /// <summary>
        /// Defines the parentDesignerItem
        /// </summary>
        private DesignerItem parentDesignerItem;

        /// <summary>
        /// Gets the ParentDesignerItem
        /// </summary>
        public DesignerItem ParentDesignerItem
        {
            get
            {
                if (parentDesignerItem == null)
                    parentDesignerItem = this.DataContext as DesignerItem;

                return parentDesignerItem;
            }
        }

        // keep track of connections that link to this connector
        /// <summary>
        /// Defines the connections
        /// </summary>
        private List<Connection> connections;

        /// <summary>
        /// Gets the Connections
        /// </summary>
        public List<Connection> Connections
        {
            get
            {
                if (connections == null)
                    connections = new List<Connection>();
                return connections;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Connector"/> class.
        /// </summary>
        public Connector()
        {
            // fired when layout changes
            base.LayoutUpdated += new EventHandler(Connector_LayoutUpdated);
        }

        // when the layout changes we update the position property
        /// <summary>
        /// The Connector_LayoutUpdated
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="EventArgs"/></param>
        internal void Connector_LayoutUpdated(object sender, EventArgs e)
        {
            EasyDb.Diagramming.DesignerCanvas designer = GetDesignerCanvas(this);
            if (designer != null)
            {
                //get centre position of this Connector relative to the DesignerCanvas
                this.Position = this.TransformToAncestor(designer).Transform(new Point(this.Width / 2, this.Height / 2));
            }
        }

        /// <summary>
        /// The OnMouseLeftButtonDown
        /// </summary>
        /// <param name="e">The e<see cref="MouseButtonEventArgs"/></param>
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            EasyDb.Diagramming.DesignerCanvas canvas = GetDesignerCanvas(this);
            if (canvas != null)
            {
                // position relative to DesignerCanvas
                this.dragStartPoint = new Point?(e.GetPosition(canvas));
                e.Handled = true;
            }
        }

        /// <summary>
        /// The OnMouseMove
        /// </summary>
        /// <param name="e">The e<see cref="MouseEventArgs"/></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            // if mouse button is not pressed we have no drag operation, ...
            if (e.LeftButton != MouseButtonState.Pressed)
                this.dragStartPoint = null;

            // but if mouse button is pressed and start point value is set we do have one
            if (this.dragStartPoint.HasValue)
            {
                // create connection adorner 
                EasyDb.Diagramming.DesignerCanvas canvas = GetDesignerCanvas(this);
                if (canvas != null)
                {
                    AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(canvas);
                    if (adornerLayer != null)
                    {
                        ConnectorAdorner adorner = new ConnectorAdorner(canvas, this);
                        if (adorner != null)
                        {
                            adornerLayer.Add(adorner);
                            e.Handled = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// The GetInfo
        /// </summary>
        /// <returns>The <see cref="ConnectorInfo"/></returns>
        internal ConnectorInfo GetInfo()
        {
            ConnectorInfo info = new ConnectorInfo();
            info.DesignerItemLeft = EasyDb.Diagramming.DesignerCanvas.GetLeft(this.ParentDesignerItem);
            info.DesignerItemTop = EasyDb.Diagramming.DesignerCanvas.GetTop(this.ParentDesignerItem);
            info.DesignerItemSize = new Size(this.ParentDesignerItem.ActualWidth, this.ParentDesignerItem.ActualHeight);
            info.Orientation = this.Orientation;
            info.Position = this.Position;
            return info;
        }

        // iterate through visual tree to get parent DesignerCanvas
        /// <summary>
        /// The GetDesignerCanvas
        /// </summary>
        /// <param name="element">The element<see cref="DependencyObject"/></param>
        /// <returns>The <see cref="EasyDb.Diagramming.DesignerCanvas"/></returns>
        private EasyDb.Diagramming.DesignerCanvas GetDesignerCanvas(DependencyObject element)
        {
            while (element != null && !(element is EasyDb.Diagramming.DesignerCanvas))
                element = VisualTreeHelper.GetParent(element);

            return element as EasyDb.Diagramming.DesignerCanvas;
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

    // provides compact info about a connector; used for the 
    // routing algorithm, instead of hand over a full fledged Connector
    /// <summary>
    /// Defines the <see cref="ConnectorInfo" />
    /// </summary>
    internal struct ConnectorInfo
    {
        /// <summary>
        /// Gets or sets the DesignerItemLeft
        /// </summary>
        public double DesignerItemLeft { get; set; }

        /// <summary>
        /// Gets or sets the DesignerItemTop
        /// </summary>
        public double DesignerItemTop { get; set; }

        /// <summary>
        /// Gets or sets the DesignerItemSize
        /// </summary>
        public Size DesignerItemSize { get; set; }

        /// <summary>
        /// Gets or sets the Position
        /// </summary>
        public Point Position { get; set; }

        /// <summary>
        /// Gets or sets the Orientation
        /// </summary>
        public ConnectorOrientation Orientation { get; set; }
    }

    /// <summary>
    /// Defines the ConnectorOrientation
    /// </summary>
    public enum ConnectorOrientation
    {
        /// <summary>
        /// Defines the None
        /// </summary>
        None,

        /// <summary>
        /// Defines the Left
        /// </summary>
        Left,

        /// <summary>
        /// Defines the Top
        /// </summary>
        Top,

        /// <summary>
        /// Defines the Right
        /// </summary>
        Right,

        /// <summary>
        /// Defines the Bottom
        /// </summary>
        Bottom
    }
}
