using EasyDb.Localization;
using EasyDb.ProjectManagment.Intefraces;
using EDb.Interfaces.iface;
using GalaSoft.MvvmLight;

namespace EasyDb.ViewModel.DataSource
{
    using System;
    using System.Linq;
    using System.Security;
    using System.Text;
    using System.Windows;
    using System.Windows.Input;
    using EasyDb.Annotations;
    using EasyDb.Commands;
    using EasyDb.View.DataSource;
    using EasyDb.ViewModel.Interfaces;
    using Edb.Environment.CommunicationArgs;
    using Edb.Environment.DatasourceManager;
    using Edb.Environment.Interface;
    using Edb.Environment.Model;
    using GalaSoft.MvvmLight.CommandWpf;
    using GalaSoft.MvvmLight.Messaging;
    using MahApps.Metro.Controls;
    using MahApps.Metro.Controls.Dialogs;
    using NuGet;
    using ILogger = Autofac.Extras.NLog.ILogger;
    using LogLevel = NLog.LogLevel;

    /// <summary>
    /// Implements logic for datasource creation control
    /// </summary>
    public class DatasourceSettingsViewModel : ViewModelBase, IDataSourceSettingsViewModel
    {
        private readonly IDataSourceManager _datasourceManager;
        private readonly IProjectEnvironment _appEnvironment;

        private readonly IDialogCoordinator _dialogCoordinator;

        private readonly IOdbcManager _odbcManager;

        private readonly IChocolateyController _chocoController;

        private readonly ILogger _logger;

        private SupportedSourceItem _selectedSourceItem;

        private UserDataSourceViewModelItem _editingUserDatasource;

        private IPackage _package;

        private bool _packageInfoLoad;

        private OdbcDriver _odbcDriver;

        private bool _odbcDriverInstalled;

        private string _warningMessage;

        private bool _gotDriverProblems;

        private bool _storePasswordSecure;

        private bool _databaseConnectionValid;

        private bool _databaseConnectionInProgress;

        private SupportedSourceItem[] _supportedDatasources;
        private bool testConnectionSuccess;
        private SecureString passwordSecureString;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatasourceSettingsViewModel"/> class.
        /// </summary>
        /// <param name="manager">The manager<see cref="IDataSourceManager"/></param>
        /// <param name="appEnvironment">The manager<see cref="IProjectEnvironment"/></param>
        /// <param name="passwordStorage">Password storage implementation</param>
        /// <param name="dialogCoord">Dialog coordinator</param>
        /// <param name="odbcManager">Odbc driver managment repository</param>
        /// <param name="chocoController">Choco controller</param>
        /// <param name="logger">Logger instance</param>
        /// <param name="connectionManager">Connection manager</param>
        /// <param name="messenger">view model messenger</param>
        public DatasourceSettingsViewModel(
            [NotNull] IDataSourceManager manager,
            [NotNull] IProjectEnvironment appEnvironment,
            [NotNull] IPasswordStorage passwordStorage,
            [NotNull] IDialogCoordinator dialogCoord,
            [NotNull] IOdbcManager odbcManager,
            [NotNull] IChocolateyController chocoController,
            [NotNull] ILogger logger,
            [NotNull] IConnectionManager connectionManager,
            [NotNull] IMessenger messenger)
        {
            if (connectionManager == null)
            {
                throw new ArgumentNullException(nameof(connectionManager));
            }

            if (messenger == null)
            {
                throw new ArgumentNullException(nameof(messenger));
            }

            if (passwordStorage == null)
            {
                throw new ArgumentNullException(nameof(passwordStorage));
            }

            _datasourceManager = manager ?? throw new ArgumentNullException(nameof(manager));
            _appEnvironment = appEnvironment ?? throw new ArgumentNullException(nameof(appEnvironment));
            _dialogCoordinator = dialogCoord ?? throw new ArgumentNullException(nameof(dialogCoord));
            _odbcManager = odbcManager ?? throw new ArgumentNullException(nameof(odbcManager));
            _chocoController = chocoController ?? throw new ArgumentNullException(nameof(chocoController));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            SupportedDatasources = _datasourceManager.SupportedDatasources.ToArray();

            // Reaction on datasources loaded communication message
            messenger.Register(this, new Action<DatasourcesIniaialized>(iniaialized => SupportedDatasources = iniaialized.SupportedSources.ToArray()));

            RefreshPackageInformationCmd = new EDbCommand(() =>
            {
                LoadPackageInformation(SelectedSourceItem?.Module);
                RefreshDriverInformation(SelectedSourceItem?.Module);
            });

            InstallPackageAutoCmd = new EDbCommand(async () =>
            {
                if (Package != null && AutoinstallSupportred)
                {
                    var dlg = SwitchPkgInstallDialog();
                    try
                    {
                        var res = await _chocoController.InstallPackage(Package.Id);
                        if (!res.Successful)
                        {
                            var sb = new StringBuilder(WarningMessage);
                            sb.Append(res.Exception.Message);
                            WarningMessage = sb.ToString();
                        }

                        LoadPackageInformation(SelectedSourceItem?.Module);
                        RefreshDriverInformation(SelectedSourceItem?.Module);
                        SwitchPkgInstallDialog(dlg, false);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex);
                        SwitchPkgInstallDialog(dlg, false);
                        throw;
                    }
                }
            });

