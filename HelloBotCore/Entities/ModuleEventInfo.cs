using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HelloBotCommunication;
using HelloBotCommunication.Interfaces;

namespace HelloBotCore.Entities
{
    public class ModuleEventInfo : ModuleCommandInfoBase
    {
        public delegate void OnEventFireDelegate(Guid eventToken);
        public TimeSpan EventRunEvery { get; set; }
        public OnEventFireDelegate EventFireCallback;

        public void Init(string dllName, ModuleEventBase eventModuleBase, IModuleClientHandler moduleClientHandler)
        {
            EventFireCallback = eventModuleBase.OnFire;
            EventRunEvery = eventModuleBase.RunEvery;
            base.Init(dllName, eventModuleBase, moduleClientHandler);
        }

        public void CallEvent(Guid commandToken)
        {
            EventFireCallback(commandToken);
        }
    }
}
