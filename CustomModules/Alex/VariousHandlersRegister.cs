using System.Collections.Generic;
using HelloBotCommunication;
using HelloBotCommunication.Interfaces;


namespace SmartAssHandlerLib
{
    public class VariousHandlersRegister : IModuleRegister
    {
        public List<ModuleBase> GetModules()
        {
            return new List<ModuleBase>()
            {
                new SmartAssStuffHandlerBase(),
                new YesNoHandlerBase(),
                new FckinWeatherModuleBase()
            };
        }

        public string ByAuthor
        {
            get { throw new System.NotImplementedException(); }
        }
    }
}
