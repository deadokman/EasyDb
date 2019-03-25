using System.Security;
using System.Windows.Input;
using EasyDb.ViewModel.DataSource;
using Edb.Environment.DatasourceManager;
using Edb.Environment.Model;
using NuGet;

namespace EasyDb.ViewModel.Interfaces
{
    /// <summary>
    /// Interface for Data source control view
    /// </summary>
    public interface IDataSourceSettingsViewModel
    {
        /// <summary>
        /// Refresh pakcage and driver information command
        /// </summary>
        ICommand RefreshPackageInformationCmd { get; set; }

        /// <summary>
        /// Install package with chocolatey in semi-auto mode
        /// </summary>
        ICommand InstallPackageAutoCmd { get; set; }

        /// <summary>
        /// Закрыть информационное сообщение
        /// </summary>
        ICommand CloseInformationMessageCmd { get; set; }

        /// <summary>
        /// Save datasource settings and finish user data source
        /// </summary>
        ICommand ApplyDatasourceSettingsCmd { get; set; }

        /// <summary>
        /// Закрыть окно настроек источника данных
        /// </summary>
        ICommand CloseSettingsWindowCmd { get; set; }

        /// <summary>
        /// Test database connection
        /// </summary>
        ICommand TestConnection { get; set; }

        /// <summary>
        /// Supported datasource collection
        /// </summary>
        SupportedSourceItem[] SupportedDatasources { get; set; }

        /// <summary>
        /// True if Database connection valid
        /// </summary>
        bool DatabaseConnectionValid { get; set; }

        /// <summary>
        /// Processing database connection
        /// </summary>
        bool DatabaseConnectionInProgress { get; set; }

        /// <summary>
        /// Password secure string
        /// </summary>
        SecureString PasswordSecureString { get; set; }

        /// <summary>
        /// Changes editing context
        /// </summary>
        SupportedSourceItem SelectedSourceItem { get; set; }

        /// <summary>
        /// Editing user data source
        /// </summary>
        UserDataSourceViewModelItem EditingUserDatasource { get; set; }

        /// <summary>
        /// Chocolatey package
        /// </summary>
        IPackage Package { get; set; }

        /// <summary>
        /// Downloading information about package
        /// </summary>
        bool ProcessInProgress { get; set; }

        /// <summary>
        /// Assosiated ODBC driver
        /// </summary>
        OdbcDriver OdbcDriver { get; set; }

        /// <summary>
        /// Message for driver display page
        /// </summary>
        string WarningMessage { get; set; }

        /// <summary>
        /// Supported driver autoinstall from chocolatey
        /// </summary>
        bool AutoinstallSupportred { get; }

        /// <summary>
        /// Datasource got problems with driver
        /// </summary>
        bool GotDriverProblems { get; }

        /// <summary>
        /// Store password securely in password storage
        /// </summary>
        bool StorePasswordSecure { get; set; }
    }
}