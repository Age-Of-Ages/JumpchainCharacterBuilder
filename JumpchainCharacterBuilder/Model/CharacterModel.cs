using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;

namespace JumpchainCharacterBuilder.Model
{
    /// <summary>
    /// Stores all data for each character.
    /// </summary>
    public partial class Character : ObservableValidator
    {
        /// <summary>
        /// Represents the name of the character.
        /// </summary>
        [ObservableProperty]
        private string _name = "";

        /// <summary>
        /// Represents the character's alias.
        /// </summary>
        [ObservableProperty]
        private string _alias = "";

        /// <summary>
        /// Represents the character's gender.
        /// </summary>
        [ObservableProperty]
        private string _gender = "";

        /// <summary>
        /// Represents the character's age.
        /// </summary>
        [ObservableProperty]
        private double _age = 0;

        /// <summary>
        /// Represents the character's true age.
        /// </summary>
        [ObservableProperty]
        private double _trueAge = 0;

        /// <summary>
        /// Represents the character's height (Feet).
        /// </summary>
        [ObservableProperty]
        private double _heightFeet = 0;

        /// <summary>
        /// Represents the character's height (Inches).
        /// </summary>
        [ObservableProperty]
        private double _heightInches = 0;

        /// <summary>
        /// Represents the character's height (Meters).
        /// </summary>
        [ObservableProperty]
        private double _heightMeters = 0;

        /// <summary>
        /// Represents the character's weight in pounds.
        /// </summary>
        [ObservableProperty]
        private double _weightPounds = 0;

        /// <summary>
        /// Represents the character's weight in Kilograms.
        /// </summary>
        [ObservableProperty]
        private double _weightKilograms = 0;

        /// <summary>
        /// Represents the character's race. Note: Not to be used for species.
        /// </summary>
        [ObservableProperty]
        private string _race = "";

        /// <summary>
        /// Represents the character's species.
        /// </summary>
        [ObservableProperty]
        private string _species = "";

        /// <summary>
        /// Represents the character's physical description.
        /// </summary>
        [ObservableProperty]
        private string _physicalDescription = "";

        /// <summary>
        /// Represents the character's personality description.
        /// </summary>
        [ObservableProperty]
        private string _personality = "";

        /// <summary>
        /// Represents the world or setting the character is from.
        /// </summary>
        [ObservableProperty]
        private string _homeworld = "";

        /// <summary>
        /// Represents the Jump number that this character was obtained on.
        /// </summary>
        [ObservableProperty]
        private int _firstJump = 1;

        /// <summary>
        /// Represents all of the character's traits.
        /// </summary>
        [ObservableProperty]
        private List<Trait> _traitRow = new();

        /// <summary>
        /// Represents the list of character's Alt-forms.
        /// </summary>
        [ObservableProperty]
        private List<AltForm> _altForms = new();

        /// <summary>
        /// Represents the character's list of Attributes.
        /// </summary>
        [ObservableProperty]
        private List<ProfileAttribute> _attributes = new()
        {
            new ProfileAttribute("Strength", "Attribute", "Physical"),
            new ProfileAttribute("Endurance", "Attribute", "Physical"),
            new ProfileAttribute("Speed", "Attribute", "Physical"),
            new ProfileAttribute("Resilience", "Attribute", "Physical"),
            new ProfileAttribute("Reflex", "Attribute", "Physical"),
            new ProfileAttribute("Flexibility", "Attribute", "Physical"),
            new ProfileAttribute("Coordination", "Attribute", "Physical"),
            new ProfileAttribute("Regeneration", "Attribute", "Physical"),
            new ProfileAttribute("Durability", "Attribute", "Physical"),
            new ProfileAttribute("Appeal", "Attribute", "Mental"),
            new ProfileAttribute("Logic", "Attribute", "Mental"),
            new ProfileAttribute("Memory", "Attribute", "Mental"),
            new ProfileAttribute("Resolve", "Attribute", "Mental"),
            new ProfileAttribute("Perception", "Attribute", "Mental"),
            new ProfileAttribute("Charisma", "Attribute", "Mental"),
            new ProfileAttribute("Luck", "Attribute", "Supernatural"),
            new ProfileAttribute("Magic", "Attribute", "Supernatural")
        };

        /// <summary>
        /// Represents the character's list of Skills.
        /// </summary>
        [ObservableProperty]
        private List<ProfileAttribute> _skills = new();

        /// <summary>
        /// Represents the character's training boosters.
        /// </summary>
        [ObservableProperty]
        private List<Booster> _boosters = new();

        /// <summary>
        /// Represents the character's Body Mod.
        /// </summary>
        [ObservableProperty]
        private BodyModUniversal _bodyMod = new();

        public Character()
        {

        }

        public Character(string name)
        {
            Name = name;
        }

    }
}
