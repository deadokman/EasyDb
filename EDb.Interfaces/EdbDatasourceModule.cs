namespace EDb.Interfaces
{
    using EDb.Interfaces.Objects;
    using EDb.Interfaces.Options;
    using System;
    using System.Data;
    using System.Linq;

    /// <summary>
    /// Defines the <see cref="EdbDatasourceModule" />
    /// </summary>
    public abstract class EdbDatasourceModule : MarshalByRefObject
    {
        /// <summary>
        /// Gets the DatabaseName
        /// Имя модуля СУБД
        /// </summary>
        public abstract string DatabaseName { get; }

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
            return this.GetOptions().Select(so => so.ToOptionDefinition()).ToArray();
        }


        /// <summary>
        /// The SetGuid
        /// </summary>
        /// <param name="guid">The guid<see cref="Guid"/></param>
        public virtual void SetGuid(Guid guid)
        {
            ModuleGuid = guid;
        }
    }
}
