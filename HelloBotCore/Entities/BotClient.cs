using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using HelloBotCommunication;
using HelloBotCommunication.Interfaces;

namespace HelloBotCore.Entities
{
    public class ModuleToClientAdapter : ITrayClient, IBotCallback, IClient
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
            _moduleClientHandler.LogModuleTraceRequest(_moduleInfo, message);
        }

        private OnCommandArgsChangedDelegate _onCommandArgsChanged; 
        public event OnCommandArgsChangedDelegate OnCommandArgsChanged
        {
            add
            {
                if (_moduleInfo.ModuleType == ModuleType.Handler)
                {
                    _moduleClientHandler.RegisterModuleChangeCommandArgsHandler(_moduleInfo as ModuleCommandInfo, NotifyModuleAboutArgsChanged);
                    _onCommandArgsChanged += value;
                }
            }
            remove { if (_onCommandArgsChanged != null) _onCommandArgsChanged -= value; }
        }

        public void ShowSuggestList(List<AutoSuggestItem> itemsToShow)
        {
            _moduleClientHandler.ShowSuggestionsToClient(itemsToShow);
        }

        private void NotifyModuleAboutArgsChanged(string command, string args)
        {

            if (_onCommandArgsChanged != null)
            {
                _onCommandArgsChanged(command, args);
            }
        }
    }
}
