using CommunityToolkit.Mvvm.Messaging.Messages;

namespace JumpchainCharacterBuilder.Messages
{
    public class CharacterDeletedMessage : ValueChangedMessage<bool>
    {
        public CharacterDeletedMessage(bool value) : base(value)
        {
        }
    }
}
