using System;
using System.Collections.Generic;
using System.Globalization;
using Xamarin.Forms;

namespace ChessClock
{
    public class IntListToStringListConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var list = (IReadOnlyCollection<int>)value;
            var result = new List<string>();
            foreach (var item in list)
                result.Add(item.ToString("D2"));
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var list = (IReadOnlyCollection<string>)value;
            var result = new List<int>();
            foreach (var item in list)
                result.Add(int.Parse(item));
            return result;
        }
    }
}
