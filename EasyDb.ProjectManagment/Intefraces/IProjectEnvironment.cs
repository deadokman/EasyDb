using EasyDb.Model;
using EasyDb.ProjectManagment.Configuration;
using EasyDb.ProjectManagment.ProjectSchema;

namespace EasyDb.ProjectManagment.Intefraces
{
    /// <summary>
    /// Working environment of applization
    /// </summary>
    public interface IProjectEnvironment
    {
        /// <summary>
        /// Initialize proje
        /// </summary>
        /// <param name="datasourceLibPath">path to datasource drivers libs</param>
        /// <param name="applicationConfigurationPath">Local appforder path</param>
        void Initialize(string datasourceLibPath, string applicationConfigurationPath);

        /// <summary>
        /// Apply new datasource configurations
        /// </summary>
        /// <param name="datasourceConfiguration">Datasource configuration</param>
        void ApplyDatasourceConfig(UserDatasourceConfiguration datasourceConfiguration);

        /// <summary>
        /// Current project
        /// </summary>
        EasyDbProject CurrentProject { get; }

        /// <summary>
        /// History information about projects
        /// </summary>
        HistoryInformation HistoryInformation { get; }

        /// <summary>
        /// Path to new foldr project
        /// </summary>
        /// <param name="folder">Folder path</param>
        /// <param name="dsConfiguration">Ds config</param>
        void InitializeNewProject(string folder, UserDatasourceConfiguration dsConfiguration = null);

        /// <summary>
        /// Default name for edb project folder
        /// </summary>
        string EdbProjectFolder { get; }

        /// <summary>
        /// Edb project file name
        /// </summary>
        string EdbProjectFile { get; }

        /// <summary>
        /// Name of file with projects history information
        /// </summary>
        string HistoryInformationFile { get; }
    }
}
