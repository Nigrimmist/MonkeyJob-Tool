using System.Collections.Generic;
using HelloBotCommunication;
using HelloBotCommunication.Interfaces;


namespace SmartAssHandlerLib
{
    public class VariousHandlersRegister : ModuleRegister
    {
        public List<ModuleCommandBase> GetModules()
        {
            return new List<ModuleCommandBase>()
            {
                new YesNoCommandCommandBase()
            };
        }

        public override AuthorInfo AuthorInfo
        {
            get { return null; }
        }
    }
}
