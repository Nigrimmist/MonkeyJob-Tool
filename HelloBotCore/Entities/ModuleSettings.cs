using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HelloBotCore.Entities
{
    public class ModuleSettings<T,T2> 
        where T : class 
        where T2 : class
    {
        public double ModuleVersion { get; set; }
        public T ModuleData { get; set; }
        public T2 ServiceData { get; set; }
        public double SettingsVersion { get; set; }

        public ModuleSettings(double moduleVersion,double settingsVersion, T moduleData,T2 serviceData)
        {
            ModuleVersion = moduleVersion;
            ModuleData = moduleData;
            SettingsVersion = settingsVersion;
            ServiceData = serviceData;
        }
    }

    public class ModuleSettings<T>
        where T : class
        
    {
        public double ModuleVersion { get; set; }
        public T ModuleData { get; set; }
        public object ServiceData { get; set; }
        public double SettingsVersion { get; set; }

        public ModuleSettings(double moduleVersion, double settingsVersion, T moduleData, object serviceData)
        {
            ModuleVersion = moduleVersion;
            ModuleData = moduleData;
            SettingsVersion = settingsVersion;
            ServiceData = serviceData;
        }
    }

    public class ModuleSettings
    {
        public double ModuleVersion { get; set; }
        public double SettingsVersion { get; set; }
        public object ModuleData { get; set; }
        public object ServiceData { get; set; }


        public ModuleSettings(double moduleVersion, double settingsVersion, object moduleData, object serviceData)
        {
            ModuleVersion = moduleVersion;
            ModuleData = moduleData;
            ServiceData = serviceData;
            SettingsVersion = settingsVersion;
        }
    }
}
