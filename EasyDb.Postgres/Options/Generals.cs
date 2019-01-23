using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using EDb.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace EasyDb.Postgres.Options
{
    /// <summary>
    /// Основные настройки для работы с Postgres
    /// </summary>
    public class GeneralOption : EdbSourceOption
    {
        private string _host;
        private string _database;
        private string _user;
        private int _port;

        public GeneralOption()
        {
            this.OptionsDefinitionName = "General";
            _port = 5432;
        }

        [Required(ErrorMessage = "Hostname required")]
        [OptionDisplayName("opt_def_hostname", "Hostname:")]
        [RegularExpression(@"^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])|((([a-zA-Z0-9]|[a-zA-Z0-9][a-zA-Z0-9\-]*[a-zA-Z0-9])\.)*([A-Za-z0-9]|[A-Za-z0-9][A-Za-z0-9\-]*[A-Za-z0-9]))$",
            ErrorMessage = "Should be IP or valid hostname")]
        public string Host
        {
            get => _host;
            set
            {
                _host = value;
                OnPropertyChanged();
            }
        }

        [Required(ErrorMessage = "Database name required")]
        [OptionDisplayName("opt_def_database", "Database:")]
        public string Database
        {
            get => _database;
            set
            {
                _database = value;
                OnPropertyChanged();
            }
        }

        [Required(ErrorMessage = "Username required")]
        [OptionDisplayName("opt_def_user", "User:")]
        public string User
        {
            get => _user;
            set
            {
                _user = value;
                OnPropertyChanged();
            }
        }

        [Required(ErrorMessage = "Port required")]
        [OptionDisplayName("opt_def_port", "Port:")]
        [RegularExpression("^[0-9]{1,7}$")]
        public int Port
        {
            get => _port;
            set
            {
                _port = value;
                OnPropertyChanged();
            }
        }
    }
}
