using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace HelloBotCommunication.Interfaces
{
    
    public interface IClient : IBaseClient
    {
        /// <summary>
        /// Show notification with text to user
        /// </summary>
        /// <param name="token">Command token. Should be recieved later from client.</param>
        /// <param name="message">Message for clients, may contains text/urls/media etc</param>
        /// <param name="title">Message title. if null, command name will be displayed instead</param>
        /// <param name="answerType">Answer type</param>
        /// <param name="messageType">Message type</param>
        /// <param name="uniqueMsgKey">Provide message unique object to prevent sending identical multiple messages to client. By default, we use message param as unique "string". Please, specify groupId if you would like to separate messages by "channels" to notify client.</param>
        IBotCallback SendMessage(Guid token, CommunicationMessage message, string title = null, AnswerBehaviourType answerType = AnswerBehaviourType.ShowText, MessageType messageType = MessageType.Default, UniqueMessageHash uniqueMsgKey = null);   
    }
}
