using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;

namespace JumpchainCharacterBuilder.Model
{
    /// <summary>
    /// Store data related to the Cosmic Locker.
    /// </summary>
    public partial class CosmicLocker : ObservableValidator
    {
        /// <summary>
        /// Represents the total number of 1-ups owned by the character.
        /// </summary>
        [ObservableProperty]
        private int _oneUps = 0;

        /// <summary>
        /// Represents the names of Perks and Items that grant 1-ups to
        /// the character.
        /// </summary>
        [ObservableProperty]
        private List<string> _oneUpsDetails = new();
    }
}
