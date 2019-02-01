using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDb.Interfaces.Options
{
    /// <summary>
    /// Class describes defenition of single module option
    /// </summary>
    public class ModuleOptionDefinition
    {
        /// <summary>
        /// Name of option defenition
        /// </summary>
        public string DefinitionName { get; set; }

        /// <summary>
        /// Property definition type name
        /// </summary>
        public string PropertyDefinitionType { get; set; }

        /// <summary>
        /// Properties collection
        /// </summary>
        public OptionProperty[] Properties { get; set; }
    }
}
