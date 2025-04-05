using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace JumpchainCharacterBuilder.Converters
{
    public class ListContainsBoolConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var containedObject = values[0];
            List<ICollection?> list = [];

            for (int i = 1; i < values.Length; i++)
            {
                list.Add(values[i] as ICollection);
            }

            if (containedObject != null && list.Count > 0)
            {
                foreach (var subList in list)
                {
                    if (subList != null)
                    {
                        foreach (var item in subList)
                        {
                            if (containedObject == item)
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
