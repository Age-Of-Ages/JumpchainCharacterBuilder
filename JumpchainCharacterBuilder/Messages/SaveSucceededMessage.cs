using CommunityToolkit.Mvvm.Messaging.Messages;

namespace JumpchainCharacterBuilder.Messages
{
    public class SaveSucceededMessage : ValueChangedMessage<bool>
    {
        public SaveSucceededMessage(bool value) : base(value)
        {
        }
    }
}
