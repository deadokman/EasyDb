namespace EDb.Interfaces.Options
{
    /// <summary>
    /// Describes single option property inside defenition
    /// </summary>
    public class OptionProperty
    {
        /// <summary>
        /// Default property name
        /// </summary>
        public string DefaultPropertyName { get; set; }

        /// <summary>
        /// Resource name for property
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// Property name in resource file
        /// </summary>
        public string ResourcePropertyName { get; set; }

        /// <summary>
        /// Property value type
        /// </summary>
        public string PropertyValueTypeName { get; set; }

        /// <summary>
        /// Property default value
        /// </summary>
        public object DefaultValue { get; set; }

        /// <summary>
        /// Actual property name
        /// </summary>
        public string ActualPropertyName =>
            string.IsNullOrEmpty(ResourcePropertyName) ? DefaultPropertyName : ResourcePropertyName;
    }
}
