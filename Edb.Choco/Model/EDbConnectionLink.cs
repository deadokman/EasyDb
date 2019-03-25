// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EDbConnectionLink.cs" company="">
//   EDbConnectionLink
// </copyright>
// <summary>
//   Defines the <see cref="EDbConnectionLink" />
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Edb.Environment.Model
{
    using System;
    using System.Data;

    using EasyDb.Model;

    using Edb.Environment.Delegates;
    using Edb.Environment.EventArgs;
    using Edb.Environment.Interface;

    using EDb.Interfaces.Annotations;

    /// <summary>
    /// Defines the <see cref="EDbConnectionLink" />
    /// </summary>
    public class EDbConnectionLink : IEDbConnectionLink
    {
        /// <summary>
        /// Defines the _configuration
        /// </summary>
        private readonly UserDatasourceConfiguration _configuration;

        /// <summary>
        /// Defines the _dbConnectionInstance
        /// </summary>
        private readonly IDbConnection _dbConnectionInstance;

        /// <summary>
        /// Initializes a new instance of the <see cref="EDbConnectionLink"/> class.
        /// </summary>
        /// <param name="configuration">The configuration<see cref="UserDatasourceConfiguration"/></param>
        /// <param name="dbConnectionInstance">The dbConnectionInstance<see cref="IDbConnection"/></param>
        public EDbConnectionLink([NotNull] UserDatasourceConfiguration configuration, [NotNull] IDbConnection dbConnectionInstance)
        {
            this._configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this._dbConnectionInstance = dbConnectionInstance ?? throw new ArgumentNullException(nameof(dbConnectionInstance));
        }

        /// <summary>
        /// The Dispose
        /// </summary>
        public void Dispose()
        {
            _dbConnectionInstance.Dispose();
        }

        /// <summary>
        /// Reises on lost connection
        /// </summary>
        public event ConnectionError ConnectionLost;

        /// <summary>
        /// Gets the UnderlyingConnection
        /// Reference to real database connection
        /// </summary>
        public IDbConnection UnderlyingConnection { get; private set; }

        /// <summary>
        /// Gets the DatasourceGuid
        /// Edb datasource GUID
        /// </summary>
        public Guid DatasourceGuid => this.ConnectionConfiguration.DatasoureGuid;

        /// <summary>
        /// Gets the ConnectionConfiguration
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

        /// <summary>
        /// The BeginTransaction
        /// </summary>
        /// <returns>An object representing the new transaction.</returns>
        public IDbTransaction BeginTransaction()
        {
            return this._dbConnectionInstance.BeginTransaction();
        }

        /// <summary>
        /// The BeginTransaction
        /// </summary>
        /// <param name="il">One of the <see cref="T:System.Data.IsolationLevel" /> values. </param>
        /// <returns>An object representing the new transaction.</returns>
        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            return this._dbConnectionInstance.BeginTransaction(il);
        }

        /// <summary>
        /// The Close
        /// </summary>
        public void Close()
        {
            this._dbConnectionInstance.Close();
        }

        /// <summary>
        /// The ChangeDatabase
        /// </summary>
        /// <param name="databaseName">The name of the database to use in place of the current database. </param>
        public void ChangeDatabase(string databaseName)
        {
            this._dbConnectionInstance.ChangeDatabase(databaseName);
        }

        /// <summary>
        /// The CreateCommand
        /// </summary>
        /// <returns>A Command object associated with the connection.</returns>
        public IDbCommand CreateCommand()
        {
            TryRestoreConnection();
            return this._dbConnectionInstance.CreateCommand();
        }


        /// <summary>
        /// The Open
        /// </summary>
        public void Open()
        {
            this._dbConnectionInstance.Open();
        }

        /// <summary>
        /// Gets or sets the ConnectionString
        /// </summary>
        public string ConnectionString { get => this._dbConnectionInstance.ConnectionString; set => this._dbConnectionInstance.ConnectionString = value; }

        /// <summary>
        /// Gets the ConnectionTimeout
        /// </summary>
        public int ConnectionTimeout { get => this._dbConnectionInstance.ConnectionTimeout; }

        /// <summary>
        /// Gets the Database
        /// </summary>
        public string Database { get => this._dbConnectionInstance.Database; }

        /// <summary>
        /// Gets the State
        /// </summary>
        public ConnectionState State { get => this._dbConnectionInstance.State; }

        private void TryRestoreConnection()
        {
            if (this._dbConnectionInstance.State == ConnectionState.Closed || this._dbConnectionInstance.State == ConnectionState.Broken)
            {
                try
                {
                    this._dbConnectionInstance.Open();
                }
                catch (Exception e)
                {
                    InvokeConnectionLost(e);
                    throw e;
                }
            }
        }

        private void InvokeConnectionLost(Exception e)
        {
            if (this.ConnectionLost != null)
            {
                ConnectionLost.Invoke(this, new ConnectionErrorEventArgs(e));
            }
        }
    }
}
