using CommunityToolkit.Mvvm.ComponentModel;

namespace JumpchainCharacterBuilder.Model
{
    /// <summary>
    /// Represents the needed lists for the currently selected Drawback's per-Jump
    /// suspensions.
    /// </summary>
    public partial class DrawbackSuspendModel : ObservableValidator
    {
        /// <summary>
        /// Represents the name of a Jump.
        /// </summary>
        [ObservableProperty]
        private string _jumpName = "";

        /// <summary>
        /// Represents whether the selected Drawback is suspended for a
        /// given Jump.
        /// </summary>
        [ObservableProperty]
        private bool _suspended = false;

        public DrawbackSuspendModel(string jumpName)
        {
            JumpName = jumpName;
        }

        public DrawbackSuspendModel()
        {
            
        }
    }
}
