using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using JumpchainCharacterBuilder.Messages;
using JumpchainCharacterBuilder.Model;
using JumpchainCharacterBuilder.Services;
using System.Collections.ObjectModel;
using System.Linq;

namespace JumpchainCharacterBuilder.ViewModel
{
    public partial class CosmicWarehouseViewModel : ViewModelBase
    {
        #region Fields
        private readonly IDialogService _dialogService;

        [ObservableProperty]
        private SaveFile _loadedSave = new();
        [ObservableProperty]
        private Options _loadedOptions = new();
        [ObservableProperty]
        private Options.CosmicWarehouseSupplements _loadedWarehouseSupplement = Options.CosmicWarehouseSupplements.Generic;
        [ObservableProperty]
        private Options.DrawbackSupplements _loadedDrawbackSupplement = Options.DrawbackSupplements.Generic;

        [ObservableProperty]
        private bool _genericWarehouseSelected = true;
        [ObservableProperty]
        private bool _personalRealitySelected = false;
        [ObservableProperty]
        private bool _purchasesAllowed = false;

        [ObservableProperty]
        private int _warehouseTabIndex = 0;
        [ObservableProperty]
        private int _purchasesTabIndex = 0;

        [ObservableProperty]
        private int _totalWP = 0;
        [ObservableProperty]
        private int _cPInvested = 0;
        [ObservableProperty]
        private int _jumpWPGained = 0;
        [ObservableProperty]
        private int _budget = 0;
        [ObservableProperty]
        private int _wPBought = 0;
        [ObservableProperty]
        private int _drawbackWP = 0;
        [ObservableProperty]
        private int _limitationWP = 0;
        [ObservableProperty]
        private int _patientJumperWP = 0;

        [ObservableProperty]
        private string _genericName = "";
        [ObservableProperty]
        private string _genericDescription = "";

        [ObservableProperty]
        private ObservableCollection<SupplementPurchase> _basicsPurchaseList = new();
        [ObservableProperty]
        private int _basicsPurchaseSelectionIndex = 0;
        [ObservableProperty]
        private ObservableCollection<SupplementPurchase> _utilitiesPurchaseList = new();
        [ObservableProperty]
        private int _utilitiesPurchaseSelectionIndex = 0;
        [ObservableProperty]
        private ObservableCollection<SupplementPurchase> _cosmeticPurchaseList = new();
        [ObservableProperty]
        private int _cosmeticPurchaseSelectionIndex = 0;
        [ObservableProperty]
        private ObservableCollection<SupplementPurchase> _facilitiesPurchaseList = new();
        [ObservableProperty]
        private int _facilitiesPurchaseSelectionIndex = 0;
        [ObservableProperty]
        private ObservableCollection<SupplementPurchase> _extensionsPurchaseList = new();
        [ObservableProperty]
        private int _extensionsPurchaseSelectionIndex = 0;
        [ObservableProperty]
        private ObservableCollection<SupplementPurchase> _equipmentPurchaseList = new();
        [ObservableProperty]
        private int _equipmentPurchaseSelectionIndex = 0;
        [ObservableProperty]
        private ObservableCollection<SupplementPurchase> _companionsPurchaseList = new();
        [ObservableProperty]
        private int _companionsPurchaseSelectionIndex = 0;
        [ObservableProperty]
        private ObservableCollection<SupplementPurchase> _miscPurchaseList = new();
        [ObservableProperty]
        private int _miscPurchaseSelectionIndex = 0;

        [ObservableProperty]
        private SupplementPurchase _purchaseSelection = new();

        [ObservableProperty]
        private ObservableCollection<SupplementDrawbackModel> _limitationList = new();
        [ObservableProperty]
        private SupplementDrawbackModel _limitationSelection = new();
        [ObservableProperty]
        private int _limitationSelectionIndex = 0;

        [ObservableProperty]
        private ObservableCollection<Purchase> _additionList = new();
        [ObservableProperty]
        private Purchase _additionSelection = new();
        [ObservableProperty]
        private int _additionSelectionIndex = new();


        #endregion

        #region Properties
        partial void OnLoadedWarehouseSupplementChanged(Options.CosmicWarehouseSupplements value)
        {
            LoadWarehouseTabs();
            LoadWarehouseData();

            WarehouseTabIndex = 0;
        }

        partial void OnLoadedDrawbackSupplementChanged(Options.DrawbackSupplements value)
        {
            LoadDrawbackData();
        }

        partial void OnWarehouseTabIndexChanged(int value)
        {
            WarehouseTabChanged();
        }

        partial void OnPurchasesTabIndexChanged(int value)
        {
            PurchaseTabChanged();
        }

        partial void OnPurchaseSelectionChanged(SupplementPurchase value)
        {
            CalculateBudget();

            DeletePurchaseCommand.NotifyCanExecuteChanged();
            MovePurchaseDownCommand.NotifyCanExecuteChanged();
            MovePurchaseUpCommand.NotifyCanExecuteChanged();
        }

        partial void OnLimitationSelectionChanged(SupplementDrawbackModel value)
        {
            DeleteLimitationCommand.NotifyCanExecuteChanged();
            MoveLimitationDownCommand.NotifyCanExecuteChanged();
            MoveLimitationUpCommand.NotifyCanExecuteChanged();
        }

        #endregion

        #region Constructor
        public CosmicWarehouseViewModel()
        {

        }

