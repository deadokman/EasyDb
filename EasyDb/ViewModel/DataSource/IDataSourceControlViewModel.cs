using System.Collections.Generic;
using EDb.Interfaces;

namespace EasyDb.ViewModel.DataSource
{
    /// <summary>
    /// Interface for Data source control view
    /// </summary>
    public interface IDatasourceControlViewModel
    {
        /// <summary>
        /// Get collection of supported datasources
        /// </summary>
        IEnumerable<IEasyDbDataSource> SupportedDatasources { get; }

        
    }
}
