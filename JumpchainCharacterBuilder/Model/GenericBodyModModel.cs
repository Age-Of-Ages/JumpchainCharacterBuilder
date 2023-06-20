using CommunityToolkit.Mvvm.ComponentModel;

namespace JumpchainCharacterBuilder.Model
{
    public partial class GenericBodyModModel : DocumentDetail
    {
        /// <summary>
        /// Represents the starting budget for the selected Body Mod Supplement.
        /// </summary>
        [ObservableProperty]
        private int _budget = 1000;

        /// <summary>
        /// Represents whether the generic Body Mod Supplement allows
        /// purchases or not.
        /// If it is a single block of text then this should probably be
        /// left as false.
        /// </summary>
        [ObservableProperty]
        private bool _purchasesAllowed = false;

        /// <summary>
        /// Represents the full description of this Body Mod Supplement.
        /// If this Supplement is just a single block of text then that
        /// information will be stored here.
        /// </summary>
        [ObservableProperty]
        private string _fullDescription = "";

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
