using Edb.Environment.Model;

namespace EasyDb.Interfaces.Data
{
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    using EasyDb.ViewModel.DataSource;
    using EasyDb.ViewModel.DataSource.Items;

    using NuGet;

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

        /// <summary>
        /// Changes editing context
        /// </summary>
        SupportedSourceItem SelectedSourceItem { get; set; }

        /// <summary>
        /// Editing user data source
        /// </summary>
        UserDataSource EditingUserDatasource { get; set; }

        /// <summary>
        /// Chocolatey package
        /// </summary>
        IPackage Package { get; set; }

        /// <summary>
        /// Downloading information about package
        /// </summary>
        bool PackageInfoLoad { get; set; }

        /// <summary>
        /// Assosiated ODBC driver
        /// </summary>
        OdbcDriver OdbcDriver { get; set; }

        /// <summary>
        /// Message for driver display page
        /// </summary>
        string DriverMessage { get; set; }

        /// <summary>
        /// Supported driver autoinstall from chocolatey
        /// </summary>
        bool AutoinstallSupportred { get; }
    }
}