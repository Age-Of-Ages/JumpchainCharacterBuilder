using CommunityToolkit.Mvvm.ComponentModel;

namespace JumpchainCharacterBuilder.Model
{
    public partial class HouseRuleModel : ObservableValidator
    {
        /// <summary>
        /// Represents the House Rule's name.
        /// </summary>
        [ObservableProperty]
        private string _houseRuleName = "";

        /// <summary>
        /// Represents the House Rule's description.
        /// </summary>
        [ObservableProperty]
        private string _houseRuleDescription = "";

        public HouseRuleModel()
        {

        }

        public HouseRuleModel(string name)
        {
            HouseRuleName = name;
        }
    }
}
