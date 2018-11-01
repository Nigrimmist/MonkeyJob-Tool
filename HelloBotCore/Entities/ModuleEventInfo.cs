using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using HelloBotCommunication;
using HelloBotCommunication.Interfaces;
using HelloBotCore.Manager;
using SharedHelper;

namespace HelloBotCore.Entities
{
    public class ModuleEventInfo : ModuleInfoBase
    {
        public delegate void OnEventFireDelegate(Guid eventToken);
        public TimeSpan EventRunEvery { get; set; }
        public OnEventFireDelegate EventFireCallback;

        public ModuleEventInfo(StorageManager storageManager) : base(storageManager)
        {
            
        }

        public Color? BodyBackgroundColor { get; set; }
        public Color? HeaderBackgroundColor { get; set; }
        
        public override void Init(string dllName, ComponentBase rootComponent, IModuleClientHandler moduleClientHandler,AuthorInfo author)
        {
            var rootModule = rootComponent as ModuleEventBase;
            EventFireCallback = rootModule.OnFire;
            EventRunEvery = rootModule.RunEvery;
            CommandDescription = new DescriptionInfo() {Description = rootModule.ModuleDescription};
            base.Init(dllName, rootModule, author);
            BodyBackgroundColor = rootModule.BodyBackgroundColor;
            HeaderBackgroundColor = rootModule.HeaderBackGroundColor;
            IClient client = new ModuleToClientAdapter(moduleClientHandler, this);
            rootModule.Init(client);
        }

        public void CallEvent(Guid commandToken)
        {
            EventFireCallback(commandToken);
        }        

        public override ModuleType ModuleType
        {
            get { return ModuleType.Event; }
        }

        public override bool IsEnabledByDefault => false;

        public override string GetDescriptionText()
        {
            return "Запускается раз в " + EventRunEvery.Humanize();
        }

        public override string GetTypeDescription()
        {
            return "Интервальный";
        }

       
    }
}
