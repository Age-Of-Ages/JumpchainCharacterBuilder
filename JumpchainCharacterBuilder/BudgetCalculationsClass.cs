using JumpchainCharacterBuilder.Model;
using System.Collections.Generic;

namespace JumpchainCharacterBuilder
{
    public static class BudgetCalculationsClass
    {
        /// <summary>
        /// Calculates the budget available to output to the current display.
        /// </summary>
        /// <param name="jump">Represents the current Jump object loaded.</param>
        /// <param name="purchaseType">Represents the type of purchase 
        /// that the user is viewing.</param>
        /// <param name="characterIndex">Represents the index of the character that
        /// the user is currently viewing.</param>
        /// <returns>Returns the remaining budget as an integer.</returns>
        public static int JumpBudgetCalculation(Jump jump, PurchaseType purchaseType,
            int characterIndex, DrawbackSupplementUniversal drawbackSupplement, int jumpIndex)
        {
            int budget = 0;
            int importStipend = 0;
            int purchaseExpenses = 0;
            int currencyIndex = purchaseType.CurrencyIndex;

            int drawbackSupplementCP = 0;
            int drawbackSupplementItemCP = 0;
            int drawbackSupplementCompanionCP = 0;

            bool isGauntlet = jump.IsGauntlet;
            bool supplementPointsAllowed = (!isGauntlet || drawbackSupplement.AllowedDuringGauntlets);
            int supplementValueModifier = isGauntlet && drawbackSupplement.HalvedPointsDuringGauntlets ? 2 : 1;

            bool drawbackApply;

            if (currencyIndex == 0)
            {
                if (supplementPointsAllowed)
                {
                    foreach (DrawbackSupplementPurchase drawback in drawbackSupplement.Purchases)
                    {
                        drawbackApply = (!isGauntlet || drawback.ApplyGauntlet);

                        if (drawback.Revoke == 0 || drawback.Revoke > jump.JumpNumber)
                        {
                            if (jumpIndex >= drawback.SuspendList.Count)
                            {
                                ListValidationClass.CheckDrawbackSuspendCount(drawback, jumpIndex + 1);
                            }
                            if (!drawback.SuspendList[jumpIndex].Suspended && drawbackApply)
                            {
                                drawbackSupplementCP += drawback.ValueChoicePoints;
                                drawbackSupplementItemCP += drawback.ValueItemPoints;
                                drawbackSupplementCompanionCP += drawback.ValueCompanionPoints;
                            }
                        }
                    }

                    drawbackSupplementCP /= supplementValueModifier;
                    drawbackSupplementItemCP /= supplementValueModifier;
                    drawbackSupplementCompanionCP /= supplementValueModifier;
                }
            }

            JumpBuild jumpBuild;

            ListValidationClass.CheckBuildCount(jump, characterIndex);
            jumpBuild = jump.Build[characterIndex];
            int originIndex = jumpBuild.OriginIndex;

            if (characterIndex > 0)
            {
                foreach (JumpBuild build in jump.Build)
                {
                    foreach (CompanionPurchase companionPurchase in build.CompanionPurchase)
                    {
                        importStipend += companionPurchase.CompanionImportDetails[characterIndex - 1].CompanionOptionValue;
                    }
                }
                budget += drawbackSupplementCompanionCP;
            }
            else
            {
                budget += drawbackSupplementCP;
                budget -= jumpBuild.WarehouseInvestment;
            }

            budget -= jumpBuild.BodyModInvestment;

            budget += jumpBuild.BankUsage;
            budget -= jumpBuild.BankedPoints;

            if (currencyIndex == 0 && purchaseType.Type != "Items")
            {
                int itemExpenses = 0;
                if (characterIndex == 0)
                {
                    if (!isGauntlet)
                    {
                        budget += jump.Currencies[currencyIndex].CurrencyBudget;
                    }
                    budget += (jumpBuild.PointStipend[currencyIndex] + importStipend);
                    if (supplementPointsAllowed)
                    {
                        itemExpenses -= drawbackSupplementItemCP;
                    }
                }
                else
                {
                    budget += jumpBuild.PointStipend[currencyIndex] + importStipend;
                }

                budget += BudgetHelpers.CalculateDrawbackTotals(jumpBuild.DrawbackSelection);
                budget += BudgetHelpers.CalculateDrawbackTotals(jumpBuild.ScenarioSelection);

                foreach (Purchase purchase in jumpBuild.Purchase)
                {
                    bool originMatches = jump.OriginDiscounts && purchase.AssociatedOriginIndex == jumpBuild.OriginIndex && jumpBuild.OriginIndex != 0;

                    if (purchase.TypeIndex == 1)
                    {
                        BudgetHelpers.CalculatePurchaseCost(purchase, ref itemExpenses, originMatches, jump.OriginItemFreebieThreshold);
                    }
                    else if (purchase.TypeIndex == 0 || jump.PurchaseTypes[purchase.TypeIndex].CurrencyIndex == 0)
                    {
                        BudgetHelpers.CalculatePurchaseCost(purchase, ref purchaseExpenses, originMatches, jump.OriginPerkFreebieThreshold);
                    }
                }
                itemExpenses -= jumpBuild.ItemStipend;
                if (itemExpenses < 0)
                {
                    itemExpenses = 0;
                }
                budget -= itemExpenses;
                budget -= purchaseExpenses;

                if (jumpBuild.Species != null)
                {
                    budget -= jumpBuild.Species.Cost;
                }
                if (jumpBuild.Location != null)
                {
                    budget -= jumpBuild.Location.Cost;
                }
                budget -= jumpBuild.AgeCost;
                budget -= jumpBuild.GenderCost;
                if (originIndex != 0)
                {
                    budget -= jump.OriginDetails[originIndex].Cost;
                }

                if (jumpBuild.MiscOriginDetails.Count > 0)
                {
                    foreach (OriginDetail originDetail in jumpBuild.MiscOriginDetails)
                    {
                        budget -= originDetail.Cost;
                    }
                }

                budget -= BudgetHelpers.CalculateCompanionPurchasesCost(jumpBuild.CompanionPurchase);

                return budget;
            }
            else if (currencyIndex == 0 && purchaseType.Type == "Items")
            {
                if (characterIndex == 0)
                {
                    if (!isGauntlet)
                    {
                        budget += jump.Currencies[currencyIndex].CurrencyBudget;
                    }
                    budget += (jumpBuild.PointStipend[currencyIndex] + jumpBuild.ItemStipend + importStipend);
                    if (supplementPointsAllowed)
                    {
                        budget += drawbackSupplementItemCP;
                    }
                }
                else
                {
                    budget += jumpBuild.PointStipend[currencyIndex] + jumpBuild.ItemStipend + importStipend;
                }

                budget += BudgetHelpers.CalculateDrawbackTotals(jumpBuild.DrawbackSelection);
                budget += BudgetHelpers.CalculateDrawbackTotals(jumpBuild.ScenarioSelection);

                foreach (Purchase purchase in jumpBuild.Purchase)
                {
                    bool originMatches = jump.OriginDiscounts && purchase.AssociatedOriginIndex == jumpBuild.OriginIndex && jumpBuild.OriginIndex != 0;

                    if (purchase.TypeIndex == 1)
                    {
                        BudgetHelpers.CalculatePurchaseCost(purchase, ref purchaseExpenses, originMatches, jump.OriginItemFreebieThreshold);
                    }
                    else if (purchase.TypeIndex == 0 || jump.PurchaseTypes[purchase.TypeIndex].CurrencyIndex == 0)
                    {
                        BudgetHelpers.CalculatePurchaseCost(purchase, ref purchaseExpenses, originMatches, jump.OriginPerkFreebieThreshold);
                    }
                }
                budget -= purchaseExpenses;

                if (jumpBuild.Species != null)
                {
                    budget -= jumpBuild.Species.Cost;
                }
                if (jumpBuild.Location != null)
                {
                    budget -= jumpBuild.Location.Cost;
                }
                budget -= jumpBuild.AgeCost;
                budget -= jumpBuild.GenderCost;
                if (originIndex != 0)
                {
                    budget -= jump.OriginDetails[originIndex].Cost;
                }

                if (jumpBuild.MiscOriginDetails.Count > 0)
                {
                    foreach (OriginDetail originDetail in jumpBuild.MiscOriginDetails)
                    {
                        budget -= originDetail.Cost;
                    }
                }

                budget -= BudgetHelpers.CalculateCompanionPurchasesCost(jumpBuild.CompanionPurchase);

                return budget;
            }
            else
            {
                if (characterIndex == 0)
                {
                    if (!isGauntlet)
                    {
                        budget += jump.Currencies[currencyIndex].CurrencyBudget;
                    }
                    budget += (jumpBuild.PointStipend[currencyIndex]);
                }
                else
                {
                    budget += jumpBuild.PointStipend[currencyIndex];
                }

                foreach (Purchase purchase in jumpBuild.Purchase)
                {
                    if (jump.PurchaseTypes[purchase.TypeIndex].CurrencyIndex == purchaseType.CurrencyIndex)
                    {
                        bool originMatches = jump.OriginDiscounts && purchase.AssociatedOriginIndex == jumpBuild.OriginIndex && jumpBuild.OriginIndex != 0;

                        if (jump.PurchaseTypes[purchase.TypeIndex].IsItemType)
                        {
                            BudgetHelpers.CalculatePurchaseCost(purchase, ref purchaseExpenses, originMatches, jump.OriginItemFreebieThreshold);
                        }
                        else
                        {
                            BudgetHelpers.CalculatePurchaseCost(purchase, ref purchaseExpenses, originMatches, jump.OriginPerkFreebieThreshold);
                        }
                    }
                }
                budget -= purchaseExpenses;
                return budget;
            }

        }

