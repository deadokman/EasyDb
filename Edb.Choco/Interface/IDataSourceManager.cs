namespace Edb.Environment.Interface
{
    using System;
    using System.Collections.Generic;

    using EasyDb.Model;

    using Edb.Environment.DatasourceManager;

    using EDb.Interfaces;

    /// <summary>
    /// The DatasourceData
    /// </summary>
    /// <param name="datasources">The datasources<see cref="IEnumerable{T}"/></param>
    /// <param name="userSources">The userSources<see cref="IEnumerable{UserDataSourceViewModelItem}"/></param>
    public delegate void DatasourceData(
        IEnumerable<SupportedSourceItem> datasources,
        IEnumerable<UserDatasourceConfiguration> userSources);

    /// <summary>
    /// Менедер источников данных для приложения (драйверов СУБД)
    /// </summary>
    public interface IDataSourceManager
    {
        /// <summary>
        /// Источники данных загружены
        /// </summary>
        event DatasourceData DatasourceLoaded;

        /// <summary>
        /// Gets the SupportedDatasources
        /// Поддерживаемые источники данных
        /// </summary>
        IEnumerable<SupportedSourceItem> SupportedDatasources { get; }

        /// <summary>
        /// Gets or sets the UserDatasources
        /// Источники данных объявленные пользователем
        /// </summary>
        List<UserDatasourceConfiguration> UserDatasourceConfigurations { get; set; }

        /// <summary>
        /// Создать новый экземпляр источника данных
        /// </summary>
        /// <param name="module">Database driver</param>
        /// <returns>User defined data source</returns>
        UserDatasourceConfiguration CreateDataSourceConfig(IEdbSourceModule module);

        /// <summary>
        /// Добавить объявленный пользователем источник данных в список
        /// </summary>
        /// <param name="uds">Источник данных прользователя</param>
        void ApplyUserDatasource(UserDatasourceConfiguration uds);

        /// <summary>
        /// Инициализировать менеджер данных
        /// </summary>
        /// <param name="dbModulesAssembliesPath">Путь к сборкам драйверов</param>
        void InitialLoad(string dbModulesAssembliesPath);

        /// <summary>
        /// Returns module instance for guid
        /// </summary>
        /// <param name="guid">Module identifier</param>
        /// <returns>Module instance</returns>
        IEdbSourceModule GetModuleByGuid(Guid guid);

        /// <summary>
        /// Save datasource configuration to config file at hard drive
        /// </summary>
        void StoreUserDatasourceConfigurations();
    }
}