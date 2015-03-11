using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HelloBotCommunication;

namespace HelloBotCore.Entities
{
    public class ModuleCommandInfo
    {
        public List<CallCommandInfo> CallCommandList { get; set; }
        public string CommandDescription { get; set; }
        public delegate void HandleMessageFunc(string command, string args, Action<AnswerInfo> sendMessageFunc);
        public HandleMessageFunc HandleMessage { get; set; }

        public ModuleCommandInfo(ModuleBase handlerBase, IModuleClientHandler moduleClientHandler)
        {
            IBot bot = new ModuleToBotAdapter(moduleClientHandler,this);
            CallCommandList = handlerBase.CallCommandList.ToList();
            CommandDescription = handlerBase.CommandDescription;
            HandleMessage = (command, args, func) => handlerBase.HandleMessage(command, args);
            handlerBase.Init(bot);
        }
    }
}
