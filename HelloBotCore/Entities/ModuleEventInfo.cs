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

        public void Init(string dllName, ModuleEventBase eventModuleBase, IModuleClientHandler moduleClientHandler,AuthorInfo author)
        {
            EventFireCallback = eventModuleBase.OnFire;
            EventRunEvery = eventModuleBase.RunEvery;
            CommandDescription = new DescriptionInfo() {Description = eventModuleBase.ModuleDescription};
            base.Init(dllName, eventModuleBase, moduleClientHandler, author);
        }

        public void CallEvent(Guid commandToken)
        {
            EventFireCallback(commandToken);
        }

        public override ModuleType ModuleType
        {
            get { return ModuleType.Event; }
        }

        public override string ToString()
        {
            var toReturn = "Запускается раз в " + EventRunEvery;

            return base.ToString() + toReturn;
        }
    }
}
