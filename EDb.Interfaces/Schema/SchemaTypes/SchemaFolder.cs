using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EDb.Interfaces.Schema.SchemaTypes
{
    /// <summary>
    /// Represents a folder object for database schema
    /// </summary>
    public class SchemaFolder : SchemaNodeBase
    {
       
        [XmlArray]
        public SchemaNodeBase[] Childs { get; set; }
    }
}
