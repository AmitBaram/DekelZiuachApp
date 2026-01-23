using System;
using System.Globalization;
using System.Windows.Data;

namespace DekelApp.Utils.Converters
{
    public class AddOneConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int i) return (i + 1).ToString();
            if (value != null && int.TryParse(value.ToString(), out int parsed)) return (parsed + 1).ToString();
            return "1";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
