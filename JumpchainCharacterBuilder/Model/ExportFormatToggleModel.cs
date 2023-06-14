using CommunityToolkit.Mvvm.ComponentModel;

namespace JumpchainCharacterBuilder.Model
{
    public partial class ExportFormatToggle : ObservableValidator
    {
        /// <summary>
        /// Represents the displayed name of the toggle.
        /// </summary>
        [ObservableProperty]
        private string _name = "";


        [ObservableProperty]
        private bool _enabled = false;

        public ExportFormatToggle()
        {

        }

        public ExportFormatToggle(string displayName, bool isEnabled)
        {
            Name = displayName;
            Enabled = isEnabled;
        }
    }
}
