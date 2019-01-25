using System.Collections.ObjectModel;
using EasyDb.ViewModel.DataSource.Items;
using EDb.Interfaces;

namespace EasyDb.ViewModel.Interfaces
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
        /// Invoke datasource properties window
        /// </summary>

        /// <summary>
        /// Datasources that has been declared by user
        /// </summary>
        ObservableCollection<UserDataSource> UserDatasources { get; }

    }
}
