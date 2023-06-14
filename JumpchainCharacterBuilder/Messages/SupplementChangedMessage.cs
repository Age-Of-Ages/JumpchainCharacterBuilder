using CommunityToolkit.Mvvm.Messaging.Messages;

namespace JumpchainCharacterBuilder.Messages
{
    public class SupplementChangedMessage : ValueChangedMessage<string>
    {
        public SupplementChangedMessage(string value) : base(value)
        {
        }
    }
}
