using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloBotCommunication
{
    public class CommunicationMessage
    {
        public List<CommunicationMessagePart> MessageParts { get; set; }
        public string FromModule { get; set; }

        public CommunicationMessage()
        {
            MessageParts = new List<CommunicationMessagePart>();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var messagePart in MessageParts)
            {
                if (messagePart.MessageFormat == CommunicationMessageFormat.Text || messagePart.MessageFormat == CommunicationMessageFormat.Url)
                    sb.AppendLine(messagePart.Value as string);
            }
            return sb.ToString();
        }
    }
}
