using JumpchainCharacterBuilder.Model;
using System.Collections.Generic;
using System.Linq;

namespace JumpchainCharacterBuilder
{
    public static class ListValidationClass
    {
        /// <summary>
        /// Ensures that an appropriate number of Jump Builds exist for the selected
        /// character index.
        /// </summary>
        /// <param name="jump">Represents the Jump to check against.</param>
        /// <param name="characterIndex">Represents the index number that should be checked for.</param>
        public static void CheckBuildCount(Jump jump, int characterIndex)
        {
            if (characterIndex > jump.Build.Count - 1)
            {
                for (int i = characterIndex - (jump.Build.Count - 1); i > 0; i--)
                {
                    jump.Build.Add(new JumpBuild());
                }
            }
        }

        public static void CheckMiscOriginCount(Jump jump, int characterIndex)
        {
            List<OriginDetail> tempOriginList = new();
            JumpBuild jumpBuild = jump.Build[characterIndex];

            foreach (string originCategory in jump.MiscOriginCategories)
            {
                if (jumpBuild.MiscOriginDetails.Any(s => s.Category == originCategory))
                {
                    int index = jumpBuild.MiscOriginDetails.FindIndex(s => s.Category == originCategory);
                    if (index >= 0)
                    {
                        tempOriginList.Add(jumpBuild.MiscOriginDetails[index]);
                    }
                    else
                    {
                        // Error, failed to find the Origin category.
                    }
                }
                else
                {
                    tempOriginList.Add(new OriginDetail(originCategory));
                }
            }

            jump.Build[characterIndex].MiscOriginDetails = tempOriginList;
        }

        public static void CheckStipendCount(Jump jump, int characterIndex)
        {
            JumpBuild jumpBuild = jump.Build[characterIndex];

            if (jumpBuild.PointStipend.Count < jump.Currencies.Count)
            {
                int difference = jump.Currencies.Count - jumpBuild.PointStipend.Count;

                for (int i = 0; i < difference; i++)
                {
                    jumpBuild.PointStipend.Add(0);
                }
            }
        }

        public static void CheckImportListCount(SaveFile saveFile, Jump jump, int characterIndex)
        {
            JumpBuild jumpBuild = jump.Build[characterIndex];

            foreach (CompanionPurchase companionPurchase in jumpBuild.CompanionPurchase)
            {
                if (companionPurchase.CompanionImportDetails.Count < saveFile.CharacterList.Count)
                {
                    int index = companionPurchase.CompanionImportDetails.Count;
                    for (int i = index; i < saveFile.CharacterList.Count - 1; i++)
                    {
                        companionPurchase.CompanionImportDetails.Add(new CompanionImportDetailClass(saveFile.CharacterList[i + 1].Name));
                    }
                }
            }
        }

        public static void CheckDrawbackSuspendCount(DrawbackSupplementPurchase drawback, int jumpCount)
        {
            if (drawback.Suspend.Count < jumpCount)
            {
                int index = drawback.Suspend.Count;
                for (int i = index; i < jumpCount; i++)
                {
                    drawback.Suspend.Add(false);
                }
            }
        }
    }
}
