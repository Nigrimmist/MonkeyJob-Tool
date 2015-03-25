using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HelloBotCommunication;
using HelloBotCommunication.Interfaces;

namespace HelloBotCore.Entities
{
    public class ModuleCommandInfoBase
    {
        public Guid Id { get; set; }
        public string CommandDescription { get; set; }
        public double Version { get; set; }
        public string ModuleName { get; set; }

        public ModuleCommandInfoBase()
        {
            Id = Guid.NewGuid();
        }

        public void Init(string dllName, ModuleBase handlerModuleBase, IModuleClientHandler moduleClientHandler)
        {
            CommandDescription = handlerModuleBase.ModuleDescription;
            Version = handlerModuleBase.ModuleVersion;           
            var handType = handlerModuleBase.GetType();
            ModuleName = dllName + "." + handType.Name;
            IClient client = new ModuleToClientAdapter(moduleClientHandler, this);
            handlerModuleBase.Init(client);
            
        }
    }


}
