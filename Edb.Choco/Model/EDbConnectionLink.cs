using System;
using System.Data;

using EasyDb.Model;
using Edb.Environment.Interface;
using EDb.Interfaces.Annotations;

namespace Edb.Environment.Model
{
    public class EDbConnectionLink : IEDbConnectionLink
    {
        private readonly UserDatasourceConfiguration _configuration;
        private readonly IDbConnection _dbConnectionInstance;

        public EDbConnectionLink([NotNull] UserDatasourceConfiguration configuration, [NotNull] IDbConnection dbConnectionInstance)
        {
            this._configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this._dbConnectionInstance = dbConnectionInstance ?? throw new ArgumentNullException(nameof(dbConnectionInstance));
        }

        public void Dispose()
        {
            _dbConnectionInstance.Dispose();
        }

        /// <summary>
        /// Reference to real database connection
        /// </summary>
        public IDbConnection UnderlyingConnection { get; private set; }

        /// <summary>
        /// Edb datasource GUID
        /// </summary>
        public Guid DatasourceGuid => this.ConnectionConfiguration.DatasoureGuid;

        /// <summary>
        /// Connection config object
        /// </summary>
        public UserDatasourceConfiguration ConnectionConfiguration => _configuration;

        /// <summary>
        /// Validates that connection active
        /// </summary>
        /// <returns>Returns true if connection active</returns>
        public bool ConnectionActive()
        {
            throw new NotImplementedException();
        }

        /// <summary>Begins a database transaction.</summary>
        /// <returns>An object representing the new transaction.</returns>
        public IDbTransaction BeginTransaction()
        {
            return this._dbConnectionInstance.BeginTransaction();
        }

        /// <summary>Begins a database transaction with the specified <see cref="T:System.Data.IsolationLevel" /> value.</summary>
        /// <param name="il">One of the <see cref="T:System.Data.IsolationLevel" /> values. </param>
        /// <returns>An object representing the new transaction.</returns>
        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            return this._dbConnectionInstance.BeginTransaction(il);
        }

        /// <summary>Closes the connection to the database.</summary>
        public void Close()
        {
            this._dbConnectionInstance.Close();
        }

        /// <summary>Changes the current database for an open <see langword="Connection" /> object.</summary>
        /// <param name="databaseName">The name of the database to use in place of the current database. </param>
        public void ChangeDatabase(string databaseName)
        {
            this._dbConnectionInstance.ChangeDatabase(databaseName);
        }

        /// <summary>Creates and returns a Command object associated with the connection.</summary>
        /// <returns>A Command object associated with the connection.</returns>
        public IDbCommand CreateCommand()
        {
            throw new NotImplementedException();
        }

        /// <summary>Opens a database connection with the settings specified by the <see langword="ConnectionString" /> property of the provider-specific Connection object.</summary>
        public void Open()
        {
            this._dbConnectionInstance.Open();
        }

        /// <summary>Gets or sets the string used to open a database.</summary>
        /// <returns>A string containing connection settings.</returns>
        public string ConnectionString
        {
            get => this._dbConnectionInstance.ConnectionString;
            set => this._dbConnectionInstance.ConnectionString = value;
        }

        /// <summary>Gets the time to wait while trying to establish a connection before terminating the attempt and generating an error.</summary>
        /// <returns>The time (in seconds) to wait for a connection to open. The default value is 15 seconds.</returns>
        public int ConnectionTimeout
        {
            get => this._dbConnectionInstance.ConnectionTimeout;
        }

        /// <summary>Gets the name of the current database or the database to be used after a connection is opened.</summary>
        /// <returns>The name of the current database or the name of the database to be used once a connection is open. The default value is an empty string.</returns>
        public string Database
        {
            get => this._dbConnectionInstance.Database;
        }

        /// <summary>Gets the current state of the connection.</summary>
        /// <returns>One of the <see cref="T:System.Data.ConnectionState" /> values.</returns>
        public ConnectionState State
        {
            get => this._dbConnectionInstance.State;
        }
    }
}
