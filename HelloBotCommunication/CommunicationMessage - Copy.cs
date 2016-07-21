using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloBotCommunication
{
    public class CommunicationClientMessage : CommunicationMessage
    {
        public string FromModule { get; set; }

        public CommunicationClientMessage(CommunicationMessage baseMsg)
        {
            this.MessageParts = baseMsg.MessageParts;
        }
    }
}
