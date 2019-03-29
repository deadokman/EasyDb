using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Autofac.Extras.NLog;
using EasyDb.Model;
using EasyDb.ProjectManagment.Annotations;
using EasyDb.ProjectManagment.Configuration;
using EasyDb.ProjectManagment.Intefraces;
using EasyDb.ProjectManagment.ProjectSchema;
using Edb.Environment.Interface;

namespace EasyDb.ProjectManagment
{
    /// <summary>
    /// Manage projects with files and datasources
    /// </summary>
    public class ProjectEnvironment : IProjectEnvironment
    {
        private readonly IDataSourceManager _datasourceManager;

        private readonly ILogger _logger;

        private EasyDbProject _currentProject;

        private XmlSerializer _projectSerializer;

        private XmlSerializer _histotySerializer;

        private readonly string ProjectInformationFilepath;

        private string _applicationConfigPath;

        private string _datasourceLibPath;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="datasourceManager">Datasource manager</param>
        public ProjectEnvironment([NotNull] IDataSourceManager datasourceManager, [NotNull] ILogger logger)
        {
            _datasourceManager = datasourceManager ?? throw new ArgumentNullException(nameof(datasourceManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _projectSerializer = new XmlSerializer(
                typeof(EasyDbProject), 
                _datasourceManager.GetAdditionalOptionTypes().ToArray());
            _histotySerializer = new XmlSerializer(typeof(HistoryInformation));
        }

        /// <summary>
        /// Get default name for Edb project file
        /// </summary>
        public string EdbProjectFile { get; } = "ProjectConfigurations.xml";

        /// <summary>
        /// Gets default name for Edb Project folder
        /// </summary>
        public string EdbProjectFolder { get; } = ".edb";

        public string HistoryInformationFile { get; } = "HistoryInformation.xml";

        /// <summary>
        /// Initialize project
        /// </summary>
        /// <param name="datasourceLibPath">path to datasource drivers libs</param>
        /// <param name="applicationConfigurationPath">Local appfolder path</param>
        public void Initialize(string datasourceLibPath, string applicationConfigurationPath)
        {
            _datasourceManager.InitialLoad(datasourceLibPath);
            _applicationConfigPath = applicationConfigurationPath;
            _datasourceLibPath = datasourceLibPath;

            // load projects history if exists
            LoadHistoryInformation(applicationConfigurationPath);

        }

        /// <summary>
        /// Apply new datasource configurations
        /// </summary>
        /// <param name="datasourceConfiguration">Datasource configuration</param>
        public void ApplyDatasourceConfig(UserDatasourceConfiguration datasourceConfiguration)
        {
            if (CurrentProject == null)
            {
                throw new NullReferenceException("Project not initialized, can't apply DataSource");
            }

            CurrentProject.ApplyUserDatasource(datasourceConfiguration);
            StoreProjectConfiguration();
        }

        /// <summary>
        /// Save datasource configuration to config file at hard drive
        /// </summary>
        public void StoreProjectConfiguration()
        {
            StoreWithTemporaryFile(CurrentProject?.ProjectFullPath,
                (s) => { _projectSerializer.Serialize(s, CurrentProject); });
        }

        /// <summary>
        /// Project opened today
        /// </summary>
        public HistoryInformation HistoryInformation { get; set; }

        /// <summary>
        /// Current working project
        /// </summary>
        public EasyDbProject CurrentProject
        {
            get => _currentProject;
            set
            {
                _currentProject = value;
            }
        }

        /// <summary>
        /// Path to new foldr project
        /// </summary>
        /// <param name="folder">Folder path</param>
        /// <param name="dsConfiguration">Ds config</param>
        public void InitializeNewProject(string folder, UserDatasourceConfiguration dsConfiguration = null)
        {
            if (!Directory.Exists(folder))
            {
                throw new Exception($"No such file or directory {folder}");
            }

            var projectFolder = Path.Combine(folder, EdbProjectFolder);
            if (Directory.Exists(projectFolder))
            {
                throw new Exception("Edb project already initialized");
            }

            var dir = Directory.CreateDirectory(projectFolder);
            dir.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
            var projectFile = Path.Combine(dir.FullName, EdbProjectFile);

            CurrentProject = new EasyDbProject(folder, projectFile);
            if (dsConfiguration != null)
            {
                CurrentProject.ConfigurationSources.Add(dsConfiguration);
            }

            using (var f = File.Create(projectFile))
            {
                _projectSerializer.Serialize(f, CurrentProject);
            }
        }

        private EasyDbProject LoadEdbProject(string projectFolderPath)
        {
            throw new NotImplementedException();
        }

        private void LoadHistoryInformation(string pathToAppfolder)
        {
            var historyConfigFile = Path.Combine(pathToAppfolder, HistoryInformationFile);
            if (!File.Exists(historyConfigFile))
            {
                HistoryInformation = new HistoryInformation();
                return;
            }

            //TODO: Implement migrations later
            try
            {
                using (var fs = File.OpenRead(historyConfigFile))
                {
                    HistoryInformation = (HistoryInformation)_histotySerializer.Deserialize(fs);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error while loading projects history information", ex);
            }
        }

        private void StoreHistoryInformation(string pathToAppfolder)
        {
            var historyConfigFile = Path.Combine(pathToAppfolder, HistoryInformationFile);
            StoreWithTemporaryFile(historyConfigFile, (s) =>
            {
                _histotySerializer.Serialize(s, HistoryInformation);
            });
        }

        private void StoreWithTemporaryFile(string filePath, Action<Stream> invoke)
        {
            var tmpFile = string.Concat(filePath, "$tmp");

            if (File.Exists(filePath))
            {
                // Create temporary copy of storage file
                try
                {
                    File.Copy(filePath, tmpFile, true);
                }
                catch (Exception ex)
                {
                    var msg = "Exception while saving datasource configuration";
                    throw new Exception(msg, ex);
                }
            }

            try
            {
                using (var dsfs = File.Open(filePath, FileMode.Truncate))
                {
                    invoke.Invoke(dsfs);
                }
            }
            catch (Exception ex)
            {
                var msg = "Error while writing new datasource configurations, trying to revert";
                _logger.Error(msg, ex);
                File.Copy(tmpFile, filePath, true);
            }

            File.Delete(tmpFile);
        }
    }
}
