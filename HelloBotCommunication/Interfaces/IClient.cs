﻿using System;
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
        IBotCallback ShowMessage(Guid token, CommunicationMessage message, string title = null, AnswerBehaviourType answerType = AnswerBehaviourType.ShowText, MessageType messageType = MessageType.Default);
    }
}
