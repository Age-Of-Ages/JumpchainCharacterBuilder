using CommunityToolkit.Mvvm.ComponentModel;

namespace JumpchainCharacterBuilder.Model
{
    /// <summary>
    /// Represents data stored for the Personal Reality Supplement.
    /// </summary>
    public partial class PersonalReality : WarehouseUniversal
    {
        /// <summary>
        /// Stores the list of available Core Mode options.
        /// </summary>
        public enum CoreModes
        {
            Upfront,
            Incremental,
            Unlimited,
            Reasonable,
            Therehouse
        }

        /// <summary>
        /// Represents the Core Mode selected by the user.
        /// </summary>
        [ObservableProperty]
        private CoreModes _coreMode = CoreModes.Upfront;

        /// <summary>
        /// Represents the user-provided description for the Core Mode of
        /// this Supplement.
        /// </summary>
        [ObservableProperty]
        private string _coreModeDescription = "Insert Core Mode description here.";

        /// <summary>
        /// Represents if the Patient Jumper extra mode was selected.
        /// </summary>
        [ObservableProperty]
        private bool _patientJumper = false;

        /// <summary>
        /// Represents if the first Jump should be counted for the Patient Jumper extra mode.
        /// </summary>
        [ObservableProperty]
        private bool _patientJumperCountFirstJump = false;

        /// <summary>
        /// Represents the multiplier for total warehouse dimensions.
        /// </summary>
        [ObservableProperty]
        private int _expansionMultiplier = 1;

        public PersonalReality()
        {
            InvestmentRatio = 25;
        }
    }
}
