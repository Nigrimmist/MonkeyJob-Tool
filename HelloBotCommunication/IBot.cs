using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HelloBotCommunication
{
    public interface IBot
    {
        /// <summary>
        /// Save you custom setting object to storage
        /// </summary>
        /// <param name="serializableSettingObject"></param>
        void SaveSettings(object serializableSettingObject);

        /// <summary>
        /// Return your settings if exist.
        /// </summary>
        /// <typeparam name="T">Type of your settings</typeparam>
        T GetSettings<T>() where T : class ;

        /// <summary>
        /// Show notification with text to user
        /// </summary>
        /// <param name="commandToken">Command token. Should be recieved later from client.</param>
        /// <param name="content">Text content</param>
        /// <param name="title">Message title. if null, command name will be displayed instead</param>
        /// <param name="answerType">Answer type</param>
        /// <param name="messageType">Message type</param>
        void ShowMessage(Guid commandToken,string content, string title = null, AnswerBehaviourType answerType = AnswerBehaviourType.ShowText, MessageType messageType = MessageType.Default);

        /// <summary>
        /// Will register a timer for you module, that will exist while client is alive. Callback will be called every "period" time.
        /// </summary>
        /// <param name="period">Timer's timeout</param>
        /// <param name="callback">That callback will be fired every "period" time</param>
        void RegisterTimerEvent(TimeSpan period, Action callback);
    }
}
