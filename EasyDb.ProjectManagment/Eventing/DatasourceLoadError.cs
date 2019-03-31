using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EasyDb.ProjectManagment.Eventing
{
    /// <summary>
    /// The project load error event args.
    /// </summary>
    public class DatasourceLoadError : EventArgs
    {
        public DatasourceLoadError()
        {
            Mrs = new ManualResetEventSlim();
        }

        /// <summary>
        /// Datasource load error
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Skip error and continue loading
        /// </summary>
        public bool SkipAndStay { get; set; }

        /// <summary>
        /// Skip for all errors
        /// </summary>
        public bool SkipForall { get; set; }

        /// <summary>
        /// Sync awaiter
        /// </summary>
        public ManualResetEventSlim Mrs { get; }
    }
}
