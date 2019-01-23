using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using EasyDb.Annotations;

namespace EasyDb.CustomControls.DatasourceSettings
{
    /// <summary>
    /// Datasource option class
    /// Опция источника данных
    /// </summary>
    public class DatasourceOption
    {
        private readonly object _depObjectTarget;
        private readonly PropertyInfo _pi;

        public DatasourceOption([NotNull] object depObjectTarget, [NotNull] PropertyInfo pi)
        {
            _depObjectTarget = depObjectTarget ?? throw new ArgumentNullException(nameof(depObjectTarget));
            _pi = pi ?? throw new ArgumentNullException(nameof(pi));
        }

        /// <summary>
        /// Option name
        /// Наименование опции
        /// </summary>
        public string OptionName { get; set; }

        /// <summary>
        /// Option value type
        /// </summary>
        public string OptionEditType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsReadOnly { get; set; }

        /// <summary>
        /// Valye of dependency object
        /// </summary>
        [DebuggerHidden]
        public object Value
        {
            get => _pi.GetValue(_depObjectTarget);
            set => _pi.SetValue(_depObjectTarget, value);
        }
    }
}
