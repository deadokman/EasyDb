namespace EasyDb.Model
{
    using System;

    using EDb.Interfaces;

    /// <summary>
    /// Настройки источника данных пользователя
    /// </summary>
    public class UserDatasourceConfiguration
    {
        /// <summary>
        /// Identifier of current configuration instance
        /// </summary>
        public Guid ConfigurationGuid { get; set; }

        /// <summary>
        /// Gets or sets the Comment
        /// Комментарий к источнику данных
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets the ModuleGuid
        /// Идентификатор записи источника данных
        /// </summary>
        public Guid ModuleGuid { get; set; }

        /// <summary>
        /// Gets or sets the Name
        /// Имя источника данных
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the SettingsObjects
        /// Общие настройки источника данных
        /// </summary>
        public EdbSourceOption[] SettingsObjects { get; set; }
    }
}