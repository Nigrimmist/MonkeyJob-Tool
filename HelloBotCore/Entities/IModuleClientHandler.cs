using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using HelloBotCommunication;
using HelloBotCommunication.Interfaces;
using Newtonsoft.Json.Bson;

namespace HelloBotCore.Entities
{
    public interface IModuleClientHandler
    {
        void SaveSettings<T>(ComponentInfoBase info, T serializableSettingObject) where T : class;
        T GetSettings<T>(ComponentInfoBase info) where T : class;
        void ShowMessage(ComponentInfoBase moduleInfo, CommunicationMessage message, string title = null, AnswerBehaviourType answerType = AnswerBehaviourType.ShowText, MessageType messageType = MessageType.Default, Guid? commandToken = null, bool useBaseClient = false, UniqueMessageHash uniqueMsgKey = null);
        void RegisterUserReactionCallback(Guid commandToken, UserReactionToCommandType userCallbackType, Action callback);
        Language GetCurrentLanguage();
        double GetCurrentVersion();
        double GetUIClientVersion();
        void UpdateTrayText(Guid token, string text, Color? textColor = null, Color? backgroundColor = null, int fontSize = 12, string fontName = "Tahoma", Color? iconBorderColor = null);
        void ShowTrayBalloonTip(Guid token, string text, TimeSpan? timeout = null, TooltipType? tooltipType = null);
        void LogComponentTraceRequest(ComponentInfoBase moduleInfo, string message);
    }
}
