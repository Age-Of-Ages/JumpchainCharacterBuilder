using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using JumpchainCharacterBuilder.Messages;
using JumpchainCharacterBuilder.Model;
using System.Text.RegularExpressions;

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
            FormatString();
        }

        partial void OnRemoveAllLineBreaksChanged(bool value)
        {
            if (value)
            {
                LeaveDoubleLineBreaks = false;
            }
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
            });
            Messenger.Register<SettingsChangedMessage>(this, (r, m) =>
            {
                SpellCheckEnabled = AppSettings.SpellCheckEnabled;
            });
        }

        #endregion

        #region Methods
        private void FormatString()
        {
            string temporaryString;

            // Copying from PDF files can cause issues with line-breaks being inserted in incorrect places, so these need to be removed.
            if (RemoveAllLineBreaks)
            {
                temporaryString = RemoveAllLineBreaksRegex().Replace(InputString, " "); 
            }
            else if (LeaveDoubleLineBreaks)
            {
                temporaryString = RemoveLineBreaksNoDoubleRegex().Replace(InputString, " ");
            }
            else
            {
                temporaryString = RemoveLineBreaksRegex().Replace(InputString, " ");
            }
            // If a string with too many line-breaks is put through this formatter multiple times to fully correct it then extra spaces will appear.
            // These should be removed to preserve the tidiness of the string.
            // Intended line-breaks also leave spaces at the beginning of the paragraph, so these should be corrected.
            temporaryString = RemoveDoubleSpacesRegex().Replace(temporaryString, " ");
            temporaryString = RemoveParagraphStartSpacesRegex().Replace(temporaryString, "");

            temporaryString = XmlFilterRegex().Replace(temporaryString, "");

            OutputString = temporaryString;
        }

        [GeneratedRegex("(\\r\\n)(?!\\r\\n)")]
        private static partial Regex RemoveLineBreaksRegex();

        [GeneratedRegex("(\\r\\n)")]
        private static partial Regex RemoveAllLineBreaksRegex();

        [GeneratedRegex("(?<!\\r\\n)(\\r\\n)(?!\\r\\n)")]
        private static partial Regex RemoveLineBreaksNoDoubleRegex();

        [GeneratedRegex("  ")]
        private static partial Regex RemoveDoubleSpacesRegex();

        [GeneratedRegex("(?<=\\n) ")]
        private static partial Regex RemoveParagraphStartSpacesRegex();

        [GeneratedRegex("[^\x09\x0A\x0D\x20-\uD7FF\uE000-\uFFFD]", RegexOptions.Compiled)]
        private static partial Regex XmlFilterRegex();
        #endregion
    }
}
