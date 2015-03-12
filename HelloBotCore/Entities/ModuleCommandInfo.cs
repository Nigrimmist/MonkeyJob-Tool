using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HelloBotCommunication;

namespace HelloBotCore.Entities
{
    public class ModuleCommandInfo
    {
        public Guid Id { get; set; }
        public List<CallCommandInfo> CallCommandList { get; set; }
        public string CommandDescription { get; set; }
        public delegate void HandleMessageFunc(string command, string args);
        public HandleMessageFunc HandleMessage { get; set; }
        public double Version { get; set; }
        public string ModuleName { get; set; }

        public ModuleCommandInfo(ModuleBase handlerBase, IModuleClientHandler moduleClientHandler)
        {
            Id = Guid.NewGuid();
            IBot bot = new ModuleToBotAdapter(moduleClientHandler,this);
            CallCommandList = handlerBase.CallCommandList.ToList();
            CommandDescription = handlerBase.CommandDescription;
            Version = handlerBase.ModuleVersion;
            HandleMessage = handlerBase.HandleMessage;
            handlerBase.Init(bot);
            ModuleName = moduleClientHandler.GetType().Name;
        }
    }
}
