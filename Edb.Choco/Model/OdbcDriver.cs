// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OdbcDriver.cs" company="">
//   
// </copyright>
// <summary>
//   Represents standart ODBC driver model
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Edb.Environment.Model
{
    /// <summary>
    /// Represents standart ODBC driver model
    /// </summary>
    public class OdbcDriver
    {
        /// <summary>
        /// Creates new instance of ODBC driver
        /// </summary>
        /// <param name="driverName">Driver name</param>
        public OdbcDriver(string driverName)
        {
            this.DriverName = driverName;
        }

        /// <summary>
        /// Odbc driver name
        /// </summary>
        public string DriverName { get; set; }
    }
}
