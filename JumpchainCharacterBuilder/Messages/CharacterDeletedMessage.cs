using CommunityToolkit.Mvvm.Messaging.Messages;

namespace JumpchainCharacterBuilder.Messages
{
    public class CharacterDeletedMessage(bool value) : ValueChangedMessage<bool>(value)
    {
    }
}
