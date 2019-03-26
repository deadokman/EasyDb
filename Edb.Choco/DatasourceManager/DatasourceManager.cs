using Edb.Environment.CommunicationArgs;
using EDb.Interfaces.iface;
using GalaSoft.MvvmLight.Messaging;

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

    using EasyDb.Model;

    using Edb.Environment.DatasourceManager;
    using Edb.Environment.Interface;
    using Edb.Environment.SandboxEnvironment;

    using EDb.Interfaces;
    using EDb.Interfaces.Annotations;

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
        private const string DataSourceStorageFile = "Edb.Datasource.Config.xml";

        /// <summary>
        /// Logger instance
        /// </summary>
        private readonly ILogger _logger;

        private readonly IMessenger _messenger;

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
        /// <param name="messenger">Messenger</param>
        public DatasourceManager([NotNull] ILogger logger, [NotNull] IMessenger messenger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _messenger = messenger ?? throw new ArgumentNullException(nameof(messenger));
            _supportedDataSources = new Dictionary<Guid, SupportedSourceItem>();
            UserDatasourceConfigurations = new List<UserDatasourceConfiguration>();
        }

        /// <summary>
        /// Defines the DatasourceLoaded
        /// </summary>
        public event DatasourceData DatasourceLoaded;

        /// <summary>
        /// Gets the SupportedDatasources
        /// </summary>
        public IEnumerable<SupportedSourceItem> SupportedDatasources => _supportedDataSources.Values;

        /// <summary>
        /// Gets or sets the UserDatasources
        /// Collection of user defined datasources
        /// </summary>
        public List<UserDatasourceConfiguration> UserDatasourceConfigurations { get; set; }

        /// <summary>
        /// Gets the path to datasource configuration storage file
        /// </summary>
        private string DatasourceConfigStorage => Path.Combine(Directory.GetCurrentDirectory(), DataSourceStorageFile);

        /// <summary>
        /// Creates new user defined datasource
        /// </summary>
        /// <param name="module">datasource module</param>
        /// <returns>User defined datasource</returns>
        public UserDatasourceConfiguration CreateDataSourceConfig(IEdbDataSource module)
        {
            var udsConfig = new UserDatasourceConfiguration();
            udsConfig.DatasoureGuid = module.ModuleGuid;
            udsConfig.ConfigurationGuid = Guid.NewGuid();
            udsConfig.OptionsObjects = module.GetOptions();
            return udsConfig;
        }

        /// <summary>
        /// Добавить объявленный пользователем источник данных в список
        /// </summary>
        /// <param name="udsc">Источник данных прользователя</param>
        public void ApplyUserDatasource(UserDatasourceConfiguration udsc)
        {
            UserDatasourceConfigurations.Add(udsc);
        }

        /// <summary>
        /// Load datasource modules
        /// Modules load in separate AppDomain for security purpose
        /// </summary>
        /// <param name="dbModulesAssembliesPath">The dbModulesAssembliesPath<see cref="string"/></param>
        public void InitialLoad(string dbModulesAssembliesPath)
        {
            List<UserDatasourceConfiguration> serializedSources;

            if (!Directory.Exists(dbModulesAssembliesPath))
            {
                throw new Exception($"Path NotFound {dbModulesAssembliesPath}");
            }

            var moduleAssemblies = Directory.GetFiles(dbModulesAssembliesPath, "*.dll");

            // Initialize secure app domain
            var untrustedDomain = InitAppdomainInstance(dbModulesAssembliesPath, moduleAssemblies);

            // Get all assemblies from path
            foreach (var assmFile in moduleAssemblies)
            {
                try
                {
                    var proxyInstanceHandle =
                        Activator.CreateInstanceFrom(
                        untrustedDomain,
                        typeof(EdbDataProxy).Assembly.ManifestModule.FullyQualifiedName,
                        typeof(EdbDataProxy).FullName);
                    var edbModuleProxy = (EdbDataProxy)proxyInstanceHandle.Unwrap();
                    if (!edbModuleProxy.InitializeProxyIntance(assmFile))
                    {
                        _logger.Warn(new Exception($"Assembly: {assmFile} skipped, because it does not implement EasyDb module interface"));
                        continue;
                    }

                    var supportedSourceItem = new SupportedSourceItem(edbModuleProxy);
                    _supportedDataSources.Add(edbModuleProxy.ModuleGuid, supportedSourceItem);
                }
                catch (Exception ex)
                {
                    _logger.Error($"err: {ex}");
                }
            }

            _xseri = new XmlSerializer(
                typeof(List<UserDatasourceConfiguration>),
                _supportedDataSources.SelectMany(sds => sds.Value.Module.GetOptions().Select(opt => opt.GetType())).ToArray());

            // Load and serialize XML doc. Create new one if it does not exists;
            if (File.Exists(DatasourceConfigStorage))
            {
                try
                {
                    using (var fs = File.OpenRead(DatasourceConfigStorage))
                    {
                        serializedSources = (List<UserDatasourceConfiguration>)_xseri.Deserialize(fs);
                    }

                }
                catch (Exception ex)
                {
                    _logger.Error($"Error while parsing user defined sources: \n {ex}");
                    serializedSources = new List<UserDatasourceConfiguration>();
                }
            }
            else
            {
                serializedSources = new List<UserDatasourceConfiguration>();
            }

            // Restore user defined datasource
            foreach (var uds in serializedSources)
            {
                // Check that user defined data source exists in datasource module
                if (_supportedDataSources.ContainsKey(uds.DatasoureGuid))
                {
                    UserDatasourceConfigurations.Add(uds);
                }
                else
                {
                    _logger.Error($"Cannot find module GUID: [{uds.DatasoureGuid}] while loading user datasource config [{uds.ConfigurationGuid}]", uds.Name);
                }
            }

            if (DatasourceLoaded != null)
            {
                DatasourceLoaded.Invoke(_supportedDataSources.Values, UserDatasourceConfigurations);
            }

            _messenger.Send(new DatasourcesIniaialized(_supportedDataSources.Values, UserDatasourceConfigurations));
        }

        /// <summary>
        /// Returns module instance for guid
        /// </summary>
        /// <param name="guid">Module identifier</param>
        /// <returns>Module instance</returns>
        public IEdbDataSource GetModuleByGuid(Guid guid)
        {
            return _supportedDataSources[guid].Module;
        }

        /// <summary>
        /// Save datasource configuration to config file at hard drive
        /// </summary>
        public void StoreUserDatasourceConfigurations()
        {
            var tmpFile = string.Concat(DataSourceStorageFile, "$tmp");
            if (!File.Exists(DataSourceStorageFile))
            {
                using (var resourceStream = Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream("Edb.Environment.DatasourceStorageTemplate.xml"))
                {
                    // Create new file from template
                    using (var newfileStream = File.Create(DataSourceStorageFile))
                    {
                        resourceStream.CopyTo(newfileStream);
                    }
                }
            }

            // Create temporary copy of storage file
            try
            {
                File.Copy(DataSourceStorageFile, tmpFile, true);
            }
            catch (Exception ex)
            {
                var msg = "Exception while saving datasource configuration";
                _logger.Error(msg, ex);
                throw new Exception(msg, ex);
            }

            try
            {
                using (var dsfs = File.Open(DataSourceStorageFile, FileMode.Truncate))
                {
                    _xseri.Serialize(dsfs, UserDatasourceConfigurations);
                }
            }
            catch (Exception ex)
            {
                var msg = "Error while writing new datasource configurations, trying to revert";
                _logger.Error(msg, ex);
                File.Copy(tmpFile, DataSourceStorageFile, true);
            }

            _messenger.Send(new DatasourcesIniaialized(SupportedDatasources, UserDatasourceConfigurations));
            File.Delete(tmpFile);
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
            AssemblyName name = typeof(EdbDataProxy).Assembly.GetName();
            var strongName = new StrongName(
                new StrongNamePublicKeyBlob(name.GetPublicKey()),
                name.Name,
                name.Version);

            var domain = AppDomain.CreateDomain("Sandbox", null, adSetup, permSet, strongName);
            return domain;
        }
    }
}