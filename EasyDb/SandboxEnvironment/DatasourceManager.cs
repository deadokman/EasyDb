namespace EasyDb.SandboxEnvironment
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Security;
    using System.Security.Permissions;
    using System.Security.Policy;
    using System.Windows;
    using System.Xml.Serialization;

    using EasyDb.Interfaces.Data;
    using EasyDb.View;
    using EasyDb.ViewModel.DataSource;
    using EasyDb.ViewModel.DataSource.Items;

    using EDb.Interfaces;

    using NLog;

    /// <summary>
    /// Get supported datasource drivers
    /// </summary>
    public sealed class DatasourceManager : IDataSourceManager
    {
        /// <summary>
        /// Datasource storage filepath
        /// </summary>
        private const string DataSourceStorageFile = "Edb.Datasource";

        /// <summary>
        /// Defines the _logger
        /// </summary>
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Supported datasources collection
        /// </summary>
        private Dictionary<Guid, SupportedSourceItem> _supportedDataSources;

        /// <summary>
        /// Defines the _xseri
        /// </summary>
        private XmlSerializer _xseri;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatasourceManager"/> class.
        /// </summary>
        public DatasourceManager()
        {
            this._supportedDataSources = new Dictionary<Guid, SupportedSourceItem>();
            this._xseri = new XmlSerializer(typeof(List<UserDatasourceConfiguration>));
            this.UserdefinedDatasources = new List<UserDataSource>();
        }

        /// <summary>
        /// Defines the DatasourceLoaded
        /// </summary>
        public event DatasourceData DatasourceLoaded;

        /// <summary>
        /// Gets the SupportedDatasources
        /// </summary>
        public IEnumerable<SupportedSourceItem> SupportedDatasources => this._supportedDataSources.Values;

        /// <summary>
        /// Gets or sets the UserdefinedDatasources
        /// Collection of user defined datasources
        /// </summary>
        public List<UserDataSource> UserdefinedDatasources { get; set; }

        /// <summary>
        /// Creates new user defined datasource
        /// </summary>
        /// <param name="module">datasource module</param>
        /// <returns>User defined datasource</returns>
        public UserDataSource CreateNewUserdatasource(IEdbDatasourceModule module)
        {
            var uds = new UserDataSource
                          {
                              LinkedEdbSourceModule = module, SettingsObjects = module.GetDefaultOptionsObjects()
                          };
            uds.SetGuid(module.ModuleGuid);
            this.UserdefinedDatasources.Add(uds);
            return uds;
        }

        /// <summary>
        /// Load datasource modules
        /// Modules load in separate AppDomain for security purpose
        /// </summary>
        /// <param name="datasourceAssembliesPath">The datasourceAssembliesPath<see cref="string"/></param>
        public void InitialLoad(string datasourceAssembliesPath)
        {
            // Initialize secure app domain
            this.InitAppdomainInstance();
            List<UserDataSource> serializedSources;

            // Load and serialize XML doc. Create new one if it does not exists;
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), DataSourceStorageFile);
            if (File.Exists(filePath))
            {
                try
                {
                    serializedSources = (List<UserDataSource>)this._xseri.Deserialize(File.OpenRead(filePath));
                }
                catch (Exception ex)
                {
                    _logger.Error($"Error while parsing user defined sources: \n {ex}");
                    serializedSources = new List<UserDataSource>();
                }
            }
            else
            {
                serializedSources = new List<UserDataSource>();
            }

            if (!Directory.Exists(datasourceAssembliesPath))
            {
                throw new Exception($"Path NotFound {datasourceAssembliesPath}");
            }

            // Get all assemblies from path
            foreach (var assmFile in Directory.GetFiles(datasourceAssembliesPath, "*.dll"))
            {
                try
                {
                    var assembly = Assembly.LoadFile(assmFile);
                    var types = assembly.GetTypes().Where(
                        t => t.IsClass && !t.IsAbstract && t.GetInterfaces().Contains(typeof(IEdbDatasourceModule)));
                    foreach (var type in types)
                    {
                        var attributes = type.GetCustomAttributes(typeof(EdbDatasourceAttribute)).ToArray();
                        if (attributes.Length == 0)
                        {
                            _logger.Warn(
                                Application.Current.Resources["log_NotImplementedAttr"].ToString(),
                                type.Name,
                                assembly.FullName);
                        }

                        if (attributes.Length > 1)
                        {
                            _logger.Warn(
                                Application.Current.Resources["log_NotImplementedAttr"].ToString(),
                                type.Name,
                                assembly.FullName);
                        }

                        var attribute = (EdbDatasourceAttribute)attributes[0];
                        var datasourceInstance = this.ProcessType(type);
                        if (datasourceInstance != null)
                        {
                            datasourceInstance.SetGuid(attribute.SourceGuid);
                            datasourceInstance.SetVersion(attribute.Version);
                            var supportedSourceItem = new SupportedSourceItem(
                                datasourceInstance,
                                (module) =>
                                    {
                                        var uds = this.CreateNewUserdatasource(module);
                                        this.DisplayUserDatasourceProperties(uds);
                                        return uds;
                                    });
                            this._supportedDataSources.Add(attribute.SourceGuid, supportedSourceItem);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error($"err: {ex}");
                }
            }

            // Restore user defined datasource
            foreach (var uds in serializedSources)
            {
                SupportedSourceItem dbSourceModule;

                // Check that user defined data source exists in datasource module
                if (this._supportedDataSources.TryGetValue(uds.DatasourceGuid, out dbSourceModule))
                {
                    uds.LinkedEdbSourceModule = dbSourceModule.Module;
                    this.UserdefinedDatasources.Add(uds);
                }
                else
                {
                    _logger.Warn(Application.Current.Resources["log_NotImplementedAttr"].ToString(), uds.Name);
                }
            }

            if (this.DatasourceLoaded != null)
            {
                this.DatasourceLoaded.Invoke(this._supportedDataSources.Values, this.UserdefinedDatasources);
            }
        }

        /// <summary>
        /// The AppOnLanguageChanged
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        /// <param name="eventArgs">The eventArgs<see cref="EventArgs"/></param>
        private void AppOnLanguageChanged(object sender, EventArgs eventArgs)
        {
        }

        /// <summary>
        /// The DisplayUserDatasourceProperties
        /// </summary>
        /// <param name="uds">The uds<see cref="UserDataSource"/></param>
        private void DisplayUserDatasourceProperties(UserDataSource uds)
        {
            var dlgWindow = new DatasourceSettingsView();
            dlgWindow.DataContext = uds;
            dlgWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            dlgWindow.Owner = Application.Current.MainWindow;
            dlgWindow.ShowDialog();
        }

        /// <summary>
        /// Initialize "SandBox" application domain
        /// </summary>
        private void InitAppdomainInstance()
        {
            var ev = new Evidence();
            ev.AddHostEvidence(new Zone(SecurityZone.Internet));
            var adSetup = new AppDomainSetup();
            var internetPS = SecurityManager.GetStandardSandbox(ev);
            var fullTrustAssembly = typeof(EasyDb.App).Assembly.Evidence.GetHostEvidence<StrongName>();
        }

        /// <summary>
        /// Process plugin type
        /// </summary>
        /// <param name="t">The t<see cref="Type"/></param>
        /// <returns>The <see cref="IEdbDatasourceModule"/></returns>
        private IEdbDatasourceModule ProcessType(Type t)
        {
            try
            {
                return (IEdbDatasourceModule)Activator.CreateInstance(t);
            }
            catch (Exception ex)
            {
                _logger.Error($"Err: \n {ex}");
                return null;
            }
        }
    }
}