        public static int WarehouseBudgetCalculation(WarehouseUniversal warehouse, int totalWP)
        {
            int budget = totalWP;

            foreach (SupplementPurchase purchase in warehouse.Purchases)
            {
                if (!purchase.Freebie)
                {
                    if (purchase.Discounted)
                    {
                        budget -= purchase.Cost / 2;
                        purchase.DisplayCost = purchase.Cost / 2;
                    }
                    else
                    {
                        budget -= purchase.Cost;
                        purchase.DisplayCost = purchase.Cost;
                    }
                }
                else
                {
                    purchase.DisplayCost = 0;
                }
            }

            return budget;
        }

        public static int BodyModBudgetCalculation(BodyModUniversal bodyMod, int totalBP, Options.BodyModSupplements supplement)
        {
            int budget = totalBP;

            switch (supplement)
            {
                case Options.BodyModSupplements.Generic:
                    foreach (SupplementPurchase purchase in bodyMod.Purchases)
                    {
                        if (purchase.Category == "Generic")
                        {
                            if (!purchase.Freebie)
                            {
                                if (purchase.Discounted)
                                {
                                    budget -= purchase.Cost / 2;
                                    purchase.DisplayCost = purchase.Cost / 2;
                                }
                                else
                                {
                                    budget -= purchase.Cost;
                                    purchase.DisplayCost = purchase.Cost;
                                }
                            }
                            else
                            {
                                purchase.DisplayCost = 0;
                            }
                        }
                    }
                    break;
                case Options.BodyModSupplements.SBBodyMod:
                    int anomalousStipend = bodyMod.AnomalousLevel * 100;
                    int baseFormTotal = 0;
                    if (bodyMod.AffinityIndex > 0)
                    {
                        budget -= 100;
                    }

                    foreach (SupplementPurchase purchase in bodyMod.BaseFormDetails)
                    {
                        if (!purchase.Freebie)
                        {
                            if (purchase.Discounted)
                            {
                                baseFormTotal += purchase.Cost / 2;
                                purchase.DisplayCost = purchase.Cost / 2;
                            }
                            else
                            {
                                baseFormTotal += purchase.Cost;
                                purchase.DisplayCost = purchase.Cost;
                            }
                        }
                        else
                        {
                            purchase.DisplayCost = 0;
                        }
                    }

                    foreach (SupplementPurchase purchase in bodyMod.ExtraBitsList)
                    {
                        if (!purchase.Freebie)
                        {
                            if (purchase.Discounted)
                            {
                                baseFormTotal += purchase.Cost / 2;
                                purchase.DisplayCost = purchase.Cost / 2;
                            }
                            else
                            {
                                baseFormTotal += purchase.Cost;
                                purchase.DisplayCost = purchase.Cost;
                            }
                        }
                        else
                        {
                            purchase.DisplayCost = 0;
                        }
                    }

                    baseFormTotal -= anomalousStipend;
                    if (baseFormTotal < 0)
                    {
                        baseFormTotal = 0;
                    }
                    budget -= baseFormTotal;

                    foreach (SupplementPurchase purchase in bodyMod.BodyAugmentList)
                    {
                        if (!purchase.Freebie)
                        {
                            if (purchase.Discounted)
                            {
                                if (purchase.Cost > 50)
                                {
                                    budget -= purchase.Cost / 2;
                                    purchase.DisplayCost = purchase.Cost / 2;
                                }
                                else
                                {
                                    purchase.DisplayCost = 0;
                                }
                            }
                            else
                            {
                                if (bodyMod.AscensionDiscount)
                                {
                                    if (purchase.Cost > 50)
                                    {
                                        budget -= purchase.Cost / 2;
                                        purchase.DisplayCost = purchase.Cost / 2;
                                    }
                                    else
                                    {
                                        purchase.DisplayCost = 0;
                                    }
                                }
                                else
                                {
                                    if (bodyMod.AffinityIndex == 1)
                                    {
                                        if (purchase.Cost > 50)
                                        {
                                            budget -= purchase.Cost / 2;
                                            purchase.DisplayCost = purchase.Cost / 2;
                                        }
                                        else
                                        {
                                            purchase.DisplayCost = 0;
                                        }
                                    }
                                    else
                                    {
                                        budget -= purchase.Cost;
                                        purchase.DisplayCost = purchase.Cost;
                                    }
                                }
                            }
                        }
                        else
                        {
                            purchase.DisplayCost = 0;
                        }
                    }

                    foreach (SupplementPurchase purchase in bodyMod.MindAugmentList)
                    {
                        if (!purchase.Freebie)
                        {
                            if (purchase.Discounted)
                            {
                                if (purchase.Cost > 50)
                                {
                                    budget -= purchase.Cost / 2;
                                    purchase.DisplayCost = purchase.Cost / 2;
                                }
                                else
                                {
                                    purchase.DisplayCost = 0;
                                }
                            }
                            else
                            {
                                if (bodyMod.AscensionDiscount)
                                {
                                    if (purchase.Cost > 50)
                                    {
                                        budget -= purchase.Cost / 2;
                                        purchase.DisplayCost = purchase.Cost / 2;
                                    }
                                    else
                                    {
                                        purchase.DisplayCost = 0;
                                    }
                                }
                                else
                                {
                                    if (bodyMod.AffinityIndex == 2)
                                    {
                                        if (purchase.Cost > 50)
                                        {
                                            budget -= purchase.Cost / 2;
                                            purchase.DisplayCost = purchase.Cost / 2;
                                        }
                                        else
                                        {
                                            purchase.DisplayCost = 0;
                                        }
                                    }
                                    else
                                    {
                                        budget -= purchase.Cost;
                                        purchase.DisplayCost = purchase.Cost;
                                    }
                                }
                            }
                        }
                        else
                        {
                            purchase.DisplayCost = 0;
                        }
                    }

                    foreach (SupplementPurchase purchase in bodyMod.HeartAugmentList)
                    {
                        if (!purchase.Freebie)
                        {
                            if (purchase.Discounted)
                            {
                                if (purchase.Cost > 50)
                                {
                                    budget -= purchase.Cost / 2;
                                    purchase.DisplayCost = purchase.Cost / 2;
                                }
                                else
                                {
                                    purchase.DisplayCost = 0;
                                }
                            }
                            else
                            {
                                if (bodyMod.AscensionDiscount)
                                {
                                    if (purchase.Cost > 50)
                                    {
                                        budget -= purchase.Cost / 2;
                                        purchase.DisplayCost = purchase.Cost / 2;
                                    }
                                    else
                                    {
                                        purchase.DisplayCost = 0;
                                    }
                                }
                                else
                                {
                                    if (bodyMod.AffinityIndex == 3)
                                    {
                                        if (purchase.Cost > 50)
                                        {
                                            budget -= purchase.Cost / 2;
                                            purchase.DisplayCost = purchase.Cost / 2;
                                        }
                                        else
                                        {
                                            purchase.DisplayCost = 0;
                                        }
                                    }
                                    else
                                    {
                                        budget -= purchase.Cost;
                                        purchase.DisplayCost = purchase.Cost;
                                    }
                                }
                            }
                        }
                        else
                        {
                            purchase.DisplayCost = 0;
                        }
                    }

                    foreach (SupplementPurchase purchase in bodyMod.SBPowerList)
                    {
                        if (!purchase.Freebie)
                        {
                            if (purchase.Discounted)
                            {
                                budget -= purchase.Cost / 2;
                                purchase.DisplayCost = purchase.Cost / 2;
                            }
                            else
                            {
                                budget -= purchase.Cost;
                                purchase.DisplayCost = purchase.Cost;
                            }
                        }
                        else
                        {
                            purchase.DisplayCost = 0;
                        }
                    }
                    break;
                case Options.BodyModSupplements.EssentialBodyMod:
                    foreach (SupplementPurchase purchase in bodyMod.EBMPurchaseList)
                    {
                        if (!purchase.Freebie)
                        {
                            if (purchase.Discounted)
                            {
                                budget -= purchase.Cost / 2;
                                purchase.DisplayCost = purchase.Cost / 2;
                            }
                            else
                            {
                                budget -= purchase.Cost;
                                purchase.DisplayCost = purchase.Cost;
                            }
                        }
                        else
                        {
                            purchase.DisplayCost = 0;
                        }
                    }
                    break;
                default:
                    break;
            }

            return budget;
        }
    }

