using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edb.Environment
{
    using Edb.Environment.Model;

    public static class SupportExtensions
    {
        public static OdbcDriver TryGetDriver(this IDictionary<string, object> collection) 
        {
            var driver = new OdbcDriver();
            object dllpath;
            if (collection.TryGetValue("Driver", out dllpath))
            {
                driver.DriverDllPath = dllpath.ToString();
            }

            object version;
            if (collection.TryGetValue("DriverODBCVer", out version))
            {
                driver.DriverVersion = new Version(version.ToString());
            }

            return driver;
        } 
    }
}
