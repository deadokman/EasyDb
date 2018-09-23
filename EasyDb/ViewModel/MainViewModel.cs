using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using EasyDb.ViewModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;

namespace EasyDb.ViewModel
{
    /// <summary>
    /// Main window view model
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private ObservableCollection<PluginBaseViewModel> _pluginsCollection;
        private PluginBaseViewModel _activePlugin;
        private bool _isInterfaceEnabled;

        public ICommand ActivatePluginCommand { get; set; }


        /// <summary>
        /// Background worker for plugin manager initialization
        /// </summary>
        private BackgroundWorker _bgWorkerInit = new BackgroundWorker();


        private bool _showLogInForm;

 
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            _bgWorkerInit.DoWork += (sender, args) =>
            {
            };

            _bgWorkerInit.RunWorkerCompleted += (sender, args) =>
            {
                //If fist time launch, show log in form
                if (Properties.Settings.Default.IsFirstTimeStartUp)
                {
                    ShowLogInForm = false;
                }
                else
                {
                    IsInterfaceEnabled = true;
                }

                IsInterfaceEnabled = true;
            };

            IsInterfaceEnabled = false;
            _bgWorkerInit.RunWorkerAsync();
        }

        private void PluginVmOnPluginClosing(PluginBaseViewModel vm)
        {
            vm.PluginClosing -= PluginVmOnPluginClosing;
            //vm.Plugin.StopPlugin();
            PaneViewModels.Remove(vm);
        }

        public override void Cleanup()
        {
            base.Cleanup();
        }

        /// <summary>
        /// Show steam shop login form
        /// </summary>
        public bool ShowLogInForm
        {
            get { return _showLogInForm; }
            set
            {
                _showLogInForm = value;
                RaisePropertyChanged(() => ShowLogInForm);
            }
        }

        /// <summary>
        /// UI enabled
        /// </summary>
        public bool IsInterfaceEnabled
        {
            get { return _isInterfaceEnabled; }
            set
            {
                _isInterfaceEnabled = value; 
                RaisePropertyChanged(() => IsInterfaceEnabled);
                RaisePropertyChanged(() => IsInterfaceDisabled);
            }
        }

        /// <summary>
        /// UI disabled
        /// </summary>
        public bool IsInterfaceDisabled
        {
            get { return !_isInterfaceEnabled; }
        }

        /// <summary>
        /// Plugin view models base class
        /// </summary>
        public ObservableCollection<PluginBaseViewModel> PaneViewModels
        {
            get
            {
                if (_pluginsCollection == null)
                {
                    _pluginsCollection =
                        new ObservableCollection<PluginBaseViewModel>();
                }

                return _pluginsCollection;
            }
        }

        /// <summary>
        /// Current selected plugin
        /// </summary>
        public PluginBaseViewModel ActivePlugin
        {
            get { return _activePlugin; }
            set
            {
                _activePlugin = value; 
                RaisePropertyChanged(() => ActivePlugin);
            }
        }
    }
}