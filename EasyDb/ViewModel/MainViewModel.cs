namespace EasyDb.ViewModel
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.IO;
    using System.Windows.Input;

    using EasyDb.Interfaces.Data;

    using GalaSoft.MvvmLight;

    /// <summary>
    /// Main window view model
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
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
        public MainViewModel(IDataSourceManager manager)
        {
            this._bgWorkerInit.DoWork += (sender, args) =>
                {
                    manager.InitialLoad(Path.Combine(Directory.GetCurrentDirectory(), "SourceExtensions"));
                };

            this._bgWorkerInit.RunWorkerCompleted += (sender, args) =>
                {
                    // If fist time launch, show log in form
                    if (Properties.Settings.Default.IsFirstTimeStartUp)
                    {
                        this.ShowLogInForm = false;
                    }
                    else
                    {
                        this.IsInterfaceEnabled = true;
                    }

                    this.IsInterfaceEnabled = true;
                };

            this.IsInterfaceEnabled = false;
            this._bgWorkerInit.RunWorkerAsync();
        }

        /// <summary>
        /// Gets or sets the ActivatePluginCommand
        /// </summary>
        public ICommand ActivatePluginCommand { get; set; }

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