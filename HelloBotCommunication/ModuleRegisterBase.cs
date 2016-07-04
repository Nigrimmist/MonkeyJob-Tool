using System.Collections.Generic;

namespace HelloBotCommunication
{
    public abstract class ModuleRegisterBase
    {
        public virtual List<ModuleCommandBase> GetModules()
        {
            return new List<ModuleCommandBase>();
        }
        public virtual List<ModuleEventBase> GetEvents()
        {
            return new List<ModuleEventBase>();
        }
        public virtual List<ModuleTrayBase> GetTrayModules()
        {
            return new List<ModuleTrayBase>();
        }

        public abstract AuthorInfo AuthorInfo { get; }
    }
}
