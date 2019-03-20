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
        /// <param name="userDatasourceConfigurations">Collection of configured datasources</param>
        public DatasourcesIniaialized(IEnumerable<SupportedSourceItem> supportedSources, IEnumerable<UserDatasourceConfiguration> userDatasourceConfigurations)
        {
            SupportedSources = supportedSources;
            UserDatasourceConfigurations = userDatasourceConfigurations;
        }

        /// <summary>
        /// Supported source items
        /// </summary>
        public IEnumerable<SupportedSourceItem> SupportedSources { get; set; }

        /// <summary>
        /// User datasource configurations
        /// </summary>
        public IEnumerable<UserDatasourceConfiguration> UserDatasourceConfigurations { get; set; }

    }
}
