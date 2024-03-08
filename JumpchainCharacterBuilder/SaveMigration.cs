using JumpchainCharacterBuilder.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace JumpchainCharacterBuilder
{
    public static partial class SaveMigration
    {
        // TODO - Implement migration for 1.3.3, to fix Item stipends being moved to purchase types.
        public static SaveFile SaveUpdate(string filePath, double saveVersion, SaveFile saveFile)
        {
            List<string> saveStrings = TxtAccess.ReadText(filePath);
            List<(int, int)> indexList = new();
            (int, int) currentIndexes;
            List<string> innerStringList = new();

            int deleteLength = 0;

            if (saveVersion < 1.3)
            {
                Regex suspendRegex = TagRemovalRegex();

                indexList = FindAllXmlTagIndexes("DrawbackSupplementPurchase", saveStrings);
                List<DrawbackSupplementPurchase> drawbacks = new();

                switch (saveFile.Options.DrawbackSupplementSetting)
                {
                    case Options.DrawbackSupplements.Generic:
                        drawbacks = saveFile.GenericDrawbackSupplement.Purchases;
                        break;
                    case Options.DrawbackSupplements.UDS:
                        drawbacks = saveFile.UniversalDrawbackSupplement.Purchases;
                        break;
                    case Options.DrawbackSupplements.UU:
                        drawbacks = saveFile.UUSupplement.Purchases;
                        break;
                }

                foreach (DrawbackSupplementPurchase drawback in drawbacks)
                {
                    ListValidationClass.CheckDrawbackSuspendCount(drawback, saveFile.JumpList.Count);
                }

                for (int y = 0; y < indexList.Count; y++)
                {
                    innerStringList = saveStrings.GetRange(indexList[y].Item1, indexList[y].Item2 - indexList[y].Item1 + 1);

                    currentIndexes = FindXmlTagIndexes("Suspend", innerStringList);

                    innerStringList = innerStringList.GetRange(currentIndexes.Item1, currentIndexes.Item2 - currentIndexes.Item1 + 1);
                    innerStringList.RemoveAt(0);
                    innerStringList.RemoveAt(innerStringList.Count - 1);

                    for (int i = 0; i < innerStringList.Count; i++)
                    {
                        deleteLength = 0;

                        for (int x = 0; x < innerStringList[i].Length; x++)
                        {
                            if (innerStringList[i][x] == ' ')
                            {
                                deleteLength++;
                            }
                            else if (innerStringList[i][x] == '<')
                            {
                                break;
                            }
                        }
                        if (deleteLength > 0)
                        {
                            innerStringList[i] = innerStringList[i].Remove(0, deleteLength);
                        }

                        innerStringList[i] = suspendRegex.Replace(innerStringList[i], "");

                        if (innerStringList[i] == "false")
                        {
                            drawbacks[y].SuspendList[i].Suspended = false;
                        }
                        else if (innerStringList[i] == "true")
                        {
                            drawbacks[y].SuspendList[i].Suspended = true;
                        }
                    }
                }
            }

            return saveFile;
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
        /// <param name="startIndex">Represents where to start searching in the list of strings.</param>
        /// <returns>A tuple with the opening and closing indexes, or -1 for each if a match is not made.</returns>
        private static (int index1, int index2) FindXmlTagIndexes(string tagName, List<string> saveStrings, int startIndex = 0)
        {
            int openingIndex = -1;
            int closingIndex = -1;

            bool openingMatched = false;
            bool closingMatched = false;

            Regex openingRegex = new($"<{tagName}(?: .+)?>", RegexOptions.Compiled);
            Regex closingRegex = new($"</{tagName}>", RegexOptions.Compiled);

            for (int i = 0; i < saveStrings.Count; i++)
            {
                if (i >= startIndex)
                {
                    if (openingRegex.IsMatch(saveStrings[i]))
                    {
                        openingIndex = i;
                        openingMatched = true;
                    }
                    if (closingRegex.IsMatch(saveStrings[i]))
                    {
                        closingIndex = i;
                        closingMatched = true;
                    }
                }

                if (openingMatched && closingMatched)
                {
                    break;
                }
            }

            return (openingIndex, closingIndex);
        }

        private static List<(int, int)> FindAllXmlTagIndexes(string tagName, List<string> saveStrings)
        {
            List<(int, int)> openingClosingIndexes = new();
            int currentClosingIndex = 0;
            bool completed = false;

            while (!completed)
            {
                openingClosingIndexes.Add(FindXmlTagIndexes(tagName, saveStrings, currentClosingIndex + 1));
                currentClosingIndex = openingClosingIndexes.Last().Item2;

                if (openingClosingIndexes.Last() == (-1, -1))
                {
                    openingClosingIndexes.RemoveAt(openingClosingIndexes.Count - 1);
                    completed = true;
                }
            }

            return openingClosingIndexes;
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

        [GeneratedRegex("<.+?>", RegexOptions.Compiled)]
        private static partial Regex TagRemovalRegex();
    }
}
