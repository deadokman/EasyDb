﻿using EasyDb.ViewModel.DataSource.Items;
using Edb.Environment.Interface;
using Edb.Environment.Model;

namespace EasyDb.ViewModel.DataSource
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Windows;
    using System.Windows.Input;

    using EasyDb.Annotations;
    using EasyDb.Interfaces.Data;
    using EasyDb.View;

    using EDb.Interfaces;

    using GalaSoft.MvvmLight.CommandWpf;

    using NuGet;

    using ILogger = Autofac.Extras.NLog.ILogger;

    /// <summary>
    /// Implements logic for datasource creation control
    /// </summary>
    public class DatasourceViewModel : IDatasourceControlViewModel, INotifyPropertyChanged
    {
        private const string NoDriverResourceName = "dsms_err_no_driver";
        private const string NoChocolateyResourceName = "dsms_err_no_autoinstall";
        private const string NoAdmonRightsResourceName = "dsms_err_no_admin";
        private readonly IDataSourceManager _datasourceManager;
        private readonly IOdbcManager _odbcManager;
        private readonly IChocolateyController _chocoController;

        private readonly ILogger _logger;

        /// <summary>
        /// Defines the _configureUserdataSourceCmd
        /// </summary>
        private ICommand _configureUserdataSourceCmd;

        /// <summary>
        /// Defines the _supportedDatasources
        /// </summary>
        private SupportedSourceItem[] _supportedDatasources;

        /// <summary>
        /// Defines the _userDatasources
        /// </summary>
        private ObservableCollection<UserDataSource> _userDatasources;

        private SupportedSourceItem _selectedSourceItem;

        private UserDataSource _editingUserDatasource;

        private IPackage _package;

        private bool _packageInfoLoad;

        private OdbcDriver _odbcDriver;

        private bool _odbcDriverInstalled;

        private string _driverMessage;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatasourceViewModel"/> class.
        /// </summary>
        /// <param name="manager">The manager<see cref="IDataSourceManager"/></param>
        /// <param name="odbcManager">Odbc driver managment repository</param>
        /// <param name="chocoController">Choco controller</param>
        /// <param name="logger">Logger instance</param>
        public DatasourceViewModel([NotNull] IDataSourceManager manager, [NotNull] IOdbcManager odbcManager, IChocolateyController chocoController, [NotNull] ILogger logger)
        {
            this._datasourceManager = manager ?? throw new ArgumentNullException(nameof(manager));
            this._odbcManager = odbcManager ?? throw new ArgumentNullException(nameof(odbcManager));
            this._chocoController = chocoController;
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
            manager.DatasourceLoaded += this.InstanceOnDatasourceLoaded;
            this.SupportedDatasources = manager.SupportedDatasources.ToArray();
            this.UserDatasources = new ObservableCollection<UserDataSource>(manager.UserdefinedDatasources);
            this.ConfigureDs = new RelayCommand<SupportedSourceItem>(
                (si) =>
                    {
                        SelectedSourceItem = si;
                        this.DisplayUserDatasourceProperties();
                    });
        }

        /// <summary>
        /// Defines the PropertyChanged
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Changes editing context
        /// </summary>
        public SupportedSourceItem SelectedSourceItem
        {
            get => this._selectedSourceItem;
            set
            {
                this._selectedSourceItem = value;
                Package = null;
                OdbcDriver = null;
                if (value != null)
                {
                    var uds = this._datasourceManager.CreateNewUserdatasource(_selectedSourceItem.Module);
                    this.EditingUserDatasource = uds;
                    if (this._chocoController.ValidateChocoInstall())
                    {
                        this.LoadPackageInformation(value.Module);
                    }

                    RefreshDriverInformation(value.Module);
                }

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Supported chocolatey autoinstall
        /// </summary>
        public bool AutoinstallSupportred
        {
            get => _chocoController.ValidateChocoInstall()
                   && SelectedSourceItem != null
                   && !string.IsNullOrEmpty(SelectedSourceItem.Module.GetCorrectDriverName())
                   && Package != null && !OdbcDriverInstalled;
        }

        /// <summary>
        /// Дарайвер ODBC установлен
        /// </summary>
        public bool OdbcDriverInstalled
        {
            get => this._odbcDriverInstalled;
            private set
            {
                this._odbcDriverInstalled = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Editing user data source
        /// </summary>
        public UserDataSource EditingUserDatasource
        {
            get => this._editingUserDatasource;
            set
            {
                this._editingUserDatasource = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the ConfigureDs
        /// Open modal window to configure Database source
        /// </summary>
        public ICommand ConfigureDs
        {
            get => this._configureUserdataSourceCmd;
            set
            {
                this._configureUserdataSourceCmd = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the SupportedDatasources
        /// Collection of supported databases
        /// </summary>
        public SupportedSourceItem[] SupportedDatasources
        {
            get => this._supportedDatasources;
            private set
            {
                this._supportedDatasources = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the UserDatasources
        /// Collection of user defined database sources
        /// </summary>
        public ObservableCollection<UserDataSource> UserDatasources
        {
            get => this._userDatasources;
            set
            {
                this._userDatasources = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Информация о пакете драйвера Chocolatey
        /// </summary>
        public IPackage Package
        {
            get => _package;
            set
            {
                _package = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Assosiated ODBC driver
        /// </summary>
        public OdbcDriver OdbcDriver
        {
            get => this._odbcDriver;
            set
            {
                this._odbcDriver = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Message for driver display page
        /// </summary>
        public string DriverMessage
        {
            get => this._driverMessage;
            set
            {
                this._driverMessage = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Downloading information about package
        /// </summary>
        public bool PackageInfoLoad
        {
            get => this._packageInfoLoad;
            set
            {
                this._packageInfoLoad = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// The OnPropertyChanged
        /// </summary>
        /// <param name="propertyName">The propertyName<see cref="string"/></param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// The InstanceOnDatasourceLoaded
        /// </summary>
        /// <param name="datasources">The datasources<see cref="IEnumerable{SupportedSourceItem}"/></param>
        /// <param name="userSources">The userSources<see cref="IEnumerable{UserDataSource}"/></param>
        private void InstanceOnDatasourceLoaded(
            IEnumerable<SupportedSourceItem> datasources,
            IEnumerable<UserDataSource> userSources)
        {
            this.SupportedDatasources = datasources.ToArray();
            this.UserDatasources = new ObservableCollection<UserDataSource>(userSources);
        }

        /// <summary>
        /// The DisplayUserDatasourceProperties
        /// </summary>
        private void DisplayUserDatasourceProperties()
        {
            var dlgWindow = new DatasourceSettingsView();
            dlgWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            dlgWindow.Owner = Application.Current.MainWindow;
            dlgWindow.ShowDialog();
        }

        /// <summary>
        /// Execute chocolatey package info download if supported by database module
        /// </summary>
        /// <param name="edbDatasourceModule">Module</param>
        private async void LoadPackageInformation(EdbDatasourceModule edbDatasourceModule)
        {
            PackageInfoLoad = true;
            var res = await this._chocoController.GetPackageInformation(edbDatasourceModule.ChocolateOdbcPackageId);
            PackageInfoLoad = false;
            Package = res?.Package;
            this.OnPropertyChanged(nameof(this.AutoinstallSupportred));
        }

        private void RefreshDriverInformation(EdbDatasourceModule edbDatasourceModule)
        {
            OdbcDriver driver;
            OdbcDriverInstalled = _odbcManager.OdbcDriverInstalled(edbDatasourceModule.GetCorrectDriverName(), out driver);
            OdbcDriver = driver;
            var sb = new StringBuilder();

            if (!OdbcDriverInstalled)
            {
                var msg = Application.Current.Resources[NoDriverResourceName] ?? "Driver not installed.";
                sb.Append(msg);
            }

            if (!_chocoController.ValidateChocoInstall())
            {
                var msg = Application.Current.Resources[NoChocolateyResourceName] ?? " Chocolatey not installed.";
                sb.Append(msg);
            }

            if (!this._chocoController.IsAdministrator())
            {
                var msg = Application.Current.Resources[NoAdmonRightsResourceName] ?? " App. have no admin rights.";
                sb.Append(msg);
            }

            DriverMessage = sb.ToString();
            this.OnPropertyChanged(nameof(this.AutoinstallSupportred));
        }
    }
}