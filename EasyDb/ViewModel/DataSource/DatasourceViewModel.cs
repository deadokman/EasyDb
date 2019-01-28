using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using EasyDb.Annotations;
using EasyDb.Interfaces.Data;
using EasyDb.View;
using EasyDb.ViewModel.DataSource.Items;
using EDb.Interfaces;
using GalaSoft.MvvmLight.CommandWpf;

namespace EasyDb.ViewModel.DataSource
{
    /// <summary>
    /// Implements logic for datasource creation control
    /// </summary>
    public class DatasourceViewModel : IDatasourceControlViewModel, INotifyPropertyChanged
    {
        private ObservableCollection<UserDataSource> _userDatasources;

        private IEdbDatasourceModule[] _supportedDatasources;
        private ICommand configureUserdataSourceCmd;

        public DatasourceViewModel(IDataSourceManager manager)
        {
            manager.DatasourceLoaded += InstanceOnDatasourceLoaded;
            SupportedDatasources = manager.SupportedDatasources.ToArray();
            UserDatasources = new ObservableCollection<UserDataSource>(manager.UserdefinedDatasources);
            ConfigureUserdataSourceCmd = new RelayCommand<IEdbDatasourceModule>((module) =>
            {
                var uds = manager.CreateNewUserdatasource(module);
                UserDatasources.Add(uds);
                DisplayUserDatasourceProperties(uds);
            });
        }

        private void InstanceOnDatasourceLoaded(IEnumerable<IEdbDatasourceModule> datasources, IEnumerable<UserDataSource> userSources)
        {
            SupportedDatasources = datasources.ToArray();
            UserDatasources = new ObservableCollection<UserDataSource>(userSources);
        }

        private void DisplayUserDatasourceProperties(UserDataSource uds)
        {
            var dlgWindow = new DatasourceSettingsView();
            dlgWindow.DataContext = uds;
            dlgWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            dlgWindow.Owner = App.Current.MainWindow;
            dlgWindow.ShowDialog();
        }

        /// <summary>
        /// Collection of supported databases
        /// </summary>
        public IEdbDatasourceModule[] SupportedDatasources
        {
            get => _supportedDatasources;
            private set
            {
                _supportedDatasources = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Open modal window to configure Database source
        /// </summary>
        public ICommand ConfigureUserdataSourceCmd
        {
            get => configureUserdataSourceCmd;
            set
            {
                configureUserdataSourceCmd = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Collection of user defined database sources
        /// </summary>
        public ObservableCollection<UserDataSource> UserDatasources
        {
            get => _userDatasources;
            set
            {
                _userDatasources = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
