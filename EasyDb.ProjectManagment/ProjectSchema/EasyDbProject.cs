namespace EasyDb.ProjectManagment.ProjectSchema
{
    using EasyDb.Model;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Represents the project of EasyDb
    /// </summary>
    public class EasyDbProject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EasyDbProject"/> class.
        /// </summary>
        public EasyDbProject()
        {
            ConfigurationSources = new List<UserDatasourceConfiguration>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EasyDbProject"/> class.
        /// </summary>
        /// <param name="projectFolderPath">Path to project folder</param>
        /// <param name="projectFilename">Project file path</param>
        public EasyDbProject(string projectFolderPath, string projectFilename)
        {
            ProjectFolderPath = projectFolderPath;
            ProjectFilename = projectFilename;
            ConfigurationSources = new List<UserDatasourceConfiguration>();
        }

        /// <summary>
        /// Gets or sets the ProjName
        /// Name of the project
        /// </summary>
        public string ProjName { get; set; }

        /// <summary>
        /// Gets or sets the ProjectFolderPath
        /// Project folder path
        /// </summary>
        public string ProjectFolderPath { get; set; }

        /// <summary>
        /// Gets or sets the ProjectFilename
        /// Project filename
        /// </summary>
        public string ProjectFilename { get; set; }

        /// <summary>
        /// Gets the ProjectFullPath
        /// Full path to project folder
        /// </summary>
        public string ProjectFullPath => Path.Combine(ProjectFolderPath, ProjectFilename);

        /// <summary>
        /// Gets or sets the ConfigurationSources
        /// User datasource configurations
        /// </summary>
        public List<UserDatasourceConfiguration> ConfigurationSources { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether GroupFiles
        /// Group files by types (knows types)
        /// </summary>
        public bool GroupFiles { get; set; }

        /// <summary>
        /// Добавить объявленный пользователем источник данных в список
        /// </summary>
        /// <param name="udsc">Источник данных прользователя</param>
        public void ApplyUserDatasource(UserDatasourceConfiguration udsc)
        {
            ConfigurationSources.Add(udsc);
        }
    }
}
