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
        public delegate void HandleMessageFunc(string command, string args, Guid token);
        public HandleMessageFunc HandleMessage { get; set; }
        public double Version { get; set; }
        public string ModuleName { get; set; }

        public ModuleCommandInfo()
        {
            Id = Guid.NewGuid();
        }

        public void Init(string dllName,ModuleBase handlerBase, IModuleClientHandler moduleClientHandler)
        {
            CommandDescription = handlerBase.CommandDescription;
            Version = handlerBase.ModuleVersion;
            HandleMessage = handlerBase.HandleMessage;
            var handType = handlerBase.GetType();
            ModuleName = dllName + "." + handType.Name;
            IBot bot = new ModuleToBotAdapter(moduleClientHandler, this);
            handlerBase.Init(bot);
            CallCommandList = handlerBase.CallCommandList.ToList();
        }
    }
}
