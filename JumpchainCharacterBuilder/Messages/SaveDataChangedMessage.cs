using CommunityToolkit.Mvvm.Messaging.Messages;

namespace JumpchainCharacterBuilder.Messages
{
    public class SaveDataChangedMessage : ValueChangedMessage<string>
    {
        public SaveDataChangedMessage(string value) : base(value)
        {
        }
    }
}
