using System.Collections.Generic;
using EasyDb.ProjectManagment.Eventing;
using LogLevel = NLog.LogLevel;

namespace EasyDb.ProjectManagment
{
    using Autofac.Extras.NLog;
    using EasyDb.Model;
    using EasyDb.ProjectManagment.Annotations;
    using EasyDb.ProjectManagment.Configuration;
    using EasyDb.ProjectManagment.Intefraces;
    using EasyDb.ProjectManagment.ProjectSchema;
    using Edb.Environment.Interface;
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Xml.Serialization;

    /// <summary>
    /// Manage projects with files and datasources
    /// </summary>
    public class ProjectEnvironment : IProjectEnvironment
    {
        /// <summary>
        /// Defines the _datasourceManager
        /// </summary>
        private readonly IDataSourceManager _datasourceManager;

        /// <summary>
        /// Defines the _logger
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Defines the _currentProject
        /// </summary>
        private EasyDbProject _currentProject;

        /// <summary>
        /// Defines the _projectSerializer
        /// </summary>
        private XmlSerializer _projectSerializer;

        /// <summary>
        /// Defines the _histotySerializer
        /// </summary>
        private XmlSerializer _histotySerializer;

        /// <summary>
        /// Defines the ProjectInformationFilepath
        /// </summary>
        private readonly string ProjectInformationFilepath;

        /// <summary>
        /// Defines the _applicationConfigPath
        /// </summary>
        private string _applicationConfigPath;

        /// <summary>
        /// Defines the _datasourceLibPath
        /// </summary>
        private string _datasourceLibPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectEnvironment"/> class.
        /// </summary>
        /// <param name="datasourceManager">Datasource manager</param>
        /// <param name="logger">The logger<see cref="ILogger"/></param>
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
        /// Gets the EdbProjectFile
        /// Get default name for Edb project file
        /// </summary>
        public string EdbProjectFile { get; } = "ProjectConfigurations.xml";

        /// <summary>
        /// Gets the EdbProjectFolder
        /// Gets default name for Edb Project folder
        /// </summary>
        public string EdbProjectFolder { get; } = ".edb";

        /// <summary>
        /// Gets the HistoryInformationFile
        /// Name of file with projects history information
        /// </summary>
        public string HistoryInformationFile { get; } = "HistoryInformation.xml";

        /// <summary>
        /// Gets the EdbDefaultProjectsFolder
        /// Default folder for empty projects
        /// </summary>
        public string EdbDefaultProjectsFolder { get; } = Path.Combine("Projects", "Default");

        /// <summary>
        /// Raises when project initializes
        /// </summary>
        public event EventHandler<EasyDbProject> ProjectInitialized;

        /// <summary>
        /// Error while loading datasource from project
        /// </summary>
        public event EventHandler<DatasourceLoadError> DatasourceLoadError;

        /// <summary>
        /// Project environment initialization completed
        /// </summary>
        public event EventHandler<ProjectEnvironmentInitialized> InitializationCompleted;

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
            InitializationCompleted?.Invoke(this, new ProjectEnvironmentInitialized());
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
        /// Gets or sets the HistoryInformation
        /// Project opened today
        /// </summary>
        public HistoryInformation HistoryInformation { get; set; }

        /// <summary>
        /// Gets or sets the CurrentProject
        /// Current working project
        /// </summary>
        public EasyDbProject CurrentProject
        {
            get => _currentProject;
            set
            {
                var initialized = value != null && _currentProject != value;
                _currentProject = value;
                if (initialized)
                {
                    ProjectInitialized.Invoke(this, _currentProject);
                }
            }
        }

