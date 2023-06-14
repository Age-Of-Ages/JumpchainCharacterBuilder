using CommunityToolkit.Mvvm.ComponentModel;

namespace JumpchainCharacterBuilder.Model
{
    /// <summary>
    /// Represents an individual dependency for a training booster.
    /// </summary>
    public partial class BoosterDependency : ObservableValidator
    {
        /// <summary>
        /// Represents the dependency source name, for display purposes.
        /// </summary>
        [ObservableProperty]
        private string _dependencySource = "";

        /// <summary>
        /// Represents the source multiplier.
        /// </summary>
        [ObservableProperty]
        private int _dependencyMultiplier = 1;

        /// <summary>
        /// Represents whether this dependency is enabled or not. Enabled dependencies will have their value used as a multiplier.
        /// </summary>
        [ObservableProperty]
        private bool _dependencyEnabled = false;

        public BoosterDependency()
        {

        }

        public BoosterDependency(Booster source)
        {
            DependencySource = source.BoosterName;
            DependencyMultiplier = source.BoosterMultiplier;
        }
    }
}
