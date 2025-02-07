using CommunityToolkit.Mvvm.ComponentModel;

namespace JumpchainCharacterBuilder.Model
{
    /// <summary>
    /// Represents an individual Origin option for a given Jump.
    /// </summary>
    public partial class OriginDetail : ObservableValidator
    {
        /// <summary>
        /// Represents the name of the Origin.
        /// </summary>
        [ObservableProperty]
        private string _name = "";

        /// <summary>
        /// Represents the category of Origin option.
        /// </summary>
        /// <example>
        /// Origin, Species, Location, etc.
        /// </example>
        [ObservableProperty]
        private string _category = "";

        /// <summary>
        /// Represents the cost of the Origin option.
        /// </summary>
        [ObservableProperty]
        private int _cost = 0;

        /// <summary>
        /// Represents the description of the Origin.
        /// </summary>
        [ObservableProperty]
        private string _description = "";

        public OriginDetail()
        {

        }

        public OriginDetail(OriginDetail existingOrigin)
        {
            Name = existingOrigin.Name;
            Category = existingOrigin.Category;
            Cost = existingOrigin.Cost;
            Description = existingOrigin.Description;
        }

        public OriginDetail(string category)
        {
            Category = category;
        }

        public OriginDetail(string name, string category)
        {
            Name = name;
            Category = category;
        }

        public OriginDetail(string name, string category, int cost, string description)
        {
            Name = name;
            Category = category;
            Cost = cost;
            Description = description;
        }
    }
}
