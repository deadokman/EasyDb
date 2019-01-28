namespace EasyDb.ViewModel
{
    using EasyDb.View;
    using System.Windows.Controls;

    /// <summary>
    /// Defines the <see cref="DiagramDesignerViewModel" />
    /// </summary>
    public class DiagramDesignerViewModel : PaneBaseViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DiagramDesignerViewModel"/> class.
        /// </summary>
        /// <param name="title">The title<see cref="string"/></param>
        public DiagramDesignerViewModel(string title)
            : base(title)
        {
            var view = new SqlQueryToolView();
            view.DataContext = this;
            ViewInstance = view;
        }

        /// <summary>
        /// Gets the ViewInstance
        /// </summary>
        public override UserControl ViewInstance { get; }
    }
}
