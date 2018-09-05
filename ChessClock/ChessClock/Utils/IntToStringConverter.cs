﻿using System;
using System.Globalization;
using Xamarin.Forms;

namespace ChessClock
{
    public class IntToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((int)value).ToString("D2");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return 0;
            return int.Parse((string)value);
        }
    }
}