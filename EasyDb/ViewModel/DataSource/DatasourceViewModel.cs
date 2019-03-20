using System.IO;
using EasyDb.ViewModel.DataSource.Items;
using EasyDb.ViewModel.Interfaces;
using Edb.Environment.Interface;
using Edb.Environment.Model;
using MahApps.Metro.Controls;

namespace EasyDb.ViewModel.DataSource
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Security;
    using System.Text;
    using System.Windows;
    using System.Windows.Input;
    using Annotations;

    using EasyDb.Interfaces;
    using EasyDb.Model;
    using EasyDb.View.DataSource;
    using Edb.Environment.DatasourceManager;
    using EDb.Interfaces;
    using GalaSoft.MvvmLight.CommandWpf;
    using MahApps.Metro.Controls.Dialogs;
    using NuGet;
    using ILogger = Autofac.Extras.NLog.ILogger;
    using LogLevel = NLog.LogLevel;

    /// <summary>
    /// Implements logic for datasource creation control
    /// </summary>
    public class DatasourceViewModel : IDataSourceSettingsViewModel, INotifyPropertyChanged
    {
        private const string NoDriverResourceName = "dsms_err_no_driver";
        private const string NoChocolateyResourceName = "dsms_err_no_autoinstall";
        private const string NoAdmonRightsResourceName = "dsms_err_no_admin";
        private readonly IDataSourceManager _datasourceManager;

        private readonly IPasswordStorage _passwordStorage;

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

        /// <summary>
        /// Initializes a new instance of the <see cref="DatasourceViewModel"/> class.
        /// </summary>
        /// <param name="manager">The manager<see cref="IDataSourceManager"/></param>
        /// <param name="passwordStorage">Password storage implementation</param>
        /// <param name="dialogCoord">Dialog coordinator</param>
        /// <param name="odbcManager">Odbc driver managment repository</param>
        /// <param name="chocoController">Choco controller</param>
        /// <param name="logger">Logger instance</param>
        public DatasourceViewModel(
            [NotNull] IDataSourceManager manager,
            [NotNull] IPasswordStorage passwordStorage,
           [NotNull] IDialogCoordinator dialogCoord,
           [NotNull] IOdbcManager odbcManager,
           [NotNull] IChocolateyController chocoController,
           [NotNull] ILogger logger)
        {
            this._datasourceManager = manager ?? throw new ArgumentNullException(nameof(manager));
            this._passwordStorage = passwordStorage ?? throw new ArgumentNullException(nameof(passwordStorage));
            this._dialogCoordinator = dialogCoord ?? throw new ArgumentNullException(nameof(dialogCoord));
            this._odbcManager = odbcManager ?? throw new ArgumentNullException(nameof(odbcManager));
            this._chocoController = chocoController ?? throw new ArgumentNullException(nameof(chocoController));
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));

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
                () =>
                {
                    this.WarningMessage = string.Empty;
                    this.Package = null;
                    this.OdbcDriver = null;
                    this.SelectedSourceItem = null;
                });

            this.ApplyDatasourceSettingsCmd = new RelayCommand<MetroWindow>(
                (w) =>
                    {
                        if (!this.EditingUserDatasource.AllValid())
                        {
                            this.WarningMessage = this.EditingUserDatasource.GetErrors();
                            return;
                        }

                        this._datasourceManager.ApplyUserDatasource(this.EditingUserDatasource.DsConfiguration);
                        try
                        {
                            this._datasourceManager.StoreUserDatasourceConfigurations();
                        }
                        catch (Exception ex)
                        {
                            var msg = "Error while saving user datasource configuration";
                            this._logger.Log(LogLevel.Error, msg, ex);
                            throw new Exception(msg, ex);
                        }

                        // Execute password store
                        if (StorePasswordSecure)
                        {
                            this._passwordStorage.StorePasswordSecure(PasswordSecureString, this.EditingUserDatasource.DatasourceGuid);
                        }

                        w.Close();
                    });

            this.CloseSettingsWindowCmd = new RelayCommand<MetroWindow>(
                (w) =>
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
        /// Store password securely in password storage
        /// </summary>
        public bool StorePasswordSecure
        {
            get => this._storePasswordSecure;
            set
            {
                this._storePasswordSecure = value;
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
        /// Password secure string
        /// </summary>
        public SecureString PasswordSecureString { get; set; }

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
            GotDriverProblems = false;
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