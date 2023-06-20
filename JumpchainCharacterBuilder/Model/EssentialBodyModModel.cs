using CommunityToolkit.Mvvm.ComponentModel;

namespace JumpchainCharacterBuilder.Model
{
    /// <summary>
    /// This stores data for the Essential Body Mod.
    /// </summary>
    public partial class EssentialBodyMod : DocumentDetail
    {
        public enum StartingModes
        {
            Hardcore,
            Standard,
            Heroic
        }

        public enum EssenceModes
        {
            NoEssence,
            SingleEssence,
            DualEssence,
            MultiEssence
        }

        public enum AdvancementModes
        {
            Standard,
            Meteoric,
            Heroic,
            Questing
        }

        public enum EPAccessModes
        {
            NoAccess,
            LesserAccess,
            StandardAccess
        }

        /// <summary>
        /// Represents the starting budget for the selected Body Mod Supplement.
        /// </summary>
        [ObservableProperty]
        private int _budget = 1000;

        /// <summary>
        /// Represents the starting mode selection of the Body Mod.
        /// </summary>
        [ObservableProperty]
        private StartingModes _startingMode = new();

        /// <summary>
        /// Represents the essence mode selection of the Body Mod.
        /// </summary>
        [ObservableProperty]
        private EssenceModes _essenceMode = new();

        /// <summary>
        /// Represents the advancement mode selection of the Body Mod.
        /// </summary>
        [ObservableProperty]
        private AdvancementModes _advancementMode = new();

        /// <summary>
        /// Represents the EP access mode selection of the Body Mod.
        /// </summary>
        [ObservableProperty]
        private EPAccessModes _ePAccessMode = new();

        /// <summary>
        /// Represents whether the above mode should be given the Cumulative
        /// modifier or not.
        /// </summary>
        [ObservableProperty]
        private bool _cumulativeAccess = false;

        /// <summary>
        /// Represents whether the Tempered By Suffering option has been selected.
        /// </summary>
        [ObservableProperty]
        private bool _temperedBySuffering = false;

        /// <summary>
        /// Represents the unbalanced mode selection of the Body Mod.
        /// </summary>
        [ObservableProperty]
        private string _unbalancedMode = "";

        /// <summary>
        /// Represents the description of the selected Unbalanced Mode.
        /// </summary>
        [ObservableProperty]
        private string _unbalancedModeDescription = "";

        /// <summary>
        /// Represents the limiters selection of the Body Mod.
        /// </summary>
        [ObservableProperty]
        private string _limiters = "";

        /// <summary>
        /// Represents the description of the selected Limiter.
        /// </summary>
        [ObservableProperty]
        private string _limiterDescription = "";

        /// <summary>
        /// If the Supplement has an incremental points system then
        /// this value will represent the points gained for each completed
        /// Jump.
        /// </summary>
        [ObservableProperty]
        private int _incrementalBudget = 0;

        /// <summary>
        /// Represents how many Jumps must pass between each deposit of
        /// the incremental budget.
        /// </summary>
        [ObservableProperty]
        private int _incrementalInterval = 1;

        /// <summary>
        /// Represents whether investing to buy more BP is allowed or not.
        /// </summary>
        [ObservableProperty]
        private bool _investmentAllowed = false;

        /// <summary>
        /// Represents the ratio of CP to a single BP, in the format x:1.
        /// </summary>
        [ObservableProperty]
        private int _investmentRatio = 1;
    }
}