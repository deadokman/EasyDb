using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edb.Environment.Interface
{
    using Edb.Environment.Model;

    /// <summary>
    /// Represents ODBC derivers repository inside the WINDOWS system
    /// </summary>
    public interface IOdbcManager
    {
        /// <summary>
        /// List odbc drivers
        /// </summary>
        /// <returns>ODBC drivers list</returns>
        IEnumerable<OdbcDriver> ListOdbcDrivers();

        /// <summary>
        /// Checks that ODBC driver installed
        /// </summary>
        /// <param name="driverName">System driver name</param>
        /// <param name="driver">Driver instance</param>
        /// <returns>returns true if ODBC driver installed</returns>
        bool OdbcDriverInstalled(string driverName, out OdbcDriver driver);

        /// <summary>
        /// Adds new DSN connection
        /// </summary>
        /// <param name="dsnName">DSN name</param>
        /// <param name="description">DSN description</param>
        /// <param name="server">Server hostname</param>
        /// <param name="driverName">ODBC driver name</param>
        /// <param name="trustedConnection">is trusted connection</param>
        /// <param name="database">Database name</param>
        void AddDSN(string dsnName, string description, string server, int port, string driverName, bool trustedConnection, string database);

        /// <summary>
        /// Remove DSN connection
        /// </summary>
        /// <param name="dsnName">DSN name</param>
        void RemoveDSN(string dsnName);

        /// <summary>
        /// Returns true if DSN exists
        /// </summary>
        /// <param name="dsnName">DNS name</param>
        /// <returns></returns>
        bool DSNExists(string dsnName);

        /// <summary>
        /// Refresh driver catalogue
        /// </summary>
        void RefreshDriversCatalog();
    }
}
