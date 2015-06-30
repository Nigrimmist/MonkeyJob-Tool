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
               new Moon(),
                new Kstati()
            };
        }

        public override AuthorInfo AuthorInfo
        {
            get { return new AuthorInfo("Dmitry Yushko","",""); }
        }
    }
}
