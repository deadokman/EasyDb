namespace EasyDb.Diagramming
{
    // Common interface for items that can be selected
    // on the DesignerCanvas; used by DesignerItem and Connection
    /// <summary>
    /// Defines the <see cref="ISelectable" />
    /// </summary>
    public interface ISelectable
    {
        /// <summary>
        /// Gets or sets a value indicating whether IsSelected
        /// </summary>
        bool IsSelected { get; set; }
    }
}
