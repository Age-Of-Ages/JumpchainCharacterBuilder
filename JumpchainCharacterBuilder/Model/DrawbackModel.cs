using CommunityToolkit.Mvvm.ComponentModel;

namespace JumpchainCharacterBuilder.Model
{
    /// <summary>
    /// Represents an individual Drawback or Scenario selection.
    /// </summary>
    public partial class Drawback : ObservableValidator
    {
        /// <summary>
        /// Represents the name of the Drawback/Scenario.
        /// </summary>
        [ObservableProperty]
        private string _name = "";

        /// <summary>
        /// Represents the points given by the Drawback/Scenario.
        /// </summary>
        [ObservableProperty]
        private int _value = 0;

        /// <summary>
        /// Represents the description of the Drawback/Scenario.
        /// </summary>
        [ObservableProperty]
        private string _description = "";

        /// <summary>
        /// Represents the reward given by the Drawback/Scenario.
        /// </summary>
        [ObservableProperty]
        private string _reward = "";

        public Drawback()
        {

        }

        public Drawback(string name)
        {
            Name = name;
        }
    }
}
