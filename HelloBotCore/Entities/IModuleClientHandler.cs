using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HelloBotCommunication;
using HelloBotCommunication.Interfaces;

namespace HelloBotCore.Entities
{
    public interface IModuleClientHandler
    {
        void SaveSettings<T>(ModuleCommandInfo commandInfo, T serializableSettingObject) where T : class;
        T GetSettings<T>(ModuleCommandInfo commandInfo) where T : class;
        void ShowMessage(Guid commandToken, ModuleCommandInfo commandInfo, string content, string title = null, AnswerBehaviourType answerType = AnswerBehaviourType.ShowText, MessageType messageType = MessageType.Default);
        void RegisterUserReactionCallback(Guid commandToken, UserReactionToCommandType userCallbackType, Action callback);
    }
}
