using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using EasyDb.Model;
using EDb.Interfaces;

namespace Edb.Environment.Interface
{
    /// <summary>
    /// Manage connections
    /// </summary>
    public interface IConnectionManager
    {
        /// <summary>
        /// Produce connection for datasource config
        /// </summary>
        /// <param name="datasourceConfig">User datasource confiuration</param>
        /// <param name="passwordStr">Password secure string</param>
        /// <returns></returns>
        IEDbConnectionLink ProduceDbConnection(UserDatasourceConfiguration datasourceConfig, SecureString passwordStr = null);

        /// <summary>
        /// List all avaliable connections
        /// </summary>
        /// <returns>Returns all avaliable connections</returns>
        IEnumerable<IEDbConnectionLink> ListConnections();

        /// <summary>
        /// Closing connection for user datasource for id
        /// </summary>
        /// <param name="userDatasourceId">Identifier of userdatasource configuration</param>
        void CloseConnectionForSource(Guid userDatasourceId);

    }
}
