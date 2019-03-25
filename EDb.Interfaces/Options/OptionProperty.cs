namespace EDb.Interfaces.Options
{
    using System;
    using System.Reflection;

    using EDb.Interfaces.Annotations;

    /// <summary>
    /// Describes single option property inside defenition
    /// </summary>
    [Serializable]
    public class OptionProperty
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OptionProperty"/> class.
        /// </summary>
        /// <param name="defaultPropertyName">The defaultPropertyName<see cref="string"/></param>
        /// <param name="resourcePropertyKey">The resourcePropertyKey<see cref="string"/></param>
        /// <param name="propertyValueTypeName">The propertyValueTypeName<see cref="string"/></param>
        /// <param name="defaultValue">The defaultValue<see cref="object"/></param>
        /// <param name="readOnly">The readOnly<see cref="bool"/></param>
        /// <param name="isPasswordProperty">The isPasswordProperty<see cref="bool"/></param>
        /// <param name="propertyInfo">The propertyInfo<see cref="PropertyInfo"/></param>
        public OptionProperty(
            [NotNull] string defaultPropertyName,
            [NotNull] string resourcePropertyKey,
            [NotNull] string propertyValueTypeName,
            object defaultValue,
            bool readOnly,
            bool isPasswordProperty,
            [NotNull] PropertyInfo propertyInfo)
        {
            DefaultPropertyName = defaultPropertyName ?? throw new ArgumentNullException(nameof(defaultPropertyName));
            ResourcePropertyKey = resourcePropertyKey ?? throw new ArgumentNullException(nameof(resourcePropertyKey));
            PropertyValueTypeName = propertyValueTypeName ?? throw new ArgumentNullException(nameof(propertyValueTypeName));
            DefaultValue = defaultValue;
            ReadOnly = readOnly;
            IsPasswordProperty = isPasswordProperty;
            PropertyInfo = propertyInfo ?? throw new ArgumentNullException(nameof(propertyInfo));
        }

        /// <summary>
        /// Gets or sets the DefaultPropertyName
        /// Default property name
        /// </summary>
        public string DefaultPropertyName { get; set; }

        /// <summary>
        /// Gets or sets the ResourcePropertyKey
        /// Property name in resource file
        /// </summary>
        public string ResourcePropertyKey { get; set; }

        /// <summary>
        /// Gets or sets the PropertyValueTypeName
        /// Property value type
        /// </summary>
        public string PropertyValueTypeName { get; set; }

        /// <summary>
        /// Gets or sets the DefaultValue
        /// Property default value
        /// </summary>
        public object DefaultValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether ReadOnly
        /// Property is read only
        /// </summary>
        public bool ReadOnly { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether IsPasswordProperty
        /// Property should contain password
        /// </summary>
        public bool IsPasswordProperty { get; set; }

        /// <summary>
        /// Gets or sets the PropertyInfo
        /// Gets or sets propety info
        /// </summary>
        public PropertyInfo PropertyInfo { get; set; }

        /// <summary>
        /// Gets the ActualPropertyName
        /// Actual property name
        /// </summary>
        public string ActualPropertyName =>
            string.IsNullOrEmpty(ResourcePropertyKey) ? DefaultPropertyName : ResourcePropertyKey;
    }
}
