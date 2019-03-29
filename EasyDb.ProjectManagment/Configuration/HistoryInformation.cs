using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyDb.ProjectManagment.Configuration
{
    /// <summary>
    /// Hold overall information about projects configurations
    /// </summary>
    public class HistoryInformation
    {
        public List<ProjectHistItem> ProjectsHistory { get; set; }
    }
}
