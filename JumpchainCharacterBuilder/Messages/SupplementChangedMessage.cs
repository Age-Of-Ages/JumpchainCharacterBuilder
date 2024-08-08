using CommunityToolkit.Mvvm.Messaging.Messages;

namespace JumpchainCharacterBuilder.Messages
{
    public class SupplementChangedMessage(string value) : ValueChangedMessage<string>(value)
    {
    }
}
