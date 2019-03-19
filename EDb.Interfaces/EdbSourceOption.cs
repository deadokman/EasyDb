using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

using EDb.Interfaces.Options;

namespace EDb.Interfaces
{
    using System.Xml.Serialization;

    /// <summary>
    /// Interface for datasource options
    /// Класс настроек для источника данных
    /// </summary>
    [Serializable]
    public abstract class EdbSourceOption : ValidationViewModelBase
    {
        [Browsable(false)]
        [XmlIgnore]
        public virtual string OptionsDefinitionName { get; set; }

        /// <summary>
        /// Initialized instance of module options definitions
        /// </summary>
        [XmlIgnore]
        protected ModuleOptionDefinition OptionDefinitions;

        /// <summary>
        /// Sets option definition
        /// </summary>
        /// <param name="definition">Option definition instance</param>
        public void SetOptionDefinition(ModuleOptionDefinition definition)
        {
            this.OptionDefinitions = definition;
        }

        /// <summary>
        /// Converts option to definition class
        /// </summary>
        /// <returns></returns>
        public virtual ModuleOptionDefinition ToOptionDefinition()
        {
            if (this.OptionDefinitions != null)
            {
                return this.OptionDefinitions;
            }

            var currentType = this.GetType();
            var definitionProps = currentType.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Select(p => new { prop = p, optDisplay = p.GetCustomAttributes<OptionDisplayNameAttribute>().FirstOrDefault()})
                .Where(p => p.optDisplay != null).ToArray();

            var resultArr = new OptionProperty[definitionProps.Length];
            for (var i = 0; i < definitionProps.Length; i++)
            {
                var pdata = definitionProps[i];
                resultArr[i] = new OptionProperty(
                    pdata.optDisplay.AlternativeName,
                    pdata.optDisplay.ResourceNameKey,
                    pdata.prop.PropertyType.FullName,
                    pdata.prop.GetValue(this),
                    !pdata.prop.CanWrite,
                    CheckIsPassword(pdata.prop),
                    pdata.prop);
            }

            var moduleOptionDefenition = new ModuleOptionDefinition();
            moduleOptionDefenition.DefinitionName = this.OptionsDefinitionName;
            moduleOptionDefenition.Properties = resultArr;
            moduleOptionDefenition.PropertyDefinitionType = currentType.FullName;

            this.OptionDefinitions = moduleOptionDefenition;
            return moduleOptionDefenition;
        }

        /// <summary>
        /// Check password attribute on property
        /// </summary>
        /// <returns>true if password</returns>
        public bool CheckIsPassword(PropertyInfo p)
        {
            return p.GetCustomAttributes<PasswordFieldAttribute>().Any();
        }
    }
}
