using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using HelloBotCommunication;
using HelloBotCommunication.Interfaces;

namespace HelloBotCore.Entities
{
    public class ModuleToClientAdapter : ITrayClient, IBotCallback, IClient, IIntegrationClient
    {
        private IModuleClientHandler _moduleClientHandler;
        private ComponentInfoBase _componentInfoBase;
        private Guid _lastToken;
        private ClientLanguage _clientLanguage;

        public ModuleToClientAdapter(IModuleClientHandler moduleClientHandler, ComponentInfoBase moduleInfo)
        {
            _moduleClientHandler = moduleClientHandler;
            _componentInfoBase = moduleInfo;
        }

        public void SaveSettings(object serializableSettingObject)
        {
            _moduleClientHandler.SaveSettings(_componentInfoBase, serializableSettingObject);
        }

        public T GetSettings<T>() where T : class 
        {
            return _moduleClientHandler.GetSettings<T>(_componentInfoBase);
        }

        public IBotCallback ShowMessage(Guid token, CommunicationMessage message, string title = null, AnswerBehaviourType answerType = AnswerBehaviourType.ShowText, MessageType messageType = MessageType.Default)
        {
            _lastToken = token;
            _moduleClientHandler.ShowMessage(_componentInfoBase, message, title, answerType, messageType, token);
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

        public void UpdateTrayText(Guid token, string text, Color? textColor = null, Color? backgroundColor = null, int fontSize = 12, string fontName = "Tahoma", Color? iconBorderColor = null)
        {
            _moduleClientHandler.UpdateTrayText(token,text,textColor,backgroundColor,fontSize,fontName,iconBorderColor);
        }

        public void ShowTrayBalloonTip(Guid token, string text, TimeSpan? timeout = null, TooltipType? tooltipType = null)
        {
            _moduleClientHandler.ShowTrayBalloonTip(token, text, timeout, tooltipType);
        }

        public void LogTrace(string message)
        {
            _moduleClientHandler.LogModuleTraceRequest(_componentInfoBase, message);
        }

        void IIntegrationClient.ShowMessage(Guid token, string message, string title, AnswerBehaviourType answerType, MessageType messageType)
        {
            _moduleClientHandler.ShowMessage(_componentInfoBase, CommunicationMessage.FromString(message), title, answerType, messageType, useBaseClient: true, commandToken: token);
        }

        
    }
}
