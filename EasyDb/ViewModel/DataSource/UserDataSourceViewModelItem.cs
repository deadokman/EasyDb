using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using EasyDb.Model;
using EDb.Interfaces;

namespace EasyDb.ViewModel.DataSource
{
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
        public UserDataSourceViewModelItem(UserDatasourceConfiguration udsConfig, IEdbDataSource module)
        {
            _dsConfig = udsConfig;
        }

        /// <summary>
        /// Returns datasource configuration model
        /// </summary>
        public UserDatasourceConfiguration DsConfiguration => _dsConfig;

        /// <summary>
        /// Gets or sets the Comment
        /// User datasource comment
        /// </summary>
        public string Comment
        {
            get => _dsConfig.Comment;
            set
            {
                _dsConfig.Comment = value;
            }
        }

        /// <summary>
        /// Gets the DatasourceGuid
        /// </summary>
        public Guid DatasourceGuid
        {
            get => _dsConfig.DatasoureGuid;
        }

        /// <summary>
        /// Gets or sets the LinkedEdbDataSource
        /// Драйвер СУБД
        /// </summary>
        public IEdbDataSource LinkedEdbDataSource { get; set; }

        /// <summary>
        /// Gets or sets the Name
        /// User datasource name
        /// </summary>
        [Required(ErrorMessageResourceName = "dsms_ex_datasourceName")]
        public string Name
        {
            get => _dsConfig.Name;
            set
            {
                _dsConfig.Name = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the SelectedDataSourceOption
        /// </summary>
        public EdbSourceOption SelectedDataSourceOption
        {
            get => _selectedDataSourceOption;
            set
            {
                _selectedDataSourceOption = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the OptionsObjects
        /// User datasource tab settings
        /// </summary>
        public EdbSourceOption[] SettingsObjects
        {
            get => _dsConfig.OptionsObjects;
        }

        /// <summary>
        /// Установить Guid модуля
        /// </summary>
        /// <param name="moduleGuid">Set datasource guid ID</param>
        public void SetGuid(Guid moduleGuid)
        {
            _dsConfig.DatasoureGuid = moduleGuid;
        }

        /// <summary>
        /// Validate all source options
        /// </summary>
        /// <returns>Returns true if all options is valid</returns>
        public bool AllValid()
        {
            if (_dsConfig.OptionsObjects != null)
            {
                return _dsConfig.OptionsObjects.All(opt => string.IsNullOrEmpty(opt.Error)) && IsValid;
            }

            return IsValid;
        }

        /// <summary>
        /// Get options erros
        /// </summary>
        /// <returns>Errors</returns>
        public string GetErrors()
        {
            var sb = new StringBuilder();
            sb.Append(" " + Error);
            foreach (var dsConfigSettingsObject in _dsConfig.OptionsObjects)
            {
                sb.Append(" " + dsConfigSettingsObject.Error);
            }

            return sb.ToString();
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return Name;
        }
    }
}