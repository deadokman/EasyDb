namespace EasyDb.CustomControls.DatasourceSettings
{
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using System.Windows.Markup;

    using EasyDb.Annotations;

    using EDb.Interfaces;
    using EDb.Interfaces.Options;

    /// <summary>
    /// Datasource option class
    /// Опция источника данных
    /// </summary>
    public class DatasourceOption
    {
        [NotNull]
        private readonly OptionProperty _optProp;

        private readonly EdbSourceOption _optionObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatasourceOption"/> class.
        /// </summary>
        /// <param name="optProp">The depObjectTarget<see cref="object"/></param>
        /// <param name="optionObject">Options object</param>
        public DatasourceOption([NotNull] OptionProperty optProp, [NotNull] EdbSourceOption optionObject)
        {
            _optProp = optProp ?? throw new ArgumentNullException(nameof(optProp));
            _optionObject = optionObject ?? throw new ArgumentNullException(nameof(optionObject));
            optionObject.SetThrowExceptionOnInvalidate(true);
        }

        /// <summary>
        /// Gets a value indicating whether IsReadOnly
        /// </summary>
        public bool IsReadOnly => _optProp.ReadOnly;

        /// <summary>
        /// Gets or sets the OptionName
        /// Option name
        /// Наименование опции
        /// </summary>
        public string OptionName { get; set; }

        /// <summary>
        /// Gets property system type
        /// </summary>
        public string PropertyType => _optProp.PropertyValueTypeName;

        /// <summary>
        /// Gets a value indicating whether property should contain password
        /// </summary>
        public bool IsPasswordProp => _optProp.IsPasswordProperty;

        /// <summary>
        /// Gets or sets the Value
        /// Value of dependency object
        /// </summary>
        [DebuggerHidden]
        public object Value
        {
            get => _optProp.PropertyInfo.GetValue(_optionObject);
            set
            {
                _optProp.PropertyInfo.SetValue(_optionObject, value);
            }
        }
    }
}