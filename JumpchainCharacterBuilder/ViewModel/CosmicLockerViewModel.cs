using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using JumpchainCharacterBuilder.Attributes;
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
        private bool _spellCheckEnabled = true;

        [ObservableProperty]
        private SaveFile _loadedSave = new();
        [ObservableProperty]
        private Options _loadedOptions = new();
        [ObservableProperty]
        private AppSettingsModel _appSettings = new();

        [ObservableProperty]
        private ObservableCollection<Character> _characterList = [];
        [ObservableProperty]
        private Character _characterSelection = new();
        [ObservableProperty]
        private int _characterSelectionIndex = 0;

        [ObservableProperty]
        private ObservableCollection<string> _itemTabList = [];
        [ObservableProperty]
        private string _itemTabName = "";
        [ObservableProperty]
        private int _itemTabIndex = 0;

        [ObservableProperty]
        private Dictionary<Character, Dictionary<string, List<Purchase>>> _inactiveItemLists = [];
        [ObservableProperty]
        private Dictionary<string, List<Purchase>> _activeItemLists = [];
        [ObservableProperty]
        private ObservableCollection<Purchase> _currentItemList = [];
        [ObservableProperty]
        private Purchase _itemSelection = new();
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _itemName = "";
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [XmlFilter]
        private string _itemDescription = "";
        #endregion

        #region Properties

        partial void OnItemTabNameChanged(string value) => LockerTabChanged();

        partial void OnCharacterSelectionChanged(Character value)
        {
            if (value != null)
            {
                CharacterChanged();
            }
        }

        partial void OnItemSelectionChanged(Purchase value)
        {
            if (value != null)
            {
                ItemName = value.Name;
                ItemDescription = value.Description;
            }
        }

        partial void OnItemNameChanged(string value)
        {
            if (!GetErrors(nameof(ItemName)).Any() && ItemSelection != null)
            {
                ItemSelection.Name = value;
            }
        }

        partial void OnItemDescriptionChanged(string value)
        {
            if (!GetErrors(nameof(ItemDescription)).Any() && ItemSelection != null)
            {
                ItemSelection.Description = value;
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
                InactiveItemLists.Add(character, []);
            }

            foreach (string category in LoadedSave.ItemCategoryList)
            {
                foreach (KeyValuePair<Character, Dictionary<string, List<Purchase>>> list in InactiveItemLists)
                {
                    list.Value.Add(category, []);
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
                        if (jump.PurchaseTypes[purchase.TypeIndex].IsItemType && !purchase.IsTemporary)
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
            if (InactiveItemLists.TryGetValue(CharacterSelection, out Dictionary<string, List<Purchase>>? value))
            {
                ActiveItemLists = value;

                ItemTabIndex = 0;

                LoadCurrentItemList();
            }
        }

        private void LoadCurrentItemList()
        {
            if (!string.IsNullOrWhiteSpace(ItemTabName) && InactiveItemLists[CharacterSelection].TryGetValue(ItemTabName, out List<Purchase>? value))
            {
                CurrentItemList = new(value);
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
