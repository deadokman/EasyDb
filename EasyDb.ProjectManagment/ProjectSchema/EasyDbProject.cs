using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using EasyDb.Model;
using Edb.Environment.CommunicationArgs;

namespace EasyDb.ProjectManagment.ProjectSchema
{
    /// <summary>
    /// Represents the project of EasyDb
    /// </summary>
    public class EasyDbProject
    {
        /// <summary>
        /// Project entity
        /// </summary>
        public EasyDbProject()
        {
            ConfigurationSources = new List<UserDatasourceConfiguration>();
        }

        /// <summary>
        /// Secondary constructor
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
        /// Name of the project
        /// </summary>
        public string ProjName { get; set; }

        /// <summary>
        /// Project folder path
        /// </summary>
        public string ProjectFolderPath { get; set; }

        /// <summary>
        /// Project filename
        /// </summary>
        public string ProjectFilename { get; set; }

        /// <summary>
        /// Full path to project folder
        /// </summary>
        public string ProjectFullPath => Path.Combine(ProjectFolderPath, ProjectFilename);

        /// <summary>
        /// User datasource configurations
        /// </summary>
        public List<UserDatasourceConfiguration> ConfigurationSources { get; set; }

        /// <summary>
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
