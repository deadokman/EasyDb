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
        [XmlIgnore]
        private EdbSourceOption[] _settingsObjects;

        [XmlIgnore]
        private EdbSourceOption _selectedDataSourceOption;

        private string _name;

        public Guid DatasourceGuid { get; set; }

        [XmlIgnore]
        public IEdbDatasourceModule LinkedEdbSourceModule { get; set; }

        public UserDataSource()
        {
            
        }

        /// <summary>
        /// User datasource tab settings
        /// </summary>
        [XmlIgnore]
        public EdbSourceOption[] SettingsObjects
        {
            get => _settingsObjects;
            set
            {
                _settingsObjects = value;
                OnPropertyChanged();
            }
        }

        [XmlIgnore]
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
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// User datasource comment
        /// </summary>
        public string Comment { get; set; }

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
