using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace HelloBotCommunication
{
    public abstract class ModuleBase
    {

        /// <summary>
        /// Using for settings version tracking.
        /// </summary>
        public virtual double ModuleVersion {
            get { return 1.0; }
        }

        /// <summary>
        /// Call command list.
        /// </summary>
        public abstract ReadOnlyCollection<CallCommandInfo> CallCommandList { get; }

        /// <summary>
        /// Your command description
        /// </summary>
        public virtual string CommandDescription {
            get { return string.Empty; } }

        public virtual void Init(IBot bot)
        {
            
        }

        /// <summary>
        /// Event will be fired for your Command
        /// </summary>
        /// <param name="command">Incoming command</param>
        /// <param name="args">Command arguments, can be empty</param>
        /// <param name="commandToken">Command token</param>
        public abstract void HandleMessage(string command,string args, Guid commandToken);
    }
}