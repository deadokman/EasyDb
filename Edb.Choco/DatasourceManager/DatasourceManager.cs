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
        /// Load datasource modules
        /// Modules load in separate AppDomain for security purpose
        /// </summary>
        /// <param name="dbModulesAssembliesPath">The dbModulesAssembliesPath<see cref="string"/></param>
        public void InitialLoad(string dbModulesAssembliesPath)
        {
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

            _messenger.Send(new DatasourcesIniaialized(_supportedDataSources.Values));
        }

        /// <summary>
        /// 
        /// </summary>
        public void ValidateUserdatasourceConfigurations(IEnumerable<UserDatasourceConfiguration> configurations, Action<UserDatasourceConfiguration, string> brokeInvoke)
        {
            // Restore user defined datasource
            foreach (var uds in configurations)
            {
                // Check that user defined data source exists in datasource module
                if (_supportedDataSources.ContainsKey(uds.DatasoureGuid))
                {
                    UserDatasourceConfigurations.Add(uds);
                }
                else
                {
                    var msg = $"Cannot find module GUID: [{uds.DatasoureGuid}] while loading user datasource config [{uds.ConfigurationGuid}]";
                    brokeInvoke?.Invoke(uds, msg);
                    _logger.Error(msg, uds.Name);
                }
            }
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
        /// Get option class types for XmkSerializer
        /// </summary>
        /// <returns>OptionTypes </returns>
        public IEnumerable<Type> GetAdditionalOptionTypes()
        {
           return SupportedDatasources.SelectMany(st => st.Module.GetOptions().Select(v => v.GetType()));
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