        /// <summary>
        /// Path to new foldr project
        /// </summary>
        /// <param name="folderProjectPath">Folder path</param>
        /// <param name="dsConfiguration">Ds config</param>
        /// <returns>The <see cref="Task{EasyDbProject}"/></returns>
        public async Task<EasyDbProject> InitializeNewProject(
            string folderProjectPath = null,
            UserDatasourceConfiguration dsConfiguration = null)
        {
            var task = Task.Factory.StartNew(() =>
            {
                var folder = folderProjectPath ?? Path.Combine(_applicationConfigPath, EdbDefaultProjectsFolder);
                if (String.IsNullOrEmpty(folderProjectPath) && !Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                if (!Directory.Exists(folder))
                {
                    throw new Exception($"No such file or directory {folder}");
                }

                EasyDbProject proj;
                var projectFolder = Path.Combine(folder, EdbProjectFolder);
                var projectFile = Path.Combine(projectFolder, EdbProjectFile);
                if (Directory.Exists(projectFolder) && !string.IsNullOrEmpty(folderProjectPath))
                {
                    throw new Exception("Edb project already initialized");
                }
                else if(string.IsNullOrEmpty(folderProjectPath) && Directory.Exists(projectFolder) && File.Exists(projectFile))
                {
                    proj = LoadEdbProject(projectFile).Result;
                }
                else
                {
                    var dir = Directory.CreateDirectory(projectFolder);
                    dir.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
                    proj = new EasyDbProject(folder, projectFile)
                    {
                        ProjName = String.Concat("NewDb", DateTime.Today.ToString("g"))
                    };

                    using (var f = File.Create(projectFile))
                    {
                        _projectSerializer.Serialize(f, proj);
                    }
                }

                if (dsConfiguration != null)
                {
                    proj.ConfigurationSources.Add(dsConfiguration);
                }

                HistoryInformation.ProjectsHistory.Add(new ProjectHistItem()
                {
                    ProjectName = proj.ProjName,
                    LastAccess = DateTime.Now,
                    Pinned = false,
                    ProjectFileLocation = proj.ProjectFolderPath
                });

                StoreHistoryInformation(_applicationConfigPath);

                CurrentProject = proj;
                return proj;
            });

            return await task;
        }

        /// <summary>
        /// Try load project from history item
        /// </summary>
        /// <param name="histItem">History item</param>
        /// <returns>Project load result</returns>
        public async Task<ProjectLoadResult> TryLoadProject(ProjectHistItem histItem)
        {
            return await TryLoadProject(Path.Combine(histItem.ProjectFileLocation));
        }

        /// <summary>
        /// Try load project from path to folder
        /// </summary>
        /// <param name="projectFolderPath">Path to project folder</param>
        /// <returns>Project load result</returns>
        public async Task<ProjectLoadResult> TryLoadProject(string projectFolderPath)
        {
            var result = new ProjectLoadResult()
            {
                ProjectFileLocatedSuccessfully = true,
                LoadSuccess = true
            };

            var path = Path.Combine(projectFolderPath, EdbProjectFolder, EdbProjectFile);
            if (!File.Exists(path))
            {
                result.ProjectFileLocatedSuccessfully = false;
                return result;
            }

            try
            {
                CurrentProject = await LoadEdbProject(path);
            }
            catch (Exception e)
            {
                result.ExceptionMessage = e.ToString();
                result.LoadSuccess = false;
                return result;
            }

            return result;
        }

        /// <summary>
        /// The LoadEdbProject
        /// </summary>
        /// <param name="projectFilePath">The projectFilePath<see cref="string"/></param>
        /// <returns>The <see cref="EasyDbProject"/></returns>
        private async Task<EasyDbProject> LoadEdbProject(string projectFilePath)
        {
            //TODO: Add project migrations
            return await Task.Factory.StartNew(() =>
            {
                using (var fs = File.OpenRead(projectFilePath))
                {
                    var projData = (EasyDbProject)_projectSerializer.Deserialize(fs);
                    var toRemove = new List<UserDatasourceConfiguration>();
                    _datasourceManager.ValidateUserdatasourceConfigurations(
                        projData.ConfigurationSources,
                        (conf, msg) =>
                        {
                            bool skipAll;
                            _logger.Log(LogLevel.Error, msg);
                            if (DatasourceLoadError != null)
                            {
                                var args = new DatasourceLoadError() { ErrorMessage = msg };
                                DatasourceLoadError.Invoke(this, args);
                                args.Mrs.Wait();
                                skipAll = args.SkipForall;
                                conf.BrokeReason = msg;
                                conf.IsConfigurationBroken = true;
                                if (!args.SkipAndStay && !skipAll)
                                {
                                    toRemove.Add(conf);
                                }
                            }
                        });

                    projData.ConfigurationSources = projData.ConfigurationSources.Except(toRemove).ToList();
                    return projData;
                }
            });
        }

        /// <summary>
        /// The LoadHistoryInformation
        /// </summary>
        /// <param name="pathToAppfolder">The pathToAppfolder<see cref="string"/></param>
        private void LoadHistoryInformation(string pathToAppfolder)
        {
            var historyConfigFile = Path.Combine(pathToAppfolder, HistoryInformationFile);
            if (!File.Exists(historyConfigFile))
            {
                HistoryInformation = new HistoryInformation()
                {
                    ProjectsHistory = new List<ProjectHistItem>()
                };

                return;
            }

            //TODO: Implement migrations later
            try
            {
                using (var fs = File.OpenRead(historyConfigFile))
                {
                    HistoryInformation = (HistoryInformation)_histotySerializer.Deserialize(fs);
                }

                HistoryInformation.ProjectsHistory = HistoryInformation.ProjectsHistory.Distinct().ToList();
            }
            catch (Exception ex)
            {
                _logger.Error("Error while loading projects history information", ex);
            }
        }

        /// <summary>
        /// The StoreHistoryInformation
        /// </summary>
        /// <param name="pathToAppfolder">The pathToAppfolder<see cref="string"/></param>
        public void StoreHistoryInformation(string pathToAppfolder = null)
        {
            var path = pathToAppfolder ?? _applicationConfigPath;
            var historyConfigFile = Path.Combine(path, HistoryInformationFile);
            StoreWithTemporaryFile(historyConfigFile, (s) =>
            {
                _histotySerializer.Serialize(s, HistoryInformation);
            });
        }

        /// <summary>
        /// The StoreWithTemporaryFile
        /// </summary>
        /// <param name="filePath">The filePath<see cref="string"/></param>
        /// <param name="invoke">The invoke<see cref="Action{Stream}"/></param>
        private void StoreWithTemporaryFile(string filePath, Action<Stream> invoke)
        {
            var tmpFile = string.Concat(filePath, "$tmp");

            var fileExists = File.Exists(filePath);
            if (fileExists)
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
                using (var dsfs = File.Open(filePath, fileExists ? FileMode.Truncate : FileMode.CreateNew))
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
