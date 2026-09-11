using CommunityToolkit.Mvvm.Messaging.Messages;

namespace JumpchainCharacterBuilder.Messages
{
    public class CharacterCreatedMessage(bool value) : ValueChangedMessage<bool>(value)
    {
    }
}
