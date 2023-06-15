using CommunityToolkit.Mvvm.ComponentModel;

namespace JumpchainCharacterBuilder.Model
{
    /// <summary>
    /// Represents a single Purchase Type.
    /// </summary>
    public partial class PurchaseType : ObservableValidator
    {
        /// <summary>
        /// Represents the name of the Purchase Type.
        /// </summary>
        [ObservableProperty]
        private string _type = "";

        /// <summary>
        /// Represents the index of the currency used by the Purchase Type.
        /// </summary>
        [ObservableProperty]
        private int _currencyIndex = 0;

        /// <summary>
        /// Represents whether this purchase type is considered an
        /// Item or not. This determines which categories are available
        /// for purchases using this type.
        /// </summary>
        [ObservableProperty]
        private bool _isItemType = false;

        public PurchaseType()
        {

        }

        public PurchaseType(string type, int currencyIndex, bool isItemType)
        {
            Type = type;
            CurrencyIndex = currencyIndex;
            IsItemType = isItemType;
        }
    }
}
