using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using EasyDb.Annotations;
using EasyDb.ProjectManagment.Intefraces;
using EasyDb.View.DataSource;
using EasyDb.ViewModel.DataSource;
using EasyDb.ViewModel.Interfaces;
using Edb.Environment.CommunicationArgs;
using Edb.Environment.DatasourceManager;
using Edb.Environment.Interface;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;

namespace EasyDb.ViewModel.DbExplorer
{
    /// <summary>
    /// Database explorer view model item
    /// </summary>
    public class DbExplorerViewModel : ViewModelBase
    {
        private readonly IDataSourceSettingsViewModel _datasourceSettingsViewModel;
        private readonly IDataSourceManager _datasourceManager;
        private readonly IProjectEnvironment projectEnvironment;

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
        /// <param name="appEnv">Application environment</param>
        /// <param name="manager">App datasource _datasourceManager</param>
        /// <param name="messenger">View model communication messenger</param>
        public DbExplorerViewModel(
            [NotNull] IDataSourceSettingsViewModel datasourceSettingsViewModel,
            [NotNull] IProjectEnvironment appEnv,
            [NotNull] IDataSourceManager manager,
            [NotNull] IMessenger messenger)
        {
            if (messenger == null)
            {
                throw new ArgumentNullException(nameof(messenger));
            }

            _datasourceSettingsViewModel = datasourceSettingsViewModel ?? throw new ArgumentNullException(nameof(datasourceSettingsViewModel));
            _datasourceManager = manager ?? throw new ArgumentNullException(nameof(manager));
            projectEnvironment = appEnv;
            ConfigureDsCmd = new RelayCommand<SupportedSourceItem>(DisplayUserDatasourceProperties);

            // Reaction on datasources loaded communication message
            messenger.Register(this, new Action<DatasourcesIniaialized>(InstanceOnDatasourceLoaded));
            RefreshViewmodelData();
        }

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
                RaisePropertyChanged(() => ConfigureDsCmd);
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
                RaisePropertyChanged(() => SupportedDatasources);
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
                RaisePropertyChanged(() => UserDatasources);
            }
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
            var udsVmi = projectEnvironment.CurrentProject?.ConfigurationSources.Select(i => new UserDataSourceViewModelItem(i, _datasourceManager.GetModuleByGuid(i.DatasoureGuid)));
            if (udsVmi != null)
            {
                UserDatasources = new ObservableCollection<UserDataSourceViewModelItem>();
            }
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
