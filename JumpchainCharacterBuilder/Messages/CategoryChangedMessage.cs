using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JumpchainCharacterBuilder.Messages
{
    public class CategoryChangedMessage : ValueChangedMessage<bool>
    {
        public CategoryChangedMessage(bool value) : base(value)
        {
        }
    }
}
