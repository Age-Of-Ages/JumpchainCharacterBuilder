using CommunityToolkit.Mvvm.Messaging.Messages;

namespace JumpchainCharacterBuilder.Messages
{
    public class SaveCommandMessage : ValueChangedMessage<bool>
    {
        public SaveCommandMessage(bool value) : base(value)
        {
        }
    }
}
