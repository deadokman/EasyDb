﻿namespace EasyDb.CustomControls.DatasourceSettings
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Template selector for grid cell
    /// </summary>
    public class DatasourceSettingsGridTemplateSelector : DataTemplateSelector
    {
        /// <summary>
        /// Defines the StringType
        /// </summary>
        private static readonly string StringType = typeof(string).FullName;

        /// <summary>
        /// Gets or sets the CheckboxTemplate
        /// </summary>
        public DataTemplate CheckboxTemplate { get; set; }

        /// <summary>
        /// Gets or sets the NumericTemplate
        /// </summary>
        public DataTemplate NumericTemplate { get; set; }

        /// <summary>
        /// Gets or sets the PasswordField
        /// </summary>
        public DataTemplate PasswordField { get; set; }

        /// <summary>
        /// Gets or sets the TextFieldTemplate
        /// </summary>
        public DataTemplate TextFieldTemplate { get; set; }

        /// <summary>
        /// The SelectTemplate
        /// </summary>
        /// <param name="item">The item<see cref="object"/></param>
        /// <param name="container">The container<see cref="DependencyObject"/></param>
        /// <returns>The <see cref="DataTemplate"/></returns>
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var optionObject = item as DatasourceOption;
            if (optionObject != null)
            {
                if (optionObject.IsPasswordProp)
                {
                    return PasswordField;
                }

                switch (optionObject.PropertyType)
                {
                    case "System.String": return TextFieldTemplate;
                    case "System.Boolean": return CheckboxTemplate;
                    case "System.Int32": return NumericTemplate;
                    case "System.Int64": return NumericTemplate;
                    case "System.Int16": return NumericTemplate;

                    default: return base.SelectTemplate(item, container);
                }
            }

            return base.SelectTemplate(item, container);
        }
    }
}