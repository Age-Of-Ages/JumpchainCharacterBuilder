using CommunityToolkit.Mvvm.Messaging.Messages;

namespace JumpchainCharacterBuilder.Messages
{
    public class JumpSelectionUpdateMessage(int value) : ValueChangedMessage<int>(value)
    {
    }
}
