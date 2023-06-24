using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace JumpchainCharacterBuilder.Model
{
    /// <summary>
    /// Stores all Jumper data for easy access and saving.
    /// </summary>
    public partial class SaveFile : ObservableValidator
    {
        /// <summary>
        /// A list of all Jumps stored in this save file.
        /// </summary>
        [ObservableProperty]
        [DataMember]
        private List<Jump> _jumpList = new();

        /// <summary>
        /// A list of all characters stored in this save file, including the Jumper.
        /// </summary>
        [ObservableProperty]
        [DataMember]
        private List<Character> _characterList = new();

        /// <summary>
        /// Stored user-selected options.
        /// </summary>
        [ObservableProperty]
        [DataMember]
        private Options _options = new();

        /// <summary>
        /// Represents settings for the Generic Body Mod.
        /// </summary>
        [ObservableProperty]
        [DataMember]
        private GenericBodyModModel _genericBodyMod = new();

        /// <summary>
        /// Represents settings for the SB Body Mod.
        /// </summary>
        [ObservableProperty]
        [DataMember]
        private SBBodyMod _sBBodyMod = new();

        /// <summary>
        /// Represents settings for the Essential Body Mod.
        /// </summary>
        [ObservableProperty]
        [DataMember]
        private EssentialBodyMod _essentialBodyMod = new();

        /// <summary>
        /// Represents settings for the Generic Warehouse Supplement.
        /// </summary>
        [ObservableProperty]
        [DataMember]
        private GenericWarehouseModel _genericWarehouse = new();

        /// <summary>
        /// Represents settings for the Personal Reality Supplement.
        /// </summary>
        [ObservableProperty]
        [DataMember]
        private PersonalReality _personalReality = new();

        /// <summary>
        /// Represents settings for the Generic Drawback Supplement.
        /// </summary>
        [ObservableProperty]
        [DataMember]
        private GenericDrawbackSupplementModel _genericDrawbackSupplement = new();

        /// <summary>
        /// Represents settings for the Universal Drawback Supplement.
        /// </summary>
        [ObservableProperty]
        [DataMember]
        private UniversalDrawbackSupplement _universalDrawbackSupplement = new();

        /// <summary>
        /// Represents settings for the Universal Universal Supplement.
        /// </summary>
        [ObservableProperty]
        [DataMember]
        private UUSupplement _uUSupplement = new();

        /// <summary>
        /// Represents the base list of Perk categories.
        /// </summary>
        public readonly List<string> BasePerkCategoryList = new()
        {
            "Physical",
            "Mental",
            "Social",
            "Stealth",
            "Magical",
            "Spiritual",
            "Technological",
            "Crafting",
            "Meta",
            "Other Perk"
        };

        /// <summary>
        /// Represents the base list of Item categories.
        /// </summary>
        public readonly List<string> BaseItemCategoryList = new()
        {
            "Weapons",
            "Armor",
            "Accessories",
            "Clothing",
            "Misc. Equipment",
            "Tools",
            "Materials",
            "Food",
            "Media",
            "Wealth",
            "Vehicles",
            "Properties",
            "Businesses",
            "Creatures",
            "Warehouse Addon",
            "Other Item"
        };

        /// <summary>
        /// Represents the users custom Perk categories
        /// </summary>
        [ObservableProperty]
        [DataMember]
        private List<string> _userPerkCategoryList = new();

        /// <summary>
        /// Represents the user's custom Item categories.
        /// </summary>
        [ObservableProperty]
        [DataMember]
        private List<string> _userItemCategoryList = new();

        /// <summary>
        /// Represents the compiled list of Perk categories.
        /// </summary>
        [ObservableProperty]
        [DataMember]
        private List<string> _perkCategoryList = new()
        {
            "Physical",
            "Mental",
            "Social",
            "Stealth",
            "Magical",
            "Spiritual",
            "Technological",
            "Crafting",
            "Meta",
            "Other Perk"
        };

        /// <summary>
        /// Represents the compiled list of Item categories.
        /// </summary>
        [ObservableProperty]
        [DataMember]
        private List<string> _itemCategoryList = new()
        {
            "Weapons",
            "Armor",
            "Accessories",
            "Clothing",
            "Misc. Equipment",
            "Tools",
            "Materials",
            "Food",
            "Media",
            "Wealth",
            "Vehicles",
            "Properties",
            "Businesses",
            "Creatures",
            "Warehouse Addon",
            "Other Item"
        };

        /// <summary>
        /// Represents the program version that this save was made on, for save-updating purposes.
        /// </summary>
        [ObservableProperty]
        [DataMember]
        private double _saveVersion = 1.1;

        public SaveFile()
        {
            CharacterList.Add(new Character("Jumper"));
            JumpList.Add(new("First Jump"));
        }

        public SaveFile(List<Jump> jumpList, List<Character> characterList, Options options)
        {
            JumpList = jumpList;
            CharacterList = characterList;
            Options = options;
        }
    }
}