    public static class BudgetHelpers
    {
        public static void CalculatePurchaseCost(Purchase purchase, ref int sectionSubtotal, bool originMatches, int freebieThreshold)
        {
            if (!purchase.FreebieEnabled)
            {
                if (purchase.DiscountEnabled)
                {
                    sectionSubtotal += (purchase.Cost / 2);
                    purchase.DisplayCost = purchase.Cost / 2;
                }
                else
                {
                    if (originMatches)
                    {
                        if (purchase.Cost > freebieThreshold)
                        {
                            sectionSubtotal += (purchase.Cost / 2);
                            purchase.DisplayCost = purchase.Cost / 2;
                        }
                        else if (purchase.Cost < 0)
                        {
                            sectionSubtotal += purchase.Cost;
                            purchase.DisplayCost = purchase.Cost;
                        }
                        else
                        {
                            purchase.DisplayCost = 0;
                        }
                    }
                    else
                    {
                        sectionSubtotal += purchase.Cost;
                        purchase.DisplayCost = purchase.Cost;
                    }
                }
            }
            else
            {
                purchase.DisplayCost = 0;
            }
        }

        public static int CalculateDrawbackTotals(List<Drawback> drawbacks)
        {
            int total = 0;

            foreach (Drawback drawback in drawbacks)
            {
                total += drawback.Value;
            }

            return total;
        }

        public static int CalculateCompanionPurchasesCost(List<CompanionPurchase> purchases)
        {
            int totalCost = 0;

            foreach (CompanionPurchase purchase in purchases)
            {
                if (!purchase.FreebieEnabled)
                {
                    if (purchase.DiscountEnabled)
                    {
                        totalCost += (purchase.Cost / 2);
                        purchase.DisplayCost = purchase.Cost;
                    }
                    else
                    {
                        totalCost += purchase.Cost;
                        purchase.DisplayCost = purchase.Cost;
                    }
                }
                else
                {
                    purchase.DisplayCost = 0;
                }
            }

            return totalCost;
        }
    }
}
