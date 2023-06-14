using CommunityToolkit.Mvvm.ComponentModel;

namespace JumpchainCharacterBuilder.Model
{
    /// <summary>
    /// Store user-selected options to be loaded on Jumper Load.
    /// </summary>
    public partial class Options : ObservableValidator
    {
        /// <summary>
        /// Stores available Cosmic Warehouse Supplement options.
        /// </summary>
        public enum CosmicWarehouseSupplements
        {
            Generic,
            PersonalReality
        }

        /// <summary>
        /// Stores available Body Mod Supplement options.
        /// </summary>
        public enum BodyModSupplements
        {
            Generic,
            SBBodyMod,
            EssentialBodyMod
        }

        /// <summary>
        /// Stores available Drawback Supplement options.
        /// </summary>
        public enum DrawbackSupplements
        {
            Generic,
            UDS,
            UU
        }

        /// <summary>
        /// Stores available height formats.
        /// </summary>
        public enum HeightFormats
        {
            FeetInches,
            Feet,
            Meters
        }

        /// <summary>
        /// Stores available weight formats.
        /// </summary>
        public enum WeightFormats
        {
            Pounds,
            Kilograms
        }

        /// <summary>
        /// Represents the selection of Cosmic Warehouse Supplement.
        /// </summary>
        [ObservableProperty]
        private CosmicWarehouseSupplements _cosmicWarehouseSetting = CosmicWarehouseSupplements.Generic;

        /// <summary>
        /// Represents the selection of Body Mod Supplement.
        /// </summary>
        [ObservableProperty]
        private BodyModSupplements _bodyModSetting = BodyModSupplements.Generic;

        /// <summary>
        /// Represents the selection of Drawback Supplement.
        /// </summary>
        [ObservableProperty]
        private DrawbackSupplements _drawbackSupplementSetting = DrawbackSupplements.Generic;

        /// <summary>
        /// Represents the default budget of Choice Points for each new
        /// Jump created.
        /// </summary>
        [ObservableProperty]
        private int _defaultBudget = 1000;

        /// <summary>
        /// Represents the default Item Stipend for each new Jump
        /// created.
        /// </summary>
        [ObservableProperty]
        private int _defaultItemStipend = 0;

        /// <summary>
        /// Represents the default Origin discount mode for each new
        /// Jump created.
        /// </summary>
        [ObservableProperty]
        private bool _originDiscounts = false;

        /// <summary>
        /// Represents the default Perk Freebie threshold for each new
        /// Jump created.
        /// </summary>
        [ObservableProperty]
        private int _defaultPerkFreebieThreshold = 100;

        /// <summary>
        /// Represents the default Item Freebie threshold for each new
        /// Jump created.
        /// </summary>
        [ObservableProperty]
        private int _defaultItemFreebieThreshold = 100;

        /// <summary>
        /// Represents whether the point banking system is enabled or not.
        /// </summary>
        [ObservableProperty]
        private bool _allowPointBank = false;

        /// <summary>
        /// Represents if points can be withdrawn from or deposited into the bank during Gauntlets.
        /// </summary>
        [ObservableProperty]
        private bool _allowGauntletBank = false;

        /// <summary>
        /// Represents if Companions are allowed to use the point bank system.
        /// </summary>
        [ObservableProperty]
        private bool _allowCompanionsBank = false;

        /// <summary>
        /// Represents the maximum number of points that can be stored in the bank.
        /// Used for both limiting how many points the Jumper can add to it and how many they can withdraw.
        /// </summary>
        [ObservableProperty]
        private int _pointBankLimit = 300;

        /// <summary>
        /// Represents the maximum number of points that a Companion can store in their bank.
        /// Used for both limiting how many points the Companion can add to it and how many they can withdraw.
        /// </summary>
        [ObservableProperty]
        private int _companionBankLimit = 300;

        /// <summary>
        /// Represents the user-preferred height display format.
        /// </summary>
        [ObservableProperty]
        private HeightFormats _heightFormat = HeightFormats.FeetInches;

        /// <summary>
        /// Represents the user-preferred weight display format.
        /// </summary>
        [ObservableProperty]
        private WeightFormats _weightFormat = WeightFormats.Pounds;

        /// <summary>
        /// Represents options for exporting character details and builds.
        /// </summary>
        [ObservableProperty]
        private ExportOptions _exportOptions = new();
    }
}
