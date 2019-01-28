namespace EasyDb.Diagramming
{
    using System;

    /// <summary>
    /// Defines the <see cref="IGroupable" />
    /// </summary>
    public interface IGroupable
    {
        /// <summary>
        /// Gets the ID
        /// </summary>
        Guid ID { get; }

        /// <summary>
        /// Gets or sets the ParentID
        /// </summary>
        Guid ParentID { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether IsGroup
        /// </summary>
        bool IsGroup { get; set; }
    }
}
