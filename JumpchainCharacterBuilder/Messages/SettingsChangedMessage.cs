using CommunityToolkit.Mvvm.Messaging.Messages;

namespace JumpchainCharacterBuilder.Messages
{
    public class SettingsChangedMessage : ValueChangedMessage<bool>
    {
        public SettingsChangedMessage(bool value) : base(value)
        {
        }
    }
}
