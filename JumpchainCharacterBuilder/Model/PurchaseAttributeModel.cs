using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;

namespace JumpchainCharacterBuilder.Model
{
    /// <summary>
    /// Stores attributes for purchases that are made.
    /// </summary>
    public partial class PurchaseAttribute : ObservableValidator
    {
        /// <summary>
        /// Represents the name of the attribute/skill.
        /// </summary>
        [ObservableProperty]
        private string _name = "";

        /// <summary>
        /// Represents the type of the attribute. This determines what categories are available to select in the interface.
        /// </summary>
        [ObservableProperty]
        private string _type = "";

        /// <summary>
        /// Represents the category for the attribute/skill. They
        /// use different systems so the category should be set
        /// based on which type it is.
        /// </summary>
        [ObservableProperty]
        private string _category = "";

        /// <summary>
        /// Represents the value of the attribute. This is used for attributes that have an associated number value,
        /// such as 1-ups or training boosters
        /// </summary>
        [ObservableProperty]
        private int _value = 0;

        /// <summary>
        /// Represents the rank of the attribute. This is used for attributes that use letter ranks for their value.
        /// </summary>
        [ObservableProperty]
        private AttributeCalculationClass.RankList _rank = AttributeCalculationClass.RankList.F;

        /// <summary>
        /// Represents the name of the purchase that this attribute is associated with, for display purposes.
        /// </summary>
        [ObservableProperty]
        private string _sourcePurchase = "";

        /// <summary>
        /// Represents the name of the Jump that this attribute is associated with, for display purposes.
        /// </summary>
        [ObservableProperty]
        private string _sourceJump = "";

        /// <summary>
        /// Represents the list of available Types. Special type is for 1-ups and other unique traits.
        /// </summary>
        [ObservableProperty]
        private List<string> _typeList = new()
        {
            "Attribute",
            "Skill",
            "Booster",
            "Special"
        };

        /// <summary>
        /// Represents the list of available Categories for the Attribute Type.
        /// </summary>
        [ObservableProperty]
        private List<string> _attributeCategoryList = new()
        {
            "Physical",
            "Mental",
            "Supernatural"
        };

        /// <summary>
        /// Represents the list of available Categories for the Skill Type.
        /// </summary>
        [ObservableProperty]
        private List<string> _skillCategoryList = new()
        {
            "Physical",
            "Mental",
            "Social",
            "Technological",
            "Supernatural"
        };

        /// <summary>
        /// Represents the list of available Categories for the Special Type.
        /// </summary>
        [ObservableProperty]
        private List<string> _specialCategoryList = new()
        {
            "1-up"
        };

        public PurchaseAttribute()
        {

        }

        public PurchaseAttribute(AttributeCalculationClass.RankList rank)
        {
            Rank = rank;
        }
    }
}
