using CommunityToolkit.Mvvm.Messaging.Messages;

namespace JumpchainCharacterBuilder.Messages
{
    public class CategoryChangedMessage : ValueChangedMessage<bool>
    {
        public CategoryChangedMessage(bool value) : base(value)
        {
        }
    }
}
