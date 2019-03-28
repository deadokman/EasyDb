using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EasyDb.ProjectManagment.ProjectSchema.Version
{
    public class Version_1_0 : IVersion
    {
        public void Apply(XmlDocument doc)
        {
            throw new NotImplementedException();
        }

        public double Number { get; }
        public int Order { get; }
    }
}
