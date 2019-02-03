using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using EDb.Interfaces.Annotations;
using EDb.Interfaces.Options;
using EDb.Interfaces.Validation;

namespace EDb.Interfaces
{
    /// <summary>
    /// Interface for datasource options
    /// Класс настроек для источника данных
    /// </summary>
    [Serializable]
    public abstract class EdbSourceOption : ValidationViewModelBase
    {
        [Browsable(false)]
        public virtual string OptionsDefinitionName { get; protected set; }

        /// <summary>
        /// Converts option to definition class
        /// </summary>
        /// <returns></returns>
        public ModuleOptionDefinition ToOptionDefinition()
        {
            var currentType = this.GetType();
            var definitionProps = currentType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty)
                .Select(p => new { prop = p, optDisplay = p.GetCustomAttributes<OptionDisplayNameAttribute>().FirstOrDefault()})
                .Where(p => p.optDisplay != null).ToArray();

            var resultArr = new OptionProperty[definitionProps.Length];
            for (var i = 0; i < definitionProps.Length; i++)
            {
                var pdata = definitionProps[i];
                var op = new OptionProperty();
                op.ResourcePropertyKey = pdata.optDisplay.ResourceNameKey;
                op.DefaultPropertyName = pdata.optDisplay.AlternativeName;
                op.DefaultValue = pdata.prop.GetValue(this);
                resultArr[i] = op;
            }

            var moduleOptionDefenition = new ModuleOptionDefinition();
            moduleOptionDefenition.DefinitionName = this.OptionsDefinitionName;
            moduleOptionDefenition.Properties = resultArr;
            moduleOptionDefenition.PropertyDefinitionType = currentType.FullName;
            return moduleOptionDefenition;
        }
    }
}
