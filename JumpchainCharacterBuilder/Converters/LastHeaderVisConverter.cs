using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace JumpchainCharacterBuilder.Converters
{
    class HeaderCheckVisConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is GridViewColumnCollection columnList &&
                values[1] is GridViewColumnHeader currentColumn)
            {
                int currentIndex = columnList.IndexOf(currentColumn.Column);
                bool indexMatches = currentIndex == 0;

                return indexMatches ? Visibility.Collapsed : Visibility.Visible;
            }
            return Visibility.Visible;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
