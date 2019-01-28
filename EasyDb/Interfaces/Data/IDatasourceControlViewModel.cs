using System.Collections.ObjectModel;
using System.Windows.Input;
using EasyDb.ViewModel.DataSource.Items;
using EDb.Interfaces;

namespace EasyDb.Interfaces.Data
{
    /// <summary>
    /// Interface for Data source control view
    /// </summary>
    public interface IDatasourceControlViewModel
    {
        /// <summary>
        /// Get collection of supported datasources
        /// </summary>
        IEdbDatasourceModule[] SupportedDatasources { get; }

        /// <summary>
        /// Datasources that has been declared by user
        /// </summary>
        ObservableCollection<UserDataSource> UserDatasources { get; }

        /// <summary>
        /// Команда конфигурирования источника данных
        /// </summary>
        ICommand ConfigureUserdataSourceCmd { get; set; }
    }
}
