using System;

namespace HelloBotCommunication.Interfaces
{
    public interface IClient
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
        /// <param name="token">Command token. Should be recieved later from client.</param>
        /// <param name="content">Text content</param>
        /// <param name="title">Message title. if null, command name will be displayed instead</param>
        /// <param name="answerType">Answer type</param>
        /// <param name="messageType">Message type</param>
        IBotCallback ShowMessage(Guid token, string content, string title = null, AnswerBehaviourType answerType = AnswerBehaviourType.ShowText, MessageType messageType = MessageType.Default);

        /// <summary>
        /// Determine client ui language
        /// </summary>
        ClientLanguage ClientLanguage { get; }

        /// <summary>
        /// Determine bot version
        /// </summary>
        double BotVersion { get; }

        /// <summary>
        /// Determine ui app version
        /// </summary>
        double UiClientVersion { get; }
    }
}
