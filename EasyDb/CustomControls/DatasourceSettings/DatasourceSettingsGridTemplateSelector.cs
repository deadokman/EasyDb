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
        public DataTemplate TextFieldTemplate { get; set; }

        public DataTemplate CheckboxTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item != null)
            {
                return base.SelectTemplate(item, container);
            }
            else
            {
                return base.SelectTemplate(item, container);
            }       
        }
    }
}
