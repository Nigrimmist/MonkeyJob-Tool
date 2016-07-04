﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelloBotCommunication;

namespace HelloBotCore.Entities
{
    public abstract class IntegrationClientBase : ComponentInfoBase
    {
        protected IntegrationClientBase(string settingsFolderAbsolutePath, string logsFolderAbsolutePath) : base(settingsFolderAbsolutePath, logsFolderAbsolutePath)
        {
        }

        public abstract void SendMessageToClient(Guid token, CommunicationMessage message);

    }
}
