using JumpchainCharacterBuilder.Model;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace JumpchainCharacterBuilder
{
    public static class SaveMigration
    {
        public static SaveFile SaveUpdate(string filePath, double saveVersion)
        {
            List<string> saveStrings = TxtAccess.ReadText(filePath);
            int index1;
            int index2;



            return XmlAccess.ReadObject(filePath);
        }

        public static SaveFile SaveModify(SaveFile save)
        {
            if (save.SaveVersion < 1.1)
            {
                if (save.PerkCategoryList.Contains("Blacksmithing"))
                {
                    save.UserPerkCategoryList.Add("Blacksmithing");
                }

                save.SaveVersion = 1.1;
            }

            return save;
        }

        /// <summary>
        /// Returns the indexes of the opening and closing tags of the first Xml tag with the provided name.
        /// </summary>
        /// <param name="tagName">Represents the name of the Xml tag to search for.</param>
        /// <param name="saveStrings">Represents the list of strings to search through.</param>
        /// <returns>A tuple with the opening and closing indexes, or -1 for each if a match is not made.</returns>
        private static (int index1, int index2) FindXmlTagIndexes(string tagName, List<string> saveStrings)
        {
            int openingIndex = -1;
            int closingIndex = -1;

            bool openingMatched = false;
            bool closingMatched = false;

            Regex openingRegex = new($"<{tagName}(?: .+)?>", RegexOptions.Compiled);
            Regex closingRegex = new($"</{tagName}>", RegexOptions.Compiled);

            foreach (string line in saveStrings)
            {
                if (openingRegex.IsMatch(line))
                {
                    openingIndex = saveStrings.IndexOf(line);
                    openingMatched = true;
                }
                if (closingRegex.IsMatch(line))
                {
                    closingIndex = saveStrings.IndexOf(line);
                    closingMatched = true;
                }

                if (openingMatched && closingMatched)
                {
                    break;
                }
            }

            return (openingIndex, closingIndex);
        }

        /// <summary>
        /// Returns the index of the single-line Xml tag with the provided name.
        /// </summary>
        /// <param name="tagName">Represents the name of the Xml tag to search for.</param>
        /// <param name="saveStrings">Represents the list of strings to search through.</param>
        /// <returns>The index of the provided list that contained the provided tag, or -1 if a match is not made.</returns>
        private static int FindSingleTag(string tagName, List<string> saveStrings)
        {
            int index = -1;

            Regex regex = new($"<(?:.+){tagName}>.+</(?:.+){tagName}>");

            foreach (string line in saveStrings)
            {
                if (regex.IsMatch(line))
                {
                    index = saveStrings.IndexOf(line);

                    break;
                }
            }

            return index;
        }
    }
}
