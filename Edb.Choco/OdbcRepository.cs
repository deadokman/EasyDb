namespace Edb.Environment
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Runtime.ExceptionServices;
    using System.Threading;

    using Edb.Environment.Interface;
    using Edb.Environment.Model;

    using Microsoft.Win32;

    /// <summary>
    /// Represents ODBC derivers repository inside the WINDOWS system
    /// </summary>
    public class OdbcRepository : IOdbcRepository
    {
        private const string BaseOdbcPath = @"SOFTWARE\ODBC\ODBCINST.INI";
        private const string OdbcDriversCatalog = "ODBC Drivers";

        public IEnumerable<OdbcDriver> ListOdbcDrivers()
        {
            var odbcDriversCatalog = new Dictionary<string, OdbcDriver>();
            var driversListPath = Path.Combine(BaseOdbcPath, OdbcDriversCatalog);
            using (var odbcDriverCatalogSubkey = Registry.LocalMachine.OpenSubKey(driversListPath))
            {
                foreach (var driverName in odbcDriverCatalogSubkey.GetValueNames())
                {
                    var driverKey = Registry.LocalMachine.OpenSubKey(Path.Combine(BaseOdbcPath, driverName));
                    var driverValues = driverKey.GetValueNames().Select(vn => new { valueName = vn, value = driverKey.GetValue(vn) })
                        .ToDictionary(kp => kp.valueName, kp => kp.value);
                    object driverDllPath;
                    if (driverValues.TryGetValue("Driver", out driverDllPath))
                    {
                        var driver = driverValues.TryGetDriver();
                        driver.DriverName = driverName;
                        odbcDriversCatalog[driverDllPath.ToString()] = driver;
                    }
                }
            }

            return odbcDriversCatalog.Values;
        }
    }
}
