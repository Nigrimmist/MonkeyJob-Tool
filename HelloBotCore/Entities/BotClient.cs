using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HelloBotCommunication;

namespace HelloBotCore.Entities
{
    public class ModuleToBotAdapter : IBot
    {
        private IModuleClientHandler _moduleClientHandler;
        private ModuleCommandInfo _moduleCommandInfo;
        public ModuleToBotAdapter(IModuleClientHandler moduleClientHandler, ModuleCommandInfo moduleCommandInfo)
        {
            _moduleClientHandler = moduleClientHandler;
            _moduleCommandInfo = moduleCommandInfo;
        }

        public void SaveSettings(object serializableSettingObject)
        {
            _moduleClientHandler.SaveSettings(serializableSettingObject);
        }

        public T GetSettings<T>()
        {
            return _moduleClientHandler.GetSettings<T>();
        }

        public void ShowMessage(string content, string title = null, AnswerBehaviourType answerType = AnswerBehaviourType.ShowText, MessageType messageType = MessageType.Default)
        {
            _moduleClientHandler.ShowMessage(content,title,answerType,messageType);
        }

        public void RegisterTimerEvent(TimeSpan period, Action callback)
        {
            _moduleClientHandler.RegisterTimerEvent(period,callback);
        }
    }
}
