namespace Edb.Environment.SandboxEnvironment
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Security;
    using EDb.Interfaces;
    using EDb.Interfaces.Objects;
    using EDb.Interfaces.Options;

    /// <summary>
    /// Proxy EdbModule calls in safe context
    /// </summary>
    [SecuritySafeCritical]
    [ComVisible(true)]
    [Serializable]
    public class EdbDataProxy : IEdbDataSource
    {
        /// <summary>
        /// Poxy entity subject
        /// </summary>
        private EdbDataDatasource _proxySubject;

        /// <summary>
        /// Initializes a new instance of the <see cref="EdbDataProxy"/> class.
        /// </summary>
        public EdbDataProxy()
        {
            this.SupportDbModuleInstance = false;
        }

        /// <summary>
        /// Gets the DatasoureGuid
        /// Module unique GUID
        /// </summary>
        public Guid ModuleGuid => this._proxySubject.ModuleGuid;

        /// <summary>
        /// Gets database image icon
        /// </summary>
        public byte[] DatabaseIcon => this._proxySubject.DatabaseIcon;

        /// <summary>
        /// Gets database name
        /// </summary>
        public string DatabaseName => this._proxySubject.DatabaseName;

        /// <summary>
        /// Gets support of db module interface
        /// </summary>
        public bool SupportDbModuleInstance { get; private set; }

        /// <summary>
        /// Gets supported object types
        /// </summary>
        public SupportedObjectTypes[] SupportedTypes => this._proxySubject.SupportedTypes;

        /// <summary>
        /// Gets module version
        /// </summary>
        public Version Version => this._proxySubject.Version;

        /// <summary>
        /// Identifier of chocolate ODBC package
        /// </summary>
        public string ChocolateOdbcPackageId => this._proxySubject.ChocolateOdbcPackageId;

        /// <summary>
        /// URL to ODBC driver package
        /// </summary>
        public string ChocolatepackageUrl => this._proxySubject.ChocolatepackageUrl;

        /// <summary>
        /// Driver download URLS
        /// </summary>
        public string[] AlternativeDriverDownloadUrls => this._proxySubject.AlternativeDriverDownloadUrls;

        /// <summary>
        /// ODBC driver name inside operating system
        /// </summary>
        public string OdbcSystemDriverName => this._proxySubject.OdbcSystemDriverName;

        /// <summary>
        /// Driver name for x32 architecture systems
        /// </summary>
        public string OdbcSystem32DriverName => this._proxySubject.OdbcSystem32DriverName;

        /// <summary>
        /// Query producer
        /// </summary>
        public IEdbDataSourceQueryProducer QueryProducer => this._proxySubject.QueryProducer;

        /// <summary>
        /// Gets module source options collection
        /// </summary>
        /// <returns>Source options collection</returns>
        public EdbSourceOption[] GetOptions()
        {
            return this._proxySubject.GetOptions();
        }

        /// <summary>
        /// Initialize proxy instance firn assembly file
        /// </summary>
        /// <param name="assemblyPath">Module assembly</param>
        /// <returns>True if assembly supports DbModuleSource</returns>
        public bool InitializeProxyIntance(string assemblyPath)
        {
            var assembly = Assembly.LoadFrom(assemblyPath);
            var types = assembly.GetTypes().Where(
                t => !t.IsAbstract && t.IsClass && t.GetInterfaces().Any(iface => iface == typeof(IEdbDataSource))).ToArray();

            if (types.Length > 1)
            {
                throw new Exception($"Module assembly: {assembly.FullName} contains more than one interface EdbDataDatasource");
            }

            if (types.Length == 0)
            {
                return false;
            }

            var moduleType = types.First();
            var attributes = moduleType.GetCustomAttributes(typeof(EdbDatasourceAttribute)).ToArray();
            if (attributes.Length == 0)
            {
                throw new Exception($"Edb module attribute not found in assembly: {assembly.FullName}");
            }

            var attribute = (EdbDatasourceAttribute)attributes[0];
            this._proxySubject = this.ProcessType(moduleType);
            this._proxySubject.SetGuid(attribute.SourceGuid);
            this._proxySubject.SetVersion(attribute.Version);

            /*
            // register a sponsor
            object lifetimeService = RemotingServices.GetLifetimeService(this._proxySubject);
            if (lifetimeService is ILease)
            {
                ILease lease = (ILease)lifetimeService;
                lease.Register(this);
            }
            */

            return true;
        }

        /// <summary>
        /// Get option defenition objects
        /// </summary>
        /// <returns>Returns module options definition</returns>
        public ModuleOptionDefinition[] GetOptionsDefenitions()
        {
            return this._proxySubject.GetOptionsDefenitions();
        }

        /// <summary>
        /// Creates connection string for datasoure
        /// </summary>
        /// <param name="options">Datasource options</param>
        /// <returns>Returns connection string</returns>
        public string IntroduceConnectionString(EdbSourceOption[] options)
        {
            return this._proxySubject.IntroduceConnectionString(options);
        }

        /// <summary>
        /// Set guid
        /// </summary>
        /// <param name="guid">guid</param>
        public void SetGuid(Guid guid)
        {
            this._proxySubject.SetGuid(guid);
        }

        /// <summary>
        /// Set version
        /// </summary>
        /// <param name="version">version</param>
        public void SetVersion(Version version)
        {
            this._proxySubject.SetVersion(version);
        }

        /// <summary>
        /// Process plugin type
        /// </summary>
        /// <param name="t">The t<see cref="Type"/></param>
        /// <returns>The <see cref="EdbDataDatasource"/></returns>
        private EdbDataDatasource ProcessType(Type t)
        {
            try
            {
                return (EdbDataDatasource)Activator.CreateInstance(t);
            }
            catch (Exception ex)
            {
                throw new Exception($"Err when creation instance on {t.FullName}: \n {ex}");
            }
        }
    }
}