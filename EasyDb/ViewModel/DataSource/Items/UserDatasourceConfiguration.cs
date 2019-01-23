using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EDb.Interfaces;

namespace EasyDb.ViewModel.DataSource.Items
{
    /// <summary>
    /// Настройки источника данных пользователя
    /// </summary>
    public class UserDatasourceConfiguration
    {

        /// <summary>
        /// Идентификатор записи источника данных
        /// </summary>
        public Guid ModuleGuid { get; set; }

        /// <summary>
        /// Имя источника данных
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Комментарий к источнику данных
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Общие настройки источника данных
        /// </summary>
        public EdbSourceOption[] SettingsObjects { get; set; }
    }
}
