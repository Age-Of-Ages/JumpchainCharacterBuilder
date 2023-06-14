using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;

namespace JumpchainCharacterBuilder.Model
{
    /// <summary>
    /// Stores attributes and skills for a character. 
    /// Both of these traits use the same format for stored data, so we'll use one class for them.
    /// </summary>
    public partial class ProfileAttribute : ObservableValidator
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
        /// such as 1-ups.
        /// </summary>
        [ObservableProperty]
        private int _value = 0;

        /// <summary>
        /// Represents the rank of the attribute. This is used for attributes that use letter ranks for their value.
        /// </summary>
        [ObservableProperty]
        private AttributeCalculationClass.RankList _rank = AttributeCalculationClass.RankList.None;

        /// <summary>
        /// Represents the display-friendly rank of the attribute.
        /// </summary>
        [ObservableProperty]
        private string _displayRank = "N/A";

        /// <summary>
        /// Represents the base value of the attribute, which will be used if no other source provides a greater value.
        /// </summary>
        [ObservableProperty]
        private int _baseValue = 0;

        /// <summary>
        /// Represents the modifier of the attribute's value, which will add to any other sources.
        /// </summary>
        [ObservableProperty]
        private int _modValue = 0;

        /// <summary>
        /// Represents the base value of the attribute, which will be used if no other source provides a greater value.
        /// </summary>
        [ObservableProperty]
        private AttributeCalculationClass.RankList _baseRank = AttributeCalculationClass.RankList.None;

        /// <summary>
        /// Represents the modifier of the attribute's value, which will add to any other sources.
        /// </summary>
        [ObservableProperty]
        private AttributeCalculationClass.RankList _modRank = AttributeCalculationClass.RankList.None;

        /// <summary>
        /// Represents the names of the purchases that contribute to this attribute.
        /// </summary>
        [ObservableProperty]
        private List<AttributeSource> _sourceList = new();

        public ProfileAttribute()
        {

        }

        public ProfileAttribute(string name, string type, string category)
        {
            Name = name;
            Type = type;
            Category = category;
        }
    }
}
