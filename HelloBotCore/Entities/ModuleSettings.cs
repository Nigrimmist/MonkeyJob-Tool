using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HelloBotCore.Entities
{
    public class ModuleSettings<T> where T : class
    {
        public double ModuleVersion { get; set; }
        public T ModuleData { get; set; }

        public ModuleSettings(double moduleVersion, T moduleData)
        {
            ModuleVersion = moduleVersion;
            ModuleData = moduleData;
        }
    }
}
