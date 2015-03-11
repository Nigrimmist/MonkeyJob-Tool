using System.Collections.Generic;
using HelloBotCommunication;
using Yushko.Modules;

namespace Yushko
{
    public class HandlerRegister: IModuleRegister
    {
        public List<ModuleBase> GetModules()
        {
            return new List<ModuleBase>()
            {
                new ExchangeRate(),
                new Sorry(),
                //new Horoscope(),
                new Moon(),
                new Kstati()
            };
        }

        public string ByAuthor
        {
            get { throw new System.NotImplementedException(); }
        }
    }
}
