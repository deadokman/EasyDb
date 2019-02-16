namespace EasyDb.ViewModel.Choco
{
    using System;
    using System.Windows;
    using System.Windows.Input;

    using Autofac.Extras.NLog;

    using EasyDb.Annotations;
    using Edb.Choco.Interface;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;

    using MahApps.Metro.Controls.Dialogs;

    /// <summary>
    /// Defines the <see cref="ChocolateyInstallViewModel" />
    /// </summary>
    public class ChocolateyInstallViewModel : ViewModelBase, IChocolateyInstallViewModel
    {
        /// <summary>
        /// Defines the _chocoController
        /// </summary>
        private readonly IChocolateyController _chocoController;

        private readonly IDialogCoordinator _dialogCoordinator;

        private readonly ILogger _logger;

        private bool _installationCompletedSuccessfully;

        private bool _installationHasErrors;

        private string _installErrors;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChocolateyInstallViewModel"/> class.
        /// </summary>
        /// <param name="chocoController">Chocolatey packages controller</param>
        /// <param name="dialogCoordinator">Dialog coordinator</param>
        /// <param name="logger">Logger</param>
        public ChocolateyInstallViewModel(
            [NotNull] IChocolateyController chocoController,
            [NotNull] IDialogCoordinator dialogCoordinator,
            [NotNull] ILogger logger)
        {
            _chocoController = chocoController ?? throw new System.ArgumentNullException(nameof(chocoController));
            _dialogCoordinator = dialogCoordinator ?? throw new ArgumentNullException(nameof(dialogCoordinator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            CloseApplication = new RelayCommand(() => Application.Current.Shutdown());
            NavigateLink = new RelayCommand(
                () =>
                    {
                        System.Diagnostics.Process.Start("https://chocolatey.org/docs/installation");
                    });
            InstallChocolateyCommand = new RelayCommand(
                async () =>
                    {
                        this._logger.Debug("Installing Chocolate via powershell");
                        var ctrl = await this._dialogCoordinator.ShowProgressAsync(
                            Application.Current.MainWindow.DataContext,
                            Application.Current.Resources["choco_dlg_SetUp_message"].ToString(),
                            Application.Current.Resources["choco_dlg_SetUp_text"].ToString());
                        ctrl.SetIndeterminate();
                        ctrl.SetMessage(Application.Current.Resources["choco_dlg_install_progress"].ToString());
                        ctrl.Canceled += (sender, args) => { ctrl.CloseAsync(); };
                        try
                        {
                            var install = await _chocoController.DownloadAndStorePowershellInstall();
                            await this._chocoController.Runpowershell(install).ContinueWith(
                                (res) => { ctrl.CloseAsync(); });
                            InstallationCompletedSuccessfully = true;
                        }
                        catch (Exception ex)
                        {
                            InstallErrors = $"$errmsg: \n {ex.Message}";
                            InstallationHasErrors = true;
                            this._logger.Error(ex);
                            await ctrl.CloseAsync();
                        }
                    });
        }

        /// <summary>
        /// Gets a value indicating whether IsInAdministrativeMode
        /// Running in administrator mode
        /// Приложение запущено в режиме администратора
        /// </summary>
        public bool IsInAdministrativeMode => this._chocoController.IsAdministrator();

        /// <summary>
        /// Application runs not in administrator mode
        /// </summary>
        public bool IsNotInAdministrativeMode => !this._chocoController.IsAdministrator();

        /// <summary>
        /// Installation was coompleted Successfully
        /// </summary>
        public bool InstallationCompletedSuccessfully
        {
            get => this._installationCompletedSuccessfully;
            set
            {
                this._installationCompletedSuccessfully = value;
                this.RaisePropertyChanged(() => InstallationCompletedSuccessfully);
            }
        }

        /// <summary>
        /// There are was one or more error while installation
        /// </summary>
        public bool InstallationHasErrors
        {
            get => this._installationHasErrors;
            set
            {
                this._installationHasErrors = value;
                this.RaisePropertyChanged(() => InstallationHasErrors);
            }
        }

        /// <summary>
        /// Errors in installation
        /// </summary>
        public string InstallErrors
        {
            get => this._installErrors;
            set
            {
                this._installErrors = value;
                this.RaisePropertyChanged(() => InstallErrors);
            }
        }

        /// <summary>
        /// Close application command
        /// </summary>
        public ICommand CloseApplication { get; }

        /// <summary>
        /// Close dialog command
        /// </summary>
        public ICommand CloseDialog { get; }

        /// <summary>
        /// Install chocolatey
        /// </summary>
        public ICommand InstallChocolateyCommand { get; private set; }

        /// <summary>
        /// Перейти по ссылке
        /// </summary>
        public ICommand NavigateLink { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether HideDialog
        /// Gets or sets property that suppress dialog display if not needed
        /// </summary>
        public bool HideDialog
        {
            get
            {
#if DEBUG
                return false;
#else
                return Properties.Settings.Default.HideChocoInstall;
#endif
            }

            set
            {
                Properties.Settings.Default.HideChocoInstall = value;
                Properties.Settings.Default.Save();
            }
        }
    }
}
