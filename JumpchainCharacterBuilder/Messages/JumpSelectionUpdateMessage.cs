using CommunityToolkit.Mvvm.Messaging.Messages;

namespace JumpchainCharacterBuilder.Messages
{
    public class JumpSelectionUpdateMessage : ValueChangedMessage<int>
    {
        public JumpSelectionUpdateMessage(int value) : base(value)
        {
        }
    }
}
