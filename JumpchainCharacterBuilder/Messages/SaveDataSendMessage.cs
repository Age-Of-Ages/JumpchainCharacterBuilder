using CommunityToolkit.Mvvm.Messaging.Messages;
using JumpchainCharacterBuilder.Model;

namespace JumpchainCharacterBuilder.Messages
{
    public class SaveDataSendMessage : ValueChangedMessage<SaveFile>
    {
        public SaveDataSendMessage(SaveFile value) : base(value)
        {
        }
    }
}
