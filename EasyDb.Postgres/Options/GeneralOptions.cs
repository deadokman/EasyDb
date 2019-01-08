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

        [Description]
        public string Host { get; set; }

        [Description]
        public string Database { get; set; }

        [Description]
        public string User { get; set; }
    }
}
