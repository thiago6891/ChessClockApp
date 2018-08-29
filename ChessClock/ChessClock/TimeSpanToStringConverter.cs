using System;
using System.Globalization;
using Xamarin.Forms;

namespace ChessClock
{
    public class TimeSpanToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var time = (TimeSpan)value;
            if (time >= TimeSpan.FromHours(1))
                return time.ToString(@"hh\:mm");
            if (time >= TimeSpan.FromMinutes(1))
                return time.ToString(@"mm\:ss");
            return time.ToString(@"ss\.ff");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // This class can only be used for one-way binding only.
            throw new InvalidOperationException();
        }
    }
}