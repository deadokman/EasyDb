namespace EasyDb.ViewModel
{
    using Autofac;
    using CommonServiceLocator;
    using EasyDb.Interfaces.Data;
    using EasyDb.IoC;
    using EasyDb.SandboxEnvironment;
    using EasyDb.ViewModel.Choco;
    using EasyDb.ViewModel.DataSource;
    using EasyDb.ViewModel.Settings;
    using GalaSoft.MvvmLight.Ioc;
    using NLog;

    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelLocator"/> class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => AutofacServiceLocator.Instance);
        }

        /// <summary>
        /// Gets the SettingsWindowViewModel
        /// </summary>
        public SettingsWindowViewModel SettingsWindowViewModel
        {
            get { return AutofacServiceLocator.Instance.GetInstance<SettingsWindowViewModel>(); }
        }

        /// <summary>
        /// Gets the LoginVm
        /// </summary>
        public LoginViewModel LoginVm
        {
            get { return AutofacServiceLocator.Instance.GetInstance<LoginViewModel>(); }
        }

        /// <summary>
        /// Gets View model for chocolatey installation process
        /// </summary>
        public IChocolateyInstallViewModel ChocolateyInstallVm
        {
            get
            {
                return AutofacServiceLocator.Instance.GetInstance<IChocolateyInstallViewModel>();
            }
        }

        /// <summary>
        /// Gets the Main
        /// </summary>
        public MainViewModel Main
        {
            get
            {
                return AutofacServiceLocator.Instance.GetInstance<MainViewModel>();
            }
        }

        /// <summary>
        /// Gets the DatasourceControlViewModel
        /// </summary>
        public IDatasourceControlViewModel DatasourceControlViewModel
        {
            get { return AutofacServiceLocator.Instance.GetInstance<IDatasourceControlViewModel>(); }
        }

        /// <summary>
        /// Gets the GeneralSettingViewModel
        /// </summary>
        public GeneralSettingsViewModel GeneralSettingViewModel
        {
            get
            {
                return AutofacServiceLocator.Instance.GetInstance<GeneralSettingsViewModel>();
            }
        }

        /// <summary>
        /// The Cleanup
        /// </summary>
        public static void Cleanup()
        {
        }
    }
}
