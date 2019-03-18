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
    using System.Text;
    using System.Windows;
    using System.Windows.Input;
    using Annotations;
    using EasyDb.Interfaces.Data;
    using EasyDb.Model;
    using EasyDb.View.DataSource;
    using Edb.Environment.DatasourceManager;
    using EDb.Interfaces;
    using GalaSoft.MvvmLight.CommandWpf;
    using MahApps.Metro.Controls.Dialogs;
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

        private readonly IDialogCoordinator _dialogCoordinator;

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
        private ObservableCollection<UserDataSourceViewModelItem> _userDatasources;

        private SupportedSourceItem _selectedSourceItem;

        private UserDataSourceViewModelItem _editingUserDatasource;

        private IPackage _package;

        private bool _packageInfoLoad;

        private OdbcDriver _odbcDriver;

        private bool _odbcDriverInstalled;

        private string _warningMessage;

        private bool _gotDriverProblems;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatasourceViewModel"/> class.
        /// </summary>
        /// <param name="manager">The manager<see cref="IDataSourceManager"/></param>
        /// <param name="dialogCoord">Dialog coordinator</param>
        /// <param name="odbcManager">Odbc driver managment repository</param>
        /// <param name="chocoController">Choco controller</param>
        /// <param name="logger">Logger instance</param>
        public DatasourceViewModel([NotNull] IDataSourceManager manager, [NotNull] IDialogCoordinator dialogCoord, [NotNull] IOdbcManager odbcManager, IChocolateyController chocoController, [NotNull] ILogger logger)
        {
            this._datasourceManager = manager ?? throw new ArgumentNullException(nameof(manager));
            this._dialogCoordinator = dialogCoord ?? throw new ArgumentNullException(nameof(dialogCoord));
            this._odbcManager = odbcManager ?? throw new ArgumentNullException(nameof(odbcManager));
            this._chocoController = chocoController;
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
            manager.DatasourceLoaded += this.InstanceOnDatasourceLoaded;
            this.SupportedDatasources = manager.SupportedDatasources.ToArray();
            var udsVmi = manager.UserDatasourceConfigurations.Select(
                i => new UserDataSourceViewModelItem(i, this._datasourceManager.GetModuleByGuid(i.ModuleGuid)));
            this.UserDatasources = new ObservableCollection<UserDataSourceViewModelItem>(udsVmi);
            this.ConfigureDsCmd = new RelayCommand<SupportedSourceItem>(
                (si) =>
                    {
                        SelectedSourceItem = si;
                        this.DisplayUserDatasourceProperties();
                    });
            this.RefreshPackageInformationCmd = new RelayCommand(() =>
            {
                LoadPackageInformation(SelectedSourceItem?.Module);
                RefreshDriverInformation(SelectedSourceItem?.Module);
            });

            InstallPackageAutoCmd = new RelayCommand(async () =>
            {
                if (Package != null && AutoinstallSupportred)
                {
                    var dlg = SwitchPkgInstallDialog();
                    try
                    {
                        var res = await _chocoController.InstallPackage(Package.Id);
                        if (!res.Successful)
                        {
                            var sb = new StringBuilder(this.WarningMessage);
                            sb.Append(res.Exception.Message);
                            this.WarningMessage = sb.ToString();
                        }

                        LoadPackageInformation(SelectedSourceItem?.Module);
                        RefreshDriverInformation(SelectedSourceItem?.Module);
                        SwitchPkgInstallDialog(dlg, false);
                    }
                    catch (Exception ex)
                    {
                        this._logger.Error(ex);
                        SwitchPkgInstallDialog(dlg, false);
                        throw ex;
                    }
                }
            });

            this.CloseInformationMessageCmd = new RelayCommand(
                () => { this.WarningMessage = string.Empty; });

            this.ApplyDatasourceSettingsCmd = new RelayCommand(
                () =>
                    {
                        if (!this.EditingUserDatasource.ValidateAll())
                        {
                            this.WarningMessage = "$someinvalidconfig";
                        }
                    });

            this.CloseSettingsWindowCmd = new RelayCommand(
                () =>
                    {
                        throw new NotImplementedException();
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
                if (value != null)
                {
                    var uds = this._datasourceManager.CreateDataSourceConfig(_selectedSourceItem.Module);
                    var udsvm = new UserDataSourceViewModelItem(uds, this._selectedSourceItem.Module);
                    this.EditingUserDatasource = udsvm;
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
                   && this._chocoController.IsAdministrator()
                   && !string.IsNullOrEmpty(SelectedSourceItem.Module.GetCorrectDriverName())
                   && Package != null && !OdbcDriverInstalled;
        }

        /// <summary>
        /// Datasource got problems with driver
        /// </summary>
        public bool GotDriverProblems
        {
            get => this._gotDriverProblems;
            set
            {
                this._gotDriverProblems = value;
                this.OnPropertyChanged();
            }
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
        public UserDataSourceViewModelItem EditingUserDatasource
        {
            get => this._editingUserDatasource;
            set
            {
                this._editingUserDatasource = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the ConfigureDsCmd
        /// Open modal window to configure Database source
        /// </summary>
        public ICommand ConfigureDsCmd
        {
            get => this._configureUserdataSourceCmd;
            set
            {
                this._configureUserdataSourceCmd = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Refresh pakcage and driver information command
        /// </summary>
        public ICommand RefreshPackageInformationCmd { get; set; }

        /// <summary>
        /// Install package with chocolatey in semi-auto mode
        /// </summary>
        public ICommand InstallPackageAutoCmd { get; set; }

        /// <summary>
        /// Закрыть информационное сообщение
        /// </summary>
        public ICommand CloseInformationMessageCmd { get; set; }

        /// <summary>
        /// Save datasource settings and finish user data source
        /// </summary>
        public ICommand ApplyDatasourceSettingsCmd { get; set; }

        /// <summary>
        /// Закрыть окно настроек источника данных
        /// </summary>
        public ICommand CloseSettingsWindowCmd { get; set; }

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
        public ObservableCollection<UserDataSourceViewModelItem> UserDatasources
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
        public string WarningMessage
        {
            get => this._warningMessage;
            set
            {
                this._warningMessage = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Downloading information about package
        /// </summary>
        public bool ProcessInProgress
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
        /// <param name="userSources">The userSources<see cref="IEnumerable{UserDataSourceViewModelItem}"/></param>
        private void InstanceOnDatasourceLoaded(
            IEnumerable<SupportedSourceItem> datasources,
            IEnumerable<UserDatasourceConfiguration> userSources)
        {
            this.SupportedDatasources = datasources.ToArray();
            var udsVmi = userSources.Select(i => new UserDataSourceViewModelItem(i, this._datasourceManager.GetModuleByGuid(i.ModuleGuid)));
            this.UserDatasources = new ObservableCollection<UserDataSourceViewModelItem>(udsVmi);
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
        private async void LoadPackageInformation(IEdbSourceModule edbDatasourceModule)
        {
            Package = null;
            if (edbDatasourceModule == null)
            {
                return;
            }

            ProcessInProgress = true;
            try
            {
                var res = await this._chocoController.GetPackageInformation(edbDatasourceModule.ChocolateOdbcPackageId);
                Package = res?.Package;
                ProcessInProgress = false;
            }
            catch (Exception ex)
            {
                ProcessInProgress = false;
                this.WarningMessage = ex.Message;
                throw;
            }

            this.OnPropertyChanged(nameof(this.AutoinstallSupportred));
        }

        private void RefreshDriverInformation(IEdbSourceModule edbDatasourceModule)
        {
            OdbcDriver = null;
            _gotDriverProblems = false;
            _odbcManager.RefreshDriversCatalog();
            if (edbDatasourceModule == null)
            {
                return;
            }

            OdbcDriver driver;
            OdbcDriverInstalled = _odbcManager.OdbcDriverInstalled(edbDatasourceModule.GetCorrectDriverName(), out driver);
            OdbcDriver = driver;
            var sb = new StringBuilder();

            if (!OdbcDriverInstalled)
            {
                var msg = Application.Current.Resources[NoDriverResourceName] ?? "Driver not installed.";
                sb.Append(msg);
                GotDriverProblems = true;
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

            if (this._package != null && !this._package.IsLatestVersion)
            {
                var msg = "Not a last driver version.";
                sb.Append(msg);
                GotDriverProblems = true;
            }

            this.WarningMessage = sb.ToString();
            this.OnPropertyChanged(nameof(this.AutoinstallSupportred));
        }

        private BaseMetroDialog SwitchPkgInstallDialog(BaseMetroDialog dlg = null, bool show = true)
        {
            if (show)
            {
                var cd = new CustomDialog();
                cd.Height = 250;
                cd.Content = new PackageInstallDialog(this._chocoController);
                this._dialogCoordinator.ShowMetroDialogAsync(this, cd);
                return cd;
            }
            else
            {
                this._dialogCoordinator.HideMetroDialogAsync(this, dlg);
                return dlg;
            }
        }
    }
}