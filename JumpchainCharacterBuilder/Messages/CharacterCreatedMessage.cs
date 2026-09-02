using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JumpchainCharacterBuilder.Messages
{
    public class CharacterCreatedMessage(bool value) : ValueChangedMessage<bool>(value)
    {
    }
}
