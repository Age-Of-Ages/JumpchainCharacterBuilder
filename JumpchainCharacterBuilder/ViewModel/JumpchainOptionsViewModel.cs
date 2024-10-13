using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using JumpchainCharacterBuilder.Attributes;
using JumpchainCharacterBuilder.Interfaces;
using JumpchainCharacterBuilder.Messages;
using JumpchainCharacterBuilder.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace JumpchainCharacterBuilder.ViewModel
{
    public partial class JumpchainOptionsViewModel : ViewModelBase
    {
        #region Fields
        private readonly IDialogService _dialogService;
        [ObservableProperty]
        private bool _spellCheckEnabled = true;

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
        private bool _allowSupplementedJumpBankSharing = false;
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
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _genericWarehouseName = "";
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _genericWarehouseVersion = "";
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _genericWarehouseAuthor = "";
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _genericWarehouseSource = "";
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _genericWarehouseFullDescription = "";

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
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _pRCoreModeDescription = "";
        [ObservableProperty]
        private bool _pRInvestmentAllowed = true;
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
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _genericBodyModName = "";
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _genericBodyModVersion = "";
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _genericBodyModAuthor = "";
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _genericBodyModSource = "";
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _genericBodyModFullDescription = "";

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
        private bool _eBMUnlockableEssenceModifierAvailable = false;
        [ObservableProperty]
        private bool _eBMUnlockableEssenceModifier = false;
        [ObservableProperty]
        private EssentialBodyMod.AdvancementModes _eBMAdvancementModeSelection = new();
        [ObservableProperty]
        private EssentialBodyMod.EPAccessModes _eBMEPAccessModeSelection = new();
        [ObservableProperty]
        private bool _eBMEPAccessModifiersAvailable = false;
        [ObservableProperty]
        private EssentialBodyMod.EPAccessModifiers _eBMEPAccessModifier = new();
        [ObservableProperty]
        private bool _eBMTrainingAllowanceAvailable = true;
        [ObservableProperty]
        private bool _eBMTrainingAllowance = false;
        [ObservableProperty]
        private bool _eBMTemperedBySufferingAvailable = true;
        [ObservableProperty]
        private bool _eBMTemperedBySuffering = false;
        [ObservableProperty]
        private EssentialBodyMod.UnbalancedVariantModes _eBMUnbalancedMode = new();
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _eBMUnbalancedModeDescription = "";
        [ObservableProperty]
        private EssentialBodyMod.Limiters _eBMLimiter = new();
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _eBMLimiterDescription = "";
        [ObservableProperty]
        private bool _eBMInvestmentAllowed = false;
        [ObservableProperty]
        private bool _eBMLimitInvestment = false;
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
        private ObservableCollection<string> _userPerkCategories = [];
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeletePerkCategoryCommand))]
        [NotifyCanExecuteChangedFor(nameof(MovePerkCategoryUpCommand))]
        [NotifyCanExecuteChangedFor(nameof(MovePerkCategoryDownCommand))]
        [NotifyDataErrorInfo]
        [UniqueName(nameof(CompiledPerkCategories))]
        [XmlFilter]
        private string _userPerkCategorySelection = "";
        [ObservableProperty]
        private int _userPerkCategoryIndex = 0;
        [ObservableProperty]
        private ObservableCollection<string> _userItemCategories = [];
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteItemCategoryCommand))]
        [NotifyCanExecuteChangedFor(nameof(MoveItemCategoryUpCommand))]
        [NotifyCanExecuteChangedFor(nameof(MoveItemCategoryDownCommand))]
        [NotifyDataErrorInfo]
        [UniqueName(nameof(CompiledItemCategories))]
        [XmlFilter]
        private string _userItemCategorySelection = "";
        [ObservableProperty]
        private int _userItemCategoryIndex = 0;

        [ObservableProperty]
        private List<string> _compiledPerkCategories = [];
        [ObservableProperty]
        private List<string> _compiledItemCategories = [];

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

        partial void OnAllowSupplementedJumpBankSharingChanged(bool value) => LoadedOptions.AllowSupplementedJumpBankSharing = value;

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

        partial void OnPRCoreModeDescriptionChanged(string value)
        {
            if (!GetErrors(nameof(PRCoreModeDescription)).Any())
            {
                LoadedSave.PersonalReality.CoreModeDescription = value;
            }
        }

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

        partial void OnEBMEssenceModeSelectionChanged(EssentialBodyMod.EssenceModes value)
        {
            LoadedSave.EssentialBodyMod.EssenceMode = value;

            if (value != EssentialBodyMod.EssenceModes.NoEssence)
            {
                EBMUnlockableEssenceModifierAvailable = true;
            }
            else
            {
                EBMUnlockableEssenceModifierAvailable = false;
            }
        }

        partial void OnEBMUnlockableEssenceModifierChanged(bool value) => LoadedSave.EssentialBodyMod.UnlockableEssenceModifier = value;

        partial void OnEBMAdvancementModeSelectionChanged(EssentialBodyMod.AdvancementModes value)
        {
            LoadedSave.EssentialBodyMod.AdvancementMode = value;

            switch (EBMAdvancementModeSelection)
            {
                case EssentialBodyMod.AdvancementModes.Standard:
                    LoadedSave.EssentialBodyMod.IncrementalBudget = 0;
                    LoadedSave.EssentialBodyMod.IncrementalInterval = 1;

                    if (EBMEPAccessModeSelection == EssentialBodyMod.EPAccessModes.NoAccess)
                    {
                        EBMTrainingAllowanceAvailable = true;
                    }
                    else
                    {
                        EBMTrainingAllowanceAvailable = false;
                    }
                    break;
                case EssentialBodyMod.AdvancementModes.Meteoric:
                    LoadedSave.EssentialBodyMod.IncrementalBudget = 100;
                    LoadedSave.EssentialBodyMod.IncrementalInterval = 1;

                    EBMTrainingAllowanceAvailable = false;
                    break;
                case EssentialBodyMod.AdvancementModes.Heroic:
                    LoadedSave.EssentialBodyMod.IncrementalBudget = 50;
                    LoadedSave.EssentialBodyMod.IncrementalInterval = 1;

                    EBMTrainingAllowanceAvailable = false;
                    break;
                case EssentialBodyMod.AdvancementModes.Questing:
                    LoadedSave.EssentialBodyMod.IncrementalBudget = 0;
                    LoadedSave.EssentialBodyMod.IncrementalInterval = 1;

                    EBMTrainingAllowanceAvailable = false;
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
                EBMEPAccessModifiersAvailable = false;

                if (EBMAdvancementModeSelection == EssentialBodyMod.AdvancementModes.Standard)
                {
                    EBMTrainingAllowanceAvailable = true;
                }
                else
                {
                    EBMTrainingAllowanceAvailable = false;
                }
            }
            else
            {
                EBMInvestmentAllowed = true;
                EBMEPAccessModifiersAvailable = true;
                EBMTrainingAllowanceAvailable = false;
            }

        }

        partial void OnEBMEPAccessModifierChanged(EssentialBodyMod.EPAccessModifiers value)
        {
            LoadedSave.EssentialBodyMod.EPAccessModifier = value;

            if (value != EssentialBodyMod.EPAccessModifiers.RetroCumulative)
            {
                EBMTemperedBySufferingAvailable = true;
            }
            else
            {
                EBMTemperedBySufferingAvailable = false;
            }
        }

        partial void OnEBMTrainingAllowanceChanged(bool value) => LoadedSave.EssentialBodyMod.TrainingAllowance = value;

        partial void OnEBMTemperedBySufferingChanged(bool value) => LoadedSave.EssentialBodyMod.TemperedBySuffering = value;

        partial void OnEBMUnbalancedModeChanged(EssentialBodyMod.UnbalancedVariantModes value) => LoadedSave.EssentialBodyMod.UnbalancedVariantMode = value;

        partial void OnEBMUnbalancedModeDescriptionChanged(string value)
        {
            if (!GetErrors(nameof(EBMUnbalancedModeDescription)).Any())
            {
                LoadedSave.EssentialBodyMod.UnbalancedModeDescription = value;
            }
        }

        partial void OnEBMLimiterChanged(EssentialBodyMod.Limiters value) => LoadedSave.EssentialBodyMod.Limiter = value;

        partial void OnEBMLimiterDescriptionChanged(string value)
        {

            if (!GetErrors(nameof(EBMLimiterDescription)).Any())
            {
                LoadedSave.EssentialBodyMod.LimiterDescription = value;
            }
        }

        partial void OnEBMInvestmentAllowedChanged(bool value) => LoadedSave.EssentialBodyMod.InvestmentAllowed = value;

        partial void OnEBMLimitInvestmentChanged(bool value) => LoadedSave.EssentialBodyMod.LimitInvestment = value;

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
            if (!GetErrors(nameof(UserPerkCategorySelection)).Any())
            {
                List<string> tempList = UserPerkCategories.ToList();

                tempList[UserPerkCategoryIndex] = value;

                tempList.AddRange(LoadedSave.BasePerkCategoryList);

                CompiledPerkCategories = new(tempList);
            }
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

            if (!GetErrors(nameof(UserItemCategorySelection)).Any())
            {
                List<string> tempList = UserItemCategories.ToList();

                tempList[UserItemCategoryIndex] = value;

                tempList.AddRange(LoadedSave.BaseItemCategoryList);

                CompiledItemCategories = new(tempList);
            }
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

        partial void OnGenericWarehouseNameChanged(string value)
        {
            if (!GetErrors(nameof(GenericWarehouseName)).Any())
            {
                LoadedSave.GenericWarehouse.Name = value;
            }
        }

        partial void OnGenericWarehouseAuthorChanged(string value)
        {
            if (!GetErrors(nameof(GenericWarehouseAuthor)).Any())
            {
                LoadedSave.GenericWarehouse.Author = value;
            }
        }

        partial void OnGenericWarehouseVersionChanged(string value)
        {
            if (!GetErrors(nameof(GenericWarehouseVersion)).Any())
            {
                LoadedSave.GenericWarehouse.Version = value;
            }
        }

        partial void OnGenericWarehouseSourceChanged(string value)
        {
            if (!GetErrors(nameof(GenericWarehouseSource)).Any())
            {
                LoadedSave.GenericWarehouse.Source = value;
            }
        }

        partial void OnGenericWarehouseFullDescriptionChanged(string value)
        {
            if (!GetErrors(nameof(GenericWarehouseFullDescription)).Any())
            {
                LoadedSave.GenericWarehouse.FullDescription = value;
            }
        }

        partial void OnGenericBodyModNameChanged(string value)
        {
            if (!GetErrors(nameof(GenericBodyModName)).Any())
            {
                LoadedSave.GenericBodyMod.Name = value;
            }
        }

        partial void OnGenericBodyModAuthorChanged(string value)
        {
            if (!GetErrors(nameof(GenericBodyModAuthor)).Any())
            {
                LoadedSave.GenericBodyMod.Author = value;
            }
        }

        partial void OnGenericBodyModVersionChanged(string value)
        {
            if (!GetErrors(nameof(GenericBodyModVersion)).Any())
            {
                LoadedSave.GenericBodyMod.Version = value;
            }
        }

        partial void OnGenericBodyModSourceChanged(string value)
        {
            if (!GetErrors(nameof(GenericBodyModSource)).Any())
            {
                LoadedSave.GenericBodyMod.Source = value;
            }
        }

        partial void OnGenericBodyModFullDescriptionChanged(string value)
        {
            if (!GetErrors(nameof(GenericBodyModFullDescription)).Any())
            {
                LoadedSave.GenericBodyMod.FullDescription = value;
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

        public Dictionary<string, EssentialBodyMod.EPAccessModifiers> EBMEPAccessModifierList { get; } =
            new()
            {
                {"None", EssentialBodyMod.EPAccessModifiers.None },
                {"Cumulative", EssentialBodyMod.EPAccessModifiers.Cumulative },
                {"Retroactive Cumulative", EssentialBodyMod.EPAccessModifiers.RetroCumulative }
            };

        public Dictionary<string, EssentialBodyMod.UnbalancedVariantModes> EBMUnbalancedVariantModeList { get; } =
            new()
            {
                {"None", EssentialBodyMod.UnbalancedVariantModes.None },
                {"Harmonized Essence", EssentialBodyMod.UnbalancedVariantModes.Harmonized },
                {"Very Harmonized Essence", EssentialBodyMod.UnbalancedVariantModes.VeryHarmonized },
                {"Perfectly Harmonized Essence", EssentialBodyMod.UnbalancedVariantModes.PerfectlyHarmonized }
            };

        public Dictionary<string, EssentialBodyMod.Limiters> EBMLimiterList { get; } =
            new()
            {
                {"None", EssentialBodyMod.Limiters.None },
                {"Everyday Hero Limiter", EssentialBodyMod.Limiters.EverydayHero },
                {"Street Level Limiter", EssentialBodyMod.Limiters.StreetLevel },
                {"Mid Level Limiter", EssentialBodyMod.Limiters.MidLevel },
                {"Body Mod Limiter", EssentialBodyMod.Limiters.BodyMod },
                {"Scaling Limiter I", EssentialBodyMod.Limiters.ScalingI },
                {"Scaling Limiter II", EssentialBodyMod.Limiters.ScalingII },
                {"Vanishing Limiter", EssentialBodyMod.Limiters.Vanishing }
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

                SpellCheckEnabled = AppSettings.SpellCheckEnabled;
            });
            Messenger.Register<SettingsChangedMessage>(this, (r, m) =>
            {
                SpellCheckEnabled = AppSettings.SpellCheckEnabled;
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
            AllowSupplementedJumpBankSharing = LoadedOptions.AllowSupplementedJumpBankSharing;
            AllowCompanionsBank = LoadedOptions.AllowCompanionsBank;
            PointBankLimit = LoadedOptions.PointBankLimit;
            CompanionBankLimit = LoadedOptions.CompanionBankLimit;

            CosmicWarehouseSelection = LoadedOptions.CosmicWarehouseSetting;
            BodyModSelection = LoadedOptions.BodyModSetting;
            DrawbackSupplementSelection = LoadedOptions.DrawbackSupplementSetting;

            GenericWarehouseName = LoadedSave.GenericWarehouse.Name;
            GenericWarehouseAuthor = LoadedSave.GenericWarehouse.Author;
            GenericWarehouseVersion = LoadedSave.GenericWarehouse.Version;
            GenericWarehouseSource = LoadedSave.GenericWarehouse.Source;
            GenericWarehouseFullDescription = LoadedSave.GenericWarehouse.FullDescription;

            PRCoreModeSelection = LoadedSave.PersonalReality.CoreMode;
            PRPatientJumper = LoadedSave.PersonalReality.PatientJumper;
            PRPatientJumperCountFirstJump = LoadedSave.PersonalReality.PatientJumperCountFirstJump;
            PRSupplementDelay = LoadedSave.PersonalReality.SupplementDelay;
            PRCoreModeDescription = LoadedSave.PersonalReality.CoreModeDescription;
            PRInvestmentAllowed = LoadedSave.PersonalReality.InvestmentAllowed;
            PRInvestmentRatio = LoadedSave.PersonalReality.InvestmentRatio;

            GenericBodyModName = LoadedSave.GenericBodyMod.Name;
            GenericBodyModAuthor = LoadedSave.GenericBodyMod.Author;
            GenericBodyModVersion = LoadedSave.GenericBodyMod.Version;
            GenericBodyModSource = LoadedSave.GenericBodyMod.Source;
            GenericBodyModFullDescription = LoadedSave.GenericBodyMod.FullDescription;

            SBBodyModBudget = LoadedSave.SBBodyMod.Budget;
            SBBodyModInvestmentAllowed = LoadedSave.SBBodyMod.InvestmentAllowed;
            SBBodyModInvestmentRatio = LoadedSave.SBBodyMod.InvestmentRatio;

            EBMStartingModeSelection = LoadedSave.EssentialBodyMod.StartingMode;
            EBMEssenceModeSelection = LoadedSave.EssentialBodyMod.EssenceMode;
            EBMUnlockableEssenceModifier = LoadedSave.EssentialBodyMod.UnlockableEssenceModifier;
            EBMAdvancementModeSelection = LoadedSave.EssentialBodyMod.AdvancementMode;
            EBMEPAccessModeSelection = LoadedSave.EssentialBodyMod.EPAccessMode;
            EBMEPAccessModifier = LoadedSave.EssentialBodyMod.EPAccessModifier;
            EBMTrainingAllowance = LoadedSave.EssentialBodyMod.TrainingAllowance;
            EBMTemperedBySuffering = LoadedSave.EssentialBodyMod.TemperedBySuffering;
            EBMUnbalancedMode = LoadedSave.EssentialBodyMod.UnbalancedVariantMode;
            EBMUnbalancedModeDescription = LoadedSave.EssentialBodyMod.UnbalancedModeDescription;
            EBMLimiter = LoadedSave.EssentialBodyMod.Limiter;
            EBMLimiterDescription = LoadedSave.EssentialBodyMod.LimiterDescription;
            EBMLimitInvestment = LoadedSave.EssentialBodyMod.LimitInvestment;

            if (EBMEssenceModeSelection != EssentialBodyMod.EssenceModes.NoEssence)
            {
                EBMUnlockableEssenceModifierAvailable = true;
            }
            else
            {
                EBMUnlockableEssenceModifierAvailable = false;
            }
            if (EBMAdvancementModeSelection == EssentialBodyMod.AdvancementModes.Standard && EBMEPAccessModeSelection == EssentialBodyMod.EPAccessModes.NoAccess)
            {
                EBMTrainingAllowanceAvailable = true;
            }
            else
            {
                EBMTrainingAllowanceAvailable = false;
            }
            if (EBMEPAccessModeSelection == EssentialBodyMod.EPAccessModes.NoAccess)
            {
                EBMInvestmentAllowed = false;
                EBMEPAccessModifiersAvailable = false;
            }
            else
            {
                EBMInvestmentAllowed = true;
                EBMEPAccessModifiersAvailable = true;
            }
            if (EBMEPAccessModifier != EssentialBodyMod.EPAccessModifiers.RetroCumulative)
            {
                EBMTemperedBySufferingAvailable = true;
            }
            else
            {
                EBMTemperedBySufferingAvailable = false;
            }


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
            List<string> compiledList = [];

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
        public void FormatInput(string targetPropertyName)
        {
            string? oldString = PropertyAccess.GetString(this, targetPropertyName);
            if (!string.IsNullOrWhiteSpace(oldString))
            {
                bool removeAllLineBreaks = AppSettings.FormatterDeleteAllLineBreaks;
                bool leaveDoubleLineBreaks = AppSettings.FormatterLeaveDoubleLineBreaks;

                string newString = FormatHelper.RemoveLineBreaks(oldString, removeAllLineBreaks, leaveDoubleLineBreaks);
                newString = FormatHelper.RemoveSpaces(newString);
                newString = FormatHelper.XmlSafeFormat(newString);

                GetType().GetProperty(targetPropertyName)?.SetValue(this, newString);
            }
        }

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
                LoadedSave.UserPerkCategoryList.Remove(category);

                UpdateCategories(perks: true);

                if (UserPerkCategories.Any())
                {
                    UserPerkCategoryIndex = 0;
                }

                DeletePerkCategoryCommand.NotifyCanExecuteChanged();
            }
        }

        private bool CanDeletePerkCategory() => UserPerkCategorySelection != "" && UserPerkCategorySelection != null && UserPerkCategoryIndex != -1;

        [RelayCommand(CanExecute = nameof(CanMovePerkCategoryUp))]
        private void MovePerkCategoryUp()
        {
            int index = UserPerkCategoryIndex;

            UserPerkCategories.SwapCollectionItems(index, index - 1);
            LoadedSave.UserPerkCategoryList.SwapListItems(index, index - 1);
            UserPerkCategoryIndex = index - 1;

            UpdateCategories(perks: true);
            Messenger.Send(new CategoryChangedMessage(true));

            DeletePerkCategoryCommand.NotifyCanExecuteChanged();
            MovePerkCategoryUpCommand.NotifyCanExecuteChanged();
            MovePerkCategoryDownCommand.NotifyCanExecuteChanged();
        }

        private bool CanMovePerkCategoryUp() => UserPerkCategoryIndex > 0;

        [RelayCommand(CanExecute = nameof(CanMovePerkCategoryDown))]
        private void MovePerkCategoryDown()
        {
            int index = UserPerkCategoryIndex;

            UserPerkCategories.SwapCollectionItems(index, index + 1);
            LoadedSave.UserPerkCategoryList.SwapListItems(index, index + 1);
            UserPerkCategoryIndex = index + 1;

            UpdateCategories(perks: true);
            Messenger.Send(new CategoryChangedMessage(true));

            DeletePerkCategoryCommand.NotifyCanExecuteChanged();
            MovePerkCategoryUpCommand.NotifyCanExecuteChanged();
            MovePerkCategoryDownCommand.NotifyCanExecuteChanged();
        }

        private bool CanMovePerkCategoryDown() => UserPerkCategoryIndex < UserPerkCategories.Count - 1;

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
                LoadedSave.UserItemCategoryList.Remove(category);

                UpdateCategories(perks: true);

                if (UserItemCategories.Any())
                {
                    UserItemCategoryIndex = 0;
                }

                DeleteItemCategoryCommand.NotifyCanExecuteChanged();
            }
        }

        private bool CanDeleteItemCategory() => UserItemCategorySelection != "" && UserItemCategorySelection != null && UserItemCategoryIndex != -1;

        [RelayCommand(CanExecute = nameof(CanMoveItemCategoryUp))]
        private void MoveItemCategoryUp()
        {
            int index = UserItemCategoryIndex;

            UserItemCategories.SwapCollectionItems(index, index - 1);
            LoadedSave.UserItemCategoryList.SwapListItems(index, index - 1);
            UserItemCategoryIndex = index - 1;

            UpdateCategories(perks: false);
            Messenger.Send(new CategoryChangedMessage(true));

            DeleteItemCategoryCommand.NotifyCanExecuteChanged();
            MoveItemCategoryUpCommand.NotifyCanExecuteChanged();
            MoveItemCategoryDownCommand.NotifyCanExecuteChanged();
        }

        private bool CanMoveItemCategoryUp() => UserItemCategoryIndex > 0;

        [RelayCommand(CanExecute = nameof(CanMoveItemCategoryDown))]
        private void MoveItemCategoryDown()
        {
            int index = UserItemCategoryIndex;

            UserItemCategories.SwapCollectionItems(index, index + 1);
            LoadedSave.UserItemCategoryList.SwapListItems(index, index + 1);
            UserItemCategoryIndex = index + 1;

            UpdateCategories(perks: false);
            Messenger.Send(new CategoryChangedMessage(true));

            DeleteItemCategoryCommand.NotifyCanExecuteChanged();
            MoveItemCategoryUpCommand.NotifyCanExecuteChanged();
            MoveItemCategoryDownCommand.NotifyCanExecuteChanged();
        }

        private bool CanMoveItemCategoryDown() => UserItemCategoryIndex < UserItemCategories.Count - 1;
        #endregion
    }
}
