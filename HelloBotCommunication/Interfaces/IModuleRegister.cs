using System.Collections.Generic;

namespace HelloBotCommunication.Interfaces
{
    public interface IModuleRegister
    {
        List<ModuleBase> GetModules();
        string ByAuthor { get; }
    }
}
