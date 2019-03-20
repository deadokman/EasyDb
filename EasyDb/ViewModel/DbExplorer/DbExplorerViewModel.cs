using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using EasyDb.Annotations;
using EasyDb.Model;
using EasyDb.View.DataSource;
using EasyDb.ViewModel.DataSource.Items;
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

        private ICommand configureDsCmd;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbExplorerViewModel"/> class.
        /// Database explorer view model
        /// </summary>
        /// <param name="datasourceSettingsViewModel">Control datasource settings</param>
        /// <param name="datasourceManager">Datasource manager</param>
        /// <param name="messenger">View model communication messenger</param>
        public DbExplorerViewModel(IDataSourceSettingsViewModel datasourceSettingsViewModel, IDataSourceManager datasourceManager, IMessenger messenger)
        {
            this._datasourceSettingsViewModel = datasourceSettingsViewModel;
            this._datasourceManager = datasourceManager;
            this.SupportedDatasources = _datasourceManager.SupportedDatasources.ToArray();
            this.ConfigureDsCmd = new RelayCommand<SupportedSourceItem>(
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
            get => configureDsCmd;
            set
            {
                configureDsCmd = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets the SupportedDatasources
        /// Collection of supported databases
        /// </summary>
        public SupportedSourceItem[] SupportedDatasources
        {
            get => this._supportedDatasources;
            private set
            {
                this._supportedDatasources = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the UserDatasources
        /// Collection of user defined database sources
        /// </summary>
        public ObservableCollection<UserDataSourceViewModelItem> UserDatasources
        {
            get => this._userDatasources;
            set
            {
                this._userDatasources = value;
                this.OnPropertyChanged();
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
            this.SupportedDatasources = _datasourceManager.SupportedDatasources.ToArray();
            var udsVmi = _datasourceManager.UserDatasourceConfigurations.Select(i => new UserDataSourceViewModelItem(i, this._datasourceManager.GetModuleByGuid(i.ModuleGuid)));
            this.UserDatasources = new ObservableCollection<UserDataSourceViewModelItem>(udsVmi);
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
