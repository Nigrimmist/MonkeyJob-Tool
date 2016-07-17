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

        public static CommunicationMessage FromString(string content)
        {
            return new CommunicationMessage() {MessageParts = new List<CommunicationMessagePart>() {new CommunicationMessagePart() {Value = content, MessageFormat = CommunicationMessageFormat.Text}}};
        }

        public static CommunicationMessage FromUrl(string url)
        {
            return new CommunicationMessage() { MessageParts = new List<CommunicationMessagePart>() { new CommunicationMessagePart() { Value = url, MessageFormat = CommunicationMessageFormat.Url } } };
        }

        public CommunicationMessage AppendString(string content)
        {
            MessageParts.Add(new CommunicationMessagePart(){MessageFormat = CommunicationMessageFormat.Text,Value = content});
            return this;
        }
        public CommunicationMessage AppendUrl(string url)
        {
            MessageParts.Add(new CommunicationMessagePart() { MessageFormat = CommunicationMessageFormat.Url, Value = url });
            return this;
        }
    }
}
