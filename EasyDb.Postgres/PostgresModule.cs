using System;
using System.Data;
using System.Drawing;
using EDb.Interfaces;
using EDb.Interfaces.Objects;

namespace EasyDb.Postgres
{
    /// <summary>
    /// Модуль базы данных Postgres
    /// </summary>
    public class PostgresModule : IEasyDbModule
    {
        /// <summary>
        /// Имя базы данных
        /// </summary>
        public string DatabaseName { get; }

        /// <summary>
        /// Поддерживаемые типы объектов базы данных
        /// </summary>
        public SupportedObjectTypes[] SupportedTypes { get; }

        /// <summary>
        /// Иконка базы
        /// </summary>
        public Icon DatabaseIcon { get; }

        /// <summary>
        /// Получить подключение к базе данных
        /// </summary>
        public IDbConnection GetDatabaseConnection { get; }

        /// <summary>
        /// Установить строку подключения
        /// </summary>
        /// <param name="connectionString">Строка подключения</param> 
        public void SetConnection(string connectionString)
        {
            throw new NotImplementedException();
        }
    }
}
