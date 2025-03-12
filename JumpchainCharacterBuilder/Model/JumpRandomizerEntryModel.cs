using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace JumpchainCharacterBuilder.Model
{
    public partial class JumpRandomizerEntry : ObservableValidator
    {
        /// <summary>
        /// Represents the name of the Jump
        /// </summary>
        [ObservableProperty]
        private string _jumpName = "";

        /// <summary>
        /// Represents the number of times the Jump appears in the list to randomly pull from.
        /// </summary>
        [ObservableProperty]
        private int _jumpWeight = 1;

        /// <summary>
        /// Represents the link to the Jump document.
        /// </summary>
        [ObservableProperty]
        private Uri _jumpUri = new("About:Blank");

        public JumpRandomizerEntry()
        {
            
        }

        public JumpRandomizerEntry(JumpRandomizerEntry old_entry)
        {
            JumpName = old_entry.JumpName;
            JumpWeight = old_entry.JumpWeight;
            JumpUri = old_entry.JumpUri;
        }
    }
}
