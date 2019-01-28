namespace EasyDb.Interfaces.Data
{
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    using EasyDb.ViewModel.DataSource;
    using EasyDb.ViewModel.DataSource.Items;

    /// <summary>
    /// Interface for Data source control view
    /// </summary>
    public interface IDatasourceControlViewModel
    {
        /// <summary>
        /// Gets or sets the ConfigureDs
        /// Команда конфигурирования источника данных
        /// </summary>
        ICommand ConfigureDs { get; set; }

        /// <summary>
        /// Gets the SupportedDatasources
        /// Get collection of supported datasources
        /// </summary>
        SupportedSourceItem[] SupportedDatasources { get; }

        /// <summary>
        /// Gets the UserDatasources
        /// Datasources that has been declared by user
        /// </summary>
        ObservableCollection<UserDataSource> UserDatasources { get; }
    }
}