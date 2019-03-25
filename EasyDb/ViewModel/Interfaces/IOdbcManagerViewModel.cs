using System.Collections.Generic;

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
