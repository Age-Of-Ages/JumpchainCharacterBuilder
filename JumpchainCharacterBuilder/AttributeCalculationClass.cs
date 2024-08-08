using JumpchainCharacterBuilder.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JumpchainCharacterBuilder
{
    public static class AttributeCalculationClass
    {
        /// <summary>
        /// Represents the available rank options and their values for calculation.
        /// </summary>
        public enum RankList
        {
            None,
            F = 10,
            FPlus = 11,
            FPlusPlus = 12,
            FPlusPlusPlus = 13,
            E = 20,
            EPlus = 22,
            EPlusPlus = 24,
            EPlusPlusPlus = 26,
            D = 40,
            DPlus = 44,
            DPlusPlus = 48,
            DPlusPlusPlus = 52,
            C = 70,
            CPlus = 77,
            CPlusPlus = 84,
            CPlusPlusPlus = 91,
            B = 110,
            BPlus = 121,
            BPlusPlus = 132,
            BPlusPlusPlus = 143,
            A = 170,
            APlus = 187,
            APlusPlus = 204,
            APlusPlusPlus = 221,
            EX_F = 500,
            EX_FPlus = 550,
            EX_FPlusPlus = 600,
            EX_FPlusPlusPlus = 650,
            EX_E = 800,
            EX_EPlus = 880,
            EX_EPlusPlus = 960,
            EX_EPlusPlusPlus = 1040,
            EX_D = 1300,
            EX_DPlus = 1430,
            EX_DPlusPlus = 1560,
            EX_DPlusPlusPlus = 1690,
            EX_C = 2000,
            EX_CPlus = 2200,
            EX_CPlusPlus = 2400,
            EX_CPlusPlusPlus = 2600,
            EX_B = 3200,
            EX_BPlus = 3520,
            EX_BPlusPlus = 3840,
            EX_BPlusPlusPlus = 4160,
            EX_A = 5000,
            EX_APlus = 5500,
            EX_APlusPlus = 6000,
            EX_APlusPlusPlus = 6500,
            S_F = 15000,
            S_FPlus = 16500,
            S_FPlusPlus = 18000,
            S_FPlusPlusPlus = 19500,
            S_E = 25000,
            S_EPlus = 27500,
            S_EPlusPlus = 30000,
            S_EPlusPlusPlus = 32500,
            S_D = 50000,
            S_DPlus = 55000,
            S_DPlusPlus = 60000,
            S_DPlusPlusPlus = 65000,
            S_C = 80000,
            S_CPlus = 88000,
            S_CPlusPlus = 96000,
            S_CPlusPlusPlus = 104000,
            S_B = 150000,
            S_BPlus = 165000,
            S_BPlusPlus = 180000,
            S_BPlusPlusPlus = 195000,
            S_A = 250000,
            S_APlus = 275000,
            S_APlusPlus = 300000,
            S_APlusPlusPlus = 325000,
            Z_F = 1000000,
            Z_FPlus = 1100000,
            Z_FPlusPlus = 1200000,
            Z_FPlusPlusPlus = 1300000,
            Z_E = 2000000,
            Z_EPlus = 2200000,
            Z_EPlusPlus = 2400000,
            Z_EPlusPlusPlus = 2600000,
            Z_D = 4000000,
            Z_DPlus = 4400000,
            Z_DPlusPlus = 4600000,
            Z_DPlusPlusPlus = 4800000,
            Z_C = 8000000,
            Z_CPlus = 8800000,
            Z_CPlusPlus = 9600000,
            Z_CPlusPlusPlus = 10400000,
            Z_B = 20000000,
            Z_BPlus = 22000000,
            Z_BPlusPlus = 24000000,
            Z_BPlusPlusPlus = 26000000,
            Z_A = 50000000,
            Z_APlus = 55000000,
            Z_APlusPlus = 60000000,
            Z_APlusPlusPlus = 65000000
        }

        private static readonly Dictionary<RankList, string> _rankListDisplayValues = new()
        {
            { RankList.None, "N/A" },
            { RankList.F, "F" },
            { RankList.FPlus, "F+" },
            { RankList.FPlusPlus, "F++" },
            { RankList.FPlusPlusPlus, "F+++" },
            { RankList.E, "E" },
            { RankList.EPlus, "E+" },
            { RankList.EPlusPlus, "E++" },
            { RankList.EPlusPlusPlus, "E+++" },
            { RankList.D, "D" },
            { RankList.DPlus, "D+" },
            { RankList.DPlusPlus, "D++" },
            { RankList.DPlusPlusPlus, "D+++" },
            { RankList.C, "C" },
            { RankList.CPlus, "C+" },
            { RankList.CPlusPlus, "C++" },
            { RankList.CPlusPlusPlus, "C+++" },
            { RankList.B, "B" },
            { RankList.BPlus, "B+" },
            { RankList.BPlusPlus, "B++" },
            { RankList.BPlusPlusPlus, "B+++" },
            { RankList.A, "A" },
            { RankList.APlus, "A+" },
            { RankList.APlusPlus, "A++" },
            { RankList.APlusPlusPlus, "A+++" },
            { RankList.EX_F, "EX-F" },
            { RankList.EX_FPlus, "EX-F+" },
            { RankList.EX_FPlusPlus, "EX-F++" },
            { RankList.EX_FPlusPlusPlus, "EX-F+++" },
            { RankList.EX_E, "EX-E" },
            { RankList.EX_EPlus, "EX-E+" },
            { RankList.EX_EPlusPlus, "EX-E++" },
            { RankList.EX_EPlusPlusPlus, "EX-E+++" },
            { RankList.EX_D, "EX-D" },
            { RankList.EX_DPlus, "EX-D+" },
            { RankList.EX_DPlusPlus, "EX-D++" },
            { RankList.EX_DPlusPlusPlus, "EX-D+++" },
            { RankList.EX_C, "EX-C" },
            { RankList.EX_CPlus, "EX-C+" },
            { RankList.EX_CPlusPlus, "EX-C++" },
            { RankList.EX_CPlusPlusPlus, "EX-C+++" },
            { RankList.EX_B, "EX-B" },
            { RankList.EX_BPlus, "EX-B+" },
            { RankList.EX_BPlusPlus, "EX-B++" },
            { RankList.EX_BPlusPlusPlus, "EX-B+++" },
            { RankList.EX_A, "EX-A" },
            { RankList.EX_APlus, "EX-A+" },
            { RankList.EX_APlusPlus, "EX-A++" },
            { RankList.EX_APlusPlusPlus, "EX-A+++" },
            { RankList.S_F, "S-F" },
            { RankList.S_FPlus, "S-F+" },
            { RankList.S_FPlusPlus, "S-F++" },
            { RankList.S_FPlusPlusPlus, "S-F+++" },
            { RankList.S_E, "S-E" },
            { RankList.S_EPlus, "S-E+" },
            { RankList.S_EPlusPlus, "S-E++" },
            { RankList.S_EPlusPlusPlus, "S-E+++" },
            { RankList.S_D, "S-D" },
            { RankList.S_DPlus, "S-D+" },
            { RankList.S_DPlusPlus, "S-D++" },
            { RankList.S_DPlusPlusPlus, "S-D+++" },
            { RankList.S_C, "S-C" },
            { RankList.S_CPlus, "S-C+" },
            { RankList.S_CPlusPlus, "S-C++" },
            { RankList.S_CPlusPlusPlus, "S-C+++" },
            { RankList.S_B, "S-B" },
            { RankList.S_BPlus, "S-B+" },
            { RankList.S_BPlusPlus, "S-B++" },
            { RankList.S_BPlusPlusPlus, "S-B+++" },
            { RankList.S_A, "S-A" },
            { RankList.S_APlus, "S-A+" },
            { RankList.S_APlusPlus, "S-A++" },
            { RankList.S_APlusPlusPlus, "S-A+++" },
            { RankList.Z_F, "Z-F" },
            { RankList.Z_FPlus, "Z-F+" },
            { RankList.Z_FPlusPlus, "Z-F++" },
            { RankList.Z_FPlusPlusPlus, "Z-F+++" },
            { RankList.Z_E, "Z-E" },
            { RankList.Z_EPlus, "Z-E+" },
            { RankList.Z_EPlusPlus, "Z-E++" },
            { RankList.Z_EPlusPlusPlus, "Z-E+++" },
            { RankList.Z_D, "Z-D" },
            { RankList.Z_DPlus, "Z-D+" },
            { RankList.Z_DPlusPlus, "Z-D++" },
            { RankList.Z_DPlusPlusPlus, "Z-D+++" },
            { RankList.Z_C, "Z-C" },
            { RankList.Z_CPlus, "Z-C+" },
            { RankList.Z_CPlusPlus, "Z-C++" },
            { RankList.Z_CPlusPlusPlus, "Z-C+++" },
            { RankList.Z_B, "Z-B" },
            { RankList.Z_BPlus, "Z-B+" },
            { RankList.Z_BPlusPlus, "Z-B++" },
            { RankList.Z_BPlusPlusPlus, "Z-B+++" },
            { RankList.Z_A, "Z-A" },
            { RankList.Z_APlus, "Z-A+" },
            { RankList.Z_APlusPlus, "Z-A++" },
            { RankList.Z_APlusPlusPlus, "Z-A+++" },
        };

        /// <summary>
        /// Calculates the value of all Physical, Mental, and Supernatural attributes for the provided character.
        /// </summary>
        /// <param name="character">Represents the character whose attributes are being calculated.</param>
        /// <param name="bodyMod">Represents the chosen Body Mod Supplement that should be used for calculations.</param>
        /// <param name="jumpList">Represents the list of Jumps to pull purchase attributes from.</param>
        public static void AttributeCalculation(Character character, int characterIndex, Options.BodyModSupplements bodyMod, List<Jump> jumpList)
        {
            List<PurchaseAttribute> physicalAttributeSources = [];
            List<PurchaseAttribute> mentalAttributeSources = [];
            List<PurchaseAttribute> supernaturalAttributeSources = [];

            switch (bodyMod)
            {
                case Options.BodyModSupplements.Generic:
                    foreach (SupplementPurchase purchase in character.BodyMod.Purchases)
                    {
                        foreach (PurchaseAttribute attribute in purchase.Attributes)
                        {
                            if (attribute.Type == "Attribute")
                            {
                                switch (attribute.Category)
                                {
                                    case "Physical":
                                        physicalAttributeSources.Add(attribute);
                                        break;
                                    case "Mental":
                                        mentalAttributeSources.Add(attribute);
                                        break;
                                    case "Supernatural":
                                        supernaturalAttributeSources.Add(attribute);
                                        break;
                                    default:
                                        break;
                                }

                                attribute.SourcePurchase = purchase.Name;
                                attribute.SourceJump = "Generic Body Mod";
                            }
                        }
                    }
                    break;
                case Options.BodyModSupplements.SBBodyMod:
                    foreach (SupplementPurchase purchase in character.BodyMod.SBPowerList)
                    {
                        foreach (PurchaseAttribute attribute in purchase.Attributes)
                        {
                            if (attribute.Type == "Attribute")
                            {
                                switch (attribute.Category)
                                {
                                    case "Physical":
                                        physicalAttributeSources.Add(attribute);
                                        break;
                                    case "Mental":
                                        mentalAttributeSources.Add(attribute);
                                        break;
                                    case "Supernatural":
                                        supernaturalAttributeSources.Add(attribute);
                                        break;
                                    default:
                                        break;
                                }

                                attribute.SourcePurchase = purchase.Name;
                                attribute.SourceJump = "SB Body Mod";
                            }
                        }
                    }

                    foreach (AugmentPurchase augment in character.BodyMod.BodyAugmentList)
                    {
                        foreach (PurchaseAttribute attribute in augment.Attributes)
                        {
                            if (attribute.Type == "Attribute")
                            {
                                switch (attribute.Category)
                                {
                                    case "Physical":
                                        physicalAttributeSources.Add(attribute);
                                        break;
                                    case "Mental":
                                        mentalAttributeSources.Add(attribute);
                                        break;
                                    case "Supernatural":
                                        supernaturalAttributeSources.Add(attribute);
                                        break;
                                    default:
                                        break;
                                }

                                attribute.SourcePurchase = augment.Name;
                                attribute.SourceJump = "SB Body Mod";
                            }
                        }
                    }

                    foreach (AugmentPurchase augment in character.BodyMod.MindAugmentList)
                    {
                        foreach (PurchaseAttribute attribute in augment.Attributes)
                        {
                            if (attribute.Type == "Attribute")
                            {
                                switch (attribute.Category)
                                {
                                    case "Physical":
                                        physicalAttributeSources.Add(attribute);
                                        break;
                                    case "Mental":
                                        mentalAttributeSources.Add(attribute);
                                        break;
                                    case "Supernatural":
                                        supernaturalAttributeSources.Add(attribute);
                                        break;
                                    default:
                                        break;
                                }

                                attribute.SourcePurchase = augment.Name;
                                attribute.SourceJump = "SB Body Mod";
                            }
                        }
                    }

                    foreach (AugmentPurchase augment in character.BodyMod.HeartAugmentList)
                    {
                        foreach (PurchaseAttribute attribute in augment.Attributes)
                        {
                            if (attribute.Type == "Attribute")
                            {
                                switch (attribute.Category)
                                {
                                    case "Physical":
                                        physicalAttributeSources.Add(attribute);
                                        break;
                                    case "Mental":
                                        mentalAttributeSources.Add(attribute);
                                        break;
                                    case "Supernatural":
                                        supernaturalAttributeSources.Add(attribute);
                                        break;
                                    default:
                                        break;
                                }

                                attribute.SourcePurchase = augment.Name;
                                attribute.SourceJump = "SB Body Mod";
                            }
                        }
                    }
                    break;
                case Options.BodyModSupplements.EssentialBodyMod:
                    foreach (SupplementPurchase purchase in character.BodyMod.EBMPurchaseList)
                    {
                        foreach (PurchaseAttribute attribute in purchase.Attributes)
                        {
                            if (attribute.Type == "Attribute")
                            {
                                switch (attribute.Category)
                                {
                                    case "Physical":
                                        physicalAttributeSources.Add(attribute);
                                        break;
                                    case "Mental":
                                        mentalAttributeSources.Add(attribute);
                                        break;
                                    case "Supernatural":
                                        supernaturalAttributeSources.Add(attribute);
                                        break;
                                    default:
                                        break;
                                }

                                attribute.SourcePurchase = purchase.Name;
                                attribute.SourceJump = "Essential Body Mod";
                            }
                        }
                    }
                    break;
                default:
                    break;
            }

            foreach (Jump jump in jumpList)
            {
                ListValidationClass.CheckBuildCount(jump, characterIndex);
                foreach (Purchase purchase in jump.Build[characterIndex].Purchase)
                {
                    if (!purchase.IsTemporary)
                    {
                        foreach (PurchaseAttribute attribute in purchase.Attributes)
                        {
                            if (attribute.Type == "Attribute")
                            {
                                switch (attribute.Category)
                                {
                                    case "Physical":
                                        physicalAttributeSources.Add(attribute);
                                        break;
                                    case "Mental":
                                        mentalAttributeSources.Add(attribute);
                                        break;
                                    case "Supernatural":
                                        supernaturalAttributeSources.Add(attribute);
                                        break;
                                    default:
                                        break;
                                }

                                attribute.SourcePurchase = purchase.Name;
                                attribute.SourceJump = jump.Name;
                            }
                        }
                    }
                }
            }

            Lookup<string, PurchaseAttribute> physicalAttributes = (Lookup<string, PurchaseAttribute>)physicalAttributeSources.ToLookup(x => x.Name);
            Lookup<string, PurchaseAttribute> mentalAttributes = (Lookup<string, PurchaseAttribute>)mentalAttributeSources.ToLookup(x => x.Name);
            Lookup<string, PurchaseAttribute> supernaturalAttributes = (Lookup<string, PurchaseAttribute>)supernaturalAttributeSources.ToLookup(x => x.Name);



            foreach (ProfileAttribute attribute in character.Attributes)
            {
                int attributeRankSum = 0;
                bool checkedHighestRank = false;
                List<PurchaseAttribute> attributeList = [];

                attribute.SourceList.Clear();

                switch (attribute.Category)
                {
                    case "Physical":
                        attributeList = [.. physicalAttributes[attribute.Name].OrderByDescending(x => (int)x.Rank)];

                        if (attribute.ModRank != RankList.None)
                        {
                            attributeList.Add(new(attribute.ModRank)
                            {
                                SourceJump = "Profile",
                                SourcePurchase = "Mod Rank"
                            });
                            attributeList = [.. attributeList.OrderByDescending(x => (int)x.Rank)];
                        }

                        foreach (PurchaseAttribute source in attributeList)
                        {
                            if (checkedHighestRank)
                            {
                                attributeRankSum += (int)source.Rank / 10;
                            }
                            else
                            {
                                attributeRankSum += (int)source.Rank;
                                checkedHighestRank = true;
                            }

                            attribute.SourceList.Add(new());
                            attribute.SourceList.Last().SourceJump = source.SourceJump;
                            attribute.SourceList.Last().SourcePurchase = source.SourcePurchase;
                            attribute.SourceList.Last().SourceRank = _rankListDisplayValues[source.Rank];
                        }
                        break;
                    case "Mental":
                        attributeList = [.. mentalAttributes[attribute.Name].OrderByDescending(x => (int)x.Rank)];

                        if (attribute.ModRank != RankList.None)
                        {
                            attributeList.Add(new(attribute.ModRank)
                            {
                                SourceJump = "Profile",
                                SourcePurchase = "Mod Rank"
                            });
                            attributeList = [.. attributeList.OrderByDescending(x => (int)x.Rank)];
                        }

                        foreach (PurchaseAttribute source in attributeList)
                        {
                            if (checkedHighestRank)
                            {
                                attributeRankSum += (int)source.Rank / 10;
                            }
                            else
                            {
                                attributeRankSum += (int)source.Rank;
                                checkedHighestRank = true;
                            }

                            attribute.SourceList.Add(new());
                            attribute.SourceList.Last().SourceJump = source.SourceJump;
                            attribute.SourceList.Last().SourcePurchase = source.SourcePurchase;
                            attribute.SourceList.Last().SourceRank = _rankListDisplayValues[source.Rank];
                        }
                        break;
                    case "Supernatural":
                        attributeList = [.. supernaturalAttributes[attribute.Name].OrderByDescending(x => x.Rank)];

                        if (attribute.ModRank != RankList.None)
                        {
                            attributeList.Add(new(attribute.ModRank)
                            {
                                SourceJump = "Profile",
                                SourcePurchase = "Mod Rank"
                            });
                            attributeList = [.. attributeList.OrderByDescending(x => (int)x.Rank)];
                        }

                        foreach (PurchaseAttribute source in attributeList)
                        {
                            if (checkedHighestRank)
                            {
                                attributeRankSum += (int)source.Rank / 10;
                            }
                            else
                            {
                                attributeRankSum += (int)source.Rank;
                                checkedHighestRank = true;
                            }

                            attribute.SourceList.Add(new());
                            attribute.SourceList.Last().SourceJump = source.SourceJump;
                            attribute.SourceList.Last().SourcePurchase = source.SourcePurchase;
                            attribute.SourceList.Last().SourceRank = _rankListDisplayValues[source.Rank];
                        }
                        break;
                    default:
                        break;
                }

                int finalRankNumber = Enum.GetValues(typeof(RankList)).Cast<int>().ToList().OrderBy(x => Math.Abs(x - attributeRankSum)).First();

                if (finalRankNumber < (int)attribute.BaseRank)
                {
                    finalRankNumber = (int)attribute.BaseRank;
                }

                attribute.Rank = (RankList)finalRankNumber;
                attribute.DisplayRank = _rankListDisplayValues[attribute.Rank];
            }
        }

        /// <summary>
        /// Calculates the value of all Physical, Mental, Social, Technological, and Supernatural skills for the provided character.
        /// </summary>
        /// <param name="character">Represents the character whose skills are being calculated.</param>
        /// <param name="bodyMod">Represents the chosen Body Mod Supplement that should be used for calculations.</param>
        /// <param name="jumpList">Represents the list of Jumps to pull purchase attributes from.</param>
        public static void SkillCalculation(Character character, int characterIndex, Options.BodyModSupplements bodyMod, List<Jump> jumpList)
        {
            List<PurchaseAttribute> physicalSkillSources = [];
            List<PurchaseAttribute> mentalSkillSources = [];
            List<PurchaseAttribute> socialSkillSources = [];
            List<PurchaseAttribute> technologicalSkillSources = [];
            List<PurchaseAttribute> supernaturalSkillSources = [];

            switch (bodyMod)
            {
                case Options.BodyModSupplements.Generic:
                    foreach (SupplementPurchase purchase in character.BodyMod.Purchases)
                    {
                        foreach (PurchaseAttribute attribute in purchase.Attributes)
                        {
                            if (attribute.Type == "Skill")
                            {
                                switch (attribute.Category)
                                {
                                    case "Physical":
                                        physicalSkillSources.Add(attribute);
                                        break;
                                    case "Mental":
                                        mentalSkillSources.Add(attribute);
                                        break;
                                    case "Social":
                                        socialSkillSources.Add(attribute);
                                        break;
                                    case "Technological":
                                        technologicalSkillSources.Add(attribute);
                                        break;
                                    case "Supernatural":
                                        supernaturalSkillSources.Add(attribute);
                                        break;
                                    default:
                                        break;
                                }

                                attribute.SourcePurchase = purchase.Name;
                                attribute.SourceJump = "Generic Body Mod";
                            }
                        }
                    }
                    break;
                case Options.BodyModSupplements.SBBodyMod:
                    foreach (SupplementPurchase purchase in character.BodyMod.SBPowerList)
                    {
                        foreach (PurchaseAttribute attribute in purchase.Attributes)
                        {
                            if (attribute.Type == "Skill")
                            {
                                switch (attribute.Category)
                                {
                                    case "Physical":
                                        physicalSkillSources.Add(attribute);
                                        break;
                                    case "Mental":
                                        mentalSkillSources.Add(attribute);
                                        break;
                                    case "Social":
                                        socialSkillSources.Add(attribute);
                                        break;
                                    case "Technological":
                                        technologicalSkillSources.Add(attribute);
                                        break;
                                    case "Supernatural":
                                        supernaturalSkillSources.Add(attribute);
                                        break;
                                    default:
                                        break;
                                }

                                attribute.SourcePurchase = purchase.Name;
                                attribute.SourceJump = "SB Body Mod";
                            }
                        }
                    }

                    foreach (AugmentPurchase augment in character.BodyMod.BodyAugmentList)
                    {
                        foreach (PurchaseAttribute attribute in augment.Attributes)
                        {
                            if (attribute.Type == "Skill")
                            {
                                switch (attribute.Category)
                                {
                                    case "Physical":
                                        physicalSkillSources.Add(attribute);
                                        break;
                                    case "Mental":
                                        mentalSkillSources.Add(attribute);
                                        break;
                                    case "Social":
                                        socialSkillSources.Add(attribute);
                                        break;
                                    case "Technological":
                                        technologicalSkillSources.Add(attribute);
                                        break;
                                    case "Supernatural":
                                        supernaturalSkillSources.Add(attribute);
                                        break;
                                    default:
                                        break;
                                }

                                attribute.SourcePurchase = augment.Name;
                                attribute.SourceJump = "SB Body Mod";
                            }
                        }
                    }

                    foreach (AugmentPurchase augment in character.BodyMod.MindAugmentList)
                    {
                        foreach (PurchaseAttribute attribute in augment.Attributes)
                        {
                            if (attribute.Type == "Skill")
                            {
                                switch (attribute.Category)
                                {
                                    case "Physical":
                                        physicalSkillSources.Add(attribute);
                                        break;
                                    case "Mental":
                                        mentalSkillSources.Add(attribute);
                                        break;
                                    case "Social":
                                        socialSkillSources.Add(attribute);
                                        break;
                                    case "Technological":
                                        technologicalSkillSources.Add(attribute);
                                        break;
                                    case "Supernatural":
                                        supernaturalSkillSources.Add(attribute);
                                        break;
                                    default:
                                        break;
                                }

                                attribute.SourcePurchase = augment.Name;
                                attribute.SourceJump = "SB Body Mod";
                            }
                        }
                    }

                    foreach (AugmentPurchase augment in character.BodyMod.HeartAugmentList)
                    {
                        foreach (PurchaseAttribute attribute in augment.Attributes)
                        {
                            if (attribute.Type == "Skill")
                            {
                                switch (attribute.Category)
                                {
                                    case "Physical":
                                        physicalSkillSources.Add(attribute);
                                        break;
                                    case "Mental":
                                        mentalSkillSources.Add(attribute);
                                        break;
                                    case "Social":
                                        socialSkillSources.Add(attribute);
                                        break;
                                    case "Technological":
                                        technologicalSkillSources.Add(attribute);
                                        break;
                                    case "Supernatural":
                                        supernaturalSkillSources.Add(attribute);
                                        break;
                                    default:
                                        break;
                                }

                                attribute.SourcePurchase = augment.Name;
                                attribute.SourceJump = "SB Body Mod";
                            }
                        }
                    }
                    break;
                case Options.BodyModSupplements.EssentialBodyMod:
                    foreach (SupplementPurchase purchase in character.BodyMod.EBMPurchaseList)
                    {
                        foreach (PurchaseAttribute attribute in purchase.Attributes)
                        {
                            if (attribute.Type == "Skill")
                            {
                                switch (attribute.Category)
                                {
                                    case "Physical":
                                        physicalSkillSources.Add(attribute);
                                        break;
                                    case "Mental":
                                        mentalSkillSources.Add(attribute);
                                        break;
                                    case "Social":
                                        socialSkillSources.Add(attribute);
                                        break;
                                    case "Technological":
                                        technologicalSkillSources.Add(attribute);
                                        break;
                                    case "Supernatural":
                                        supernaturalSkillSources.Add(attribute);
                                        break;
                                    default:
                                        break;
                                }

                                attribute.SourcePurchase = purchase.Name;
                                attribute.SourceJump = "Essential Body Mod";
                            }
                        }
                    }
                    break;
                default:
                    break;
            }

            foreach (Jump jump in jumpList)
            {
                ListValidationClass.CheckBuildCount(jump, characterIndex);
                foreach (Purchase purchase in jump.Build[characterIndex].Purchase)
                {
                    if (!purchase.IsTemporary)
                    {
                        foreach (PurchaseAttribute attribute in purchase.Attributes)
                        {
                            if (attribute.Type == "Skill")
                            {
                                switch (attribute.Category)
                                {
                                    case "Physical":
                                        physicalSkillSources.Add(attribute);
                                        break;
                                    case "Mental":
                                        mentalSkillSources.Add(attribute);
                                        break;
                                    case "Social":
                                        socialSkillSources.Add(attribute);
                                        break;
                                    case "Technological":
                                        technologicalSkillSources.Add(attribute);
                                        break;
                                    case "Supernatural":
                                        supernaturalSkillSources.Add(attribute);
                                        break;
                                    default:
                                        break;
                                }

                                attribute.SourcePurchase = purchase.Name;
                                attribute.SourceJump = jump.Name;
                            }
                        }
                    }
                }
            }

            Lookup<string, PurchaseAttribute> physicalSkills = (Lookup<string, PurchaseAttribute>)physicalSkillSources.ToLookup(x => x.Name);
            Lookup<string, PurchaseAttribute> mentalSkills = (Lookup<string, PurchaseAttribute>)mentalSkillSources.ToLookup(x => x.Name);
            Lookup<string, PurchaseAttribute> socialSkills = (Lookup<string, PurchaseAttribute>)socialSkillSources.ToLookup(x => x.Name);
            Lookup<string, PurchaseAttribute> technologicalSkills = (Lookup<string, PurchaseAttribute>)technologicalSkillSources.ToLookup(x => x.Name);
            Lookup<string, PurchaseAttribute> supernaturalSkills = (Lookup<string, PurchaseAttribute>)supernaturalSkillSources.ToLookup(x => x.Name);



            foreach (ProfileAttribute skill in character.Skills)
            {
                int skillRankSum = 0;
                bool checkedHighestRank = false;
                List<PurchaseAttribute> skillList = [];

                skill.SourceList.Clear();

                switch (skill.Category)
                {
                    case "Physical":
                        skillList = [.. physicalSkills[skill.Name].OrderByDescending(x => (int)x.Rank)];

                        if (skill.ModRank != RankList.None)
                        {
                            skillList.Add(new(skill.ModRank)
                            {
                                SourceJump = "Profile",
                                SourcePurchase = "Mod Rank"
                            });
                            skillList = [.. skillList.OrderByDescending(x => (int)x.Rank)];
                        }

                        foreach (PurchaseAttribute source in skillList)
                        {
                            if (checkedHighestRank)
                            {
                                skillRankSum += (int)source.Rank / 10;
                            }
                            else
                            {
                                skillRankSum += (int)source.Rank;
                                checkedHighestRank = true;
                            }

                            skill.SourceList.Add(new());
                            skill.SourceList.Last().SourceJump = source.SourceJump;
                            skill.SourceList.Last().SourcePurchase = source.SourcePurchase;
                            skill.SourceList.Last().SourceRank = _rankListDisplayValues[source.Rank];
                        }
                        break;
                    case "Mental":
                        skillList = [.. mentalSkills[skill.Name].OrderByDescending(x => (int)x.Rank)];

                        if (skill.ModRank != RankList.None)
                        {
                            skillList.Add(new(skill.ModRank)
                            {
                                SourceJump = "Profile",
                                SourcePurchase = "Mod Rank"
                            });
                            skillList = [.. skillList.OrderByDescending(x => (int)x.Rank)];
                        }

                        foreach (PurchaseAttribute source in skillList)
                        {
                            if (checkedHighestRank)
                            {
                                skillRankSum += (int)source.Rank / 10;
                            }
                            else
                            {
                                skillRankSum += (int)source.Rank;
                                checkedHighestRank = true;
                            }

                            skill.SourceList.Add(new());
                            skill.SourceList.Last().SourceJump = source.SourceJump;
                            skill.SourceList.Last().SourcePurchase = source.SourcePurchase;
                            skill.SourceList.Last().SourceRank = _rankListDisplayValues[source.Rank];
                        }
                        break;
                    case "Social":
                        skillList = [.. socialSkills[skill.Name].OrderByDescending(x => (int)x.Rank)];

                        if (skill.ModRank != RankList.None)
                        {
                            skillList.Add(new(skill.ModRank)
                            {
                                SourceJump = "Profile",
                                SourcePurchase = "Mod Rank"
                            });
                            skillList = [.. skillList.OrderByDescending(x => (int)x.Rank)];
                        }

                        foreach (PurchaseAttribute source in skillList)
                        {
                            if (checkedHighestRank)
                            {
                                skillRankSum += (int)source.Rank / 10;
                            }
                            else
                            {
                                skillRankSum += (int)source.Rank;
                                checkedHighestRank = true;
                            }

                            skill.SourceList.Add(new());
                            skill.SourceList.Last().SourceJump = source.SourceJump;
                            skill.SourceList.Last().SourcePurchase = source.SourcePurchase;
                            skill.SourceList.Last().SourceRank = _rankListDisplayValues[source.Rank];
                        }
                        break;
                    case "Technological":
                        skillList = [.. technologicalSkills[skill.Name].OrderByDescending(x => (int)x.Rank)];

                        if (skill.ModRank != RankList.None)
                        {
                            skillList.Add(new(skill.ModRank)
                            {
                                SourceJump = "Profile",
                                SourcePurchase = "Mod Rank"
                            });
                            skillList = [.. skillList.OrderByDescending(x => (int)x.Rank)];
                        }

                        foreach (PurchaseAttribute source in skillList)
                        {
                            if (checkedHighestRank)
                            {
                                skillRankSum += (int)source.Rank / 10;
                            }
                            else
                            {
                                skillRankSum += (int)source.Rank;
                                checkedHighestRank = true;
                            }

                            skill.SourceList.Add(new());
                            skill.SourceList.Last().SourceJump = source.SourceJump;
                            skill.SourceList.Last().SourcePurchase = source.SourcePurchase;
                            skill.SourceList.Last().SourceRank = _rankListDisplayValues[source.Rank];
                        }
                        break;
                    case "Supernatural":
                        skillList = [.. supernaturalSkills[skill.Name].OrderByDescending(x => x.Rank)];

                        if (skill.ModRank != RankList.None)
                        {
                            skillList.Add(new(skill.ModRank)
                            {
                                SourceJump = "Profile",
                                SourcePurchase = "Mod Rank"
                            });
                            skillList = [.. skillList.OrderByDescending(x => (int)x.Rank)];
                        }

                        foreach (PurchaseAttribute source in skillList)
                        {
                            if (checkedHighestRank)
                            {
                                skillRankSum += (int)source.Rank / 10;
                            }
                            else
                            {
                                skillRankSum += (int)source.Rank;
                                checkedHighestRank = true;
                            }

                            skill.SourceList.Add(new());
                            skill.SourceList.Last().SourceJump = source.SourceJump;
                            skill.SourceList.Last().SourcePurchase = source.SourcePurchase;
                            skill.SourceList.Last().SourceRank = _rankListDisplayValues[source.Rank];
                        }
                        break;
                    default:
                        break;
                }

                int finalRankNumber = Enum.GetValues(typeof(RankList)).Cast<int>().ToList().OrderBy(x => Math.Abs(x - skillRankSum)).First();

                if (finalRankNumber < (int)skill.BaseRank)
                {
                    finalRankNumber = (int)skill.BaseRank;
                }

                skill.Rank = (RankList)finalRankNumber;
                skill.DisplayRank = _rankListDisplayValues[skill.Rank];
            }
        }


        public static void BoosterCalculation(Character character, int characterIndex, Options.BodyModSupplements bodyMod, List<Jump> jumpList)
        {
            List<PurchaseAttribute> boosterSources = [];

            switch (bodyMod)
            {
                case Options.BodyModSupplements.Generic:
                    foreach (SupplementPurchase purchase in character.BodyMod.Purchases)
                    {
                        foreach (PurchaseAttribute attribute in purchase.Attributes)
                        {
                            if (attribute.Type == "Booster")
                            {
                                boosterSources.Add(attribute);

                                attribute.SourcePurchase = purchase.Name;
                                attribute.SourceJump = "Generic Body Mod";
                            }
                        }
                    }
                    break;
                case Options.BodyModSupplements.SBBodyMod:
                    foreach (SupplementPurchase purchase in character.BodyMod.SBPowerList)
                    {
                        foreach (PurchaseAttribute attribute in purchase.Attributes)
                        {
                            if (attribute.Type == "Booster")
                            {
                                boosterSources.Add(attribute);

                                attribute.SourcePurchase = purchase.Name;
                                attribute.SourceJump = "SB Body Mod";
                            }
                        }
                    }

                    foreach (AugmentPurchase augment in character.BodyMod.BodyAugmentList)
                    {
                        foreach (PurchaseAttribute attribute in augment.Attributes)
                        {
                            if (attribute.Type == "Booster")
                            {
                                boosterSources.Add(attribute);

                                attribute.SourcePurchase = augment.Name;
                                attribute.SourceJump = "SB Body Mod";
                            }
                        }
                    }

                    foreach (AugmentPurchase augment in character.BodyMod.MindAugmentList)
                    {
                        foreach (PurchaseAttribute attribute in augment.Attributes)
                        {
                            if (attribute.Type == "Booster")
                            {
                                boosterSources.Add(attribute);

                                attribute.SourcePurchase = augment.Name;
                                attribute.SourceJump = "SB Body Mod";
                            }
                        }
                    }

                    foreach (AugmentPurchase augment in character.BodyMod.HeartAugmentList)
                    {
                        foreach (PurchaseAttribute attribute in augment.Attributes)
                        {
                            if (attribute.Type == "Booster")
                            {
                                boosterSources.Add(attribute);

                                attribute.SourcePurchase = augment.Name;
                                attribute.SourceJump = "SB Body Mod";
                            }
                        }
                    }
                    break;
                case Options.BodyModSupplements.EssentialBodyMod:
                    foreach (SupplementPurchase purchase in character.BodyMod.EBMPurchaseList)
                    {
                        foreach (PurchaseAttribute attribute in purchase.Attributes)
                        {
                            if (attribute.Type == "Booster")
                            {
                                boosterSources.Add(attribute);

                                attribute.SourcePurchase = purchase.Name;
                                attribute.SourceJump = "Essential Body Mod";
                            }
                        }
                    }
                    break;
                default:
                    break;
            }

            foreach (Jump jump in jumpList)
            {
                ListValidationClass.CheckBuildCount(jump, characterIndex);
                foreach (Purchase purchase in jump.Build[characterIndex].Purchase)
                {
                    if (!purchase.IsTemporary)
                    {
                        foreach (PurchaseAttribute attribute in purchase.Attributes)
                        {
                            if (attribute.Type == "Booster")
                            {
                                boosterSources.Add(attribute);

                                attribute.SourcePurchase = purchase.Name;
                                attribute.SourceJump = jump.Name;
                            }
                        }
                    }
                }
            }

            Lookup<string, PurchaseAttribute> boosterAttributes = (Lookup<string, PurchaseAttribute>)boosterSources.ToLookup(x => x.Name);



            foreach (Booster booster in character.Boosters)
            {
                int attributeMultiplierSum = 1;
                List<PurchaseAttribute> attributeList = [];

                booster.SourceList.Clear();

                attributeList = [.. boosterAttributes[booster.BoosterName].OrderByDescending(x => x.Value)];

                foreach (PurchaseAttribute source in attributeList)
                {
                    attributeMultiplierSum *= source.Value;

                    booster.SourceList.Add(new());
                    booster.SourceList.Last().SourceJump = source.SourceJump;
                    booster.SourceList.Last().SourcePurchase = source.SourcePurchase;
                    booster.SourceList.Last().SourceRank = source.Value.ToString();
                }

                booster.BoosterRaw = attributeMultiplierSum;
                booster.BoosterMultiplier = attributeMultiplierSum;
            }

            foreach (Booster booster in character.Boosters)
            {
                foreach (BoosterDependency dependency in booster.BoosterDependencies)
                {
                    if (booster.BoosterDependencies.IndexOf(dependency) < character.Boosters.IndexOf(booster))
                    {
                        dependency.DependencyMultiplier = character.Boosters[booster.BoosterDependencies.IndexOf(dependency)].BoosterRaw;
                    }
                    else if (booster.BoosterDependencies.IndexOf(dependency) >= character.Boosters.IndexOf(booster))
                    {
                        dependency.DependencyMultiplier = character.Boosters[booster.BoosterDependencies.IndexOf(dependency) + 1].BoosterRaw;
                    }

                    if (dependency.DependencyEnabled)
                    {
                        booster.BoosterMultiplier *= dependency.DependencyMultiplier;
                    }
                }
            }
        }
    }
}
