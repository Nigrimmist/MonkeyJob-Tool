using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HelloBotCommunication.Interfaces
{
    public abstract class ModuleBase
    {
        /// <summary>
        /// Using for settings version tracking.
        /// </summary>
        public virtual double ModuleVersion
        {
            get { return 1.0; }
        }

        public virtual void Init(IClient client)
        {

        }

        /// <summary>
        /// Your command description
        /// </summary>
        public virtual string ModuleDescription
        {
            get { return string.Empty; }
        }
    }
}
