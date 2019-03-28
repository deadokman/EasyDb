namespace EasyDb.ProjectManagment.Configuration
{
    /// <summary>
    /// Project history item
    /// </summary>
    public class ProjectHistItem
    {
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
