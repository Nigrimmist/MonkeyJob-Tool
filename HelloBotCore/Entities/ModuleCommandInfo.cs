using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HelloBotCommunication;
using HelloBotCommunication.Interfaces;

namespace HelloBotCore.Entities
{
    public class ModuleCommandInfo : ModuleCommandInfoBase
    {
        public List<CallCommandInfo> CallCommandList { get; set; }
        public delegate void HandleMessageFunc(string command, string args, Guid token);
        public HandleMessageFunc HandleMessage { get; set; }
        public delegate void OnEventFireDelegate(Guid eventToken);
        
        
        public void Init(string dllName,ModuleHandlerBase handlerModuleBase, IModuleClientHandler moduleClientHandler)
        {
            HandleMessage = handlerModuleBase.HandleMessage;
            base.Init(dllName,handlerModuleBase, moduleClientHandler);
            CallCommandList = handlerModuleBase.CallCommandList.ToList();
        }

    }
}
