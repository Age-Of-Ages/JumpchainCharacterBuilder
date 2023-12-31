﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace JumpchainCharacterBuilder.Converters
{
    public class BoolResizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? ResizeMode.CanResize : ResizeMode.CanMinimize;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
