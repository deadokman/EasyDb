using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using EasyDb.Annotations;
using EDb.Interfaces;

namespace EasyDb.ViewModel.DataSource.Items
{
    /// <summary>
    /// Presents datasource created by user
    /// </summary>
    public class UserDataSource : INotifyPropertyChanged
    {
        [XmlIgnore]
        private EdbSourceOption[] _settingsObjects;

        public Guid DatasourceGuid { get; set; }

        [XmlIgnore]
        public IEdbDatasourceModule LinkedEdbSourceModule { get; set; }

        public UserDataSource()
        {
            
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

        public string DatasourceName { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
