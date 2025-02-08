using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;

namespace JumpchainCharacterBuilder.Model
{
    /// <summary>
    /// Represents an individual purchase from a Supplement.
    /// </summary>
    public partial class SupplementPurchase : ObservableValidator
    {
        /// <summary>
        /// Represents the name of the purchase.
        /// </summary>
        [ObservableProperty]
        private string _name = "";

        /// <summary>
        /// Represents the cost of the purchase.
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
        /// Represents whether a purchase should be discounted
        /// by 50% or not.
        /// </summary>
        [ObservableProperty]
        private bool _discounted = false;

        /// <summary>
        /// Represents whether the purchase should be made free.
        /// </summary>
        [ObservableProperty]
        private bool _freebie = false;

        /// <summary>
        /// Represents the category that the purchase falls under.
        /// </summary>
        [ObservableProperty]
        private string _category = "";

        /// <summary>
        /// Represents the description of the purchase.
        /// </summary>
        [ObservableProperty]
        private string _description = "";

        /// <summary>
        /// Represents whatever special attributes come with the purchase.
        /// </summary>
        [ObservableProperty]
        private List<PurchaseAttribute> _attributes = [];

        public SupplementPurchase()
        {

        }

        public SupplementPurchase(SupplementPurchase existingPurchase)
        {
            Name = existingPurchase.Name;
            Cost = existingPurchase.Cost;
            Discounted = existingPurchase.Discounted;
            Freebie = existingPurchase.Freebie;
            Category = existingPurchase.Category;
            Description = existingPurchase.Description;

            foreach (PurchaseAttribute attribute in existingPurchase.Attributes)
            {
                Attributes.Add(new(attribute));
            }
        }

        public SupplementPurchase(string name)
        {
            Name = name;
        }

        public SupplementPurchase(string name, string category)
        {
            Name = name;
            Category = category;
        }
    }
}