            CloseInformationMessageCmd = new EDbCommand(
                () =>
                {
                    WarningMessage = string.Empty;
                    Package = null;
                    OdbcDriver = null;
                    SelectedSourceItem = null;
                });

            ApplyDatasourceSettingsCmd = new EDbCommand<MetroWindow>(
                (w) =>
                    {
                        if (!EditingUserDatasource.AllValid())
                        {
                            WarningMessage = EditingUserDatasource.GetErrors();
                            return;
                        }

                        try
                        {
                            _appEnvironment.ApplyDatasourceConfig(EditingUserDatasource.DsConfiguration);
                        }
                        catch (Exception ex)
                        {
                            var msg = "Error while saving user datasource configuration";
                            _logger.Log(LogLevel.Error, msg, ex);
                            throw new Exception(msg, ex);
                        }

                        // Execute password store
                        if (StorePasswordSecure)
                        {
                            passwordStorage.StorePasswordSecure(PasswordSecureString, EditingUserDatasource.DatasourceGuid);
                        }

                        w.Close();
                    });

            CloseSettingsWindowCmd = new RelayCommand<MetroWindow>(
                (w) =>
                    {
                        w.Close();
                    });

            TestConnection = new EDbCommand(
                () =>
                    {
                        DatabaseConnectionInProgress = true;
                        if (EditingUserDatasource.AllValid() && SelectedSourceItem != null)
                        {
                            connectionManager.RemoveConnectionFromSource(EditingUserDatasource.DsConfiguration.ConfigurationGuid);
                            using (var conn = connectionManager.ProduceDbConnection(EditingUserDatasource.DsConfiguration, PasswordSecureString))
                            {
                                try
                                {
                                    var cmd = conn.CreateCommand();
                                    cmd.CommandText = SelectedSourceItem.Module.GetQueryProducer().GetTestConnectionQuery();
                                    cmd.ExecuteNonQuery();
                                    WarningMessage = string.Concat(Application.Current.Resources[ResourceKeynames.DsmsTestConnSuccessKey].ToString(), WarningMessage);
                                    TestConnectionSuccess = true;
                                    _dialogCoordinator.ShowMessageAsync(this, Application.Current.Resources[ResourceKeynames.DsmsConnectionButtonKey].ToString(), Application.Current.Resources[ResourceKeynames.DsmsTestConnSuccessKey].ToString(), MessageDialogStyle.Affirmative);
                                }
                                catch (Exception ex)
                                {
                                    _dialogCoordinator.ShowMessageAsync(this, Application.Current.Resources[ResourceKeynames.DsmsConnectionButtonKey].ToString(), ex.Message, MessageDialogStyle.Affirmative);
                                }
                            }
                        }
                        else
                        {
                            WarningMessage = "$cannottestinvalid";
                        }
                    });
        }

