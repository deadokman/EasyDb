using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EDb.Interfaces;

namespace EasyDb.ViewModel.DataSource
{
    /// <summary>
    /// Implements logic for datasource creation control
    /// </summary>
    public class DatasourceViewModel : IDatasourceControlViewModel
    {
        public IEnumerable<IEasyDbDataSource> SupportedDatasources { get; }
    }
}
