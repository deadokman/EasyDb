using System.Linq;

namespace EasyDb.SandboxEnvironment
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Security;
    using System.Security.Permissions;
    using System.Security.Policy;
    using System.Windows;
    using System.Xml.Serialization;

    using EasyDb.Annotations;
    using EasyDb.Interfaces.Data;
    using EasyDb.ViewModel.DataSource;
    using EasyDb.ViewModel.DataSource.Items;

    using EDb.Interfaces;

    using NLog;

    using ILogger = Autofac.Extras.NLog.ILogger;

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
        /// Logger instance
        /// </summary>
        private readonly ILogger _logger;

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
        /// <param name="logger">Class logger</param>
        public DatasourceManager([NotNull] ILogger logger)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
        public UserDataSource CreateNewUserdatasource(EdbDatasourceModule module)
        {
            var uds = new UserDataSource
            {
                LinkedEdbSourceModule = module,
                SettingsObjects = module.GetOptions()
                    .Select(opt => new EdbSourceOptionProxy(opt)).ToArray()
            };

            uds.SetGuid(module.ModuleGuid);
            return uds;
        }

        /// <summary>
        /// Добавить объявленный пользователем источник данных в список
        /// </summary>
        /// <param name="uds">Источник данных прользователя</param>
        public void ApplyUserDatasource(UserDataSource uds)
        {
            this.UserdefinedDatasources.Add(uds);
        }

        /// <summary>
        /// Load datasource modules
        /// Modules load in separate AppDomain for security purpose
        /// </summary>
        /// <param name="dbModulesAssembliesPath">The dbModulesAssembliesPath<see cref="string"/></param>
        public void InitialLoad(string dbModulesAssembliesPath)
        {
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

            if (!Directory.Exists(dbModulesAssembliesPath))
            {
                throw new Exception($"Path NotFound {dbModulesAssembliesPath}");
            }

            var moduleAssemblies = Directory.GetFiles(dbModulesAssembliesPath, "*.dll");

            // Initialize secure app domain
            var untrustedDomain = this.InitAppdomainInstance(dbModulesAssembliesPath, moduleAssemblies);

            // Get all assemblies from path
            foreach (var assmFile in moduleAssemblies)
            {
                try
                {
                    var proxyInstanceHandle =
                        Activator.CreateInstanceFrom(
                        untrustedDomain,
                        typeof(EdbModuleProxy).Assembly.ManifestModule.FullyQualifiedName,
                        typeof(EdbModuleProxy).FullName);
                    var edbModuleProxy = (EdbModuleProxy)proxyInstanceHandle.Unwrap();
                    if (!edbModuleProxy.InitializeProxyIntance(assmFile))
                    {
                        _logger.Log(LogLevel.Warn, new Exception($"Assembly: {assmFile} skipped, because it does not implement EasyDb module interface"));
                        continue;
                    }

                    var supportedSourceItem = new SupportedSourceItem(edbModuleProxy);
                    this._supportedDataSources.Add(edbModuleProxy.ModuleGuid, supportedSourceItem);
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
        /// Initialize "SandBox" application domain
        /// </summary>
        /// <param name="untrustedPath"> Path to untrusted libs </param>
        /// <param name="assemblies">module assemblies</param>
        /// <returns>Add domain instance</returns>
        private AppDomain InitAppdomainInstance(string untrustedPath, string[] assemblies)
        {
            var adSetup = new AppDomainSetup();
            adSetup.ApplicationBase = Path.GetFullPath(untrustedPath);
            var permSet = new PermissionSet(PermissionState.None);
            permSet.AddPermission(new SecurityPermission(SecurityPermissionFlag.AllFlags));
            permSet.AddPermission(new MediaPermission(MediaPermissionImage.SafeImage));
            permSet.AddPermission(new ReflectionPermission(ReflectionPermissionFlag.MemberAccess));

            // permSet.AddPermission()
            var fileIOpermissions = new FileIOPermission(FileIOPermissionAccess.Read, assemblies);
            fileIOpermissions.AddPathList(FileIOPermissionAccess.PathDiscovery, assemblies);
            permSet.AddPermission(fileIOpermissions);
            AssemblyName name = typeof(EdbModuleProxy).Assembly.GetName();
            var strongName = new StrongName(
                new StrongNamePublicKeyBlob(name.GetPublicKey()),
                name.Name,
                name.Version);

            var domain = AppDomain.CreateDomain("Sandbox", null, adSetup, permSet, strongName);
            return domain;
        }
    }
}