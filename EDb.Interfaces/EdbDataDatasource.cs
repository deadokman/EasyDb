using System.Security;
using System.Security.Permissions;

[assembly: AllowPartiallyTrustedCallers]
namespace EDb.Interfaces
{
    using EDb.Interfaces.Objects;
    using Options;
    using System;
    using System.Data;
    using System.Linq;

    /// <summary>
    /// Defines the <see cref="EdbDataDatasource" />
    /// </summary>
    public abstract class EdbDataDatasource : MarshalByRefObject, IEdbDataSource
    {
        /// <summary>
        /// Gets the DatasourceName
        /// Имя модуля СУБД
        /// </summary>
        public abstract string DatasourceName { get; }

        /// <summary>
        /// Gets the SupportedTypes
        /// Типы поддерживаемых объектов базы данных
        /// </summary>
        public abstract SupportedObjectTypes[] SupportedTypes { get; }

        /// <summary>
        /// Gets the DatabaseIcon
        /// Значек базы данных
        /// </summary>
        public abstract byte[] DatabaseIcon { get; }

        /// <summary>
        /// Option objects
        /// Вернуть объекты настроек
        /// </summary>
        /// <returns>Plugin options collection</returns>
        public abstract EdbSourceOption[] GetOptions();

        /// <summary>
        /// Gets the ModuleGuid
        /// Module unique GUID
        /// </summary>
        public virtual Guid ModuleGuid { get; private set; }

        /// <summary>
        /// Gets the Version
        /// Module version
        /// </summary>
        public virtual Version Version { get; private set; }

        /// <summary>
        /// Identifier of chocolate ODBC package
        /// </summary>
        public abstract string ChocolateOdbcPackageId { get; }

        /// <summary>
        /// URL to ODBC driver package
        /// </summary>
        public abstract string ChocolatepackageUrl { get; }

        /// <summary>
        /// Driver download URLS
        /// </summary>
        public virtual string[] AlternativeDriverDownloadUrls { get; }

        /// <summary>
        /// ODBC driver name inside operating system
        /// </summary>
        public abstract string OdbcSystemDriverName { get; }

        /// <summary>
        /// Driver name for x32 architecture systems
        /// </summary>
        public abstract string OdbcSystem32DriverName { get; }

        /// <summary>
        /// The SetVersion
        /// </summary>
        /// <param name="version">The version<see cref="Version"/></param>
        public virtual void SetVersion(Version version)
        {
            Version = version;
        }

        /// <summary>
        /// Get option defenition objects
        /// </summary>
        /// <returns>Returns module options definition</returns>
        public virtual ModuleOptionDefinition[] GetOptionsDefenitions()
        {
            return GetOptions().Select(so => so.ToOptionDefinition()).ToArray();
        }

        [SecurityCritical]
        public override object InitializeLifetimeService()
        {
            return null;
        }


        /// <summary>
        /// Creates connection string for datasoure
        /// </summary>
        /// <param name="options">Datasource options</param>
        /// <param name="passwordTag">Password tag string replacement</param>
        /// <returns>Returns connection string</returns>
        public abstract string IntroduceConnectionString(EdbSourceOption[] options, out string passwordTag);

        /// <summary>
        /// The SetGuid
        /// </summary>
        /// <param name="guid">The guid<see cref="Guid"/></param>
        public virtual void SetGuid(Guid guid)
        {
            ModuleGuid = guid;
        }

        /// <summary>
        /// Database query producer
        /// </summary>
        /// <returns>Instance of database query producer</returns>
        public abstract IEdbDataSourceQueryProducer GetQueryProducer();
    }
}
