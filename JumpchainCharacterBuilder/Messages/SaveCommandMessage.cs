using CommunityToolkit.Mvvm.Messaging.Messages;

namespace JumpchainCharacterBuilder.Messages
{
    public class SaveCommandMessage(bool value) : ValueChangedMessage<bool>(value)
    {
    }
}
