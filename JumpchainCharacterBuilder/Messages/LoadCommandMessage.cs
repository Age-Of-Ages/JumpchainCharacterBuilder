using CommunityToolkit.Mvvm.Messaging.Messages;

namespace JumpchainCharacterBuilder.Messages
{
    public class LoadCommandMessage : ValueChangedMessage<bool>
    {
        public LoadCommandMessage(bool value) : base(value)
        {
        }
    }
}
