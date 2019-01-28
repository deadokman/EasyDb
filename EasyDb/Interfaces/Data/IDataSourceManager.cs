namespace EasyDb.Interfaces.Data
{
    using System.Collections.Generic;

    using EasyDb.ViewModel.DataSource;
    using EasyDb.ViewModel.DataSource.Items;

    using EDb.Interfaces;

    /// <summary>
    /// The DatasourceData
    /// </summary>
    /// <param name="datasources">The datasources<see cref="IEnumerable{T}"/></param>
    /// <param name="userSources">The userSources<see cref="IEnumerable{UserDataSource}"/></param>
    public delegate void DatasourceData(
        IEnumerable<SupportedSourceItem> datasources,
        IEnumerable<UserDataSource> userSources);

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
        /// Gets or sets the UserdefinedDatasources
        /// Источники данных объявленные пользователем
        /// </summary>
        List<UserDataSource> UserdefinedDatasources { get; set; }

        /// <summary>
        /// Создать новый экземпляр источника данных
        /// </summary>
        /// <param name="module">Database driver</param>
        /// <returns>User defined data source</returns>
        UserDataSource CreateNewUserdatasource(IEdbDatasourceModule module);

        /// <summary>
        /// Инициализировать менеджер данных
        /// </summary>
        /// <param name="datasourceAssembliesPath">Путь к сборкам драйверов</param>
        void InitialLoad(string datasourceAssembliesPath);
    }
}