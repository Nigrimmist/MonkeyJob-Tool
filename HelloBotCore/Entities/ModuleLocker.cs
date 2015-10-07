using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HelloBotCore.Entities
{
    public class ModuleLocker
    {
        public object SettingsLock { get; set; }
        public object LogTraceLock { get; set; }


        public ModuleLocker()
        {
            SettingsLock = new object();
            LogTraceLock = new object();
        }
    }
}
