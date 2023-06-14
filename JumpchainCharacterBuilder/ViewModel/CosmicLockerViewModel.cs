using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using JumpchainCharacterBuilder.Messages;
using JumpchainCharacterBuilder.Model;
using JumpchainCharacterBuilder.Services;
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
        private int _lockerTabIndex = 0;

        [ObservableProperty]
        private Purchase _itemSelection = new();

        [ObservableProperty]
        private ObservableCollection<ObservableCollection<Purchase>> _inactiveWeaponsItemLists = new();
        [ObservableProperty]
        private ObservableCollection<Purchase> _weaponsItemList = new();
        [ObservableProperty]
        private int _weaponsItemSelectionIndex = 0;
        [ObservableProperty]
        private ObservableCollection<ObservableCollection<Purchase>> _inactiveArmorItemLists = new();
        [ObservableProperty]
        private ObservableCollection<Purchase> _armorItemList = new();
        [ObservableProperty]
        private int _armorItemSelectionIndex = 0;
        [ObservableProperty]
        private ObservableCollection<ObservableCollection<Purchase>> _inactiveAccessoriesItemLists = new();
        [ObservableProperty]
        private ObservableCollection<Purchase> _accessoriesItemList = new();
        [ObservableProperty]
        private int _accessoriesItemSelectionIndex = 0;
        [ObservableProperty]
        private ObservableCollection<ObservableCollection<Purchase>> _inactiveClothingItemLists = new();
        [ObservableProperty]
        private ObservableCollection<Purchase> _clothingItemList = new();
        [ObservableProperty]
        private int _clothingItemSelectionIndex = 0;
        [ObservableProperty]
        private ObservableCollection<ObservableCollection<Purchase>> _inactiveMiscEquipItemLists = new();
        [ObservableProperty]
        private ObservableCollection<Purchase> _miscEquipItemList = new();
        [ObservableProperty]
        private int _miscEquipItemSelectionIndex = 0;
        [ObservableProperty]
        private ObservableCollection<ObservableCollection<Purchase>> _inactiveToolsItemLists = new();
        [ObservableProperty]
        private ObservableCollection<Purchase> _toolsItemList = new();
        [ObservableProperty]
        private int _toolsItemSelectionIndex = 0;
        [ObservableProperty]
        private ObservableCollection<ObservableCollection<Purchase>> _inactiveMaterialsItemLists = new();
        [ObservableProperty]
        private ObservableCollection<Purchase> _materialsItemList = new();
        [ObservableProperty]
        private int _materialsItemSelectionIndex = 0;
        [ObservableProperty]
        private ObservableCollection<ObservableCollection<Purchase>> _inactiveFoodItemLists = new();
        [ObservableProperty]
        private ObservableCollection<Purchase> _foodItemList = new();
        [ObservableProperty]
        private int _foodItemSelectionIndex = 0;
        [ObservableProperty]
        private ObservableCollection<ObservableCollection<Purchase>> _inactiveMediaItemLists = new();
        [ObservableProperty]
        private ObservableCollection<Purchase> _mediaItemList = new();
        [ObservableProperty]
        private int _mediaItemSelectionIndex = 0;
        [ObservableProperty]
        private ObservableCollection<ObservableCollection<Purchase>> _inactiveWealthItemLists = new();
        [ObservableProperty]
        private ObservableCollection<Purchase> _wealthItemList = new();
        [ObservableProperty]
        private int _wealthItemSelectionIndex = 0;
        [ObservableProperty]
        private ObservableCollection<ObservableCollection<Purchase>> _inactiveVehiclesItemLists = new();
        [ObservableProperty]
        private ObservableCollection<Purchase> _vehiclesItemList = new();
        [ObservableProperty]
        private int _vehiclesItemSelectionIndex = 0;
        [ObservableProperty]
        private ObservableCollection<ObservableCollection<Purchase>> _inactivePropertiesItemLists = new();
        [ObservableProperty]
        private ObservableCollection<Purchase> _propertiesItemList = new();
        [ObservableProperty]
        private int _propertiesItemSelectionIndex = 0;
        [ObservableProperty]
        private ObservableCollection<ObservableCollection<Purchase>> _inactiveBusinessItemLists = new();
        [ObservableProperty]
        private ObservableCollection<Purchase> _businessItemList = new();
        [ObservableProperty]
        private int _businessItemSelectionIndex = 0;
        [ObservableProperty]
        private ObservableCollection<ObservableCollection<Purchase>> _inactiveCreaturesItemLists = new();
        [ObservableProperty]
        private ObservableCollection<Purchase> _creaturesItemList = new();
        [ObservableProperty]
        private int _creaturesItemSelectionIndex = 0;
        [ObservableProperty]
        private ObservableCollection<ObservableCollection<Purchase>> _inactiveOtherItemLists = new();
        [ObservableProperty]
        private ObservableCollection<Purchase> _otherItemList = new();
        [ObservableProperty]
        private int _otherItemSelectionIndex = 0;

        #endregion

        #region Properties

        partial void OnLockerTabIndexChanged(int value)
        {
            LockerTabChanged();
        }

        partial void OnCharacterSelectionIndexChanged(int value)
        {
            if (value != -1)
            {
                CharacterChanged(value);
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
            // Clean and regenerate existing lists to ensure proper indexing.
            InactiveWeaponsItemLists.Clear();
            InactiveArmorItemLists.Clear();
            InactiveAccessoriesItemLists.Clear();
            InactiveClothingItemLists.Clear();
            InactiveMiscEquipItemLists.Clear();
            InactiveToolsItemLists.Clear();
            InactiveMaterialsItemLists.Clear();
            InactiveFoodItemLists.Clear();
            InactiveMediaItemLists.Clear();
            InactiveWealthItemLists.Clear();
            InactiveVehiclesItemLists.Clear();
            InactivePropertiesItemLists.Clear();
            InactiveBusinessItemLists.Clear();
            InactiveCreaturesItemLists.Clear();
            InactiveOtherItemLists.Clear();

            int charCount = LoadedSave.CharacterList.Count;

            for (int i = 0; i < charCount; i++)
            {
                InactiveWeaponsItemLists.Add(new ObservableCollection<Purchase>());
                InactiveArmorItemLists.Add(new ObservableCollection<Purchase>());
                InactiveAccessoriesItemLists.Add(new ObservableCollection<Purchase>());
                InactiveClothingItemLists.Add(new ObservableCollection<Purchase>());
                InactiveMiscEquipItemLists.Add(new ObservableCollection<Purchase>());
                InactiveToolsItemLists.Add(new ObservableCollection<Purchase>());
                InactiveMaterialsItemLists.Add(new ObservableCollection<Purchase>());
                InactiveFoodItemLists.Add(new ObservableCollection<Purchase>());
                InactiveMediaItemLists.Add(new ObservableCollection<Purchase>());
                InactiveWealthItemLists.Add(new ObservableCollection<Purchase>());
                InactiveVehiclesItemLists.Add(new ObservableCollection<Purchase>());
                InactivePropertiesItemLists.Add(new ObservableCollection<Purchase>());
                InactiveBusinessItemLists.Add(new ObservableCollection<Purchase>());
                InactiveCreaturesItemLists.Add(new ObservableCollection<Purchase>());
                InactiveOtherItemLists.Add(new ObservableCollection<Purchase>());
            }

            int charIndex;

            foreach (Jump jump in LoadedSave.JumpList)
            {
                foreach (JumpBuild build in jump.Build)
                {
                    charIndex = jump.Build.IndexOf(build);

                    foreach (Purchase purchase in build.Purchase)
                    {
                        switch (purchase.Category)
                        {
                            case "Weapons":
                                InactiveWeaponsItemLists[charIndex].Add(purchase);
                                break;
                            case "Armor":
                                InactiveArmorItemLists[charIndex].Add(purchase);
                                break;
                            case "Accessories":
                                InactiveAccessoriesItemLists[charIndex].Add(purchase);
                                break;
                            case "Clothing":
                                InactiveClothingItemLists[charIndex].Add(purchase);
                                break;
                            case "Misc. Equipment":
                                InactiveMiscEquipItemLists[charIndex].Add(purchase);
                                break;
                            case "Tools":
                                InactiveToolsItemLists[charIndex].Add(purchase);
                                break;
                            case "Materials":
                                InactiveMaterialsItemLists[charIndex].Add(purchase);
                                break;
                            case "Food":
                                InactiveFoodItemLists[charIndex].Add(purchase);
                                break;
                            case "Media":
                                InactiveMediaItemLists[charIndex].Add(purchase);
                                break;
                            case "Wealth":
                                InactiveWealthItemLists[charIndex].Add(purchase);
                                break;
                            case "Vehicles":
                                InactiveVehiclesItemLists[charIndex].Add(purchase);
                                break;
                            case "Properties":
                                InactivePropertiesItemLists[charIndex].Add(purchase);
                                break;
                            case "Businesses":
                                InactiveBusinessItemLists[charIndex].Add(purchase);
                                break;
                            case "Creatures":
                                InactiveCreaturesItemLists[charIndex].Add(purchase);
                                break;
                            case "Other Item":
                                InactiveOtherItemLists[charIndex].Add(purchase);
                                break;
                            default:
                                break;
                        }

                        purchase.SourceJump = jump.Name;
                        purchase.SourceCharacter = LoadedSave.CharacterList[charIndex].Name;
                    }
                }
            }

            CharacterChanged(0);
        }

        private void CharacterChanged(int charIndex)
        {
            WeaponsItemList = InactiveWeaponsItemLists[charIndex];
            ArmorItemList = InactiveArmorItemLists[charIndex];
            AccessoriesItemList = InactiveAccessoriesItemLists[charIndex];
            ClothingItemList = InactiveClothingItemLists[charIndex];
            MiscEquipItemList = InactiveMiscEquipItemLists[charIndex];
            ToolsItemList = InactiveToolsItemLists[charIndex];
            MaterialsItemList = InactiveMaterialsItemLists[charIndex];
            FoodItemList = InactiveFoodItemLists[charIndex];
            MediaItemList = InactiveMediaItemLists[charIndex];
            WealthItemList = InactiveWealthItemLists[charIndex];
            VehiclesItemList = InactiveVehiclesItemLists[charIndex];
            PropertiesItemList = InactivePropertiesItemLists[charIndex];
            BusinessItemList = InactiveBusinessItemLists[charIndex];
            CreaturesItemList = InactiveCreaturesItemLists[charIndex];
            OtherItemList = InactiveOtherItemLists[charIndex];

            LockerTabIndex = 0;
        }

        private void LockerTabChanged()
        {
            switch (LockerTabIndex)
            {
                case 0:
                    if (WeaponsItemList.Any())
                    {
                        WeaponsItemSelectionIndex = 0;
                        ItemSelection = WeaponsItemList.First();
                    }
                    break;
                case 1:
                    if (ArmorItemList.Any())
                    {
                        ArmorItemSelectionIndex = 0;
                        ItemSelection = ArmorItemList.First();
                    }
                    break;
                case 2:
                    if (AccessoriesItemList.Any())
                    {
                        AccessoriesItemSelectionIndex = 0;
                        ItemSelection = AccessoriesItemList.First();
                    }
                    break;
                case 3:
                    if (ClothingItemList.Any())
                    {
                        ClothingItemSelectionIndex = 0;
                        ItemSelection = ClothingItemList.First();
                    }
                    break;
                case 4:
                    if (MiscEquipItemList.Any())
                    {
                        MiscEquipItemSelectionIndex = 0;
                        ItemSelection = MiscEquipItemList.First();
                    }
                    break;
                case 5:
                    if (ToolsItemList.Any())
                    {
                        ToolsItemSelectionIndex = 0;
                        ItemSelection = ToolsItemList.First();
                    }
                    break;
                case 6:
                    if (MaterialsItemList.Any())
                    {
                        MaterialsItemSelectionIndex = 0;
                        ItemSelection = MaterialsItemList.First();
                    }
                    break;
                case 7:
                    if (FoodItemList.Any())
                    {
                        FoodItemSelectionIndex = 0;
                        ItemSelection = FoodItemList.First();
                    }
                    break;
                case 8:
                    if (MediaItemList.Any())
                    {
                        MediaItemSelectionIndex = 0;
                        ItemSelection = MediaItemList.First();
                    }
                    break;
                case 9:
                    if (WealthItemList.Any())
                    {
                        WealthItemSelectionIndex = 0;
                        ItemSelection = WealthItemList.First();
                    }
                    break;
                case 10:
                    if (VehiclesItemList.Any())
                    {
                        VehiclesItemSelectionIndex = 0;
                        ItemSelection = VehiclesItemList.First();
                    }
                    break;
                case 11:
                    if (PropertiesItemList.Any())
                    {
                        PropertiesItemSelectionIndex = 0;
                        ItemSelection = PropertiesItemList.First();
                    }
                    break;
                case 12:
                    if (BusinessItemList.Any())
                    {
                        BusinessItemSelectionIndex = 0;
                        ItemSelection = BusinessItemList.First();
                    }
                    break;
                case 13:
                    if (CreaturesItemList.Any())
                    {
                        CreaturesItemSelectionIndex = 0;
                        ItemSelection = CreaturesItemList.First();
                    }
                    break;
                case 14:
                    if (OtherItemList.Any())
                    {
                        OtherItemSelectionIndex = 0;
                        ItemSelection = OtherItemList.First();
                    }
                    break;
                default:
                    break;
            }
        }

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
