using System;
using System.Windows.Data;
using CSGO.Trader.ViewModel;
using EasyDb.ViewModel;

namespace EasyDb.Converters
{
    public class ActiveDocumentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is PaneBaseViewModel)
            {
                return value;
            }

            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is PaneBaseViewModel)
            {
                return value;
            }

            return Binding.DoNothing;
        }
    }
}
