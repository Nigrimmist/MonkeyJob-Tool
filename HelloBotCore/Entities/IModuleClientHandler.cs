using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HelloBotCommunication;

namespace HelloBotCore.Entities
{
    public interface IModuleClientHandler
    {
        void SaveSettings(ModuleCommandInfo commandInfo,object serializableSettingObject);
        T GetSettings<T>(ModuleCommandInfo commandInfo);
        void ShowMessage(ModuleCommandInfo commandInfo, string content, string title = null, AnswerBehaviourType answerType = AnswerBehaviourType.ShowText, MessageType messageType = MessageType.Default);
        void RegisterTimerEvent(ModuleCommandInfo commandInfo, TimeSpan period, Action callback);
    }
}
