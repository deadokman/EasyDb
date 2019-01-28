namespace EasyDb.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    using EasyDb.ViewModel;

    /// <summary>
    /// Defines the <see cref="ActiveDocumentConverter" />
    /// </summary>
    public class ActiveDocumentConverter : IValueConverter
    {
        /// <summary>
        /// The Convert
        /// </summary>
        /// <param name="value">The value<see cref="object"/></param>
        /// <param name="targetType">The targetType<see cref="Type"/></param>
        /// <param name="parameter">The parameter<see cref="object"/></param>
        /// <param name="culture">The culture<see cref="System.Globalization.CultureInfo"/></param>
        /// <returns>The <see cref="object"/></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is PaneBaseViewModel)
            {
                return value;
            }

            return Binding.DoNothing;
        }

        /// <summary>
        /// The ConvertBack
        /// </summary>
        /// <param name="value">The value<see cref="object"/></param>
        /// <param name="targetType">The targetType<see cref="Type"/></param>
        /// <param name="parameter">The parameter<see cref="object"/></param>
        /// <param name="culture">The culture<see cref="System.Globalization.CultureInfo"/></param>
        /// <returns>The <see cref="object"/></returns>
        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            if (value is PaneBaseViewModel)
            {
                return value;
            }

            return Binding.DoNothing;
        }
    }
}