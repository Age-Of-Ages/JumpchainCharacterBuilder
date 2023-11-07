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
    public partial class CosmicPassportViewModel : ViewModelBase
    {
        // TODO - Improve the attribute type/category system, as the current implementation is definitely kind of rough.
        // TODO - Implement Booster reordering.
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
        private ObservableCollection<Character> _characterList = new();
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteCharacterCommand))]
        private Character _characterSelection = new();
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _characterName = "";
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _characterAlias = "";
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _characterGender = "";
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _characterHomeworld = "";
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _characterSpecies = "";
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _characterRace = "";
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _characterPersonality = "";
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _characterPhysicalDescription = "";
        [ObservableProperty]
        private int _characterSelectionIndex = 0;

        [ObservableProperty]
        private ObservableCollection<Trait> _characterTraitList = new();
        [ObservableProperty]
        private Trait _characterTraitSelection = new();
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _like = "";
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _dislike = "";
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _hobby = "";
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _quirk = "";
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _goal = "";

        [ObservableProperty]
        private int _passportTabIndex = 0;
        [ObservableProperty]
        private int _gBMTabIndex = 0;
        [ObservableProperty]
        private int _sBBodyModTabIndex = 0;
        [ObservableProperty]
        private int _eBMTabIndex = 0;

        [ObservableProperty]
        private bool _usingFeetInches = true;
        [ObservableProperty]
        private bool _usingFeet = false;
        [ObservableProperty]
        private bool _usingMeters = false;

        [ObservableProperty]
        private bool _usingPounds = true;
        [ObservableProperty]
        private bool _usingKilograms = false;

        [ObservableProperty]
        private ObservableCollection<AltForm> _characterAltFormList = new();
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(NewStrengthWeaknessRowCommand))]
        [NotifyCanExecuteChangedFor(nameof(DeleteStrengthWeaknessRowCommand))]
        private AltForm _characterAltFormSelection = new();
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _altFormName = "";
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _altFormSpecies = "";
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _altFormDescription = "";
        [ObservableProperty]
        private int _characterAltFormSelectionIndex = 0;

        [ObservableProperty]
        private ObservableCollection<AltFormTraitModel> _altFormStrengthWeaknessList = new();
        [ObservableProperty]
        private AltFormTraitModel _altFormStrengthWeaknessSelection = new();
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _altFormStrength = "";
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _altFormWeakness = "";

        [ObservableProperty]
        private ObservableCollection<string> _perkTabList = new();
        [ObservableProperty]
        private string _perkTabName = "";
        [ObservableProperty]
        private int _perkTabIndex = 0;

        [ObservableProperty]
        private Dictionary<Character, Dictionary<string, List<Purchase>>> _inactivePerkLists = new();
        [ObservableProperty]
        private Dictionary<string, List<Purchase>> _activePerkLists = new();
        [ObservableProperty]
        private ObservableCollection<Purchase> _currentPerkList = new();
        [ObservableProperty]
        private Purchase _perkSelection = new();
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _perkName = "";
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _perkDescription = "";

        [ObservableProperty]
        private Options.BodyModSupplements _loadedBodyModSupplement = Options.BodyModSupplements.Generic;

        [ObservableProperty]
        private bool _genericBodyModSelected = true;
        [ObservableProperty]
        private bool _sBBodyModSelected = false;
        [ObservableProperty]
        private bool _essentialBodyModSelected = false;

        [ObservableProperty]
        private bool _bodyModPurchasesAllowed = true;


        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _genericBodyModName = "";
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _genericBodyModFullDescription = "";

        [ObservableProperty]
        private int _totalBP = 0;
        [ObservableProperty]
        private int _cPInvested = 0;
        [ObservableProperty]
        private int _jumpBPGained = 0;
        [ObservableProperty]
        private int _budget = 0;
        [ObservableProperty]
        private int _bPBought = 0;
        [ObservableProperty]
        private int _drawbackBP = 0;
        [ObservableProperty]
        private int _questBP = 0;
        [ObservableProperty]
        private int _gauntletBP = 0;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteBodyModPurchaseCommand))]
        [NotifyCanExecuteChangedFor(nameof(MoveBodyModPurchaseUpCommand))]
        [NotifyCanExecuteChangedFor(nameof(MoveBodyModPurchaseDownCommand))]
        [NotifyCanExecuteChangedFor(nameof(DeleteSBPowerTraitCommand))]
        private SupplementPurchase _bodyModPurchaseSelection = new();
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _bodyModPurchaseName = "";
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _bodyModPurchaseDescription = "";
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteBodyModDrawbackCommand))]
        [NotifyCanExecuteChangedFor(nameof(MoveBodyModDrawbackUpCommand))]
        [NotifyCanExecuteChangedFor(nameof(MoveBodyModDrawbackDownCommand))]
        private SupplementDrawbackModel _bodyModDrawbackSelection = new();
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _bodyModDrawbackName = "";
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _bodyModDrawbackDescription = "";

        [ObservableProperty]
        private ObservableCollection<Purchase> _bodyModAdditionList = new();
        [ObservableProperty]
        private Purchase _bodyModAdditionSelection = new();
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _bodyModAdditionName = "";
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _bodyModAdditionDescription = "";
        [ObservableProperty]
        private int _bodyModAdditionSelectionIndex = new();

        [ObservableProperty]
        private ObservableCollection<SupplementPurchase> _genericBodyModPurchaseList = new();
        [ObservableProperty]
        private int _genericBodyModPurchaseSelectionIndex = 0;

        [ObservableProperty]
        private ObservableCollection<SupplementDrawbackModel> _genericBodyModDrawbackList = new();
        [ObservableProperty]
        private int _genericBodyModDrawbackIndex = 0;
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Range(0, int.MaxValue, ErrorMessage = "Supplements cannot be taken on a negative Jump #.")]
        private int _gBMSupplementDelay = 0;

        [ObservableProperty]
        private Dictionary<string, int> _sBBodyModAnomalous = new()
        {
            {"No Anomalous", 0 },
            {"Level 1", 100 },
            {"Level 2", 200 },
            {"Level 3", 300 }
        };
        [ObservableProperty]
        private int _sBBodyModAnomalousStipend = 0;
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _sBBodyModAnomalousDescription = "";
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Range(0, int.MaxValue, ErrorMessage = "Supplements cannot be taken on a negative Jump #.")]
        private int _sBBodyModSupplementDelay = 0;
        [ObservableProperty]
        private int _sBBodyModGauntlets = 0;
        [ObservableProperty]
        private SupplementPurchase _characterBaseFormDetailSelection = new();
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _characterBaseFormDetailName = "";
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _characterBaseFormDetailDescription = "";
        [ObservableProperty]
        private ObservableCollection<SupplementPurchase> _sBExtraBitsList = new();
        [ObservableProperty]
        private SupplementPurchase _sBExtraBitsSelection = new();
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteSBExtraBitsCommand))]
        private int _sBExtraBitsIndex = 0;
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _sBExtraBitsName = "";
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _sBExtraBitsDescription = "";

        [ObservableProperty]
        private Dictionary<string, int> _sBBodyModAffinity = new()
        {
            {"No Affinity", 0 },
            {"Body", 1 },
            {"Mind", 2 },
            {"Heart", 3 }
        };
        [ObservableProperty]
        private int _sBBodyModAffinityIndex = 0;
        [ObservableProperty]
        private int _sBBodyModAffinityCost = 0;
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _sBBodyModAffinityDescription = "";

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteAugmentTraitCommand))]
        private AugmentPurchase _sBAugmentSelection = new();
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _sBAugmentName = "";
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _sBAugmentDescription = "";

        [ObservableProperty]
        private ObservableCollection<SupplementPurchase> _sBBodyModPowerList = new();
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteSBPowerTraitCommand))]
        private SupplementPurchase _sBBodyModPowerSelection = new();
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _sBBodyModPowerName = "";
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _sBBodyModPowerDescription = "";
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteSBPowerCommand))]
        private int _sBBodyModPowerIndex = 0;
        [ObservableProperty]
        private List<string> _sBBodyModPowerCategories = new()
        {
            "Shapeshifting",
            "Variability",
            "Permanence",
            "Adaptability",
            "Quality of Life",
            "Gag Powers and Incentives"
        };


        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Range(0, int.MaxValue, ErrorMessage = "Supplements cannot be taken on a negative Jump #.")]
        private int _eBMSupplementDelay = 0;
        [ObservableProperty]
        private int _eBMMinorQuests = 0;
        [ObservableProperty]
        private int _eBMMajorQuests = 0;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteEBMPerkCommand))]
        [NotifyCanExecuteChangedFor(nameof(DeleteEBMDrawbackCommand))]
        private int _eBMPurchaseTabIndex = 0;

        [ObservableProperty]
        private bool _eBMEssenceAllowed = true;
        [ObservableProperty]
        private bool _eBMSingleEssence = true;
        [ObservableProperty]
        private bool _eBMDualEssence = false;
        [ObservableProperty]
        private bool _eBMMultiEssence = false;

        [ObservableProperty]
        private ObservableCollection<EBMEssence> _eBMEssenceList = new();
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteEssenceCommand))]
        private EBMEssence _eBMEssenceSelection = new();
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _eBMEssenceName = "";
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _eBMEssenceDescription = "";
        [ObservableProperty]
        private int _eBMEssenceIndex = 0;

        [ObservableProperty]
        private ObservableCollection<SupplementPurchase> _eBMBasicPerkList = new();
        [ObservableProperty]
        private SupplementPurchase _eBMBasicPerkSelection = new();
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _eBMBasicPerkName = "";
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _eBMBasicPerkDescription = "";
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteEBMPerkCommand))]
        private int _eBMBasicPerkIndex = 0;
        [ObservableProperty]
        private ObservableCollection<SupplementPurchase> _eBMPhysicalPerkList = new();
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteEBMTraitCommand))]
        private SupplementPurchase _eBMPhysicalPerkSelection = new();
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _eBMPhysicalPerkName = "";
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _eBMPhysicalPerkDescription = "";
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteEBMPerkCommand))]
        private int _eBMPhysicalPerkIndex = 0;
        [ObservableProperty]
        private ObservableCollection<SupplementPurchase> _eBMMentalPerkList = new();
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteEBMTraitCommand))]
        private SupplementPurchase _eBMMentalPerkSelection = new();
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _eBMMentalPerkName = "";
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _eBMMentalPerkDescription = "";
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteEBMPerkCommand))]
        private int _eBMMentalPerkIndex = 0;
        [ObservableProperty]
        private ObservableCollection<SupplementPurchase> _eBMSpiritualPerkList = new();
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteEBMTraitCommand))]
        private SupplementPurchase _eBMSpiritualPerkSelection = new();
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _eBMSpiritualPerkName = "";
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _eBMSpiritualPerkDescription = "";
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteEBMPerkCommand))]
        private int _eBMSpiritualPerkIndex = 0;
        [ObservableProperty]
        private ObservableCollection<SupplementPurchase> _eBMSkillPerkList = new();
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteEBMTraitCommand))]
        private SupplementPurchase _eBMSkillPerkSelection = new();
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _eBMSkillPerkName = "";
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _eBMSkillPerkDescription = "";
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteEBMPerkCommand))]
        private int _eBMSkillPerkIndex = 0;
        [ObservableProperty]
        private ObservableCollection<SupplementPurchase> _eBMSupernaturalPerkList = new();
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteEBMTraitCommand))]
        private SupplementPurchase _eBMSupernaturalPerkSelection = new();
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _eBMSupernaturalPerkName = "";
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _eBMSupernaturalPerkDescription = "";
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteEBMPerkCommand))]
        private int _eBMSupernaturalPerkIndex = 0;
        [ObservableProperty]
        private ObservableCollection<SupplementPurchase> _eBMItemPerkList = new();
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteEBMTraitCommand))]
        private SupplementPurchase _eBMItemPerkSelection = new();
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _eBMItemPerkName = "";
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _eBMItemPerkDescription = "";
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteEBMPerkCommand))]
        private int _eBMItemPerkIndex = 0;
        [ObservableProperty]
        private ObservableCollection<SupplementPurchase> _eBMCompanionPerkList = new();
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteEBMTraitCommand))]
        private SupplementPurchase _eBMCompanionPerkSelection = new();
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _eBMCompanionPerkName = "";
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _eBMCompanionPerkDescription = "";
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteEBMPerkCommand))]
        private int _eBMCompanionPerkIndex = 0;

        [ObservableProperty]
        private ObservableCollection<SupplementDrawbackModel> _eBMDrawbackList = new();
        [ObservableProperty]
        private SupplementDrawbackModel _eBMDrawbackSelection = new();
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _eBMDrawbackName = "";
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _eBMDrawbackDescription = "";
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteEBMDrawbackCommand))]
        private int _eBMDrawbackIndex = 0;

        [ObservableProperty]
        private ObservableCollection<PurchaseAttribute> _purchaseAttributeList = new();
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteAugmentTraitCommand))]
        private PurchaseAttribute _purchaseAttributeSelection = new();
        [ObservableProperty]
        private int _purchaseAttributeIndex = 0;
        [ObservableProperty]
        private ObservableCollection<string> _availableAttributeTypeList = new();
        [ObservableProperty]
        private ObservableCollection<string> _availableAttributeCategoryList = new();
        [ObservableProperty]
        private string _attributeTypeSelection = "";
        [ObservableProperty]
        private string _attributeCategorySelection = "";

        [ObservableProperty]
        private ObservableCollection<string> _currentAttributeList = new();
        [ObservableProperty]
        private string _currentAttributeSelection = "";

        [ObservableProperty]
        private bool _attributeBasicSelected = true;
        [ObservableProperty]
        private bool _attributeBoosterSelected = false;
        [ObservableProperty]
        private bool _attributeSpecialSelected = false;

        [ObservableProperty]
        private Dictionary<string, AttributeCalculationClass.RankList> _attributeRankDictionary = new()
        {
            { "N/A", AttributeCalculationClass.RankList.None },
            { "F", AttributeCalculationClass.RankList.F },
            { "F+", AttributeCalculationClass.RankList.FPlus },
            { "F++", AttributeCalculationClass.RankList.FPlusPlus },
            { "F+++", AttributeCalculationClass.RankList.FPlusPlusPlus },
            { "E", AttributeCalculationClass.RankList.E },
            { "E+", AttributeCalculationClass.RankList.EPlus },
            { "E++", AttributeCalculationClass.RankList.EPlusPlus },
            { "E+++", AttributeCalculationClass.RankList.EPlusPlusPlus },
            { "D", AttributeCalculationClass.RankList.D },
            { "D+", AttributeCalculationClass.RankList.DPlus },
            { "D++", AttributeCalculationClass.RankList.DPlusPlus },
            { "D+++", AttributeCalculationClass.RankList.DPlusPlusPlus },
            { "C", AttributeCalculationClass.RankList.C },
            { "C+", AttributeCalculationClass.RankList.CPlus },
            { "C++", AttributeCalculationClass.RankList.CPlusPlus },
            { "C+++", AttributeCalculationClass.RankList.CPlusPlusPlus },
            { "B", AttributeCalculationClass.RankList.B },
            { "B+", AttributeCalculationClass.RankList.BPlus },
            { "B++", AttributeCalculationClass.RankList.BPlusPlus },
            { "B+++", AttributeCalculationClass.RankList.BPlusPlusPlus },
            { "A", AttributeCalculationClass.RankList.A },
            { "A+", AttributeCalculationClass.RankList.APlus },
            { "A++", AttributeCalculationClass.RankList.APlusPlus },
            { "A+++", AttributeCalculationClass.RankList.APlusPlusPlus },
            { "EX-F", AttributeCalculationClass.RankList.EX_F },
            { "EX-F+", AttributeCalculationClass.RankList.EX_FPlus },
            { "EX-F++", AttributeCalculationClass.RankList.EX_FPlusPlus },
            { "EX-F+++", AttributeCalculationClass.RankList.EX_FPlusPlusPlus },
            { "EX-E", AttributeCalculationClass.RankList.EX_E },
            { "EX-E+", AttributeCalculationClass.RankList.EX_EPlus },
            { "EX-E++", AttributeCalculationClass.RankList.EX_EPlusPlus },
            { "EX-E+++", AttributeCalculationClass.RankList.EX_EPlusPlusPlus },
            { "EX-D", AttributeCalculationClass.RankList.EX_D },
            { "EX-D+", AttributeCalculationClass.RankList.EX_DPlus },
            { "EX-D++", AttributeCalculationClass.RankList.EX_DPlusPlus },
            { "EX-D+++", AttributeCalculationClass.RankList.EX_DPlusPlusPlus },
            { "EX-C", AttributeCalculationClass.RankList.EX_C },
            { "EX-C+", AttributeCalculationClass.RankList.EX_CPlus },
            { "EX-C++", AttributeCalculationClass.RankList.EX_CPlusPlus },
            { "EX-C+++", AttributeCalculationClass.RankList.EX_CPlusPlusPlus },
            { "EX-B", AttributeCalculationClass.RankList.EX_B },
            { "EX-B+", AttributeCalculationClass.RankList.EX_BPlus },
            { "EX-B++", AttributeCalculationClass.RankList.EX_BPlusPlus },
            { "EX-B+++", AttributeCalculationClass.RankList.EX_BPlusPlusPlus },
            { "EX-A", AttributeCalculationClass.RankList.EX_A },
            { "EX-A+", AttributeCalculationClass.RankList.EX_APlus },
            { "EX-A++", AttributeCalculationClass.RankList.EX_APlusPlus },
            { "EX-A+++", AttributeCalculationClass.RankList.EX_APlusPlusPlus },
            { "S-F", AttributeCalculationClass.RankList.S_F },
            { "S-F+", AttributeCalculationClass.RankList.S_FPlus },
            { "S-F++", AttributeCalculationClass.RankList.S_FPlusPlus },
            { "S-F+++", AttributeCalculationClass.RankList.S_FPlusPlusPlus },
            { "S-E", AttributeCalculationClass.RankList.S_E },
            { "S-E+", AttributeCalculationClass.RankList.S_EPlus },
            { "S-E++", AttributeCalculationClass.RankList.S_EPlusPlus },
            { "S-E+++", AttributeCalculationClass.RankList.S_EPlusPlusPlus },
            { "S-D", AttributeCalculationClass.RankList.S_D },
            { "S-D+", AttributeCalculationClass.RankList.S_DPlus },
            { "S-D++", AttributeCalculationClass.RankList.S_DPlusPlus },
            { "S-D+++", AttributeCalculationClass.RankList.S_DPlusPlusPlus },
            { "S-C", AttributeCalculationClass.RankList.S_C },
            { "S-C+", AttributeCalculationClass.RankList.S_CPlus },
            { "S-C++", AttributeCalculationClass.RankList.S_CPlusPlus },
            { "S-C+++", AttributeCalculationClass.RankList.S_CPlusPlusPlus },
            { "S-B", AttributeCalculationClass.RankList.S_B },
            { "S-B+", AttributeCalculationClass.RankList.S_BPlus },
            { "S-B++", AttributeCalculationClass.RankList.S_BPlusPlus },
            { "S-B+++", AttributeCalculationClass.RankList.S_BPlusPlusPlus },
            { "S-A", AttributeCalculationClass.RankList.S_A },
            { "S-A+", AttributeCalculationClass.RankList.S_APlus },
            { "S-A++", AttributeCalculationClass.RankList.S_APlusPlus },
            { "S-A+++", AttributeCalculationClass.RankList.S_APlusPlusPlus },
            { "Z-F", AttributeCalculationClass.RankList.Z_F },
            { "Z-F+", AttributeCalculationClass.RankList.Z_FPlus },
            { "Z-F++", AttributeCalculationClass.RankList.Z_FPlusPlus },
            { "Z-F+++", AttributeCalculationClass.RankList.Z_FPlusPlusPlus },
            { "Z-E", AttributeCalculationClass.RankList.Z_E },
            { "Z-E+", AttributeCalculationClass.RankList.Z_EPlus },
            { "Z-E++", AttributeCalculationClass.RankList.Z_EPlusPlus },
            { "Z-E+++", AttributeCalculationClass.RankList.Z_EPlusPlusPlus },
            { "Z-D", AttributeCalculationClass.RankList.Z_D },
            { "Z-D+", AttributeCalculationClass.RankList.Z_DPlus },
            { "Z-D++", AttributeCalculationClass.RankList.Z_DPlusPlus },
            { "Z-D+++", AttributeCalculationClass.RankList.Z_DPlusPlusPlus },
            { "Z-C", AttributeCalculationClass.RankList.Z_C },
            { "Z-C+", AttributeCalculationClass.RankList.Z_CPlus },
            { "Z-C++", AttributeCalculationClass.RankList.Z_CPlusPlus },
            { "Z-C+++", AttributeCalculationClass.RankList.Z_CPlusPlusPlus },
            { "Z-B", AttributeCalculationClass.RankList.Z_B },
            { "Z-B+", AttributeCalculationClass.RankList.Z_BPlus },
            { "Z-B++", AttributeCalculationClass.RankList.Z_BPlusPlus },
            { "Z-B+++", AttributeCalculationClass.RankList.Z_BPlusPlusPlus },
            { "Z-A", AttributeCalculationClass.RankList.Z_A },
            { "Z-A+", AttributeCalculationClass.RankList.Z_APlus },
            { "Z-A++", AttributeCalculationClass.RankList.Z_APlusPlus },
            { "Z-A+++", AttributeCalculationClass.RankList.Z_APlusPlusPlus },
        };

        [ObservableProperty]
        private ObservableCollection<ProfileAttribute> _currentPhysicalAttributeList = new();
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeletePhysicalAttributeCommand))]
        [NotifyCanExecuteChangedFor(nameof(MovePhysicalAttributeUpCommand))]
        [NotifyCanExecuteChangedFor(nameof(MovePhysicalAttributeDownCommand))]
        private ProfileAttribute _currentPhysicalAttributeSelection = new();
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _currentPhysicalAttributeName = "";
        [ObservableProperty]
        private int _currentPhysicalAttributeIndex = 0;
        [ObservableProperty]
        private ObservableCollection<ProfileAttribute> _currentMentalAttributeList = new();
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteMentalAttributeCommand))]
        [NotifyCanExecuteChangedFor(nameof(MoveMentalAttributeUpCommand))]
        [NotifyCanExecuteChangedFor(nameof(MoveMentalAttributeDownCommand))]
        private ProfileAttribute _currentMentalAttributeSelection = new();
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _currentMentalAttributeName = "";
        [ObservableProperty]
        private int _currentMentalAttributeIndex = 0;
        [ObservableProperty]
        private ObservableCollection<ProfileAttribute> _currentSupernaturalAttributeList = new();
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteSupernaturalAttributeCommand))]
        [NotifyCanExecuteChangedFor(nameof(MoveSupernaturalAttributeUpCommand))]
        [NotifyCanExecuteChangedFor(nameof(MoveSupernaturalAttributeDownCommand))]
        private ProfileAttribute _currentSupernaturalAttributeSelection = new();
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _currentSupernaturalAttributeName = "";
        [ObservableProperty]
        private int _currentSupernaturalAttributeIndex = 0;

        [ObservableProperty]
        private ObservableCollection<ProfileAttribute> _currentPhysicalSkillList = new();
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeletePhysicalSkillCommand))]
        [NotifyCanExecuteChangedFor(nameof(MovePhysicalSkillUpCommand))]
        [NotifyCanExecuteChangedFor(nameof(MovePhysicalSkillDownCommand))]
        private ProfileAttribute _currentPhysicalSkillSelection = new();
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _currentPhysicalSkillName = "";
        [ObservableProperty]
        private int _currentPhysicalSkillIndex = 0;
        [ObservableProperty]
        private ObservableCollection<ProfileAttribute> _currentMentalSkillList = new();
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteMentalSkillCommand))]
        [NotifyCanExecuteChangedFor(nameof(MoveMentalSkillUpCommand))]
        [NotifyCanExecuteChangedFor(nameof(MoveMentalSkillDownCommand))]
        private ProfileAttribute _currentMentalSkillSelection = new();
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _currentMentalSkillName = "";
        [ObservableProperty]
        private int _currentMentalSkillIndex = 0;
        [ObservableProperty]
        private ObservableCollection<ProfileAttribute> _currentSocialSkillList = new();
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteSocialSkillCommand))]
        [NotifyCanExecuteChangedFor(nameof(MoveSocialSkillUpCommand))]
        [NotifyCanExecuteChangedFor(nameof(MoveSocialSkillDownCommand))]
        private ProfileAttribute _currentSocialSkillSelection = new();
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _currentSocialSkillName = "";
        [ObservableProperty]
        private int _currentSocialSkillIndex = 0;
        [ObservableProperty]
        private ObservableCollection<ProfileAttribute> _currentTechnologicalSkillList = new();
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteTechnologicalSkillCommand))]
        [NotifyCanExecuteChangedFor(nameof(MoveTechnologicalSkillUpCommand))]
        [NotifyCanExecuteChangedFor(nameof(MoveTechnologicalSkillDownCommand))]
        private ProfileAttribute _currentTechnologicalSkillSelection = new();
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _currentTechnologicalSkillName = "";
        [ObservableProperty]
        private int _currentTechnologicalSkillIndex = 0;
        [ObservableProperty]
        private ObservableCollection<ProfileAttribute> _currentSupernaturalSkillList = new();
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteSupernaturalSkillCommand))]
        [NotifyCanExecuteChangedFor(nameof(MoveSupernaturalSkillUpCommand))]
        [NotifyCanExecuteChangedFor(nameof(MoveSupernaturalSkillDownCommand))]
        private ProfileAttribute _currentSupernaturalSkillSelection = new();
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _currentSupernaturalSkillName = "";
        [ObservableProperty]
        private int _currentSupernaturalSkillIndex = 0;

        [ObservableProperty]
        private ObservableCollection<Booster> _currentBoosterList = new();
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteBoosterCommand))]
        private Booster _currentBoosterSelection = new();
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _currentBoosterName = "";
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _currentBoosterDescription = "";
        [ObservableProperty]
        private int _currentBoosterIndex = 0;
        #endregion

        #region Properties

        partial void OnCharacterSelectionChanged(Character value)
        {
            if (CharacterSelection != null)
            {
                LoadCharacterTraits(value);
                CalculateTrueAge(value);
                LoadAltForms(value);

                CharacterChanged(CharacterSelectionIndex);

                CharacterName = value.Name;
                CharacterAlias = value.Alias;
                CharacterGender = value.Gender;
                CharacterSpecies = value.Species;
                CharacterHomeworld = value.Homeworld;
                CharacterRace = value.Race;
                CharacterPersonality = value.Personality;
                CharacterPhysicalDescription = value.PhysicalDescription;
            }
        }

        partial void OnCharacterTraitSelectionChanged(Trait value)
        {
            if (value != null)
            {
                Like = value.Like;
                Dislike = value.Dislike;
                Hobby = value.Hobby;
                Quirk = value.Quirk;
                Goal = value.Goal;
            }
        }

        partial void OnCharacterAltFormSelectionChanged(AltForm value) => LoadAltFormTraits();

        partial void OnAltFormStrengthWeaknessSelectionChanged(AltFormTraitModel value)
        {
            if (value != null)
            {
                AltFormStrength = value.Strength;
                AltFormWeakness = value.Weakness;
            }
        }

        partial void OnPerkTabNameChanged(string value) => PerkTabChanged();

        partial void OnPerkSelectionChanged(Purchase value)
        {
            if (value != null)
            {
                PerkName = value.Name;
                PerkDescription = value.Description;
            }
        }

        partial void OnLoadedBodyModSupplementChanged(Options.BodyModSupplements value)
        {
            LoadBodyModTabs();

            ClearAttributeList();

            if (value == Options.BodyModSupplements.Generic)
            {
                GenericBodyModName = LoadedSave.GenericBodyMod.Name;
                GenericBodyModFullDescription = LoadedSave.GenericBodyMod.FullDescription;
            }
        }

        partial void OnSBBodyModSupplementDelayChanged(int value)
        {
            if (!GetErrors(nameof(SBBodyModSupplementDelay)).Any())
            {
                CharacterSelection.BodyMod.SupplementDelay = value;
            }
        }

        partial void OnGBMSupplementDelayChanged(int value)
        {
            if (!GetErrors(nameof(GBMSupplementDelay)).Any())
            {
                CharacterSelection.BodyMod.SupplementDelay = value;
            }
        }

        partial void OnBodyModPurchaseSelectionChanged(SupplementPurchase value)
        {
            if (value != null)
            {
                BodyModPurchaseName = value.Name;
                BodyModPurchaseDescription = value.Description;

                if (value.Attributes.Any())
                {
                    PurchaseAttributeList.Clear();

                    foreach (PurchaseAttribute attribute in value.Attributes)
                    {
                        PurchaseAttributeList.Add(attribute);
                    }

                    PurchaseAttributeIndex = 0;
                }
                else
                {
                    ClearAttributeList();
                }
            }
            else
            {
                ClearAttributeList();
            }
        }

        partial void OnBodyModDrawbackSelectionChanged(SupplementDrawbackModel value)
        {
            if (value != null)
            {
                BodyModDrawbackName = value.Name;
                BodyModDrawbackDescription = value.Description;
            }
        }

        partial void OnBodyModAdditionSelectionChanged(Purchase value)
        {
            if (value != null)
            {
                BodyModAdditionName = value.Name;
                BodyModAdditionDescription = value.Description;
            }
        }

        partial void OnSBBodyModGauntletsChanged(int value) => CharacterSelection.BodyMod.GauntletsFinished = value;

        partial void OnSBBodyModAnomalousStipendChanged(int value)
        {
            switch (value)
            {
                case 0:
                    CharacterSelection.BodyMod.AnomalousLevel = 0;
                    break;
                case 100:
                    CharacterSelection.BodyMod.AnomalousLevel = 1;
                    break;
                case 200:
                    CharacterSelection.BodyMod.AnomalousLevel = 2;
                    break;
                case 300:
                    CharacterSelection.BodyMod.AnomalousLevel = 3;
                    break;
                default:
                    CharacterSelection.BodyMod.AnomalousLevel = 0;
                    break;
            }
        }

        partial void OnSBBodyModAnomalousDescriptionChanged(string value)
        {
            if (!GetErrors(nameof(SBBodyModAnomalousDescription)).Any())
            {
                CharacterSelection.BodyMod.AnomalousDescription = value;
            }
        }

        partial void OnSBBodyModAffinityIndexChanged(int value)
        {
            CharacterSelection.BodyMod.AffinityIndex = value;
            if (value == 0)
            {
                SBBodyModAffinityCost = 0;
            }
            else
            {
                SBBodyModAffinityCost = 100;
            }
        }

        partial void OnSBBodyModAffinityDescriptionChanged(string value)
        {
            if (!GetErrors(nameof(SBBodyModAffinityDescription)).Any())
            {
                CharacterSelection.BodyMod.AffinityDescription = value;
            }
        }

        partial void OnSBAugmentSelectionChanged(AugmentPurchase value)
        {
            if (value != null)
            {
                SBAugmentName = value.Name;
                SBAugmentDescription = value.Description;

                if (value.Attributes.Any())
                {
                    PurchaseAttributeList.Clear();

                    foreach (PurchaseAttribute attribute in value.Attributes)
                    {
                        PurchaseAttributeList.Add(attribute);
                    }

                    PurchaseAttributeIndex = 0;
                }
                else
                {
                    ClearAttributeList();
                }
            }
            else
            {
                ClearAttributeList();
            }
        }

        partial void OnSBBodyModPowerSelectionChanged(SupplementPurchase value)
        {

            if (value != null)
            {
                SBBodyModPowerName = value.Name;
                SBBodyModPowerDescription = value.Description;

                if (value.Attributes.Any())
                {
                    PurchaseAttributeList.Clear();

                    foreach (PurchaseAttribute attribute in value.Attributes)
                    {
                        PurchaseAttributeList.Add(attribute);
                    }

                    PurchaseAttributeIndex = 0;
                }
                else
                {
                    ClearAttributeList();
                }
            }
            else
            {
                ClearAttributeList();
            }
        }

        partial void OnEBMEssenceSelectionChanged(EBMEssence value)
        {
            if (value != null)
            {
                EBMEssenceName = value.Name;
                EBMEssenceDescription = value.Description;
            }
        }

        partial void OnEBMSupplementDelayChanged(int value)
        {
            if (!GetErrors(nameof(EBMSupplementDelay)).Any())
            {
                CharacterSelection.BodyMod.SupplementDelay = value;
            }
        }

        partial void OnEBMMinorQuestsChanged(int value) => CharacterSelection.BodyMod.EBMMinorQuests = value;

        partial void OnEBMMajorQuestsChanged(int value) => CharacterSelection.BodyMod.EBMMajorQuests = value;


        partial void OnEBMBasicPerkSelectionChanged(SupplementPurchase value)
        {
            if (value != null)
            {
                EBMBasicPerkName = value.Name;
                EBMBasicPerkDescription = value.Description;

                if (value.Attributes.Any())
                {
                    PurchaseAttributeList.Clear();

                    foreach (PurchaseAttribute attribute in value.Attributes)
                    {
                        PurchaseAttributeList.Add(attribute);
                    }

                    PurchaseAttributeIndex = 0;
                }
                else
                {
                    ClearAttributeList();
                }
            }
            else
            {
                ClearAttributeList();
            }
        }


        partial void OnEBMPhysicalPerkSelectionChanged(SupplementPurchase value)
        {
            if (value != null)
            {
                EBMPhysicalPerkName = value.Name;
                EBMPhysicalPerkDescription = value.Description;

                if (value.Attributes.Any())
                {
                    PurchaseAttributeList.Clear();

                    foreach (PurchaseAttribute attribute in value.Attributes)
                    {
                        PurchaseAttributeList.Add(attribute);
                    }

                    PurchaseAttributeIndex = 0;
                }
                else
                {
                    ClearAttributeList();
                }
            }
            else
            {
                ClearAttributeList();
            }
        }

        partial void OnEBMMentalPerkSelectionChanged(SupplementPurchase value)
        {
            if (value != null)
            {
                EBMMentalPerkName = value.Name;
                EBMMentalPerkDescription = value.Description;

                if (value.Attributes.Any())
                {
                    PurchaseAttributeList.Clear();

                    foreach (PurchaseAttribute attribute in value.Attributes)
                    {
                        PurchaseAttributeList.Add(attribute);
                    }

                    PurchaseAttributeIndex = 0;
                }
                else
                {
                    ClearAttributeList();
                }
            }
            else
            {
                ClearAttributeList();
            }
        }

        partial void OnEBMSpiritualPerkSelectionChanged(SupplementPurchase value)
        {
            if (value != null)
            {
                EBMSpiritualPerkName = value.Name;
                EBMSpiritualPerkDescription = value.Description;

                if (value.Attributes.Any())
                {
                    PurchaseAttributeList.Clear();

                    foreach (PurchaseAttribute attribute in value.Attributes)
                    {
                        PurchaseAttributeList.Add(attribute);
                    }

                    PurchaseAttributeIndex = 0;
                }
                else
                {
                    ClearAttributeList();
                }
            }
            else
            {
                ClearAttributeList();
            }
        }

        partial void OnEBMSkillPerkSelectionChanged(SupplementPurchase value)
        {
            if (value != null)
            {
                EBMSkillPerkName = value.Name;
                EBMSkillPerkDescription = value.Description;

                if (value.Attributes.Any())
                {
                    PurchaseAttributeList.Clear();

                    foreach (PurchaseAttribute attribute in value.Attributes)
                    {
                        PurchaseAttributeList.Add(attribute);
                    }

                    PurchaseAttributeIndex = 0;
                }
                else
                {
                    ClearAttributeList();
                }
            }
            else
            {
                ClearAttributeList();
            }
        }

        partial void OnEBMSupernaturalPerkSelectionChanged(SupplementPurchase value)
        {
            if (value != null)
            {
                EBMSupernaturalPerkName = value.Name;
                EBMSupernaturalPerkDescription = value.Description;

                if (value.Attributes.Any())
                {
                    PurchaseAttributeList.Clear();

                    foreach (PurchaseAttribute attribute in value.Attributes)
                    {
                        PurchaseAttributeList.Add(attribute);
                    }

                    PurchaseAttributeIndex = 0;
                }
                else
                {
                    ClearAttributeList();
                }
            }
            else
            {
                ClearAttributeList();
            }
        }

        partial void OnEBMItemPerkSelectionChanged(SupplementPurchase value)
        {
            if (value != null)
            {
                EBMItemPerkName = value.Name;
                EBMItemPerkDescription = value.Description;

                if (value.Attributes.Any())
                {
                    PurchaseAttributeList.Clear();

                    foreach (PurchaseAttribute attribute in value.Attributes)
                    {
                        PurchaseAttributeList.Add(attribute);
                    }

                    PurchaseAttributeIndex = 0;
                }
                else
                {
                    ClearAttributeList();
                }
            }
            else
            {
                ClearAttributeList();
            }
        }

        partial void OnEBMCompanionPerkSelectionChanged(SupplementPurchase value)
        {
            if (value != null)
            {
                EBMCompanionPerkName = value.Name;
                EBMCompanionPerkDescription = value.Description;

                if (value.Attributes.Any())
                {
                    PurchaseAttributeList.Clear();

                    foreach (PurchaseAttribute attribute in value.Attributes)
                    {
                        PurchaseAttributeList.Add(attribute);
                    }

                    PurchaseAttributeIndex = 0;
                }
                else
                {
                    ClearAttributeList();
                }
            }
            else
            {
                ClearAttributeList();
            }
        }

        partial void OnEBMPurchaseTabIndexChanged(int value) => ClearAttributeList();

        partial void OnEBMDrawbackSelectionChanged(SupplementDrawbackModel value)
        {
            if (value != null)
            {
                EBMDrawbackName = value.Name;
                EBMDrawbackDescription = value.Description;
            }
        }

        partial void OnPurchaseAttributeSelectionChanged(PurchaseAttribute value)
        {
            if (value != null)
            {
                ReloadAttributeCategories();

                AttributeTypeSelection = value.Type;
                AttributeCategorySelection = value.Category;
                CurrentAttributeSelection = value.Name;
            }
            else
            {
                AvailableAttributeCategoryList.Clear();
            }
        }

        partial void OnAttributeTypeSelectionChanged(string value)
        {
            if (PurchaseAttributeSelection != null && value != null)
            {
                if (PurchaseAttributeSelection.Type != value)
                {
                    PurchaseAttributeSelection.Type = value;
                }

                ReloadAttributeCategories();

                if (PurchaseAttributeSelection.Type != AttributeTypeSelection)
                {
                    CurrentAttributeSelection = "";
                }

                if (value == "Booster")
                {
                    AttributeBasicSelected = false;
                    AttributeBoosterSelected = true;
                    AttributeSpecialSelected = false;
                }
                else if (value == "Special")
                {
                    AttributeBasicSelected = false;
                    AttributeBoosterSelected = false;
                    AttributeSpecialSelected = true;
                }
                else
                {
                    AttributeBasicSelected = true;
                    AttributeBoosterSelected = false;
                    AttributeSpecialSelected = false;
                }
            }
        }

        partial void OnAttributeCategorySelectionChanged(string value)
        {
            if (PurchaseAttributeSelection != null && value != null)
            {
                PurchaseAttributeSelection.Category = value;

                LoadCurrentAttributeList();
            }
        }

        partial void OnCurrentAttributeSelectionChanged(string value)
        {
            if (PurchaseAttributeSelection != null && value != null)
            {
                PurchaseAttributeSelection.Name = value;
            }
        }

        partial void OnCharacterNameChanged(string value)
        {
            if (!GetErrors(nameof(CharacterName)).Any() && CharacterSelection != null)
            {
                CharacterSelection.Name = value;
            }
        }

        partial void OnCharacterAliasChanged(string value)
        {
            if (!GetErrors(nameof(CharacterAlias)).Any() && CharacterSelection != null)
            {
                CharacterSelection.Alias = value;
            }
        }

        partial void OnCharacterGenderChanged(string value)
        {
            if (!GetErrors(nameof(CharacterGender)).Any() && CharacterSelection != null)
            {
                CharacterSelection.Gender = value;
            }
        }

        partial void OnCharacterSpeciesChanged(string value)
        {
            if (!GetErrors(nameof(CharacterSpecies)).Any() && CharacterSelection != null)
            {
                CharacterSelection.Species = value;
            }
        }

        partial void OnCharacterHomeworldChanged(string value)
        {
            if (!GetErrors(nameof(CharacterHomeworld)).Any() && CharacterSelection != null)
            {
                CharacterSelection.Homeworld = value;
            }
        }

        partial void OnCharacterRaceChanged(string value)
        {
            if (!GetErrors(nameof(CharacterRace)).Any() && CharacterSelection != null)
            {
                CharacterSelection.Race = value;
            }
        }

        partial void OnCharacterPersonalityChanged(string value)
        {
            if (!GetErrors(nameof(CharacterPersonality)).Any() && CharacterSelection != null)
            {
                CharacterSelection.Personality = value;
            }
        }

        partial void OnCharacterPhysicalDescriptionChanged(string value)
        {
            if (!GetErrors(nameof(CharacterPhysicalDescription)).Any() && CharacterSelection != null)
            {
                CharacterSelection.PhysicalDescription = value;
            }
        }

        partial void OnLikeChanged(string value)
        {
            if (!GetErrors(nameof(Like)).Any() && CharacterTraitSelection != null)
            {
                CharacterTraitSelection.Like = value;
            }
        }

        partial void OnDislikeChanged(string value)
        {
            if (!GetErrors(nameof(Dislike)).Any() && CharacterTraitSelection != null)
            {
                CharacterTraitSelection.Dislike = value;
            }
        }

        partial void OnHobbyChanged(string value)
        {
            if (!GetErrors(nameof(Hobby)).Any() && CharacterTraitSelection != null)
            {
                CharacterTraitSelection.Hobby = value;
            }
        }

        partial void OnQuirkChanged(string value)
        {
            if (!GetErrors(nameof(Quirk)).Any() && CharacterTraitSelection != null)
            {
                CharacterTraitSelection.Quirk = value;
            }
        }

        partial void OnGoalChanged(string value)
        {
            if (!GetErrors(nameof(Goal)).Any() && CharacterTraitSelection != null)
            {
                CharacterTraitSelection.Goal = value;
            }
        }

        partial void OnAltFormNameChanged(string value)
        {
            if (!GetErrors(nameof(AltFormName)).Any() && CharacterAltFormSelection != null)
            {
                CharacterAltFormSelection.AltFormName = value;
            }
        }

        partial void OnAltFormSpeciesChanged(string value)
        {
            if (!GetErrors(nameof(AltFormSpecies)).Any() && CharacterAltFormSelection != null)
            {
                CharacterAltFormSelection.AltFormSpecies = value;
            }
        }

        partial void OnAltFormDescriptionChanged(string value)
        {
            if (!GetErrors(nameof(AltFormDescription)).Any() && CharacterAltFormSelection != null)
            {
                CharacterAltFormSelection.AltFormDescription = value;
            }
        }

        partial void OnAltFormStrengthChanged(string value)
        {
            if (!GetErrors(nameof(AltFormStrength)).Any() && AltFormStrengthWeaknessSelection != null)
            {
                AltFormStrengthWeaknessSelection.Strength = value;
            }
        }

        partial void OnAltFormWeaknessChanged(string value)
        {
            if (!GetErrors(nameof(AltFormWeakness)).Any() && AltFormStrengthWeaknessSelection != null)
            {
                AltFormStrengthWeaknessSelection.Weakness = value;
            }
        }

        partial void OnPerkNameChanged(string value)
        {
            if (!GetErrors(nameof(PerkName)).Any() && PerkSelection != null)
            {
                PerkSelection.Name = value;
            }
        }

        partial void OnPerkDescriptionChanged(string value)
        {
            if (!GetErrors(nameof(PerkDescription)).Any() && PerkSelection != null)
            {
                PerkSelection.Description = value;
            }
        }

        partial void OnGenericBodyModNameChanged(string value)
        {
            if (!GetErrors(nameof(GenericBodyModName)).Any())
            {
                LoadedSave.GenericBodyMod.Name = value;
            }
        }

        partial void OnGenericBodyModFullDescriptionChanged(string value)
        {
            if (!GetErrors(nameof(GenericBodyModFullDescription)).Any())
            {
                LoadedSave.GenericBodyMod.FullDescription = value;
            }
        }

        partial void OnBodyModPurchaseNameChanged(string value)
        {
            if (!GetErrors(nameof(BodyModPurchaseName)).Any() && BodyModPurchaseSelection != null)
            {
                BodyModPurchaseSelection.Name = value;
            }
        }

        partial void OnBodyModPurchaseDescriptionChanged(string value)
        {
            if (!GetErrors(nameof(BodyModPurchaseDescription)).Any() && BodyModPurchaseSelection != null)
            {
                BodyModPurchaseSelection.Description = value;
            }
        }

        partial void OnBodyModDrawbackNameChanged(string value)
        {
            if (!GetErrors(nameof(BodyModDrawbackName)).Any() && BodyModDrawbackSelection != null)
            {
                BodyModDrawbackSelection.Name = value;
            }
        }

        partial void OnBodyModDrawbackDescriptionChanged(string value)
        {
            if (!GetErrors(nameof(BodyModDrawbackDescription)).Any() && BodyModDrawbackSelection != null)
            {
                BodyModDrawbackSelection.Description = value;
            }
        }

        partial void OnBodyModAdditionNameChanged(string value)
        {
            if (!GetErrors(nameof(BodyModAdditionName)).Any() && BodyModAdditionSelection != null)
            {
                BodyModAdditionSelection.Name = value;
            }
        }

        partial void OnBodyModAdditionDescriptionChanged(string value)
        {
            if (!GetErrors(nameof(BodyModAdditionDescription)).Any() && BodyModAdditionSelection != null)
            {
                BodyModAdditionSelection.Description = value;
            }
        }

        partial void OnCharacterBaseFormDetailSelectionChanged(SupplementPurchase value)
        {
            if (value != null)
            {
                CharacterBaseFormDetailName = value.Name;
                CharacterBaseFormDetailDescription = value.Description;
            }
        }

        partial void OnCharacterBaseFormDetailNameChanged(string value)
        {
            if (!GetErrors(nameof(CharacterBaseFormDetailName)).Any() && CharacterBaseFormDetailSelection != null)
            {
                CharacterBaseFormDetailSelection.Name = value;
            }
        }

        partial void OnCharacterBaseFormDetailDescriptionChanged(string value)
        {
            if (!GetErrors(nameof(CharacterBaseFormDetailDescription)).Any() && CharacterBaseFormDetailSelection != null)
            {
                CharacterBaseFormDetailSelection.Description = value;
            }
        }

        partial void OnSBExtraBitsSelectionChanged(SupplementPurchase value)
        {
            if (value != null)
            {
                SBExtraBitsName = value.Name;
                SBExtraBitsDescription = value.Description;
            }
        }

        partial void OnSBExtraBitsNameChanged(string value)
        {
            if (!GetErrors(nameof(SBExtraBitsName)).Any() && SBExtraBitsSelection != null)
            {
                SBExtraBitsSelection.Name = value;
            }
        }

        partial void OnSBExtraBitsDescriptionChanged(string value)
        {
            if (!GetErrors(nameof(SBExtraBitsDescription)).Any() && SBExtraBitsSelection != null)
            {
                SBExtraBitsSelection.Description = value;
            }
        }

        partial void OnSBAugmentNameChanged(string value)
        {
            if (!GetErrors(nameof(SBAugmentName)).Any() && SBAugmentSelection != null)
            {
                SBAugmentSelection.Name = value;
            }
        }

        partial void OnSBAugmentDescriptionChanged(string value)
        {
            if (!GetErrors(nameof(SBAugmentDescription)).Any() && SBAugmentSelection != null)
            {
                SBAugmentSelection.Description = value;
            }
        }

        partial void OnSBBodyModPowerNameChanged(string value)
        {
            if (!GetErrors(nameof(SBBodyModPowerName)).Any() && SBBodyModPowerSelection != null)
            {
                SBBodyModPowerSelection.Name = value;
            }
        }

        partial void OnSBBodyModPowerDescriptionChanged(string value)
        {
            if (!GetErrors(nameof(SBBodyModPowerDescription)).Any() && SBBodyModPowerSelection != null)
            {
                SBBodyModPowerSelection.Description = value;
            }
        }

        partial void OnEBMEssenceNameChanged(string value)
        {
            if (!GetErrors(nameof(EBMEssenceName)).Any() && EBMEssenceSelection != null)
            {
                EBMEssenceSelection.Name = value;
            }
        }

        partial void OnEBMEssenceDescriptionChanged(string value)
        {
            if (!GetErrors(nameof(EBMEssenceDescription)).Any() && EBMEssenceSelection != null)
            {
                EBMEssenceSelection.Description = value;
            }
        }

        partial void OnEBMBasicPerkNameChanged(string value)
        {
            if (!GetErrors(nameof(EBMBasicPerkName)).Any() && EBMBasicPerkSelection != null)
            {
                EBMBasicPerkSelection.Name = value;
            }
        }

        partial void OnEBMBasicPerkDescriptionChanged(string value)
        {
            if (!GetErrors(nameof(EBMBasicPerkDescription)).Any() && EBMBasicPerkSelection != null)
            {
                EBMBasicPerkSelection.Description = value;
            }
        }

        partial void OnEBMPhysicalPerkNameChanged(string value)
        {
            if (!GetErrors(nameof(EBMPhysicalPerkName)).Any() && EBMPhysicalPerkSelection != null)
            {
                EBMPhysicalPerkSelection.Name = value;
            }
        }

        partial void OnEBMPhysicalPerkDescriptionChanged(string value)
        {
            if (!GetErrors(nameof(EBMPhysicalPerkDescription)).Any() && EBMPhysicalPerkSelection != null)
            {
                EBMPhysicalPerkSelection.Description = value;
            }
        }

        partial void OnEBMMentalPerkNameChanged(string value)
        {
            if (!GetErrors(nameof(EBMMentalPerkName)).Any() && EBMMentalPerkSelection != null)
            {
                EBMMentalPerkSelection.Name = value;
            }
        }

        partial void OnEBMMentalPerkDescriptionChanged(string value)
        {
            if (!GetErrors(nameof(EBMMentalPerkDescription)).Any() && EBMMentalPerkSelection != null)
            {
                EBMMentalPerkSelection.Description = value;
            }
        }

        partial void OnEBMSpiritualPerkNameChanged(string value)
        {
            if (!GetErrors(nameof(EBMSpiritualPerkName)).Any() && EBMSpiritualPerkSelection != null)
            {
                EBMSpiritualPerkSelection.Name = value;
            }
        }

        partial void OnEBMSpiritualPerkDescriptionChanged(string value)
        {
            if (!GetErrors(nameof(EBMSpiritualPerkDescription)).Any() && EBMSpiritualPerkSelection != null)
            {
                EBMSpiritualPerkSelection.Description = value;
            }
        }

        partial void OnEBMSkillPerkNameChanged(string value)
        {
            if (!GetErrors(nameof(EBMSkillPerkName)).Any() && EBMSkillPerkSelection != null)
            {
                EBMSkillPerkSelection.Name = value;
            }
        }

        partial void OnEBMSkillPerkDescriptionChanged(string value)
        {
            if (!GetErrors(nameof(EBMSkillPerkDescription)).Any() && EBMSkillPerkSelection != null)
            {
                EBMSkillPerkSelection.Description = value;
            }
        }

        partial void OnEBMSupernaturalPerkNameChanged(string value)
        {
            if (!GetErrors(nameof(EBMSupernaturalPerkName)).Any() && EBMSupernaturalPerkSelection != null)
            {
                EBMSupernaturalPerkSelection.Name = value;
            }
        }

        partial void OnEBMSupernaturalPerkDescriptionChanged(string value)
        {
            if (!GetErrors(nameof(EBMSupernaturalPerkDescription)).Any() && EBMSupernaturalPerkSelection != null)
            {
                EBMSupernaturalPerkSelection.Description = value;
            }
        }

        partial void OnEBMItemPerkNameChanged(string value)
        {
            if (!GetErrors(nameof(EBMItemPerkName)).Any() && EBMItemPerkSelection != null)
            {
                EBMItemPerkSelection.Name = value;
            }
        }

        partial void OnEBMItemPerkDescriptionChanged(string value)
        {
            if (!GetErrors(nameof(EBMItemPerkDescription)).Any() && EBMItemPerkSelection != null)
            {
                EBMItemPerkSelection.Description = value;
            }
        }

        partial void OnEBMCompanionPerkNameChanged(string value)
        {
            if (!GetErrors(nameof(EBMCompanionPerkName)).Any() && EBMCompanionPerkSelection != null)
            {
                EBMCompanionPerkSelection.Name = value;
            }
        }

        partial void OnEBMCompanionPerkDescriptionChanged(string value)
        {
            if (!GetErrors(nameof(EBMCompanionPerkDescription)).Any() && EBMCompanionPerkSelection != null)
            {
                EBMCompanionPerkSelection.Description = value;
            }
        }

        partial void OnEBMDrawbackNameChanged(string value)
        {
            if (!GetErrors(nameof(EBMDrawbackName)).Any() && EBMDrawbackSelection != null)
            {
                EBMDrawbackSelection.Name = value;
            }
        }

        partial void OnEBMDrawbackDescriptionChanged(string value)
        {
            if (!GetErrors(nameof(EBMDrawbackDescription)).Any() && EBMDrawbackSelection != null)
            {
                EBMDrawbackSelection.Description = value;
            }
        }

        partial void OnCurrentPhysicalAttributeSelectionChanged(ProfileAttribute value)
        {
            if (value != null)
            {
                CurrentPhysicalAttributeName = value.Name;
            }
        }

        partial void OnCurrentPhysicalAttributeNameChanged(string value)
        {
            if (!GetErrors(nameof(CurrentPhysicalAttributeName)).Any() && CurrentPhysicalAttributeSelection != null)
            {
                CurrentPhysicalAttributeSelection.Name = value;
            }
        }

        partial void OnCurrentMentalAttributeSelectionChanged(ProfileAttribute value)
        {
            if (value != null)
            {
                CurrentMentalAttributeName = value.Name;
            }
        }

        partial void OnCurrentMentalAttributeNameChanged(string value)
        {
            if (!GetErrors(nameof(CurrentMentalAttributeName)).Any() && CurrentMentalAttributeSelection != null)
            {
                CurrentMentalAttributeSelection.Name = value;
            }
        }

        partial void OnCurrentSupernaturalAttributeSelectionChanged(ProfileAttribute value)
        {
            if (value != null)
            {
                CurrentSupernaturalAttributeName = value.Name;
            }
        }

        partial void OnCurrentSupernaturalAttributeNameChanged(string value)
        {
            if (!GetErrors(nameof(CurrentSupernaturalAttributeName)).Any() && CurrentSupernaturalAttributeSelection != null)
            {
                CurrentSupernaturalAttributeSelection.Name = value;
            }
        }

        partial void OnCurrentPhysicalSkillSelectionChanged(ProfileAttribute value)
        {
            if (value != null)
            {
                CurrentPhysicalSkillName = value.Name;
            }
        }

        partial void OnCurrentPhysicalSkillNameChanged(string value)
        {
            if (!GetErrors(nameof(CurrentPhysicalSkillName)).Any() && CurrentPhysicalSkillSelection != null)
            {
                CurrentPhysicalSkillSelection.Name = value;
            }
        }

        partial void OnCurrentMentalSkillSelectionChanged(ProfileAttribute value)
        {
            if (value != null)
            {
                CurrentMentalSkillName = value.Name;
            }
        }

        partial void OnCurrentMentalSkillNameChanged(string value)
        {
            if (!GetErrors(nameof(CurrentMentalSkillName)).Any() && CurrentMentalSkillSelection != null)
            {
                CurrentMentalSkillSelection.Name = value;
            }
        }

        partial void OnCurrentSocialSkillSelectionChanged(ProfileAttribute value)
        {
            if (value != null)
            {
                CurrentSocialSkillName = value.Name;
            }
        }

        partial void OnCurrentSocialSkillNameChanged(string value)
        {
            if (!GetErrors(nameof(CurrentSocialSkillName)).Any() && CurrentSocialSkillSelection != null)
            {
                CurrentSocialSkillSelection.Name = value;
            }
        }

        partial void OnCurrentTechnologicalSkillSelectionChanged(ProfileAttribute value)
        {
            if (value != null)
            {
                CurrentTechnologicalSkillName = value.Name;
            }
        }

        partial void OnCurrentTechnologicalSkillNameChanged(string value)
        {
            if (!GetErrors(nameof(CurrentTechnologicalSkillName)).Any() && CurrentTechnologicalSkillSelection != null)
            {
                CurrentTechnologicalSkillSelection.Name = value;
            }
        }

        partial void OnCurrentSupernaturalSkillSelectionChanged(ProfileAttribute value)
        {
            if (value != null)
            {
                CurrentSupernaturalSkillName = value.Name;
            }
        }

        partial void OnCurrentSupernaturalSkillNameChanged(string value)
        {
            if (!GetErrors(nameof(CurrentSupernaturalSkillName)).Any() && CurrentSupernaturalSkillSelection != null)
            {
                CurrentSupernaturalSkillSelection.Name = value;
            }
        }

        partial void OnCurrentBoosterSelectionChanged(Booster value)
        {
            if (value != null)
            {
                CurrentBoosterName = value.BoosterName;
                CurrentBoosterDescription = value.BoosterDescription;
            }
        }

        partial void OnCurrentBoosterNameChanged(string value)
        {
            if (!GetErrors(nameof(CurrentBoosterName)).Any() && CurrentBoosterSelection != null)
            {
                CurrentBoosterSelection.BoosterName = value;
            }
        }

        partial void OnCurrentBoosterDescriptionChanged(string value)
        {
            if (!GetErrors(nameof(CurrentBoosterDescription)).Any() && CurrentBoosterSelection != null)
            {
                CurrentBoosterSelection.BoosterDescription = value;
            }
        }
        #endregion

        #region Constructor
        public CosmicPassportViewModel()
        {

        }

        public CosmicPassportViewModel(IDialogService dialogService)
        {
            Messenger.Register<SaveDataChangedMessage>(this, (r, m) =>
            {
                LoadedOptions = LoadedSave.Options;

                LoadCharacterList();
                LoadAllPerkLists();
                LoadBodyModDetails();

                ClearAttributeList();
                LoadAttributeTypes();

                LoadProfileAttributes();

                AttributeCalculationClass.AttributeCalculation(CharacterSelection, CharacterSelectionIndex, LoadedBodyModSupplement, LoadedSave.JumpList);
                AttributeCalculationClass.SkillCalculation(CharacterSelection, CharacterSelectionIndex, LoadedBodyModSupplement, LoadedSave.JumpList);
                AttributeCalculationClass.BoosterCalculation(CharacterSelection, CharacterSelectionIndex, LoadedBodyModSupplement, LoadedSave.JumpList);

                LoadDisplaySettings();
            });
            Messenger.Register<SaveDataSendMessage>(this, (r, m) =>
            {
                LoadedSave = m.Value;
                LoadedOptions = LoadedSave.Options;

                LoadCharacterList();
                LoadAllPerkLists();
                LoadBodyModDetails();

                ClearAttributeList();
                LoadAttributeTypes();

                LoadProfileAttributes();

                AttributeCalculationClass.AttributeCalculation(CharacterSelection, CharacterSelectionIndex, LoadedBodyModSupplement, LoadedSave.JumpList);
                AttributeCalculationClass.SkillCalculation(CharacterSelection, CharacterSelectionIndex, LoadedBodyModSupplement, LoadedSave.JumpList);
                AttributeCalculationClass.BoosterCalculation(CharacterSelection, CharacterSelectionIndex, LoadedBodyModSupplement, LoadedSave.JumpList);

                LoadDisplaySettings();
            });
            Messenger.Register<SupplementChangedMessage>(this, (r, m) =>
            {
                if (m.Value == "Body Mod")
                {
                    LoadedBodyModSupplement = LoadedOptions.BodyModSetting;
                    LoadBodyModDetails();

                    ClearAttributeList();
                    LoadAttributeTypes();

                    LoadProfileAttributes();

                    AttributeCalculationClass.AttributeCalculation(CharacterSelection, CharacterSelectionIndex, LoadedBodyModSupplement, LoadedSave.JumpList);
                    AttributeCalculationClass.SkillCalculation(CharacterSelection, CharacterSelectionIndex, LoadedBodyModSupplement, LoadedSave.JumpList);
                    AttributeCalculationClass.BoosterCalculation(CharacterSelection, CharacterSelectionIndex, LoadedBodyModSupplement, LoadedSave.JumpList);
                }
            });
            Messenger.Register<SettingsLoadedMessage>(this, (r, m) =>
            {
                AppSettings = m.Value;
                LoadDisplaySettings();

                SpellCheckEnabled = AppSettings.SpellCheckEnabled;
            });
            Messenger.Register<SettingsChangedMessage>(this, (r, m) =>
            {
                LoadDisplaySettings();

                SpellCheckEnabled = AppSettings.SpellCheckEnabled;
            });
            Messenger.Register<CategoryChangedMessage>(this, (r, m) =>
            {
                LoadAllPerkLists();
            });

            _dialogService = dialogService;
        }

        #endregion

        #region Methods

        private void LoadCharacterList()
        {
            CharacterList = new(LoadedSave.CharacterList);

            if (CharacterList.Any())
            {
                CharacterSelection = CharacterList.First();
                CharacterSelectionIndex = 0;
            }
        }

        private void LoadDisplaySettings()
        {
            switch (AppSettings.WeightFormat)
            {
                case AppSettingsModel.WeightFormats.Pounds:
                    UsingPounds = true;
                    UsingKilograms = false;
                    break;
                case AppSettingsModel.WeightFormats.Kilograms:
                    UsingPounds = false;
                    UsingKilograms = true;
                    break;
                default:
                    break;
            }

            switch (AppSettings.HeightFormat)
            {
                case AppSettingsModel.HeightFormats.FeetInches:
                    UsingFeet = false;
                    UsingFeetInches = true;
                    UsingMeters = false;
                    break;
                case AppSettingsModel.HeightFormats.Feet:
                    UsingFeet = true;
                    UsingFeetInches = false;
                    UsingMeters = false;
                    break;
                case AppSettingsModel.HeightFormats.Meters:
                    UsingFeet = false;
                    UsingFeetInches = false;
                    UsingMeters = true;
                    break;
                default:
                    break;
            }
        }

        private void LoadCharacterTraits(Character character)
        {
            CharacterTraitList.Clear();

            if (character != null)
            {
                CharacterTraitList = new(character.TraitRow);

                if (CharacterTraitList.Any())
                {
                    CharacterTraitSelection = CharacterTraitList.First();
                }
                else
                {
                    CharacterTraitSelection = new();
                }
            }

            DeleteTraitRowCommand.NotifyCanExecuteChanged();
        }

        private void LoadAltForms(Character character)
        {
            CharacterAltFormList.Clear();

            if (character != null && character.AltForms.Any())
            {
                CharacterAltFormList = new(character.AltForms);

                CharacterAltFormSelection = CharacterAltFormList.Last();
                CharacterAltFormSelectionIndex = CharacterAltFormList.Count - 1;
            }

            DeleteAltFormCommand.NotifyCanExecuteChanged();
        }

        private void LoadAltFormTraits()
        {
            AltFormStrengthWeaknessList.Clear();

            if (CharacterAltFormSelection != null)
            {
                AltFormStrengthWeaknessList = new(CharacterAltFormSelection.StrengthWeaknessRow);

                if (AltFormStrengthWeaknessList.Any())
                {
                    AltFormStrengthWeaknessSelection = AltFormStrengthWeaknessList.First();
                }

                AltFormName = CharacterAltFormSelection.AltFormName;
                AltFormSpecies = CharacterAltFormSelection.AltFormSpecies;
                AltFormDescription = CharacterAltFormSelection.AltFormDescription;
            }

            NewStrengthWeaknessRowCommand.NotifyCanExecuteChanged();
            DeleteStrengthWeaknessRowCommand.NotifyCanExecuteChanged();
        }

        private void LoadAllPerkLists()
        {
            int charIndex;

            InactivePerkLists.Clear();
            CurrentPerkList.Clear();
            PerkTabList.Clear();

            foreach (Character character in LoadedSave.CharacterList)
            {
                InactivePerkLists.Add(character, new());
            }

            foreach (string category in LoadedSave.PerkCategoryList)
            {
                foreach (KeyValuePair<Character, Dictionary<string, List<Purchase>>> list in InactivePerkLists)
                {
                    list.Value.Add(category, new());
                }

                PerkTabList.Add(category);
            }

            foreach (Jump jump in LoadedSave.JumpList)
            {
                foreach (JumpBuild build in jump.Build)
                {
                    charIndex = jump.Build.IndexOf(build);

                    foreach (Purchase purchase in build.Purchase)
                    {
                        if (!jump.PurchaseTypes[purchase.TypeIndex].IsItemType && !purchase.IsTemporary)
                        {
                            InactivePerkLists[CharacterList[charIndex]][purchase.Category].Add(purchase);

                            purchase.SourceJump = jump.Name;
                            purchase.SourceCharacter = LoadedSave.CharacterList[charIndex].Name;
                        }
                    }
                }
            }

            LoadActivePerkLists();
        }

        private void LoadActivePerkLists()
        {
            if (InactivePerkLists.ContainsKey(CharacterSelection))
            {
                ActivePerkLists = InactivePerkLists[CharacterSelection];

                PerkTabIndex = 0;

                LoadCurrentPerkList();
            }
        }

        private void LoadCurrentPerkList()
        {
            if (!string.IsNullOrWhiteSpace(PerkTabName) && InactivePerkLists[CharacterSelection].ContainsKey(PerkTabName))
            {
                CurrentPerkList = new(InactivePerkLists[CharacterSelection][PerkTabName]);
            }
        }

        private void LoadBodyModTabs()
        {
            switch (LoadedBodyModSupplement)
            {
                case Options.BodyModSupplements.Generic:
                    GenericBodyModSelected = true;
                    SBBodyModSelected = false;
                    EssentialBodyModSelected = false;

                    if (LoadedSave.GenericBodyMod.PurchasesAllowed)
                    {
                        BodyModPurchasesAllowed = true;
                    }
                    else
                    {
                        BodyModPurchasesAllowed = false;
                    }
                    break;
                case Options.BodyModSupplements.SBBodyMod:
                    GenericBodyModSelected = false;
                    SBBodyModSelected = true;
                    EssentialBodyModSelected = false;

                    BodyModPurchasesAllowed = true;
                    break;
                case Options.BodyModSupplements.EssentialBodyMod:
                    GenericBodyModSelected = false;
                    SBBodyModSelected = false;
                    EssentialBodyModSelected = true;

                    BodyModPurchasesAllowed = true;
                    break;
                default:
                    break;
            }
        }

        private void LoadBodyModDetails()
        {
            switch (LoadedBodyModSupplement)
            {
                case Options.BodyModSupplements.Generic:
                    if (LoadedSave.GenericBodyMod.IncrementalBudget > 0)
                    {
                        CalculateJumpBP();
                    }
                    else
                    {
                        JumpBPGained = 0;
                    }

                    if (LoadedSave.GenericBodyMod.InvestmentAllowed)
                    {
                        CalculateInvestedCP(CharacterSelectionIndex);

                        CharacterSelection.BodyMod.TotalInvestment = (CPInvested / LoadedSave.GenericBodyMod.InvestmentRatio);
                        BPBought = CharacterSelection.BodyMod.TotalInvestment;
                    }
                    else
                    {
                        CPInvested = 0;
                        BPBought = 0;
                    }
                    break;
                case Options.BodyModSupplements.SBBodyMod:
                    if (LoadedSave.SBBodyMod.InvestmentAllowed)
                    {
                        CalculateInvestedCP(CharacterSelectionIndex);

                        CharacterSelection.BodyMod.TotalInvestment = (CPInvested / LoadedSave.SBBodyMod.InvestmentRatio);
                        BPBought = CharacterSelection.BodyMod.TotalInvestment;
                    }
                    else
                    {
                        CPInvested = 0;
                        BPBought = 0;
                    }
                    break;
                case Options.BodyModSupplements.EssentialBodyMod:
                    if (LoadedSave.EssentialBodyMod.IncrementalBudget > 0)
                    {
                        CalculateJumpBP();
                    }
                    else
                    {
                        JumpBPGained = 0;
                    }

                    if (LoadedSave.EssentialBodyMod.EPAccessModifier != EssentialBodyMod.EPAccessModifiers.RetroCumulative &&
                        LoadedSave.EssentialBodyMod.TemperedBySuffering)
                    {
                        int delay = CharacterSelection.BodyMod.SupplementDelay - 1;
                        int joinedJumpNumber = CharacterSelection.FirstJump;

                        if (delay < 0)
                        {
                            delay = 0;
                        }
                        int gauntletBonus = 0;

                        foreach (Jump jump in LoadedSave.JumpList)
                        {
                            if (jump.JumpNumber > delay && jump.JumpNumber >= joinedJumpNumber)
                            {
                                break;
                            }

                            if (jump.IsGauntlet)
                            {
                                gauntletBonus += 100;
                            }
                        }

                        GauntletBP = gauntletBonus;
                    }
                    else
                    {
                        GauntletBP = 0;
                    }

                    if (LoadedSave.EssentialBodyMod.InvestmentAllowed)
                    {
                        CalculateInvestedCP(CharacterSelectionIndex);

                        CharacterSelection.BodyMod.TotalInvestment = (CPInvested / LoadedSave.EssentialBodyMod.InvestmentRatio);
                        BPBought = CharacterSelection.BodyMod.TotalInvestment;
                    }
                    else
                    {
                        CPInvested = 0;
                        BPBought = 0;
                    }

                    EBMSupplementDelay = CharacterSelection.BodyMod.SupplementDelay;
                    EBMMinorQuests = CharacterSelection.BodyMod.EBMMinorQuests;
                    EBMMajorQuests = CharacterSelection.BodyMod.EBMMajorQuests;

                    switch (LoadedSave.EssentialBodyMod.EssenceMode)
                    {
                        case EssentialBodyMod.EssenceModes.SingleEssence:
                            EBMEssenceAllowed = true;
                            EBMSingleEssence = true;
                            EBMDualEssence = false;
                            EBMMultiEssence = false;
                            break;
                        case EssentialBodyMod.EssenceModes.DualEssence:
                            EBMEssenceAllowed = true;
                            EBMSingleEssence = false;
                            EBMDualEssence = true;
                            EBMMultiEssence = false;
                            break;
                        case EssentialBodyMod.EssenceModes.MultiEssence:
                            EBMEssenceAllowed = true;
                            EBMSingleEssence = false;
                            EBMDualEssence = false;
                            EBMMultiEssence = true;
                            break;
                        default:
                            EBMEssenceAllowed = false;
                            EBMSingleEssence = false;
                            EBMDualEssence = false;
                            EBMMultiEssence = false;
                            break;
                    }

                    EBMEssenceList = new(CharacterSelection.BodyMod.EBMEssenceList);
                    break;
                default:
                    break;
            }

            LoadBodyModDrawbacks();
            LoadBodyModPurchases();
            CalculateQuestBP();
            CalculateTotalBP();

            CalculateBudget();

            LoadBodyModAdditions();

            LoadBodyModTabs();
        }

        private void LoadBodyModDrawbacks()
        {
            switch (LoadedBodyModSupplement)
            {
                case Options.BodyModSupplements.Generic:
                    GenericBodyModDrawbackList = new(CharacterSelection.BodyMod.Limitations);

                    CalculateDrawbackBP(CharacterSelection.BodyMod);

                    break;
                case Options.BodyModSupplements.EssentialBodyMod:
                    EBMDrawbackList = new(CharacterSelection.BodyMod.EBMDrawbackList);

                    EBMDrawbackIndex = 0;

                    CalculateDrawbackBP(CharacterSelection.BodyMod);
                    break;
                default:
                    break;
            }
        }

        private void LoadBodyModPurchases()
        {
            switch (LoadedBodyModSupplement)
            {
                case Options.BodyModSupplements.Generic:
                    if (LoadedSave.GenericBodyMod.PurchasesAllowed)
                    {
                        GenericBodyModPurchaseList = new(CharacterSelection.BodyMod.Purchases);
                    }
                    break;
                case Options.BodyModSupplements.SBBodyMod:
                    SBBodyModGauntlets = CharacterSelection.BodyMod.GauntletsFinished;
                    SBBodyModAnomalousStipend = CharacterSelection.BodyMod.AnomalousLevel * 100;

                    SBExtraBitsList = new(CharacterSelection.BodyMod.ExtraBitsList);

                    SBExtraBitsIndex = 0;

                    switch (CharacterSelection.BodyMod.AffinityIndex)
                    {
                        case 0:
                            SBBodyModAffinityIndex = 0;
                            SBBodyModAffinityCost = 0;
                            break;
                        case 1:
                            SBBodyModAffinityIndex = 1;
                            SBBodyModAffinityCost = 100;
                            break;
                        case 2:
                            SBBodyModAffinityIndex = 2;
                            SBBodyModAffinityCost = 100;
                            break;
                        case 3:
                            SBBodyModAffinityIndex = 3;
                            SBBodyModAffinityCost = 100;
                            break;
                        default:
                            break;
                    }

                    SBBodyModAffinityDescription = CharacterSelection.BodyMod.AffinityDescription;

                    SBBodyModPowerList = new(CharacterSelection.BodyMod.SBPowerList);

                    SBBodyModPowerIndex = 0;
                    break;
                case Options.BodyModSupplements.EssentialBodyMod:
                    EBMBasicPerkList.Clear();
                    EBMPhysicalPerkList.Clear();
                    EBMMentalPerkList.Clear();
                    EBMSpiritualPerkList.Clear();
                    EBMSkillPerkList.Clear();
                    EBMSupernaturalPerkList.Clear();
                    EBMItemPerkList.Clear();
                    EBMCompanionPerkList.Clear();

                    foreach (SupplementPurchase purchase in CharacterSelection.BodyMod.EBMPurchaseList)
                    {
                        switch (purchase.Category)
                        {
                            case "Basic":
                                EBMBasicPerkList.Add(purchase);
                                break;
                            case "Physical":
                                EBMPhysicalPerkList.Add(purchase);
                                break;
                            case "Mental":
                                EBMMentalPerkList.Add(purchase);
                                break;
                            case "Spiritual":
                                EBMSpiritualPerkList.Add(purchase);
                                break;
                            case "Skill":
                                EBMSkillPerkList.Add(purchase);
                                break;
                            case "Supernatural":
                                EBMSupernaturalPerkList.Add(purchase);
                                break;
                            case "Item":
                                EBMItemPerkList.Add(purchase);
                                break;
                            case "Companion":
                                EBMCompanionPerkList.Add(purchase);
                                break;
                            default:
                                break;
                        }
                    }

                    EBMBasicPerkIndex = 0;
                    EBMPhysicalPerkIndex = 0;
                    EBMMentalPerkIndex = 0;
                    EBMSpiritualPerkIndex = 0;
                    EBMSkillPerkIndex = 0;
                    EBMSupernaturalPerkIndex = 0;
                    EBMItemPerkIndex = 0;
                    EBMCompanionPerkIndex = 0;
                    break;
                default:
                    break;
            }
        }

        private void LoadBodyModAdditions()
        {
            BodyModAdditionList.Clear();

            foreach (Jump jump in LoadedSave.JumpList)
            {
                if (jump.Build.Count > CharacterSelectionIndex)
                {
                    foreach (Purchase purchase in jump.Build[CharacterSelectionIndex].Purchase)
                    {
                        if (purchase.BodyModAddition)
                        {
                            BodyModAdditionList.Add(purchase);
                        }
                    }
                }
            }

            if (BodyModAdditionList.Any())
            {
                BodyModAdditionSelection = BodyModAdditionList.Last();
            }
        }

        private void CharacterChanged(int charIndex)
        {
            if (charIndex != -1)
            {
                LoadActivePerkLists();

                LoadBodyModDetails();

                LoadProfileAttributes();

                AttributeCalculationClass.AttributeCalculation(CharacterSelection, CharacterSelectionIndex, LoadedBodyModSupplement, LoadedSave.JumpList);
                AttributeCalculationClass.SkillCalculation(CharacterSelection, CharacterSelectionIndex, LoadedBodyModSupplement, LoadedSave.JumpList);
                AttributeCalculationClass.BoosterCalculation(CharacterSelection, CharacterSelectionIndex, LoadedBodyModSupplement, LoadedSave.JumpList);
            }
        }

        private void PerkTabChanged() => LoadCurrentPerkList();

        private void CalculateTrueAge(Character character)
        {
            int totalYears = 0;
            int totalMonths = 0;
            int totalDays = 0;

            foreach (Jump jump in LoadedSave.JumpList)
            {
                if (jump.JumpNumber >= character.FirstJump && !jump.SkipNumber)
                {
                    totalYears += jump.DurationYears;
                    totalMonths += jump.DurationMonths;
                    totalDays += jump.DurationDays;
                }
            }

            double years = Convert.ToDouble(totalYears);
            double months = Convert.ToDouble(totalMonths);
            double days = Convert.ToDouble(totalDays);

            character.TrueAge = character.Age + Math.Round((years + (months / 12) + (days / 365)), 2, MidpointRounding.AwayFromZero);
        }

        private void CalculateJumpBP()
        {
            int jumps;
            int deposits;

            switch (LoadedBodyModSupplement)
            {
                case Options.BodyModSupplements.Generic:
                    jumps = LoadedSave.JumpList.Last().JumpNumber - CharacterSelection.BodyMod.SupplementDelay;
                    if (jumps < 0)
                    {
                        jumps = 0;
                    }

                    deposits = jumps / LoadedSave.GenericBodyMod.IncrementalInterval;

                    JumpBPGained = (deposits * LoadedSave.GenericBodyMod.IncrementalBudget);
                    break;
                case Options.BodyModSupplements.SBBodyMod:
                    jumps = LoadedSave.JumpList.Last().JumpNumber - CharacterSelection.BodyMod.SupplementDelay;
                    if (jumps < 0)
                    {
                        jumps = 0;
                    }

                    deposits = jumps / LoadedSave.SBBodyMod.IncrementalInterval;

                    JumpBPGained = (deposits * LoadedSave.SBBodyMod.IncrementalBudget);
                    break;
                case Options.BodyModSupplements.EssentialBodyMod:
                    jumps = LoadedSave.JumpList.Last().JumpNumber - CharacterSelection.BodyMod.SupplementDelay;
                    if (jumps < 0)
                    {
                        jumps = 0;
                    }

                    deposits = jumps / LoadedSave.EssentialBodyMod.IncrementalInterval;

                    JumpBPGained = (deposits * LoadedSave.EssentialBodyMod.IncrementalBudget);
                    break;
                default:
                    JumpBPGained = 0;
                    break;
            }
        }

        private void CalculateInvestedCP(int charIndex)
        {
            int total = 0;

            foreach (Jump jump in LoadedSave.JumpList)
            {
                if (jump.Build.Count >= charIndex + 1 && jump.Build[charIndex] != null)
                {
                    total += jump.Build[charIndex].BodyModInvestment;
                }
            }

            CPInvested = total;
        }

        private void CalculateDrawbackBP(BodyModUniversal bodyMod)
        {
            int total = 0;

            switch (LoadedBodyModSupplement)
            {
                case Options.BodyModSupplements.Generic:
                    foreach (SupplementDrawbackModel drawback in bodyMod.Limitations)
                    {
                        if (drawback.Category == "Generic")
                        {
                            total += drawback.Value;
                        }
                    }
                    break;
                case Options.BodyModSupplements.EssentialBodyMod:
                    foreach (SupplementDrawbackModel drawback in bodyMod.EBMDrawbackList)
                    {
                        total += drawback.Value;
                    }
                    break;
                default:
                    break;
            }

            DrawbackBP = total;
        }

        private void CalculateQuestBP()
        {
            if (LoadedBodyModSupplement == Options.BodyModSupplements.EssentialBodyMod)
            {
                int MinorBP = EBMMinorQuests * 50;
                int MajorBP = EBMMajorQuests * 100;

                QuestBP = MinorBP + MajorBP;
            }
            else
            {
                QuestBP = 0;
            }
        }

        private void CalculateTotalBP()
        {
            switch (LoadedBodyModSupplement)
            {
                case Options.BodyModSupplements.Generic:
                    TotalBP = LoadedSave.GenericBodyMod.Budget;
                    TotalBP += JumpBPGained;
                    TotalBP += BPBought;
                    TotalBP += DrawbackBP;
                    break;
                case Options.BodyModSupplements.SBBodyMod:
                    TotalBP = LoadedSave.SBBodyMod.Budget;
                    TotalBP += JumpBPGained;
                    TotalBP += BPBought;
                    break;
                case Options.BodyModSupplements.EssentialBodyMod:
                    TotalBP = LoadedSave.EssentialBodyMod.Budget;
                    TotalBP += JumpBPGained;
                    TotalBP += GauntletBP;
                    TotalBP += BPBought;
                    TotalBP += DrawbackBP;
                    TotalBP += QuestBP;
                    break;
                default:
                    TotalBP = 0;
                    break;
            }
        }

        private void CalculateBudget() => Budget = BudgetCalculationsClass.BodyModBudgetCalculation(CharacterSelection.BodyMod, TotalBP, LoadedBodyModSupplement);

        private void ClearAttributeList()
        {
            PurchaseAttributeList.Clear();
            PurchaseAttributeSelection = new();
            PurchaseAttributeIndex = -1;
        }

        private void LoadAttributeTypes()
        {
            AvailableAttributeTypeList = new()
            {
                "Attribute",
                "Skill",
                "Booster",
                "Special"
            };
        }

        private void ReloadAttributeCategories()
        {
            AvailableAttributeCategoryList.Clear();
            switch (PurchaseAttributeSelection.Type)
            {
                case "Attribute":
                    foreach (string category in PurchaseAttributeSelection.AttributeCategoryList)
                    {
                        AvailableAttributeCategoryList.Add(category);
                    }
                    break;
                case "Skill":
                    foreach (string category in PurchaseAttributeSelection.SkillCategoryList)
                    {
                        AvailableAttributeCategoryList.Add(category);
                    }
                    break;
                case "Booster":
                    AvailableAttributeCategoryList.Add("Booster");
                    AttributeCategorySelection = AvailableAttributeCategoryList.First();
                    break;
                case "Special":
                    foreach (string category in PurchaseAttributeSelection.SpecialCategoryList)
                    {
                        AvailableAttributeCategoryList.Add(category);
                    }
                    break;
                default:
                    break;
            }
        }

        private void LoadProfileAttributes()
        {
            CurrentPhysicalAttributeList.Clear();
            CurrentMentalAttributeList.Clear();
            CurrentSupernaturalAttributeList.Clear();
            CurrentPhysicalSkillList.Clear();
            CurrentMentalSkillList.Clear();
            CurrentSocialSkillList.Clear();
            CurrentTechnologicalSkillList.Clear();
            CurrentSupernaturalSkillList.Clear();
            CurrentBoosterList.Clear();

            foreach (ProfileAttribute attribute in CharacterSelection.Attributes)
            {
                switch (attribute.Category)
                {
                    case "Physical":
                        CurrentPhysicalAttributeList.Add(attribute);
                        break;
                    case "Mental":
                        CurrentMentalAttributeList.Add(attribute);
                        break;
                    case "Supernatural":
                        CurrentSupernaturalAttributeList.Add(attribute);
                        break;
                    default:
                        break;
                }
            }
            foreach (ProfileAttribute attribute in CharacterSelection.Skills)
            {
                switch (attribute.Category)
                {
                    case "Physical":
                        CurrentPhysicalSkillList.Add(attribute);
                        break;
                    case "Mental":
                        CurrentMentalSkillList.Add(attribute);
                        break;
                    case "Social":
                        CurrentSocialSkillList.Add(attribute);
                        break;
                    case "Technological":
                        CurrentTechnologicalSkillList.Add(attribute);
                        break;
                    case "Supernatural":
                        CurrentSupernaturalSkillList.Add(attribute);
                        break;
                    default:
                        break;
                }
            }
            foreach (Booster booster in CharacterSelection.Boosters)
            {
                CurrentBoosterList.Add(booster);
            }
        }

        private void LoadCurrentAttributeList()
        {
            CurrentAttributeList.Clear();

            switch (PurchaseAttributeSelection.Type)
            {
                case "Attribute":
                    switch (PurchaseAttributeSelection.Category)
                    {
                        case "Physical":
                            foreach (ProfileAttribute attribute in CharacterSelection.Attributes)
                            {
                                if (attribute.Category == "Physical")
                                {
                                    CurrentAttributeList.Add(attribute.Name);
                                }
                            }
                            break;
                        case "Mental":
                            foreach (ProfileAttribute attribute in CharacterSelection.Attributes)
                            {
                                if (attribute.Category == "Mental")
                                {
                                    CurrentAttributeList.Add(attribute.Name);
                                }
                            }
                            break;
                        case "Supernatural":
                            foreach (ProfileAttribute attribute in CharacterSelection.Attributes)
                            {
                                if (attribute.Category == "Supernatural")
                                {
                                    CurrentAttributeList.Add(attribute.Name);
                                }
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                case "Skill":
                    switch (PurchaseAttributeSelection.Category)
                    {
                        case "Physical":
                            foreach (ProfileAttribute attribute in CharacterSelection.Skills)
                            {
                                if (attribute.Category == "Physical")
                                {
                                    CurrentAttributeList.Add(attribute.Name);
                                }
                            }
                            break;
                        case "Mental":
                            foreach (ProfileAttribute attribute in CharacterSelection.Skills)
                            {
                                if (attribute.Category == "Mental")
                                {
                                    CurrentAttributeList.Add(attribute.Name);
                                }
                            }
                            break;
                        case "Social":
                            foreach (ProfileAttribute attribute in CharacterSelection.Skills)
                            {
                                if (attribute.Category == "Social")
                                {
                                    CurrentAttributeList.Add(attribute.Name);
                                }
                            }
                            break;
                        case "Technological":
                            foreach (ProfileAttribute attribute in CharacterSelection.Skills)
                            {
                                if (attribute.Category == "Technological")
                                {
                                    CurrentAttributeList.Add(attribute.Name);
                                }
                            }
                            break;
                        case "Supernatural":
                            foreach (ProfileAttribute attribute in CharacterSelection.Skills)
                            {
                                if (attribute.Category == "Supernatural")
                                {
                                    CurrentAttributeList.Add(attribute.Name);
                                }
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                case "Booster":
                    foreach (Booster booster in CharacterSelection.Boosters)
                    {
                        CurrentAttributeList.Add(booster.BoosterName);
                    }
                    break;
                case "Special":
                    switch (PurchaseAttributeSelection.Category)
                    {
                        case "1-up":
                            CurrentAttributeList.Add("1-up");
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }

        private void UpdateBoosterDependencyNames()
        {
            foreach (Booster booster in CurrentBoosterList)
            {
                foreach (BoosterDependency dependency in booster.BoosterDependencies)
                {
                    if (booster.BoosterDependencies.IndexOf(dependency) < CurrentBoosterList.IndexOf(booster))
                    {
                        dependency.DependencySource = CurrentBoosterList[booster.BoosterDependencies.IndexOf(dependency)].BoosterName;
                    }
                    else if (booster.BoosterDependencies.IndexOf(dependency) >= CurrentBoosterList.IndexOf(booster))
                    {
                        dependency.DependencySource = CurrentBoosterList[booster.BoosterDependencies.IndexOf(dependency) + 1].BoosterName;
                    }
                }
            }
        }
        #endregion

        #region Commands

        [RelayCommand]
        private void NewCharacter()
        {
            Character character = new($"Character #{LoadedSave.CharacterList.Count + 1}");

            LoadedSave.CharacterList.Add(character);
            CharacterList.Add(character);

            CharacterSelection = CharacterList.Last();
            CharacterSelectionIndex = CharacterList.Count - 1;

            RefreshPassports();

            DeleteCharacterCommand.NotifyCanExecuteChanged();
        }

        [RelayCommand(CanExecute = nameof(CanDeleteCharacter))]
        private void DeleteCharacter()
        {
            if (_dialogService.ConfirmDialog("Are you sure you want to delete the currently selected character? " +
                "WARNING: This will result in the permanent and irreversible loss of all data associated with the " +
                "character. This decision cannot be reversed."))
            {
                int index = CharacterSelectionIndex;
                CharacterList.RemoveAt(index);
                LoadedSave.CharacterList.RemoveAt(index);

                foreach (Jump jump in LoadedSave.JumpList)
                {
                    if (jump.Build.Count > index)
                    {
                        jump.Build.RemoveAt(index);
                    }

                    foreach (JumpBuild build in jump.Build)
                    {
                        foreach (CompanionPurchase purchase in build.CompanionPurchase)
                        {
                            if (purchase.CompanionImportDetails.Count > index)
                            {
                                purchase.CompanionImportDetails.RemoveAt(index);
                            }
                        }
                    }
                }

                CharacterSelectionIndex = 0;

                DeleteCharacterCommand.NotifyCanExecuteChanged();
                Messenger.Send(new CharacterDeletedMessage(true));
            }

        }

        private bool CanDeleteCharacter() => CharacterList.Any() && CharacterSelectionIndex > 0;

        [RelayCommand]
        private void NewTraitRow()
        {
            Trait trait = new();

            CharacterTraitList.Add(trait);
            CharacterSelection.TraitRow.Add(trait);

            DeleteTraitRowCommand.NotifyCanExecuteChanged();
        }

        [RelayCommand(CanExecute = nameof(CanDeleteTraitRow))]
        private void DeleteTraitRow()
        {
            CharacterTraitList.Remove(CharacterTraitList.Last());
            CharacterSelection.TraitRow.Remove(CharacterSelection.TraitRow.Last());

            DeleteTraitRowCommand.NotifyCanExecuteChanged();
        }

        private bool CanDeleteTraitRow() => CharacterTraitList.Any();

        [RelayCommand]
        private void RefreshPassports()
        {
            LoadCharacterTraits(CharacterSelection);
            CalculateTrueAge(CharacterSelection);
            LoadAltForms(CharacterSelection);
            LoadAllPerkLists();
            LoadBodyModTabs();
            LoadBodyModDetails();
            LoadBodyModAdditions();

            ClearAttributeList();
            LoadAttributeTypes();

            AttributeCalculationClass.AttributeCalculation(CharacterSelection, CharacterSelectionIndex, LoadedBodyModSupplement, LoadedSave.JumpList);
            AttributeCalculationClass.SkillCalculation(CharacterSelection, CharacterSelectionIndex, LoadedBodyModSupplement, LoadedSave.JumpList);
            AttributeCalculationClass.BoosterCalculation(CharacterSelection, CharacterSelectionIndex, LoadedBodyModSupplement, LoadedSave.JumpList);

            LoadDisplaySettings();
        }

        [RelayCommand]
        private void NewAltForm()
        {
            AltForm altForm = new($"Alt-form #{CharacterSelection.AltForms.Count + 1}");

            CharacterAltFormList.Add(altForm);
            CharacterSelection.AltForms.Add(altForm);

            CharacterAltFormSelection = CharacterAltFormList.Last();
            CharacterAltFormSelectionIndex = CharacterAltFormList.Count - 1;

            DeleteAltFormCommand.NotifyCanExecuteChanged();
            NewStrengthWeaknessRowCommand.NotifyCanExecuteChanged();
            DeleteStrengthWeaknessRowCommand.NotifyCanExecuteChanged();
        }

        [RelayCommand(CanExecute = nameof(CanDeleteAltForm))]
        private void DeleteAltForm()
        {
            if (_dialogService.ConfirmDialog("Are you sure that you want to delete this Alt-Form? " +
                "This decision cannot be reversed."))
            {
                int index = CharacterAltFormSelectionIndex;

                CharacterAltFormList.RemoveAt(index);
                CharacterSelection.AltForms.RemoveAt(index);

                if (CharacterAltFormList.Any())
                {
                    CharacterAltFormSelection = CharacterAltFormList.First();
                    CharacterAltFormSelectionIndex = 0;
                }

                DeleteAltFormCommand.NotifyCanExecuteChanged();
                NewStrengthWeaknessRowCommand.NotifyCanExecuteChanged();
                DeleteStrengthWeaknessRowCommand.NotifyCanExecuteChanged();
            }
        }

        private bool CanDeleteAltForm() => CharacterAltFormList.Any() && CharacterAltFormSelection != null;

        [RelayCommand(CanExecute = nameof(CanNewStrengthWeaknessRow))]
        private void NewStrengthWeaknessRow()
        {
            if (CharacterAltFormSelection != null)
            {
                AltFormTraitModel row = new();

                AltFormStrengthWeaknessList.Add(row);
                CharacterAltFormSelection.StrengthWeaknessRow.Add(row);

                DeleteStrengthWeaknessRowCommand.NotifyCanExecuteChanged();
            }
        }

        private bool CanNewStrengthWeaknessRow() => CharacterAltFormSelection != null;

        [RelayCommand(CanExecute = nameof(CanDeleteStrengthWeaknessRow))]
        private void DeleteStrengthWeaknessRow()
        {
            if (CharacterAltFormSelection != null)
            {
                AltFormStrengthWeaknessList.Remove(AltFormStrengthWeaknessList.Last());
                CharacterAltFormSelection.StrengthWeaknessRow.Remove(CharacterAltFormSelection.StrengthWeaknessRow.Last());

                DeleteStrengthWeaknessRowCommand.NotifyCanExecuteChanged();
            }
        }

        private bool CanDeleteStrengthWeaknessRow() => AltFormStrengthWeaknessList.Any();

        [RelayCommand]
        private void RefreshBP() => LoadBodyModDetails();

        [RelayCommand]
        private void NewBodyModPurchase()
        {
            SupplementPurchase purchase = new();

            switch (LoadedBodyModSupplement)
            {
                case Options.BodyModSupplements.Generic:
                    purchase.Category = "Generic";
                    purchase.Name = "New Purchase";

                    CharacterSelection.BodyMod.Purchases.Add(purchase);
                    GenericBodyModPurchaseList.Add(purchase);

                    GenericBodyModPurchaseSelectionIndex = GenericBodyModPurchaseList.Count - 1;
                    break;
                case Options.BodyModSupplements.SBBodyMod:
                    break;
                case Options.BodyModSupplements.EssentialBodyMod:
                    break;
                default:
                    break;
            }

            DeleteBodyModPurchaseCommand.NotifyCanExecuteChanged();
            MoveBodyModPurchaseUpCommand.NotifyCanExecuteChanged();
            MoveBodyModPurchaseDownCommand.NotifyCanExecuteChanged();
        }

        [RelayCommand(CanExecute = nameof(CanDeleteBodyModPurchase))]
        private void DeleteBodyModPurchase()
        {
            if (_dialogService.ConfirmDialog("Are you sure you want to delete this purchase? " +
                "This action cannot be reversed."))
            {
                switch (LoadedBodyModSupplement)
                {
                    case Options.BodyModSupplements.Generic:
                        CharacterSelection.BodyMod.Purchases.RemoveAt(GenericBodyModPurchaseSelectionIndex);
                        GenericBodyModPurchaseList.RemoveAt(GenericBodyModPurchaseSelectionIndex);
                        GenericBodyModPurchaseSelectionIndex = 0;
                        break;
                    case Options.BodyModSupplements.SBBodyMod:
                        break;
                    case Options.BodyModSupplements.EssentialBodyMod:
                        break;
                    default:
                        break;
                }

                DeleteBodyModPurchaseCommand.NotifyCanExecuteChanged();
                MoveBodyModPurchaseUpCommand.NotifyCanExecuteChanged();
                MoveBodyModPurchaseDownCommand.NotifyCanExecuteChanged();
            }
        }

        private bool CanDeleteBodyModPurchase()
        {
            return LoadedBodyModSupplement switch
            {
                Options.BodyModSupplements.Generic => GenericBodyModPurchaseList.Any() && GenericBodyModPurchaseSelectionIndex != -1,
                _ => false,
            };
        }

        [RelayCommand(CanExecute = nameof(CanMoveBodyModPurchaseUp))]
        private void MoveBodyModPurchaseUp()
        {
            int index;
            int indexList1;
            int indexList2;

            switch (LoadedBodyModSupplement)
            {
                case Options.BodyModSupplements.Generic:
                    index = GenericBodyModPurchaseSelectionIndex;
                    indexList1 = CharacterSelection.BodyMod.Purchases.IndexOf(GenericBodyModPurchaseList[index]);
                    indexList2 = CharacterSelection.BodyMod.Purchases.IndexOf(GenericBodyModPurchaseList[index - 1]);
                    GenericBodyModPurchaseList.SwapCollectionItems(index, index - 1);
                    CharacterSelection.BodyMod.Purchases.SwapListItems(indexList1, indexList2);
                    GenericBodyModPurchaseSelectionIndex = index - 1;
                    break;
                case Options.BodyModSupplements.SBBodyMod:
                    break;
                case Options.BodyModSupplements.EssentialBodyMod:
                    break;
                default:
                    break;
            }

            DeleteBodyModPurchaseCommand.NotifyCanExecuteChanged();
            MoveBodyModPurchaseUpCommand.NotifyCanExecuteChanged();
            MoveBodyModPurchaseDownCommand.NotifyCanExecuteChanged();
        }

        private bool CanMoveBodyModPurchaseUp()
        {
            return LoadedBodyModSupplement switch
            {
                Options.BodyModSupplements.Generic => GenericBodyModPurchaseSelectionIndex > 0,
                _ => false,
            };
        }

        [RelayCommand(CanExecute = nameof(CanMoveBodyModPurchaseDown))]
        private void MoveBodyModPurchaseDown()
        {
            int index;
            int indexList1;
            int indexList2;

            switch (LoadedBodyModSupplement)
            {
                case Options.BodyModSupplements.Generic:
                    index = GenericBodyModPurchaseSelectionIndex;
                    indexList1 = CharacterSelection.BodyMod.Purchases.IndexOf(GenericBodyModPurchaseList[index]);
                    indexList2 = CharacterSelection.BodyMod.Purchases.IndexOf(GenericBodyModPurchaseList[index + 1]);
                    GenericBodyModPurchaseList.SwapCollectionItems(index, index + 1);
                    CharacterSelection.BodyMod.Purchases.SwapListItems(indexList1, indexList2);
                    GenericBodyModPurchaseSelectionIndex = index + 1;
                    break;
                case Options.BodyModSupplements.SBBodyMod:
                    break;
                case Options.BodyModSupplements.EssentialBodyMod:
                    break;
                default:
                    break;
            }

            DeleteBodyModPurchaseCommand.NotifyCanExecuteChanged();
            MoveBodyModPurchaseUpCommand.NotifyCanExecuteChanged();
            MoveBodyModPurchaseDownCommand.NotifyCanExecuteChanged();
        }

        private bool CanMoveBodyModPurchaseDown()
        {
            return LoadedBodyModSupplement switch
            {
                Options.BodyModSupplements.Generic => GenericBodyModPurchaseSelectionIndex < GenericBodyModPurchaseList.Count - 1,
                _ => false,
            };
        }

        [RelayCommand]
        private void NewBodyModDrawback()
        {
            SupplementDrawbackModel drawback = new();

            switch (LoadedBodyModSupplement)
            {
                case Options.BodyModSupplements.Generic:
                    drawback.Category = "Generic";
                    drawback.Name = "New Purchase";

                    CharacterSelection.BodyMod.Limitations.Add(drawback);
                    GenericBodyModDrawbackList.Add(drawback);

                    GenericBodyModDrawbackIndex = GenericBodyModDrawbackList.Count - 1;
                    break;
                case Options.BodyModSupplements.SBBodyMod:
                    break;
                case Options.BodyModSupplements.EssentialBodyMod:
                    break;
                default:
                    break;
            }

            DeleteBodyModDrawbackCommand.NotifyCanExecuteChanged();
            MoveBodyModDrawbackUpCommand.NotifyCanExecuteChanged();
            MoveBodyModDrawbackDownCommand.NotifyCanExecuteChanged();
        }

        [RelayCommand(CanExecute = nameof(CanDeleteBodyModDrawback))]
        private void DeleteBodyModDrawback()
        {
            if (_dialogService.ConfirmDialog("Are you sure you want to delete this Drawback? " +
                "This action cannot be reversed."))
            {
                switch (LoadedBodyModSupplement)
                {
                    case Options.BodyModSupplements.Generic:
                        CharacterSelection.BodyMod.Limitations.RemoveAt(GenericBodyModDrawbackIndex);
                        GenericBodyModDrawbackList.RemoveAt(GenericBodyModDrawbackIndex);
                        GenericBodyModDrawbackIndex = 0;
                        break;
                    case Options.BodyModSupplements.SBBodyMod:
                        break;
                    case Options.BodyModSupplements.EssentialBodyMod:
                        break;
                    default:
                        break;
                }

                DeleteBodyModDrawbackCommand.NotifyCanExecuteChanged();
                MoveBodyModDrawbackUpCommand.NotifyCanExecuteChanged();
                MoveBodyModDrawbackDownCommand.NotifyCanExecuteChanged();
            }
        }

        private bool CanDeleteBodyModDrawback()
        {
            return LoadedBodyModSupplement switch
            {
                Options.BodyModSupplements.Generic => GenericBodyModDrawbackList.Any() && GenericBodyModDrawbackIndex != -1,
                _ => false,
            };
        }

        [RelayCommand(CanExecute = nameof(CanMoveBodyModDrawbackUp))]
        private void MoveBodyModDrawbackUp()
        {
            int index;
            int indexList1;
            int indexList2;

            switch (LoadedBodyModSupplement)
            {
                case Options.BodyModSupplements.Generic:
                    index = GenericBodyModDrawbackIndex;
                    indexList1 = CharacterSelection.BodyMod.Limitations.IndexOf(GenericBodyModDrawbackList[index]);
                    indexList2 = CharacterSelection.BodyMod.Limitations.IndexOf(GenericBodyModDrawbackList[index - 1]);
                    GenericBodyModDrawbackList.SwapCollectionItems(index, index - 1);
                    CharacterSelection.BodyMod.Limitations.SwapListItems(indexList1, indexList2);
                    GenericBodyModDrawbackIndex = index - 1;
                    break;
                case Options.BodyModSupplements.SBBodyMod:
                    break;
                case Options.BodyModSupplements.EssentialBodyMod:
                    break;
                default:
                    break;
            }

            DeleteBodyModDrawbackCommand.NotifyCanExecuteChanged();
            MoveBodyModDrawbackUpCommand.NotifyCanExecuteChanged();
            MoveBodyModDrawbackDownCommand.NotifyCanExecuteChanged();
        }

        private bool CanMoveBodyModDrawbackUp()
        {
            return LoadedBodyModSupplement switch
            {
                Options.BodyModSupplements.Generic => GenericBodyModDrawbackIndex > 0,
                _ => false,
            };
        }

        [RelayCommand(CanExecute = nameof(CanMoveBodyModDrawbackDown))]
        private void MoveBodyModDrawbackDown()
        {
            int index;
            int indexList1;
            int indexList2;

            switch (LoadedBodyModSupplement)
            {
                case Options.BodyModSupplements.Generic:
                    index = GenericBodyModDrawbackIndex;
                    indexList1 = CharacterSelection.BodyMod.Limitations.IndexOf(GenericBodyModDrawbackList[index]);
                    indexList2 = CharacterSelection.BodyMod.Limitations.IndexOf(GenericBodyModDrawbackList[index + 1]);
                    GenericBodyModDrawbackList.SwapCollectionItems(index, index + 1);
                    CharacterSelection.BodyMod.Limitations.SwapListItems(indexList1, indexList2);
                    GenericBodyModDrawbackIndex = index + 1;
                    break;
                case Options.BodyModSupplements.SBBodyMod:
                    break;
                case Options.BodyModSupplements.EssentialBodyMod:
                    break;
                default:
                    break;
            }

            DeleteBodyModDrawbackCommand.NotifyCanExecuteChanged();
            MoveBodyModDrawbackUpCommand.NotifyCanExecuteChanged();
            MoveBodyModDrawbackDownCommand.NotifyCanExecuteChanged();
        }

        private bool CanMoveBodyModDrawbackDown()
        {
            return LoadedBodyModSupplement switch
            {
                Options.BodyModSupplements.Generic => GenericBodyModDrawbackIndex < GenericBodyModDrawbackList.Count - 1,
                _ => false,
            };
        }

        [RelayCommand]
        private void NewSBExtraBits()
        {
            SupplementPurchase purchase = new();

            CharacterSelection.BodyMod.ExtraBitsList.Add(purchase);
            SBExtraBitsList.Add(purchase);

            SBExtraBitsIndex = SBExtraBitsList.Count - 1;

            DeleteSBExtraBitsCommand.NotifyCanExecuteChanged();
        }

        [RelayCommand(CanExecute = nameof(CanDeleteSBExtraBits))]
        private void DeleteSBExtraBits()
        {
            if (_dialogService.ConfirmDialog("Are you sure you want to delete this purchase? " +
                "This action cannot be reversed."))
            {
                CharacterSelection.BodyMod.ExtraBitsList.RemoveAt(SBExtraBitsIndex);
                SBExtraBitsList.RemoveAt(SBExtraBitsIndex);

                SBExtraBitsIndex = 0;

                DeleteSBExtraBitsCommand.NotifyCanExecuteChanged();
            }
        }

        private bool CanDeleteSBExtraBits() => SBExtraBitsList.Any() && SBExtraBitsIndex != -1;

        [RelayCommand]
        private void NewSBPower()
        {
            SupplementPurchase purchase = new($"Power #{SBBodyModPowerList.Count + 1}");

            SBBodyModPowerList.Add(purchase);
            CharacterSelection.BodyMod.SBPowerList.Add(purchase);

            SBBodyModPowerIndex = SBBodyModPowerList.Count - 1;

            DeleteSBPowerCommand.NotifyCanExecuteChanged();
        }

        [RelayCommand(CanExecute = nameof(CanDeleteSBPower))]
        private void DeleteSBPower()
        {
            if (_dialogService.ConfirmDialog("Are you sure you want to delete this power? " +
                "This action cannot be reversed."))
            {
                CharacterSelection.BodyMod.SBPowerList.RemoveAt(SBBodyModPowerIndex);
                SBBodyModPowerList.RemoveAt(SBBodyModPowerIndex);

                SBBodyModPowerIndex = 0;

                DeleteSBPowerCommand.NotifyCanExecuteChanged();
            }
        }

        private bool CanDeleteSBPower() => SBBodyModPowerList.Any() && SBBodyModPowerIndex != -1;

        [RelayCommand]
        private void NewEssence()
        {
            EBMEssence essence = new()
            {
                Name = $"Essence #{EBMEssenceList.Count + 1}"
            };

            EBMEssenceList.Add(essence);
            CharacterSelection.BodyMod.EBMEssenceList.Add(essence);

            EBMEssenceSelection = EBMEssenceList.Last();

            DeleteEssenceCommand.NotifyCanExecuteChanged();
        }

        [RelayCommand(CanExecute = nameof(CanDeleteEssence))]
        private void DeleteEssence()
        {
            if (_dialogService.ConfirmDialog("Are you sure you want to delete this essence? " +
                "This action cannot be reversed."))
            {
                CharacterSelection.BodyMod.EBMEssenceList.RemoveAt(EBMEssenceIndex);
                EBMEssenceList.RemoveAt(EBMEssenceIndex);

                EBMEssenceIndex = 0;

                DeleteEssenceCommand.NotifyCanExecuteChanged();
            }
        }

        private bool CanDeleteEssence() => EBMEssenceSelection != null && EBMEssenceList.Any();

        [RelayCommand]
        private void NewEBMPerk()
        {
            SupplementPurchase purchase = new();
            switch (EBMPurchaseTabIndex)
            {
                case 0:
                    purchase.Name = $"Purchase #{EBMBasicPerkList.Count + 1}";
                    purchase.Category = "Basic";

                    EBMBasicPerkList.Add(purchase);
                    CharacterSelection.BodyMod.EBMPurchaseList.Add(purchase);

                    EBMBasicPerkIndex = EBMBasicPerkList.Count - 1;
                    break;
                case 1:
                    purchase.Name = $"Purchase #{EBMPhysicalPerkList.Count + 1}";
                    purchase.Category = "Physical";

                    EBMPhysicalPerkList.Add(purchase);
                    CharacterSelection.BodyMod.EBMPurchaseList.Add(purchase);

                    EBMPhysicalPerkIndex = EBMPhysicalPerkList.Count - 1;
                    break;
                case 2:
                    purchase.Name = $"Purchase #{EBMMentalPerkList.Count + 1}";
                    purchase.Category = "Mental";

                    EBMMentalPerkList.Add(purchase);
                    CharacterSelection.BodyMod.EBMPurchaseList.Add(purchase);

                    EBMMentalPerkIndex = EBMMentalPerkList.Count - 1;
                    break;
                case 3:
                    purchase.Name = $"Purchase #{EBMSpiritualPerkList.Count + 1}";
                    purchase.Category = "Spiritual";

                    EBMSpiritualPerkList.Add(purchase);
                    CharacterSelection.BodyMod.EBMPurchaseList.Add(purchase);

                    EBMSpiritualPerkIndex = EBMSpiritualPerkList.Count - 1;
                    break;
                case 4:
                    purchase.Name = $"Purchase #{EBMSkillPerkList.Count + 1}";
                    purchase.Category = "Skill";

                    EBMSkillPerkList.Add(purchase);
                    CharacterSelection.BodyMod.EBMPurchaseList.Add(purchase);

                    EBMSkillPerkIndex = EBMSkillPerkList.Count - 1;
                    break;
                case 5:
                    purchase.Name = $"Purchase #{EBMSupernaturalPerkList.Count + 1}";
                    purchase.Category = "Supernatural";

                    EBMSupernaturalPerkList.Add(purchase);
                    CharacterSelection.BodyMod.EBMPurchaseList.Add(purchase);

                    EBMSupernaturalPerkIndex = EBMSupernaturalPerkList.Count - 1;
                    break;
                case 6:
                    purchase.Name = $"Purchase #{EBMItemPerkList.Count + 1}";
                    purchase.Category = "Item";

                    EBMItemPerkList.Add(purchase);
                    CharacterSelection.BodyMod.EBMPurchaseList.Add(purchase);

                    EBMItemPerkIndex = EBMItemPerkList.Count - 1;
                    break;
                case 7:
                    purchase.Name = $"Purchase #{EBMCompanionPerkList.Count + 1}";
                    purchase.Category = "Companion";

                    EBMCompanionPerkList.Add(purchase);
                    CharacterSelection.BodyMod.EBMPurchaseList.Add(purchase);

                    EBMCompanionPerkIndex = EBMCompanionPerkList.Count - 1;
                    break;
                default:
                    break;
            }

            DeleteEBMPerkCommand.NotifyCanExecuteChanged();
        }

        [RelayCommand(CanExecute = nameof(CanDeleteEBMPerk))]
        private void DeleteEBMPerk()
        {
            if (_dialogService.ConfirmDialog("Are you sure you want to delete this power? " +
                "This action cannot be reversed."))
            {
                switch (EBMPurchaseTabIndex)
                {
                    case 0:
                        CharacterSelection.BodyMod.EBMPurchaseList.RemoveAt(CharacterSelection.BodyMod.EBMPurchaseList.IndexOf(EBMBasicPerkSelection));
                        EBMBasicPerkList.RemoveAt(EBMBasicPerkIndex);

                        EBMBasicPerkIndex = 0;
                        break;
                    case 1:
                        CharacterSelection.BodyMod.EBMPurchaseList.RemoveAt(CharacterSelection.BodyMod.EBMPurchaseList.IndexOf(EBMPhysicalPerkSelection));
                        EBMPhysicalPerkList.RemoveAt(EBMPhysicalPerkIndex);

                        EBMPhysicalPerkIndex = 0;
                        break;
                    case 2:
                        CharacterSelection.BodyMod.EBMPurchaseList.RemoveAt(CharacterSelection.BodyMod.EBMPurchaseList.IndexOf(EBMMentalPerkSelection));
                        EBMMentalPerkList.RemoveAt(EBMMentalPerkIndex);

                        EBMMentalPerkIndex = 0;
                        break;
                    case 3:
                        CharacterSelection.BodyMod.EBMPurchaseList.RemoveAt(CharacterSelection.BodyMod.EBMPurchaseList.IndexOf(EBMSpiritualPerkSelection));
                        EBMSpiritualPerkList.RemoveAt(EBMSpiritualPerkIndex);

                        EBMSpiritualPerkIndex = 0;
                        break;
                    case 4:
                        CharacterSelection.BodyMod.EBMPurchaseList.RemoveAt(CharacterSelection.BodyMod.EBMPurchaseList.IndexOf(EBMSkillPerkSelection));
                        EBMSkillPerkList.RemoveAt(EBMSkillPerkIndex);

                        EBMSkillPerkIndex = 0;
                        break;
                    case 5:
                        CharacterSelection.BodyMod.EBMPurchaseList.RemoveAt(CharacterSelection.BodyMod.EBMPurchaseList.IndexOf(EBMSupernaturalPerkSelection));
                        EBMSupernaturalPerkList.RemoveAt(EBMSupernaturalPerkIndex);

                        EBMSupernaturalPerkIndex = 0;
                        break;
                    case 6:
                        CharacterSelection.BodyMod.EBMPurchaseList.RemoveAt(CharacterSelection.BodyMod.EBMPurchaseList.IndexOf(EBMItemPerkSelection));
                        EBMItemPerkList.RemoveAt(EBMItemPerkIndex);

                        EBMItemPerkIndex = 0;
                        break;
                    case 7:
                        CharacterSelection.BodyMod.EBMPurchaseList.RemoveAt(CharacterSelection.BodyMod.EBMPurchaseList.IndexOf(EBMCompanionPerkSelection));
                        EBMCompanionPerkList.RemoveAt(EBMCompanionPerkIndex);

                        EBMCompanionPerkIndex = 0;
                        break;
                    default:
                        break;
                }

                DeleteEBMPerkCommand.NotifyCanExecuteChanged();
            }
        }

        private bool CanDeleteEBMPerk()
        {
            return EBMPurchaseTabIndex switch
            {
                0 => EBMBasicPerkList.Any() && EBMBasicPerkIndex != -1,
                1 => EBMPhysicalPerkList.Any() && EBMPhysicalPerkIndex != -1,
                2 => EBMMentalPerkList.Any() && EBMMentalPerkIndex != -1,
                3 => EBMSpiritualPerkList.Any() && EBMSpiritualPerkIndex != -1,
                4 => EBMSkillPerkList.Any() && EBMSkillPerkIndex != -1,
                5 => EBMSupernaturalPerkList.Any() && EBMSupernaturalPerkIndex != -1,
                6 => EBMItemPerkList.Any() && EBMItemPerkIndex != -1,
                7 => EBMCompanionPerkList.Any() && EBMCompanionPerkIndex != -1,
                _ => false,
            };
        }

        [RelayCommand]
        private void NewEBMDrawback()
        {
            SupplementDrawbackModel drawback = new()
            {
                Name = $"Drawback #{EBMDrawbackList.Count + 1}"
            };

            CharacterSelection.BodyMod.EBMDrawbackList.Add(drawback);
            EBMDrawbackList.Add(drawback);

            EBMDrawbackIndex = EBMDrawbackList.Count - 1;

            DeleteEBMDrawbackCommand.NotifyCanExecuteChanged();
        }

        [RelayCommand(CanExecute = nameof(CanDeleteEBMDrawback))]
        private void DeleteEBMDrawback()
        {
            if (_dialogService.ConfirmDialog("Are you sure you want to delete this Drawback? " +
                "This action cannot be reversed."))
            {
                CharacterSelection.BodyMod.EBMDrawbackList.RemoveAt(EBMDrawbackIndex);
                EBMDrawbackList.RemoveAt(EBMDrawbackIndex);

                EBMDrawbackIndex = 0;

                DeleteEBMDrawbackCommand.NotifyCanExecuteChanged();
            }
        }

        private bool CanDeleteEBMDrawback() => EBMDrawbackList.Any() && EBMDrawbackIndex != -1;

        [RelayCommand]
        private void NewAugmentTrait()
        {
            if (SBAugmentSelection != null)
            {
                PurchaseAttribute attribute = new()
                {
                    Name = $"Trait #{SBAugmentSelection.Attributes.Count + 1}"
                };

                SBAugmentSelection.Attributes.Add(attribute);
                PurchaseAttributeList.Add(attribute);

                PurchaseAttributeIndex = PurchaseAttributeList.Count - 1;

                DeleteAugmentTraitCommand.NotifyCanExecuteChanged();
            }
        }

        [RelayCommand(CanExecute = nameof(CanDeleteAugmentTrait))]
        private void DeleteAugmentTrait()
        {
            SBAugmentSelection.Attributes.RemoveAt(PurchaseAttributeIndex);
            PurchaseAttributeList.RemoveAt(PurchaseAttributeIndex);

            PurchaseAttributeIndex = 0;

            DeleteAugmentTraitCommand.NotifyCanExecuteChanged();
        }

        private bool CanDeleteAugmentTrait() => PurchaseAttributeList.Any() && PurchaseAttributeIndex != -1 && SBAugmentSelection != null;

        [RelayCommand]
        private void NewGBMPurchaseTrait()
        {
            if (BodyModPurchaseSelection != null)
            {
                PurchaseAttribute attribute = new()
                {
                    Name = $"Trait #{BodyModPurchaseSelection.Attributes.Count + 1}"
                };

                BodyModPurchaseSelection.Attributes.Add(attribute);
                PurchaseAttributeList.Add(attribute);

                PurchaseAttributeIndex = PurchaseAttributeList.Count - 1;

                DeleteGBMPurchaseTraitCommand.NotifyCanExecuteChanged();
            }
        }

        [RelayCommand(CanExecute = nameof(CanDeleteGBMPurchaseTrait))]
        private void DeleteGBMPurchaseTrait()
        {
            BodyModPurchaseSelection.Attributes.RemoveAt(PurchaseAttributeIndex);
            PurchaseAttributeList.RemoveAt(PurchaseAttributeIndex);

            PurchaseAttributeIndex = 0;

            DeleteGBMPurchaseTraitCommand.NotifyCanExecuteChanged();
        }

        private bool CanDeleteGBMPurchaseTrait() => PurchaseAttributeList.Any() && PurchaseAttributeIndex != -1 && BodyModPurchaseSelection != null;

        [RelayCommand]
        private void NewSBPowerTrait()
        {
            if (SBBodyModPowerSelection != null)
            {
                PurchaseAttribute attribute = new()
                {
                    Name = $"Trait #{SBBodyModPowerSelection.Attributes.Count + 1}"
                };

                SBBodyModPowerSelection.Attributes.Add(attribute);
                PurchaseAttributeList.Add(attribute);

                PurchaseAttributeIndex = PurchaseAttributeList.Count - 1;

                DeleteSBPowerTraitCommand.NotifyCanExecuteChanged();
            }
        }

        [RelayCommand(CanExecute = nameof(CanDeleteSBPowerTrait))]
        private void DeleteSBPowerTrait()
        {
            SBBodyModPowerSelection.Attributes.RemoveAt(PurchaseAttributeIndex);
            PurchaseAttributeList.RemoveAt(PurchaseAttributeIndex);

            PurchaseAttributeIndex = 0;

            DeleteSBPowerTraitCommand.NotifyCanExecuteChanged();
        }

        private bool CanDeleteSBPowerTrait() => PurchaseAttributeList.Any() && PurchaseAttributeIndex != -1 && SBBodyModPowerSelection != null;

        [RelayCommand]
        private void NewEBMTrait()
        {
            switch (EBMPurchaseTabIndex)
            {
                case 0:
                    if (EBMBasicPerkIndex != -1)
                    {
                        PurchaseAttribute attribute = new()
                        {
                            Name = $"Trait #{EBMBasicPerkSelection.Attributes.Count + 1}"
                        };

                        EBMBasicPerkSelection.Attributes.Add(attribute);
                        PurchaseAttributeList.Add(attribute);

                        PurchaseAttributeIndex = PurchaseAttributeList.Count - 1;

                        DeleteEBMTraitCommand.NotifyCanExecuteChanged();
                    }
                    break;
                case 1:
                    if (EBMPhysicalPerkIndex != -1)
                    {
                        PurchaseAttribute attribute = new()
                        {
                            Name = $"Trait #{EBMPhysicalPerkSelection.Attributes.Count + 1}"
                        };

                        EBMPhysicalPerkSelection.Attributes.Add(attribute);
                        PurchaseAttributeList.Add(attribute);

                        PurchaseAttributeIndex = PurchaseAttributeList.Count - 1;

                        DeleteEBMTraitCommand.NotifyCanExecuteChanged();
                    }
                    break;
                case 2:
                    if (EBMMentalPerkIndex != -1)
                    {
                        PurchaseAttribute attribute = new()
                        {
                            Name = $"Trait #{EBMMentalPerkSelection.Attributes.Count + 1}"
                        };

                        EBMMentalPerkSelection.Attributes.Add(attribute);
                        PurchaseAttributeList.Add(attribute);

                        PurchaseAttributeIndex = PurchaseAttributeList.Count - 1;

                        DeleteEBMTraitCommand.NotifyCanExecuteChanged();
                    }
                    break;
                case 3:
                    if (EBMSpiritualPerkIndex != -1)
                    {
                        PurchaseAttribute attribute = new()
                        {
                            Name = $"Trait #{EBMSpiritualPerkSelection.Attributes.Count + 1}"
                        };

                        EBMSpiritualPerkSelection.Attributes.Add(attribute);
                        PurchaseAttributeList.Add(attribute);

                        PurchaseAttributeIndex = PurchaseAttributeList.Count - 1;

                        DeleteEBMTraitCommand.NotifyCanExecuteChanged();
                    }
                    break;
                case 4:
                    if (EBMSkillPerkIndex != -1)
                    {
                        PurchaseAttribute attribute = new()
                        {
                            Name = $"Trait #{EBMSkillPerkSelection.Attributes.Count + 1}"
                        };

                        EBMSkillPerkSelection.Attributes.Add(attribute);
                        PurchaseAttributeList.Add(attribute);

                        PurchaseAttributeIndex = PurchaseAttributeList.Count - 1;

                        DeleteEBMTraitCommand.NotifyCanExecuteChanged();
                    }
                    break;
                case 5:
                    if (EBMSupernaturalPerkIndex != -1)
                    {
                        PurchaseAttribute attribute = new()
                        {
                            Name = $"Trait #{EBMSupernaturalPerkSelection.Attributes.Count + 1}"
                        };

                        EBMSupernaturalPerkSelection.Attributes.Add(attribute);
                        PurchaseAttributeList.Add(attribute);

                        PurchaseAttributeIndex = PurchaseAttributeList.Count - 1;

                        DeleteEBMTraitCommand.NotifyCanExecuteChanged();
                    }
                    break;
                case 6:
                    if (EBMItemPerkIndex != -1)
                    {
                        PurchaseAttribute attribute = new()
                        {
                            Name = $"Trait #{EBMItemPerkSelection.Attributes.Count + 1}"
                        };

                        EBMItemPerkSelection.Attributes.Add(attribute);
                        PurchaseAttributeList.Add(attribute);

                        PurchaseAttributeIndex = PurchaseAttributeList.Count - 1;

                        DeleteEBMTraitCommand.NotifyCanExecuteChanged();
                    }
                    break;
                case 7:
                    if (EBMCompanionPerkIndex != -1)
                    {
                        PurchaseAttribute attribute = new()
                        {
                            Name = $"Trait #{EBMCompanionPerkSelection.Attributes.Count + 1}"
                        };

                        EBMCompanionPerkSelection.Attributes.Add(attribute);
                        PurchaseAttributeList.Add(attribute);

                        PurchaseAttributeIndex = PurchaseAttributeList.Count - 1;

                        DeleteEBMTraitCommand.NotifyCanExecuteChanged();
                    }
                    break;
                default:
                    break;
            }

        }

        [RelayCommand(CanExecute = nameof(CanDeleteEBMTrait))]
        private void DeleteEBMTrait()
        {
            switch (EBMPurchaseTabIndex)
            {
                case 0:
                    EBMBasicPerkSelection.Attributes.RemoveAt(PurchaseAttributeIndex);
                    PurchaseAttributeList.RemoveAt(PurchaseAttributeIndex);

                    PurchaseAttributeIndex = 0;

                    DeleteEBMTraitCommand.NotifyCanExecuteChanged();

                    break;
                case 1:
                    EBMPhysicalPerkSelection.Attributes.RemoveAt(PurchaseAttributeIndex);
                    PurchaseAttributeList.RemoveAt(PurchaseAttributeIndex);

                    PurchaseAttributeIndex = 0;

                    DeleteEBMTraitCommand.NotifyCanExecuteChanged();

                    break;
                case 2:
                    EBMMentalPerkSelection.Attributes.RemoveAt(PurchaseAttributeIndex);
                    PurchaseAttributeList.RemoveAt(PurchaseAttributeIndex);

                    PurchaseAttributeIndex = 0;

                    DeleteEBMTraitCommand.NotifyCanExecuteChanged();

                    break;
                case 3:
                    EBMSpiritualPerkSelection.Attributes.RemoveAt(PurchaseAttributeIndex);
                    PurchaseAttributeList.RemoveAt(PurchaseAttributeIndex);

                    PurchaseAttributeIndex = 0;

                    DeleteEBMTraitCommand.NotifyCanExecuteChanged();

                    break;
                case 4:
                    EBMSkillPerkSelection.Attributes.RemoveAt(PurchaseAttributeIndex);
                    PurchaseAttributeList.RemoveAt(PurchaseAttributeIndex);

                    PurchaseAttributeIndex = 0;

                    DeleteEBMTraitCommand.NotifyCanExecuteChanged();

                    break;
                case 5:
                    EBMSupernaturalPerkSelection.Attributes.RemoveAt(PurchaseAttributeIndex);
                    PurchaseAttributeList.RemoveAt(PurchaseAttributeIndex);

                    PurchaseAttributeIndex = 0;

                    DeleteEBMTraitCommand.NotifyCanExecuteChanged();

                    break;
                case 6:
                    EBMItemPerkSelection.Attributes.RemoveAt(PurchaseAttributeIndex);
                    PurchaseAttributeList.RemoveAt(PurchaseAttributeIndex);

                    PurchaseAttributeIndex = 0;

                    DeleteEBMTraitCommand.NotifyCanExecuteChanged();

                    break;
                case 7:
                    EBMCompanionPerkSelection.Attributes.RemoveAt(PurchaseAttributeIndex);
                    PurchaseAttributeList.RemoveAt(PurchaseAttributeIndex);

                    PurchaseAttributeIndex = 0;

                    DeleteEBMTraitCommand.NotifyCanExecuteChanged();

                    break;
                default:
                    break;
            }
        }

        private bool CanDeleteEBMTrait()
        {
            return EBMPurchaseTabIndex switch
            {
                0 => PurchaseAttributeList.Any() && PurchaseAttributeIndex != -1 && EBMBasicPerkIndex != -1,
                1 => PurchaseAttributeList.Any() && PurchaseAttributeIndex != -1 && EBMPhysicalPerkIndex != -1,
                2 => PurchaseAttributeList.Any() && PurchaseAttributeIndex != -1 && EBMMentalPerkIndex != -1,
                3 => PurchaseAttributeList.Any() && PurchaseAttributeIndex != -1 && EBMSpiritualPerkIndex != -1,
                4 => PurchaseAttributeList.Any() && PurchaseAttributeIndex != -1 && EBMSkillPerkIndex != -1,
                5 => PurchaseAttributeList.Any() && PurchaseAttributeIndex != -1 && EBMSupernaturalPerkIndex != -1,
                6 => PurchaseAttributeList.Any() && PurchaseAttributeIndex != -1 && EBMItemPerkIndex != -1,
                7 => PurchaseAttributeList.Any() && PurchaseAttributeIndex != -1 && EBMCompanionPerkIndex != -1,
                _ => false,
            };
        }

        [RelayCommand]
        private void RefreshAttributes() => AttributeCalculationClass.AttributeCalculation(CharacterSelection,
                                                                                           CharacterSelectionIndex,
                                                                                           LoadedBodyModSupplement,
                                                                                           LoadedSave.JumpList);

        [RelayCommand]
        private void NewPhysicalAttribute()
        {
            ProfileAttribute attribute = new($"Physical Attribute{CurrentPhysicalAttributeList.Count + 1}", "Attribute", "Physical");

            CharacterSelection.Attributes.Add(attribute);
            CurrentPhysicalAttributeList.Add(attribute);

            CurrentPhysicalAttributeIndex = CurrentPhysicalAttributeList.Count - 1;

            DeletePhysicalAttributeCommand.NotifyCanExecuteChanged();
        }

        [RelayCommand(CanExecute = nameof(CanDeletePhysicalAttribute))]
        private void DeletePhysicalAttribute()
        {
            if (_dialogService.ConfirmDialog("Are you sure you want to delete this Attribute? " +
                "This action cannot be reversed and any purchase traits assigned to it will need to be reset manually."))
            {
                CharacterSelection.Attributes.Remove(CurrentPhysicalAttributeSelection);
                CurrentPhysicalAttributeList.RemoveAt(CurrentPhysicalAttributeIndex);

                CurrentPhysicalAttributeIndex = 0;

                DeletePhysicalAttributeCommand.NotifyCanExecuteChanged();
            }
        }

        private bool CanDeletePhysicalAttribute() => CurrentPhysicalAttributeList.Any() && CurrentPhysicalAttributeIndex != -1;

        [RelayCommand(CanExecute = nameof(CanMovePhysicalAttributeUp))]
        private void MovePhysicalAttributeUp()
        {
            int index = CurrentPhysicalAttributeIndex;
            int indexList1 = CharacterSelection.Attributes.IndexOf(CurrentPhysicalAttributeList[index]);
            int indexList2 = CharacterSelection.Attributes.IndexOf(CurrentPhysicalAttributeList[index - 1]);

            CurrentPhysicalAttributeList.SwapCollectionItems(index, index - 1);
            CharacterSelection.Attributes.SwapListItems(indexList1, indexList2);
            CurrentPhysicalAttributeIndex = index - 1;

            DeletePhysicalAttributeCommand.NotifyCanExecuteChanged();
            MovePhysicalAttributeUpCommand.NotifyCanExecuteChanged();
            MovePhysicalAttributeDownCommand.NotifyCanExecuteChanged();
        }

        private bool CanMovePhysicalAttributeUp() => CurrentPhysicalAttributeIndex > 0;

        [RelayCommand(CanExecute = nameof(CanMovePhysicalAttributeDown))]
        private void MovePhysicalAttributeDown()
        {
            int index = CurrentPhysicalAttributeIndex;
            int indexList1 = CharacterSelection.Attributes.IndexOf(CurrentPhysicalAttributeList[index]);
            int indexList2 = CharacterSelection.Attributes.IndexOf(CurrentPhysicalAttributeList[index + 1]);

            CurrentPhysicalAttributeList.SwapCollectionItems(index, index + 1);
            CharacterSelection.Attributes.SwapListItems(indexList1, indexList2);
            CurrentPhysicalAttributeIndex = index + 1;

            DeletePhysicalAttributeCommand.NotifyCanExecuteChanged();
            MovePhysicalAttributeUpCommand.NotifyCanExecuteChanged();
            MovePhysicalAttributeDownCommand.NotifyCanExecuteChanged();
        }

        private bool CanMovePhysicalAttributeDown() => CurrentPhysicalAttributeIndex < CurrentPhysicalAttributeList.Count - 1;

        [RelayCommand]
        private void NewMentalAttribute()
        {
            ProfileAttribute attribute = new($"Mental Attribute{CurrentMentalAttributeList.Count + 1}", "Attribute", "Mental");

            CharacterSelection.Attributes.Add(attribute);
            CurrentMentalAttributeList.Add(attribute);

            CurrentMentalAttributeIndex = CurrentMentalAttributeList.Count - 1;

            DeleteMentalAttributeCommand.NotifyCanExecuteChanged();
        }

        [RelayCommand(CanExecute = nameof(CanDeleteMentalAttribute))]
        private void DeleteMentalAttribute()
        {
            if (_dialogService.ConfirmDialog("Are you sure you want to delete this Attribute? " +
                "This action cannot be reversed and any purchase traits assigned to it will need to be reset manually."))
            {
                CharacterSelection.Attributes.Remove(CurrentMentalAttributeSelection);
                CurrentMentalAttributeList.RemoveAt(CurrentMentalAttributeIndex);

                CurrentMentalAttributeIndex = 0;

                DeleteMentalAttributeCommand.NotifyCanExecuteChanged();
            }
        }

        private bool CanDeleteMentalAttribute() => CurrentMentalAttributeList.Any() && CurrentMentalAttributeIndex != -1;

        [RelayCommand(CanExecute = nameof(CanMoveMentalAttributeUp))]
        private void MoveMentalAttributeUp()
        {
            int index = CurrentMentalAttributeIndex;
            int indexList1 = CharacterSelection.Attributes.IndexOf(CurrentMentalAttributeList[index]);
            int indexList2 = CharacterSelection.Attributes.IndexOf(CurrentMentalAttributeList[index - 1]);

            CurrentMentalAttributeList.SwapCollectionItems(index, index - 1);
            CharacterSelection.Attributes.SwapListItems(indexList1, indexList2);
            CurrentMentalAttributeIndex = index - 1;

            DeleteMentalAttributeCommand.NotifyCanExecuteChanged();
            MoveMentalAttributeUpCommand.NotifyCanExecuteChanged();
            MoveMentalAttributeDownCommand.NotifyCanExecuteChanged();
        }

        private bool CanMoveMentalAttributeUp() => CurrentMentalAttributeIndex > 0;

        [RelayCommand(CanExecute = nameof(CanMoveMentalAttributeDown))]
        private void MoveMentalAttributeDown()
        {
            int index = CurrentMentalAttributeIndex;
            int indexList1 = CharacterSelection.Attributes.IndexOf(CurrentMentalAttributeList[index]);
            int indexList2 = CharacterSelection.Attributes.IndexOf(CurrentMentalAttributeList[index + 1]);

            CurrentMentalAttributeList.SwapCollectionItems(index, index + 1);
            CharacterSelection.Attributes.SwapListItems(indexList1, indexList2);
            CurrentMentalAttributeIndex = index + 1;

            DeleteMentalAttributeCommand.NotifyCanExecuteChanged();
            MoveMentalAttributeUpCommand.NotifyCanExecuteChanged();
            MoveMentalAttributeDownCommand.NotifyCanExecuteChanged();
        }

        private bool CanMoveMentalAttributeDown() => CurrentMentalAttributeIndex < CurrentMentalAttributeList.Count - 1;

        [RelayCommand]
        private void NewSupernaturalAttribute()
        {
            ProfileAttribute attribute = new($"Supernatural Attribute{CurrentSupernaturalAttributeList.Count + 1}", "Attribute", "Supernatural");

            CharacterSelection.Attributes.Add(attribute);
            CurrentSupernaturalAttributeList.Add(attribute);

            CurrentSupernaturalAttributeIndex = CurrentSupernaturalAttributeList.Count - 1;

            DeleteSupernaturalAttributeCommand.NotifyCanExecuteChanged();
        }

        [RelayCommand(CanExecute = nameof(CanDeleteSupernaturalAttribute))]
        private void DeleteSupernaturalAttribute()
        {
            if (_dialogService.ConfirmDialog("Are you sure you want to delete this Attribute? " +
                "This action cannot be reversed and any purchase traits assigned to it will need to be reset manually."))
            {
                CharacterSelection.Attributes.Remove(CurrentSupernaturalAttributeSelection);
                CurrentSupernaturalAttributeList.RemoveAt(CurrentSupernaturalAttributeIndex);

                CurrentSupernaturalAttributeIndex = 0;

                DeleteSupernaturalAttributeCommand.NotifyCanExecuteChanged();
            }
        }

        private bool CanDeleteSupernaturalAttribute() => CurrentSupernaturalAttributeList.Any() && CurrentSupernaturalAttributeIndex != -1;

        [RelayCommand(CanExecute = nameof(CanMoveSupernaturalAttributeUp))]
        private void MoveSupernaturalAttributeUp()
        {
            int index = CurrentSupernaturalAttributeIndex;
            int indexList1 = CharacterSelection.Attributes.IndexOf(CurrentSupernaturalAttributeList[index]);
            int indexList2 = CharacterSelection.Attributes.IndexOf(CurrentSupernaturalAttributeList[index - 1]);

            CurrentSupernaturalAttributeList.SwapCollectionItems(index, index - 1);
            CharacterSelection.Attributes.SwapListItems(indexList1, indexList2);
            CurrentSupernaturalAttributeIndex = index - 1;

            DeleteSupernaturalAttributeCommand.NotifyCanExecuteChanged();
            MoveSupernaturalAttributeUpCommand.NotifyCanExecuteChanged();
            MoveSupernaturalAttributeDownCommand.NotifyCanExecuteChanged();
        }

        private bool CanMoveSupernaturalAttributeUp() => CurrentSupernaturalAttributeIndex > 0;

        [RelayCommand(CanExecute = nameof(CanMoveSupernaturalAttributeDown))]
        private void MoveSupernaturalAttributeDown()
        {
            int index = CurrentSupernaturalAttributeIndex;
            int indexList1 = CharacterSelection.Attributes.IndexOf(CurrentSupernaturalAttributeList[index]);
            int indexList2 = CharacterSelection.Attributes.IndexOf(CurrentSupernaturalAttributeList[index + 1]);

            CurrentSupernaturalAttributeList.SwapCollectionItems(index, index + 1);
            CharacterSelection.Attributes.SwapListItems(indexList1, indexList2);
            CurrentSupernaturalAttributeIndex = index + 1;

            DeleteSupernaturalAttributeCommand.NotifyCanExecuteChanged();
            MoveSupernaturalAttributeUpCommand.NotifyCanExecuteChanged();
            MoveSupernaturalAttributeDownCommand.NotifyCanExecuteChanged();
        }

        private bool CanMoveSupernaturalAttributeDown() => CurrentSupernaturalAttributeIndex < CurrentSupernaturalAttributeList.Count - 1;

        [RelayCommand]
        private void RefreshSkills() => AttributeCalculationClass.SkillCalculation(CharacterSelection,
                                                                                   CharacterSelectionIndex,
                                                                                   LoadedBodyModSupplement,
                                                                                   LoadedSave.JumpList);

        [RelayCommand]
        private void NewPhysicalSkill()
        {
            ProfileAttribute skill = new($"Physical Skill{CurrentPhysicalSkillList.Count + 1}", "Skill", "Physical");

            CharacterSelection.Skills.Add(skill);
            CurrentPhysicalSkillList.Add(skill);

            CurrentPhysicalSkillIndex = CurrentPhysicalSkillList.Count - 1;

            DeletePhysicalSkillCommand.NotifyCanExecuteChanged();
        }

        [RelayCommand(CanExecute = nameof(CanDeletePhysicalSkill))]
        private void DeletePhysicalSkill()
        {
            if (_dialogService.ConfirmDialog("Are you sure you want to delete this Skill? " +
                "This action cannot be reversed and any purchase traits assigned to it will need to be reset manually."))
            {
                CharacterSelection.Skills.Remove(CurrentPhysicalSkillSelection);
                CurrentPhysicalSkillList.RemoveAt(CurrentPhysicalSkillIndex);

                CurrentPhysicalSkillIndex = 0;

                DeletePhysicalSkillCommand.NotifyCanExecuteChanged();
            }
        }

        private bool CanDeletePhysicalSkill() => CurrentPhysicalSkillList.Any() && CurrentPhysicalSkillIndex != -1;

        [RelayCommand(CanExecute = nameof(CanMovePhysicalSkillUp))]
        private void MovePhysicalSkillUp()
        {
            int index = CurrentPhysicalSkillIndex;
            int indexList1 = CharacterSelection.Skills.IndexOf(CurrentPhysicalSkillList[index]);
            int indexList2 = CharacterSelection.Skills.IndexOf(CurrentPhysicalSkillList[index - 1]);

            CurrentPhysicalSkillList.SwapCollectionItems(index, index - 1);
            CharacterSelection.Skills.SwapListItems(indexList1, indexList2);
            CurrentPhysicalSkillIndex = index - 1;

            DeletePhysicalSkillCommand.NotifyCanExecuteChanged();
            MovePhysicalSkillUpCommand.NotifyCanExecuteChanged();
            MovePhysicalSkillDownCommand.NotifyCanExecuteChanged();
        }

        private bool CanMovePhysicalSkillUp() => CurrentPhysicalSkillIndex > 0;

        [RelayCommand(CanExecute = nameof(CanMovePhysicalSkillDown))]
        private void MovePhysicalSkillDown()
        {
            int index = CurrentPhysicalSkillIndex;
            int indexList1 = CharacterSelection.Skills.IndexOf(CurrentPhysicalSkillList[index]);
            int indexList2 = CharacterSelection.Skills.IndexOf(CurrentPhysicalSkillList[index + 1]);

            CurrentPhysicalSkillList.SwapCollectionItems(index, index + 1);
            CharacterSelection.Skills.SwapListItems(indexList1, indexList2);
            CurrentPhysicalSkillIndex = index + 1;

            DeletePhysicalSkillCommand.NotifyCanExecuteChanged();
            MovePhysicalSkillUpCommand.NotifyCanExecuteChanged();
            MovePhysicalSkillDownCommand.NotifyCanExecuteChanged();
        }

        private bool CanMovePhysicalSkillDown() => CurrentPhysicalSkillIndex < CurrentPhysicalSkillList.Count - 1;

        [RelayCommand]
        private void NewMentalSkill()
        {
            ProfileAttribute skill = new($"Mental Skill{CurrentMentalSkillList.Count + 1}", "Skill", "Mental");

            CharacterSelection.Skills.Add(skill);
            CurrentMentalSkillList.Add(skill);

            CurrentMentalSkillIndex = CurrentMentalSkillList.Count - 1;

            DeleteMentalSkillCommand.NotifyCanExecuteChanged();
        }

        [RelayCommand(CanExecute = nameof(CanDeleteMentalSkill))]
        private void DeleteMentalSkill()
        {
            if (_dialogService.ConfirmDialog("Are you sure you want to delete this Skill? " +
                "This action cannot be reversed and any purchase traits assigned to it will need to be reset manually."))
            {
                CharacterSelection.Skills.Remove(CurrentMentalSkillSelection);
                CurrentMentalSkillList.RemoveAt(CurrentMentalSkillIndex);

                CurrentMentalSkillIndex = 0;

                DeleteMentalSkillCommand.NotifyCanExecuteChanged();
            }
        }

        private bool CanDeleteMentalSkill() => CurrentMentalSkillList.Any() && CurrentMentalSkillIndex != -1;

        [RelayCommand(CanExecute = nameof(CanMoveMentalSkillUp))]
        private void MoveMentalSkillUp()
        {
            int index = CurrentMentalSkillIndex;
            int indexList1 = CharacterSelection.Skills.IndexOf(CurrentMentalSkillList[index]);
            int indexList2 = CharacterSelection.Skills.IndexOf(CurrentMentalSkillList[index - 1]);

            CurrentMentalSkillList.SwapCollectionItems(index, index - 1);
            CharacterSelection.Skills.SwapListItems(indexList1, indexList2);
            CurrentMentalSkillIndex = index - 1;

            DeleteMentalSkillCommand.NotifyCanExecuteChanged();
            MoveMentalSkillUpCommand.NotifyCanExecuteChanged();
            MoveMentalSkillDownCommand.NotifyCanExecuteChanged();
        }

        private bool CanMoveMentalSkillUp() => CurrentMentalSkillIndex > 0;

        [RelayCommand(CanExecute = nameof(CanMoveMentalSkillDown))]
        private void MoveMentalSkillDown()
        {
            int index = CurrentMentalSkillIndex;
            int indexList1 = CharacterSelection.Skills.IndexOf(CurrentMentalSkillList[index]);
            int indexList2 = CharacterSelection.Skills.IndexOf(CurrentMentalSkillList[index + 1]);

            CurrentMentalSkillList.SwapCollectionItems(index, index + 1);
            CharacterSelection.Skills.SwapListItems(indexList1, indexList2);
            CurrentMentalSkillIndex = index + 1;

            DeleteMentalSkillCommand.NotifyCanExecuteChanged();
            MoveMentalSkillUpCommand.NotifyCanExecuteChanged();
            MoveMentalSkillDownCommand.NotifyCanExecuteChanged();
        }

        private bool CanMoveMentalSkillDown() => CurrentMentalSkillIndex < CurrentMentalSkillList.Count - 1;

        [RelayCommand]
        private void NewSocialSkill()
        {
            ProfileAttribute skill = new($"Social Skill{CurrentSocialSkillList.Count + 1}", "Skill", "Social");

            CharacterSelection.Skills.Add(skill);
            CurrentSocialSkillList.Add(skill);

            CurrentSocialSkillIndex = CurrentSocialSkillList.Count - 1;

            DeleteSocialSkillCommand.NotifyCanExecuteChanged();
        }

        [RelayCommand(CanExecute = nameof(CanDeleteSocialSkill))]
        private void DeleteSocialSkill()
        {
            if (_dialogService.ConfirmDialog("Are you sure you want to delete this Skill? " +
                "This action cannot be reversed and any purchase traits assigned to it will need to be reset manually."))
            {
                CharacterSelection.Skills.Remove(CurrentSocialSkillSelection);
                CurrentSocialSkillList.RemoveAt(CurrentSocialSkillIndex);

                CurrentSocialSkillIndex = 0;

                DeleteSocialSkillCommand.NotifyCanExecuteChanged();
            }
        }

        private bool CanDeleteSocialSkill()
        {
            return CurrentSocialSkillList.Any() && CurrentSocialSkillIndex != -1;
        }

        [RelayCommand(CanExecute = nameof(CanMoveSocialSkillUp))]
        private void MoveSocialSkillUp()
        {
            int index = CurrentSocialSkillIndex;
            int indexList1 = CharacterSelection.Skills.IndexOf(CurrentSocialSkillList[index]);
            int indexList2 = CharacterSelection.Skills.IndexOf(CurrentSocialSkillList[index - 1]);

            CurrentSocialSkillList.SwapCollectionItems(index, index - 1);
            CharacterSelection.Skills.SwapListItems(indexList1, indexList2);
            CurrentSocialSkillIndex = index - 1;

            DeleteSocialSkillCommand.NotifyCanExecuteChanged();
            MoveSocialSkillUpCommand.NotifyCanExecuteChanged();
            MoveSocialSkillDownCommand.NotifyCanExecuteChanged();
        }

        private bool CanMoveSocialSkillUp() => CurrentSocialSkillIndex > 0;

        [RelayCommand(CanExecute = nameof(CanMoveSocialSkillDown))]
        private void MoveSocialSkillDown()
        {
            int index = CurrentSocialSkillIndex;
            int indexList1 = CharacterSelection.Skills.IndexOf(CurrentSocialSkillList[index]);
            int indexList2 = CharacterSelection.Skills.IndexOf(CurrentSocialSkillList[index + 1]);

            CurrentSocialSkillList.SwapCollectionItems(index, index + 1);
            CharacterSelection.Skills.SwapListItems(indexList1, indexList2);
            CurrentSocialSkillIndex = index + 1;

            DeleteSocialSkillCommand.NotifyCanExecuteChanged();
            MoveSocialSkillUpCommand.NotifyCanExecuteChanged();
            MoveSocialSkillDownCommand.NotifyCanExecuteChanged();
        }

        private bool CanMoveSocialSkillDown() => CurrentSocialSkillIndex < CurrentSocialSkillList.Count - 1;

        [RelayCommand]
        private void NewTechnologicalSkill()
        {
            ProfileAttribute skill = new($"Technological Skill{CurrentTechnologicalSkillList.Count + 1}", "Skill", "Technological");

            CharacterSelection.Skills.Add(skill);
            CurrentTechnologicalSkillList.Add(skill);

            CurrentTechnologicalSkillIndex = CurrentTechnologicalSkillList.Count - 1;

            DeleteTechnologicalSkillCommand.NotifyCanExecuteChanged();
        }

        [RelayCommand(CanExecute = nameof(CanDeleteTechnologicalSkill))]
        private void DeleteTechnologicalSkill()
        {
            if (_dialogService.ConfirmDialog("Are you sure you want to delete this Skill? " +
                "This action cannot be reversed and any purchase traits assigned to it will need to be reset manually."))
            {
                CharacterSelection.Skills.Remove(CurrentTechnologicalSkillSelection);
                CurrentTechnologicalSkillList.RemoveAt(CurrentTechnologicalSkillIndex);

                CurrentTechnologicalSkillIndex = 0;

                DeleteTechnologicalSkillCommand.NotifyCanExecuteChanged();
            }
        }

        private bool CanDeleteTechnologicalSkill()
        {
            return CurrentTechnologicalSkillList.Any() && CurrentTechnologicalSkillIndex != -1;
        }

        [RelayCommand(CanExecute = nameof(CanMoveTechnologicalSkillUp))]
        private void MoveTechnologicalSkillUp()
        {
            int index = CurrentTechnologicalSkillIndex;
            int indexList1 = CharacterSelection.Skills.IndexOf(CurrentTechnologicalSkillList[index]);
            int indexList2 = CharacterSelection.Skills.IndexOf(CurrentTechnologicalSkillList[index - 1]);

            CurrentTechnologicalSkillList.SwapCollectionItems(index, index - 1);
            CharacterSelection.Skills.SwapListItems(indexList1, indexList2);
            CurrentTechnologicalSkillIndex = index - 1;

            DeleteTechnologicalSkillCommand.NotifyCanExecuteChanged();
            MoveTechnologicalSkillUpCommand.NotifyCanExecuteChanged();
            MoveTechnologicalSkillDownCommand.NotifyCanExecuteChanged();
        }

        private bool CanMoveTechnologicalSkillUp() => CurrentTechnologicalSkillIndex > 0;

        [RelayCommand(CanExecute = nameof(CanMoveTechnologicalSkillDown))]
        private void MoveTechnologicalSkillDown()
        {
            int index = CurrentTechnologicalSkillIndex;
            int indexList1 = CharacterSelection.Skills.IndexOf(CurrentTechnologicalSkillList[index]);
            int indexList2 = CharacterSelection.Skills.IndexOf(CurrentTechnologicalSkillList[index + 1]);

            CurrentTechnologicalSkillList.SwapCollectionItems(index, index + 1);
            CharacterSelection.Skills.SwapListItems(indexList1, indexList2);
            CurrentTechnologicalSkillIndex = index + 1;

            DeleteTechnologicalSkillCommand.NotifyCanExecuteChanged();
            MoveTechnologicalSkillUpCommand.NotifyCanExecuteChanged();
            MoveTechnologicalSkillDownCommand.NotifyCanExecuteChanged();
        }

        private bool CanMoveTechnologicalSkillDown() => CurrentTechnologicalSkillIndex < CurrentTechnologicalSkillList.Count - 1;

        [RelayCommand]
        private void NewSupernaturalSkill()
        {
            ProfileAttribute skill = new($"Supernatural Skill{CurrentSupernaturalSkillList.Count + 1}", "Skill", "Supernatural");

            CharacterSelection.Skills.Add(skill);
            CurrentSupernaturalSkillList.Add(skill);

            CurrentSupernaturalSkillIndex = CurrentSupernaturalSkillList.Count - 1;

            DeleteSupernaturalSkillCommand.NotifyCanExecuteChanged();
        }

        [RelayCommand(CanExecute = nameof(CanDeleteSupernaturalSkill))]
        private void DeleteSupernaturalSkill()
        {
            if (_dialogService.ConfirmDialog("Are you sure you want to delete this Skill? " +
                "This action cannot be reversed and any purchase traits assigned to it will need to be reset manually."))
            {
                CharacterSelection.Skills.Remove(CurrentSupernaturalSkillSelection);
                CurrentSupernaturalSkillList.RemoveAt(CurrentSupernaturalSkillIndex);

                CurrentSupernaturalSkillIndex = 0;

                DeleteSupernaturalSkillCommand.NotifyCanExecuteChanged();
            }
        }

        private bool CanDeleteSupernaturalSkill() => CurrentSupernaturalSkillList.Any() && CurrentSupernaturalSkillIndex != -1;

        [RelayCommand(CanExecute = nameof(CanMoveSupernaturalSkillUp))]
        private void MoveSupernaturalSkillUp()
        {
            int index = CurrentSupernaturalSkillIndex;
            int indexList1 = CharacterSelection.Skills.IndexOf(CurrentSupernaturalSkillList[index]);
            int indexList2 = CharacterSelection.Skills.IndexOf(CurrentSupernaturalSkillList[index - 1]);

            CurrentSupernaturalSkillList.SwapCollectionItems(index, index - 1);
            CharacterSelection.Skills.SwapListItems(indexList1, indexList2);
            CurrentSupernaturalSkillIndex = index - 1;

            DeleteSupernaturalSkillCommand.NotifyCanExecuteChanged();
            MoveSupernaturalSkillUpCommand.NotifyCanExecuteChanged();
            MoveSupernaturalSkillDownCommand.NotifyCanExecuteChanged();
        }

        private bool CanMoveSupernaturalSkillUp() => CurrentSupernaturalSkillIndex > 0;

        [RelayCommand(CanExecute = nameof(CanMoveSupernaturalSkillDown))]
        private void MoveSupernaturalSkillDown()
        {
            int index = CurrentSupernaturalSkillIndex;
            int indexList1 = CharacterSelection.Skills.IndexOf(CurrentSupernaturalSkillList[index]);
            int indexList2 = CharacterSelection.Skills.IndexOf(CurrentSupernaturalSkillList[index + 1]);

            CurrentSupernaturalSkillList.SwapCollectionItems(index, index + 1);
            CharacterSelection.Skills.SwapListItems(indexList1, indexList2);
            CurrentSupernaturalSkillIndex = index + 1;

            DeleteSupernaturalSkillCommand.NotifyCanExecuteChanged();
            MoveSupernaturalSkillUpCommand.NotifyCanExecuteChanged();
            MoveSupernaturalSkillDownCommand.NotifyCanExecuteChanged();
        }

        private bool CanMoveSupernaturalSkillDown() => CurrentSupernaturalSkillIndex < CurrentSupernaturalSkillList.Count - 1;

        [RelayCommand]
        private void NewBooster()
        {
            Booster booster = new($"Booster{CurrentBoosterList.Count + 1}");

            CharacterSelection.Boosters.Add(booster);
            CurrentBoosterList.Add(booster);

            CurrentBoosterIndex = CurrentBoosterList.Count - 1;

            foreach (Booster boosterEntry in CurrentBoosterList)
            {
                if (booster != boosterEntry)
                {
                    boosterEntry.BoosterDependencies.Add(new(booster));
                }
                else
                {
                    foreach (Booster boosterSecondEntry in CurrentBoosterList.Where(x => x != booster))
                    {
                        booster.BoosterDependencies.Add(new(boosterSecondEntry));
                    }
                }
            }

            DeleteBoosterCommand.NotifyCanExecuteChanged();
        }

        [RelayCommand(CanExecute = nameof(CanDeleteBooster))]
        private void DeleteBooster()
        {
            if (_dialogService.ConfirmDialog("Are you sure you want to delete this Booster? " +
                "This action cannot be reversed and any purchase traits assigned to it will need to be reset manually."))
            {
                int oldIndex = CurrentBoosterIndex;

                foreach (Booster boosterEntry in CurrentBoosterList)
                {
                    if (oldIndex < CurrentBoosterList.IndexOf(boosterEntry))
                    {
                        boosterEntry.BoosterDependencies.RemoveAt(oldIndex);
                    }
                    else if (oldIndex > CurrentBoosterList.IndexOf(boosterEntry))
                    {
                        boosterEntry.BoosterDependencies.RemoveAt(oldIndex - 1);
                    }
                }

                CharacterSelection.Boosters.Remove(CurrentBoosterSelection);
                CurrentBoosterList.RemoveAt(CurrentBoosterIndex);

                CurrentBoosterIndex = 0;

                DeleteBoosterCommand.NotifyCanExecuteChanged();
            }
        }

        private bool CanDeleteBooster() => CurrentBoosterList.Any() && CurrentBoosterIndex != -1;

        [RelayCommand]
        private void RefreshBoosters()
        {
            UpdateBoosterDependencyNames();

            AttributeCalculationClass.BoosterCalculation(CharacterSelection, CharacterSelectionIndex, LoadedBodyModSupplement, LoadedSave.JumpList);
        }
        #endregion
    }
}
