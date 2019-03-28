using EasyDb.Model;

namespace EasyDb.ProjectManagment.ProjectSchema
{
    /// <summary>
    /// Represents the project of EasyDb
    /// </summary>
    public class EasyDbProject
    {
        /// <summary>
        /// Project folder path
        /// </summary>
        public string ProjectFolderPath { get; set; }

        /// <summary>
        /// User datasource configurations
        /// </summary>
        public UserDatasourceConfiguration[] ConfigurationSources { get; set; }

        /// <summary>
        /// Group files by types (knows types)
        /// </summary>
        public bool GroupFiles { get; set; }
    }
}
