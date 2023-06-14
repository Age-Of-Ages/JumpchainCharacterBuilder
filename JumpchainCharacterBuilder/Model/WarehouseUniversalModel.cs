using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;

namespace JumpchainCharacterBuilder.Model
{
    /// <summary>
    /// Represents details and purchases that are universal to all Warehouse Supplements.
    /// </summary>
    public partial class WarehouseUniversal : DocumentDetail
    {
        /// <summary>
        /// Represents the starting budget for the selected Warehouse Supplement.
        /// </summary>
        [ObservableProperty]
        private int _budget = 1000;

        /// <summary>
        /// Represents whether investing to buy more WP is allowed or not.
        /// </summary>
        [ObservableProperty]
        private bool _investmentAllowed = false;

        /// <summary>
        /// Represents the ratio of CP to a single WP, in the format x:1.
        /// </summary>
        [ObservableProperty]
        private int _investmentRatio = 1;

        /// <summary>
        /// Represents the number of Warehouse Points purchased through investments.
        /// </summary>
        [ObservableProperty]
        private int _totalInvestment = 0;

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
        /// Represents the number of Jumps before user took this Supplement.
        /// </summary>
        [ObservableProperty]
        private int _supplementDelay = 0;

        /// <summary>
        /// Represents the list of Supplement purchases.
        /// </summary>
        [ObservableProperty]
        private List<SupplementPurchase> _purchases = new();

        /// <summary>
        /// Represents the list of Supplement Drawbacks/Limitations.
        /// </summary>
        [ObservableProperty]
        private List<SupplementDrawbackModel> _limitations = new();

    }
}
