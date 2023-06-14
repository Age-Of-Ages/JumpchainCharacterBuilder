using CommunityToolkit.Mvvm.ComponentModel;

namespace JumpchainCharacterBuilder.Model
{
    public partial class AltFormTraitModel : ObservableValidator
    {
        /// <summary>
        /// Represents a strength associated with an Alt-Form.
        /// </summary>
        [ObservableProperty]
        private string _strength = "";

        /// <summary>
        /// Represents a weakness associated with an Alt-Form.
        /// </summary>
        [ObservableProperty]
        private string _weakness = "";
    }
}
