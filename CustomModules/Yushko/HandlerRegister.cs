using System.Collections.Generic;
using HelloBotCommunication;
using HelloBotCommunication.Interfaces;
using Yushko.Modules;

namespace Yushko
{
    public class HandlerRegister: ModuleRegister
    {
        public override List<ModuleHandlerBase> GetModules()
        {
            return new List<ModuleHandlerBase>()
            {
                new ExchangeRate(),
                new Sorry(),
                //new Horoscope(),
                new Moon(),
                new Kstati()
            };
        }

        public override string ByAuthor
        {
            get { throw new System.NotImplementedException(); }
        }
    }
}
