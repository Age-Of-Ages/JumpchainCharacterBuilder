using CommunityToolkit.Mvvm.ComponentModel;

namespace JumpchainCharacterBuilder.Model
{
    /// <summary>
    /// Stores attribute sources.
    /// </summary>
    public partial class AttributeSource : ObservableValidator
    {
        /// <summary>
        /// Represents the name of the purchase that contributes to the attribute.
        /// </summary>
        [ObservableProperty]
        private string _sourcePurchase = "";

        /// <summary>
        /// Represents the name of the Jump that the purchase source belongs to.
        /// </summary>
        [ObservableProperty]
        private string _sourceJump = "";

        /// <summary>
        /// Represents the rank of the purchase source.
        /// </summary>
        [ObservableProperty]
        private string _sourceRank = "";
    }
}
