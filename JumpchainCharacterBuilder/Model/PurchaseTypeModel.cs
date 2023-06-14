using CommunityToolkit.Mvvm.ComponentModel;

namespace JumpchainCharacterBuilder.Model
{
    /// <summary>
    /// Represents a single Purchase Type.
    /// </summary>
    public class PurchaseType : ObservableValidator
    {
        private string _type = "";
        /// <summary>
        /// Represents the name of the Purchase Type.
        /// </summary>
        public string Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
                OnPropertyChanged(nameof(Type));
            }
        }

        private int _currencyIndex = 0;
        /// <summary>
        /// Represents the index of the currency used by the Purchase Type.
        /// </summary>
        public int CurrencyIndex
        {
            get
            {
                return _currencyIndex;
            }
            set
            {
                _currencyIndex = value;
                OnPropertyChanged(nameof(CurrencyIndex));
            }
        }

        private bool _isItemType = false;
        /// <summary>
        /// Represents whether this purchase type is considered an
        /// Item or not. This determines which categories are available
        /// for purchases using this type.
        /// </summary>
        public bool IsItemType
        {
            get => _isItemType;
            set
            {
                SetProperty(ref _isItemType, value, true);
            }
        }

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
