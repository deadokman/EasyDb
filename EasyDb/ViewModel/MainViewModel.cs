using System.Linq;
using System.Security;
using GalaSoft.MvvmLight.Messaging;

namespace EasyDb.ViewModel
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.IO;
    using System.Windows;
    using System.Windows.Input;

    using EasyDb.Annotations;
    using EasyDb.CustomControls.Choco;
    using EasyDb.View;
    using EasyDb.ViewModel.Choco;

    using Edb.Environment.Interface;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;

    using MahApps.Metro.Controls.Dialogs;

    /// <summary>
    /// Main window view model
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly IDataSourceManager _manager;

        private readonly IDialogCoordinator _dialogCoordinator;

        private readonly IChocolateyController _chocoController;

        /// <summary>
        /// Defines the _activePane
        /// </summary>
        private PaneBaseViewModel _activePane;

        /// <summary>
        /// Background worker for plugin manager initialization
        /// </summary>
        private BackgroundWorker _bgWorkerInit = new BackgroundWorker();

        /// <summary>
        /// Defines the _isInterfaceEnabled
        /// </summary>
        private bool _isInterfaceEnabled;

        /// <summary>
        /// Defines the _panesCollection
        /// </summary>
        private ObservableCollection<PaneBaseViewModel> _panesCollection;

        /// <summary>
        /// Defines the _showLogInForm
        /// </summary>
        private bool _showLogInForm;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        /// <param name="manager">The manager<see cref="IDataSourceManager"/></param>
        /// <param name="dialogCoordinator">Dialog coordinator</param>
        /// <param name="chocoController">Chocolatey controller</param>
        /// <param name="chocoInstallVm">Chocolatey install view model</param>
        /// <param name="messenger">Messanger instance</param>
        public MainViewModel(
                             [NotNull] IDataSourceManager manager,
                             [NotNull] IDialogCoordinator dialogCoordinator,
                             [NotNull] IChocolateyController chocoController,
                             [NotNull] IChocolateyInstallViewModel chocoInstallVm,
                             [NotNull] IMessenger messenger)
        {
            if (chocoController == null)
            {
                throw new ArgumentNullException(nameof(chocoController));
            }

            if (chocoInstallVm == null)
            {
                throw new ArgumentNullException(nameof(chocoInstallVm));
            }

            if (messenger == null)
            {
                throw new ArgumentNullException(nameof(messenger));
            }

            this._manager = manager ?? throw new ArgumentNullException(nameof(manager));
            this._dialogCoordinator = dialogCoordinator ?? throw new ArgumentNullException(nameof(dialogCoordinator));
            this._chocoController = chocoController ?? throw new ArgumentNullException(nameof(chocoController));
            this.IsInterfaceEnabled = false;
            this._bgWorkerInit.DoWork += (sender, args) =>
                {
                    manager.InitialLoad(Path.Combine(Directory.GetCurrentDirectory(), "SourceExtensions"));
                };

            this._bgWorkerInit.RunWorkerCompleted += (sender, args) =>
                {
                    // If fist time launch, show log in form
                    if (Properties.Settings.Default.IsFirstTimeStartUp)
                    {
                    }

                    this.IsInterfaceEnabled = true;
                };

            this.ContentRendered = new RelayCommand(async () =>
                    {
                        if (!this._chocoController.ValidateChocoInstall() && !chocoInstallVm.HideDialog)
                        {
                            var cd = new CustomDialog();
                            cd.Height = 250;
                            cd.Content = new ChocolateyInstallControll(
                                () => { this._dialogCoordinator.HideMetroDialogAsync(this, cd); });
                            await this._dialogCoordinator.ShowMetroDialogAsync(this, cd);
                        }
                    });

            OpenOdbcManager = new RelayCommand(
                () =>
                    {
                        var managerW = new OdbcManagerView();
                        managerW.Owner = App.Current.MainWindow;
                        managerW.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                        managerW.Show();
                    });

            // Validate choco install if necessary
            this.IsInterfaceEnabled = false;
            this._bgWorkerInit.RunWorkerAsync();
        }

        /// <summary>
        /// Gets or sets View content rendered
        /// </summary>
        public ICommand ContentRendered { get; set; }

        /// <summary>
        /// Gets or sets the ActivatePluginCommand
        /// </summary>
        public ICommand ActivatePluginCommand { get; set; }

        /// <summary>
        /// Opens ODBC manager as modal window
        /// </summary>
        public ICommand OpenOdbcManager { get; set; }

        /// <summary>
        /// Gets or sets the ActivePane
        /// Current selected plugin
        /// </summary>
        public PaneBaseViewModel ActivePane
        {
            get
            {
                return this._activePane;
            }

            set
            {
                this._activePane = value;
                RaisePropertyChanged(() => this.ActivePane);
            }
        }

        /// <summary>
        /// Gets a value indicating whether IsInterfaceDisabled
        /// UI disabled
        /// </summary>
        public bool IsInterfaceDisabled
        {
            get
            {
                return !this._isInterfaceEnabled;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether IsInterfaceEnabled
        /// UI enabled
        /// </summary>
        public bool IsInterfaceEnabled
        {
            get
            {
                return this._isInterfaceEnabled;
            }

            set
            {
                this._isInterfaceEnabled = value;
                RaisePropertyChanged(() => this.IsInterfaceEnabled);
                RaisePropertyChanged(() => this.IsInterfaceDisabled);
            }
        }

        /// <summary>
        /// Gets the PaneViewModels
        /// Plugin view models base class
        /// </summary>
        public ObservableCollection<PaneBaseViewModel> PaneViewModels
        {
            get
            {
                if (this._panesCollection == null)
                {
                    this._panesCollection = new ObservableCollection<PaneBaseViewModel>();
                }

                return this._panesCollection;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether ShowLogInForm
        /// Show steam shop login form
        /// </summary>
        public bool ShowLogInForm
        {
            get
            {
                return this._showLogInForm;
            }

            set
            {
                this._showLogInForm = value;
                RaisePropertyChanged(() => this.ShowLogInForm);
            }
        }

        /// <summary>
        /// The Cleanup
        /// </summary>
        public override void Cleanup()
        {
            base.Cleanup();
        }

        /// <summary>
        /// The PaneClosing
        /// </summary>
        /// <param name="vm">The vm<see cref="PaneBaseViewModel"/></param>
        private void PaneClosing(PaneBaseViewModel vm)
        {
            vm.PaneClosing -= this.PaneClosing;

            // vm.Plugin.StopPlugin();
            this.PaneViewModels.Remove(vm);
        }
    }
}