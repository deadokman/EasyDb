using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyDb.ProjectManagment
{
    /// <summary>
    /// Project load result
    /// </summary>
    public class ProjectLoadResult
    {
        public bool ProjectFileLocatedSuccessfully { get; set; }

        public bool LoadSuccess { get; set; }

        public string ExceptionMessage { get; set; }
    }
}
