using System.Collections.Generic;


namespace HelloBotCommunication
{
    public interface IModuleRegister
    {
        List<ModuleBase> GetModules();
        string ByAuthor { get; }
    }
}
