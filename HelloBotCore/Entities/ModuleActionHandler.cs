using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HelloBotCommunication;

namespace HelloBotCore.Entities
{
    public class ModuleActionHandler
    {
        public List<CallCommandInfo> CallCommandList { get; set; }
        public string CommandDescription { get; set; }
        public delegate void HandleMessageFunc(string command, string args, object clientData, Action<AnswerInfo> sendMessageFunc);
        public HandleMessageFunc HandleMessage { get; set; }

        public ModuleActionHandler(IActionHandler handler)
        {
            CallCommandList = handler.CallCommandList;
            CommandDescription = handler.CommandDescription;
            HandleMessage = (command, args, data, func) => handler.HandleMessage(command, args, data, (answer, type) => func(
                new AnswerInfo(){Answer = answer,Type = type,Command = command}
                ));
        }
    }
}
