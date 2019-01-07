using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDb.Interfaces
{
    public class EdbDatasourceAttribute : Attribute
    {
        public EdbDatasourceAttribute(string guid)
        {
            SourceGuid = new Guid(guid);
        }

        public Guid SourceGuid { get; private set; }
    }
}
