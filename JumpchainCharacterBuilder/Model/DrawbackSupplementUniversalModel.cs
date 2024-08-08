using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;

namespace JumpchainCharacterBuilder.Model
{
    /// <summary>
    /// This is a universal template for all Drawback Supplements, which
    /// configures details and the list of Drawback selections.
    /// </summary>
    public partial class DrawbackSupplementUniversal : DocumentDetail
    {
        /// <summary>
        /// Represents total CP gained from this Supplement.
        /// </summary>
        [ObservableProperty]
        private int _cPGained = 0;

        /// <summary>
        /// Represents CP gained for Companions from this Supplement.
        /// </summary>
        [ObservableProperty]
        private int _cPCompanionGained = 0;

        /// <summary>
        /// Represents Item-only CP gained from this Supplement.
        /// </summary>
        [ObservableProperty]
        private int _cPItemGained = 0;

        /// <summary>
        /// Represents Warehouse Points gained from this Supplement.
        /// </summary>
        [ObservableProperty]
        private int _wPGained = 0;

        /// <summary>
        /// Represents whether certain Drawbacks can apply their points during Gauntlets or not.
        /// </summary>
        [ObservableProperty]
        private bool _allowedDuringGauntlets = false;

        /// <summary>
        /// If the Supplement gives points during Gauntlets then this determines if those Drawbacks give half points or not.
        /// </summary>
        [ObservableProperty]
        private bool _halvedPointsDuringGauntlets = false;

        /// <summary>
        /// Represents the list of Drawback Supplement selections.
        /// </summary>
        [ObservableProperty]
        private List<DrawbackSupplementPurchase> _purchases = [];

        /// <summary>
        /// Represents the list of House Rules.
        /// </summary>
        [ObservableProperty]
        private List<HouseRuleModel> _houseRules = [];
    }
}
