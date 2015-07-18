﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using HelloBotCommunication;
using HelloBotCommunication.Interfaces;

namespace HelloBotCore.Entities
{
    public class ModuleEventInfo : ModuleInfoBase
    {
        public delegate void OnEventFireDelegate(Guid eventToken);
        public TimeSpan EventRunEvery { get; set; }
        public OnEventFireDelegate EventFireCallback;
        public Color? BodyBackgroundColor { get; set; }
        public Color? HeaderBackgroundColor { get; set; }

        public void Init(string dllName, ModuleEventBase eventModuleBase, IModuleClientHandler moduleClientHandler,AuthorInfo author)
        {
            EventFireCallback = eventModuleBase.OnFire;
            EventRunEvery = eventModuleBase.RunEvery;
            CommandDescription = new DescriptionInfo() {Description = eventModuleBase.ModuleDescription};
            base.Init(dllName, eventModuleBase, author);
            BodyBackgroundColor = eventModuleBase.BodyBackgroundColor;
            HeaderBackgroundColor = eventModuleBase.HeaderBackGroundColor;
            IClient client = new ModuleToClientAdapter(moduleClientHandler, this);
            eventModuleBase.Init(client);
        }

        public void CallEvent(Guid commandToken)
        {
            EventFireCallback(commandToken);
        }

        public override ModuleType ModuleType
        {
            get { return ModuleType.Event; }
        }

        public override string GetDescriptionText()
        {
            string runEveryStr = string.Empty;

            if (EventRunEvery.Days > 0) runEveryStr += EventRunEvery.Days+" д. ";
            if (EventRunEvery.Hours > 0) runEveryStr += EventRunEvery.Hours + " ч. ";
            if (EventRunEvery.Minutes > 0) runEveryStr += EventRunEvery.Minutes + " мин. ";
            if (EventRunEvery.Seconds > 0) runEveryStr += EventRunEvery.Seconds + " сек.";

            return "Запускается раз в " + runEveryStr;
        }
    }
}
