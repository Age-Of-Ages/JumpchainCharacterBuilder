using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using JumpchainCharacterBuilder.Messages;
using JumpchainCharacterBuilder.Model;

namespace JumpchainCharacterBuilder.ViewModel
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        #region Fields
        // TODO - Consider converting all datagrids to gridviews for improved performance.
        [ObservableProperty]
        private SaveFile _loadedSave = new SaveFile();

        #endregion

        #region Properties


        #endregion

        public MainWindowViewModel()
        {
            Messenger.Send(new SaveDataSendMessage(LoadedSave));
        }
    }
}
