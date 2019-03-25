namespace EasyDb.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    /// <summary>
    /// Defines the <see cref="SettingsSizeLimitConverter" />
    /// </summary>
    public class SettingsSizeLimitConverter : IValueConverter
    {
        /// <summary>
        /// Gets or sets the Delimiter
        /// </summary>
        public double? Delimiter { get; set; }

        /// <summary>
        /// The Convert
        /// </summary>
        /// <param name="value">The value<see cref="object"/></param>
        /// <param name="targetType">The targetType<see cref="Type"/></param>
        /// <param name="parameter">The parameter<see cref="object"/></param>
        /// <param name="culture">The culture<see cref="CultureInfo"/></param>
        /// <returns>The <see cref="object"/></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return CalcWidth(value);
        }

        /// <summary>
        /// The ConvertBack
        /// </summary>
        /// <param name="value">The value<see cref="object"/></param>
        /// <param name="targetType">The targetType<see cref="Type"/></param>
        /// <param name="parameter">The parameter<see cref="object"/></param>
        /// <param name="culture">The culture<see cref="CultureInfo"/></param>
        /// <returns>The <see cref="object"/></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return CalcWidth(value);
        }

        /// <summary>
        /// The CalcWidth
        /// </summary>
        /// <param name="value">The value<see cref="object"/></param>
        /// <returns>The <see cref="object"/></returns>
        private object CalcWidth(object value)
        {
            if (value is double)
            {
                // Main window width
                var dVal = (double)value;
                var percentWidth = Properties.Settings.Default.SettingsFlyoutWidthLimiter > 100
                                   || Properties.Settings.Default.SettingsFlyoutWidthLimiter < 25
                                       ? 25
                                       : Properties.Settings.Default.SettingsFlyoutWidthLimiter;
                var calcWidth = dVal * percentWidth / 100;

                return calcWidth;
            }

            return value;
        }
    }
}