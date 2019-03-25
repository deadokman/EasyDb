namespace Edb.Environment
{
    using Interface;
    using Microsoft.Win32;
    using Model;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Represents ODBC derivers repository inside the WINDOWS system
    /// </summary>
    public class OdbcManager : IOdbcManager
    {
        /// <summary>
        /// Defines the OdbcIniRegPath
        /// </summary>
        private readonly string OdbcIniRegPath = Path.Combine("SOFTWARE", "ODBC", "ODBC.INI");

        /// <summary>
        /// Defines the OdbcInstallationRegPath
        /// </summary>
        private readonly string OdbcInstallationRegPath = Path.Combine("SOFTWARE", "ODBC", "ODBCINST.INI");

        /// <summary>
        /// Defines the OdbcDriversCatalog
        /// </summary>
        private const string OdbcDriversCatalog = "ODBC Drivers";

        /// <summary>
        /// Collection of ODBC drivers
        /// </summary>
        private Dictionary<string, OdbcDriver> _odbcDrivers;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcManager"/> class.
        /// </summary>
        public OdbcManager()
        {
            _odbcDrivers = new Dictionary<string, OdbcDriver>();
            RefreshDriversCatalog();
        }

        /// <summary>
        /// List odbc drivers
        /// </summary>
        /// <returns>ODBC drivers list</returns>
        public IEnumerable<OdbcDriver> ListOdbcDrivers()
        {
            return _odbcDrivers.Values;
        }

        /// <summary>
        /// Checks that ODBC driver installed
        /// </summary>
        /// <param name="driverName">System driver name</param>
        /// <param name="driver">Driver instance</param>
        /// <returns>returns true if ODBC driver installed</returns>
        public bool OdbcDriverInstalled(string driverName, out OdbcDriver driver)
        {
            return _odbcDrivers.TryGetValue(driverName, out driver);
        }

        /// <summary>
        /// Adds new DSN connection
        /// </summary>
        /// <param name="dsnName">DSN name</param>
        /// <param name="description">DSN description</param>
        /// <param name="server">Server hostname</param>
        /// <param name="port">Connection port</param>
        /// <param name="driverName">ODBC driver name</param>
        /// <param name="trustedConnection">is trusted connection</param>
        /// <param name="database">Database name</param>
        public void AddDSN(string dsnName, string description, string server, int port, string driverName, bool trustedConnection,
            string database)
        {
            RefreshDriversCatalog();

            // Lookup driver path from driver name
            var driverKey = Registry.LocalMachine.CreateSubKey(Path.Combine(OdbcInstallationRegPath, driverName));
            if (driverKey == null)
            {
                throw new Exception(string.Format("ODBC Registry key for driver '{0}' does not exist", driverName));
            }

            var driverPath = driverKey.GetValue("Driver").ToString();

            // Add value to odbc data sources
            var datasourcesKey = Registry.LocalMachine.CreateSubKey(OdbcIniRegPath + "ODBC Data Sources");
            if (datasourcesKey == null)
            {
                throw new Exception("ODBC Registry key for datasources does not exist");
            }

            datasourcesKey.SetValue(dsnName, driverName);

            // Create new key in odbc.ini with dsn name and add values
            var dsnKey = Registry.LocalMachine.CreateSubKey(OdbcIniRegPath + dsnName);
            if (dsnKey == null)
            {
                throw new Exception("ODBC Registry key for DSN was not created");
            }

            dsnKey.SetValue("Database", database);
            dsnKey.SetValue("Description", description);
            dsnKey.SetValue("Driver", driverPath);

            // dsnKey.SetValue("LastUser", Environment.UserName);
            dsnKey.SetValue("Server", server);
            dsnKey.SetValue("Database", database);
            dsnKey.SetValue("Trusted_Connection", trustedConnection ? "Yes" : "No");
        }

        /// <summary>
        /// Remove DSN connection
        /// </summary>
        /// <param name="dsnName">DSN name</param>
        public void RemoveDSN(string dsnName)
        {
            // Remove DSN key
            Registry.LocalMachine.DeleteSubKeyTree(OdbcIniRegPath + dsnName);

            // Remove DSN name from values list in ODBC Data Sources key
            var datasourcesKey = Registry.LocalMachine.CreateSubKey(OdbcIniRegPath + "ODBC Data Sources");
            if (datasourcesKey == null)
            {
                throw new Exception("ODBC Registry key for datasources does not exist");
            }

            datasourcesKey.DeleteValue(dsnName);
        }

        /// <summary>
        /// Returns true if DSN exists
        /// </summary>
        /// <param name="dsnName">DNS name</param>
        /// <returns></returns>
        public bool DSNExists(string dsnName)
        {
            var driversKey = Registry.LocalMachine.CreateSubKey(OdbcInstallationRegPath + "ODBC Drivers");
            if (driversKey == null)
            {
                throw new Exception("ODBC Registry key for drivers does not exist");
            }

            return driversKey.GetValue(dsnName) != null;
        }

        /// <summary>
        /// Refresh driver catalogue
        /// </summary>
        public void RefreshDriversCatalog()
        {
            _odbcDrivers.Clear();
            var driversListPath = Path.Combine(OdbcInstallationRegPath, OdbcDriversCatalog);
            using (var odbcDriverCatalogSubkey = Registry.LocalMachine.OpenSubKey(driversListPath))
            {
                foreach (var driverName in odbcDriverCatalogSubkey.GetValueNames())
                {
                    var driverKey = Registry.LocalMachine.OpenSubKey(Path.Combine(OdbcInstallationRegPath, driverName));
                    var driverValues = driverKey.GetValueNames().Select(vn => new { valueName = vn, value = driverKey.GetValue(vn) })
                        .ToDictionary(kp => kp.valueName, kp => kp.value);
                    object driverDllPath;
                    if (driverValues.TryGetValue("Driver", out driverDllPath))
                    {
                        var driver = driverValues.TryGetDriver();
                        driver.DriverName = driverName;
                        _odbcDrivers[driverName] = driver;
                    }
                }
            }
        }
    }
}
