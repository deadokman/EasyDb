using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using EasyDb.Annotations;
using EasyDb.View.DataSource;
using EasyDb.ViewModel.DataSource;
using EasyDb.ViewModel.Interfaces;
using Edb.Environment.CommunicationArgs;
using Edb.Environment.DatasourceManager;
using Edb.Environment.Interface;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;

namespace EasyDb.ViewModel.DbExplorer
{
    /// <summary>
    /// Database explorer view model item
    /// </summary>
    public class DbExplorerViewModel : INotifyPropertyChanged
    {
        private readonly IDataSourceSettingsViewModel _datasourceSettingsViewModel;
        private readonly IDataSourceManager _datasourceManager;

        /// <summary>
        /// Defines the _supportedDatasources
        /// </summary>
        private SupportedSourceItem[] _supportedDatasources;

        /// <summary>
        /// Defines the _userDatasources
        /// </summary>
        private ObservableCollection<UserDataSourceViewModelItem> _userDatasources;

        private ICommand _configureDsCmd;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbExplorerViewModel"/> class.
        /// Database explorer view model
        /// </summary>
        /// <param name="datasourceSettingsViewModel">Control datasource settings</param>
        /// <param name="datasourceManager">Datasource manager</param>
        /// <param name="messenger">View model communication messenger</param>
        public DbExplorerViewModel(IDataSourceSettingsViewModel datasourceSettingsViewModel, IDataSourceManager datasourceManager, IMessenger messenger)
        {
            _datasourceSettingsViewModel = datasourceSettingsViewModel;
            _datasourceManager = datasourceManager;
            SupportedDatasources = _datasourceManager.SupportedDatasources.ToArray();
            ConfigureDsCmd = new RelayCommand<SupportedSourceItem>(
                DisplayUserDatasourceProperties);

            // Reaction on datasources loaded communication message
            messenger.Register(this, new Action<DatasourcesIniaialized>(InstanceOnDatasourceLoaded));
            RefreshViewmodelData();
        }

        /// <summary>
        /// Property changed event
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the ConfigureDsCmd
        /// Open modal window to configure Database source
        /// </summary>
        public ICommand ConfigureDsCmd
        {
            get => _configureDsCmd;
            set
            {
                _configureDsCmd = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the SupportedDatasources
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
        /// Gets or sets the UserDatasources
        /// Collection of user defined database sources
        /// </summary>
        public ObservableCollection<UserDataSourceViewModelItem> UserDatasources
        {
            get => _userDatasources;
            set
            {
                _userDatasources = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Property changed invoke
        /// </summary>
        /// <param name="propertyName">Prop name</param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// The InstanceOnDatasourceLoaded
        /// </summary>
        /// <param name="dsInitializedArg">The datasources<see cref="DatasourcesIniaialized"/></param>
        private void InstanceOnDatasourceLoaded(DatasourcesIniaialized dsInitializedArg)
        {
            RefreshViewmodelData();
        }

        private void RefreshViewmodelData()
        {
            SupportedDatasources = _datasourceManager.SupportedDatasources.ToArray();
            var udsVmi = _datasourceManager.UserDatasourceConfigurations.Select(i => new UserDataSourceViewModelItem(i, _datasourceManager.GetModuleByGuid(i.DatasoureGuid)));
            UserDatasources = new ObservableCollection<UserDataSourceViewModelItem>(udsVmi);
        }

        /// <summary>
        ///  Display UserDatasource Properties window
        /// </summary>
        /// <param name="supportedSourceItem">selected suported source item</param>
        private void DisplayUserDatasourceProperties(SupportedSourceItem supportedSourceItem)
        {
            var dlgWindow = new DatasourceSettingsView();
            dlgWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            dlgWindow.Owner = Application.Current.MainWindow;
            _datasourceSettingsViewModel.SelectedSourceItem = supportedSourceItem;
            dlgWindow.ShowDialog();
        }
    }
}
