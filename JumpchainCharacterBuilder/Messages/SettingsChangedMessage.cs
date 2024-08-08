using CommunityToolkit.Mvvm.Messaging.Messages;

namespace JumpchainCharacterBuilder.Messages
{
    public class SettingsChangedMessage(bool value) : ValueChangedMessage<bool>(value)
    {
    }
}
