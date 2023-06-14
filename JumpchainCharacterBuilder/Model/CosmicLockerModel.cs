using System.Collections.Generic;

namespace JumpchainCharacterBuilder.Model
{
    /// <summary>
    /// Store data related to the Cosmic Locker.
    /// </summary>
    public class CosmicLocker
    {
        /// <summary>
        /// Represents the total number of 1-ups owned by the character.
        /// </summary>
        public int OneUps { get; set; } = 0;

        /// <summary>
        /// Represents the names of Perks and Items that grant 1-ups to
        /// the character.
        /// </summary>
        public List<string> OneUpsDetails { get; set; } = new List<string>();
    }
}
