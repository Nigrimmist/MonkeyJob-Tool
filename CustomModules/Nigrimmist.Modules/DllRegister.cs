using System.Collections.Generic;


using HelloBotCommunication;
using HelloBotCommunication.Interfaces;
using Nigrimmist.Modules.Modules;

namespace Nigrimmist.Modules
{

    public class DllRegister : ModuleRegister
    {
        public override List<ModuleCommandBase> GetModules()
        {
            return new List<ModuleCommandBase>()
            {
                new Calculator(),
                new Weather(),
                new Boobs(),
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
                new BrowserUrlsOpen(),
                new IpChecker(),
                new CopyToBufferModule()
            };
        }

        public override List<ModuleEventBase> GetEvents()
        {
            return new List<ModuleEventBase>()
            {
                new Diary(),
                new ToolUpdate(),
                new MtsEthernetBYModule(),
                new MtsGsmByModule(),
                new OnlinerRent(),
                new NeagentBY()
            };
        }

        public override List<ModuleTrayBase> GetTrayModules()
        {
            return new List<ModuleTrayBase>()
            {
                new PingModule(),
                new WeatherTrayModule(),
                new MemoryUsageTrayModule()
            };
        }

        public override AuthorInfo AuthorInfo
        {
            get { return new AuthorInfo("Nikita Vasileusky", "Nigrimmist@gmail.com", "Nigrimmist+MJ-ModuleLogs@gmail.com"); }
        }
    }
}
