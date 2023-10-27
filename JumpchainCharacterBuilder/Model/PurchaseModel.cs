using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;

namespace JumpchainCharacterBuilder.Model
{
    /// <summary>
    /// Represents one purchase from a Jump document.
    /// </summary>
    public partial class Purchase : ObservableValidator
    {
        /// <summary>
        /// Represents the name of a purchase.
        /// </summary>
        [ObservableProperty]
        private string _name = "";

        /// <summary>
        /// Represents the cost deducted from the selected currency.
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
        /// Note: Overrides the automated Origin discounts if enabled.
        /// </summary>
        [ObservableProperty]
        private bool _discountEnabled = false;

        /// <summary>
        /// Represents if the purchase has been made free.
        /// Note: Overrides the automated Origin freebies if enabled.
        /// </summary>
        [ObservableProperty]
        private bool _freebieEnabled = false;

        /// <summary>
        /// Represents the Origin associated with this purchase, for
        /// the purpose of automated discounts and freebies.
        /// </summary>
        [ObservableProperty]
        private int _associatedOriginIndex = 0;

        /// <summary>
        /// Represents the category of this purchase.
        /// Examples differ depending on what type of purchase it
        /// is. For example, Perks might be split into Physical, Mental,
        /// Spiritual, and so on. Items might be Weapons, Armor, Tools,
        /// or other categories.
        /// </summary>
        [ObservableProperty]
        private string _category = "Other";

        /// <summary>
        /// Represents the type of purchase. This is distinct from
        /// the category and refers to if it is a Perk, Item, or some
        /// other custom purchase type set by the user. Defaults include
        /// Perk, Item, and Power, but more can be set easily.
        /// </summary>
        [ObservableProperty]
        private int _typeIndex = 0;

        /// <summary>
        /// Represents whether this purchase becomes part of the Jumper's
        /// Body Mod or not.
        /// </summary>
        [ObservableProperty]
        private bool _bodyModAddition = false;

        /// <summary>
        /// Represents if this purchase is temporary and lost after the end of the Jump.
        /// Temporary purchases are not included in Cosmic Passport Perk list.
        /// </summary>
        [ObservableProperty]
        private bool _isTemporary = false;

        /// <summary>
        /// Represents the description of the purchase.
        /// </summary>
        [ObservableProperty]
        private string _description = "";

        /// <summary>
        /// Represents any special values associated with the purchase.
        /// This can mean modifiers, attributes, skills, or more. It could
        /// also refer to things granted by it, like an Item or Perk that grants a 1-up.
        /// </summary>
        [ObservableProperty]
        private List<PurchaseAttribute> _Attributes = new();

        /// <summary>
        /// Represents the Jump this was purchased in.
        /// </summary>
        [ObservableProperty]
        private string _sourceJump = "";

        /// <summary>
        /// Represents the character that made this purchase.
        /// </summary>
        [ObservableProperty]
        private string _sourceCharacter = "";

        public Purchase()
        {

        }

        public Purchase(string name, int typeIndex, string category)
        {
            Name = name;
            TypeIndex = typeIndex;
            Category = category;
        }
    }
}
