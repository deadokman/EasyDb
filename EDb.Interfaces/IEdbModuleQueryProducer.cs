using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDb.Interfaces
{
    /// <summary>
    /// Produces querys for datasource module
    /// </summary>
    public interface IEdbModuleQueryProducer
    {
        /// <summary>
        /// Returns connection test query for datasource module
        /// </summary>
        /// <returns>Returns connection test query for datasource module</returns>
        string GetTestConnectionQuery();
    }
}
