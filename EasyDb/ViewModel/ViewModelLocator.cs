namespace EasyDb.ViewModel
{
    using CommonServiceLocator;
    using EasyDb.Interfaces.Data;
    using EasyDb.SandboxEnvironment;
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
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            var loggerFactory = new LogFactory();
            SimpleIoc.Default.Register<IDataSourceManager, DatasourceManager>();
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<GeneralSettingsViewModel>();
            SimpleIoc.Default.Register<SettingsWindowViewModel>();
            SimpleIoc.Default.Register<LoginViewModel>();
            SimpleIoc.Default.Register<IDatasourceControlViewModel, DatasourceViewModel>();
        }

        /// <summary>
        /// Gets the SettingsWindowViewModel
        /// </summary>
        public SettingsWindowViewModel SettingsWindowViewModel
        {
            get { return ServiceLocator.Current.GetInstance<SettingsWindowViewModel>(); }
        }

        /// <summary>
        /// Gets the LoginVm
        /// </summary>
        public LoginViewModel LoginVm
        {
            get { return ServiceLocator.Current.GetInstance<LoginViewModel>(); }
        }

        /// <summary>
        /// Gets the Main
        /// </summary>
        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        /// <summary>
        /// Gets the DatasourceControlViewModel
        /// </summary>
        public IDatasourceControlViewModel DatasourceControlViewModel
        {
            get { return ServiceLocator.Current.GetInstance<IDatasourceControlViewModel>(); }
        }

        /// <summary>
        /// Gets the GeneralSettingViewModel
        /// </summary>
        public GeneralSettingsViewModel GeneralSettingViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<GeneralSettingsViewModel>();
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
