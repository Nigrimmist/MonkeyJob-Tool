using System.Collections.Generic;
using HelloBotCommunication;
using HelloBotCommunication.Interfaces;


namespace SmartAssHandlerLib
{
    public class VariousHandlersRegister : ModuleRegister
    {
        public List<ModuleHandlerBase> GetModules()
        {
            return new List<ModuleHandlerBase>()
            {
                new YesNoHandlerHandlerBase()
            };
        }

        public override AuthorInfo AuthorInfo
        {
            get { return null; }
        }
    }
}
