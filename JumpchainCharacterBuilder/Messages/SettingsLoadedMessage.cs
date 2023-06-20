using CommunityToolkit.Mvvm.Messaging.Messages;
using JumpchainCharacterBuilder.Model;

namespace JumpchainCharacterBuilder.Messages
{
    public class SettingsLoadedMessage : ValueChangedMessage<AppSettingsModel>
    {
        public SettingsLoadedMessage(AppSettingsModel value) : base(value)
        {
        }
    }
}