        /// <summary>
        /// Changes editing context
        /// </summary>
        public SupportedSourceItem SelectedSourceItem
        {
            get => _selectedSourceItem;
            set
            {
                _selectedSourceItem = value;
                Package = null;
                if (value != null)
                {
                    var uds = _datasourceManager.CreateDataSourceConfig(_selectedSourceItem.Module);
                    var udsvm = new UserDataSourceViewModelItem(uds, _selectedSourceItem.Module);
                    EditingUserDatasource = udsvm;
                    if (_chocoController.ValidateChocoInstall())
                    {
                        LoadPackageInformation(value.Module);
                    }

                    RefreshDriverInformation(value.Module);
                }

                RaisePropertyChanged(() => SelectedSourceItem);
            }
        }

        /// <summary>
        /// Supported chocolatey autoinstall
        /// </summary>
        public bool AutoinstallSupportred
        {
            get => _chocoController.ValidateChocoInstall()
                   && SelectedSourceItem != null
                   && _chocoController.IsAdministrator()
                   && !string.IsNullOrEmpty(SelectedSourceItem.Module.GetCorrectDriverName())
                   && Package != null && !OdbcDriverInstalled;
        }

        /// <summary>
        /// Datasource got problems with driver
        /// </summary>
        public bool GotDriverProblems
        {
            get => _gotDriverProblems;
            set
            {
                _gotDriverProblems = value;
                RaisePropertyChanged(() => GotDriverProblems);
            }
        }

        /// <summary>
        /// Store password securely in password storage
        /// </summary>
        public bool StorePasswordSecure
        {
            get => _storePasswordSecure;
            set
            {
                _storePasswordSecure = value;
                RaisePropertyChanged(() => StorePasswordSecure);
            }
        }

        /// <summary>
        /// Connection test success
        /// </summary>
        public bool TestConnectionSuccess
        {
            get => testConnectionSuccess;
            set
            {
                testConnectionSuccess = value;
                RaisePropertyChanged(() => TestConnectionSuccess);
            }
        }

        /// <summary>
        /// Дарайвер ODBC установлен
        /// </summary>
        public bool OdbcDriverInstalled
        {
            get => _odbcDriverInstalled;
            private set
            {
                _odbcDriverInstalled = value;
                RaisePropertyChanged(() => OdbcDriverInstalled);
            }
        }

