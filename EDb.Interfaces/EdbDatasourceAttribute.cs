using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDb.Interfaces
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class EdbDatasourceAttribute : Attribute
    {
        public EdbDatasourceAttribute(string guid, string version)
        {
            SourceGuid = new Guid(guid);
            Version = new Version(version);
        }

        public Guid SourceGuid { get; private set; }

        public Version Version { get; set; }
    }
}
