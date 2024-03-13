﻿using CommunityToolkit.Mvvm.ComponentModel;

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
        /// Represents if the window is allowed to be resized or not.
        /// </summary>
        [ObservableProperty]
        private bool _canResizeWindow = false;

        /// <summary>
        /// Represents if the application should ask the user to save their current data before closing.
        /// </summary>
        [ObservableProperty]
        private bool _confirmSaveOnClose = true;

        /// <summary>
        /// Represents if the application-wide spellcheck should be enabled or not.
        /// </summary>
        [ObservableProperty]
        private bool _spellCheckEnabled = true;

        /// <summary>
        /// Represents if the application should skip to and display the newest Jump when a save is loaded.
        /// </summary>
        [ObservableProperty]
        private bool _focusNewestJumpOnLoad = true;
    }
}
