using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace JumpchainCharacterBuilder.Converters
{
    public class UriVisConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Uri uri = (Uri)value;

            string uriAsString = uri.ToString();

            if (uriAsString != "about:Blank")
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Hidden;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
