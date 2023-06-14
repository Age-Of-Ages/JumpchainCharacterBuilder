using CommunityToolkit.Mvvm.ComponentModel;

namespace JumpchainCharacterBuilder.Model
{
    /// <summary>
    /// Represents unique data for the Universal Universal Supplement.
    /// </summary>
    public partial class UUSupplement : DrawbackSupplementUniversal
    {

        /// <summary>
        /// Represents the Mode selection.
        /// </summary>
        [ObservableProperty]
        private string _mode = "";

        /// <summary>
        /// Represents the Mode description.
        /// </summary>
        [ObservableProperty]
        private string _modeDescription = "";

        /// <summary>
        /// Represents the Risk Level selection.
        /// </summary>
        [ObservableProperty]
        private string _riskLevel = "";

        /// <summary>
        /// Represents the Risk Level description.
        /// </summary>
        [ObservableProperty]
        private string _riskLevelDescription = "";
    }
}
