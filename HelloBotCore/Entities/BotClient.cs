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
        private ClientLanguage _clientLanguage;

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

        public IBotCallback OnClosed(Action onNotifiedCallback)
        {
            _moduleClientHandler.RegisterUserReactionCallback(_lastToken, UserReactionToCommandType.Closed, onNotifiedCallback);
            return this;
        }

        public ClientLanguage ClientLanguage
        {
            get { return (ClientLanguage)(int)_moduleClientHandler.GetCurrentLanguage(); }
        }

        public double BotVersion
        {
            get { return _moduleClientHandler.GetCurrentVersion(); }
        }

        public double UiClientVersion
        {
            get { return _moduleClientHandler.GetUIClientVersion(); }
        }
    }
}
