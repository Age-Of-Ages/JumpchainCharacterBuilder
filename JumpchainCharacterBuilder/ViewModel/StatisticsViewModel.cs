﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using JumpchainCharacterBuilder.Interfaces;
using JumpchainCharacterBuilder.Messages;
using JumpchainCharacterBuilder.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace JumpchainCharacterBuilder.ViewModel
{
    public partial class StatisticsViewModel : ViewModelBase
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
        private ObservableCollection<Jump> _jumpList = [];

        [ObservableProperty]
        private ObservableCollection<Character> _characterList = [];
        [ObservableProperty]
        private Character _characterSelection = new();
        [ObservableProperty]
        private int _characterSelectionIndex = 0;

        [ObservableProperty]
        private int _jumpsCompleted = 0;
        [ObservableProperty]
        private int _gauntletsCompleted = 0;

        [ObservableProperty]
        private int _overallPointsSpent = 0;
        [ObservableProperty]
        private string _overallPointsSpentString = "";
        [ObservableProperty]
        private int _overallPerkPointsSpent = 0;
        [ObservableProperty]
        private string _overallPerkPointsSpentString = "";
        [ObservableProperty]
        private int _overallItemPointsSpent = 0;
        [ObservableProperty]
        private string _overallItemPointsSpentString = "";
        [ObservableProperty]
        private int _overallCompanionPointsSpent = 0;
        [ObservableProperty]
        private string _overallCompanionPointsSpentString = "";
        [ObservableProperty]
        private int _overallDrawbackPointsEarned = 0;
        [ObservableProperty]
        private string _overallDrawbackPointsEarnedString = "";
        [ObservableProperty]
        private int _overallScenarioPointsEarned = 0;
        [ObservableProperty]
        private string _overallScenarioPointsEarnedString = "";

        [ObservableProperty]
        private int _characterPointsSpent = 0;
        [ObservableProperty]
        private string _characterPointsSpentString = "";
        [ObservableProperty]
        private int _characterPerkPointsSpent = 0;
        [ObservableProperty]
        private string _characterPerkPointsSpentString = "";
        [ObservableProperty]
        private int _characterItemPointsSpent = 0;
        [ObservableProperty]
        private string _characterItemPointsSpentString = "";
        [ObservableProperty]
        private int _characterCompanionPointsSpent = 0;
        [ObservableProperty]
        private string _characterCompanionPointsSpentString = "";
        [ObservableProperty]
        private int _characterDrawbackPointsEarned = 0;
        [ObservableProperty]
        private string _characterDrawbackPointsEarnedString = "";
        [ObservableProperty]
        private int _characterScenarioPointsEarned = 0;
        [ObservableProperty]
        private string _characterScenarioPointsEarnedString = "";

        [ObservableProperty]
        private int _overallPerksBought = 0;
        [ObservableProperty]
        private int _overallItemsBought = 0;
        [ObservableProperty]
        private int _overallDrawbacksTaken = 0;
        [ObservableProperty]
        private int _overallScenariosTaken = 0;
        [ObservableProperty]
        private int _overallBodyModAddons = 0;
        [ObservableProperty]
        private int _overallWarehouseAddons = 0;

        [ObservableProperty]
        private int _characterPerksBought = 0;
        [ObservableProperty]
        private int _characterItemsBought = 0;
        [ObservableProperty]
        private int _characterDrawbacksTaken = 0;
        [ObservableProperty]
        private int _characterScenariosTaken = 0;
        [ObservableProperty]
        private int _characterBodyModAddons = 0;
        [ObservableProperty]
        private int _characterWarehouseAddons = 0;

        [ObservableProperty]
        private int _overallOneUps = 0;
        [ObservableProperty]
        private int _characterOneUps = 0;

        [ObservableProperty]
        private ObservableCollection<PurchaseCategoryStatistics> _overallPerkCategoryTotals = [];
        [ObservableProperty]
        private ObservableCollection<PurchaseCategoryStatistics> _overallItemCategoryTotals = [];

        [ObservableProperty]
        private ObservableCollection<PurchaseCategoryStatistics> _characterPerkCategoryTotals = [];
        [ObservableProperty]
        private ObservableCollection<PurchaseCategoryStatistics> _characterItemCategoryTotals = [];

        #endregion

        #region Properties
        partial void OnCharacterSelectionIndexChanged(int value)
        {
            if (value != -1)
            {
                CharacterPointsSpent = 0;
                CharacterPerkPointsSpent = 0;
                CharacterItemPointsSpent = 0;
                CharacterCompanionPointsSpent = 0;
                CharacterDrawbackPointsEarned = 0;
                CharacterScenarioPointsEarned = 0;

                CharacterPerksBought = 0;
                CharacterItemsBought = 0;
                CharacterDrawbacksTaken = 0;
                CharacterScenariosTaken = 0;
                CharacterBodyModAddons = 0;
                CharacterWarehouseAddons = 0;

                CharacterOneUps = 0;

                foreach (PurchaseCategoryStatistics category in CharacterPerkCategoryTotals)
                {
                    category.Total = 0;
                }

                foreach (PurchaseCategoryStatistics category in CharacterItemCategoryTotals)
                {
                    category.Total = 0;
                }

                foreach (Jump jump in JumpList)
                {
                    if (jump.Build.Count > value && jump.Build[value] != null)
                    {
                        LoadSingleCharacterData(jump, jump.Build[CharacterSelectionIndex]);
                    }
                }
            }
        }

        partial void OnOverallPointsSpentChanged(int value)
        {
            OverallPointsSpentString = FormatHelper.FormatBudgetString(AppSettings.BudgetThousandsSeparator, OverallPointsSpent);
        }

        partial void OnOverallPerkPointsSpentChanged(int value)
        {
            OverallPerkPointsSpentString = FormatHelper.FormatBudgetString(AppSettings.BudgetThousandsSeparator, OverallPerkPointsSpent);
        }

        partial void OnOverallItemPointsSpentChanged(int value)
        {
            OverallItemPointsSpentString = FormatHelper.FormatBudgetString(AppSettings.BudgetThousandsSeparator, OverallItemPointsSpent);
        }

        partial void OnOverallCompanionPointsSpentChanged(int value)
        {
            OverallCompanionPointsSpentString = FormatHelper.FormatBudgetString(AppSettings.BudgetThousandsSeparator, OverallCompanionPointsSpent);
        }

        partial void OnOverallDrawbackPointsEarnedChanged(int value)
        {
            OverallDrawbackPointsEarnedString = FormatHelper.FormatBudgetString(AppSettings.BudgetThousandsSeparator, OverallDrawbackPointsEarned);
        }

        partial void OnOverallScenarioPointsEarnedChanged(int value)
        {
            OverallScenarioPointsEarnedString = FormatHelper.FormatBudgetString(AppSettings.BudgetThousandsSeparator, OverallScenarioPointsEarned);
        }

        partial void OnCharacterPointsSpentChanged(int value)
        {
            CharacterPointsSpentString = FormatHelper.FormatBudgetString(AppSettings.BudgetThousandsSeparator, CharacterPointsSpent);
        }

        partial void OnCharacterPerkPointsSpentChanged(int value)
        {
            CharacterPerkPointsSpentString = FormatHelper.FormatBudgetString(AppSettings.BudgetThousandsSeparator, CharacterPerkPointsSpent);
        }

        partial void OnCharacterItemPointsSpentChanged(int value)
        {
            CharacterItemPointsSpentString = FormatHelper.FormatBudgetString(AppSettings.BudgetThousandsSeparator, CharacterItemPointsSpent);
        }

        partial void OnCharacterCompanionPointsSpentChanged(int value)
        {
            CharacterCompanionPointsSpentString = FormatHelper.FormatBudgetString(AppSettings.BudgetThousandsSeparator, CharacterCompanionPointsSpent);
        }

        partial void OnCharacterDrawbackPointsEarnedChanged(int value)
        {
            CharacterDrawbackPointsEarnedString = FormatHelper.FormatBudgetString(AppSettings.BudgetThousandsSeparator, CharacterDrawbackPointsEarned);
        }

        partial void OnCharacterScenarioPointsEarnedChanged(int value)
        {
            CharacterScenarioPointsEarnedString = FormatHelper.FormatBudgetString(AppSettings.BudgetThousandsSeparator, CharacterScenarioPointsEarned);
        }

        #endregion

        #region Constructor
        public StatisticsViewModel()
        {

        }

        public StatisticsViewModel(IDialogService dialogService)
        {
            Messenger.Register<SaveDataChangedMessage>(this, (r, m) =>
            {
                LoadedOptions = LoadedSave.Options;

                LoadCategoryList();
                LoadCharacterList();
                LoadJumpList();
            });
            Messenger.Register<SaveDataSendMessage>(this, (r, m) =>
            {
                LoadedSave = m.Value;
                LoadedOptions = LoadedSave.Options;

                LoadCategoryList();
                LoadCharacterList();
                LoadJumpList();
            });
            Messenger.Register<SupplementChangedMessage>(this, (r, m) =>
            {

            });
            Messenger.Register<CategoryChangedMessage>(this, (r, m) =>
            {
                LoadCategoryList();
            });
            Messenger.Register<SettingsLoadedMessage>(this, (r, m) =>
            {
                AppSettings = m.Value;

                SpellCheckEnabled = AppSettings.SpellCheckEnabled;
                OverallPointsSpentString = FormatHelper.FormatBudgetString(AppSettings.BudgetThousandsSeparator, OverallPointsSpent);
                OverallPerkPointsSpentString = FormatHelper.FormatBudgetString(AppSettings.BudgetThousandsSeparator, OverallPerkPointsSpent);
                OverallItemPointsSpentString = FormatHelper.FormatBudgetString(AppSettings.BudgetThousandsSeparator, OverallItemPointsSpent);
                OverallCompanionPointsSpentString = FormatHelper.FormatBudgetString(AppSettings.BudgetThousandsSeparator, OverallCompanionPointsSpent);
                OverallDrawbackPointsEarnedString = FormatHelper.FormatBudgetString(AppSettings.BudgetThousandsSeparator, OverallDrawbackPointsEarned);
                OverallScenarioPointsEarnedString = FormatHelper.FormatBudgetString(AppSettings.BudgetThousandsSeparator, OverallScenarioPointsEarned);
                CharacterPointsSpentString = FormatHelper.FormatBudgetString(AppSettings.BudgetThousandsSeparator, CharacterPointsSpent);
                CharacterPerkPointsSpentString = FormatHelper.FormatBudgetString(AppSettings.BudgetThousandsSeparator, CharacterPerkPointsSpent);
                CharacterItemPointsSpentString = FormatHelper.FormatBudgetString(AppSettings.BudgetThousandsSeparator, CharacterItemPointsSpent);
                CharacterCompanionPointsSpentString = FormatHelper.FormatBudgetString(AppSettings.BudgetThousandsSeparator, CharacterCompanionPointsSpent);
                CharacterDrawbackPointsEarnedString = FormatHelper.FormatBudgetString(AppSettings.BudgetThousandsSeparator, CharacterDrawbackPointsEarned);
                CharacterScenarioPointsEarnedString = FormatHelper.FormatBudgetString(AppSettings.BudgetThousandsSeparator, CharacterScenarioPointsEarned);

            });
            Messenger.Register<SettingsChangedMessage>(this, (r, m) =>
            {
                SpellCheckEnabled = AppSettings.SpellCheckEnabled;
                OverallPointsSpentString = FormatHelper.FormatBudgetString(AppSettings.BudgetThousandsSeparator, OverallPointsSpent);
                OverallPerkPointsSpentString = FormatHelper.FormatBudgetString(AppSettings.BudgetThousandsSeparator, OverallPerkPointsSpent);
                OverallItemPointsSpentString = FormatHelper.FormatBudgetString(AppSettings.BudgetThousandsSeparator, OverallItemPointsSpent);
                OverallCompanionPointsSpentString = FormatHelper.FormatBudgetString(AppSettings.BudgetThousandsSeparator, OverallCompanionPointsSpent);
                OverallDrawbackPointsEarnedString = FormatHelper.FormatBudgetString(AppSettings.BudgetThousandsSeparator, OverallDrawbackPointsEarned);
                OverallScenarioPointsEarnedString = FormatHelper.FormatBudgetString(AppSettings.BudgetThousandsSeparator, OverallScenarioPointsEarned);
                CharacterPointsSpentString = FormatHelper.FormatBudgetString(AppSettings.BudgetThousandsSeparator, CharacterPointsSpent);
                CharacterPerkPointsSpentString = FormatHelper.FormatBudgetString(AppSettings.BudgetThousandsSeparator, CharacterPerkPointsSpent);
                CharacterItemPointsSpentString = FormatHelper.FormatBudgetString(AppSettings.BudgetThousandsSeparator, CharacterItemPointsSpent);
                CharacterCompanionPointsSpentString = FormatHelper.FormatBudgetString(AppSettings.BudgetThousandsSeparator, CharacterCompanionPointsSpent);
                CharacterDrawbackPointsEarnedString = FormatHelper.FormatBudgetString(AppSettings.BudgetThousandsSeparator, CharacterDrawbackPointsEarned);
                CharacterScenarioPointsEarnedString = FormatHelper.FormatBudgetString(AppSettings.BudgetThousandsSeparator, CharacterScenarioPointsEarned);

            });

            LoadCategoryList();

            _dialogService = dialogService;
        }

        #endregion

        #region Methods
        private void LoadCharacterList()
        {
            CharacterList.Clear();

            foreach (Character character in LoadedSave.CharacterList)
            {
                CharacterList.Add(character);
            }

            if (CharacterList.Any())
            {
                CharacterSelection = CharacterList.First();
                CharacterSelectionIndex = 0;
            }
        }

        private void LoadJumpList()
        {
            JumpList.Clear();

            foreach (Jump jump in LoadedSave.JumpList)
            {
                JumpList.Add(jump);
            }
        }

        private void LoadCategoryList()
        {
            OverallPerkCategoryTotals.Clear();
            OverallItemCategoryTotals.Clear();

            CharacterPerkCategoryTotals.Clear();
            CharacterItemCategoryTotals.Clear();

            foreach (string category in LoadedSave.PerkCategoryList)
            {
                OverallPerkCategoryTotals.Add(new()
                {
                    Name = category
                });
                CharacterPerkCategoryTotals.Add(new()
                {
                    Name = category
                });
            }

            foreach (string category in LoadedSave.ItemCategoryList)
            {
                OverallItemCategoryTotals.Add(new()
                {
                    Name = category
                });
                CharacterItemCategoryTotals.Add(new()
                {
                    Name = category
                });
            }
        }

        private void LoadJumpStatistics()
        {
            LoadJumpList();
            LoadCharacterList();

            JumpsCompleted = 0;
            GauntletsCompleted = 0;

            OverallPointsSpent = 0;
            OverallPerkPointsSpent = 0;
            OverallItemPointsSpent = 0;
            OverallCompanionPointsSpent = 0;
            OverallDrawbackPointsEarned = 0;
            OverallScenarioPointsEarned = 0;

            CharacterPointsSpent = 0;
            CharacterPerkPointsSpent = 0;
            CharacterItemPointsSpent = 0;
            CharacterCompanionPointsSpent = 0;
            CharacterDrawbackPointsEarned = 0;
            CharacterScenarioPointsEarned = 0;

            OverallPerksBought = 0;
            OverallItemsBought = 0;
            OverallDrawbacksTaken = 0;
            OverallScenariosTaken = 0;
            OverallBodyModAddons = 0;
            OverallWarehouseAddons = 0;

            CharacterPerksBought = 0;
            CharacterItemsBought = 0;
            CharacterDrawbacksTaken = 0;
            CharacterScenariosTaken = 0;
            CharacterBodyModAddons = 0;
            CharacterWarehouseAddons = 0;

            OverallOneUps = 0;
            CharacterOneUps = 0;

            foreach (PurchaseCategoryStatistics category in OverallPerkCategoryTotals)
            {
                category.Total = 0;
            }

            foreach (PurchaseCategoryStatistics category in CharacterPerkCategoryTotals)
            {
                category.Total = 0;
            }

            foreach (PurchaseCategoryStatistics category in OverallItemCategoryTotals)
            {
                category.Total = 0;
            }

            foreach (PurchaseCategoryStatistics category in CharacterItemCategoryTotals)
            {
                category.Total = 0;
            }

            foreach (Jump jump in JumpList)
            {
                if (jump.IsGauntlet)
                {
                    GauntletsCompleted++;
                }
                else
                {
                    JumpsCompleted++;
                }

                foreach (JumpBuild build in jump.Build)
                {
                    LoadJumpBuildData(jump, build, jump.Build.IndexOf(build));
                }
            }
        }

        private void LoadJumpBuildData(Jump jump, JumpBuild build, int characterIndex)
        {
            Dictionary<string, PurchaseCategoryStatistics> overallPerks = OverallPerkCategoryTotals.ToDictionary(x => x.Name);
            Dictionary<string, PurchaseCategoryStatistics> overallItems = OverallItemCategoryTotals.ToDictionary(x => x.Name);

            Dictionary<string, PurchaseCategoryStatistics> characterPerks = CharacterPerkCategoryTotals.ToDictionary(x => x.Name);
            Dictionary<string, PurchaseCategoryStatistics> characterItems = CharacterItemCategoryTotals.ToDictionary(x => x.Name);


            if (characterIndex == CharacterSelectionIndex)
            {
                foreach (Purchase purchase in build.Purchase)
                {
                    OverallPointsSpent += purchase.DisplayCost;
                    CharacterPointsSpent += purchase.DisplayCost;

                    if (jump.PurchaseTypes[purchase.TypeIndex].IsItemType)
                    {
                        OverallItemsBought++;
                        CharacterItemsBought++;

                        OverallItemPointsSpent += purchase.DisplayCost;
                        CharacterItemPointsSpent += purchase.DisplayCost;

                        if (purchase.Category == "Warehouse Addon")
                        {
                            OverallWarehouseAddons++;
                            CharacterWarehouseAddons++;
                        }

                        overallItems[purchase.Category].Total++;
                        characterItems[purchase.Category].Total++;
                    }
                    else
                    {
                        OverallPerksBought++;
                        CharacterPerksBought++;

                        OverallPerkPointsSpent += purchase.DisplayCost;
                        CharacterPerkPointsSpent += purchase.DisplayCost;

                        if (purchase.BodyModAddition)
                        {
                            OverallBodyModAddons++;
                            CharacterBodyModAddons++;
                        }

                        overallPerks[purchase.Category].Total++;
                        characterPerks[purchase.Category].Total++;
                    }

                    if (purchase.Attributes.Count != 0)
                    {
                        foreach (PurchaseAttribute attribute in purchase.Attributes)
                        {
                            if (attribute.Category == "1-up")
                            {
                                OverallOneUps += attribute.Value;
                                CharacterOneUps += attribute.Value;
                            }
                        }
                    }
                }

                foreach (CompanionPurchase import in build.CompanionPurchase)
                {
                    OverallCompanionPointsSpent += import.DisplayCost;
                    CharacterCompanionPointsSpent += import.DisplayCost;
                }

                foreach (Drawback drawback in build.DrawbackSelection)
                {
                    OverallDrawbackPointsEarned += drawback.Value;
                    CharacterDrawbackPointsEarned += drawback.Value;
                    OverallDrawbacksTaken++;
                }

                foreach (Drawback scenario in build.ScenarioSelection)
                {
                    OverallScenarioPointsEarned += scenario.Value;
                    CharacterScenarioPointsEarned += scenario.Value;
                    OverallScenariosTaken++;
                }
            }
            else
            {
                foreach (Purchase purchase in build.Purchase)
                {
                    OverallPointsSpent += purchase.DisplayCost;

                    if (jump.PurchaseTypes[purchase.TypeIndex].IsItemType)
                    {
                        OverallItemsBought++;

                        OverallItemPointsSpent += purchase.DisplayCost;

                        if (purchase.Category == "Warehouse Addon")
                        {
                            OverallWarehouseAddons++;
                        }

                        overallItems[purchase.Category].Total++;
                    }
                    else
                    {
                        OverallPerksBought++;

                        OverallPerkPointsSpent += purchase.DisplayCost;

                        if (purchase.BodyModAddition)
                        {
                            OverallBodyModAddons++;
                        }

                        overallPerks[purchase.Category].Total++;
                    }

                    if (purchase.Attributes.Count != 0)
                    {
                        foreach (PurchaseAttribute attribute in purchase.Attributes)
                        {
                            if (attribute.Category == "1-up")
                            {
                                OverallOneUps += attribute.Value;
                            }
                        }
                    }
                }

                foreach (CompanionPurchase import in build.CompanionPurchase)
                {
                    OverallCompanionPointsSpent += import.DisplayCost;
                }

                foreach (Drawback drawback in build.DrawbackSelection)
                {
                    OverallDrawbackPointsEarned += drawback.Value;
                }

                foreach (Drawback scenario in build.ScenarioSelection)
                {
                    OverallScenarioPointsEarned += scenario.Value;
                }
            }
        }

        private void LoadSingleCharacterData(Jump jump, JumpBuild build)
        {
            Dictionary<string, PurchaseCategoryStatistics> characterPerks = CharacterPerkCategoryTotals.ToDictionary(x => x.Name);
            Dictionary<string, PurchaseCategoryStatistics> characterItems = CharacterItemCategoryTotals.ToDictionary(x => x.Name);

            foreach (Purchase purchase in build.Purchase)
            {
                CharacterPointsSpent += purchase.DisplayCost;

                if (jump.PurchaseTypes[purchase.TypeIndex].IsItemType)
                {
                    CharacterItemsBought++;

                    CharacterItemPointsSpent += purchase.DisplayCost;

                    if (purchase.Category == "Warehouse Addon")
                    {
                        CharacterWarehouseAddons++;
                    }

                    characterItems[purchase.Category].Total++;
                }
                else
                {
                    CharacterPerksBought++;

                    CharacterPerkPointsSpent += purchase.DisplayCost;

                    if (purchase.BodyModAddition)
                    {
                        CharacterBodyModAddons++;
                    }

                    characterPerks[purchase.Category].Total++;
                }

                if (purchase.Attributes.Count != 0)
                {
                    foreach (PurchaseAttribute attribute in purchase.Attributes)
                    {
                        if (attribute.Category == "1-up")
                        {
                            CharacterOneUps += attribute.Value;
                        }
                    }
                }
            }

            foreach (CompanionPurchase import in build.CompanionPurchase)
            {
                CharacterCompanionPointsSpent += import.DisplayCost;
            }

            foreach (Drawback drawback in build.DrawbackSelection)
            {
                CharacterDrawbackPointsEarned += drawback.Value;
            }

            foreach (Drawback scenario in build.ScenarioSelection)
            {
                CharacterScenarioPointsEarned += scenario.Value;
            }
        }

        #endregion

        #region Commands
        [RelayCommand]
        private void RefreshStatistics() => LoadJumpStatistics();
        #endregion
    }
}
