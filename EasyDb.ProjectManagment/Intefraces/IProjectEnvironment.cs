using System;
using EasyDb.ProjectManagment.Eventing;

namespace EasyDb.ProjectManagment.Intefraces
{
    using EasyDb.Model;
    using EasyDb.ProjectManagment.Configuration;
    using EasyDb.ProjectManagment.ProjectSchema;
    using System.Threading.Tasks;

    /// <summary>
    /// Working environment of applization
    /// </summary>
    public interface IProjectEnvironment
    {
        /// <summary>
        /// Raises when project initializes
        /// </summary>
        event EventHandler<EasyDbProject> ProjectInitialized;

        /// <summary>
        /// Error while loading datasource from project
        /// </summary>
        event EventHandler<DatasourceLoadError> DatasourceLoadError;

        /// <summary>
        /// Project environment initialization completed
        /// </summary>
        event EventHandler<ProjectEnvironmentInitialized> InitializationCompleted; 

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
        /// Gets the CurrentProject
        /// Current project
        /// </summary>
        EasyDbProject CurrentProject { get; }

        /// <summary>
        /// Gets the HistoryInformation
        /// History information about projects
        /// </summary>
        HistoryInformation HistoryInformation { get; }

        /// <summary>
        /// Path to new foldr project
        /// </summary>
        /// <param name="folder">Folder path</param>
        /// <param name="dsConfiguration">Ds config</param>
        /// <returns>The <see cref="Task{EasyDbProject}"/></returns>
        Task<EasyDbProject> InitializeNewProject(string folder = null, UserDatasourceConfiguration dsConfiguration = null);

        /// <summary>
        /// Gets the EdbProjectFolder
        /// Default name for edb project folder
        /// </summary>
        string EdbProjectFolder { get; }

        /// <summary>
        /// Gets the EdbProjectFile
        /// Edb project file name
        /// </summary>
        string EdbProjectFile { get; }

        /// <summary>
        /// Gets the HistoryInformationFile
        /// Name of file with projects history information
        /// </summary>
        string HistoryInformationFile { get; }

        /// <summary>
        /// The StoreHistoryInformation
        /// </summary>
        /// <param name="pathToAppfolder">The pathToAppfolder<see cref="string"/></param>
        void StoreHistoryInformation(string pathToAppfolder = null);

        /// <summary>
        /// Try load project from history item
        /// </summary>
        /// <param name="histItem">History item</param>
        /// <returns>Project load result</returns>
        Task<ProjectLoadResult> TryLoadProject(ProjectHistItem histItem);


        /// <summary>
        /// Try load project from path to folder
        /// </summary>
        /// <param name="projectFolderPath">Path to project folder</param>
        /// <returns>Project load result</returns>
        Task<ProjectLoadResult> TryLoadProject(string projectFolderPath);
    }
}
