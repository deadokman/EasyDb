using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using EasyDb.Annotations;
using EDb.Interfaces;

namespace EasyDb.ViewModel.DataSource.Items
{
    /// <summary>
    /// Presents datasource created by user
    /// </summary>
    public class UserDefinedDataSource : INotifyPropertyChanged
    {
        private string _name;
        private string _hostname;
        private int _port;
        private string _userName;

        public Guid DatasourceGuid { get; set; }

        public IEasyDbDataSource LinkedEdbDatasource { get; set; }

        public UserDefinedDataSource()
        {
            
        }

        /// <summary>
        /// Имя текущего подключения к источнику данных
        /// </summary>
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public string Hostname
        {
            get => _hostname;
            set
            {
                _hostname = value;
                OnPropertyChanged();
            }
        }

        public int Port
        {
            get => _port;
            set
            {
                _port = value;
                OnPropertyChanged();
            }
        }

        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
