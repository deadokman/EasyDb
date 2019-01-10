using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EDb.Interfaces;

namespace EasyDb.Postgres.Options
{
    /// <summary>
    /// Основные настройки для работы с Postgres
    /// </summary>
    public class GeneralOption : EdbSourceOption
    {
        public GeneralOption()
        {
            this.OptionsDefinitionName = "General";
        }

        [OptionDisplayName("opt_def_hostname", "Hostname:")]
        public string Host { get; set; }

        [OptionDisplayName("opt_def_database", "Database:")]
        public string Database { get; set; }

        [OptionDisplayName("opt_def_user", "User:")]
        public string User { get; set; }
    }
}
