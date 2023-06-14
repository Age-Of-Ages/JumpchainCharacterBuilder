using CommunityToolkit.Mvvm.ComponentModel;

namespace JumpchainCharacterBuilder.Model
{
    public partial class CompanionImportDetailClass : ObservableRecipient
    {
        /// <summary>
        /// Represents the Companion's name.
        /// </summary>
        [ObservableProperty]
        private string _companionName = "";

        /// <summary>
        /// Represents if the character has been imported or not.
        /// </summary>
        [ObservableProperty]
        private bool _companionSelected = false;

        /// <summary>
        /// Represents the stipend of Choice Points given to the Companion
        /// by this import option.
        /// </summary>
        [ObservableProperty]
        private int _companionOptionValue = 0;

        public CompanionImportDetailClass()
        {

        }

        public CompanionImportDetailClass(string companionName)
        {
            CompanionName = companionName;
        }
    }
}
