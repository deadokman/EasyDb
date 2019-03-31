namespace EasyDb.ProjectManagment.Configuration
{
    using System.Collections.Generic;

    /// <summary>
    /// Hold overall information about projects configurations
    /// </summary>
    public class HistoryInformation
    {
        /// <summary>
        /// Gets or sets the ProjectsHistory
        /// </summary>
        public List<ProjectHistItem> ProjectsHistory { get; set; }
    }
}
