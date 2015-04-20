using System.Collections.Generic;

namespace HelloBotCommunication.Interfaces
{
    public abstract class ModuleRegister
    {
        public virtual List<ModuleHandlerBase> GetModules()
        {
            return new List<ModuleHandlerBase>();
        }
        public virtual List<ModuleEventBase> GetEvents()
        {
            return new List<ModuleEventBase>();
        }
        public abstract AuthorInfo AuthorInfo { get; }
    }
}
