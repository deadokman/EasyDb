using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyDb.ProjectManagment.Intefraces
{
    /// <summary>
    /// Working environment of applization
    /// </summary>
    public interface IApplicationEnvironment
    {
        /// <summary>
        /// Initialize proje
        /// </summary>
        /// <param name="datasourceLibPath">path to datasource drivers libs</param>
        /// <param name="applicationConfigurationPath">Local appforder path</param>
        void Initialize(string datasourceLibPath, string applicationConfigurationPath);
    }
}
