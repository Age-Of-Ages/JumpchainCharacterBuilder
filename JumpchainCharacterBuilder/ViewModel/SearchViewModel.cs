using CommunityToolkit.Mvvm.ComponentModel;
using JumpchainCharacterBuilder.Attributes;
using JumpchainCharacterBuilder.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JumpchainCharacterBuilder.ViewModel
{
    public partial class SearchViewModel : ViewModelBase
    {

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
        private ObservableCollection<Purchase> _fullPerkList = [];
        [ObservableProperty]
        private ObservableCollection<Purchase> _searchResults = [];
        [ObservableProperty]
        private bool _searchPerks = false;
        [ObservableProperty]
        private bool _searchItems = false;
        [ObservableProperty]
        private bool _searchName = false;
        [ObservableProperty]
        private bool _searchDescription = false;
        [ObservableProperty]
        private string _searchString = "";
    }
}
