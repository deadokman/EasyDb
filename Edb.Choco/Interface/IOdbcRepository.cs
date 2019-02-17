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
    }
}
