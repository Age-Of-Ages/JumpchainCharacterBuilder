using CommunityToolkit.Mvvm.ComponentModel;

namespace JumpchainCharacterBuilder.Model
{
    /// <summary>
    /// Represents generic detail information stored for all documents.
    /// </summary>
    public partial class DocumentDetail : ObservableValidator
    {
        /// <summary>
        /// Represents the document's name.
        /// </summary>
        [ObservableProperty]
        private string _name = "";

        /// <summary>
        /// Represents the version of the document.
        /// </summary>
        [ObservableProperty]
        private string _version = "";

        /// <summary>
        /// Represents the source of the document, likely as a link to it.
        /// </summary>
        [ObservableProperty]
        private string _source = "";

        /// <summary>
        /// Represents the name of the document's author.
        /// </summary>
        [ObservableProperty]
        private string _author = "";
    }
}
