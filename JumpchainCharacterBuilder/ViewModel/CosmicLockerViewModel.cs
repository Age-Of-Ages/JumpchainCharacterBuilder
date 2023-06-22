using CommunityToolkit.Mvvm.ComponentModel;
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
    public partial class CosmicLockerViewModel : ViewModelBase
    {
        #region Fields
        private readonly IDialogService _dialogService;

        [ObservableProperty]
        private SaveFile _loadedSave = new();
        [ObservableProperty]
        private Options _loadedOptions = new();

        [ObservableProperty]
        private ObservableCollection<Character> _characterList = new();
        [ObservableProperty]
        private Character _characterSelection = new();
        [ObservableProperty]
        private int _characterSelectionIndex = 0;

        [ObservableProperty]
        private ObservableCollection<string> _itemTabList = new();
        [ObservableProperty]
        private string _itemTabName = "";
        [ObservableProperty]
        private int _itemTabIndex = 0;

        [ObservableProperty]
        private Dictionary<Character, Dictionary<string, List<Purchase>>> _inactiveItemLists = new();
        [ObservableProperty]
        private Dictionary<string, List<Purchase>> _activeItemLists = new();
        [ObservableProperty]
        private ObservableCollection<Purchase> _currentItemList = new();
        [ObservableProperty]
        private Purchase _itemSelection = new();
        #endregion

        #region Properties

        partial void OnItemTabIndexChanged(int value) => LockerTabChanged();

        partial void OnCharacterSelectionIndexChanged(int value)
        {
            if (value != -1)
            {
                CharacterChanged();
            }
        }

        #endregion

        #region Constructor
        public CosmicLockerViewModel()
        {

        }

        public CosmicLockerViewModel(IDialogService dialogService)
        {
            Messenger.Register<SaveDataChangedMessage>(this, (r, m) =>
            {
                LoadedOptions = LoadedSave.Options;
                CreateCharacterList();
                LoadAllLists();
            });
            Messenger.Register<SaveDataSendMessage>(this, (r, m) =>
            {
                LoadedSave = m.Value;
                LoadedOptions = LoadedSave.Options;

                CreateCharacterList();
                LoadAllLists();
            });
            Messenger.Register<CharacterDeletedMessage>(this, (r, m) =>
            {
                CreateCharacterList();
                LoadAllLists();
            });

            _dialogService = dialogService;
        }

        #endregion

        #region Methods
        private void CreateCharacterList()
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

        private void LoadAllLists()
        {

            int charIndex;

            InactiveItemLists.Clear();
            CurrentItemList.Clear();
            ItemTabList.Clear();

            foreach (Character character in LoadedSave.CharacterList)
            {
                InactiveItemLists.Add(character, new());
            }

            foreach (string category in LoadedSave.ItemCategoryList)
            {
                foreach (KeyValuePair<Character, Dictionary<string, List<Purchase>>> list in InactiveItemLists)
                {
                    list.Value.Add(category, new());
                }

                ItemTabList.Add(category);
            }

            foreach (Jump jump in LoadedSave.JumpList)
            {
                foreach (JumpBuild build in jump.Build)
                {
                    charIndex = jump.Build.IndexOf(build);

                    foreach (Purchase purchase in build.Purchase)
                    {
                        if (jump.PurchaseTypes[purchase.TypeIndex].IsItemType)
                        {
                            InactiveItemLists[CharacterList[charIndex]][purchase.Category].Add(purchase);

                            purchase.SourceJump = jump.Name;
                            purchase.SourceCharacter = LoadedSave.CharacterList[charIndex].Name;
                        }
                    }
                }
            }

            LoadActiveItemLists();

            CharacterChanged();
        }

        private void LoadActiveItemLists()
        {
            if (InactiveItemLists.ContainsKey(CharacterSelection))
            {
                ActiveItemLists = InactiveItemLists[CharacterSelection];

                ItemTabIndex = 0;

                LoadCurrentItemList();
            }
        }

        private void LoadCurrentItemList()
        {
            if (ItemTabName != "" && InactiveItemLists[CharacterSelection].ContainsKey(ItemTabName))
            {
                CurrentItemList = new(InactiveItemLists[CharacterSelection][ItemTabName]);
            }
        }

        private void CharacterChanged()
        {
            LoadActiveItemLists();
        }

        private void LockerTabChanged() => LoadCurrentItemList();

        #endregion

        #region Commands
        [RelayCommand]
        private void RefreshLists()
        {
            CreateCharacterList();
            LoadAllLists();
        }

        #endregion
    }
}
