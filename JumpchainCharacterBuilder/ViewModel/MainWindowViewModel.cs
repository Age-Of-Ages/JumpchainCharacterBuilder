using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using JumpchainCharacterBuilder.Interfaces;
using JumpchainCharacterBuilder.Messages;
using JumpchainCharacterBuilder.Model;

namespace JumpchainCharacterBuilder.ViewModel
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        // TODO - Implement themes.
        // TODO - (Eventually) make the whole UI dynamic.
        #region Fields
        private readonly IDialogService _dialogService;

        [ObservableProperty]
        private SaveFile _loadedSave = new();
        [ObservableProperty]
        private AppSettingsModel _appSettings = new();

        [ObservableProperty]
        private bool _resizeAllowed = false;

        [ObservableProperty]
        private bool _saveSucceeded = true;

        #endregion

        #region Properties


        #endregion

        #region Constructor
        public MainWindowViewModel()
        {
            
        }

        public MainWindowViewModel(IDialogService dialogService)
        {
            Messenger.Register<MainWindowViewModel, SettingsRequestMessage>(this, (r, m) =>
            {
                m.Reply(r.AppSettings);
            });
            Messenger.Register<SettingsChangedMessage>(this, (r, m) =>
            {
                ResizeAllowed = AppSettings.CanResizeWindow;
            });
            Messenger.Register<SaveSucceededMessage>(this, (r, m) =>
            {
                SaveSucceeded = m.Value;
            });

            Messenger.Send(new SaveDataSendMessage(LoadedSave));

            CfgAccess.ReadCfgFile(AppSettings);
            Messenger.Send(new SettingsLoadedMessage(AppSettings));

            ResizeAllowed = AppSettings.CanResizeWindow;

            _dialogService = dialogService;
        }

        #endregion

        #region Commands
        [RelayCommand]
        private void TriggerSave()
        {
            SaveSucceeded = false;

            if (AppSettings.ConfirmSaveOnClose)
            {
                if (_dialogService.ConfirmDialog("Save current Jumper data before closing?"))
                {
                    Messenger.Send(new SaveCommandMessage(true));
                }
                else
                {
                    SaveSucceeded = true;
                }
            }
            else
            {
                SaveSucceeded = true;
            }
        }

        [RelayCommand]
        private void SaveHotkey()
        {
            Messenger.Send(new SaveCommandMessage(true));
        }

        [RelayCommand]
        private void OpenHotkey()
        {
            Messenger.Send(new LoadCommandMessage(true));
        }
        #endregion
    }
}
