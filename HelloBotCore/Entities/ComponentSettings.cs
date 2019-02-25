using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HelloBotCore.Entities
{
    public class ComponentSettings<TComponentType, TServiceType> 
        where TComponentType : class 
        where TServiceType : class
    {
        public double ComponentVersion { get; set; }
        public TComponentType ComponentData { get; set; }
        public TServiceType ServiceData { get; set; }
        public double SettingsVersion { get; set; }

        public ComponentSettings(double componentVersion,double settingsVersion, TComponentType componentData, TServiceType serviceData)
        {
            ComponentVersion = componentVersion;
            ComponentData = componentData;
            SettingsVersion = settingsVersion;
            ServiceData = serviceData;
        }
    }

    public class ComponentSettings<TComponentType>
        where TComponentType : class
        
    {
        public double ComponentVersion { get; set; }
        public TComponentType ComponentData { get; set; }
        public object ServiceData { get; set; }
        public double SettingsVersion { get; set; }

        public ComponentSettings(double componentVersion, double settingsVersion, TComponentType componentData, object serviceData)
        {
            ComponentVersion = componentVersion;
            ComponentData = componentData;
            SettingsVersion = settingsVersion;
            ServiceData = serviceData;
        }
    }

    public class ComponentSettings
    {
        public double ComponentVersion { get; set; }
        public double SettingsVersion { get; set; }
        public object ComponentData { get; set; }
        public object ServiceData { get; set; }


        public ComponentSettings(double componentVersion, double settingsVersion, object componentData, object serviceData)
        {
            ComponentVersion = componentVersion;
            ComponentData = componentData;
            ServiceData = serviceData;
            SettingsVersion = settingsVersion;
        }
    }
}
