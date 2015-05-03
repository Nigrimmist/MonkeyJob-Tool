using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HelloBotCommunication.Attributes.SettingAttributes;
using HelloBotCore.Entities;
using MonkeyJobTool.Entities;
using Newtonsoft.Json;

namespace MonkeyJobTool.Forms
{
    public partial class ModuleSettingsForm : Form
    {
        public ModuleCommandInfoBase Module { get; set; }


        public ModuleSettingsForm()
        {
            InitializeComponent();
        }

        private void ModuleSettingsForm_Load(object sender, EventArgs e)
        {
            string moduleFileName = Module.ModuleSystemName + ".json";
            string fullPath = @"E:\My Dream\MonkeyJobTool\MonkeyJob-Tool\MonkeyJobTool\bin\Debug\ModuleSettings\" + moduleFileName;
            string data = File.ReadAllText(fullPath);
            var settingsWrapper = JsonConvert.DeserializeObject(data, typeof(ModuleSettings)) as ModuleSettings;
            var rawSettingsJson = JsonConvert.SerializeObject(settingsWrapper.ModuleData);
            var settings = JsonConvert.DeserializeObject(rawSettingsJson, Module.ModuleSettingsType);
            BindObject(settings, pnlSettingControls);
        }


        public void BindObject(object obj, Control parentControl)
        {
            var props = obj.GetType().GetProperties();

            foreach (var info in props)
            {
                var tInfo = info.PropertyType;
                var propTitle = Attribute.GetCustomAttribute(info, typeof(SettingsNameFieldAttribute)) as SettingsNameFieldAttribute;

                if (propTitle != null)
                {
                    if (tInfo == typeof (int))
                    {
                        AddControl(propTitle.Label, new TextBox() { Text = info.GetValue(obj, null).ToString() }, info.Name, parentControl);
                    }
                    else if (tInfo == typeof (string))
                    {
                        AddControl(propTitle.Label, new TextBox() { Text = info.GetValue(obj, null).ToString() }, info.Name, parentControl);
                    }
                    else if (tInfo == typeof(bool))
                    {
                        AddControl(propTitle.Label, new CheckBox() { Checked = (bool)info.GetValue(obj, null) }, info.Name, parentControl);
                    }
                    else if (typeof(IList).IsAssignableFrom(info.PropertyType))
                    {
                        foreach (var item in (IEnumerable)info.GetValue(obj, null))
                        {
                            Panel newPanel = new Panel();
                            newPanel.Margin = new Padding(){Left = 20};
                            parentControl.Controls.Add(newPanel);
                            BindObject(item, newPanel);
                        }
                    }
                    else
                    {
                        BindObject(info, parentControl);
                    }
                }
                
            }
        }

        private int _totalHeight = 0;

        private void AddControl(string label, Control cntrl, string propName, Control parentControl)
        {
            var lbl = new Label()
            {
                Text = label,
                Top = _totalHeight
            };
            parentControl.Controls.Add(lbl);

            cntrl.AccessibleName = propName;
            cntrl.Left += lbl.Width;
            cntrl.Top = _totalHeight;
            parentControl.Controls.Add(cntrl);

            _totalHeight += 40;
        }
    }
}
