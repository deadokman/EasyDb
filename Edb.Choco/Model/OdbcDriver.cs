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
    using System;

    /// <summary>
    /// Represents standart ODBC driver model
    /// </summary>
    public class OdbcDriver
    {
        /// <summary>
        /// Creates new instance of ODBC driver
        /// </summary>
        /// <param name="driverName">Driver name</param>
        public OdbcDriver()
        {
        }

        /// <summary>
        /// Odbc driver name
        /// </summary>
        public string DriverName { get; set; }

        /// <summary>
        /// Path to driver dll
        /// </summary>
        public string DriverDllPath { get; set; }

        /// <summary>
        /// Driver version
        /// </summary>
        public Version DriverVersion { get; set; }
    }
}
