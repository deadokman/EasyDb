using System.Xml.Serialization;

namespace EDb.Interfaces.Schema.SchemaTypes
{
    /// <summary>
    /// Schema node
    /// </summary>
    public class SchemaNodeBase
    {
        /// <summary>
        /// Node name
        /// </summary>
        [XmlAttribute]
        public string Name { get; set; }

        /// <summary>
        /// Node type name
        /// </summary>
        [XmlAttribute]
        public string NodeTypeName { get; set; }

        /// <summary>
        /// Node represents folder in schema
        /// </summary>
        [XmlAttribute]
        public bool Folder { get; set; }
    }
}
