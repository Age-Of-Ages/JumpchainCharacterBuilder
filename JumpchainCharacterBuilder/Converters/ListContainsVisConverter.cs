using System;
using System.Collections;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace JumpchainCharacterBuilder.Converters
{
    public class ListContainsVisConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var containedObject = values[0];
            ICollection? list = values[1] as ICollection;
            
            if (containedObject != null && list != null)
            {
                foreach (var item in list)
                {
                    if (containedObject == item)
                    {
                        return Visibility.Visible;
                    }
                }
            }

            return Visibility.Hidden;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
