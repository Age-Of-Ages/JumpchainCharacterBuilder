using CommunityToolkit.Mvvm.Messaging.Messages;
using JumpchainCharacterBuilder.Model;

namespace JumpchainCharacterBuilder.Messages
{
    public class SettingsLoadedMessage(AppSettingsModel value) : ValueChangedMessage<AppSettingsModel>(value)
    {
    }
}
