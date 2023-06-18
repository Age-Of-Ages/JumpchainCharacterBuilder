﻿using CommunityToolkit.Mvvm.Messaging.Messages;
using JumpchainCharacterBuilder.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JumpchainCharacterBuilder.Messages
{
    public class SettingsRequestMessage : RequestMessage<AppSettingsModel>
    {
    }
}
