namespace EasyDb.ViewModel.DataSource.Items
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Windows.Input;
    using System.Xml.Serialization;

    using EasyDb.Model;

    using EDb.Interfaces;

    /// <summary>
    /// Presents datasource created by user
    /// </summary>
    public class UserDataSourceViewModelItem : ValidationViewModelBase
    {
        /// <summary>
        /// Defines the _dsConfig
        /// </summary>
        private UserDatasourceConfiguration _dsConfig;

        /// <summary>
        /// Defines the _selectedDataSourceOption
        /// </summary>
        private EdbSourceOption _selectedDataSourceOption;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserDataSourceViewModelItem"/> class.
        /// Конструктор класса
        /// </summary>
        /// <param name="udsConfig">Конфигурация</param>
        /// <param name="module">Модуль</param>
        public UserDataSourceViewModelItem(UserDatasourceConfiguration udsConfig, IEdbSourceModule module)
        {
            this._dsConfig = udsConfig;
        }

        /// <summary>
        /// Returns datasource configuration model
        /// </summary>
        public UserDatasourceConfiguration DsConfiguration => this._dsConfig;

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
        public IEdbSourceModule LinkedEdbSourceModule { get; set; }

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
        public EdbSourceOption SelectedDataSourceOption
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
        public EdbSourceOption[] SettingsObjects
        {
            get => this._dsConfig.SettingsObjects;
        }

        /// <summary>
        /// Установить Guid модуля
        /// </summary>
        /// <param name="moduleGuid">Set datasource guid ID</param>
        public void SetGuid(Guid moduleGuid)
        {
            this._dsConfig.ModuleGuid = moduleGuid;
        }

        /// <summary>
        /// Validate all source options
        /// </summary>
        /// <returns>Returns true if all options is valid</returns>
        public bool AllValid()
        {
            if (this._dsConfig.SettingsObjects != null)
            {
                return this._dsConfig.SettingsObjects.All(opt => string.IsNullOrEmpty(opt.Error)) && this.IsValid;
            }

            return this.IsValid;
        }

        /// <summary>
        /// Get options erros
        /// </summary>
        /// <returns>Errors</returns>
        public string GetErrors()
        {
            var sb = new StringBuilder();
            sb.Append(" " + this.Error);
            foreach (var dsConfigSettingsObject in this._dsConfig.SettingsObjects)
            {
                sb.Append(" " + dsConfigSettingsObject.Error);
            }

            return sb.ToString();
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return this.Name;
        }
    }
}