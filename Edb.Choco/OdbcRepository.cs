namespace Edb.Environment
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;

    using Edb.Environment.Interface;
    using Edb.Environment.Model;

    using Microsoft.Win32;

    /// <summary>
    /// Represents ODBC derivers repository inside the WINDOWS system
    /// </summary>
    public class OdbcRepository : IOdbcRepository
    {
        public IEnumerable<OdbcDriver> ListOdbcDrivers()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
            var odbcDrivers = new List<OdbcDriver>();
            var reg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\ODBC\ODBCINST.INI");

            if (reg != null)
            {
                // Iterate over all subkeys
                foreach (var subKeyName in reg.GetSubKeyNames())
                {
                    
                }

                reg.GetSubKeyNames();

            }
            try
            {
                reg.Close();
            }
            catch { /* ignore this exception if we couldn't close */ }

            return odbcDrivers;
        }
    }
}
