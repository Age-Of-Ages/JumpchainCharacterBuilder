using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using JumpchainCharacterBuilder.Messages;
using JumpchainCharacterBuilder.Model;

namespace JumpchainCharacterBuilder.ViewModel
{
    public partial class InputFormatterViewModel : ViewModelBase
    {
        #region Fields
        [ObservableProperty]
        private AppSettingsModel _appSettings = new();
        [ObservableProperty]
        private bool _spellCheckEnabled = true;

        [ObservableProperty]
        private string _inputString = "";

        [ObservableProperty]
        private string _outputString = "";

        [ObservableProperty]
        private bool _leaveDoubleLineBreaks = false;

        [ObservableProperty]
        private bool _removeAllLineBreaks = false;

        #endregion

        #region Properties
        partial void OnInputStringChanged(string value)
        {
            if (value != "")
            {
                FormatString();
            }
            else
            {
                OutputString = "";
            }
        }

        partial void OnLeaveDoubleLineBreaksChanged(bool value)
        {
            if (value)
            {
                RemoveAllLineBreaks = false;
            }
            AppSettings.FormatterLeaveDoubleLineBreaks = value;
            Messenger.Send(new SettingsChangedMessage(true));
            CfgAccess.WriteCfgFile(AppSettings);
            FormatString();
        }

        partial void OnRemoveAllLineBreaksChanged(bool value)
        {
            if (value)
            {
                LeaveDoubleLineBreaks = false;
            }
            AppSettings.FormatterDeleteAllLineBreaks = value;
            Messenger.Send(new SettingsChangedMessage(true));
            CfgAccess.WriteCfgFile(AppSettings);
            FormatString();
        }

        #endregion

        #region Constructor
        public InputFormatterViewModel()
        {
            Messenger.Register<SettingsLoadedMessage>(this, (r, m) =>
            {
                AppSettings = m.Value;

                SpellCheckEnabled = AppSettings.SpellCheckEnabled;
                RemoveAllLineBreaks = AppSettings.FormatterDeleteAllLineBreaks;
                LeaveDoubleLineBreaks = AppSettings.FormatterLeaveDoubleLineBreaks;
            });
            Messenger.Register<SettingsChangedMessage>(this, (r, m) =>
            {
                SpellCheckEnabled = AppSettings.SpellCheckEnabled;
            });

            AppSettings = Messenger.Send<SettingsRequestMessage>();

            SpellCheckEnabled = AppSettings.SpellCheckEnabled;
            RemoveAllLineBreaks = AppSettings.FormatterDeleteAllLineBreaks;
            LeaveDoubleLineBreaks = AppSettings.FormatterLeaveDoubleLineBreaks;
        }

        #endregion

        #region Methods
        private void FormatString()
        {
            string temporaryString;

            temporaryString = FormatHelper.RemoveLineBreaks(InputString, RemoveAllLineBreaks, LeaveDoubleLineBreaks);

            temporaryString = FormatHelper.RemoveSpaces(temporaryString);
            temporaryString = FormatHelper.XmlSafeFormat(temporaryString);

            OutputString = temporaryString;
        }
        #endregion
    }
}
