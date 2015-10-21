using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace HelloBotCommunication.Interfaces
{
    public delegate void OnCommandArgsChangedDelegate(string command,string args);
    public interface IClient : IBaseClient
    {
        /// <summary>
        /// Show notification with text to user
        /// </summary>
        /// <param name="token">Command token. Should be recieved later from client.</param>
        /// <param name="content">Text content</param>
        /// <param name="title">Message title. if null, command name will be displayed instead</param>
        /// <param name="answerType">Answer type</param>
        /// <param name="messageType">Message type</param>
        IBotCallback ShowMessage(Guid token, string content, string title = null, AnswerBehaviourType answerType = AnswerBehaviourType.ShowText, MessageType messageType = MessageType.Default);


        /// <summary>
        /// Fired avery time when arguments to your command has been changed by user. Can be used only for commands
        /// </summary>
        event OnCommandArgsChangedDelegate OnCommandArgsChanged;

        /// <summary>
        /// Will display suggestions for users
        /// </summary>
        /// <param name="itemsToShow"></param>
        void ShowSuggestList(List<AutoSuggestItem> itemsToShow);
    }
}
