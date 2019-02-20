using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyDb.ViewModel.Interfaces
{
    using Edb.Environment.Model;

    /// <summary>
    /// ODBC drivers manger view model
    /// </summary>
    public interface IOdbcManagerViewModel
    {
        /// <summary>
        /// List Odbc drivers
        /// </summary>
        IEnumerable<OdbcDriver> OdbcDrivers { get; }
    }
}
