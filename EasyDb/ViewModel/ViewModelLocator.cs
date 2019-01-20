/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:CSGO.Trader"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using EasyDb.ViewModel;
using EasyDb.ViewModel.DataSource;
using EasyDb.ViewModel.Interfaces;
using EasyDb.ViewModel.Settings;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace EasyDb.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<GeneralSettingsViewModel>();
            SimpleIoc.Default.Register<SettingsWindowViewModel>();
            SimpleIoc.Default.Register<LoginViewModel>();
            SimpleIoc.Default.Register<IDatasourceControlViewModel, DatasourceViewModel>();
        }

        public SettingsWindowViewModel SettingsWindowViewModel
        {
            get { return ServiceLocator.Current.GetInstance<SettingsWindowViewModel>(); }
        }

        public LoginViewModel LoginVm
        {
            get { return ServiceLocator.Current.GetInstance<LoginViewModel>(); }
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public IDatasourceControlViewModel DatasourceControlViewModel
        {
            get { return ServiceLocator.Current.GetInstance<IDatasourceControlViewModel>(); }
        }

        public GeneralSettingsViewModel GeneralSettingViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<GeneralSettingsViewModel>();
            }
        }
        
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}