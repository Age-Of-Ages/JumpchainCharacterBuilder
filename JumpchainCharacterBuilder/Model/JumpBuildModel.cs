using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;

namespace JumpchainCharacterBuilder.Model
{
    /// <summary>
    /// Build details for a given Jump are stored here.
    /// </summary>
    public partial class JumpBuild : ObservableValidator
    {
        /// <summary>
        /// Represents specific stipends given for a currency, usually if the
        /// user bought an option that granted them separately from whatever it
        /// would usually come with.
        /// </summary>
        [ObservableProperty]
        private List<int> _pointStipend = new();

        /// <summary>
        /// Represents Item-specific stipend.
        /// </summary>
        [ObservableProperty]
        private int _itemStipend = 0;

        /// <summary>
        /// Represents the chosen Origin index.
        /// </summary>
        [ObservableProperty]
        private int _originIndex = 0;

        /// <summary>
        /// Represents the chosen species/race.
        /// </summary>
        [ObservableProperty]
        private OriginDetail _species = new();

        /// <summary>
        /// Represents the chosen location.
        /// </summary>
        [ObservableProperty]
        private OriginDetail _location = new();

        /// <summary>
        /// Represents miscellaneous Origin details beyond the default
        /// types available.
        /// </summary>
        [ObservableProperty]
        private List<OriginDetail> _miscOriginDetails = new();

        /// <summary>
        /// Represents the chosen age.
        /// </summary>
        [ObservableProperty]
        private int _age = 0;

        /// <summary>
        /// Represents the cost of the chosen Age option.
        /// </summary>
        [ObservableProperty]
        private int _ageCost = 0;

        /// <summary>
        /// Represents the chosen gender.
        /// </summary>
        [ObservableProperty]
        private string _gender = "";

        /// <summary>
        /// Represents the cost of the chosen Location option.
        /// </summary>
        [ObservableProperty]
        private int _genderCost = 0;

        /// <summary>
        /// Represents the list of purchases made for this build.
        /// </summary>
        [ObservableProperty]
        private List<Purchase> _purchase = new();

        /// <summary>
        /// Represents the list of Companion Purchase Options taken.
        /// </summary>
        [ObservableProperty]
        private List<CompanionPurchase> _companionPurchase = new();

        /// <summary>
        /// Represents the list of Drawbacks selected for this build.
        /// </summary>
        [ObservableProperty]
        private List<Drawback> _drawbackSelection = new();

        /// <summary>
        /// Represents the list of Scenarios selected for this build.
        /// </summary>
        [ObservableProperty]
        private List<Drawback> _scenarioSelection = new();

        /// <summary>
        /// Represents the number of points spent on Warehouse Investment.
        /// </summary>
        [ObservableProperty]
        private int _warehouseInvestment = 0;

        /// <summary>
        /// Represents the number of points spent on Body Mod Investment.
        /// </summary>
        [ObservableProperty]
        private int _bodyModInvestment = 0;

        /// <summary>
        /// Represents how many points were banked from this Jump by the user.
        /// </summary>
        [ObservableProperty]
        private int _bankedPoints = 0;

        /// <summary>
        /// Represents how many points were added to this build's starting value from the point bank.
        /// </summary>
        [ObservableProperty]
        private int _bankUsage = 0;

        public JumpBuild()
        {
            PointStipend.Add(0);

            Location = new("No Location", "Location", 0, "");
            Species = new("Human", "Species", 0, "");


        }

        public JumpBuild(int itemStipend)
        {
            PointStipend.Add(0);
            ItemStipend = itemStipend;

            Location = new("No Location", "Location", 0, "");
            Species = new("Human", "Species", 0, "");
        }
    }
}
