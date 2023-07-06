using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using JumpchainCharacterBuilder.Messages;
using JumpchainCharacterBuilder.Model;

namespace JumpchainCharacterBuilder.ViewModel
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        // TODO - Implement themes.
        // TODO - (Eventually) make the whole UI dynamic.
        #region Fields
        [ObservableProperty]
        private SaveFile _loadedSave = new();
        [ObservableProperty]
        private AppSettingsModel _appSettings = new();

        [ObservableProperty]
        private bool _resizeAllowed = false;

        #endregion

        #region Properties


        #endregion

        public MainWindowViewModel()
        {
            Messenger.Register<MainWindowViewModel, SettingsRequestMessage>(this, (r, m) =>
            {
                m.Reply(r.AppSettings);
            });
            Messenger.Register<SettingsChangedMessage>(this, (r, m) =>
            {
                ResizeAllowed = AppSettings.CanResizeWindow;
            });

            Messenger.Send(new SaveDataSendMessage(LoadedSave));

            CfgAccess.ReadCfgFile(AppSettings);
            Messenger.Send(new SettingsLoadedMessage(AppSettings));

            ResizeAllowed = AppSettings.CanResizeWindow;
        }
    }
}
