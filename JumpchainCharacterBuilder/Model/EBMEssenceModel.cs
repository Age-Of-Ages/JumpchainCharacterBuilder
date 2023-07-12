using CommunityToolkit.Mvvm.ComponentModel;

namespace JumpchainCharacterBuilder.Model
{
    /// <summary>
    /// Stores data for an individual Essence for the Essential Body Mod.
    /// </summary>
    public partial class EBMEssence : ObservableValidator
    {
        /// <summary>
        /// Represents the name of the Essence.
        /// </summary>
        [ObservableProperty]
        private string _name = "";

        /// <summary>
        /// Represents the description of the Essence.
        /// </summary>
        [ObservableProperty]
        private string _description = "";
    }
}
