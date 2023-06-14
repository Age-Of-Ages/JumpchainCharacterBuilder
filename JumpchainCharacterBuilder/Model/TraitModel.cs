using CommunityToolkit.Mvvm.ComponentModel;

namespace JumpchainCharacterBuilder.Model
{
    /// <summary>
    /// Stores lists of character traits.
    /// </summary>
    public partial class Trait : ObservableValidator
    {
        /// <summary>
        /// Represents a single 'Like'.
        /// </summary>
        [ObservableProperty]
        private string _like = "";

        /// <summary>
        /// Represents a single 'Dislike'.
        /// </summary>
        [ObservableProperty]
        private string _dislike = "";

        /// <summary>
        /// Represents a single hobby.
        /// </summary>
        [ObservableProperty]
        private string _hobby = "";

        /// <summary>
        /// Represents a single quirk.
        /// </summary>
        [ObservableProperty]
        private string _quirk = "";

        /// <summary>
        /// Represents a single goal.
        /// </summary>
        [ObservableProperty]
        private string _goal = "";

        public Trait()
        {

        }
    }
}
