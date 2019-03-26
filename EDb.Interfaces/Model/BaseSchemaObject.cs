using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDb.Interfaces.Model
{
    /// <summary>
    /// Represents base class for database schema object
    /// </summary>
    public abstract class BaseSchemaObject
    {
        /// <summary>
        /// Object name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Object type
        /// </summary>
        public ObjectType Type { get; set; }
    }
}
