using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyDb.Model;
using Edb.Environment.DatasourceManager;

namespace Edb.Environment.CommunicationArgs
{
    /// <summary>
    /// Raises by datasource manager when avaliable supported sources or user datasources changed
    /// </summary>
    public class DatasourcesIniaialized
    {
        /// <summary>
        /// Datasource initialized communication message
        /// </summary>
        /// <param name="supportedSources">Collection of supported datasources</param>
        public DatasourcesIniaialized(IEnumerable<SupportedSourceItem> supportedSources)
        {
            SupportedSources = supportedSources;
        }

        /// <summary>
        /// Supported source items
        /// </summary>
        public IEnumerable<SupportedSourceItem> SupportedSources { get; set; }
    }
}
