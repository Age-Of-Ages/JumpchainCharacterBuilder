﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using JumpchainCharacterBuilder.Messages;
using JumpchainCharacterBuilder.Model;
using System.Collections.Generic;

namespace JumpchainCharacterBuilder.ViewModel
{
    public partial class SettingsViewModel : ViewModelBase
    {
        #region Fields
        [ObservableProperty]
        private AppSettingsModel _appSettings = new();

        [ObservableProperty]
        private AppSettingsModel.HeightFormats _heightFormatSelection = AppSettingsModel.HeightFormats.FeetInches;
        [ObservableProperty]
        private AppSettingsModel.WeightFormats _weightFormatSelection = AppSettingsModel.WeightFormats.Pounds;

        [ObservableProperty]
        private bool _canResizeWindow = false;
        [ObservableProperty]
        private bool _confirmSaveOnClose = true;
        [ObservableProperty]
        private bool _spellCheckEnabled = true;
        [ObservableProperty]
        private bool _focusNewestJumpOnLoad = true;
        #endregion

        #region Properties 
        partial void OnHeightFormatSelectionChanged(AppSettingsModel.HeightFormats value)
        {
            AppSettings.HeightFormat = value;
            Messenger.Send(new SettingsChangedMessage(true));
            CfgAccess.WriteCfgFile(AppSettings);
        }

        partial void OnWeightFormatSelectionChanged(AppSettingsModel.WeightFormats value)
        {
            AppSettings.WeightFormat = value;
            Messenger.Send(new SettingsChangedMessage(true));
            CfgAccess.WriteCfgFile(AppSettings);
        }

        partial void OnCanResizeWindowChanged(bool value)
        {
            AppSettings.CanResizeWindow = value;
            Messenger.Send(new SettingsChangedMessage(true));
            CfgAccess.WriteCfgFile(AppSettings);
        }

        partial void OnConfirmSaveOnCloseChanged(bool value)
        {
            AppSettings.ConfirmSaveOnClose = value;
            Messenger.Send(new SettingsChangedMessage(true));
            CfgAccess.WriteCfgFile(AppSettings);
        }

        partial void OnSpellCheckEnabledChanged(bool value)
        {
            AppSettings.SpellCheckEnabled = value;
            Messenger.Send(new SettingsChangedMessage(true));
            CfgAccess.WriteCfgFile(AppSettings);
        }

        partial void OnFocusNewestJumpOnLoadChanged(bool value)
        {
            AppSettings.FocusNewestJumpOnLoad = value;
            Messenger.Send(new SettingsChangedMessage(true));
            CfgAccess.WriteCfgFile(AppSettings);
        }

        public Dictionary<string, AppSettingsModel.HeightFormats> HeightFormatList { get; } =
            new()
            {
                {"Feet/Inches", AppSettingsModel.HeightFormats.FeetInches},
                {"Feet", AppSettingsModel.HeightFormats.Feet},
                {"Meters", AppSettingsModel.HeightFormats.Meters}
            };

        public Dictionary<string, AppSettingsModel.WeightFormats> WeightFormatList { get; } =
            new()
            {
                {"Pounds", AppSettingsModel.WeightFormats.Pounds},
                {"Kilograms", AppSettingsModel.WeightFormats.Kilograms}
            };

        #endregion

        #region Constructor
        public SettingsViewModel()
        {
            AppSettings = Messenger.Send<SettingsRequestMessage>();
            LoadSettings();
        }
        #endregion

        #region Methods
        private void LoadSettings()
        {
            WeightFormatSelection = AppSettings.WeightFormat;
            HeightFormatSelection = AppSettings.HeightFormat;

            CanResizeWindow = AppSettings.CanResizeWindow;
            ConfirmSaveOnClose = AppSettings.ConfirmSaveOnClose;
            SpellCheckEnabled = AppSettings.SpellCheckEnabled;
            FocusNewestJumpOnLoad = AppSettings.FocusNewestJumpOnLoad;
        }

        #endregion
    }
}
