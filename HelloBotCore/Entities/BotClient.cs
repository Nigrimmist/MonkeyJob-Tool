using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using HelloBotCommunication;
using HelloBotCommunication.Interfaces;

namespace HelloBotCore.Entities
{
    public class ModuleToClientAdapter : ITrayClient, IBotCallback
    {
        private IModuleClientHandler _moduleClientHandler;
        private ModuleInfoBase _moduleInfo;
        private Guid _lastToken;
        private ClientLanguage _clientLanguage;

        public ModuleToClientAdapter(IModuleClientHandler moduleClientHandler, ModuleInfoBase moduleInfo)
        {
            _moduleClientHandler = moduleClientHandler;
            _moduleInfo = moduleInfo;
        }

        public void SaveSettings(object serializableSettingObject)
        {
            _moduleClientHandler.SaveSettings(_moduleInfo,serializableSettingObject);
        }

        public T GetSettings<T>() where T : class 
        {
            return _moduleClientHandler.GetSettings<T>(_moduleInfo);
        }

        public IBotCallback ShowMessage(Guid token, string content, string title = null, AnswerBehaviourType answerType = AnswerBehaviourType.ShowText, MessageType messageType = MessageType.Default)
        {
            _lastToken = token;
            _moduleClientHandler.ShowMessage(token,_moduleInfo, content, title, answerType, messageType);
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

        public void UpdateTrayText(Guid token,string text)
        {
            _moduleClientHandler.SetTrayText(token,text);
        }

        public void UpdateTrayColor(Guid token,Color color)
        {
            _moduleClientHandler.SetTrayColor(token,color);
        }
    }
}
