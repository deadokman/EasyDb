using System.Security.Permissions;

namespace EasyDb.SandboxEnvironment
{
    using System;
    using System.Data;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Media;

    using EDb.Interfaces;
    using EDb.Interfaces.Objects;

    /// <summary>
    /// Proxy EdbModule calls in safe context
    /// </summary>
    public class EdbModuleProxy : EdbDatasourceModule
    {
        /// <summary>
        /// Poxy entity subject
        /// </summary>
        private EdbDatasourceModule _proxySubject;

        /// <summary>
        /// Initializes a new instance of the <see cref="EdbModuleProxy"/> class.
        /// </summary>
        public EdbModuleProxy()
        {
            this.SupportDbModuleInstance = false;
        }

        /// <summary>
        /// Gets database image icon
        /// </summary>
        public override byte[] DatabaseIcon => this._proxySubject.DatabaseIcon;

        /// <summary>
        /// Gets database name
        /// </summary>
        public override string DatabaseName => this._proxySubject.DatabaseName;

        /// <summary>
        /// Gets support of db module interface
        /// </summary>
        public bool SupportDbModuleInstance { get; private set; }

        /// <summary>
        /// Gets supported object types
        /// </summary>
        public override SupportedObjectTypes[] SupportedTypes => this._proxySubject.SupportedTypes;

        /// <summary>
        /// Gets module version
        /// </summary>
        public override Version Version => this._proxySubject.Version;

        /// <summary>
        /// Identifier of chocolate ODBC package
        /// </summary>
        public override string ChocolateOdbcPackageId => this._proxySubject.ChocolateOdbcPackageId;

        /// <summary>
        /// URL to ODBC driver package
        /// </summary>
        public override string ChocolatepackageUrl => this._proxySubject.ChocolatepackageUrl;

        /// <summary>
        /// Driver download URLS
        /// </summary>
        public override string[] AlternativeDriverDownloadUrls => this._proxySubject.AlternativeDriverDownloadUrls;

        /// <summary>
        /// Gets module source options collection
        /// </summary>
        /// <returns>Source options collection</returns>
        public override EdbSourceOption[] GetOptions()
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
                t => t.IsClass && !t.IsAbstract && t.BaseType == typeof(EdbDatasourceModule)).ToArray();

            if (types.Length > 1)
            {
                throw new Exception($"Module assembly: {assembly.FullName} contains more than one interface EdbDatasourceModule");
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
            return true;
        }

        /// <summary>
        /// Set guid
        /// </summary>
        /// <param name="guid">guid</param>
        public override void SetGuid(Guid guid)
        {
            this._proxySubject.SetGuid(guid);
        }

        /// <summary>
        /// Set version
        /// </summary>
        /// <param name="version">version</param>
        public override void SetVersion(Version version)
        {
            this._proxySubject.SetVersion(version);
        }

        /// <summary>
        /// Process plugin type
        /// </summary>
        /// <param name="t">The t<see cref="Type"/></param>
        /// <returns>The <see cref="EdbDatasourceModule"/></returns>
        private EdbDatasourceModule ProcessType(Type t)
        {
            try
            {
                return (EdbDatasourceModule)Activator.CreateInstance(t);
            }
            catch (Exception ex)
            {
                throw new Exception($"Err when creation instance on {t.FullName}: \n {ex}");
            }
        }
    }
}