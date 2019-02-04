namespace EasyDb.CustomControls.DatasourceSettings
{
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using System.Windows.Markup;

    using EasyDb.Annotations;

    using EDb.Interfaces.Options;

    /// <summary>
    /// Datasource option class
    /// Опция источника данных
    /// </summary>
    public class DatasourceOption
    {
        [NotNull]
        private readonly OptionProperty _optProp;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatasourceOption"/> class.
        /// </summary>
        /// <param name="optProp">The depObjectTarget<see cref="object"/></param>
        public DatasourceOption([NotNull] OptionProperty optProp)
        {
            this._optProp = optProp ?? throw new ArgumentNullException(nameof(optProp));
        }

        /// <summary>
        /// Gets a value indicating whether IsReadOnly
        /// </summary>
        public bool IsReadOnly => this._optProp.ReadOnly;

        /// <summary>
        /// Gets or sets the OptionName
        /// Option name
        /// Наименование опции
        /// </summary>
        public string OptionName { get; set; }

        /// <summary>
        /// Gets property system type
        /// </summary>
        public string PropertyType => this._optProp.PropertyValueTypeName;

        /// <summary>
        /// Gets or sets the Value
        /// Value of dependency object
        /// </summary>
        [DebuggerHidden]
        public object Value
        {
            get => this._optProp.Value ?? this._optProp.DefaultValue;
            set => this._optProp.Value = value;
        }
    }
}