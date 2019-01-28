using System.Collections.Generic;
using EasyDb.ViewModel.DataSource.Items;
using EDb.Interfaces;

namespace EasyDb.Interfaces.Data
{
    public delegate void DatasourceData(IEnumerable<IEdbDatasourceModule> datasources, IEnumerable<UserDataSource> userSources);


    /// <summary>
    /// Менедер источников данных для приложения (драйверов СУБД)
    /// </summary>
    public interface IDataSourceManager
    {
        /// <summary>
        /// Инициализировать менеджер данных
        /// </summary>
        /// <param name="datasourceAssembliesPath">Путь к сборкам драйверов</param>
        void InitialLoad(string datasourceAssembliesPath);

        /// <summary>
        /// Источники данных загружены
        /// </summary>
        event DatasourceData DatasourceLoaded;

        /// <summary>
        /// Создать новый экземпляр источника данных
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        UserDataSource CreateNewUserdatasource(IEdbDatasourceModule module);

        /// <summary>
        /// Поддерживаемые источники данных
        /// </summary>
        IEnumerable<IEdbDatasourceModule> SupportedDatasources { get; }

        /// <summary>
        /// Источники данных объявленные пользователем
        /// </summary>
        List<UserDataSource> UserdefinedDatasources { get; set; }
    }
}
