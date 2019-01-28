namespace EasyDb.CustomControls.DatasourceSettings
{
    using System;
    using System.Diagnostics;
    using System.Reflection;

    using EasyDb.Annotations;

    /// <summary>
    /// Datasource option class
    /// Опция источника данных
    /// </summary>
    public class DatasourceOption
    {
        /// <summary>
        /// Defines the _depObjectTarget
        /// </summary>
        private readonly object depObjectTarget;

        /// <summary>
        /// Defines the _pi
        /// </summary>
        private readonly PropertyInfo pi;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatasourceOption"/> class.
        /// </summary>
        /// <param name="depObjectTarget">The depObjectTarget<see cref="object"/></param>
        /// <param name="pi">The pi<see cref="PropertyInfo"/></param>
        public DatasourceOption([NotNull] object depObjectTarget, [NotNull] PropertyInfo pi)
        {
            this.depObjectTarget = depObjectTarget ?? throw new ArgumentNullException(nameof(depObjectTarget));
            this.pi = pi ?? throw new ArgumentNullException(nameof(pi));
        }

        /// <summary>
        /// Gets or sets a value indicating whether IsReadOnly
        /// </summary>
        public bool IsReadOnly { get; set; }

        /// <summary>
        /// Gets or sets the OptionEditType
        /// Option value type
        /// </summary>
        public string OptionEditType { get; set; }

        /// <summary>
        /// Gets or sets the OptionName
        /// Option name
        /// Наименование опции
        /// </summary>
        public string OptionName { get; set; }

        /// <summary>
        /// Gets or sets the Value
        /// Valye of dependency object
        /// </summary>
        [DebuggerHidden]
        public object Value
        {
            get => this.pi.GetValue(this.depObjectTarget);
            set => this.pi.SetValue(this.depObjectTarget, value);
        }
    }
}