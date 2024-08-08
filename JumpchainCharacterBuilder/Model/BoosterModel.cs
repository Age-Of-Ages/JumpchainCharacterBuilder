using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;

namespace JumpchainCharacterBuilder.Model
{
    /// <summary>
    /// Represents data for training/learning boosters.
    /// </summary>
    public partial class Booster : ObservableValidator
    {
        /// <summary>
        /// Represents the name of the booster, as a shorthand for what it affects.
        /// </summary>
        [ObservableProperty]
        private string _boosterName = "";

        /// <summary>
        /// Represents a description of the boost and what it applies to.
        /// </summary>
        [ObservableProperty]
        private string _boosterDescription = "";

        /// <summary>
        /// Represents the final multiplier for training speed.
        /// </summary>
        [ObservableProperty]
        private int _boosterMultiplier = 1;

        /// <summary>
        /// Represents the raw value of the Booster, used for Dependants.
        /// </summary>
        [ObservableProperty]
        private int _boosterRaw = 1;

        /// <summary>
        /// Represents a list of all dependancies, and uses their raw value to determine
        /// final multiplier.
        /// </summary>
        [ObservableProperty]
        private List<BoosterDependency> _boosterDependencies = [];

        /// <summary>
        /// Represents the names of the purchases that contribute to this attribute.
        /// </summary>
        [ObservableProperty]
        private List<AttributeSource> _sourceList = [];

        public Booster()
        {

        }

        public Booster(string name)
        {
            BoosterName = name;
        }
    }
}
