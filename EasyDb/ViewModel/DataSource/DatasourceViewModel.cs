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

        private SupportedSourceItem[] _supportedDatasources;
        private ICommand _configureUserdataSourceCmd;

        public DatasourceViewModel(IDataSourceManager manager)
        {
            manager.DatasourceLoaded += InstanceOnDatasourceLoaded;
            SupportedDatasources = manager.SupportedDatasources.ToArray();
            UserDatasources = new ObservableCollection<UserDataSource>(manager.UserdefinedDatasources);
            ConfigureDs = new RelayCommand<SupportedSourceItem>((si) =>
            {
                var item = si.InvokeConfigure();
                UserDatasources.Add(item);
            });
        }

        private void InstanceOnDatasourceLoaded(IEnumerable<SupportedSourceItem> datasources, IEnumerable<UserDataSource> userSources)
        {
            SupportedDatasources = datasources.ToArray();
            UserDatasources = new ObservableCollection<UserDataSource>(userSources);
        }

        /// <summary>
        /// Collection of supported databases
        /// </summary>
        public SupportedSourceItem[] SupportedDatasources
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
        public ICommand ConfigureDs
        {
            get => _configureUserdataSourceCmd;
            set
            {
                _configureUserdataSourceCmd = value;
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
