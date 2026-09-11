using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using JumpchainCharacterBuilder.Attributes;
using JumpchainCharacterBuilder.Messages;
using JumpchainCharacterBuilder.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace JumpchainCharacterBuilder.ViewModel
{
    public partial class SearchViewModel : ViewModelBase
    {
        #region Fields
        [ObservableProperty]
        private bool _spellCheckEnabled = true;

        [ObservableProperty]
        private SaveFile _loadedSave = new();
        [ObservableProperty]
        private AppSettingsModel _appSettings = new();

        [ObservableProperty]
        private ObservableCollection<Character> _characterList = [];
        [ObservableProperty]
        private Character _characterSelection = new();
        [ObservableProperty]
        private int _characterSelectionIndex = 0;

        [ObservableProperty]
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
        private ObservableCollection<Purchase> _fullPerkList = [];
        [ObservableProperty]
        private ObservableCollection<Purchase> _fullItemList = [];
        [ObservableProperty]
        private ObservableCollection<Purchase> _searchResults = [];
        [ObservableProperty]
        private bool _searchPerks = true;
        [ObservableProperty]
        private bool _searchItems = true;
        [ObservableProperty]
        private bool _searchName = true;
        [ObservableProperty]
        private bool _searchDescription = true;
        [ObservableProperty]
        private string _searchString = "";
        #endregion

        #region Properties
        partial void OnCharacterSelectionChanged(Character value)
        {
            if (value != null)
            {
                PopulatePerkList();
                if (!string.IsNullOrWhiteSpace(SearchString))
                {
                    SearchLists();
                }
            }
        }

        partial void OnPurchaseSelectionChanged(Purchase value)
        {
            if (value != null)
            {
                PurchaseName = value.Name;
                PurchaseDescription = value.Description;
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

        partial void OnSearchStringChanged(string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                SearchLists();
            }
        }

        #endregion

        #region Constructor
        public SearchViewModel()
        {
            Messenger.Register<SaveDataChangedMessage>(this, (r, m) =>
            {
                LoadCharacterList();
            });
            Messenger.Register<SaveDataSendMessage>(this, (r, m) =>
            {
                LoadedSave = m.Value;
                LoadCharacterList();
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

            LoadedSave = Messenger.Send<SaveRequestMessage>();
            LoadCharacterList();
            AppSettings = Messenger.Send<SettingsRequestMessage>();
            SpellCheckEnabled = AppSettings.SpellCheckEnabled;
        }

        #endregion


        #region Methods
        private void PopulatePerkList()
        {
            FullPerkList.Clear();
            FullItemList.Clear();

            if (CharacterSelection != null)
            {
                int charIndex = CharacterList.IndexOf(CharacterSelection);
                JumpBuild currentBuild;

                foreach (Jump jump in LoadedSave.JumpList)
                {
                    if (charIndex < jump.Build.Count)
                    {
                        currentBuild = jump.Build[charIndex];

                        foreach (Purchase purchase in currentBuild.Purchase)
                        {
                            if (!jump.PurchaseTypes[purchase.TypeIndex].IsItemType && !purchase.IsTemporary)
                            {
                                FullPerkList.Add(purchase);

                                purchase.SourceJump = jump.Name;
                                purchase.SourceCharacter = LoadedSave.CharacterList[charIndex].Name;
                            }
                            else if (jump.PurchaseTypes[purchase.TypeIndex].IsItemType && !purchase.IsTemporary)
                            {
                                FullItemList.Add(purchase);

                                purchase.SourceJump = jump.Name;
                                purchase.SourceCharacter = LoadedSave.CharacterList[charIndex].Name;
                            }
                        }
                    }
                }
            }
        }

        private void SearchLists()
        {
            List<Purchase> searchList = [];

            if (SearchPerks && SearchItems)
            {
                searchList = [.. FullPerkList, .. FullItemList];
            }
            else if (SearchPerks)
            {
                searchList = [.. FullPerkList];
            }
            else if (SearchItems)
            {
                searchList = [.. FullItemList];
            }

            SearchResults = new(searchList.Where(x => (SearchName && SearchDescription ? (x.Name.Contains(SearchString, StringComparison.OrdinalIgnoreCase) ||
                                                                                          x.Description.Contains(SearchString, StringComparison.OrdinalIgnoreCase)) :
                                                       SearchName ? x.Name.Contains(SearchString, StringComparison.OrdinalIgnoreCase) :
                                                       SearchDescription ? x.Description.Contains(SearchString, StringComparison.OrdinalIgnoreCase) :
                                                       false)).OrderBy(x => x.Category, StringComparer.OrdinalIgnoreCase));
        }

        private void LoadCharacterList()
        {
            CharacterList = [.. LoadedSave.CharacterList];

            if (CharacterList.Any())
            {
                CharacterSelection = CharacterList.First();
            }
        }
        #endregion
    }
}
