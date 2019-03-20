// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConnectionManager.cs" company="SimpleExample">
//   SimpleExample
// </copyright>
// <summary>
//   Connections manager
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Edb.Environment.Connection
{
    using System;
    using System.Collections.Generic;
    using System.Data.Odbc;
    using System.Diagnostics.CodeAnalysis;

    using EasyDb.Model;

    using Edb.Environment.Interface;
    using Edb.Environment.Model;

    using EDb.Interfaces.Annotations;

    /// <summary>
    /// Connections manager
    /// </summary>
    public class ConnectionManager : IConnectionManager
    {
        /// <summary>
        /// The _datasource manager.
        /// </summary>
        private readonly IDataSourceManager _datasourceManager;

        /// <summary>
        /// The _connection links.
        /// </summary>
        private readonly Dictionary<Guid, IEDbConnectionLink> _connectionLinks;

        /// <summary>
        /// Creates 
        /// </summary>
        /// <param name="datasourceManager">Datasource manager</param>
        public ConnectionManager(IDataSourceManager datasourceManager)
        {
            this._datasourceManager = datasourceManager;
            _connectionLinks = new Dictionary<Guid, IEDbConnectionLink>();
        }

        /// <summary>
        /// Produce connection for datasource config
        /// </summary>
        /// <param name="datasourceConfig">User datasource confiuration</param>
        /// <returns></returns>
        public IEDbConnectionLink ProduceDbConnection([NotNull] UserDatasourceConfiguration datasourceConfig)
        {
            if (datasourceConfig == null)
            {
                throw new ArgumentNullException(nameof(datasourceConfig));
            }

            IEDbConnectionLink connectionLink;
            if (!_connectionLinks.TryGetValue(datasourceConfig.ConfigurationGuid, out connectionLink))
            {
                var module = _datasourceManager.GetModuleByGuid(datasourceConfig.DatasoureGuid);
                var connectionString = module.IntroduceConnectionString(datasourceConfig.OptionsObjects);
                var connection = new OdbcConnection(connectionString);
                connectionLink =  new EDbConnectionLink(datasourceConfig, connection);
                _connectionLinks.Add(datasourceConfig.ConfigurationGuid, connectionLink);
            }

            return connectionLink;
        }

        /// <summary>
        /// List all avaliable connections
        /// </summary>
        /// <returns>Returns all avaliable connections</returns>
        public IEnumerable<IEDbConnectionLink> ListConnections()
        {
            return _connectionLinks.Values;
        }

        public void CloseConnectionForSource(Guid userDatasourceId)
        {
            throw new NotImplementedException();
        }
    }
}
