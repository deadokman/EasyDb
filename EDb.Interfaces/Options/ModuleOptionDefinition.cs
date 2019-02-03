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
    [Serializable]
    public class ModuleOptionDefinition : MarshalByRefObject
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

        /// <summary>
        /// Converts option definition to object
        /// </summary>
        /// <typeparam name="T"> Option class type </typeparam>
        /// <returns>Option class type</returns>
        public T ToOptionObject<T>()
            where T : EdbSourceOption
        {
            return default(T);
        }
    }
}
