namespace EasyDb.ProjectManagment.Configuration
{
    using System;

    /// <summary>
    /// Project history item
    /// </summary>
    public class ProjectHistItem : IEquatable<ProjectHistItem>
    {
        /// <summary>
        /// Gets or sets the LastAccess
        /// Last date time asscess
        /// </summary>
        public DateTime LastAccess { get; set; }

        /// <summary>
        /// Gets or sets the ProjectName
        /// Project name
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// Gets or sets the ProjectFileLocation
        /// Project file location
        /// </summary>
        public string ProjectFileLocation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Pinned
        /// Pinned project
        /// </summary>
        public bool Pinned { get; set; }

        /// <summary>
        /// Launch project automatically
        /// </summary>
        public bool Autolaunch { get; set; }

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// <see langword="true" /> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <see langword="false" />.</returns>
        public bool Equals(ProjectHistItem other)
        {
            return other != null && other.ProjectFileLocation == this.ProjectFileLocation;
        }

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="obj">The object to compare with the current object. </param>
        /// <returns>
        /// <see langword="true" /> if the specified object  is equal to the current object; otherwise, <see langword="false" />.</returns>
        public override bool Equals(object obj)
        {
            var hst = obj as ProjectHistItem;
            return hst != null && hst.ProjectFileLocation != this.ProjectFileLocation;
        }

        /// <summary>Serves as the default hash function. </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return ProjectFileLocation.GetHashCode();
        }
    }
}
