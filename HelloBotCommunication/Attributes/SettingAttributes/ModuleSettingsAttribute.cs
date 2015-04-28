using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HelloBotCommunication.Attributes.SettingAttributes
{
    public class ModuleSettingsForAttribute : Attribute
    {
        public Type ModuleType { get; set; }

        public ModuleSettingsForAttribute(Type moduleType)
        {
            ModuleType = moduleType;
        }
    }
}
