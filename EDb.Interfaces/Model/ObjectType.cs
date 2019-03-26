using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDb.Interfaces.Model
{
    /// <summary>
    /// Represents supported object type
    /// </summary>
    public class ObjectType
    {
        /// <summary>
        /// Object type name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Object desctiption
        /// </summary>
        public string Description { get; set; }
    }
}
