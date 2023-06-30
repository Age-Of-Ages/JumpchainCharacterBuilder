using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using JumpchainCharacterBuilder.Interfaces;
using JumpchainCharacterBuilder.Messages;
using JumpchainCharacterBuilder.Model;
using System.Collections.ObjectModel;
using System.Linq;

namespace JumpchainCharacterBuilder.ViewModel
{
    public partial class StatisticsViewModel : ViewModelBase
    {
        // TODO - Track per-category totals for both Perks and Items.
        #region Fields
        private readonly IDialogService _dialogService;

        [ObservableProperty]
        private SaveFile _loadedSave = new();
        [ObservableProperty]
        private Options _loadedOptions = new();
        [ObservableProperty]
        private ObservableCollection<Jump> _jumpList = new();

        [ObservableProperty]
        private ObservableCollection<Character> _characterList = new();
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
        private int _overallPerkPointsSpent = 0;
        [ObservableProperty]
        private int _overallItemPointsSpent = 0;
        [ObservableProperty]
        private int _overallCompanionPointsSpent = 0;
        [ObservableProperty]
        private int _overallDrawbackPointsEarned = 0;
        [ObservableProperty]
        private int _overallScenarioPointsEarned = 0;

        [ObservableProperty]
        private int _characterPointsSpent = 0;
        [ObservableProperty]
        private int _characterPerkPointsSpent = 0;
        [ObservableProperty]
        private int _characterItemPointsSpent = 0;
        [ObservableProperty]
        private int _characterCompanionPointsSpent = 0;
        [ObservableProperty]
        private int _characterDrawbackPointsEarned = 0;
        [ObservableProperty]
        private int _characterScenarioPointsEarned = 0;

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

                foreach (Jump jump in JumpList)
                {
                    if (jump.Build.Count > value && jump.Build[value] != null)
                    {
                        LoadSingleCharacterData(jump, jump.Build[CharacterSelectionIndex]);
                    }
                }
            }
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

                LoadCharacterList();
                LoadJumpList();
            });
            Messenger.Register<SaveDataSendMessage>(this, (r, m) =>
            {
                LoadedSave = m.Value;
                LoadedOptions = LoadedSave.Options;

                LoadCharacterList();
                LoadJumpList();
            });
            Messenger.Register<SupplementChangedMessage>(this, (r, m) =>
            {

            });

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
                    }

                    if (purchase.Attributes.Any())
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
                }

                foreach (Drawback scenario in build.ScenarioSelection)
                {
                    OverallScenarioPointsEarned += scenario.Value;
                    CharacterScenarioPointsEarned += scenario.Value;
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
                    }
                    else
                    {
                        OverallPerksBought++;

                        OverallPerkPointsSpent += purchase.DisplayCost;

                        if (purchase.BodyModAddition)
                        {
                            OverallBodyModAddons++;
                        }
                    }

                    if (purchase.Attributes.Any())
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
                }
                else
                {
                    CharacterPerksBought++;

                    CharacterPerkPointsSpent += purchase.DisplayCost;

                    if (purchase.BodyModAddition)
                    {
                        CharacterBodyModAddons++;
                    }
                }

                if (purchase.Attributes.Any())
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
