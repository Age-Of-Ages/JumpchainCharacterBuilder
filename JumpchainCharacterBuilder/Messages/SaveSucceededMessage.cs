using CommunityToolkit.Mvvm.Messaging.Messages;

namespace JumpchainCharacterBuilder.Messages
{
    public class SaveSucceededMessage(bool value) : ValueChangedMessage<bool>(value)
    {
    }
}
