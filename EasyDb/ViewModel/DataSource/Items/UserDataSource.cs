using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Xml.Serialization;
using EasyDb.Annotations;
using EDb.Interfaces;

namespace EasyDb.ViewModel.DataSource.Items
{
    /// <summary>
    /// Presents datasource created by user
    /// </summary>
    public class UserDataSource :  ValidationViewModelBase
    {
        private EdbSourceOption[] _settingsObjects;
        private EdbSourceOption _selectedDataSourceOption;

        /// <summary>
        /// Драйвер СУБД
        /// </summary>
        public IEdbDatasourceModule LinkedEdbSourceModule { get; set; }

        private UserDatasourceConfiguration _dsConfig;

        public UserDataSource()
        {
            _dsConfig = new UserDatasourceConfiguration();
        }

        /// <summary>
        /// Установить Guid модуля
        /// </summary>
        /// <param name="moduleGuid"></param>
        public void SetGuid(Guid moduleGuid)
        {
            _dsConfig.ModuleGuid = moduleGuid;
        }

        public Guid DatasourceGuid
        {
            get => _dsConfig.ModuleGuid;
        }

        /// <summary>
        /// User datasource tab settings
        /// </summary>
        public EdbSourceOption[] SettingsObjects
        {
            get => _settingsObjects;
            set
            {
                _settingsObjects = value;
                OnPropertyChanged();
            }
        }
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
        /// User datasource comment
        /// </summary>
        public string Comment
        {
            get => _dsConfig.Comment;
            set { _dsConfig.Comment = value; }
        }

        /// <summary>
        /// Applies settings to the datasource
        /// </summary>
        [XmlIgnore]
        public ICommand ApplySettingsCommand { get; set; }

        /// <summary>
        /// Command to test Connection
        /// </summary>
        [XmlIgnore]
        public ICommand TestConnection { get; set; }

    }
}
