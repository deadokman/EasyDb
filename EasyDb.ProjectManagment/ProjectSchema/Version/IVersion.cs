using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EasyDb.ProjectManagment.ProjectSchema.Version
{
    public interface IVersion
    {
        void Apply(XmlDocument doc);

        double Number { get; }

        int Order { get; }
    }
}
