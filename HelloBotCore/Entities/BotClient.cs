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
            _moduleClientHandler.SaveSettings(_moduleCommandInfo,serializableSettingObject);
        }

        public T GetSettings<T>() where T : class 
        {
            return _moduleClientHandler.GetSettings<T>(_moduleCommandInfo);
        }

        public void ShowMessage(Guid commandToken,string content, string title = null, AnswerBehaviourType answerType = AnswerBehaviourType.ShowText, MessageType messageType = MessageType.Default)
        {
            _moduleClientHandler.ShowMessage(commandToken,_moduleCommandInfo, content, title, answerType, messageType);
        }

        public void RegisterTimerEvent(TimeSpan period, Action callback)
        {
            _moduleClientHandler.RegisterTimerEvent(_moduleCommandInfo, period, callback);
        }
    }
}
