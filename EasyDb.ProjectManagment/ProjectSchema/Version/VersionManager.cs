using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace EasyDb.ProjectManagment.ProjectSchema.Version
{
    internal class VersionManager
    {
        private void CheckVersion(XmlDocument doc, string fileName)
        {
            var ase = Assembly.GetExecutingAssembly();
            var versions = ase.GetTypes().Where(x => typeof(IVersion).IsAssignableFrom(x)
                                                     && x.GetConstructor(Type.EmptyTypes) != null)
                .Select(x => Activator.CreateInstance(x) as IVersion)
                .OrderBy(x => x.Order)
                .ToList();

            if (versions.Count == 0)
            {
                return;
            }

            double currentVersion = 1.0;
            var ins = doc.SelectSingleNode("Instance");
            if (ins != null && ins.Attributes != null)
            {
                var attribute = ins.Attributes["version"];
                if (attribute != null)
                {
                    currentVersion = Convert.ToDouble(attribute.Value, CultureInfo.InvariantCulture);
                }
                else
                {
                    attribute = doc.CreateAttribute("version");
                    attribute.Value = currentVersion.ToString(CultureInfo.InvariantCulture);
                    ins.Attributes.Append(attribute);
                    doc.Save(fileName);
                }

                foreach (var version in versions.Where(x => x.Number > currentVersion))
                {
                    version.Apply(doc);
                    attribute.Value = version.Number.ToString(CultureInfo.InvariantCulture);
                    doc.Save(fileName);
                }
            }
        }
    }
}
