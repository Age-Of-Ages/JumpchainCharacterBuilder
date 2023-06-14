using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using JumpchainCharacterBuilder.Messages;
using JumpchainCharacterBuilder.Model;

namespace JumpchainCharacterBuilder.ViewModel
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        #region Fields

        [ObservableProperty]
        private SaveFile _loadedSave = new SaveFile();

        #endregion

        #region Properties


        #endregion

        public MainWindowViewModel()
        {
            //for (int i = 1; i < 5; i++)
            //{
            //    LoadedSave.JumpList.Add(new Jump($"Jump #{i}", 1));
            //}

            //for (int i = 1; i < 4; i++)
            //{
            //    LoadedSave.CharacterList.Add(new Character($"Companion {i}"));
            //}

            //for (int i = 1; i < LoadedSave.CharacterList.Count; i++)
            //{
            //    foreach (Jump jump in LoadedSave.JumpList)
            //    {
            //        jump.Build.Add(new JumpBuild());
            //    }
            //}

            //Random rand = new Random();
            //for (int i = 0; i < 7; i++)
            //{
            //    LoadedSave.JumpList[0].Build[0].Purchase.Add(new Purchase($"Perk #{i}", 0));

            //    switch (rand.Next(1, 5))
            //    {
            //        case 1:
            //            LoadedSave.JumpList[0].Build[0].Purchase.Last().Category = "Physical";
            //            break;
            //        case 2:
            //            LoadedSave.JumpList[0].Build[0].Purchase.Last().Category = "Mental";
            //            break;
            //        case 3:
            //            LoadedSave.JumpList[0].Build[0].Purchase.Last().Category = "Social";
            //            break;
            //        case 4:
            //            LoadedSave.JumpList[0].Build[0].Purchase.Last().Category = "Magical";
            //            break;
            //        case 5:
            //            LoadedSave.JumpList[0].Build[0].Purchase.Last().Category = "Other Perk";
            //            break;
            //        default:
            //            break;
            //    }
            //}

            //for (int i = 0; i < 7; i++)
            //{
            //    LoadedSave.JumpList[1].Build[0].Purchase.Add(new Purchase($"Perk #{i}", 0));

            //    switch (rand.Next(1, 5))
            //    {
            //        case 1:
            //            LoadedSave.JumpList[1].Build[0].Purchase.Last().Category = "Physical";
            //            break;
            //        case 2:
            //            LoadedSave.JumpList[1].Build[0].Purchase.Last().Category = "Mental";
            //            break;
            //        case 3:
            //            LoadedSave.JumpList[1].Build[0].Purchase.Last().Category = "Social";
            //            break;
            //        case 4:
            //            LoadedSave.JumpList[1].Build[0].Purchase.Last().Category = "Magical";
            //            break;
            //        case 5:
            //            LoadedSave.JumpList[1].Build[0].Purchase.Last().Category = "Other Perk";
            //            break;
            //        default:
            //            break;
            //    }
            //}

            //for (int i = 0; i < 7; i++)
            //{
            //    LoadedSave.JumpList[2].Build[0].Purchase.Add(new Purchase($"Perk #{i}", 0));

            //    switch (rand.Next(1, 5))
            //    {
            //        case 1:
            //            LoadedSave.JumpList[2].Build[0].Purchase.Last().Category = "Physical";
            //            break;
            //        case 2:
            //            LoadedSave.JumpList[2].Build[0].Purchase.Last().Category = "Mental";
            //            break;
            //        case 3:
            //            LoadedSave.JumpList[2].Build[0].Purchase.Last().Category = "Social";
            //            break;
            //        case 4:
            //            LoadedSave.JumpList[2].Build[0].Purchase.Last().Category = "Magical";
            //            break;
            //        case 5:
            //            LoadedSave.JumpList[2].Build[0].Purchase.Last().Category = "Other Perk";
            //            break;
            //        default:
            //            break;
            //    }
            //}

            //for (int i = 0; i < 5; i++)
            //{
            //    LoadedSave.JumpList[3].Build[0].Purchase.Add(new Purchase($"Perk #{i}", 0));

            //    if (rand.Next(1,6) < 3)
            //    {
            //        LoadedSave.JumpList[3].Build[0].Purchase.Last().BodyModAddition = true;
            //    }
            //}


            //LoadedSave.JumpList[0].Build[0].Purchase.Add(new Purchase("Name", 1));
            //LoadedSave.JumpList[0].Build[0].Purchase.Last().Name = "Warehouse Addon 1";
            //LoadedSave.JumpList[0].Build[0].Purchase.Last().Category = "Warehouse Addon";
            //LoadedSave.JumpList[0].Build[0].Purchase.Add(new Purchase("Name", 1));
            //LoadedSave.JumpList[0].Build[0].Purchase.Last().Name = "Warehouse Addon 2";
            //LoadedSave.JumpList[0].Build[0].Purchase.Last().Category = "Warehouse Addon";
            //LoadedSave.JumpList[0].Build[1].Purchase.Add(new Purchase("Name", 1));
            //LoadedSave.JumpList[0].Build[1].Purchase.Last().Name = "Warehouse Addon 3";
            //LoadedSave.JumpList[0].Build[1].Purchase.Last().Category = "Warehouse Addon";

            //LoadedSave.JumpList[0].Build[0].Purchase.Add(new Purchase("Item 1", 1));
            //LoadedSave.JumpList[0].Build[0].Purchase.Last().Category = "Weapons";
            //LoadedSave.JumpList[0].Build[0].Purchase.Add(new Purchase("Item 2", 1));
            //LoadedSave.JumpList[0].Build[0].Purchase.Last().Category = "Clothing";
            //LoadedSave.JumpList[0].Build[0].Purchase.Add(new Purchase("Item 3", 1));
            //LoadedSave.JumpList[0].Build[0].Purchase.Last().Category = "Clothing";
            //LoadedSave.JumpList[0].Build[0].Purchase.Add(new Purchase("Item 4", 1));
            //LoadedSave.JumpList[0].Build[0].Purchase.Last().Category = "Businesses";
            //LoadedSave.JumpList[0].Build[0].Purchase.Add(new Purchase("Item 5", 1));
            //LoadedSave.JumpList[0].Build[0].Purchase.Last().Category = "Businesses";

            //LoadedSave.JumpList[0].Build[2].Purchase.Add(new Purchase("Companion Item 1", 1));
            //LoadedSave.JumpList[0].Build[2].Purchase.Last().Category = "Clothing";
            //LoadedSave.JumpList[0].Build[2].Purchase.Add(new Purchase("Companion Item 2", 1));
            //LoadedSave.JumpList[0].Build[2].Purchase.Last().Category = "Clothing";
            //LoadedSave.JumpList[0].Build[2].Purchase.Add(new Purchase("Companion Item 3", 1));
            //LoadedSave.JumpList[0].Build[2].Purchase.Last().Category = "Media";
            //LoadedSave.JumpList[0].Build[2].Purchase.Add(new Purchase("Companion Item 4", 1));
            //LoadedSave.JumpList[0].Build[2].Purchase.Last().Category = "Media";

            //LoadedSave.JumpList[1].Build[2].Purchase.Add(new Purchase("Companion Item 5", 1));
            //LoadedSave.JumpList[1].Build[2].Purchase.Add(new Purchase("Companion Item 6", 1));
            //LoadedSave.JumpList[1].Build[2].Purchase.Add(new Purchase("Companion Item 7", 1));
            //LoadedSave.JumpList[1].Build[2].Purchase.Add(new Purchase("Companion Item 8", 1));

            //LoadedSave.JumpList[2].Build[0].Purchase.Add(new Purchase("Name", 1));
            //LoadedSave.JumpList[2].Build[0].Purchase.Last().Name = "Warehouse Addon 5";
            //LoadedSave.JumpList[2].Build[0].Purchase.Last().Category = "Warehouse Addon";

            //LoadedSave.GenericDrawbackSupplement.Purchases.Add(new DrawbackSupplementPurchase("Drawback 1", LoadedSave.JumpList.Count));
            //LoadedSave.GenericDrawbackSupplement.Purchases.Last().ValueChoicePoints = 100;
            //LoadedSave.GenericDrawbackSupplement.Purchases.Add(new DrawbackSupplementPurchase("Drawback 2", LoadedSave.JumpList.Count));
            //LoadedSave.GenericDrawbackSupplement.Purchases.Last().ValueItemPoints = 300;
            //LoadedSave.GenericDrawbackSupplement.Purchases.Last().ValueCompanionPoints = 50;
            //LoadedSave.GenericDrawbackSupplement.Purchases.Add(new DrawbackSupplementPurchase("Drawback 3", LoadedSave.JumpList.Count));
            //LoadedSave.GenericDrawbackSupplement.Purchases.Last().ValueChoicePoints = 250;
            //LoadedSave.GenericDrawbackSupplement.Purchases.Last().ValueWarehousePoints = 250;

            //LoadedSave.Options.CosmicWarehouseSetting = Options.CosmicWarehouseSupplements.PersonalReality;

            //LoadedSave.PersonalReality.Purchases.Add(new SupplementPurchase("Purchase 1", "Basics"));
            //LoadedSave.PersonalReality.Purchases.Last().Cost = 50;
            //LoadedSave.PersonalReality.Purchases.Add(new SupplementPurchase("Purchase 2", "Basics"));
            //LoadedSave.PersonalReality.Purchases.Last().Cost = 50;
            //LoadedSave.PersonalReality.Purchases.Add(new SupplementPurchase("Purchase 3", "Basics"));
            //LoadedSave.PersonalReality.Purchases.Last().Cost = 100;
            //LoadedSave.PersonalReality.Purchases.Add(new SupplementPurchase("Purchase 4", "Basics"));
            //LoadedSave.PersonalReality.Purchases.Last().Cost = 150;
            //LoadedSave.PersonalReality.Purchases.Add(new SupplementPurchase("Purchase 5", "Basics"));
            //LoadedSave.PersonalReality.Purchases.Last().Cost = 200;

            //LoadedSave.PersonalReality.Purchases.Add(new SupplementPurchase("Purchase 1", "Utilities"));
            //LoadedSave.PersonalReality.Purchases.Last().Cost = 50;
            //LoadedSave.PersonalReality.Purchases.Add(new SupplementPurchase("Purchase 2", "Utilities"));
            //LoadedSave.PersonalReality.Purchases.Last().Cost = 150;
            //LoadedSave.PersonalReality.Purchases.Add(new SupplementPurchase("Purchase 3", "Utilities"));
            //LoadedSave.PersonalReality.Purchases.Last().Cost = 50;
            //LoadedSave.PersonalReality.Purchases.Add(new SupplementPurchase("Purchase 4", "Utilities"));
            //LoadedSave.PersonalReality.Purchases.Last().Cost = 200;
            //LoadedSave.PersonalReality.Purchases.Add(new SupplementPurchase("Purchase 5", "Utilities"));
            //LoadedSave.PersonalReality.Purchases.Last().Cost = 250;

            //LoadedSave.PersonalReality.Purchases.Add(new SupplementPurchase("Purchase 1", "Cosmetic"));
            //LoadedSave.PersonalReality.Purchases.Add(new SupplementPurchase("Purchase 2", "Cosmetic"));

            //LoadedSave.PersonalReality.Purchases.Add(new SupplementPurchase("Purchase 1", "Facilities"));
            //LoadedSave.PersonalReality.Purchases.Add(new SupplementPurchase("Purchase 2", "Facilities"));

            //LoadedSave.PersonalReality.Purchases.Add(new SupplementPurchase("Purchase 1", "Extensions"));
            //LoadedSave.PersonalReality.Purchases.Add(new SupplementPurchase("Purchase 2", "Extensions"));

            //LoadedSave.PersonalReality.Purchases.Add(new SupplementPurchase("Purchase 1", "Equipment"));
            //LoadedSave.PersonalReality.Purchases.Add(new SupplementPurchase("Purchase 2", "Equipment"));

            //LoadedSave.PersonalReality.Purchases.Add(new SupplementPurchase("Purchase 1", "Companions"));
            //LoadedSave.PersonalReality.Purchases.Add(new SupplementPurchase("Purchase 2", "Companions"));

            //LoadedSave.PersonalReality.Purchases.Add(new SupplementPurchase("Purchase 1", "Misc"));
            //LoadedSave.PersonalReality.Purchases.Add(new SupplementPurchase("Purchase 2", "Misc"));

            //LoadedSave.PersonalReality.Limitations.Add(new SupplementDrawbackModel("Limitation 1"));
            //LoadedSave.PersonalReality.Limitations.Last().Value = 50;
            //LoadedSave.PersonalReality.Limitations.Add(new SupplementDrawbackModel("Limitation 2"));
            //LoadedSave.PersonalReality.Limitations.Last().Value = 100;
            //LoadedSave.PersonalReality.Limitations.Add(new SupplementDrawbackModel("Limitation 3"));
            //LoadedSave.PersonalReality.Limitations.Last().Value = 150;

            //LoadedSave.GenericBodyMod.PurchasesAllowed = true;

            //LoadedSave.Options.BodyModSetting = Options.BodyModSupplements.SBBodyMod;

            //for (int i = 0; i < 10; i++)
            //{
            //    LoadedSave.CharacterList[0].BodyMod.Limitations.Add(new($"Limitation #{i}"));
            //    LoadedSave.CharacterList[0].BodyMod.Limitations.Last().Category = "Generic";

            //    switch (rand.Next(1, 4))
            //    {
            //        case 1:
            //            LoadedSave.CharacterList[0].BodyMod.Limitations.Last().Value = 100;
            //            break;
            //        case 2:
            //            LoadedSave.CharacterList[0].BodyMod.Limitations.Last().Value = 200;
            //            break;
            //        case 3:
            //            LoadedSave.CharacterList[0].BodyMod.Limitations.Last().Value = 400;
            //            break;
            //        case 4:
            //            LoadedSave.CharacterList[0].BodyMod.Limitations.Last().Value = 0;
            //            break;
            //        default:
            //            LoadedSave.CharacterList[0].BodyMod.Limitations.Last().Value = 0;
            //            break;
            //    }
            //}

            //for (int i = 0; i < 10; i++)
            //{
            //    LoadedSave.CharacterList[0].BodyMod.Purchases.Add(new($"Purchase #{i}", "Generic"));

            //    switch (rand.Next(1, 4))
            //    {
            //        case 1:
            //            LoadedSave.CharacterList[0].BodyMod.Purchases.Last().Cost = 50;
            //            break;
            //        case 2:
            //            LoadedSave.CharacterList[0].BodyMod.Purchases.Last().Cost = 100;
            //            break;
            //        case 3:
            //            LoadedSave.CharacterList[0].BodyMod.Purchases.Last().Cost = 200;
            //            break;
            //        case 4:
            //            LoadedSave.CharacterList[0].BodyMod.Purchases.Last().Cost = 400;
            //            break;
            //        default:
            //            LoadedSave.CharacterList[0].BodyMod.Purchases.Last().Cost = 0;
            //            break;
            //    }
            //}

            Messenger.Send(new SaveDataSendMessage(LoadedSave));
        }
    }
}
