using CommunityToolkit.Mvvm.Messaging.Messages;
using JumpchainCharacterBuilder.Model;

namespace JumpchainCharacterBuilder.Messages
{
    public class AddJumpToChainMessage(JumpRandomizerEntry value) : ValueChangedMessage<JumpRandomizerEntry>(value)
    {
    }
}
