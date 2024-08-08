using CommunityToolkit.Mvvm.Messaging.Messages;

namespace JumpchainCharacterBuilder.Messages
{
    public class CategoryChangedMessage(bool value) : ValueChangedMessage<bool>(value)
    {
    }
}
