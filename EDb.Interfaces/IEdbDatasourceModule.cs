using System;
using System.Data;
using System.Drawing;
using System.Windows.Media;
using EDb.Interfaces.Objects;

namespace EDb.Interfaces
{
    public interface IEdbDatasourceModule
    {
        /// <summary>
        /// Имя модуля СУБД
        /// </summary>
        string DatabaseName { get; }

        /// <summary>
        /// Типы поддерживаемых объектов базы данных
        /// </summary>
        SupportedObjectTypes[] SupportedTypes { get; }

        /// <summary>
        /// Значек базы данных
        /// </summary>
        byte[] DatabaseIcon { get; }

        /// <summary>
        /// Получить подключение к СУБД
        /// </summary>
        IDbConnection GetDatabaseConnection { get; }

        /// <summary>
        /// Установить строку подключения к базе данных
        /// </summary>
        /// <param name="connectionString">Строка подключения к базе данных</param>
        void SetConnection(string connectionString);

        /// <summary>
        /// Вернуть объекты настроек
        /// </summary>
        /// <returns></returns>
        EdbSourceOption[] GetDefaultOptionsObjects();

        /// <summary>
        /// Module Guid
        /// </summary>
        Guid ModuleGuid { get; }

        /// <summary>
        /// Set module guid
        /// </summary>
        /// <param name="guid">Guid</param>
        void SetGuid(Guid guid);

        Version Version { get; }

        /// <summary>
        /// Module version
        /// </summary>
        /// <param name="version">Version</param>
        void SetVersion(Version version);

    }
}
