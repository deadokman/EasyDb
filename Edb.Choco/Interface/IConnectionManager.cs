using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
        /// <returns></returns>
        IEdbConnectionLink ProduceDbConnection(UserDatasourceConfiguration datasourceConfig);


    }
}
