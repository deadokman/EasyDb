namespace EasyDb.ViewModel.DataSource
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Windows.Input;

    using EasyDb.Annotations;
    using EasyDb.Interfaces.Data;
    using EasyDb.ViewModel.DataSource.Items;

    using GalaSoft.MvvmLight.CommandWpf;

    /// <summary>
    /// Implements logic for datasource creation control
    /// </summary>
    public class DatasourceViewModel : IDatasourceControlViewModel, INotifyPropertyChanged
    {
        /// <summary>
        /// Defines the _configureUserdataSourceCmd
        /// </summary>
        private ICommand _configureUserdataSourceCmd;

        /// <summary>
        /// Defines the _supportedDatasources
        /// </summary>
        private SupportedSourceItem[] _supportedDatasources;

        /// <summary>
        /// Defines the _userDatasources
        /// </summary>
        private ObservableCollection<UserDataSource> _userDatasources;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatasourceViewModel"/> class.
        /// </summary>
        /// <param name="manager">The manager<see cref="IDataSourceManager"/></param>
        public DatasourceViewModel(IDataSourceManager manager)
        {
            manager.DatasourceLoaded += this.InstanceOnDatasourceLoaded;
            this.SupportedDatasources = manager.SupportedDatasources.ToArray();
            this.UserDatasources = new ObservableCollection<UserDataSource>(manager.UserdefinedDatasources);
            this.ConfigureDs = new RelayCommand<SupportedSourceItem>(
                (si) =>
                    {
                        var item = si.InvokeConfigure();
                        this.UserDatasources.Add(item);
                    });
        }

        /// <summary>
        /// Defines the PropertyChanged
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the ConfigureDs
        /// Open modal window to configure Database source
        /// </summary>
        public ICommand ConfigureDs
        {
            get => this._configureUserdataSourceCmd;
            set
            {
                this._configureUserdataSourceCmd = value;
                this.OnPropertyChanged();
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
        public ObservableCollection<UserDataSource> UserDatasources
        {
            get => this._userDatasources;
            set
            {
                this._userDatasources = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// The OnPropertyChanged
        /// </summary>
        /// <param name="propertyName">The propertyName<see cref="string"/></param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// The InstanceOnDatasourceLoaded
        /// </summary>
        /// <param name="datasources">The datasources<see cref="IEnumerable{SupportedSourceItem}"/></param>
        /// <param name="userSources">The userSources<see cref="IEnumerable{UserDataSource}"/></param>
        private void InstanceOnDatasourceLoaded(
            IEnumerable<SupportedSourceItem> datasources,
            IEnumerable<UserDataSource> userSources)
        {
            this.SupportedDatasources = datasources.ToArray();
            this.UserDatasources = new ObservableCollection<UserDataSource>(userSources);
        }
    }
}