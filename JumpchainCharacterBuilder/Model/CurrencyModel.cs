using CommunityToolkit.Mvvm.ComponentModel;

namespace JumpchainCharacterBuilder.Model
{
    /// <summary>
    /// Stores individual currency types.
    /// </summary>
    public class Currency : ObservableValidator
    {
        private string _currencyName = "";
        /// <summary>
        /// Represents the name of the currency.
        /// </summary>
        public string CurrencyName
        {
            get
            {
                return _currencyName;
            }
            set
            {
                _currencyName = value;
                OnPropertyChanged(nameof(CurrencyName));
            }
        }

        private string _currencyAbbreviation = "";
        /// <summary>
        /// Represents the abbreviation of the currency's name.
        /// </summary>
        public string CurrencyAbbreviation
        {
            get
            {
                return _currencyAbbreviation;
            }
            set
            {
                _currencyAbbreviation = value;
                OnPropertyChanged(nameof(CurrencyAbbreviation));
            }
        }

        private int _currencyBudget = 0;
        /// <summary>
        /// Represents the universally applied budget associated with this currency
        /// type.
        /// </summary>
        public int CurrencyBudget
        {
            get
            {
                return _currencyBudget;
            }
            set
            {
                _currencyBudget = value;
                OnPropertyChanged();
            }
        }

        public Currency()
        {

        }

        public Currency(string currencyName, string currencyAbbreviation)
        {
            CurrencyName = currencyName;
            CurrencyAbbreviation = currencyAbbreviation;
        }

        public Currency(string currencyName, string currencyAbbreviation, int budget)
        {
            CurrencyName = currencyName;
            CurrencyAbbreviation = currencyAbbreviation;
            CurrencyBudget = budget;
        }
    }
}
