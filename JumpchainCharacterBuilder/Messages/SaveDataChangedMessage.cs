using CommunityToolkit.Mvvm.Messaging.Messages;

namespace JumpchainCharacterBuilder.Messages
{
    public class SaveDataChangedMessage(string value) : ValueChangedMessage<string>(value)
    {
    }
}
