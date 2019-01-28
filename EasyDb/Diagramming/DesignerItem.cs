namespace EasyDb.Diagramming
{
    using EasyDb.Diagramming.Controls;
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    //These attributes identify the types of the named parts that are used for templating
    /// <summary>
    /// Defines the <see cref="DesignerItem" />
    /// </summary>
    [TemplatePart(Name = "PART_DragThumb", Type = typeof(DragThumb))]
    [TemplatePart(Name = "PART_ResizeDecorator", Type = typeof(Control))]
    [TemplatePart(Name = "PART_ConnectorDecorator", Type = typeof(Control))]
    [TemplatePart(Name = "PART_ContentPresenter", Type = typeof(ContentPresenter))]
    public class DesignerItem : ContentControl, ISelectable, IGroupable
    {
        /// <summary>
        /// Defines the id
        /// </summary>
        private Guid id;

        /// <summary>
        /// Gets the ID
        /// </summary>
        public Guid ID
        {
            get { return id; }
        }

        /// <summary>
        /// Gets or sets the ParentID
        /// </summary>
        public Guid ParentID
        {
            get { return (Guid)GetValue(ParentIDProperty); }
            set { SetValue(ParentIDProperty, value); }
        }

        /// <summary>
        /// Defines the ParentIDProperty
        /// </summary>
        public static readonly DependencyProperty ParentIDProperty = DependencyProperty.Register("ParentID", typeof(Guid), typeof(DesignerItem));

        /// <summary>
        /// Gets or sets a value indicating whether IsGroup
        /// </summary>
        public bool IsGroup
        {
            get { return (bool)GetValue(IsGroupProperty); }
            set { SetValue(IsGroupProperty, value); }
        }

        /// <summary>
        /// Defines the IsGroupProperty
        /// </summary>
        public static readonly DependencyProperty IsGroupProperty =
            DependencyProperty.Register("IsGroup", typeof(bool), typeof(DesignerItem));

        /// <summary>
        /// Gets or sets a value indicating whether IsSelected
        /// </summary>
        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        /// <summary>
        /// Defines the IsSelectedProperty
        /// </summary>
        public static readonly DependencyProperty IsSelectedProperty =
          DependencyProperty.Register("IsSelected",
                                       typeof(bool),
                                       typeof(DesignerItem),
                                       new FrameworkPropertyMetadata(false));

        // can be used to replace the default template for the DragThumb
        /// <summary>
        /// Defines the DragThumbTemplateProperty
        /// </summary>
        public static readonly DependencyProperty DragThumbTemplateProperty =
            DependencyProperty.RegisterAttached("DragThumbTemplate", typeof(ControlTemplate), typeof(DesignerItem));

        /// <summary>
        /// The GetDragThumbTemplate
        /// </summary>
        /// <param name="element">The element<see cref="UIElement"/></param>
        /// <returns>The <see cref="ControlTemplate"/></returns>
        public static ControlTemplate GetDragThumbTemplate(UIElement element)
        {
            return (ControlTemplate)element.GetValue(DragThumbTemplateProperty);
        }

        /// <summary>
        /// The SetDragThumbTemplate
        /// </summary>
        /// <param name="element">The element<see cref="UIElement"/></param>
        /// <param name="value">The value<see cref="ControlTemplate"/></param>
        public static void SetDragThumbTemplate(UIElement element, ControlTemplate value)
        {
            element.SetValue(DragThumbTemplateProperty, value);
        }

        // can be used to replace the default template for the ConnectorDecorator
        /// <summary>
        /// Defines the ConnectorDecoratorTemplateProperty
        /// </summary>
        public static readonly DependencyProperty ConnectorDecoratorTemplateProperty =
            DependencyProperty.RegisterAttached("ConnectorDecoratorTemplate", typeof(ControlTemplate), typeof(DesignerItem));

        /// <summary>
        /// The GetConnectorDecoratorTemplate
        /// </summary>
        /// <param name="element">The element<see cref="UIElement"/></param>
        /// <returns>The <see cref="ControlTemplate"/></returns>
        public static ControlTemplate GetConnectorDecoratorTemplate(UIElement element)
        {
            return (ControlTemplate)element.GetValue(ConnectorDecoratorTemplateProperty);
        }

        /// <summary>
        /// The SetConnectorDecoratorTemplate
        /// </summary>
        /// <param name="element">The element<see cref="UIElement"/></param>
        /// <param name="value">The value<see cref="ControlTemplate"/></param>
        public static void SetConnectorDecoratorTemplate(UIElement element, ControlTemplate value)
        {
            element.SetValue(ConnectorDecoratorTemplateProperty, value);
        }

        // while drag connection procedure is ongoing and the mouse moves over 
        // this item this value is true; if true the ConnectorDecorator is triggered
        // to be visible, see template
        /// <summary>
        /// Gets or sets a value indicating whether IsDragConnectionOver
        /// </summary>
        public bool IsDragConnectionOver
        {
            get { return (bool)GetValue(IsDragConnectionOverProperty); }
            set { SetValue(IsDragConnectionOverProperty, value); }
        }

        /// <summary>
        /// Defines the IsDragConnectionOverProperty
        /// </summary>
        public static readonly DependencyProperty IsDragConnectionOverProperty =
            DependencyProperty.Register("IsDragConnectionOver",
                                         typeof(bool),
                                         typeof(DesignerItem),
                                         new FrameworkPropertyMetadata(false));

        /// <summary>
        /// Initializes static members of the <see cref="DesignerItem"/> class.
        /// </summary>
        static DesignerItem()
        {
            // set the key to reference the style for this control
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(
                typeof(DesignerItem), new FrameworkPropertyMetadata(typeof(DesignerItem)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DesignerItem"/> class.
        /// </summary>
        /// <param name="id">The id<see cref="Guid"/></param>
        public DesignerItem(Guid id)
        {
            this.id = id;
            this.Loaded += new RoutedEventHandler(DesignerItem_Loaded);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DesignerItem"/> class.
        /// </summary>
        public DesignerItem()
            : this(Guid.NewGuid())
        {
        }

        /// <summary>
        /// The OnPreviewMouseDown
        /// </summary>
        /// <param name="e">The e<see cref="MouseButtonEventArgs"/></param>
        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(e);
            DesignerCanvas designer = VisualTreeHelper.GetParent(this) as DesignerCanvas;

            // update selection
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
        /// The DesignerItem_Loaded
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="e">The e<see cref="RoutedEventArgs"/></param>
        internal void DesignerItem_Loaded(object sender, RoutedEventArgs e)
        {
            if (base.Template != null)
            {
                ContentPresenter contentPresenter =
                    this.Template.FindName("PART_ContentPresenter", this) as ContentPresenter;
                if (contentPresenter != null)
                {
                    UIElement contentVisual = VisualTreeHelper.GetChild(contentPresenter, 0) as UIElement;
                    if (contentVisual != null)
                    {
                        DragThumb thumb = this.Template.FindName("PART_DragThumb", this) as DragThumb;
                        if (thumb != null)
                        {
                            ControlTemplate template =
                                DesignerItem.GetDragThumbTemplate(contentVisual) as ControlTemplate;
                            if (template != null)
                                thumb.Template = template;
                        }
                    }
                }
            }
        }
    }
}
