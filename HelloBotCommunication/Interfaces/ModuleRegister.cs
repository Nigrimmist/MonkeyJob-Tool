using System.Collections.Generic;

namespace HelloBotCommunication.Interfaces
{
    public abstract class ModuleRegister
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
