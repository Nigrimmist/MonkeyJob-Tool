using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HelloBotCommunication;
using HelloBotCommunication.Interfaces;

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
        public ModuleType ModuleType { get; set; }
        public delegate void OnEventFireDelegate(Guid eventToken);
        public TimeSpan EventRunEvery { get; set; }

        public OnEventFireDelegate EventFireCallback;

        public ModuleCommandInfo()
        {
            Id = Guid.NewGuid();
        }

        public void InitModule(string dllName,ModuleHandlerBase handlerModuleBase, IModuleClientHandler moduleClientHandler)
        {
            ModuleType = ModuleType.Handler;
            CommandDescription = handlerModuleBase.ModuleDescription;
            Version = handlerModuleBase.ModuleVersion;
            HandleMessage = handlerModuleBase.HandleMessage;
            var handType = handlerModuleBase.GetType();
            ModuleName = dllName + "." + handType.Name;
            IClient client = new ModuleToClientAdapter(moduleClientHandler, this);
            handlerModuleBase.Init(client);

            CallCommandList = handlerModuleBase.CallCommandList.ToList();

        }

        public void InitEvent(string dllName, ModuleEventBase eventModuleBase, IModuleClientHandler moduleClientHandler)
        {
            ModuleType = ModuleType.Event;
            CommandDescription = eventModuleBase.ModuleDescription;
            Version = eventModuleBase.ModuleVersion;
            var handType = eventModuleBase.GetType();
            ModuleName = dllName + "." + handType.Name;
            EventFireCallback = eventModuleBase.OnFire;
            EventRunEvery = eventModuleBase.RunEvery;
            IClient client = new ModuleToClientAdapter(moduleClientHandler, this);
            eventModuleBase.Init(client);
        }

        public void CallEvent(Guid commandToken)
        {
            EventFireCallback(commandToken);
        }
    }
}
