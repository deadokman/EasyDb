using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EasyDb.CustomControls.DatasourceSettings
{
    /// <summary>
    /// Template selctor for grid cell
    /// </summary>
    public class DatasourceSettingsGridTemplateSelector : DataTemplateSelector
    {
        private static readonly string StringType = typeof(string).FullName;

        public DataTemplate TextFieldTemplate { get; set; }

        public DataTemplate CheckboxTemplate { get; set; }

        public DataTemplate NumericTemplate{ get; set; }

        public DataTemplate PasswordField { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var optionObject = item as DatasourceOption;
            if (optionObject != null)
            {

                switch (optionObject.OptionEditType)
                {
                    case "System.String": return TextFieldTemplate;
                    case "System.Boolean": return CheckboxTemplate;
                    case "System.Int32": return NumericTemplate;
                    case "System.Int64": return NumericTemplate;
                    case "System.Int16": return NumericTemplate;
                    case "PasswordTextBox": return PasswordField;

                    default: return base.SelectTemplate(item, container);
                }
            }

            return base.SelectTemplate(item, container);
        }
    }
}
