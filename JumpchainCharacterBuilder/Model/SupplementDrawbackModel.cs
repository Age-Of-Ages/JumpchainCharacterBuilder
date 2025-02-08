using CommunityToolkit.Mvvm.ComponentModel;

namespace JumpchainCharacterBuilder.Model
{
    public partial class SupplementDrawbackModel : ObservableValidator
    {
        /// <summary>
        /// Represents the name of the Drawback.
        /// </summary>
        [ObservableProperty]
        private string _name = "";

        /// <summary>
        /// Represents the description of the Drawback.
        /// </summary>
        [ObservableProperty]
        private string _description = "";

        /// <summary>
        /// Represents the category that the Drawback falls under.
        /// </summary>
        [ObservableProperty]
        private string _category = "";

        /// <summary>
        /// Represents the number of points given by the Drawback, towards
        /// whatever currency the Supplement uses.
        /// </summary>
        [ObservableProperty]
        private int _value = 0;

        public SupplementDrawbackModel()
        {

        }

        public SupplementDrawbackModel(SupplementDrawbackModel existingDrawback)
        {
            Name = existingDrawback.Name;
            Description = existingDrawback.Description;
            Category = existingDrawback.Category;
            Value = existingDrawback.Value;
        }

        public SupplementDrawbackModel(string name)
        {
            Name = name;
        }
    }
}
