using System;
using System.Globalization;
using System.Windows.Data;

namespace JumpchainCharacterBuilder.Converters
{
    public class NullBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
