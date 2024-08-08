using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;

namespace JumpchainCharacterBuilder.Model
{
    /// <summary>
    /// List and store details of all Alt-forms that a character
    /// possesses.
    /// </summary>
    public partial class AltForm : ObservableValidator
    {
        /// <summary>
        /// Represents the Alt-form's name.
        /// </summary>
        [ObservableProperty]
        private string _altFormName = "";

        /// <summary>
        /// Represents the Alt-form's Species/Race.
        /// </summary>
        [ObservableProperty]
        private string _altFormSpecies = "";

        /// <summary>
        /// Represents the Alt-form's physical description.
        /// </summary>
        [ObservableProperty]
        private string _altFormDescription = "";

        /// <summary>
        /// Represents the Alt-form's strengths.
        /// </summary>
        [ObservableProperty]
        private string _strengths = "";

        /// <summary>
        /// Represents the Alt-form's weaknesses.
        /// </summary>
        [ObservableProperty]
        private string _weaknesses = "";

        public AltForm()
        {

        }

        public AltForm(string name)
        {
            AltFormName = name;
        }
    }
}
