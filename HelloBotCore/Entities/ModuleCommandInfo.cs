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

        public ModuleCommandInfo()
        {
            
        }

        public void Init(string dllName,ModuleHandlerBase handlerModuleBase, IModuleClientHandler moduleClientHandler, AuthorInfo author)
        {
            HandleMessage = handlerModuleBase.HandleMessage;
            base.Init(dllName, handlerModuleBase, moduleClientHandler, author);
            CallCommandList = handlerModuleBase.CallCommandList.ToList();
            CommandDescription = handlerModuleBase.ModuleDescription;
        }

        public override ModuleType ModuleType
        {
            get { return ModuleType.Handler; }
        }


    }
}
