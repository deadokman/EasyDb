using EDb.Interfaces.Objects;
using System;

namespace EDb.Interfaces
{
    using EDb.Interfaces.Options;

    /// <summary>
    /// Base module interface
    /// </summary>
    public interface IEdbSourceModule
    {
        /// <summary>
        /// Gets the DatabaseName
        /// Имя модуля СУБД
        /// </summary>
        string DatabaseName { get; }

        /// <summary>
        /// Gets the SupportedTypes
        /// Типы поддерживаемых объектов базы данных
        /// </summary>
        SupportedObjectTypes[] SupportedTypes { get; }

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
        /// Query producer
        /// </summary>
        IEdbModuleQueryProducer QueryModuleProducer { get; }

        /// <summary>
        /// The SetGuid
        /// </summary>
        /// <param name="guid">The guid<see cref="Guid"/></param>
        void SetGuid(Guid guid);
    }
}
