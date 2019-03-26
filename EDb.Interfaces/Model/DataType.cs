using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDb.Interfaces.Model
{
    /// <summary>
    /// represents supported column datatype
    /// </summary>
    public class DataType
    {
        /// <summary>
        /// Type name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Type description
        /// </summary>
        public string Description { get; set; }
    }
}
