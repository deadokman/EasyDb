using EasyDb.ViewModel.DataSource.Items;
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
    using System.Windows;
    using System.Windows.Input;

    using Autofac.Extras.NLog;

    using EasyDb.Annotations;
    using EasyDb.Interfaces.Data;
    using EasyDb.View;
    using GalaSoft.MvvmLight.CommandWpf;

    /// <summary>
    /// Implements logic for datasource creation control
    /// </summary>
    public class DatasourceViewModel : IDatasourceControlViewModel, INotifyPropertyChanged
    {
        private readonly IDataSourceManager _manager;
        private readonly IOdbcRepository _odbcRepository;
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
        private ChocolateyPackage _package;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatasourceViewModel"/> class.
        /// </summary>
        /// <param name="manager">The manager<see cref="IDataSourceManager"/></param>
        /// <param name="odbcRepository">Odbc driver managment repository</param>
        /// <param name="chocoController">Choco controller</param>
        /// <param name="logger">Logger instance</param>
        public DatasourceViewModel([NotNull] IDataSourceManager manager, [NotNull] IOdbcRepository odbcRepository, IChocolateyController chocoController, [NotNull] ILogger logger)
        {
            this._manager = manager ?? throw new ArgumentNullException(nameof(manager));
            this._odbcRepository = odbcRepository ?? throw new ArgumentNullException(nameof(odbcRepository));
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
                if (value != null)
                {
                    var uds = _manager.CreateNewUserdatasource(_selectedSourceItem.Module);
                    this.EditingUserDatasource = uds;
                }

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Supported chocolatey autoinstall
        /// </summary>
        public bool AutoinstallSupportred
        {
            get => _chocoController.IsAdministrator() && _chocoController.ValidateChocoInstall();
        }

        /// <summary>
        /// Дарайвер ODBC установлен
        /// </summary>
        public bool OdbcDriverInstalled
        {
            get => _odbcRepository.OdbcDriverInstalled(this.SelectedSourceItem?.Module.OdbcSystemDriverName);
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
        public ChocolateyPackage Package
        {
            get => _package;
            set
            {
                _package = value;
                OnPropertyChanged();
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
    }
}