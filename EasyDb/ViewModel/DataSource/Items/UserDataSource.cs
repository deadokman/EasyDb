namespace EasyDb.ViewModel.DataSource.Items
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Windows.Input;
    using System.Xml.Serialization;

    using EDb.Interfaces;

    /// <summary>
    /// Presents datasource created by user
    /// </summary>
    public class UserDataSource : ValidationViewModelBase
    {
        /// <summary>
        /// Defines the _dsConfig
        /// </summary>
        private UserDatasourceConfiguration _dsConfig;

        /// <summary>
        /// Defines the _selectedDataSourceOption
        /// </summary>
        private EdbSourceOptionProxy _selectedDataSourceOption;

        /// <summary>
        /// Defines the _settingsObjects
        /// </summary>
        private EdbSourceOptionProxy[] _settingsObjects;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserDataSource"/> class.
        /// </summary>
        public UserDataSource()
        {
            this._dsConfig = new UserDatasourceConfiguration();
        }

        /// <summary>
        /// Gets or sets the ApplySettingsCommand
        /// Applies settings to the datasource
        /// </summary>
        [XmlIgnore]
        public ICommand ApplySettingsCommand { get; set; }

        /// <summary>
        /// Gets or sets the Comment
        /// User datasource comment
        /// </summary>
        public string Comment
        {
            get => this._dsConfig.Comment;
            set
            {
                this._dsConfig.Comment = value;
            }
        }

        /// <summary>
        /// Gets the DatasourceGuid
        /// </summary>
        public Guid DatasourceGuid
        {
            get => this._dsConfig.ModuleGuid;
        }

        /// <summary>
        /// Gets or sets the LinkedEdbSourceModule
        /// Драйвер СУБД
        /// </summary>
        public IEdbDatasourceModule LinkedEdbSourceModule { get; set; }

        /// <summary>
        /// Gets or sets the Name
        /// User datasource name
        /// </summary>
        [Required(ErrorMessageResourceName = "dsms_ex_datasourceName")]
        public string Name
        {
            get => this._dsConfig.Name;
            set
            {
                this._dsConfig.Name = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the SelectedDataSourceOption
        /// </summary>
        public EdbSourceOptionProxy SelectedDataSourceOption
        {
            get => this._selectedDataSourceOption;
            set
            {
                this._selectedDataSourceOption = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the SettingsObjects
        /// User datasource tab settings
        /// </summary>
        public EdbSourceOptionProxy[] SettingsObjects
        {
            get => this._settingsObjects;
            set
            {
                this._settingsObjects = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the TestConnection
        /// Command to test Connection
        /// </summary>
        [XmlIgnore]
        public ICommand TestConnection { get; set; }

        /// <summary>
        /// Установить Guid модуля
        /// </summary>
        /// <param name="moduleGuid">Set datasource guid ID</param>
        public void SetGuid(Guid moduleGuid)
        {
            this._dsConfig.ModuleGuid = moduleGuid;
        }
    }
}