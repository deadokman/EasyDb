using System.Linq;
using System.Security;
using System.Windows.Controls;
using CommonServiceLocator;
using EasyDb.Commands;
using EasyDb.ProjectManagment.Intefraces;
using EasyDb.ProjectManagment.ProjectSchema;
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
        private readonly IProjectEnvironment _environment;

        private readonly IDialogCoordinator _dialogCoordinator;

        private readonly IChocolateyController _chocoController;

        private readonly string _applicationConfigurationPath;

        /// <summary>
        /// Defines the _activePane
        /// </summary>
        private PaneBaseViewModel _activePane;

        /// <summary>
        /// Background worker for plugin projectEnvironment initialization
        /// </summary>
        private BackgroundWorker _bgWorkerInit = new BackgroundWorker();

        /// <summary>
        /// Defines the isHideSplashScreen
        /// </summary>
        private bool isHideSplashScreen;

        /// <summary>
        /// Defines the _panesCollection
        /// </summary>
        private ObservableCollection<PaneBaseViewModel> _panesCollection;

        private string mainWindowTitleDisplay;
        private bool projectLoaded;
        private EasyDbProject currentProject;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        /// <param name="projectEnvironment">The projectEnvironment<see cref="IProjectEnvironment"/></param>
        /// <param name="dialogCoordinator">Dialog coordinator</param>
        /// <param name="chocoController">Chocolatey controller</param>
        /// <param name="chocoInstallVm">Chocolatey install view model</param>
        /// <param name="messenger">Messanger instance</param>
        public MainViewModel(
                             [NotNull] IProjectEnvironment projectEnvironment,
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

            MainWindowTitleDisplay = "EasyDb";
            _applicationConfigurationPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    Properties.Settings.Default.ApplicationFolderName);
            _environment = projectEnvironment ?? throw new ArgumentNullException(nameof(projectEnvironment));
            _dialogCoordinator = dialogCoordinator ?? throw new ArgumentNullException(nameof(dialogCoordinator));
            _chocoController = chocoController ?? throw new ArgumentNullException(nameof(chocoController));
            HideSplashScreen = false;
            _bgWorkerInit.DoWork += (sender, args) =>
                {
                    projectEnvironment.Initialize(Path.GetFullPath(Properties.Settings.Default.PluginsPath), _applicationConfigurationPath);
                };

            _bgWorkerInit.RunWorkerCompleted += (sender, args) =>
                {
                    // If fist time launch, do somth usefull
                    if (Properties.Settings.Default.IsFirstTimeStartUp)
                    {
                    }

                    // Enable main window interface
                    HideSplashScreen = true;
                };

            ContentRenderedCommand = new EDbCommand(async () =>
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

            OpenOdbcManagerCommand = new EDbCommand(
                () =>
                    {
                        var managerW = new OdbcManagerView();
                        managerW.Owner = App.Current.MainWindow;
                        managerW.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                        managerW.Show();
                    });

            OpenStartupPageCommand = new EDbCommand(OpenStartupPage);

            // Validate choco install if necessary
            HideSplashScreen = false;
            _bgWorkerInit.RunWorkerAsync();

            OpenStartupPage();

            // Register reaction on new project selected
            messenger.Register<EasyDbProject>(this, ReactOnProjectChange);
        }

        /// <summary>
        /// Gets or sets View content rendered
        /// </summary>
        public ICommand ContentRenderedCommand { get; set; }

        /// <summary>
        /// Opens ODBC projectEnvironment as modal window
        /// </summary>
        public ICommand OpenOdbcManagerCommand { get; set; }

        /// <summary>
        /// Открыть стартовую страницу
        /// </summary>
        public ICommand OpenStartupPageCommand { get; set; }

        /// <summary>
        /// Title of the main window
        /// </summary>
        public string MainWindowTitleDisplay
        {
            get => mainWindowTitleDisplay;
            set
            {
                mainWindowTitleDisplay = value;
                RaisePropertyChanged(() => MainWindowTitleDisplay);
            }
        }

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
        /// Gets a value indicating whether IsShowSplashScreen
        /// UI disabled
        /// </summary>
        public bool IsShowSplashScreen
        {
            get
            {
                return !isHideSplashScreen;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether HideSplashScreen
        /// UI enabled
        /// </summary>
        public bool HideSplashScreen
        {
            get
            {
                return isHideSplashScreen;
            }

            set
            {
                isHideSplashScreen = value;
                RaisePropertyChanged(() => HideSplashScreen);
                RaisePropertyChanged(() => IsShowSplashScreen);
            }
        }

        /// <summary>
        /// Проект загружен
        /// </summary>
        public bool IsProjectLoaded
        {
            get => projectLoaded;
            set
            {
                projectLoaded = value;
                RaisePropertyChanged(() => IsProjectLoaded);
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
        /// Current EasyDb project
        /// </summary>
        public EasyDbProject CurrentProject
        {
            get => currentProject;
            set
            {
                currentProject = value;
                IsProjectLoaded = value != null;
                RaisePropertyChanged(() => CurrentProject);
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

            // Clean data
            Application.Current.Dispatcher.Invoke(() => PaneViewModels.Remove(vm));
            vm.Cleanup();
        }

        private void ReactOnProjectChange(EasyDbProject project)
        {
            MainWindowTitleDisplay = $"{project.ProjName} - EasyDb";

            var stpPages = PaneViewModels.Where(p => p is IStartUpPageViewModel).ToArray();
            foreach (var pbVm in stpPages)
            {
                pbVm.Close();
            }

            CurrentProject = project;
        }

        private void OpenStartupPage()
        {
            // Add startup page pane
            var stpPaneVm = ServiceLocator.Current.GetInstance<StartUpPageViewModel>();
            stpPaneVm.PaneClosing += PaneClosing;
            PaneViewModels.Add(stpPaneVm);
        }
    }
}