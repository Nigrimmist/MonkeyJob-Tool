using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HelloBotCore.Entities
{
    public class ModuleSettings<TModuleType, TServiceType> 
        where TModuleType : class 
        where TServiceType : class
    {
        public double ModuleVersion { get; set; }
        public TModuleType ModuleData { get; set; }
        public TServiceType ServiceData { get; set; }
        public double SettingsVersion { get; set; }

        public ModuleSettings(double moduleVersion,double settingsVersion, TModuleType moduleData, TServiceType serviceData)
        {
            ModuleVersion = moduleVersion;
            ModuleData = moduleData;
            SettingsVersion = settingsVersion;
            ServiceData = serviceData;
        }
    }

    public class ModuleSettings<TModuleType>
        where TModuleType : class
        
    {
        public double ModuleVersion { get; set; }
        public TModuleType ModuleData { get; set; }
        public object ServiceData { get; set; }
        public double SettingsVersion { get; set; }

        public ModuleSettings(double moduleVersion, double settingsVersion, TModuleType moduleData, object serviceData)
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
