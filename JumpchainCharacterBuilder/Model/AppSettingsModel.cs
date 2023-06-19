using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JumpchainCharacterBuilder.Model
{
    /// <summary>
    /// Stores settings for the application.
    /// </summary>
    public partial class AppSettingsModel : ObservableValidator
    {
        /// <summary>
        /// Stores available height formats.
        /// </summary>
        public enum HeightFormats
        {
            FeetInches,
            Feet,
            Meters
        }

        /// <summary>
        /// Stores available weight formats.
        /// </summary>
        public enum WeightFormats
        {
            Pounds,
            Kilograms
        }

        /// <summary>
        /// Represents the user-preferred height display format.
        /// </summary>
        [ObservableProperty]
        private HeightFormats _heightFormat = HeightFormats.FeetInches;

        /// <summary>
        /// Represents the user-preferred weight display format.
        /// </summary>
        [ObservableProperty]
        private WeightFormats _weightFormat = WeightFormats.Pounds;

        /// <summary>
        /// Represents the chosen interface theme.
        /// </summary>
        [ObservableProperty]
        private string _theme = "Light";

        /// <summary>
        /// Represents the link to the Github repository.
        /// </summary>
        [ObservableProperty]
        private string _githubUrl = "";
    }
}
