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
    // TODO - Implement ability to add a stipend for a specific purchase type regardless of currency chosen.
    public partial class JumpchainOverviewViewModel : ViewModelBase
    {
        #region Fields
        private readonly IDialogService _dialogService;

        [ObservableProperty]
        private SaveFile _loadedSave = new();
        [ObservableProperty]
        private Options _loadedOptions = new();
        [ObservableProperty]
        private AppSettingsModel _appSettings = new();
        [ObservableProperty]
        private bool _spellCheckEnabled = true;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteJumpCommand))]
        [NotifyCanExecuteChangedFor(nameof(MoveJumpUpCommand))]
        [NotifyCanExecuteChangedFor(nameof(MoveJumpDownCommand))]
        private Jump _jumpSelection = new();
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _jumpName = "";
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _jumpVersion = "";
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _jumpAuthor = "";
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _jumpSource = "";
        [ObservableProperty]
        private int _jumpSelectionIndex = 0;
        [ObservableProperty]
        private ObservableCollection<Jump> _jumpList = new();

        [ObservableProperty]
        private int _buildTabIndex = 0;

        [ObservableProperty]
        private Character _characterSelection = new();
        [ObservableProperty]
        private int _characterSelectionIndex = 0;
        [ObservableProperty]
        private ObservableCollection<Character> _characterList = new();

        [ObservableProperty]
        private int _budget = 1000;

        [ObservableProperty]
        private OriginDetail _originSelection = new();
        [ObservableProperty]
        private int _originSelectionIndex = 0;
        [ObservableProperty]
        private ObservableCollection<OriginDetail> _originList = new();
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteOriginCommand))]
        private OriginDetail _originEditSelection = new();
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _originEditName = "";
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _originEditDescription = "";
        [ObservableProperty]
        private int _originEditSelectionIndex = 0;

        [ObservableProperty]
        private OriginDetail _miscOriginDetailSelection = new();
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _miscOriginDetailName = "";
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _miscOriginDetailDescription = "";
        [ObservableProperty]
        private int _miscOriginDetailIndex = 0;
        [ObservableProperty]
        private ObservableCollection<OriginDetail> _miscOriginDetailList = new();
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteTypeCommand))]
        private string _miscOriginTypeSelection = "";
        [ObservableProperty]
        private int _miscOriginTypeSelectionIndex = 0;
        [ObservableProperty]
        private ObservableCollection<string> _miscOriginTypeList = new();

        [ObservableProperty]
        private bool _originDiscountsSelection = false;
        [ObservableProperty]
        private int _perkFreebieThreshold = 100;
        [ObservableProperty]
        private int _itemFreebieThreshold = 100;
        [ObservableProperty]
        private bool _skipJumpNumber = false;

        [ObservableProperty]
        private Currency _currencySelection = new();
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _currencyName = "";
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _currencyAbbreviation = "";
        [ObservableProperty]
        private int _currencySelectionIndex = 0;
        [ObservableProperty]
        private ObservableCollection<Currency> _currencyList = new();

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeletePurchaseTypeCommand))]
        private PurchaseType _purchaseTypeSelection = new();
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _purchaseTypeName = "";
        [ObservableProperty]
        private int _purchaseTypeSelectionIndex = 0;
        [ObservableProperty]
        private ObservableCollection<PurchaseType> _purchaseTypeList = new();
        [ObservableProperty]
        private Currency _purchaseTypeCurrencySelection = new();
        [ObservableProperty]
        private int _purchaseTypeCurrencyIndex = 0;
        [ObservableProperty]
        private bool _isItemType = false;

        [ObservableProperty]
        private int _stipendSelectionIndex = 0;
        [ObservableProperty]
        private int _currencyStipend = 0;
        [ObservableProperty]
        private int _itemStipend = 0;

        [ObservableProperty]
        private ObservableCollection<ObservableCollection<Purchase>> _purchaseList = new();
        [ObservableProperty]
        private ObservableCollection<Purchase> _currentLoadedPurchaseList = new();
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeletePurchaseCommand))]
        [NotifyCanExecuteChangedFor(nameof(DeletePurchaseTraitCommand))]
        [NotifyCanExecuteChangedFor(nameof(MovePurchaseUpCommand))]
        [NotifyCanExecuteChangedFor(nameof(MovePurchaseDownCommand))]
        private Purchase _purchaseSelection = new();
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _purchaseName = "";
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _purchaseDescription = "";
        [ObservableProperty]
        private int _purchaseSelectionIndex = 0;
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteCurrencyCommand))]
        private string _categorySelection = "";
        [ObservableProperty]
        private ObservableCollection<string> _categoryList = new();
        [ObservableProperty]
        private int _purchaseOriginIndex = 0;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteDrawbackCommand))]
        [NotifyCanExecuteChangedFor(nameof(MoveDrawbackUpCommand))]
        [NotifyCanExecuteChangedFor(nameof(MoveDrawbackDownCommand))]
        private Drawback _drawbackSelection = new();
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _drawbackName = "";
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _drawbackDescription = "";
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _drawbackReward = "";
        [ObservableProperty]
        private int _drawbackSelectionIndex = 0;
        [ObservableProperty]
        private ObservableCollection<Drawback> _drawbackList = new();

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteScenarioCommand))]
        [NotifyCanExecuteChangedFor(nameof(MoveScenarioUpCommand))]
        [NotifyCanExecuteChangedFor(nameof(MoveScenarioDownCommand))]
        private Drawback _scenarioSelection = new();
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _scenarioName = "";
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _scenarioDescription = "";
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _scenarioReward = "";
        [ObservableProperty]
        private int _scenarioSelectionIndex = 0;
        [ObservableProperty]
        private ObservableCollection<Drawback> _scenarioList = new();

        [ObservableProperty]
        private CompanionPurchase _importOptionSelection = new();
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _importOptionName = "";
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _importOptionDescription = "";
        [ObservableProperty]
        private int _importOptionSelectionIndex = 0;
        [ObservableProperty]
        private ObservableCollection<CompanionPurchase> _importOptionList = new();

        [ObservableProperty]
        private CompanionImportDetailClass _importCharacterSelection = new();
        [ObservableProperty]
        private int _importCharacterIndex = 0;
        [ObservableProperty]
        private ObservableCollection<CompanionImportDetailClass> _importCharacterList = new();

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Range(0, int.MaxValue, ErrorMessage = "Age must be a positive number.")]
        private int _age = 0;
        [ObservableProperty]
        private int _ageCost = 0;
        [ObservableProperty]
        private string _gender = "";
        [ObservableProperty]
        private int _genderCost = 0;

        [ObservableProperty]
        private Options.DrawbackSupplements _loadedDrawbackSupplement = Options.DrawbackSupplements.Generic;

        [ObservableProperty]
        private Options.CosmicWarehouseSupplements _loadedWarehouseSupplement = Options.CosmicWarehouseSupplements.Generic;

        [ObservableProperty]
        private Options.BodyModSupplements _loadedBodyModSupplement = Options.BodyModSupplements.Generic;

        [ObservableProperty]
        private bool _warehouseInvestmentAllowed = false;
        [ObservableProperty]
        private int _warehouseInvestmentRatio = 1;
        [ObservableProperty]
        private int _warehousePointsInvested = 0;
        [ObservableProperty]
        private int _warehousePointsGained = 0;
        [ObservableProperty]
        private bool _pRUnlimitedSelected = false;
        [ObservableProperty]
        private int _pRUnlimitedRemainingInvestment = 0;

        [ObservableProperty]
        private bool _bodyModInvestmentAllowed = false;
        [ObservableProperty]
        private int _bodyModInvestmentRatio = 1;
        [ObservableProperty]
        private int _bodyModPointsInvested = 0;
        [ObservableProperty]
        private int _bodyModPointsGained = 0;

        [ObservableProperty]
        private bool _isGauntlet = false;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Range(0, int.MaxValue, ErrorMessage = "Banked points must be a positive integer")]
        private int _bankedPoints = 0;
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Range(0, int.MaxValue, ErrorMessage = "Withdrawn points must be a positive integer")]
        private int _bankUsage = 0;

        [ObservableProperty]
        private bool _pointBankAllowed = false;

        [ObservableProperty]
        private int _currentBankTotal = 0;

        [ObservableProperty]
        private ObservableCollection<PurchaseAttribute> _purchaseAttributeList = new();
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeletePurchaseTraitCommand))]
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
        #endregion

        #region Properties
        partial void OnLoadedSaveChanged(SaveFile value) => LoadedOptions = value.Options;

        partial void OnJumpSelectionChanged(Jump value)
        {
            if (value != null)
            {
                LoadJumpSelection();
            }
        }

        partial void OnJumpListChanged(ObservableCollection<Jump> value)
        {
            if (JumpList.Count > 0)
            {
                JumpSelection = AppSettings.FocusNewestJumpOnLoad ? JumpList.Last() : JumpList.First();
            }
            else
            {
                NewJump();
            }
        }

        partial void OnBuildTabIndexChanged(int value) => BuildTabChanged();

        partial void OnCharacterSelectionChanged(Character value)
        {
            if (value != null)
            {
                ListValidationClass.CheckBuildCount(JumpSelection, CharacterSelectionIndex);

                LoadOriginSelection();
                LoadMiscOriginList();

                if (BuildTabIndex == 0 || BuildTabIndex == 3 || BuildTabIndex == 4)
                {
                    Budget = SetBudget(JumpSelection.PurchaseTypes[0]);
                }

                ListValidationClass.CheckStipendCount(JumpSelection, CharacterSelectionIndex);
                if (StipendSelectionIndex >= 0)
                {
                    if (StipendSelectionIndex != CurrencySelectionIndex)
                    {
                        StipendSelectionIndex = CurrencySelectionIndex;
                    }
                    else
                    {
                        CurrencyStipend = JumpSelection.Build[CharacterSelectionIndex].PointStipend[StipendSelectionIndex];
                    }
                }
                ItemStipend = JumpSelection.Build[CharacterSelectionIndex].ItemStipend;

                LoadAllPurchaseLists();
                LoadImportOptionList();

                LoadDrawbackList();
                LoadScenarioList();

                if (CharacterSelectionIndex == 0)
                {
                    if (LoadedOptions.AllowPointBank)
                    {
                        if (!JumpSelection.IsGauntlet || LoadedOptions.AllowGauntletBank)
                        {
                            PointBankAllowed = true;
                        }
                        else
                        {
                            PointBankAllowed = false;
                            JumpSelection.Build[CharacterSelectionIndex].BankedPoints = 0;
                            JumpSelection.Build[CharacterSelectionIndex].BankUsage = 0;
                        }
                    }
                    else
                    {
                        PointBankAllowed = false;
                        JumpSelection.Build[CharacterSelectionIndex].BankedPoints = 0;
                        JumpSelection.Build[CharacterSelectionIndex].BankUsage = 0;
                    }
                }
                else
                {
                    if (LoadedOptions.AllowCompanionsBank)
                    {
                        if (!JumpSelection.IsGauntlet || LoadedOptions.AllowGauntletBank)
                        {
                            PointBankAllowed = true;
                        }
                        else
                        {
                            PointBankAllowed = false;
                            JumpSelection.Build[CharacterSelectionIndex].BankedPoints = 0;
                            JumpSelection.Build[CharacterSelectionIndex].BankUsage = 0;
                        }
                    }
                    else
                    {
                        PointBankAllowed = false;
                        JumpSelection.Build[CharacterSelectionIndex].BankedPoints = 0;
                        JumpSelection.Build[CharacterSelectionIndex].BankUsage = 0;
                    }
                }

                BankedPoints = JumpSelection.Build[CharacterSelectionIndex].BankedPoints;
                BankUsage = JumpSelection.Build[CharacterSelectionIndex].BankUsage;

                LoadCurrentPointBank();
                LoadBodyModInvestment();
            }
        }

        partial void OnCharacterListChanged(ObservableCollection<Character> value) => CharacterSelection = CharacterList[0];

        partial void OnOriginSelectionChanged(OriginDetail value)
        {
            if (BuildTabIndex == 0)
            {
                Budget = SetBudget(JumpSelection.PurchaseTypes[0]);
            }
        }

        partial void OnOriginEditSelectionChanged(OriginDetail value)
        {
            if (value != null)
            {
                OriginEditName = value.Name;
                OriginEditDescription = value.Description;
            }
        }

        partial void OnOriginSelectionIndexChanged(int value)
        {
            if (OriginSelectionIndex != -1)
            {
                JumpSelection.Build[CharacterSelectionIndex].OriginIndex = value;
            }
        }


        partial void OnMiscOriginDetailSelectionChanged(OriginDetail value)
        {
            Budget = SetBudget(JumpSelection.PurchaseTypes[0]);

            if (value != null)
            {
                MiscOriginDetailName = value.Name;
                MiscOriginDetailDescription = value.Description;
            }
        }

        partial void OnCurrencySelectionChanged(Currency value)
        {
            if (value != null)
            {
                CurrencyName = value.CurrencyName;
                CurrencyAbbreviation = value.CurrencyAbbreviation;
            }
        }

        partial void OnMiscOriginTypeSelectionChanged(string value)
        {
            if (MiscOriginTypeList.Any())
            {
                if (value != null && value != MiscOriginTypeList[MiscOriginTypeSelectionIndex])
                {
                    JumpSelection.MiscOriginCategories[MiscOriginTypeSelectionIndex] = value;
                    LoadMiscOriginTypes();
                }
            }
        }

        partial void OnOriginDiscountsSelectionChanged(bool value) => JumpSelection.OriginDiscounts = value;

        partial void OnPerkFreebieThresholdChanged(int value) => JumpSelection.OriginPerkFreebieThreshold = value;

        partial void OnItemFreebieThresholdChanged(int value) => JumpSelection.OriginItemFreebieThreshold = value;

        partial void OnSkipJumpNumberChanged(bool value)
        {
            JumpSelection.SkipNumber = value;
            CalculateJumpNumber(LoadedSave.JumpList);
        }

        partial void OnPurchaseTypeSelectionChanged(PurchaseType value)
        {
            if (value != null)
            {
                if (PurchaseTypeCurrencyIndex != PurchaseTypeSelection.CurrencyIndex)
                {
                    PurchaseTypeCurrencySelection = CurrencyList[PurchaseTypeSelection.CurrencyIndex];
                }

                LoadPurchaseCategoryList();
                LoadCurrentPurchaseList();

                IsItemType = PurchaseTypeSelection.IsItemType;

                PurchaseTypeName = value.Type;
            }
        }

        partial void OnPurchaseTypeCurrencyIndexChanged(int value)
        {
            if (value >= 0 && PurchaseTypeSelectionIndex >= 2)
            {
                PurchaseTypeSelection.CurrencyIndex = value;
            }
        }

        partial void OnIsItemTypeChanged(bool value) => PurchaseTypeSelection.IsItemType = value;

        partial void OnStipendSelectionIndexChanged(int value)
        {
            ListValidationClass.CheckStipendCount(JumpSelection, CharacterSelectionIndex);

            if (value >= 0)
            {
                CurrencyStipend = JumpSelection.Build[CharacterSelectionIndex].PointStipend[value];
            }
        }

        partial void OnCurrencyStipendChanged(int value)
        {
            if (StipendSelectionIndex != -1)
            {
                JumpSelection.Build[CharacterSelectionIndex].PointStipend[StipendSelectionIndex] = value;
            }
        }

        partial void OnItemStipendChanged(int value)
        {
            if (value != JumpSelection.Build[CharacterSelectionIndex].ItemStipend)
            {
                JumpSelection.Build[CharacterSelectionIndex].ItemStipend = value;
            }
        }

        partial void OnPurchaseSelectionChanged(Purchase value)
        {
            Budget = SetBudget(PurchaseTypeSelection);

            if (value != null)
            {
                LoadCurrentPurchaseData();
            }
            else
            {
                ClearAttributeList();
            }
        }

        partial void OnPurchaseOriginIndexChanged(int value)
        {
            if (value != -1 && value != PurchaseSelection.AssociatedOriginIndex)
            {
                PurchaseSelection.AssociatedOriginIndex = value;
            }
        }

        partial void OnCategorySelectionChanged(string value)
        {
            if (PurchaseSelection != null && value != "" && value != null)
            {
                PurchaseSelection.Category = value;
            }
        }

        partial void OnDrawbackSelectionChanged(Drawback value)
        {
            Budget = SetBudget(JumpSelection.PurchaseTypes[0]);

            if (value != null)
            {
                DrawbackName = value.Name;
                DrawbackDescription = value.Description;
                DrawbackReward = value.Reward;
            }

        }


        partial void OnScenarioSelectionChanged(Drawback value)
        {
            Budget = SetBudget(JumpSelection.PurchaseTypes[0]);

            if (value != null)
            {
                ScenarioName = value.Name;
                ScenarioDescription = value.Description;
                ScenarioReward = value.Reward;
            }

        }

        partial void OnImportOptionSelectionChanged(CompanionPurchase value)
        {
            if (value != null)
            {
                LoadImportCharacterList();

                ImportOptionName = value.Name;
                ImportOptionDescription = value.Description;
            }
        }

        partial void OnAgeChanged(int value)
        {
            if (!GetErrors(nameof(Age)).Any())
            {
                JumpSelection.Build[CharacterSelectionIndex].Age = value;
            }
        }

        partial void OnAgeCostChanged(int value)
        {
            JumpSelection.Build[CharacterSelectionIndex].AgeCost = value;
            Budget = SetBudget(JumpSelection.PurchaseTypes[0]);
        }

        partial void OnGenderChanged(string value) => JumpSelection.Build[CharacterSelectionIndex].Gender = value;

        partial void OnGenderCostChanged(int value)
        {
            JumpSelection.Build[CharacterSelectionIndex].GenderCost = value;
            Budget = SetBudget(JumpSelection.PurchaseTypes[0]);
        }

        partial void OnLoadedWarehouseSupplementChanged(Options.CosmicWarehouseSupplements value) => LoadWarehouseInvestment();

        partial void OnLoadedBodyModSupplementChanged(Options.BodyModSupplements value) => LoadBodyModInvestment();

        partial void OnWarehousePointsInvestedChanged(int value)
        {
            if (LoadedWarehouseSupplement == Options.CosmicWarehouseSupplements.PersonalReality &&
                LoadedSave.PersonalReality.CoreMode == PersonalReality.CoreModes.Unlimited)
            {
                int unlimitedModeSubtotal = value;
                int postUnlimitedSubtotal = 0;

                if (unlimitedModeSubtotal > 100)
                {
                    postUnlimitedSubtotal = unlimitedModeSubtotal - 100;
                    unlimitedModeSubtotal = 100;
                }

                WarehousePointsGained = unlimitedModeSubtotal + (postUnlimitedSubtotal / WarehouseInvestmentRatio);
                JumpSelection.Build[0].WarehouseInvestment = value;
                PRUnlimitedRemainingInvestment = 100 - unlimitedModeSubtotal;
            }
            else
            {
                WarehousePointsGained = value / WarehouseInvestmentRatio;
                JumpSelection.Build[0].WarehouseInvestment = value;
            }
        }

        partial void OnBodyModPointsInvestedChanged(int value)
        {
            BodyModPointsGained = value / BodyModInvestmentRatio;

            if (LoadedBodyModSupplement == Options.BodyModSupplements.EssentialBodyMod && LoadedSave.EssentialBodyMod.LimitInvestment)
            {
                int jumpInvestmentTotal = 0;

                if (LoadedSave.EssentialBodyMod.EPAccessMode == EssentialBodyMod.EPAccessModes.LesserAccess)
                {
                    switch (LoadedSave.EssentialBodyMod.EPAccessModifier)
                    {
                        case EssentialBodyMod.EPAccessModifiers.None:
                            if (BodyModPointsGained > 50)
                            {
                                BodyModPointsInvested = 50 * BodyModInvestmentRatio;
                            }
                            break;
                        case EssentialBodyMod.EPAccessModifiers.Cumulative:
                            int currentJumpIndex = JumpList.IndexOf(JumpSelection);
                            List<Jump> jumps = JumpList.Where(x => (x.JumpNumber >= CharacterSelection.BodyMod.SupplementDelay && JumpList.IndexOf(x) < currentJumpIndex)).ToList();

                            foreach (Jump jump in jumps)
                            {
                                ListValidationClass.CheckBuildCount(jump, CharacterSelectionIndex);

                                jumpInvestmentTotal += 50;
                                jumpInvestmentTotal -= jump.Build[CharacterSelectionIndex].BodyModInvestment / BodyModInvestmentRatio;
                            }

                            jumpInvestmentTotal += 50;

                            if (jumpInvestmentTotal < BodyModPointsGained)
                            {
                                BodyModPointsInvested = jumpInvestmentTotal * BodyModInvestmentRatio;
                            }
                            break;
                        case EssentialBodyMod.EPAccessModifiers.RetroCumulative:
                            foreach (Jump jump in JumpList)
                            {
                                jumpInvestmentTotal += 50;

                                if (jump == JumpSelection)
                                {
                                    break;
                                }

                                ListValidationClass.CheckBuildCount(jump, CharacterSelectionIndex);

                                jumpInvestmentTotal -= jump.Build[CharacterSelectionIndex].BodyModInvestment / BodyModInvestmentRatio;
                            }

                            if (jumpInvestmentTotal < BodyModPointsGained)
                            {
                                BodyModPointsInvested = jumpInvestmentTotal * BodyModInvestmentRatio;
                            }
                            break;
                        default:
                            break;
                    }
                }
                else if (LoadedSave.EssentialBodyMod.EPAccessMode == EssentialBodyMod.EPAccessModes.StandardAccess)
                {
                    switch (LoadedSave.EssentialBodyMod.EPAccessModifier)
                    {
                        case EssentialBodyMod.EPAccessModifiers.None:
                            if (BodyModPointsGained > 100)
                            {
                                BodyModPointsInvested = 100 * BodyModInvestmentRatio;
                            }
                            break;
                        case EssentialBodyMod.EPAccessModifiers.Cumulative:
                            int currentJumpIndex = JumpList.IndexOf(JumpSelection);
                            List<Jump> jumps = JumpList.Where(x => (x.JumpNumber >= CharacterSelection.BodyMod.SupplementDelay && JumpList.IndexOf(x) < currentJumpIndex)).ToList();

                            foreach (Jump jump in jumps)
                            {
                                ListValidationClass.CheckBuildCount(jump, CharacterSelectionIndex);

                                jumpInvestmentTotal += 100;
                                jumpInvestmentTotal -= jump.Build[CharacterSelectionIndex].BodyModInvestment / BodyModInvestmentRatio;
                            }

                            jumpInvestmentTotal += 100;

                            if (jumpInvestmentTotal < BodyModPointsGained)
                            {
                                BodyModPointsInvested = jumpInvestmentTotal * BodyModInvestmentRatio;
                            }
                            break;
                        case EssentialBodyMod.EPAccessModifiers.RetroCumulative:
                            foreach (Jump jump in JumpList)
                            {
                                jumpInvestmentTotal += 100;

                                if (jump == JumpSelection)
                                {
                                    break;
                                }

                                ListValidationClass.CheckBuildCount(jump, CharacterSelectionIndex);

                                jumpInvestmentTotal -= jump.Build[CharacterSelectionIndex].BodyModInvestment / BodyModInvestmentRatio;
                            }

                            if (jumpInvestmentTotal < BodyModPointsGained)
                            {
                                BodyModPointsInvested = jumpInvestmentTotal * BodyModInvestmentRatio;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }

            JumpSelection.Build[CharacterSelectionIndex].BodyModInvestment = BodyModPointsInvested;
        }

        partial void OnIsGauntletChanged(bool value) => JumpSelection.IsGauntlet = value;

        partial void OnBankedPointsChanged(int value)
        {
            if (!GetErrors(nameof(BankedPoints)).Any())
            {
                if (value > LoadedOptions.PointBankLimit)
                {
                    BankedPoints = LoadedOptions.PointBankLimit;
                }
                else
                {
                    JumpSelection.Build[CharacterSelectionIndex].BankedPoints = value;
                    Budget = SetBudget(JumpSelection.PurchaseTypes[0]);
                }

                LoadCurrentPointBank();
            }
        }

        partial void OnBankUsageChanged(int value)
        {
            if (!GetErrors(nameof(BankUsage)).Any())
            {
                if (value > LoadedOptions.PointBankLimit)
                {
                    BankUsage = LoadedOptions.PointBankLimit;
                }
                else
                {
                    if (value > CurrentBankTotal + JumpSelection.Build[CharacterSelectionIndex].BankUsage)
                    {
                        BankUsage = CurrentBankTotal + JumpSelection.Build[CharacterSelectionIndex].BankUsage;
                    }
                    else
                    {
                        JumpSelection.Build[CharacterSelectionIndex].BankUsage = value;
                        Budget = SetBudget(JumpSelection.PurchaseTypes[0]);
                    }
                }

                LoadCurrentPointBank();
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
            if (PurchaseAttributeSelection != null && value != null && value != "")
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
            if (PurchaseAttributeSelection != null && value != null && value != "")
            {
                PurchaseAttributeSelection.Category = value;

                LoadCurrentAttributeList();
            }
        }

        partial void OnCurrentAttributeSelectionChanged(string value)
        {
            if (PurchaseAttributeSelection != null && value != null && value != "")
            {
                PurchaseAttributeSelection.Name = value;
            }
        }

        partial void OnJumpNameChanged(string value)
        {
            if (!GetErrors(nameof(JumpName)).Any() && JumpSelection != null)
            {
                JumpSelection.Name = value;
            }
        }

        partial void OnJumpAuthorChanged(string value)
        {
            if (!GetErrors(nameof(JumpAuthor)).Any() && JumpSelection != null)
            {
                JumpSelection.Author = value;
            }
        }

        partial void OnJumpVersionChanged(string value)
        {
            if (!GetErrors(nameof(JumpVersion)).Any() && JumpSelection != null)
            {
                JumpSelection.Version = value;
            }
        }

        partial void OnJumpSourceChanged(string value)
        {
            if (!GetErrors(nameof(JumpSource)).Any() && JumpSelection != null)
            {
                JumpSelection.Source = value;
            }
        }

        partial void OnOriginEditNameChanged(string value)
        {
            if (!GetErrors(nameof(OriginEditName)).Any() && OriginEditSelection != null)
            {
                OriginEditSelection.Name = value;
            }
        }

        partial void OnOriginEditDescriptionChanged(string value)
        {
            if (!GetErrors(nameof(OriginEditDescription)).Any() && OriginEditSelection != null)
            {
                OriginEditSelection.Description = value;
            }
        }

        partial void OnMiscOriginDetailNameChanged(string value)
        {
            if (!GetErrors(nameof(MiscOriginDetailName)).Any() && MiscOriginDetailSelection != null)
            {
                MiscOriginDetailSelection.Name = value;
            }
        }

        partial void OnMiscOriginDetailDescriptionChanged(string value)
        {
            if (!GetErrors(nameof(MiscOriginDetailDescription)).Any() && MiscOriginDetailSelection != null)
            {
                MiscOriginDetailSelection.Description = value;
            }
        }

        partial void OnCurrencyNameChanged(string value)
        {
            if (!GetErrors(nameof(CurrencyName)).Any() && CurrencySelection != null)
            {
                CurrencySelection.CurrencyName = value;
            }
        }

        partial void OnCurrencyAbbreviationChanged(string value)
        {
            if (!GetErrors(nameof(CurrencyAbbreviation)).Any() && CurrencySelection != null)
            {
                CurrencySelection.CurrencyAbbreviation = value;
            }
        }

        partial void OnPurchaseTypeNameChanged(string value)
        {
            if (!GetErrors(nameof(PurchaseTypeName)).Any() && PurchaseTypeSelection != null)
            {
                PurchaseTypeSelection.Type = value;
            }
        }

        partial void OnPurchaseNameChanged(string value)
        {
            if (!GetErrors(nameof(PurchaseName)).Any() && PurchaseSelection != null)
            {
                PurchaseSelection.Name = value;
            }
        }

        partial void OnPurchaseDescriptionChanged(string value)
        {
            if (!GetErrors(nameof(PurchaseDescription)).Any() && PurchaseSelection != null)
            {
                PurchaseSelection.Description = value;
            }
        }

        partial void OnDrawbackNameChanged(string value)
        {
            if (!GetErrors(nameof(DrawbackName)).Any() && DrawbackSelection != null)
            {
                DrawbackSelection.Name = value;
            }
        }

        partial void OnDrawbackDescriptionChanged(string value)
        {
            if (!GetErrors(nameof(DrawbackDescription)).Any() && DrawbackSelection != null)
            {
                DrawbackSelection.Description = value;
            }
        }

        partial void OnDrawbackRewardChanged(string value)
        {
            if (!GetErrors(nameof(DrawbackReward)).Any() && DrawbackSelection != null)
            {
                DrawbackSelection.Reward = value;
            }
        }

        partial void OnScenarioNameChanged(string value)
        {
            if (!GetErrors(nameof(ScenarioName)).Any() && ScenarioSelection != null)
            {
                ScenarioSelection.Name = value;
            }
        }

        partial void OnScenarioDescriptionChanged(string value)
        {
            if (!GetErrors(nameof(ScenarioDescription)).Any() && ScenarioSelection != null)
            {
                ScenarioSelection.Description = value;
            }
        }

        partial void OnScenarioRewardChanged(string value)
        {
            if (!GetErrors(nameof(ScenarioReward)).Any() && ScenarioSelection != null)
            {
                ScenarioSelection.Reward = value;
            }
        }

        partial void OnImportOptionNameChanged(string value)
        {
            if (!GetErrors(nameof(ImportOptionName)).Any() && ImportOptionSelection != null)
            {
                ImportOptionSelection.Name = value;
            }
        }

        partial void OnImportOptionDescriptionChanged(string value)
        {
            if (!GetErrors(nameof(ImportOptionDescription)).Any() && ImportOptionSelection != null)
            {
                ImportOptionSelection.Description = value;
            }
        }
        #endregion

        public JumpchainOverviewViewModel()
        {

        }

        public JumpchainOverviewViewModel(IDialogService dialogService)
        {
            Messenger.Register<SaveDataChangedMessage>(this, (r, m) =>
            {
                LoadedOptions = LoadedSave.Options;

                LoadedDrawbackSupplement = LoadedOptions.DrawbackSupplementSetting;
                LoadedWarehouseSupplement = LoadedOptions.CosmicWarehouseSetting;
                if (LoadedWarehouseSupplement == Options.CosmicWarehouseSupplements.PersonalReality &&
                    LoadedSave.PersonalReality.CoreMode == PersonalReality.CoreModes.Unlimited)
                {
                    PRUnlimitedSelected = true;
                }
                else
                {
                    PRUnlimitedSelected = false;
                }

                LoadedBodyModSupplement = LoadedOptions.BodyModSetting;

                CreateJumpList();
                CreateCharacterList();

                if (JumpList.Any())
                {
                    JumpSelection = AppSettings.FocusNewestJumpOnLoad ? JumpList.Last() : JumpList.First();
                    LoadJumpSelection();
                }

                ClearAttributeList();
                LoadAttributeTypes();
            });
            Messenger.Register<SaveDataSendMessage>(this, (r, m) =>
            {
                LoadedSave = m.Value;

                CreateJumpList();

                ClearAttributeList();
                LoadAttributeTypes();
            });
            Messenger.Register<SupplementChangedMessage>(this, (r, m) =>
            {
                if (m.Value == "Drawback")
                {
                    LoadedDrawbackSupplement = LoadedOptions.DrawbackSupplementSetting;
                }
                else if (m.Value == "Warehouse")
                {
                    LoadedWarehouseSupplement = LoadedOptions.CosmicWarehouseSetting;

                    if (LoadedWarehouseSupplement == Options.CosmicWarehouseSupplements.PersonalReality &&
                    LoadedSave.PersonalReality.CoreMode == PersonalReality.CoreModes.Unlimited)
                    {
                        PRUnlimitedSelected = true;
                    }
                    else
                    {
                        PRUnlimitedSelected = false;
                    }
                }
                else if (m.Value == "Body Mod")
                {
                    LoadedBodyModSupplement = LoadedOptions.BodyModSetting;
                }
            });
            Messenger.Register<CharacterDeletedMessage>(this, (r, m) =>
            {
                CreateCharacterList();
                JumpSelection = JumpList.Last();

                ClearAttributeList();
                LoadAttributeTypes();
            });
            Messenger.Register<CategoryChangedMessage>(this, (r, m) =>
            {
                LoadPurchaseCategoryList();
                LoadCurrentPurchaseData();
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

            CreateJumpList();

            ClearAttributeList();
            LoadAttributeTypes();

            _dialogService = dialogService;
        }

        #region Methods
        public void CreateJumpList()
        {
            JumpList.Clear();

            foreach (Jump jump in LoadedSave.JumpList)
            {
                JumpList.Add(jump);
            }

            CalculateJumpNumber(LoadedSave.JumpList);

            JumpSelectionIndex = AppSettings.FocusNewestJumpOnLoad ? JumpList.Count - 1 : 0;
        }

        private static void CalculateJumpNumber(List<Jump> jumps)
        {
            int currentNumber = 1;

            foreach (Jump jump in jumps)
            {
                if (!jump.SkipNumber)
                {
                    jump.JumpNumber = currentNumber;
                    currentNumber++;
                }
                else
                {
                    jump.JumpNumber = currentNumber - 1;
                }
            }
        }

        private void CreateCharacterList()
        {
            CharacterList.Clear();

            foreach (Character character in LoadedSave.CharacterList)
            {
                CharacterList.Add(character);
            }

            if (CharacterList.Any())
            {
                CharacterSelectionIndex = 0;
                CharacterSelection = CharacterList.First();
            }
        }

        private void LoadJumpSelection()
        {
            CharacterSelectionIndex = 0;

            LoadMiscOriginTypes();
            LoadCurrencies();
            CreateOriginList();
            LoadPurchaseTypes();
            LoadAllPurchaseLists();
            LoadDrawbackList();
            LoadScenarioList();
            LoadImportOptionList();
            LoadImportCharacterList();
            CreateCharacterList();
            LoadWarehouseInvestment();
            LoadBodyModInvestment();

            OriginDiscountsSelection = JumpSelection.OriginDiscounts;
            PerkFreebieThreshold = JumpSelection.OriginPerkFreebieThreshold;
            ItemFreebieThreshold = JumpSelection.OriginItemFreebieThreshold;
            SkipJumpNumber = JumpSelection.SkipNumber;
            IsGauntlet = JumpSelection.IsGauntlet;

            JumpName = JumpSelection.Name;
            JumpVersion = JumpSelection.Version;
            JumpAuthor = JumpSelection.Author;
            JumpSource = JumpSelection.Source;

            if (LoadedOptions.AllowPointBank)
            {
                if (!JumpSelection.IsGauntlet || LoadedOptions.AllowGauntletBank)
                {
                    PointBankAllowed = true;
                }
                else
                {
                    PointBankAllowed = false;
                    JumpSelection.Build[CharacterSelectionIndex].BankedPoints = 0;
                    JumpSelection.Build[CharacterSelectionIndex].BankUsage = 0;
                }
            }
            else
            {
                PointBankAllowed = false;
                JumpSelection.Build[CharacterSelectionIndex].BankedPoints = 0;
                JumpSelection.Build[CharacterSelectionIndex].BankUsage = 0;
            }

            BankedPoints = JumpSelection.Build[CharacterSelectionIndex].BankedPoints;
            BankUsage = JumpSelection.Build[CharacterSelectionIndex].BankUsage;

            LoadCurrentPointBank();

            Budget = SetBudget(JumpSelection.PurchaseTypes[0]);

            BuildTabIndex = 0;
        }

        private void CreateOriginList()
        {
            OriginList.Clear();

            foreach (OriginDetail origin in JumpSelection.OriginDetails)
            {
                OriginList.Add(origin);
            }

            LoadOriginSelection();
        }

        private void LoadOriginSelection()
        {
            if (CharacterSelection != null)
            {
                ListValidationClass.CheckBuildCount(JumpSelection, CharacterSelectionIndex);

                if (JumpSelection.Build[CharacterSelectionIndex].OriginIndex == -1)
                {
                    OriginSelection = JumpSelection.OriginDetails.First();
                    OriginSelectionIndex = 0;
                }
                else
                {
                    int index = JumpSelection.Build[CharacterSelectionIndex].OriginIndex;
                    OriginSelection = JumpSelection.OriginDetails[index];
                    OriginSelectionIndex = index;
                }
            }

            LoadOtherOriginDetails();
        }

        private void LoadMiscOriginList()
        {
            if (JumpSelection.Build.Count > CharacterSelectionIndex)
            {
                ObservableCollection<OriginDetail> origins = new()
                {
                    JumpSelection.Build[CharacterSelectionIndex].Location,
                    JumpSelection.Build[CharacterSelectionIndex].Species
                };

                ListValidationClass.CheckMiscOriginCount(JumpSelection, CharacterSelectionIndex);

                foreach (OriginDetail origin in JumpSelection.Build[CharacterSelectionIndex].MiscOriginDetails)
                {
                    origins.Add(origin);
                }

                MiscOriginDetailList = origins;
                MiscOriginDetailIndex = 0;
            }
        }

        private void LoadOtherOriginDetails()
        {
            Age = JumpSelection.Build[CharacterSelectionIndex].Age;
            AgeCost = JumpSelection.Build[CharacterSelectionIndex].AgeCost;
            Gender = JumpSelection.Build[CharacterSelectionIndex].Gender;
            GenderCost = JumpSelection.Build[CharacterSelectionIndex].GenderCost;
        }

        private void LoadMiscOriginTypes()
        {
            MiscOriginTypeList.Clear();

            foreach (string type in JumpSelection.MiscOriginCategories)
            {
                MiscOriginTypeList.Add(type);
            }

            LoadMiscOriginList();
        }

        private void LoadCurrencies()
        {
            CurrencyList.Clear();

            foreach (Currency currency in JumpSelection.Currencies)
            {
                CurrencyList.Add(currency);
            }

            CurrencySelectionIndex = 0;
            StipendSelectionIndex = 0;

            ListValidationClass.CheckStipendCount(JumpSelection, CharacterSelectionIndex);
        }

        private void LoadPurchaseTypes()
        {
            PurchaseTypeList.Clear();

            foreach (PurchaseType type in JumpSelection.PurchaseTypes)
            {
                PurchaseTypeList.Add(type);
            }

            PurchaseTypeSelectionIndex = 0;
        }

        private void LoadAllPurchaseLists()
        {
            PurchaseList.Clear();

            for (int i = 0; i < JumpSelection.PurchaseTypes.Count; i++)
            {
                PurchaseList.Add(new ObservableCollection<Purchase>());
            }
            foreach (Purchase purchase in JumpSelection.Build[CharacterSelectionIndex].Purchase)
            {
                PurchaseList[purchase.TypeIndex].Add(purchase);
            }

            LoadCurrentPurchaseList();
        }

        private void LoadCurrentPurchaseList()
        {
            if (JumpSelection.PurchaseTypes.Count > PurchaseList.Count)
            {
                LoadAllPurchaseLists();
            }
            else
            {
                CurrentLoadedPurchaseList = PurchaseList[PurchaseTypeSelectionIndex];
                if (CurrentLoadedPurchaseList.Count > 0)
                {
                    PurchaseSelection = CurrentLoadedPurchaseList.Last();
                    LoadCurrentPurchaseData();
                }
                else
                {
                    PurchaseSelectionIndex = 0;
                }
                PurchaseType purchaseType = PurchaseTypeList[PurchaseTypeSelectionIndex];
                Budget = SetBudget(purchaseType);
            }
        }

        private void LoadCurrentPurchaseData()
        {
            CategorySelection = PurchaseSelection.Category;
            PurchaseOriginIndex = PurchaseSelection.AssociatedOriginIndex;

            PurchaseName = PurchaseSelection.Name;
            PurchaseDescription = PurchaseSelection.Description;

            if (PurchaseSelection.Attributes.Any())
            {
                PurchaseAttributeList.Clear();

                foreach (PurchaseAttribute attribute in PurchaseSelection.Attributes)
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

        private void LoadPurchaseCategoryList()
        {
            CategoryList.Clear();

            if (PurchaseTypeSelection.IsItemType)
            {
                foreach (string category in LoadedSave.ItemCategoryList)
                {
                    CategoryList.Add(category);
                }
            }
            else
            {
                foreach (string category in LoadedSave.PerkCategoryList)
                {
                    CategoryList.Add(category);
                }
            }

        }

        private void LoadDrawbackList()
        {
            DrawbackList.Clear();

            foreach (Drawback drawback in JumpSelection.Build[CharacterSelectionIndex].DrawbackSelection)
            {
                DrawbackList.Add(drawback);
            }

            if (DrawbackList.Any())
            {
                DrawbackSelection = DrawbackList.Last();
            }
        }

        private void LoadScenarioList()
        {
            ScenarioList.Clear();

            foreach (Drawback scenario in JumpSelection.Build[CharacterSelectionIndex].ScenarioSelection)
            {
                ScenarioList.Add(scenario);
            }

            if (ScenarioList.Any())
            {
                ScenarioSelection = ScenarioList.Last();
            }
        }

        private void LoadImportOptionList()
        {
            ImportOptionList.Clear();

            foreach (CompanionPurchase import in JumpSelection.Build[CharacterSelectionIndex].CompanionPurchase)
            {
                ImportOptionList.Add(import);
            }

            if (ImportOptionList.Any())
            {
                ImportOptionSelection = ImportOptionList.Last();
            }
        }

        private void LoadImportCharacterList()
        {
            if (JumpSelection.Build[CharacterSelectionIndex].CompanionPurchase.Count > 0)
            {
                ImportCharacterList.Clear();

                ListValidationClass.CheckImportListCount(LoadedSave, JumpSelection, CharacterSelectionIndex);

                if (ImportOptionSelectionIndex != -1)
                {
                    foreach (CompanionImportDetailClass import in
                            JumpSelection.Build[CharacterSelectionIndex].CompanionPurchase[ImportOptionSelectionIndex].CompanionImportDetails)
                    {
                        ImportCharacterList.Add(import);
                    }
                }
            }
            else
            {
                ImportCharacterList = new()
                {
                    new("Dummy Entry")
                };
            }
        }

        private void LoadCurrentPointBank()
        {
            CurrentBankTotal = 0;

            List<Jump> jumpSubset = LoadedSave.JumpList.GetRange(0, JumpSelectionIndex);

            foreach (Jump jump in jumpSubset)
            {
                if (jump.Build.Count > CharacterSelectionIndex && (!jump.IsGauntlet || LoadedOptions.AllowGauntletBank))
                {
                    if (jump.JumpNumber != JumpSelection.JumpNumber || LoadedOptions.AllowSupplementedJumpBankSharing)
                    {
                        CurrentBankTotal += jump.Build[CharacterSelectionIndex].BankedPoints;
                        CurrentBankTotal -= jump.Build[CharacterSelectionIndex].BankUsage;
                    }
                    else
                    {
                        CurrentBankTotal -= jump.Build[CharacterSelectionIndex].BankUsage;
                    }
                }
            }

            CurrentBankTotal += JumpSelection.Build[CharacterSelectionIndex].BankedPoints;
            CurrentBankTotal -= JumpSelection.Build[CharacterSelectionIndex].BankUsage;

            JumpSelection.Build[CharacterSelectionIndex].CurrentBank = CurrentBankTotal;
        }

        private int SetBudget(PurchaseType purchaseType)
        {
            int jumpIndex = JumpList.IndexOf(JumpSelection);

            if (jumpIndex < 0)
            {
                jumpIndex = 0;
            }

            if (purchaseType.CurrencyIndex == 0)
            {
                return LoadedDrawbackSupplement switch
                {
                    Options.DrawbackSupplements.Generic => BudgetCalculationsClass.JumpBudgetCalculation(JumpSelection, purchaseType,
                                                CharacterSelectionIndex, LoadedSave.GenericDrawbackSupplement, jumpIndex),
                    Options.DrawbackSupplements.UDS => BudgetCalculationsClass.JumpBudgetCalculation(JumpSelection, purchaseType,
                                                CharacterSelectionIndex, LoadedSave.UniversalDrawbackSupplement, jumpIndex),
                    Options.DrawbackSupplements.UU => BudgetCalculationsClass.JumpBudgetCalculation(JumpSelection, purchaseType,
                                                CharacterSelectionIndex, LoadedSave.UUSupplement, jumpIndex),
                    _ => BudgetCalculationsClass.JumpBudgetCalculation(JumpSelection, purchaseType,
                                                CharacterSelectionIndex, LoadedSave.GenericDrawbackSupplement, jumpIndex),
                };
            }
            else
            {
                return LoadedDrawbackSupplement switch
                {
                    Options.DrawbackSupplements.Generic => BudgetCalculationsClass.JumpBudgetCalculation(JumpSelection, purchaseType,
                                                CharacterSelectionIndex, LoadedSave.GenericDrawbackSupplement, jumpIndex),
                    Options.DrawbackSupplements.UDS => BudgetCalculationsClass.JumpBudgetCalculation(JumpSelection, purchaseType,
                                                CharacterSelectionIndex, LoadedSave.UniversalDrawbackSupplement, jumpIndex),
                    Options.DrawbackSupplements.UU => BudgetCalculationsClass.JumpBudgetCalculation(JumpSelection, purchaseType,
                                                CharacterSelectionIndex, LoadedSave.UUSupplement, jumpIndex),
                    _ => BudgetCalculationsClass.JumpBudgetCalculation(JumpSelection, purchaseType,
                                                CharacterSelectionIndex, LoadedSave.GenericDrawbackSupplement, jumpIndex),
                };
            }
        }

        private void FixDrawbackSuspendsOnDelete(int jumpIndex)
        {
            switch (LoadedDrawbackSupplement)
            {
                case Options.DrawbackSupplements.Generic:
                    foreach (DrawbackSupplementPurchase drawback in LoadedSave.GenericDrawbackSupplement.Purchases)
                    {
                        if (drawback.SuspendList.Count >= jumpIndex + 1)
                        {
                            drawback.SuspendList.RemoveAt(jumpIndex);
                        }
                    }
                    break;
                case Options.DrawbackSupplements.UDS:
                    foreach (DrawbackSupplementPurchase drawback in LoadedSave.UniversalDrawbackSupplement.Purchases)
                    {
                        if (drawback.SuspendList.Count >= jumpIndex + 1)
                        {
                            drawback.SuspendList.RemoveAt(jumpIndex);
                        }
                    }
                    break;
                case Options.DrawbackSupplements.UU:
                    foreach (DrawbackSupplementPurchase drawback in LoadedSave.UUSupplement.Purchases)
                    {
                        if (drawback.SuspendList.Count >= jumpIndex + 1)
                        {
                            drawback.SuspendList.RemoveAt(jumpIndex);
                        }
                    }
                    break;
            }
        }

        private void LoadWarehouseInvestment()
        {
            switch (LoadedWarehouseSupplement)
            {
                case Options.CosmicWarehouseSupplements.Generic:
                    WarehouseInvestmentAllowed = LoadedSave.GenericWarehouse.InvestmentAllowed;
                    WarehouseInvestmentRatio = LoadedSave.GenericWarehouse.InvestmentRatio;
                    break;
                case Options.CosmicWarehouseSupplements.PersonalReality:
                    WarehouseInvestmentAllowed = LoadedSave.PersonalReality.InvestmentAllowed;
                    WarehouseInvestmentRatio = LoadedSave.PersonalReality.InvestmentRatio;
                    break;
                default:
                    break;
            }

            WarehousePointsInvested = JumpSelection.Build[0].WarehouseInvestment;

            if (LoadedWarehouseSupplement == Options.CosmicWarehouseSupplements.PersonalReality &&
                LoadedSave.PersonalReality.CoreMode == PersonalReality.CoreModes.Unlimited)
            {
                int unlimitedModeSubtotal = WarehousePointsInvested;
                int postUnlimitedSubtotal = 0;

                if (unlimitedModeSubtotal > 100)
                {
                    postUnlimitedSubtotal = unlimitedModeSubtotal - 100;
                    unlimitedModeSubtotal = 100;
                }

                WarehousePointsGained = unlimitedModeSubtotal + (postUnlimitedSubtotal / WarehouseInvestmentRatio);
                PRUnlimitedRemainingInvestment = 100 - unlimitedModeSubtotal;
                PRUnlimitedSelected = true;
            }
            else
            {
                WarehousePointsGained = WarehousePointsInvested / WarehouseInvestmentRatio;
                PRUnlimitedSelected = false;
            }
        }

        private void LoadBodyModInvestment()
        {
            switch (LoadedBodyModSupplement)
            {
                case Options.BodyModSupplements.Generic:
                    BodyModInvestmentAllowed = LoadedSave.GenericBodyMod.InvestmentAllowed;
                    BodyModInvestmentRatio = LoadedSave.GenericBodyMod.InvestmentRatio;
                    break;
                case Options.BodyModSupplements.SBBodyMod:
                    BodyModInvestmentAllowed = LoadedSave.SBBodyMod.InvestmentAllowed;
                    BodyModInvestmentRatio = LoadedSave.SBBodyMod.InvestmentRatio;
                    break;
                case Options.BodyModSupplements.EssentialBodyMod:
                    BodyModInvestmentAllowed = LoadedSave.EssentialBodyMod.InvestmentAllowed;
                    BodyModInvestmentRatio = LoadedSave.EssentialBodyMod.InvestmentRatio;
                    break;
                default:
                    break;
            }

            BodyModPointsInvested = JumpSelection.Build[CharacterSelectionIndex].BodyModInvestment;
            BodyModPointsGained = BodyModPointsInvested / BodyModInvestmentRatio;
        }

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

        #endregion


        #region Commands
        [RelayCommand]
        private void NewJump()
        {
            int budget = LoadedOptions.DefaultBudget;
            int itemStipend = LoadedOptions.DefaultItemStipend;
            bool originDiscounts = LoadedOptions.OriginDiscounts;
            int perkFreebieThreshold = LoadedOptions.DefaultPerkFreebieThreshold;
            int itemFreebieThreshold = LoadedOptions.DefaultItemFreebieThreshold;

            Jump jump = new($"Jump #{JumpList.Count + 1}", budget, itemStipend, originDiscounts, perkFreebieThreshold, itemFreebieThreshold);

            LoadedSave.JumpList.Add(jump);
            JumpList.Add(jump);

            CalculateJumpNumber(LoadedSave.JumpList);

            JumpSelectionIndex = JumpList.Count - 1;
            BuildTabIndex = 5;
        }

        [RelayCommand(CanExecute = nameof(CanDeleteJump))]
        private void DeleteJump()
        {
            if (_dialogService.ConfirmDialog("Are you sure you want to delete the selected Jump? This decision cannot be reversed."))
            {
                int index = LoadedSave.JumpList.IndexOf(JumpSelection);
                if (index == 0)
                {
                    JumpSelectionIndex = 1;
                }
                else
                {
                    JumpSelectionIndex = 0;
                }
                JumpList.RemoveAt(index);
                LoadedSave.JumpList.RemoveAt(index);

                CalculateJumpNumber(LoadedSave.JumpList);

                DeleteJumpCommand.NotifyCanExecuteChanged();
                MoveJumpDownCommand.NotifyCanExecuteChanged();
                MoveJumpUpCommand.NotifyCanExecuteChanged();

                FixDrawbackSuspendsOnDelete(index);
            }
        }

        private bool CanDeleteJump() => JumpList != null && JumpList.Count > 1 && JumpSelection != null;

        [RelayCommand(CanExecute = nameof(CanMoveJumpUp))]
        private void MoveJumpUp()
        {
            int index = LoadedSave.JumpList.IndexOf(JumpSelection);

            JumpList.SwapCollectionItems(index, index - 1);
            LoadedSave.JumpList.SwapListItems(index, index - 1);

            JumpSelectionIndex = index - 1;

            CalculateJumpNumber(LoadedSave.JumpList);
        }

        private bool CanMoveJumpUp() => LoadedSave.JumpList.IndexOf(JumpSelection) > 0 && JumpSelection != null;

        [RelayCommand(CanExecute = nameof(CanMoveJumpDown))]
        private void MoveJumpDown()
        {
            int index = LoadedSave.JumpList.IndexOf(JumpSelection);

            JumpList.SwapCollectionItems(index, index + 1);
            LoadedSave.JumpList.SwapListItems(index, index + 1);

            JumpSelectionIndex = index + 1;

            CalculateJumpNumber(LoadedSave.JumpList);
        }
        private bool CanMoveJumpDown() => LoadedSave.JumpList.IndexOf(JumpSelection) < (JumpList.Count - 1) && JumpSelection != null;

        [RelayCommand]
        private void NewOrigin()
        {
            JumpSelection.OriginDetails.Add(new($"Origin #{JumpSelection.OriginDetails.Count}", "Origin"));

            CreateOriginList();
        }

        [RelayCommand(CanExecute = nameof(CanDeleteOrigin))]
        private void DeleteOrigin()
        {
            if (_dialogService.ConfirmDialog("Are you sure you want to delete this Origin? " +
                "This action cannot be reversed and will reset anything using it to the default."))
            {
                foreach (JumpBuild build in JumpSelection.Build)
                {
                    if (build.OriginIndex == OriginEditSelectionIndex)
                    {
                        build.OriginIndex = 0;
                    }
                    else if (build.OriginIndex > OriginEditSelectionIndex)
                    {
                        build.OriginIndex--;
                    }

                    foreach (Purchase purchase in build.Purchase)
                    {
                        if (purchase.AssociatedOriginIndex == OriginEditSelectionIndex)
                        {
                            purchase.AssociatedOriginIndex = 0;
                        }
                        else if (purchase.AssociatedOriginIndex > OriginEditSelectionIndex)
                        {
                            purchase.AssociatedOriginIndex--;
                        }
                    }
                }

                JumpSelection.OriginDetails.RemoveAt(OriginEditSelectionIndex);

                CreateOriginList();
            }
        }

        private bool CanDeleteOrigin() => OriginEditSelectionIndex > 0;

        [RelayCommand]
        private void NewType()
        {
            JumpSelection.MiscOriginCategories.Add($"New Type {JumpSelection.MiscOriginCategories.Count}");

            LoadMiscOriginTypes();
            LoadMiscOriginList();
        }

        [RelayCommand(CanExecute = nameof(CanDeleteType))]
        private void DeleteType()
        {
            if (_dialogService.ConfirmDialog("Are you sure you want to delete this Type? " +
                "This action cannot be reversed and all data associated with it will be lost."))
            {
                JumpSelection.MiscOriginCategories.RemoveAt(MiscOriginTypeSelectionIndex);

                LoadMiscOriginTypes();
                LoadMiscOriginList();

                MiscOriginTypeSelectionIndex = 0;
            }
        }

        private bool CanDeleteType() => MiscOriginTypeList.Any() && MiscOriginTypeSelectionIndex != -1;

        [RelayCommand]

        private void NewCurrency()
        {
            JumpSelection.Currencies.Add(new($"Currency #{JumpSelection.Currencies.Count}", "CP"));

            LoadCurrencies();

            CurrencySelection = CurrencyList.Last();

            DeleteCurrencyCommand.NotifyCanExecuteChanged();
        }

        [RelayCommand(CanExecute = nameof(CanDeleteCurrency))]

        private void DeleteCurrency()
        {
            if (_dialogService.ConfirmDialog("Are you sure you want to delete this currency? " +
                "This action cannot be reversed and all purchase types using it will be set " +
                "to Choice Points."))
            {
                foreach (PurchaseType type in JumpSelection.PurchaseTypes)
                {
                    if (type.CurrencyIndex == CurrencySelectionIndex)
                    {
                        type.CurrencyIndex = 0;
                    }
                    else if (type.CurrencyIndex > CurrencySelectionIndex)
                    {
                        type.CurrencyIndex--;
                    }
                }

                JumpSelection.Currencies.RemoveAt(CurrencySelectionIndex);

                LoadCurrencies();

                CurrencySelectionIndex = 0;

                DeleteCurrencyCommand.NotifyCanExecuteChanged();
            }
        }

        private bool CanDeleteCurrency() => CurrencySelectionIndex != 0;

        [RelayCommand]

        private void NewPurchaseType()
        {
            JumpSelection.PurchaseTypes.Add(new($"Purchase Type {JumpSelection.PurchaseTypes.Count}", 0, false));

            LoadPurchaseTypes();
        }

        [RelayCommand(CanExecute = nameof(CanDeletePurchaseType))]

        private void DeletePurchaseType()
        {
            if (_dialogService.ConfirmDialog("Are you sure you want to delete this purchase type? " +
                "This action cannot be reversed and all purchases using this type will " +
                "revert to the Perks type."))
            {
                foreach (JumpBuild jumpBuild in JumpSelection.Build)
                {
                    foreach (Purchase purchase in jumpBuild.Purchase)
                    {
                        if (purchase.TypeIndex == PurchaseTypeSelectionIndex)
                        {
                            purchase.TypeIndex = 0;
                        }
                        else if (purchase.TypeIndex > PurchaseTypeSelectionIndex)
                        {
                            purchase.TypeIndex--;
                        }
                    }
                }

                JumpSelection.PurchaseTypes.RemoveAt(PurchaseTypeSelectionIndex);

                LoadPurchaseTypes();

                PurchaseTypeSelectionIndex = 0;

                DeletePurchaseTypeCommand.NotifyCanExecuteChanged();
            }
        }

        private bool CanDeletePurchaseType() => PurchaseTypeSelectionIndex > 1;

        [RelayCommand]
        private void NewPurchase()
        {
            Purchase purchase;

            if (PurchaseTypeSelection.IsItemType)
            {
                purchase = new("New Purchase", PurchaseTypeSelectionIndex, "Other Item");

                JumpSelection.Build[CharacterSelectionIndex].Purchase.Add(purchase);
                PurchaseList[purchase.TypeIndex].Add(purchase);
            }
            else
            {
                purchase = new("New Purchase", PurchaseTypeSelectionIndex, "Other Perk");

                JumpSelection.Build[CharacterSelectionIndex].Purchase.Add(purchase);
                PurchaseList[purchase.TypeIndex].Add(purchase);
            }

            PurchaseSelection = CurrentLoadedPurchaseList.Last();
            LoadCurrentPurchaseData();

            Budget = SetBudget(PurchaseTypeList[PurchaseTypeSelectionIndex]);

            DeletePurchaseCommand.NotifyCanExecuteChanged();
            MovePurchaseUpCommand.NotifyCanExecuteChanged();
            MovePurchaseDownCommand.NotifyCanExecuteChanged();
        }

        [RelayCommand(CanExecute = nameof(CanDeletePurchase))]
        private void DeletePurchase()
        {
            if (_dialogService.ConfirmDialog("Are you sure you want to delete this purchase? " +
                "This action cannot be reversed."))
            {
                JumpSelection.Build[CharacterSelectionIndex].Purchase.Remove(PurchaseSelection);
                CurrentLoadedPurchaseList.Remove(PurchaseSelection);

                LoadAllPurchaseLists();

                DeletePurchaseCommand.NotifyCanExecuteChanged();
                MovePurchaseUpCommand.NotifyCanExecuteChanged();
                MovePurchaseDownCommand.NotifyCanExecuteChanged();
            }
        }

        private bool CanDeletePurchase() => CurrentLoadedPurchaseList.Any() && PurchaseSelection != null;

        [RelayCommand(CanExecute = nameof(CanMovePurchaseUp))]
        private void MovePurchaseUp()
        {
            int index = PurchaseSelectionIndex;

            int buildIndex1 = JumpSelection.Build[CharacterSelectionIndex].Purchase.IndexOf(CurrentLoadedPurchaseList[index]);
            int buildIndex2 = JumpSelection.Build[CharacterSelectionIndex].Purchase.IndexOf(CurrentLoadedPurchaseList[index - 1]);

            ListOperationsClass.SwapListItems(JumpSelection.Build[CharacterSelectionIndex].Purchase, buildIndex1, buildIndex2);
            ListOperationsClass.SwapCollectionItems(CurrentLoadedPurchaseList, index, index - 1);

            PurchaseSelectionIndex = index - 1;

            MovePurchaseUpCommand.NotifyCanExecuteChanged();
            MovePurchaseDownCommand.NotifyCanExecuteChanged();
        }

        private bool CanMovePurchaseUp() => PurchaseSelectionIndex > 0;

        [RelayCommand(CanExecute = nameof(CanMovePurchaseDown))]
        private void MovePurchaseDown()
        {
            int index = PurchaseSelectionIndex;

            int buildIndex1 = JumpSelection.Build[CharacterSelectionIndex].Purchase.IndexOf(CurrentLoadedPurchaseList[index]);
            int buildIndex2 = JumpSelection.Build[CharacterSelectionIndex].Purchase.IndexOf(CurrentLoadedPurchaseList[index + 1]);

            ListOperationsClass.SwapListItems(JumpSelection.Build[CharacterSelectionIndex].Purchase, buildIndex1, buildIndex2);
            ListOperationsClass.SwapCollectionItems(CurrentLoadedPurchaseList, index, index + 1);

            PurchaseSelectionIndex = index + 1;

            MovePurchaseUpCommand.NotifyCanExecuteChanged();
            MovePurchaseDownCommand.NotifyCanExecuteChanged();
        }


        private bool CanMovePurchaseDown() => PurchaseSelectionIndex != -1 && PurchaseSelectionIndex < CurrentLoadedPurchaseList.Count - 1;


        [RelayCommand]
        private void NewScenario()
        {
            JumpSelection.Build[CharacterSelectionIndex].ScenarioSelection.Add(new($"Scenario #{ScenarioList.Count}"));

            LoadScenarioList();

            DeleteScenarioCommand.NotifyCanExecuteChanged();
            MoveScenarioUpCommand.NotifyCanExecuteChanged();
            MoveScenarioDownCommand.NotifyCanExecuteChanged();
        }

        [RelayCommand(CanExecute = nameof(CanDeleteScenario))]
        private void DeleteScenario()
        {
            if (_dialogService.ConfirmDialog("Are you sure you want to delete this Scenario? " +
                "This action cannot be reversed."))
            {
                int index = ScenarioSelectionIndex;

                JumpSelection.Build[CharacterSelectionIndex].ScenarioSelection.RemoveAt(index);
                ScenarioList.RemoveAt(index);

                LoadScenarioList();

                DeleteScenarioCommand.NotifyCanExecuteChanged();
                MoveScenarioUpCommand.NotifyCanExecuteChanged();
                MoveScenarioDownCommand.NotifyCanExecuteChanged();
            }
        }

        private bool CanDeleteScenario() => ScenarioList.Any() && ScenarioSelection != null;

        [RelayCommand(CanExecute = nameof(CanMoveScenarioUp))]
        private void MoveScenarioUp()
        {
            int index = ScenarioSelectionIndex;

            ListOperationsClass.SwapListItems(JumpSelection.Build[CharacterSelectionIndex].ScenarioSelection, index, index - 1);
            ListOperationsClass.SwapCollectionItems(ScenarioList, index, index - 1);

            ScenarioSelectionIndex = index - 1;

            MoveScenarioUpCommand.NotifyCanExecuteChanged();
            MoveScenarioDownCommand.NotifyCanExecuteChanged();
        }

        private bool CanMoveScenarioUp() => ScenarioSelectionIndex > 0;

        [RelayCommand(CanExecute = nameof(CanMoveScenarioDown))]
        private void MoveScenarioDown()
        {
            int index = ScenarioSelectionIndex;

            ListOperationsClass.SwapListItems(JumpSelection.Build[CharacterSelectionIndex].ScenarioSelection, index, index + 1);
            ListOperationsClass.SwapCollectionItems(ScenarioList, index, index + 1);

            ScenarioSelectionIndex = index + 1;

            MoveScenarioUpCommand.NotifyCanExecuteChanged();
            MoveScenarioDownCommand.NotifyCanExecuteChanged();
        }


        private bool CanMoveScenarioDown() => ScenarioSelectionIndex != -1 && ScenarioSelectionIndex < ScenarioList.Count - 1;

        [RelayCommand]
        private void NewDrawback()
        {
            JumpSelection.Build[CharacterSelectionIndex].DrawbackSelection.Add(new($"Drawback #{DrawbackList.Count}"));

            LoadDrawbackList();

            DeleteDrawbackCommand.NotifyCanExecuteChanged();
            MoveDrawbackUpCommand.NotifyCanExecuteChanged();
            MoveDrawbackDownCommand.NotifyCanExecuteChanged();
        }

        [RelayCommand(CanExecute = nameof(CanDeleteDrawback))]
        private void DeleteDrawback()
        {
            if (_dialogService.ConfirmDialog("Are you sure you want to delete this Drawback? " +
                "This action cannot be reversed."))
            {
                int index = DrawbackSelectionIndex;

                JumpSelection.Build[CharacterSelectionIndex].DrawbackSelection.RemoveAt(index);
                DrawbackList.RemoveAt(index);

                LoadDrawbackList();

                DeleteDrawbackCommand.NotifyCanExecuteChanged();
                MoveDrawbackUpCommand.NotifyCanExecuteChanged();
                MoveDrawbackDownCommand.NotifyCanExecuteChanged();
            }
        }

        private bool CanDeleteDrawback() => DrawbackList.Any() && DrawbackSelection != null;

        [RelayCommand(CanExecute = nameof(CanMoveDrawbackUp))]
        private void MoveDrawbackUp()
        {
            int index = DrawbackSelectionIndex;

            ListOperationsClass.SwapListItems(JumpSelection.Build[CharacterSelectionIndex].DrawbackSelection, index, index - 1);
            ListOperationsClass.SwapCollectionItems(DrawbackList, index, index - 1);

            DrawbackSelectionIndex = index - 1;

            MoveDrawbackUpCommand.NotifyCanExecuteChanged();
            MoveDrawbackDownCommand.NotifyCanExecuteChanged();
        }

        private bool CanMoveDrawbackUp() => DrawbackSelectionIndex > 0;

        [RelayCommand(CanExecute = nameof(CanMoveDrawbackDown))]
        private void MoveDrawbackDown()
        {
            int index = DrawbackSelectionIndex;

            ListOperationsClass.SwapListItems(JumpSelection.Build[CharacterSelectionIndex].DrawbackSelection, index, index + 1);
            ListOperationsClass.SwapCollectionItems(DrawbackList, index, index + 1);

            DrawbackSelectionIndex = index + 1;

            MoveDrawbackUpCommand.NotifyCanExecuteChanged();
            MoveDrawbackDownCommand.NotifyCanExecuteChanged();
        }


        private bool CanMoveDrawbackDown() => DrawbackSelectionIndex != -1 && DrawbackSelectionIndex < DrawbackList.Count - 1;

        [RelayCommand]
        private void NewImport()
        {
            JumpSelection.Build[CharacterSelectionIndex].CompanionPurchase.Add(new());

            LoadImportOptionList();

            DeleteImportCommand.NotifyCanExecuteChanged();
        }

        [RelayCommand(CanExecute = nameof(CanDeleteImport))]
        private void DeleteImport()
        {
            if (_dialogService.ConfirmDialog("Are you sure you want to delete this Import option? " +
                "This action cannot be reversed."))
            {
                int index = ImportOptionSelectionIndex;

                JumpSelection.Build[CharacterSelectionIndex].CompanionPurchase.RemoveAt(index);
                ImportOptionList.RemoveAt(index);

                ImportOptionSelectionIndex = 0;

                LoadImportOptionList();
                LoadImportCharacterList();

                DeleteImportCommand.NotifyCanExecuteChanged();
            }
        }

        private bool CanDeleteImport() => ImportOptionList.Any() && ImportOptionSelection != null;

        private void BuildTabChanged()
        {
            switch (BuildTabIndex)
            {
                case 0:
                    Budget = SetBudget(JumpSelection.PurchaseTypes[0]);
                    break;
                case 1:
                    if (PurchaseTypeSelection != null)
                    {
                        Budget = SetBudget(PurchaseTypeSelection);
                    }
                    break;
                case 2:
                    LoadImportOptionList();
                    LoadImportCharacterList();
                    Budget = SetBudget(JumpSelection.PurchaseTypes[0]);
                    break;
                case 3:
                    Budget = SetBudget(JumpSelection.PurchaseTypes[0]);
                    break;
                case 4:
                    Budget = SetBudget(JumpSelection.PurchaseTypes[0]);
                    break;
                default:
                    break;
            }
        }

        [RelayCommand]
        private void NewPurchaseTrait()
        {
            if (PurchaseSelection != null)
            {
                PurchaseAttribute attribute = new()
                {
                    Name = $"Trait #{PurchaseSelection.Attributes.Count + 1}"
                };

                PurchaseSelection.Attributes.Add(attribute);
                PurchaseAttributeList.Add(attribute);

                PurchaseAttributeIndex = PurchaseAttributeList.Count - 1;

                DeletePurchaseTraitCommand.NotifyCanExecuteChanged();
            }
        }

        [RelayCommand(CanExecute = nameof(CanDeletePurchaseTrait))]
        private void DeletePurchaseTrait()
        {
            PurchaseSelection.Attributes.RemoveAt(PurchaseAttributeIndex);
            PurchaseAttributeList.RemoveAt(PurchaseAttributeIndex);

            PurchaseAttributeIndex = 0;

            DeletePurchaseTraitCommand.NotifyCanExecuteChanged();
        }

        private bool CanDeletePurchaseTrait() => PurchaseAttributeList.Any() && PurchaseAttributeIndex != -1 && PurchaseSelection != null;

        #endregion
    }
}
