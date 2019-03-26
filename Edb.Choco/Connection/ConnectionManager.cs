// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConnectionManager.cs" company="SimpleExample">
//   SimpleExample
// </copyright>
// <summary>
//   Connections manager
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Security;
using chocolatey;

namespace Edb.Environment.Connection
{
    using System;
    using System.Collections.Generic;
    using System.Data.Odbc;
    using EasyDb.Model;

    using Interface;
    using Model;

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

        private readonly IPasswordStorage _passwordStorage;

        /// <summary>
        /// The _connection links.
        /// </summary>
        private readonly Dictionary<Guid, IEDbConnectionLink> _connectionLinks;

        /// <summary>
        /// Creates 
        /// </summary>
        /// <param name="datasourceManager">Datasource manager</param>
        public ConnectionManager(IDataSourceManager datasourceManager, IPasswordStorage passwordStorage)
        {
            _datasourceManager = datasourceManager ?? throw new ArgumentNullException(nameof(datasourceManager));
            _passwordStorage = passwordStorage ?? throw new ArgumentNullException(nameof(passwordStorage));
            _connectionLinks = new Dictionary<Guid, IEDbConnectionLink>();
        }

        /// <summary>
        /// Produce connection for datasource config
        /// </summary>
        /// <param name="datasourceConfig">User datasource confiuration</param>
        /// <param name="passwordStr">Password secure string</param>
        /// <returns></returns>
        public IEDbConnectionLink ProduceDbConnection([NotNull] UserDatasourceConfiguration datasourceConfig, SecureString passwordStr = null)
        {
            if (datasourceConfig == null)
            {
                throw new ArgumentNullException(nameof(datasourceConfig));
            }

            IEDbConnectionLink connectionLink;
            if (!_connectionLinks.TryGetValue(datasourceConfig.ConfigurationGuid, out connectionLink))
            {
                var module = _datasourceManager.GetModuleByGuid(datasourceConfig.DatasoureGuid);

                // restore password from storage
                if (passwordStr == null && !_passwordStorage.TryGetPluginPassword(out passwordStr, datasourceConfig.ConfigurationGuid))
                {
                    throw new Exception("Password does not supplied");
                }

                string passwordTag;
                var connectionString = module.IntroduceConnectionString(datasourceConfig.OptionsObjects, out passwordTag);
                connectionString = connectionString.Replace(passwordTag, passwordStr.SecureStringToString());
                var connection = new OdbcConnection(connectionString);
                connectionLink = new EDbConnectionLink(datasourceConfig, connection);
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

        /// <summary>
        /// Closing connection for user datasource for id
        /// </summary>
        /// <param name="configurationSourceGuid">Identifier of userdatasource configuration</param>
        public void RemoveConnectionFromSource(Guid configurationSourceGuid)
        {
            _connectionLinks.Remove(configurationSourceGuid);
        }
    }
}
