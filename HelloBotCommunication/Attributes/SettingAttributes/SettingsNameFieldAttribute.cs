using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HelloBotCommunication.Attributes.SettingAttributes
{
    public class SettingsNameFieldAttribute : Attribute
    {
        public string Label { get; set; }
        public SettingsNameFieldAttribute(string label)
        {
            Label = label;
        }
    }
}
