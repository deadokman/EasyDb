namespace EasyDb.Diagramming
{
    using System.Windows;
    using System.Windows.Controls;

    // Implements ItemsControl for ToolboxItems
    /// <summary>
    /// Defines the <see cref="Toolbox" />
    /// </summary>
    public class Toolbox : ItemsControl
    {
        // Defines the ItemHeight and ItemWidth properties of
        // the WrapPanel used for this Toolbox
        /// <summary>
        /// Gets or sets the ItemSize
        /// </summary>
        public Size ItemSize
        {
            get { return itemSize; }
            set { itemSize = value; }
        }

        /// <summary>
        /// Defines the itemSize
        /// </summary>
        private Size itemSize = new Size(50, 50);

        // Creates or identifies the element that is used to display the given item.
        /// <summary>
        /// The GetContainerForItemOverride
        /// </summary>
        /// <returns>The <see cref="DependencyObject"/></returns>
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new ToolboxItem();
        }

        // Determines if the specified item is (or is eligible to be) its own container.
        /// <summary>
        /// The IsItemItsOwnContainerOverride
        /// </summary>
        /// <param name="item">The item<see cref="object"/></param>
        /// <returns>The <see cref="bool"/></returns>
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return (item is ToolboxItem);
        }
    }
}
