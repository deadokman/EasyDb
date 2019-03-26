using System;
using EDb.Interfaces.Model;
using EDb.Interfaces.Objects;
using EDb.Interfaces.Options;

namespace EDb.Interfaces.iface
{
    /// <summary>
    /// Base module interface
    /// </summary>
    public interface IEdbDataSource
    {
        /// <summary>
        /// Gets the DatasourceName
        /// Имя модуля СУБД
        /// </summary>
        string DatasourceName { get; }

        /// <summary>
        /// Gets the DatabaseIcon
        /// Значек базы данных
        /// </summary>
        byte[] DatabaseIcon { get; }

        /// <summary>
        /// Option objects
        /// Вернуть объекты настроек
        /// </summary>
        /// <returns>Plugin options collection</returns>
        EdbSourceOption[] GetOptions();

        /// <summary>
        /// Gets the SupportedTypes
        /// Типы поддерживаемых объектов базы данных
        /// </summary>
        SupportedObjectTypes[] SupportedTypes { get; }

        /// <summary>
        /// Retuns the list of supported datatypes for datasource
        /// </summary>
        DataType[] DataTypes { get; }

        /// <summary>
        /// Gets the ModuleGuid
        /// Module unique GUID
        /// </summary>
        Guid ModuleGuid { get; }

        /// <summary>
        /// Gets the Version
        /// Module version
        /// </summary>
        Version Version { get;  }

        /// <summary>
        /// Identifier of chocolate ODBC package
        /// </summary>
        string ChocolateOdbcPackageId { get; }

        /// <summary>
        /// URL to ODBC driver package
        /// </summary>
        string ChocolatepackageUrl { get; }

        /// <summary>
        /// Driver download URLS
        /// </summary>
        string[] AlternativeDriverDownloadUrls { get; }

        /// <summary>
        /// ODBC driver name inside operating system
        /// </summary>
        string OdbcSystemDriverName { get; }

        /// <summary>
        /// Driver name for x32 architecture systems
        /// </summary>
        string OdbcSystem32DriverName { get; }

        /// <summary>
        /// The SetVersion
        /// </summary>
        /// <param name="version">The version<see cref="Version"/></param>
        void SetVersion(Version version);

        /// <summary>
        /// Get option defenition objects
        /// </summary>
        /// <returns>Returns module options definition</returns>
        ModuleOptionDefinition[] GetOptionsDefenitions();

        /// <summary>
        /// Creates connection string for datasoure
        /// </summary>
        /// <param name="options">Datasource options</param>
        /// <param name="passwordTag">Password tag string replacement</param>
        /// <returns>Returns connection string</returns>
        string IntroduceConnectionString(EdbSourceOption[] options, out string passwordTag);

        /// <summary>
        /// The SetGuid
        /// </summary>
        /// <param name="guid">The guid<see cref="Guid"/></param>
        void SetGuid(Guid guid);

        /// <summary>
        /// Get database query producer
        /// </summary>
        /// <returns></returns>
        IEdbDataSourceQueryProducer GetQueryProducer();
    }
}
