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

        public enum EPAccessModifiers
        {
            None,
            Cumulative,
            RetroCumulative
        }

        public enum UnbalancedVariantModes
        {
            None,
            Harmonized,
            VeryHarmonized,
            PerfectlyHarmonized
        }

        public enum Limiters
        {
            None,
            EverydayHero,
            StreetLevel,
            MidLevel,
            BodyMod,
            ScalingI,
            ScalingII,
            Vanishing
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
        /// Represents if the Unlockable modifier has been selected for the Essence Mode.
        /// </summary>
        [ObservableProperty]
        private bool _unlockableEssenceModifier = false;

        /// <summary>
        /// Represents whether investment should be limited to the values allowed by the selected Access Mode
        /// and cumulative/retroactive if those options are selected.
        /// </summary>
        [ObservableProperty]
        private bool _limitInvestment = false;

        /// <summary>
        /// Represents what modifier the EP Access Mode should be given, if any.
        /// </summary>
        [ObservableProperty]
        private EPAccessModifiers _ePAccessModifier = new();

        /// <summary>
        /// Represents if the Training Allowance Variant Mode has been selected.
        /// </summary>
        [ObservableProperty]
        private bool _trainingAllowance = false;

        /// <summary>
        /// Represents if the Tempered By Suffering Variant Mode has been selected.
        /// </summary>
        [ObservableProperty]
        private bool _temperedBySuffering = false;

        /// <summary>
        /// Represents what Unbalanced Mode should be used, if any.
        /// </summary>
        [ObservableProperty]
        private UnbalancedVariantModes _unbalancedVariantMode = new();

        /// <summary>
        /// Represents the description of the selected Unbalanced Mode.
        /// </summary>
        [ObservableProperty]
        private string _unbalancedModeDescription = "";

        /// <summary>
        /// Represents the limiter selection of the Body Mod.
        /// </summary>
        [ObservableProperty]
        private Limiters _limiter = new();

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