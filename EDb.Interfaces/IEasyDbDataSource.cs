using System.Data;
using System.Drawing;
using EDb.Interfaces.Objects;

namespace EDb.Interfaces
{
    public interface IEasyDbDataSource
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
        Icon DatabaseIcon { get; }

        /// <summary>
        /// Получить подключение к СУБД
        /// </summary>
        IDbConnection GetDatabaseConnection { get; }

        /// <summary>
        /// Установить строку подключения к базе данных
        /// </summary>
        /// <param name="connectionString">Строка подключения к базе данных</param>
        void SetConnection(string connectionString);
    }
}
