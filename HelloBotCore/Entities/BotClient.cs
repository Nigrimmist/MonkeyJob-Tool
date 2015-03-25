using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HelloBotCommunication;
using HelloBotCommunication.Interfaces;

namespace HelloBotCore.Entities
{
    public class ModuleToClientAdapter : IClient, IBotCallback
    {
        private IModuleClientHandler _moduleClientHandler;
        private ModuleCommandInfoBase _moduleCommandInfo;
        private Guid _lastToken;
        public ModuleToClientAdapter(IModuleClientHandler moduleClientHandler, ModuleCommandInfoBase moduleCommandInfo)
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

        public IBotCallback ShowMessage(Guid token, string content, string title = null, AnswerBehaviourType answerType = AnswerBehaviourType.ShowText, MessageType messageType = MessageType.Default)
        {
            _lastToken = token;
            _moduleClientHandler.ShowMessage(token,_moduleCommandInfo, content, title, answerType, messageType);
            return this;
        }

        public IBotCallback OnClick(Action onClickCallback)
        {
            _moduleClientHandler.RegisterUserReactionCallback(_lastToken, UserReactionToCommandType.Clicked, onClickCallback);
            return this;
        }

        public IBotCallback OnIgnore(Action onIgnoreCallback)
        {
            _moduleClientHandler.RegisterUserReactionCallback(_lastToken, UserReactionToCommandType.Ignored, onIgnoreCallback);
            return this;
        }

        public IBotCallback OnNotified(Action onNotifiedCallback)
        {
            _moduleClientHandler.RegisterUserReactionCallback(_lastToken, UserReactionToCommandType.Notified, onNotifiedCallback);
            return this;
        }
    }
}
