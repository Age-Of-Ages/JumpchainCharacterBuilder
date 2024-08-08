using CommunityToolkit.Mvvm.Messaging.Messages;

namespace JumpchainCharacterBuilder.Messages
{
    public class LoadCommandMessage(bool value) : ValueChangedMessage<bool>(value)
    {
    }
}
