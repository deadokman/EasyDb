using System;
using System.Globalization;
using System.Windows.Data;

namespace EasyDb.Converters
{
    public class SettingsSizeLimitConverter : IValueConverter
    {
        public double? Delimiter { get; set; }

        private object CalcWidth(object value)
        {
            if (value is double)
            {
                //Main window width
                var dVal = (double)value;
                var percentWidth = Properties.Settings.Default.SettingsFlyoutWidthLimiter > 100 
                    || Properties.Settings.Default.SettingsFlyoutWidthLimiter < 25 
                        ? 25 : Properties.Settings.Default.SettingsFlyoutWidthLimiter;
                var calcWidth = dVal * percentWidth / 100;

                return calcWidth;
                
            }

            return value;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return CalcWidth(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return CalcWidth(value);
        }
    }
}
