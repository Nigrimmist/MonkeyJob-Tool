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
                    sb.AppendLine(messagePart.Value.ToString());
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

        /// <summary>
        /// Use it when you should like to reset all previous message hashes. For example, when you call sendMessage with identical content - client will receive only one of them, but if you will call one, then second with NoContent and third - the same as first - two messages will appear.
        /// </summary>
        /// <returns></returns>
        public static CommunicationMessage NoContent()
        {
            return new CommunicationMessage() { MessageParts = new List<CommunicationMessagePart>() { new CommunicationMessagePart() { MessageFormat = CommunicationMessageFormat.NoContent } } };
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
