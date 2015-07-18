using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using HelloBotCommunication;
using HelloBotCommunication.Interfaces;
using HelloBotCore.Helpers;

namespace HelloBotCore.Entities
{

    public class ModuleTrayInfo : ModuleInfoBase
    {
        public delegate void OnEventFireDelegate(Guid eventToken);
        public TimeSpan EventRunEvery { get; set; }
        public OnEventFireDelegate EventFireCallback;
        public Icon TrayIcon { get; set; }

        public void Init(string dllName, ModuleTrayBase trayModuleBase, IModuleClientHandler moduleClientHandler, AuthorInfo author)
        {
            EventFireCallback = trayModuleBase.OnFire;
            EventRunEvery = trayModuleBase.RunEvery;
            CommandDescription = new DescriptionInfo() { Description = trayModuleBase.ModuleDescription };
            base.Init(dllName, trayModuleBase, author);
            if (!string.IsNullOrEmpty(trayModuleBase.TrayIconIn64Base))
            {
                TrayIcon = ImageHelper.ConvertoToIcon(ImageHelper.ResizeImage(ImageHelper.GetFromBase64(trayModuleBase.TrayIconIn64Base), 32, 32));
            }

            ITrayClient client = new ModuleToClientAdapter(moduleClientHandler, this);
            trayModuleBase.Init(client);
        }

        public void CallEvent(Guid commandToken)
        {
            EventFireCallback(commandToken);
        }

        public override ModuleType ModuleType
        {
            get { return ModuleType.Tray; }
        }

        public override string GetDescriptionText()
        {
            string runEveryStr = string.Empty;

            if (EventRunEvery.Days > 0) runEveryStr += EventRunEvery.Days + " д. ";
            if (EventRunEvery.Hours > 0) runEveryStr += EventRunEvery.Hours + " ч. ";
            if (EventRunEvery.Minutes > 0) runEveryStr += EventRunEvery.Minutes + " мин. ";
            if (EventRunEvery.Seconds > 0) runEveryStr += EventRunEvery.Seconds + " сек.";

            return "Запускается раз в " + runEveryStr;
        }
    }
}
