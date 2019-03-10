using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using HelloBotCommunication;
using HelloBotCommunication.Interfaces;
using HelloBotCore.Helpers;
using HelloBotCore.Manager;
using SharedHelper;

namespace HelloBotCore.Entities
{

    public class ModuleTrayInfo : ModuleInfoBase
    {
        public delegate void OnEventFireDelegate(Guid eventToken);
        public TimeSpan EventRunEvery { get; set; }
        public OnEventFireDelegate EventFireCallback;

        public ModuleTrayInfo(StorageManager storageManager) : base(storageManager)
        {
            
        }

        public Icon TrayIcon { get; set; }

        public override void Init(string dllName, ComponentBase rootComponent, IModuleClientHandler moduleClientHandler, AuthorInfo author)
        {
            ModuleTrayBase trayModuleBase = rootComponent as ModuleTrayBase;
            EventFireCallback = trayModuleBase.OnFire;
            EventRunEvery = trayModuleBase.RunEvery;
            CommandDescription = new DescriptionInfo() { Description = trayModuleBase.ModuleDescription };
            base.Init(dllName, trayModuleBase, author);
            if (!string.IsNullOrEmpty(trayModuleBase.TrayIconInBase64))
            {
                TrayIcon = ImageHelper.ConvertoToIcon(ImageHelper.ResizeImage(ImageHelper.GetFromBase64(trayModuleBase.TrayIconInBase64), 16, 16));
            }

            ITrayClient client = new ModuleToClientAdapter(moduleClientHandler, this);
            trayModuleBase.Init(client);
        }

        public void CallEvent(Guid commandToken)
        {
            EventFireCallback(commandToken);
        }

        public override ComponentType ModuleType
        {
            get { return ComponentType.Tray; }
        }

        public override bool IsEnabledByDefault => false;
        public override string GetDescriptionText()
        {
            string runEveryStr = string.Empty;

            if (EventRunEvery.Days > 0) runEveryStr += EventRunEvery.Days + " д. ";
            if (EventRunEvery.Hours > 0) runEveryStr += EventRunEvery.Hours + " ч. ";
            if (EventRunEvery.Minutes > 0) runEveryStr += EventRunEvery.Minutes + " мин. ";
            if (EventRunEvery.Seconds > 0) runEveryStr += EventRunEvery.Seconds + " сек.";

            return "Запускается раз в " + runEveryStr;
        }

        public override string GetTypeDescription()
        {
            return "Трей";
        }
    }
}
