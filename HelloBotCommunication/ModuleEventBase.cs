using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using HelloBotCommunication.Interfaces;

namespace HelloBotCommunication
{
    public abstract class ModuleEventBase : ModulePopupableBase
    {
        /// <summary>
        /// Your OnFire method will be executed every "RunEvery" time + time, that will be elapsed for handle you OnFire method implementation.
        /// </summary>
        public abstract TimeSpan RunEvery { get; }

        /// <summary>
        /// You event handle implementation. Will be fired by timer using RunEvery timespan delay.
        /// </summary>
        /// <param name="eventToken"></param>
        public abstract void OnFire(Guid eventToken);

        /// <summary>
        /// General event description
        /// </summary>
        public virtual string ModuleDescription { get { return null; } }

        
    }
}
