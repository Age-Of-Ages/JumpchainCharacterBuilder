using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;

namespace JumpchainCharacterBuilder.Model
{
    /// <summary>
    /// Represents an individual Augment for the SB Body Mod.
    /// </summary>
    public partial class AugmentPurchase : SupplementPurchase
    {
        /// <summary>
        /// Represents the available Augment levels.
        /// </summary>
        [ObservableProperty]
        private List<int> _augmentLevelList =
        [
            0,
            1,
            2,
            3
        ];

        /// <summary>
        /// Represents the chosen level of this Augment.
        /// </summary>
        [ObservableProperty]
        private int _augmentLevel = 0;

        public AugmentPurchase()
        {

        }

        public AugmentPurchase(int level, string name)
        {
            AugmentLevel = level;
            Name = name;
        }
    }
}
