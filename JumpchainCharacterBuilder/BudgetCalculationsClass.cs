using JumpchainCharacterBuilder.Model;

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
            bool supplementPointsAllowed = true;
            bool halvedSupplementPoints = false;
            if (isGauntlet && !drawbackSupplement.AllowedDuringGauntlets)
            {
                supplementPointsAllowed = false;
            }
            if (drawbackSupplement.HalvedPointsDuringGauntlets)
            {
                halvedSupplementPoints = true;
            }


            if (currencyIndex == 0)
            {
                if (isGauntlet)
                {
                    if (supplementPointsAllowed)
                    {
                        foreach (DrawbackSupplementPurchase drawback in drawbackSupplement.Purchases)
                        {
                            if (drawback.Revoke == 0 || drawback.Revoke > jumpIndex)
                            {
                                if (jumpIndex >= drawback.Suspend.Count)
                                {
                                    ListValidationClass.CheckDrawbackSuspendCount(drawback, jumpIndex + 1);
                                }
                                if (!drawback.Suspend[jumpIndex])
                                {
                                    if (drawback.ApplyGauntlet)
                                    {
                                        if (halvedSupplementPoints)
                                        {
                                            drawbackSupplementCP += drawback.ValueChoicePoints / 2;
                                            drawbackSupplementItemCP += drawback.ValueItemPoints / 2;
                                            drawbackSupplementCompanionCP += drawback.ValueCompanionPoints / 2;
                                        }
                                        else
                                        {
                                            drawbackSupplementCP += drawback.ValueChoicePoints;
                                            drawbackSupplementItemCP += drawback.ValueItemPoints;
                                            drawbackSupplementCompanionCP += drawback.ValueCompanionPoints;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    foreach (DrawbackSupplementPurchase drawback in drawbackSupplement.Purchases)
                    {
                        if (drawback.Revoke == 0 || drawback.Revoke > jumpIndex)
                        {
                            if (jumpIndex >= drawback.Suspend.Count)
                            {
                                ListValidationClass.CheckDrawbackSuspendCount(drawback, jumpIndex + 1);
                            }
                            if (!drawback.Suspend[jumpIndex])
                            {
                                drawbackSupplementCP += drawback.ValueChoicePoints;
                                drawbackSupplementItemCP += drawback.ValueItemPoints;
                                drawbackSupplementCompanionCP += drawback.ValueCompanionPoints;
                            }
                        }
                    }
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
                if (supplementPointsAllowed)
                {
                    budget += drawbackSupplementCompanionCP;
                }
            }
            else
            {
                if (supplementPointsAllowed)
                {
                    budget += drawbackSupplementCP;
                }
                budget -= jumpBuild.WarehouseInvestment;
            }

            budget -= jumpBuild.BodyModInvestment;

            budget += jumpBuild.BankUsage;
            budget -= jumpBuild.BankedPoints;

            // If Origin discounts are enabled then use a method that accounts for them in automation.
            if (jump.OriginDiscounts)
            {
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

                    foreach (Drawback drawback in jumpBuild.DrawbackSelection)
                    {
                        budget += drawback.Value;
                    }

                    foreach (Drawback scenario in jumpBuild.ScenarioSelection)
                    {
                        budget += scenario.Value;
                    }

                    foreach (Purchase purchase in jumpBuild.Purchase)
                    {
                        if (purchase.TypeIndex == 1)
                        {
                            if (!purchase.FreebieEnabled)
                            {
                                if (purchase.DiscountEnabled)
                                {
                                    itemExpenses += (purchase.Cost / 2);
                                    purchase.DisplayCost = purchase.Cost / 2;
                                }
                                else
                                {
                                    if (purchase.AssociatedOriginIndex == jumpBuild.OriginIndex && jumpBuild.OriginIndex != 0)
                                    {
                                        if (purchase.Cost > jump.OriginItemFreebieThreshold)
                                        {
                                            itemExpenses += (purchase.Cost / 2);
                                            purchase.DisplayCost = purchase.Cost / 2;
                                        }
                                        else if (purchase.Cost < 0)
                                        {
                                            itemExpenses += purchase.Cost;
                                            purchase.DisplayCost = purchase.Cost;
                                        }
                                        else
                                        {
                                            purchase.DisplayCost = 0;
                                        }
                                    }
                                    else
                                    {
                                        itemExpenses += purchase.Cost;
                                        purchase.DisplayCost = purchase.Cost;
                                    }
                                }
                            }
                            else
                            {
                                purchase.DisplayCost = 0;
                            }

                        }
                        else if (purchase.TypeIndex == 0)
                        {
                            if (!purchase.FreebieEnabled)
                            {
                                if (purchase.DiscountEnabled)
                                {
                                    purchaseExpenses += (purchase.Cost / 2);
                                    purchase.DisplayCost = purchase.Cost / 2;
                                }
                                else
                                {
                                    if (purchase.AssociatedOriginIndex == jumpBuild.OriginIndex && jumpBuild.OriginIndex != 0)
                                    {
                                        if (purchase.Cost > jump.OriginPerkFreebieThreshold)
                                        {
                                            purchaseExpenses += (purchase.Cost / 2);
                                            purchase.DisplayCost = purchase.Cost / 2;
                                        }
                                        else if (purchase.Cost < 0)
                                        {
                                            purchaseExpenses += purchase.Cost;
                                            purchase.DisplayCost = purchase.Cost;
                                        }
                                        else
                                        {
                                            purchase.DisplayCost = 0;
                                        }
                                    }
                                    else
                                    {
                                        purchaseExpenses += purchase.Cost;
                                        purchase.DisplayCost = purchase.Cost;
                                    }
                                }
                            }
                            else
                            {
                                purchase.DisplayCost = 0;
                            }
                        }
                        else if (jump.PurchaseTypes[purchase.TypeIndex].CurrencyIndex == 0)
                        {
                            if (!purchase.FreebieEnabled)
                            {
                                if (purchase.DiscountEnabled)
                                {
                                    purchaseExpenses += (purchase.Cost / 2);
                                    purchase.DisplayCost = purchase.Cost / 2;
                                }
                                else
                                {
                                    if (purchase.AssociatedOriginIndex == jumpBuild.OriginIndex && jumpBuild.OriginIndex != 0)
                                    {
                                        if (purchase.Cost > jump.OriginPerkFreebieThreshold)
                                        {
                                            purchaseExpenses += (purchase.Cost / 2);
                                            purchase.DisplayCost = purchase.Cost / 2;
                                        }
                                        else if (purchase.Cost < 0)
                                        {
                                            purchaseExpenses += purchase.Cost;
                                            purchase.DisplayCost = purchase.Cost;
                                        }
                                        else
                                        {
                                            purchase.DisplayCost = 0;
                                        }
                                    }
                                    else
                                    {
                                        purchaseExpenses += purchase.Cost;
                                        purchase.DisplayCost = purchase.Cost;
                                    }
                                }
                            }
                            else
                            {
                                purchase.DisplayCost = 0;
                            }
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


                    foreach (CompanionPurchase companionPurchase in jumpBuild.CompanionPurchase)
                    {
                        if (!companionPurchase.FreebieEnabled)
                        {
                            if (companionPurchase.DiscountEnabled)
                            {
                                budget -= (companionPurchase.Cost / 2);
                                companionPurchase.DisplayCost = companionPurchase.Cost / 2;
                            }
                            else
                            {
                                budget -= companionPurchase.Cost;
                                companionPurchase.DisplayCost = companionPurchase.Cost;
                            }
                        }
                        else
                        {
                            companionPurchase.DisplayCost = 0;
                        }

                    }
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


                    foreach (Drawback drawback in jumpBuild.DrawbackSelection)
                    {
                        budget += drawback.Value;
                    }

                    foreach (Drawback scenario in jumpBuild.ScenarioSelection)
                    {
                        budget += scenario.Value;
                    }

                    foreach (Purchase purchase in jumpBuild.Purchase)
                    {
                        if (purchase.TypeIndex <= 1)
                        {
                            if (!purchase.FreebieEnabled)
                            {
                                if (purchase.DiscountEnabled)
                                {
                                    purchaseExpenses += (purchase.Cost / 2);
                                    purchase.DisplayCost = purchase.Cost / 2;
                                }
                                else
                                {
                                    if (purchase.AssociatedOriginIndex == jumpBuild.OriginIndex && jumpBuild.OriginIndex != 0)
                                    {
                                        if (purchase.Cost > jump.OriginPerkFreebieThreshold)
                                        {
                                            purchaseExpenses += (purchase.Cost / 2);
                                            purchase.DisplayCost = purchase.Cost / 2;
                                        }
                                        else if (purchase.Cost < 0)
                                        {
                                            purchaseExpenses += purchase.Cost;
                                            purchase.DisplayCost = purchase.Cost;
                                        }
                                        else
                                        {
                                            purchase.DisplayCost = 0;
                                        }
                                    }
                                    else
                                    {
                                        purchaseExpenses += purchase.Cost;
                                        purchase.DisplayCost = purchase.Cost;
                                    }
                                }
                            }
                            else
                            {
                                purchase.DisplayCost = 0;
                            }
                        }
                        else if (jump.PurchaseTypes[purchase.TypeIndex].CurrencyIndex == 0)
                        {
                            if (!purchase.FreebieEnabled)
                            {
                                if (purchase.DiscountEnabled)
                                {
                                    purchaseExpenses += (purchase.Cost / 2);
                                    purchase.DisplayCost = purchase.Cost / 2;
                                }
                                else
                                {
                                    if (purchase.AssociatedOriginIndex == jumpBuild.OriginIndex && jumpBuild.OriginIndex != 0)
                                    {
                                        if (purchase.Cost > jump.OriginPerkFreebieThreshold)
                                        {
                                            purchaseExpenses += (purchase.Cost / 2);
                                            purchase.DisplayCost = purchase.Cost / 2;
                                        }
                                        else if (purchase.Cost < 0)
                                        {
                                            purchaseExpenses += purchase.Cost;
                                            purchase.DisplayCost = purchase.Cost;
                                        }
                                        else
                                        {
                                            purchase.DisplayCost = 0;
                                        }
                                    }
                                    else
                                    {
                                        purchaseExpenses += purchase.Cost;
                                        purchase.DisplayCost = purchase.Cost;
                                    }
                                }
                            }
                            else
                            {
                                purchase.DisplayCost = 0;
                            }
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

                    foreach (CompanionPurchase companionPurchase in jumpBuild.CompanionPurchase)
                    {
                        if (!companionPurchase.FreebieEnabled)
                        {
                            if (companionPurchase.DiscountEnabled)
                            {
                                budget -= (companionPurchase.Cost / 2);
                                companionPurchase.DisplayCost = companionPurchase.Cost;
                            }
                            else
                            {
                                budget -= companionPurchase.Cost;
                                companionPurchase.DisplayCost = companionPurchase.Cost;
                            }
                        }
                        else
                        {
                            companionPurchase.DisplayCost = 0;
                        }
                    }
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
                            if (!purchase.FreebieEnabled)
                            {
                                if (purchase.DiscountEnabled)
                                {
                                    purchaseExpenses += (purchase.Cost / 2);
                                    purchase.DisplayCost = purchase.Cost / 2;
                                }
                                else
                                {
                                    if (purchase.AssociatedOriginIndex == jumpBuild.OriginIndex && jumpBuild.OriginIndex != 0)
                                    {
                                        if (purchase.Cost > jump.OriginPerkFreebieThreshold)
                                        {
                                            purchaseExpenses += (purchase.Cost / 2);
                                            purchase.DisplayCost = purchase.Cost / 2;
                                        }
                                        else if (purchase.Cost < 0)
                                        {
                                            purchaseExpenses += purchase.Cost;
                                            purchase.DisplayCost = purchase.Cost;
                                        }
                                        else
                                        {
                                            purchase.DisplayCost = 0;
                                        }
                                    }
                                    else
                                    {
                                        purchaseExpenses += purchase.Cost;
                                        purchase.DisplayCost = purchase.Cost;
                                    }
                                }
                            }
                            else
                            {
                                purchase.DisplayCost = 0;
                            }
                        }
                    }
                    budget -= purchaseExpenses;
                    return budget;
                }
            }
            else
            {
                if (currencyIndex == 0 && purchaseType.Type != "Items")
                {
                    int itemExpenses = 0;
                    if (characterIndex == 0)
                    {
                        if (supplementPointsAllowed)
                        {
                            itemExpenses -= drawbackSupplementItemCP;
                        }
                    }
                    if (characterIndex == 0)
                    {
                        if (!isGauntlet)
                        {
                            budget += jump.Currencies[currencyIndex].CurrencyBudget;
                        }
                        budget += (jumpBuild.PointStipend[currencyIndex] + importStipend);
                    }
                    else
                    {
                        budget += jumpBuild.PointStipend[currencyIndex] + importStipend;
                    }

                    foreach (Drawback drawback in jumpBuild.DrawbackSelection)
                    {
                        budget += drawback.Value;
                    }

                    foreach (Drawback scenario in jumpBuild.ScenarioSelection)
                    {
                        budget += scenario.Value;
                    }

                    foreach (Purchase purchase in jumpBuild.Purchase)
                    {
                        if (purchase.TypeIndex == 1)
                        {
                            if (!purchase.FreebieEnabled)
                            {
                                if (purchase.DiscountEnabled)
                                {
                                    itemExpenses += (purchase.Cost / 2);
                                    purchase.DisplayCost = purchase.Cost / 2;
                                }
                                else
                                {
                                    itemExpenses += purchase.Cost;
                                    purchase.DisplayCost = purchase.Cost;
                                }
                            }
                            else
                            {
                                purchase.DisplayCost = 0;
                            }

                        }
                        else if (purchase.TypeIndex == 0)
                        {
                            if (!purchase.FreebieEnabled)
                            {
                                if (purchase.DiscountEnabled)
                                {
                                    purchaseExpenses += (purchase.Cost / 2);
                                    purchase.DisplayCost = purchase.Cost / 2;
                                }
                                else
                                {
                                    purchaseExpenses += purchase.Cost;
                                    purchase.DisplayCost = purchase.Cost;
                                }
                            }
                            else
                            {
                                purchase.DisplayCost = 0;
                            }
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


                    foreach (CompanionPurchase companionPurchase in jumpBuild.CompanionPurchase)
                    {
                        if (!companionPurchase.FreebieEnabled)
                        {
                            if (companionPurchase.DiscountEnabled)
                            {
                                budget -= (companionPurchase.Cost / 2);
                                companionPurchase.DisplayCost = companionPurchase.Cost / 2;
                            }
                            else
                            {
                                budget -= companionPurchase.Cost;
                                companionPurchase.DisplayCost = companionPurchase.Cost;
                            }
                        }
                        else
                        {
                            companionPurchase.DisplayCost = 0;
                        }

                    }
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

                    foreach (Drawback drawback in jumpBuild.DrawbackSelection)
                    {
                        budget += drawback.Value;
                    }

                    foreach (Drawback scenario in jumpBuild.ScenarioSelection)
                    {
                        budget += scenario.Value;
                    }

                    foreach (Purchase purchase in jumpBuild.Purchase)
                    {
                        if (purchase.TypeIndex <= 1)
                        {
                            if (!purchase.FreebieEnabled)
                            {
                                if (purchase.DiscountEnabled)
                                {
                                    purchaseExpenses += (purchase.Cost / 2);
                                    purchase.DisplayCost = purchase.Cost / 2;
                                }
                                else
                                {
                                    purchaseExpenses += purchase.Cost;
                                    purchase.DisplayCost = purchase.Cost;
                                }
                            }
                            else
                            {
                                purchase.DisplayCost = 0;
                            }
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

                    foreach (CompanionPurchase companionPurchase in jumpBuild.CompanionPurchase)
                    {
                        if (!companionPurchase.FreebieEnabled)
                        {
                            if (companionPurchase.DiscountEnabled)
                            {
                                budget -= (companionPurchase.Cost / 2);
                                companionPurchase.DisplayCost = companionPurchase.Cost / 2;
                            }
                            else
                            {
                                budget -= companionPurchase.Cost;
                                companionPurchase.DisplayCost = companionPurchase.Cost;
                            }
                        }
                        else
                        {
                            companionPurchase.DisplayCost = 0;
                        }
                    }
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
                            if (!purchase.FreebieEnabled)
                            {
                                if (purchase.DiscountEnabled)
                                {
                                    purchaseExpenses += (purchase.Cost / 2);
                                    purchase.DisplayCost = purchase.Cost / 2;
                                }
                                else
                                {
                                    purchaseExpenses += purchase.Cost;
                                    purchase.DisplayCost = purchase.Cost;
                                }
                            }
                            else
                            {
                                purchase.DisplayCost = 0;
                            }
                        }
                    }
                    budget -= purchaseExpenses;
                    return budget;
                }
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
}
