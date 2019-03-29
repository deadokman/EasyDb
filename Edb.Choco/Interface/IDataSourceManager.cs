using EDb.Interfaces.iface;

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
        /// Gets the SupportedDatasources
        /// Поддерживаемые источники данных
        /// </summary>
        IEnumerable<SupportedSourceItem> SupportedDatasources { get; }

        /// <summary>
        /// Создать новый экземпляр источника данных
        /// </summary>
        /// <param name="module">Database driver</param>
        /// <returns>User defined data source</returns>
        UserDatasourceConfiguration CreateDataSourceConfig(IEdbDataSource module);

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
        IEdbDataSource GetModuleByGuid(Guid guid);

        /// <summary>
        /// Get option class types for XmkSerializer
        /// </summary>
        /// <returns>OptionTypes </returns>
        IEnumerable<Type> GetAdditionalOptionTypes();
    }
}