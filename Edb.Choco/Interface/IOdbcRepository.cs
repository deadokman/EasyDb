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
    public interface IOdbcRepository
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
        /// <returns>returns true if ODBC driver installed</returns>
        bool OdbcDriverInstalled(string driverName);
    }
}
