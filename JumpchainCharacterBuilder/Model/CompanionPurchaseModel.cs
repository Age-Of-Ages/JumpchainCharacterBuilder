using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;

namespace JumpchainCharacterBuilder.Model
{
    /// <summary>
    /// Store all Companion Option purchases for a given Jump.
    /// </summary>
    public partial class CompanionPurchase : ObservableValidator
    {
        /// <summary>
        /// Represents the name of the Companion Purchase Option.
        /// </summary>
        [ObservableProperty]
        private string _name = "Import Name";


        /// <summary>
        /// Represents the cost of the Companion Purchase Option.
        /// </summary>
        [ObservableProperty]
        private int _cost = 0;

        /// <summary>
        /// Represents the displayed cost of the purchase.
        /// This is for displaying discounted or freebie cost in the UI.
        /// </summary>
        [ObservableProperty]
        private int _displayCost = 0;

        /// <summary>
        /// Represents if the purchase has been discounted by 50%.
        /// </summary>
        [ObservableProperty]
        private bool _discountEnabled = false;

        /// <summary>
        /// Represents if the purchase has been made free.
        /// </summary>
        [ObservableProperty]
        private bool _freebieEnabled = false;

        /// <summary>
        /// Represents the description of the Companion Purchase Option.
        /// </summary>
        [ObservableProperty]
        private string _description = "";

        /// <summary>
        /// Represents the list of Companion import details.
        /// </summary>
        [ObservableProperty]
        private List<CompanionImportDetailClass> _companionImportDetails = new();
    }
}
