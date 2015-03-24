using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using HelloBotCommunication.Interfaces;

namespace HelloBotCommunication
{
    public abstract class ModuleHandlerBase : ModuleBase
    {
        /// <summary>
        /// Call command list.
        /// </summary>
        public abstract ReadOnlyCollection<CallCommandInfo> CallCommandList { get; }
        

        /// <summary>
        /// Event will be fired for your Command
        /// </summary>
        /// <param name="command">Incoming command</param>
        /// <param name="args">Command arguments, can be empty</param>
        /// <param name="commandToken">Command token</param>
        public abstract void HandleMessage(string command,string args, Guid commandToken);
    }
}