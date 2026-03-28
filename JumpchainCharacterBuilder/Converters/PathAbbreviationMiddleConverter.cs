using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace JumpchainCharacterBuilder.Converters
{
    public class PathAbbreviationMiddleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string path = (string)value;
            int maxLength = 50;
            List<string> folderNames = [];
            string driveLetter = "";
            string fileName = "";

            folderNames = [.. path.Split("\\")];
            driveLetter = folderNames.First();
            fileName = folderNames.Last();
            
            folderNames.RemoveAt(0);
            folderNames.RemoveAt(folderNames.Count - 1);

            int folderNamesLength = 0;
            foreach (string folderName in folderNames)
            {
                folderNamesLength += folderName.Length;
            }

            if (folderNamesLength > maxLength)
            {
                int abbreviationIndex = 0;
                int currentFolderLength = 0;

                while (folderNamesLength > maxLength)
                {
                    abbreviationIndex = folderNames.Count / 2;
                    currentFolderLength = folderNames[folderNames.Count / 2].Length;
                    folderNamesLength -= currentFolderLength;
                    folderNames.RemoveAt(folderNames.Count / 2);
                } 
                
                if (abbreviationIndex > 0)
                {
                    folderNames.Insert(abbreviationIndex, "...");
                }
            }

            string folderPath = string.Join("\\", folderNames);

            return string.Join("\\", driveLetter, folderPath, fileName);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
