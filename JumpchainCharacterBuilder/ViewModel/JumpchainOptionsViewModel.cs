﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using JumpchainCharacterBuilder.Attributes;
using JumpchainCharacterBuilder.Interfaces;
using JumpchainCharacterBuilder.Messages;
using JumpchainCharacterBuilder.Model;
using JumpchainCharacterBuilder.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;

namespace JumpchainCharacterBuilder.ViewModel
{
    public partial class JumpchainOptionsViewModel : ViewModelBase
    {
        // TODO - Implement EBM options correctly.
        #region Fields
        private readonly IDialogService _dialogService;

        [ObservableProperty]
        private SaveFile _loadedSave = new();
        [ObservableProperty]
        private Options _loadedOptions = new();
        [ObservableProperty]
        private AppSettingsModel _appSettings = new();

        [ObservableProperty]
        private int _defaultBudget = 1000;
        [ObservableProperty]
        private int _itemStipend = 100;
        [ObservableProperty]
        private bool _originDiscounts = false;
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Range(0, int.MaxValue, ErrorMessage = "Perk Freebie threshold must be a positive number.")]
        private int _perkFreebieThreshold = 100;
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Range(0, int.MaxValue, ErrorMessage = "Item Freebie threshold must be a positive number.")]
        private int _itemFreebieThreshold = 100;

        [ObservableProperty]
        private bool _allowPointBank = false;
        [ObservableProperty]
        private bool _allowGauntletBank = false;
        [ObservableProperty]
        private bool _allowCompanionsBank = false;
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Range(0, int.MaxValue, ErrorMessage = "Point bank cap must be a positive integer.")]
        private int _pointBankLimit = 300;
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Range(0, int.MaxValue, ErrorMessage = "Point bank cap must be a positive integer.")]
        private int _companionBankLimit = 300;

        [ObservableProperty]
        private Options.CosmicWarehouseSupplements _cosmicWarehouseSelection = new();
        [ObservableProperty]
        private Options.BodyModSupplements _bodyModSelection = new();
        [ObservableProperty]
        private Options.DrawbackSupplements _drawbackSupplementSelection = new();

        [ObservableProperty]
        private bool _genericWarehouseSelected = true;
        [ObservableProperty]
        private bool _personalRealitySelected = false;

        [ObservableProperty]
        private PersonalReality.CoreModes _pRCoreModeSelection = new();
        [ObservableProperty]
        private bool _pRPatientJumper = false;
        [ObservableProperty]
        private bool _pRPatientJumperCountFirstJump = false;
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Range(0, int.MaxValue, ErrorMessage = "Supplements cannot be taken on a negative Jump #.")]
        private int _pRSupplementDelay = 0;
        [ObservableProperty]
        private string _pRCoreModeDescription = "";
        [ObservableProperty]
        private bool _pRInvestmentAllowed = false;
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Range(1, int.MaxValue, ErrorMessage = "Ratio cannot be lower than 1:1.")]
        private int _pRInvestmentRatio = 25;

        [ObservableProperty]
        private bool _genericBodyModSelected = true;
        [ObservableProperty]
        private bool _sBBodyModSelected = false;
        [ObservableProperty]
        private bool _essentialBodyModSelected = false;

        [ObservableProperty]
        private int _sBBodyModBudget = 1000;
        [ObservableProperty]
        private bool _sBBodyModInvestmentAllowed = false;
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Range(1, int.MaxValue, ErrorMessage = "Ratio cannot be lower than 1:1.")]
        private int _sBBodyModInvestmentRatio = 1;

        [ObservableProperty]
        private EssentialBodyMod.StartingModes _eBMStartingModeSelection = new();
        [ObservableProperty]
        private EssentialBodyMod.EssenceModes _eBMEssenceModeSelection = new();
        [ObservableProperty]
        private EssentialBodyMod.AdvancementModes _eBMAdvancementModeSelection = new();
        [ObservableProperty]
        private EssentialBodyMod.EPAccessModes _eBMEPAccessModeSelection = new();
        [ObservableProperty]
        private bool _eBMInvestmentAllowed = false;
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Range(1, int.MaxValue, ErrorMessage = "Ratio cannot be lower than 1:1.")]
        private int _eBMInvestmentRatio = 1;

        [ObservableProperty]
        private int _gBMBudget = 0;
        [ObservableProperty]
        private int _gBMIncrementalBudget = 0;
        [ObservableProperty]
        private bool _gBMPurchasesAllowed = false;
        [ObservableProperty]
        private bool _gBMInvestmentAllowed = false;
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Range(1, int.MaxValue, ErrorMessage = "Ratio cannot be lower than 1:1.")]
        private int _gBMInvestmentRatio = 1;

        [ObservableProperty]
        private int _gWBudget = 0;
        [ObservableProperty]
        private int _gWIncrementalBudget = 0;
        [ObservableProperty]
        private bool _gWPurchasesAllowed = false;
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Range(0, int.MaxValue, ErrorMessage = "Supplements cannot be taken on a negative Jump #.")]
        private int _gWSupplementDelay = 0;
        [ObservableProperty]
        private bool _gWInvestmentAllowed = false;
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Range(1, int.MaxValue, ErrorMessage = "Ratio cannot be lower than 1:1.")]
        private int _gWInvestmentRatio = 1;

        [ObservableProperty]
        private bool _genericDrawbackSupplementSelected = true;
        [ObservableProperty]
        private bool _uDSSelected = false;
        [ObservableProperty]
        private bool _uUSelected = false;
        [ObservableProperty]
        private bool _genericDrawbackGauntletPoints = false;
        [ObservableProperty]
        private bool _genericDrawbackGauntletHalved = false;
        [ObservableProperty]
        private bool _uDSGauntletPoints = false;
        [ObservableProperty]
        private bool _uDSGauntletHalved = false;
        [ObservableProperty]
        private bool _uUGauntletPoints = false;
        [ObservableProperty]
        private bool _uUGauntletHalved = false;

        [ObservableProperty]
        private ObservableCollection<string> _userPerkCategories = new();
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeletePerkCategoryCommand))]
        [NotifyDataErrorInfo]
        [UniqueName(nameof(CompiledPerkCategories))]
        private string _userPerkCategorySelection = "";
        [ObservableProperty]
        private int _userPerkCategoryIndex = 0;
        [ObservableProperty]
        private ObservableCollection<string> _userItemCategories = new();
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteItemCategoryCommand))]
        [NotifyDataErrorInfo]
        [UniqueName(nameof(CompiledItemCategories))]
        private string _userItemCategorySelection = "";
        [ObservableProperty]
        private int _userItemCategoryIndex = 0;

        [ObservableProperty]
        private List<string> _compiledPerkCategories = new();
        [ObservableProperty]
        private List<string> _compiledItemCategories = new();

        #endregion

        #region Properties
        partial void OnDefaultBudgetChanged(int value) => LoadedOptions.DefaultBudget = value;

        partial void OnPerkFreebieThresholdChanged(int value)
        {
            if (!GetErrors(nameof(PerkFreebieThreshold)).Any())
            {
                LoadedOptions.DefaultPerkFreebieThreshold = value;
            }
        }

        partial void OnItemFreebieThresholdChanged(int value)
        {
            if (!GetErrors(nameof(ItemFreebieThreshold)).Any())
            {
                LoadedOptions.DefaultItemFreebieThreshold = value;
            }
        }

        partial void OnItemStipendChanged(int value) => LoadedOptions.DefaultItemStipend = value;

        partial void OnLoadedOptionsChanged(Options value) => ReloadOptions();

        partial void OnOriginDiscountsChanged(bool value) => LoadedOptions.OriginDiscounts = value;

        partial void OnAllowPointBankChanged(bool value) => LoadedOptions.AllowPointBank = value;

        partial void OnAllowGauntletBankChanged(bool value) => LoadedOptions.AllowGauntletBank = value;

        partial void OnAllowCompanionsBankChanged(bool value) => LoadedOptions.AllowCompanionsBank = value;

        partial void OnPointBankLimitChanged(int value)
        {
            if (!GetErrors(nameof(PointBankLimit)).Any())
            {
                LoadedOptions.PointBankLimit = value;
            }
        }

        partial void OnCompanionBankLimitChanged(int value)
        {
            if (!GetErrors(nameof(CompanionBankLimit)).Any())
            {
                LoadedOptions.CompanionBankLimit = value;
            }
        }

        partial void OnCosmicWarehouseSelectionChanged(Options.CosmicWarehouseSupplements value)
        {
            LoadedOptions.CosmicWarehouseSetting = value;

            switch (value)
            {
                case Options.CosmicWarehouseSupplements.Generic:
                    GenericWarehouseSelected = true;
                    PersonalRealitySelected = false;
                    break;
                case Options.CosmicWarehouseSupplements.PersonalReality:
                    GenericWarehouseSelected = false;
                    PersonalRealitySelected = true;
                    break;
            }

            Messenger.Send(new SupplementChangedMessage("Warehouse"));
        }

        partial void OnBodyModSelectionChanged(Options.BodyModSupplements value)
        {
            LoadedOptions.BodyModSetting = value;

            switch (value)
            {
                case Options.BodyModSupplements.Generic:
                    GenericBodyModSelected = true;
                    SBBodyModSelected = false;
                    EssentialBodyModSelected = false;
                    break;
                case Options.BodyModSupplements.SBBodyMod:
                    GenericBodyModSelected = false;
                    SBBodyModSelected = true;
                    EssentialBodyModSelected = false;
                    break;
                case Options.BodyModSupplements.EssentialBodyMod:
                    GenericBodyModSelected = false;
                    SBBodyModSelected = false;
                    EssentialBodyModSelected = true;
                    break;
            }

            Messenger.Send(new SupplementChangedMessage("Body Mod"));
        }

        partial void OnDrawbackSupplementSelectionChanged(Options.DrawbackSupplements value)
        {
            LoadedOptions.DrawbackSupplementSetting = value;

            switch (value)
            {
                case Options.DrawbackSupplements.Generic:
                    GenericDrawbackSupplementSelected = true;
                    UDSSelected = false;
                    UUSelected = false;
                    break;
                case Options.DrawbackSupplements.UDS:
                    GenericDrawbackSupplementSelected = false;
                    UDSSelected = true;
                    UUSelected = false;
                    break;
                case Options.DrawbackSupplements.UU:
                    GenericDrawbackSupplementSelected = false;
                    UDSSelected = false;
                    UUSelected = true;
                    break;
            }

            Messenger.Send(new SupplementChangedMessage("Drawback"));
        }

        partial void OnPRCoreModeSelectionChanged(PersonalReality.CoreModes value)
        {
            LoadedSave.PersonalReality.CoreMode = value;

            switch (PRCoreModeSelection)
            {
                case PersonalReality.CoreModes.Upfront:
                    LoadedSave.PersonalReality.Budget = 1500;
                    LoadedSave.PersonalReality.IncrementalBudget = 0;
                    LoadedSave.PersonalReality.IncrementalInterval = 1;
                    break;
                case PersonalReality.CoreModes.Incremental:
                    LoadedSave.PersonalReality.Budget = 500;
                    LoadedSave.PersonalReality.IncrementalBudget = 50;
                    LoadedSave.PersonalReality.IncrementalInterval = 1;
                    break;
                case PersonalReality.CoreModes.Unlimited:
                    LoadedSave.PersonalReality.Budget = 0;
                    LoadedSave.PersonalReality.IncrementalBudget = 0;
                    LoadedSave.PersonalReality.IncrementalInterval = 1;
                    break;
                case PersonalReality.CoreModes.Reasonable:
                    LoadedSave.PersonalReality.Budget = 3000;
                    LoadedSave.PersonalReality.IncrementalBudget = 100;
                    LoadedSave.PersonalReality.IncrementalInterval = 5;
                    break;
                case PersonalReality.CoreModes.Therehouse:
                    LoadedSave.PersonalReality.Budget = 5000;
                    LoadedSave.PersonalReality.IncrementalBudget = 0;
                    LoadedSave.PersonalReality.IncrementalInterval = 1;
                    break;
                default:
                    break;
            }
        }

        partial void OnPRPatientJumperChanged(bool value) => LoadedSave.PersonalReality.PatientJumper = value;

        partial void OnPRPatientJumperCountFirstJumpChanged(bool value) => LoadedSave.PersonalReality.PatientJumperCountFirstJump = value;

        partial void OnPRSupplementDelayChanged(int value)
        {
            if (!GetErrors(nameof(PRSupplementDelay)).Any())
            {
                LoadedSave.PersonalReality.SupplementDelay = value;
            }
        }

        partial void OnPRCoreModeDescriptionChanged(string value) => LoadedSave.PersonalReality.CoreModeDescription = value;

        partial void OnPRInvestmentAllowedChanged(bool value) => LoadedSave.PersonalReality.InvestmentAllowed = value;

        partial void OnPRInvestmentRatioChanged(int value)
        {
            if (!GetErrors(nameof(PRInvestmentRatio)).Any())
            {
                LoadedSave.PersonalReality.InvestmentRatio = value;
            }

        }

        partial void OnSBBodyModBudgetChanged(int value) => LoadedSave.SBBodyMod.Budget = value;

        partial void OnSBBodyModInvestmentAllowedChanged(bool value) => LoadedSave.SBBodyMod.InvestmentAllowed = value;

        partial void OnSBBodyModInvestmentRatioChanged(int value)
        {
            if (!GetErrors(nameof(SBBodyModInvestmentRatio)).Any())
            {
                LoadedSave.SBBodyMod.InvestmentRatio = value;
            }
        }

        partial void OnEBMStartingModeSelectionChanged(EssentialBodyMod.StartingModes value)
        {
            LoadedSave.EssentialBodyMod.StartingMode = value;

            switch (EBMStartingModeSelection)
            {
                case EssentialBodyMod.StartingModes.Hardcore:
                    LoadedSave.EssentialBodyMod.Budget = 0;
                    break;
                case EssentialBodyMod.StartingModes.Standard:
                    LoadedSave.EssentialBodyMod.Budget = 100;
                    break;
                case EssentialBodyMod.StartingModes.Heroic:
                    LoadedSave.EssentialBodyMod.Budget = 500;
                    break;
                default:
                    break;
            }
        }

        partial void OnEBMEssenceModeSelectionChanged(EssentialBodyMod.EssenceModes value) => LoadedSave.EssentialBodyMod.EssenceMode = value;

        partial void OnEBMAdvancementModeSelectionChanged(EssentialBodyMod.AdvancementModes value)
        {
            LoadedSave.EssentialBodyMod.AdvancementMode = value;

            switch (EBMAdvancementModeSelection)
            {
                case EssentialBodyMod.AdvancementModes.Standard:
                    LoadedSave.EssentialBodyMod.IncrementalBudget = 0;
                    LoadedSave.EssentialBodyMod.IncrementalInterval = 1;
                    break;
                case EssentialBodyMod.AdvancementModes.Meteoric:
                    LoadedSave.EssentialBodyMod.IncrementalBudget = 100;
                    LoadedSave.EssentialBodyMod.IncrementalInterval = 1;
                    break;
                case EssentialBodyMod.AdvancementModes.Heroic:
                    LoadedSave.EssentialBodyMod.IncrementalBudget = 50;
                    LoadedSave.EssentialBodyMod.IncrementalInterval = 1;
                    break;
                case EssentialBodyMod.AdvancementModes.Questing:
                    LoadedSave.EssentialBodyMod.IncrementalBudget = 0;
                    LoadedSave.EssentialBodyMod.IncrementalInterval = 1;
                    break;
                default:
                    break;
            }
        }

        partial void OnEBMEPAccessModeSelectionChanged(EssentialBodyMod.EPAccessModes value)
        {
            LoadedSave.EssentialBodyMod.EPAccessMode = value;

            if (EBMEPAccessModeSelection == EssentialBodyMod.EPAccessModes.NoAccess)
            {
                EBMInvestmentAllowed = false;
            }
            else
            {
                EBMInvestmentAllowed = true;
            }

        }
        partial void OnEBMInvestmentAllowedChanged(bool value) => LoadedSave.EssentialBodyMod.InvestmentAllowed = value;

        partial void OnEBMInvestmentRatioChanged(int value)
        {
            if (!GetErrors(nameof(EBMInvestmentRatio)).Any())
            {
                LoadedSave.EssentialBodyMod.InvestmentRatio = value;
            }
        }

        partial void OnGBMBudgetChanged(int value) => LoadedSave.GenericBodyMod.Budget = value;

        partial void OnGBMIncrementalBudgetChanged(int value) => LoadedSave.GenericBodyMod.IncrementalBudget = value;

        partial void OnGBMPurchasesAllowedChanged(bool value) => LoadedSave.GenericBodyMod.PurchasesAllowed = value;

        partial void OnGBMInvestmentAllowedChanged(bool value) => LoadedSave.GenericBodyMod.InvestmentAllowed = value;

        partial void OnGBMInvestmentRatioChanged(int value)
        {
            if (!GetErrors(nameof(GBMInvestmentRatio)).Any())
            {
                LoadedSave.GenericBodyMod.InvestmentRatio = value;
            }
        }

        partial void OnGWBudgetChanged(int value) => LoadedSave.GenericWarehouse.Budget = value;

        partial void OnGWIncrementalBudgetChanged(int value) => LoadedSave.GenericWarehouse.IncrementalBudget = value;

        partial void OnGWPurchasesAllowedChanged(bool value) => LoadedSave.GenericWarehouse.PurchasesAllowed = value;
        partial void OnGWSupplementDelayChanged(int value)
        {
            if (!GetErrors(nameof(GWSupplementDelay)).Any())
            {
                LoadedSave.GenericWarehouse.SupplementDelay = value;
            }
        }
        partial void OnGWInvestmentAllowedChanged(bool value) => LoadedSave.GenericWarehouse.InvestmentAllowed = value;

        partial void OnGWInvestmentRatioChanged(int value)
        {
            if (!GetErrors(nameof(GWInvestmentRatio)).Any())
            {
                LoadedSave.GenericWarehouse.InvestmentRatio = value;
            }
        }

        partial void OnGenericDrawbackGauntletPointsChanged(bool value) => LoadedSave.GenericDrawbackSupplement.AllowedDuringGauntlets = value;

        partial void OnGenericDrawbackGauntletHalvedChanged(bool value) => LoadedSave.GenericDrawbackSupplement.HalvedPointsDuringGauntlets = value;

        partial void OnUDSGauntletPointsChanged(bool value) => LoadedSave.UniversalDrawbackSupplement.AllowedDuringGauntlets = value;

        partial void OnUDSGauntletHalvedChanged(bool value) => LoadedSave.UniversalDrawbackSupplement.HalvedPointsDuringGauntlets = value;

        partial void OnUUGauntletPointsChanged(bool value) => LoadedSave.UUSupplement.AllowedDuringGauntlets = value;

        partial void OnUUGauntletHalvedChanged(bool value) => LoadedSave.UUSupplement.HalvedPointsDuringGauntlets = value;

        partial void OnUserPerkCategoryIndexChanged(int value)
        {
            if (value != -1 && UserPerkCategories.Any())
            {
                UserPerkCategorySelection = UserPerkCategories[value];
            }
        }

        partial void OnUserPerkCategorySelectionChanging(string value)
        {
            List<string> tempList = UserPerkCategories.ToList();

            tempList[UserPerkCategoryIndex] = value;

            tempList.AddRange(LoadedSave.BasePerkCategoryList);

            CompiledPerkCategories = new(tempList);
        }

        partial void OnUserPerkCategorySelectionChanged(string value)
        {
            int index = UserPerkCategoryIndex;
            string oldCategory = LoadedSave.UserPerkCategoryList[index];

            if (!GetErrors(nameof(UserPerkCategorySelection)).Any())
            {
                if (value != null && UserPerkCategoryIndex != -1)
                {
                    if (UserPerkCategories[index] != value)
                    {
                        LoadedSave.UserPerkCategoryList[index] = value;
                        UserPerkCategories[index] = value;

                        UpdateCategories(perks: true);
                        ChangePurchaseCategory(oldCategory, value);
                    }
                }
            }
        }

        partial void OnUserItemCategoryIndexChanged(int value)
        {
            if (value != -1 && UserItemCategories.Any())
            {
                UserItemCategorySelection = UserItemCategories[value];
            }
        }

        partial void OnUserItemCategorySelectionChanging(string value)
        {
            List<string> tempList = UserItemCategories.ToList();

            tempList[UserItemCategoryIndex] = value;

            tempList.AddRange(LoadedSave.BaseItemCategoryList);

            CompiledItemCategories = new(tempList);
        }

        partial void OnUserItemCategorySelectionChanged(string value)
        {
            int index = UserItemCategoryIndex;
            string oldCategory = LoadedSave.UserItemCategoryList[index];

            if (!GetErrors(nameof(UserItemCategorySelection)).Any())
            {
                if (value != null && UserItemCategoryIndex != -1)
                {
                    if (UserItemCategories[index] != value)
                    {
                        LoadedSave.UserItemCategoryList[index] = value;
                        UserItemCategories[index] = value;

                        UpdateCategories(perks: false);
                        ChangePurchaseCategory(oldCategory, value);
                    }
                }
            }
        }

        public Dictionary<string, Options.CosmicWarehouseSupplements> CosmicWarehouseList { get; } =
            new()
            {
                {"Generic Supplement", Options.CosmicWarehouseSupplements.Generic},
                {"Personal Reality", Options.CosmicWarehouseSupplements.PersonalReality}
            };

        public Dictionary<string, Options.BodyModSupplements> BodyModList { get; } =
            new()
            {
                {"Generic Supplement", Options.BodyModSupplements.Generic},
                {"SB Body Mod", Options.BodyModSupplements.SBBodyMod},
                {"Essential Body Mod", Options.BodyModSupplements.EssentialBodyMod}
            };

        public Dictionary<string, Options.DrawbackSupplements> DrawbackSupplementList { get; } =
            new()
            {
                {"Generic Supplement", Options.DrawbackSupplements.Generic},
                {"UDS", Options.DrawbackSupplements.UDS},
                {"U.U. Supplement", Options.DrawbackSupplements.UU}
            };

        public Dictionary<string, PersonalReality.CoreModes> PRCoreModeList { get; } =
            new()
            {
                {"Upfront", PersonalReality.CoreModes.Upfront},
                {"Incremental", PersonalReality.CoreModes.Incremental},
                {"Unlimited", PersonalReality.CoreModes.Unlimited},
                {"Reasonable", PersonalReality.CoreModes.Reasonable},
                {"Therehouse", PersonalReality.CoreModes.Therehouse}
            };

        public Dictionary<string, EssentialBodyMod.StartingModes> EBMStartingModeList { get; } =
            new()
            {
                {"Hardcore", EssentialBodyMod.StartingModes.Hardcore},
                {"Standard", EssentialBodyMod.StartingModes.Standard},
                {"Heroic", EssentialBodyMod.StartingModes.Heroic}
            };

        public Dictionary<string, EssentialBodyMod.EssenceModes> EBMEssenceModeList { get; } =
            new()
            {
                {"None", EssentialBodyMod.EssenceModes.NoEssence},
                {"Single", EssentialBodyMod.EssenceModes.SingleEssence},
                {"Dual", EssentialBodyMod.EssenceModes.DualEssence},
                {"Multi", EssentialBodyMod.EssenceModes.MultiEssence}
            };

        public Dictionary<string, EssentialBodyMod.AdvancementModes> EBMAdvancementModeList { get; } =
            new()
            {
                {"Standard", EssentialBodyMod.AdvancementModes.Standard},
                {"Meteoric", EssentialBodyMod.AdvancementModes.Meteoric},
                {"Heroic", EssentialBodyMod.AdvancementModes.Heroic},
                {"Questing", EssentialBodyMod.AdvancementModes.Questing}
            };

        public Dictionary<string, EssentialBodyMod.EPAccessModes> EBMEPAccessModeList { get; } =
            new()
            {
                {"No Access", EssentialBodyMod.EPAccessModes.NoAccess},
                {"Lesser", EssentialBodyMod.EPAccessModes.LesserAccess},
                {"Standard", EssentialBodyMod.EPAccessModes.StandardAccess}
            };

        #endregion

        #region Constructor
        public JumpchainOptionsViewModel()
        {
            
        }

        public JumpchainOptionsViewModel(IDialogService dialogService)
        {
            Messenger.Register<SaveDataChangedMessage>(this, (r, m) =>
            {
                AssignOptions();
                ReloadOptions();
            });
            Messenger.Register<SaveDataSendMessage>(this, (r, m) =>
            {
                LoadedSave = m.Value;

                AssignOptions();
            });
            Messenger.Register<SettingsLoadedMessage>(this, (r, m) =>
            {
                AppSettings = m.Value;
            });

            _dialogService = dialogService;
        }
        #endregion

        #region Methods

        public void AssignOptions() => LoadedOptions = LoadedSave.Options;

        public void ReloadOptions()
        {
            DefaultBudget = LoadedOptions.DefaultBudget;
            ItemStipend = LoadedOptions.DefaultItemStipend;
            OriginDiscounts = LoadedOptions.OriginDiscounts;
            PerkFreebieThreshold = LoadedOptions.DefaultPerkFreebieThreshold;
            ItemFreebieThreshold = LoadedOptions.DefaultItemFreebieThreshold;

            AllowPointBank = LoadedOptions.AllowPointBank;
            AllowGauntletBank = LoadedOptions.AllowGauntletBank;
            AllowCompanionsBank = LoadedOptions.AllowCompanionsBank;
            PointBankLimit = LoadedOptions.PointBankLimit;
            CompanionBankLimit = LoadedOptions.CompanionBankLimit;

            CosmicWarehouseSelection = LoadedOptions.CosmicWarehouseSetting;
            BodyModSelection = LoadedOptions.BodyModSetting;
            DrawbackSupplementSelection = LoadedOptions.DrawbackSupplementSetting;

            PRCoreModeSelection = LoadedSave.PersonalReality.CoreMode;
            PRPatientJumper = LoadedSave.PersonalReality.PatientJumper;
            PRPatientJumperCountFirstJump = LoadedSave.PersonalReality.PatientJumperCountFirstJump;
            PRSupplementDelay = LoadedSave.PersonalReality.SupplementDelay;
            PRCoreModeDescription = LoadedSave.PersonalReality.CoreModeDescription;
            PRInvestmentAllowed = LoadedSave.PersonalReality.InvestmentAllowed;
            PRInvestmentRatio = LoadedSave.PersonalReality.InvestmentRatio;

            SBBodyModBudget = LoadedSave.SBBodyMod.Budget;
            SBBodyModInvestmentAllowed = LoadedSave.SBBodyMod.InvestmentAllowed;
            SBBodyModInvestmentRatio = LoadedSave.SBBodyMod.InvestmentRatio;

            EBMStartingModeSelection = LoadedSave.EssentialBodyMod.StartingMode;
            EBMEssenceModeSelection = LoadedSave.EssentialBodyMod.EssenceMode;
            EBMAdvancementModeSelection = LoadedSave.EssentialBodyMod.AdvancementMode;
            EBMEPAccessModeSelection = LoadedSave.EssentialBodyMod.EPAccessMode;

            GBMBudget = LoadedSave.GenericBodyMod.Budget;
            GBMIncrementalBudget = LoadedSave.GenericBodyMod.IncrementalBudget;
            GBMPurchasesAllowed = LoadedSave.GenericBodyMod.PurchasesAllowed;
            GBMInvestmentAllowed = LoadedSave.GenericBodyMod.InvestmentAllowed;
            GBMInvestmentRatio = LoadedSave.GenericBodyMod.InvestmentRatio;

            GWBudget = LoadedSave.GenericWarehouse.Budget;
            GWIncrementalBudget = LoadedSave.GenericWarehouse.IncrementalBudget;
            GWPurchasesAllowed = LoadedSave.GenericWarehouse.PurchasesAllowed;
            GWSupplementDelay = LoadedSave.GenericWarehouse.SupplementDelay;
            GWInvestmentAllowed = LoadedSave.GenericWarehouse.InvestmentAllowed;
            GWInvestmentRatio = LoadedSave.GenericWarehouse.InvestmentRatio;

            GenericDrawbackGauntletPoints = LoadedSave.GenericDrawbackSupplement.AllowedDuringGauntlets;
            GenericDrawbackGauntletHalved = LoadedSave.GenericDrawbackSupplement.HalvedPointsDuringGauntlets;

            UDSGauntletPoints = LoadedSave.UniversalDrawbackSupplement.AllowedDuringGauntlets;
            UDSGauntletHalved = LoadedSave.UniversalDrawbackSupplement.HalvedPointsDuringGauntlets;

            UUGauntletPoints = LoadedSave.UUSupplement.AllowedDuringGauntlets;
            UUGauntletHalved = LoadedSave.UUSupplement.HalvedPointsDuringGauntlets;

            LoadCategories();
        }

        private void LoadCategories()
        {
            UserPerkCategories = new(LoadedSave.UserPerkCategoryList);
            UserItemCategories = new(LoadedSave.UserItemCategoryList);

            CompiledPerkCategories = LoadedSave.PerkCategoryList;
            CompiledItemCategories = LoadedSave.ItemCategoryList;

            if (UserPerkCategories.Any())
            {
                UserPerkCategoryIndex = 0;
                UserPerkCategorySelection = UserPerkCategories[UserPerkCategoryIndex];
            }
            if (UserItemCategories.Any())
            {
                UserItemCategoryIndex = 0;
                UserItemCategorySelection = UserItemCategories[UserItemCategoryIndex];
            }
        }

        private void UpdateCategories(bool perks)
        {
            List<string> compiledList = new();

            if (perks)
            {
                compiledList.AddRange(LoadedSave.BasePerkCategoryList);
                compiledList.AddRange(UserPerkCategories);

                LoadedSave.PerkCategoryList = compiledList;
                CompiledPerkCategories = new(compiledList);
            }
            else
            {
                compiledList.AddRange(LoadedSave.BaseItemCategoryList);
                compiledList.AddRange(UserItemCategories);

                LoadedSave.ItemCategoryList = compiledList;
                CompiledItemCategories = new(compiledList);
            }
        }

        private void ChangePurchaseCategory(string oldCategory, string newCategory)
        {
            foreach (Jump jump in LoadedSave.JumpList)
            {
                foreach (JumpBuild build in jump.Build)
                {
                    foreach (Purchase purchase in build.Purchase)
                    {
                        if (purchase.Category == oldCategory)
                        {
                            purchase.Category = newCategory;
                        }
                    }
                }
            }

            Messenger.Send(new CategoryChangedMessage(true));
        }
        #endregion

        #region Commands
        [RelayCommand]
        private void NewPerkCategory()
        {
            int count = LoadedSave.PerkCategoryList.Count + 1;
            string category = $"Custom Category #{count}";

            while (LoadedSave.PerkCategoryList.Contains(category))
            {
                count++;
                category = $"Custom Category #{count}";
            }

            UserPerkCategories.Add(category);
            LoadedSave.UserPerkCategoryList.Add(category);

            UpdateCategories(perks: true);

            UserPerkCategoryIndex = UserPerkCategories.Count - 1;

            Messenger.Send(new CategoryChangedMessage(true));

            DeletePerkCategoryCommand.NotifyCanExecuteChanged();
        }

        [RelayCommand(CanExecute = nameof(CanDeletePerkCategory))]
        private void DeletePerkCategory()
        {
            if (_dialogService.ConfirmDialog("Are you sure that you want to delete this category? " +
                "This will reset all Perks using this to the default."))
            {
                string category = UserPerkCategorySelection;

                ChangePurchaseCategory(category, "Other Perk");

                UserPerkCategories.Remove(category);
                CompiledPerkCategories.Remove(category);

                UpdateCategories(perks: true);

                if (UserPerkCategories.Any())
                {
                    UserPerkCategoryIndex = 0;
                }

                DeletePerkCategoryCommand.NotifyCanExecuteChanged();
            }
        }

        private bool CanDeletePerkCategory()
        {
            return UserPerkCategorySelection != "" && UserPerkCategorySelection != null && UserPerkCategoryIndex != -1;
        }

        [RelayCommand]
        private void NewItemCategory()
        {
            int count = LoadedSave.ItemCategoryList.Count + 1;
            string category = $"Custom Category #{count}";

            while (LoadedSave.ItemCategoryList.Contains(category))
            {
                count++;
                category = $"Custom Category #{count}";
            }

            UserItemCategories.Add(category);
            LoadedSave.UserItemCategoryList.Add(category);

            UpdateCategories(perks: false);

            UserItemCategoryIndex = UserItemCategories.Count - 1;

            Messenger.Send(new CategoryChangedMessage(true));

            DeleteItemCategoryCommand.NotifyCanExecuteChanged();
        }

        [RelayCommand(CanExecute = nameof(CanDeleteItemCategory))]
        private void DeleteItemCategory()
        {
            if (_dialogService.ConfirmDialog("Are you sure that you want to delete this category? " +
                "This will reset all Items using this to the default."))
            {
                string category = UserItemCategorySelection;

                ChangePurchaseCategory(category, "Other Item");

                UserItemCategories.Remove(category);
                CompiledItemCategories.Remove(category);

                UpdateCategories(perks: true);

                if (UserItemCategories.Any())
                {
                    UserItemCategoryIndex = 0;
                }

                DeleteItemCategoryCommand.NotifyCanExecuteChanged();
            }
        }

        private bool CanDeleteItemCategory()
        {
            return UserItemCategorySelection != "" && UserItemCategorySelection != null && UserItemCategoryIndex != -1;
        }
        #endregion
    }
}
