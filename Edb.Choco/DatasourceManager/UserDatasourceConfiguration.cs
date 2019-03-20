namespace EasyDb.Model
{
    using System;
    using System.Xml.Serialization;

    using EDb.Interfaces;
    using EDb.Interfaces.Options;

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
        /// Gets or sets the DatasoureGuid
        /// Идентификатор записи источника данных
        /// </summary>
        public Guid DatasoureGuid { get; set; }

        /// <summary>
        /// Gets or sets the Name
        /// Имя источника данных
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the OptionsObjects
        /// Общие настройки источника данных
        /// </summary>
        public EdbSourceOption[] OptionsObjects { get; set; }
    }
}