        public CosmicWarehouseViewModel(IDialogService dialogService)
        {
            Messenger.Register<SaveDataChangedMessage>(this, (r, m) =>
            {
                LoadedOptions = LoadedSave.Options;
            });
            Messenger.Register<SaveDataSendMessage>(this, (r, m) =>
            {
                LoadedSave = m.Value;
                LoadedOptions = LoadedSave.Options;
            });
            Messenger.Register<SupplementChangedMessage>(this, (r, m) =>
            {
                if (m.Value == "Warehouse")
                {
                    LoadedWarehouseSupplement = LoadedOptions.CosmicWarehouseSetting;
                }
                else if (m.Value == "Drawback")
                {
                    LoadedDrawbackSupplement = LoadedOptions.DrawbackSupplementSetting;
                }
            });

            _dialogService = dialogService;
        }

        #endregion

        #region Methods
        private void LoadWarehouseTabs()
        {
            switch (LoadedWarehouseSupplement)
            {
                case Options.CosmicWarehouseSupplements.Generic:
                    GenericWarehouseSelected = true;
                    PersonalRealitySelected = false;

                    if (LoadedSave.GenericWarehouse.PurchasesAllowed)
                    {
                        PurchasesAllowed = true;
                    }
                    else
                    {
                        PurchasesAllowed = false;
                    }
                    break;
                case Options.CosmicWarehouseSupplements.PersonalReality:
                    GenericWarehouseSelected = false;
                    PersonalRealitySelected = true;
                    PurchasesAllowed = true;
                    break;
                default:
                    GenericWarehouseSelected = true;
                    PersonalRealitySelected = false;
                    PurchasesAllowed = false;
                    break;
            }
        }
        private void LoadWarehouseData()
        {
            switch (LoadedWarehouseSupplement)
            {
                case Options.CosmicWarehouseSupplements.Generic:
                    if (LoadedSave.GenericWarehouse.IncrementalBudget > 0)
                    {
                        CalculateJumpWP(LoadedSave.GenericWarehouse);
                    }
                    else
                    {
                        JumpWPGained = 0;
                    }

                    if (LoadedSave.GenericWarehouse.InvestmentAllowed)
                    {
                        CalculateInvestedCP();

                        LoadedSave.GenericWarehouse.TotalInvestment = (CPInvested / LoadedSave.GenericWarehouse.InvestmentRatio);
                        WPBought = LoadedSave.GenericWarehouse.TotalInvestment;
                    }

                    GenericName = LoadedSave.GenericWarehouse.Name;
                    GenericDescription = LoadedSave.GenericWarehouse.FullDescription;
                    break;
                case Options.CosmicWarehouseSupplements.PersonalReality:
                    if (LoadedSave.PersonalReality.IncrementalBudget > 0)
                    {
                        CalculateJumpWP(LoadedSave.PersonalReality);
                    }
                    else
                    {
                        JumpWPGained = 0;
                    }

                    if (LoadedSave.PersonalReality.InvestmentAllowed)
                    {
                        CalculateInvestedCP();

                        LoadedSave.PersonalReality.TotalInvestment = (CPInvested / LoadedSave.PersonalReality.InvestmentRatio);
                        WPBought = LoadedSave.PersonalReality.TotalInvestment;
                    }

                    if (LoadedSave.PersonalReality.PatientJumper)
                    {
                        if (LoadedSave.PersonalReality.PatientJumperCountFirstJump)
                        {
                            int delay = LoadedSave.PersonalReality.SupplementDelay - 1;
                            if (delay < 0)
                            {
                                delay = 0;
                            }

                            PatientJumperWP = delay * 100;
                        }
                        else
                        {
                            int delay = LoadedSave.PersonalReality.SupplementDelay - 2;
                            if (delay < 0)
                            {
                                delay = 0;
                            }

                            PatientJumperWP = delay * 100;
                        }
                    }
                    break;
                default:
                    break;
            }

            LoadDrawbackData();
            LoadLimitationList();
            LoadPurchaseLists();
            LoadWarehouseTabs();
            LoadAdditionsList();

            CalculateTotalWP();

            CalculateBudget();
        }

        private void LoadDrawbackData()
        {
            switch (LoadedDrawbackSupplement)
            {
                case Options.DrawbackSupplements.Generic:
                    DrawbackWP = LoadedSave.GenericDrawbackSupplement.WPGained;
                    break;
                case Options.DrawbackSupplements.UDS:
                    DrawbackWP = LoadedSave.UniversalDrawbackSupplement.WPGained;
                    break;
                case Options.DrawbackSupplements.UU:
                    DrawbackWP = LoadedSave.UUSupplement.WPGained;
                    break;
                default:
                    DrawbackWP = 0;
                    break;
            }
        }

