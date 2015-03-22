using System.Collections.Generic;


using HelloBotCommunication;
using HelloBotCommunication.Interfaces;
using Nigrimmist.Modules.Modules;

namespace Nigrimmist.Modules
{

    public class DllRegister : IModuleRegister
    {
        public List<ModuleBase> GetModules()
        {
            return new List<ModuleBase>()
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

        public string ByAuthor
        {
            get { throw new System.NotImplementedException(); }
        }
    }
}
