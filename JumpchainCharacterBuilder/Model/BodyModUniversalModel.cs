using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;

namespace JumpchainCharacterBuilder.Model
{
    /// <summary>
    /// Stores each character's Body Mod details.
    /// </summary>
    public partial class BodyModUniversal : ObservableValidator
    {
        /// <summary>
        /// Represents purchases for the Body Mod Supplement.
        /// </summary>
        [ObservableProperty]
        private List<SupplementPurchase> _purchases = new();

        /// <summary>
        /// Represents the list of Supplement Drawbacks/Limitations.
        /// </summary>
        [ObservableProperty]
        private List<SupplementDrawbackModel> _limitations = new();

        /// <summary>
        /// Represents the number of Jumps before user took this Supplement.
        /// </summary>
        [ObservableProperty]
        private int _supplementDelay = 0;

        /// <summary>
        /// Represents the number of Body-Mod Points purchased through investments.
        /// </summary>
        [ObservableProperty]
        private int _totalInvestment = 0;

        // Below are sections specifically for the SB Body Mod, that are specific to the character.
        /// <summary>
        /// Represents the list of SB Body Mod base form details.
        /// </summary>
        [ObservableProperty]
        private List<SupplementPurchase> _baseFormDetails = new()
        {
            {new("", "Sex") },
            {new("", "Proportions") },
            {new("", "Hair") },
            {new("", "Color") }
        };

        /// <summary>
        /// Represents the list of extra bits for the SB Body Mod.
        /// </summary>
        [ObservableProperty]
        private List<SupplementPurchase> _extraBitsList = new();

        /// <summary>
        /// Represents the number of points invested into Augments by
        /// the character.
        /// </summary>
        [ObservableProperty]
        private int _augmentInvestment = 0;

        /// <summary>
        /// Represents the number of Anomalous levels taken.
        /// </summary>
        [ObservableProperty]
        private int _anomalousLevel = 0;

        /// <summary>
        /// Represents the description for the selected Anomalous level.
        /// </summary>
        [ObservableProperty]
        private string _anomalousDescription = "";

        /// <summary>
        /// Represents if the Ascension discount is in use or not.
        /// </summary>
        [ObservableProperty]
        private bool _ascensionDiscount = false;

        /// <summary>
        /// Represents the index of the selected Affinity.
        /// </summary>
        [ObservableProperty]
        private int _affinityIndex = 0;

        /// <summary>
        /// Represents the description of the selected Affinity.
        /// </summary>
        [ObservableProperty]
        private string _affinityDescription = "";

        /// <summary>
        /// Represents the Body Augments.
        /// </summary>
        [ObservableProperty]
        private List<AugmentPurchase> _bodyAugmentList = new()
        {
            {new(0, "Strength") },
            {new(0, "Endurance") },
            {new(0, "Speed") },
            {new(0, "Resilience") },
            {new(0, "Reflex") }
        };

        /// <summary>
        /// Represents the Mind Augments.
        /// </summary>
        [ObservableProperty]
        private List<AugmentPurchase> _mindAugmentList = new()
        {
            {new(0, "Logic") },
            {new(0, "Memory") },
            {new(0, "Resolve") },
            {new(0, "Coordination") },
            {new(0, "Perception") }
        };

        /// <summary>
        /// Represents the Heart Augments.
        /// </summary>
        [ObservableProperty]
        private List<AugmentPurchase> _heartAugmentList = new()
        {
            {new(0, "Charisma") },
            {new(0, "Appeal") },
            {new(0, "Empathy") },
            {new(0, "Flexibility") },
            {new(0, "Fertility") }
        };

        /// <summary>
        /// Represents the list of Power purchases for the SB Body Mod.
        /// </summary>
        [ObservableProperty]
        private List<SupplementPurchase> _sBPowerList = new();

        /// <summary>
        /// Represents the number of Gauntlets finished before taking the SB Body Mod
        /// Supplement.
        /// </summary>
        [ObservableProperty]
        private int _gauntletsFinished = 0;


        // Below are sections specifically for the Essential Body Mod, that are specific to the character.
        /// <summary>
        /// Represents the number of Minor Quests completed.
        /// </summary>
        [ObservableProperty]
        private int _eBMMinorQuests = 0;

        /// <summary>
        /// Represents the number of Major Quests completed.
        /// </summary>
        [ObservableProperty]
        private int _eBMMajorQuests = 0;

        /// <summary>
        /// Represents the Essence(s) selected for the Essential Body Mod.
        /// </summary>
        [ObservableProperty]
        private List<EBMEssence> _eBMEssenceList = new();

        /// <summary>
        /// Represents the list of purchases for the EBM.
        /// </summary>
        [ObservableProperty]
        private List<SupplementPurchase> _eBMPurchaseList = new();

        /// <summary>
        /// Represents the list of EBM Drawbacks.
        /// </summary>
        [ObservableProperty]
        private List<SupplementDrawbackModel> _eBMDrawbackList = new();

        public BodyModUniversal()
        {

        }

    }
}
