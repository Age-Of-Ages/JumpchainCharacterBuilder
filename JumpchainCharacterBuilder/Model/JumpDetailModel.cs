using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JumpchainCharacterBuilder.Model
{
    /// <summary>
    /// Store Jump details and build, for the purpose of creating a new Jump entry.
    /// </summary>
    public partial class Jump : DocumentDetail
    {
        /// <summary>
        /// Represents the list of available Origin options.
        /// </summary>
        [ObservableProperty]
        private List<OriginDetail> _originDetails = [];

        /// <summary>
        /// Represents the list of available misc. Origin categories.
        /// This does not require Location or Species as those are assumed.
        /// </summary>
        [ObservableProperty]
        private List<string> _miscOriginCategories = [];

        /// <summary>
        /// Represents the number of days spent in a Jump.
        /// </summary>
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Range(0, 366, ErrorMessage = "Days must be a value between 0 and 366")]
        private int durationDays = 0;

        /// <summary>
        /// Represents the number of months spent in a Jump.
        /// </summary>
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Range(0, 12, ErrorMessage = "Months must be a value between 0 and 12")]
        private int durationMonths = 0;

        /// <summary>
        /// Represents the number of years spent in a Jump.
        /// </summary>
        [ObservableProperty]
        private int durationYears = 10;

        /// <summary>
        /// Represents the Jump's number.
        /// </summary>
        [ObservableProperty]
        private int _jumpNumber = 0;

        /// <summary>
        /// Represents whether to skip this Jump for anything that
        /// uses the Jump number, such as incremental Supplement
        /// modes.
        /// </summary>
        [ObservableProperty]
        private bool _skipNumber = false;

        /// <summary>
        /// Represents the discount mode of the Jump.
        /// </summary>
        /// <example>
        /// False - Full manual mode, disregard the set Origins.
        /// True - Automated Discounts on, purchases use set Origin for
        /// discounts and freebies.
        /// </example>
        [ObservableProperty]
        private bool _originDiscounts = false;

        /// <summary>
        /// Represents the value below which a purchase is automatically free
        /// in origin discounts mode if the origins match.
        /// </summary>
        [ObservableProperty]
        private int _originPerkFreebieThreshold = 100;

        /// <summary>
        /// Represents the value below which a purchase is automatically free
        /// in origin discounts mode if the origins match.
        /// </summary>
        [ObservableProperty]
        private int _originItemFreebieThreshold = 100;

        /// <summary>
        /// Represents the list of Jump builds for each character.
        /// This uses the index value of each Companion in the Save File,
        /// starting at 1.
        /// </summary>
        [ObservableProperty]
        private List<JumpBuild> _build = [];

        /// <summary>
        /// Represents the name and abbreviation for a specific currency.
        /// </summary>
        [ObservableProperty]
        private List<Currency> _currencies = [];

        /// <summary>
        /// Represents the list of all purchase types used for the Jump.
        /// </summary>
        [ObservableProperty]
        private List<PurchaseType> _purchaseTypes = [];

        /// <summary>
        /// Represents whether the Jump is a Gauntlet or not.
        /// </summary>
        [ObservableProperty]
        private bool _isGauntlet = false;

        public Jump()
        {
            Name = "Jump Name";

            Build.Add(new JumpBuild());

            OriginDetails.Add(new OriginDetail("No Origin", "Origin", 0, ""));
            OriginDetails.Add(new OriginDetail("Origin #1", "Origin", 0, ""));

            Currencies.Add(new Currency("Choice Points", "CP"));
            Currencies[0].CurrencyBudget = 1000;

            PurchaseTypes.Add(new PurchaseType("Perks", 0, false));
            PurchaseTypes.Add(new PurchaseType("Items", 0, true));
        }

        public Jump(string name)
        {
            Name = name;

            Build.Add(new JumpBuild());

            OriginDetails.Add(new OriginDetail("No Origin", "Origin", 0, ""));
            OriginDetails.Add(new OriginDetail("Origin #1", "Origin", 0, ""));

            Currencies.Add(new Currency("Choice Points", "CP"));
            Currencies[0].CurrencyBudget = 1000;

            PurchaseTypes.Add(new PurchaseType("Perks", 0, false));
            PurchaseTypes.Add(new PurchaseType("Items", 0, true));
        }

        public Jump(string name, int choicePointBudget, int itemStipend, bool originDiscounts,
            int perkFreebieThreshold, int itemFreebieThreshold)
        {
            Name = name;
            Currencies.Add(new Currency("Choice Points", "CP", choicePointBudget));
            OriginDetails.Add(new OriginDetail("No Origin", "Origin", 0, ""));
            PurchaseTypes.Add(new PurchaseType("Perks", 0, false));
            PurchaseTypes.Add(new PurchaseType("Items", 0, true));

            OriginDiscounts = originDiscounts;
            OriginPerkFreebieThreshold = perkFreebieThreshold;
            OriginItemFreebieThreshold = itemFreebieThreshold;

            Build.Add(new JumpBuild(itemStipend));
        }
    }
}
