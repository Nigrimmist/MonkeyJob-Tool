using System.Collections.Generic;


using HelloBotCommunication;
using HelloBotCommunication.Interfaces;
using Nigrimmist.Modules.Modules;

namespace Nigrimmist.Modules
{

    public class DllRegister : ModuleRegister
    {
        public override List<ModuleHandlerBase> GetModules()
        {
            return new List<ModuleHandlerBase>()
            {
                new Calculator(),
                new Weather(),
                new Boobs(),
                new Say(),
                new Translate(),
                new Bash(),
                new ItHappens(),
                new WhatIsIt(),
                new Map(),
                new LangExecuter(),
                new Advice(),
                new Or(),
                new Quote(),
                new ShortLink(),
                new BrowserUrlsOpen()
            };
        }

        public override List<ModuleEventBase> GetEvents()
        {
            return new List<ModuleEventBase>()
            {
                new Diary(),
                new ToolUpdate(),
                new TestEvent()
            };
        }

        public override string ByAuthor
        {
            get { throw new System.NotImplementedException(); }
        }
    }
}
