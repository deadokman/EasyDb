using System;

namespace EasyDb.ProjectManagment.Configuration
{
    /// <summary>
    /// Project history item
    /// </summary>
    public class ProjectHistItem
    {
        /// <summary>
        /// Last date time asscess
        /// </summary>
        public DateTime LastAccess { get; set; }

        /// <summary>
        /// Project name
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// Project file location
        /// </summary>
        public string ProjectFileLocation { get; set; }

        /// <summary>
        /// Pinned project
        /// </summary>
        public bool Pinned { get; set; }
    }
}
