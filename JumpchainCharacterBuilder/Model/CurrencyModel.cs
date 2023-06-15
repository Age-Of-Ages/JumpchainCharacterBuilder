using CommunityToolkit.Mvvm.ComponentModel;

namespace JumpchainCharacterBuilder.Model
{
    /// <summary>
    /// Stores individual currency types.
    /// </summary>
    public partial class Currency : ObservableValidator
    {
        /// <summary>
        /// Represents the name of the currency.
        /// </summary>
        [ObservableProperty]
        private string _currencyName = "";

        /// <summary>
        /// Represents the abbreviation of the currency's name.
        /// </summary>
        [ObservableProperty]
        private string _currencyAbbreviation = "";

        /// <summary>
        /// Represents the universally applied budget associated with this currency
        /// type.
        /// </summary>
        [ObservableProperty]
        private int _currencyBudget = 0;

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
