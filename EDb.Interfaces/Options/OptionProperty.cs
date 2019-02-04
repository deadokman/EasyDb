using System;

namespace EDb.Interfaces.Options
{
    /// <summary>
    /// Describes single option property inside defenition
    /// </summary>
    [Serializable]
    public class OptionProperty
    {
        /// <summary>
        /// Default property name
        /// </summary>
        public string DefaultPropertyName { get; set; }

        /// <summary>
        /// Property name in resource file
        /// </summary>
        public string ResourcePropertyKey { get; set; }

        /// <summary>
        /// Property value type
        /// </summary>
        public string PropertyValueTypeName { get; set; }

        /// <summary>
        /// Property default value
        /// </summary>
        public object DefaultValue { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Property is read only
        /// </summary>
        public bool ReadOnly { get; set; }

        /// <summary>
        /// Actual property name
        /// </summary>
        public string ActualPropertyName =>
            string.IsNullOrEmpty(this.ResourcePropertyKey) ? DefaultPropertyName : this.ResourcePropertyKey;
    }
}