        private void LoadPurchaseLists()
        {
            BasicsPurchaseList.Clear();
            UtilitiesPurchaseList.Clear();
            CosmeticPurchaseList.Clear();
            FacilitiesPurchaseList.Clear();
            ExtensionsPurchaseList.Clear();
            EquipmentPurchaseList.Clear();
            CompanionsPurchaseList.Clear();
            MiscPurchaseList.Clear();

            switch (LoadedWarehouseSupplement)
            {
                case Options.CosmicWarehouseSupplements.Generic:
                    foreach (SupplementPurchase purchase in LoadedSave.GenericWarehouse.Purchases)
                    {
                        BasicsPurchaseList.Add(purchase);
                    }
                    break;
                case Options.CosmicWarehouseSupplements.PersonalReality:
                    foreach (SupplementPurchase purchase in LoadedSave.PersonalReality.Purchases)
                    {
                        switch (purchase.Category)
                        {
                            case "Basics":
                                BasicsPurchaseList.Add(purchase);
                                break;
                            case "Utilities":
                                UtilitiesPurchaseList.Add(purchase);
                                break;
                            case "Cosmetic":
                                CosmeticPurchaseList.Add(purchase);
                                break;
                            case "Facilities":
                                FacilitiesPurchaseList.Add(purchase);
                                break;
                            case "Extensions":
                                ExtensionsPurchaseList.Add(purchase);
                                break;
                            case "Equipment":
                                EquipmentPurchaseList.Add(purchase);
                                break;
                            case "Companions":
                                CompanionsPurchaseList.Add(purchase);
                                break;
                            case "Misc":
                                MiscPurchaseList.Add(purchase);
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        private void LoadLimitationList()
        {
            LimitationList.Clear();

            switch (LoadedWarehouseSupplement)
            {
                case Options.CosmicWarehouseSupplements.Generic:
                    foreach (SupplementDrawbackModel limitation in LoadedSave.GenericWarehouse.Limitations)
                    {
                        LimitationList.Add(limitation);
                    }
                    CalculateLimitationWP(LoadedSave.GenericWarehouse);
                    break;
                case Options.CosmicWarehouseSupplements.PersonalReality:
                    foreach (SupplementDrawbackModel limitation in LoadedSave.PersonalReality.Limitations)
                    {
                        LimitationList.Add(limitation);
                    }
                    CalculateLimitationWP(LoadedSave.PersonalReality);
                    break;
                default:
                    break;
            }

        }

        private void LoadAdditionsList()
        {
            AdditionList.Clear();

            foreach (Jump jump in LoadedSave.JumpList)
            {
                foreach (JumpBuild build in jump.Build)
                {
                    foreach (Purchase purchase in build.Purchase)
                    {
                        if (purchase.Category == "Warehouse Addon")
                        {
                            AdditionList.Add(purchase);
                            purchase.SourceJump = jump.Name;
                            purchase.SourceCharacter = LoadedSave.CharacterList[jump.Build.IndexOf(build)].Name;
                        }
                    }
                }
            }

            AdditionSelectionIndex = 0;
        }

        private void CalculateJumpWP(WarehouseUniversal warehouse)
        {
            int jumps = LoadedSave.JumpList.Last().JumpNumber - warehouse.SupplementDelay;
            if (jumps < 0)
            {
                jumps = 0;
            }

            int deposits = jumps / warehouse.IncrementalInterval;

            JumpWPGained = deposits * warehouse.IncrementalBudget;
        }

        private void CalculateInvestedCP()
        {
            int total = 0;

            foreach (Jump jump in LoadedSave.JumpList)
            {
                if (jump.Build[0].WarehouseInvestment > 0)
                {
                    total += jump.Build[0].WarehouseInvestment;
                }
            }

            CPInvested = total;
        }

        private void CalculateLimitationWP(WarehouseUniversal warehouse)
        {
            int total = 0;

            foreach (SupplementDrawbackModel limitation in warehouse.Limitations)
            {
                total += limitation.Value;
            }

            LimitationWP = total;
        }

        private void CalculateTotalWP()
        {
            switch (LoadedWarehouseSupplement)
            {
                case Options.CosmicWarehouseSupplements.Generic:
                    TotalWP = LoadedSave.GenericWarehouse.Budget;
                    TotalWP += JumpWPGained;
                    TotalWP += WPBought;
                    TotalWP += DrawbackWP;
                    TotalWP += LimitationWP;
                    break;
                case Options.CosmicWarehouseSupplements.PersonalReality:
                    TotalWP = LoadedSave.PersonalReality.Budget;
                    TotalWP += JumpWPGained;
                    TotalWP += WPBought;
                    TotalWP += DrawbackWP;
                    TotalWP += LimitationWP;
                    TotalWP += PatientJumperWP;
                    break;
                default:
                    break;
            }
        }

        private void CalculateBudget()
        {
            switch (LoadedWarehouseSupplement)
            {
                case Options.CosmicWarehouseSupplements.Generic:
                    Budget = BudgetCalculationsClass.WarehouseBudgetCalculation(LoadedSave.GenericWarehouse, TotalWP);
                    break;
                case Options.CosmicWarehouseSupplements.PersonalReality:
                    Budget = BudgetCalculationsClass.WarehouseBudgetCalculation(LoadedSave.PersonalReality, TotalWP);
                    break;
                default:
                    Budget = 0;
                    break;
            }
        }

        private void WarehouseTabChanged()
        {
            LoadWarehouseData();

            switch (WarehouseTabIndex)
            {
                case 2:
                    PurchaseTabChanged();
                    break;
                case 3:
                    LoadAdditionsList();
                    break;
                case 4:
                    LimitationSelectionIndex = 0;
                    break;
                default:
                    break;
            }
        }

        private void PurchaseTabChanged()
        {
            switch (PurchasesTabIndex)
            {
                case 0:
                    if (BasicsPurchaseList.Any())
                    {
                        BasicsPurchaseSelectionIndex = 0;
                        PurchaseSelection = BasicsPurchaseList.First();
                    }
                    break;
                case 1:
                    if (UtilitiesPurchaseList.Any())
                    {
                        UtilitiesPurchaseSelectionIndex = 0;
                        PurchaseSelection = UtilitiesPurchaseList.First();
                    }
                    break;
                case 2:
                    if (CosmeticPurchaseList.Any())
                    {
                        CosmeticPurchaseSelectionIndex = 0;
                        PurchaseSelection = CosmeticPurchaseList.First();
                    }
                    break;
                case 3:
                    if (FacilitiesPurchaseList.Any())
                    {
                        FacilitiesPurchaseSelectionIndex = 0;
                        PurchaseSelection = FacilitiesPurchaseList.First();
                    }
                    break;
                case 4:
                    if (ExtensionsPurchaseList.Any())
                    {
                        ExtensionsPurchaseSelectionIndex = 0;
                        PurchaseSelection = ExtensionsPurchaseList.First();
                    }
                    break;
                case 5:
                    if (EquipmentPurchaseList.Any())
                    {
                        EquipmentPurchaseSelectionIndex = 0;
                        PurchaseSelection = EquipmentPurchaseList.First();
                    }
                    break;
                case 6:
                    if (CompanionsPurchaseList.Any())
                    {
                        CompanionsPurchaseSelectionIndex = 0;
                        PurchaseSelection = CompanionsPurchaseList.First();
                    }
                    break;
                case 7:
                    if (MiscPurchaseList.Any())
                    {
                        MiscPurchaseSelectionIndex = 0;
                        PurchaseSelection = MiscPurchaseList.First();
                    }
                    break;
                default:
                    break;
            }

            DeletePurchaseCommand.NotifyCanExecuteChanged();
            MovePurchaseDownCommand.NotifyCanExecuteChanged();
            MovePurchaseUpCommand.NotifyCanExecuteChanged();
        }
        #endregion

        #region Commands
        [RelayCommand]
        private void RefreshWP()
        {
            LoadWarehouseData();
        }

        [RelayCommand]
        private void NewPurchase()
        {
            SupplementPurchase purchase = new();

            switch (LoadedWarehouseSupplement)
            {
                case Options.CosmicWarehouseSupplements.Generic:
                    purchase.Category = "Basics";
                    purchase.Name = $"Purchase #{BasicsPurchaseList.Count + 1}";
                    LoadedSave.GenericWarehouse.Purchases.Add(purchase);
                    BasicsPurchaseList.Add(purchase);
                    BasicsPurchaseSelectionIndex = BasicsPurchaseList.Count - 1;
                    break;
                case Options.CosmicWarehouseSupplements.PersonalReality:
                    switch (PurchasesTabIndex)
                    {
                        case 0:
                            purchase.Category = "Basics";
                            purchase.Name = $"Purchase #{BasicsPurchaseList.Count + 1}";
                            LoadedSave.PersonalReality.Purchases.Add(purchase);
                            BasicsPurchaseList.Add(purchase);
                            BasicsPurchaseSelectionIndex = BasicsPurchaseList.Count - 1;
                            break;
                        case 1:
                            purchase.Category = "Utilities";
                            purchase.Name = $"Purchase #{UtilitiesPurchaseList.Count + 1}";
                            LoadedSave.PersonalReality.Purchases.Add(purchase);
                            UtilitiesPurchaseList.Add(purchase);
                            UtilitiesPurchaseSelectionIndex = UtilitiesPurchaseList.Count - 1;
                            break;
                        case 2:
                            purchase.Category = "Cosmetic";
                            purchase.Name = $"Purchase #{CosmeticPurchaseList.Count + 1}";
                            LoadedSave.PersonalReality.Purchases.Add(purchase);
                            CosmeticPurchaseList.Add(purchase);
                            CosmeticPurchaseSelectionIndex = CosmeticPurchaseList.Count - 1;
                            break;
                        case 3:
                            purchase.Category = "Facilities";
                            purchase.Name = $"Purchase #{FacilitiesPurchaseList.Count + 1}";
                            LoadedSave.PersonalReality.Purchases.Add(purchase);
                            FacilitiesPurchaseList.Add(purchase);
                            FacilitiesPurchaseSelectionIndex = FacilitiesPurchaseList.Count - 1;
                            break;
                        case 4:
                            purchase.Category = "Extensions";
                            purchase.Name = $"Purchase #{ExtensionsPurchaseList.Count + 1}";
                            LoadedSave.PersonalReality.Purchases.Add(purchase);
                            ExtensionsPurchaseList.Add(purchase);
                            ExtensionsPurchaseSelectionIndex = ExtensionsPurchaseList.Count - 1;
                            break;
                        case 5:
                            purchase.Category = "Equipment";
                            purchase.Name = $"Purchase #{EquipmentPurchaseList.Count + 1}";
                            LoadedSave.PersonalReality.Purchases.Add(purchase);
                            EquipmentPurchaseList.Add(purchase);
                            EquipmentPurchaseSelectionIndex = EquipmentPurchaseList.Count - 1;
                            break;
                        case 6:
                            purchase.Category = "Companions";
                            purchase.Name = $"Purchase #{CompanionsPurchaseList.Count + 1}";
                            LoadedSave.PersonalReality.Purchases.Add(purchase);
                            CompanionsPurchaseList.Add(purchase);
                            CompanionsPurchaseSelectionIndex = CompanionsPurchaseList.Count - 1;
                            break;
                        case 7:
                            purchase.Category = "Misc";
                            purchase.Name = $"Purchase #{MiscPurchaseList.Count + 1}";
                            LoadedSave.PersonalReality.Purchases.Add(purchase);
                            MiscPurchaseList.Add(purchase);
                            MiscPurchaseSelectionIndex = MiscPurchaseList.Count - 1;
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }

        [RelayCommand(CanExecute = "CanDeletePurchase")]
        private void DeletePurchase()
        {
            if (_dialogService.ConfirmDialog("Are you sure you want to delete this Purchase " +
                "This action cannot be reversed."))
            {
                switch (LoadedWarehouseSupplement)
                {
                    case Options.CosmicWarehouseSupplements.Generic:
                        LoadedSave.GenericWarehouse.Purchases.RemoveAt(BasicsPurchaseSelectionIndex);
                        BasicsPurchaseList.RemoveAt(BasicsPurchaseSelectionIndex);
                        BasicsPurchaseSelectionIndex = 0;
                        break;
                    case Options.CosmicWarehouseSupplements.PersonalReality:
                        switch (PurchasesTabIndex)
                        {
                            case 0:
                                LoadedSave.PersonalReality.Purchases.RemoveAt(BasicsPurchaseSelectionIndex);
                                BasicsPurchaseList.RemoveAt(BasicsPurchaseSelectionIndex);
                                BasicsPurchaseSelectionIndex = 0;
                                break;
                            case 1:
                                LoadedSave.PersonalReality.Purchases.RemoveAt(UtilitiesPurchaseSelectionIndex);
                                UtilitiesPurchaseList.RemoveAt(UtilitiesPurchaseSelectionIndex);
                                UtilitiesPurchaseSelectionIndex = 0;
                                break;
                            case 2:
                                LoadedSave.PersonalReality.Purchases.RemoveAt(CosmeticPurchaseSelectionIndex);
                                CosmeticPurchaseList.RemoveAt(CosmeticPurchaseSelectionIndex);
                                CosmeticPurchaseSelectionIndex = 0;
                                break;
                            case 3:
                                LoadedSave.PersonalReality.Purchases.RemoveAt(FacilitiesPurchaseSelectionIndex);
                                FacilitiesPurchaseList.RemoveAt(FacilitiesPurchaseSelectionIndex);
                                FacilitiesPurchaseSelectionIndex = 0;
                                break;
                            case 4:
                                LoadedSave.PersonalReality.Purchases.RemoveAt(ExtensionsPurchaseSelectionIndex);
                                ExtensionsPurchaseList.RemoveAt(ExtensionsPurchaseSelectionIndex);
                                ExtensionsPurchaseSelectionIndex = 0;
                                break;
                            case 5:
                                LoadedSave.PersonalReality.Purchases.RemoveAt(EquipmentPurchaseSelectionIndex);
                                EquipmentPurchaseList.RemoveAt(EquipmentPurchaseSelectionIndex);
                                EquipmentPurchaseSelectionIndex = 0;
                                break;
                            case 6:
                                LoadedSave.PersonalReality.Purchases.RemoveAt(CompanionsPurchaseSelectionIndex);
                                CompanionsPurchaseList.RemoveAt(CompanionsPurchaseSelectionIndex);
                                CompanionsPurchaseSelectionIndex = 0;
                                break;
                            case 7:
                                LoadedSave.PersonalReality.Purchases.RemoveAt(MiscPurchaseSelectionIndex);
                                MiscPurchaseList.RemoveAt(MiscPurchaseSelectionIndex);
                                MiscPurchaseSelectionIndex = 0;
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        break;
                }

                DeletePurchaseCommand.NotifyCanExecuteChanged();
                MovePurchaseDownCommand.NotifyCanExecuteChanged();
                MovePurchaseUpCommand.NotifyCanExecuteChanged();
            }
        }

        private bool CanDeletePurchase()
        {
            switch (PurchasesTabIndex)
            {
                case 0:
                    return BasicsPurchaseList.Any() && BasicsPurchaseSelectionIndex != -1;
                case 1:
                    return UtilitiesPurchaseList.Any() && UtilitiesPurchaseSelectionIndex != -1;
                case 2:
                    return CosmeticPurchaseList.Any() && CosmeticPurchaseSelectionIndex != -1;
                case 3:
                    return FacilitiesPurchaseList.Any() && FacilitiesPurchaseSelectionIndex != -1;
                case 4:
                    return ExtensionsPurchaseList.Any() && ExtensionsPurchaseSelectionIndex != -1;
                case 5:
                    return EquipmentPurchaseList.Any() && EquipmentPurchaseSelectionIndex != -1;
                case 6:
                    return CompanionsPurchaseList.Any() && CompanionsPurchaseSelectionIndex != -1;
                case 7:
                    return MiscPurchaseList.Any() && MiscPurchaseSelectionIndex != -1;
                default:
                    return false;
            }
        }

        [RelayCommand(CanExecute = "CanMovePurchaseUp")]
        private void MovePurchaseUp()
        {
            int index;
            int indexList1;
            int indexList2;
            switch (LoadedWarehouseSupplement)
            {
                case Options.CosmicWarehouseSupplements.Generic:
                    index = BasicsPurchaseSelectionIndex;
                    BasicsPurchaseList.SwapCollectionItems(index, index - 1);
                    LoadedSave.GenericWarehouse.Purchases.SwapListItems(index, index - 1);
                    BasicsPurchaseSelectionIndex = index - 1;
                    break;
                case Options.CosmicWarehouseSupplements.PersonalReality:
                    switch (PurchasesTabIndex)
                    {
                        case 0:
                            index = BasicsPurchaseSelectionIndex;
                            indexList1 = LoadedSave.PersonalReality.Purchases.IndexOf(BasicsPurchaseList[index]);
                            indexList2 = LoadedSave.PersonalReality.Purchases.IndexOf(BasicsPurchaseList[index - 1]);
                            BasicsPurchaseList.SwapCollectionItems(index, index - 1);
                            LoadedSave.PersonalReality.Purchases.SwapListItems(indexList1, indexList2);
                            BasicsPurchaseSelectionIndex = index - 1;
                            break;
                        case 1:
                            index = UtilitiesPurchaseSelectionIndex;
                            indexList1 = LoadedSave.PersonalReality.Purchases.IndexOf(UtilitiesPurchaseList[index]);
                            indexList2 = LoadedSave.PersonalReality.Purchases.IndexOf(UtilitiesPurchaseList[index - 1]);
                            UtilitiesPurchaseList.SwapCollectionItems(index, index - 1);
                            LoadedSave.PersonalReality.Purchases.SwapListItems(indexList1, indexList2);
                            UtilitiesPurchaseSelectionIndex = index - 1;
                            break;
                        case 2:
                            index = CosmeticPurchaseSelectionIndex;
                            indexList1 = LoadedSave.PersonalReality.Purchases.IndexOf(CosmeticPurchaseList[index]);
                            indexList2 = LoadedSave.PersonalReality.Purchases.IndexOf(CosmeticPurchaseList[index - 1]);
                            CosmeticPurchaseList.SwapCollectionItems(index, index - 1);
                            LoadedSave.PersonalReality.Purchases.SwapListItems(indexList1, indexList2);
                            CosmeticPurchaseSelectionIndex = index - 1;
                            break;
                        case 3:
                            index = FacilitiesPurchaseSelectionIndex;
                            indexList1 = LoadedSave.PersonalReality.Purchases.IndexOf(FacilitiesPurchaseList[index]);
                            indexList2 = LoadedSave.PersonalReality.Purchases.IndexOf(FacilitiesPurchaseList[index - 1]);
                            FacilitiesPurchaseList.SwapCollectionItems(index, index - 1);
                            LoadedSave.PersonalReality.Purchases.SwapListItems(indexList1, indexList2);
                            FacilitiesPurchaseSelectionIndex = index - 1;
                            break;
                        case 4:
                            index = ExtensionsPurchaseSelectionIndex;
                            indexList1 = LoadedSave.PersonalReality.Purchases.IndexOf(ExtensionsPurchaseList[index]);
                            indexList2 = LoadedSave.PersonalReality.Purchases.IndexOf(ExtensionsPurchaseList[index - 1]);
                            ExtensionsPurchaseList.SwapCollectionItems(index, index - 1);
                            LoadedSave.PersonalReality.Purchases.SwapListItems(indexList1, indexList2);
                            ExtensionsPurchaseSelectionIndex = index - 1;
                            break;
                        case 5:
                            index = EquipmentPurchaseSelectionIndex;
                            indexList1 = LoadedSave.PersonalReality.Purchases.IndexOf(EquipmentPurchaseList[index]);
                            indexList2 = LoadedSave.PersonalReality.Purchases.IndexOf(EquipmentPurchaseList[index - 1]);
                            EquipmentPurchaseList.SwapCollectionItems(index, index - 1);
                            LoadedSave.PersonalReality.Purchases.SwapListItems(indexList1, indexList2);
                            EquipmentPurchaseSelectionIndex = index - 1;
                            break;
                        case 6:
                            index = CompanionsPurchaseSelectionIndex;
                            indexList1 = LoadedSave.PersonalReality.Purchases.IndexOf(CompanionsPurchaseList[index]);
                            indexList2 = LoadedSave.PersonalReality.Purchases.IndexOf(CompanionsPurchaseList[index - 1]);
                            CompanionsPurchaseList.SwapCollectionItems(index, index - 1);
                            LoadedSave.PersonalReality.Purchases.SwapListItems(indexList1, indexList2);
                            CompanionsPurchaseSelectionIndex = index - 1;
                            break;
                        case 7:
                            index = MiscPurchaseSelectionIndex;
                            indexList1 = LoadedSave.PersonalReality.Purchases.IndexOf(MiscPurchaseList[index]);
                            indexList2 = LoadedSave.PersonalReality.Purchases.IndexOf(MiscPurchaseList[index - 1]);
                            MiscPurchaseList.SwapCollectionItems(index, index - 1);
                            LoadedSave.PersonalReality.Purchases.SwapListItems(indexList1, indexList2);
                            MiscPurchaseSelectionIndex = index - 1;
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }

            DeletePurchaseCommand.NotifyCanExecuteChanged();
            MovePurchaseDownCommand.NotifyCanExecuteChanged();
            MovePurchaseUpCommand.NotifyCanExecuteChanged();
        }

        private bool CanMovePurchaseUp()
        {
            switch (PurchasesTabIndex)
            {
                case 0:
                    return BasicsPurchaseSelectionIndex > 0;
                case 1:
                    return UtilitiesPurchaseSelectionIndex > 0;
                case 2:
                    return CosmeticPurchaseSelectionIndex > 0;
                case 3:
                    return FacilitiesPurchaseSelectionIndex > 0;
                case 4:
                    return ExtensionsPurchaseSelectionIndex > 0;
                case 5:
                    return EquipmentPurchaseSelectionIndex > 0;
                case 6:
                    return CompanionsPurchaseSelectionIndex > 0;
                case 7:
                    return MiscPurchaseSelectionIndex > 0;
                default:
                    return false;
            }
        }

        [RelayCommand(CanExecute = "CanMovePurchaseDown")]
        private void MovePurchaseDown()
        {
            int index;
            int indexList1;
            int indexList2;
            switch (LoadedWarehouseSupplement)
            {
                case Options.CosmicWarehouseSupplements.Generic:
                    index = BasicsPurchaseSelectionIndex;
                    BasicsPurchaseList.SwapCollectionItems(index, index + 1);
                    LoadedSave.GenericWarehouse.Purchases.SwapListItems(index, index + 1);
                    BasicsPurchaseSelectionIndex = index + 1;
                    break;
                case Options.CosmicWarehouseSupplements.PersonalReality:
                    switch (PurchasesTabIndex)
                    {
                        case 0:
                            index = BasicsPurchaseSelectionIndex;
                            indexList1 = LoadedSave.PersonalReality.Purchases.IndexOf(BasicsPurchaseList[index]);
                            indexList2 = LoadedSave.PersonalReality.Purchases.IndexOf(BasicsPurchaseList[index + 1]);
                            BasicsPurchaseList.SwapCollectionItems(index, index + 1);
                            LoadedSave.PersonalReality.Purchases.SwapListItems(indexList1, indexList2);
                            BasicsPurchaseSelectionIndex = index + 1;
                            break;
                        case 1:
                            index = UtilitiesPurchaseSelectionIndex;
                            indexList1 = LoadedSave.PersonalReality.Purchases.IndexOf(UtilitiesPurchaseList[index]);
                            indexList2 = LoadedSave.PersonalReality.Purchases.IndexOf(UtilitiesPurchaseList[index + 1]);
                            UtilitiesPurchaseList.SwapCollectionItems(index, index + 1);
                            LoadedSave.PersonalReality.Purchases.SwapListItems(indexList1, indexList2);
                            UtilitiesPurchaseSelectionIndex = index + 1;
                            break;
                        case 2:
                            index = CosmeticPurchaseSelectionIndex;
                            indexList1 = LoadedSave.PersonalReality.Purchases.IndexOf(CosmeticPurchaseList[index]);
                            indexList2 = LoadedSave.PersonalReality.Purchases.IndexOf(CosmeticPurchaseList[index + 1]);
                            CosmeticPurchaseList.SwapCollectionItems(index, index + 1);
                            LoadedSave.PersonalReality.Purchases.SwapListItems(indexList1, indexList2);
                            CosmeticPurchaseSelectionIndex = index + 1;
                            break;
                        case 3:
                            index = FacilitiesPurchaseSelectionIndex;
                            indexList1 = LoadedSave.PersonalReality.Purchases.IndexOf(FacilitiesPurchaseList[index]);
                            indexList2 = LoadedSave.PersonalReality.Purchases.IndexOf(FacilitiesPurchaseList[index + 1]);
                            FacilitiesPurchaseList.SwapCollectionItems(index, index + 1);
                            LoadedSave.PersonalReality.Purchases.SwapListItems(indexList1, indexList2);
                            FacilitiesPurchaseSelectionIndex = index + 1;
                            break;
                        case 4:
                            index = ExtensionsPurchaseSelectionIndex;
                            indexList1 = LoadedSave.PersonalReality.Purchases.IndexOf(ExtensionsPurchaseList[index]);
                            indexList2 = LoadedSave.PersonalReality.Purchases.IndexOf(ExtensionsPurchaseList[index + 1]);
                            ExtensionsPurchaseList.SwapCollectionItems(index, index + 1);
                            LoadedSave.PersonalReality.Purchases.SwapListItems(indexList1, indexList2);
                            ExtensionsPurchaseSelectionIndex = index + 1;
                            break;
                        case 5:
                            index = EquipmentPurchaseSelectionIndex;
                            indexList1 = LoadedSave.PersonalReality.Purchases.IndexOf(EquipmentPurchaseList[index]);
                            indexList2 = LoadedSave.PersonalReality.Purchases.IndexOf(EquipmentPurchaseList[index + 1]);
                            EquipmentPurchaseList.SwapCollectionItems(index, index + 1);
                            LoadedSave.PersonalReality.Purchases.SwapListItems(indexList1, indexList2);
                            EquipmentPurchaseSelectionIndex = index + 1;
                            break;
                        case 6:
                            index = CompanionsPurchaseSelectionIndex;
                            indexList1 = LoadedSave.PersonalReality.Purchases.IndexOf(CompanionsPurchaseList[index]);
                            indexList2 = LoadedSave.PersonalReality.Purchases.IndexOf(CompanionsPurchaseList[index + 1]);
                            CompanionsPurchaseList.SwapCollectionItems(index, index + 1);
                            LoadedSave.PersonalReality.Purchases.SwapListItems(indexList1, indexList2);
                            CompanionsPurchaseSelectionIndex = index + 1;
                            break;
                        case 7:
                            index = MiscPurchaseSelectionIndex;
                            indexList1 = LoadedSave.PersonalReality.Purchases.IndexOf(MiscPurchaseList[index]);
                            indexList2 = LoadedSave.PersonalReality.Purchases.IndexOf(MiscPurchaseList[index + 1]);
                            MiscPurchaseList.SwapCollectionItems(index, index + 1);
                            LoadedSave.PersonalReality.Purchases.SwapListItems(indexList1, indexList2);
                            MiscPurchaseSelectionIndex = index + 1;
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }

            DeletePurchaseCommand.NotifyCanExecuteChanged();
            MovePurchaseDownCommand.NotifyCanExecuteChanged();
            MovePurchaseUpCommand.NotifyCanExecuteChanged();
        }

        private bool CanMovePurchaseDown()
        {
            switch (PurchasesTabIndex)
            {
                case 0:
                    return BasicsPurchaseSelectionIndex < BasicsPurchaseList.Count - 1;
                case 1:
                    return UtilitiesPurchaseSelectionIndex < UtilitiesPurchaseList.Count - 1;
                case 2:
                    return CosmeticPurchaseSelectionIndex < CosmeticPurchaseList.Count - 1;
                case 3:
                    return FacilitiesPurchaseSelectionIndex < FacilitiesPurchaseList.Count - 1;
                case 4:
                    return ExtensionsPurchaseSelectionIndex < ExtensionsPurchaseList.Count - 1;
                case 5:
                    return EquipmentPurchaseSelectionIndex < EquipmentPurchaseList.Count - 1;
                case 6:
                    return CompanionsPurchaseSelectionIndex < CompanionsPurchaseList.Count - 1;
                case 7:
                    return MiscPurchaseSelectionIndex < MiscPurchaseList.Count - 1;
                default:
                    return false;
            }

        }

        [RelayCommand]
        private void NewLimitation()
        {
            SupplementDrawbackModel limitation = new($"Limitation #{LimitationList.Count + 1}");

            switch (LoadedWarehouseSupplement)
            {
                case Options.CosmicWarehouseSupplements.Generic:
                    LoadedSave.GenericWarehouse.Limitations.Add(limitation);
                    break;
                case Options.CosmicWarehouseSupplements.PersonalReality:
                    LoadedSave.PersonalReality.Limitations.Add(limitation);
                    break;
                default:
                    break;
            }

            LimitationList.Add(limitation);

            DeleteLimitationCommand.NotifyCanExecuteChanged();
            MoveLimitationDownCommand.NotifyCanExecuteChanged();
            MoveLimitationUpCommand.NotifyCanExecuteChanged();
        }

        [RelayCommand(CanExecute = "CanDeleteLimitation")]
        private void DeleteLimitation()
        {
            if (_dialogService.ConfirmDialog("Are you sure that you want to delete this Limitation? " +
                "This action cannot be reversed."))
            {
                switch (LoadedWarehouseSupplement)
                {
                    case Options.CosmicWarehouseSupplements.Generic:
                        LoadedSave.GenericWarehouse.Limitations.RemoveAt(LimitationSelectionIndex);
                        break;
                    case Options.CosmicWarehouseSupplements.PersonalReality:
                        LoadedSave.PersonalReality.Limitations.RemoveAt(LimitationSelectionIndex);
                        break;
                    default:
                        break;
                }

                LimitationList.RemoveAt(LimitationSelectionIndex);

                DeleteLimitationCommand.NotifyCanExecuteChanged();
                MoveLimitationDownCommand.NotifyCanExecuteChanged();
                MoveLimitationUpCommand.NotifyCanExecuteChanged();
            }
        }

        private bool CanDeleteLimitation()
        {
            return LimitationList.Any() && LimitationSelection != null;
        }

        [RelayCommand(CanExecute = "CanMoveLimitationUp")]
        private void MoveLimitationUp()
        {
            int index = LimitationSelectionIndex;

            LimitationList.SwapCollectionItems(index, index - 1);

            switch (LoadedWarehouseSupplement)
            {
                case Options.CosmicWarehouseSupplements.Generic:
                    LoadedSave.GenericWarehouse.Limitations.SwapListItems(index, index - 1);
                    break;
                case Options.CosmicWarehouseSupplements.PersonalReality:
                    LoadedSave.PersonalReality.Limitations.SwapListItems(index, index - 1);
                    break;
                default:
                    break;
            }

            LimitationSelectionIndex = index - 1;

            DeleteLimitationCommand.NotifyCanExecuteChanged();
            MoveLimitationDownCommand.NotifyCanExecuteChanged();
            MoveLimitationUpCommand.NotifyCanExecuteChanged();
        }

        private bool CanMoveLimitationUp()
        {
            return LimitationSelectionIndex > 0;
        }

        [RelayCommand(CanExecute = "CanMoveLimitationDown")]
        private void MoveLimitationDown()
        {
            int index = LimitationSelectionIndex;

            LimitationList.SwapCollectionItems(index, index + 1);

            switch (LoadedWarehouseSupplement)
            {
                case Options.CosmicWarehouseSupplements.Generic:
                    LoadedSave.GenericWarehouse.Limitations.SwapListItems(index, index + 1);
                    break;
                case Options.CosmicWarehouseSupplements.PersonalReality:
                    LoadedSave.PersonalReality.Limitations.SwapListItems(index, index + 1);
                    break;
                default:
                    break;
            }

            LimitationSelectionIndex = index + 1;

            DeleteLimitationCommand.NotifyCanExecuteChanged();
            MoveLimitationDownCommand.NotifyCanExecuteChanged();
            MoveLimitationUpCommand.NotifyCanExecuteChanged();
        }

        private bool CanMoveLimitationDown()
        {
            return LimitationSelectionIndex < LimitationList.Count - 1;
        }
        #endregion
    }
}
