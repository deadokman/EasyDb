using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDb.Interfaces
{
    /// <summary>
    /// Attrivute for marking options files that sould be displayed
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, AllowMultiple = false)]
    public class OptionDisplayNameAttribute : Attribute
    {
        /// <summary>
        /// Key for resource in different translations
        /// </summary>
        public string ResourceNameKey { get; }

        /// <summary>
        /// Alternative name if resource key did not found
        /// </summary>
        public string AlternativeName { get; }

        public OptionDisplayNameAttribute(string resourceNameKey, string alternativeName)
        {
            ResourceNameKey = resourceNameKey;
            AlternativeName = alternativeName;
        }
    }
}
