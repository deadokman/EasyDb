namespace EasyDb.ViewModel
{
    using System.Linq;

    using Xceed.Wpf.AvalonDock.Layout;

    /// <summary>
    /// Defines the <see cref="LayoutInitializer" />
    /// </summary>
    public class LayoutInitializer : ILayoutUpdateStrategy
    {
        /// <summary>
        /// The AfterInsertAnchorable
        /// </summary>
        /// <param name="layout">The layout<see cref="LayoutRoot"/></param>
        /// <param name="anchorableShown">The anchorableShown<see cref="LayoutAnchorable"/></param>
        public void AfterInsertAnchorable(LayoutRoot layout, LayoutAnchorable anchorableShown)
        {
        }

        /// <summary>
        /// The AfterInsertDocument
        /// </summary>
        /// <param name="layout">The layout<see cref="LayoutRoot"/></param>
        /// <param name="anchorableShown">The anchorableShown<see cref="LayoutDocument"/></param>
        public void AfterInsertDocument(LayoutRoot layout, LayoutDocument anchorableShown)
        {
        }

        /// <summary>
        /// The BeforeInsertAnchorable
        /// </summary>
        /// <param name="layout">The layout<see cref="LayoutRoot"/></param>
        /// <param name="anchorableToShow">The anchorableToShow<see cref="LayoutAnchorable"/></param>
        /// <param name="destinationContainer">The destinationContainer<see cref="ILayoutContainer"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public bool BeforeInsertAnchorable(
            LayoutRoot layout,
            LayoutAnchorable anchorableToShow,
            ILayoutContainer destinationContainer)
        {
            // AD wants to add the anchorable into destinationContainer
            // just for test provide a new anchorablepane
            // if the pane is floating let the manager go ahead
            LayoutAnchorablePane destPane = destinationContainer as LayoutAnchorablePane;
            if (destinationContainer != null && destinationContainer.FindParent<LayoutFloatingWindow>() != null)
            {
                return false;
            }

            var toolsPane = layout.Descendents().OfType<LayoutAnchorablePane>()
                .FirstOrDefault(d => d.Name == "ToolsPane");
            if (toolsPane != null)
            {
                toolsPane.Children.Add(anchorableToShow);
                return true;
            }

            return false;
        }

        /// <summary>
        /// The BeforeInsertDocument
        /// </summary>
        /// <param name="layout">The layout<see cref="LayoutRoot"/></param>
        /// <param name="anchorableToShow">The anchorableToShow<see cref="LayoutDocument"/></param>
        /// <param name="destinationContainer">The destinationContainer<see cref="ILayoutContainer"/></param>
        /// <returns>The <see cref="bool"/></returns>
        public bool BeforeInsertDocument(
            LayoutRoot layout,
            LayoutDocument anchorableToShow,
            ILayoutContainer destinationContainer)
        {
            return false;
        }
    }
}