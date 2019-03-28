using System.Linq;
using System.Security;
using CommonServiceLocator;
using EasyDb.ProjectManagment.Intefraces;
using EasyDb.ViewModel.Interfaces;
using EasyDb.ViewModel.StartupPage;
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
        private readonly IApplicationEnvironment _environment;

        private readonly IDialogCoordinator _dialogCoordinator;

        private readonly IChocolateyController _chocoController;

        private readonly string _applicationConfigurationPath;

        /// <summary>
        /// Defines the _activePane
        /// </summary>
        private PaneBaseViewModel _activePane;

        /// <summary>
        /// Background worker for plugin applicationEnvironment initialization
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
        /// <param name="applicationEnvironment">The applicationEnvironment<see cref="IApplicationEnvironment"/></param>
        /// <param name="dialogCoordinator">Dialog coordinator</param>
        /// <param name="chocoController">Chocolatey controller</param>
        /// <param name="chocoInstallVm">Chocolatey install view model</param>
        /// <param name="messenger">Messanger instance</param>
        public MainViewModel(
                             [NotNull] IApplicationEnvironment applicationEnvironment,
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

            _applicationConfigurationPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    Properties.Settings.Default.ApplicationFolderName);
            _environment = applicationEnvironment ?? throw new ArgumentNullException(nameof(applicationEnvironment));
            _dialogCoordinator = dialogCoordinator ?? throw new ArgumentNullException(nameof(dialogCoordinator));
            _chocoController = chocoController ?? throw new ArgumentNullException(nameof(chocoController));
            IsInterfaceEnabled = false;
            _bgWorkerInit.DoWork += (sender, args) =>
                {
                    applicationEnvironment.Initialize(Path.GetFullPath(Properties.Settings.Default.PluginsPath), _applicationConfigurationPath);
                };

            _bgWorkerInit.RunWorkerCompleted += (sender, args) =>
                {
                    // If fist time launch, do somth usefull
                    if (Properties.Settings.Default.IsFirstTimeStartUp)
                    {
                    }

                    IsInterfaceEnabled = true;
                };

            ContentRendered = new RelayCommand(async () =>
                    {
                        if (!_chocoController.ValidateChocoInstall() && !chocoInstallVm.HideDialog)
                        {
                            var cd = new CustomDialog();
                            cd.Height = 250;
                            cd.Content = new ChocolateyInstallControll(
                                () => { _dialogCoordinator.HideMetroDialogAsync(this, cd); });
                            await _dialogCoordinator.ShowMetroDialogAsync(this, cd);
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
            IsInterfaceEnabled = false;
            _bgWorkerInit.RunWorkerAsync();

            // Add startup page pane
            PaneViewModels.Add(ServiceLocator.Current.GetInstance<StartUpPageViewModel>());
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
        /// Opens ODBC applicationEnvironment as modal window
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
                return _activePane;
            }

            set
            {
                _activePane = value;
                RaisePropertyChanged(() => ActivePane);
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
                return !_isInterfaceEnabled;
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
                return _isInterfaceEnabled;
            }

            set
            {
                _isInterfaceEnabled = value;
                RaisePropertyChanged(() => IsInterfaceEnabled);
                RaisePropertyChanged(() => IsInterfaceDisabled);
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
                if (_panesCollection == null)
                {
                    _panesCollection = new ObservableCollection<PaneBaseViewModel>();
                }

                return _panesCollection;
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
                return _showLogInForm;
            }

            set
            {
                _showLogInForm = value;
                RaisePropertyChanged(() => ShowLogInForm);
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
            vm.PaneClosing -= PaneClosing;

            // vm.Plugin.StopPlugin();
            PaneViewModels.Remove(vm);
        }
    }
}