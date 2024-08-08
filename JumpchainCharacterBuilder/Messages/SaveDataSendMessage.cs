using CommunityToolkit.Mvvm.Messaging.Messages;
using JumpchainCharacterBuilder.Model;

namespace JumpchainCharacterBuilder.Messages
{
    public class SaveDataSendMessage(SaveFile value) : ValueChangedMessage<SaveFile>(value)
    {
    }
}
