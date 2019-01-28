namespace EasyDb.ViewModel.Panes
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Defines the <see cref="PaneStyleTemplateSelector" />
    /// </summary>
    public class PaneStyleTemplateSelector : StyleSelector
    {
        /// <summary>
        /// Gets or sets the PaneWindowStyle
        /// </summary>
        public Style PaneWindowStyle { get; set; }

        /// <summary>
        /// The SelectStyle
        /// </summary>
        /// <param name="item">The item<see cref="object"/></param>
        /// <param name="container">The container<see cref="DependencyObject"/></param>
        /// <returns>The <see cref="Style"/></returns>
        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (item is PaneBaseViewModel)
            {
                return this.PaneWindowStyle;
            }

            return base.SelectStyle(item, container);
        }
    }
}