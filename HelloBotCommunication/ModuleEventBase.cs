using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HelloBotCommunication.Interfaces;

namespace HelloBotCommunication
{
    public abstract class ModuleEventBase : ModuleBase
    {
        /// <summary>
        /// Required field for events. You OnFire method will be executed every "RunEvery" time + time, that will be elapsed for handle you OnFire method implementation.
        /// </summary>
        public abstract TimeSpan RunEvery { get; }

        /// <summary>
        /// You event handle implementation. Will be fired by timer using RunEvery timespan delay.
        /// </summary>
        /// <param name="eventToken"></param>
        public abstract void OnFire(Guid eventToken);
    }
}
