using EasyDb.ViewModel.DbExplorer;

namespace EasyDb.ViewModel
{
    using Autofac;
    using CommonServiceLocator;
    using EasyDb.IoC;
    using EasyDb.SandboxEnvironment;
    using EasyDb.ViewModel.Choco;
    using EasyDb.ViewModel.DataSource;
    using EasyDb.ViewModel.Interfaces;
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
        /// Gets instacne of OdbcMangerViewModel
        /// </summary>
        public IOdbcManagerViewModel OdbcManagerViewModel
        {
            get
            {
                return AutofacServiceLocator.Instance.GetInstance<IOdbcManagerViewModel>();
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
        /// Gets the DataSourceSettingsViewModel
        /// </summary>
        public IDataSourceSettingsViewModel DataSourceSettingsViewModel
        {
            get { return AutofacServiceLocator.Instance.GetInstance<IDataSourceSettingsViewModel>(); }
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
        /// Db explorer view model
        /// </summary>
        public DbExplorerViewModel DbExplorerViewModel
        {
            get
            {
                return AutofacServiceLocator.Instance.GetInstance<DbExplorerViewModel>();
            }
        }

        /// <summary>
        /// ¬ью-модель стартовой страницы
        /// </summary>
        public IStartUpPageViewModel StartUpPageViewModel
        {
            get
            {
                return AutofacServiceLocator.Instance.GetInstance<IStartUpPageViewModel>();
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
