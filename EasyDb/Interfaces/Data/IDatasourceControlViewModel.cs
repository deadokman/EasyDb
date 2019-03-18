﻿using Edb.Environment.Model;

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
        /// Gets or sets the ConfigureDsCmd
        /// Команда конфигурирования источника данных
        /// </summary>
        ICommand ConfigureDsCmd { get; set; }

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
        ICommand CloseInformationMessage { get; set; }

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
        bool ProcessInProgress { get; set; }

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