        /// <summary>
        /// Editing user data source
        /// </summary>
        public UserDataSourceViewModelItem EditingUserDatasource
        {
            get => _editingUserDatasource;
            set
            {
                _editingUserDatasource = value;
                RaisePropertyChanged(() => EditingUserDatasource);
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
        /// Test database connection
        /// </summary>
        public ICommand TestConnection { get; set; }

        /// <summary>
        /// Supported datasource collection
        /// </summary>
        public SupportedSourceItem[] SupportedDatasources
        {
            get => _supportedDatasources;
            set
            {
                _supportedDatasources = value;
                RaisePropertyChanged(() => SupportedDatasources);
            }
        }

        /// <summary>
        /// True if Database connection valid
        /// </summary>
        public bool DatabaseConnectionValid
        {
            get => _databaseConnectionValid;
            set
            {
                _databaseConnectionValid = value;
                RaisePropertyChanged(() => DatabaseConnectionValid);
            }
        }

        /// <summary>
        /// Processing database connection
        /// </summary>
        public bool DatabaseConnectionInProgress
        {
            get => _databaseConnectionInProgress;
            set
            {
                _databaseConnectionInProgress = value;
                RaisePropertyChanged(() => DatabaseConnectionInProgress);
            }
        }

        /// <summary>
        /// Password secure string
        /// </summary>
        public SecureString PasswordSecureString
        {
            get => passwordSecureString;
            set
            {
                passwordSecureString = value;
                TestConnectionSuccess = false;
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
                RaisePropertyChanged(() => Package);
            }
        }

        /// <summary>
        /// Assosiated ODBC driver
        /// </summary>
        public OdbcDriver OdbcDriver
        {
            get => _odbcDriver;
            set
            {
                _odbcDriver = value;
                RaisePropertyChanged(() => OdbcDriver);
            }
        }

        /// <summary>
        /// Message for driver display page
        /// </summary>
        public string WarningMessage
        {
            get => _warningMessage;
            set
            {
                _warningMessage = value;
                RaisePropertyChanged(() => WarningMessage);
            }
        }

        /// <summary>
        /// Downloading information about package
        /// </summary>
        public bool ProcessInProgress
        {
            get => _packageInfoLoad;
            set
            {
                _packageInfoLoad = value;
                RaisePropertyChanged(() => ProcessInProgress);
            }
        }

        /// <summary>
        /// Unregisters this instance from the Messenger class.
        /// <para>To cleanup additional resources, override this method, clean
        /// up and then call base.Cleanup().</para>
        /// </summary>
        public override void Cleanup()
        {
            SupportedDatasources = null;
            SelectedSourceItem = null;
            MessengerInstance.Unregister<DatasourcesIniaialized>(this);
            base.Cleanup();
        }

        /// <summary>
        /// Execute chocolatey package info download if supported by database module
        /// </summary>
        /// <param name="edbDataDatasource">Module</param>
        private async void LoadPackageInformation(IEdbDataSource edbDataDatasource)
        {
            Package = null;
            if (edbDataDatasource == null)
            {
                return;
            }

            ProcessInProgress = true;
            try
            {
                var res = await _chocoController.GetPackageInformation(edbDataDatasource.ChocolateOdbcPackageId);
                Package = res?.Package;
                ProcessInProgress = false;
            }
            catch (Exception ex)
            {
                ProcessInProgress = false;
                WarningMessage = ex.Message;
                throw;
            }

            RaisePropertyChanged(() => AutoinstallSupportred);
        }

        private void RefreshDriverInformation(IEdbDataSource edbDataDatasource)
        {
            OdbcDriver = null;
            GotDriverProblems = false;
            _odbcManager.RefreshDriversCatalog();
            if (edbDataDatasource == null)
            {
                return;
            }

            OdbcDriver driver;
            OdbcDriverInstalled = _odbcManager.OdbcDriverInstalled(edbDataDatasource.GetCorrectDriverName(), out driver);
            OdbcDriver = driver;
            var sb = new StringBuilder();

            if (!OdbcDriverInstalled)
            {
                var msg = Application.Current.Resources[ResourceKeynames.DsmsNoDriverResourceKey] ?? "Driver not installed.";
                sb.Append(msg);
                GotDriverProblems = true;
            }

            if (!_chocoController.ValidateChocoInstall())
            {
                var msg = Application.Current.Resources[ResourceKeynames.DsmsNoChocolateyResourceKey] ?? " Chocolatey not installed.";
                sb.Append(msg);
            }

            if (!_chocoController.IsAdministrator())
            {
                var msg = Application.Current.Resources[ResourceKeynames.NoAdminPrivilegesKey] ?? " App. have no admin rights.";
                sb.Append(msg);
            }

            if (_package != null && !_package.IsLatestVersion)
            {
                var msg = "Not a last driver version.";
                sb.Append(msg);
                GotDriverProblems = true;
            }

            WarningMessage = sb.ToString();
            RaisePropertyChanged(() => AutoinstallSupportred);
        }

        private BaseMetroDialog SwitchPkgInstallDialog(BaseMetroDialog dlg = null, bool show = true)
        {
            if (show)
            {
                var cd = new CustomDialog();
                cd.Height = 250;
                cd.Content = new PackageInstallDialog(_chocoController);
                _dialogCoordinator.ShowMetroDialogAsync(this, cd);
                return cd;
            }
            else
            {
                _dialogCoordinator.HideMetroDialogAsync(this, dlg);
                return dlg;
            }
        }
    }
}