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

        public GeneralOption()
        {
            this.OptionsDefinitionName = "General";
        }

        [Required(ErrorMessage = "Hostname required")]
        [OptionDisplayName("opt_def_hostname", "Hostname:")]
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
    }
}
